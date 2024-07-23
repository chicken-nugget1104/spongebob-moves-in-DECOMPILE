using System;
using System.Collections.Generic;

// Token: 0x02000122 RID: 290
public class ButtonTapCondition : MatchableCondition
{
	// Token: 0x06000ABF RID: 2751 RVA: 0x000429E8 File Offset: 0x00040BE8
	public static ButtonTapCondition FromDict(Dictionary<string, object> dict)
	{
		ButtonTapCondition buttonTapCondition = new ButtonTapCondition();
		buttonTapCondition.Parse(dict, "button_tap", new List<string>
		{
			typeof(ButtonTapAction).ToString()
		}, new List<IMatcher>
		{
			ButtonTapMatcher.FromDict(dict)
		}, -1);
		return buttonTapCondition;
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x00042A38 File Offset: 0x00040C38
	public override string Description(Game game)
	{
		return "button_tap";
	}

	// Token: 0x04000745 RID: 1861
	public const string LOAD_TOKEN = "button_tap";
}
