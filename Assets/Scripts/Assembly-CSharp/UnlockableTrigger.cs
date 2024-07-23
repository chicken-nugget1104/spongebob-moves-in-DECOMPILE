using System;
using System.Collections.Generic;

// Token: 0x02000271 RID: 625
public static class UnlockableTrigger
{
	// Token: 0x0600141C RID: 5148 RVA: 0x0008A580 File Offset: 0x00088780
	public static ITrigger CreateTrigger(string unlockableType, int unlockableDid)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["unlockable_type"] = unlockableType;
		dictionary["unlockable_id"] = unlockableDid;
		return new Trigger("unlockable", dictionary);
	}

	// Token: 0x04000E15 RID: 3605
	public const string TYPE = "unlockable";
}
