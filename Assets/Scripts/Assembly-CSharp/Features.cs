using System;
using System.Collections.Generic;

// Token: 0x02000171 RID: 369
public static class Features
{
	// Token: 0x04000851 RID: 2129
	public const string FEATURE_TREASURE = "allow_treasure";

	// Token: 0x04000852 RID: 2130
	public const string FEATURE_DRIFTWOOD = "allow_driftwood";

	// Token: 0x04000853 RID: 2131
	public const string FEATURE_DRIFTWOOD_TUTORIAL = "allow_driftwood_tutorial";

	// Token: 0x04000854 RID: 2132
	public const string FEATURE_JUST_IN_TIME_JJ_PURCHASES = "jit_jj_purchases";

	// Token: 0x04000855 RID: 2133
	public const string FEATURE_EXPAND = "purchase_expansions";

	// Token: 0x04000856 RID: 2134
	public const string FEATURE_EXPAND_BOARDWALK = "purchase_expansions_boardwalk";

	// Token: 0x04000857 RID: 2135
	public const string FEATURE_DEBRIS_CLEARING = "debris_clearing";

	// Token: 0x04000858 RID: 2136
	public const string FEATURE_WISHES = "resident_wishes";

	// Token: 0x04000859 RID: 2137
	public const string FEATURE_WISH_FULL_POOL = "resident_wishes_full_pool";

	// Token: 0x0400085A RID: 2138
	public const string FEATURE_INVENTORY_SOFT = "inventory_soft";

	// Token: 0x0400085B RID: 2139
	public const string FEATURE_STASHING_SOFT = "stash_soft";

	// Token: 0x0400085C RID: 2140
	public const string FEATURE_SELLING_SOFT = "sell_soft";

	// Token: 0x0400085D RID: 2141
	public const string FEATURE_MOVE_REJECT_LOCK = "move_reject_lock";

	// Token: 0x0400085E RID: 2142
	public const string FEATURE_RECIPE_DROPS = "recipe_drops";

	// Token: 0x0400085F RID: 2143
	public const string FEATURE_AUTO_FEED_LOCKOUT = "autofeed";

	// Token: 0x04000860 RID: 2144
	public const string FEATURE_ALLOW_RANDOM_QUESTS = "allow_random_quests";

	// Token: 0x04000861 RID: 2145
	public const string FEATURE_ALLOW_AUTO_QUESTS = "allow_auto_quests";

	// Token: 0x04000862 RID: 2146
	public const string FEATURE_ALLOW_PRODUCTION_SLOT_PURCHASE = "allow_production_slot_purchase";

	// Token: 0x04000863 RID: 2147
	public const string FEATURE_TUTORIAL_COMPLETE = "unrestrict_clicks";

	// Token: 0x04000864 RID: 2148
	public static readonly HashSet<string> FeatureSet = new HashSet<string>
	{
		"jit_jj_purchases",
		"purchase_expansions",
		"debris_clearing",
		"resident_wishes",
		"resident_wishes_full_pool",
		"inventory_soft",
		"stash_soft",
		"sell_soft",
		"move_reject_lock",
		"recipe_drops",
		"autofeed",
		"allow_random_quests",
		"allow_auto_quests",
		"allow_production_slot_purchase",
		"allow_treasure",
		"allow_driftwood",
		"allow_driftwood_tutorial",
		"unrestrict_clicks",
		"purchase_expansions_boardwalk"
	};
}
