using System;
using System.Collections.Generic;

// Token: 0x0200026F RID: 623
public class TriggerRouter
{
	// Token: 0x06001418 RID: 5144 RVA: 0x0008A4D4 File Offset: 0x000886D4
	public TriggerRouter(List<ITriggerObserver> observers)
	{
		this.observers = observers;
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x0008A4E4 File Offset: 0x000886E4
	public void RouteTrigger(ITrigger trigger, Game game)
	{
		foreach (ITriggerObserver triggerObserver in this.observers)
		{
			triggerObserver.ProcessTrigger(trigger, game);
		}
	}

	// Token: 0x04000E13 RID: 3603
	public const bool DEBUG_LOG_TRIGGERS = false;

	// Token: 0x04000E14 RID: 3604
	private List<ITriggerObserver> observers;
}
