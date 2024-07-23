using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class SBGUIMarketplaceScreen : SBGUITabbedScrollableDialog
{
	// Token: 0x0600054B RID: 1355 RVA: 0x00021DD0 File Offset: 0x0001FFD0
	protected override SBGUIScrollListElement MakeSlot()
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.slotPrefab);
		gameObject.name = "MarketplaceSlot_" + this.slotNameCounter++;
		SBGUIMarketplaceSlot component = gameObject.GetComponent<SBGUIMarketplaceSlot>();
		component.SetVisibilityMode(false);
		return component;
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x00021E24 File Offset: 0x00020024
	public override void Start()
	{
		this.m_pStoreImpressions = new List<SBGUIMarketplaceScreen.StoreImpression>();
		this.region.ScrollStopEvent.AddListener(delegate
		{
			this.AddStoreImpression();
		});
		this.UpdateCallback.AddListener(new Action<SBGUIScreen, Session>(this.UpdateStoreImpressions));
		this.m_nStoreImpressionIndex = -1;
		base.Start();
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00021E7C File Offset: 0x0002007C
	private void UpdateStoreImpressions(SBGUIScreen screen, Session session)
	{
		if (this.m_bImpressionScheduled && this.AddStoreImpression())
		{
			this.m_bImpressionScheduled = false;
		}
		if (this.m_nStoreImpressionIndex < 0 || this.m_nStoreImpressionIndex >= this.m_pStoreImpressions.Count)
		{
			return;
		}
		this.m_pStoreImpressions[this.m_nStoreImpressionIndex].m_fTimeDelta += Time.deltaTime;
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00021EEC File Offset: 0x000200EC
	private void AddEmptyStoreImpression()
	{
		if (this.m_pStoreImpressions == null)
		{
			return;
		}
		this.m_pStoreImpressions.Add(new SBGUIMarketplaceScreen.StoreImpression());
		this.m_nStoreImpressionIndex++;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x00021F24 File Offset: 0x00020124
	private bool AddStoreImpression()
	{
		SBGUIMarketplaceScreen.StoreImpression storeImpression = new SBGUIMarketplaceScreen.StoreImpression();
		List<SBGUIScrollListElement> visibleSrollListElements = base.GetVisibleSrollListElements();
		if (visibleSrollListElements == null)
		{
			return false;
		}
		int count = visibleSrollListElements.Count;
		for (int i = 0; i < count; i++)
		{
			if (visibleSrollListElements[i] != null && visibleSrollListElements[i] is SBGUIMarketplaceSlot)
			{
				SBGUIMarketplaceSlot sbguimarketplaceSlot = visibleSrollListElements[i] as SBGUIMarketplaceSlot;
				if (sbguimarketplaceSlot.offer != null)
				{
					storeImpression.m_pOffers.Add(sbguimarketplaceSlot.offer);
				}
			}
		}
		if (storeImpression.m_pOffers.Count > 0)
		{
			this.m_pStoreImpressions.Add(storeImpression);
			this.m_nStoreImpressionIndex++;
		}
		return true;
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x00021FE0 File Offset: 0x000201E0
	private void FlushStoreImpressions()
	{
		if (this.m_pStoreImpressions == null)
		{
			return;
		}
		if (this.m_bImpressionScheduled)
		{
			this.AddStoreImpression();
			this.m_bImpressionScheduled = false;
		}
		AnalyticsWrapper.LogStoreImpressions(this.session.TheGame, this.m_pStoreImpressions);
		this.m_pStoreImpressions = new List<SBGUIMarketplaceScreen.StoreImpression>();
		this.m_nStoreImpressionIndex = -1;
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000551 RID: 1361 RVA: 0x0002203C File Offset: 0x0002023C
	public override Bounds TotalBounds
	{
		get
		{
			return base.FindChild("window").TotalBounds;
		}
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x00022050 File Offset: 0x00020250
	public float GetMainWindowZ()
	{
		return base.FindChild("window").transform.localPosition.z;
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0002207C File Offset: 0x0002027C
	protected override void LoadCategories(Session session)
	{
		this.categories = new Dictionary<string, SBTabCategory>();
		this.offers = new Dictionary<int, SBMarketOffer>();
		Catalog catalog = session.TheGame.catalog;
		foreach (object obj in ((List<object>)catalog.CatalogDict["market"]))
		{
			SBMarketCategory sbmarketCategory = new SBMarketCategory((Dictionary<string, object>)obj);
			this.categories[sbmarketCategory.Name] = sbmarketCategory;
		}
		foreach (object obj2 in ((List<object>)catalog.CatalogDict["offers"]))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj2;
			if (!dictionary.ContainsKey("show_in_store") || (bool)dictionary["show_in_store"])
			{
				SBMarketOffer sbmarketOffer = new SBMarketOffer((Dictionary<string, object>)obj2);
				this.offers[sbmarketOffer.identity] = sbmarketOffer;
			}
		}
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x000221E4 File Offset: 0x000203E4
	public override void SetManagers(Session session)
	{
		base.SetManagers(session);
		this.infoWindow = (SBGUIAtlasImage)base.FindChild("info_window");
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("info_message");
		if (sbguilabel != null)
		{
			sbguilabel.SetText(TFUtils.AssignStorePlatformText("!!MARKETLABEL_JJ_WARNING"));
		}
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0002223C File Offset: 0x0002043C
	public void LocalizeInitialLabel()
	{
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("name_label");
		sbguilabel.SetText(Language.Get(sbguilabel.Text));
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0002226C File Offset: 0x0002046C
	protected override Vector2 GetSlotSize()
	{
		YGSprite component = this.slotPrefab.GetComponent<YGSprite>();
		return component.size * 0.01f;
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x00022298 File Offset: 0x00020498
	protected override void BuildTabForButton(SBGUITabButton tab)
	{
		object obj = this.session.CheckAsyncRequest("target_store_tab");
		if (obj != null)
		{
			string tabName = (string)obj;
			base.ViewTab(tabName);
			return;
		}
		base.BuildTabForButton(tab);
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x000222D4 File Offset: 0x000204D4
	protected override IEnumerator BuildTabCoroutine(string tabName)
	{
		if (!this.categories.ContainsKey(tabName))
		{
			TFUtils.WarningLog(string.Format("Category {0} not found in catalog", tabName));
		}
		else
		{
			if (tabName == "rmt")
			{
				this.infoWindow.SetActive(true);
			}
			else
			{
				this.infoWindow.SetActive(false);
			}
			base.ClearCachedSlotInfos();
			this.region.ClearSlotActions();
			if (!this.tabContents.TryGetValue(tabName, out this.currentTab))
			{
				SBGUIElement anchor = SBGUIElement.Create(this.region.Marker);
				anchor.name = string.Format(tabName, new object[0]);
				anchor.transform.localPosition = Vector3.zero;
				this.tabContents[tabName] = anchor;
				this.currentTab = anchor;
			}
			yield return null;
			SBTabCategory category = this.categories[tabName];
			this.LoadSlotInfo(category, this.currentTab);
			this.m_bImpressionScheduled = true;
			AnalyticsWrapper.LogShopTabOpened(this.session.TheGame, Catalog.ConvertTypeToDeltaDNAType(tabName));
		}
		yield break;
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x00022300 File Offset: 0x00020500
	private void LoadSlotInfo(SBTabCategory tabCategory, SBGUIElement anchor)
	{
		SBMarketCategory sbmarketCategory = (SBMarketCategory)tabCategory;
		SBMarketOffer sbmarketOffer = null;
		int num = 0;
		int num2 = this.resourceMgr.Query(ResourceManager.LEVEL);
		if (sbmarketCategory.Type == "rmt")
		{
			int num3 = 0;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Dictionary<string, SoaringPurchasable> soaringProducts = this.session.TheGame.store.soaringProducts;
			int num4 = -100;
			foreach (KeyValuePair<string, SoaringPurchasable> keyValuePair in soaringProducts)
			{
				SoaringPurchasable soaringPurchasable = soaringProducts[keyValuePair.Key];
				dictionary.Clear();
				string texture = soaringPurchasable.Texture;
				if (texture == null)
				{
					goto IL_200;
				}
				if (SBGUIMarketplaceScreen.<>f__switch$map4 == null)
				{
					SBGUIMarketplaceScreen.<>f__switch$map4 = new Dictionary<string, int>(10)
					{
						{
							"JellyfishJellyShortStack_Icon.png",
							0
						},
						{
							"JellyfishJellyCrateTower_Icon.png",
							1
						},
						{
							"JellyfishJellyCrateTowers_Icon.png",
							2
						},
						{
							"JellyfishJellyCratePyramid_Icon.png",
							3
						},
						{
							"JellyfishJellyBoat_Icon.png",
							4
						},
						{
							"JellyfishJellyTower_Icon.png",
							5
						},
						{
							"CoinStack_Portrait.png",
							6
						},
						{
							"CoinBarrel_Portrait.png",
							7
						},
						{
							"CoinChest_Portrait.png",
							8
						},
						{
							"CoinWheelbarrel_Portrait.png",
							9
						}
					};
				}
				int resourceType;
				if (!SBGUIMarketplaceScreen.<>f__switch$map4.TryGetValue(texture, out resourceType))
				{
					goto IL_200;
				}
				int num5;
				int num6;
				switch (resourceType)
				{
				case 0:
					num5 = 108;
					num6 = 120;
					break;
				case 1:
					num5 = 56;
					num6 = 122;
					break;
				case 2:
					num5 = 116;
					num6 = 94;
					break;
				case 3:
					num5 = 118;
					num6 = 112;
					break;
				case 4:
					num5 = 124;
					num6 = 62;
					break;
				case 5:
					num5 = 94;
					num6 = 120;
					break;
				case 6:
					num5 = 128;
					num6 = 128;
					break;
				case 7:
					num5 = 94;
					num6 = 124;
					break;
				case 8:
					num5 = 122;
					num6 = 118;
					break;
				case 9:
					num5 = 126;
					num6 = 118;
					break;
				default:
					goto IL_200;
				}
				IL_20D:
				dictionary.Add("identity", num4 - num3);
				dictionary.Add("name", soaringPurchasable.DisplayName);
				dictionary.Add("description", soaringPurchasable.Description);
				dictionary.Add("result_type", "resource");
				dictionary.Add("cost", new Dictionary<string, object>());
				Dictionary<string, object> dictionary2 = dictionary;
				string key = "data";
				Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
				Dictionary<string, object> dictionary4 = dictionary3;
				resourceType = soaringPurchasable.ResourceType;
				dictionary4.Add(resourceType.ToString(), soaringPurchasable.Amount);
				dictionary2.Add(key, dictionary3);
				dictionary.Add("code", soaringPurchasable.ProductID);
				dictionary.Add("display", new Dictionary<string, object>
				{
					{
						"model_type",
						"sprite"
					},
					{
						"width",
						num5
					},
					{
						"height",
						num6
					}
				});
				dictionary.Add("display.default", new Dictionary<string, object>
				{
					{
						"texture",
						soaringPurchasable.Texture
					},
					{
						"name",
						"default"
					}
				});
				sbmarketOffer = new SBMarketOffer(dictionary);
				if (sbmarketOffer.type == null)
				{
					sbmarketOffer.type = sbmarketCategory.Type;
				}
				this.region.SetupSlotActions.Insert(num3, this.SetupSlotClosure(anchor, sbmarketOffer, this.OfferClickedEvent, this.GetSlotOffset(num3), false, null, this.resourceMgr, this.entityMgr, this.costumeMgr, this.session, this.session.TheGame.store));
				string sessionActionId = SBGUIMarketplaceSlot.GetSessionActionId(sbmarketOffer);
				if (this.sessionActionIdSearchRequests.Contains(sessionActionId))
				{
					this.sessionActionSlotMap[sessionActionId] = num3;
				}
				num3++;
				continue;
				IL_200:
				num5 = 124;
				num6 = 124;
				goto IL_20D;
			}
			if (num3 > 0)
			{
				base.PostLoadRegionContentInfo(this.region.SetupSlotActions.Count);
				return;
			}
		}
		int num7 = -1;
		float num8 = 0f;
		object obj = this.session.CheckAsyncRequest("target_store_did");
		if (obj != null)
		{
			num7 = (int)obj;
		}
		int num9 = 0;
		foreach (int num10 in sbmarketCategory.Dids)
		{
			if (this.offers.TryGetValue(num10, out sbmarketOffer))
			{
				if (sbmarketOffer.microEventDID >= 0)
				{
					MicroEvent microEvent = this.session.TheGame.microEventManager.GetMicroEvent(sbmarketOffer.microEventDID);
					if (microEvent == null || !microEvent.IsActive() || (sbmarketOffer.microEventOnly && microEvent.IsCompleted()))
					{
						goto IL_7FD;
					}
				}
				if (sbmarketOffer.type == null)
				{
					sbmarketOffer.type = sbmarketCategory.Type;
				}
				if (num10 == -255)
				{
					sbmarketOffer.type = "path";
				}
				int? minLevelToShow = null;
				bool flag = false;
				Blueprint blueprint = EntityManager.GetBlueprint(sbmarketOffer.type, sbmarketOffer.identity, true);
				if (blueprint != null)
				{
					int num11 = (int)blueprint.Invariable["level.minimum"];
					if (this.session.TheGame.buildingUnlockManager.CheckBuildingUnlock(sbmarketOffer.identity))
					{
						flag = false;
						sbmarketOffer.itemLocked = false;
						if (blueprint.GetInstanceLimitByLevel(num2) != null)
						{
							int entityCount = this.entityMgr.GetEntityCount(EntityTypeNamingHelper.StringToType(sbmarketOffer.type), sbmarketOffer.identity);
							int? instanceLimitByLevel = blueprint.GetInstanceLimitByLevel(num2);
							if (instanceLimitByLevel != null && instanceLimitByLevel.Value <= entityCount)
							{
								flag = true;
								sbmarketOffer.itemLocked = true;
							}
						}
					}
					else if (num11 > num2 || num11 == -1)
					{
						flag = true;
						sbmarketOffer.itemLocked = true;
						minLevelToShow = new int?(num11);
					}
					else if (blueprint.GetInstanceLimitByLevel(num2) != null)
					{
						int entityCount2 = this.entityMgr.GetEntityCount(EntityTypeNamingHelper.StringToType(sbmarketOffer.type), sbmarketOffer.identity);
						int? instanceLimitByLevel2 = blueprint.GetInstanceLimitByLevel(num2);
						if (instanceLimitByLevel2 != null && instanceLimitByLevel2.Value <= entityCount2)
						{
							flag = true;
							sbmarketOffer.itemLocked = true;
						}
					}
				}
				if (blueprint == null)
				{
					goto IL_741;
				}
				int? instanceLimitByLevel3 = blueprint.GetInstanceLimitByLevel(num2);
				if (instanceLimitByLevel3 == null || this.entityMgr.GetEntityCount(EntityTypeNamingHelper.StringToType(sbmarketOffer.type), sbmarketOffer.identity) < instanceLimitByLevel3.Value || !flag)
				{
					goto IL_741;
				}
				this.region.SetupSlotActions.Insert(num, this.SetupSlotClosure(anchor, sbmarketOffer, this.OfferClickedEvent, this.GetSlotOffset(num), flag, minLevelToShow, this.resourceMgr, this.entityMgr, this.costumeMgr, this.session, this.session.TheGame.store));
				num++;
				IL_7A3:
				string sessionActionId2 = SBGUIMarketplaceSlot.GetSessionActionId(sbmarketOffer);
				if (this.sessionActionIdSearchRequests.Contains(sessionActionId2))
				{
					this.sessionActionSlotMap[sessionActionId2] = num;
				}
				if (num7 == -1 || num10 != num7)
				{
					goto IL_7FD;
				}
				num8 = 0f;
				if (num != 0)
				{
					num8 = this.GetSlotOffset(num - 1).x;
					goto IL_7FD;
				}
				goto IL_7FD;
				IL_741:
				num9++;
				this.region.SetupSlotActions.Insert(num9 - 1, this.SetupSlotClosure(anchor, sbmarketOffer, this.OfferClickedEvent, this.GetSlotOffset(num9 - 1), flag, minLevelToShow, this.resourceMgr, this.entityMgr, this.costumeMgr, this.session, this.session.TheGame.store));
				num++;
				goto IL_7A3;
			}
			TFUtils.WarningLog(string.Format("Offer [{0}] not found", sbmarketCategory.Dids[num]));
			IL_7FD:;
		}
		this.region.ResetScroll();
		if (num8 != 0f)
		{
			num8 = this.region.MinScroll.x - num8;
			base.PostLoadRegionContentInfo(this.region.SetupSlotActions.Count, new Vector3(num8, this.region.MinScroll.y, this.region.MinScroll.z));
		}
		else
		{
			base.PostLoadRegionContentInfo(this.region.SetupSlotActions.Count);
		}
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x00022BC8 File Offset: 0x00020DC8
	private Action<SBGUIScrollListElement> SetupSlotClosure(SBGUIElement anchor, SBMarketOffer offer, EventDispatcher<SBMarketOffer> OfferClickedEvent, Vector2 offset, bool isDisabled, int? minLevelToShow, ResourceManager resourceMgr, EntityManager entityMgr, CostumeManager costumeMgr, Session session, RmtStore store)
	{
		return delegate(SBGUIScrollListElement slot)
		{
			((SBGUIMarketplaceSlot)slot).Setup(anchor, offer, OfferClickedEvent, offset, isDisabled, minLevelToShow, resourceMgr, entityMgr, costumeMgr, session, session.TheGame.store);
		};
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00022C34 File Offset: 0x00020E34
	protected override Rect CalculateTabContentsSize(string tabName)
	{
		SBMarketCategory sbmarketCategory = (SBMarketCategory)this.categories[tabName];
		return base.CalculateScrollRegionSize(sbmarketCategory.Dids.Length);
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x00022C64 File Offset: 0x00020E64
	public override void Deactivate()
	{
		SBGUIRewardWidget.ClearWidgetPool();
		this.OfferClickedEvent.ClearListeners();
		this.FlushStoreImpressions();
		this.m_bImpressionScheduled = false;
		this.session.marketpalceActive = false;
		base.Deactivate();
	}

	// Token: 0x04000414 RID: 1044
	private const int START_SLOTPOOL_SIZE = 6;

	// Token: 0x04000415 RID: 1045
	public GameObject slotPrefab;

	// Token: 0x04000416 RID: 1046
	public EventDispatcher<SBMarketOffer> OfferClickedEvent = new EventDispatcher<SBMarketOffer>();

	// Token: 0x04000417 RID: 1047
	private List<SBGUIMarketplaceScreen.StoreImpression> m_pStoreImpressions;

	// Token: 0x04000418 RID: 1048
	private bool m_bImpressionScheduled;

	// Token: 0x04000419 RID: 1049
	private int m_nStoreImpressionIndex = -1;

	// Token: 0x0400041A RID: 1050
	private Dictionary<int, SBMarketOffer> offers;

	// Token: 0x0400041B RID: 1051
	private SBGUIAtlasImage infoWindow;

	// Token: 0x0400041C RID: 1052
	private int slotNameCounter;

	// Token: 0x0200008D RID: 141
	public class StoreImpression
	{
		// Token: 0x0600055E RID: 1374 RVA: 0x00022CA4 File Offset: 0x00020EA4
		public StoreImpression()
		{
			this.m_pOffers = new List<SBMarketOffer>();
			this.m_fTimeDelta = 0f;
		}

		// Token: 0x0400041E RID: 1054
		public List<SBMarketOffer> m_pOffers;

		// Token: 0x0400041F RID: 1055
		public float m_fTimeDelta;
	}
}
