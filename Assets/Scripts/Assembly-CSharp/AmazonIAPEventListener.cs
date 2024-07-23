using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class AmazonIAPEventListener : MonoBehaviour
{
	// Token: 0x06000207 RID: 519 RVA: 0x00009CEC File Offset: 0x00007EEC
	public static AmazonIAPEventListener getInstance()
	{
		if (null == AmazonIAPEventListener.amazonIapListener)
		{
			AmazonIAPEventListener.amazonIapListener = new GameObject
			{
				name = "AmazonIAPEventListener"
			}.AddComponent<AmazonIAPEventListener>();
		}
		return AmazonIAPEventListener.amazonIapListener;
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00009D2C File Offset: 0x00007F2C
	private void OnEnable()
	{
		SoaringDebug.Log("AmazonIAPListener: OnEnable");
		IAmazonIapV2 instance = AmazonIapV2Impl.Instance;
		instance.AddGetUserDataResponseListener(new GetUserDataResponseDelegate(this.onGetUserDataResponse));
		instance.AddPurchaseResponseListener(new PurchaseResponseDelegate(this.onPurchaseResponse));
		instance.AddGetPurchaseUpdatesResponseListener(new GetPurchaseUpdatesResponseDelegate(this.onPurchaseUpdateResponse));
		instance.AddGetProductDataResponseListener(new GetProductDataResponseDelegate(this.onProductDataResponse));
		instance.GetUserData();
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00009D98 File Offset: 0x00007F98
	private void OnDisable()
	{
		SoaringDebug.Log("AmazonIAPListener: OnDisable");
		IAmazonIapV2 instance = AmazonIapV2Impl.Instance;
		instance.RemoveGetUserDataResponseListener(new GetUserDataResponseDelegate(this.onGetUserDataResponse));
		instance.RemovePurchaseResponseListener(new PurchaseResponseDelegate(this.onPurchaseResponse));
		instance.RemoveGetPurchaseUpdatesResponseListener(new GetPurchaseUpdatesResponseDelegate(this.onPurchaseUpdateResponse));
		instance.RemoveGetProductDataResponseListener(new GetProductDataResponseDelegate(this.onProductDataResponse));
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00009E00 File Offset: 0x00008000
	private void onSdkAvailableEvent(bool isTestMode)
	{
		Debug.Log("onSdkAvailableEvent. isTestMode: " + isTestMode);
		this.isAvailable = isTestMode;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x00009E20 File Offset: 0x00008020
	private void onGetUserDataResponse(GetUserDataResponse args)
	{
		string requestId = args.RequestId;
		string marketplace = args.AmazonUserData.Marketplace;
		string status = args.Status;
		this.userId = args.AmazonUserData.UserId;
		SoaringDebug.Log(string.Concat(new string[]
		{
			"onGetUserDataResponse: ",
			this.userId,
			" : ",
			requestId,
			" : ",
			marketplace,
			" : ",
			status
		}));
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00009EA0 File Offset: 0x000080A0
	private void onPurchaseResponse(PurchaseResponse args)
	{
		string status = args.Status;
		Debug.Log("onPurchaseResponse: " + status + " : " + this.userId);
		if (status == AmazonIAPEventListener.kSuccessKey)
		{
			this.onPurchaseSuccessfulEventv2(args.PurchaseReceipt);
		}
		else
		{
			this.onPurchaseFailedEventv2(status);
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00009EF8 File Offset: 0x000080F8
	private void onPurchaseSuccessfulEventv2(PurchaseReceipt receipt)
	{
		if (this.session == null)
		{
			Debug.LogError("AmazonIapEventListener: Invalid Session");
			return;
		}
		try
		{
			Debug.Log("onPurchaseSuccessfulEventv2: " + receipt);
			TFUtils.DebugLog("userId:" + this.userId);
			if (this.userId == null)
			{
				this.userId = "DefaultTestUser";
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("userId", this.userId);
			dictionary.Add("productId", receipt.Sku);
			dictionary.Add("receipt", receipt.ReceiptId);
			this.session.TheGame.store.RecordPurchaseCompleted(dictionary, this.session);
			TFUtils.DebugLog("---------purchaseSucceededEvent-----1-------");
			string iapBundleName = TFUtils.TryLoadString(dictionary, "productId");
			int amount = this.session.TheGame.resourceManager.Resources[ResourceManager.LEVEL].Amount;
			this.session.analytics.LogCompleteInAppPurchase(iapBundleName, amount);
			this.session.ChangeState("Playing", true);
			TFUtils.DebugLog("---------purchaseSucceededEvent-----2-------");
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("AmazonIAPListener: " + ex.Message, LogType.Error);
		}
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000A050 File Offset: 0x00008250
	private void onPurchaseFailedEventv2(string reason)
	{
		Debug.Log("onPurchaseFailedEventv2: " + reason);
		RmtStore.IsPurchasing = false;
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000A068 File Offset: 0x00008268
	private void onPurchaseUpdateResponse(GetPurchaseUpdatesResponse args)
	{
		if (args.Status == AmazonIAPEventListener.kSuccessKey)
		{
			this.onPurchaseUpdatesRequestSuccessfulEventV2(args.Receipts);
		}
		else
		{
			Debug.Log("onPurchaseUpdateResponse Failed");
			RmtStore.IsPurchasing = false;
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000A0AC File Offset: 0x000082AC
	private void onPurchaseUpdatesRequestSuccessfulEventV2(List<PurchaseReceipt> receipts)
	{
		try
		{
			Debug.Log("onPurchaseUpdateResponse Success. revoked skus: " + receipts.Count);
			foreach (PurchaseReceipt message in receipts)
			{
				Debug.Log(message);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("AmazonIapEventListener: " + ex.Message);
		}
		if (this.session == null)
		{
			Debug.LogError("AmazonIapEventListener: Invalid Session");
			return;
		}
		this.session.TheGame.store.receivedProductInfo = true;
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000A18C File Offset: 0x0000838C
	private void onProductDataResponse(GetProductDataResponse args)
	{
		string status = args.Status;
		if (status == AmazonIAPEventListener.kSuccessKey)
		{
			Debug.Log("onProductDataResponse: success");
			string requestId = args.RequestId;
			Dictionary<string, ProductData> productDataMap = args.ProductDataMap;
			if (productDataMap.Count > 0)
			{
				this.listToDictionary_v2(productDataMap);
			}
		}
		else
		{
			Debug.Log("onProductDataResponse: failed");
			RmtStore.IsPurchasing = false;
		}
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000A1F0 File Offset: 0x000083F0
	private void listToDictionary_v2(Dictionary<string, ProductData> availableItems)
	{
		if (this.session == null)
		{
			SoaringDebug.Log("AmazonIAPEventListener: Invalid Session", LogType.Error);
			return;
		}
		this.session.TheGame.store.rmtProducts = new Dictionary<string, RmtProduct>();
		foreach (string key in availableItems.Keys)
		{
			ProductData productData = availableItems[key];
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string sku = productData.Sku;
			TFUtils.DebugLog("sku.title " + productData.Title);
			dictionary.Add("title", sku);
			dictionary.Add("price", productData.Price);
			dictionary.Add("localizedprice", productData.Price);
			dictionary.Add("description", productData.Description);
			dictionary.Add("productId", sku);
			TFUtils.DebugLog("productId " + sku);
			RmtProduct rmtProduct = new RmtProduct(dictionary);
			this.session.TheGame.store.rmtProducts[rmtProduct.productId] = rmtProduct;
			TFUtils.DebugLog("--------" + rmtProduct.productId);
			TFUtils.DebugLog("--------" + rmtProduct.localizedprice);
		}
		this.session.TheGame.store.receivedProductInfo = true;
	}

	// Token: 0x04000111 RID: 273
	public Session session;

	// Token: 0x04000112 RID: 274
	public bool isAvailable;

	// Token: 0x04000113 RID: 275
	private string userId;

	// Token: 0x04000114 RID: 276
	public static AmazonIAPEventListener amazonIapListener;

	// Token: 0x04000115 RID: 277
	public static string kSuccessKey = "SUCCESSFUL";

	// Token: 0x04000116 RID: 278
	public static string kNotSupportedKey = "NOT_SUCCESSFUL";

	// Token: 0x04000117 RID: 279
	public static string kFailedKey = "FAILED";
}
