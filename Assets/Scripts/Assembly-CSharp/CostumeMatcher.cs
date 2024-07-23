using System;
using System.Collections.Generic;

// Token: 0x02000187 RID: 391
public class CostumeMatcher : Matcher
{
	// Token: 0x06000D49 RID: 3401 RVA: 0x00051E9C File Offset: 0x0005009C
	public static CostumeMatcher FromDict(Dictionary<string, object> dict)
	{
		CostumeMatcher costumeMatcher = new CostumeMatcher();
		costumeMatcher.RegisterProperty("costume_id", dict);
		return costumeMatcher;
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x00051EC0 File Offset: 0x000500C0
	public override string DescribeSubject(Game game)
	{
		if (game == null)
		{
			return "did " + this.GetTarget("costume_id");
		}
		uint nCostumeDID = uint.Parse(this.GetTarget("costume_id"));
		return game.costumeManager.GetCostume((int)nCostumeDID).m_sName;
	}

	// Token: 0x040008E4 RID: 2276
	public const string DEFINITION_ID = "costume_id";
}
