using System;
using System.Collections.Generic;

// Token: 0x020000F9 RID: 249
public class RushDebrisAction : PersistedSimulatedAction
{
	// Token: 0x060008F4 RID: 2292 RVA: 0x00039170 File Offset: 0x00037370
	public RushDebrisAction(Identity id, Cost rushCost, ulong readyTime) : base("rd", id, typeof(RushDebrisAction).ToString())
	{
		this.rushCost = rushCost;
		this.readyTime = readyTime;
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0003919C File Offset: 0x0003739C
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x000391A0 File Offset: 0x000373A0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["clear_rush_cost"] = this.rushCost.ToDict();
		dictionary["ready_time"] = this.readyTime;
		return dictionary;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x000391E4 File Offset: 0x000373E4
	public new static RushDebrisAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["clear_rush_cost"]);
		ulong num = TFUtils.LoadUlong(data, "ready_time", 0UL);
		return new RushDebrisAction(id, cost, num);
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x00039238 File Offset: 0x00037438
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
		simulated.ClearPendingCommands();
		simulated.timebarMixinArgs.hasTimebar = false;
		entity.ClearCompleteTime = new ulong?(this.readyTime);
		simulated.LoadInitialState(EntityManager.DebrisActions["deleting"]);
		game.resourceManager.Apply(this.rushCost, game);
		base.Apply(game, utcNow);
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x000392C0 File Offset: 0x000374C0
	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (this.readyTime == 18446744073709551615UL)
		{
			this.readyTime = time;
		}
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x000392E0 File Offset: 0x000374E0
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(this.rushCost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["debris"];
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object d) => ((string)((Dictionary<string, object>)d)["label"]).Equals(this.target.Describe()));
		dictionary["clear_complete_time"] = this.readyTime;
		base.Confirm(gameState);
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x00039350 File Offset: 0x00037550
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}

	// Token: 0x04000653 RID: 1619
	public const string RUSH_DEBRIS = "rd";

	// Token: 0x04000654 RID: 1620
	public const ulong INVALID_ULONG = 18446744073709551615UL;

	// Token: 0x04000655 RID: 1621
	private Cost rushCost;

	// Token: 0x04000656 RID: 1622
	private ulong readyTime;
}
