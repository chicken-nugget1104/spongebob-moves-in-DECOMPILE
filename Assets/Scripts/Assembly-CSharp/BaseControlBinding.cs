using System;

// Token: 0x020002A4 RID: 676
public abstract class BaseControlBinding : IControlBinding
{
	// Token: 0x170002CA RID: 714
	// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0008C1A8 File Offset: 0x0008A3A8
	public Action<Session> Action
	{
		get
		{
			return this.action;
		}
	}

	// Token: 0x170002CB RID: 715
	// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0008C1BC File Offset: 0x0008A3BC
	// (set) Token: 0x060014B6 RID: 5302 RVA: 0x0008C1B0 File Offset: 0x0008A3B0
	public SBGUIButton DynamicButton
	{
		get
		{
			return this.button;
		}
		set
		{
			this.button = value;
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x060014B8 RID: 5304 RVA: 0x0008C1C4 File Offset: 0x0008A3C4
	public Action Callback
	{
		get
		{
			return this.callback;
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x060014BA RID: 5306 RVA: 0x0008C1D8 File Offset: 0x0008A3D8
	// (set) Token: 0x060014B9 RID: 5305 RVA: 0x0008C1CC File Offset: 0x0008A3CC
	public string Label
	{
		get
		{
			return this.label;
		}
		set
		{
			this.label = value;
		}
	}

	// Token: 0x060014BB RID: 5307 RVA: 0x0008C1E0 File Offset: 0x0008A3E0
	public virtual void DynamicUpdate(Session session)
	{
	}

	// Token: 0x060014BC RID: 5308 RVA: 0x0008C1E4 File Offset: 0x0008A3E4
	protected void Initialize(Action<Session> action, Action callback, string targetSessionActionToken)
	{
		this.action = action;
		this.callback = callback;
		this.targetSessionActionToken = targetSessionActionToken;
	}

	// Token: 0x060014BD RID: 5309 RVA: 0x0008C1FC File Offset: 0x0008A3FC
	public string DecorateSessionActionId(uint ownerDid)
	{
		return SessionActionSimulationHelper.DecorateSessionActionId(ownerDid, this.targetSessionActionToken);
	}

	// Token: 0x04000E7B RID: 3707
	private Action<Session> action;

	// Token: 0x04000E7C RID: 3708
	private SBGUIButton button;

	// Token: 0x04000E7D RID: 3709
	private Action callback;

	// Token: 0x04000E7E RID: 3710
	private string label;

	// Token: 0x04000E7F RID: 3711
	private string targetSessionActionToken;
}
