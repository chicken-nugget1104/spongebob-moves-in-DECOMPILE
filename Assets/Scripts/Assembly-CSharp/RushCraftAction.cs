using System;
using System.Collections.Generic;

// Token: 0x020000F8 RID: 248
public class RushCraftAction : PersistedSimulatedAction
{
	// Token: 0x060008EC RID: 2284 RVA: 0x00038E58 File Offset: 0x00037058
	public RushCraftAction(Identity id, int slotId, Cost rushCost, ulong newReadyTime, Reward craftReward) : base("rc", id, typeof(RushCraftAction).ToString())
	{
		this.rushCost = rushCost;
		this.craftReadyTime = newReadyTime;
		this.craftReward = craftReward;
		this.slotId = slotId;
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x060008ED RID: 2285 RVA: 0x00038E94 File Offset: 0x00037094
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x00038E98 File Offset: 0x00037098
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["cost"] = this.rushCost.ToDict();
		dictionary["ready_time"] = this.craftReadyTime;
		dictionary["reward"] = this.craftReward.ToDict();
		dictionary["slot_id"] = this.slotId;
		return dictionary;
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00038F08 File Offset: 0x00037108
	public new static RushCraftAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "slot_id");
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		ulong newReadyTime = TFUtils.LoadUlong(data, "ready_time", 0UL);
		Reward reward = Reward.FromDict((Dictionary<string, object>)data["reward"]);
		return new RushCraftAction(id, num, cost, newReadyTime, reward);
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00038F84 File Offset: 0x00037184
	public override void Apply(Game game, ulong utcNow)
	{
		game.craftManager.GetCraftingInstance(this.target, this.slotId).ReadyTimeUtc = utcNow;
		game.simulation.Router.CancelMatching(Command.TYPE.CRAFTED, this.target, this.target, new Dictionary<string, object>
		{
			{
				"slot_id",
				this.slotId
			}
		});
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		if (utcNow > this.craftReadyTime)
		{
			game.simulation.Router.Send(CraftedCommand.Create(this.target, this.target, this.slotId), utcNow - this.craftReadyTime);
			if (simulated.GetEntity<BuildingEntity>().CraftRewards == null)
			{
				simulated.LoadInitialState(EntityManager.BuildingActions["crafting"]);
			}
			else
			{
				simulated.LoadInitialState(EntityManager.BuildingActions["craftcycling"]);
			}
		}
		else
		{
			simulated.LoadInitialState(EntityManager.BuildingActions["craftcycling"]);
		}
		game.resourceManager.Apply(this.rushCost, game);
		base.Apply(game, utcNow);
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x000390BC File Offset: 0x000372BC
	public override void AddEnvelope(ulong time, string tag)
	{
		base.AddEnvelope(time, tag);
		if (this.craftReadyTime == 18446744073709551615UL)
		{
			this.craftReadyTime = time;
		}
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x000390DC File Offset: 0x000372DC
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(this.rushCost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["crafts"];
		string targetString = this.target.Describe();
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find((object c) => ((string)((Dictionary<string, object>)c)["building_label"]).Equals(targetString));
		dictionary["ready_time"] = this.craftReadyTime;
		base.Confirm(gameState);
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x00039164 File Offset: 0x00037364
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}

	// Token: 0x0400064D RID: 1613
	public const string RUSH_CRAFT = "rc";

	// Token: 0x0400064E RID: 1614
	public const ulong INVALID_ULONG = 18446744073709551615UL;

	// Token: 0x0400064F RID: 1615
	private Cost rushCost;

	// Token: 0x04000650 RID: 1616
	private ulong craftReadyTime;

	// Token: 0x04000651 RID: 1617
	private Reward craftReward;

	// Token: 0x04000652 RID: 1618
	private int slotId;
}
