using System;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

// Token: 0x020000A1 RID: 161
[RequireComponent(typeof(ScrollRegion))]
public class SBGUIScrollRegion : SBGUIElement
{
	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060005E2 RID: 1506 RVA: 0x00025690 File Offset: 0x00023890
	public SBGUIElement Marker
	{
		get
		{
			return this.subViewMarker;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00025698 File Offset: 0x00023898
	public Vector3 MinScroll
	{
		get
		{
			return this.minScroll;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060005E4 RID: 1508 RVA: 0x000256A0 File Offset: 0x000238A0
	public Vector3 MaxScroll
	{
		get
		{
			return this.maxScroll;
		}
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x000256A8 File Offset: 0x000238A8
	public override void SetVisible(bool viz)
	{
		this.region.Visible = viz;
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060005E6 RID: 1510 RVA: 0x000256B8 File Offset: 0x000238B8
	public Vector3 InitialMarkerPos
	{
		get
		{
			return this.initialMarkerPos;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060005E7 RID: 1511 RVA: 0x000256C0 File Offset: 0x000238C0
	public List<Action<SBGUIScrollListElement>> SetupSlotActions
	{
		get
		{
			return this.createSlotActions;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060005E8 RID: 1512 RVA: 0x000256C8 File Offset: 0x000238C8
	public bool WasRecentlyTouched
	{
		get
		{
			return this.touchedSemaphore > 0;
		}
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x000256D4 File Offset: 0x000238D4
	protected override void Awake()
	{
		this.region = base.GetComponent<ScrollRegion>();
		this.region.ScrollEvent.AddListener(new Func<YGEvent, bool>(this.ScrollHandler));
		if (this.scrollBar != null)
		{
			this.scrollBar.scrollDirection = this.scrollDirection;
		}
		this.momentum = new Momentum(8);
		this.moving = false;
		base.Awake();
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x00025744 File Offset: 0x00023944
	public void Update()
	{
		if (this.subViewMarker != null && this.momentum != null)
		{
			this.momentum.TrackForSmoothing(this.subViewMarker.tform.position);
		}
		if (this.movedSemaphore <= 0 && this.moving && this.momentum != null)
		{
			this.momentum.ApplyFriction(0.85f);
			Vector2 v = new Vector2(this.momentum.Velocity.x, -this.momentum.Velocity.y);
			float num = v.SqrMagnitude();
			if ((double)num <= 0.0001)
			{
				Vector2? vector = this.scrollScreenStart;
				if (vector == null)
				{
					this.moving = false;
					this.ScrollStopEvent.FireEvent();
				}
			}
			else
			{
				this.DeltaScroll(v);
			}
		}
		this.movedSemaphore--;
		if (this.touchedSemaphore <= 0)
		{
			this.scrollScreenStart = null;
		}
		this.touchedSemaphore--;
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x00025874 File Offset: 0x00023A74
	private bool ScrollHandler(YGEvent evt)
	{
		this.touchedSemaphore = 10;
		YGEvent.TYPE type = evt.type;
		switch (type)
		{
		case YGEvent.TYPE.TOUCH_BEGIN:
			SoundEffectManager.CreateSoundEffectManager().PlaySound("scrolling");
			this.scrollScreenStart = new Vector2?(evt.position);
			this.lastDelta = Vector2.zero;
			this.momentum.ClearTrackPositions();
			break;
		case YGEvent.TYPE.TOUCH_END:
		case YGEvent.TYPE.TOUCH_CANCEL:
			if (this.scrollScreenStart != null)
			{
				Vector2 a = (evt.position - this.scrollScreenStart.Value) * 0.01f;
				a = new Vector2(a.x, -a.y);
				this.DeltaScroll(a - this.lastDelta);
				this.momentum.CalculateSmoothVelocity();
			}
			this.scrollScreenStart = null;
			this.lastDelta = Vector2.zero;
			this.movedSemaphore = 1;
			this.touchedSemaphore = 0;
			break;
		default:
			if (type == YGEvent.TYPE.RESET)
			{
				this.scrollScreenStart = null;
				this.lastDelta = Vector2.zero;
			}
			break;
		case YGEvent.TYPE.TOUCH_MOVE:
		{
			if (this.scrollScreenStart == null)
			{
				this.scrollScreenStart = new Vector2?(evt.position);
				this.lastDelta = Vector2.zero;
			}
			Vector2 a2 = (evt.position - this.scrollScreenStart.Value) * 0.01f;
			a2 = new Vector2(a2.x, -a2.y);
			this.DeltaScroll(a2 - this.lastDelta);
			this.momentum.CalculateSmoothVelocity();
			this.lastDelta = a2;
			this.movedSemaphore = 1;
			this.moving = true;
			break;
		}
		}
		return false;
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x00025A50 File Offset: 0x00023C50
	public void ResetToMinScroll()
	{
		this.moving = false;
		this.SetScroll(this.minScroll);
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x00025A68 File Offset: 0x00023C68
	public void ResetScroll()
	{
		if (this.createSlotActions.Count > 0)
		{
			this.ResetScroll(this.boundingRect);
		}
		else
		{
			Bounds totalBounds = this.GetTotalBounds();
			this.ResetScroll(totalBounds.ToRect());
		}
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x00025AAC File Offset: 0x00023CAC
	public void ResetScroll(Rect scrollSize)
	{
		this.moving = false;
		float thumbSize = 1f;
		this.minScroll = (this.maxScroll = this.InitialMarkerPos);
		if (this.scrollBar != null && scrollSize.width == 0f && scrollSize.height == 0f)
		{
			this.scrollBar.SetThumbSize(thumbSize);
			this.scrollBar.Reset();
			return;
		}
		Rect worldRect = this.region.GetWorldRect();
		this.contentRect = scrollSize;
		if (this.scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.HORIZONTAL)
		{
			if (this.contentRect.width > worldRect.width)
			{
				this.maxScroll.x = this.maxScroll.x - (this.contentRect.width - worldRect.width);
				this.maxScroll.y = this.minScroll.y;
				this.maxScroll.z = this.minScroll.z;
				thumbSize = Mathf.Clamp01(worldRect.width / this.contentRect.width);
			}
		}
		else if (this.scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.VERTICAL && this.contentRect.height > worldRect.height)
		{
			this.maxScroll.y = this.maxScroll.y + (this.contentRect.height - worldRect.height);
			this.maxScroll.x = this.minScroll.x;
			this.maxScroll.z = this.minScroll.z;
			thumbSize = Mathf.Clamp01(worldRect.height / this.contentRect.height);
		}
		if (this.scrollBar != null)
		{
			this.scrollBar.SetThumbSize(thumbSize);
			this.scrollBar.Reset();
		}
		base.View.RefreshEvent += base.ReregisterColliders;
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x00025C98 File Offset: 0x00023E98
	public Bounds GetTotalBounds()
	{
		return this.region.GetTotalBounds();
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x00025CA8 File Offset: 0x00023EA8
	public Rect GetWorldRect()
	{
		return this.region.GetWorldRect();
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x00025CB8 File Offset: 0x00023EB8
	private Vector3 ClampPosition(Vector3 pos)
	{
		pos.x = Mathf.Clamp(pos.x, Mathf.Min(this.minScroll.x, this.maxScroll.x), Mathf.Max(this.minScroll.x, this.maxScroll.x));
		pos.y = Mathf.Clamp(pos.y, Mathf.Min(this.minScroll.y, this.maxScroll.y), Mathf.Max(this.minScroll.y, this.maxScroll.y));
		return pos;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x00025D58 File Offset: 0x00023F58
	public bool DeltaScroll(Vector3 delta)
	{
		if (delta.sqrMagnitude <= 1E-06f)
		{
			return false;
		}
		if (this.subViewMarker == null)
		{
			return false;
		}
		Vector3 position = this.subViewMarker.tform.position;
		Vector3 vector = position;
		float value = 0f;
		if (this.scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.VERTICAL)
		{
			delta.x = 0f;
			vector = this.ClampPosition(vector - delta);
			value = (vector.y - this.initialMarkerPos.y) / this.contentRect.height;
		}
		else if (this.scrollDirection == SBGUIScrollRegion.SCROLL_DIRECTION.HORIZONTAL)
		{
			delta.y = 0f;
			delta.x *= -1f;
			vector = this.ClampPosition(vector - delta);
			value = (this.initialMarkerPos.x - vector.x) / this.contentRect.width;
		}
		if (vector == position)
		{
			return false;
		}
		if (this.scrollBar != null)
		{
			this.scrollBar.UpdateScroll(Mathf.Clamp01(value));
		}
		this.SetScroll(vector);
		return true;
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x00025E84 File Offset: 0x00024084
	public void SetScroll(Vector3 pos)
	{
		this.subViewMarker.tform.position = pos;
		YG2DBody[] componentsInChildren = this.subViewMarker.gameObject.GetComponentsInChildren<YG2DBody>();
		foreach (YG2DBody yg2DBody in componentsInChildren)
		{
			yg2DBody.MatchTransform3D();
		}
		this.ScrollEvent.FireEvent();
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x00025EE0 File Offset: 0x000240E0
	public void MatchAndRegister()
	{
		this.region.MatchSubView();
		this.region.ReadyEvent.AddListener(new Action(this.Register));
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00025F0C File Offset: 0x0002410C
	private void Register()
	{
		Vector3 pos = base.View.WorldToScreen(base.tform.position);
		this.initialMarkerPos = this.region.ScreenToWorld(pos);
		this.initialMarkerPos.z = this.initialMarkerPos.z + (base.tform.position.z - base.View.transform.position.z);
		this.subViewMarker = SBGUIElement.Create();
		this.subViewMarker.name = string.Format("{0}_marker_{1}", base.gameObject.name, (uint)base.GetInstanceID());
		this.subViewMarker.tform.parent = this.region.SubViewTform;
		if (this.anchorPosition == SBGUIScrollRegion.ANCHOR_POSITION.TOP_LEFT)
		{
			this.initialMarkerPos.y = this.initialMarkerPos.y + this.region.size.y * 0.01f;
		}
		this.subViewMarker.tform.position = this.initialMarkerPos;
		this.subViewMarker.MuteButtons(this.muted);
		this.ReadyEvent.FireEvent();
		this.isReady = true;
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00026040 File Offset: 0x00024240
	public void ClearSlotActions()
	{
		this.createSlotActions.Clear();
	}

	// Token: 0x04000477 RID: 1143
	private const int RESET_TOUCH_SEMAPHORE = 10;

	// Token: 0x04000478 RID: 1144
	public ReadyEventDispatcher ReadyEvent = new ReadyEventDispatcher();

	// Token: 0x04000479 RID: 1145
	public EventDispatcher ScrollEvent = new EventDispatcher();

	// Token: 0x0400047A RID: 1146
	public EventDispatcher ScrollStopEvent = new EventDispatcher();

	// Token: 0x0400047B RID: 1147
	public bool isReady;

	// Token: 0x0400047C RID: 1148
	public SBGUIScrollRegion.ANCHOR_POSITION anchorPosition = SBGUIScrollRegion.ANCHOR_POSITION.BOTTOM_LEFT;

	// Token: 0x0400047D RID: 1149
	public SBGUIScrollRegion.SCROLL_DIRECTION scrollDirection = SBGUIScrollRegion.SCROLL_DIRECTION.HORIZONTAL;

	// Token: 0x0400047E RID: 1150
	public SBGUIScrollBar scrollBar;

	// Token: 0x0400047F RID: 1151
	public Rect boundingRect;

	// Token: 0x04000480 RID: 1152
	public Momentum momentum;

	// Token: 0x04000481 RID: 1153
	private bool moving;

	// Token: 0x04000482 RID: 1154
	private int movedSemaphore;

	// Token: 0x04000483 RID: 1155
	private int touchedSemaphore = 10;

	// Token: 0x04000484 RID: 1156
	private ScrollRegion region;

	// Token: 0x04000485 RID: 1157
	public SBGUIElement subViewMarker;

	// Token: 0x04000486 RID: 1158
	private Rect contentRect;

	// Token: 0x04000487 RID: 1159
	private Vector3 minScroll;

	// Token: 0x04000488 RID: 1160
	private Vector3 maxScroll;

	// Token: 0x04000489 RID: 1161
	private Vector2? scrollScreenStart;

	// Token: 0x0400048A RID: 1162
	private Vector2 lastDelta;

	// Token: 0x0400048B RID: 1163
	private List<Action<SBGUIScrollListElement>> createSlotActions = new List<Action<SBGUIScrollListElement>>();

	// Token: 0x0400048C RID: 1164
	private Vector3 initialMarkerPos;

	// Token: 0x0400048D RID: 1165
	private Vector3 currentMarkerPos;

	// Token: 0x020000A2 RID: 162
	public enum ANCHOR_POSITION
	{
		// Token: 0x0400048F RID: 1167
		TOP_LEFT,
		// Token: 0x04000490 RID: 1168
		BOTTOM_LEFT
	}

	// Token: 0x020000A3 RID: 163
	public enum SCROLL_DIRECTION
	{
		// Token: 0x04000492 RID: 1170
		VERTICAL,
		// Token: 0x04000493 RID: 1171
		HORIZONTAL
	}
}
