using System;
using UnityEngine;

// Token: 0x02000079 RID: 121
[AddComponentMenu("FingerGestures/Gesture Recognizers/Drag")]
public class DragGestureRecognizer : AveragedGestureRecognizer
{
	// Token: 0x1400002B RID: 43
	// (add) Token: 0x0600041D RID: 1053 RVA: 0x00011890 File Offset: 0x0000FA90
	// (remove) Token: 0x0600041E RID: 1054 RVA: 0x000118AC File Offset: 0x0000FAAC
	public event FGComponent.EventDelegate<DragGestureRecognizer> OnDragBegin;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x0600041F RID: 1055 RVA: 0x000118C8 File Offset: 0x0000FAC8
	// (remove) Token: 0x06000420 RID: 1056 RVA: 0x000118E4 File Offset: 0x0000FAE4
	public event FGComponent.EventDelegate<DragGestureRecognizer> OnDragMove;

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06000421 RID: 1057 RVA: 0x00011900 File Offset: 0x0000FB00
	// (remove) Token: 0x06000422 RID: 1058 RVA: 0x0001191C File Offset: 0x0000FB1C
	public event FGComponent.EventDelegate<DragGestureRecognizer> OnDragStationary;

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06000423 RID: 1059 RVA: 0x00011938 File Offset: 0x0000FB38
	// (remove) Token: 0x06000424 RID: 1060 RVA: 0x00011954 File Offset: 0x0000FB54
	public event FGComponent.EventDelegate<DragGestureRecognizer> OnDragEnd;

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000425 RID: 1061 RVA: 0x00011970 File Offset: 0x0000FB70
	// (set) Token: 0x06000426 RID: 1062 RVA: 0x00011978 File Offset: 0x0000FB78
	public Vector2 MoveDelta
	{
		get
		{
			return this.delta;
		}
		private set
		{
			this.delta = value;
		}
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00011984 File Offset: 0x0000FB84
	protected override bool CanBegin(FingerGestures.IFingerList touches)
	{
		return base.CanBegin(touches) && touches.GetAverageDistanceFromStart() >= this.MoveTolerance;
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x000119B4 File Offset: 0x0000FBB4
	protected override void OnBegin(FingerGestures.IFingerList touches)
	{
		base.Position = touches.GetAveragePosition();
		base.StartPosition = base.Position;
		this.MoveDelta = Vector2.zero;
		this.lastPos = base.Position;
		this.RaiseOnDragBegin();
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x000119F8 File Offset: 0x0000FBF8
	protected override GestureRecognizer.GestureState OnActive(FingerGestures.IFingerList touches)
	{
		if (touches.Count == this.RequiredFingerCount)
		{
			base.Position = touches.GetAveragePosition();
			this.MoveDelta = base.Position - this.lastPos;
			if (this.MoveDelta.sqrMagnitude > 0f)
			{
				this.RaiseOnDragMove();
				this.lastPos = base.Position;
			}
			else
			{
				this.RaiseOnDragStationary();
			}
			return GestureRecognizer.GestureState.InProgress;
		}
		if (touches.Count < this.RequiredFingerCount)
		{
			this.RaiseOnDragEnd();
			return GestureRecognizer.GestureState.Recognized;
		}
		return GestureRecognizer.GestureState.Failed;
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00011A8C File Offset: 0x0000FC8C
	protected void RaiseOnDragBegin()
	{
		if (this.OnDragBegin != null)
		{
			this.OnDragBegin(this);
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00011AA8 File Offset: 0x0000FCA8
	protected void RaiseOnDragMove()
	{
		if (this.OnDragMove != null)
		{
			this.OnDragMove(this);
		}
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x00011AC4 File Offset: 0x0000FCC4
	protected void RaiseOnDragStationary()
	{
		if (this.OnDragStationary != null)
		{
			this.OnDragStationary(this);
		}
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00011AE0 File Offset: 0x0000FCE0
	protected void RaiseOnDragEnd()
	{
		if (this.OnDragEnd != null)
		{
			this.OnDragEnd(this);
		}
	}

	// Token: 0x0400022C RID: 556
	public float MoveTolerance = 5f;

	// Token: 0x0400022D RID: 557
	private Vector2 delta = Vector2.zero;

	// Token: 0x0400022E RID: 558
	private Vector2 lastPos = Vector2.zero;
}
