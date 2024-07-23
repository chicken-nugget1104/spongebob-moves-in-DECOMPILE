using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020000B2 RID: 178
public abstract class SBGUITabbedDialog : SBGUIScreen
{
	// Token: 0x060006B7 RID: 1719 RVA: 0x0002A4A4 File Offset: 0x000286A4
	public void SetManagers(Session inSession)
	{
		this.session = inSession;
		this.entityMgr = this.session.TheGame.entities;
		this.resourceMgr = this.session.TheGame.resourceManager;
		this.soundEffectMgr = this.session.TheSoundEffectManager;
		this.LoadCategories(this.session);
		this.SetupTabCategories();
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x0002A508 File Offset: 0x00028708
	public void SetupTabCategories()
	{
		SBGUITabBar sbguitabBar = (SBGUITabBar)base.FindChild("tabbar");
		sbguitabBar.TabChangeEvent.AddListener(new Action<SBGUITabButton>(this.BuildTabForButton));
		sbguitabBar.SetupCategories(this.categories, this.session);
	}

	// Token: 0x060006B9 RID: 1721
	protected abstract void LoadCategories(Session session);

	// Token: 0x060006BA RID: 1722 RVA: 0x0002A550 File Offset: 0x00028750
	public void ViewTab(string tabName)
	{
		SBGUITabButton sbguitabButton = (SBGUITabButton)base.FindChild(string.Format("tab_{0}", tabName));
		SBGUITabBar sbguitabBar = (SBGUITabBar)base.FindChild("tabbar");
		sbguitabBar.TabClick(sbguitabButton);
		this.BuildTabForButton(sbguitabButton);
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x0002A594 File Offset: 0x00028794
	public void ViewCurrentTab()
	{
		if (this.currentTab != null)
		{
			this.ViewTab(this.currentTab.name);
		}
		else
		{
			SBGUITabBar sbguitabBar = (SBGUITabBar)base.FindChild("tabbar");
			sbguitabBar.TabClick(0);
		}
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0002A5E0 File Offset: 0x000287E0
	protected virtual void BuildTabForButton(SBGUITabButton tab)
	{
		this.TabClickedEvent.FireEvent(tab);
		string name = tab.category.Name;
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("name_label");
		TFUtils.Assert(tab.category.Label != null, "Tab does not contain a 'label' in catalog.json");
		if (sbguilabel != null)
		{
			sbguilabel.SetText(Language.Get(tab.category.Label));
		}
		if (this.firstTabBuilt)
		{
			this.soundEffectMgr.PlaySound("SwitchTab");
		}
		if (this.currentTab != null)
		{
			this.currentTab.SetActive(false);
			this.currentTab = null;
		}
		this.BuildTab(name);
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0002A69C File Offset: 0x0002889C
	private void BuildTab(string tabName)
	{
		SBGUIActivityIndicator sbguiactivityIndicator = (SBGUIActivityIndicator)base.FindChildSessionActionId("ActivityIndicator", false);
		if (sbguiactivityIndicator != null)
		{
			sbguiactivityIndicator.StartActivityIndicator();
		}
		this.mustWaitForInfoToLoad = true;
		base.StopAllCoroutines();
		base.StartCoroutine(this.BuildTabCoroutine(tabName));
	}

	// Token: 0x060006BE RID: 1726
	protected abstract IEnumerator BuildTabCoroutine(string tabName);

	// Token: 0x060006BF RID: 1727 RVA: 0x0002A6E8 File Offset: 0x000288E8
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

	// Token: 0x060006C0 RID: 1728 RVA: 0x0002A748 File Offset: 0x00028948
	public override void Deactivate()
	{
		if (this.currentTab != null)
		{
			this.currentTab.SetActive(false);
		}
		this.TabClickedEvent.ClearListeners();
		base.Deactivate();
	}

	// Token: 0x04000504 RID: 1284
	public EventDispatcher<SBGUITabButton> TabClickedEvent = new EventDispatcher<SBGUITabButton>();

	// Token: 0x04000505 RID: 1285
	protected Dictionary<string, SBTabCategory> categories;

	// Token: 0x04000506 RID: 1286
	protected Dictionary<string, SBGUIElement> tabContents = new Dictionary<string, SBGUIElement>();

	// Token: 0x04000507 RID: 1287
	protected SBGUIElement currentTab;

	// Token: 0x04000508 RID: 1288
	protected EntityManager entityMgr;

	// Token: 0x04000509 RID: 1289
	protected ResourceManager resourceMgr;

	// Token: 0x0400050A RID: 1290
	protected SoundEffectManager soundEffectMgr;

	// Token: 0x0400050B RID: 1291
	protected bool mustWaitForInfoToLoad = true;

	// Token: 0x0400050C RID: 1292
	private bool firstTabBuilt;
}
