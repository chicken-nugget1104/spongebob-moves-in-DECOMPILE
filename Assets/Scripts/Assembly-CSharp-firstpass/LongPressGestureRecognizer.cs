using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
[AddComponentMenu("FingerGestures/Gesture Recognizers/Long Press")]
public class LongPressGestureRecognizer : AveragedGestureRecognizer
{
	// Token: 0x14000035 RID: 53
	// (add) Token: 0x06000452 RID: 1106 RVA: 0x00011F74 File Offset: 0x00010174
	// (remove) Token: 0x06000453 RID: 1107 RVA: 0x00011F90 File Offset: 0x00010190
	public event FGComponent.EventDelegate<LongPressGestureRecognizer> OnLongPress;

	// Token: 0x06000454 RID: 1108 RVA: 0x00011FAC File Offset: 0x000101AC
	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00011FD4 File Offset: 0x000101D4
	protected override GestureRecognizer.GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != this.RequiredFingerCount)
		{
			return GestureRecognizer.GestureState.Failed;
		}
		if (base.ElapsedTime >= this.Duration)
		{
			this.RaiseOnLongPress();
			return GestureRecognizer.GestureState.Recognized;
		}
		if (touches.GetAverageDistanceFromStart() > this.MoveTolerance)
		{
			return GestureRecognizer.GestureState.Failed;
		}
		return GestureRecognizer.GestureState.InProgress;
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00012024 File Offset: 0x00010224
	protected void RaiseOnLongPress()
	{
		if (this.OnLongPress != null)
		{
			this.OnLongPress(this);
		}
	}

	// Token: 0x04000245 RID: 581
	public float Duration = 1f;

	// Token: 0x04000246 RID: 582
	public float MoveTolerance = 5f;
}
