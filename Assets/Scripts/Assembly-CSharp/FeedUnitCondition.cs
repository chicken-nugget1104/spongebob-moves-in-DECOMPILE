using System;
using System.Collections.Generic;

// Token: 0x02000138 RID: 312
public class FeedUnitCondition : MatchableCondition
{
	// Token: 0x06000B13 RID: 2835 RVA: 0x00044214 File Offset: 0x00042414
	public static FeedUnitCondition FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher item = ResourceMatcher.FromDict(dict);
		SimulatedMatcher item2 = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item2);
		list.Insert(1, item);
		FeedUnitCondition feedUnitCondition = new FeedUnitCondition();
		feedUnitCondition.Parse(dict, "feed_unit", new List<string>
		{
			typeof(FeedUnitAction).ToString()
		}, list, -1);
		return feedUnitCondition;
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x0004427C File Offset: 0x0004247C
	public override string Description(Game game)
	{
		if (!base.Matchers[0].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_FEED_SOMEONE_RESOURCE"), Language.Get(base.Matchers[1].DescribeSubject(game)));
		}
		if (base.Matchers[1].HasRequirements())
		{
			return string.Format(Language.Get("!!COND_FEED_UNIT_RESOURCE"), Language.Get(base.Matchers[0].DescribeSubject(game)), Language.Get(base.Matchers[1].DescribeSubject(game)));
		}
		return string.Format(Language.Get("!!COND_FEED_UNIT"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x04000776 RID: 1910
	public const string LOAD_TOKEN = "feed_unit";

	// Token: 0x04000777 RID: 1911
	public const int UNIT_MATCHER = 0;

	// Token: 0x04000778 RID: 1912
	public const int RESOURCE_MATCHER = 1;
}
