using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
[AddComponentMenu("FingerGestures/Gesture Recognizers/Rotation")]
public class RotationGestureRecognizer : MultiFingerGestureRecognizer
{
	// Token: 0x1400003A RID: 58
	// (add) Token: 0x06000477 RID: 1143 RVA: 0x00012838 File Offset: 0x00010A38
	// (remove) Token: 0x06000478 RID: 1144 RVA: 0x00012854 File Offset: 0x00010A54
	public event FGComponent.EventDelegate<RotationGestureRecognizer> OnRotationBegin;

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x06000479 RID: 1145 RVA: 0x00012870 File Offset: 0x00010A70
	// (remove) Token: 0x0600047A RID: 1146 RVA: 0x0001288C File Offset: 0x00010A8C
	public event FGComponent.EventDelegate<RotationGestureRecognizer> OnRotationMove;

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x0600047B RID: 1147 RVA: 0x000128A8 File Offset: 0x00010AA8
	// (remove) Token: 0x0600047C RID: 1148 RVA: 0x000128C4 File Offset: 0x00010AC4
	public event FGComponent.EventDelegate<RotationGestureRecognizer> OnRotationEnd;

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600047D RID: 1149 RVA: 0x000128E0 File Offset: 0x00010AE0
	public float TotalRotation
	{
		get
		{
			return this.totalRotation;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x0600047E RID: 1150 RVA: 0x000128E8 File Offset: 0x00010AE8
	public float RotationDelta
	{
		get
		{
			return this.rotationDelta;
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x000128F0 File Offset: 0x00010AF0
	private bool FingersMovedInOppositeDirections(FingerGestures.Finger finger0, FingerGestures.Finger finger1)
	{
		return FingerGestures.FingersMovedInOppositeDirections(finger0, finger1, this.MinDOT);
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00012900 File Offset: 0x00010B00
	private static float SignedAngularGap(FingerGestures.Finger finger0, FingerGestures.Finger finger1, Vector2 refPos0, Vector2 refPos1)
	{
		Vector2 normalized = (finger0.Position - finger1.Position).normalized;
		Vector2 normalized2 = (refPos0 - refPos1).normalized;
		return 57.29578f * FingerGestures.SignedAngle(normalized2, normalized);
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00012944 File Offset: 0x00010B44
	protected override int GetRequiredFingerCount()
	{
		return 2;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00012948 File Offset: 0x00010B48
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
		float f = RotationGestureRecognizer.SignedAngularGap(finger, finger2, finger.StartPosition, finger2.StartPosition);
		return Mathf.Abs(f) >= this.MinRotation;
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x000129C4 File Offset: 0x00010BC4
	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		FingerGestures.Finger finger = touches[0];
		FingerGestures.Finger finger2 = touches[1];
		base.StartPosition[0] = finger.StartPosition;
		base.StartPosition[1] = finger2.StartPosition;
		base.Position[0] = finger.Position;
		base.Position[1] = finger2.Position;
		float num = RotationGestureRecognizer.SignedAngularGap(finger, finger2, finger.StartPosition, finger2.StartPosition);
		this.totalRotation = Mathf.Sign(num) * this.MinRotation;
		this.rotationDelta = 0f;
		if (this.OnRotationBegin != null)
		{
			this.OnRotationBegin(this);
		}
		this.rotationDelta = num - this.totalRotation;
		this.totalRotation = num;
		if (this.OnRotationMove != null)
		{
			this.OnRotationMove(this);
		}
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00012AB4 File Offset: 0x00010CB4
	protected override GestureRecognizer.GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != base.RequiredFingerCount)
		{
			if (touches.Count < base.RequiredFingerCount)
			{
				if (this.OnRotationEnd != null)
				{
					this.OnRotationEnd(this);
				}
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
			this.rotationDelta = RotationGestureRecognizer.SignedAngularGap(finger, finger2, finger.PreviousPosition, finger2.PreviousPosition);
			this.totalRotation += this.rotationDelta;
			if (this.OnRotationMove != null)
			{
				this.OnRotationMove(this);
			}
			return GestureRecognizer.GestureState.InProgress;
		}
	}

	// Token: 0x0400025D RID: 605
	public float MinDOT = -0.7f;

	// Token: 0x0400025E RID: 606
	public float MinRotation = 1f;

	// Token: 0x0400025F RID: 607
	private float totalRotation;

	// Token: 0x04000260 RID: 608
	private float rotationDelta;
}
