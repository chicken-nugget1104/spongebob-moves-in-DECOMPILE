using System;
using System.Collections.Generic;

// Token: 0x020000CC RID: 204
public class CollectRentAction : PersistedSimulatedAction
{
	// Token: 0x060007B4 RID: 1972 RVA: 0x00032424 File Offset: 0x00030624
	public CollectRentAction(Simulated building, Reward reward) : base("cr", building.Id, "RentPickup")
	{
		PeriodicProductionDecorator entity = building.GetEntity<PeriodicProductionDecorator>();
		this.reward = reward;
		this.rentPeriod = entity.RentProductionTime;
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00032474 File Offset: 0x00030674
	public CollectRentAction(Simulated building, Reward reward, ulong rentReadyTime) : base("cr", building.Id, "RentPickup")
	{
		PeriodicProductionDecorator entity = building.GetEntity<PeriodicProductionDecorator>();
		this.reward = reward;
		this.rentPeriod = entity.RentProductionTime;
		this.rentReadyTime = rentReadyTime;
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x000324C8 File Offset: 0x000306C8
	private CollectRentAction(Identity id, Reward reward, ulong rentReadyTime) : base("cr", id, "RentPickup")
	{
		this.reward = reward;
		this.rentReadyTime = rentReadyTime;
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060007B7 RID: 1975 RVA: 0x000324FC File Offset: 0x000306FC
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00032500 File Offset: 0x00030700
	public new static CollectRentAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromObject(data["reward"]);
		ulong num = TFUtils.LoadUlong(data, "rentReadyTime", 0UL);
		CollectRentAction collectRentAction = new CollectRentAction(id, reward, num);
		collectRentAction.DropTargetDataFromDict(data);
		return collectRentAction;
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00032554 File Offset: 0x00030754
	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (this.rentReadyTime == 18446744073709551615UL && this.rentPeriod != 18446744073709551615UL)
		{
			this.rentReadyTime = time + this.rentPeriod;
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00032594 File Offset: 0x00030794
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = this.reward.ToDict();
		dictionary["rentReadyTime"] = this.rentReadyTime;
		base.DropTargetDataToDict(dictionary);
		return dictionary;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x000325DC File Offset: 0x000307DC
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
		PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
		if (entity == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ulong num = base.GetTime() + entity.RentProductionTime;
		entity.ProductReadyTime = num;
		if (num <= utcNow)
		{
			simulated.EnterInitialState(EntityManager.BuildingActions["produced"], game.simulation);
		}
		else
		{
			simulated.EnterInitialState(EntityManager.BuildingActions["producing"], game.simulation);
		}
		game.ApplyReward(this.reward, base.GetTime(), true);
		base.AddPickup(game.simulation);
		base.Apply(game, utcNow);
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x000326C4 File Offset: 0x000308C4
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = this.target.Describe();
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString));
		dictionary["rent_ready_time"] = this.rentReadyTime;
		RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
		base.AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00032758 File Offset: 0x00030958
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		this.reward.AddDataToTrigger(ref data);
	}

	// Token: 0x040005B2 RID: 1458
	public const string COLLECT_RENT = "cr";

	// Token: 0x040005B3 RID: 1459
	public const ulong INVALID_ULONG = 18446744073709551615UL;

	// Token: 0x040005B4 RID: 1460
	public const string PICKUP_TRIGGERTYPE = "RentPickup";

	// Token: 0x040005B5 RID: 1461
	public Reward reward;

	// Token: 0x040005B6 RID: 1462
	public ulong rentReadyTime = ulong.MaxValue;

	// Token: 0x040005B7 RID: 1463
	public ulong rentPeriod = ulong.MaxValue;
}
