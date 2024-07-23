using System;
using System.Collections.Generic;
using DeltaDNA;

// Token: 0x020001C5 RID: 453
public class SBMIDeltaDNA
{
	// Token: 0x06000F84 RID: 3972 RVA: 0x00063DF8 File Offset: 0x00061FF8
	public static void LogMissionStart(Game pGame, SBMIDeltaDNA.MissionObject pMission)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		pMission.AddToDict(eventBuilder, null, false);
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogMissionStart: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("missionStarted", eventBuilder);
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x00063E60 File Offset: 0x00062060
	public static void LogMissionComplete(Game pGame, SBMIDeltaDNA.MissionObject pMission, SBMIDeltaDNA.RewardObject pReward)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		pMission.AddToDict(eventBuilder, null, false);
		if (pReward != null)
		{
			pReward.AddToDict(eventBuilder, null, true);
		}
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogMissionComplete: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("missionCompleted", eventBuilder);
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x00063ED8 File Offset: 0x000620D8
	public static void LogLevelUp(Game pGame, SBMIDeltaDNA.RewardObject pReward, int nLevel)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("levelUpName", nLevel.ToString());
		if (pReward != null)
		{
			pReward.AddToDict(eventBuilder, null, true);
		}
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogLevelUp: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("levelUp", eventBuilder);
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x00063F58 File Offset: 0x00062158
	public static void LogWishGranted(Game pGame, string sWishName, SBMIDeltaDNA.RewardObject pReward)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("wishName", sWishName);
		if (pReward != null)
		{
			pReward.AddToDict(eventBuilder, null, true);
		}
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogWishGranted: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("wishGranted", eventBuilder);
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x00063FD4 File Offset: 0x000621D4
	public static void LogUIInteraction(Game pGame, string sUIName, string sType, string sAction)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("UIName", sUIName);
		eventBuilder.AddParam("UIAction", sAction);
		eventBuilder.AddParam("UIType", sType);
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogUIInteraction: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("uiInteraction", eventBuilder);
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x0006405C File Offset: 0x0006225C
	public static void LogFeatureUnlocked(Game pGame, string sFeatureName, string sFeatureType)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("featureName", sFeatureName);
		eventBuilder.AddParam("featureType", sFeatureType);
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogFeatureUnlocked: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("featureUnlocked", eventBuilder);
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x000640D4 File Offset: 0x000622D4
	public static void LogShopEntered(Game pGame, string sShopName)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("shopName", sShopName);
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogShopEntered: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("shopEntered", eventBuilder);
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x00064140 File Offset: 0x00062340
	public static void LogTransaction(Game pGame, SBMIDeltaDNA.RewardObject pSpent, SBMIDeltaDNA.RewardObject pRecieved, SBMIDeltaDNA.TransactionObject pTransaction, string sTransactionType, string sTransactionName)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("transactionType", sTransactionType);
		eventBuilder.AddParam("transactionName", sTransactionName);
		if (pSpent != null)
		{
			pSpent.AddToDict(eventBuilder, null, false);
		}
		if (pRecieved != null)
		{
			pRecieved.AddToDict(eventBuilder, null, false);
		}
		if (pTransaction != null)
		{
			pTransaction.AddToDict(eventBuilder, null, false);
		}
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogTransaction: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("transaction", eventBuilder);
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x000641E8 File Offset: 0x000623E8
	public static void LogTransaction(Game pGame, int jellyfishJellyCost, int itemDID, string itemName, string itemType, string sTransactionType, string sTransactionName)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		eventBuilder.AddParam("transactionType", sTransactionType);
		eventBuilder.AddParam("transactionName", sTransactionName);
		eventBuilder.AddParam("productsSpent", new ProductBuilder().AddItem("Jellyfish Jelly", "resource", jellyfishJellyCost));
		eventBuilder.AddParam("productsReceived", new ProductBuilder().AddItem(itemName, itemType, 1));
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogTransaction: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("transaction", eventBuilder);
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x000642A0 File Offset: 0x000624A0
	public static void LogItemCollected(Game pGame, SBMIDeltaDNA.RewardObject pReward)
	{
		EventBuilder eventBuilder = new EventBuilder();
		pGame.GetDeltaDNAPlayerObject().AddToDict(eventBuilder, null, false);
		if (pReward != null)
		{
			pReward.AddToDict(eventBuilder, null, true);
		}
		if (SBMIDeltaDNA._bDEBUG_LOG)
		{
			SoaringDictionary soaringDictionary = SBMISoaring.ConvertDictionary(eventBuilder.ToDictionary());
			string str = soaringDictionary.ToJsonString();
			TFUtils.ErrorLog("SBMIDeltaDNA.cs | LogItemCollected: " + str);
		}
		Singleton<SDK>.Instance.RecordEvent("itemCollected", eventBuilder);
	}

