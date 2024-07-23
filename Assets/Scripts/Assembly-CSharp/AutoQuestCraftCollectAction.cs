using System;
using System.Collections.Generic;

// Token: 0x020000C6 RID: 198
public class AutoQuestCraftCollectAction : PersistedSimulatedAction
{
	// Token: 0x06000784 RID: 1924 RVA: 0x00031944 File Offset: 0x0002FB44
	public AutoQuestCraftCollectAction(int nDID, int nCount) : base("aqcc", Identity.Null(), typeof(AutoQuestCraftCollectAction).ToString())
	{
		this.recipeId = nDID;
		this.count = nCount;
		this.reward = new Reward(new Dictionary<int, int>
		{
			{
				nDID,
				-nCount
			}
		}, null, null, null, null, null, null, null, false, null);
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000785 RID: 1925 RVA: 0x000319A4 File Offset: 0x0002FBA4
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x000319A8 File Offset: 0x0002FBA8
	public new static AutoQuestCraftCollectAction FromDict(Dictionary<string, object> data)
	{
		int nDID = TFUtils.LoadInt(data, "recipe_id");
		int nCount = TFUtils.LoadInt(data, "count");
		return new AutoQuestCraftCollectAction(nDID, nCount);
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x000319D4 File Offset: 0x0002FBD4
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["count"] = this.count;
		dictionary["recipe_id"] = this.reward.ToDict();
		return dictionary;
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x00031A18 File Offset: 0x0002FC18
	public override void Process(Game game)
	{
		game.ApplyReward(this.reward, base.GetTime(), true);
		base.Process(game);
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00031A34 File Offset: 0x0002FC34
	public override void Apply(Game game, ulong utcNow)
	{
		game.ApplyReward(this.reward, base.GetTime(), true);
		base.Apply(game, utcNow);
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00031A54 File Offset: 0x0002FC54
	public override void Confirm(Dictionary<string, object> gameState)
	{
		RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
		base.Confirm(gameState);
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00031A70 File Offset: 0x0002FC70
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		this.reward.AddDataToTrigger(ref data);
		data["recipe_id"] = this.recipeId;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00031AA8 File Offset: 0x0002FCA8
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		Simulated simulated = (Simulated)data["simulated"];
		this.entityId = simulated.entity.Id;
		this.definitionId = simulated.entity.DefinitionId;
		this.simType = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005A1 RID: 1441
	public const string AUTO_QUEST_CRAFT_COLLECT = "aqcc";

	// Token: 0x040005A2 RID: 1442
	private Reward reward;

	// Token: 0x040005A3 RID: 1443
	private int recipeId;

	// Token: 0x040005A4 RID: 1444
	private int count;
}
