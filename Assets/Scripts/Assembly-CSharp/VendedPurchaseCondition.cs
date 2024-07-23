using System;
using System.Collections.Generic;

// Token: 0x02000152 RID: 338
public class VendedPurchaseCondition : MatchableCondition
{
	// Token: 0x06000B6F RID: 2927 RVA: 0x000453E8 File Offset: 0x000435E8
	public static VendedPurchaseCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		VendedPurchaseCondition vendedPurchaseCondition = new VendedPurchaseCondition();
		vendedPurchaseCondition.Parse(dict, "vended_purchase", new List<string>
		{
			"va"
		}, list, -1);
		return vendedPurchaseCondition;
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x00045444 File Offset: 0x00043644
	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_VEND_FROM"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return string.Format(Language.Get("!!COND_VEND_PURCHASE"), Language.Get(base.Matchers[1].DescribeSubject(game)));
	}

	// Token: 0x040007A1 RID: 1953
	public const string LOAD_TOKEN = "vended_purchase";

	// Token: 0x040007A2 RID: 1954
	public const int BUILDING_MATCHER = 0;

	// Token: 0x040007A3 RID: 1955
	public const int RESOURCE_MATCHER = 1;
}
