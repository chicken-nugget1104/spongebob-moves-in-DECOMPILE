using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[AddComponentMenu("FingerGestures/Toolbox/FingerDown")]
public class TBFingerDown : TBComponent
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000127 RID: 295 RVA: 0x00006A40 File Offset: 0x00004C40
	// (remove) Token: 0x06000128 RID: 296 RVA: 0x00006A5C File Offset: 0x00004C5C
	public event TBComponent.EventHandler<TBFingerDown> OnFingerDown;

	// Token: 0x06000129 RID: 297 RVA: 0x00006A78 File Offset: 0x00004C78
	public bool RaiseFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		if (this.OnFingerDown != null)
		{
			this.OnFingerDown(this);
		}
		base.Send(this.message);
		return true;
	}

	// Token: 0x040000A4 RID: 164
	public TBComponent.Message message = new TBComponent.Message("OnFingerDown");
}
