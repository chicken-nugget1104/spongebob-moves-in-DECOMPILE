using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
[AddComponentMenu("FingerGestures/Toolbox/Swipe")]
public class TBSwipe : TBComponent
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000145 RID: 325 RVA: 0x000072E8 File Offset: 0x000054E8
	// (remove) Token: 0x06000146 RID: 326 RVA: 0x00007304 File Offset: 0x00005504
	public event TBComponent.EventHandler<TBSwipe> OnSwipe;

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000147 RID: 327 RVA: 0x00007320 File Offset: 0x00005520
	// (set) Token: 0x06000148 RID: 328 RVA: 0x00007328 File Offset: 0x00005528
	public FingerGestures.SwipeDirection Direction
	{
		get
		{
			return this.direction;
		}
		protected set
		{
			this.direction = value;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000149 RID: 329 RVA: 0x00007334 File Offset: 0x00005534
	// (set) Token: 0x0600014A RID: 330 RVA: 0x0000733C File Offset: 0x0000553C
	public float Velocity
	{
		get
		{
			return this.velocity;
		}
		protected set
		{
			this.velocity = value;
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00007348 File Offset: 0x00005548
	public bool IsValid(FingerGestures.SwipeDirection direction)
	{
		if (direction == FingerGestures.SwipeDirection.Left)
		{
			return this.swipeLeft;
		}
		if (direction == FingerGestures.SwipeDirection.Right)
		{
			return this.swipeRight;
		}
		if (direction == FingerGestures.SwipeDirection.Up)
		{
			return this.swipeUp;
		}
		return direction == FingerGestures.SwipeDirection.Down && this.swipeDown;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00007384 File Offset: 0x00005584
	private TBComponent.Message GetMessageForSwipeDirection(FingerGestures.SwipeDirection direction)
	{
		if (direction == FingerGestures.SwipeDirection.Left)
		{
			return this.swipeLeftMessage;
		}
		if (direction == FingerGestures.SwipeDirection.Right)
		{
			return this.swipeRightMessage;
		}
		if (direction == FingerGestures.SwipeDirection.Up)
		{
			return this.swipeUpMessage;
		}
		return this.swipeDownMessage;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000073C4 File Offset: 0x000055C4
	public bool RaiseSwipe(int fingerIndex, Vector2 fingerPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		if (velocity < this.minVelocity)
		{
			return false;
		}
		if (!this.IsValid(direction))
		{
			return false;
		}
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		this.Direction = direction;
		this.Velocity = velocity;
		if (this.OnSwipe != null)
		{
			this.OnSwipe(this);
		}
		base.Send(this.swipeMessage);
		base.Send(this.GetMessageForSwipeDirection(direction));
		return true;
	}

	// Token: 0x040000BC RID: 188
	public bool swipeLeft = true;

	// Token: 0x040000BD RID: 189
	public bool swipeRight = true;

	// Token: 0x040000BE RID: 190
	public bool swipeUp = true;

	// Token: 0x040000BF RID: 191
	public bool swipeDown = true;

	// Token: 0x040000C0 RID: 192
	public float minVelocity;

	// Token: 0x040000C1 RID: 193
	public TBComponent.Message swipeMessage = new TBComponent.Message("OnSwipe");

	// Token: 0x040000C2 RID: 194
	public TBComponent.Message swipeLeftMessage = new TBComponent.Message("OnSwipeLeft", false);

	// Token: 0x040000C3 RID: 195
	public TBComponent.Message swipeRightMessage = new TBComponent.Message("OnSwipeRight", false);

	// Token: 0x040000C4 RID: 196
	public TBComponent.Message swipeUpMessage = new TBComponent.Message("OnSwipeUp", false);

	// Token: 0x040000C5 RID: 197
	public TBComponent.Message swipeDownMessage = new TBComponent.Message("OnSwipeDown", false);

	// Token: 0x040000C6 RID: 198
	private FingerGestures.SwipeDirection direction;

	// Token: 0x040000C7 RID: 199
	private float velocity;
}
