using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x020000B1 RID: 177
public class SBCommunityEventCategory : SBTabCategory
{
	// Token: 0x060006A8 RID: 1704 RVA: 0x0002A330 File Offset: 0x00028530
	public SBCommunityEventCategory(Dictionary<string, object> cat)
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
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0002A3BC File Offset: 0x000285BC
	// (set) Token: 0x060006AA RID: 1706 RVA: 0x0002A3C4 File Offset: 0x000285C4
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

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x060006AB RID: 1707 RVA: 0x0002A3D0 File Offset: 0x000285D0
	// (set) Token: 0x060006AC RID: 1708 RVA: 0x0002A3D8 File Offset: 0x000285D8
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

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x060006AD RID: 1709 RVA: 0x0002A3E4 File Offset: 0x000285E4
	// (set) Token: 0x060006AE RID: 1710 RVA: 0x0002A3EC File Offset: 0x000285EC
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

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x060006AF RID: 1711 RVA: 0x0002A3F8 File Offset: 0x000285F8
	// (set) Token: 0x060006B0 RID: 1712 RVA: 0x0002A3FC File Offset: 0x000285FC
	public int MicroEventDID
	{
		get
		{
			return -1;
		}
		set
		{
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0002A400 File Offset: 0x00028600
	// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0002A404 File Offset: 0x00028604
	public bool MicroEventOnly
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0002A408 File Offset: 0x00028608
	// (set) Token: 0x060006B4 RID: 1716 RVA: 0x0002A410 File Offset: 0x00028610
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

	// Token: 0x060006B5 RID: 1717 RVA: 0x0002A41C File Offset: 0x0002861C
	public override string ToString()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["name"] = this.name;
		dictionary["type"] = this.type;
		dictionary["texture"] = this.texture;
		dictionary["label"] = this.label;
		return Json.Serialize(dictionary);
	}

	// Token: 0x04000500 RID: 1280
	private string name;

	// Token: 0x04000501 RID: 1281
	private string type;

	// Token: 0x04000502 RID: 1282
	private string texture;

	// Token: 0x04000503 RID: 1283
	private string label;
}
