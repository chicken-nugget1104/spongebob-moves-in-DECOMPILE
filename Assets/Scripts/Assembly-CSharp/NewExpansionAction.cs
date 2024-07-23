using System;
using System.Collections.Generic;

// Token: 0x020000E4 RID: 228
public class NewExpansionAction : PersistedTriggerableAction
{
	// Token: 0x06000861 RID: 2145 RVA: 0x000361FC File Offset: 0x000343FC
	public NewExpansionAction(int id, Cost cost, List<TerrainSlotObject> debris, List<TerrainSlotObject> landmarks) : base("ne", Identity.Null())
	{
		this.did = id;
		this.cost = cost;
		this.debris = debris;
		this.landmarks = landmarks;
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000862 RID: 2146 RVA: 0x0003622C File Offset: 0x0003442C
	public TriggerableMixin Triggerable
	{
		get
		{
			return this.triggerable;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000863 RID: 2147 RVA: 0x00036234 File Offset: 0x00034434
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00036238 File Offset: 0x00034438
	public new static NewExpansionAction FromDict(Dictionary<string, object> data)
	{
		int id = TFUtils.LoadInt(data, "did");
		List<TerrainSlotObject> list = TerrainSlot.LoadExpansionObjectData((List<object>)data["debris"]);
		List<TerrainSlotObject> list2 = TerrainSlot.LoadExpansionObjectData((List<object>)data["landmarks"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		return new NewExpansionAction(id, cost, list, list2);
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x000362A0 File Offset: 0x000344A0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = this.did;
		dictionary["cost"] = this.cost.ToDict();
		dictionary["debris"] = TerrainSlot.SerializeExpansionObjectData(this.debris);
		dictionary["landmarks"] = TerrainSlot.SerializeExpansionObjectData(this.landmarks);
		return dictionary;
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00036310 File Offset: 0x00034510
	public override void Apply(Game game, ulong utcNow)
	{
		game.resourceManager.Apply(this.cost, game);
		foreach (TerrainSlotObject terrainSlotObject in this.landmarks)
		{
			Simulated simulated = game.simulation.FindSimulated(terrainSlotObject.id);
			if (simulated != null)
			{
				game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), simulated.Id));
			}
			else
			{
				simulated = game.simulation.CreateSimulated(EntityType.LANDMARK, terrainSlotObject.did, terrainSlotObject.position.ToVector2());
				simulated.Warp(simulated.Position, game.simulation);
				simulated.Visible = true;
			}
		}
		foreach (TerrainSlotObject terrainSlotObject2 in this.debris)
		{
			Simulated simulated2 = game.simulation.FindSimulated(terrainSlotObject2.id);
			if (simulated2 != null)
			{
				game.simulation.Router.Send(PurchaseCommand.Create(simulated2.Id, simulated2.Id));
			}
			else
			{
				simulated2 = game.simulation.CreateSimulated(EntityType.DEBRIS, terrainSlotObject2.did, terrainSlotObject2.position.ToVector2());
				simulated2.Warp(simulated2.Position, game.simulation);
				simulated2.Visible = true;
			}
		}
		game.terrain.AddExpansionSlot(this.did);
		if (game.featureManager.CheckFeature("purchase_expansions"))
		{
			game.terrain.UpdateRealtySigns(game.entities.DisplayControllerManager, new BillboardDelegate(SBCamera.BillboardDefinition), game);
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0003651C File Offset: 0x0003471C
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["expansions"];
		list.Add(this.did);
		List<object> list2 = (List<object>)((Dictionary<string, object>)gameState["farm"])["landmarks"];
		foreach (TerrainSlotObject terrainSlotObject in this.landmarks)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = terrainSlotObject.did;
			dictionary["label"] = terrainSlotObject.id.Describe();
			dictionary["x"] = terrainSlotObject.position.X;
			dictionary["y"] = terrainSlotObject.position.Y;
			list2.Add(dictionary);
		}
		List<object> list3 = (List<object>)((Dictionary<string, object>)gameState["farm"])["debris"];
		foreach (TerrainSlotObject terrainSlotObject2 in this.debris)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2["did"] = terrainSlotObject2.did;
			dictionary2["label"] = terrainSlotObject2.id.Describe();
			dictionary2["x"] = terrainSlotObject2.position.X;
			dictionary2["y"] = terrainSlotObject2.position.Y;
			list3.Add(dictionary2);
		}
		ResourceManager.ApplyCostToGameState(this.cost, gameState);
		base.Confirm(gameState);
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00036744 File Offset: 0x00034944
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data.Add("expansion_id", this.did);
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00036760 File Offset: 0x00034960
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x0400060D RID: 1549
	public const string NEW_EXPANSION = "ne";

	// Token: 0x0400060E RID: 1550
	public int did;

	// Token: 0x0400060F RID: 1551
	public Cost cost;

	// Token: 0x04000610 RID: 1552
	public List<TerrainSlotObject> debris;

	// Token: 0x04000611 RID: 1553
	public List<TerrainSlotObject> landmarks;
}
