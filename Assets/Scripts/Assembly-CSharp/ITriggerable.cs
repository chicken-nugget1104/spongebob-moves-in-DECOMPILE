using System;
using System.Collections.Generic;

// Token: 0x0200026B RID: 619
public interface ITriggerable
{
	// Token: 0x0600140B RID: 5131
	ITrigger CreateTrigger(Dictionary<string, object> data);
}
