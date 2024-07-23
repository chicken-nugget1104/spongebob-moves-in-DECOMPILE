using System;
using System.Collections.Generic;

// Token: 0x0200013A RID: 314
public class HideWandererCondition : MatchableCondition
{
	// Token: 0x06000B19 RID: 2841 RVA: 0x000443D8 File Offset: 0x000425D8
	public static HideWandererCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		int simulatedExistsID = -1;
		if (dict.ContainsKey("simulated_exists"))
		{
			simulatedExistsID = TFUtils.LoadInt(dict, "simulated_exists");
		}
		HideWandererCondition hideWandererCondition = new HideWandererCondition();
		hideWandererCondition.Parse(dict, "hide_wanderer", new List<string>
		{
			typeof(HideWandererAction).ToString()
		}, list, simulatedExistsID);
		return hideWandererCondition;
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x0004444C File Offset: 0x0004264C
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_HIDE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x0400077B RID: 1915
	public const string LOAD_TOKEN = "hide_wanderer";

	// Token: 0x0400077C RID: 1916
	public const int WANDERER_MATCHER = 0;
}
