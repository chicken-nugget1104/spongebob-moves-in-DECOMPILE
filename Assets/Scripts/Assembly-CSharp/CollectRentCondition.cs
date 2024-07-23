using System;
using System.Collections.Generic;

// Token: 0x02000125 RID: 293
public class CollectRentCondition : MatchableCondition
{
	// Token: 0x06000AC8 RID: 2760 RVA: 0x00042BFC File Offset: 0x00040DFC
	public static CollectRentCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		CollectRentCondition collectRentCondition = new CollectRentCondition();
		collectRentCondition.Parse(dict, "collect_rent", new List<string>
		{
			"RentPickup"
		}, list, -1);
		return collectRentCondition;
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x00042C58 File Offset: 0x00040E58
	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_COLLECT_FROM"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return string.Format(Language.Get("!!COND_COLLECT_AS_RENT"), Language.Get(base.Matchers[1].DescribeSubject(game)));
	}

	// Token: 0x0400074A RID: 1866
	public const string LOAD_TOKEN = "collect_rent";

	// Token: 0x0400074B RID: 1867
	public const int BUILDING_MATCHER = 0;

	// Token: 0x0400074C RID: 1868
	public const int RESOURCE_MATCHER = 1;
}
