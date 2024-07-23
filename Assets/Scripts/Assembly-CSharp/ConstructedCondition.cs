using System;
using System.Collections.Generic;

// Token: 0x02000131 RID: 305
public class ConstructedCondition : MatchableCondition
{
	// Token: 0x06000B00 RID: 2816 RVA: 0x00043E24 File Offset: 0x00042024
	public static ConstructedCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Add(item);
		ConstructedCondition constructedCondition = new ConstructedCondition();
		constructedCondition.Parse(dict, "constructed", new List<string>
		{
			"contruction_complete"
		}, list, -1);
		return constructedCondition;
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x00043E6C File Offset: 0x0004206C
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!PLACE_SIMULATED_TYPE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x0400076B RID: 1899
	public const string LOAD_TOKEN = "constructed";
}
