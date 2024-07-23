using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class CompleteBuildingAction : PersistedSimulatedAction
{
	// Token: 0x060007BE RID: 1982 RVA: 0x00032770 File Offset: 0x00030970
	private CompleteBuildingAction(Identity target, ulong completeTime, List<CompleteBuildingAction.ResidentInfo> residents, Reward reward) : this(target, completeTime, typeof(CompleteBuildingAction).ToString())
	{
		this.residents = residents;
		this.reward = reward;
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x000327A4 File Offset: 0x000309A4
	public CompleteBuildingAction(Simulated simulated, List<Simulated> residents, Reward reward) : this(simulated.Id, TFUtils.EpochTime(), typeof(CompleteBuildingAction).ToString())
	{
		if (simulated.HasEntity<PeriodicProductionDecorator>())
		{
			PeriodicProductionDecorator entity = simulated.GetEntity<PeriodicProductionDecorator>();
			this.productReady = this.completeTime + entity.RentProductionTime;
		}
		if (residents == null)
		{
			this.residents = null;
		}
		else
		{
			this.residents = new List<CompleteBuildingAction.ResidentInfo>();
			foreach (Simulated livingResident in residents)
			{
				this.residents.Add(new CompleteBuildingAction.ResidentInfo(livingResident));
			}
		}
		this.reward = reward;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x00032878 File Offset: 0x00030A78
	private CompleteBuildingAction(Identity target, ulong completeTime, string triggerType) : base("cb", target, triggerType)
	{
		this.completeTime = completeTime;
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00032890 File Offset: 0x00030A90
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x00032894 File Offset: 0x00030A94
	public new static CompleteBuildingAction FromDict(Dictionary<string, object> data)
	{
		Identity target = new Identity((string)data["target"]);
		ulong num = TFUtils.LoadUlong(data, "completeTime", 0UL);
		Dictionary<string, object> dictionary = TFUtils.TryLoadDict(data, "residents");
		List<CompleteBuildingAction.ResidentInfo> list = new List<CompleteBuildingAction.ResidentInfo>();
		if (dictionary != null)
		{
			foreach (object obj in dictionary.Values)
			{
				Dictionary<string, object> data2 = (Dictionary<string, object>)obj;
				list.Add(CompleteBuildingAction.ResidentInfo.FromDict(data2));
			}
		}
		if (data.ContainsKey("residentId"))
		{
			list.Add(new CompleteBuildingAction.ResidentInfo
			{
				did = TFUtils.LoadInt(data, "residentDid"),
				id = (string)data["residentId"],
				hungryAt = TFUtils.LoadUlong(data, "residentHungryTime", 0UL)
			});
		}
		Reward reward = (!data.ContainsKey("reward")) ? null : Reward.FromObject(data["reward"]);
		CompleteBuildingAction completeBuildingAction = new CompleteBuildingAction(target, num, list, reward);
		completeBuildingAction.DropTargetDataFromDict(data);
		return completeBuildingAction;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000329E0 File Offset: 0x00030BE0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["completeTime"] = this.completeTime;
		if (this.residents == null)
		{
			dictionary["residents"] = null;
		}
		else
		{
			Dictionary<string, Dictionary<string, object>> dictionary2 = new Dictionary<string, Dictionary<string, object>>();
			int num = 0;
			foreach (CompleteBuildingAction.ResidentInfo residentInfo in this.residents)
			{
				dictionary2.Add(num++.ToString(), residentInfo.ToDict());
			}
			dictionary["residents"] = dictionary2;
		}
		if (this.reward != null)
		{
			dictionary["reward"] = this.reward.ToDict();
		}
		base.DropTargetDataToDict(dictionary);
		return dictionary;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00032AD4 File Offset: 0x00030CD4
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ActivatableDecorator entity = simulated.GetEntity<ActivatableDecorator>();
		simulated.ClearPendingCommands();
		game.simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id, null);
		entity.Activated = utcNow;
		simulated.entity.PatchReferences(game);
		if (simulated.HasEntity<PeriodicProductionDecorator>())
		{
			PeriodicProductionDecorator entity2 = simulated.GetEntity<PeriodicProductionDecorator>();
			this.productReady = base.GetTime() + entity2.RentProductionTime;
			TFUtils.DebugLog(string.Concat(new object[]
			{
				"setting product.ready to ",
				this.productReady,
				". That is ",
				this.productReady - utcNow,
				" seconds from now."
			}));
			entity2.ProductReadyTime = this.productReady;
			if (this.productReady <= utcNow)
			{
				simulated.EnterInitialState(EntityManager.BuildingActions["produced"], game.simulation);
			}
			else
			{
				simulated.EnterInitialState(EntityManager.BuildingActions["producing"], game.simulation);
				simulated.AddPendingCommand(new Simulated.PendingCommand
				{
					c = CompleteCommand.Create(simulated.Id, simulated.Id),
					delay = new float?(this.productReady - utcNow)
				});
			}
		}
		else
		{
			simulated.EnterInitialState(EntityManager.BuildingActions["reflecting"], game.simulation);
		}
		foreach (CompleteBuildingAction.ResidentInfo residentInfo in this.residents)
		{
			ResidentEntity decorator = game.entities.Create(EntityType.RESIDENT, residentInfo.did, new Identity(residentInfo.id), true).GetDecorator<ResidentEntity>();
			if (decorator.Disabled)
			{
				this.residents.Remove(residentInfo);
			}
			else
			{
				ulong nextHungerTime = 0UL;
				Simulated.Resident.Load(decorator, this.target, null, null, null, nextHungerTime, null, null, game.simulation, utcNow);
			}
		}
		if (this.reward != null)
		{
			game.ApplyReward(this.reward, base.GetTime(), true);
		}
		base.AddPickup(game.simulation);
		simulated.FirstAnimate(game.simulation);
		simulated.RemoveScaffolding(game.simulation);
		simulated.RemoveFence(game.simulation);
		Vector3 b = new Vector3((simulated.Box.xmax + simulated.Box.xmin) * 0.5f, (simulated.Box.ymax + simulated.Box.ymin) * 0.5f, 0f);
		simulated.DisplayController.Position = simulated.DisplayOffsetWorld + b - simulated.TextureOriginWorld;
		base.Apply(game, utcNow);
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00032DFC File Offset: 0x00030FFC
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = this.target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		ActivatableDecorator.Serialize(ref dictionary, this.completeTime);
		if (dictionary.ContainsKey("build_finish_time"))
		{
			dictionary.Remove("build_finish_time");
		}
		dictionary["rent_ready_time"] = ((this.productReady != 0UL) ? this.productReady : null);
		foreach (CompleteBuildingAction.ResidentInfo residentInfo in this.residents)
		{
			Simulated.Building.AddResidentToGameState(gameState, residentInfo.id, residentInfo.did, this.target.Describe(), residentInfo.hungryAt);
		}
		if (this.reward != null)
		{
			RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
		}
		base.AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}

	// Token: 0x040005B8 RID: 1464
	public const string COMPLETE_BUILDING = "cb";

	// Token: 0x040005B9 RID: 1465
	public const int NO_HUNGER = -1;

	// Token: 0x040005BA RID: 1466
	public ulong completeTime;

	// Token: 0x040005BB RID: 1467
	private List<CompleteBuildingAction.ResidentInfo> residents;

	// Token: 0x040005BC RID: 1468
	public ulong productReady;

	// Token: 0x040005BD RID: 1469
	public Reward reward;

	// Token: 0x020000CE RID: 206
	private class ResidentInfo
	{
		// Token: 0x060007C6 RID: 1990 RVA: 0x00032F50 File Offset: 0x00031150
		public ResidentInfo()
		{
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00032F58 File Offset: 0x00031158
		public ResidentInfo(Simulated livingResident)
		{
			this.id = livingResident.Id.Describe();
			this.did = livingResident.entity.DefinitionId;
			this.hungryAt = livingResident.GetEntity<ResidentEntity>().HungryAt;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00032FA0 File Offset: 0x000311A0
		public static CompleteBuildingAction.ResidentInfo FromDict(Dictionary<string, object> data)
		{
			return new CompleteBuildingAction.ResidentInfo
			{
				id = TFUtils.LoadString(data, "id"),
				did = TFUtils.LoadInt(data, "did"),
				hungryAt = TFUtils.LoadUlong(data, "hungry", 0UL)
			};
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00032FEC File Offset: 0x000311EC
		public Dictionary<string, object> ToDict()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["id"] = this.id;
			dictionary["did"] = this.did;
			dictionary["hungry"] = this.hungryAt;
			return dictionary;
		}

		// Token: 0x040005BE RID: 1470
		private const string ID = "id";

		// Token: 0x040005BF RID: 1471
		private const string DID = "did";

		// Token: 0x040005C0 RID: 1472
		private const string HUNGRY_AT = "hungry";

		// Token: 0x040005C1 RID: 1473
		public string id;

		// Token: 0x040005C2 RID: 1474
		public int did;

		// Token: 0x040005C3 RID: 1475
		public ulong hungryAt;
	}
}
