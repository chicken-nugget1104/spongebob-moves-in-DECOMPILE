using System;
using System.Collections.Generic;

// Token: 0x020000F4 RID: 244
public class ReceiveRewardAction : PersistedTriggerableAction
{
	// Token: 0x060008CD RID: 2253 RVA: 0x000384B0 File Offset: 0x000366B0
	public ReceiveRewardAction(Reward reward, string redemptionOffer) : base("rra", Identity.Null())
	{
		this.redemptionOffer = redemptionOffer;
		this.reward = reward;
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060008CE RID: 2254 RVA: 0x000384D0 File Offset: 0x000366D0
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x000384D4 File Offset: 0x000366D4
	public new static ReceiveRewardAction FromDict(Dictionary<string, object> data)
	{
		string text = (!data.ContainsKey("redemption_offer")) ? string.Empty : TFUtils.LoadString(data, "redemption_offer");
		Reward reward = Reward.FromObject(data["reward"]);
		return new ReceiveRewardAction(reward, text);
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x00038520 File Offset: 0x00036720
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = this.reward.ToDict();
		return dictionary;
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0003854C File Offset: 0x0003674C
	public override void Apply(Game game, ulong utcNow)
	{
		game.ApplyReward(this.reward, base.GetTime(), true);
		base.Apply(game, utcNow);
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0003856C File Offset: 0x0003676C
	public override void Confirm(Dictionary<string, object> gameState)
	{
		RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
		base.Confirm(gameState);
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x00038588 File Offset: 0x00036788
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data.Add("redemption_id", this.redemptionOffer);
		this.reward.AddDataToTrigger(ref data);
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x000385A8 File Offset: 0x000367A8
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger("rra", new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x0400063D RID: 1597
	public const string RECEIVE_REWARD = "rra";

	// Token: 0x0400063E RID: 1598
	public Reward reward;

	// Token: 0x0400063F RID: 1599
	public string redemptionOffer;
}
