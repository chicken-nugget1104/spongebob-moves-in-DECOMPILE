using System;
using System.Collections.Generic;

// Token: 0x0200012E RID: 302
public class ConditionalProgress
{
	// Token: 0x06000AF0 RID: 2800 RVA: 0x00043BD8 File Offset: 0x00041DD8
	public ConditionalProgress() : this(new List<uint>())
	{
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00043BE8 File Offset: 0x00041DE8
	public ConditionalProgress(List<uint> metIds)
	{
		this.metIds = metIds;
	}

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00043BF8 File Offset: 0x00041DF8
	public List<uint> MetIds
	{
		get
		{
			return this.metIds;
		}
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00043C00 File Offset: 0x00041E00
	public override string ToString()
	{
		string text = "[ConditionalProgress (metIds=";
		foreach (uint num in this.metIds)
		{
			text = text + num + ", ";
		}
		text += ")]";
		return text;
	}

	// Token: 0x04000768 RID: 1896
	private List<uint> metIds;
}
