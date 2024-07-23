using System;
using System.Collections.Generic;

// Token: 0x020000FB RID: 251
public class RushRentAction : PersistedSimulatedAction
{
	// Token: 0x06000905 RID: 2309 RVA: 0x00039510 File Offset: 0x00037710
	public RushRentAction(Identity id, Cost rushCost, ulong nextRentReadyTime) : base("rr", id, typeof(RushRentAction).ToString())
	{
		this.rushCost = rushCost;
		this.rentReadyTime = nextRentReadyTime;
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000906 RID: 2310 RVA: 0x0003953C File Offset: 0x0003773C
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00039540 File Offset: 0x00037740
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["cost"] = this.rushCost.ToDict();
		dictionary["rent_ready_time"] = this.rentReadyTime;
		return dictionary;
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x00039584 File Offset: 0x00037784
	public new static RushRentAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		ulong nextRentReadyTime = TFUtils.LoadUlong(data, "rent_ready_time", 0UL);
		return new RushRentAction(id, cost, nextRentReadyTime);
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x000395D8 File Offset: 0x000377D8
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.ClearPendingCommands();
		game.simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id, null);
		BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
		if (entity.HasDecorator<PeriodicProductionDecorator>())
		{
			entity.GetDecorator<PeriodicProductionDecorator>().ProductReadyTime = this.rentReadyTime;
			simulated.EnterInitialState(EntityManager.BuildingActions["produced"], game.simulation);
		}
		else
		{
			simulated.EnterInitialState(EntityManager.BuildingActions["active"], game.simulation);
		}
		game.resourceManager.Apply(this.rushCost, game);
		base.Apply(game, utcNow);
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x000396A4 File Offset: 0x000378A4
	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (this.rentReadyTime == 18446744073709551615UL)
		{
			this.rentReadyTime = time;
		}
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x000396C4 File Offset: 0x000378C4
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(this.rushCost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = this.target.Describe();
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString));
		dictionary["rent_ready_time"] = this.rentReadyTime;
		base.Confirm(gameState);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003974C File Offset: 0x0003794C
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}

	// Token: 0x0400065B RID: 1627
	public const string RUSH_RENT = "rr";

	// Token: 0x0400065C RID: 1628
	public const ulong INVALID_ULONG = 18446744073709551615UL;

	// Token: 0x0400065D RID: 1629
	private Cost rushCost;

	// Token: 0x0400065E RID: 1630
	private ulong rentReadyTime;
}
