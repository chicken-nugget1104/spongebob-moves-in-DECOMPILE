using System;
using System.Collections.Generic;

// Token: 0x02000191 RID: 401
public class RedemptionMatcher : Matcher
{
	// Token: 0x06000D7F RID: 3455 RVA: 0x00052678 File Offset: 0x00050878
	public static RedemptionMatcher FromDict(Dictionary<string, object> dict)
	{
		RedemptionMatcher redemptionMatcher = new RedemptionMatcher();
		redemptionMatcher.RegisterProperty("redemption_id", dict);
		return redemptionMatcher;
	}

	// Token: 0x06000D80 RID: 3456 RVA: 0x0005269C File Offset: 0x0005089C
	public override string DescribeSubject(Game game)
	{
		return "redeem offer";
	}

	// Token: 0x040008F4 RID: 2292
	public const string REDEMPTION_ID = "redemption_id";
}
