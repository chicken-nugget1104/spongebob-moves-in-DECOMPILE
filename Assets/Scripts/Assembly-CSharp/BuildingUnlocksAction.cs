using System;
using System.Collections.Generic;

// Token: 0x020000C8 RID: 200
public class BuildingUnlocksAction : PersistedTriggerableAction
{
	// Token: 0x06000794 RID: 1940 RVA: 0x00031E68 File Offset: 0x00030068
	public BuildingUnlocksAction(List<int> buildings) : base("ub", Identity.Null())
	{
		this.buildings = buildings;
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000795 RID: 1941 RVA: 0x00031E84 File Offset: 0x00030084
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00031E88 File Offset: 0x00030088
	public new static BuildingUnlocksAction FromDict(Dictionary<string, object> data)
	{
		List<int> list = TFUtils.LoadList<int>(data, "building_unlocks");
		return new BuildingUnlocksAction(list);
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00031EAC File Offset: 0x000300AC
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["building_unlocks"] = TFUtils.CloneAndCastList<int, object>(this.buildings);
		return dictionary;
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00031ED8 File Offset: 0x000300D8
	public override void Apply(Game game, ulong utcNow)
	{
		foreach (int buildingDid in this.buildings)
		{
			game.buildingUnlockManager.UnlockBuilding(buildingDid);
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00031F4C File Offset: 0x0003014C
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("building_unlocks"))
		{
			dictionary["building_unlocks"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["building_unlocks"];
		foreach (int num in this.buildings)
		{
			if (!list.Contains(num))
			{
				list.Add(num);
			}
		}
		base.Confirm(gameState);
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00032014 File Offset: 0x00030214
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00032018 File Offset: 0x00030218
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005A8 RID: 1448
	public const string UNLOCK_BUILDING = "ub";

	// Token: 0x040005A9 RID: 1449
	public List<int> buildings;
}
