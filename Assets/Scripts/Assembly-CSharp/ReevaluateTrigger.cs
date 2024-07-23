using System;
using System.Collections.Generic;

// Token: 0x0200026C RID: 620
public static class ReevaluateTrigger
{
	// Token: 0x0600140C RID: 5132 RVA: 0x0008A228 File Offset: 0x00088428
	public static ITrigger CreateTrigger()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		return new Trigger("reevaluate", data);
	}

	// Token: 0x04000E0A RID: 3594
	public const string TYPE = "reevaluate";
}
