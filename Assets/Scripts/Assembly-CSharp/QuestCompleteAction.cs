using System;
using System.Collections.Generic;

// Token: 0x020000EE RID: 238
public class QuestCompleteAction : QuestAction
{
	// Token: 0x060008AA RID: 2218 RVA: 0x00037974 File Offset: 0x00035B74
	private QuestCompleteAction(uint questId, ulong? startTime, ulong? completionTime, Reward reward, Dictionary<string, object> buildingLabels) : base("qc", questId, startTime, completionTime)
	{
		this.reward = reward;
		this.buildingLabels = buildingLabels;
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00037994 File Offset: 0x00035B94
	public QuestCompleteAction(Quest quest, Reward reward, Dictionary<string, object> buildingLabels) : this(quest.Did, quest.StartTime, quest.CompletionTime, reward, buildingLabels)
	{
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x000379BC File Offset: 0x00035BBC
	public new static QuestCompleteAction FromDict(Dictionary<string, object> data)
	{
		Reward reward = Reward.FromObject(data["reward"]);
		uint questId = TFUtils.LoadUint(data, "did");
		ulong? startTime = TFUtils.LoadNullableUlong(data, "start_time");
		ulong? completionTime = TFUtils.LoadNullableUlong(data, "completion_time");
		Dictionary<string, object> dictionary = (Dictionary<string, object>)data["building_labels"];
		return new QuestCompleteAction(questId, startTime, completionTime, reward, dictionary);
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00037A20 File Offset: 0x00035C20
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = this.reward.ToDict();
		dictionary["building_labels"] = this.buildingLabels;
		return dictionary;
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00037A5C File Offset: 0x00035C5C
	public override void Process(Game game)
	{
		game.communityEventManager.QuestComplete(this.questId);
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x00037A70 File Offset: 0x00035C70
	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
		game.ApplyReward(this.reward, base.GetTime(), true);
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00037A98 File Offset: 0x00035C98
	public override void Confirm(Dictionary<string, object> gameState)
	{
		base.Confirm(gameState);
		RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x00037AB4 File Offset: 0x00035CB4
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		data["quest_id"] = (int)this.questId;
	}

	// Token: 0x0400062D RID: 1581
	public const string QUEST_COMPLETE = "qc";

	// Token: 0x0400062E RID: 1582
	private Reward reward;

	// Token: 0x0400062F RID: 1583
	private Dictionary<string, object> buildingLabels;
}
