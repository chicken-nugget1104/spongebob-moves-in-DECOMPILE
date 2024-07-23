using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x020000B0 RID: 176
public class SBMarketCategory : SBTabCategory
{
	// Token: 0x06000696 RID: 1686 RVA: 0x0002A0F8 File Offset: 0x000282F8
	public SBMarketCategory(Dictionary<string, object> cat)
	{
		this.name = (string)cat["name"];
		if (cat.ContainsKey("display.material"))
		{
			this.texture = (string)cat["display.material"];
		}
		if (cat.ContainsKey("type"))
		{
			this.type = (string)cat["type"];
		}
		this.label = (string)cat["label"];
		List<int> list = ((List<object>)cat["dids"]).ConvertAll<int>((object x) => Convert.ToInt32(x));
		if (cat.ContainsKey("micro_event_did"))
		{
			this.microEventDID = TFUtils.LoadInt(cat, "micro_event_did");
		}
		else
		{
			this.microEventDID = -1;
		}
		if (cat.ContainsKey("event_only"))
		{
			this.microEventOnly = TFUtils.LoadBool(cat, "event_only");
		}
		else
		{
			this.microEventOnly = false;
		}
		this.dids = list.ToArray();
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000697 RID: 1687 RVA: 0x0002A21C File Offset: 0x0002841C
	// (set) Token: 0x06000698 RID: 1688 RVA: 0x0002A224 File Offset: 0x00028424
	public string Name
	{
		get
		{
			return this.name;
		}
		set
		{
			this.name = value;
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000699 RID: 1689 RVA: 0x0002A230 File Offset: 0x00028430
	public string DeltaDNAName
	{
		get
		{
			return Catalog.ConvertTypeToDeltaDNAType(this.Name);
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x0600069A RID: 1690 RVA: 0x0002A240 File Offset: 0x00028440
	// (set) Token: 0x0600069B RID: 1691 RVA: 0x0002A248 File Offset: 0x00028448
	public string Type
	{
		get
		{
			return this.type;
		}
		set
		{
			this.type = value;
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600069C RID: 1692 RVA: 0x0002A254 File Offset: 0x00028454
	// (set) Token: 0x0600069D RID: 1693 RVA: 0x0002A25C File Offset: 0x0002845C
	public string Texture
	{
		get
		{
			return this.texture;
		}
		set
		{
			this.texture = value;
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600069E RID: 1694 RVA: 0x0002A268 File Offset: 0x00028468
	// (set) Token: 0x0600069F RID: 1695 RVA: 0x0002A270 File Offset: 0x00028470
	public int MicroEventDID
	{
		get
		{
			return this.microEventDID;
		}
		set
		{
			this.microEventDID = value;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0002A27C File Offset: 0x0002847C
	// (set) Token: 0x060006A1 RID: 1697 RVA: 0x0002A284 File Offset: 0x00028484
	public bool MicroEventOnly
	{
		get
		{
			return this.microEventOnly;
		}
		set
		{
			this.microEventOnly = value;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0002A290 File Offset: 0x00028490
	// (set) Token: 0x060006A3 RID: 1699 RVA: 0x0002A298 File Offset: 0x00028498
	public string Label
	{
		get
		{
			return this.label;
		}
		set
		{
			this.label = value;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0002A2A4 File Offset: 0x000284A4
	// (set) Token: 0x060006A5 RID: 1701 RVA: 0x0002A2AC File Offset: 0x000284AC
	public int[] Dids
	{
		get
		{
			return this.dids;
		}
		set
		{
			this.dids = value;
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x0002A2B8 File Offset: 0x000284B8
	public override string ToString()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["name"] = this.name;
		dictionary["type"] = this.type;
		dictionary["dids"] = this.dids;
		dictionary["texture"] = this.texture;
		dictionary["label"] = this.label;
		return Json.Serialize(dictionary);
	}

	// Token: 0x040004F7 RID: 1271
	private string name;

	// Token: 0x040004F8 RID: 1272
	private string type;

	// Token: 0x040004F9 RID: 1273
	private string texture;

	// Token: 0x040004FA RID: 1274
	private string deltaDNAName;

	// Token: 0x040004FB RID: 1275
	private int[] dids;

	// Token: 0x040004FC RID: 1276
	private string label;

	// Token: 0x040004FD RID: 1277
	private int microEventDID;

	// Token: 0x040004FE RID: 1278
	private bool microEventOnly;
}
