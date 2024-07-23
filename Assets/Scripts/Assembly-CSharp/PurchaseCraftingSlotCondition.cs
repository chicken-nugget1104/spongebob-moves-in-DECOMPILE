using System;
using System.Collections.Generic;

// Token: 0x02000148 RID: 328
public class PurchaseCraftingSlotCondition : MatchableCondition
{
	// Token: 0x06000B51 RID: 2897 RVA: 0x00044E84 File Offset: 0x00043084
	public static PurchaseCraftingSlotCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		PurchaseCraftingSlotCondition purchaseCraftingSlotCondition = new PurchaseCraftingSlotCondition();
		purchaseCraftingSlotCondition.Parse(dict, "purchase_production_slot", new List<string>
		{
			typeof(PurchaseCraftingSlotAction).ToString()
		}, list, -1);
		return purchaseCraftingSlotCondition;
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x00044ED8 File Offset: 0x000430D8
	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_PURCHASE_PRODUCTION_SLOT"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return Language.Get("!!COND_PURCHASE_PRODUCTION_SLOT_GENERIC");
	}

	// Token: 0x0400078D RID: 1933
	public const string LOAD_TOKEN = "purchase_production_slot";

	// Token: 0x0400078E RID: 1934
	public const int BUILDING_MATCHER = 0;
}
