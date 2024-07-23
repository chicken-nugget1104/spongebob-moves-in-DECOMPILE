using System;
using System.Collections.Generic;

// Token: 0x020000CB RID: 203
public class CollectMatchBonusAction : PersistedSimulatedAction
{
	// Token: 0x060007AD RID: 1965 RVA: 0x000322A8 File Offset: 0x000304A8
	public CollectMatchBonusAction(Identity id, Reward reward) : base("cmb", id, "BonusPickup")
	{
		this.reward = reward;
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060007AE RID: 1966 RVA: 0x000322C4 File Offset: 0x000304C4
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x000322C8 File Offset: 0x000304C8
	public new static CollectMatchBonusAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromDict(TFUtils.LoadDict(data, "match_bonus"));
		CollectMatchBonusAction collectMatchBonusAction = new CollectMatchBonusAction(id, reward);
		collectMatchBonusAction.DropTargetDataFromDict(data);
		return collectMatchBonusAction;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x0003230C File Offset: 0x0003050C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["match_bonus"] = this.reward.ToDict();
		base.DropTargetDataToDict(dictionary);
		return dictionary;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00032340 File Offset: 0x00030540
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
		entity.MatchBonus = null;
		simulated.EnterInitialState(EntityManager.ResidentActions["start_wander"], game.simulation);
		game.ApplyReward(this.reward, base.GetTime(), true);
		base.AddPickup(game.simulation);
		base.Apply(game, utcNow);
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x000323C4 File Offset: 0x000305C4
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, this.target);
		unitGameState["match_bonus"] = null;
		RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
		base.AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x0003240C File Offset: 0x0003060C
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		this.reward.AddDataToTrigger(ref data);
	}

	// Token: 0x040005AF RID: 1455
	public const string COLLECT_MATCH_BONUS = "cmb";

	// Token: 0x040005B0 RID: 1456
	public const string PICKUP_TRIGGERTYPE = "BonusPickup";

	// Token: 0x040005B1 RID: 1457
	public Reward reward;
}
