using System;
using System.Collections.Generic;

// Token: 0x0200013F RID: 319
public class MoveCondition : MatchableCondition
{
	// Token: 0x06000B3A RID: 2874 RVA: 0x00044AC0 File Offset: 0x00042CC0
	public static MoveCondition FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher item = SimulatedMatcher.FromDict(dict);
		List<IMatcher> list = new List<IMatcher>();
		list.Insert(0, item);
		MoveCondition moveCondition = new MoveCondition();
		moveCondition.Parse(dict, "move", new List<string>
		{
			typeof(MoveAction).ToString()
		}, list, -1);
		return moveCondition;
	}

	// Token: 0x06000B3B RID: 2875 RVA: 0x00044B14 File Offset: 0x00042D14
	public override string Description(Game game)
	{
		return string.Format(Language.Get("!!COND_MOVE_OBJECT"), Language.Get(base.Matchers[0].DescribeSubject(game)));
	}

	// Token: 0x04000783 RID: 1923
	public const string LOAD_TOKEN = "move";

	// Token: 0x04000784 RID: 1924
	public const int TARGET_MATCHER = 0;
}
