using System;
using System.Collections.Generic;

// Token: 0x0200014D RID: 333
public class StartBuildingCondition : MatchableCondition
{
	// Token: 0x06000B60 RID: 2912 RVA: 0x00045144 File Offset: 0x00043344
	public static StartBuildingCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		ResourceMatcher item2 = ResourceMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		list.Insert(1, item2);
		StartBuildingCondition startBuildingCondition = new StartBuildingCondition();
		startBuildingCondition.Parse(dict, "start_building", new List<string>
		{
			typeof(NewBuildingAction).ToString()
		}, list, -1);
		return startBuildingCondition;
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x000451AC File Offset: 0x000433AC
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_START_BUILD"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x04000798 RID: 1944
	public const string LOAD_TOKEN = "start_building";

	// Token: 0x04000799 RID: 1945
	public const int BUILDING_MATCHER = 0;

	// Token: 0x0400079A RID: 1946
	public const int RESOURCE_MATCHER = 1;
}
