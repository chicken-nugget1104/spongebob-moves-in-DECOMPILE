using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarg;

// Token: 0x020000A9 RID: 169
public class SBGUIStandardScreen : SBGUIScreen
{
	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x0600062C RID: 1580 RVA: 0x000276B4 File Offset: 0x000258B4
	public SBGUILabel QuestCountLabel
	{
		get
		{
			return this.questCountLabel;
		}
	}

	// Token: 0x170000A1 RID: 161
	// (set) Token: 0x0600062D RID: 1581 RVA: 0x000276BC File Offset: 0x000258BC
	public int HelpshiftNotificationCount
	{
		set
		{
			this.helpshiftNotificationCount = value;
		}
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x000276C8 File Offset: 0x000258C8
	protected override void Awake()
	{
		base.Awake();
		TFUtils.Assert(this.FoodDeliverySize != Vector2.one, "You must change the default size for FoodDeliverySize on standard screen!");
		int childCount = base.gameObject.transform.childCount;
		this.nativeElements = new List<GameObject>(childCount);
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = base.gameObject.transform.GetChild(i).gameObject;
			this.nativeElements.Add(gameObject);
		}
		this.softCurrencyBar = (SBGUIPulseImage)base.FindChild("money_bar");
		this.softCurrencyBar.InitializePulser(this.softCurrencyBar.Size, 1.5f, 0.2f);
		this.softCurrencyIcon = (SBGUIPulseImage)base.FindChild("coin_icon");
		this.softCurrencyIcon.InitializePulser(this.softCurrencyIcon.Size, 2f, 0.2f);
		this.hardCurrencyBar = (SBGUIPulseImage)base.FindChild("jj_bar");
		this.hardCurrencyBar.InitializePulser(this.hardCurrencyBar.Size, 1.5f, 0.2f);
		this.hardCurrencyIcon = (SBGUIPulseImage)base.FindChild("jj_icon");
		this.hardCurrencyIcon.InitializePulser(this.hardCurrencyIcon.Size, 2f, 0.2f);
		this.xpBar = base.FindChild("happyface");
		this.xpBarStar = (SBGUIPulseImage)base.FindChild("levelup_bar_star");
		this.xpBarStar.InitializePulser(this.xpBarStar.Size, 2f, 0.2f);
		this.questMarker = base.FindChild("quest_marker");
		this.questsOrigin = (SBGUIButton)base.FindChild("quest");
		this.questCountIcon = base.FindChild("red_button");
		this.inventory = (SBGUIInventoryHudWidget)base.FindChild("inventory_widget");
		this.settingsHudIcon = base.FindChild("settings");
		this.settingsHudCountIcon = base.FindChild("red_counter");
		this.settingsHudCountLabel = (SBGUILabel)base.FindChild("red_counter_label");
		this.editModeHudIcon = base.FindChild("edit");
		this.inventoryHudIcon = base.FindChild("inventory");
		this.marketplaceHudIcon = base.FindChild("marketplace");
		this.communityEventHudIcon = base.FindChild("community_event");
		this.patchyHudTitleIcon = base.FindChild("patchy_title_icon");
		this.patchyHudTitleBg = base.FindChild("patchy_title_bg");
		this.patchyHudTitleLabel = base.FindChild("patchy_title_label");
		this.patchyHudIcon = base.FindChild("patchy");
		this.happyfaceHud = base.FindChild("happyface");
		this.jjBarHud = base.FindChild("jj_bar");
		this.moneyBarHud = base.FindChild("money_bar");
		this.specialBarHud = base.FindChild("special_bar");
		this.pathEditToggle = base.FindChild("editpath_toggle").gameObject.GetComponent<SBGUIAtlasButton>();
		this.questScrollUp = (SBGUIPulseButton)base.FindChild("quest_scroll_up");
		this.questScrollDown = (SBGUIPulseButton)base.FindChild("quest_scroll_down");
		this.questCountLabel = (SBGUILabel)base.FindChild("quest_count");
		TFUtils.Assert(this.questMarker != null, "Couldn't find the quest marker!");
		TFUtils.Assert(this.questsOrigin != null, "Couldn't find the quest HUD icon!");
		TFUtils.Assert(this.inventory != null, "Couldn't find the inventory HUD widget!");
		TFUtils.Assert(this.settingsHudIcon != null, "Couldn't find the settings HUD button!");
		TFUtils.Assert(this.editModeHudIcon != null, "Couldn't find the edit mode HUD button!");
		TFUtils.Assert(this.inventoryHudIcon != null, "Couldn't find the inventory HUD button!");
		TFUtils.Assert(this.marketplaceHudIcon != null, "Couldn't find the marketplace HUD button!");
		TFUtils.Assert(this.communityEventHudIcon != null, "Couldn't find the community event HUD button!");
		TFUtils.Assert(this.patchyHudTitleIcon != null, "Couldn't find the patchy Hud title icon!");
		TFUtils.Assert(this.patchyHudTitleBg != null, "Couldn't find the patchy HUD title background!");
		TFUtils.Assert(this.patchyHudTitleLabel != null, "Couldn't find the patchy HUD title label!");
		TFUtils.Assert(this.patchyHudIcon != null, "Couldn't find the patchy HUD button!");
		TFUtils.Assert(this.questScrollUp != null, "Couldn't find the Quest Scroll up button!");
		TFUtils.Assert(this.questScrollDown != null, "Couldn't find the Quest Scroll down button!");
		TFUtils.Assert(this.pathEditToggle != null, "Couldn't find the edit/path toggle HUD button!");
		this.QuestStatusEvent.AddListener(new Action<int>(this.ExamineQuest));
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x00027B78 File Offset: 0x00025D78
	protected override void OnDisable()
	{
		if (!this.questMarkersDoneAnimating)
		{
			if (SBGUIStandardScreen.questsShown)
			{
				this.HideQuestsCoroutineFinish();
			}
			else
			{
				this.ShowQuestsCoroutineFinish();
			}
		}
		base.OnDisable();
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00027BB4 File Offset: 0x00025DB4
	public void Initialize(Session session)
	{
		TFUtils.Assert(this.inventory != null, "Inventory cannot be null!");
		TFUtils.Assert(session.TheGame != null, "Game cannot be null!");
		Action<YGEvent, int?> interactCallback = delegate(YGEvent evt, int? productId)
		{
			this.ChildInventoryGotEvent(evt, productId, session);
			session.AddAsyncResponse("ResetSimulationDrag", true, false);
		};
		this.inventory.Setup(session.TheGame, session.TheGame.craftManager, session.TheGame.vendingManager, session.TheGame.resourceManager, session.TheSoundEffectManager, interactCallback, this.marketplaceHudIcon.GetScreenPosition().y - 60f * GUIView.ResolutionScaleFactor());
		this.inventory.CloseRows();
		this.DeactivateQuestScrollButtons();
		this.ToggleQuestTracker(session, true, false);
		this.ReadyEvent.FireEvent();
		bool flag = this.ShouldCommunityShow();
		if (flag)
		{
			CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
			SBGUIAtlasImage component = this.communityEventHudIcon.GetComponent<SBGUIAtlasImage>();
			if (component != null)
			{
				component.SetTextureFromAtlas(activeEvent.m_sEventButtonTexture);
			}
			this.ResetCommunityImage(true);
		}
		else
		{
			SBGUIAtlasImage component2 = this.communityEventHudIcon.GetComponent<SBGUIAtlasImage>();
			if (component2 != null)
			{
				component2.SetTextureFromAtlas("_blank_.png");
			}
			this.ResetCommunityImage(false);
		}
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00027D3C File Offset: 0x00025F3C
	public void SetInventoryWidgetDraggingCallbacks(Action<int, YGEvent> startDragoutCallback, Action<YGEvent> dragThroughHandler)
	{
		this.inventory.StartDragOutCallback = startDragoutCallback;
		this.inventory.DragThroughCallback = dragThroughHandler;
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x00027D58 File Offset: 0x00025F58
	public void RefreshFromCache()
	{
		this.ResetCommunityImage(this.ShouldCommunityShow());
		this.DeactivateQuestScrollButtons();
		this.DeactivatePatchyUI();
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00027D74 File Offset: 0x00025F74
	public void DisableInactiveElements()
	{
		if (!this.ShouldCommunityShow())
		{
			this.ResetCommunityImage(false);
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00027D88 File Offset: 0x00025F88
	public override void Update()
	{
		if (this.session == null || this.session.TheGame == null)
		{
			return;
		}
		base.Update();
		if (!this.didSetupTweeningParams)
		{
			this.CalculateTweeningParams();
		}
		this.uiDuration -= Time.deltaTime;
		if (this.hidable && this.uiDuration < 0f && this.isUiOn)
		{
			base.StartCoroutine(this.HideUICoroutine());
		}
		this.inventory.OnUpdate(this.session.TheGame.resourceManager);
		this.UpdateGoodDeliveries();
		if ((int?)this.session.CheckAsyncRequest(ResourceDrop.MakeResourceKey(ResourceManager.XP)) != null)
		{
			this.XPBarStarAnimatedPulse();
		}
		if ((int?)this.session.CheckAsyncRequest(ResourceDrop.MakeResourceKey(ResourceManager.SOFT_CURRENCY)) != null)
		{
			this.SoftCurrencyBarAnimatedPulse();
		}
		if ((int?)this.session.CheckAsyncRequest(ResourceDrop.MakeResourceKey(ResourceManager.HARD_CURRENCY)) != null)
		{
			this.HardCurrencyBarAnimatedPulse();
		}
		if (this.questMarkersDoneAnimating && this.IsQuestTrackerVisible())
		{
			bool flag = false;
			bool flag2 = false;
			float? num = this.questScrollUpperBound;
			if (num == null)
			{
				this.questScrollUpperBound = new float?(this.questScrollUp.GetScreenPosition().y + (float)this.questScrollUp.Height * 0.01f);
			}
			float? num2 = this.questScrollLowerBound;
			if (num2 == null)
			{
				this.questScrollLowerBound = new float?(this.questScrollDown.GetScreenPosition().y + (float)this.questScrollDown.Height * 0.01f);
			}
			foreach (SBGUIButton sbguibutton in this.questButtons.Values)
			{
				SBGUIQuestTrackerSlot sbguiquestTrackerSlot = (SBGUIQuestTrackerSlot)sbguibutton;
				SBGUIQuestTrackerSlot sbguiquestTrackerSlot2 = sbguiquestTrackerSlot;
				float? num3 = this.questScrollUpperBound;
				float value = num3.Value;
				float? num4 = this.questScrollLowerBound;
				SBGUIQuestTrackerSlot.QuestTrackerState questTrackerState = sbguiquestTrackerSlot2.OnUpdate(value, num4.Value);
				if (questTrackerState == SBGUIQuestTrackerSlot.QuestTrackerState.AboveBounds)
				{
					flag = true;
				}
				else if (questTrackerState == SBGUIQuestTrackerSlot.QuestTrackerState.BelowBounds)
				{
					flag2 = true;
				}
			}
			if (flag && !this.questScrollUp.IsActive())
			{
				this.questScrollUp.SetActive(true);
			}
			else if (!flag && this.questScrollUp.IsActive())
			{
				this.questScrollUp.SetActive(false);
			}
			if (flag2 && !this.questScrollDown.IsActive())
			{
				this.questScrollDown.SetActive(true);
			}
			else if (!flag2 && this.questScrollDown.IsActive())
			{
				this.questScrollDown.SetActive(false);
			}
			if (!this.session.InFriendsGame)
			{
				if (this.inventory.IsActive() && this.session.marketpalceActive)
				{
					this.inventory.SetActive(false);
				}
				else if (!this.inventory.IsActive() && !this.session.marketpalceActive)
				{
					this.inventory.SetActive(true);
				}
			}
		}
	}

	// Token: 0x170000A2 RID: 162
	// (set) Token: 0x06000635 RID: 1589 RVA: 0x00028104 File Offset: 0x00026304
	public override bool UsedInSessionAction
	{
		set
		{
			base.UsedInSessionAction = value;
			this.EnableFullHiding(!this.UsedInSessionAction);
		}
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0002811C File Offset: 0x0002631C
	private void UpdateGoodDeliveries()
	{
		foreach (GoodWidgetTransfer goodWidgetTransfer in this.goodWidgetTransfers)
		{
			Vector2 targetScreenPosition = goodWidgetTransfer.GetTargetScreenPosition(this.session, this.inventory.GetScreenPosition());
			Vector2 screenPosition = goodWidgetTransfer.icon.GetScreenPosition();
			Vector2 vector = targetScreenPosition - screenPosition;
			float num = vector.SqrMagnitude();
			vector.Normalize();
			vector *= goodWidgetTransfer.speed;
			goodWidgetTransfer.icon.SetScreenPosition(screenPosition + vector);
			float num2 = goodWidgetTransfer.speed * goodWidgetTransfer.speed * 2f;
			if (num <= num2)
			{
				this.goodWidgetTransferCorpses.Add(goodWidgetTransfer);
			}
			this.inventory.IncrementDeductionsForTick(goodWidgetTransfer.goodId);
			this.ResetUIVisibleDuration();
		}
		foreach (GoodWidgetTransfer goodWidgetTransfer2 in this.goodWidgetTransferCorpses)
		{
			this.goodWidgetTransfers.Remove(goodWidgetTransfer2);
			goodWidgetTransfer2.icon.Destroy();
		}
		this.goodWidgetTransferCorpses.Clear();
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00028294 File Offset: 0x00026494
	private void ResetUIVisibleDuration()
	{
		this.uiDuration = 30f;
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x000282A4 File Offset: 0x000264A4
	private void ChildInventoryGotEvent(YGEvent evt, int? productId, Session session)
	{
		if (evt != null)
		{
			if (evt.type == YGEvent.TYPE.TOUCH_BEGIN)
			{
				session.AddAsyncResponse("OriginalDragEvent", evt, false);
				session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
			}
			else if (evt.type == YGEvent.TYPE.TOUCH_MOVE)
			{
				session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
			}
			else
			{
				session.CheckAsyncRequest("OriginalDragEvent");
				session.TheCamera.SetEnableUserInput(true, false, default(Vector3));
			}
		}
		if (productId != null)
		{
			session.AddAsyncResponse("LastStandardHudTouchedProduct", productId.Value, false);
		}
		this.ResetUIVisibleDuration();
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00028360 File Offset: 0x00026560
	public bool ShowInventoryWidget()
	{
		this.inventory.SetActive(true);
		return this.inventory.ActivateAllTabs();
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0002837C File Offset: 0x0002657C
	public void CloseInventoryWidget()
	{
		this.inventory.CloseRows();
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0002838C File Offset: 0x0002658C
	public void TryPulseResourceError(int resourceId)
	{
		this.inventory.TryPulseResourceError(resourceId);
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0002839C File Offset: 0x0002659C
	public void DeliverGood(GoodToSimulatedDeliveryRequest goodDelivery)
	{
		SBGUIPulseImage sbguipulseImage = SBGUIPulseImage.Create(this, goodDelivery.materialName, this.FoodDeliverySize, 2f, 0.5f, null);
		sbguipulseImage.name = "HudDeliveryIcon_" + sbguipulseImage.name;
		sbguipulseImage.SetScreenPosition(goodDelivery.GetOriginalScreenPosition(this.session, this.inventory.GetScreenPosition()));
		sbguipulseImage.Pulser.PulseOneShot();
		goodDelivery.icon = sbguipulseImage;
		this.goodWidgetTransfers.Add(goodDelivery);
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00028418 File Offset: 0x00026618
	public void ReturnGood(GoodReturnRequest goodReturn)
	{
		SBGUIPulseImage sbguipulseImage = SBGUIPulseImage.Create(this, goodReturn.materialName, this.FoodDeliverySize, 2f, 0.2f, null);
		sbguipulseImage.name = "HudReturnIcon_" + sbguipulseImage.name;
		sbguipulseImage.SetScreenPosition(goodReturn.GetOriginalScreenPosition(this.session, this.inventory.GetScreenPosition()));
		sbguipulseImage.Pulser.PulseStartLoop();
		goodReturn.icon = sbguipulseImage;
		this.goodWidgetTransfers.Add(goodReturn);
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00028494 File Offset: 0x00026694
	public void ToggleQuestTracker(Session session, bool bForce = false, bool bIsButton = false)
	{
		if (bForce)
		{
			SBGUIStandardScreen.questsThinkTheyreOn = bForce;
			this.EnableQuestTracker(SBGUIStandardScreen.questsThinkTheyreOn, session, bForce);
			this.ResetUIVisibleDuration();
			return;
		}
		if (SBGUIStandardScreen.questsThinkTheyreOn)
		{
			session.TheSoundEffectManager.PlaySound("CloseQuestList");
			if (bIsButton)
			{
				AnalyticsWrapper.LogUIInteraction(session.TheGame, "ui_hide_quests", "button", "tap");
			}
		}
		else
		{
			session.TheSoundEffectManager.PlaySound("OpenQuestList");
			if (bIsButton)
			{
				AnalyticsWrapper.LogUIInteraction(session.TheGame, "ui_display_quests", "button", "tap");
			}
		}
		SBGUIStandardScreen.questsThinkTheyreOn = !SBGUIStandardScreen.questsThinkTheyreOn;
		this.EnableQuestTracker(SBGUIStandardScreen.questsThinkTheyreOn, session, false);
		this.ResetUIVisibleDuration();
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00028554 File Offset: 0x00026754
	public bool EnableQuestTracker(bool enable, Session session, bool bForce = false)
	{
		if (enable)
		{
			if ((!SBGUIStandardScreen.questsShown || bForce) && base.IsActive())
			{
				base.StartCoroutine(this.ShowQuestsCoroutine());
				return true;
			}
		}
		else if ((SBGUIStandardScreen.questsShown || bForce) && base.IsActive())
		{
			base.StartCoroutine(this.HideQuestsCoroutine());
			return true;
		}
		return false;
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x000285C4 File Offset: 0x000267C4
	public void EnableUI(bool enable)
	{
		if (enable)
		{
			this.ResetUIVisibleDuration();
			if (!this.isUiOn)
			{
				base.gameObject.SetActive(true);
				base.StartCoroutine(this.ShowUICoroutine());
			}
		}
		else if (this.isUiOn)
		{
			base.StartCoroutine(this.HideUICoroutine());
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00028620 File Offset: 0x00026820
	public void EnableFullHiding(bool enabled)
	{
		if (this.hidable == enabled)
		{
			return;
		}
		this.hidable = enabled;
		if (!this.hidable && !this.isUiOn)
		{
			base.gameObject.active = true;
			base.StartCoroutine(this.ShowUICoroutine());
		}
		else if (this.hidable)
		{
			this.ResetUIVisibleDuration();
		}
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x00028688 File Offset: 0x00026888
	public void SetPatchyHudIconVisible()
	{
		if (this.session == null)
		{
			return;
		}
		if (this.session.TheGame == null)
		{
			return;
		}
		QuestManager questManager = this.session.TheGame.questManager;
		if (questManager == null)
		{
			return;
		}
		if (this.patchyHudIcon == null)
		{
			TFUtils.ErrorLog("Patchy Button Does Not Yet Exist");
			return;
		}
		try
		{
			if ((string.Compare(this.session.TheState.ToString(), "Session+Playing") == 0 || string.Compare(this.session.TheState.ToString(), "Session+DragFeeding") == 0) && !SBSettings.DisableFriendPark)
			{
				if (questManager.IsQuestActive(2400U))
				{
					this.patchyHudIcon.SetActive(true);
				}
				else
				{
					this.patchyHudIcon.SetActive(questManager.IsQuestCompleted(2400U));
				}
			}
			else
			{
				this.patchyHudIcon.SetActive(false);
			}
		}
		catch (Exception ex)
		{
			TFUtils.ErrorLog("SetPatchyHudIconVisible: " + ex.Message + "\n" + ex.StackTrace);
		}
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x000287C0 File Offset: 0x000269C0
	public void SetVisibleNonEssentialElements(bool visible)
	{
		this.SetVisibleNonEssentialElements(visible, false);
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x000287CC File Offset: 0x000269CC
	public void SetVisibleNonEssentialElements(bool visible, bool alsoHideGrubWidget)
	{
		this.settingsHudIcon.SetActive(visible);
		this.ShowHelpshiftNotification();
		this.editModeHudIcon.SetActive(visible);
		this.pathEditToggle.SetActive(visible);
		this.inventoryHudIcon.SetActive(visible);
		this.marketplaceHudIcon.SetActive(visible);
		this.questsOrigin.SetActive(visible);
		this.questScrollDown.SetActive(visible);
		this.questScrollUp.SetActive(visible);
		if (this.session.InFriendsGame)
		{
			this.patchyHudTitleIcon.SetActive(visible);
			this.patchyHudTitleBg.SetActive(visible);
			this.patchyHudTitleLabel.SetActive(visible);
		}
		else
		{
			this.patchyHudTitleIcon.SetActive(false);
			this.patchyHudTitleBg.SetActive(false);
			this.patchyHudTitleLabel.SetActive(false);
		}
		this.patchyHudIcon.SetActive(visible && !SBSettings.DisableFriendPark);
		if (this.session.TheGame.communityEventManager.GetActiveEvent() != null)
		{
			this.ResetCommunityImage(this.ShouldCommunityShow() && visible);
		}
		else
		{
			this.ResetCommunityImage(false);
		}
		if (!visible)
		{
			if (alsoHideGrubWidget)
			{
				this.inventory.SetActive(false);
			}
			this.EnableQuestTracker(false, this.session, false);
		}
		else
		{
			this.inventory.SetActive(true);
			if (SBGUIStandardScreen.questsThinkTheyreOn)
			{
				this.EnableQuestTracker(true, this.session, false);
			}
		}
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00028944 File Offset: 0x00026B44
	public void HideAllElements()
	{
		this.SetVisibleNonEssentialElements(false, true);
		this.HideCurrencies();
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x00028954 File Offset: 0x00026B54
	public void HideCurrencies()
	{
		this.jjBarHud.SetActive(false);
		this.moneyBarHud.SetActive(false);
		this.specialBarHud.SetActive(false);
		this.xpBar.SetActive(false);
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00028994 File Offset: 0x00026B94
	public void ShowCurrencies()
	{
		this.jjBarHud.SetActive(true);
		this.moneyBarHud.SetActive(true);
		this.specialBarHud.SetActive(true);
		this.xpBar.SetActive(true);
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x000289D4 File Offset: 0x00026BD4
	public void HideElementsForEditMode(bool editMode)
	{
		if (editMode)
		{
			this.EnableQuestTracker(false, this.session, false);
			this.xpBar.SetActive(false);
			this.inventory.SetActive(false);
			this.questsOrigin.transform.localPosition = new Vector3(this.questsOrigin.transform.localPosition.x, this.questsOrigin.transform.localPosition.y, 100f);
			this.questsOrigin.MuteButtons(true);
			this.questsOrigin.EnableButtons(false);
		}
		else
		{
			if (this.session.TheState.GetType().Equals(typeof(Session.Playing)) && SBGUIStandardScreen.questsThinkTheyreOn)
			{
				this.EnableQuestTracker(true, this.session, false);
			}
			this.xpBar.SetActive(true);
			this.inventory.SetActive(true);
			this.questsOrigin.transform.localPosition = new Vector3(this.questsOrigin.transform.localPosition.x, this.questsOrigin.transform.localPosition.y, 1f);
			this.questsOrigin.MuteButtons(false);
			this.questsOrigin.EnableButtons(true);
		}
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x00028B2C File Offset: 0x00026D2C
	public void SoftCurrencyBarAnimatedPulse()
	{
		this.softCurrencyBar.Pulser.PulseOneShot();
		this.softCurrencyIcon.Pulser.PulseOneShot();
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x00028B5C File Offset: 0x00026D5C
	public void HardCurrencyBarAnimatedPulse()
	{
		this.hardCurrencyBar.Pulser.PulseOneShot();
		this.hardCurrencyIcon.Pulser.PulseOneShot();
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00028B8C File Offset: 0x00026D8C
	public void XPBarStarAnimatedPulse()
	{
		this.xpBarStar.Pulser.PulseOneShot();
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00028BA0 File Offset: 0x00026DA0
	private IEnumerator HideQuestsCoroutine()
	{
		SBGUIStandardScreen.questsShown = false;
		float questInterp = 0f;
		this.questMarkersDoneAnimating = false;
		this.DeactivateQuestScrollButtons();
		this.DeactivatePatchyUI();
		this.questMarker.EnableButtons(false);
		this.questScrollDown.EnableButtons(false);
		this.questScrollUp.EnableButtons(false);
		this.questsOrigin.EnableButtons(false);
		while (questInterp <= 1f)
		{
			questInterp += Time.deltaTime / this.HideQuestsAnimDuration;
			this.InterpolateQuestButtons(1f - questInterp, 0f, new Func<float, float, float, float>(Easing.EaseInCirc));
			yield return null;
		}
		this.HideQuestsCoroutineFinish();
		yield break;
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x00028BBC File Offset: 0x00026DBC
	private void HideQuestsCoroutineFinish()
	{
		this.InterpolateQuestButtons(0f, 0f, new Func<float, float, float, float>(Easing.Linear));
		foreach (object obj in base.transform.FindChild("upperleft/quest_marker"))
		{
			Transform transform = (Transform)obj;
			transform.renderer.enabled = false;
			transform.FindChild("questbackground").renderer.enabled = false;
		}
		this.questMarker.EnableButtons(true);
		this.questScrollDown.EnableButtons(true);
		this.questScrollUp.EnableButtons(true);
		this.questsOrigin.EnableButtons(true);
		this.questMarkersDoneAnimating = true;
		this.DeactivateQuestScrollButtons();
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x00028CAC File Offset: 0x00026EAC
	private IEnumerator ShowQuestsCoroutine()
	{
		SBGUIStandardScreen.questsShown = true;
		bool buttonsOn = false;
		foreach (object obj in base.transform.FindChild("upperleft/quest_marker"))
		{
			Transform child = (Transform)obj;
			child.renderer.enabled = true;
			child.FindChild("questbackground").renderer.enabled = true;
		}
		this.questMarkersDoneAnimating = false;
		this.questMarker.EnableButtons(buttonsOn);
		float offset = (float)(this.questButtons.Keys.Count - 1) * 0.05f * -1f;
		this.questInterp = offset;
		while (this.questInterp <= 1f)
		{
			this.questInterp += Time.deltaTime / 0.8f;
			this.InterpolateQuestButtons(this.questInterp + offset, 0.05f, new Func<float, float, float, float>(Easing.EaseOutElastic));
			this.ResetUIVisibleDuration();
			if (!buttonsOn && this.questInterp > 0.66f)
			{
				buttonsOn = true;
				this.questMarker.EnableButtons(buttonsOn);
			}
			yield return null;
		}
		this.ShowQuestsCoroutineFinish();
		yield break;
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x00028CC8 File Offset: 0x00026EC8
	private void ShowQuestsCoroutineFinish()
	{
		this.InterpolateQuestButtons(1f, 0f, new Func<float, float, float, float>(Easing.Linear));
		this.questMarkersDoneAnimating = true;
		this.questMarker.EnableButtons(true);
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x00028CFC File Offset: 0x00026EFC
	private void InterpolateQuestButtons(float interp, float delay, Func<float, float, float, float> easeFn)
	{
		Vector3 b = new Vector3(0f, -0.7f, 0f);
		Vector3 vector = Vector3.zero;
		float num = 0f;
		Vector3 start = this.questsOrigin.WorldPosition + new Vector3(0f, 0f, 0.2f);
		foreach (SBGUIButton sbguibutton in this.questButtons.Values)
		{
			sbguibutton.WorldPosition = Easing.Vector3Easing(start, this.questMarker.WorldPosition + vector, Mathf.Clamp01(interp + num), easeFn);
			vector += b;
			num += delay;
		}
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x00028DE0 File Offset: 0x00026FE0
	public override void Close()
	{
		this.postHideCallback = new Action(base.Close);
		this.hidable = true;
		if (this.isUiOn)
		{
			this.inventory.CloseRows();
			this.SetActive(false);
			this.isUiOn = false;
		}
		base.Close();
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00028E30 File Offset: 0x00027030
	public override void Deactivate()
	{
		this.hidable = true;
		if (this.isUiOn)
		{
			if (base.gameObject.activeSelf)
			{
				base.StartCoroutine(this.HideUICoroutine());
			}
		}
		else
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name != "upperleft")
				{
					transform.gameObject.SetActive(false);
				}
			}
		}
		foreach (object obj2 in base.transform.FindChild("upperleft/quest_marker"))
		{
			Transform transform2 = (Transform)obj2;
			transform2.renderer.enabled = false;
			transform2.FindChild("questbackground").renderer.enabled = false;
		}
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x00028F78 File Offset: 0x00027178
	protected override void OnEnable()
	{
		this.EnableUI(true);
		base.OnEnable();
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x00028F88 File Offset: 0x00027188
	public void CalculateTweeningParams()
	{
		TFUtils.Assert(!this.didSetupTweeningParams, "Should only setup the tweening params once.");
		Vector3 vector = Vector3.zero;
		foreach (GameObject gameObject in this.nativeElements)
		{
			vector += gameObject.transform.position;
		}
		vector /= (float)this.nativeElements.Count;
		this.elementPositionings.Clear();
		foreach (GameObject gameObject2 in this.nativeElements)
		{
			Vector3 position = gameObject2.transform.position;
			Vector3 vector2 = gameObject2.transform.position - vector;
			vector2.Normalize();
			vector2 *= 2f;
			vector2 += position;
			this.elementPositionings.Add(gameObject2.name, new SBGUIStandardScreen.Positioning(gameObject2, position, vector2));
		}
		this.didSetupTweeningParams = true;
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x000290E8 File Offset: 0x000272E8
	private IEnumerator HideUICoroutine()
	{
		if (this.isUiOn)
		{
			while (this.uiInterpolator.IsLocked)
			{
				yield return null;
			}
			if (!this.questMarkersDoneAnimating)
			{
				this.questInterp = 1f;
				yield return null;
			}
			this.uiInterpolator.Lock();
			if (this.isUiOn)
			{
				this.isUiOn = false;
				this.inventory.CloseRows();
				this.DeactivatePatchyUI();
				float interp = 0f;
				while (interp <= 1f)
				{
					interp += Time.deltaTime / 0.3f;
					this.uiInterpolator.UpdateUIEasing(this.elementPositionings, interp, new Func<float, float, float, float>(Easing.EaseInBack));
					yield return null;
				}
				foreach (object obj in base.transform)
				{
					Transform child = (Transform)obj;
					if (child.name != "upperleft")
					{
						child.gameObject.SetActive(false);
					}
				}
				foreach (object obj2 in base.transform.FindChild("upperleft/quest_marker"))
				{
					Transform child2 = (Transform)obj2;
					child2.renderer.enabled = false;
					child2.FindChild("questbackground").renderer.enabled = false;
				}
				this.isUiOn = false;
				if (this.postHideCallback != null)
				{
					this.postHideCallback();
				}
			}
			this.uiInterpolator.Unlock();
		}
		yield break;
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x00029104 File Offset: 0x00027304
	private IEnumerator ShowUICoroutine()
	{
		if (!this.isUiOn)
		{
			while (this.uiInterpolator.IsLocked)
			{
				yield return null;
			}
			if (!this.questMarkersDoneAnimating)
			{
				this.questInterp = 1f;
				yield return null;
			}
			this.uiInterpolator.Lock();
			if (!this.isUiOn)
			{
				this.SetActive(true);
				this.HideZeroQuestCount();
				this.ShowHelpshiftNotification();
				foreach (object obj in base.transform.FindChild("upperleft/quest_marker"))
				{
					Transform child = (Transform)obj;
					child.renderer.enabled = true;
					child.FindChild("questbackground").renderer.enabled = true;
				}
				SBGUIAtlasImage editModeFishPerson = (SBGUIAtlasImage)base.FindChild("edit_mode_fish_person");
				if (editModeFishPerson.IsActive())
				{
					editModeFishPerson.SetTextureFromAtlas("EditMode_FishPerson", true, false, true, false, false, 0);
				}
				this.ResetCommunityImage(this.ShouldCommunityShow());
				base.EnableButtons(false);
				this.isUiOn = true;
				this.DeactivateQuestScrollButtons();
				if (!this.session.InFriendsGame)
				{
					this.DeactivatePatchyUI();
				}
				else
				{
					this.DeactivateNonPatchyUI();
				}
				if (!SBGUIStandardScreen.userClosedWishList)
				{
					this.inventory.ActivateAllTabs();
				}
				else
				{
					this.inventory.CloseRows();
				}
				float interp = 1f;
				while (interp > 0f)
				{
					interp -= Time.deltaTime / 0.2f;
					interp = Mathf.Max(0f, interp);
					this.uiInterpolator.UpdateUIEasing(this.elementPositionings, interp, new Func<float, float, float, float>(Easing.EaseInBack));
					yield return null;
				}
				this.uiInterpolator.UpdateUIEasing(this.elementPositionings, 0f, new Func<float, float, float, float>(Easing.Linear));
				if (!SBGUIStandardScreen.userClosedWishList)
				{
					this.inventory.ActivateAllTabs();
				}
				else
				{
					this.inventory.CloseRows();
				}
				base.EnableButtons(true);
				this.ResetCommunityImage(this.ShouldCommunityShow());
				this.isUiOn = true;
			}
			base.ReregisterColliders();
			this.uiInterpolator.Unlock();
		}
		yield break;
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x00029120 File Offset: 0x00027320
	private SBGUIButton AddQuestTracker(uint did, string texture, Action clickHandler)
	{
		if (this.session.TheGame.questManager.IsQuestActivated(40000U))
		{
			return null;
		}
		TFUtils.Assert(this.questMarker != null, "Must find reference for questMarker first!");
		if (!string.Equals(texture, "n/a"))
		{
			SBGUIQuestTrackerSlot sbguiquestTrackerSlot = (SBGUIQuestTrackerSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/QuestIcon");
			sbguiquestTrackerSlot.SetParent(this.questMarker);
			sbguiquestTrackerSlot.SessionActionId = QuestDefinition.GenerateSessionActionId(did);
			int scalePixel = (int)sbguiquestTrackerSlot.Size.x;
			sbguiquestTrackerSlot.SetTextureFromAtlas(texture, true, false, true, scalePixel);
			this.AttachAnalyticsToButton("questIcon", sbguiquestTrackerSlot);
			sbguiquestTrackerSlot.ClickEvent += clickHandler;
			if (this.questButtons.ContainsKey(did))
			{
				this.RemoveQuestTracker(did);
			}
			this.questButtons.Add(did, sbguiquestTrackerSlot);
			this.ToggleQuestTracker(this.session, true, false);
			return sbguiquestTrackerSlot;
		}
		return null;
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00029200 File Offset: 0x00027400
	private void RemoveQuestTracker(uint did)
	{
		SBGUIButton sbguibutton;
		if (!this.questButtons.TryGetValue(did, out sbguibutton))
		{
			return;
		}
		sbguibutton.SetParent(null);
		UnityEngine.Object.Destroy(sbguibutton.gameObject);
		this.questButtons.Remove(did);
		if (SBGUIStandardScreen.questsThinkTheyreOn)
		{
			this.EnableQuestTracker(true, this.session, true);
		}
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x0002925C File Offset: 0x0002745C
	public void RemoveQuestTrackers()
	{
		List<uint> list = new List<uint>();
		list.AddRange(this.questButtons.Keys);
		foreach (uint did in list)
		{
			this.RemoveQuestTracker(did);
		}
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x000292D4 File Offset: 0x000274D4
	public void RefreshQuestTrackers(Session session)
	{
		List<uint> list = new List<uint>();
		List<uint> list2 = new List<uint>();
		list2.AddRange(this.questButtons.Keys);
		foreach (uint num in list2)
		{
			if (session.TheGame.questManager.IsQuestActive(num))
			{
				list.Add(num);
			}
			else
			{
				this.RemoveQuestTracker(num);
			}
		}
		uint questDid;
		foreach (uint questDid2 in session.TheGame.questManager.ActiveQuestDidsNotInPostponed)
		{
			questDid = questDid2;
			if (!list.Exists((uint did) => did == questDid))
			{
				Action clickHandler = this.HandleClick(session, (int)questDid);
				QuestDefinition questDefinition = session.TheGame.questManager.GetQuestDefinition(questDid);
				if (!questDefinition.Icon.Equals(string.Empty))
				{
					this.AddQuestTracker(questDefinition.Did, questDefinition.Icon, clickHandler);
				}
			}
		}
		if (CommunityEventManager._pEventStatusDialogData != null)
		{
			CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
			if (activeEvent != null)
			{
				Action clickHandler2 = delegate()
				{
					if (CommunityEventManager._pEventStatusDialogData != null)
					{
						List<DialogInputData> inputs = new List<DialogInputData>
						{
							new SpongyGamesDialogInputData(600001U, CommunityEventManager._pEventStatusDialogData)
						};
						if (session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, inputs, 600001U))
						{
							session.TheGame.communityEventManager.DialogNeeded();
						}
					}
					else
					{
						this.RemoveQuestTracker(600001U);
					}
				};
				this.AddQuestTracker(600001U, activeEvent.m_sQuestIcon, clickHandler2);
			}
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x0600065B RID: 1627 RVA: 0x000294C8 File Offset: 0x000276C8
	public int GetQuestButtonCount
	{
		get
		{
			return this.questButtons.Count;
		}
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x000294D8 File Offset: 0x000276D8
	public void HideZeroQuestCount()
	{
		if (this.questButtons.Count == 0)
		{
			this.questCountIcon.SetActive(false);
		}
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x000294F8 File Offset: 0x000276F8
	public void ShowHelpshiftNotification()
	{
		if (this.settingsHudCountIcon != null && this.settingsHudIcon.IsActive())
		{
			if (this.helpshiftNotificationCount > 0)
			{
				this.settingsHudCountIcon.SetActive(true);
			}
			else
			{
				this.settingsHudCountIcon.SetActive(false);
			}
			this.settingsHudCountLabel.SetText(this.helpshiftNotificationCount.ToString());
		}
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x00029568 File Offset: 0x00027768
	private Action HandleClick(Session session, int did)
	{
		return delegate()
		{
			if (session.TheState.GetType().Equals(typeof(Session.Playing)))
			{
				this.QuestStatusEvent.FireEvent(did);
				TFUtils.ErrorLog("QuestStatusEvent.FireEvent: " + did + " (HandleClick)");
			}
		};
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x0002959C File Offset: 0x0002779C
	public void TryFireQuestStatusEvent(Session session, int did)
	{
		if (session.TheState.GetType().Equals(typeof(Session.Playing)))
		{
			this.QuestStatusEvent.FireEvent(did);
		}
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x000295D4 File Offset: 0x000277D4
	private void DeactivateQuestScrollButtons()
	{
		if (this.questScrollUp.IsActive())
		{
			this.questScrollUp.SetActive(false);
		}
		if (this.questScrollDown.IsActive())
		{
			this.questScrollDown.SetActive(false);
		}
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0002961C File Offset: 0x0002781C
	private void DeactivatePatchyUI()
	{
		if (this.patchyHudTitleBg.IsActive())
		{
			this.patchyHudTitleBg.SetActive(false);
		}
		if (this.patchyHudTitleIcon.IsActive())
		{
			this.patchyHudTitleIcon.SetActive(false);
		}
		if (this.patchyHudTitleLabel.IsActive())
		{
			this.patchyHudTitleLabel.SetActive(false);
		}
		this.SetPatchyHudIconVisible();
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00029684 File Offset: 0x00027884
	private void DeactivateNonPatchyUI()
	{
		this.jjBarHud.SetActive(false);
		this.moneyBarHud.SetActive(false);
		this.specialBarHud.SetActive(false);
		this.ResetCommunityImage(false);
		this.inventoryHudIcon.SetActive(false);
		this.editModeHudIcon.SetActive(false);
		this.questMarker.SetActive(false);
		this.happyfaceHud.SetActive(false);
		this.marketplaceHudIcon.SetActive(false);
		this.inventory.SetActive(false);
		this.questsOrigin.SetActive(false);
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00029710 File Offset: 0x00027910
	public bool IsQuestTrackerVisible()
	{
		return SBGUIStandardScreen.questsShown;
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00029718 File Offset: 0x00027918
	private void ExamineQuest(int questDid)
	{
		Quest quest = this.session.TheGame.questManager.GetQuest((uint)questDid);
		if (quest == null)
		{
			return;
		}
		if (SBGUI.GetInstance().ContainsGUIScreen<SBGUIAutoQuestStatusDialog>() || SBGUI.GetInstance().ContainsGUIScreen<SBGUIChunkQuestDialog>() || SBGUI.GetInstance().ContainsGUIScreen<SBGUIQuestDialog>())
		{
			return;
		}
		QuestDefinition questDefinition = this.session.TheGame.questManager.GetQuestDefinition(quest.Did);
		List<ConditionDescription> list = new List<ConditionDescription>();
		foreach (ConditionState conditionState in quest.EndConditions)
		{
			list.AddRange(conditionState.Describe(this.session.TheGame));
		}
		this.EnableUI(false);
		this.session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
		this.session.AddAsyncResponse("ignore_request_rush_sim", true);
		Action<SBGUIEvent, Session> oldGuiEventHandler = SBUIBuilder.UpdateGuiEventHandler(this.session, null);
		Action action = delegate()
		{
			this.session.TheSoundEffectManager.PlaySound("Accept");
			SBUIBuilder.ReleaseTopScreen();
			this.EnableUI(true);
			this.session.TheCamera.SetEnableUserInput(true, false, default(Vector3));
			this.session.CheckAsyncRequest("ignore_request_rush_sim");
			SBUIBuilder.UpdateGuiEventHandler(this.session, oldGuiEventHandler);
		};
		Action findButton = delegate()
		{
			this.session.TheSoundEffectManager.PlaySound("Accept");
			this.EnableUI(true);
			this.session.TheCamera.SetEnableUserInput(true, false, default(Vector3));
			this.session.CheckAsyncRequest("ignore_request_rush_sim");
			SBUIBuilder.UpdateGuiEventHandler(this.session, oldGuiEventHandler);
		};
		Action allDoneButton = delegate()
		{
			this.session.TheSoundEffectManager.PlaySound("Accept");
			SBUIBuilder.ReleaseTopScreen();
			this.EnableUI(true);
			this.session.TheCamera.SetEnableUserInput(true, false, default(Vector3));
			this.session.CheckAsyncRequest("ignore_request_rush_sim");
			SBUIBuilder.UpdateGuiEventHandler(this.session, oldGuiEventHandler);
			this.session.TheGame.simulation.ModifyGameState(new AutoQuestAllDoneAction(QuestDefinition.LastAutoQuestId));
		};
		if (questDefinition.Chunk)
		{
			if (questDefinition.Did == QuestDefinition.LastAutoQuestId)
			{
				SBUIBuilder.MakeAndPushAutoQuestStatusDialog(this, this.session, questDefinition, list, action, allDoneButton, action);
			}
			else
			{
				SBUIBuilder.MakeAndPushChunkQuestStatusDialog(this, this.session, questDefinition, list, action, action);
			}
		}
		else
		{
			SBUIBuilder.MakeAndPushQuestStatusDialog(this, this.session, questDefinition, list, action, findButton);
		}
		this.session.TheSoundEffectManager.PlaySound("quest_bubbles");
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x000298FC File Offset: 0x00027AFC
	private bool ShouldCommunityShow()
	{
		if (this.session == null)
		{
			return false;
		}
		if (this.session.TheGame == null)
		{
			return false;
		}
		if (this.session.TheGame.communityEventManager == null)
		{
			return false;
		}
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		return (activeEvent == null || activeEvent.m_nQuestPrereqID < 0 || this.session.TheGame.questManager.IsQuestCompleted((uint)activeEvent.m_nQuestPrereqID)) && (activeEvent != null && !activeEvent.m_bHideUI && !(this.session.TheState is Session.CommunityEventSession)) && !(this.session.TheState is Session.Shopping);
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x000299C8 File Offset: 0x00027BC8
	private void ResetCommunityImage(bool reset)
	{
		if (reset)
		{
			CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
			SBGUIAtlasImage component = this.communityEventHudIcon.GetComponent<SBGUIAtlasImage>();
			if (component != null)
			{
				component.SetTextureFromAtlas(activeEvent.m_sEventButtonTexture);
				this.communityEventHudIcon.SetActive(true);
			}
		}
		else
		{
			SBGUIAtlasImage component2 = this.communityEventHudIcon.GetComponent<SBGUIAtlasImage>();
			if (component2 != null)
			{
				component2.SetTextureFromAtlas("_blank_.png");
			}
			this.communityEventHudIcon.SetActive(false);
		}
	}

	// Token: 0x040004A7 RID: 1191
	public const string ORIGINAL_DRAG_EVENT = "OriginalDragEvent";

	// Token: 0x040004A8 RID: 1192
	public const string LAST_TOUCHED_PRODUCT = "LastStandardHudTouchedProduct";

	// Token: 0x040004A9 RID: 1193
	private const int QUEST_GAP = 12;

	// Token: 0x040004AA RID: 1194
	private const float DISPLAY_UI_TIMEOUT = 30f;

	// Token: 0x040004AB RID: 1195
	public Vector2 FoodDeliverySize = Vector2.one;

	// Token: 0x040004AC RID: 1196
	public static bool userClosedWishList;

	// Token: 0x040004AD RID: 1197
	public EventDispatcher<int> QuestStatusEvent = new EventDispatcher<int>();

	// Token: 0x040004AE RID: 1198
	private Dictionary<string, SBGUIStandardScreen.Positioning> elementPositionings = new Dictionary<string, SBGUIStandardScreen.Positioning>();

	// Token: 0x040004AF RID: 1199
	private List<GameObject> nativeElements;

	// Token: 0x040004B0 RID: 1200
	public SBGUIElement questMarker;

	// Token: 0x040004B1 RID: 1201
	private SBGUIButton questsOrigin;

	// Token: 0x040004B2 RID: 1202
	public SBGUIElement questCountIcon;

	// Token: 0x040004B3 RID: 1203
	private SBGUIElement settingsHudIcon;

	// Token: 0x040004B4 RID: 1204
	private SBGUIElement settingsHudCountIcon;

	// Token: 0x040004B5 RID: 1205
	private SBGUILabel settingsHudCountLabel;

	// Token: 0x040004B6 RID: 1206
	private SBGUIElement editModeHudIcon;

	// Token: 0x040004B7 RID: 1207
	private SBGUIElement inventoryHudIcon;

	// Token: 0x040004B8 RID: 1208
	private SBGUIElement marketplaceHudIcon;

	// Token: 0x040004B9 RID: 1209
	private SBGUIElement communityEventHudIcon;

	// Token: 0x040004BA RID: 1210
	private SBGUIElement patchyHudTitleIcon;

	// Token: 0x040004BB RID: 1211
	private SBGUIElement patchyHudTitleLabel;

	// Token: 0x040004BC RID: 1212
	private SBGUIElement patchyHudTitleBg;

	// Token: 0x040004BD RID: 1213
	private SBGUIElement patchyHudIcon;

	// Token: 0x040004BE RID: 1214
	private SBGUIElement happyfaceHud;

	// Token: 0x040004BF RID: 1215
	private SBGUIElement jjBarHud;

	// Token: 0x040004C0 RID: 1216
	private SBGUIElement moneyBarHud;

	// Token: 0x040004C1 RID: 1217
	private SBGUIElement specialBarHud;

	// Token: 0x040004C2 RID: 1218
	private SBGUILabel questCountLabel;

	// Token: 0x040004C3 RID: 1219
	private int helpshiftNotificationCount;

	// Token: 0x040004C4 RID: 1220
	private SBGUIAtlasButton pathEditToggle;

	// Token: 0x040004C5 RID: 1221
	private SBGUIPulseImage softCurrencyBar;

	// Token: 0x040004C6 RID: 1222
	private SBGUIPulseImage softCurrencyIcon;

	// Token: 0x040004C7 RID: 1223
	private SBGUIPulseImage hardCurrencyBar;

	// Token: 0x040004C8 RID: 1224
	private SBGUIPulseImage hardCurrencyIcon;

	// Token: 0x040004C9 RID: 1225
	private SBGUIElement xpBar;

	// Token: 0x040004CA RID: 1226
	private SBGUIPulseImage xpBarStar;

	// Token: 0x040004CB RID: 1227
	private SBGUIPulseButton questScrollUp;

	// Token: 0x040004CC RID: 1228
	private SBGUIPulseButton questScrollDown;

	// Token: 0x040004CD RID: 1229
	private float? questScrollUpperBound;

	// Token: 0x040004CE RID: 1230
	private float? questScrollLowerBound;

	// Token: 0x040004CF RID: 1231
	private bool questMarkersDoneAnimating = true;

	// Token: 0x040004D0 RID: 1232
	public SBGUIInventoryHudWidget inventory;

	// Token: 0x040004D1 RID: 1233
	public ReadyEventDispatcher ReadyEvent = new ReadyEventDispatcher();

	// Token: 0x040004D2 RID: 1234
	private Vector3 visiblePos;

	// Token: 0x040004D3 RID: 1235
	private Vector3 hiddenPos;

	// Token: 0x040004D4 RID: 1236
	private float uiDuration = 30f;

	// Token: 0x040004D5 RID: 1237
	private bool isUiOn = true;

	// Token: 0x040004D6 RID: 1238
	private static bool questsThinkTheyreOn = true;

	// Token: 0x040004D7 RID: 1239
	private static bool questsShown;

	// Token: 0x040004D8 RID: 1240
	private bool hidable = true;

	// Token: 0x040004D9 RID: 1241
	private Action postHideCallback;

	// Token: 0x040004DA RID: 1242
	private bool didSetupTweeningParams;

	// Token: 0x040004DB RID: 1243
	private SBGUIStandardScreen.Interpolator uiInterpolator = new SBGUIStandardScreen.Interpolator();

	// Token: 0x040004DC RID: 1244
	private SortedDictionary<uint, SBGUIButton> questButtons = new SortedDictionary<uint, SBGUIButton>();

	// Token: 0x040004DD RID: 1245
	private List<GoodWidgetTransfer> goodWidgetTransfers = new List<GoodWidgetTransfer>();

	// Token: 0x040004DE RID: 1246
	private List<GoodWidgetTransfer> goodWidgetTransferCorpses = new List<GoodWidgetTransfer>();

	// Token: 0x040004DF RID: 1247
	private float HideQuestsAnimDuration = 0.2f;

	// Token: 0x040004E0 RID: 1248
	private float questInterp;

	// Token: 0x020000AA RID: 170
	public class Positioning
	{
		// Token: 0x06000667 RID: 1639 RVA: 0x00029A58 File Offset: 0x00027C58
		public Positioning(GameObject gameObject, Vector3 origin, Vector3 target)
		{
			this.gameObject = gameObject;
			this.origin = origin;
			this.target = target;
		}

		// Token: 0x040004E1 RID: 1249
		public GameObject gameObject;

		// Token: 0x040004E2 RID: 1250
		public Vector3 origin;

		// Token: 0x040004E3 RID: 1251
		public Vector3 target;
	}

	// Token: 0x020000AB RID: 171
	private class Interpolator
	{
		// Token: 0x06000669 RID: 1641 RVA: 0x00029A80 File Offset: 0x00027C80
		public void Lock()
		{
			this.locks++;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00029A90 File Offset: 0x00027C90
		public void Unlock()
		{
			this.locks--;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00029AA0 File Offset: 0x00027CA0
		public bool IsLocked
		{
			get
			{
				return this.locks > 0;
			}
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00029AAC File Offset: 0x00027CAC
		public void UpdateUIEasing(Dictionary<string, SBGUIStandardScreen.Positioning> elementPositionings, float interp, Func<float, float, float, float> easingMethod)
		{
			foreach (string text in elementPositionings.Keys)
			{
				TFUtils.Assert(elementPositionings[text].gameObject != null, "Got a null GameObject for element=" + text);
				elementPositionings[text].gameObject.transform.position = Easing.Vector3Easing(elementPositionings[text].origin, elementPositionings[text].target, interp, easingMethod);
			}
		}

		// Token: 0x040004E4 RID: 1252
		private int locks;
	}
}
