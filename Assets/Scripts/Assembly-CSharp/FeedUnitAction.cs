using System;
using System.Collections.Generic;

// Token: 0x020000D8 RID: 216
public class FeedUnitAction : PersistedSimulatedAction
{
	// Token: 0x0600080D RID: 2061 RVA: 0x000343B8 File Offset: 0x000325B8
	public FeedUnitAction(Simulated unit, ulong hungerPeriod, int hungerResourceId, int? prevHungerResourceId, Reward reward) : this(unit.Id, hungerPeriod, hungerResourceId, prevHungerResourceId, reward)
	{
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x000343D8 File Offset: 0x000325D8
	private FeedUnitAction(Identity id, ulong hungerPeriod, int hungerResourceId, int? prevHungerResourceId, Reward reward) : base("fu", id, typeof(FeedUnitAction).ToString())
	{
		this.hungerPeriod = hungerPeriod;
		this.hungerResourceId = hungerResourceId;
		this.reward = reward;
		this.prevHungerResourceId = prevHungerResourceId;
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x0600080F RID: 2063 RVA: 0x00034414 File Offset: 0x00032614
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00034418 File Offset: 0x00032618
	public new static FeedUnitAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		ulong num = TFUtils.LoadUlong(data, "hungerPeriod", 0UL);
		int num2 = TFUtils.LoadInt(data, "hungerResourceId");
		int? num3 = TFUtils.TryLoadInt(data, "prevHungerResourceId");
		Reward reward = null;
		if (data.ContainsKey("reward"))
		{
			reward = Reward.FromObject(data["reward"]);
		}
		FeedUnitAction feedUnitAction = new FeedUnitAction(id, num, num2, num3, reward);
		feedUnitAction.DropTargetDataFromDict(data);
		return feedUnitAction;
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x000344A0 File Offset: 0x000326A0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["hungerResourceId"] = this.hungerResourceId;
		dictionary["hungerPeriod"] = this.hungerPeriod;
		if (this.reward != null)
		{
			dictionary["reward"] = this.reward.ToDict();
		}
		if (this.prevHungerResourceId != null)
		{
			dictionary["prevHungerResourceId"] = this.prevHungerResourceId;
		}
		base.DropTargetDataToDict(dictionary);
		return dictionary;
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00034530 File Offset: 0x00032730
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.ClearPendingCommands();
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.HungerResourceId = null;
		entity.WishExpiresAt = null;
		ulong hungryAt = base.GetTime() + this.hungerPeriod;
		entity.HungryAt = hungryAt;
		entity.FullnessLength = this.hungerPeriod;
		entity.PreviousResourceId = this.prevHungerResourceId;
		simulated.EnterInitialState(EntityManager.ResidentActions["try_spin"], game.simulation);
		game.resourceManager.Add(this.hungerResourceId, -1, game);
		if (this.reward != null)
		{
			game.ApplyReward(this.reward, base.GetTime(), true);
			base.AddPickup(game.simulation);
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00034618 File Offset: 0x00032818
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResidentEntity.UpdateHungerTimeInGameState(gameState, this.target, base.GetTime() + this.hungerPeriod);
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, this.target);
		unitGameState.Remove("wish_product_id");
		unitGameState["fullness_length"] = this.hungerPeriod;
		if (this.prevHungerResourceId != null)
		{
			unitGameState["prev_wish_product_id"] = this.prevHungerResourceId;
		}
		ResourceManager.ApplyCostToGameState(this.hungerResourceId, 1, gameState);
		if (this.reward != null)
		{
			RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
			base.AddPickupToGameState(gameState);
		}
		base.Confirm(gameState);
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x000346CC File Offset: 0x000328CC
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		Resource.AddToTriggerData(ref data, this.hungerResourceId);
	}

	// Token: 0x040005DD RID: 1501
	public const string FEED_RESIDENT = "fu";

	// Token: 0x040005DE RID: 1502
	public const ulong INVALID_ULONG = 18446744073709551615UL;

	// Token: 0x040005DF RID: 1503
	public ulong hungerPeriod;

	// Token: 0x040005E0 RID: 1504
	public int hungerResourceId;

	// Token: 0x040005E1 RID: 1505
	public int? prevHungerResourceId;

	// Token: 0x040005E2 RID: 1506
	public Reward reward;
}
