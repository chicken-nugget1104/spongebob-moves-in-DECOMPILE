using System;
using System.Collections.Generic;

// Token: 0x0200014E RID: 334
public class StartQuestCondition : MatchableCondition
{
	// Token: 0x06000B63 RID: 2915 RVA: 0x000451E8 File Offset: 0x000433E8
	public static StartQuestCondition FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher item = QuestMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		StartQuestCondition startQuestCondition = new StartQuestCondition();
		startQuestCondition.Parse(dict, "start_quest", new List<string>
		{
			typeof(QuestStartAction).ToString()
		}, list, -1);
		return startQuestCondition;
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0004523C File Offset: 0x0004343C
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_START_QUEST"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x0400079B RID: 1947
	public const string LOAD_TOKEN = "start_quest";

	// Token: 0x0400079C RID: 1948
	public const int QUEST_MATCHER = 0;
}
