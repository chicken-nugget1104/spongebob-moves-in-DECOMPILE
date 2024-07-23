using System;
using System.Collections.Generic;

// Token: 0x02000336 RID: 822
public static class SBMIAnalytics
{
	// Token: 0x060017CE RID: 6094 RVA: 0x0009D418 File Offset: 0x0009B618
	private static SoaringDictionary GetDictFromCommonData(SBMIAnalytics.CommonData pCommonData)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(pCommonData.ulDateTime, "event_time");
		soaringDictionary.addValue(pCommonData.ulFirstPlayTime, "first_play_time");
		soaringDictionary.addValue(pCommonData.nPlayerLevel, "player_level");
		soaringDictionary.addValue(pCommonData.nSoftCurrency, "player_coin_balance");
		soaringDictionary.addValue(pCommonData.nHardCurreny, "player_jelly_balance");
		soaringDictionary.addValue(pCommonData.nCharacters, "count_of_characters");
		soaringDictionary.addValue(pCommonData.nHouses, "count_of_houses");
		soaringDictionary.addValue(pCommonData.nLandExpansions, "count_of_land_expansions");
		soaringDictionary.addValue((!pCommonData.bIsEligibleForSpongyGames) ? 0 : 1, "is_eligible_for_spongy_games");
		soaringDictionary.addValue(pCommonData.nSpongyCurrency, "doorknob_balance");
		soaringDictionary.addValue(pCommonData.sPlayerID, "player_id");
		soaringDictionary.addValue(pCommonData.sPlatform, "platform");
		soaringDictionary.addValue(pCommonData.sDeviceName, "device_name");
		soaringDictionary.addValue(pCommonData.sBinaryVersion, "binary_version");
		soaringDictionary.addValue(pCommonData.sOSVersion, "os_version");
		soaringDictionary.addValue(pCommonData.sManifest, "manifest");
		soaringDictionary.addValue(pCommonData.sGUID, "guid");
		soaringDictionary.addValue(pCommonData.sDeviceGUID, "device_seq_id");
		soaringDictionary.addValue(pCommonData.ulSequence, "device_seq_num");
		if (pCommonData.sCampaignData != null)
		{
			soaringDictionary.addValue(pCommonData.sCampaignData, "player_ab_test");
		}
		return soaringDictionary;
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x0009D614 File Offset: 0x0009B814
	public static void LogQuestCompleted(Game pGame, SBMIAnalytics.QuestObject pQuest, SBMIAnalytics.RewardObject pReward)
	{
		string text = "quest_completed";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		pQuest.AddToDict(soaringDictionary2, "quest", true);
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x0009D684 File Offset: 0x0009B884
	public static void LogAutoQuestStarted(Game pGame, SBMIAnalytics.AutoQuestObject pAutoQuest)
	{
		string text = "autoquest_started";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		pAutoQuest.AddToDict(soaringDictionary2, "autoquest", true);
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x0009D6E4 File Offset: 0x0009B8E4
	public static void LogAutoQuestCompleted(Game pGame, SBMIAnalytics.AutoQuestObject pAutoQuest, SBMIAnalytics.RewardObject pReward)
	{
		string text = "autoquest_completed";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		pAutoQuest.AddToDict(soaringDictionary2, "autoquest", true);
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x0009D754 File Offset: 0x0009B954
	public static void LogTaskStarted(Game pGame, SBMIAnalytics.TaskObject pTask, SBMIAnalytics.CharacterObject pCharacter, SBMIAnalytics.CostumeObject pCostume)
	{
		string text = "character_task_started";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		pTask.AddToDict(soaringDictionary2, "task", true);
		pCharacter.AddToDict(soaringDictionary2, "character", true);
		if (pCostume != null)
		{
			pCostume.AddToDict(soaringDictionary2, "costume", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x0009D7D4 File Offset: 0x0009B9D4
	public static void LogTaskCompleted(Game pGame, SBMIAnalytics.TaskObject pTask, SBMIAnalytics.CharacterObject pCharacter, SBMIAnalytics.CostumeObject pCostume, SBMIAnalytics.RewardObject pReward)
	{
		string text = "character_task_completed";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		pTask.AddToDict(soaringDictionary2, "task", true);
		pCharacter.AddToDict(soaringDictionary2, "character", true);
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward", true);
		}
		if (pCostume != null)
		{
			pCostume.AddToDict(soaringDictionary2, "costume", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x0009D868 File Offset: 0x0009BA68
	public static void LogCostumeUnlocked(Game pGame, SBMIAnalytics.CostumeObject pCostume)
	{
		string text = "costume_unlock";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		if (pCostume != null)
		{
			pCostume.AddToDict(soaringDictionary2, "costume", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x0009D8CC File Offset: 0x0009BACC
	public static void LogCostumeChanged(Game pGame, SBMIAnalytics.CharacterObject pCharacter, SBMIAnalytics.CostumeObject pCostumeOld, SBMIAnalytics.CostumeObject pCostumeNew)
	{
		string text = "change_costume";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		pCharacter.AddToDict(soaringDictionary2, "character", true);
		if (pCostumeOld != null)
		{
			pCostumeOld.AddToDict(soaringDictionary2, "costume_old", true);
		}
		if (pCostumeNew != null)
		{
			pCostumeNew.AddToDict(soaringDictionary2, "costume_new", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x0009D950 File Offset: 0x0009BB50
	public static void LogDailyReward(Game pGame, int nDay, SBMIAnalytics.RewardObject pReward)
	{
		string text = "daily_reward";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		soaringDictionary2.addValue(nDay, "day_number");
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x0009D9C4 File Offset: 0x0009BBC4
	public static void LogChestPickup(Game pGame, SBMIAnalytics.ChestObject pChest, SBMIAnalytics.RewardObject pReward)
	{
		string text = "chest_pickup";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		pChest.AddToDict(soaringDictionary2, "chest", true);
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x0009DA34 File Offset: 0x0009BC34
	public static void LogCharacterFeed(Game pGame, SBMIAnalytics.CharacterObject pCharacter, SBMIAnalytics.CostumeObject pCostume, SBMIAnalytics.ItemObject pItem, SBMIAnalytics.RewardObject pReward)
	{
		string text = "character_feed";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		pCharacter.AddToDict(soaringDictionary2, "character", true);
		pItem.AddToDict(soaringDictionary2, "item", true);
		if (pCostume != null)
		{
			pCostume.AddToDict(soaringDictionary2, "costume", true);
		}
		if (pReward != null)
		{
			pReward.AddToDict(soaringDictionary2, "reward", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x0009DAC8 File Offset: 0x0009BCC8
	public static void LogVisitPark(Game pGame)
	{
		string text = "visit_park";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x0009DB18 File Offset: 0x0009BD18
	public static void LogSessionBegin(Game pGame)
	{
		string text = "session_begin";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x0009DB68 File Offset: 0x0009BD68
	public static void LogPurchaseComplete(Game pGame, SBMIAnalytics.IAPObject pIAP)
	{
		string text = "purchase_complete";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		if (pIAP != null)
		{
			pIAP.AddToDict(soaringDictionary2, "iap", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x0009DBCC File Offset: 0x0009BDCC
	public static void LogLevelUp(Game pGame)
	{
		string text = "level_up";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x0009DC1C File Offset: 0x0009BE1C
	public static void LogItemPlacement(Game pGame, SBMIAnalytics.ItemObject pItem, bool bFromInventory, string sAction)
	{
		string text = "item_placement";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, SBMIAnalytics._nTRACKING_VERSION_LOG_ITEM_PLACEMENT).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		if (pItem != null)
		{
			pItem.AddToDict(soaringDictionary2, "item", true);
		}
		soaringDictionary2.addValue((!bFromInventory) ? 0 : 1, "from_inventory");
		soaringDictionary2.addValue((!bFromInventory) ? 1 : 0, "from_marketplace");
		if (!string.IsNullOrEmpty(sAction))
		{
			soaringDictionary2.addValue(sAction, "action");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x0009DCDC File Offset: 0x0009BEDC
	public static void LogJellyConfirmation(Game pGame, SBMIAnalytics.ItemObject pItem, string sTriggerEventType, string sSpeedupType, string sAction)
	{
		string text = "jelly_confirmation";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		if (pItem != null)
		{
			pItem.AddToDict(soaringDictionary2, "item", true);
		}
		if (!string.IsNullOrEmpty(sTriggerEventType))
		{
			soaringDictionary2.addValue(sTriggerEventType, "trigger_event_type");
		}
		if (!string.IsNullOrEmpty(sSpeedupType))
		{
			soaringDictionary2.addValue(sSpeedupType, "speedup_type");
		}
		if (!string.IsNullOrEmpty(sAction))
		{
			soaringDictionary2.addValue(sAction, "action");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x0009DD98 File Offset: 0x0009BF98
	public static void LogRecievedEventItem(Game pGame, SBMIAnalytics.ItemObject pItem)
	{
		string text = "received_event_item";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		if (pItem != null)
		{
			pItem.AddToDict(soaringDictionary2, "item", true);
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x0009DDFC File Offset: 0x0009BFFC
	public static void LogCraftCollected(Game pGame, SBMIAnalytics.ItemObject pItemOld, SBMIAnalytics.ItemObject pItemNew, int nItemCount)
	{
		string text = "item_craf_collected";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		if (pItemOld != null)
		{
			pItemOld.AddToDict(soaringDictionary2, "item_parent", true);
		}
		if (pItemNew != null)
		{
			pItemNew.AddToDict(soaringDictionary2, "item_child", true);
		}
		soaringDictionary2.addValue(nItemCount, "item_count");
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x0009DE84 File Offset: 0x0009C084
	public static void LogStoreImpressions(Game pGame, List<SBGUIMarketplaceScreen.StoreImpression> pStoreImpressions)
	{
		int count = pStoreImpressions.Count;
		if (count <= 0)
		{
			return;
		}
		SoaringDictionary dictFromCommonData = SBMIAnalytics.GetDictFromCommonData(pGame.GetAnalyticsCommonData());
		SoaringArray soaringArray = new SoaringArray();
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			SBGUIMarketplaceScreen.StoreImpression storeImpression = pStoreImpressions[i];
			int count2 = storeImpression.m_pOffers.Count;
			for (int j = 0; j < count2; j++)
			{
				SBMarketOffer sbmarketOffer = storeImpression.m_pOffers[j];
				int b = (!sbmarketOffer.cost.ContainsKey(ResourceManager.HARD_CURRENCY)) ? 0 : sbmarketOffer.cost[ResourceManager.HARD_CURRENCY];
				int b2 = (!sbmarketOffer.cost.ContainsKey(ResourceManager.SOFT_CURRENCY)) ? 0 : sbmarketOffer.cost[ResourceManager.SOFT_CURRENCY];
				string text = null;
				if (sbmarketOffer.type == "rmt" || sbmarketOffer.type == "path")
				{
					text = sbmarketOffer.innerOffer;
				}
				else
				{
					Blueprint blueprint = EntityManager.GetBlueprint(sbmarketOffer.type, sbmarketOffer.identity, false);
					if (blueprint != null)
					{
						text = (string)blueprint.Invariable["name"];
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					SoaringDictionary soaringDictionary = new SoaringDictionary();
					soaringDictionary.addValue(sbmarketOffer.identity, "item_did");
					soaringDictionary.addValue(text, "item_name");
					soaringDictionary.addValue(storeImpression.m_fTimeDelta, "time_seconds");
					soaringDictionary.addValue(num, "group_id");
					soaringDictionary.addValue((!sbmarketOffer.itemLocked) ? 0 : 1, "item_locked");
					if (sbmarketOffer.itemLocked)
					{
						TFUtils.ErrorLog("\nitem is locked: " + sbmarketOffer.identity);
					}
					if (sbmarketOffer.type == "rmt")
					{
						if (pGame.store.soaringProducts.ContainsKey(text))
						{
							soaringDictionary.addValue(pGame.store.soaringProducts[text].USDPrice, "product_usd_cost");
						}
					}
					else
					{
						soaringDictionary.addValue(b, "jelly_cost");
						soaringDictionary.addValue(b2, "gold_cost");
					}
					soaringArray.addObject(soaringDictionary);
				}
			}
			if (count2 > 0)
			{
				num++;
			}
		}
		if (soaringArray.count() <= 0)
		{
			return;
		}
		dictFromCommonData.addValue(soaringArray, "impressions");
		Soaring.SaveStat("store_item_impressions", dictFromCommonData);
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x0009E148 File Offset: 0x0009C348
	public static void LogMarketplaceUI(Game pGame, string sAction, string sOpenType, string sLeaveType)
	{
		string text = "store_event";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		pGame.GetAnalyticsMetaObject(text, -1).AddToDict(soaringDictionary, null, false);
		pGame.GetAnalyticsPlayerObject().AddToDict(soaringDictionary2, null, true);
		if (!string.IsNullOrEmpty(sAction))
		{
			soaringDictionary.addValue(sAction, "action");
		}
		if (!string.IsNullOrEmpty(sOpenType))
		{
			soaringDictionary.addValue(sOpenType, "open_type");
		}
		if (!string.IsNullOrEmpty(sLeaveType))
		{
			soaringDictionary.addValue(sLeaveType, "leave_type");
		}
		soaringDictionary.addValue(soaringDictionary2, "data");
		Soaring.SaveStat(text, soaringDictionary);
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x0009E1EC File Offset: 0x0009C3EC
	public static void LogEventButtonClick(Game pGame)
	{
		SoaringDictionary dictFromCommonData = SBMIAnalytics.GetDictFromCommonData(pGame.GetAnalyticsCommonData());
		Soaring.SaveStat("event_button_click", dictFromCommonData);
	}

	// Token: 0x04000FBE RID: 4030
	public const string _sSPEEDUP = "speedup";

	// Token: 0x04000FBF RID: 4031
	public const string _sUNLOCK = "unlock";

	// Token: 0x04000FC0 RID: 4032
	public const string _sINSTANT_PURCHASE = "instant_purchase";

	// Token: 0x04000FC1 RID: 4033
	public const string _sSTORE_RESTOCKING = "store_restocking";

	// Token: 0x04000FC2 RID: 4034
	public const string _sCONSTRUCTION = "construction";

	// Token: 0x04000FC3 RID: 4035
	public const string _sFULLNESS = "fullness";

	// Token: 0x04000FC4 RID: 4036
	public const string _sCOMMUNITY_EVENT_PURCHASE = "community_event_purchase";

	// Token: 0x04000FC5 RID: 4037
	public const string _sRENT = "rent";

	// Token: 0x04000FC6 RID: 4038
	public const string _sCRAFT = "craft";

	// Token: 0x04000FC7 RID: 4039
	public const string _sOPEN = "open";

	// Token: 0x04000FC8 RID: 4040
	public const string _sCLOSE = "close";

	// Token: 0x04000FC9 RID: 4041
	public const string _sSTORE_BUTTON = "store_open_button";

	// Token: 0x04000FCA RID: 4042
	public const string _sOPEN_IAP_TAB_SOFT = "store_open_plus_buy_gold";

	// Token: 0x04000FCB RID: 4043
	public const string _sOPEN_IAP_TAB_HARD = "store_open_plus_buy_jelly";

	// Token: 0x04000FCC RID: 4044
	public const string _sOPEN_IAP_TAB_REDIRECT = "store_open_need_currency_redirect";

	// Token: 0x04000FCD RID: 4045
	public const string _sIAP_ERROR_DIALOG_OPEN = "store_open_iap_error_return";

	// Token: 0x04000FCE RID: 4046
	public const string _sCANCEL_PURCHASE = "store_open_too_poor_return";

	// Token: 0x04000FCF RID: 4047
	public const string _sNOT_ENOUGH_CURRENCY_OPEN = "store_open_too_poor_return";

	// Token: 0x04000FD0 RID: 4048
	public const string _sBACK_BUTTON = "store_close_back_button";

	// Token: 0x04000FD1 RID: 4049
	public const string _sIAP_ERROR = "store_close_unknown_error_iap";

	// Token: 0x04000FD2 RID: 4050
	public const string _sIAP_ERROR_DIALOG_CLOSE = "store_close_known_error_iap";

	// Token: 0x04000FD3 RID: 4051
	public const string _sNOT_ENOUGH_CURRENCY_CLOSE = "store_close_im_broke";

	// Token: 0x04000FD4 RID: 4052
	public const string _sPURCHASE_IAP_CLOSE = "store_close_purchase_iap";

	// Token: 0x04000FD5 RID: 4053
	public const string _sPAVING = "store_close_road_purchase_start";

	// Token: 0x04000FD6 RID: 4054
	public const string _sITEM_PURCHASE_START = "store_close_item_purchase_start";

	// Token: 0x04000FD7 RID: 4055
	public const string _sEXPANDING = "expanding";

	// Token: 0x04000FD8 RID: 4056
	public const string _sPLACING = "placing";

	// Token: 0x04000FD9 RID: 4057
	public const string _sSHOPS = "shops";

	// Token: 0x04000FDA RID: 4058
	public const string _sRECIPES = "recipes";

	// Token: 0x04000FDB RID: 4059
	public const string _sDEBRIS = "debris";

	// Token: 0x04000FDC RID: 4060
	public const string _sBUILDINGS = "buildings";

	// Token: 0x04000FDD RID: 4061
	public const string _sCHARACTERS = "characters";

	// Token: 0x04000FDE RID: 4062
	public const string _sCOSTUMES = "costumes";

	// Token: 0x04000FDF RID: 4063
	public const string _sCONFIRM = "confirm";

	// Token: 0x04000FE0 RID: 4064
	public const string _sCANCEL = "cancel";

	// Token: 0x04000FE1 RID: 4065
	public const string _sTASK = "task";

	// Token: 0x04000FE2 RID: 4066
	public const string _sDATA = "data";

	// Token: 0x04000FE3 RID: 4067
	public static int _nTRACKING_VERSION = 3;

	// Token: 0x04000FE4 RID: 4068
	public static int _nTRACKING_VERSION_LOG_ITEM_PLACEMENT = 4;

	// Token: 0x02000337 RID: 823
	public struct CommonData
	{
		// Token: 0x04000FE5 RID: 4069
		public ulong ulDateTime;

		// Token: 0x04000FE6 RID: 4070
		public ulong ulFirstPlayTime;

		// Token: 0x04000FE7 RID: 4071
		public int nPlayerLevel;

		// Token: 0x04000FE8 RID: 4072
		public int nSoftCurrency;

		// Token: 0x04000FE9 RID: 4073
		public int nHardCurreny;

		// Token: 0x04000FEA RID: 4074
		public int nCharacters;

		// Token: 0x04000FEB RID: 4075
		public int nHouses;

		// Token: 0x04000FEC RID: 4076
		public int nLandExpansions;

		// Token: 0x04000FED RID: 4077
		public int nSpongyCurrency;

		// Token: 0x04000FEE RID: 4078
		public bool bIsEligibleForSpongyGames;

		// Token: 0x04000FEF RID: 4079
		public string sPlayerID;

		// Token: 0x04000FF0 RID: 4080
		public string sPlatform;

		// Token: 0x04000FF1 RID: 4081
		public string sDeviceName;

		// Token: 0x04000FF2 RID: 4082
		public string sBinaryVersion;

		// Token: 0x04000FF3 RID: 4083
		public string sOSVersion;

		// Token: 0x04000FF4 RID: 4084
		public string sManifest;

		// Token: 0x04000FF5 RID: 4085
		public string sGUID;

		// Token: 0x04000FF6 RID: 4086
		public string sDeviceGUID;

		// Token: 0x04000FF7 RID: 4087
		public ulong ulSequence;

		// Token: 0x04000FF8 RID: 4088
		public SoaringDictionary sCampaignData;
	}

	// Token: 0x02000338 RID: 824
	public abstract class Object
	{
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060017E5 RID: 6117 RVA: 0x0009E224 File Offset: 0x0009C424
		// (set) Token: 0x060017E6 RID: 6118 RVA: 0x0009E22C File Offset: 0x0009C42C
		public string m_sKey { get; protected set; }

		// Token: 0x060017E7 RID: 6119 RVA: 0x0009E238 File Offset: 0x0009C438
		public void AddToDict(SoaringDictionary pDict, string sOverrideKey = null, bool bNested = true)
		{
			if (bNested)
			{
				string text = string.IsNullOrEmpty(sOverrideKey) ? this.m_sKey : sOverrideKey;
				if (this.m_pData != null && pDict != null && !string.IsNullOrEmpty(text))
				{
					pDict.addValue(this.m_pData, text);
				}
			}
			else
			{
				string[] array = this.m_pData.allKeys();
				SoaringObjectBase[] array2 = this.m_pData.allValues();
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					pDict.addValue(array2[i], array[i]);
				}
			}
		}

		// Token: 0x04000FF9 RID: 4089
		protected SoaringDictionary m_pData = new SoaringDictionary();
	}

	// Token: 0x02000339 RID: 825
	public class MetaObject : SBMIAnalytics.Object
	{
		// Token: 0x060017E8 RID: 6120 RVA: 0x0009E2D4 File Offset: 0x0009C4D4
		public MetaObject(string sObjectKey, string sEventName, string sDeviceName, string sBinaryVersion, string sOSVersion, string sManifest, string sPlatform, string sGUID, string sDeviceGUID, int nTrackingVersion, ulong ulSequence, ulong ulEventTime)
		{
			base.m_sKey = sObjectKey;
			this.m_pData.addValue(sEventName, "event_name");
			this.m_pData.addValue(sDeviceName, "device_name");
			this.m_pData.addValue(sBinaryVersion, "binary_version");
			this.m_pData.addValue(sOSVersion, "os_version");
			this.m_pData.addValue(sManifest, "manifest");
			this.m_pData.addValue(sPlatform, "platform");
			this.m_pData.addValue(sDeviceGUID, "device_seq_id");
			this.m_pData.addValue(sGUID, "guid");
			this.m_pData.addValue(ulSequence, "device_seq_num");
			this.m_pData.addValue(nTrackingVersion, "tracking_version");
			this.m_pData.addValue(ulEventTime, "event_time");
		}
	}

	// Token: 0x0200033A RID: 826
	public class PlayerObject : SBMIAnalytics.Object
	{
		// Token: 0x060017E9 RID: 6121 RVA: 0x0009E3F0 File Offset: 0x0009C5F0
		public PlayerObject(string sObjectKey, string sPlayerID, string sLiveEventName, ulong ulFirstPlayTime, int nLevel, int nNumCharacters, int nNumHouses, int nNumLandExpansions, int nNumSoftCurrency, int nNumHardCurrency, int nSpecialCurrencyDID, int nSpecialCurrencyAmount, SoaringDictionary sABTest)
		{
			base.m_sKey = sObjectKey;
			this.m_pData.addValue(sPlayerID, "player_id");
			this.m_pData.addValue(ulFirstPlayTime, "first_play_time");
			this.m_pData.addValue(nLevel, "player_level");
			this.m_pData.addValue(nNumSoftCurrency, "gold_balance");
			this.m_pData.addValue(nNumHardCurrency, "jelly_balance");
			this.m_pData.addValue(nNumCharacters, "count_of_characters");
			this.m_pData.addValue(nNumHouses, "count_of_houses");
			this.m_pData.addValue(nNumLandExpansions, "count_of_land_expansions");
			if (nSpecialCurrencyDID >= 0)
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(nSpecialCurrencyAmount, nSpecialCurrencyDID.ToString());
				this.m_pData.addValue(soaringDictionary, "special_currency");
			}
			if (!string.IsNullOrEmpty(sLiveEventName))
			{
				this.m_pData.addValue(sLiveEventName, "live_event_name");
			}
			if (sABTest != null)
			{
				this.m_pData.addValue(sABTest, "ab_test");
			}
		}
	}

	// Token: 0x0200033B RID: 827
	public class QuestObject : SBMIAnalytics.Object
	{
		// Token: 0x060017EA RID: 6122 RVA: 0x0009E530 File Offset: 0x0009C730
		public QuestObject(string sObjectKey, string sName, string sTag, int nID, string sBranch)
		{
			base.m_sKey = sObjectKey;
			this.m_pData.addValue(sName, "name");
			this.m_pData.addValue(sTag, "tag");
			this.m_pData.addValue(nID, "id");
			this.m_pData.addValue(sBranch, "branch");
		}
	}

	// Token: 0x0200033C RID: 828
	public class RewardObject : SBMIAnalytics.Object
	{
		// Token: 0x060017EB RID: 6123 RVA: 0x0009E5A4 File Offset: 0x0009C7A4
		public RewardObject(string sObjectKey, Dictionary<int, int> pRewards)
		{
			base.m_sKey = sObjectKey;
			foreach (KeyValuePair<int, int> keyValuePair in pRewards)
			{
				if (keyValuePair.Key == ResourceManager.SOFT_CURRENCY)
				{
					this.m_pData.addValue(keyValuePair.Value, "gold_amount");
				}
				else if (keyValuePair.Key == ResourceManager.HARD_CURRENCY)
				{
					this.m_pData.addValue(keyValuePair.Value, "jelly_amount");
				}
				else if (keyValuePair.Key == ResourceManager.XP)
				{
					this.m_pData.addValue(keyValuePair.Value, "xp_amount");
				}
				else
				{
					if (!this.m_pData.containsKey("other_rewards"))
					{
						this.m_pData.addValue(new SoaringDictionary(), "other_rewards");
					}
					((SoaringDictionary)this.m_pData["other_rewards"]).addValue(keyValuePair.Value, keyValuePair.Key.ToString());
				}
			}
		}
	}

	// Token: 0x0200033D RID: 829
	public class AutoQuestObject : SBMIAnalytics.Object
	{
		// Token: 0x060017EC RID: 6124 RVA: 0x0009E700 File Offset: 0x0009C900
		public AutoQuestObject(string sObjectKey, string sName, int nID, int nCharacterID, SoaringDictionary pFoodIDs)
		{
			base.m_sKey = sObjectKey;
			this.m_pData.addValue(sName, "name");
			this.m_pData.addValue(nID, "id");
			this.m_pData.addValue(nCharacterID, "character_id");
			if (pFoodIDs != null && pFoodIDs.count() > 0)
			{
				this.m_pData.addValue(pFoodIDs, "food_tasks");
			}
		}
	}

	// Token: 0x0200033E RID: 830
	public class TaskObject : SBMIAnalytics.Object
	{
		// Token: 0x060017ED RID: 6125 RVA: 0x0009E784 File Offset: 0x0009C984
		public TaskObject(string sObjectKey, string sName, int nID, int nCharacterID, int nDuration)
		{
			base.m_sKey = sObjectKey;
			this.m_pData.addValue(sName, "name");
			this.m_pData.addValue(nID, "id");
			this.m_pData.addValue(nCharacterID, "character_id");
			this.m_pData.addValue(nDuration, "duration_seconds");
		}
	}

	// Token: 0x0200033F RID: 831
	public class CharacterObject : SBMIAnalytics.Object
	{
		// Token: 0x060017EE RID: 6126 RVA: 0x0009E7F8 File Offset: 0x0009C9F8
		public CharacterObject(string sObjectKey, string sName, int nID, int? nWishID, int? nFullnessTime)
		{
			base.m_sKey = sObjectKey;
			this.m_pData.addValue(sName, "name");
			this.m_pData.addValue(nID, "id");
			if (nWishID != null)
			{
				this.m_pData.addValue(nWishID.Value, "wish_id");
			}
			if (nFullnessTime != null)
			{
				this.m_pData.addValue(nFullnessTime.Value, "fullness_timer_seconds");
			}
		}
	}

	// Token: 0x02000340 RID: 832
	public class CostumeObject : SBMIAnalytics.Object
	{
		// Token: 0x060017EF RID: 6127 RVA: 0x0009E890 File Offset: 0x0009CA90
		public CostumeObject(string sObjectKey, string sName, int nID, int nCharacterID)
		{
			base.m_sKey = sObjectKey;
			if (!string.IsNullOrEmpty(sName))
			{
				this.m_pData.addValue(sName, "name");
			}
			if (nID >= 0)
			{
				this.m_pData.addValue(nID, "id");
			}
			if (nCharacterID >= 0)
			{
				this.m_pData.addValue(nCharacterID, "character_id");
			}
		}
	}

	// Token: 0x02000341 RID: 833
	public class ItemObject : SBMIAnalytics.Object
	{
		// Token: 0x060017F0 RID: 6128 RVA: 0x0009E908 File Offset: 0x0009CB08
		public ItemObject(string sObjectKey, string sName, string sCategory, int nID, int nSoftCost, int nHardCost)
		{
			base.m_sKey = sObjectKey;
			if (!string.IsNullOrEmpty(sName))
			{
				this.m_pData.addValue(sName, "name");
			}
			if (nID > 0)
			{
				this.m_pData.addValue(nID, "id");
			}
			if (!string.IsNullOrEmpty(sCategory))
			{
				this.m_pData.addValue(sCategory, "category");
			}
			if (nSoftCost > 0)
			{
				this.m_pData.addValue(nSoftCost, "gold_cost");
			}
			if (nHardCost > 0)
			{
				this.m_pData.addValue(nHardCost, "jelly_cost");
			}
		}
	}

	// Token: 0x02000342 RID: 834
	public class ChestObject : SBMIAnalytics.Object
	{
		// Token: 0x060017F1 RID: 6129 RVA: 0x0009E9C4 File Offset: 0x0009CBC4
		public ChestObject(string sObjectKey, string sLocation, int nID)
		{
			base.m_sKey = sObjectKey;
			this.m_pData.addValue(sLocation, "location");
			this.m_pData.addValue(nID, "id");
		}
	}

	// Token: 0x02000343 RID: 835
	public class IAPObject : SBMIAnalytics.Object
	{
		// Token: 0x060017F2 RID: 6130 RVA: 0x0009EA0C File Offset: 0x0009CC0C
		public IAPObject(string sObjectKey, string sProductCode, string sProductLinkCode, string sCurrencyType, int nAmount, int nCost)
		{
			base.m_sKey = sObjectKey;
			if (!string.IsNullOrEmpty(sProductCode))
			{
				this.m_pData.addValue(sProductCode, "product_code");
			}
			if (!string.IsNullOrEmpty(sProductLinkCode))
			{
				this.m_pData.addValue(sProductLinkCode, "product_link_code");
			}
			if (!string.IsNullOrEmpty(sCurrencyType))
			{
				this.m_pData.addValue(sCurrencyType, "product_currency_type");
			}
			if (nAmount > 0)
			{
				this.m_pData.addValue(nAmount, "product_amount");
			}
			if (nCost > 0)
			{
				this.m_pData.addValue(nCost, "product_usd_cost");
			}
		}
	}
}
