using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class RmtProduct
{
	// Token: 0x060009CE RID: 2510 RVA: 0x0003D50C File Offset: 0x0003B70C
	public RmtProduct(Dictionary<string, object> data)
	{
		this.productId = TFUtils.LoadString(data, "productId");
		this.localizedprice = TFUtils.LoadString(data, "localizedprice");
		this.currencyCode = TFUtils.LoadString(data, "currencyCode", "USD");
		try
		{
			if (data.ContainsKey("price"))
			{
				this.price = TFUtils.LoadFloat(data, "price");
			}
			else
			{
				float.TryParse(this.localizedprice, out this.price);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("RmtProduct: Failed to parse price: " + ex.Message);
			this.price = 0f;
		}
	}

	// Token: 0x040006BF RID: 1727
	public string localizedprice;

	// Token: 0x040006C0 RID: 1728
	public string currencyCode;

	// Token: 0x040006C1 RID: 1729
	public string productId;

	// Token: 0x040006C2 RID: 1730
	public float price;
}
