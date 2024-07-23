using System;

// Token: 0x020000A0 RID: 160
public class SBGUIScrollListElement : SBGUIElement
{
	// Token: 0x060005DE RID: 1502 RVA: 0x000255D8 File Offset: 0x000237D8
	protected virtual void OnBecameVisible()
	{
		this.VisibleEvent.FireEvent();
		this.VisibleEvent.ClearListeners();
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x000255F0 File Offset: 0x000237F0
	protected virtual void OnBecameInvisible()
	{
		this.InvisibleEvent.FireEvent();
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x00025600 File Offset: 0x00023800
	public virtual void Deactivate()
	{
		this.VisibleEvent.ClearListeners();
		this.InvisibleEvent.ClearListeners();
		this.MuteButtons(false);
		this.SetParent(null);
		this.SetActive(false);
	}

	// Token: 0x04000475 RID: 1141
	public EventDispatcher VisibleEvent = new EventDispatcher();

	// Token: 0x04000476 RID: 1142
	public EventDispatcher InvisibleEvent = new EventDispatcher();
}
