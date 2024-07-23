using System;

// Token: 0x0200025B RID: 603
public class UiSpawnMixin
{
	// Token: 0x06001352 RID: 4946 RVA: 0x00085450 File Offset: 0x00083650
	public void OnRegisterNewInstance(SessionActionTracker parentAction, SBGUIScreen containingScreen)
	{
		this.parentAction = parentAction;
		this.containingScreen = containingScreen;
		this.containingScreen.OnPutIntoCache.AddListener(new Action(this.FailOnStash));
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x00085488 File Offset: 0x00083688
	public void Destroy()
	{
		this.containingScreen.OnPutIntoCache.RemoveListener(new Action(this.FailOnStash));
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x000854A8 File Offset: 0x000836A8
	private void FailOnStash()
	{
		if (this.parentAction.Status == SessionActionTracker.StatusCode.STARTED)
		{
			this.parentAction.MarkFailed();
		}
	}

	// Token: 0x04000D6A RID: 3434
	private SessionActionTracker parentAction;

	// Token: 0x04000D6B RID: 3435
	private SBGUIScreen containingScreen;
}
