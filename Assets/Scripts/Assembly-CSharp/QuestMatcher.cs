using System;
using System.Collections.Generic;

// Token: 0x0200018F RID: 399
public class QuestMatcher : Matcher
{
	// Token: 0x06000D78 RID: 3448 RVA: 0x00052528 File Offset: 0x00050728
	public static QuestMatcher FromDict(Dictionary<string, object> dict)
	{
		QuestMatcher questMatcher = new QuestMatcher();
		questMatcher.RegisterProperty("quest_id", dict);
		return questMatcher;
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x0005254C File Offset: 0x0005074C
	public override string DescribeSubject(Game game)
	{
		if (game == null)
		{
			return "did " + this.GetTarget("quest_id");
		}
		uint did = uint.Parse(this.GetTarget("quest_id"));
		return game.questManager.GetQuestDefinition(did).Name;
	}

	// Token: 0x040008F2 RID: 2290
	public const string DEFINITION_ID = "quest_id";
}
