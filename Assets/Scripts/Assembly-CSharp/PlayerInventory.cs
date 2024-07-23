using System;
using System.Collections.Generic;

// Token: 0x020001A9 RID: 425
public class PlayerInventory
{
	// Token: 0x06000E33 RID: 3635 RVA: 0x000568F4 File Offset: 0x00054AF4
	public void AddItem(BuildingEntity entity, List<KeyValuePair<int, Identity>> associatedEntities)
	{
		string name = (string)entity.Invariable["name"];
		string filename = (string)entity.Invariable["portrait"];
		SBInventoryItem item = new SBInventoryItem(entity, associatedEntities, "stashed", name, string.Empty, filename, false, null);
		this.items.Add(item);
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x00056950 File Offset: 0x00054B50
	public void AddAssociatedEntities(Identity entityId, List<KeyValuePair<int, Identity>> associatedEntities)
	{
		foreach (SBInventoryItem sbinventoryItem in this.items)
		{
			if (sbinventoryItem.entity.Id.Equals(entityId))
			{
				if (sbinventoryItem.associatedEntities == null)
				{
					sbinventoryItem.associatedEntities = associatedEntities;
				}
				else
				{
					sbinventoryItem.associatedEntities.AddRange(associatedEntities);
				}
				break;
			}
		}
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x000569F0 File Offset: 0x00054BF0
	public bool HasItem(int? did)
	{
		using (List<SBInventoryItem>.Enumerator enumerator = this.items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.entity.DefinitionId == did)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x00056A7C File Offset: 0x00054C7C
	public bool HasItem(Identity ID)
	{
		foreach (SBInventoryItem sbinventoryItem in this.items)
		{
			if (sbinventoryItem.entity.Id.Equals(ID))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x00056AFC File Offset: 0x00054CFC
	public int GetNumItems(int? did)
	{
		int num = 0;
		using (List<SBInventoryItem>.Enumerator enumerator = this.items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.entity.DefinitionId == did)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x00056B88 File Offset: 0x00054D88
	public int GetNumItems(Identity ID)
	{
		int num = 0;
		foreach (SBInventoryItem sbinventoryItem in this.items)
		{
			if (sbinventoryItem.entity.Id.Equals(ID))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x00056C04 File Offset: 0x00054E04
	public List<SBInventoryItem> GetItems()
	{
		return this.items;
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x00056C0C File Offset: 0x00054E0C
	public Entity RemoveEntity(Identity id, out List<KeyValuePair<int, Identity>> outAssociatedEntities)
	{
		Entity result = null;
		outAssociatedEntities = new List<KeyValuePair<int, Identity>>();
		for (int i = 0; i < this.items.Count; i++)
		{
			if (this.items[i].entity.Id.Equals(id))
			{
				result = this.items[i].entity;
				outAssociatedEntities = this.items[i].associatedEntities;
				this.items.RemoveAt(i);
				break;
			}
		}
		return result;
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x00056C98 File Offset: 0x00054E98
	public int GetNumUniqueItems()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (SBInventoryItem sbinventoryItem in this.items)
		{
			if (!dictionary.ContainsKey(sbinventoryItem.entity.DefinitionId))
			{
				dictionary.Add(sbinventoryItem.entity.DefinitionId, 1);
			}
		}
		return dictionary.Count;
	}

	// Token: 0x0400095B RID: 2395
	private List<SBInventoryItem> items = new List<SBInventoryItem>();
}
