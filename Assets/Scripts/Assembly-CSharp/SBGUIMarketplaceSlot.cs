using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class SBGUIMarketplaceSlot : SBGUIScrollListElement
{
	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000560 RID: 1376 RVA: 0x00022CCC File Offset: 0x00020ECC
	// (set) Token: 0x06000561 RID: 1377 RVA: 0x00022CD4 File Offset: 0x00020ED4
	public SBMarketOffer offer { get; private set; }

	// Token: 0x06000562 RID: 1378 RVA: 0x00022CE0 File Offset: 0x00020EE0
	public void Setup(SBGUIElement parent, SBMarketOffer offer, EventDispatcher<SBMarketOffer> offerClickedEvent, Vector3 offset, bool isDisabled, int? showLevelLock, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store)
	{
		base.gameObject.name = SBGUIMarketplaceSlot.GetSessionActionId(offer);
		this.SetActive(true);
		base.tform.localPosition = offset;
		this.SetParent(parent);
		this.isDisabled = isDisabled;
		this.showLevelLock = showLevelLock;
		this.Setup(offer, offerClickedEvent, resourceManager, entityManager, costumeManager, session, store);
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x00022D3C File Offset: 0x00020F3C
	public static SBGUIMarketplaceSlot Create(GameObject prefab, SBGUIElement parent, SBMarketOffer offer, EventDispatcher<SBMarketOffer> offerClickedEvent, Vector3 offset, bool isDisabled, int? showLevelLock, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
		gameObject.name = SBGUIMarketplaceSlot.GetSessionActionId(offer);
		TFUtils.ErrorLog("go.name: " + gameObject.name);
		SBGUIMarketplaceSlot component = gameObject.GetComponent<SBGUIMarketplaceSlot>();
		component.Setup(parent, offer, offerClickedEvent, offset, isDisabled, showLevelLock, resourceManager, entityManager, costumeManager, session, store);
		return component;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00022D98 File Offset: 0x00020F98
	private void Setup(SBMarketOffer o, EventDispatcher<SBMarketOffer> offerClickedEvent, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store)
	{
		base.StartCoroutine(this.SetupCoroutine(o, offerClickedEvent, resourceManager, entityManager, costumeManager, session, store));
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00022DC0 File Offset: 0x00020FC0
	public override void Deactivate()
	{
		base.ClearButtonActions("button");
		SBGUIRewardWidget[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIRewardWidget>(true);
		foreach (SBGUIRewardWidget sbguirewardWidget in componentsInChildren)
		{
			sbguirewardWidget.SetParent(null);
			sbguirewardWidget.gameObject.SetActiveRecursively(false);
			SBGUIRewardWidget.ReleaseRewardWidget(sbguirewardWidget);
		}
		this.VisibleEvent.ClearListeners();
		this.InvisibleEvent.ClearListeners();
		this.muted = false;
		base.Deactivate();
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00022E3C File Offset: 0x0002103C
	private new Dictionary<string, SBGUIElement> CacheChildren()
	{
		Dictionary<string, SBGUIElement> dictionary = new Dictionary<string, SBGUIElement>();
		SBGUIElement[] componentsInChildren = base.gameObject.GetComponentsInChildren<SBGUIElement>(true);
		foreach (SBGUIElement sbguielement in componentsInChildren)
		{
			dictionary.Add(sbguielement.name, sbguielement);
		}
		return dictionary;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00022E8C File Offset: 0x0002108C
	private IEnumerator SetupCoroutine(SBMarketOffer o, EventDispatcher<SBMarketOffer> offerClickedEvent, ResourceManager resourceManager, EntityManager entityManager, CostumeManager costumeManager, Session session, RmtStore store)
	{
		this.offer = o;
		this.SetVisibilityMode(false);
		Blueprint blueprint = EntityManager.GetBlueprint(this.offer.type, this.offer.identity, true);
		int currentLevel = resourceManager.Query(ResourceManager.LEVEL);
		Dictionary<string, SBGUIElement> child = this.CacheChildren();
		SBGUIMarketplaceSlot.GetChild getChild = (string x) => (!child.ContainsKey(x)) ? null : child[x];
		this.numberOwnedLabel = (SBGUILabel)getChild("owned_num_label");
		this.salePercentLabel = (SBGUILabel)getChild("sale_percent_text");
		RmtProduct prod = null;
		bool rmtDisabled = false;
		if (this.offer.type == "rmt")
		{
			if (!Soaring.IsOnline)
			{
				SoaringDebug.Log(string.Concat(new object[]
				{
					"Store Offline: Product Info Available: ",
					store.receivedProductInfo,
					" : Ready: ",
					store.RmtReady
				}), LogType.Error);
				if (store.receivedProductInfo)
				{
					SoaringInternal.instance.ClearOfflineMode();
				}
			}
			if (!Soaring.IsOnline)
			{
				rmtDisabled = true;
				this.isDisabled = true;
			}
			else
			{
				TFUtils.Assert(this.offer.innerOffer != null, "We should not have an RMT offer without an offer code: " + this.offer.itemName);
				if (store.RmtReady && !store.rmtProducts.TryGetValue(this.offer.innerOffer, out prod))
				{
					TFUtils.DebugLog("Got catalog entry " + this.offer.innerOffer + " without corresponding store entry");
				}
				if (prod == null)
				{
					rmtDisabled = true;
					this.isDisabled = true;
				}
			}
		}
		this.offerIcon = (SBGUIAtlasImage)getChild("icon");
		if (this.offer.width != 0 && this.offer.height != 0)
		{
			this.offerIcon.SetSizeNoRebuild(new Vector2((float)this.offer.width, (float)this.offer.height));
		}
		else if (this.showLevelLock != null)
		{
			this.offerIcon.SetSizeNoRebuild(new Vector2(110f, 110f));
		}
		else
		{
			this.offerIcon.SetSizeNoRebuild(new Vector2(150f, 150f));
		}
		if (!string.IsNullOrEmpty(this.offer.texture))
		{
			this.offerIcon.SetTextureFromAtlas(this.offer.texture);
		}
		else if (!string.IsNullOrEmpty(this.offer.material))
		{
			this.offerIcon.SetTextureFromMaterialPath(this.offer.material);
		}
		else if (blueprint != null)
		{
			this.offerIcon.SetTextureFromAtlas((string)blueprint.Invariable["portrait"], true, false, true, false, false, 0);
		}
		this.offerIcon.SetActive(true);
		yield return null;
		this.offerNameLabel = (SBGUILabel)getChild("name_label");
		string localizedNameLabel = null;
		if (blueprint != null)
		{
			localizedNameLabel = (string)blueprint.Invariable["name"];
			localizedNameLabel = Language.Get(localizedNameLabel);
			this.offerNameLabel.SetText(localizedNameLabel);
		}
		else
		{
			this.offerNameLabel.SetText(string.Format("offer {0}", this.offer.identity));
		}
		this.offerNameLabel.SetActive(true);
		yield return null;
		this.offerCostLabel = (SBGUILabel)getChild("price_label");
		this.offerCostIcon = (SBGUIAtlasImage)getChild("purchase_icon");
		this.button = (SBGUIPulseButton)base.FindChild("button");
		Color? color = this.buttonDefaultColor;
		if (color == null)
		{
			this.buttonDefaultColor = new Color?(this.button.GetComponent<YGSprite>().color);
		}
		if (this.offer.buttonTexture != null)
		{
			this.button.SetTextureFromAtlas(this.offer.buttonTexture, true, false, false, 0);
			this.button.SetColor(Color.white);
			this.button.InitializePulser(this.button.Size, this.button.Amplitude, this.button.Period);
			Vector3 newPos = Vector3.zero;
			if (this.offer.buttonTexture == "StorePurchaseButton_Halloween.png")
			{
				newPos = Vector3.zero;
			}
			this.button.tform.localPosition = newPos;
		}
		else
		{
			this.button.SetTextureFromAtlas("StorePurchaseButton.png", true, false, false, 0);
			SBGUIImage sbguiimage = this.button;
			Color? color2 = this.buttonDefaultColor;
			sbguiimage.SetColor(color2.Value);
			this.button.InitializePulser(this.button.Size, this.button.Amplitude, this.button.Period);
			this.button.tform.localPosition = Vector3.zero;
		}
		base.gameObject.transform.FindChild("sale_group").gameObject.SetActive(false);
		base.gameObject.transform.FindChild("new_item_group").gameObject.SetActive(false);
		base.gameObject.transform.FindChild("limited_item_group").gameObject.SetActive(false);
		if (!this.isDisabled)
		{
			base.gameObject.transform.FindChild("window_dimmer").gameObject.SetActiveRecursively(false);
			base.gameObject.transform.FindChild("lock_group").gameObject.SetActiveRecursively(false);
			base.gameObject.transform.FindChild("button_group").gameObject.SetActiveRecursively(true);
			base.gameObject.transform.FindChild("button_placed_group").gameObject.SetActiveRecursively(false);
			base.gameObject.transform.FindChild("button_disabled_group").gameObject.SetActiveRecursively(false);
			if (this.offer.isNewItem)
			{
				base.gameObject.transform.FindChild("new_item_group").gameObject.SetActive(true);
			}
			if (this.offer.isSaleItem)
			{
				base.gameObject.transform.FindChild("sale_group").gameObject.SetActive(true);
				if (this.offer.salePercent != 0f)
				{
					this.salePercentLabel.SetText((this.offer.salePercent * 100f).ToString() + " %");
				}
			}
			if (this.offer.isLimitedItem)
			{
				base.gameObject.transform.FindChild("limited_item_group").gameObject.SetActive(true);
			}
			if (prod != null)
			{
				string currencySymbol = "CurrencySymbol.png";
				string locale = Language.getDeviceLocale().ToLower();
				if (locale.Equals("gb"))
				{
					currencySymbol = "CurrencySymbol_Pound.png";
				}
				else if (locale.Equals("me"))
				{
					currencySymbol = "CurrencySymbol_Peso.png";
				}
				else if (locale.Equals("ru"))
				{
					currencySymbol = null;
				}
				if (currencySymbol != null)
				{
					this.offerCostIcon.SetTextureFromAtlas(currencySymbol, true, false, false, false, false, 0);
				}
				else
				{
					this.offerCostIcon.SetActive(false);
				}
				string localizedPrice = prod.localizedprice;
				if (localizedPrice.Contains("$"))
				{
					localizedPrice = localizedPrice.Split(new char[]
					{
						'$'
					}).Last<string>();
				}
				this.offerCostLabel.SetText(localizedPrice);
				this.CenterBuyButtonContents();
			}
			else if (this.offer.cost.ContainsKey(ResourceManager.SOFT_CURRENCY))
			{
				if (this.offer.cost[ResourceManager.SOFT_CURRENCY] == 0)
				{
					this.offerCostLabel.SetText(Language.Get("!!PREFAB_FREE"));
					this.offerCostIcon.SetActive(false);
				}
				else
				{
					this.offerCostLabel.SetText(this.offer.cost[ResourceManager.SOFT_CURRENCY].ToString());
					this.offerCostIcon.SetTextureFromAtlas(resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
					this.offerCostIcon.ScaleToMaxSize(32);
				}
				this.CenterBuyButtonContents();
			}
			else if (this.offer.cost.ContainsKey(ResourceManager.HARD_CURRENCY))
			{
				this.offerCostLabel.SetText(this.offer.cost[ResourceManager.HARD_CURRENCY].ToString());
				this.offerCostIcon.SetTextureFromAtlas(resourceManager.Resources[ResourceManager.HARD_CURRENCY].GetResourceTexture());
				this.offerCostIcon.ScaleToMaxSize(32);
				this.CenterBuyButtonContents();
			}
			else
			{
				TFUtils.Assert(this.offer.cost.Count == 1, string.Concat(new object[]
				{
					"Offers should always have a single resource cost, found offer id ",
					this.offer.identity,
					" with ",
					this.offer.cost.Count
				}));
				this.offerCostLabel.SetText(this.offer.cost[this.offer.cost.Keys.First<int>()].ToString());
				this.offerCostIcon.SetTextureFromAtlas(resourceManager.Resources[this.offer.cost.Keys.First<int>()].GetResourceTexture());
				this.offerCostIcon.ScaleToMaxSize(32);
				this.CenterBuyButtonContents();
			}
			base.EnableButtons(true);
			base.AttachActionToButton("button", delegate()
			{
				offerClickedEvent.FireEvent(this.offer);
			});
		}
		else
		{
			base.EnableButtons(false);
			base.gameObject.transform.FindChild("button_group").gameObject.SetActiveRecursively(false);
			if (this.showLevelLock != null)
			{
				base.gameObject.transform.FindChild("button_placed_group").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.FindChild("button_disabled_group").gameObject.SetActiveRecursively(true);
				base.gameObject.transform.FindChild("window_dimmer").gameObject.SetActiveRecursively(true);
				base.gameObject.transform.FindChild("lock_group").gameObject.SetActiveRecursively(true);
			}
			else if (rmtDisabled)
			{
				base.gameObject.transform.FindChild("button_placed_group").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.FindChild("button_disabled_group").gameObject.SetActiveRecursively(true);
				base.gameObject.transform.FindChild("window_dimmer").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.FindChild("lock_group").gameObject.SetActiveRecursively(false);
				base.EnableButtons(true);
				SBGUILabel label = (SBGUILabel)base.FindChild("button_label_disabled");
				if (store.rmtEnabled)
				{
					label.SetText(Language.Get("!!PREFAB_OFFLINE"));
					base.AttachActionToButton("button_disabled", delegate()
					{
						TFUtils.TriggerIAPOfflineWarning();
					});
				}
				else
				{
					base.AttachActionToButton("button_disabled", delegate()
					{
						TFUtils.TriggerIAPDisabledWarning();
					});
				}
			}
			else
			{
				base.gameObject.transform.FindChild("button_placed_group").gameObject.SetActiveRecursively(true);
				base.gameObject.transform.FindChild("button_disabled_group").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.FindChild("window_dimmer").gameObject.SetActiveRecursively(false);
				base.gameObject.transform.FindChild("lock_group").gameObject.SetActiveRecursively(false);
			}
		}
		yield return null;
		this.descriptionLabel = (SBGUILabel)getChild("description_label");
		this.productionInfo = getChild("makes_info");
		this.rewardMarker = getChild("makes_icon");
		this.productionTimeLabel = (SBGUILabel)getChild("makes_per_hour_label");
		this.ownedInfo = getChild("owned_info");
		this.numberOwnedLabel = (SBGUILabel)getChild("owned_num_label");
		SBGUILabel unlocksTextLabel = (SBGUILabel)getChild("unlocks_at_text");
		if (this.showLevelLock != null)
		{
			this.descriptionLabel.gameObject.SetActiveRecursively(false);
			this.productionInfo.gameObject.SetActiveRecursively(false);
			this.rewardMarker.gameObject.SetActiveRecursively(false);
			this.productionTimeLabel.gameObject.SetActiveRecursively(false);
			this.ownedInfo.gameObject.SetActiveRecursively(false);
			this.numberOwnedLabel.gameObject.SetActiveRecursively(false);
			unlocksTextLabel.gameObject.SetActiveRecursively(true);
			if (this.showLevelLock == -1)
			{
				unlocksTextLabel.SetText(Language.Get("!!PREFAB_UNLOCKED_AT_QUEST"));
			}
			else
			{
				unlocksTextLabel.SetText(string.Format(Language.Get("!!PREFAB_UNLOCKED_AT"), this.showLevelLock.ToString()));
			}
		}
		else
		{
			this.descriptionLabel.gameObject.SetActiveRecursively(true);
			this.productionInfo.gameObject.SetActiveRecursively(true);
			this.rewardMarker.gameObject.SetActiveRecursively(true);
			this.productionTimeLabel.gameObject.SetActiveRecursively(true);
			this.ownedInfo.gameObject.SetActiveRecursively(true);
			this.numberOwnedLabel.gameObject.SetActiveRecursively(true);
			unlocksTextLabel.gameObject.SetActiveRecursively(false);
			this.descriptionLabel.gameObject.SetActiveRecursively(true);
			string type = this.offer.type;
			switch (type)
			{
			case "building":
			case "annex":
			{
				if (blueprint.Invariable["product"] != null)
				{
					this.RemoveDescriptionInfo();
					RewardDefinition rewardDef = (RewardDefinition)blueprint.Invariable["product"];
					SBGUIRewardWidget.SetupRewardWidget(resourceManager, rewardDef.Summary, string.Empty, 2, this.rewardMarker, 10f, false, Color.white, true, 1f);
					SBGUIElement clock = getChild("clock_icon");
					clock.SetActive(true);
					ulong productionTime = (ulong)blueprint.Invariable["time.production"];
					this.productionTimeLabel.SetText(TFUtils.DurationToString(productionTime));
					this.productionTimeLabel.SetActive(true);
				}
				else
				{
					this.RemoveProductionInfo();
					TFUtils.Assert(this.offer.description != null, "Rentless building is missing a description: " + this.offer.itemName);
					this.descriptionLabel.SetText(this.offer.description);
				}
				int? instanceLimitByLevel = blueprint.GetInstanceLimitByLevel(currentLevel);
				if (instanceLimitByLevel != null && entityManager.GetEntityCount(EntityTypeNamingHelper.StringToType(this.offer.type), this.offer.identity) >= instanceLimitByLevel.Value)
				{
					this.isDisabled = true;
				}
				else
				{
					this.numberOwnedLabel.SetText(entityManager.GetEntityCount(EntityTypeNamingHelper.StringToType(o.type), o.identity).ToString());
				}
				this.numberOwnedLabel.SetActive(true);
				goto IL_1961;
			}
			case "rmt":
			case "path":
			case "expansion":
			case "resource":
				this.RemoveProductionInfo();
				this.RemoveOwnedInfo();
				this.offerNameLabel.SetText(this.offer.itemName);
				this.offerNameLabel.SetActive(true);
				this.descriptionLabel.SetText(this.offer.description);
				this.descriptionLabel.SetActive(true);
				goto IL_1961;
			case "costume":
			{
				this.RemoveProductionInfo();
				this.RemoveOwnedInfo();
				this.offerIcon.SetTextureFromAtlas(costumeManager.GetCostume(this.offer.identity).m_sPortrait, true, false, true, false, false, 0);
				this.offerNameLabel.SetText(Language.Get(costumeManager.GetCostume(this.offer.identity).m_sName));
				this.offerNameLabel.SetActive(true);
				this.descriptionLabel.SetText(this.offer.description);
				this.descriptionLabel.SetActive(true);
				if (costumeManager.IsCostumeUnlocked(this.offer.identity))
				{
					base.gameObject.transform.FindChild("button_placed_group").gameObject.SetActiveRecursively(true);
					base.gameObject.transform.FindChild("button_disabled_group").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.FindChild("window_dimmer").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.FindChild("lock_group").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.FindChild("button_group").gameObject.SetActiveRecursively(false);
				}
				else if (this.CheckCostumeUnlockCriteriaFullfilled(costumeManager.GetCostume(this.offer.identity), session) && !costumeManager.GetCostume(this.offer.identity).m_bLockedViaCSpanel)
				{
					base.gameObject.transform.FindChild("lock_group").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.FindChild("button_disabled_group").gameObject.SetActiveRecursively(false);
					base.gameObject.transform.FindChild("button_group").gameObject.SetActiveRecursively(true);
				}
				else
				{
					base.gameObject.transform.FindChild("lock_group").gameObject.SetActiveRecursively(true);
					base.gameObject.transform.FindChild("button_disabled_group").gameObject.SetActiveRecursively(true);
					base.gameObject.transform.FindChild("button_group").gameObject.SetActiveRecursively(false);
					unlocksTextLabel.SetText(Language.Get("!!PREFAB_UNLOCKED_AT_QUEST"));
					this.descriptionLabel.SetActive(false);
				}
				SBGUILabel buttonPlacedLabel = (SBGUILabel)getChild("button_label_placed");
				buttonPlacedLabel.SetText(Language.Get("!!PREFAB_ALREADY_OWNED"));
				goto IL_1961;
			}
			}
			TFUtils.Assert(false, "Unknown Offer Type " + this.offer.type);
		}
		IL_1961:
		this.SetVisibilityMode(true);
		base.FindChild("button").UpdateCollider();
		yield break;
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00022F14 File Offset: 0x00021114
	private bool CheckCostumeUnlockCriteriaFullfilled(CostumeManager.Costume costume, Session session)
	{
		bool result = false;
		if (costume.m_nUnlockLevel > 0)
		{
			result = (session.TheGame.resourceManager.PlayerLevelAmount >= costume.m_nUnlockLevel);
		}
		if (costume.m_nUnlockAssetDid > 0)
		{
			result = (session.TheGame.inventory.HasItem(new int?(costume.m_nUnlockAssetDid)) || session.TheGame.simulation.FindSimulated(new int?(costume.m_nUnlockAssetDid)) != null);
		}
		if (costume.m_nUnlockQuest1 > 0 || costume.m_nUnlockQuest2 > 0)
		{
			result = (session.TheGame.questManager.IsQuestCompleted((uint)costume.m_nUnlockQuest1) || session.TheGame.questManager.IsQuestCompleted((uint)costume.m_nUnlockQuest2));
		}
		return result;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00023000 File Offset: 0x00021200
	private void RemoveProductionInfo()
	{
		this.productionInfo.gameObject.SetActiveRecursively(false);
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00023014 File Offset: 0x00021214
	private void RemoveOwnedInfo()
	{
		this.ownedInfo.gameObject.SetActiveRecursively(false);
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x00023028 File Offset: 0x00021228
	private void RemoveDescriptionInfo()
	{
		this.descriptionLabel.gameObject.SetActiveRecursively(false);
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0002303C File Offset: 0x0002123C
	private void CenterBuyButtonContents()
	{
		this.offerCostLabel.tform.localPosition = new Vector3(0f, 0f, -0.1f);
		Vector3 localPosition = this.offerCostLabel.tform.localPosition;
		localPosition.x -= ((float)this.offerCostLabel.Width / 2f + 3f) * 0.01f;
		this.offerCostIcon.tform.localPosition = localPosition;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x000230BC File Offset: 0x000212BC
	public static string GetSessionActionId(SBMarketOffer offer)
	{
		return string.Format("Slot_{0}", offer.identity);
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x000230D4 File Offset: 0x000212D4
	public void SetVisibilityMode(bool viz)
	{
		foreach (SBGUIElement sbguielement in base.GetComponentsInChildren<SBGUIElement>(true))
		{
			if (sbguielement != this)
			{
				sbguielement.SetVisible(viz);
			}
		}
	}

	// Token: 0x04000420 RID: 1056
	private const int GAP_SIZE = 6;

	// Token: 0x04000421 RID: 1057
	private const int MAX_SLOT_ICON_SIZE = 150;

	// Token: 0x04000422 RID: 1058
	private const int MAX_LOCKED_SLOT_ICON_SIZE = 110;

	// Token: 0x04000423 RID: 1059
	private const int MAX_COST_ICON_SIZE = 32;

	// Token: 0x04000424 RID: 1060
	private const int MAX_REWARDS = 2;

	// Token: 0x04000425 RID: 1061
	private const int REWARD_GAP_SIZE = 10;

	// Token: 0x04000426 RID: 1062
	public SBGUIPulseButton button;

	// Token: 0x04000427 RID: 1063
	private Color? buttonDefaultColor;

	// Token: 0x04000428 RID: 1064
	protected SBGUIAtlasImage offerIcon;

	// Token: 0x04000429 RID: 1065
	protected SBGUILabel offerNameLabel;

	// Token: 0x0400042A RID: 1066
	protected SBGUILabel offerCostLabel;

	// Token: 0x0400042B RID: 1067
	protected SBGUIAtlasImage offerCostIcon;

	// Token: 0x0400042C RID: 1068
	protected SBGUIElement productionInfo;

	// Token: 0x0400042D RID: 1069
	protected SBGUILabel productionTimeLabel;

	// Token: 0x0400042E RID: 1070
	protected SBGUIElement rewardMarker;

	// Token: 0x0400042F RID: 1071
	protected SBGUIElement ownedInfo;

	// Token: 0x04000430 RID: 1072
	protected SBGUILabel numberOwnedLabel;

	// Token: 0x04000431 RID: 1073
	protected SBGUILabel descriptionLabel;

	// Token: 0x04000432 RID: 1074
	protected SBGUILabel salePercentLabel;

	// Token: 0x04000433 RID: 1075
	public bool isDisabled;

	// Token: 0x04000434 RID: 1076
	public int? showLevelLock;

	// Token: 0x02000495 RID: 1173
	// (Invoke) Token: 0x060024B7 RID: 9399
	private delegate SBGUIElement GetChild(string key);
}
