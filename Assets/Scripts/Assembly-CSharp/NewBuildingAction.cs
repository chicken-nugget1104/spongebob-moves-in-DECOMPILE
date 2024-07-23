using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class NewBuildingAction : PersistedSimulatedAction
{
	// Token: 0x06000858 RID: 2136 RVA: 0x00035D20 File Offset: 0x00033F20
	public NewBuildingAction(Identity id, string blueprint, int did, EntityType types, bool built, ulong buildCompleteTime, Vector2 position, bool flip, Cost cost) : base("nb", id, typeof(NewBuildingAction).ToString())
	{
		this.Initialize(blueprint, did, types, built, buildCompleteTime, position, flip, cost);
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00035D5C File Offset: 0x00033F5C
	public NewBuildingAction(Simulated simulated, Cost cost) : base("nb", simulated.Id, typeof(NewBuildingAction).ToString())
	{
		Entity entity = simulated.entity;
		ErectableDecorator decorator = entity.GetDecorator<ErectableDecorator>();
		this.Initialize(entity.BlueprintName, entity.DefinitionId, entity.AllTypes, false, decorator.ErectionCompleteTime.Value, simulated.Position, simulated.Flip, cost);
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x0600085A RID: 2138 RVA: 0x00035DCC File Offset: 0x00033FCC
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00035DD0 File Offset: 0x00033FD0
	public new static NewBuildingAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		bool flag = (bool)data["built"];
		string text = (string)data["blueprint"];
		int did = TFUtils.LoadInt(data, "did");
		EntityType types = (EntityType)TFUtils.LoadUint(data, "extensions");
		ulong num = TFUtils.LoadUlong(data, "buildCompleteTime", 0UL);
		Vector2 vector = new Vector2((float)TFUtils.LoadInt(data, "x"), (float)TFUtils.LoadInt(data, "y"));
		bool flag2 = (bool)data["flip"];
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		return new NewBuildingAction(id, text, did, types, flag, num, vector, flag2, cost);
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00035E9C File Offset: 0x0003409C
	private void Initialize(string blueprint, int did, EntityType types, bool built, ulong buildCompleteTime, Vector2 position, bool flip, Cost cost)
	{
		this.blueprint = blueprint;
		this.dId = did;
		this.extensions = types;
		this.built = built;
		this.buildCompleteTime = buildCompleteTime;
		this.position = position;
		this.flip = flip;
		this.cost = cost;
		TFUtils.Assert(cost != null, "Cannot create a NewBuildingAction with a null cost");
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00035EF8 File Offset: 0x000340F8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["x"] = this.position.x;
		dictionary["y"] = this.position.y;
		dictionary["flip"] = this.flip;
		dictionary["blueprint"] = this.blueprint;
		dictionary["did"] = this.dId;
		dictionary["extensions"] = (uint)this.extensions;
		dictionary["buildCompleteTime"] = this.buildCompleteTime;
		dictionary["activatedTime"] = null;
		dictionary["built"] = this.built;
		dictionary["cost"] = this.cost.ToDict();
		dictionary["product.ready"] = null;
		return dictionary;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00035FF0 File Offset: 0x000341F0
	public override void Apply(Game game, ulong utcNow)
	{
		Entity entity = game.entities.Create(this.extensions, this.dId, this.target, true);
		BuildingEntity decorator = entity.GetDecorator<BuildingEntity>();
		decorator.Slots = game.craftManager.GetInitialSlots(decorator.DefinitionId);
		decorator.GetDecorator<ErectableDecorator>().ErectionCompleteTime = new ulong?(this.buildCompleteTime);
		game.resourceManager.Apply(this.cost, game);
		Simulated simulated = Simulated.Building.Load(decorator, game.simulation, this.position, this.flip, utcNow);
		if ((this.extensions & EntityType.ANNEX) != EntityType.INVALID)
		{
			Simulated.Annex.Extend(simulated, game.simulation);
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x000360A0 File Offset: 0x000342A0
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["did"] = this.dId;
		dictionary["extensions"] = (uint)this.extensions;
		ActivatableDecorator.Serialize(ref dictionary, 0UL);
		dictionary["label"] = this.target.Describe();
		dictionary["x"] = this.position.x;
		dictionary["y"] = this.position.y;
		dictionary["flip"] = this.flip;
		dictionary["build_finish_time"] = this.buildCompleteTime;
		dictionary["rent_ready_time"] = null;
		list.Add(dictionary);
		ResourceManager.ApplyCostToGameState(this.cost, gameState);
		base.Confirm(gameState);
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x000361A8 File Offset: 0x000343A8
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		data["notification_time"] = TFUtils.EpochToDateTime(this.buildCompleteTime);
		data["notification_label"] = "build:" + this.target.Describe();
	}

	// Token: 0x04000604 RID: 1540
	public const string NEW_BUILDING = "nb";

	// Token: 0x04000605 RID: 1541
	public Vector2 position;

	// Token: 0x04000606 RID: 1542
	public bool flip;

	// Token: 0x04000607 RID: 1543
	public string blueprint;

	// Token: 0x04000608 RID: 1544
	public bool built;

	// Token: 0x04000609 RID: 1545
	public ulong buildCompleteTime;

	// Token: 0x0400060A RID: 1546
	public int dId;

	// Token: 0x0400060B RID: 1547
	public EntityType extensions;

	// Token: 0x0400060C RID: 1548
	public Cost cost;
}
