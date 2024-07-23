using System;
using System.Collections.Generic;

// Token: 0x02000120 RID: 288
public class AutoQuestCraftCollectCondition : CraftCondition
{
	// Token: 0x06000AAF RID: 2735 RVA: 0x0004282C File Offset: 0x00040A2C
	public static AutoQuestCraftCollectCondition FromDict(Dictionary<string, object> dict)
	{
		AutoQuestCraftCollectCondition autoQuestCraftCollectCondition = new AutoQuestCraftCollectCondition();
		CraftCondition.FromDictHelper(dict, autoQuestCraftCollectCondition, "auto_quest_craft_collect", new List<string>
		{
			typeof(AutoQuestCraftCollectAction).ToString()
		});
		return autoQuestCraftCollectCondition;
	}

	// Token: 0x04000740 RID: 1856
	public const string LOAD_TOKEN = "auto_quest_craft_collect";
}
