using System;
using System.Collections.Generic;

// Token: 0x02000186 RID: 390
public class ButtonTapMatcher : Matcher
{
	// Token: 0x06000D46 RID: 3398 RVA: 0x00051E68 File Offset: 0x00050068
	public static ButtonTapMatcher FromDict(Dictionary<string, object> dict)
	{
		ButtonTapMatcher buttonTapMatcher = new ButtonTapMatcher();
		buttonTapMatcher.RegisterProperty("button_id", dict);
		return buttonTapMatcher;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00051E8C File Offset: 0x0005008C
	public override string DescribeSubject(Game game)
	{
		return "button_tap";
	}

	// Token: 0x040008E3 RID: 2275
	public const string BUTTON_ID = "button_id";
}
