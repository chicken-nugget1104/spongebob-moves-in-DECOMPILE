using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x020000AF RID: 175
public class SBInventoryCategory : SBTabCategory
{
	// Token: 0x06000686 RID: 1670 RVA: 0x00029F98 File Offset: 0x00028198
	public SBInventoryCategory(Dictionary<string, object> cat)
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

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000687 RID: 1671 RVA: 0x0002A024 File Offset: 0x00028224
	// (set) Token: 0x06000688 RID: 1672 RVA: 0x0002A02C File Offset: 0x0002822C
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

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000689 RID: 1673 RVA: 0x0002A038 File Offset: 0x00028238
	// (set) Token: 0x0600068A RID: 1674 RVA: 0x0002A040 File Offset: 0x00028240
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

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x0600068B RID: 1675 RVA: 0x0002A04C File Offset: 0x0002824C
	// (set) Token: 0x0600068C RID: 1676 RVA: 0x0002A054 File Offset: 0x00028254
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

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x0600068D RID: 1677 RVA: 0x0002A060 File Offset: 0x00028260
	// (set) Token: 0x0600068E RID: 1678 RVA: 0x0002A064 File Offset: 0x00028264
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

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x0600068F RID: 1679 RVA: 0x0002A068 File Offset: 0x00028268
	// (set) Token: 0x06000690 RID: 1680 RVA: 0x0002A06C File Offset: 0x0002826C
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

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000691 RID: 1681 RVA: 0x0002A070 File Offset: 0x00028270
	// (set) Token: 0x06000692 RID: 1682 RVA: 0x0002A078 File Offset: 0x00028278
	public int NumItems
	{
		get
		{
			return this.numItems;
		}
		set
		{
			this.numItems = value;
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000693 RID: 1683 RVA: 0x0002A084 File Offset: 0x00028284
	// (set) Token: 0x06000694 RID: 1684 RVA: 0x0002A08C File Offset: 0x0002828C
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

	// Token: 0x06000695 RID: 1685 RVA: 0x0002A098 File Offset: 0x00028298
	public override string ToString()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["name"] = this.name;
		dictionary["type"] = this.type;
		dictionary["texture"] = this.texture;
		dictionary["label"] = this.label;
		return Json.Serialize(dictionary);
	}

	// Token: 0x040004F2 RID: 1266
	private string name;

	// Token: 0x040004F3 RID: 1267
	private string label;

	// Token: 0x040004F4 RID: 1268
	private string type;

	// Token: 0x040004F5 RID: 1269
	private string texture;

	// Token: 0x040004F6 RID: 1270
	private int numItems;
}
