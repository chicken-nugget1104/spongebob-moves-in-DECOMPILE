using System;
using System.Collections.Generic;

// Token: 0x02000135 RID: 309
public class CraftStartCondition : CraftCondition
{
	// Token: 0x06000B0A RID: 2826 RVA: 0x0004404C File Offset: 0x0004224C
	public static CraftStartCondition FromDict(Dictionary<string, object> dict)
	{
		CraftStartCondition craftStartCondition = new CraftStartCondition();
		CraftCondition.FromDictHelper(dict, craftStartCondition, "craft_start", new List<string>
		{
			typeof(CraftStartAction).ToString()
		});
		return craftStartCondition;
	}

	// Token: 0x04000770 RID: 1904
	public const string LOAD_TOKEN = "craft_start";
}
