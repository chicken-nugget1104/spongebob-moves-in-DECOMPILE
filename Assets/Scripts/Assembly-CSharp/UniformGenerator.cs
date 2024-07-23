using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public class UniformGenerator : ResultGenerator
{
	// Token: 0x0600217B RID: 8571 RVA: 0x000CED98 File Offset: 0x000CCF98
	public UniformGenerator(List<object> list)
	{
		this.options = new List<string>();
		foreach (object obj in list)
		{
			this.options.Add(obj.ToString());
		}
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000CEE14 File Offset: 0x000CD014
	public string GetResult()
	{
		if (this.options.Count == 0)
		{
			return null;
		}
		return this.options[UnityEngine.Random.Range(0, this.options.Count - 1)];
	}

	// Token: 0x0600217D RID: 8573 RVA: 0x000CEE54 File Offset: 0x000CD054
	public string GetExpectedValue()
	{
		if (this.options.Count == 0)
		{
			return null;
		}
		float num = 0f;
		foreach (string s in this.options)
		{
			num += (float)int.Parse(s);
		}
		return string.Empty + num / (float)this.options.Count;
	}

	// Token: 0x0600217E RID: 8574 RVA: 0x000CEEF4 File Offset: 0x000CD0F4
	public string GetLowestValue()
	{
		if (this.options.Count == 0)
		{
			return "0";
		}
		float num = -1f;
		foreach (string s in this.options)
		{
			float num2 = (float)int.Parse(s);
			if (num == -1f || num2 < num)
			{
				num = num2;
			}
		}
		if (num < 0f)
		{
			num = 0f;
		}
		return string.Empty + num;
	}

	// Token: 0x040014B1 RID: 5297
	private List<string> options;
}
