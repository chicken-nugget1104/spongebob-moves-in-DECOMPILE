using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B3 RID: 179
public abstract class SBGUITabbedScrollableDialog : SBGUISlottedScrollableDialog
{
	// Token: 0x060006C2 RID: 1730 RVA: 0x0002A7A4 File Offset: 0x000289A4
	public override void SetManagers(Session session)
	{
		base.SetManagers(session);
		this.LoadCategories(session);
		this.SetupTabCategories();
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x0002A7BC File Offset: 0x000289BC
	public void SetupTabCategories()
	{
		SBGUITabBar sbguitabBar = (SBGUITabBar)base.FindChild("tabbar");
		sbguitabBar.TabChangeEvent.AddListener(new Action<SBGUITabButton>(this.BuildTabForButton));
		sbguitabBar.SetupCategories(this.categories, this.session);
	}

	// Token: 0x060006C4 RID: 1732
	protected abstract void LoadCategories(Session session);

	// Token: 0x060006C5 RID: 1733 RVA: 0x0002A804 File Offset: 0x00028A04
	public void ViewTab(string tabName)
	{
		SBGUITabButton sbguitabButton = (SBGUITabButton)base.FindChild(string.Format("tab_{0}", tabName));
		SBGUITabBar sbguitabBar = (SBGUITabBar)base.FindChild("tabbar");
		sbguitabBar.TabClick(sbguitabButton);
		this.BuildTabForButton(sbguitabButton);
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x0002A848 File Offset: 0x00028A48
	public void ViewCurrentTab()
	{
		if (this.currentTab != null)
		{
			this.ViewTab(this.currentTab.name);
		}
	}

	// Token: 0x060006C7 RID: 1735
	protected abstract Rect CalculateTabContentsSize(string tabName);

	// Token: 0x060006C8 RID: 1736 RVA: 0x0002A878 File Offset: 0x00028A78
	protected override int GetSlotIndex(Vector2 pos)
	{
		return Mathf.FloorToInt(pos.x / this.GetSlotSize().x);
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0002A8A0 File Offset: 0x00028AA0
	protected override Vector2 GetSlotOffset(int index)
	{
		return new Vector2(this.GetSlotSize().x * (float)index, 0f);
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x0002A8C8 File Offset: 0x00028AC8
	protected virtual void BuildTabForButton(SBGUITabButton tab)
	{
		this.TabClickedEvent.FireEvent(tab);
		string name = tab.category.Name;
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("name_label");
		TFUtils.Assert(tab.category.Label != null, "Tab does not contain a 'label' in catalog.json");
		sbguilabel.SetText(Language.Get(tab.category.Label));
		if (this.firstTabBuilt)
		{
			this.soundEffectMgr.PlaySound("SwitchTab");
		}
		if (this.currentTab != null)
		{
			base.ClearCachedSlotInfos();
			this.region.ClearSlotActions();
			this.currentTab = null;
		}
		this.BuildTab(name);
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x0002A97C File Offset: 0x00028B7C
	private void BuildTab(string tabName)
	{
		base.ClearCachedSlotInfos();
		base.PreLoadRegionContentInfo();
		base.StartCoroutine(this.BuildTabCoroutine(tabName));
	}

	// Token: 0x060006CC RID: 1740
	protected abstract IEnumerator BuildTabCoroutine(string tabName);

	// Token: 0x060006CD RID: 1741 RVA: 0x0002A9A4 File Offset: 0x00028BA4
	public override SBGUIElement FindDynamicSubElementSessionActionId(string sessionActionId, bool includeInactive)
	{
		SBGUITabBar sbguitabBar = (SBGUITabBar)base.FindChild("tabbar");
		SBGUITabButton sbguitabButton = sbguitabBar.FindButton(sessionActionId, includeInactive);
		TFUtils.Assert(sbguitabButton != null, string.Format("Trying to find dynamic sub element({0}), but that element does not exist on this element({1}). Could not find child named({2}) on tabbar({3}).", new object[]
		{
			sessionActionId,
			this.ToString(),
			sessionActionId,
			sbguitabBar.ToString()
		}));
		return sbguitabButton;
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x0002AA04 File Offset: 0x00028C04
	public override void Deactivate()
	{
		this.TabClickedEvent.ClearListeners();
		base.Deactivate();
	}

	// Token: 0x0400050D RID: 1293
	public EventDispatcher<SBGUITabButton> TabClickedEvent = new EventDispatcher<SBGUITabButton>();

	// Token: 0x0400050E RID: 1294
	protected Dictionary<string, SBTabCategory> categories;

	// Token: 0x0400050F RID: 1295
	protected Dictionary<string, SBGUIElement> tabContents = new Dictionary<string, SBGUIElement>();

	// Token: 0x04000510 RID: 1296
	protected SBGUIElement currentTab;

	// Token: 0x04000511 RID: 1297
	private bool firstTabBuilt;
}
