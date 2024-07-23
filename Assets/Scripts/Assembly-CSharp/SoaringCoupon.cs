using System;

// Token: 0x020003C3 RID: 963
public class SoaringCoupon : SoaringObjectBase
{
	// Token: 0x06001C9A RID: 7322 RVA: 0x000B59C0 File Offset: 0x000B3BC0
	public SoaringCoupon(string id, string reciept) : base(SoaringObjectBase.IsType.Object)
	{
		this.mCoupon = id;
		this.mReciept = reciept;
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x000B59D8 File Offset: 0x000B3BD8
	public override string ToString()
	{
		return this.mCoupon;
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x000B59E0 File Offset: 0x000B3BE0
	public override string ToJsonString()
	{
		return string.Concat(new string[]
		{
			"{\n\"coupon\" : \"",
			this.mCoupon,
			"\",\n\"receipt\" : \"",
			this.mReciept,
			"\"\n}"
		});
	}

	// Token: 0x0400125C RID: 4700
	private string mCoupon;

	// Token: 0x0400125D RID: 4701
	private string mReciept;
}
