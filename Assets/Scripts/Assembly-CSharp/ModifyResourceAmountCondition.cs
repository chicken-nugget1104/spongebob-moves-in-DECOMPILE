using System;
using System.Collections.Generic;

// Token: 0x0200013E RID: 318
public class ModifyResourceAmountCondition : MatchableCondition
{
	// Token: 0x06000B37 RID: 2871 RVA: 0x00044968 File Offset: 0x00042B68
	public static ModifyResourceAmountCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		TFUtils.Assert(list[0].IsRequired("delta") || list[0].IsRequired("balance"), string.Format("You must specify either {0} or {1} for this condition!", "delta", "balance"));
		ModifyResourceAmountCondition modifyResourceAmountCondition = new ModifyResourceAmountCondition();
		modifyResourceAmountCondition.Parse(dict, "update_resource", new List<string>
		{
			"UpdateResource"
		}, list, -1);
		return modifyResourceAmountCondition;
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x000449F4 File Offset: 0x00042BF4
	public override string Description(Game game)
	{
		if (base.Matchers[0].IsRequired("balance"))
		{
			return string.Format(Language.Get("!!COND_CHANGE_RESOURCE"), base.Matchers[0].GetTarget("balance"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		if (base.Matchers[0].IsRequired("delta"))
		{
			return string.Format(Language.Get("!!COND_CHANGE_RESOURCE"), base.Matchers[0].GetTarget("delta"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return string.Empty;
	}

	// Token: 0x04000781 RID: 1921
	public const string LOAD_TOKEN = "update_resource";

	// Token: 0x04000782 RID: 1922
	public const int RESOURCE_MATCHER = 0;
}
