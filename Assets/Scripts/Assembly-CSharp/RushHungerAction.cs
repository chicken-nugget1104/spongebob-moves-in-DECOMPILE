using System;
using System.Collections.Generic;

// Token: 0x020000FA RID: 250
public class RushHungerAction : PersistedSimulatedAction
{
	// Token: 0x060008FD RID: 2301 RVA: 0x00039384 File Offset: 0x00037584
	public RushHungerAction(Identity id, Cost rushCost, ulong nextReadyTime) : base("rh", id, typeof(RushHungerAction).ToString())
	{
		this.rushCost = rushCost;
		this.readyTime = nextReadyTime;
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060008FE RID: 2302 RVA: 0x000393B0 File Offset: 0x000375B0
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x000393B4 File Offset: 0x000375B4
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["rush_cost"] = this.rushCost.ToDict();
		dictionary["ready_time"] = this.readyTime;
		return dictionary;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x000393F8 File Offset: 0x000375F8
	public new static RushHungerAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["rush_cost"]);
		ulong nextReadyTime = TFUtils.LoadUlong(data, "ready_time", 0UL);
		return new RushHungerAction(id, cost, nextReadyTime);
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0003944C File Offset: 0x0003764C
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
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.HungryAt = this.readyTime;
		game.resourceManager.Apply(this.rushCost, game);
		base.Apply(game, utcNow);
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x000394BC File Offset: 0x000376BC
	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (this.readyTime == 18446744073709551615UL)
		{
			this.readyTime = time;
		}
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x000394DC File Offset: 0x000376DC
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(this.rushCost, gameState);
		ResidentEntity.UpdateHungerTimeInGameState(gameState, this.target, base.GetTime());
		base.Confirm(gameState);
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x00039504 File Offset: 0x00037704
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}

	// Token: 0x04000657 RID: 1623
	public const string RUSH_HUNGER = "rh";

	// Token: 0x04000658 RID: 1624
	public const ulong INVALID_ULONG = 18446744073709551615UL;

	// Token: 0x04000659 RID: 1625
	private Cost rushCost;

	// Token: 0x0400065A RID: 1626
	private ulong readyTime;
}
