using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[AddComponentMenu("FingerGestures/Toolbox/Tap")]
public class TBTap : TBComponent
{
	// Token: 0x14000008 RID: 8
	// (add) Token: 0x0600014F RID: 335 RVA: 0x00007458 File Offset: 0x00005658
	// (remove) Token: 0x06000150 RID: 336 RVA: 0x00007474 File Offset: 0x00005674
	public event TBComponent.EventHandler<TBTap> OnTap;

	// Token: 0x06000151 RID: 337 RVA: 0x00007490 File Offset: 0x00005690
	public bool RaiseTap(int fingerIndex, Vector2 fingerPos)
	{
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		if (this.OnTap != null)
		{
			this.OnTap(this);
		}
		base.Send(this.message);
		return true;
	}

	// Token: 0x040000C9 RID: 201
	public TBTap.TapMode tapMode;

	// Token: 0x040000CA RID: 202
	public TBComponent.Message message = new TBComponent.Message("OnTap");

	// Token: 0x02000022 RID: 34
	public enum TapMode
	{
		// Token: 0x040000CD RID: 205
		SingleTap,
		// Token: 0x040000CE RID: 206
		DoubleTap
	}
}
