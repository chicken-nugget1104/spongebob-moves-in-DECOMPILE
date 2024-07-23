using System;
using System.Collections.Generic;

// Token: 0x020000F6 RID: 246
public class RewardCapAction : PersistedTriggerableAction
{
	// Token: 0x060008DC RID: 2268 RVA: 0x00038A0C File Offset: 0x00036C0C
	public RewardCapAction(int jelly, int recipes, ulong expiration) : base("cap", Identity.Null())
	{
		this.jelly = jelly;
		this.recipes = recipes;
		this.expiration = expiration;
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060008DD RID: 2269 RVA: 0x00038A34 File Offset: 0x00036C34
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00038A38 File Offset: 0x00036C38
	public new static RewardCapAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "jelly_count");
		int num2 = TFUtils.LoadInt(data, "recipe_count");
		ulong num3 = TFUtils.LoadUlong(data, "expiration", 0UL);
		return new RewardCapAction(num, num2, num3);
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x00038A78 File Offset: 0x00036C78
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["recipe_count"] = this.recipes;
		dictionary["jelly_count"] = this.jelly;
		dictionary["expiration"] = this.expiration;
		return dictionary;
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00038AD0 File Offset: 0x00036CD0
	public override void Apply(Game game, ulong utcNow)
	{
		game.simulation.rewardCap.Reset(this.jelly, this.recipes, this.expiration);
		base.Apply(game, utcNow);
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x00038B08 File Offset: 0x00036D08
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["recipe_count"] = this.recipes;
		dictionary["jelly_count"] = this.jelly;
		dictionary["expiration"] = this.expiration;
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)gameState["farm"];
		dictionary2["caps"] = dictionary;
		base.Confirm(gameState);
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x00038B84 File Offset: 0x00036D84
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00038B88 File Offset: 0x00036D88
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x04000645 RID: 1605
	public const string REWARD_CAP = "cap";

	// Token: 0x04000646 RID: 1606
	private ulong expiration;

	// Token: 0x04000647 RID: 1607
	private int recipes;

	// Token: 0x04000648 RID: 1608
	private int jelly;
}
