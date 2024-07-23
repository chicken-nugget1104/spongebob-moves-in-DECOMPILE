using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class GoogleIapListener : GoogleIABEventListener
{
	// Token: 0x0600025E RID: 606 RVA: 0x0000BC88 File Offset: 0x00009E88
	public static GoogleIapListener getInstance()
	{
		if (null == GoogleIapListener.googleIapListener)
		{
			GameObject gameObject = new GameObject();
			GoogleIapListener.googleIapListener = gameObject.AddComponent<GoogleIapListener>();
		}
		return GoogleIapListener.googleIapListener;
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000BCBC File Offset: 0x00009EBC
	private void OnEnable()
	{
		GoogleIABManager.billingSupportedEvent += this.billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += this.billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += this.queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += this.queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += this.purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += this.purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += this.purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += this.consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += this.consumePurchaseFailedEvent;
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0000BD64 File Offset: 0x00009F64
	private void OnDisable()
	{
		GoogleIABManager.billingSupportedEvent -= this.billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= this.billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= this.queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= this.queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += this.purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= this.purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= this.purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= this.consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= this.consumePurchaseFailedEvent;
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000BE0C File Offset: 0x0000A00C
	private void billingSupportedEvent()
	{
		Debug.Log("billingSupportedEvent");
		if (GoogleIAB.areSubscriptionsSupported())
		{
			GoogleIAB.queryInventory(this._productIds);
		}
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000BE30 File Offset: 0x0000A030
	private void billingNotSupportedEvent(string error)
	{
		Debug.Log("billingNotSupportedEvent: " + error);
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000BE44 File Offset: 0x0000A044
	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		Debug.Log(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
		if (purchases.Count > 0)
		{
			string[] array = new string[purchases.Count];
			int num = 0;
			foreach (GooglePurchase googlePurchase in purchases)
			{
				array[num++] = googlePurchase.productId;
				TFUtils.DebugLog("--------consume---------------" + googlePurchase.productId);
			}
			GoogleIAB.consumeProducts(array);
		}
		Utils.logObject(purchases);
		Utils.logObject(skus);
		if (!this.session.TheGame.store.receivedProductInfo)
		{
			this.listToDictionary(skus);
		}
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0000BF34 File Offset: 0x0000A134
	private void listToDictionary(List<GoogleSkuInfo> skus)
	{
		this.session.TheGame.store.rmtProducts = new Dictionary<string, RmtProduct>();
		foreach (GoogleSkuInfo googleSkuInfo in skus)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string productId = googleSkuInfo.productId;
			TFUtils.DebugLog("sku.productId " + googleSkuInfo.productId);
			dictionary.Add("title", productId);
			dictionary.Add("price", googleSkuInfo.price);
			dictionary.Add("localizedprice", googleSkuInfo.price);
			dictionary.Add("description", googleSkuInfo.description);
			dictionary.Add("productId", productId);
			RmtProduct rmtProduct = new RmtProduct(dictionary);
			this.session.TheGame.store.rmtProducts[rmtProduct.productId] = rmtProduct;
			TFUtils.DebugLog("--------" + rmtProduct.productId);
			TFUtils.DebugLog("--------" + rmtProduct.localizedprice);
		}
		this.session.TheGame.store.receivedProductInfo = true;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000C084 File Offset: 0x0000A284
	private void queryInventoryFailedEvent(string error)
	{
		Debug.Log("queryInventoryFailedEvent: " + error);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0000C098 File Offset: 0x0000A298
	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("purchaseSucceededEvent: " + purchase);
		GoogleIAB.consumeProduct(purchase.productId);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("transactionId", purchase.orderId);
		dictionary.Add("productId", purchase.productId);
		dictionary.Add("receipt", purchase.purchaseToken);
		this.session.TheGame.store.RecordPurchaseCompleted(dictionary, this.session);
		TFUtils.DebugLog("---------purchaseSucceededEvent-----1-------");
		string iapBundleName = TFUtils.TryLoadString(dictionary, "productId");
		int amount = this.session.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount;
		this.session.analytics.LogCompleteInAppPurchase(iapBundleName, amount);
		this.session.ChangeState("Playing", true);
		TFUtils.DebugLog("---------purchaseSucceededEvent-----2-------");
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0000C198 File Offset: 0x0000A398
	private void purchaseFailedEvent(string error)
	{
		Debug.Log("purchaseFailedEvent: " + error);
		RmtStore.IsPurchasing = false;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000C1B0 File Offset: 0x0000A3B0
	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
	}

	// Token: 0x0600026A RID: 618 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
	private void consumePurchaseFailedEvent(string error)
	{
		Debug.Log("consumePurchaseFailedEvent: " + error);
		RmtStore.IsPurchasing = false;
	}

	// Token: 0x04000139 RID: 313
	public static GoogleIapListener googleIapListener;

	// Token: 0x0400013A RID: 314
	public string[] _productIds;

	// Token: 0x0400013B RID: 315
	public Session session;

	// Token: 0x0400013C RID: 316
	public string _productId;
}
