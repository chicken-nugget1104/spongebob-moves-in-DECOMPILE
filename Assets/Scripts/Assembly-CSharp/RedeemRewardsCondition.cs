using System;
using System.Collections.Generic;

// Token: 0x0200014A RID: 330
public class RedeemRewardsCondition : MatchableCondition
{
	// Token: 0x06000B58 RID: 2904 RVA: 0x00044F80 File Offset: 0x00043180
	public static RedeemRewardsCondition FromDict(Dictionary<string, object> dict)
	{
		RedemptionMatcher item = RedemptionMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		RedeemRewardsCondition redeemRewardsCondition = new RedeemRewardsCondition();
		redeemRewardsCondition.Parse(dict, "redeem_reward", new List<string>
		{
			"rra"
		}, list, -1);
		return redeemRewardsCondition;
	}

	// Token: 0x04000791 RID: 1937
	public const string LOAD_TOKEN = "redeem_reward";

	// Token: 0x04000792 RID: 1938
	public const int REDEMPTION_MATCHER = 0;
}
