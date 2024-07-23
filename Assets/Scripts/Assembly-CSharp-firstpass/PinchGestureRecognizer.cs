using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
[AddComponentMenu("FingerGestures/Gesture Recognizers/Pinch")]
public class PinchGestureRecognizer : MultiFingerGestureRecognizer
{
	// Token: 0x14000037 RID: 55
	// (add) Token: 0x06000466 RID: 1126 RVA: 0x000124B4 File Offset: 0x000106B4
	// (remove) Token: 0x06000467 RID: 1127 RVA: 0x000124D0 File Offset: 0x000106D0
	public event FGComponent.EventDelegate<PinchGestureRecognizer> OnPinchBegin;

	// Token: 0x14000038 RID: 56
	// (add) Token: 0x06000468 RID: 1128 RVA: 0x000124EC File Offset: 0x000106EC
	// (remove) Token: 0x06000469 RID: 1129 RVA: 0x00012508 File Offset: 0x00010708
	public event FGComponent.EventDelegate<PinchGestureRecognizer> OnPinchMove;

	// Token: 0x14000039 RID: 57
	// (add) Token: 0x0600046A RID: 1130 RVA: 0x00012524 File Offset: 0x00010724
	// (remove) Token: 0x0600046B RID: 1131 RVA: 0x00012540 File Offset: 0x00010740
	public event FGComponent.EventDelegate<PinchGestureRecognizer> OnPinchEnd;

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600046C RID: 1132 RVA: 0x0001255C File Offset: 0x0001075C
	public float Delta
	{
		get
		{
			return this.delta;
		}
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00012564 File Offset: 0x00010764
	protected override int GetRequiredFingerCount()
	{
		return 2;
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00012568 File Offset: 0x00010768
	protected override bool CanBegin(FingerGestures.IFingerList touches)
	{
		if (!base.CanBegin(touches))
		{
			return false;
		}
		FingerGestures.Finger finger = touches[0];
		FingerGestures.Finger finger2 = touches[1];
		if (!FingerGestures.AllFingersMoving(new FingerGestures.Finger[]
		{
			finger,
			finger2
		}))
		{
			return false;
		}
		if (!this.FingersMovedInOppositeDirections(finger, finger2))
		{
			return false;
		}
		float f = this.ComputeGapDelta(finger, finger2, finger.StartPosition, finger2.StartPosition);
		return Mathf.Abs(f) >= this.MinDistance;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x000125E8 File Offset: 0x000107E8
	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		FingerGestures.Finger finger = touches[0];
		FingerGestures.Finger finger2 = touches[1];
		base.StartPosition[0] = finger.StartPosition;
		base.StartPosition[1] = finger2.StartPosition;
		base.Position[0] = finger.Position;
		base.Position[1] = finger2.Position;
		this.RaiseOnPinchBegin();
		float num = this.ComputeGapDelta(finger, finger2, finger.StartPosition, finger2.StartPosition);
		this.delta = this.DeltaScale * (num - Mathf.Sign(num) * this.MinDistance);
		this.RaiseOnPinchMove();
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x000126A0 File Offset: 0x000108A0
	protected override GestureRecognizer.GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != base.RequiredFingerCount)
		{
			if (touches.Count < base.RequiredFingerCount)
			{
				this.RaiseOnPinchEnd();
				return GestureRecognizer.GestureState.Recognized;
			}
			return GestureRecognizer.GestureState.Failed;
		}
		else
		{
			FingerGestures.Finger finger = touches[0];
			FingerGestures.Finger finger2 = touches[1];
			base.Position[0] = finger.Position;
			base.Position[1] = finger2.Position;
			if (!FingerGestures.AllFingersMoving(new FingerGestures.Finger[]
			{
				finger,
				finger2
			}))
			{
				return GestureRecognizer.GestureState.InProgress;
			}
			float num = this.ComputeGapDelta(finger, finger2, finger.PreviousPosition, finger2.PreviousPosition);
			if (Mathf.Abs(num) > 0.001f)
			{
				if (!this.FingersMovedInOppositeDirections(finger, finger2))
				{
					return GestureRecognizer.GestureState.InProgress;
				}
				this.delta = this.DeltaScale * num;
				this.RaiseOnPinchMove();
			}
			return GestureRecognizer.GestureState.InProgress;
		}
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0001277C File Offset: 0x0001097C
	protected void RaiseOnPinchBegin()
	{
		if (this.OnPinchBegin != null)
		{
			this.OnPinchBegin(this);
		}
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x00012798 File Offset: 0x00010998
	protected void RaiseOnPinchMove()
	{
		if (this.OnPinchMove != null)
		{
			this.OnPinchMove(this);
		}
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x000127B4 File Offset: 0x000109B4
	protected void RaiseOnPinchEnd()
	{
		if (this.OnPinchEnd != null)
		{
			this.OnPinchEnd(this);
		}
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x000127D0 File Offset: 0x000109D0
	private bool FingersMovedInOppositeDirections(FingerGestures.Finger finger0, FingerGestures.Finger finger1)
	{
		return FingerGestures.FingersMovedInOppositeDirections(finger0, finger1, this.MinDOT);
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x000127E0 File Offset: 0x000109E0
	private float ComputeGapDelta(FingerGestures.Finger finger0, FingerGestures.Finger finger1, Vector2 refPos1, Vector2 refPos2)
	{
		Vector2 vector = finger0.Position - finger1.Position;
		Vector2 vector2 = refPos1 - refPos2;
		return vector.magnitude - vector2.magnitude;
	}

	// Token: 0x04000256 RID: 598
	public float MinDOT = -0.7f;

	// Token: 0x04000257 RID: 599
	public float MinDistance = 5f;

	// Token: 0x04000258 RID: 600
	public float DeltaScale = 1f;

	// Token: 0x04000259 RID: 601
	protected float delta;
}
