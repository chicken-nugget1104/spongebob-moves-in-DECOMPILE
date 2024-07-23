using System;
using System.Collections.Generic;

// Token: 0x020000DB RID: 219
public class LevelUpAction : PersistedTriggerableAction
{
	// Token: 0x06000820 RID: 2080 RVA: 0x000348C8 File Offset: 0x00032AC8
	public LevelUpAction(int level, Reward reward, ulong buildCompleteTime) : base("lu", Identity.Null())
	{
		this.level = level;
		this.reward = reward;
		this.buildCompleteTime = buildCompleteTime;
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000821 RID: 2081 RVA: 0x000348F0 File Offset: 0x00032AF0
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x000348F4 File Offset: 0x00032AF4
	public new static LevelUpAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "level");
		Reward reward = Reward.FromObject(data["reward"]);
		ulong num2 = TFUtils.LoadUlong(data, "build_complete_time", 0UL);
		return new LevelUpAction(num, reward, num2);
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00034938 File Offset: 0x00032B38
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = Reward.RewardToDict(this.reward);
		dictionary["build_complete_time"] = this.buildCompleteTime;
		dictionary["level"] = this.level;
		return dictionary;
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x00034990 File Offset: 0x00032B90
	public override void Apply(Game game, ulong utcNow)
	{
		game.resourceManager.Add(ResourceManager.LEVEL, 1, game);
		if (this.reward != null)
		{
			game.ApplyReward(this.reward, base.GetTime(), true);
		}
		game.playtimeRegistrar.UpdateLevel(game.resourceManager.Query(ResourceManager.LEVEL), base.GetTime());
		base.Apply(game, utcNow);
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x000349F8 File Offset: 0x00032BF8
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.AddAmountToGameState(ResourceManager.LEVEL, 1, gameState);
		if (this.reward != null)
		{
			RewardManager.ApplyToGameState(this.reward, this.buildCompleteTime, gameState);
		}
		PlaytimeRegistrar.ApplyToGameState(ref gameState, this.level, base.GetTime(), base.GetTime(), 0UL);
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.level, "level");
		soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
		Soaring.FireEvent("LevelUp", soaringDictionary);
		base.Confirm(gameState);
	}

	// Token: 0x040005E7 RID: 1511
	public const string LEVEL_UP = "lu";

	// Token: 0x040005E8 RID: 1512
	private const string WALLTIME_START_PREVIOUS_LEVEL = "wts_begin";

	// Token: 0x040005E9 RID: 1513
	private const string PLAYTIME_TO_LEVEL = "time_to";

	// Token: 0x040005EA RID: 1514
	private Reward reward;

	// Token: 0x040005EB RID: 1515
	private Dictionary<string, object> buildingLabels;

	// Token: 0x040005EC RID: 1516
	private ulong buildCompleteTime;

	// Token: 0x040005ED RID: 1517
	private int level;
}
