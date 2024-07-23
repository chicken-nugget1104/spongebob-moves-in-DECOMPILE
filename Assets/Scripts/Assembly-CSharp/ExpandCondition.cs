using System;
using System.Collections.Generic;

// Token: 0x02000137 RID: 311
public class ExpandCondition : MatchableCondition
{
	// Token: 0x06000B10 RID: 2832 RVA: 0x000441B0 File Offset: 0x000423B0
	public static ExpandCondition FromDict(Dictionary<string, object> dict)
	{
		ExpandCondition expandCondition = new ExpandCondition();
		expandCondition.Parse(dict, "expand", new List<string>
		{
			typeof(NewExpansionAction).ToString()
		}, new List<IMatcher>
		{
			ExpansionMatcher.FromDict(dict)
		}, -1);
		return expandCondition;
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x00044200 File Offset: 0x00042400
	public override string Description(Game game)
	{
		return Language.Get("!!COND_BUY_EXPANSION");
	}

	// Token: 0x04000775 RID: 1909
	public const string LOAD_TOKEN = "expand";
}