	// Token: 0x04000A79 RID: 2681
	private static bool _bDEBUG_LOG;

	// Token: 0x020001C6 RID: 454
	public abstract class Object
	{
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000F8F RID: 3983 RVA: 0x00064324 File Offset: 0x00062524
		// (set) Token: 0x06000F90 RID: 3984 RVA: 0x0006432C File Offset: 0x0006252C
		public string m_sKey { get; protected set; }

		// Token: 0x06000F91 RID: 3985 RVA: 0x00064338 File Offset: 0x00062538
		public void AddToDict(EventBuilder pEventBuilder, string sOverrideKey = null, bool bNested = true)
		{
			if (bNested)
			{
				string text = string.IsNullOrEmpty(sOverrideKey) ? this.m_sKey : sOverrideKey;
				if (this.m_pEventBuilder != null && pEventBuilder != null && !string.IsNullOrEmpty(text))
				{
					pEventBuilder.AddParam(text, this.m_pEventBuilder.ToDictionary());
				}
			}
			else
			{
				Dictionary<string, object> dictionary = this.m_pEventBuilder.ToDictionary();
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					pEventBuilder.AddParam(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x04000A7A RID: 2682
		protected EventBuilder m_pEventBuilder = new EventBuilder();
	}

	// Token: 0x020001C7 RID: 455
	public class DeviceObject : SBMIDeltaDNA.Object
	{
		// Token: 0x06000F92 RID: 3986 RVA: 0x00064408 File Offset: 0x00062608
		public DeviceObject(string sObjectKey, string sDeviceName, string sDeviceType, string sHardwareVersion, string sOS, string sOSVersion, string sManufacturer, string sTimezoneOffset, string sUserLanguage)
		{
			base.m_sKey = sObjectKey;
			this.m_pEventBuilder.AddParam("deviceName", sDeviceName);
			this.m_pEventBuilder.AddParam("deviceType", sDeviceType);
			this.m_pEventBuilder.AddParam("hardwareVersion", sHardwareVersion);
			this.m_pEventBuilder.AddParam("operatingSystem", sOS);
			this.m_pEventBuilder.AddParam("operatingSystemVersion", sOSVersion);
			this.m_pEventBuilder.AddParam("manufacturer", sManufacturer);
			this.m_pEventBuilder.AddParam("timezoneOffset", sTimezoneOffset);
			this.m_pEventBuilder.AddParam("userLanguage", sUserLanguage);
		}
	}

	// Token: 0x020001C8 RID: 456
	public class PlayerObject : SBMIDeltaDNA.Object
	{
		// Token: 0x06000F93 RID: 3987 RVA: 0x000644B8 File Offset: 0x000626B8
		public PlayerObject(string sObjectKey, int nLevel, int nXP, int nHardCurrency, int nSoftCurrency)
		{
			base.m_sKey = sObjectKey;
			this.m_pEventBuilder.AddParam("userLevel", nLevel);
			this.m_pEventBuilder.AddParam("userXP", nXP);
			this.m_pEventBuilder.AddParam("userJelly", nHardCurrency);
			this.m_pEventBuilder.AddParam("userCoins", nSoftCurrency);
		}
	}

	// Token: 0x020001C9 RID: 457
	public class MissionObject : SBMIDeltaDNA.Object
	{
		// Token: 0x06000F94 RID: 3988 RVA: 0x00064530 File Offset: 0x00062730
		public MissionObject(string sObjectKey, string sMissionName, string sMissionType, int nMissionID)
		{
			this.SetBaseData(sObjectKey, sMissionName, sMissionType, nMissionID);
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00064544 File Offset: 0x00062744
		public MissionObject(string sObjectKey, string sMissionName, string sMissionType, int nMissionID, ulong ulMissionDuration)
		{
			this.SetBaseData(sObjectKey, sMissionName, sMissionType, nMissionID);
			this.m_pEventBuilder.AddParam("missionDuration", ulMissionDuration);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00064570 File Offset: 0x00062770
		private void SetBaseData(string sObjectKey, string sMissionName, string sMissionType, int nMissionID)
		{
			base.m_sKey = sObjectKey;
			this.m_pEventBuilder.AddParam("missionName", sMissionName);
			this.m_pEventBuilder.AddParam("missionID", nMissionID.ToString());
			this.m_pEventBuilder.AddParam("missionType", sMissionType);
		}
	}

	// Token: 0x020001CA RID: 458
	public class RewardObject : SBMIDeltaDNA.Object
	{
		// Token: 0x06000F97 RID: 3991 RVA: 0x000645C0 File Offset: 0x000627C0
		public RewardObject(string sObjectKey, string sRewardName, Reward pReward, Game pGame, int nRealCurrencyAmount = -1, string sRealCurrencyType = null, string sTypeOverride = null)
		{
			base.m_sKey = sObjectKey;
			ProductBuilder productBuilder = new ProductBuilder();
			if (pReward != null)
			{
				Dictionary<int, int> resourceAmounts = pReward.ResourceAmounts;
				Dictionary<int, int> buildingAmounts = pReward.BuildingAmounts;
				List<int> costumeUnlocks = pReward.CostumeUnlocks;
				List<int> recipeUnlocks = pReward.RecipeUnlocks;
				List<int> clearedLands = pReward.ClearedLands;
				foreach (KeyValuePair<int, int> keyValuePair in resourceAmounts)
				{
					Resource resource = (!pGame.resourceManager.Resources.ContainsKey(keyValuePair.Key)) ? null : pGame.resourceManager.Resources[keyValuePair.Key];
					if (resource != null)
					{
						productBuilder.AddItem(resource.Name, string.IsNullOrEmpty(sTypeOverride) ? "resource" : sTypeOverride, keyValuePair.Value);
					}
				}
				foreach (KeyValuePair<int, int> keyValuePair2 in buildingAmounts)
				{
					Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, keyValuePair2.Key, true);
					if (blueprint != null)
					{
						productBuilder.AddItem((string)blueprint.Invariable["name"], string.IsNullOrEmpty(sTypeOverride) ? "building" : sTypeOverride, keyValuePair2.Value);
					}
				}
				int count = costumeUnlocks.Count;
				for (int i = 0; i < count; i++)
				{
					CostumeManager.Costume costume = pGame.costumeManager.GetCostume(costumeUnlocks[i]);
					if (costume != null)
					{
						productBuilder.AddItem(costume.m_sName, string.IsNullOrEmpty(sTypeOverride) ? "costume" : sTypeOverride, 1);
					}
				}
				count = recipeUnlocks.Count;
				for (int j = 0; j < count; j++)
				{
					CraftingRecipe recipeById = pGame.craftManager.GetRecipeById(recipeUnlocks[j]);
					if (recipeById != null)
					{
						productBuilder.AddItem(recipeById.recipeName, string.IsNullOrEmpty(sTypeOverride) ? "recipe" : sTypeOverride, 1);
					}
				}
				count = clearedLands.Count;
				for (int k = 0; k < count; k++)
				{
					productBuilder.AddItem(clearedLands[k].ToString(), string.IsNullOrEmpty(sTypeOverride) ? "expansion" : sTypeOverride, 1);
				}
			}
			if (!string.IsNullOrEmpty(sRealCurrencyType) && nRealCurrencyAmount > 0)
			{
				productBuilder.AddRealCurrency(sRealCurrencyType, nRealCurrencyAmount);
			}
			this.m_pEventBuilder.AddParam(sRewardName, productBuilder);
		}
	}

	// Token: 0x020001CB RID: 459
	public class TransactionObject : SBMIDeltaDNA.Object
	{
		// Token: 0x06000F98 RID: 3992 RVA: 0x000648C8 File Offset: 0x00062AC8
		public TransactionObject(string sObjectKey, string sTransactorID, string sTransactionServer, string sTransactionReceipt, string sProductID, string sTransactionID, bool bIsInitiator)
		{
			base.m_sKey = sObjectKey;
			this.m_pEventBuilder.AddParam("transactorID", sTransactorID);
			this.m_pEventBuilder.AddParam("transactionServer", sTransactionServer);
			this.m_pEventBuilder.AddParam("transactionReceipt", sTransactionReceipt);
			this.m_pEventBuilder.AddParam("productID", sProductID);
			this.m_pEventBuilder.AddParam("transactionID", sTransactionID);
			this.m_pEventBuilder.AddParam("isInitiator", bIsInitiator);
		}
	}
}
