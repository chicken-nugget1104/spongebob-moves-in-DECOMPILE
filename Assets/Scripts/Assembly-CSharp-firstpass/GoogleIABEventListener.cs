using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public class GoogleIABEventListener : MonoBehaviour
{
	// Token: 0x060006A5 RID: 1701 RVA: 0x00017C10 File Offset: 0x00015E10
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

	// Token: 0x060006A6 RID: 1702 RVA: 0x00017CB8 File Offset: 0x00015EB8
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

	// Token: 0x060006A7 RID: 1703 RVA: 0x00017D60 File Offset: 0x00015F60
	private void billingSupportedEvent()
	{
		Debug.Log("billingSupportedEvent");
		if (GoogleIAB.areSubscriptionsSupported())
		{
			GoogleIAB.queryInventory(this.productIds);
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00017D84 File Offset: 0x00015F84
	private void billingNotSupportedEvent(string error)
	{
		Debug.Log("billingNotSupportedEvent: " + error);
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00017D98 File Offset: 0x00015F98
	private void queryInventorySucceededEvent(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus)
	{
		Debug.Log(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
		Utils.logObject(purchases);
		Utils.logObject(skus);
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00017DD8 File Offset: 0x00015FD8
	private void listToDictionary(List<GoogleSkuInfo> skus)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (GoogleSkuInfo googleSkuInfo in skus)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			int num = googleSkuInfo.title.IndexOf(" (");
		}
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00017E50 File Offset: 0x00016050
	private void queryInventoryFailedEvent(string error)
	{
		Debug.Log("queryInventoryFailedEvent: " + error);
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00017E64 File Offset: 0x00016064
	private void purchaseCompleteAwaitingVerificationEvent(string purchaseData, string signature)
	{
		Debug.Log("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature);
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x00017E7C File Offset: 0x0001607C
	private void purchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("purchaseSucceededEvent: " + purchase);
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00017E90 File Offset: 0x00016090
	private void purchaseFailedEvent(string error)
	{
		Debug.Log("purchaseFailedEvent: " + error);
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x00017EA4 File Offset: 0x000160A4
	private void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		Debug.Log("consumePurchaseSucceededEvent: " + purchase);
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x00017EB8 File Offset: 0x000160B8
	private void consumePurchaseFailedEvent(string error)
	{
		Debug.Log("consumePurchaseFailedEvent: " + error);
	}

	// Token: 0x040003A1 RID: 929
	public string[] productIds;
}
