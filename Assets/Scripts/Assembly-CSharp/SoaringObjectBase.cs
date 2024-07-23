using System;

// Token: 0x020003D6 RID: 982
public class SoaringObjectBase
{
	// Token: 0x06001D89 RID: 7561 RVA: 0x000B9D80 File Offset: 0x000B7F80
	public SoaringObjectBase(SoaringObjectBase.IsType t)
	{
		this.mType = t;
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06001D8A RID: 7562 RVA: 0x000B9D90 File Offset: 0x000B7F90
	public SoaringObjectBase.IsType Type
	{
		get
		{
			return this.mType;
		}
	}

	// Token: 0x06001D8B RID: 7563 RVA: 0x000B9D98 File Offset: 0x000B7F98
	public virtual string ToJsonString()
	{
		return this.ToString();
	}

	// Token: 0x040012A6 RID: 4774
	protected SoaringObjectBase.IsType mType;

	// Token: 0x020003D7 RID: 983
	public enum IsType
	{
		// Token: 0x040012A8 RID: 4776
		Int,
		// Token: 0x040012A9 RID: 4777
		Float,
		// Token: 0x040012AA RID: 4778
		String,
		// Token: 0x040012AB RID: 4779
		Array,
		// Token: 0x040012AC RID: 4780
		Dictionary,
		// Token: 0x040012AD RID: 4781
		Object,
		// Token: 0x040012AE RID: 4782
		Module,
		// Token: 0x040012AF RID: 4783
		Management,
		// Token: 0x040012B0 RID: 4784
		Boolean,
		// Token: 0x040012B1 RID: 4785
		Null
	}
}
