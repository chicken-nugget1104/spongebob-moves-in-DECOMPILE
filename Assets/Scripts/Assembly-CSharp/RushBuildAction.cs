using System;
using System.Collections.Generic;

// Token: 0x020000F7 RID: 247
public class RushBuildAction : PersistedSimulatedAction
{
	// Token: 0x060008E4 RID: 2276 RVA: 0x00038BBC File Offset: 0x00036DBC
	public RushBuildAction(Identity id, Cost rushCost, ulong nextReadyTime) : base("rb", id, typeof(RushBuildAction).ToString())
	{
		this.rushCost = rushCost;
		this.readyTime = nextReadyTime;
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00038BE8 File Offset: 0x00036DE8
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00038BEC File Offset: 0x00036DEC
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["rush_cost"] = this.rushCost.ToDict();
		dictionary["ready_time"] = this.readyTime;
		return dictionary;
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00038C30 File Offset: 0x00036E30
	public new static RushBuildAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["rush_cost"]);
		ulong nextReadyTime = TFUtils.LoadUlong(data, "ready_time", 0UL);
		return new RushBuildAction(id, cost, nextReadyTime);
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00038C84 File Offset: 0x00036E84
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.ClearPendingCommands();
		simulated.timebarMixinArgs.hasTimebar = false;
		simulated.RemoveScaffolding(game.simulation);
		simulated.RemoveFence(game.simulation);
		ErectableDecorator entity = simulated.GetEntity<ErectableDecorator>();
		entity.ErectionCompleteTime = new ulong?(this.readyTime);
		Identity identity = (!simulated.Variable.ContainsKey("employee")) ? null : (simulated.Variable["employee"] as Identity);
		if (identity != null)
		{
			Simulated simulated2 = game.simulation.FindSimulated(identity);
			if (simulated2 != null)
			{
				simulated2.ClearPendingCommands();
			}
			game.simulation.Router.Send(ReturnCommand.Create(simulated.Id, identity));
		}
		simulated.EnterInitialState(EntityManager.BuildingActions["inactive"], game.simulation);
		simulated.FirstAnimate(game.simulation);
		game.resourceManager.Apply(this.rushCost, game);
		base.Apply(game, utcNow);
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00038DA4 File Offset: 0x00036FA4
	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (this.readyTime == 18446744073709551615UL)
		{
			this.readyTime = time;
		}
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00038DC4 File Offset: 0x00036FC4
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(this.rushCost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = this.target.Describe();
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString));
		dictionary["build_finish_time"] = this.readyTime;
		base.Confirm(gameState);
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00038E4C File Offset: 0x0003704C
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}

	// Token: 0x04000649 RID: 1609
	public const string RUSH_BUILD = "rb";

	// Token: 0x0400064A RID: 1610
	public const ulong INVALID_ULONG = 18446744073709551615UL;

	// Token: 0x0400064B RID: 1611
	private Cost rushCost;

	// Token: 0x0400064C RID: 1612
	private ulong readyTime;
}
