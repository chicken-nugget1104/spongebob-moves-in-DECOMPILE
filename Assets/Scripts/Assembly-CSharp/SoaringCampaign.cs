using System;

// Token: 0x0200036B RID: 875
public class SoaringCampaign : SoaringDictionary
{
	// Token: 0x060018F4 RID: 6388 RVA: 0x000A4C00 File Offset: 0x000A2E00
	public SoaringCampaign(SoaringDictionary dictionary)
	{
		base.CopyExisting(dictionary);
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x060018F5 RID: 6389 RVA: 0x000A4C10 File Offset: 0x000A2E10
	public string CampaignId
	{
		get
		{
			return base.soaringValue("campaignId");
		}
	}

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x060018F6 RID: 6390 RVA: 0x000A4C24 File Offset: 0x000A2E24
	public string Description
	{
		get
		{
			return base.soaringValue("description");
		}
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x060018F7 RID: 6391 RVA: 0x000A4C38 File Offset: 0x000A2E38
	public string Group
	{
		get
		{
			return base.soaringValue("group");
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x060018F8 RID: 6392 RVA: 0x000A4C4C File Offset: 0x000A2E4C
	public string Custom
	{
		get
		{
			return base.soaringValue("custom");
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x060018F9 RID: 6393 RVA: 0x000A4C60 File Offset: 0x000A2E60
	public string CampaignType
	{
		get
		{
			return base.soaringValue("type");
		}
	}
}
