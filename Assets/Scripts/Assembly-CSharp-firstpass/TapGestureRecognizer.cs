using System;
using UnityEngine;

// Token: 0x02000082 RID: 130
[AddComponentMenu("FingerGestures/Gesture Recognizers/Tap")]
public class TapGestureRecognizer : AveragedGestureRecognizer
{
	// Token: 0x1400003E RID: 62
	// (add) Token: 0x06000491 RID: 1169 RVA: 0x00012DCC File Offset: 0x00010FCC
	// (remove) Token: 0x06000492 RID: 1170 RVA: 0x00012DE8 File Offset: 0x00010FE8
	public event FGComponent.EventDelegate<TapGestureRecognizer> OnTap;

	// Token: 0x06000493 RID: 1171 RVA: 0x00012E04 File Offset: 0x00011004
	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00012E2C File Offset: 0x0001102C
	protected override GestureRecognizer.GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != this.RequiredFingerCount)
		{
			if (touches.Count == 0)
			{
				this.RaiseOnTap();
				return GestureRecognizer.GestureState.Recognized;
			}
			return GestureRecognizer.GestureState.Failed;
		}
		else
		{
			if (this.MaxDuration > 0f && base.ElapsedTime > this.MaxDuration)
			{
				return GestureRecognizer.GestureState.Failed;
			}
			float num = Vector3.SqrMagnitude(touches.GetAveragePosition() - base.StartPosition);
			if (num >= this.MoveTolerance * this.MoveTolerance)
			{
				return GestureRecognizer.GestureState.Failed;
			}
			return GestureRecognizer.GestureState.InProgress;
		}
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00012EB4 File Offset: 0x000110B4
	protected void RaiseOnTap()
	{
		if (this.OnTap != null)
		{
			this.OnTap(this);
		}
	}

	// Token: 0x0400026C RID: 620
	public float MoveTolerance = 5f;

	// Token: 0x0400026D RID: 621
	public float MaxDuration;
}
