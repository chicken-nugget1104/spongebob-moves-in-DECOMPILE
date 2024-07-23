using System;
using System.Collections.Generic;

// Token: 0x02000147 RID: 327
public class ProgressQuestCondition : MatchableCondition
{
	// Token: 0x06000B4E RID: 2894 RVA: 0x00044DF4 File Offset: 0x00042FF4
	public static ProgressQuestCondition FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher item = QuestMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		ProgressQuestCondition progressQuestCondition = new ProgressQuestCondition();
		progressQuestCondition.Parse(dict, "progress_quest", new List<string>
		{
			typeof(QuestProgressAction).ToString()
		}, list, -1);
		return progressQuestCondition;
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x00044E48 File Offset: 0x00043048
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_PROGRESS_QUEST"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x0400078B RID: 1931
	public const string LOAD_TOKEN = "progress_quest";

	// Token: 0x0400078C RID: 1932
	public const int QUEST_MATCHER = 0;
}
