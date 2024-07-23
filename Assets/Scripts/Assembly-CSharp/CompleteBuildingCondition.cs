using System;
using System.Collections.Generic;

// Token: 0x02000126 RID: 294
public class CompleteBuildingCondition : MatchableCondition
{
	// Token: 0x06000ACB RID: 2763 RVA: 0x00042CD0 File Offset: 0x00040ED0
	public static CompleteBuildingCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		ResourceMatcher item2 = ResourceMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		list.Insert(1, item2);
		CompleteBuildingCondition completeBuildingCondition = new CompleteBuildingCondition();
		completeBuildingCondition.Parse(dict, "complete_building", new List<string>
		{
			typeof(CompleteBuildingAction).ToString()
		}, list, -1);
		return completeBuildingCondition;
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00042D38 File Offset: 0x00040F38
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_BUILD"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x0400074D RID: 1869
	public const string LOAD_TOKEN = "complete_building";

	// Token: 0x0400074E RID: 1870
	public const int BUILDING_MATCHER = 0;

	// Token: 0x0400074F RID: 1871
	public const int RESOURCE_MATCHER = 1;
}
