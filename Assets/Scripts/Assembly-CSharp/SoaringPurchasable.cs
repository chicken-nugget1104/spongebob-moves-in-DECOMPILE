using System;

// Token: 0x020003D8 RID: 984
public class SoaringPurchasable : SoaringObjectBase
{
	// Token: 0x06001D8C RID: 7564 RVA: 0x000B9DA0 File Offset: 0x000B7FA0
	public SoaringPurchasable(SoaringDictionary data) : base(SoaringObjectBase.IsType.Object)
	{
		this.mProductID = data.soaringValue("productId");
		this.mAlias = data.soaringValue("alias");
		this.mPriceTier = data.soaringValue("priceTier");
		this.mScreenshot = data.soaringValue("screenshotName");
		this.mDisplayName = data.soaringValue("displayName");
		this.mAmount = data.soaringValue("amount");
		this.mDescription = data.soaringValue("description");
		this.mTexture = data.soaringValue("screenshotName");
		this.mUSDPrice = data.soaringValue("usdPrice");
		string s = data.soaringValue("resourceType");
		this.mResourceType = -1;
		int.TryParse(s, out this.mResourceType);
		if (this.mResourceType < 0)
		{
			SoaringDebug.Log("SoaringPurchasable: failed to parse resourceType as integer.");
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06001D8D RID: 7565 RVA: 0x000B9EB8 File Offset: 0x000B80B8
	public string ProductID
	{
		get
		{
			return this.mProductID;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06001D8E RID: 7566 RVA: 0x000B9EC0 File Offset: 0x000B80C0
	public string Alias
	{
		get
		{
			return this.mAlias;
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06001D8F RID: 7567 RVA: 0x000B9EC8 File Offset: 0x000B80C8
	public string PriceTier
	{
		get
		{
			return this.mPriceTier;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06001D90 RID: 7568 RVA: 0x000B9ED0 File Offset: 0x000B80D0
	public string Screenshot
	{
		get
		{
			return this.mScreenshot;
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06001D91 RID: 7569 RVA: 0x000B9ED8 File Offset: 0x000B80D8
	public string DisplayName
	{
		get
		{
			return this.mDisplayName;
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x06001D92 RID: 7570 RVA: 0x000B9EE0 File Offset: 0x000B80E0
	public int Amount
	{
		get
		{
			return this.mAmount;
		}
	}

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x06001D93 RID: 7571 RVA: 0x000B9EE8 File Offset: 0x000B80E8
	public string Description
	{
		get
		{
			return this.mDescription;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x06001D94 RID: 7572 RVA: 0x000B9EF0 File Offset: 0x000B80F0
	public int ResourceType
	{
		get
		{
			return this.mResourceType;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06001D95 RID: 7573 RVA: 0x000B9EF8 File Offset: 0x000B80F8
	public int USDPrice
	{
		get
		{
			return this.mUSDPrice;
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06001D96 RID: 7574 RVA: 0x000B9F00 File Offset: 0x000B8100
	public string Texture
	{
		get
		{
			return this.mTexture;
		}
	}

	// Token: 0x040012B2 RID: 4786
	private string mProductID;

	// Token: 0x040012B3 RID: 4787
	private string mAlias;

	// Token: 0x040012B4 RID: 4788
	private string mPriceTier;

	// Token: 0x040012B5 RID: 4789
	private string mScreenshot;

	// Token: 0x040012B6 RID: 4790
	private string mDisplayName;

	// Token: 0x040012B7 RID: 4791
	private int mAmount;

	// Token: 0x040012B8 RID: 4792
	private string mDescription;

	// Token: 0x040012B9 RID: 4793
	private int mResourceType;

	// Token: 0x040012BA RID: 4794
	private int mUSDPrice;

	// Token: 0x040012BB RID: 4795
	private string mTexture;
}
