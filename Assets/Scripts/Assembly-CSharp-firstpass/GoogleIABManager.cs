using System;
using System.Collections.Generic;
using Prime31;

// Token: 0x020000A2 RID: 162
public class GoogleIABManager : AbstractManager
{
	// Token: 0x06000664 RID: 1636 RVA: 0x000173D8 File Offset: 0x000155D8
	static GoogleIABManager()
	{
		AbstractManager.initialize(typeof(GoogleIABManager));
	}

	// Token: 0x140000A0 RID: 160
	// (add) Token: 0x06000665 RID: 1637 RVA: 0x000173EC File Offset: 0x000155EC
	// (remove) Token: 0x06000666 RID: 1638 RVA: 0x00017404 File Offset: 0x00015604
	public static event Action billingSupportedEvent;

	// Token: 0x140000A1 RID: 161
	// (add) Token: 0x06000667 RID: 1639 RVA: 0x0001741C File Offset: 0x0001561C
	// (remove) Token: 0x06000668 RID: 1640 RVA: 0x00017434 File Offset: 0x00015634
	public static event Action<string> billingNotSupportedEvent;

	// Token: 0x140000A2 RID: 162
	// (add) Token: 0x06000669 RID: 1641 RVA: 0x0001744C File Offset: 0x0001564C
	// (remove) Token: 0x0600066A RID: 1642 RVA: 0x00017464 File Offset: 0x00015664
	public static event Action<List<GooglePurchase>, List<GoogleSkuInfo>> queryInventorySucceededEvent;

	// Token: 0x140000A3 RID: 163
	// (add) Token: 0x0600066B RID: 1643 RVA: 0x0001747C File Offset: 0x0001567C
	// (remove) Token: 0x0600066C RID: 1644 RVA: 0x00017494 File Offset: 0x00015694
	public static event Action<string> queryInventoryFailedEvent;

	// Token: 0x140000A4 RID: 164
	// (add) Token: 0x0600066D RID: 1645 RVA: 0x000174AC File Offset: 0x000156AC
	// (remove) Token: 0x0600066E RID: 1646 RVA: 0x000174C4 File Offset: 0x000156C4
	public static event Action<string, string> purchaseCompleteAwaitingVerificationEvent;

	// Token: 0x140000A5 RID: 165
	// (add) Token: 0x0600066F RID: 1647 RVA: 0x000174DC File Offset: 0x000156DC
	// (remove) Token: 0x06000670 RID: 1648 RVA: 0x000174F4 File Offset: 0x000156F4
	public static event Action<GooglePurchase> purchaseSucceededEvent;

	// Token: 0x140000A6 RID: 166
	// (add) Token: 0x06000671 RID: 1649 RVA: 0x0001750C File Offset: 0x0001570C
	// (remove) Token: 0x06000672 RID: 1650 RVA: 0x00017524 File Offset: 0x00015724
	public static event Action<string> purchaseFailedEvent;

	// Token: 0x140000A7 RID: 167
	// (add) Token: 0x06000673 RID: 1651 RVA: 0x0001753C File Offset: 0x0001573C
	// (remove) Token: 0x06000674 RID: 1652 RVA: 0x00017554 File Offset: 0x00015754
	public static event Action<GooglePurchase> consumePurchaseSucceededEvent;

	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x06000675 RID: 1653 RVA: 0x0001756C File Offset: 0x0001576C
	// (remove) Token: 0x06000676 RID: 1654 RVA: 0x00017584 File Offset: 0x00015784
	public static event Action<string> consumePurchaseFailedEvent;

	// Token: 0x06000677 RID: 1655 RVA: 0x0001759C File Offset: 0x0001579C
	public void billingSupported(string empty)
	{
		GoogleIABManager.billingSupportedEvent.fire();
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x000175A8 File Offset: 0x000157A8
	public void billingNotSupported(string error)
	{
		GoogleIABManager.billingNotSupportedEvent.fire(error);
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x000175B8 File Offset: 0x000157B8
	public void queryInventorySucceeded(string json)
	{
		if (GoogleIABManager.queryInventorySucceededEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			GoogleIABManager.queryInventorySucceededEvent(GooglePurchase.fromList(dictionary["purchases"] as List<object>), GoogleSkuInfo.fromList(dictionary["skus"] as List<object>));
		}
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0001760C File Offset: 0x0001580C
	public void queryInventoryFailed(string error)
	{
		GoogleIABManager.queryInventoryFailedEvent.fire(error);
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0001761C File Offset: 0x0001581C
	public void purchaseCompleteAwaitingVerification(string json)
	{
		if (GoogleIABManager.purchaseCompleteAwaitingVerificationEvent != null)
		{
			Dictionary<string, object> dictionary = json.dictionaryFromJson();
			string arg = dictionary["purchaseData"].ToString();
			string arg2 = dictionary["signature"].ToString();
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent(arg, arg2);
		}
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00017668 File Offset: 0x00015868
	public void purchaseSucceeded(string json)
	{
		GoogleIABManager.purchaseSucceededEvent.fire(new GooglePurchase(json.dictionaryFromJson()));
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x00017680 File Offset: 0x00015880
	public void purchaseFailed(string error)
	{
		GoogleIABManager.purchaseFailedEvent.fire(error);
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00017690 File Offset: 0x00015890
	public void consumePurchaseSucceeded(string json)
	{
		if (GoogleIABManager.consumePurchaseSucceededEvent != null)
		{
			GoogleIABManager.consumePurchaseSucceededEvent.fire(new GooglePurchase(json.dictionaryFromJson()));
		}
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x000176B4 File Offset: 0x000158B4
	public void consumePurchaseFailed(string error)
	{
		GoogleIABManager.consumePurchaseFailedEvent.fire(error);
	}
}
