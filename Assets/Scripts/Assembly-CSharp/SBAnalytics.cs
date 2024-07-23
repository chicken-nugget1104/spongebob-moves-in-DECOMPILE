using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class SBAnalytics
{
	// Token: 0x060002DF RID: 735 RVA: 0x0000E810 File Offset: 0x0000CA10
	public SBAnalytics()
	{
		this.deviceId = TFUtils.DeviceId;
		this.deviceInfo = string.Format("{0} {1} {2}", SystemInfo.deviceModel, SystemInfo.processorType, SystemInfo.operatingSystem);
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		GameObject gameObject = new GameObject("LoadingFunnel");
		this.loadingFunnel = gameObject.AddComponent<LoadingFunnel>();
		this.loadingFunnel.Initialize(ref eventData);
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000E880 File Offset: 0x0000CA80
	// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000E888 File Offset: 0x0000CA88
	public string PlayerId
	{
		get
		{
			return this.playerId;
		}
		set
		{
			this.playerId = value;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000E894 File Offset: 0x0000CA94
	// (set) Token: 0x060002E3 RID: 739 RVA: 0x0000E89C File Offset: 0x0000CA9C
	public bool IsOffline
	{
		get
		{
			return this.isOffline;
		}
		set
		{
			this.isOffline = value;
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000E8A8 File Offset: 0x0000CAA8
	// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000E8B0 File Offset: 0x0000CAB0
	public int StartingJJAmount
	{
		get
		{
			return this.jjamount;
		}
		set
		{
			this.jjamount = value;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x060002E6 RID: 742 RVA: 0x0000E8BC File Offset: 0x0000CABC
	public string androidDeviceType
	{
		get
		{
			return TFUtils.GetAndroidDeviceTypeString();
		}
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0000E8C4 File Offset: 0x0000CAC4
	public void AddCommon(Dictionary<string, object> eventData)
	{
		eventData["DeviceId"] = this.deviceId;
		eventData["DeviceInfo"] = this.deviceInfo;
		eventData["PlayerId"] = this.playerId;
		eventData["Offline"] = this.isOffline;
		eventData["JJAmount"] = this.jjamount;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0000E930 File Offset: 0x0000CB30
	public void AddSubtypes(Dictionary<string, object> eventData, string subtype1, string subtype2 = null, string subtype3 = null)
	{
		if (subtype1 != null)
		{
			eventData["subtype1"] = subtype1;
		}
		if (subtype2 != null)
		{
			eventData["subtype2"] = subtype2;
		}
		if (subtype3 != null)
		{
			eventData["subtype3"] = subtype3;
		}
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0000E978 File Offset: 0x0000CB78
	public static void AddCost(Dictionary<string, object> eventData, Cost cost)
	{
		int count = cost.ResourceAmounts.Count;
		if (count == 1)
		{
			int onlyCostKey = cost.GetOnlyCostKey();
			eventData["CostType"] = ResourceManager.TypeDescription(onlyCostKey);
			eventData["value"] = cost.ResourceAmounts[onlyCostKey];
		}
		foreach (int num in cost.ResourceAmounts.Keys)
		{
			eventData[ResourceManager.TypeDescription(num)] = cost.ResourceAmounts[num];
		}
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0000EA40 File Offset: 0x0000CC40
	public void LogLoadingFunnelStep(string stepName)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.loadingFunnel.LogStep(stepName, ref eventData);
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0000EA68 File Offset: 0x0000CC68
	public void LogStartedPlaying(int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", null, null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent("Login", dictionary);
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0000EAAC File Offset: 0x0000CCAC
	public void LogSessionBegin()
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", null, null);
		TFAnalytics.LogEvent("kontagent_session_begin", eventData);
	}

	// Token: 0x060002ED RID: 749 RVA: 0x0000EAE0 File Offset: 0x0000CCE0
	public void LogRequestInAppPurchase(string iapBundleName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", "IAP", "RequestIap");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(iapBundleName, dictionary);
	}

	// Token: 0x060002EE RID: 750 RVA: 0x0000EB28 File Offset: 0x0000CD28
	public void LogCompleteInAppPurchase(string iapBundleName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", "IAP", "SucceedIap");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(iapBundleName, dictionary);
		TFAnalytics.LogRevenueTracking(1);
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0000EB78 File Offset: 0x0000CD78
	public void LogCancelInAppPurchase(string iapBundleName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", "IAP", "CancelIap");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(iapBundleName, dictionary);
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0000EBC0 File Offset: 0x0000CDC0
	public void LogFailInAppPurchase(string iapBundleName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", "IAP", "FailIap");
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(iapBundleName, dictionary);
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0000EC08 File Offset: 0x0000CE08
	public void LogSoaringIAPPurchaseComplete(string iapBundleName)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "MonetizationByLevel", "Soaring_IAP", "FailIap");
		TFAnalytics.LogEvent(iapBundleName, eventData);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0000EC40 File Offset: 0x0000CE40
	public void LogPlayerConfirmHardSpend(int amountOfJelly, bool canAfford, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", "JJMicroConfirm", null);
		dictionary["value"] = amountOfJelly;
		dictionary["level"] = playerLevel;
		string eventName = (!canAfford) ? "InsufficientJJ" : "Success";
		TFAnalytics.LogEvent(eventName, dictionary);
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0000ECAC File Offset: 0x0000CEAC
	public void LogPlayerRejectHardSpend(int amountOfJelly, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", "JJMicroConfirm", null);
		dictionary["value"] = amountOfJelly;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent("Reject", dictionary);
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0000ED08 File Offset: 0x0000CF08
	private void LogRush(string logName, string eventID, string subeventID, int rushCost)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", eventID, subeventID);
		dictionary["value"] = rushCost;
		TFAnalytics.LogEvent(logName, dictionary);
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0000ED4C File Offset: 0x0000CF4C
	public void LogRushBuild(string buildingName, int rushCost, bool able)
	{
		this.LogRush(buildingName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedBuild", rushCost);
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0000ED7C File Offset: 0x0000CF7C
	public void LogRushRent(string generatorName, int rushCost, bool able)
	{
		this.LogRush(generatorName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedPay", rushCost);
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0000EDAC File Offset: 0x0000CFAC
	public void LogRushTask(string taskName, int rushCost, bool able)
	{
		this.LogRush(taskName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedTask", rushCost);
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x0000EDDC File Offset: 0x0000CFDC
	public void LogRushFullness(string characterName, int rushCost, bool able)
	{
		this.LogRush(characterName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedFullness", rushCost);
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x0000EE0C File Offset: 0x0000D00C
	public void LogRushCraft(string recipeName, int rushCost, bool able)
	{
		this.LogRush(recipeName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedCraft", rushCost);
	}

	// Token: 0x060002FA RID: 762 RVA: 0x0000EE3C File Offset: 0x0000D03C
	public void LogRushClear(string debrisName, int rushCost, bool able)
	{
		this.LogRush(debrisName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedClear", rushCost);
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0000EE6C File Offset: 0x0000D06C
	public void LogRushRestock(string buildingName, int rushCost, bool able)
	{
		this.LogRush(buildingName, (!able) ? "NotEnoughJelly" : "SpendJelly", "SpeedRestock", rushCost);
	}

	// Token: 0x060002FC RID: 764 RVA: 0x0000EE9C File Offset: 0x0000D09C
	public void LogResourceEconomySource(string nameOfResource, int amountOfResource, int playerLevelBeforeEvent, int playerLevelPostEvent, ResourceManager resourceMgr)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "ResourceEconomy", nameOfResource, "Source");
		dictionary["value"] = amountOfResource;
		dictionary["level"] = playerLevelBeforeEvent;
		TFAnalytics.LogEvent(nameOfResource, dictionary);
		for (int i = 1 + playerLevelBeforeEvent; i <= playerLevelPostEvent; i++)
		{
			this.LogLevelGold(resourceMgr.Resources[ResourceManager.SOFT_CURRENCY].Amount, i);
			this.LogLevelJJ(resourceMgr.Resources[ResourceManager.HARD_CURRENCY].Amount, i);
			this.LogLevelPositions(i);
		}
	}

	// Token: 0x060002FD RID: 765 RVA: 0x0000EF48 File Offset: 0x0000D148
	public void LogSinkResources(string nameOfResource, int amountOfResource, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", "SinkRez", null);
		dictionary["value"] = amountOfResource * -1;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(nameOfResource, dictionary);
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0000EFA0 File Offset: 0x0000D1A0
	public void LogResourceEconomySink(string nameOfResource, int amountOfResource, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "ResourceEconomy", nameOfResource, "Sink");
		dictionary["value"] = amountOfResource * -1;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(nameOfResource, dictionary);
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
	public void LogQuestStart(string questTag, string questName, uint questUID, int playerLevel)
	{
		TFUtils.Assert(questTag != null, "All quest logging must have a valid questTag!");
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "QuestStart", null);
		dictionary["level"] = playerLevel;
		dictionary["QuestUID"] = questUID;
		dictionary["QuestName"] = questName;
		TFAnalytics.LogEvent(questTag, dictionary);
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0000F06C File Offset: 0x0000D26C
	public void LogQuestCompleteSoaring(string questTag)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", "quest_completed", null);
		TFAnalytics.LogEvent(questTag, eventData);
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0000F0A0 File Offset: 0x0000D2A0
	public void LogQuestComplete(string questTag, string questName, uint questUID, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "QuestComplete", null);
		dictionary["level"] = playerLevel;
		dictionary["QuestUID"] = questUID;
		dictionary["QuestName"] = questName;
		TFAnalytics.LogEvent(questTag, dictionary);
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0000F104 File Offset: 0x0000D304
	public void LogQuestCompleteJJAMT(string questTag, string questName, uint questUID, int playerLevel, int amtjj)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "QuestCompleteJJAmt", null);
		dictionary["level"] = playerLevel;
		dictionary["value"] = amtjj;
		dictionary["QuestUID"] = questUID;
		dictionary["QuestName"] = questName;
		TFAnalytics.LogEvent(questTag, dictionary);
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0000F178 File Offset: 0x0000D378
	public void LogQuestCompleteGoldAMT(string questTag, string questName, uint questUID, int playerLevel, int amtgold)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "QuestCompleteGoldAmt", null);
		dictionary["level"] = playerLevel;
		dictionary["value"] = amtgold;
		dictionary["QuestUID"] = questUID;
		dictionary["QuestName"] = questName;
		TFAnalytics.LogEvent(questTag, dictionary);
	}

	// Token: 0x06000304 RID: 772 RVA: 0x0000F1EC File Offset: 0x0000D3EC
	public void LogAutoQuestStarted(string questTag)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", "autoquest_started", null);
		TFAnalytics.LogEvent(questTag, eventData);
	}

	// Token: 0x06000305 RID: 773 RVA: 0x0000F220 File Offset: 0x0000D420
	public void LogAutoQuestCompleted(string questTag)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", "autoquest_completed", null);
		TFAnalytics.LogEvent(questTag, eventData);
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0000F254 File Offset: 0x0000D454
	public void LogTaskStarted(int taskDID)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", "character_task_started", null);
		TFAnalytics.LogEvent(taskDID.ToString(), eventData);
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0000F290 File Offset: 0x0000D490
	public void LogTaskCompleted(int taskDID, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "character_task_completed", null);
		dictionary["value"] = playerLevel;
		TFAnalytics.LogEvent(taskDID.ToString(), dictionary);
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0000F2DC File Offset: 0x0000D4DC
	public void LogCostumeUnlocked(int costumeDID)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", "change_costume", null);
		TFAnalytics.LogEvent(costumeDID.ToString(), eventData);
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0000F318 File Offset: 0x0000D518
	public void LogCostumeChanged(int costumeDID)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", "costume_unlock", null);
		TFAnalytics.LogEvent(costumeDID.ToString(), eventData);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0000F354 File Offset: 0x0000D554
	public void LogDailyReward(int day)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", "daily_reward", null);
		TFAnalytics.LogEvent("day_" + day.ToString(), eventData);
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0000F398 File Offset: 0x0000D598
	public void LogChestPickup(int did)
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", "chest_pickup", null);
		TFAnalytics.LogEvent(did.ToString(), eventData);
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0000F3D4 File Offset: 0x0000D5D4
	public void LogCharacterFeed(int characterDID, int foodDID)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "character_feed", null);
		dictionary["value"] = foodDID;
		TFAnalytics.LogEvent(characterDID.ToString(), dictionary);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x0000F420 File Offset: 0x0000D620
	public void LogVisitPark()
	{
		Dictionary<string, object> eventData = new Dictionary<string, object>();
		this.AddCommon(eventData);
		this.AddSubtypes(eventData, "Progression", "visit_park", null);
		TFAnalytics.LogEvent("visit_park", eventData);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x0000F458 File Offset: 0x0000D658
	public void LogEligiblePromoEvent(int playerLevel, string promoEventName)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Acquisition", "PromoEvent", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(promoEventName, dictionary);
	}

	// Token: 0x0600030F RID: 783 RVA: 0x0000F49C File Offset: 0x0000D69C
	public void LogPlacement(string itemName, bool decoration, bool premium, Cost cost, int playerLevel, float fps)
	{
		string[] array = new string[]
		{
			"Build",
			"BuyPremBuild",
			"Decoration",
			"BuyPremDeco"
		};
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", array[2 * ((!decoration) ? 0 : 1) + ((!premium) ? 0 : 1)], null);
		dictionary["level"] = playerLevel;
		dictionary["value"] = (int)fps;
		SBAnalytics.AddCost(dictionary, cost);
		TFAnalytics.LogEvent(itemName, dictionary);
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0000F540 File Offset: 0x0000D740
	public void LogPlacementFromInventory(string itemName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "PlaceFromInventory", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(itemName, dictionary);
	}

	// Token: 0x06000311 RID: 785 RVA: 0x0000F584 File Offset: 0x0000D784
	public void LogPurchaseEventReward(string itemName, Cost cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "BuyEventReward", null);
		dictionary["level"] = playerLevel;
		SBAnalytics.AddCost(dictionary, cost);
		TFAnalytics.LogEvent(itemName, dictionary);
	}

	// Token: 0x06000312 RID: 786 RVA: 0x0000F5D0 File Offset: 0x0000D7D0
	public void LogBuildingAddToInventory(string itemName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "BuildingAddedToInventory", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(itemName, dictionary);
	}

	// Token: 0x06000313 RID: 787 RVA: 0x0000F614 File Offset: 0x0000D814
	public void LogAchievement(string achievementName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "Achievement", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(achievementName, dictionary);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x0000F658 File Offset: 0x0000D858
	public void LogLevelGold(int amountOfSoftCurrency, int newLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "LevelGold", null);
		dictionary["level"] = TFUtils.KontagentCurrencyLevelIndex(newLevel);
		dictionary["value"] = amountOfSoftCurrency;
		TFAnalytics.LogEvent(string.Format("Level{0:00}", newLevel), dictionary);
	}

	// Token: 0x06000315 RID: 789 RVA: 0x0000F6C4 File Offset: 0x0000D8C4
	public void LogLevelJJ(int amountOfHardCurrency, int newLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "LevelJJ", null);
		dictionary["level"] = TFUtils.KontagentCurrencyLevelIndex(newLevel);
		dictionary["value"] = amountOfHardCurrency;
		TFAnalytics.LogEvent(string.Format("Level{0:00}", newLevel), dictionary);
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0000F730 File Offset: 0x0000D930
	public void LogLevelPositions(int newLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "Level", null);
		dictionary["level"] = newLevel;
		dictionary["value"] = 1;
		TFAnalytics.LogEvent("user_level_positions", dictionary);
		dictionary["level"] = newLevel - 1;
		dictionary["value"] = -1;
		TFAnalytics.LogEvent("user_level_positions", dictionary);
	}

	// Token: 0x06000317 RID: 791 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
	public void LogLevelPlaytime(int levelJustFinished, ulong walltimeMinutes, ulong playtimeMinutes)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "LevelWalltime", null);
		dictionary["level"] = 0;
		dictionary["value"] = walltimeMinutes;
		if (walltimeMinutes > 0UL)
		{
			TFAnalytics.LogEvent(string.Format("Level{0:00}", levelJustFinished), dictionary);
		}
		this.AddSubtypes(dictionary, "Progression", "LevelPlaytime", null);
		dictionary["value"] = playtimeMinutes;
		if (playtimeMinutes > 0UL)
		{
			TFAnalytics.LogEvent(string.Format("Level{0:00}", levelJustFinished), dictionary);
		}
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0000F868 File Offset: 0x0000DA68
	public void LogTask(string taskName, string srcUnit, string targetName, Type targetType, int coinsEarned, int playerLevel)
	{
		string eventName = targetName;
		string subtype;
		if (targetType != null && targetType == typeof(ResidentEntity))
		{
			subtype = "CharacterTask";
		}
		else if (targetType != null && targetType == typeof(BuildingEntity))
		{
			subtype = "BuildingTask";
		}
		else if (targetType != null && targetType == typeof(DebrisEntity))
		{
			subtype = "PlayTask";
		}
		else if (targetType != null && targetType == typeof(LandmarkEntity))
		{
			subtype = "PlayTask";
		}
		else
		{
			subtype = "AnyTask";
			eventName = taskName;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", subtype, null);
		dictionary["value"] = coinsEarned;
		dictionary["level"] = playerLevel;
		dictionary["SrcUnit"] = srcUnit;
		dictionary["TaskName"] = taskName;
		TFAnalytics.LogEvent(eventName, dictionary);
	}

	// Token: 0x06000319 RID: 793 RVA: 0x0000F970 File Offset: 0x0000DB70
	public void LogClearDebris(string debrisName, Cost cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "ClearDebris", null);
		dictionary["level"] = playerLevel;
		SBAnalytics.AddCost(dictionary, cost);
		TFAnalytics.LogEvent(debrisName, dictionary);
	}

	// Token: 0x0600031A RID: 794 RVA: 0x0000F9BC File Offset: 0x0000DBBC
	public void LogMoveObject(string objectName, int playerLevel, float distance, float fps)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Acquisition", "Interaction", "MoveObject");
		dictionary["value"] = (int)fps;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("Move_FPS_{0}", objectName), dictionary);
		dictionary["value"] = (int)distance;
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("Move_Dist_{0}", objectName), dictionary);
	}

	// Token: 0x0600031B RID: 795 RVA: 0x0000FA58 File Offset: 0x0000DC58
	public void LogOpenSettings(int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "OpenSettings", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent("OpenSettings", dictionary);
	}

	// Token: 0x0600031C RID: 796 RVA: 0x0000FAA0 File Offset: 0x0000DCA0
	public void LogSell(string objectName, bool decoration, Cost cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", (!decoration) ? "SellBuilding" : "SellDeco", null);
		dictionary["level"] = playerLevel;
		SBAnalytics.AddCost(dictionary, cost);
		TFAnalytics.LogEvent(string.Format("Sell{0}", objectName), dictionary);
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0000FB08 File Offset: 0x0000DD08
	public void LogCrafting(string productName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "Crafting", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(productName, dictionary);
	}

	// Token: 0x0600031E RID: 798 RVA: 0x0000FB4C File Offset: 0x0000DD4C
	public void LogPremiumCrafting(string productName, int playerLevel, Cost cost, bool canAfford)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", (!canAfford) ? "NotEnoughJelly" : "SpendJelly", "PremiumCrafting");
		dictionary["level"] = playerLevel;
		SBAnalytics.AddCost(dictionary, cost);
		TFAnalytics.LogEvent(productName, dictionary);
	}

	// Token: 0x0600031F RID: 799 RVA: 0x0000FBAC File Offset: 0x0000DDAC
	public void LogCollectCraftedGood(int buildingDid, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Interactions", "CollectCraft", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("CollectCraft_{0}", buildingDid), dictionary);
	}

	// Token: 0x06000320 RID: 800 RVA: 0x0000FC00 File Offset: 0x0000DE00
	public void LogCollectRentReward(int buildingDid, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Interactions", "CollectRent", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("CollectRent_{0}", buildingDid), dictionary);
	}

	// Token: 0x06000321 RID: 801 RVA: 0x0000FC54 File Offset: 0x0000DE54
	public void LogCollectVendedReward(int buildingDid, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Interactions", "CollectVend", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("CollectVend_{0}", buildingDid), dictionary);
	}

	// Token: 0x06000322 RID: 802 RVA: 0x0000FCA8 File Offset: 0x0000DEA8
	public void LogPremiumVending(string productName, int playerLevel, Cost cost, bool canAfford)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", (!canAfford) ? "NotEnoughJelly" : "SpendJelly", "PremiumVending");
		dictionary["level"] = playerLevel;
		SBAnalytics.AddCost(dictionary, cost);
		TFAnalytics.LogEvent(productName, dictionary);
	}

	// Token: 0x06000323 RID: 803 RVA: 0x0000FD08 File Offset: 0x0000DF08
	public void LogResourceDrop(string ResourceName, int amount, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Drops", "ResourceDrop", null);
		dictionary["level"] = playerLevel;
		dictionary["value"] = amount;
		TFAnalytics.LogEvent(string.Format("ResourceDrop_{0}", ResourceName), dictionary);
	}

	// Token: 0x06000324 RID: 804 RVA: 0x0000FD68 File Offset: 0x0000DF68
	public void LogBuildingDrop(string BuildingName, int amount, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Drops", "BuildingDrop", null);
		dictionary["level"] = playerLevel;
		dictionary["value"] = amount;
		TFAnalytics.LogEvent(string.Format("BuildingDrop_{0}", BuildingName), dictionary);
	}

	// Token: 0x06000325 RID: 805 RVA: 0x0000FDC8 File Offset: 0x0000DFC8
	public void LogRecipeDrop(string recipeName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "GetRecipe", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(recipeName, dictionary);
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0000FE0C File Offset: 0x0000E00C
	public void LogMovieDrop(string movieName, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "GetMovie", null);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(movieName, dictionary);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0000FE50 File Offset: 0x0000E050
	public void LogPlayMovie(string movieName, ulong timePlayed, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		dictionary["level"] = playerLevel;
		dictionary["value"] = timePlayed;
		this.AddSubtypes(dictionary, "Acquisition", "Interaction", "SkipMovie");
		TFAnalytics.LogEvent(Path.GetFileName(movieName), dictionary);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0000FEB0 File Offset: 0x0000E0B0
	public void LogExpansion(int expansionId, Cost cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Progression", "BuyExpansion", null);
		dictionary["level"] = playerLevel;
		SBAnalytics.AddCost(dictionary, cost);
		TFAnalytics.LogEvent(string.Format("Expansion{0:0000}", expansionId), dictionary);
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0000FF0C File Offset: 0x0000E10C
	public void LogPurchaseProductionSlot(string buildingName, int slotId, Cost cost, bool able, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "MonetizationByLevel", (!able) ? "NotEnoughJelly" : "SpendJelly", "BuyProductionSlot");
		SBAnalytics.AddCost(dictionary, cost);
		dictionary["level"] = playerLevel;
		TFAnalytics.LogEvent(string.Format("{0}_Slot{1}", buildingName, slotId), dictionary);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x0000FF80 File Offset: 0x0000E180
	public void LogInsufficientDialog(string purchaseName, int cost, int playerLevel)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Acquisition", "Dialog", null);
		dictionary["level"] = playerLevel;
		dictionary["value"] = cost;
		TFAnalytics.LogEvent(string.Format("NotEnough_{0}", purchaseName), dictionary);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0000FFE0 File Offset: 0x0000E1E0
	public void LogDialog(string dialogName, string buttonName, double elapsedTimeInMilliseconds, int playerLevel)
	{
		int num = (int)(elapsedTimeInMilliseconds / 100.0 + 0.5);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Acquisition", "Dialog", null);
		dictionary["level"] = playerLevel;
		dictionary["value"] = num;
		string text = string.Format("Dialog_{0}", dialogName);
		if (text.Length > 32)
		{
			text = text.Substring(0, 32);
		}
		TFAnalytics.LogEvent(text, dictionary);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00010070 File Offset: 0x0000E270
	public void LogPlayerInfo(int startingJJ, bool IsOffline, bool firstSession, int level)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "PlayerInfo", null, null);
		dictionary["value"] = startingJJ;
		dictionary["level"] = level;
		TFAnalytics.LogEvent("startingJJ", dictionary);
		dictionary["value"] = TFUtils.BoolToInt(IsOffline);
		if (firstSession)
		{
			TFAnalytics.LogEvent("offline_game_start", dictionary);
		}
		else
		{
			this.AddSubtypes(dictionary, "Acquisition", null, null);
			TFAnalytics.LogEvent("return_game_start", dictionary);
		}
		this.AddSubtypes(dictionary, "PlayerInfo", this.androidDeviceType, null);
		TFAnalytics.LogEvent("iOSDeviceType", dictionary);
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0001012C File Offset: 0x0000E32C
	public void InitGameValues(Game game)
	{
		bool firstSession = true;
		this.LogLevelGold(game.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount, 1);
		this.LogLevelJJ(game.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount, 1);
		this.LogPlayerInfo(game.resourceManager.Resources[ResourceManager.DEFAULT_JJ].Amount, this.isOffline, firstSession, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x000101C0 File Offset: 0x0000E3C0
	public void UpdateGameValues(Game game)
	{
		bool firstSession = false;
		this.LogPlayerInfo(game.resourceManager.Resources[ResourceManager.DEFAULT_JJ].Amount, this.isOffline, firstSession, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00010210 File Offset: 0x0000E410
	public void LogFrameRenderRates(string bucketType, int frameRenderTime)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.AddCommon(dictionary);
		this.AddSubtypes(dictionary, "Performance", "FramePerf", null);
		dictionary["value"] = frameRenderTime;
		TFAnalytics.LogEvent(bucketType, dictionary);
	}

	// Token: 0x040001A2 RID: 418
	private const string PLAYER_ID = "PlayerId";

	// Token: 0x040001A3 RID: 419
	private const string DEVICE_ID = "DeviceId";

	// Token: 0x040001A4 RID: 420
	private const string DEVICE_INFO = "DeviceInfo";

	// Token: 0x040001A5 RID: 421
	private const string OFFLINE = "Offline";

	// Token: 0x040001A6 RID: 422
	private const string JJAMT = "JJAmount";

	// Token: 0x040001A7 RID: 423
	private const string SUBTYPE_1 = "subtype1";

	// Token: 0x040001A8 RID: 424
	private const string SUBTYPE_2 = "subtype2";

	// Token: 0x040001A9 RID: 425
	private const string SUBTYPE_3 = "subtype3";

	// Token: 0x040001AA RID: 426
	private const string LEVEL = "level";

	// Token: 0x040001AB RID: 427
	private const string VALUE = "value";

	// Token: 0x040001AC RID: 428
	private const string COST_TYPE = "CostType";

	// Token: 0x040001AD RID: 429
	private const string CATEGORY_MONETIZATION = "MonetizationByLevel";

	// Token: 0x040001AE RID: 430
	private const string CATEGORY_ACQUISITION = "Acquisition";

	// Token: 0x040001AF RID: 431
	private const string CATEGORY_PROGRESSION = "Progression";

	// Token: 0x040001B0 RID: 432
	private const string CATEGORY_RETENTION = "Retention";

	// Token: 0x040001B1 RID: 433
	private const string CATEGORY_PLAYER = "PlayerInfo";

	// Token: 0x040001B2 RID: 434
	private const string CATEGORY_JJ_ECONOMY = "JJEconomy";

	// Token: 0x040001B3 RID: 435
	private const string CATEGORY_COIN_ECONOMY = "CoinEconomy";

	// Token: 0x040001B4 RID: 436
	private const string CATEGORY_REZ_ECONOMY = "ResourceEconomy";

	// Token: 0x040001B5 RID: 437
	private const string CATEGORY_PERFORMANCE = "Performance";

	// Token: 0x040001B6 RID: 438
	private const string CATEGORY_DROPS = "Drops";

	// Token: 0x040001B7 RID: 439
	private const string CATEGORY_INTERACTIONS = "Interactions";

	// Token: 0x040001B8 RID: 440
	private const string EVENT_SOARING_IN_APP_PURCHASE = "Soaring_IAP";

	// Token: 0x040001B9 RID: 441
	private const string EVENT_IN_APP_PURCHASE = "IAP";

	// Token: 0x040001BA RID: 442
	private const string EVENT_HARDSPEND_CONFIRMATION = "JJMicroConfirm";

	// Token: 0x040001BB RID: 443
	private const string EVENT_SPEND_JELLY = "SpendJelly";

	// Token: 0x040001BC RID: 444
	private const string EVENT_NOT_ENOUGH_JELLY = "NotEnoughJelly";

	// Token: 0x040001BD RID: 445
	private const string EVENT_TUTORIAL = "Tutorial";

	// Token: 0x040001BE RID: 446
	private const string EVENT_QUEST_START = "QuestStart";

	// Token: 0x040001BF RID: 447
	private const string EVENT_QUEST_COMPLETE = "QuestComplete";

	// Token: 0x040001C0 RID: 448
	private const string EVENT_QUEST_COMPLETE_JJAMT = "QuestCompleteJJAmt";

	// Token: 0x040001C1 RID: 449
	private const string EVENT_QUEST_COMPLETE_GOLDAMT = "QuestCompleteGoldAmt";

	// Token: 0x040001C2 RID: 450
	private const string EVENT_QUEST_COMPLETE_SOARING = "quest_completed";

	// Token: 0x040001C3 RID: 451
	private const string EVENT_AUTO_QUEST_START = "autoquest_started";

	// Token: 0x040001C4 RID: 452
	private const string EVENT_AUTO_QUEST_COMPLETE = "autoquest_completed";

	// Token: 0x040001C5 RID: 453
	private const string EVENT_TASK_START = "character_task_started";

	// Token: 0x040001C6 RID: 454
	private const string EVENT_TASK_COMPLETE = "character_task_completed";

	// Token: 0x040001C7 RID: 455
	private const string EVENT_COSTUME_UNLOCK = "change_costume";

	// Token: 0x040001C8 RID: 456
	private const string EVENT_COSTUME_CHANGED = "costume_unlock";

	// Token: 0x040001C9 RID: 457
	private const string EVENT_DAILY_REWARD = "daily_reward";

	// Token: 0x040001CA RID: 458
	private const string EVENT_CHEST_PICKUP = "chest_pickup";

	// Token: 0x040001CB RID: 459
	private const string EVENT_CHARACTER_FEED = "character_feed";

	// Token: 0x040001CC RID: 460
	private const string EVENT_VISIT_PARK = "visit_park";

	// Token: 0x040001CD RID: 461
	private const string EVENT_PROMOTION_EVENT = "PromoEvent";

	// Token: 0x040001CE RID: 462
	private const string EVENT_BUILD = "Build";

	// Token: 0x040001CF RID: 463
	private const string EVENT_DIALOG = "Dialog";

	// Token: 0x040001D0 RID: 464
	private const string EVENT_DECORATION = "Decoration";

	// Token: 0x040001D1 RID: 465
	private const string EVENT_PREMIUM_BUILD = "BuyPremBuild";

	// Token: 0x040001D2 RID: 466
	private const string EVENT_PREMIUM_DECORATION = "BuyPremDeco";

	// Token: 0x040001D3 RID: 467
	private const string EVENT_ACHIEVEMENT = "Achievement";

	// Token: 0x040001D4 RID: 468
	private const string EVENT_LEVEL = "Level";

	// Token: 0x040001D5 RID: 469
	private const string EVENT_LEVEL_GOLD = "LevelGold";

	// Token: 0x040001D6 RID: 470
	private const string EVENT_LEVEL_JJ = "LevelJJ";

	// Token: 0x040001D7 RID: 471
	private const string EVENT_CHARACTER_TASK = "CharacterTask";

	// Token: 0x040001D8 RID: 472
	private const string EVENT_PLAY_TASK = "PlayTask";

	// Token: 0x040001D9 RID: 473
	private const string EVENT_BUILDING_TASK = "BuildingTask";

	// Token: 0x040001DA RID: 474
	private const string EVENT_ANY_TASK = "AnyTask";

	// Token: 0x040001DB RID: 475
	private const string EVENT_CLEAR_DEBRIS = "ClearDebris";

	// Token: 0x040001DC RID: 476
	private const string EVENT_OPEN_SETTINGS = "OpenSettings";

	// Token: 0x040001DD RID: 477
	private const string EVENT_SELL_BUILDING = "SellBuilding";

	// Token: 0x040001DE RID: 478
	private const string EVENT_SELL_DECO = "SellDeco";

	// Token: 0x040001DF RID: 479
	private const string EVENT_CRAFTING = "Crafting";

	// Token: 0x040001E0 RID: 480
	private const string EVENT_CRAFTING_PREMIUM = "PremiumCrafting";

	// Token: 0x040001E1 RID: 481
	private const string EVENT_GET_RECIPE = "GetRecipe";

	// Token: 0x040001E2 RID: 482
	private const string EVENT_GET_MOVIE = "GetMovie";

	// Token: 0x040001E3 RID: 483
	private const string EVENT_EXPANSION = "BuyExpansion";

	// Token: 0x040001E4 RID: 484
	private const string EVENT_PRODUCTION_SLOT = "BuyProductionSlot";

	// Token: 0x040001E5 RID: 485
	private const string EVENT_LOGIN = "Login";

	// Token: 0x040001E6 RID: 486
	private const string EVENT_SESSION_BEGIN = "kontagent_session_begin";

	// Token: 0x040001E7 RID: 487
	private const string EVENT_SOURCE_GOLD = "SourceGold";

	// Token: 0x040001E8 RID: 488
	private const string EVENT_SOURCE_JJ = "SourceJJ";

	// Token: 0x040001E9 RID: 489
	private const string EVENT_SOURCE_REZ = "SourceRez";

	// Token: 0x040001EA RID: 490
	private const string EVENT_SINK_GOLD = "SinkGold";

	// Token: 0x040001EB RID: 491
	private const string EVENT_SINK_JJ = "SinkJJ";

	// Token: 0x040001EC RID: 492
	private const string EVENT_SINK_REZ = "SinkRez";

	// Token: 0x040001ED RID: 493
	private const string EVENT_SINK = "Sink";

	// Token: 0x040001EE RID: 494
	private const string EVENT_DEVICE_TYPE = "iOSDeviceType";

	// Token: 0x040001EF RID: 495
	private const string EVENT_INTERACTION = "Interaction";

	// Token: 0x040001F0 RID: 496
	private const string EVENT_FRAMERATE = "FramePerf";

	// Token: 0x040001F1 RID: 497
	private const string EVENT_RESOURCE_DROP = "ResourceDrop";

	// Token: 0x040001F2 RID: 498
	private const string EVENT_BUILDING_DROP = "BuildingDrop";

	// Token: 0x040001F3 RID: 499
	private const string EVENT_COLLECT_RENT = "CollectRent";

	// Token: 0x040001F4 RID: 500
	private const string EVENT_COLLECT_CRAFT = "CollectCraft";

	// Token: 0x040001F5 RID: 501
	private const string EVENT_COLLECT_VEND = "CollectVend";

	// Token: 0x040001F6 RID: 502
	private const string EVENT_VENDING_PREMIUM = "PremiumVending";

	// Token: 0x040001F7 RID: 503
	private const string EVENT_BUY_EVENT_REWARD = "BuyEventReward";

	// Token: 0x040001F8 RID: 504
	private const string EVENT_ADD_BUILDING_TO_INVENTORY = "BuildingAddedToInventory";

	// Token: 0x040001F9 RID: 505
	private const string EVENT_PLACE_FROM_INVENTORY = "PlaceFromInventory";

	// Token: 0x040001FA RID: 506
	private const string SUBEVENT_REQUEST_IAP = "RequestIap";

	// Token: 0x040001FB RID: 507
	private const string SUBEVENT_SUCCEED_IAP = "SucceedIap";

	// Token: 0x040001FC RID: 508
	private const string SUBEVENT_FAIL_IAP = "FailIap";

	// Token: 0x040001FD RID: 509
	private const string SUBEVENT_CANCEL_IAP = "CancelIap";

	// Token: 0x040001FE RID: 510
	private const string SUBEVENT_SPEED_BUILD = "SpeedBuild";

	// Token: 0x040001FF RID: 511
	private const string SUBEVENT_SPEED_PAY = "SpeedPay";

	// Token: 0x04000200 RID: 512
	private const string SUBEVENT_SPEED_TASK = "SpeedTask";

	// Token: 0x04000201 RID: 513
	private const string SUBEVENT_SPEED_FULLNESS = "SpeedFullness";

	// Token: 0x04000202 RID: 514
	private const string SUBEVENT_SPEED_CRAFT = "SpeedCraft";

	// Token: 0x04000203 RID: 515
	private const string SUBEVENT_SPEED_CLEAR = "SpeedClear";

	// Token: 0x04000204 RID: 516
	private const string SUBEVENT_SPEED_RESTOCK = "SpeedRestock";

	// Token: 0x04000205 RID: 517
	private const string SUBEVENT_BUY_INGREDIENTS = "BuyIngredients";

	// Token: 0x04000206 RID: 518
	private const string SUBEVENT_BUY_PRODUCTION_SLOT = "BuyProductionSlot";

	// Token: 0x04000207 RID: 519
	private const string SUBEVENT_CANT_AFFORD_SPEED_BUILD = "CantAffordSpeedBuild";

	// Token: 0x04000208 RID: 520
	private const string SUBEVENT_CANT_AFFORD_SPEED_PAY = "CantAffordSpeedPay";

	// Token: 0x04000209 RID: 521
	private const string SUBEVENT_CANT_AFFORD_SPEED_TASK = "CantAffordSpeedTask";

	// Token: 0x0400020A RID: 522
	private const string SUBEVENT_CANT_AFFORD_INGREDIENTS = "CantAffordIngredients";

	// Token: 0x0400020B RID: 523
	private const string SUBEVENT_FIRST_LOGIN_OF_DAY = "FirstLoginOfDay";

	// Token: 0x0400020C RID: 524
	private const string SUBEVENT_SECOND_LOGIN_OF_DAY = "SecondLoginOfDay";

	// Token: 0x0400020D RID: 525
	private const string SUBEVENT_MORE_LOGIN_OF_DAY = "MoreLoginOfDay";

	// Token: 0x0400020E RID: 526
	private const string SUBEVENT_SOURCE_GOLD = "SourceGold";

	// Token: 0x0400020F RID: 527
	private const string SUBEVENT_SOURCE_JJ = "SourceJJ";

	// Token: 0x04000210 RID: 528
	private const string SUBEVENT_SINK_GOLD = "SinkGold";

	// Token: 0x04000211 RID: 529
	private const string SUBEVENT_SINK_JJ = "SinkJJ";

	// Token: 0x04000212 RID: 530
	private const string SUBEVENT_SINK_REZ = "Sink";

	// Token: 0x04000213 RID: 531
	private const string SUBEVENT_SOURCE_REZ = "Source";

	// Token: 0x04000214 RID: 532
	private const string SUBEVENT_MOVE_OBJECT = "MoveObject";

	// Token: 0x04000215 RID: 533
	private const string SUBEVENT_PLAY_MOVIE = "SkipMovie";

	// Token: 0x04000216 RID: 534
	private const string QUEST_UID = "QuestUID";

	// Token: 0x04000217 RID: 535
	private const string QUEST_NAME = "QuestName";

	// Token: 0x04000218 RID: 536
	private const string TASK_SRC_UNIT = "SrcUnit";

	// Token: 0x04000219 RID: 537
	private const string TASK_NAME = "TaskName";

	// Token: 0x0400021A RID: 538
	private const string DIALOG_DURATION = "Duration";

	// Token: 0x0400021B RID: 539
	private LoadingFunnel loadingFunnel;

	// Token: 0x0400021C RID: 540
	private string deviceId;

	// Token: 0x0400021D RID: 541
	private string deviceInfo;

	// Token: 0x0400021E RID: 542
	private string playerId;

	// Token: 0x0400021F RID: 543
	private bool isOffline;

	// Token: 0x04000220 RID: 544
	private int jjamount;
}
