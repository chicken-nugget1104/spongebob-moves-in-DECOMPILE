using System;
using System.Collections.Generic;

// Token: 0x02000132 RID: 306
public class CraftCollectCondition : CraftCondition
{
	// Token: 0x06000B03 RID: 2819 RVA: 0x00043EA8 File Offset: 0x000420A8
	public static CraftCollectCondition FromDict(Dictionary<string, object> dict)
	{
		CraftCollectCondition craftCollectCondition = new CraftCollectCondition();
		CraftCondition.FromDictHelper(dict, craftCollectCondition, "craft_collect", new List<string>
		{
			"CraftPickup"
		});
		return craftCollectCondition;
	}

	// Token: 0x0400076C RID: 1900
	public const string LOAD_TOKEN = "craft_collect";
}
