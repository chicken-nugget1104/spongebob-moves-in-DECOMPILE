using System;
using System.Collections.Generic;

// Token: 0x02000105 RID: 261
public class TreasureCollectAction : PersistedSimulatedAction
{
	// Token: 0x0600094A RID: 2378 RVA: 0x0003A70C File Offset: 0x0003890C
	public TreasureCollectAction(Identity id, Reward reward, string persistName, ulong? timeToTreasure) : base("tc", id, typeof(TreasureSpawnAction).ToString())
	{
		this.reward = reward;
		this.persistName = persistName;
		this.nextTreasureTime = timeToTreasure;
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x0600094B RID: 2379 RVA: 0x0003A740 File Offset: 0x00038940
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0003A744 File Offset: 0x00038944
	public new static TreasureCollectAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromObject(data["reward"]);
		string text = "time_to_spawn";
		ulong? timeToTreasure = null;
		if (data.ContainsKey("persist_name"))
		{
			text = TFUtils.LoadString(data, "persist_name");
			timeToTreasure = TFUtils.TryLoadUlong(data, "next_treasure_time", 0UL);
		}
		TreasureCollectAction treasureCollectAction = new TreasureCollectAction(id, reward, text, timeToTreasure);
		treasureCollectAction.DropTargetDataFromDict(data);
		return treasureCollectAction;
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0003A7C8 File Offset: 0x000389C8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = this.reward.ToDict();
		dictionary["persist_name"] = this.persistName;
		dictionary["next_treasure_time"] = this.nextTreasureTime;
		base.DropTargetDataToDict(dictionary);
		return dictionary;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0003A824 File Offset: 0x00038A24
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		TreasureSpawner treasureTiming = simulated.GetEntity<TreasureEntity>().TreasureTiming;
		treasureTiming.MarkCollected();
		simulated.SetFootprint(game.simulation, false);
		game.simulation.RemoveSimulated(simulated);
		game.entities.Destroy(this.target);
		game.ApplyReward(this.reward, base.GetTime(), true);
		base.AddPickup(game.simulation);
		simulated.ClearPendingCommands();
		base.Apply(game, utcNow);
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0003A8BC File Offset: 0x00038ABC
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (dictionary.ContainsKey("treasure_state"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["treasure_state"];
			dictionary2[this.persistName] = this.nextTreasureTime;
		}
		List<object> orCreateList = TFUtils.GetOrCreateList<object>(dictionary, "treasure");
		string targetString = this.target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		object obj = orCreateList.Find(match);
		TFUtils.Assert(obj != null, "Trying to Collect a missing treasure!");
		orCreateList.Remove(obj);
		RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
		base.AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0003A98C File Offset: 0x00038B8C
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}

	// Token: 0x04000679 RID: 1657
	public const string TREASURE_COLLECT = "tc";

	// Token: 0x0400067A RID: 1658
	private Reward reward;

	// Token: 0x0400067B RID: 1659
	private ulong? nextTreasureTime;

	// Token: 0x0400067C RID: 1660
	private string persistName;
}
