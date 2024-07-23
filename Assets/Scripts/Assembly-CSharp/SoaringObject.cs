using System;

// Token: 0x020003D5 RID: 981
public class SoaringObject : SoaringObjectBase
{
	// Token: 0x06001D85 RID: 7557 RVA: 0x000B9D1C File Offset: 0x000B7F1C
	public SoaringObject(object obj) : base(SoaringObjectBase.IsType.Object)
	{
		this.mObject = obj;
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06001D86 RID: 7558 RVA: 0x000B9D2C File Offset: 0x000B7F2C
	public object Object
	{
		get
		{
			return this.mObject;
		}
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x000B9D34 File Offset: 0x000B7F34
	public override string ToString()
	{
		string result;
		if (this.mObject != null)
		{
			result = this.mObject.ToString();
		}
		else
		{
			result = string.Empty;
		}
		return result;
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x000B9D68 File Offset: 0x000B7F68
	public override string ToJsonString()
	{
		return "\"" + this.ToString() + "\"";
	}

	// Token: 0x040012A5 RID: 4773
	private object mObject;
}
