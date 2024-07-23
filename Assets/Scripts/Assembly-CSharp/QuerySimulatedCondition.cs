using System;
using System.Collections.Generic;

// Token: 0x02000149 RID: 329
public class QuerySimulatedCondition : MatchableCondition
{
	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00044F34 File Offset: 0x00043134
	public override bool IsExpensiveToCalculate
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00044F38 File Offset: 0x00043138
	public static QuerySimulatedCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedQuerier item = SimulatedQuerier.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		QuerySimulatedCondition querySimulatedCondition = new QuerySimulatedCondition();
		querySimulatedCondition.Parse(dict, "query_simulated", null, list, -1);
		return querySimulatedCondition;
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00044F70 File Offset: 0x00043170
	public override string Description(Game game)
	{
		return string.Empty;
	}

	// Token: 0x0400078F RID: 1935
	public const string LOAD_TOKEN = "query_simulated";

	// Token: 0x04000790 RID: 1936
	public const int QUERIER = 0;
}
