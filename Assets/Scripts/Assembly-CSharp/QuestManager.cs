using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class QuestManager : ITriggerObserver
{
	// Token: 0x06000EBF RID: 3775 RVA: 0x0005B49C File Offset: 0x0005969C
	public QuestManager()
	{
		this.questList = new Dictionary<uint, Quest>();
		this.questDefinitionList = new Dictionary<uint, QuestDefinition>();
		this.randomQuestTemplateList = new Dictionary<uint, QuestTemplate>();
		this.questLineProgress = new Dictionary<string, Vector2>();
		this.activatedDids = new OrderedSet<uint>();
		this.completedDids = new OrderedSet<uint>();
		this.deactivatedCompletedDids = new OrderedSet<uint>();
		this.LoadAndInitializeQuestPrototypes();
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0005B53C File Offset: 0x0005973C
	public bool IsActive
	{
		get
		{
			return this.isActive;
		}
	}

	// Token: 0x17000211 RID: 529
	// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x0005B544 File Offset: 0x00059744
	public Action OnShowDialogCallback
	{
		set
		{
			this.onShowDialogCallback = value;
		}
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x0005B550 File Offset: 0x00059750
	private string[] GetFilesToLoad()
	{
		return Config.QUESTS_PATH;
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x0005B558 File Offset: 0x00059758
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x0005B55C File Offset: 0x0005975C
	private void LoadAndInitializeQuestPrototypes()
	{
		string[] filesToLoad = this.GetFilesToLoad();
		foreach (string text in filesToLoad)
		{
			string filePathFromString = this.GetFilePathFromString(text);
			string json = TFUtils.ReadAllText(filePathFromString);
			List<object> list = (List<object>)Json.Deserialize(json);
			foreach (object obj in list)
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
				if (!dictionary.ContainsKey("type"))
				{
					TFUtils.ErrorLog("Quest Manager cannot process file: " + text);
				}
				else
				{
					string text2 = (string)dictionary["type"];
					if (text2 != null)
					{
						if (QuestManager.<>f__switch$map12 == null)
						{
							QuestManager.<>f__switch$map12 = new Dictionary<string, int>(2)
							{
								{
									"quest",
									0
								},
								{
									"quest_template",
									1
								}
							};
						}
						int num;
						if (QuestManager.<>f__switch$map12.TryGetValue(text2, out num))
						{
							if (num == 0)
							{
								QuestDefinition questDefinition = QuestDefinition.FromDict(dictionary);
								TFUtils.Assert(questDefinition.Did < 400000U || questDefinition.Did > 500000U, string.Concat(new object[]
								{
									"Invalid Quest Id (",
									questDefinition.Did,
									") in file ",
									text,
									".  Quest ids from ",
									400000U,
									" to ",
									500000U,
									" are reserved for Randomly Generated Quests."
								}));
								this.AddQuestDefinition(questDefinition);
								continue;
							}
							if (num == 1)
							{
								QuestTemplate questDef = QuestTemplate.FromDict(dictionary);
								this.AddRandomQuestTemplate(questDef);
								continue;
							}
						}
					}
					TFUtils.ErrorLog(string.Concat(new object[]
					{
						"Quest Manager found unexpected file type in Quests: ",
						dictionary["type"],
						" in ",
						text
					}));
				}
			}
		}
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x0005B788 File Offset: 0x00059988
	public void AddRandomQuestTemplate(QuestTemplate questDef)
	{
		this.randomQuestTemplateList[questDef.Did] = questDef;
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0005B79C File Offset: 0x0005999C
	public Quest AddQuestDefinition(QuestDefinition questDef)
	{
		if (!SBSettings.EnableRandomQuests && questDef.Did >= 400000U && questDef.Did <= 500000U)
		{
			return null;
		}
		if (!SBSettings.EnableAutoQuests && questDef.Did >= 500001U && questDef.Did <= 600000U)
		{
			return null;
		}
		TFUtils.Assert(!this.questDefinitionList.ContainsKey(questDef.Did), "Duplicate Quest ID found for quest: " + questDef.Did);
		Quest result = this.CreateNewQuestInfo(questDef.Did);
		this.questDefinitionList[questDef.Did] = questDef;
		if (questDef.QuestLine != null && questDef.QuestLine.HasProgress)
		{
			string name = questDef.QuestLine.Name;
			Vector2 vector = Vector2.up;
			if (this.questLineProgress.ContainsKey(name))
			{
				vector += this.questLineProgress[name];
			}
			this.questLineProgress[name] = vector;
		}
		return result;
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x0005B8B0 File Offset: 0x00059AB0
	public QuestTemplate GetRandomQuestTemplate()
	{
		if (this.randomQuestTemplateList.Count == 0)
		{
			TFUtils.ErrorLog("Failed to find any random quest templates");
		}
		uint[] array = new uint[this.randomQuestTemplateList.Keys.Count];
		this.randomQuestTemplateList.Keys.CopyTo(array, 0);
		return this.randomQuestTemplateList[array[UnityEngine.Random.Range(0, this.randomQuestTemplateList.Count)]];
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x0005B920 File Offset: 0x00059B20
	public Quest CreateNewQuestInfo(uint did)
	{
		ConditionalProgress conditionalProgress = new ConditionalProgress();
		Quest quest = new Quest(did, conditionalProgress, conditionalProgress, null, null, false);
		this.questList[did] = quest;
		return quest;
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x0005B960 File Offset: 0x00059B60
	public void SetDialogManager(DialogPackageManager dialogPackageMgr)
	{
		this.dialogPackageManager = dialogPackageMgr;
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x0005B96C File Offset: 0x00059B6C
	public void Activate(Game game)
	{
		foreach (Quest quest in this.questList.Values)
		{
			quest.StartConditions = new ConditionState(this.questDefinitionList[quest.Did].Start.Chunks[0].Condition);
			quest.StartConditions.Hydrate(quest.StartProgress, game, null);
			foreach (QuestBookendInfo.ChunkConditions chunkConditions in this.questDefinitionList[quest.Did].End.Chunks)
			{
				ConditionState conditionState = new ConditionState(chunkConditions.Condition);
				conditionState.Hydrate(quest.EndProgress, game, null);
				quest.EndConditions.Add(conditionState);
			}
		}
		foreach (Quest quest2 in this.questList.Values)
		{
			QuestDefinition questDefinition = this.GetQuestDefinition(quest2.Did);
			if (questDefinition == null || questDefinition.MicroEventDID == null || game.microEventManager.IsMicroEventActive(questDefinition.MicroEventDID.Value))
			{
				if (quest2.StartTime == null)
				{
					if (quest2.StartConditions.Examine() == ConditionResult.PASS)
					{
						this.ProgressTowardsStartConditions(quest2, game, quest2.StartConditions.Dehydrate().MetIds);
						this.ActivateQuest(quest2, game);
						game.ModifyGameState(new QuestStartAction(quest2));
					}
				}
				else if (quest2.CompletionTime == null)
				{
					if (questDefinition.SessionActions != null)
					{
						SessionActionTracker sessionAction = quest2.InstantiateSessionAction(questDefinition.SessionActions);
						game.sessionActionManager.Request(sessionAction, game, quest2.TrackerTag);
					}
					if (questDefinition.Start.DialogSequenceId != null)
					{
						game.triggerRouter.RouteTrigger(new QuestCompleteDialogInputData(questDefinition.Start.DialogSequenceId.Value, null, null, new uint?(questDefinition.Did)).CreateTrigger(TFUtils.EpochTime()), game);
					}
				}
			}
		}
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		this.isActive = true;
		List<ITrigger> list4 = new List<ITrigger>(this.completedDids.Count);
		foreach (uint num in this.completedDids)
		{
			Quest quest3 = this.questList[num];
			QuestCompleteAction questCompleteAction = new QuestCompleteAction(quest3, null, null);
			list4.Add(questCompleteAction.CreateTrigger(new Dictionary<string, object>()));
			list.AddRange(this.HandleFeatureUnlocks(game, this.GetQuestDefinition(num)));
			list2.AddRange(this.HandleBuildingUnlocks(game, this.GetQuestDefinition(num)));
			list3.AddRange(this.HandleCostumeUnlocks(game, this.GetQuestDefinition(num)));
			this.UpdateQuestLineProgress(quest3);
		}
		foreach (ITrigger trigger in list4)
		{
			game.triggerRouter.RouteTrigger(trigger, game);
		}
		foreach (uint did in this.activatedDids)
		{
			list.AddRange(this.HandleFeatureUnlocks(game, this.GetQuestDefinition(did)));
			list2.AddRange(this.HandleBuildingUnlocks(game, this.GetQuestDefinition(did)));
			list3.AddRange(this.HandleCostumeUnlocks(game, this.GetQuestDefinition(did)));
		}
		if (list.Count > 0)
		{
			game.simulation.ModifyGameState(new FeatureUnlocksAction(list));
		}
		int count = list2.Count;
		if (count > 0)
		{
			game.simulation.ModifyGameState(new BuildingUnlocksAction(list2));
			string empty = string.Empty;
			string text = string.Empty;
			for (int i = 0; i < count; i++)
			{
				game.catalog.GetNameAndTypeForDID(list2[i], out empty, out text);
				text = Catalog.ConvertTypeToDeltaDNAType(text);
				AnalyticsWrapper.LogFeatureUnlocked(game, empty, text);
			}
		}
		foreach (int nCostumeDID in list3)
		{
			CostumeManager.Costume costume = game.costumeManager.GetCostume(nCostumeDID);
			if (costume != null)
			{
				game.analytics.LogCostumeUnlocked(costume.m_nDID);
				AnalyticsWrapper.LogCostumeUnlocked(game, costume);
			}
			game.simulation.ModifyGameState(new UnlockCostumeAction(nCostumeDID));
		}
		Quest quest4 = this.GetQuest(QuestDefinition.LastAutoQuestId);
		if (quest4 != null && quest4.CompletionTime == null)
		{
			QuestDefinition questDefinition2 = this.GetQuestDefinition(quest4.Did);
			if (questDefinition2 != null && !game.autoQuestDatabase.IsQuestValid(game, questDefinition2))
			{
				game.ModifyGameState(new AutoQuestCleanupAction(quest4));
				this.m_uQuestCompletionTimestamp = new ulong?((ulong)SoaringTime.AdjustedServerTime);
				game.ModifyGameState(new UpdateVariableAction<ulong?>("auto_quest_completion_time", this.m_uQuestCompletionTimestamp));
				if (this.m_autoQuestCount == 1)
				{
					this.m_uAutoQuestStartTime = new ulong?(this.m_uCurrentTime);
					this.m_uAutoQuestStartTime = new ulong?((ulong)SoaringTime.AdjustedServerTime);
					game.ModifyGameState(new UpdateVariableAction<ulong?>("first_auto_quest_completion_timestamp", this.m_uAutoQuestStartTime));
				}
				game.ModifyGameState(new UpdateVariableAction<int>("current_auto_quest_completion_count", this.m_autoQuestCount));
				this.m_autoQuestCount++;
			}
		}
		game.triggerRouter.RouteTrigger(Trigger.Null, game);
		game.triggerRouter.RouteTrigger(ReevaluateTrigger.CreateTrigger(), game);
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x0005C05C File Offset: 0x0005A25C
	public Quest GetQuest(uint did)
	{
		if (this.questList.ContainsKey(did))
		{
			return this.questList[did];
		}
		return null;
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x0005C080 File Offset: 0x0005A280
	public QuestDefinition GetQuestDefinition(uint did)
	{
		if (this.questDefinitionList.ContainsKey(did))
		{
			return this.questDefinitionList[did];
		}
		return null;
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x0005C0A4 File Offset: 0x0005A2A4
	public List<int> GetTasksCompleting()
	{
		List<int> list = new List<int>();
		List<uint> list2 = new List<uint>(this.activatedDids);
		int count = list2.Count;
		for (int i = 0; i < count; i++)
		{
			QuestDefinition questDefinition = this.GetQuestDefinition(list2[i]);
			if (questDefinition != null)
			{
				List<QuestBookendInfo.ChunkConditions> chunks = questDefinition.End.Chunks;
				int count2 = chunks.Count;
				for (int j = 0; j < count2; j++)
				{
					if (chunks[j].Condition is TaskCompleteCondition)
					{
						Dictionary<string, object> dictionary = chunks[j].Condition.ToDict();
						if (dictionary.ContainsKey("task_id"))
						{
							list.Add(TFUtils.LoadInt(dictionary, "task_id"));
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0005C178 File Offset: 0x0005A378
	public void RegisterQuest(Game pGame, Quest quest)
	{
		uint did = quest.Did;
		QuestDefinition questDefinition = this.GetQuestDefinition(did);
		if (questDefinition == null)
		{
			if (quest.CompletionTime != null && !this.deactivatedCompletedDids.Contains(did))
			{
				this.deactivatedCompletedDids.Add(did);
			}
			return;
		}
		if (questDefinition.MicroEventDID != null && !pGame.microEventManager.IsMicroEventActive(questDefinition.MicroEventDID.Value))
		{
			return;
		}
		this.questList[did] = quest;
		if (quest.CompletionTime != null)
		{
			if (this.completedDids.Contains(did))
			{
			}
			this.activatedDids.Remove(did);
			this.completedDids.Add(did);
		}
		else if (quest.StartTime != null)
		{
			this.activatedDids.Add(did);
			TFUtils.Assert(!this.completedDids.Contains(did), "An active quest shouldn't have been on the completed list");
		}
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0005C28C File Offset: 0x0005A48C
	public void ActivateQuest(Quest quest, Game game)
	{
		uint did = quest.Did;
		TFUtils.Assert(!this.activatedDids.Contains(did) && !this.completedDids.Contains(did), string.Format("Tried to Re-Activate Quest {0}. It a to have already been activated or completed before.", quest.ToString()));
		TFUtils.Assert(quest.TrackerTag != null && quest.TrackerTag != string.Empty, "Quest is missing its tracker tag. Ensure that it has one before calling ActivateQuest");
		quest.Start(TFUtils.EpochTime());
		this.activatedDids.Add(quest.Did);
		QuestDefinition questDefinition = this.questDefinitionList[quest.Did];
		if (questDefinition.HasFeatureUnlocks)
		{
			this.HandleFeatureUnlocks(game, questDefinition);
			game.Record(new FeatureUnlocksAction(questDefinition.FeatureUnlocks));
		}
		if (questDefinition.HasBuildingUnlocks)
		{
			game.Record(new BuildingUnlocksAction(questDefinition.BuildingUnlocks));
			this.HandleBuildingUnlocks(game, questDefinition);
			string empty = string.Empty;
			string text = string.Empty;
			int count = questDefinition.BuildingUnlocks.Count;
			for (int i = 0; i < count; i++)
			{
				game.catalog.GetNameAndTypeForDID(questDefinition.BuildingUnlocks[i], out empty, out text);
				text = Catalog.ConvertTypeToDeltaDNAType(text);
				AnalyticsWrapper.LogFeatureUnlocked(game, empty, text);
			}
		}
		if (quest.Did >= 500001U && quest.Did <= 600000U)
		{
			List<QuestBookendInfo.ChunkConditions> chunks = questDefinition.End.Chunks;
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			int num = chunks.Count - 1;
			for (int j = 0; j < num; j++)
			{
				Dictionary<string, object> dictionary = chunks[j].Condition.ToDict();
				if (dictionary.ContainsKey("count") && dictionary.ContainsKey("resource_id"))
				{
					soaringDictionary.addValue(TFUtils.LoadInt(dictionary, "count"), TFUtils.LoadInt(dictionary, "resource_id").ToString());
				}
			}
			if (soaringDictionary.count() <= 0)
			{
				soaringDictionary = null;
			}
			game.analytics.LogAutoQuestStarted(questDefinition.Tag);
			AnalyticsWrapper.LogAutoQuestStarted(game, questDefinition, soaringDictionary);
		}
		game.analytics.LogQuestStart(questDefinition.Tag, questDefinition.Name, questDefinition.Did, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
		AnalyticsWrapper.LogQuestStarted(game, questDefinition);
		if (questDefinition.Start.DialogSequenceId != null)
		{
			this.QueueDialogSequences(questDefinition.DialogPackageDid, questDefinition.Start.DialogSequenceId.Value, new List<Reward>
			{
				questDefinition.Reward
			}, questDefinition.Start.Postpone, questDefinition.Did);
		}
		if (questDefinition.HasCostumeUnlocks)
		{
			List<int> list = this.HandleCostumeUnlocks(game, questDefinition);
			foreach (int num2 in list)
			{
				CostumeManager.Costume costume = game.costumeManager.GetCostume(num2);
				if (costume != null)
				{
					game.analytics.LogCostumeUnlocked(costume.m_nDID);
					AnalyticsWrapper.LogCostumeUnlocked(game, costume);
				}
				game.simulation.ModifyGameState(new UnlockCostumeAction(num2));
				this.QueueDialogSequences((uint)num2, 999999995U, new List<Reward>(), 0f, 999999995U);
			}
		}
		if (questDefinition.SessionActions != null)
		{
			SessionActionTracker sessionAction = quest.InstantiateSessionAction(questDefinition.SessionActions);
			game.sessionActionManager.Request(sessionAction, game, quest.TrackerTag);
		}
		game.triggerRouter.RouteTrigger(ReevaluateTrigger.CreateTrigger(), game);
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x0005C658 File Offset: 0x0005A858
	public void DeactivateQuest(Game game, uint did)
	{
		if (this.activatedDids.Contains(did))
		{
			this.activatedDids.Remove(did);
		}
		if (this.completedDids.Contains(did))
		{
			this.completedDids.Remove(did);
			this.deactivatedCompletedDids.Add(did);
		}
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x0005C6B0 File Offset: 0x0005A8B0
	public void CompleteQuest(Quest quest, Game game)
	{
		QuestDefinition questDefinition = this.questDefinitionList[quest.Did];
		ulong num = TFUtils.EpochTime();
		quest.Complete(num);
		this.activatedDids.Remove(quest.Did);
		this.completedDids.Add(quest.Did);
		this.UpdateQuestLineProgress(quest);
		Reward reward = questDefinition.GenerateReward(game.simulation);
		if (quest.Did >= 500001U && quest.Did <= 600000U)
		{
			List<QuestBookendInfo.ChunkConditions> chunks = questDefinition.End.Chunks;
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			int num2 = chunks.Count - 1;
			for (int i = 0; i < num2; i++)
			{
				Dictionary<string, object> dictionary = chunks[i].Condition.ToDict();
				if (dictionary.ContainsKey("count") && dictionary.ContainsKey("resource_id"))
				{
					soaringDictionary.addValue(TFUtils.LoadInt(dictionary, "count"), TFUtils.LoadInt(dictionary, "resource_id").ToString());
				}
			}
			if (soaringDictionary.count() <= 0)
			{
				soaringDictionary = null;
			}
			game.analytics.LogAutoQuestCompleted(questDefinition.Tag);
			AnalyticsWrapper.LogAutoQuestCompleted(game, questDefinition, soaringDictionary, reward);
		}
		else
		{
			game.analytics.LogQuestCompleteSoaring(questDefinition.Tag);
			AnalyticsWrapper.LogQuestCompleted(game, questDefinition, reward);
		}
		game.analytics.LogQuestComplete(questDefinition.Tag, questDefinition.Name, quest.Did, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
		game.analytics.LogQuestCompleteJJAMT(questDefinition.Tag, questDefinition.Name, quest.Did, game.resourceManager.Resources[ResourceManager.LEVEL].Amount, game.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount);
		game.analytics.LogQuestCompleteGoldAMT(questDefinition.Tag, questDefinition.Name, quest.Did, game.resourceManager.Resources[ResourceManager.LEVEL].Amount, game.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount);
		if (questDefinition.End.DialogSequenceId != null)
		{
			this.QueueDialogSequences(questDefinition.DialogPackageDid, questDefinition.End.DialogSequenceId.Value, new List<Reward>
			{
				questDefinition.Reward
			}, questDefinition.End.Postpone, questDefinition.Did);
		}
		Dictionary<string, object> buildingLabels = reward.BuildingLabels;
		List<Reward> list = new List<Reward>();
		foreach (KeyValuePair<int, int> keyValuePair in reward.ResourceAmounts)
		{
			int key = keyValuePair.Key;
			int value = keyValuePair.Value;
			IResourceProgressCalculator resourceCalculator = game.resourceCalculatorManager.GetResourceCalculator(key);
			if (resourceCalculator != null)
			{
				List<Reward> list2;
				resourceCalculator.GetRewardsForIncreasingResource(game.simulation, game.simulation.resourceManager.Resources, value, out list2);
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						Reward reward2 = list2[j];
						if (reward2 != null)
						{
							game.ApplyReward(reward2, num, false);
						}
						game.resourceManager.Add(ResourceManager.LEVEL, 1, game);
						int level = game.resourceManager.Query(ResourceManager.LEVEL);
						game.ModifyGameState(new LevelUpAction(level, reward2, TFUtils.EpochTime()));
					}
					list.AddRange(list2);
				}
			}
		}
		game.ApplyReward(reward, num, false);
		if (list.Count > 0)
		{
			int packageId = game.resourceManager.Query(ResourceManager.LEVEL);
			this.QueueDialogSequences((uint)packageId, 999999999U, list, questDefinition.End.Postpone, 999999999U);
		}
		QuestCompleteAction action = new QuestCompleteAction(quest, reward, buildingLabels);
		this.EnqueueDeferredAction(game, action);
		if (quest.Did >= 400000U && quest.Did <= 500000U)
		{
			game.ModifyGameState(new RandomQuestCleanupAction(quest));
		}
		if (quest.Did >= 500001U && quest.Did <= 600000U)
		{
			game.ModifyGameState(new AutoQuestCleanupAction(quest));
			this.m_uQuestCompletionTimestamp = new ulong?((ulong)SoaringTime.AdjustedServerTime);
			game.ModifyGameState(new UpdateVariableAction<ulong?>("auto_quest_completion_time", this.m_uQuestCompletionTimestamp));
			if (this.m_autoQuestCount == 1)
			{
				this.m_uAutoQuestStartTime = new ulong?(this.m_uCurrentTime);
				this.m_uAutoQuestStartTime = new ulong?((ulong)SoaringTime.AdjustedServerTime);
				game.ModifyGameState(new UpdateVariableAction<ulong?>("first_auto_quest_completion_timestamp", this.m_uAutoQuestStartTime));
			}
			game.ModifyGameState(new UpdateVariableAction<int>("current_auto_quest_completion_count", this.m_autoQuestCount));
			this.m_autoQuestCount++;
		}
		game.sessionActionManager.ObliterateAnyTagged(quest.TrackerTag, game);
		if (questDefinition.PostSessionActions != null)
		{
			SessionActionTracker sessionAction = quest.InstantiateSessionAction(questDefinition.PostSessionActions);
			game.sessionActionManager.Request(sessionAction, game, quest.TrackerTag);
		}
		foreach (int packageId2 in reward.CostumeUnlocks)
		{
			this.QueueDialogSequences((uint)packageId2, 999999995U, new List<Reward>(), questDefinition.End.Postpone, 999999995U);
		}
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x0005CC64 File Offset: 0x0005AE64
	public float? GetQuestLineProgress(QuestDefinition questDef)
	{
		Vector2 vector;
		if (questDef.QuestLine == null || !this.questLineProgress.TryGetValue(questDef.QuestLine.Name, out vector))
		{
			return null;
		}
		return new float?(vector.x / vector.y);
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x0005CCB8 File Offset: 0x0005AEB8
	public float? GetQuestLineLastProgress(QuestDefinition questDef)
	{
		Vector2 a;
		if (questDef.QuestLine == null || !this.questLineProgress.TryGetValue(questDef.QuestLine.Name, out a))
		{
			return null;
		}
		a -= Vector2.right;
		return new float?(a.x / a.y);
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x0005CD18 File Offset: 0x0005AF18
	private void ProgressTowardsStartConditions(Quest quest, Game game, List<uint> conditionIds)
	{
		TFUtils.Assert(!this.completedDids.Contains(quest.Did), "Trying to make start progress on a quest that is already complete");
		this.EnqueueDeferredAction(game, new QuestProgressAction(quest, QuestProgressAction.ConditionType.START, conditionIds));
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0005CD54 File Offset: 0x0005AF54
	private void ProgressTowardsEndConditions(Quest quest, Game game, List<uint> conditionIds)
	{
		TFUtils.Assert(this.activatedDids.Contains(quest.Did), "Trying to make ending progress on a quest that isn't active");
		this.EnqueueDeferredAction(game, new QuestProgressAction(quest, QuestProgressAction.ConditionType.END, conditionIds));
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0005CD8C File Offset: 0x0005AF8C
	private void FailQuest(Quest quest)
	{
		TFUtils.Assert(false, "Quest failures not implemented");
		this.activatedDids.Remove(quest.Did);
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0005CDAC File Offset: 0x0005AFAC
	public void OnUpdate(Game game)
	{
		if (this.postponed.Count > 0)
		{
			QuestManager.PostponedDialogParams postponedDialogParams = this.postponed.Peek();
			if (postponedDialogParams.complete.CompareTo(DateTime.Now) <= 0)
			{
				postponedDialogParams = this.postponed.Dequeue();
				this.AddDialogSequences(game, postponedDialogParams.packageId, postponedDialogParams.sequenceId, postponedDialogParams.rewards, postponedDialogParams.questId, true);
			}
		}
		if (this.isActive)
		{
			if (SBSettings.EnableRandomQuests && game.featureManager.CheckFeature("allow_random_quests") && !game.questManager.IsQuestActivated(QuestDefinition.LastRandomQuestId))
			{
				this.CreateAndTriggerRandomQuest(game);
			}
			if (SBSettings.EnableAutoQuests && game.featureManager.CheckFeature("allow_auto_quests") && !game.questManager.IsQuestActivated(QuestDefinition.LastAutoQuestId))
			{
				if (this.m_uQuestCompletionTimestamp != null)
				{
					ulong? uQuestCompletionTimestamp = this.m_uQuestCompletionTimestamp;
					ulong? num = (uQuestCompletionTimestamp == null) ? null : new ulong?((ulong)(SoaringTime.AdjustedServerTime - (long)uQuestCompletionTimestamp.Value));
					if (num == null || num.Value <= this.m_uQuestTimeGap)
					{
						goto IL_160;
					}
				}
				if (this.m_autoQuestCount < 3)
				{
					this.m_autoQuestCount++;
					this.CreateAndTriggerAutoQuest(game);
				}
			}
		}
		IL_160:
		if (this.showDialogs)
		{
			if (this.onShowDialogCallback != null)
			{
				this.onShowDialogCallback();
			}
			this.showDialogs = false;
		}
		uint num2 = 40000U;
		if (game.resourceManager.PlayerLevelAmount > 1 && game.questManager.IsQuestActivated(num2))
		{
			this.activatedDids.Remove(num2);
			this.completedDids.Add(num2);
			game.sessionActionManager.ObliterateAnyTagged(this.questList[num2].TrackerTag, game);
		}
		uint num3 = 2720U;
		if (game.questManager.IsQuestActivated(num3) || game.questManager.IsQuestActive(num3))
		{
			this.activatedDids.Remove(num3);
			this.completedDids.Add(num3);
			game.sessionActionManager.ObliterateAnyTagged(this.questList[num3].TrackerTag, game);
		}
		this.m_uCurrentTime = (ulong)SoaringTime.AdjustedServerTime;
		ulong? uAutoQuestStartTime = this.m_uAutoQuestStartTime;
		if (uAutoQuestStartTime == null)
		{
			this.m_uAutoQuestStartTime = new ulong?(this.m_uCurrentTime);
			this.m_autoQuestCount = 0;
		}
		ulong? uAutoQuestStartTime2 = this.m_uAutoQuestStartTime;
		if (TFUtils.EpochToDateTime(uAutoQuestStartTime2.Value + this.m_uTimeTillResetQuest) < TFUtils.EpochToDateTime(this.m_uCurrentTime))
		{
			this.m_autoQuestCount = 0;
			this.m_uAutoQuestStartTime = 0UL;
		}
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0005D080 File Offset: 0x0005B280
	public bool IsQuestActivated(uint did)
	{
		return this.activatedDids.Contains(did);
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0005D090 File Offset: 0x0005B290
	public bool IsQuestCompleted(uint did)
	{
		return this.completedDids.Contains(did) || this.deactivatedCompletedDids.Contains(did);
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0005D0C0 File Offset: 0x0005B2C0
	public void CreateAndTriggerRandomQuest(Game game)
	{
		if (QuestDefinition.LastRandomQuestId == 0U || (QuestDefinition.LastRandomQuestId += 1U) > 500000U)
		{
			QuestDefinition.LastRandomQuestId = 400000U;
		}
		QuestDefinition questDefinition = QuestDefinition.CreateRandom(this, game);
		game.ModifyGameState(new RandomQuestCreateAction(questDefinition));
		Quest quest = this.GetQuest(questDefinition.Did);
		this.ActivateQuest(quest, game);
		game.ModifyGameState(new QuestStartAction(quest));
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0005D130 File Offset: 0x0005B330
	public void CreateAndTriggerAutoQuest(Game pGame)
	{
		if (QuestDefinition.LastAutoQuestId == 0U || (QuestDefinition.LastAutoQuestId += 1U) > 600000U)
		{
			QuestDefinition.LastAutoQuestId = 500001U;
		}
		QuestDefinition questDefinition = QuestDefinition.CreateAuto(pGame);
		if (questDefinition == null)
		{
			return;
		}
		pGame.ModifyGameState(new AutoQuestCreateAction(questDefinition));
		Quest quest = this.GetQuest(questDefinition.Did);
		this.ActivateQuest(quest, pGame);
		pGame.ModifyGameState(new QuestStartAction(quest));
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0005D1A4 File Offset: 0x0005B3A4
	private void QueueDialogSequences(uint packageId, uint sequenceId, List<Reward> rewards, float postpone, uint questId)
	{
		QuestManager.PostponedDialogParams postponedDialogParams = new QuestManager.PostponedDialogParams();
		postponedDialogParams.packageId = packageId;
		postponedDialogParams.sequenceId = sequenceId;
		postponedDialogParams.rewards = rewards;
		postponedDialogParams.complete = DateTime.Now.AddSeconds((double)postpone);
		postponedDialogParams.questId = questId;
		this.postponed.Enqueue(postponedDialogParams);
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0005D1F8 File Offset: 0x0005B3F8
	public void AddDialogSequences(Game game, uint packageId, uint sequenceId, List<Reward> rewards, uint questId, bool bShowDialogs = true)
	{
		int count = rewards.Count;
		List<object> list = new List<object>(count);
		for (int i = 0; i < count; i++)
		{
			list.Add(rewards[i].ToDict());
		}
		if (sequenceId == 0U)
		{
			return;
		}
		Dictionary<string, object> contextData = new Dictionary<string, object>
		{
			{
				"rewards",
				list
			}
		};
		if (sequenceId == 10000U)
		{
			if (!QuestDefinition.StartInputPrompts.ContainsKey(QuestDefinition.LastRandomQuestId))
			{
				QuestDefinition.RecreateRandomQuestStartInputData(game, QuestDefinition.LastRandomQuestId);
			}
			if (QuestDefinition.StartInputPrompts.ContainsKey(QuestDefinition.LastRandomQuestId))
			{
				List<DialogInputData> list2 = new List<DialogInputData>();
				QuestStartDialogInputData item = new QuestStartDialogInputData(10000U, QuestDefinition.StartInputPrompts[QuestDefinition.LastRandomQuestId], contextData, new uint?(QuestDefinition.LastRandomQuestId));
				list2.Add(item);
				this.dialogPackageManager.AddDialogInputBatch(game, list2, 10000U);
			}
		}
		else if (sequenceId == 10001U)
		{
			if (!QuestDefinition.CompleteInputPrompts.ContainsKey(QuestDefinition.LastRandomQuestId))
			{
				QuestDefinition.RecreateRandomQuestCompleteInputData(game, QuestDefinition.LastRandomQuestId);
			}
			if (QuestDefinition.CompleteInputPrompts.ContainsKey(QuestDefinition.LastRandomQuestId))
			{
				List<DialogInputData> list3 = new List<DialogInputData>();
				QuestCompleteDialogInputData item2 = new QuestCompleteDialogInputData(10001U, QuestDefinition.CompleteInputPrompts[QuestDefinition.LastRandomQuestId], contextData, new uint?(QuestDefinition.LastRandomQuestId));
				list3.Add(item2);
				this.dialogPackageManager.AddDialogInputBatch(game, list3, 10001U);
			}
		}
		else if (sequenceId == 10002U)
		{
			if (!QuestDefinition.StartInputPrompts.ContainsKey(QuestDefinition.LastAutoQuestId))
			{
				QuestDefinition.RecreateAutoQuestIntroInputData(game, QuestDefinition.LastAutoQuestId);
			}
			List<DialogInputData> list4 = new List<DialogInputData>();
			if (QuestDefinition.StartInputPrompts.ContainsKey(QuestDefinition.LastAutoQuestId))
			{
				CharacterDialogInputData item3 = new CharacterDialogInputData(10002U, QuestDefinition.StartInputPrompts[QuestDefinition.LastAutoQuestId]);
				list4.Add(item3);
			}
			list4.Add(new QuestStartDialogInputData(10002U, new Dictionary<string, object>
			{
				{
					"type",
					"quest_start"
				}
			}, contextData, new uint?(QuestDefinition.LastAutoQuestId)));
			this.dialogPackageManager.AddDialogInputBatch(game, list4, 10002U);
		}
		else if (sequenceId == 10003U)
		{
			if (!QuestDefinition.CompleteInputPrompts.ContainsKey(QuestDefinition.LastAutoQuestId))
			{
				QuestDefinition.RecreateAutoQuestOutroInputData(game, QuestDefinition.LastAutoQuestId);
			}
			List<DialogInputData> list5 = new List<DialogInputData>();
			if (QuestDefinition.CompleteInputPrompts.ContainsKey(QuestDefinition.LastAutoQuestId))
			{
				CharacterDialogInputData item4 = new CharacterDialogInputData(10003U, QuestDefinition.CompleteInputPrompts[QuestDefinition.LastAutoQuestId]);
				list5.Add(item4);
			}
			list5.Add(new QuestCompleteDialogInputData(10003U, new Dictionary<string, object>
			{
				{
					"type",
					"quest_complete"
				}
			}, contextData, new uint?(QuestDefinition.LastAutoQuestId)));
			this.dialogPackageManager.AddDialogInputBatch(game, list5, 10003U);
		}
		else if (sequenceId == 999999999U)
		{
			List<DialogInputData> list6 = new List<DialogInputData>();
			list6.Add(new LevelUpDialogInputData((int)packageId, rewards));
			this.dialogPackageManager.AddDialogInputBatch(game, list6, uint.MaxValue);
		}
		else if (sequenceId == 999999995U)
		{
			CostumeManager.Costume costume = game.costumeManager.GetCostume((int)packageId);
			FoundItemDialogInputData item5 = new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), Language.Get(costume.m_sName)), costume.m_sPortrait, "Beat_FoundRecipe");
			this.dialogPackageManager.AddDialogInputBatch(game, new List<DialogInputData>
			{
				item5
			}, uint.MaxValue);
		}
		else
		{
			DialogPackage dialogPackage = this.dialogPackageManager.GetDialogPackage(packageId);
			List<DialogInputData> dialogInputsInSequence = dialogPackage.GetDialogInputsInSequence(sequenceId, contextData, new uint?(questId));
			this.dialogPackageManager.AddDialogInputBatch(game, dialogInputsInSequence, sequenceId);
		}
		if (bShowDialogs)
		{
			this.showDialogs = true;
		}
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0005D5D0 File Offset: 0x0005B7D0
	public void ProcessTrigger(ITrigger trigger, Game game)
	{
		if (!this.isActive)
		{
			return;
		}
		foreach (Quest quest in this.questList.Values)
		{
			if (!this.activatedDids.Contains(quest.Did) && !this.completedDids.Contains(quest.Did))
			{
				if (trigger != null && quest.StartConditions.Recalculate(game, trigger, null))
				{
					this.ProgressTowardsStartConditions(quest, game, quest.StartConditions.Dehydrate().MetIds);
				}
				ConditionResult conditionResult = quest.StartConditions.Examine();
				QuestDefinition questDefinition = this.GetQuestDefinition(quest.Did);
				if (questDefinition == null || questDefinition.MicroEventDID == null || game.microEventManager.IsMicroEventActive(questDefinition.MicroEventDID.Value))
				{
					if (conditionResult == ConditionResult.PASS)
					{
						this.ActivateQuest(quest, game);
						this.EnqueueDeferredAction(game, new QuestStartAction(quest));
					}
					else if (conditionResult == ConditionResult.FAIL)
					{
						this.FailQuest(quest);
					}
				}
			}
		}
		List<uint> list = new List<uint>(this.activatedDids);
		foreach (uint num in list)
		{
			if (this.activatedDids.Contains(num))
			{
				Quest quest2 = this.questList[num];
				ConditionResult conditionResult2 = ConditionResult.PASS;
				bool flag = false;
				foreach (ConditionState conditionState in quest2.EndConditions)
				{
					if (trigger != null && conditionState.Recalculate(game, trigger, null))
					{
						flag = true;
					}
					ConditionResult conditionResult3 = conditionState.Examine();
					if (conditionResult3 != ConditionResult.PASS && conditionResult2 != ConditionResult.FAIL)
					{
						conditionResult2 = conditionResult3;
					}
				}
				if (flag || conditionResult2 == ConditionResult.PASS)
				{
					ConditionalProgress conditionalProgress = ConditionState.DehydrateChunks(quest2.EndConditions);
					this.ProgressTowardsEndConditions(quest2, game, conditionalProgress.MetIds);
				}
				if (conditionResult2 == ConditionResult.PASS)
				{
					this.CompleteQuest(quest2, game);
				}
				else if (conditionResult2 == ConditionResult.FAIL)
				{
					this.FailQuest(quest2);
				}
				else if (trigger != null && quest2.Did == QuestDefinition.LastAutoQuestId && trigger.Type == "UpdateResource")
				{
					Session session = game.communityEventManager.GetSession();
					SBGUIStandardScreen sbguistandardScreen = null;
					if (session != null)
					{
						sbguistandardScreen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
						if (sbguistandardScreen != null)
						{
							session.AddAsyncResponse("standard_screen", sbguistandardScreen);
						}
					}
					if (sbguistandardScreen != null)
					{
						QuestDefinition questDefinition2 = this.GetQuestDefinition(quest2.Did);
						if (questDefinition2 != null)
						{
							List<QuestBookendInfo.ChunkConditions> list2 = new List<QuestBookendInfo.ChunkConditions>(questDefinition2.End.Chunks);
							list2.RemoveAt(list2.Count - 1);
							List<ConditionState> endConditions = quest2.EndConditions;
							int count = endConditions.Count;
							int num2 = 0;
							for (int i = 0; i < count; i++)
							{
								if (i - num2 >= list2.Count)
								{
									break;
								}
								if (endConditions[i].Examine() == ConditionResult.PASS)
								{
									list2.RemoveAt(i - num2);
									num2++;
								}
							}
							int count2 = list2.Count;
							if (count2 > 0)
							{
								Dictionary<string, object> data = trigger.Data;
								if (data.ContainsKey("resource_amounts"))
								{
									Dictionary<string, object> dictionary = (Dictionary<string, object>)data["resource_amounts"];
									bool flag2 = true;
									bool flag3 = false;
									for (int j = 0; j < count2; j++)
									{
										QuestBookendInfo.ChunkConditions chunkConditions = list2[j];
										Dictionary<string, object> dictionary2 = chunkConditions.Condition.ToDict();
										if (dictionary2.ContainsKey("count") && dictionary2.ContainsKey("resource_id"))
										{
											int num3 = TFUtils.LoadInt(dictionary2, "count");
											int key = TFUtils.LoadInt(dictionary2, "resource_id");
											int amount = game.resourceManager.Resources[key].Amount;
											if (dictionary.ContainsKey(key.ToString()))
											{
												flag3 = true;
											}
											if (amount < num3)
											{
												flag2 = false;
												break;
											}
										}
									}
									if (!flag2 || flag3)
									{
									}
								}
							}
						}
					}
				}
			}
		}
		if (this.deferredTriggers.Count > 0)
		{
			List<ITrigger> list3 = this.deferredTriggers;
			this.deferredTriggers = new List<ITrigger>();
			foreach (ITrigger trigger2 in list3)
			{
				game.triggerRouter.RouteTrigger(trigger2, game);
			}
		}
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x0005DB28 File Offset: 0x0005BD28
	public void HandleMicroEventClosedStatusChange(Game pGame, MicroEvent pMicroEvent)
	{
		if (pMicroEvent == null)
		{
			return;
		}
		int nDID = pMicroEvent.m_pMicroEventData.m_nDID;
		List<int> list = new List<int>();
		foreach (Quest quest in this.questList.Values)
		{
			if (!this.IsQuestCompleted(quest.Did))
			{
				QuestDefinition questDefinition = this.GetQuestDefinition(quest.Did);
				if (questDefinition != null && questDefinition.MicroEventDID != null && questDefinition.MicroEventDID.Value == nDID)
				{
					if (pMicroEvent.m_bIsClosed)
					{
						this.DeactivateQuest(pGame, quest.Did);
						if (!pMicroEvent.IsCompleted() && !list.Contains(questDefinition.MicroEventDID.Value))
						{
							this.QueueDialogSequences(1U, (uint)pMicroEvent.m_pMicroEventData.m_nCloseDialogSequenceDID, new List<Reward>(), 0f, 0U);
							list.Add(questDefinition.MicroEventDID.Value);
						}
					}
					else
					{
						this.activatedDids.Add(quest.Did);
						this.ProcessTrigger(null, pGame);
					}
				}
			}
		}
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0005DC8C File Offset: 0x0005BE8C
	public bool IsQuestActive(uint did)
	{
		return this.activatedDids.Contains(did);
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x0005DC9C File Offset: 0x0005BE9C
	public OrderedSet<uint> ActiveQuestDids
	{
		get
		{
			return this.activatedDids;
		}
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x0005DCA4 File Offset: 0x0005BEA4
	public bool QuestContainsPostponedDialog(int nQuestDID)
	{
		QuestManager.PostponedDialogParams[] array = this.postponed.ToArray();
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			QuestManager.PostponedDialogParams postponedDialogParams = array[i];
			if ((ulong)postponedDialogParams.questId == (ulong)((long)nQuestDID))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000EE4 RID: 3812 RVA: 0x0005DCE8 File Offset: 0x0005BEE8
	public OrderedSet<uint> ActiveQuestDidsNotInPostponed
	{
		get
		{
			QuestManager.PostponedDialogParams[] array = this.postponed.ToArray();
			int num = array.Length;
			OrderedSet<uint> orderedSet = new OrderedSet<uint>();
			foreach (uint num2 in this.ActiveQuestDids)
			{
				bool flag = false;
				for (int i = 0; i < num; i++)
				{
					QuestManager.PostponedDialogParams postponedDialogParams = array[i];
					if (num2 == postponedDialogParams.questId)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					orderedSet.Add(num2);
				}
			}
			return orderedSet;
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0005DDA4 File Offset: 0x0005BFA4
	public OrderedSet<uint> CompletedQuestDids
	{
		get
		{
			return this.completedDids;
		}
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x0005DDAC File Offset: 0x0005BFAC
	public int GetNumberOfActiveQuests()
	{
		return this.activatedDids.Count;
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06000EE7 RID: 3815 RVA: 0x0005DDBC File Offset: 0x0005BFBC
	public Dictionary<uint, QuestDefinition> QuestDefinitionList
	{
		get
		{
			return this.questDefinitionList;
		}
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0005DDC4 File Offset: 0x0005BFC4
	public void DebugCompleteAllQuests(Game game)
	{
		foreach (Quest quest in this.questList.Values)
		{
			if (!this.completedDids.Contains(quest.Did))
			{
				if (!this.IsQuestActive(quest.Did))
				{
					this.ProgressTowardsStartConditions(quest, game, quest.StartConditions.Dehydrate().MetIds);
					this.ActivateQuest(quest, game);
					game.Record(new QuestStartAction(quest));
				}
				this.CompleteQuest(quest, game);
			}
		}
		game.dialogPackageManager.ClearDialogs(game);
		this.postponed.Clear();
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x0005DE9C File Offset: 0x0005C09C
	private List<string> HandleFeatureUnlocks(Game game, QuestDefinition questDef)
	{
		List<string> list = new List<string>();
		foreach (string text in questDef.FeatureUnlocks)
		{
			if (!game.featureManager.CheckFeature(text))
			{
				list.Add(text);
				game.featureManager.UnlockFeature(text);
				game.featureManager.ActivateFeatureActions(game, text);
				if (text == "unrestrict_clicks" && game.tutorialLocked)
				{
					game.tutorialLocked = false;
					RestrictInteraction.RemoveWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
					RestrictInteraction.RemoveWhitelistExpansion(game.simulation, int.MinValue);
					RestrictInteraction.RemoveWhitelistSimulated(game.simulation, int.MinValue);
					if (game.simulation.FindSimulated(PlayHavenController.PAYMIUM_ITEM_DID) == null && game.inventory.HasItem(PlayHavenController.PAYMIUM_ITEM_DID))
					{
						game.playHavenController.RequestContent("end_tutorial_paymium_item_in_inventory");
					}
				}
				else if (text == "purchase_expansions" || text == "purchase_expansions_boardwalk")
				{
					game.terrain.UpdateRealtySigns(game.entities.DisplayControllerManager, new BillboardDelegate(SBCamera.BillboardDefinition), game);
				}
			}
		}
		return list;
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x0005E008 File Offset: 0x0005C208
	private List<int> HandleBuildingUnlocks(Game game, QuestDefinition questDef)
	{
		List<int> list = new List<int>();
		foreach (int num in questDef.BuildingUnlocks)
		{
			if (!game.buildingUnlockManager.CheckBuildingUnlock(num))
			{
				list.Add(num);
				game.buildingUnlockManager.UnlockBuilding(num);
			}
		}
		return list;
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0005E094 File Offset: 0x0005C294
	private List<int> HandleCostumeUnlocks(Game game, QuestDefinition questDef)
	{
		List<int> list = new List<int>();
		foreach (int num in questDef.CostumeUnlocks)
		{
			if (!game.costumeManager.IsCostumeUnlocked(num))
			{
				list.Add(num);
				game.costumeManager.UnlockCostume(num);
			}
		}
		return list;
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0005E120 File Offset: 0x0005C320
	private void UpdateQuestLineProgress(Quest quest)
	{
		QuestDefinition questDefinition = this.questDefinitionList[quest.Did];
		if (questDefinition.QuestLine != null && questDefinition.QuestLine.HasProgress)
		{
			Vector2 vector = this.questLineProgress[questDefinition.QuestLine.Name];
			vector += Vector2.right;
			this.questLineProgress[questDefinition.QuestLine.Name] = vector;
		}
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x0005E194 File Offset: 0x0005C394
	private void EnqueueDeferredAction(Game game, PersistedTriggerableAction action)
	{
		action.Process(game);
		game.Record(action);
		this.deferredTriggers.Add(action.CreateTrigger(new Dictionary<string, object>()));
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x0005E1C8 File Offset: 0x0005C3C8
	public string GetStoreTabValue()
	{
		List<uint> list = new List<uint>(this.activatedDids);
		foreach (uint num in list)
		{
			if (this.activatedDids.Contains(num))
			{
				QuestDefinition questDefinition = this.questDefinitionList[num];
				if (!string.IsNullOrEmpty(questDefinition.StoreTab))
				{
					return questDefinition.StoreTab;
				}
			}
		}
		return null;
	}

	// Token: 0x040009DC RID: 2524
	public ulong m_uQuestTimeGap = 600UL;

	// Token: 0x040009DD RID: 2525
	public ulong? m_uQuestCompletionTimestamp;

	// Token: 0x040009DE RID: 2526
	public ulong? m_uAutoQuestStartTime;

	// Token: 0x040009DF RID: 2527
	public ulong m_uTimeTillResetQuest = 86400UL;

	// Token: 0x040009E0 RID: 2528
	public ulong m_uCurrentTime;

	// Token: 0x040009E1 RID: 2529
	public int m_autoQuestCount;

	// Token: 0x040009E2 RID: 2530
	private static readonly string QUESTS_PATH = "Quests";

	// Token: 0x040009E3 RID: 2531
	private bool showDialogs;

	// Token: 0x040009E4 RID: 2532
	private bool isActive;

	// Token: 0x040009E5 RID: 2533
	private DialogPackageManager dialogPackageManager;

	// Token: 0x040009E6 RID: 2534
	private List<ITrigger> deferredTriggers = new List<ITrigger>();

	// Token: 0x040009E7 RID: 2535
	private Dictionary<uint, Quest> questList;

	// Token: 0x040009E8 RID: 2536
	private Dictionary<uint, QuestDefinition> questDefinitionList;

	// Token: 0x040009E9 RID: 2537
	private Dictionary<uint, QuestTemplate> randomQuestTemplateList;

	// Token: 0x040009EA RID: 2538
	private OrderedSet<uint> activatedDids;

	// Token: 0x040009EB RID: 2539
	private OrderedSet<uint> completedDids;

	// Token: 0x040009EC RID: 2540
	private OrderedSet<uint> deactivatedCompletedDids;

	// Token: 0x040009ED RID: 2541
	private Action onShowDialogCallback;

	// Token: 0x040009EE RID: 2542
	private Dictionary<string, Vector2> questLineProgress;

	// Token: 0x040009EF RID: 2543
	private Queue<QuestManager.PostponedDialogParams> postponed = new Queue<QuestManager.PostponedDialogParams>();

	// Token: 0x020001B6 RID: 438
	private class PostponedDialogParams
	{
		// Token: 0x040009F1 RID: 2545
		public uint packageId;

		// Token: 0x040009F2 RID: 2546
		public uint sequenceId;

		// Token: 0x040009F3 RID: 2547
		public List<Reward> rewards;

		// Token: 0x040009F4 RID: 2548
		public DateTime complete;

		// Token: 0x040009F5 RID: 2549
		public uint questId;
	}
}
