using System;
using System.Collections.Generic;

// Token: 0x02000146 RID: 326
public class PlaceCondition : MatchableCondition
{
	// Token: 0x06000B4B RID: 2891 RVA: 0x00044D64 File Offset: 0x00042F64
	public static PlaceCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Add(item);
		PlaceCondition placeCondition = new PlaceCondition();
		placeCondition.Parse(dict, "place", new List<string>
		{
			typeof(NewBuildingAction).ToString()
		}, list, -1);
		return placeCondition;
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x00044DB8 File Offset: 0x00042FB8
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!PLACE_SIMULATED_TYPE"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x0400078A RID: 1930
	public const string LOAD_TOKEN = "place";
}
