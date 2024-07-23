using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
[AddComponentMenu("FingerGestures/Toolbox/FingerUp")]
public class TBFingerUp : TBComponent
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600012B RID: 299 RVA: 0x00006AD0 File Offset: 0x00004CD0
	// (remove) Token: 0x0600012C RID: 300 RVA: 0x00006AEC File Offset: 0x00004CEC
	public event TBComponent.EventHandler<TBFingerUp> OnFingerUp;

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600012D RID: 301 RVA: 0x00006B08 File Offset: 0x00004D08
	// (set) Token: 0x0600012E RID: 302 RVA: 0x00006B10 File Offset: 0x00004D10
	public float TimeHeldDown
	{
		get
		{
			return this.timeHeldDown;
		}
		private set
		{
			this.timeHeldDown = value;
		}
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00006B1C File Offset: 0x00004D1C
	public bool RaiseFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		this.TimeHeldDown = timeHeldDown;
		if (this.OnFingerUp != null)
		{
			this.OnFingerUp(this);
		}
		base.Send(this.message);
		return true;
	}

	// Token: 0x040000A6 RID: 166
	public TBComponent.Message message = new TBComponent.Message("OnFingerUp");

	// Token: 0x040000A7 RID: 167
	private float timeHeldDown;
}
