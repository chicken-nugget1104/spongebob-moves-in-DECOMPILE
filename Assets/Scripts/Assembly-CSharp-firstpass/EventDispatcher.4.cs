using System;

// Token: 0x0200006C RID: 108
public class EventDispatcher
{
	// Token: 0x14000029 RID: 41
	// (add) Token: 0x060003C3 RID: 963 RVA: 0x000103FC File Offset: 0x0000E5FC
	// (remove) Token: 0x060003C4 RID: 964 RVA: 0x00010418 File Offset: 0x0000E618
	private event Action eventListener;

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060003C5 RID: 965 RVA: 0x00010434 File Offset: 0x0000E634
	public bool HasListeners
	{
		get
		{
			return this.eventListener != null;
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00010444 File Offset: 0x0000E644
	public virtual void AddListener(Action value)
	{
		if (value == null)
		{
			return;
		}
		if (this.eventListener == null)
		{
			this.eventListener = (Action)Delegate.Combine(this.eventListener, value);
			return;
		}
		this.eventListener = (Action)Delegate.Remove(this.eventListener, value);
		this.eventListener = (Action)Delegate.Combine(this.eventListener, value);
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x000104AC File Offset: 0x0000E6AC
	public virtual void RemoveListener(Action value)
	{
		if (this.eventListener == null)
		{
			return;
		}
		this.eventListener = (Action)Delegate.Remove(this.eventListener, value);
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x000104D4 File Offset: 0x0000E6D4
	public void ClearListeners()
	{
		this.eventListener = null;
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x000104E0 File Offset: 0x0000E6E0
	public virtual void FireEvent()
	{
		if (this.eventListener == null)
		{
			return;
		}
		this.eventListener();
	}
}
