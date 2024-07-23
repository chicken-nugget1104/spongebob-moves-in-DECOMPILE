using System;
using System.Collections.Generic;

// Token: 0x020000FC RID: 252
public class RushRestockAction : PersistedSimulatedAction
{
	// Token: 0x0600090D RID: 2317 RVA: 0x00039758 File Offset: 0x00037958
	public RushRestockAction(Identity id, Cost rushCost) : base("rrs", id, typeof(RushRestockAction).ToString())
	{
		this.rushCost = rushCost;
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x0600090E RID: 2318 RVA: 0x00039788 File Offset: 0x00037988
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0003978C File Offset: 0x0003798C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["rush_cost"] = this.rushCost.ToDict();
		return dictionary;
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x000397B8 File Offset: 0x000379B8
	public new static RushRestockAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["rush_cost"]);
		return new RushRestockAction(id, cost);
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x000397FC File Offset: 0x000379FC
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		VendingDecorator entity = simulated.GetEntity<VendingDecorator>();
		if (entity != null)
		{
			entity.RestockTime = base.GetTime();
		}
		game.resourceManager.Apply(this.rushCost, game);
		base.Apply(game, utcNow);
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00039860 File Offset: 0x00037A60
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(this.rushCost, gameState);
		string targetString = this.target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		dictionary["general_restock"] = base.GetTime();
		base.Confirm(gameState);
	}

	// Token: 0x0400065F RID: 1631
	public const string RUSH_RESTOCK = "rrs";

	// Token: 0x04000660 RID: 1632
	private Cost rushCost;
}
