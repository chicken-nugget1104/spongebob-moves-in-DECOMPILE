using System;
using System.Collections.Generic;

// Token: 0x0200014B RID: 331
public class RemoveDebrisCondition : MatchableCondition
{
	// Token: 0x06000B5A RID: 2906 RVA: 0x00044FD4 File Offset: 0x000431D4
	public static RemoveDebrisCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		RemoveDebrisCondition removeDebrisCondition = new RemoveDebrisCondition();
		removeDebrisCondition.Parse(dict, "remove_debris", new List<string>
		{
			typeof(DebrisCompleteAction).ToString()
		}, list, -1);
		return removeDebrisCondition;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00045028 File Offset: 0x00043228
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_REMOVE_DEBRIS_TYPE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x04000793 RID: 1939
	public const string LOAD_TOKEN = "remove_debris";

	// Token: 0x04000794 RID: 1940
	public const int DEBRIS_MATCHER = 0;
}
