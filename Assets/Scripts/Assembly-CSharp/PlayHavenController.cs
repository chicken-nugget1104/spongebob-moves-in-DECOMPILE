using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000047 RID: 71
public class PlayHavenController
{
	// Token: 0x060002D3 RID: 723 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
	public PlayHavenController()
	{
		this.namesToResource = new Dictionary<string, string>();
		this.namesToResource["_gold"] = ResourceManager.SOFT_CURRENCY.ToString();
		this.namesToResource["_jelly"] = ResourceManager.HARD_CURRENCY.ToString();
		this.namesToResource["_xp"] = ResourceManager.XP.ToString();
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0000E13C File Offset: 0x0000C33C
	public void Initialize(Session session)
	{
		this.session = session;
		session.TheGame.store.PurchaseError += this.OnPurchaseError;
		session.TheGame.store.PurchaseReceiptReceived += this.OnPurchaseReceiptReceived;
		UpsightManager.unlockedRewardEvent += this.OnRewardGiven;
		UpsightManager.makePurchaseEvent += this.OnVirtualGoodsPromotionClicked;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0000E1AC File Offset: 0x0000C3AC
	public void RequestContent(string placement)
	{
		TFUtils.DebugLog("Requesting placement to Playhaven " + placement);
		Upsight.sendContentRequest(placement, false, true, null);
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0000E1C8 File Offset: 0x0000C3C8
	public void OnRewardGiven(UpsightReward reward)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
		if (this.namesToResource.ContainsKey(reward.name))
		{
			string key = this.namesToResource[reward.name];
			dictionary[key] = reward.quantity;
		}
		this.PopulateRewardDict("_building_", dictionary2, reward.name, reward.quantity);
		this.PopulateRewardDict("_movie_", dictionary4, reward.name, reward.quantity);
		this.PopulateRewardDict("_recipe_", dictionary3, reward.name, reward.quantity);
		Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
		dictionary5["resources"] = dictionary;
		dictionary5["recipes"] = dictionary3;
		dictionary5["movies"] = dictionary4;
		dictionary5["buildings"] = dictionary2;
		if (this.session != null && this.session.TheGame != null)
		{
			RewardDefinition rewardDefinition = RewardDefinition.FromDict(dictionary5);
			Reward reward2 = rewardDefinition.GenerateReward(this.session.TheGame.simulation, false);
			foreach (KeyValuePair<int, int> keyValuePair in reward2.BuildingAmounts)
			{
				int key2 = keyValuePair.Key;
				Blueprint blueprint = EntityManager.GetBlueprint("building", key2, true);
				int? num = null;
				if (blueprint != null)
				{
					num = blueprint.GetInstanceLimitByLevel(this.session.TheGame.resourceManager.PlayerLevelAmount);
				}
				if (num != null)
				{
					int num2 = 0;
					List<Simulated> list = this.session.TheGame.simulation.FindAllSimulateds(key2, null);
					foreach (Simulated simulated in list)
					{
						if ((simulated.Entity.AllTypes & EntityType.BUILDING) != EntityType.INVALID)
						{
							num2++;
						}
					}
					foreach (SBInventoryItem sbinventoryItem in this.session.TheGame.inventory.GetItems())
					{
						if (sbinventoryItem.entity.DefinitionId == key2)
						{
							num2++;
						}
					}
					if (num2 > num.Value)
					{
						Debug.LogError("Cannot add another instance of this building since instance limit of " + num.Value + " has been reached!");
						reward2.BuildingAmounts[keyValuePair.Key] = 0;
					}
				}
			}
			this.session.TheGame.ApplyReward(reward2, TFUtils.EpochTime(), false);
			this.session.TheGame.ModifyGameState(new ReceiveRewardAction(reward2, reward.name));
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000E514 File Offset: 0x0000C714
	private void PopulateRewardDict(string prefix, Dictionary<string, object> dict, string rewardName, int quantity)
	{
		if (rewardName.StartsWith(prefix))
		{
			string key = rewardName.Substring(prefix.Length);
			dict[key] = quantity;
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0000E548 File Offset: 0x0000C748
	public void OnPurchaseError(object sender, RmtStore.StoreEventArgs args)
	{
		string text = "OnPurchaseError | ";
		Dictionary<string, object> results = args.results;
		foreach (KeyValuePair<string, object> keyValuePair in results)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				" key: ",
				keyValuePair.Key,
				" value: ",
				keyValuePair.Value.ToString()
			});
		}
		TFUtils.DebugLog(text);
		string productId = (string)args.results["productId"];
		bool flag = (string)args.results["reason"] == "userCancelled";
		if (flag)
		{
			this.PurchaseItem(productId, 1, null, PlayHavenController.PurchaseResolution.CANCEL, null);
		}
		else
		{
			this.PurchaseItem(productId, 1, null, PlayHavenController.PurchaseResolution.ERROR, null);
		}
	}

	// Token: 0x060002DA RID: 730 RVA: 0x0000E650 File Offset: 0x0000C850
	public void OnPurchaseReceiptReceived(object sender, RmtStore.StoreEventArgs args)
	{
		string text = (string)args.results["productId"];
		string receipt = (string)args.results["receipt"];
		string transactionID;
		if (args.results.ContainsKey("transactionId"))
		{
			transactionID = (string)args.results["transactionId"];
		}
		else
		{
			transactionID = text;
		}
		this.PurchaseItem(text, 1, receipt, PlayHavenController.PurchaseResolution.BUY, transactionID);
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
	public void PurchaseItem(string productId, int quantity, string receipt, PlayHavenController.PurchaseResolution resolution, string transactionID)
	{
		TFUtils.DebugLog(string.Concat(new object[]
		{
			"Tracking a purchase item for ",
			productId,
			" with resolution ",
			resolution
		}));
		UpsightAndroidPurchaseResolution resolutionType = UpsightAndroidPurchaseResolution.Error;
		switch (resolution)
		{
		case PlayHavenController.PurchaseResolution.BUY:
			resolutionType = UpsightAndroidPurchaseResolution.Bought;
			break;
		case PlayHavenController.PurchaseResolution.CANCEL:
			resolutionType = UpsightAndroidPurchaseResolution.Cancelled;
			break;
		case PlayHavenController.PurchaseResolution.ERROR:
			resolutionType = UpsightAndroidPurchaseResolution.Error;
			break;
		}
		Upsight.trackInAppPurchase(productId, quantity, resolutionType, 0.0, transactionID, null);
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0000E748 File Offset: 0x0000C948
	public void OnVirtualGoodsPromotionClicked(UpsightPurchase purchase)
	{
		TFUtils.DebugLog(string.Concat(new object[]
		{
			"Virtual goods promotion clicked:",
			purchase.quantity,
			" ",
			purchase.productIdentifier
		}));
		this.session.PurchasePremiumProduct(purchase.productIdentifier);
	}

	// Token: 0x04000185 RID: 389
	public const string MORE_NICK_PLACEMENT = "more_nick_click";

	// Token: 0x04000186 RID: 390
	public const string FIRST_TIME_APP_START_PLACEMENT = "first_time_app_start";

	// Token: 0x04000187 RID: 391
	public const string APP_START_PLACEMENT = "app_start";

	// Token: 0x04000188 RID: 392
	public const string APP_RESUME_PLACEMENT = "app_resume";

	// Token: 0x04000189 RID: 393
	public const string LOADING_SCREEN_END_PLACEMENT = "loading_screen_end";

	// Token: 0x0400018A RID: 394
	public const string SHOP_OPEN_PLACEMENT = "shop_open";

	// Token: 0x0400018B RID: 395
	public const string LEVEL_PLACEMENT = "level_";

	// Token: 0x0400018C RID: 396
	public const string LOW_BALANCE_COINS_PLACEMENT = "low_balance_coins";

	// Token: 0x0400018D RID: 397
	public const string LOW_BALANCE_JJ_PLACEMENT = "low_balance_jellyfish_jelly";

	// Token: 0x0400018E RID: 398
	public const string PAYMIUM_END_TUTORIAL_PAYMIUM_ITEM_IN_INVENTORY = "end_tutorial_paymium_item_in_inventory";

	// Token: 0x0400018F RID: 399
	public const string PIRATE_BOOTY_GAME_INITIALIZED_NO_SHIP = "loading_screen_end_existingplayer_no_ship";

	// Token: 0x04000190 RID: 400
	public const string PIRATE_BOOTY_GAME_INITIALIZED_HAS_SHIP = "loading_screen_end_existingplayer_with_ship";

	// Token: 0x04000191 RID: 401
	public const int LOW_BALANCE_COINS_THRESHOLD = 100;

	// Token: 0x04000192 RID: 402
	public const int LOW_BALANCE_JJ_THRESHOLD = 20;

	// Token: 0x04000193 RID: 403
	public const string DASHBOARD_RESOURCE_JELLY = "_jelly";

	// Token: 0x04000194 RID: 404
	public const string DASHBOARD_RESOURCE_GOLD = "_gold";

	// Token: 0x04000195 RID: 405
	public const string DASHBOARD_RESOURCE_XP = "_xp";

	// Token: 0x04000196 RID: 406
	public const string DASHBOARD_BUILDING_PREFIX = "_building_";

	// Token: 0x04000197 RID: 407
	public const string DASHBOARD_RECIPE_PREFIX = "_recipe_";

	// Token: 0x04000198 RID: 408
	public const string DASHBOARD_MOVIE_PREFIX = "_movie_";

	// Token: 0x04000199 RID: 409
	public static int? PAYMIUM_ITEM_DID = new int?(9010);

	// Token: 0x0400019A RID: 410
	public static int? PIRATE_BOOTY_SHIP_DID = new int?(9011);

	// Token: 0x0400019B RID: 411
	public Dictionary<string, string> namesToResource = new Dictionary<string, string>();

	// Token: 0x0400019C RID: 412
	private Session session;

	// Token: 0x02000048 RID: 72
	public enum PurchaseResolution
	{
		// Token: 0x0400019E RID: 414
		BUY,
		// Token: 0x0400019F RID: 415
		CANCEL,
		// Token: 0x040001A0 RID: 416
		ERROR
	}
}
