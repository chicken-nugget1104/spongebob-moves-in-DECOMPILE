using System;
using System.Collections.Generic;

// Token: 0x020000A5 RID: 165
public class GoogleSkuInfo
{
	// Token: 0x06000697 RID: 1687 RVA: 0x00017A10 File Offset: 0x00015C10
	public GoogleSkuInfo(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("title"))
		{
			this.title = (dict["title"] as string);
		}
		if (dict.ContainsKey("price"))
		{
			this.price = (dict["price"] as string);
		}
		if (dict.ContainsKey("type"))
		{
			this.type = (dict["type"] as string);
		}
		if (dict.ContainsKey("description"))
		{
			this.description = (dict["description"] as string);
		}
		if (dict.ContainsKey("productId"))
		{
			this.productId = (dict["productId"] as string);
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06000698 RID: 1688 RVA: 0x00017AE4 File Offset: 0x00015CE4
	// (set) Token: 0x06000699 RID: 1689 RVA: 0x00017AEC File Offset: 0x00015CEC
	public string title { get; private set; }

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x0600069A RID: 1690 RVA: 0x00017AF8 File Offset: 0x00015CF8
	// (set) Token: 0x0600069B RID: 1691 RVA: 0x00017B00 File Offset: 0x00015D00
	public string price { get; private set; }

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x0600069C RID: 1692 RVA: 0x00017B0C File Offset: 0x00015D0C
	// (set) Token: 0x0600069D RID: 1693 RVA: 0x00017B14 File Offset: 0x00015D14
	public string type { get; private set; }

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x0600069E RID: 1694 RVA: 0x00017B20 File Offset: 0x00015D20
	// (set) Token: 0x0600069F RID: 1695 RVA: 0x00017B28 File Offset: 0x00015D28
	public string description { get; private set; }

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00017B34 File Offset: 0x00015D34
	// (set) Token: 0x060006A1 RID: 1697 RVA: 0x00017B3C File Offset: 0x00015D3C
	public string productId { get; private set; }

	// Token: 0x060006A2 RID: 1698 RVA: 0x00017B48 File Offset: 0x00015D48
	public static List<GoogleSkuInfo> fromList(List<object> items)
	{
		List<GoogleSkuInfo> list = new List<GoogleSkuInfo>();
		foreach (object obj in items)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)obj;
			list.Add(new GoogleSkuInfo(dict));
		}
		return list;
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00017BBC File Offset: 0x00015DBC
	public override string ToString()
	{
		return string.Format("<GoogleSkuInfo> title: {0}, price: {1}, type: {2}, description: {3}, productId: {4}", new object[]
		{
			this.title,
			this.price,
			this.type,
			this.description,
			this.productId
		});
	}
}
