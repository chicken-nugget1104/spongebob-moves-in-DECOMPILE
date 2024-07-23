using System;

// Token: 0x020003CF RID: 975
public class SoaringContext : SoaringDictionary
{
	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06001D34 RID: 7476 RVA: 0x000B83F0 File Offset: 0x000B65F0
	// (set) Token: 0x06001D35 RID: 7477 RVA: 0x000B83F8 File Offset: 0x000B65F8
	public string Name
	{
		get
		{
			return this.mContextName;
		}
		set
		{
			this.mContextName = value;
		}
	}

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x06001D36 RID: 7478 RVA: 0x000B8404 File Offset: 0x000B6604
	// (set) Token: 0x06001D37 RID: 7479 RVA: 0x000B840C File Offset: 0x000B660C
	public SoaringDelegate Responder
	{
		get
		{
			return this.mMainResponder;
		}
		set
		{
			this.mMainResponder = value;
		}
	}

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06001D38 RID: 7480 RVA: 0x000B8418 File Offset: 0x000B6618
	// (set) Token: 0x06001D39 RID: 7481 RVA: 0x000B8420 File Offset: 0x000B6620
	public SoaringContextDelegate ContextResponder
	{
		get
		{
			return this.mContextResponder;
		}
		set
		{
			this.mContextResponder = value;
		}
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x000B842C File Offset: 0x000B662C
	public static implicit operator SoaringContext(SoaringDelegate b)
	{
		SoaringContext soaringContext = new SoaringContext();
		if (b != null)
		{
			soaringContext.Responder = b;
		}
		return soaringContext;
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x000B8450 File Offset: 0x000B6650
	public static implicit operator SoaringContext(string b)
	{
		SoaringContext soaringContext = new SoaringContext();
		if (b != null)
		{
			soaringContext.mContextName = b;
		}
		return soaringContext;
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x000B8474 File Offset: 0x000B6674
	public static implicit operator string(SoaringContext b)
	{
		string result = null;
		if (b != null)
		{
			result = b.mContextName;
		}
		return result;
	}

	// Token: 0x0400128D RID: 4749
	private const string kDefaultContextName = "_def";

	// Token: 0x0400128E RID: 4750
	private string mContextName = "_def";

	// Token: 0x0400128F RID: 4751
	private SoaringDelegate mMainResponder;

	// Token: 0x04001290 RID: 4752
	private SoaringContextDelegate mContextResponder;
}
