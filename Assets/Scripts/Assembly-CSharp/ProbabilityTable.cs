using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class ProbabilityTable : IEnumerable, IEnumerable<ProbabilityTable.Entry>, ResultGenerator
{
	// Token: 0x06002167 RID: 8551 RVA: 0x000CE64C File Offset: 0x000CC84C
	public ProbabilityTable()
	{
		this.entries = new Dictionary<string, ProbabilityTable.Entry>();
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x000CE660 File Offset: 0x000CC860
	public ProbabilityTable(Dictionary<string, object> dict)
	{
		this.entries = new Dictionary<string, ProbabilityTable.Entry>(dict.Count);
		foreach (KeyValuePair<string, object> keyValuePair in dict)
		{
			string key = keyValuePair.Key;
			double probability = Convert.ToDouble(keyValuePair.Value);
			this.Add(key, probability);
		}
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x000CE6F0 File Offset: 0x000CC8F0
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x0600216A RID: 8554 RVA: 0x000CE6F8 File Offset: 0x000CC8F8
	public double TotalRange
	{
		get
		{
			return this.totalRange;
		}
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x000CE700 File Offset: 0x000CC900
	public void Add(string eventName, double probability)
	{
		if (this.entries.ContainsKey(eventName))
		{
			ProbabilityTable.Entry entry = this.entries[eventName];
			entry.rangeEnd += probability;
			foreach (ProbabilityTable.Entry entry2 in this.entries.Values)
			{
				if (entry2.rangeStart >= entry.rangeStart)
				{
					entry2.rangeStart += probability;
					entry2.rangeEnd += probability;
				}
			}
		}
		else
		{
			double num = this.totalRange;
			double rangeEnd = num + probability;
			this.entries.Add(eventName, new ProbabilityTable.Entry(num, rangeEnd, eventName));
		}
		this.totalRange += probability;
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x000CE7F4 File Offset: 0x000CC9F4
	public void Normalize()
	{
		double num = 0.0;
		double num2 = this.totalRange;
		foreach (ProbabilityTable.Entry entry in this.entries.Values)
		{
			double range = entry.Range;
			entry.rangeStart = num;
			entry.rangeEnd = num + range / num2;
			num = entry.rangeEnd;
		}
		this.totalRange = 1.0;
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x000CE89C File Offset: 0x000CCA9C
	public string GetResult()
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		foreach (ProbabilityTable.Entry entry in this.entries.Values)
		{
			if ((double)num >= entry.rangeStart && (double)num < entry.rangeEnd)
			{
				return entry.eventName;
			}
		}
		return null;
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x000CE93C File Offset: 0x000CCB3C
	public string GetExpectedValue()
	{
		double num = 0.0;
		foreach (ProbabilityTable.Entry entry in this.entries.Values)
		{
			num += double.Parse(entry.eventName) * entry.Range;
		}
		return string.Empty + num;
	}

	// Token: 0x0600216F RID: 8559 RVA: 0x000CE9D0 File Offset: 0x000CCBD0
	public string GetLowestValue()
	{
		double num = -1.0;
		foreach (ProbabilityTable.Entry entry in this.entries.Values)
		{
			double num2 = double.Parse(entry.eventName);
			if (num == -1.0 || num2 < num)
			{
				num = num2;
			}
		}
		if (num < 0.0)
		{
			num = 0.0;
		}
		return string.Empty + num;
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x000CEA8C File Offset: 0x000CCC8C
	public IEnumerator<ProbabilityTable.Entry> GetEnumerator()
	{
		return this.entries.Values.GetEnumerator();
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x000CEAA4 File Offset: 0x000CCCA4
	public ProbabilityTable Where(Func<string, bool> predicate, bool normalize)
	{
		ProbabilityTable probabilityTable = new ProbabilityTable();
		double num = 0.0;
		Dictionary<string, ProbabilityTable.Entry> dictionary = this.entries;
		List<ProbabilityTable.Entry> list = new List<ProbabilityTable.Entry>();
		foreach (ProbabilityTable.Entry entry in dictionary.Values)
		{
			if (predicate(entry.eventName))
			{
				list.Add(entry);
				num += entry.Range;
			}
		}
		double num2 = 0.0;
		foreach (ProbabilityTable.Entry entry2 in list)
		{
			double range = entry2.Range;
			double num3 = (!normalize) ? range : (range / num);
			probabilityTable.Add(entry2.eventName, num3);
			num2 += num3;
		}
		return probabilityTable;
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x000CEBD4 File Offset: 0x000CCDD4
	public double ProbabilityOfEvent(string eventName)
	{
		ProbabilityTable.Entry entry = this.entries[eventName];
		if (entry == null)
		{
			return 0.0;
		}
		return entry.Range;
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x000CEC04 File Offset: 0x000CCE04
	public void AssertLte1()
	{
		TFUtils.Assert(Mathf.Approximately((float)this.TotalRange, 1f) || this.TotalRange < 1.0, "Cumulative probability exceeding 1.0. Options after 1.0 are unreachable. Encountered " + this.TotalRange);
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x000CEC58 File Offset: 0x000CCE58
	public override string ToString()
	{
		string text = "[ProbabilityTable (";
		foreach (ProbabilityTable.Entry entry in this.entries.Values)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"\n p:",
				entry.Range,
				", v:",
				entry.eventName
			});
		}
		text += ")]";
		return text;
	}

	// Token: 0x040014AC RID: 5292
	private Dictionary<string, ProbabilityTable.Entry> entries;

	// Token: 0x040014AD RID: 5293
	private double totalRange;

	// Token: 0x0200043D RID: 1085
	public class Entry
	{
		// Token: 0x06002175 RID: 8565 RVA: 0x000CED08 File Offset: 0x000CCF08
		public Entry(double rangeStart, double rangeEnd, string eventName)
		{
			this.rangeStart = rangeStart;
			this.rangeEnd = rangeEnd;
			this.eventName = eventName;
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x000CED28 File Offset: 0x000CCF28
		public double Range
		{
			get
			{
				return this.rangeEnd - this.rangeStart;
			}
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000CED38 File Offset: 0x000CCF38
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[Entry event(",
				this.eventName,
				"), range:[",
				this.rangeStart,
				",",
				this.rangeEnd,
				") ]"
			});
		}

		// Token: 0x040014AE RID: 5294
		public double rangeStart;

		// Token: 0x040014AF RID: 5295
		public double rangeEnd;

		// Token: 0x040014B0 RID: 5296
		public string eventName;
	}
}
