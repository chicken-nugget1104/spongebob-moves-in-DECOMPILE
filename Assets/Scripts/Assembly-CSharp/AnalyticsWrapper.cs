using System;
using System.Collections.Generic;
using DeltaDNA;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class AnalyticsWrapper : UnityEngine.Object
{
	// Token: 0x06000A12 RID: 2578 RVA: 0x0003E644 File Offset: 0x0003C844
	public static void LogQuestStarted(Game pGame, QuestDefinition pQuestDef)
	{
		if (pGame.questManager.GetQuest(pQuestDef.Did) == null)
		{
		}
		SBMIDeltaDNA.LogMissionStart(pGame, new SBMIDeltaDNA.MissionObject("mission", pQuestDef.Tag, "quest", (int)pQuestDef.Did));
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0003E68C File Offset: 0x0003C88C
	public static void LogQuestCompleted(Game pGame, QuestDefinition pQuestDef, Reward pReward)
	{
		SBMIAnalytics.LogQuestCompleted(pGame, new SBMIAnalytics.QuestObject("quest", pQuestDef.Name, pQuestDef.Tag, (int)pQuestDef.Did, pQuestDef.Branch), new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts));
		Quest quest = pGame.questManager.GetQuest(pQuestDef.Did);
		SBMIDeltaDNA.LogMissionComplete(pGame, new SBMIDeltaDNA.MissionObject("mission", pQuestDef.Tag, "quest", (int)pQuestDef.Did, (quest.CompletionTime.Value >= quest.StartTime.Value) ? (quest.CompletionTime.Value - quest.StartTime.Value) : 0UL), new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, null));
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x0003E760 File Offset: 0x0003C960
	public static void LogAutoQuestStarted(Game pGame, QuestDefinition pQuestDef, SoaringDictionary pFoodDict)
	{
		SBMIAnalytics.LogAutoQuestStarted(pGame, new SBMIAnalytics.AutoQuestObject("autoquest", pQuestDef.Name, pQuestDef.AutoQuestID, pQuestDef.AutoQuestCharacterID, pFoodDict));
		if (pGame.questManager.GetQuest(pQuestDef.Did) == null)
		{
		}
		SBMIDeltaDNA.LogMissionStart(pGame, new SBMIDeltaDNA.MissionObject("mission", pQuestDef.Tag, "quest", (int)pQuestDef.Did));
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x0003E7CC File Offset: 0x0003C9CC
	public static void LogAutoQuestCompleted(Game pGame, QuestDefinition pQuestDef, SoaringDictionary pFoodDict, Reward pReward)
	{
		SBMIAnalytics.LogAutoQuestCompleted(pGame, new SBMIAnalytics.AutoQuestObject("autoquest", pQuestDef.Name, pQuestDef.AutoQuestID, pQuestDef.AutoQuestCharacterID, pFoodDict), new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts));
		Quest quest = pGame.questManager.GetQuest(pQuestDef.Did);
		SBMIDeltaDNA.LogMissionComplete(pGame, new SBMIDeltaDNA.MissionObject("mission", pQuestDef.Tag, "quest", (int)pQuestDef.Did, (quest.CompletionTime.Value >= quest.StartTime.Value) ? (quest.CompletionTime.Value - quest.StartTime.Value) : 0UL), new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, null));
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x0003E89C File Offset: 0x0003CA9C
	public static void LogTaskStarted(Game pGame, Task pTask)
	{
		Simulated simulated = pGame.simulation.FindSimulated(new int?(pTask.m_pTaskData.m_nSourceDID));
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		int? hungerResourceId = entity.HungerResourceId;
		int? nFullnessTime = null;
		if (hungerResourceId == null)
		{
			nFullnessTime = new int?((int)Mathf.Clamp(entity.HungryAt - TFUtils.EpochTime(), 0f, float.MaxValue));
		}
		CostumeManager.Costume costume = null;
		if (entity.CostumeDID != null || entity.DefaultCostumeDID != null)
		{
			costume = pGame.costumeManager.GetCostume((entity.CostumeDID == null) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value);
		}
		SBMIAnalytics.LogTaskStarted(pGame, new SBMIAnalytics.TaskObject("task", pTask.m_pTaskData.m_sName, pTask.m_pTaskData.m_nDID, pTask.m_pTaskData.m_nSourceDID, pTask.m_pTaskData.m_nDuration), new SBMIAnalytics.CharacterObject("character", simulated.entity.Name, entity.DefinitionId, hungerResourceId, nFullnessTime), (costume != null) ? new SBMIAnalytics.CostumeObject("costume", costume.m_sName, costume.m_nDID, entity.DefinitionId) : null);
		SBMIDeltaDNA.LogMissionStart(pGame, new SBMIDeltaDNA.MissionObject("mission", pTask.m_pTaskData.m_sName, "task", pTask.m_pTaskData.m_nDID));
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0003EA2C File Offset: 0x0003CC2C
	public static void LogTaskCompleted(Game pGame, Task pTask)
	{
		Simulated simulated = pGame.simulation.FindSimulated(new int?(pTask.m_pTaskData.m_nSourceDID));
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		int? hungerResourceId = entity.HungerResourceId;
		int? nFullnessTime = null;
		if (hungerResourceId == null)
		{
			nFullnessTime = new int?((int)Mathf.Clamp(entity.HungryAt - TFUtils.EpochTime(), 0f, float.MaxValue));
		}
		CostumeManager.Costume costume = null;
		if (entity.CostumeDID != null || entity.DefaultCostumeDID != null)
		{
			costume = pGame.costumeManager.GetCostume((entity.CostumeDID == null) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value);
		}
		SBMIAnalytics.LogTaskCompleted(pGame, new SBMIAnalytics.TaskObject("task", pTask.m_pTaskData.m_sName, pTask.m_pTaskData.m_nDID, pTask.m_pTaskData.m_nSourceDID, pTask.m_pTaskData.m_nDuration), new SBMIAnalytics.CharacterObject("character", simulated.entity.Name, entity.DefinitionId, hungerResourceId, nFullnessTime), (costume != null) ? new SBMIAnalytics.CostumeObject("costume", costume.m_sName, costume.m_nDID, entity.DefinitionId) : null, new SBMIAnalytics.RewardObject("reward", pTask.m_pTaskData.m_pReward.ResourceAmounts));
		SBMIDeltaDNA.LogMissionComplete(pGame, new SBMIDeltaDNA.MissionObject("mission", pTask.m_pTaskData.m_sName, "task", pTask.m_pTaskData.m_nDID, (pTask.m_ulCompleteTime >= pTask.m_ulStartTime) ? (pTask.m_ulCompleteTime - pTask.m_ulStartTime) : 0UL), new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pTask.m_pTaskData.m_pReward, pGame, -1, null, null));
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0003EC1C File Offset: 0x0003CE1C
	public static void LogCostumeUnlocked(Game pGame, CostumeManager.Costume pCostume)
	{
		SBMIAnalytics.LogCostumeUnlocked(pGame, new SBMIAnalytics.CostumeObject("costume", pCostume.m_sName, pCostume.m_nDID, pCostume.m_nUnitDID));
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0003EC4C File Offset: 0x0003CE4C
	public static void LogCostumeChanged(Game pGame, ResidentEntity pResidentEntity, CostumeManager.Costume pOldCostume, CostumeManager.Costume pNewCostume)
	{
		int? hungerResourceId = pResidentEntity.HungerResourceId;
		int? nFullnessTime = null;
		if (hungerResourceId == null)
		{
			nFullnessTime = new int?((int)Mathf.Clamp(pResidentEntity.HungryAt - TFUtils.EpochTime(), 0f, float.MaxValue));
		}
		SBMIAnalytics.LogCostumeChanged(pGame, new SBMIAnalytics.CharacterObject("character", pResidentEntity.Name, pResidentEntity.DefinitionId, hungerResourceId, nFullnessTime), new SBMIAnalytics.CostumeObject("costume_old", (pOldCostume != null) ? pOldCostume.m_sName : null, (pOldCostume != null) ? pOldCostume.m_nDID : -1, pResidentEntity.DefinitionId), new SBMIAnalytics.CostumeObject("costume_new", pNewCostume.m_sName, pNewCostume.m_nDID, pResidentEntity.DefinitionId));
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0003ED08 File Offset: 0x0003CF08
	public static void LogDailyReward(Game pGame, int nDay, Reward pReward)
	{
		SBMIAnalytics.LogDailyReward(pGame, nDay, new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts));
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "daily_bonus"));
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0003ED4C File Offset: 0x0003CF4C
	public static void LogChestPickup(Game pGame, Simulated pChestSimulated, Reward pReward)
	{
		SBMIAnalytics.LogChestPickup(pGame, new SBMIAnalytics.ChestObject("chest", "home", pChestSimulated.entity.DefinitionId), new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts));
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "treasure_chest"));
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0003EDA8 File Offset: 0x0003CFA8
	public static void LogPatchyChestPickup(Game pGame, Simulated pChestSimulated, Reward pReward)
	{
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "patchy_treasure_chest"));
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0003EDD4 File Offset: 0x0003CFD4
	public static void LogRentCollected(Game pGame, Simulated pSimulated, Reward pReward)
	{
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "rent_collected_" + pSimulated.entity.Name));
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x0003EE10 File Offset: 0x0003D010
	public static void LogCharacterFeed(Game pGame, ResidentEntity pResidentEntity, int nHungerResourceDID, Reward pReward)
	{
		CostumeManager.Costume costume = null;
		if (pResidentEntity.CostumeDID != null || pResidentEntity.DefaultCostumeDID != null)
		{
			costume = pGame.costumeManager.GetCostume((pResidentEntity.CostumeDID == null) ? pResidentEntity.DefaultCostumeDID.Value : pResidentEntity.CostumeDID.Value);
		}
		SBMIAnalytics.LogCharacterFeed(pGame, new SBMIAnalytics.CharacterObject("character", pResidentEntity.Name, pResidentEntity.DefinitionId, new int?(nHungerResourceDID), null), (costume != null) ? new SBMIAnalytics.CostumeObject("costume", costume.m_sName, costume.m_nDID, pResidentEntity.DefinitionId) : null, new SBMIAnalytics.ItemObject("item", pGame.simulation.resourceManager.Resources[nHungerResourceDID].Name, "recipes", nHungerResourceDID, -1, -1), (pReward != null) ? new SBMIAnalytics.RewardObject("reward", pReward.ResourceAmounts) : null);
		string sWishName = string.Empty;
		if (pGame.resourceManager.Resources.ContainsKey(nHungerResourceDID))
		{
			sWishName = pGame.resourceManager.Resources[nHungerResourceDID].Name;
		}
		SBMIDeltaDNA.LogWishGranted(pGame, sWishName, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, null));
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x0003EF74 File Offset: 0x0003D174
	public static void LogBonusChest(Game pGame, Simulated pSimulated, Reward pReward)
	{
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", pReward, pGame, -1, null, "bonus_chest_" + pSimulated.entity.Name));
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0003EFB0 File Offset: 0x0003D1B0
	public static void LogVisitPark(Game pGame)
	{
		SBMIAnalytics.LogVisitPark(pGame);
		SBMIDeltaDNA.LogUIInteraction(pGame, pGame.LastAction(), pGame.GetType().ToString(), pGame.LastAction());
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0003EFE0 File Offset: 0x0003D1E0
	public static void LogSessionBegin(Game pGame, ulong ulPauseTime)
	{
		SBMIAnalytics.LogSessionBegin(pGame);
		if (ulPauseTime >= 120UL)
		{
			Singleton<SDK>.Instance.NewSession();
		}
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0003EFFC File Offset: 0x0003D1FC
	public static void LogPurchaseComplete(Game pGame, SoaringPurchasable pSoaringPurchasable, string sReceipt, string sTransactionID, RmtProduct rmtProduct = null)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		string sCurrencyType = "jelly";
		if (pSoaringPurchasable.ResourceType == 3)
		{
			sCurrencyType = "coin";
			dictionary.Add(ResourceManager.SOFT_CURRENCY, pSoaringPurchasable.Amount);
		}
		else
		{
			dictionary.Add(ResourceManager.HARD_CURRENCY, pSoaringPurchasable.Amount);
		}
		SBMIAnalytics.LogPurchaseComplete(pGame, new SBMIAnalytics.IAPObject("iap", pSoaringPurchasable.ProductID, pSoaringPurchasable.Alias, sCurrencyType, pSoaringPurchasable.Amount, pSoaringPurchasable.USDPrice));
		string sTransactionServer = "APPLE";
		if (SoaringPlatform.Platform == SoaringPlatformType.Amazon)
		{
			sTransactionServer = "AMAZON";
		}
		else if (SoaringPlatform.Platform == SoaringPlatformType.Android)
		{
			sTransactionServer = "GOOGLE";
		}
		string sRealCurrencyType = "USD";
		int nRealCurrencyAmount = pSoaringPurchasable.USDPrice * 100;
		if (rmtProduct != null)
		{
			sRealCurrencyType = rmtProduct.currencyCode;
			nRealCurrencyAmount = Mathf.RoundToInt(rmtProduct.price * 100f);
		}
		SBMIDeltaDNA.LogTransaction(pGame, new SBMIDeltaDNA.RewardObject("productsSpent", "productsSpent", null, pGame, nRealCurrencyAmount, sRealCurrencyType, null), new SBMIDeltaDNA.RewardObject("productsReceived", "productsReceived", new Reward(dictionary, null, null, null, null, null, null, null, false, null), pGame, -1, null, null), new SBMIDeltaDNA.TransactionObject("transaction", "server", sTransactionServer, sReceipt, pSoaringPurchasable.ProductID, sTransactionID, true), "PURCHASE", pSoaringPurchasable.DisplayName);
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x0003F13C File Offset: 0x0003D33C
	public static void LogLevelUp(Game pGame, List<Reward> pRewards, int nLevel)
	{
		SBMIAnalytics.LogLevelUp(pGame);
		int count = pRewards.Count;
		Reward reward = (count <= 0) ? null : pRewards[0];
		for (int i = 1; i < count; i++)
		{
			reward += pRewards[i];
		}
		SBMIDeltaDNA.LogLevelUp(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", reward, pGame, -1, null, null), nLevel);
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x0003F1A8 File Offset: 0x0003D3A8
	public static void LogItemPlacement(Game pGame, Entity pEntity, bool bFromInventory, bool bAccepted)
	{
		int num = -1;
		int num2 = -1;
		if (bAccepted)
		{
			Cost cost = pGame.catalog.GetCost(pEntity.DefinitionId);
			if (cost != null)
			{
				if (cost.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
				{
					num = cost.ResourceAmounts[ResourceManager.SOFT_CURRENCY];
				}
				if (cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
				{
					num2 = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
				}
			}
		}
		SBMIAnalytics.LogItemPlacement(pGame, new SBMIAnalytics.ItemObject("item", pEntity.Name, "buildings", pEntity.DefinitionId, num, num2), bFromInventory, (!bAccepted) ? "cancel" : "confirm");
		if (!bFromInventory)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			if (num > 0)
			{
				dictionary.Add(ResourceManager.SOFT_CURRENCY, num);
			}
			if (num2 > 0)
			{
				dictionary.Add(ResourceManager.HARD_CURRENCY, num2);
			}
			SBMIDeltaDNA.LogTransaction(pGame, new SBMIDeltaDNA.RewardObject("productsSpent", "productsSpent", new Reward(dictionary, null, null, null, null, null, null, null, false, null), pGame, -1, null, null), new SBMIDeltaDNA.RewardObject("productsReceived", "productsReceived", new Reward(null, new Dictionary<int, int>
			{
				{
					pEntity.DefinitionId,
					1
				}
			}, null, null, null, null, null, null, false, null), pGame, -1, null, null), null, "PURCHASE", pEntity.Name);
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0003F2FC File Offset: 0x0003D4FC
	public static void LogExpansion(Game pGame, int nExpansionID, Cost pCost)
	{
		SBMIDeltaDNA.LogTransaction(pGame, new SBMIDeltaDNA.RewardObject("productsSpent", "productsSpent", new Reward((pCost != null) ? pCost.ResourceAmounts : null, null, null, null, null, null, null, null, false, null), pGame, -1, null, null), new SBMIDeltaDNA.RewardObject("productsReceived", "productsReceived", new Reward(null, null, null, null, null, null, new List<int>
		{
			nExpansionID
		}, null, false, null), pGame, -1, null, null), null, "PURCHASE", "expansion_" + nExpansionID.ToString());
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0003F388 File Offset: 0x0003D588
	public static void LogCostumePurchased(Game pGame, CostumeManager.Costume pCostume, int nCurrencyDID, int nNumCurrency)
	{
		SBMIDeltaDNA.LogTransaction(pGame, new SBMIDeltaDNA.RewardObject("productsSpent", "productsSpent", new Reward(new Dictionary<int, int>
		{
			{
				nCurrencyDID,
				nNumCurrency
			}
		}, null, null, null, null, null, null, null, false, null), pGame, -1, null, null), new SBMIDeltaDNA.RewardObject("productsReceived", "productsReceived", new Reward(null, null, null, null, null, new List<int>
		{
			pCostume.m_nDID
		}, null, null, false, null), pGame, -1, null, null), null, "PURCHASE", pCostume.m_sName);
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0003F40C File Offset: 0x0003D60C
	public static void LogJellyConfirmation(Game pGame, int nItemDID, int nJellyCost, string sItemName, string sItemType, string sTriggerEventType, string sSpeedupType, string sAction)
	{
		SBMIAnalytics.LogJellyConfirmation(pGame, new SBMIAnalytics.ItemObject("item", sItemName, sItemType, nItemDID, -1, nJellyCost), sTriggerEventType, sSpeedupType, sAction);
		if (sItemType == "building" || sItemType == "buildings")
		{
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, nItemDID, sItemName, "building", "PURCHASE", sItemName);
		}
		if (sItemType == "costumes")
		{
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, nItemDID, sItemName, "costume", "PURCHASE", sItemName);
		}
		if (sItemType == "debris")
		{
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, nItemDID, sItemName, "debris", "PURCHASE", sItemName);
		}
		if (sItemType == "craft")
		{
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, nItemDID, sItemName, "resource", "PURCHASE", sItemName);
		}
		if (sItemType == "!!BLDG_NAME_TREAT_SHOP" || sItemType == "!!BLDG_NAME_JUICEBAR" || sItemType == "!!BLDG_NAME_KK" || sItemType == "!!BLDG_NAME_BAKERY" || sItemType == "!!BLDG_NAME_CHUMINATOR" || sItemType == "!!BLDG_NAME_CHUMBUCKET" || sItemType == "!!BLDG_NAME_SEEDSTORE" || sItemType == "!!BLDG_NAME_RUSTYS_RIB_EYE" || sItemType == "!!BLDG_NAME_ICECREAMSHOP" || sItemType == "!!BLDG_NAME_FLOWERSHOP" || sItemType == "!!BLDG_NAME_PERFUMESHOP" || sItemType == "!!BLDG_NAME_WHOLE_BROW_FOODS")
		{
			string text = "Production_Slot_" + sItemName;
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, nItemDID, text, "production slot", "PURCHASE", text);
		}
		if (sItemType == "task")
		{
			string text = "Accelerate_" + sItemName;
			SBMIDeltaDNA.LogTransaction(pGame, nJellyCost, 5, text, "task", "PURCHASE", text);
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0003F614 File Offset: 0x0003D814
	public static void LogRecievedEventItem(Game pGame, int nItemDID, string sItemName)
	{
		SBMIAnalytics.LogRecievedEventItem(pGame, new SBMIAnalytics.ItemObject("item", sItemName, string.Empty, nItemDID, -1, -1));
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0003F630 File Offset: 0x0003D830
	public static void LogCraftCollected(Game pGame, Entity pBuildingEntity, int nCraftDID, int nNumCrafted, string sCraftName)
	{
		SBMIAnalytics.LogCraftCollected(pGame, new SBMIAnalytics.ItemObject("item", pBuildingEntity.Name, "buildings", pBuildingEntity.DefinitionId, -1, -1), new SBMIAnalytics.ItemObject("item", sCraftName, "recipes", nCraftDID, -1, -1), nNumCrafted);
		SBMIDeltaDNA.LogItemCollected(pGame, new SBMIDeltaDNA.RewardObject("reward", "rewardProducts", new Reward(new Dictionary<int, int>
		{
			{
				nCraftDID,
				nNumCrafted
			}
		}, null, null, null, null, null, null, null, false, null), pGame, -1, null, "craft"));
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0003F6B0 File Offset: 0x0003D8B0
	public static void LogStoreImpressions(Game pGame, List<SBGUIMarketplaceScreen.StoreImpression> pStoreImpressions)
	{
		SBMIAnalytics.LogStoreImpressions(pGame, pStoreImpressions);
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0003F6BC File Offset: 0x0003D8BC
	public static void LogMarketplaceUI(Game pGame, string sAction, string sOpenType, string sLeaveType)
	{
		SBMIAnalytics.LogMarketplaceUI(pGame, sAction, sOpenType, sLeaveType);
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0003F6C8 File Offset: 0x0003D8C8
	public static void LogEventButtonClick(Game pGame)
	{
		SBMIAnalytics.LogEventButtonClick(pGame);
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x0003F6D0 File Offset: 0x0003D8D0
	public static void LogUIInteraction(Game pGame, string sUIName, string sType, string sAction)
	{
		SBMIDeltaDNA.LogUIInteraction(pGame, sUIName, sType, sAction);
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x0003F6DC File Offset: 0x0003D8DC
	public static void LogFeatureUnlocked(Game pGame, string sFeatureName, string sFeatureType)
	{
		SBMIDeltaDNA.LogFeatureUnlocked(pGame, sFeatureName, sFeatureType);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0003F6E8 File Offset: 0x0003D8E8
	public static void LogShopTabOpened(Game pGame, string sTabName)
	{
		SBMIDeltaDNA.LogShopEntered(pGame, sTabName);
	}
}
