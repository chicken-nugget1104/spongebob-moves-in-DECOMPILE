using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[AddComponentMenu("FingerGestures/Toolbox/LongPress")]
public class TBLongPress : TBComponent
{
	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000141 RID: 321 RVA: 0x000071EC File Offset: 0x000053EC
	// (remove) Token: 0x06000142 RID: 322 RVA: 0x00007208 File Offset: 0x00005408
	public event TBComponent.EventHandler<TBLongPress> OnLongPress;

	// Token: 0x06000143 RID: 323 RVA: 0x00007224 File Offset: 0x00005424
	public bool RaiseLongPress(int fingerIndex, Vector2 fingerPos)
	{
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		if (this.OnLongPress != null)
		{
			this.OnLongPress(this);
		}
		base.Send(this.message);
		return true;
	}

	// Token: 0x040000BA RID: 186
	public TBComponent.Message message = new TBComponent.Message("OnLongPress");
}
