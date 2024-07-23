using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class TFBilling
{
	// Token: 0x06001E6C RID: 7788 RVA: 0x000BBC5C File Offset: 0x000B9E5C
	public static bool BillingIsAvailable()
	{
		return TFBilling.InternalBillingIsAvailable();
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x000BBC64 File Offset: 0x000B9E64
	public static void InitializeStore()
	{
		TFBilling.InternalInitializeStore();
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x000BBC6C File Offset: 0x000B9E6C
	public static void ResetStore()
	{
		TFBilling.InternalResetStore();
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x000BBC74 File Offset: 0x000B9E74
	public static void FetchProductBillingInfo(Session session, List<string> productIds)
	{
		TFBilling.InternalFetchBillingInfo(session, productIds);
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x000BBC80 File Offset: 0x000B9E80
	public static void StartRmtPurchase(string productId)
	{
		TFBilling.InternalStartRmtPurchase(productId);
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x000BBC88 File Offset: 0x000B9E88
	public static void CompleteRmtPurchase(string transactionId)
	{
		TFBilling.InternalCompleteRmtPurchase(transactionId);
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x000BBC90 File Offset: 0x000B9E90
	private static bool InternalBillingIsAvailable()
	{
		if (TFUtils.isAmazon())
		{
			return AmazonIAPEventListener.getInstance().isAvailable;
		}
		return GoogleIAB.areSubscriptionsSupported();
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x000BBCAC File Offset: 0x000B9EAC
	private static void InternalInitializeStore()
	{
		if (TFUtils.isAmazon())
		{
			AmazonIAPEventListener.getInstance();
			IAmazonIapV2 instance = AmazonIapV2Impl.Instance;
		}
		else
		{
			GoogleIAB.init(SBSettings.BillingKey);
		}
	}

	// Token: 0x06001E74 RID: 7796 RVA: 0x000BBCE0 File Offset: 0x000B9EE0
	private static void InternalResetStore()
	{
	}

	// Token: 0x06001E75 RID: 7797 RVA: 0x000BBCE4 File Offset: 0x000B9EE4
	private static void InternalFetchBillingInfo(Session session, List<string> productIds)
	{
		string[] array = productIds.ToArray();
		if (TFUtils.isAmazon())
		{
			try
			{
				AmazonIAPEventListener.getInstance().session = session;
				for (int i = 0; i < array.Length; i++)
				{
					TFUtils.DebugLog("fetched: " + array[i]);
				}
				SkusInput skusInput = new SkusInput();
				skusInput.Skus = productIds;
				AmazonIapV2Impl.Instance.GetProductData(skusInput);
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message + " : " + ex.StackTrace);
				session.TheGame.store.receivedProductInfo = false;
				RmtStore.IsPurchasing = false;
			}
		}
		else
		{
			for (int j = 0; j < array.Length; j++)
			{
				TFUtils.DebugLog("fetched: " + array[j]);
			}
			GoogleIapListener.getInstance()._productIds = array;
			GoogleIapListener.getInstance().session = session;
			TFUtils.DebugLog(GoogleIapListener.getInstance()._productIds.Length);
			if (TFBilling.InternalBillingIsAvailable())
			{
				TFUtils.DebugLog("InternalBillingIsAvailable: " + TFBilling.InternalBillingIsAvailable());
				GoogleIAB.queryInventory(array);
			}
		}
	}

	// Token: 0x06001E76 RID: 7798 RVA: 0x000BBE2C File Offset: 0x000BA02C
	private static void InternalStartRmtPurchase(string productId)
	{
		if (TFUtils.isAmazon())
		{
			SkuInput skuInput = new SkuInput();
			skuInput.Sku = productId;
			AmazonIapV2Impl.Instance.Purchase(skuInput);
		}
		else
		{
			GoogleIAB.purchaseProduct(productId);
		}
	}

	// Token: 0x06001E77 RID: 7799 RVA: 0x000BBE68 File Offset: 0x000BA068
	private static void InternalCompleteRmtPurchase(string transactionId)
	{
		Debug.Log("InternalCompleteRmtPurchase----------------------");
	}

	// Token: 0x040012CE RID: 4814
	public const string PRODUCT_INFO_REQUEST = "productInfo";

	// Token: 0x040012CF RID: 4815
	public const string PURCHASE_UPDATE = "purchaseUpdate";

	// Token: 0x040012D0 RID: 4816
	public const string PURCHASE_COMPLETED = "completed";

	// Token: 0x040012D1 RID: 4817
	public const string PURCHASE_FAILED = "failed";

	// Token: 0x040012D2 RID: 4818
	public const string PURCHASE_STARTED = "started";

	// Token: 0x040012D3 RID: 4819
	public const string PURCHASE_DEFERED = "defered";

	// Token: 0x040012D4 RID: 4820
	public const string TECHNICAL_FAILURE = "technicalFailure";

	// Token: 0x040012D5 RID: 4821
	public const string USER_CANCEL = "userCancelled";

	// Token: 0x040012D6 RID: 4822
	public const string STATE = "state";

	// Token: 0x040012D7 RID: 4823
	public const string REASON = "reason";

	// Token: 0x040012D8 RID: 4824
	public const string DESCRIPTION = "description";

	// Token: 0x040012D9 RID: 4825
	public const string PRODUCT_ID = "productId";

	// Token: 0x040012DA RID: 4826
	public const string TOKEN = "token";

	// Token: 0x040012DB RID: 4827
	public const string ORDER_ID = "orderId";

	// Token: 0x040012DC RID: 4828
	public const string USER_ID = "userId";

	// Token: 0x040012DD RID: 4829
	public const string TRANSACTION_ID = "transactionId";

	// Token: 0x040012DE RID: 4830
	public const string RECEIPT = "receipt";

	// Token: 0x040012DF RID: 4831
	public const string PRODUCTS = "products";

	// Token: 0x040012E0 RID: 4832
	public const string INVALID_PRODUCTS = "invalidProductIdentifiers";

	// Token: 0x040012E1 RID: 4833
	public const string LOCALIZED_PRICE = "localizedprice";

	// Token: 0x040012E2 RID: 4834
	public const string CURRENCY_CODE = "currencyCode";

	// Token: 0x040012E3 RID: 4835
	public const string PRICE = "price";

	// Token: 0x040012E4 RID: 4836
	public const string TITLE = "title";
}
