using System;
using System.Collections.Generic;

// Token: 0x02000123 RID: 291
public class ChangeCostumeCondition : MatchableCondition
{
	// Token: 0x06000AC2 RID: 2754 RVA: 0x00042A48 File Offset: 0x00040C48
	public static ChangeCostumeCondition FromDict(Dictionary<string, object> dict)
	{
		ChangeCostumeCondition changeCostumeCondition = new ChangeCostumeCondition();
		changeCostumeCondition.Parse(dict, "change_costume", new List<string>
		{
			typeof(ChangeCostumeAction).ToString()
		}, new List<IMatcher>
		{
			CostumeMatcher.FromDict(dict)
		}, -1);
		return changeCostumeCondition;
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x00042A98 File Offset: 0x00040C98
	public override string Description(Game game)
	{
		return "change_costume";
	}

	// Token: 0x04000746 RID: 1862
	public const string LOAD_TOKEN = "change_costume";
}
