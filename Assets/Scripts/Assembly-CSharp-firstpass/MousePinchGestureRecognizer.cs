using System;
using UnityEngine;

// Token: 0x0200007D RID: 125
[AddComponentMenu("FingerGestures/Gesture Recognizers/Mouse Pinch")]
public class MousePinchGestureRecognizer : PinchGestureRecognizer
{
	// Token: 0x06000458 RID: 1112 RVA: 0x0001205C File Offset: 0x0001025C
	protected override int GetRequiredFingerCount()
	{
		return this.requiredFingers;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x00012064 File Offset: 0x00010264
	protected override bool CanBegin(FingerGestures.IFingerList touches)
	{
		if (!this.CheckCanBeginDelegate(touches))
		{
			return false;
		}
		float f = Input.GetAxis(this.axis);
		return Mathf.Abs(f) >= 0.0001f;
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x000120A0 File Offset: 0x000102A0
	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.StartPosition[0] = (base.StartPosition[1] = Input.mousePosition);
		base.Position[0] = (base.Position[1] = Input.mousePosition);
		this.delta = 0f;
		base.RaiseOnPinchBegin();
		this.delta = this.DeltaScale * Input.GetAxis(this.axis);
		this.resetTime = Time.time + 0.1f;
		base.RaiseOnPinchMove();
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0001214C File Offset: 0x0001034C
	protected override GestureRecognizer.GestureState OnActive(FingerGestures.IFingerList touches)
	{
		float num = Input.GetAxis(this.axis);
		if (Mathf.Abs(num) >= 0.001f)
		{
			this.resetTime = Time.time + 0.1f;
			base.Position[0] = (base.Position[1] = Input.mousePosition);
			this.delta = this.DeltaScale * num;
			base.RaiseOnPinchMove();
			return GestureRecognizer.GestureState.InProgress;
		}
		if (this.resetTime <= Time.time)
		{
			base.RaiseOnPinchEnd();
			return GestureRecognizer.GestureState.Recognized;
		}
		return GestureRecognizer.GestureState.InProgress;
	}

	// Token: 0x04000248 RID: 584
	public string axis = "Mouse ScrollWheel";

	// Token: 0x04000249 RID: 585
	private int requiredFingers = 2;

	// Token: 0x0400024A RID: 586
	private float resetTime;
}
