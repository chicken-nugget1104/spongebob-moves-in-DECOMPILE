using System;
using System.Collections.Generic;

// Token: 0x0200014F RID: 335
public class TapWandererCondition : MatchableCondition
{
	// Token: 0x06000B66 RID: 2918 RVA: 0x00045278 File Offset: 0x00043478
	public static TapWandererCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		int simulatedExistsID = -1;
		if (dict.ContainsKey("simulated_exists"))
		{
			simulatedExistsID = TFUtils.LoadInt(dict, "simulated_exists");
		}
		TapWandererCondition tapWandererCondition = new TapWandererCondition();
		tapWandererCondition.Parse(dict, "tap_wanderer", new List<string>
		{
			typeof(TapWandererAction).ToString()
		}, list, simulatedExistsID);
		return tapWandererCondition;
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x000452EC File Offset: 0x000434EC
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_TAP"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x0400079D RID: 1949
	public const string LOAD_TOKEN = "tap_wanderer";

	// Token: 0x0400079E RID: 1950
	public const int WANDERER_MATCHER = 0;
}
