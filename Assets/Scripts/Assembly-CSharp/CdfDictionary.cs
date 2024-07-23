using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class CdfDictionary<T>
{
	// Token: 0x06000DD5 RID: 3541 RVA: 0x000543FC File Offset: 0x000525FC
	public static CdfDictionary<T> FromList(List<object> data, CdfDictionary<T>.ParseT parser)
	{
		CdfDictionary<T> cdfDictionary = new CdfDictionary<T>();
		foreach (object obj in data)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			if (SBSettings.AssertDataValidity)
			{
				TFUtils.AssertKeyExists(dictionary, "p");
				TFUtils.AssertKeyExists(dictionary, "value");
			}
			double probability = TFUtils.LoadDouble(dictionary, "p");
			T val = parser(dictionary["value"]);
			cdfDictionary.Add(val, probability);
		}
		return cdfDictionary;
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x000544B0 File Offset: 0x000526B0
	public int Count
	{
		get
		{
			return this.values.Count;
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x000544C0 File Offset: 0x000526C0
	public List<T> ValuesClone
	{
		get
		{
			return new List<T>(this.values.Values);
		}
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x000544D4 File Offset: 0x000526D4
	public void Add(T val, double probability)
	{
		string text = val.GetHashCode().ToString();
		this.values[text] = val;
		this.randomIndexer.Add(text, probability);
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x00054514 File Offset: 0x00052714
	public CdfDictionary<T> Clone()
	{
		return new CdfDictionary<T>
		{
			values = new Dictionary<string, T>(this.values),
			randomIndexer = this.randomIndexer
		};
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x00054548 File Offset: 0x00052748
	public CdfDictionary<T> Where(Func<T, bool> predicate, bool normalize)
	{
		List<T> matchingValues = this.values.Values.Where(predicate).ToList<T>();
		CdfDictionary<T> rv = new CdfDictionary<T>();
		rv.values = (from kvp in this.values
		where matchingValues.Contains(kvp.Value)
		select kvp).ToDictionary((KeyValuePair<string, T> kvp) => kvp.Key, (KeyValuePair<string, T> kvp) => kvp.Value);
		rv.randomIndexer = this.randomIndexer.Where((string i) => rv.values.Keys.Contains(i), normalize);
		return rv;
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x0005460C File Offset: 0x0005280C
	public CdfDictionary<T> Join(CdfDictionary<T> that)
	{
		CdfDictionary<T> rv = new CdfDictionary<T>();
		Action<CdfDictionary<T>> action = delegate(CdfDictionary<T> cdf)
		{
			foreach (ProbabilityTable.Entry entry in cdf.randomIndexer)
			{
				rv.Add(cdf.values[entry.eventName], entry.Range);
			}
		};
		action(this);
		action(that);
		return rv;
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x0005464C File Offset: 0x0005284C
	public void Normalize()
	{
		this.randomIndexer.Normalize();
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x0005465C File Offset: 0x0005285C
	public T Spin()
	{
		return this.Spin(default(T));
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x00054678 File Offset: 0x00052878
	public T Spin(T defaultValue)
	{
		this.randomIndexer.AssertLte1();
		string result = this.randomIndexer.GetResult();
		if (result == null)
		{
			return defaultValue;
		}
		return this.values[result];
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x000546B0 File Offset: 0x000528B0
	public void Validate(bool ensureFullRange, string message)
	{
		this.randomIndexer.AssertLte1();
		TFUtils.Assert(!ensureFullRange || Mathf.Approximately((float)this.randomIndexer.TotalRange, 1f), message + "This CDF Dictionary is required to specify the full range of possibilities (from 0 to 1). Instead it specifies 0-" + this.randomIndexer.TotalRange);
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x00054708 File Offset: 0x00052908
	public override string ToString()
	{
		string text = string.Empty;
		foreach (string text2 in this.values.Keys)
		{
			string text3 = text;
			text = string.Concat(new object[]
			{
				text3,
				" { p: ",
				this.randomIndexer.ProbabilityOfEvent(text2),
				", v: ",
				this.values[text2],
				" }\n"
			});
		}
		return string.Concat(new object[]
		{
			"CDF:[totalRange=",
			this.randomIndexer.TotalRange,
			"\n",
			text,
			" ]"
		});
	}

	// Token: 0x0400092E RID: 2350
	private Dictionary<string, T> values = new Dictionary<string, T>();

	// Token: 0x0400092F RID: 2351
	private ProbabilityTable randomIndexer = new ProbabilityTable();

	// Token: 0x020004A6 RID: 1190
	// (Invoke) Token: 0x060024FB RID: 9467
	public delegate T ParseT(object data);
}
