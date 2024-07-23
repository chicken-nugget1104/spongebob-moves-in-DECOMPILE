using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
[AddComponentMenu("FingerGestures/Gesture Recognizers/Tap (Multi)")]
public class MultiTapGestureRecognizer : AveragedGestureRecognizer
{
	// Token: 0x14000036 RID: 54
	// (add) Token: 0x0600045D RID: 1117 RVA: 0x00012204 File Offset: 0x00010404
	// (remove) Token: 0x0600045E RID: 1118 RVA: 0x00012220 File Offset: 0x00010420
	public event FGComponent.EventDelegate<MultiTapGestureRecognizer> OnTap;

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600045F RID: 1119 RVA: 0x0001223C File Offset: 0x0001043C
	public int Taps
	{
		get
		{
			return this.taps;
		}
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00012244 File Offset: 0x00010444
	private bool HasTimedOut()
	{
		return (this.MaxDelayBetweenTaps > 0f && Time.time - this.lastTapTime > this.MaxDelayBetweenTaps) || (this.MaxDuration > 0f && base.ElapsedTime > this.MaxDuration);
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x000122A0 File Offset: 0x000104A0
	protected override void Reset()
	{
		this.taps = 0;
		this.down = false;
		this.wasDown = false;
		base.Reset();
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x000122C0 File Offset: 0x000104C0
	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
		this.lastTapTime = Time.time;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x000122F0 File Offset: 0x000104F0
	protected override GestureRecognizer.GestureState OnActive(FingerGestures.IFingerList touches)
	{
		this.wasDown = this.down;
		this.down = false;
		if (touches.Count == this.RequiredFingerCount)
		{
			this.down = true;
			this.lastDownTime = Time.time;
		}
		else if (touches.Count == 0)
		{
			this.down = false;
		}
		else if (touches.Count < this.RequiredFingerCount)
		{
			if (Time.time - this.lastDownTime > 0.25f)
			{
				return GestureRecognizer.GestureState.Failed;
			}
		}
		else if (!base.Young(touches))
		{
			return GestureRecognizer.GestureState.Failed;
		}
		if (!this.HasTimedOut())
		{
			if (this.down)
			{
				float num = Vector3.SqrMagnitude(touches.GetAveragePosition() - base.StartPosition);
				if (num >= this.MoveTolerance * this.MoveTolerance)
				{
					return GestureRecognizer.GestureState.Failed;
				}
			}
			if (this.wasDown != this.down && !this.down)
			{
				this.taps++;
				this.lastTapTime = Time.time;
				if (this.RequiredTaps > 0 && this.taps >= this.RequiredTaps)
				{
					this.RaiseOnTap();
					return GestureRecognizer.GestureState.Recognized;
				}
				if (this.RaiseEventOnEachTap)
				{
					this.RaiseOnTap();
				}
			}
			return GestureRecognizer.GestureState.InProgress;
		}
		if (this.RequiredTaps == 0 && this.Taps > 0)
		{
			if (!this.RaiseEventOnEachTap)
			{
				this.RaiseOnTap();
			}
			return GestureRecognizer.GestureState.Recognized;
		}
		return GestureRecognizer.GestureState.Failed;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001246C File Offset: 0x0001066C
	protected void RaiseOnTap()
	{
		if (this.OnTap != null)
		{
			this.OnTap(this);
		}
	}

	// Token: 0x0400024B RID: 587
	public int RequiredTaps;

	// Token: 0x0400024C RID: 588
	public bool RaiseEventOnEachTap;

	// Token: 0x0400024D RID: 589
	public float MaxDelayBetweenTaps = 0.25f;

	// Token: 0x0400024E RID: 590
	public float MaxDuration;

	// Token: 0x0400024F RID: 591
	public float MoveTolerance = 5f;

	// Token: 0x04000250 RID: 592
	private int taps;

	// Token: 0x04000251 RID: 593
	private bool down;

	// Token: 0x04000252 RID: 594
	private bool wasDown;

	// Token: 0x04000253 RID: 595
	private float lastDownTime;

	// Token: 0x04000254 RID: 596
	private float lastTapTime;
}
