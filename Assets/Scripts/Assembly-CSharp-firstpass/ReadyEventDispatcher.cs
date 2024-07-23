using System;

// Token: 0x0200006B RID: 107
public class ReadyEventDispatcher : EventDispatcher
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060003BF RID: 959 RVA: 0x000103B0 File Offset: 0x0000E5B0
	public bool IsReady
	{
		get
		{
			return this.ready;
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x000103B8 File Offset: 0x0000E5B8
	public override void AddListener(Action value)
	{
		if (this.ready && value != null)
		{
			value();
			return;
		}
		base.AddListener(value);
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x000103DC File Offset: 0x0000E5DC
	public override void FireEvent()
	{
		this.ready = true;
		base.FireEvent();
		base.ClearListeners();
	}

	// Token: 0x0400020B RID: 523
	private bool ready;
}
