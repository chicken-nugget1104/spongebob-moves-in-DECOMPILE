using System;
using System.Collections.Generic;

// Token: 0x0200018E RID: 398
public class PaveMatcher : Matcher
{
	// Token: 0x06000D75 RID: 3445 RVA: 0x000524F4 File Offset: 0x000506F4
	public static PaveMatcher FromDict(Dictionary<string, object> dict)
	{
		PaveMatcher paveMatcher = new PaveMatcher();
		paveMatcher.RegisterProperty("pave_type", dict);
		return paveMatcher;
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x00052518 File Offset: 0x00050718
	public override string DescribeSubject(Game game)
	{
		return "path";
	}

	// Token: 0x040008F1 RID: 2289
	public const string PAVE_TYPE = "pave_type";
}
