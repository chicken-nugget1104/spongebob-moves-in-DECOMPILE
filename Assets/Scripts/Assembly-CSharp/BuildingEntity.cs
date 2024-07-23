using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003F7 RID: 1015
public class BuildingEntity : EntityDecorator
{
	// Token: 0x06001F16 RID: 7958 RVA: 0x000BF1B0 File Offset: 0x000BD3B0
	public BuildingEntity(Entity toDecorate) : base(toDecorate)
	{
		new StructureDecorator(this);
		new ErectableDecorator(this);
		new ActivatableDecorator(this);
		if (this.Invariable["product"] != null)
		{
			new PeriodicProductionDecorator(this);
		}
		if (this.Invariable.ContainsKey("vendor_id"))
		{
			new VendingDecorator(this);
		}
		this.Variable["annexes"] = new List<Entity>();
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06001F17 RID: 7959 RVA: 0x000BF228 File Offset: 0x000BD428
	public override EntityType Type
	{
		get
		{
			return EntityType.BUILDING;
		}
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06001F18 RID: 7960 RVA: 0x000BF22C File Offset: 0x000BD42C
	public List<Entity> Annexes
	{
		get
		{
			return (List<Entity>)this.Variable["annexes"];
		}
	}

	// Token: 0x06001F19 RID: 7961 RVA: 0x000BF244 File Offset: 0x000BD444
	public void RegisterAnnex(Entity annex)
	{
		List<Entity> list = (List<Entity>)this.Variable["annexes"];
		list.Add(annex);
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06001F1A RID: 7962 RVA: 0x000BF270 File Offset: 0x000BD470
	public List<int> ResidentDids
	{
		get
		{
			return (List<int>)this.Invariable["residents"];
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06001F1B RID: 7963 RVA: 0x000BF288 File Offset: 0x000BD488
	public int? PetDid
	{
		get
		{
			return (int?)this.Invariable["pet"];
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06001F1C RID: 7964 RVA: 0x000BF2A0 File Offset: 0x000BD4A0
	public Vector2 PointOfInterestOffset
	{
		get
		{
			return (Vector2)this.Invariable["point_of_interest"];
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06001F1D RID: 7965 RVA: 0x000BF2B8 File Offset: 0x000BD4B8
	public bool HasResident
	{
		get
		{
			return !this.Invariable.ContainsKey("residents") || this.Invariable["residents"] != null;
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06001F1E RID: 7966 RVA: 0x000BF2F4 File Offset: 0x000BD4F4
	public bool CanCraft
	{
		get
		{
			return this.Invariable.ContainsKey("crafting_menu") && this.Invariable["crafting_menu"] != null;
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06001F1F RID: 7967 RVA: 0x000BF330 File Offset: 0x000BD530
	public int CraftMenu
	{
		get
		{
			return (int)this.Invariable["crafting_menu"];
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06001F20 RID: 7968 RVA: 0x000BF348 File Offset: 0x000BD548
	public bool ShuntsCrafting
	{
		get
		{
			return (bool)this.Invariable["shunts_crafting"];
		}
	}

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06001F21 RID: 7969 RVA: 0x000BF360 File Offset: 0x000BD560
	public bool HasSlots
	{
		get
		{
			return this.Variable.ContainsKey("crafting_slots") && this.Slots != -1;
		}
	}

	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06001F22 RID: 7970 RVA: 0x000BF394 File Offset: 0x000BD594
	// (set) Token: 0x06001F23 RID: 7971 RVA: 0x000BF3E0 File Offset: 0x000BD5E0
	public int Slots
	{
		get
		{
			TFUtils.Assert(this.Variable.ContainsKey("crafting_slots"), "Trying to lookup production slots on this entity, but none were assigned. Is there an appropriate production slots file linked to this entity? EntityDid=" + this.DefinitionId);
			return (int)this.Variable["crafting_slots"];
		}
		set
		{
			this.Variable["crafting_slots"] = value;
		}
	}

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06001F24 RID: 7972 RVA: 0x000BF3F8 File Offset: 0x000BD5F8
	// (set) Token: 0x06001F25 RID: 7973 RVA: 0x000BF438 File Offset: 0x000BD638
	public Reward CraftRewards
	{
		get
		{
			return (!this.Variable.ContainsKey("craft_rewards")) ? null : ((Reward)this.Variable["craft_rewards"]);
		}
		set
		{
			this.Variable["craft_rewards"] = value;
		}
	}

	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06001F26 RID: 7974 RVA: 0x000BF44C File Offset: 0x000BD64C
	// (set) Token: 0x06001F27 RID: 7975 RVA: 0x000BF488 File Offset: 0x000BD688
	public int TaskSourceFeedDID
	{
		get
		{
			if (this.Variable.ContainsKey("task_source_feed_did"))
			{
				return (int)this.Variable["task_source_feed_did"];
			}
			return 0;
		}
		set
		{
			this.Variable["task_source_feed_did"] = value;
		}
	}

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06001F28 RID: 7976 RVA: 0x000BF4A0 File Offset: 0x000BD6A0
	public bool CanVend
	{
		get
		{
			return this.Invariable.ContainsKey("vendor_id");
		}
	}

	// Token: 0x1700042B RID: 1067
	// (get) Token: 0x06001F29 RID: 7977 RVA: 0x000BF4B4 File Offset: 0x000BD6B4
	public string OverrideRewardTexture
	{
		get
		{
			if (this.Invariable.ContainsKey("crafted_icon"))
			{
				return (string)this.Invariable["crafted_icon"];
			}
			return null;
		}
	}

	// Token: 0x1700042C RID: 1068
	// (get) Token: 0x06001F2A RID: 7978 RVA: 0x000BF4F0 File Offset: 0x000BD6F0
	public bool Stashable
	{
		get
		{
			return (bool)this.Invariable["stashable"];
		}
	}

	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06001F2B RID: 7979 RVA: 0x000BF508 File Offset: 0x000BD708
	public bool Flippable
	{
		get
		{
			return (bool)this.Invariable["flippable"];
		}
	}

	// Token: 0x06001F2C RID: 7980 RVA: 0x000BF520 File Offset: 0x000BD720
	public void CraftingComplete(Reward reward)
	{
		Reward craftRewards = this.CraftRewards;
		if (craftRewards != null)
		{
			this.CraftRewards = craftRewards + reward;
		}
		else
		{
			this.CraftRewards = reward;
		}
	}

	// Token: 0x06001F2D RID: 7981 RVA: 0x000BF554 File Offset: 0x000BD754
	public void ClearCraftingRewards()
	{
		if (this.Variable.ContainsKey("craft_rewards"))
		{
			this.Variable.Remove("craft_rewards");
		}
		else if (this.Variable.ContainsKey("craft.rewards"))
		{
			this.Variable.Remove("craft.rewards");
		}
	}

	// Token: 0x06001F2E RID: 7982 RVA: 0x000BF5B0 File Offset: 0x000BD7B0
	public void AddCraftingSlot()
	{
		this.Variable["crafting_slots"] = this.Slots + 1;
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06001F2F RID: 7983 RVA: 0x000BF5DC File Offset: 0x000BD7DC
	// (set) Token: 0x06001F30 RID: 7984 RVA: 0x000BF5F4 File Offset: 0x000BD7F4
	public int BusyAnnexCount
	{
		get
		{
			return (int)this.Variable["busy_annex_count"];
		}
		set
		{
			this.Variable["busy_annex_count"] = value;
		}
	}

	// Token: 0x04001344 RID: 4932
	public const string TYPE = "building";

	// Token: 0x04001345 RID: 4933
	public const string ANNEXES = "annexes";

	// Token: 0x04001346 RID: 4934
	public const string TASKBOOK_ID = "taskbook_id";

	// Token: 0x04001347 RID: 4935
	public const string SHUNTS_CRAFTING = "shunts_crafting";

	// Token: 0x04001348 RID: 4936
	public const string CRAFTING_SLOTS = "crafting_slots";

	// Token: 0x04001349 RID: 4937
	public const string RESIDENTS = "residents";
}
