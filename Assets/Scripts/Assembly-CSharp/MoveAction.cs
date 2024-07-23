using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class MoveAction : PersistedSimulatedAction
{
	// Token: 0x0600084E RID: 2126 RVA: 0x00035280 File Offset: 0x00033480
	public MoveAction(Simulated simulated, List<Simulated> residents) : this(simulated.Id, new int?((int)simulated.Position.x), new int?((int)simulated.Position.y), new bool?(simulated.Flip), residents)
	{
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x000352D0 File Offset: 0x000334D0
	public MoveAction(Identity id, int? x, int? y, bool? flip, List<Simulated> residents) : base("m", id, typeof(MoveAction).ToString())
	{
		this.x = x;
		this.y = y;
		this.flip = flip;
		if (residents != null)
		{
			this.InitializeResidents(residents);
		}
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00035320 File Offset: 0x00033520
	private MoveAction(Identity id, int? x, int? y, bool? flip, List<MoveAction.ResidentInfo> residentInfos) : this(id, x, y, flip, null)
	{
		this.residentInfos = residentInfos;
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000851 RID: 2129 RVA: 0x00035338 File Offset: 0x00033538
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0003533C File Offset: 0x0003353C
	public new static MoveAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int? num = TFUtils.LoadNullableInt(data, "x");
		int? num2 = TFUtils.LoadNullableInt(data, "y");
		bool? flag = TFUtils.LoadNullableBool(data, "flip");
		List<object> list = (List<object>)data["residents"];
		List<MoveAction.ResidentInfo> list2 = null;
		if (list != null)
		{
			list2 = new List<MoveAction.ResidentInfo>();
			int num3 = 0;
			foreach (object obj in list)
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
				string id2 = (string)dictionary["id"];
				int did = TFUtils.LoadInt(dictionary, "did");
				int? hungerId = TFUtils.TryLoadInt(dictionary, "wish_product_id");
				int? prevHungerId = TFUtils.TryLoadInt(dictionary, "prev_wish_product_id");
				ulong? wishExpiresAt = TFUtils.TryLoadUlong(dictionary, "wish_expires_at", 0UL);
				ulong hungryAt = TFUtils.LoadUlong(dictionary, "hungry_at", 0UL);
				ulong? fullnessLength = TFUtils.TryLoadUlong(dictionary, "fullness_length", 0UL);
				list2.Add(new MoveAction.ResidentInfo(id2, did, hungerId, prevHungerId, wishExpiresAt, hungryAt, fullnessLength));
				num3++;
			}
		}
		return new MoveAction(id, num, num2, flag, list2);
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x0003549C File Offset: 0x0003369C
	private void InitializeResidents(List<Simulated> residents)
	{
		this.residentInfos = new List<MoveAction.ResidentInfo>();
		int num = 0;
		foreach (Simulated simulated in residents)
		{
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			this.residentInfos.Add(new MoveAction.ResidentInfo(simulated.Id.Describe(), simulated.entity.DefinitionId, entity.HungerResourceId, entity.PreviousResourceId, entity.WishExpiresAt, entity.HungryAt, new ulong?(entity.FullnessLength)));
			num++;
		}
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x00035558 File Offset: 0x00033758
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["x"] = this.x;
		dictionary["y"] = this.y;
		dictionary["flip"] = this.flip;
		List<object> list = null;
		if (this.residentInfos != null)
		{
			list = new List<object>();
			foreach (MoveAction.ResidentInfo residentInfo in this.residentInfos)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2["id"] = residentInfo.id;
				dictionary2["did"] = residentInfo.did;
				dictionary2["wish_product_id"] = residentInfo.hungerId;
				dictionary2["wish_expires_at"] = residentInfo.wishExpiresAt;
				dictionary2["hungry_at"] = residentInfo.hungryAt;
				list.Add(dictionary2);
			}
		}
		dictionary["residents"] = list;
		return dictionary;
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x000356A4 File Offset: 0x000338A4
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated != null)
		{
			int? num = this.x;
			if (num != null)
			{
				int? num2 = this.y;
				if (num2 != null)
				{
					Simulated simulated2 = simulated;
					int? num3 = this.x;
					float num4 = (float)num3.Value;
					int? num5 = this.y;
					simulated2.Warp(new Vector2(num4, (float)num5.Value), game.simulation);
					simulated.Flip = (this.flip != null && this.flip.Value);
					simulated.simFlags |= Simulated.SimulatedFlags.FIRST_ANIMATE;
					Simulated.Building.AdjustWorkerPosition(simulated, game.simulation);
					goto IL_184;
				}
			}
			BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
			List<KeyValuePair<int, Identity>> list = new List<KeyValuePair<int, Identity>>();
			if (this.residentInfos != null)
			{
				foreach (MoveAction.ResidentInfo residentInfo in this.residentInfos)
				{
					Identity identity = new Identity(residentInfo.id);
					list.Add(new KeyValuePair<int, Identity>(residentInfo.did, identity));
					Simulated simulated3 = game.simulation.FindSimulated(identity);
					if (simulated3 != null)
					{
						game.simulation.RemoveSimulated(simulated3);
						SwarmManager.Instance.RemoveResident(simulated3.GetEntity<ResidentEntity>(), simulated);
					}
				}
			}
			game.inventory.AddItem(entity, list);
			simulated.SetFootprint(game.simulation, false);
			game.simulation.RemoveSimulated(simulated);
			IL_184:;
		}
		else
		{
			int? num6 = this.x;
			if (num6 != null)
			{
				int? num7 = this.y;
				if (num7 != null)
				{
					List<KeyValuePair<int, Identity>> list2;
					Entity entity2 = game.inventory.RemoveEntity(this.target, out list2);
					if (entity2 != null)
					{
						BuildingEntity decorator = entity2.GetDecorator<BuildingEntity>();
						if (decorator.HasDecorator<PeriodicProductionDecorator>())
						{
							PeriodicProductionDecorator decorator2 = decorator.GetDecorator<PeriodicProductionDecorator>();
							decorator2.ProductReadyTime = base.GetTime() + decorator2.RentProductionTime;
						}
						BuildingEntity buildingEntity = decorator;
						Simulation simulation = game.simulation;
						int? num8 = this.x;
						float num9 = (float)num8.Value;
						int? num10 = this.y;
						Simulated.Building.Load(buildingEntity, simulation, new Vector2(num9, (float)num10.Value), this.flip != null && this.flip.Value, utcNow);
						if (this.residentInfos != null && this.residentInfos.Count > 0)
						{
							foreach (MoveAction.ResidentInfo item in this.residentInfos)
							{
								ResidentEntity decorator3 = game.entities.GetEntity(new Identity(item.id)).GetDecorator<ResidentEntity>();
								if (decorator3.Disabled)
								{
									this.residentInfos.Remove(item);
								}
								else
								{
									Simulated.Resident.Load(decorator3, this.target, item.wishExpiresAt, item.hungerId, item.prevHungerId, item.hungryAt, null, null, game.simulation, utcNow);
								}
							}
						}
					}
				}
			}
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00035A10 File Offset: 0x00033C10
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = this.target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		if (dictionary == null)
		{
			base.Confirm(gameState);
			return;
		}
		int? num = TFUtils.LoadNullableInt(dictionary, "x");
		int? num2 = TFUtils.LoadNullableInt(dictionary, "y");
		dictionary["x"] = this.x;
		dictionary["y"] = this.y;
		dictionary["flip"] = this.flip;
		int did = TFUtils.LoadInt(dictionary, "did");
		if (this.residentInfos != null)
		{
			if (num == null && num2 == null && this.x != null && this.y != null)
			{
				Blueprint blueprint = EntityManager.GetBlueprint("building", did, false);
				if (blueprint.Invariable["time.production"] != null)
				{
					dictionary["rent_ready_time"] = base.GetTime() + (ulong)blueprint.Invariable["time.production"];
				}
				foreach (MoveAction.ResidentInfo residentInfo in this.residentInfos)
				{
					Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, new Identity(residentInfo.id));
					if (unitGameState == null)
					{
						Simulated.Building.AddResidentToGameState(gameState, residentInfo.id, residentInfo.did, this.target.Describe(), residentInfo.hungryAt);
					}
					else
					{
						ResidentEntity.UpdateHungerTimeInUnitState(unitGameState, residentInfo.hungryAt);
						ResidentEntity.SetActiveStatusInUnitState(unitGameState, true);
					}
				}
			}
			else if (num != null && num2 != null && this.x == null && this.y == null)
			{
				foreach (MoveAction.ResidentInfo residentInfo2 in this.residentInfos)
				{
					Dictionary<string, object> unitGameState2 = ResidentEntity.GetUnitGameState(gameState, new Identity(residentInfo2.id));
					if (unitGameState2 != null)
					{
						ResidentEntity.SetActiveStatusInUnitState(unitGameState2, false);
					}
				}
			}
		}
		base.Confirm(gameState);
	}

	// Token: 0x040005F8 RID: 1528
	public const string MOVE = "m";

	// Token: 0x040005F9 RID: 1529
	public int? x;

	// Token: 0x040005FA RID: 1530
	public int? y;

	// Token: 0x040005FB RID: 1531
	public bool? flip;

	// Token: 0x040005FC RID: 1532
	private List<MoveAction.ResidentInfo> residentInfos;

	// Token: 0x020000E2 RID: 226
	private struct ResidentInfo
	{
		// Token: 0x06000857 RID: 2135 RVA: 0x00035CE8 File Offset: 0x00033EE8
		public ResidentInfo(string id, int did, int? hungerId, int? prevHungerId, ulong? wishExpiresAt, ulong hungryAt, ulong? fullnessLength)
		{
			this.id = id;
			this.did = did;
			this.hungerId = hungerId;
			this.prevHungerId = prevHungerId;
			this.wishExpiresAt = wishExpiresAt;
			this.hungryAt = hungryAt;
			this.fullnessLength = fullnessLength;
		}

		// Token: 0x040005FD RID: 1533
		public string id;

		// Token: 0x040005FE RID: 1534
		public int did;

		// Token: 0x040005FF RID: 1535
		public int? hungerId;

		// Token: 0x04000600 RID: 1536
		public int? prevHungerId;

		// Token: 0x04000601 RID: 1537
		public ulong? wishExpiresAt;

		// Token: 0x04000602 RID: 1538
		public ulong hungryAt;

		// Token: 0x04000603 RID: 1539
		public ulong? fullnessLength;
	}
}
