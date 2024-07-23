using System;

// Token: 0x020003DB RID: 987
public class SoaringNullValue : SoaringValue
{
	// Token: 0x06001DB5 RID: 7605 RVA: 0x000BA330 File Offset: 0x000B8530
	public SoaringNullValue() : base(0)
	{
		this.mType = SoaringObjectBase.IsType.Null;
	}

	// Token: 0x06001DB6 RID: 7606 RVA: 0x000BA344 File Offset: 0x000B8544
	public override string ToString()
	{
		return "null";
	}

	// Token: 0x06001DB7 RID: 7607 RVA: 0x000BA34C File Offset: 0x000B854C
	public override string ToJsonString()
	{
		return this.ToString();
	}
}
