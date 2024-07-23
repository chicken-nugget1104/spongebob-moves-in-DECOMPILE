using System;
using System.Collections.Generic;

// Token: 0x02000189 RID: 393
public class ExpansionMatcher : Matcher
{
	// Token: 0x06000D4E RID: 3406 RVA: 0x00051F48 File Offset: 0x00050148
	public static ExpansionMatcher FromDict(Dictionary<string, object> dict)
	{
		ExpansionMatcher expansionMatcher = new ExpansionMatcher();
		expansionMatcher.RegisterProperty("expansion_id", dict);
		return expansionMatcher;
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00051F6C File Offset: 0x0005016C
	public override string DescribeSubject(Game game)
	{
		return "expand";
	}

	// Token: 0x040008E6 RID: 2278
	public const string EXPANSION_ID = "expansion_id";
}
