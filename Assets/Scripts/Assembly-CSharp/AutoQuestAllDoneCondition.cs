using System;
using System.Collections.Generic;

// Token: 0x0200011F RID: 287
public class AutoQuestAllDoneCondition : MatchableCondition
{
	// Token: 0x06000AAC RID: 2732 RVA: 0x000427C8 File Offset: 0x000409C8
	public static AutoQuestAllDoneCondition FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher item = QuestMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		AutoQuestAllDoneCondition autoQuestAllDoneCondition = new AutoQuestAllDoneCondition();
		autoQuestAllDoneCondition.Parse(dict, "auto_quest_all_done", new List<string>
		{
			typeof(AutoQuestAllDoneAction).ToString()
		}, list, -1);
		return autoQuestAllDoneCondition;
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0004281C File Offset: 0x00040A1C
	public override string Description(Game game)
	{
		return string.Empty;
	}

	// Token: 0x0400073E RID: 1854
	public const string LOAD_TOKEN = "auto_quest_all_done";

	// Token: 0x0400073F RID: 1855
	public const int QUEST_MATCHER = 0;
}
