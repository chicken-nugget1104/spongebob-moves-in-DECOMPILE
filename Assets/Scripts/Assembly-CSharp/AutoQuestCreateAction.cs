using System;
using System.Collections.Generic;

// Token: 0x020000C7 RID: 199
public class AutoQuestCreateAction : PersistedTriggerableAction
{
	// Token: 0x0600078D RID: 1933 RVA: 0x00031B24 File Offset: 0x0002FD24
	public AutoQuestCreateAction(QuestDefinition pQuestDef) : base("aq", Identity.Null())
	{
		this.m_pQuestDef = pQuestDef;
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x0600078E RID: 1934 RVA: 0x00031B40 File Offset: 0x0002FD40
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00031B44 File Offset: 0x0002FD44
	public new static AutoQuestCreateAction FromDict(Dictionary<string, object> pData)
	{
		QuestDefinition pQuestDef = QuestDefinition.FromDict((Dictionary<string, object>)pData["questdef"]);
		return new AutoQuestCreateAction(pQuestDef);
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00031B70 File Offset: 0x0002FD70
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["questdef"] = this.m_pQuestDef.ToDict(false);
		return dictionary;
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00031B9C File Offset: 0x0002FD9C
	public override void Process(Game pGame)
	{
		this.m_pQuest = pGame.questManager.AddQuestDefinition(this.m_pQuestDef);
		this.m_pQuest.StartConditions = new ConditionState(this.m_pQuestDef.Start.Chunks[0].Condition);
		this.m_pQuest.StartConditions.Hydrate(this.m_pQuest.StartProgress, pGame, null);
		int count = this.m_pQuestDef.End.Chunks.Count;
		for (int i = 0; i < count; i++)
		{
			ConditionState conditionState = new ConditionState(this.m_pQuestDef.End.Chunks[i].Condition);
			this.m_pQuest.EndConditions.Add(conditionState);
			conditionState.Hydrate(this.m_pQuest.EndProgress, pGame, null);
		}
		pGame.autoQuestDatabase.AddPreviousAutoQuests(this.m_pQuestDef.AutoQuestID, this.m_pQuestDef.AutoQuestCharacterID);
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00031C98 File Offset: 0x0002FE98
	public override void Apply(Game pGame, ulong nUtcNow)
	{
		base.Apply(pGame, nUtcNow);
		this.m_pQuest = pGame.questManager.AddQuestDefinition(this.m_pQuestDef);
		if (QuestDefinition.LastAutoQuestId < this.m_pQuestDef.Did)
		{
			QuestDefinition.LastAutoQuestId = this.m_pQuestDef.Did;
		}
		this.m_pQuest.StartConditions = new ConditionState(this.m_pQuestDef.Start.Chunks[0].Condition);
		this.m_pQuest.StartConditions.Hydrate(this.m_pQuest.StartProgress, pGame, null);
		int count = this.m_pQuestDef.End.Chunks.Count;
		for (int i = 0; i < count; i++)
		{
			ConditionState conditionState = new ConditionState(this.m_pQuestDef.End.Chunks[0].Condition);
			this.m_pQuest.EndConditions.Add(conditionState);
			conditionState.Hydrate(this.m_pQuest.EndProgress, pGame, null);
		}
		pGame.questManager.RegisterQuest(pGame, this.m_pQuest);
		pGame.autoQuestDatabase.AddPreviousAutoQuests(this.m_pQuestDef.AutoQuestID, this.m_pQuestDef.AutoQuestCharacterID);
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00031DD4 File Offset: 0x0002FFD4
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		List<object> list = (List<object>)dictionary["generated_quest_definition"];
		list.Add(this.m_pQuestDef.ToDict(true));
		List<object> list2 = (List<object>)dictionary["quests"];
		list2.Add(this.m_pQuest.ToDict());
		((Dictionary<string, object>)pGameState["farm"])["auto_quest_id"] = QuestDefinition.LastAutoQuestId;
		AutoQuestDatabase.WritePreviousAutoQuestDataToGameState(pGameState);
		base.Confirm(pGameState);
	}

	// Token: 0x040005A5 RID: 1445
	public const string QUEST_CREATE = "aq";

	// Token: 0x040005A6 RID: 1446
	private QuestDefinition m_pQuestDef;

	// Token: 0x040005A7 RID: 1447
	private Quest m_pQuest;
}
