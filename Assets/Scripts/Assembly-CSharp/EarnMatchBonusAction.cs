using System;
using System.Collections.Generic;

// Token: 0x020000D5 RID: 213
public class EarnMatchBonusAction : PersistedSimulatedAction
{
	// Token: 0x060007F8 RID: 2040 RVA: 0x00033F10 File Offset: 0x00032110
	public EarnMatchBonusAction(Identity id, Reward reward) : base("emb", id, typeof(EarnMatchBonusAction).ToString())
	{
		this.reward = reward;
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00033F40 File Offset: 0x00032140
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00033F44 File Offset: 0x00032144
	public new static EarnMatchBonusAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromDict(TFUtils.LoadDict(data, "match_bonus"));
		return new EarnMatchBonusAction(id, reward);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00033F84 File Offset: 0x00032184
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["match_bonus"] = ((this.reward != null) ? this.reward.ToDict() : null);
		return dictionary;
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00033FC0 File Offset: 0x000321C0
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.MatchBonus = this.reward;
		simulated.ClearPendingCommands();
		if (this.reward != null)
		{
			simulated.EnterInitialState(EntityManager.ResidentActions["wait_bonus"], game.simulation);
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00034038 File Offset: 0x00032238
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, this.target);
		unitGameState["match_bonus"] = ((this.reward != null) ? this.reward.ToDict() : null);
		base.Confirm(gameState);
	}

	// Token: 0x040005D8 RID: 1496
	public const string EARN_MATCH_BONUS = "emb";

	// Token: 0x040005D9 RID: 1497
	public Reward reward;
}
