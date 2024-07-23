using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
[AddComponentMenu("FingerGestures/Toolbox/Drag")]
public class TBDrag : TBComponent
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000117 RID: 279 RVA: 0x000067AC File Offset: 0x000049AC
	// (remove) Token: 0x06000118 RID: 280 RVA: 0x000067C8 File Offset: 0x000049C8
	public event TBComponent.EventHandler<TBDrag> OnDragBegin;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000119 RID: 281 RVA: 0x000067E4 File Offset: 0x000049E4
	// (remove) Token: 0x0600011A RID: 282 RVA: 0x00006800 File Offset: 0x00004A00
	public event TBComponent.EventHandler<TBDrag> OnDragMove;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600011B RID: 283 RVA: 0x0000681C File Offset: 0x00004A1C
	// (remove) Token: 0x0600011C RID: 284 RVA: 0x00006838 File Offset: 0x00004A38
	public event TBComponent.EventHandler<TBDrag> OnDragEnd;

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x0600011D RID: 285 RVA: 0x00006854 File Offset: 0x00004A54
	// (set) Token: 0x0600011E RID: 286 RVA: 0x0000685C File Offset: 0x00004A5C
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
		private set
		{
			if (this.dragging != value)
			{
				this.dragging = value;
				if (this.dragging)
				{
					FingerGestures.OnFingerDragMove += this.FingerGestures_OnDragMove;
					FingerGestures.OnFingerDragEnd += this.FingerGestures_OnDragEnd;
				}
				else
				{
					FingerGestures.OnFingerDragMove -= this.FingerGestures_OnDragMove;
					FingerGestures.OnFingerDragEnd -= this.FingerGestures_OnDragEnd;
				}
			}
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x0600011F RID: 287 RVA: 0x000068D0 File Offset: 0x00004AD0
	// (set) Token: 0x06000120 RID: 288 RVA: 0x000068D8 File Offset: 0x00004AD8
	public Vector2 MoveDelta
	{
		get
		{
			return this.moveDelta;
		}
		private set
		{
			this.moveDelta = value;
		}
	}

	// Token: 0x06000121 RID: 289 RVA: 0x000068E4 File Offset: 0x00004AE4
	public bool BeginDrag(int fingerIndex, Vector2 fingerPos)
	{
		if (this.Dragging)
		{
			return false;
		}
		base.FingerIndex = fingerIndex;
		base.FingerPos = fingerPos;
		this.Dragging = true;
		if (this.OnDragBegin != null)
		{
			this.OnDragBegin(this);
		}
		base.Send(this.dragBeginMessage);
		return true;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00006938 File Offset: 0x00004B38
	public bool EndDrag()
	{
		if (!this.Dragging)
		{
			return false;
		}
		if (this.OnDragEnd != null)
		{
			this.OnDragEnd(this);
		}
		base.Send(this.dragEndMessage);
		this.Dragging = false;
		base.FingerIndex = -1;
		return true;
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00006988 File Offset: 0x00004B88
	private void FingerGestures_OnDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
		if (this.Dragging && base.FingerIndex == fingerIndex)
		{
			base.FingerPos = fingerPos;
			this.MoveDelta = delta;
			if (this.OnDragMove != null)
			{
				this.OnDragMove(this);
			}
			base.Send(this.dragMoveMessage);
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000069E0 File Offset: 0x00004BE0
	private void FingerGestures_OnDragEnd(int fingerIndex, Vector2 fingerPos)
	{
		if (this.Dragging && base.FingerIndex == fingerIndex)
		{
			base.FingerPos = fingerPos;
			this.EndDrag();
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00006A14 File Offset: 0x00004C14
	private void OnDisable()
	{
		if (this.Dragging)
		{
			this.EndDrag();
		}
	}

	// Token: 0x0400009C RID: 156
	public TBComponent.Message dragBeginMessage = new TBComponent.Message("OnDragBegin");

	// Token: 0x0400009D RID: 157
	public TBComponent.Message dragMoveMessage = new TBComponent.Message("OnDragMove", false);

	// Token: 0x0400009E RID: 158
	public TBComponent.Message dragEndMessage = new TBComponent.Message("OnDragEnd");

	// Token: 0x0400009F RID: 159
	private bool dragging;

	// Token: 0x040000A0 RID: 160
	private Vector2 moveDelta;
}
