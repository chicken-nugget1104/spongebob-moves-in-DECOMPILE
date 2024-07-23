using System;
using System.Collections.Generic;

// Token: 0x02000133 RID: 307
public abstract class CraftCondition : MatchableCondition
{
	// Token: 0x06000B05 RID: 2821 RVA: 0x00043EE4 File Offset: 0x000420E4
	protected static void FromDictHelper(Dictionary<string, object> dict, CraftCondition objectToReturn, string loadToken, List<string> relevantTypes)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		objectToReturn.Parse(dict, loadToken, relevantTypes, list, -1);
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x00043F20 File Offset: 0x00042120
	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			if (base.Matchers[1].HasRequirements())
			{
				return string.Format(Language.Get("!!COND_CRAFT_RESOURCE_AT_PLACE"), Language.Get(base.Matchers[1].DescribeSubject(game)), Language.Get(base.Matchers[0].DescribeSubject(game)));
			}
			return string.Format(Language.Get("!!COND_CRAFT_AT"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		else
		{
			if (base.Matchers[1].HasRequirements())
			{
				return string.Format(Language.Get("!!COND_CRAFT"), Language.Get(base.Matchers[1].DescribeSubject(game)));
			}
			return string.Empty;
		}
	}

	// Token: 0x0400076D RID: 1901
	public const int BUILDING_MATCHER = 0;

	// Token: 0x0400076E RID: 1902
	public const int RESOURCE_MATCHER = 1;
}
