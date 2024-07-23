using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x020000D2 RID: 210
public class UpsightPurchase
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600083C RID: 2108 RVA: 0x0001EFE4 File Offset: 0x0001D1E4
	// (set) Token: 0x0600083D RID: 2109 RVA: 0x0001EFEC File Offset: 0x0001D1EC
	public string placement { get; private set; }

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x0600083E RID: 2110 RVA: 0x0001EFF8 File Offset: 0x0001D1F8
	// (set) Token: 0x0600083F RID: 2111 RVA: 0x0001F000 File Offset: 0x0001D200
	public int quantity { get; private set; }

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000840 RID: 2112 RVA: 0x0001F00C File Offset: 0x0001D20C
	// (set) Token: 0x06000841 RID: 2113 RVA: 0x0001F014 File Offset: 0x0001D214
	public string productIdentifier { get; private set; }

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000842 RID: 2114 RVA: 0x0001F020 File Offset: 0x0001D220
	// (set) Token: 0x06000843 RID: 2115 RVA: 0x0001F028 File Offset: 0x0001D228
	public string store { get; private set; }

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000844 RID: 2116 RVA: 0x0001F034 File Offset: 0x0001D234
	// (set) Token: 0x06000845 RID: 2117 RVA: 0x0001F03C File Offset: 0x0001D23C
	public string receipt { get; private set; }

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000846 RID: 2118 RVA: 0x0001F048 File Offset: 0x0001D248
	// (set) Token: 0x06000847 RID: 2119 RVA: 0x0001F050 File Offset: 0x0001D250
	public string title { get; private set; }

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000848 RID: 2120 RVA: 0x0001F05C File Offset: 0x0001D25C
	// (set) Token: 0x06000849 RID: 2121 RVA: 0x0001F064 File Offset: 0x0001D264
	public double price { get; private set; }

	// Token: 0x0600084A RID: 2122 RVA: 0x0001F070 File Offset: 0x0001D270
	public static UpsightPurchase purchaseFromJson(string json)
	{
		UpsightPurchase upsightPurchase = new UpsightPurchase();
		Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
		if (dictionary != null)
		{
			if (dictionary.ContainsKey("placement") && dictionary["placement"] != null)
			{
				upsightPurchase.placement = dictionary["placement"].ToString();
			}
			if (dictionary.ContainsKey("quantity") && dictionary["quantity"] != null)
			{
				upsightPurchase.quantity = int.Parse(dictionary["quantity"].ToString());
			}
			if (dictionary.ContainsKey("productIdentifier") && dictionary["productIdentifier"] != null)
			{
				upsightPurchase.productIdentifier = dictionary["productIdentifier"].ToString();
			}
			if (dictionary.ContainsKey("store") && dictionary["store"] != null)
			{
				upsightPurchase.store = dictionary["store"].ToString();
			}
			if (dictionary.ContainsKey("receipt") && dictionary["receipt"] != null)
			{
				upsightPurchase.receipt = dictionary["receipt"].ToString();
			}
			if (dictionary.ContainsKey("title") && dictionary["title"] != null)
			{
				upsightPurchase.title = dictionary["title"].ToString();
			}
			if (dictionary.ContainsKey("price") && dictionary["price"] != null)
			{
				upsightPurchase.price = double.Parse(dictionary["price"].ToString());
			}
		}
		return upsightPurchase;
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0001F21C File Offset: 0x0001D41C
	public override string ToString()
	{
		return string.Format("[UpsightPurchase: placement={0}, quantity={1}, productIdentifier={2}, store={3}, receipt={4}, title={5}, price={6}]", new object[]
		{
			this.placement,
			this.quantity,
			this.productIdentifier,
			this.store,
			this.receipt,
			this.title,
			this.price
		});
	}
}
