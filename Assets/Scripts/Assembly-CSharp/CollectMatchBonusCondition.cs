using System;
using System.Collections.Generic;

// Token: 0x02000124 RID: 292
public class CollectMatchBonusCondition : MatchableCondition
{
	// Token: 0x06000AC5 RID: 2757 RVA: 0x00042AA8 File Offset: 0x00040CA8
	public static CollectMatchBonusCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		CollectMatchBonusCondition collectMatchBonusCondition = new CollectMatchBonusCondition();
		collectMatchBonusCondition.Parse(dict, "collect_match_bonus", new List<string>
		{
			"BonusPickup"
		}, list, -1);
		return collectMatchBonusCondition;
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00042B04 File Offset: 0x00040D04
	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			if (base.Matchers[1].HasRequirements())
			{
				return string.Format(Language.Get("!!COND_MATCH_WISH_ALL"), Language.Get(base.Matchers[0].DescribeSubject(game)), Language.Get(base.Matchers[1].DescribeSubject(game)));
			}
			return string.Format(Language.Get("!!COND_MATCH_WISH_CHARACTER"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		else
		{
			if (base.Matchers[1].HasRequirements())
			{
				return string.Format(Language.Get("!!COND_MATCH_WISH_RESOURCE"), Language.Get(base.Matchers[1].DescribeSubject(game)));
			}
			return string.Format(Language.Get("!!COND_MATCH_WISH_ANY"), new object[0]);
		}
	}

	// Token: 0x04000747 RID: 1863
	public const string LOAD_TOKEN = "collect_match_bonus";

	// Token: 0x04000748 RID: 1864
	public const int RESIDENT_MATCHER = 0;

	// Token: 0x04000749 RID: 1865
	public const int RESOURCE_MATCHER = 1;
}
