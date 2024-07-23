using System;
using System.Collections.Generic;

// Token: 0x02000128 RID: 296
public class CompleteQuestCondition : MatchableCondition
{
	// Token: 0x06000ACF RID: 2767 RVA: 0x00042DE8 File Offset: 0x00040FE8
	public static CompleteQuestCondition FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher item = QuestMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		CompleteQuestCondition completeQuestCondition = new CompleteQuestCondition();
		completeQuestCondition.Parse(dict, "complete_quest", new List<string>
		{
			typeof(QuestCompleteAction).ToString()
		}, list, -1);
		return completeQuestCondition;
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x00042E3C File Offset: 0x0004103C
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_COMPLETE_QUEST"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x04000750 RID: 1872
	public const string LOAD_TOKEN = "complete_quest";

	// Token: 0x04000751 RID: 1873
	public const int QUEST_MATCHER = 0;
}
