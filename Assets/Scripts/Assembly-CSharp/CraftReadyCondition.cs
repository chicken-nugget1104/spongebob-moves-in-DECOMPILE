using System;
using System.Collections.Generic;

// Token: 0x02000134 RID: 308
public class CraftReadyCondition : CraftCondition
{
	// Token: 0x06000B08 RID: 2824 RVA: 0x00044008 File Offset: 0x00042208
	public static CraftReadyCondition FromDict(Dictionary<string, object> dict)
	{
		CraftReadyCondition craftReadyCondition = new CraftReadyCondition();
		CraftCondition.FromDictHelper(dict, craftReadyCondition, "craft_ready", new List<string>
		{
			typeof(CraftCompleteAction).ToString()
		});
		return craftReadyCondition;
	}

	// Token: 0x0400076F RID: 1903
	public const string LOAD_TOKEN = "craft_ready";
}
