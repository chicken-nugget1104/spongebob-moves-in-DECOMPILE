using System;
using System.Collections.Generic;

// Token: 0x0200014C RID: 332
public class RushRentCondition : MatchableCondition
{
	// Token: 0x06000B5D RID: 2909 RVA: 0x00045064 File Offset: 0x00043264
	public static RushRentCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		RushRentCondition rushRentCondition = new RushRentCondition();
		rushRentCondition.Parse(dict, "rush_rent", new List<string>
		{
			typeof(RushRentAction).ToString()
		}, list, -1);
		return rushRentCondition;
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x000450CC File Offset: 0x000432CC
	public override string Description(Game game)
	{
		if (base.Matchers[0].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_RUSH_COLLECTION_ON_PLACE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
		}
		return string.Format(Language.Get("!!COND_RUSH_RESOURCE_COLLECTION"), Language.Get(base.Matchers[1].DescribeSubject(game)));
	}

	// Token: 0x04000795 RID: 1941
	public const string LOAD_TOKEN = "rush_rent";

	// Token: 0x04000796 RID: 1942
	public const int BUILDING_MATCHER = 0;

	// Token: 0x04000797 RID: 1943
	public const int RESOURCE_MATCHER = 1;
}
