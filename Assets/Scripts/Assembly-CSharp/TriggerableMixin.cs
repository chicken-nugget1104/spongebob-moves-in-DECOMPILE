using System;
using System.Collections.Generic;

// Token: 0x02000270 RID: 624
public class TriggerableMixin
{
	// Token: 0x0600141B RID: 5147 RVA: 0x0008A554 File Offset: 0x00088754
	public ITrigger BuildTrigger(string type, TriggerableMixin.AddDataCallback addMoreDataCallback, Identity target = null, Identity dropID = null)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		addMoreDataCallback(ref data);
		return new Trigger(type, data, TFUtils.EpochTime(), target, dropID);
	}

	// Token: 0x020004AC RID: 1196
	// (Invoke) Token: 0x06002513 RID: 9491
	public delegate void AddDataCallback(ref Dictionary<string, object> data);
}
