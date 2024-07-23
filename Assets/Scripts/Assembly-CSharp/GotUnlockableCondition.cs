using System;
using System.Collections.Generic;

// Token: 0x02000139 RID: 313
public class GotUnlockableCondition : MatchableCondition
{
	// Token: 0x06000B16 RID: 2838 RVA: 0x00044348 File Offset: 0x00042548
	public static GotUnlockableCondition FromDict(Dictionary<string, object> dict)
	{
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, UnlockableMatcher.FromDict(dict));
		GotUnlockableCondition gotUnlockableCondition = new GotUnlockableCondition();
		gotUnlockableCondition.Parse(dict, "got_unlockable", new List<string>
		{
			"unlockable",
			"reevaluate"
		}, list, -1);
		return gotUnlockableCondition;
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0004439C File Offset: 0x0004259C
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_GET_UNLOCK"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x04000779 RID: 1913
	public const string LOAD_TOKEN = "got_unlockable";

	// Token: 0x0400077A RID: 1914
	private const int UNLOCKABLE_MATCHER = 0;
}
