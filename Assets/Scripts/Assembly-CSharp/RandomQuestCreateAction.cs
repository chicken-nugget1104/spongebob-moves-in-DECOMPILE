using System;
using System.Collections.Generic;

// Token: 0x020000F3 RID: 243
public class RandomQuestCreateAction : PersistedTriggerableAction
{
	// Token: 0x060008C6 RID: 2246 RVA: 0x000381B0 File Offset: 0x000363B0
	public RandomQuestCreateAction(QuestDefinition questDef) : base("rq", Identity.Null())
	{
		this.questDef = questDef;
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060008C7 RID: 2247 RVA: 0x000381CC File Offset: 0x000363CC
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x000381D0 File Offset: 0x000363D0
	public new static RandomQuestCreateAction FromDict(Dictionary<string, object> data)
	{
		QuestDefinition questDefinition = QuestDefinition.FromDict((Dictionary<string, object>)data["questdef"]);
		return new RandomQuestCreateAction(questDefinition);
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x000381FC File Offset: 0x000363FC
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["questdef"] = this.questDef.ToDict(false);
		return dictionary;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00038228 File Offset: 0x00036428
	public override void Process(Game game)
	{
		this.quest = game.questManager.AddQuestDefinition(this.questDef);
		this.quest.StartConditions = new ConditionState(game.questManager.QuestDefinitionList[this.quest.Did].Start.Chunks[0].Condition);
		this.quest.StartConditions.Hydrate(this.quest.StartProgress, game, null);
		ConditionState conditionState = new ConditionState(game.questManager.QuestDefinitionList[this.quest.Did].End.Chunks[0].Condition);
		this.quest.EndConditions.Add(conditionState);
		conditionState.Hydrate(this.quest.EndProgress, game, null);
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x00038304 File Offset: 0x00036504
	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
		this.quest = game.questManager.AddQuestDefinition(this.questDef);
		if (QuestDefinition.LastRandomQuestId < this.questDef.Did)
		{
			QuestDefinition.LastRandomQuestId = this.questDef.Did;
		}
		this.quest.StartConditions = new ConditionState(game.questManager.QuestDefinitionList[this.quest.Did].Start.Chunks[0].Condition);
		this.quest.StartConditions.Hydrate(this.quest.StartProgress, game, null);
		ConditionState conditionState = new ConditionState(game.questManager.QuestDefinitionList[this.quest.Did].End.Chunks[0].Condition);
		this.quest.EndConditions.Add(conditionState);
		conditionState.Hydrate(this.quest.EndProgress, game, null);
		game.questManager.RegisterQuest(game, this.quest);
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00038420 File Offset: 0x00036620
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		List<object> list = (List<object>)dictionary["generated_quest_definition"];
		list.Add(this.questDef.ToDict(true));
		List<object> list2 = (List<object>)dictionary["quests"];
		list2.Add(this.quest.ToDict());
		((Dictionary<string, object>)gameState["farm"])["random_quest_id"] = QuestDefinition.LastRandomQuestId;
		base.Confirm(gameState);
	}

	// Token: 0x0400063A RID: 1594
	public const string QUEST_CREATE = "rq";

	// Token: 0x0400063B RID: 1595
	private QuestDefinition questDef;

	// Token: 0x0400063C RID: 1596
	private Quest quest;
}
