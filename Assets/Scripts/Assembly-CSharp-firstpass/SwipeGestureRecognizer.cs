using System;
using UnityEngine;

// Token: 0x02000081 RID: 129
[AddComponentMenu("FingerGestures/Gesture Recognizers/Swipe")]
public class SwipeGestureRecognizer : AveragedGestureRecognizer
{
	// Token: 0x1400003D RID: 61
	// (add) Token: 0x06000486 RID: 1158 RVA: 0x00012BD0 File Offset: 0x00010DD0
	// (remove) Token: 0x06000487 RID: 1159 RVA: 0x00012BEC File Offset: 0x00010DEC
	public event FGComponent.EventDelegate<SwipeGestureRecognizer> OnSwipe;

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06000488 RID: 1160 RVA: 0x00012C08 File Offset: 0x00010E08
	// (set) Token: 0x06000489 RID: 1161 RVA: 0x00012C10 File Offset: 0x00010E10
	public Vector2 Move
	{
		get
		{
			return this.move;
		}
		private set
		{
			this.move = value;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600048A RID: 1162 RVA: 0x00012C1C File Offset: 0x00010E1C
	public FingerGestures.SwipeDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x00012C24 File Offset: 0x00010E24
	public float Velocity
	{
		get
		{
			return this.velocity;
		}
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x00012C2C File Offset: 0x00010E2C
	public bool IsValidDirection(FingerGestures.SwipeDirection dir)
	{
		return dir != FingerGestures.SwipeDirection.None && (this.ValidDirections & dir) == dir;
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x00012C44 File Offset: 0x00010E44
	protected override bool CanBegin(FingerGestures.IFingerList touches)
	{
		return base.CanBegin(touches) && touches.GetAverageDistanceFromStart() >= 0.5f;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00012C68 File Offset: 0x00010E68
	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
		this.direction = FingerGestures.SwipeDirection.None;
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00012C94 File Offset: 0x00010E94
	protected override GestureRecognizer.GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count != this.RequiredFingerCount)
		{
			if (touches.Count < this.RequiredFingerCount && this.direction != FingerGestures.SwipeDirection.None)
			{
				if (this.OnSwipe != null)
				{
					this.OnSwipe(this);
				}
				return GestureRecognizer.GestureState.Recognized;
			}
			return GestureRecognizer.GestureState.Failed;
		}
		else
		{
			base.Position = touches.GetAveragePosition();
			this.Move = base.Position - base.StartPosition;
			float magnitude = this.Move.magnitude;
			if (magnitude < this.MinDistance)
			{
				return GestureRecognizer.GestureState.InProgress;
			}
			if (base.ElapsedTime > 0f)
			{
				this.velocity = magnitude / base.ElapsedTime;
			}
			else
			{
				this.velocity = 0f;
			}
			if (this.velocity < this.MinVelocity)
			{
				return GestureRecognizer.GestureState.Failed;
			}
			FingerGestures.SwipeDirection swipeDirection = FingerGestures.GetSwipeDirection(this.Move.normalized, this.DirectionTolerance);
			if (!this.IsValidDirection(swipeDirection) || (this.direction != FingerGestures.SwipeDirection.None && swipeDirection != this.direction))
			{
				return GestureRecognizer.GestureState.Failed;
			}
			this.direction = swipeDirection;
			return GestureRecognizer.GestureState.InProgress;
		}
	}

	// Token: 0x04000264 RID: 612
	public FingerGestures.SwipeDirection ValidDirections = FingerGestures.SwipeDirection.All;

	// Token: 0x04000265 RID: 613
	public float MinDistance = 1f;

	// Token: 0x04000266 RID: 614
	public float MinVelocity = 1f;

	// Token: 0x04000267 RID: 615
	public float DirectionTolerance = 0.2f;

	// Token: 0x04000268 RID: 616
	private Vector2 move;

	// Token: 0x04000269 RID: 617
	private FingerGestures.SwipeDirection direction;

	// Token: 0x0400026A RID: 618
	private float velocity;
}
