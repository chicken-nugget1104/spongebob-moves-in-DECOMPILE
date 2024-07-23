using System;
using System.Collections.Generic;
using System.IO;
using com.amazon.device.iap.cpt;
using MiniJSON;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class RmtStore
{
	// Token: 0x06000997 RID: 2455 RVA: 0x0003BC68 File Offset: 0x00039E68
	public RmtStore(bool rmtEnabled, Dictionary<string, Dictionary<string, object>> pendingTransactions)
	{
		this.rmtEnabled = rmtEnabled;
		if (this.rmtEnabled)
		{
			string bundleIdentifier = SBSettings.BundleIdentifier;
			if (bundleIdentifier != null && (bundleIdentifier.StartsWith("com.tinyfunstudios.") || bundleIdentifier.StartsWith("com.kungfufactory.")))
			{
				this.rmtEnabled = false;
			}
		}
		this.pendingTransactions = pendingTransactions;
	}

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000999 RID: 2457 RVA: 0x0003BD08 File Offset: 0x00039F08
	// (remove) Token: 0x0600099A RID: 2458 RVA: 0x0003BD24 File Offset: 0x00039F24
	public event RmtStore.StoreEventHandler ProductInfoReceived;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x0600099B RID: 2459 RVA: 0x0003BD40 File Offset: 0x00039F40
	// (remove) Token: 0x0600099C RID: 2460 RVA: 0x0003BD5C File Offset: 0x00039F5C
	public event RmtStore.StoreEventHandler PurchaseUpdateReceived;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x0600099D RID: 2461 RVA: 0x0003BD78 File Offset: 0x00039F78
	// (remove) Token: 0x0600099E RID: 2462 RVA: 0x0003BD94 File Offset: 0x00039F94
	public event RmtStore.StoreEventHandler PurchaseResponseReceived;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x0600099F RID: 2463 RVA: 0x0003BDB0 File Offset: 0x00039FB0
	// (remove) Token: 0x060009A0 RID: 2464 RVA: 0x0003BDCC File Offset: 0x00039FCC
	public event RmtStore.StoreEventHandler GetProductInfoResponseReceived;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x060009A1 RID: 2465 RVA: 0x0003BDE8 File Offset: 0x00039FE8
	// (remove) Token: 0x060009A2 RID: 2466 RVA: 0x0003BE04 File Offset: 0x0003A004
	public event RmtStore.StoreEventHandler PurchaseReceiptReceived;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x060009A3 RID: 2467 RVA: 0x0003BE20 File Offset: 0x0003A020
	// (remove) Token: 0x060009A4 RID: 2468 RVA: 0x0003BE3C File Offset: 0x0003A03C
	public event RmtStore.StoreEventHandler PurchaseInfoReceived;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x060009A5 RID: 2469 RVA: 0x0003BE58 File Offset: 0x0003A058
	// (remove) Token: 0x060009A6 RID: 2470 RVA: 0x0003BE74 File Offset: 0x0003A074
	public event RmtStore.StoreEventHandler PurchaseError;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x060009A7 RID: 2471 RVA: 0x0003BE90 File Offset: 0x0003A090
	// (remove) Token: 0x060009A8 RID: 2472 RVA: 0x0003BEAC File Offset: 0x0003A0AC
	public event RmtStore.StoreEventHandler PurchaseDefered;

	// Token: 0x060009A9 RID: 2473 RVA: 0x0003BEC8 File Offset: 0x0003A0C8
	public void OnProductInfoReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.ProductInfoReceived != null)
		{
			RmtStore.StoreEventArgs args = new RmtStore.StoreEventArgs(results);
			this.ProductInfoReceived(this, args);
		}
		else
		{
			Debug.LogError("RMTStore Error: No ProductInfoReceived Function");
		}
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0003BF04 File Offset: 0x0003A104
	public void OnPurchaseUpdateReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseUpdateReceived != null)
		{
			RmtStore.StoreEventArgs args = new RmtStore.StoreEventArgs(results);
			this.PurchaseUpdateReceived(this, args);
		}
		else
		{
			Debug.LogError("RMTStore Error: No OnPurchaseUpdateReceived Function");
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0003BF40 File Offset: 0x0003A140
	public void OnPurchaseReceiptReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseReceiptReceived != null)
		{
			RmtStore.StoreEventArgs args = new RmtStore.StoreEventArgs(results);
			this.PurchaseReceiptReceived(this, args);
		}
		else
		{
			Debug.LogError("RMTStore Error: No OnPurchaseReceiptReceived Function");
		}
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0003BF7C File Offset: 0x0003A17C
	public void OnPurchaseResponseReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseResponseReceived != null)
		{
			RmtStore.StoreEventArgs args = new RmtStore.StoreEventArgs(results);
			this.PurchaseResponseReceived(this, args);
		}
		else
		{
			Debug.LogError("RMTStore Error: No OnPurchaseResponseReceived Function");
		}
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x0003BFB8 File Offset: 0x0003A1B8
	public void OnGetProductInfoResponseReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.GetProductInfoResponseReceived != null)
		{
			RmtStore.StoreEventArgs args = new RmtStore.StoreEventArgs(results);
			this.GetProductInfoResponseReceived(this, args);
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0003BFE4 File Offset: 0x0003A1E4
	public void OnPurchaseInfoReceived(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseInfoReceived != null)
		{
			RmtStore.StoreEventArgs args = new RmtStore.StoreEventArgs(results);
			this.PurchaseInfoReceived(this, args);
		}
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0003C010 File Offset: 0x0003A210
	public void OnPurchaseError(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseError != null)
		{
			RmtStore.StoreEventArgs args = new RmtStore.StoreEventArgs(results);
			this.PurchaseError(this, args);
		}
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0003C03C File Offset: 0x0003A23C
	public void OnPurchaseDefered(Dictionary<string, object> results, object userDarta)
	{
		if (this.PurchaseDefered != null)
		{
			RmtStore.StoreEventArgs args = new RmtStore.StoreEventArgs(results);
			this.PurchaseDefered(this, args);
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0003C068 File Offset: 0x0003A268
	public static bool PreloadRmtProducts(Session session)
	{
		Soaring.RequestProducts(RmtStore.GetStoreName(), "en", RmtStore.HandleProductsDelegate.CreateDelegate(session, "RequestProducts", null, null));
		return true;
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0003C088 File Offset: 0x0003A288
	public static Cost CostFromCollection(Session session, List<object> sales, string field)
	{
		Cost cost = new Cost();
		foreach (object obj in sales)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			if (TFUtils.LoadBoolAsInt(dictionary, "confirmed"))
			{
				string text = (string)dictionary["product_id"];
				Dictionary<string, object> offerByCode = session.TheGame.catalog.GetOfferByCode(text);
				if (offerByCode != null)
				{
					Dictionary<string, object> dict = (Dictionary<string, object>)offerByCode[field];
					cost += Cost.FromDict(dict);
				}
				else
				{
					TFUtils.ErrorLog("Failed to find offer for " + text);
				}
			}
			else
			{
				string str = (string)dictionary["product_id"];
				TFUtils.DebugLog("Skipping unconfirmed sale of " + str);
			}
		}
		return cost;
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0003C184 File Offset: 0x0003A384
	public static RmtStore LoadFromFilesystem(bool rmtEnabled)
	{
		if (!TFUtils.FileIsExists(RmtStore.TRANSACTION_LOG))
		{
			using (FileStream fileStream = File.Create(RmtStore.TRANSACTION_LOG))
			{
				fileStream.Close();
			}
		}
		string[] array = File.ReadAllLines(RmtStore.TRANSACTION_LOG);
		Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>();
		for (int i = 0; i < array.Length; i++)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)Json.Deserialize(array[i]);
			dictionary.Add((string)dictionary2["transactionId"], dictionary2);
		}
		return new RmtStore(rmtEnabled, dictionary);
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0003C234 File Offset: 0x0003A434
	public void Init(Session session)
	{
		TFUtils.DebugLog("Initializing RMT Store");
		this.ProductInfoReceived = (RmtStore.StoreEventHandler)Delegate.Combine(this.ProductInfoReceived, new RmtStore.StoreEventHandler(delegate(object sender, RmtStore.StoreEventArgs args)
		{
			RmtStore.HandleProductInfo(session, args);
		}));
		this.PurchaseInfoReceived = (RmtStore.StoreEventHandler)Delegate.Combine(this.PurchaseInfoReceived, new RmtStore.StoreEventHandler(delegate(object sender, RmtStore.StoreEventArgs args)
		{
			RmtStore.HandlePurchaseInfo(session, args);
		}));
		this.PurchaseUpdateReceived = (RmtStore.StoreEventHandler)Delegate.Combine(this.PurchaseUpdateReceived, new RmtStore.StoreEventHandler(delegate(object sender, RmtStore.StoreEventArgs args)
		{
			RmtStore.HandlePurchaseUpdate(session, args);
		}));
		this.PurchaseResponseReceived = (RmtStore.StoreEventHandler)Delegate.Combine(this.PurchaseResponseReceived, new RmtStore.StoreEventHandler(delegate(object sender, RmtStore.StoreEventArgs args)
		{
			RmtStore.HandlePurchaseResponse(session, args);
		}));
		this.GetProductInfoResponseReceived = (RmtStore.StoreEventHandler)Delegate.Combine(this.GetProductInfoResponseReceived, new RmtStore.StoreEventHandler(delegate(object sender, RmtStore.StoreEventArgs args)
		{
			RmtStore.HandleGetProductInfoResponse(session, args);
		}));
		session.RegisterExternalCallback("productInfo", new TFServer.JsonResponseHandler(this.OnProductInfoReceived));
		session.RegisterExternalCallback("purchaseUpdate", new TFServer.JsonResponseHandler(this.OnPurchaseUpdateReceived));
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x0003C33C File Offset: 0x0003A53C
	public void Start()
	{
		TFBilling.InitializeStore();
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x0003C344 File Offset: 0x0003A544
	public void Reset(Session session)
	{
		TFBilling.ResetStore();
		session.unregisterExternalCallback("productInfo", new TFServer.JsonResponseHandler(this.OnProductInfoReceived));
		session.unregisterExternalCallback("purchaseUpdate", new TFServer.JsonResponseHandler(this.OnPurchaseUpdateReceived));
		this.ProductInfoReceived = null;
		this.PurchaseInfoReceived = null;
		this.PurchaseUpdateReceived = null;
		this.PurchaseResponseReceived = null;
		this.GetProductInfoResponseReceived = null;
		this.txProductId = null;
		RmtStore.IsPurchasing = false;
		this.pendingTransactions.Clear();
		this.receivedProductInfo = false;
		this.receivedPurchaseInfo = false;
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x0003C3D0 File Offset: 0x0003A5D0
	public bool LoadRmtProductInfo(Catalog catalog, Dictionary<string, object> rawRmtProductInfo)
	{
		List<object> list = (List<object>)rawRmtProductInfo["products"];
		TFUtils.DebugLog("Premium product info: " + list.Count);
		if (list.Count == 0)
		{
			return false;
		}
		this.rmtProducts = new Dictionary<string, RmtProduct>();
		foreach (object obj in list)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)obj;
			RmtProduct rmtProduct = new RmtProduct(data);
			this.rmtProducts[rmtProduct.productId] = rmtProduct;
		}
		return true;
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x0003C48C File Offset: 0x0003A68C
	public bool LoadRmtProductInfo(SoaringPurchasable[] pPurchasables)
	{
		this.soaringProducts = new Dictionary<string, SoaringPurchasable>();
		if (pPurchasables == null)
		{
			return false;
		}
		int num = pPurchasables.Length;
		for (int i = 0; i < num; i++)
		{
			SoaringPurchasable soaringPurchasable = pPurchasables[i];
			this.soaringProducts.Add(soaringPurchasable.ProductID, soaringPurchasable);
		}
		return true;
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x0003C4DC File Offset: 0x0003A6DC
	public void OpenTransaction(string productId)
	{
		this.txProductId = productId;
		RmtStore.IsPurchasing = true;
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x0003C4EC File Offset: 0x0003A6EC
	public void StartRmtPurchase(Session session)
	{
		TFUtils.DebugLog("Purchasing product: " + this.txProductId);
		if (session.TheGame.store.RmtReady)
		{
			RmtStore.IsPurchasing = true;
			TFBilling.StartRmtPurchase(this.txProductId);
		}
		else
		{
			TFUtils.DebugLog("Skipping premium purchase, since premium is not supported");
		}
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0003C544 File Offset: 0x0003A744
	public void RecordPurchaseCompleted(Dictionary<string, object> purchaseInfo, Session session)
	{
		try
		{
			string text = (string)purchaseInfo["productId"];
			string text2;
			if (purchaseInfo.ContainsKey("transactionId"))
			{
				text2 = (string)purchaseInfo["transactionId"];
			}
			else
			{
				text2 = text;
			}
			string value = (string)purchaseInfo["receipt"];
			string playerId = session.ThePlayer.playerId;
			if (!this.soaringProducts.ContainsKey(text))
			{
				TFUtils.ErrorLog("missing offer " + text);
			}
			else
			{
				SoaringPurchasable soaringPurchasable = this.soaringProducts[text];
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["transactionId"] = text2;
				dictionary["productId"] = text;
				dictionary["playerId"] = playerId;
				dictionary["receipt"] = value;
				dictionary["data"] = new Cost(new Dictionary<int, int>
				{
					{
						soaringPurchasable.ResourceType,
						soaringPurchasable.Amount
					}
				}).ToDict();
				dictionary["type"] = soaringPurchasable.ResourceType.ToString();
				if (purchaseInfo.ContainsKey("userId"))
				{
					dictionary["userId"] = purchaseInfo["userId"];
				}
				string text3 = Json.Serialize(dictionary);
				File.AppendAllText(RmtStore.TRANSACTION_LOG, text3);
				this.pendingTransactions[text2] = dictionary;
				TFUtils.DebugLog("json for purchase: " + text3);
				this.OnPurchaseResponseReceived(dictionary, null);
				this.OnPurchaseReceiptReceived(dictionary, null);
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message + "\n" + ex.StackTrace, LogType.Error);
		}
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x0003C71C File Offset: 0x0003A91C
	public Dictionary<string, Dictionary<string, object>> PendingTransactions()
	{
		return this.pendingTransactions;
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0003C724 File Offset: 0x0003A924
	public void GetPurchases(Session session)
	{
		Soaring.RequestPurchases(RmtStore.GetStoreName(), RmtStore.HandleProductsDelegate.CreateDelegate(session, "GetPurchases", null, null));
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0003C740 File Offset: 0x0003A940
	public void ApplyRmtPurchases(Session session, Cost data)
	{
		session.TheGame.resourceManager.SetPurchasedResources(data);
		Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
		{
			ResourceManager.ApplyPurchasesToGameState(data, gameState);
		};
		session.TheGame.LockedGameStateChange(writer);
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0003C78C File Offset: 0x0003A98C
	public void ApplyRmtPurchase(Session session, Cost data, string sale_tag, string transactionId)
	{
		if (!session.TheGame.store.RmtReady)
		{
			TFUtils.WarningLog("Failed to apply premium purchase, since premium is not ready");
			return;
		}
		session.TheGame.resourceManager.PurchaseResourcesWithHardCurrency(0, new Cost(), session.TheGame);
		session.TheGame.resourceManager.SetPurchasedResources(data);
		Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
		{
			ResourceManager.ApplyPurchasesToGameState(data, gameState);
			((Dictionary<string, object>)gameState["farm"])["last_action"] = sale_tag;
		};
		session.TheGame.LockedGameStateChange(writer);
		this.ClearTransaction(transactionId);
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x0003C820 File Offset: 0x0003AA20
	private void ClearTransaction(string transactionId)
	{
		if (this.pendingTransactions.ContainsKey(transactionId))
		{
			this.pendingTransactions.Remove(transactionId);
		}
		else
		{
			TFUtils.DebugLog("Did not find transaction for id: " + transactionId + " in in-memory log");
		}
		string[] array = File.ReadAllLines(RmtStore.TRANSACTION_LOG);
		List<string> list = new List<string>();
		bool flag = false;
		foreach (string text in array)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(text);
			if (dictionary["transactionId"].Equals(transactionId))
			{
				flag = true;
			}
			else
			{
				list.Add(text);
			}
		}
		File.WriteAllText(RmtStore.TRANSACTION_LOG, string.Join("\n", list.ToArray()));
		if (!flag)
		{
			TFUtils.DebugLog("Did not find transaction for id: " + transactionId + " in on-disk log");
		}
		TFBilling.CompleteRmtPurchase(transactionId);
		TFUtils.DebugLog("Closed transaction for " + transactionId);
		this.txProductId = null;
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0003C918 File Offset: 0x0003AB18
	private static void HandleProductInfo(Session session, RmtStore.StoreEventArgs args)
	{
		Dictionary<string, object> results = args.results;
		bool flag = session.TheGame.store.LoadRmtProductInfo(session.TheGame.catalog, results);
		TFUtils.DebugLog("Got premium product info:");
		if (!flag)
		{
			TFUtils.WarningLog("Ignoring invalid response for product info with no products");
		}
		session.TheGame.store.receivedProductInfo = flag;
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0003C974 File Offset: 0x0003AB74
	private static void HandlePurchaseInfo(Session session, RmtStore.StoreEventArgs args)
	{
		Dictionary<string, object> results = args.results;
		if (TFServer.IsNetworkError(results))
		{
			TFUtils.DebugLog("Failed to load purchases. Continuing.");
			session.TheGame.store.receivedPurchaseInfo = true;
			RmtStore.IsPurchasing = false;
		}
		else
		{
			Dictionary<string, object> dictionary = results;
			Cost data = RmtStore.CostFromCollection(session, (List<object>)dictionary["sales"], "data");
			session.TheGame.store.ApplyRmtPurchases(session, data);
			session.TheGame.store.receivedPurchaseInfo = true;
		}
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0003C9FC File Offset: 0x0003ABFC
	private static void HandlePurchaseUpdate(Session session, RmtStore.StoreEventArgs args)
	{
		Dictionary<string, object> results = args.results;
		TFUtils.DebugLog("Got purchase update: " + results);
		string text = (string)results["state"];
		string iapBundleName = TFUtils.TryLoadString(results, "productId");
		int num = 1;
		if (results.ContainsKey("data"))
		{
			Dictionary<int, int> dictionary = results["data"] as Dictionary<int, int>;
			if (dictionary != null && dictionary.ContainsKey(ResourceManager.HARD_CURRENCY))
			{
				num = dictionary[ResourceManager.HARD_CURRENCY];
			}
			TFUtils.DebugLog("Found purchase amount! " + num);
		}
		int amount = session.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount;
		string text2 = text;
		switch (text2)
		{
		case "started":
			return;
		case "completed":
			session.TheGame.store.RecordPurchaseCompleted(results, session);
			session.analytics.LogCompleteInAppPurchase(iapBundleName, amount);
			return;
		case "failed":
			session.TheGame.store.OnPurchaseError(results, null);
			session.analytics.LogFailInAppPurchase(iapBundleName, amount);
			RmtStore.IsPurchasing = false;
			return;
		case "defered":
			session.TheGame.store.OnPurchaseDefered(results, null);
			return;
		}
		session.TheGame.store.OnPurchaseError(results, null);
		RmtStore.IsPurchasing = false;
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x0003CBCC File Offset: 0x0003ADCC
	private static void HandlePurchaseResponse(Session session, RmtStore.StoreEventArgs args)
	{
		RmtStore.IsPurchasing = false;
		Dictionary<string, object> results = args.results;
		if (TFServer.IsNetworkError(results))
		{
			TFUtils.ErrorLog("Network access is required to complete RMT");
			session.TheGame.store.OnPurchaseError(results, null);
		}
		else
		{
			Dictionary<string, object> dictionary = results;
			Cost cost = Cost.FromDict((Dictionary<string, object>)dictionary["data"]);
			foreach (string text in dictionary.Keys)
			{
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"key:",
					text,
					" value:",
					dictionary[text]
				}));
			}
			string b = string.Empty;
			string b2 = string.Empty;
			if (dictionary.ContainsKey("sale_tag"))
			{
				b = TFUtils.LoadString(dictionary, "sale_tag");
			}
			if (dictionary.ContainsKey("transactionId"))
			{
				b2 = TFUtils.LoadString(dictionary, "transactionId");
			}
			string userID = null;
			if (dictionary.ContainsKey("userId"))
			{
				userID = TFUtils.LoadString(dictionary, "userId");
			}
			string text2 = TFUtils.LoadString(dictionary, "receipt").Replace("\r", string.Empty);
			text2 = text2.Replace("\n", string.Empty);
			string text3 = TFUtils.LoadString(dictionary, "productId");
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			SoaringDictionary soaringDictionary2 = new SoaringDictionary();
			soaringDictionary.addValue(b2, "transaction_id");
			soaringDictionary.addValue(b, "sale_tag");
			soaringDictionary.addValue(text3, "productId");
			soaringDictionary.addValue(text2, "receipt");
			Dictionary<int, int> resourceAmounts = cost.ResourceAmounts;
			foreach (KeyValuePair<int, int> keyValuePair in resourceAmounts)
			{
				soaringDictionary2.addValue(keyValuePair.Value + session.TheGame.resourceManager.Resources[keyValuePair.Key].AmountPurchased, keyValuePair.Key.ToString());
			}
			soaringDictionary.addValue(soaringDictionary2, "cost");
			Soaring.ValidatePurchasableReciept(text2, session.TheGame.store.soaringProducts[text3], RmtStore.GetStoreName(), SBSettings.UseProductionIAP, userID, RmtStore.HandleProductsDelegate.CreateDelegate(session, "ValidatePurchasableReciept", null, soaringDictionary));
		}
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x0003CE8C File Offset: 0x0003B08C
	private static void HandleGetProductInfoResponse(Session session, RmtStore.StoreEventArgs args)
	{
		Dictionary<string, object> results = args.results;
		TFUtils.DebugLog("purchaseResponse " + results);
		if (TFServer.IsNetworkError(results))
		{
			TFUtils.ErrorLog("Network access is required to complete RMT");
			session.TheGame.store.OnPurchaseError(results, null);
		}
		else
		{
			TFUtils.DebugLog("args " + args);
			Dictionary<string, object> dictionary = results;
			TFUtils.DebugLog("0000");
			foreach (string text in dictionary.Keys)
			{
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"------ key:",
					text,
					" value:",
					dictionary[text]
				}));
			}
			List<object> list = (List<object>)dictionary["products"];
			TFUtils.DebugLog("length " + list.Count);
			List<string> list2 = new List<string>();
			foreach (object obj in list)
			{
				TFUtils.DebugLog("------ " + obj);
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
				list2.Add(dictionary2["code"].ToString());
				foreach (string text2 in dictionary2.Keys)
				{
					TFUtils.DebugLog(string.Concat(new object[]
					{
						"------ key:",
						text2,
						" value:",
						dictionary2[text2]
					}));
				}
			}
			if (TFUtils.isAmazon())
			{
				SkusInput skusInput = new SkusInput();
				skusInput.Skus = list2;
				AmazonIapV2Impl.Instance.GetProductData(skusInput);
			}
			else
			{
				GoogleIAB.queryInventory(list2.ToArray());
			}
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0003D0E8 File Offset: 0x0003B2E8
	public bool RmtReady
	{
		get
		{
			return this.rmtEnabled && this.rmtProducts != null;
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0003D104 File Offset: 0x0003B304
	private static string GetStoreName()
	{
		return SBSettings.StoreName;
	}

	// Token: 0x040006AA RID: 1706
	public const float STORE_TIMEOUT = 15f;

	// Token: 0x040006AB RID: 1707
	private static string TRANSACTION_LOG = Application.persistentDataPath + Path.DirectorySeparatorChar + "txn.json";

	// Token: 0x040006AC RID: 1708
	public bool rmtEnabled;

	// Token: 0x040006AD RID: 1709
	public Dictionary<string, RmtProduct> rmtProducts;

	// Token: 0x040006AE RID: 1710
	public Dictionary<string, SoaringPurchasable> soaringProducts = new Dictionary<string, SoaringPurchasable>();

	// Token: 0x040006AF RID: 1711
	public bool receivedProductInfo;

	// Token: 0x040006B0 RID: 1712
	public bool receivedPurchaseInfo;

	// Token: 0x040006B1 RID: 1713
	private string txProductId;

	// Token: 0x040006B2 RID: 1714
	public static bool IsPurchasing = false;

	// Token: 0x040006B3 RID: 1715
	private Dictionary<string, Dictionary<string, object>> pendingTransactions;

	// Token: 0x0200010E RID: 270
	public class HandleProductsDelegate : SoaringDelegate
	{
		// Token: 0x060009C9 RID: 2505 RVA: 0x0003D114 File Offset: 0x0003B314
		public static SoaringContext CreateDelegate(Session session, string name = null, SoaringContextDelegate del = null, SoaringObjectBase passthrough = null)
		{
			SoaringContext soaringContext = new SoaringContext();
			soaringContext.Name = name;
			soaringContext.Responder = new RmtStore.HandleProductsDelegate
			{
				session = session
			};
			soaringContext.ContextResponder = del;
			if (passthrough != null)
			{
				soaringContext.addValue(passthrough, "passthrough");
			}
			return soaringContext;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0003D15C File Offset: 0x0003B35C
		public override void OnRetrieveProducts(bool success, SoaringError error, SoaringPurchasable[] purchasables, SoaringContext context)
		{
			this.session.TheGame.store.LoadRmtProductInfo(purchasables);
			List<string> list = new List<string>();
			int num = (purchasables != null) ? purchasables.Length : 0;
			for (int i = 0; i < num; i++)
			{
				list.Add(purchasables[i].ProductID);
			}
			TFBilling.FetchProductBillingInfo(this.session, list);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0003D1C4 File Offset: 0x0003B3C4
		public override void OnRetrievePurchases(bool success, SoaringError error, SoaringPurchase[] purchases, SoaringContext context)
		{
			if (success)
			{
				this.session.TheGame.store.receivedPurchaseInfo = true;
			}
			if (purchases == null)
			{
				return;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			int num = purchases.Length;
			for (int i = 0; i < num; i++)
			{
				SoaringPurchase soaringPurchase = purchases[i];
				if (dictionary.ContainsKey(soaringPurchase.ResourceType))
				{
					Dictionary<int, int> dictionary3;
					Dictionary<int, int> dictionary2 = dictionary3 = dictionary;
					int num2;
					int key = num2 = soaringPurchase.ResourceType;
					num2 = dictionary3[num2];
					dictionary2[key] = num2 + soaringPurchase.Amount;
				}
				else
				{
					dictionary.Add(soaringPurchase.ResourceType, soaringPurchase.Amount);
				}
			}
			Cost data = new Cost(dictionary);
			this.session.TheGame.store.ApplyRmtPurchases(this.session, data);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0003D288 File Offset: 0x0003B488
		public override void OnRecieptValidated(bool success, SoaringError error, SoaringContext context)
		{
			if (context != null)
			{
				SoaringDictionary soaringDictionary = (SoaringDictionary)context["passthrough"];
				SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary["cost"];
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				string[] array = soaringDictionary2.allKeys();
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					if (string.IsNullOrEmpty(array[i]))
					{
						SoaringDebug.Log("Invalid CostKey: " + i);
					}
					else
					{
						dictionary.Add(int.Parse(array[i]), (SoaringValue)soaringDictionary2[array[i]]);
					}
				}
				string text = (SoaringValue)soaringDictionary["transaction_id"];
				string text2 = string.Empty;
				if (soaringDictionary.containsKey("receipt"))
				{
					text2 = (SoaringValue)soaringDictionary["receipt"];
				}
				if ((error != null || !success) && Soaring.IsOnline)
				{
					if (error != null)
					{
						SoaringDebug.Log("OnRecieptValidated: " + error, LogType.Error);
					}
					else
					{
						SoaringDebug.Log("OnRecieptValidated: Unknown Error", LogType.Error);
					}
					this.session.TheGame.store.ClearTransaction(text);
				}
				else if (success)
				{
					this.session.TheGame.store.ApplyRmtPurchase(this.session, new Cost(dictionary), (SoaringValue)soaringDictionary["sale_tag"], text);
					SoaringPurchasable soaringPurchasable = this.session.TheGame.store.soaringProducts[(SoaringValue)soaringDictionary["productId"]];
					RmtProduct rmtProduct = null;
					if (!this.session.TheGame.store.rmtProducts.TryGetValue(soaringPurchasable.ProductID, out rmtProduct))
					{
						SoaringDebug.Log("No Remote Product Found For Analytics: " + soaringPurchasable.ProductID, LogType.Error);
					}
					if (TFUtils.isAmazon())
					{
						NotifyFulfillmentInput notifyFulfillmentInput = new NotifyFulfillmentInput();
						notifyFulfillmentInput.FulfillmentResult = "FULFILLED";
						notifyFulfillmentInput.ReceiptId = text2;
						AmazonIapV2Impl.Instance.NotifyFulfillment(notifyFulfillmentInput);
					}
					this.session.TheGame.analytics.LogSoaringIAPPurchaseComplete(soaringPurchasable.ProductID);
					AnalyticsWrapper.LogPurchaseComplete(this.session.TheGame, soaringPurchasable, text2, text, null);
				}
				else
				{
					SoaringDebug.Log("OnRecieptValidated: Critical Error: Reciept Valied To Validated, Offline Mode", LogType.Error);
				}
			}
		}

		// Token: 0x040006BD RID: 1725
		public Session session;
	}

	// Token: 0x0200010F RID: 271
	public class StoreEventArgs : EventArgs
	{
		// Token: 0x060009CD RID: 2509 RVA: 0x0003D4FC File Offset: 0x0003B6FC
		public StoreEventArgs(Dictionary<string, object> res)
		{
			this.results = res;
		}

		// Token: 0x040006BE RID: 1726
		public Dictionary<string, object> results;
	}

	// Token: 0x020004A0 RID: 1184
	// (Invoke) Token: 0x060024E3 RID: 9443
	public delegate void StoreEventHandler(object sender, RmtStore.StoreEventArgs args);
}
