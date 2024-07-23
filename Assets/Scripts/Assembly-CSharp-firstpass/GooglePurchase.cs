using System;
using System.Collections.Generic;

// Token: 0x020000A3 RID: 163
public class GooglePurchase
{
	// Token: 0x06000680 RID: 1664 RVA: 0x000176C4 File Offset: 0x000158C4
	public GooglePurchase(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("packageName"))
		{
			this.packageName = dict["packageName"].ToString();
		}
		if (dict.ContainsKey("orderId"))
		{
			this.orderId = dict["orderId"].ToString();
		}
		if (dict.ContainsKey("productId"))
		{
			this.productId = dict["productId"].ToString();
		}
		if (dict.ContainsKey("developerPayload"))
		{
			this.developerPayload = dict["developerPayload"].ToString();
		}
		if (dict.ContainsKey("type"))
		{
			this.type = (dict["type"] as string);
		}
		if (dict.ContainsKey("purchaseTime"))
		{
			this.purchaseTime = long.Parse(dict["purchaseTime"].ToString());
		}
		if (dict.ContainsKey("purchaseState"))
		{
			this.purchaseState = (GooglePurchase.GooglePurchaseState)int.Parse(dict["purchaseState"].ToString());
		}
		if (dict.ContainsKey("purchaseToken"))
		{
			this.purchaseToken = dict["purchaseToken"].ToString();
		}
		if (dict.ContainsKey("signature"))
		{
			this.signature = dict["signature"].ToString();
		}
		if (dict.ContainsKey("originalJson"))
		{
			this.originalJson = dict["originalJson"].ToString();
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000681 RID: 1665 RVA: 0x00017860 File Offset: 0x00015A60
	// (set) Token: 0x06000682 RID: 1666 RVA: 0x00017868 File Offset: 0x00015A68
	public string packageName { get; private set; }

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000683 RID: 1667 RVA: 0x00017874 File Offset: 0x00015A74
	// (set) Token: 0x06000684 RID: 1668 RVA: 0x0001787C File Offset: 0x00015A7C
	public string orderId { get; private set; }

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000685 RID: 1669 RVA: 0x00017888 File Offset: 0x00015A88
	// (set) Token: 0x06000686 RID: 1670 RVA: 0x00017890 File Offset: 0x00015A90
	public string productId { get; private set; }

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000687 RID: 1671 RVA: 0x0001789C File Offset: 0x00015A9C
	// (set) Token: 0x06000688 RID: 1672 RVA: 0x000178A4 File Offset: 0x00015AA4
	public string developerPayload { get; private set; }

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000689 RID: 1673 RVA: 0x000178B0 File Offset: 0x00015AB0
	// (set) Token: 0x0600068A RID: 1674 RVA: 0x000178B8 File Offset: 0x00015AB8
	public string type { get; private set; }

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600068B RID: 1675 RVA: 0x000178C4 File Offset: 0x00015AC4
	// (set) Token: 0x0600068C RID: 1676 RVA: 0x000178CC File Offset: 0x00015ACC
	public long purchaseTime { get; private set; }

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x0600068D RID: 1677 RVA: 0x000178D8 File Offset: 0x00015AD8
	// (set) Token: 0x0600068E RID: 1678 RVA: 0x000178E0 File Offset: 0x00015AE0
	public GooglePurchase.GooglePurchaseState purchaseState { get; private set; }

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x0600068F RID: 1679 RVA: 0x000178EC File Offset: 0x00015AEC
	// (set) Token: 0x06000690 RID: 1680 RVA: 0x000178F4 File Offset: 0x00015AF4
	public string purchaseToken { get; private set; }

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000691 RID: 1681 RVA: 0x00017900 File Offset: 0x00015B00
	// (set) Token: 0x06000692 RID: 1682 RVA: 0x00017908 File Offset: 0x00015B08
	public string signature { get; private set; }

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000693 RID: 1683 RVA: 0x00017914 File Offset: 0x00015B14
	// (set) Token: 0x06000694 RID: 1684 RVA: 0x0001791C File Offset: 0x00015B1C
	public string originalJson { get; private set; }

	// Token: 0x06000695 RID: 1685 RVA: 0x00017928 File Offset: 0x00015B28
	public static List<GooglePurchase> fromList(List<object> items)
	{
		List<GooglePurchase> list = new List<GooglePurchase>();
		foreach (object obj in items)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)obj;
			list.Add(new GooglePurchase(dict));
		}
		return list;
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0001799C File Offset: 0x00015B9C
	public override string ToString()
	{
		return string.Format("<GooglePurchase> packageName: {0}, orderId: {1}, productId: {2}, developerPayload: {3}, purchaseToken: {4}, purchaseState: {5}, signature: {6}, type: {7}, json: {8}", new object[]
		{
			this.packageName,
			this.orderId,
			this.productId,
			this.developerPayload,
			this.purchaseToken,
			this.purchaseState,
			this.signature,
			this.type,
			this.originalJson
		});
	}

	// Token: 0x020000A4 RID: 164
	public enum GooglePurchaseState
	{
		// Token: 0x04000399 RID: 921
		Purchased,
		// Token: 0x0400039A RID: 922
		Canceled,
		// Token: 0x0400039B RID: 923
		Refunded
	}
}
