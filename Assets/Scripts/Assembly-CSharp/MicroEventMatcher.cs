using System;
using System.Collections.Generic;

// Token: 0x0200018D RID: 397
public class MicroEventMatcher : Matcher
{
	// Token: 0x06000D72 RID: 3442 RVA: 0x0005247C File Offset: 0x0005067C
	public static MicroEventMatcher FromDict(Dictionary<string, object> dict)
	{
		MicroEventMatcher microEventMatcher = new MicroEventMatcher();
		microEventMatcher.RegisterProperty("micro_event_id", dict);
		return microEventMatcher;
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x000524A0 File Offset: 0x000506A0
	public override string DescribeSubject(Game game)
	{
		if (game == null)
		{
			return "did " + this.GetTarget("micro_event_id");
		}
		uint nDID = uint.Parse(this.GetTarget("micro_event_id"));
		return game.microEventManager.GetMicroEventData((int)nDID, false).m_sName;
	}

	// Token: 0x040008F0 RID: 2288
	public const string DEFINITION_ID = "micro_event_id";
}
