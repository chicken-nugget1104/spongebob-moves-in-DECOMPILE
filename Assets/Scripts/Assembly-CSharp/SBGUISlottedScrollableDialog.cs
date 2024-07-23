using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public abstract class SBGUISlottedScrollableDialog : SBGUIScrollableDialog
{
	// Token: 0x0600060F RID: 1551 RVA: 0x00026734 File Offset: 0x00024934
	public override void Start()
	{
		this.region.ScrollEvent.AddListener(new Action(this.UpdateSlotVisibility));
		base.Start();
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x00026764 File Offset: 0x00024964
	public virtual void SetManagers(Session session)
	{
		this.session = session;
		base.SetManagers(session.TheGame.entities, session.TheGame.resourceManager, session.TheSoundEffectManager, session.TheGame.costumeManager);
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x000267A8 File Offset: 0x000249A8
	public SBGUIElement FindDynamicSubElementInScrollRegionSessionActionId(string sessionActionId, bool assertOnNullScrollRegionSubComponents = true)
	{
		TFUtils.Assert(this.region != null, "No scroll region set for this screen");
		SBGUIElement componentInChildren = this.region.GetComponent<ScrollRegion>().subView.GetComponentInChildren<SBGUIElement>();
		TFUtils.Assert(!assertOnNullScrollRegionSubComponents || componentInChildren != null, "It appears that this dialog's scroll region has no ScrollRegion component! This might be a race condition that occurs when trying to Find a Dynamic Sub Element while this screen is still initializing.");
		if (componentInChildren == null)
		{
			return null;
		}
		return componentInChildren.FindChild(sessionActionId);
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x00026810 File Offset: 0x00024A10
	public virtual void FindDynamicSubElementInScrollRegionSessionActionIdAsync(string sessionActionId, Action<SBGUIElement> foundCallback)
	{
		TFUtils.Assert(foundCallback != null, "FindDynamicSubElementInScrollRegionSessionActionIdAsync requires callbacks to not be null.");
		Action<SBGUIElement> onSlotReadyEvent = null;
		onSlotReadyEvent = delegate(SBGUIElement slot)
		{
			if (slot.name.Equals(sessionActionId))
			{
				foundCallback(slot);
				this.SlotReadyEvent.RemoveListener(onSlotReadyEvent);
			}
		};
		SBGUIElement sbguielement = null;
		if (this.region != null && this.region.isReady)
		{
			sbguielement = this.FindDynamicSubElementInScrollRegionSessionActionId(sessionActionId, true);
		}
		if (sbguielement != null)
		{
			this.SlotReadyEvent.AddListener(onSlotReadyEvent);
			onSlotReadyEvent(sbguielement);
		}
		else
		{
			this.SlotReadyEvent.AddListener(onSlotReadyEvent);
			if (!this.sessionActionIdSearchRequests.Contains(sessionActionId))
			{
				this.sessionActionIdSearchRequests.Add(sessionActionId);
			}
			if (this.sessionActionSlotMap.ContainsKey(sessionActionId))
			{
				int num = this.sessionActionSlotMap[sessionActionId];
				if (this.region != null && this.region.SetupSlotActions.Count > num)
				{
					this.CreateSlot(num);
				}
			}
		}
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x00026958 File Offset: 0x00024B58
	protected void PreLoadRegionContentInfo()
	{
		SBGUIActivityIndicator sbguiactivityIndicator = (SBGUIActivityIndicator)base.FindChildSessionActionId("ActivityIndicator", false);
		if (sbguiactivityIndicator != null)
		{
			sbguiactivityIndicator.StartActivityIndicator();
		}
		this.mustWaitForInfoToLoad = true;
		base.StopAllCoroutines();
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x00026998 File Offset: 0x00024B98
	protected void PostLoadRegionContentInfo(int slotCount)
	{
		this.PostLoadRegionContentInfo(slotCount, Vector3.zero);
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x000269A8 File Offset: 0x00024BA8
	protected void PostLoadRegionContentInfo(int slotCount, Vector3 scrollPos)
	{
		SBGUIActivityIndicator sbguiactivityIndicator = (SBGUIActivityIndicator)base.FindChildSessionActionId("ActivityIndicator", false);
		if (sbguiactivityIndicator != null)
		{
			sbguiactivityIndicator.StopActivityIndicator();
		}
		this.mustWaitForInfoToLoad = false;
		this.ResetScrolling(slotCount, scrollPos);
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x000269E8 File Offset: 0x00024BE8
	private void ResetScrolling(int slotCount, Vector3 scrollPos)
	{
		Rect regionSize = this.CalculateScrollRegionSize(slotCount);
		base.View.RefreshEvent += delegate()
		{
			this.region.ResetScroll(regionSize);
			if (scrollPos != Vector3.zero)
			{
				if (scrollPos.sqrMagnitude > this.region.MaxScroll.sqrMagnitude)
				{
					this.region.SetScroll(this.region.MaxScroll);
				}
				else
				{
					this.region.SetScroll(scrollPos);
				}
			}
			else
			{
				this.region.SetScroll(this.region.MinScroll);
			}
		};
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00026A30 File Offset: 0x00024C30
	public override void Deactivate()
	{
		this.region.ClearSlotActions();
		this.SlotReadyEvent.ClearListeners();
		base.StopAllCoroutines();
		this.ClearCachedSlotInfos();
		base.Deactivate();
	}

	// Token: 0x06000618 RID: 1560
	protected abstract SBGUIScrollListElement MakeSlot();

	// Token: 0x06000619 RID: 1561 RVA: 0x00026A68 File Offset: 0x00024C68
	private void CreateSlot(int i)
	{
		if (i > this.region.SetupSlotActions.Count)
		{
			TFUtils.Assert(false, "how are we trying to generate an out of bounds slot?");
			return;
		}
		SBGUIScrollListElement sbguiscrollListElement = null;
		this.slotRefs.TryGetValue(i, out sbguiscrollListElement);
		if (sbguiscrollListElement == null)
		{
			sbguiscrollListElement = this.slotPool.Create(new Alloc<SBGUIScrollListElement>(this.MakeSlot));
			this.region.SetupSlotActions[i](sbguiscrollListElement);
			sbguiscrollListElement.transform.localPosition = this.GetSlotOffset(i);
			this.SlotReadyEvent.FireEvent(sbguiscrollListElement);
			this.slotRefs[i] = sbguiscrollListElement;
		}
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x00026B14 File Offset: 0x00024D14
	protected void ClearCachedSlotInfos()
	{
		this.slotPool.Clear(new Deactivate<SBGUIScrollListElement>(SBGUISlottedScrollableDialog.DeactivateSlot));
		this.slotRefs.Clear();
		this.sessionActionSlotMap.Clear();
		this.sessionActionIdSearchRequests.Clear();
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x00026B5C File Offset: 0x00024D5C
	private void UpdateSlotVisibility()
	{
		if (!this.mustWaitForInfoToLoad)
		{
			base.StopAllCoroutines();
		}
		base.StartCoroutine(this.ShowSlotsAsNeeded(true));
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x00026B80 File Offset: 0x00024D80
	public List<SBGUIScrollListElement> GetVisibleSrollListElements()
	{
		if (this.region.subViewMarker == null)
		{
			return null;
		}
		int count = this.region.SetupSlotActions.Count;
		float num = this.region.InitialMarkerPos.x - this.region.subViewMarker.tform.localPosition.x;
		float y = 0f;
		float x = num + this.region.GetComponent<ScrollRegion>().size.x * 0.01f + this.GetSlotSize().x;
		float y2 = this.region.GetComponent<ScrollRegion>().size.y * 0.01f;
		int num2 = Mathf.Max(0, this.GetSlotIndex(new Vector2(num, y)));
		int num3 = Mathf.Min(count - 1, this.GetSlotIndex(new Vector2(x, y2)));
		SBGUIScrollListElement sbguiscrollListElement = null;
		List<SBGUIScrollListElement> list = new List<SBGUIScrollListElement>();
		for (int i = 0; i < count; i++)
		{
			if ((i >= num2 && i < num3) || (i == num3 && i == count - 1))
			{
				this.slotRefs.TryGetValue(i, out sbguiscrollListElement);
				if (sbguiscrollListElement != null)
				{
					list.Add(sbguiscrollListElement);
				}
			}
			else if (i > num3)
			{
				break;
			}
		}
		return list;
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00026CE8 File Offset: 0x00024EE8
	protected IEnumerator ShowSlotsAsNeeded(bool deferProcessing)
	{
		if (this.region == null)
		{
			TFUtils.ErrorLog("SBGUISlottedScrollableDialog.ShowSlotsAsNeeded - region is null");
			yield return null;
		}
		int totalSlots = this.region.SetupSlotActions.Count;
		float startVisibleX = this.region.InitialMarkerPos.x - this.region.subViewMarker.tform.localPosition.x;
		float startVisibleY = 0f;
		float endVisibleX = startVisibleX + this.region.GetComponent<ScrollRegion>().size.x * 0.01f + this.GetSlotSize().x;
		float endVisibleY = this.region.GetComponent<ScrollRegion>().size.y * 0.01f;
		int startVisibleI = Mathf.Max(0, this.GetSlotIndex(new Vector2(startVisibleX, startVisibleY)));
		int endVisibleI = Mathf.Min(totalSlots - 1, this.GetSlotIndex(new Vector2(endVisibleX, endVisibleY)));
		for (int i = startVisibleI; i <= endVisibleI; i++)
		{
			this.CreateSlot(i);
		}
		int offscreenSlot = this.CheckOffscreenSelectedSlot(startVisibleI, endVisibleI);
		if (offscreenSlot != -1)
		{
			this.CreateSlot(offscreenSlot);
		}
		this.OnSlotsVisible();
		for (int j = 0; j < totalSlots; j++)
		{
			if (j < startVisibleI || j > endVisibleI)
			{
				SBGUIScrollListElement slot = null;
				this.slotRefs.TryGetValue(j, out slot);
				if (slot != null)
				{
					if (!SBGUI.GetInstance().CheckWhitelisted(slot))
					{
						SBGUISlottedScrollableDialog.DeactivateSlot(slot);
						this.slotPool.Release(slot);
						this.slotRefs[j] = null;
					}
					else
					{
						this.CreateSlot(j);
					}
				}
			}
			if (deferProcessing)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00026D14 File Offset: 0x00024F14
	protected virtual int CheckOffscreenSelectedSlot(int visibleStart, int visibleEnd)
	{
		return -1;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00026D18 File Offset: 0x00024F18
	protected virtual void OnSlotsVisible()
	{
		this.session.TheGame.sessionActionManager.RequestProcess(this.session.TheGame);
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x00026D48 File Offset: 0x00024F48
	protected static void DeactivateSlot(SBGUIScrollListElement s)
	{
		if (s != null)
		{
			s.name = "inactive";
			s.SessionActionId = null;
			s.Deactivate();
		}
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00026D7C File Offset: 0x00024F7C
	public override void ShowScrollRegion(bool visible)
	{
		if (!visible)
		{
			this.region.SetupSlotActions.Clear();
			this.SlotReadyEvent.ClearListeners();
			base.StopAllCoroutines();
			this.ClearCachedSlotInfos();
		}
		base.ShowScrollRegion(visible);
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00026DC0 File Offset: 0x00024FC0
	public override void OnDestroy()
	{
		this.slotPool.Clear(new Deactivate<SBGUIScrollListElement>(SBGUISlottedScrollableDialog.DeactivateSlot));
		this.slotPool.Purge(new Deactivate<SBGUIScrollListElement>(this.DestroySlot));
		base.OnDestroy();
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00026E04 File Offset: 0x00025004
	public void DestroySlot(SBGUIScrollListElement elem)
	{
		if (elem != null)
		{
			YGAtlasSprite[] componentsInChildren = elem.gameObject.GetComponentsInChildren<YGAtlasSprite>();
			foreach (YGAtlasSprite ygatlasSprite in componentsInChildren)
			{
				if (!string.IsNullOrEmpty(ygatlasSprite.nonAtlasName))
				{
					base.View.Library.incrementTextureDuplicates(ygatlasSprite.nonAtlasName);
				}
			}
			UnityGameResources.Destroy(elem.gameObject);
		}
	}

	// Token: 0x06000624 RID: 1572
	protected abstract Vector2 GetSlotSize();

	// Token: 0x06000625 RID: 1573
	protected abstract int GetSlotIndex(Vector2 pos);

	// Token: 0x06000626 RID: 1574
	protected abstract Vector2 GetSlotOffset(int index);

	// Token: 0x06000627 RID: 1575 RVA: 0x00026E74 File Offset: 0x00025074
	protected Rect CalculateScrollRegionSize(int slotCount)
	{
		Vector2 slotOffset = this.GetSlotOffset(0);
		Bounds value = new Bounds(slotOffset, Vector2.zero);
		for (int i = 0; i < slotCount; i++)
		{
			Vector2 slotOffset2 = this.GetSlotOffset(i);
			value.Encapsulate(slotOffset2);
			value.Encapsulate(slotOffset2 + this.GetSlotSize());
		}
		return value.ToRect();
	}

	// Token: 0x040004A0 RID: 1184
	public int ScrollSubElementCount;

	// Token: 0x040004A1 RID: 1185
	public EventDispatcher<SBGUIElement> SlotReadyEvent = new EventDispatcher<SBGUIElement>();

	// Token: 0x040004A2 RID: 1186
	protected bool mustWaitForInfoToLoad = true;

	// Token: 0x040004A3 RID: 1187
	private TFPool<SBGUIScrollListElement> slotPool = new TFPool<SBGUIScrollListElement>();

	// Token: 0x040004A4 RID: 1188
	protected Dictionary<int, SBGUIScrollListElement> slotRefs = new Dictionary<int, SBGUIScrollListElement>();

	// Token: 0x040004A5 RID: 1189
	protected HashSet<string> sessionActionIdSearchRequests = new HashSet<string>();

	// Token: 0x040004A6 RID: 1190
	protected Dictionary<string, int> sessionActionSlotMap = new Dictionary<string, int>();
}
