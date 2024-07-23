using System;

// Token: 0x020003D9 RID: 985
public class SoaringPurchase : SoaringObjectBase
{
	// Token: 0x06001D97 RID: 7575 RVA: 0x000B9F08 File Offset: 0x000B8108
	public SoaringPurchase(SoaringDictionary data, SoaringPurchasable purchasable) : base(SoaringObjectBase.IsType.Object)
	{
		this.mPurchasable = purchasable;
		this.mProductID = data.soaringValue("productId");
		this.mDatetime = data.soaringValue("datetime");
		this.mValid = data.soaringValue("valid");
		this.mGift = data.soaringValue("gift");
		this.mUpdateDatetime = data.soaringValue("updateDatetime");
		this.mAmount = data.soaringValue("amount");
		string s = data.soaringValue("resourceType");
		this.mResourceType = -1;
		int.TryParse(s, out this.mResourceType);
		if (this.mResourceType < 0)
		{
			SoaringDebug.Log("SoaringPurchase: failed to parse resourceType as integer.");
		}
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x06001D98 RID: 7576 RVA: 0x000B9FE4 File Offset: 0x000B81E4
	public SoaringPurchasable Purchasable
	{
		get
		{
			return this.mPurchasable;
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x06001D99 RID: 7577 RVA: 0x000B9FEC File Offset: 0x000B81EC
	public string ProductID
	{
		get
		{
			return this.mProductID;
		}
	}

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x06001D9A RID: 7578 RVA: 0x000B9FF4 File Offset: 0x000B81F4
	public string Datetime
	{
		get
		{
			return this.mDatetime;
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x06001D9B RID: 7579 RVA: 0x000B9FFC File Offset: 0x000B81FC
	public string UpdateDatetime
	{
		get
		{
			return this.mUpdateDatetime;
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06001D9C RID: 7580 RVA: 0x000BA004 File Offset: 0x000B8204
	public bool Gift
	{
		get
		{
			return this.mGift;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x06001D9D RID: 7581 RVA: 0x000BA00C File Offset: 0x000B820C
	public bool Valid
	{
		get
		{
			return this.mValid;
		}
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06001D9E RID: 7582 RVA: 0x000BA014 File Offset: 0x000B8214
	public int Amount
	{
		get
		{
			return this.mAmount;
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06001D9F RID: 7583 RVA: 0x000BA01C File Offset: 0x000B821C
	public int ResourceType
	{
		get
		{
			return this.mResourceType;
		}
	}

	// Token: 0x040012BC RID: 4796
	private SoaringPurchasable mPurchasable;

	// Token: 0x040012BD RID: 4797
	private string mProductID;

	// Token: 0x040012BE RID: 4798
	private string mDatetime;

	// Token: 0x040012BF RID: 4799
	private string mUpdateDatetime;

	// Token: 0x040012C0 RID: 4800
	private bool mGift;

	// Token: 0x040012C1 RID: 4801
	private bool mValid;

	// Token: 0x040012C2 RID: 4802
	private int mAmount;

	// Token: 0x040012C3 RID: 4803
	private int mResourceType;
}
