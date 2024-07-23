using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000112 RID: 274
public class SBInventoryItem : IComparable<SBInventoryItem>
{
	// Token: 0x060009DE RID: 2526 RVA: 0x0003DA94 File Offset: 0x0003BC94
	public SBInventoryItem(Entity entity, List<KeyValuePair<int, Identity>> associatedEntities, string type, string name, string description, string filename, bool isDiscardable, string movieFileName = null)
	{
		this.entity = entity;
		this.associatedEntities = associatedEntities;
		this.displayName = name;
		this.iconFilename = filename;
		this.description = description;
		this.discardable = isDiscardable;
		this.itemType = type;
		this.movieFileName = movieFileName;
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x0003DAE4 File Offset: 0x0003BCE4
	public override string ToString()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["entity.did"] = this.entity.DefinitionId;
		dictionary["itemType"] = this.itemType;
		dictionary["displayName"] = this.displayName;
		dictionary["iconFilename"] = this.iconFilename;
		dictionary["description"] = this.description;
		dictionary["discardable"] = this.discardable;
		dictionary["movieFileName"] = this.movieFileName;
		return Json.Serialize(dictionary);
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x0003DB84 File Offset: 0x0003BD84
	public int CompareTo(SBInventoryItem rhs)
	{
		string text = Language.Get(this.displayName);
		string strB = Language.Get(rhs.displayName);
		return text.CompareTo(strB);
	}

	// Token: 0x040006C6 RID: 1734
	public Entity entity;

	// Token: 0x040006C7 RID: 1735
	public List<KeyValuePair<int, Identity>> associatedEntities;

	// Token: 0x040006C8 RID: 1736
	public string itemType;

	// Token: 0x040006C9 RID: 1737
	public string displayName;

	// Token: 0x040006CA RID: 1738
	public string iconFilename;

	// Token: 0x040006CB RID: 1739
	public bool discardable;

	// Token: 0x040006CC RID: 1740
	public string description;

	// Token: 0x040006CD RID: 1741
	public string movieFileName;
}
