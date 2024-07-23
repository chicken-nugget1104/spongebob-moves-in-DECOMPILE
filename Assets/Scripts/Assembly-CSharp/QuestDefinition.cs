using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B3 RID: 435
public class QuestDefinition
{
	// Token: 0x06000E88 RID: 3720 RVA: 0x0005965C File Offset: 0x0005785C
	private QuestDefinition()
	{
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0005967C File Offset: 0x0005787C
	public static string GenerateSessionActionId(uint did)
	{
		return "QuestTracker_" + did;
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000E8B RID: 3723 RVA: 0x00059690 File Offset: 0x00057890
	public uint Did
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000E8C RID: 3724 RVA: 0x00059698 File Offset: 0x00057898
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000E8D RID: 3725 RVA: 0x000596A0 File Offset: 0x000578A0
	public bool Chunk
	{
		get
		{
			return this.chunk;
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000E8E RID: 3726 RVA: 0x000596A8 File Offset: 0x000578A8
	public string Tag
	{
		get
		{
			return this.tag;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000E8F RID: 3727 RVA: 0x000596B0 File Offset: 0x000578B0
	public string Icon
	{
		get
		{
			return this.icon;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000E90 RID: 3728 RVA: 0x000596B8 File Offset: 0x000578B8
	public string DialogHeading
	{
		get
		{
			return this.dialogHeading;
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06000E91 RID: 3729 RVA: 0x000596C0 File Offset: 0x000578C0
	public string StoreTab
	{
		get
		{
			return this.storeTab;
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000E92 RID: 3730 RVA: 0x000596C8 File Offset: 0x000578C8
	public string DialogBody
	{
		get
		{
			return this.dialogBody;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000E93 RID: 3731 RVA: 0x000596D0 File Offset: 0x000578D0
	public string Portrait
	{
		get
		{
			return this.portrait;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000E94 RID: 3732 RVA: 0x000596D8 File Offset: 0x000578D8
	public uint DialogPackageDid
	{
		get
		{
			return this.dialogPackageDid;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000E95 RID: 3733 RVA: 0x000596E0 File Offset: 0x000578E0
	public int AutoQuestID
	{
		get
		{
			return this.autoQuestID;
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000E96 RID: 3734 RVA: 0x000596E8 File Offset: 0x000578E8
	public int AutoQuestCharacterID
	{
		get
		{
			return this.autoQuestCharacterID;
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000E97 RID: 3735 RVA: 0x000596F0 File Offset: 0x000578F0
	public int? MicroEventDID
	{
		get
		{
			return this.microEventDID;
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000E98 RID: 3736 RVA: 0x000596F8 File Offset: 0x000578F8
	public QuestBookendInfo Start
	{
		get
		{
			return this.start;
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06000E99 RID: 3737 RVA: 0x00059700 File Offset: 0x00057900
	public QuestBookendInfo End
	{
		get
		{
			return this.end;
		}
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06000E9A RID: 3738 RVA: 0x00059708 File Offset: 0x00057908
	public Reward Reward
	{
		get
		{
			return this.rewardDefinition.Summary;
		}
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x06000E9B RID: 3739 RVA: 0x00059718 File Offset: 0x00057918
	public bool HasFeatureUnlocks
	{
		get
		{
			return this.featureUnlocks.Count > 0;
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06000E9C RID: 3740 RVA: 0x00059728 File Offset: 0x00057928
	public bool HasBuildingUnlocks
	{
		get
		{
			return this.buildingUnlocks.Count > 0;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x06000E9D RID: 3741 RVA: 0x00059738 File Offset: 0x00057938
	public bool HasCostumeUnlocks
	{
		get
		{
			return this.costumeUnlocks.Count > 0;
		}
	}

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x06000E9E RID: 3742 RVA: 0x00059748 File Offset: 0x00057948
	public List<string> FeatureUnlocks
	{
		get
		{
			return TFUtils.CloneAndCastList<string, string>(this.featureUnlocks);
		}
	}

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x06000E9F RID: 3743 RVA: 0x00059758 File Offset: 0x00057958
	public List<int> BuildingUnlocks
	{
		get
		{
			return TFUtils.CloneAndCastList<int, int>(this.buildingUnlocks);
		}
	}

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x00059768 File Offset: 0x00057968
	public List<int> CostumeUnlocks
	{
		get
		{
			return TFUtils.CloneAndCastList<int, int>(this.costumeUnlocks);
		}
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x00059778 File Offset: 0x00057978
	// (set) Token: 0x06000EA2 RID: 3746 RVA: 0x00059780 File Offset: 0x00057980
	public string CollectStart
	{
		get
		{
			return this.collectStart;
		}
		set
		{
			this.collectStart = value;
		}
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x0005978C File Offset: 0x0005798C
	// (set) Token: 0x06000EA4 RID: 3748 RVA: 0x00059794 File Offset: 0x00057994
	public string CollectComplete
	{
		get
		{
			return this.collectComplete;
		}
		set
		{
			this.collectComplete = value;
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x000597A0 File Offset: 0x000579A0
	public SessionActionDefinition SessionActions
	{
		get
		{
			return this.sessionActions;
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x000597A8 File Offset: 0x000579A8
	public SessionActionDefinition PostSessionActions
	{
		get
		{
			return this.postSessionActions;
		}
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x000597B0 File Offset: 0x000579B0
	public QuestLineInfo QuestLine
	{
		get
		{
			return this.questLine;
		}
	}

	// Token: 0x1700020C RID: 524
	// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x000597B8 File Offset: 0x000579B8
	public string Branch
	{
		get
		{
			return this.branch;
		}
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x000597C0 File Offset: 0x000579C0
	public Dictionary<string, object> ToDict(bool bForceRandomQuestTrigger)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["did"] = this.did;
		dictionary["name"] = this.name;
		dictionary["chunk"] = this.chunk;
		dictionary["icon"] = this.icon;
		dictionary["dialog_heading"] = this.dialogHeading;
		dictionary["dialog_body"] = this.dialogBody;
		dictionary["portrait"] = this.portrait;
		dictionary["tag"] = this.tag;
		dictionary["start"] = this.start.ToDict();
		dictionary["end"] = this.end.ToDict();
		dictionary["dialog_package_did"] = this.dialogPackageDid;
		dictionary["branch"] = this.branch;
		if (!string.IsNullOrEmpty(this.storeTab))
		{
			dictionary["store_tab"] = this.storeTab;
		}
		if (this.microEventDID != null)
		{
			dictionary["micro_event_id"] = this.microEventDID.Value;
		}
		if (this.questLine != null)
		{
			dictionary["quest_line"] = this.questLine.ToDict();
		}
		if (this.sessionActions != null)
		{
			dictionary["session_actions"] = this.sessionActions.ToDict();
		}
		if (this.postSessionActions != null)
		{
			dictionary["post_session_actions"] = this.postSessionActions.ToDict();
		}
		if (this.rewardDefinition != null)
		{
			dictionary["reward"] = this.rewardDefinition.ToDict();
		}
		if (this.CollectStart != null)
		{
			dictionary["CollectStart"] = this.CollectStart;
		}
		if (this.CollectComplete != null)
		{
			dictionary["CollectComplete"] = this.CollectComplete;
		}
		if (this.autoQuestID >= 0)
		{
			dictionary["auto_quest_id"] = this.autoQuestID;
		}
		if (this.autoQuestCharacterID >= 0)
		{
			dictionary["auto_quest_char_id"] = this.autoQuestCharacterID;
		}
		return dictionary;
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x00059A08 File Offset: 0x00057C08
	public static QuestDefinition FromDict(Dictionary<string, object> data)
	{
		string text = TFUtils.TryLoadString(data, "type");
		QuestDefinition questDefinition = new QuestDefinition();
		if (text == null)
		{
		}
		uint num = TFUtils.LoadUint(data, "did");
		string text2 = TFUtils.LoadString(data, "name");
		string text3 = string.Empty;
		string text4 = string.Empty;
		string text5 = TFUtils.TryLoadString(data, "tag");
		string text6 = TFUtils.TryLoadString(data, "store_tab");
		if (data.ContainsKey("branch"))
		{
			text4 = TFUtils.LoadString(data, "branch");
		}
		if (text5 == null)
		{
			text5 = "misc_quest";
		}
		if (data.ContainsKey("icon"))
		{
			text3 = TFUtils.LoadString(data, "icon");
		}
		else
		{
			TFUtils.Assert(false, "QuestDid " + num + " does not have an icon.");
		}
		int? num2 = null;
		if (data.ContainsKey("micro_event_id"))
		{
			num2 = TFUtils.TryLoadNullableInt(data, "micro_event_id");
		}
		int num3 = -1;
		if (data.ContainsKey("auto_quest_id"))
		{
			num3 = TFUtils.LoadInt(data, "auto_quest_id");
		}
		int num4 = -1;
		if (data.ContainsKey("auto_quest_char_id"))
		{
			num4 = TFUtils.LoadInt(data, "auto_quest_char_id");
		}
		string text7 = string.Empty;
		string text8 = string.Empty;
		string text9 = string.Empty;
		bool chunkQuest = false;
		if (data.ContainsKey("chunk"))
		{
			chunkQuest = TFUtils.LoadBool(data, "chunk");
			text8 = TFUtils.LoadString(data, "dialog_body");
			text9 = TFUtils.LoadString(data, "portrait");
			if (data.ContainsKey("dialog_heading"))
			{
				text7 = TFUtils.LoadString(data, "dialog_heading");
			}
		}
		questDefinition.CollectStart = TFUtils.TryLoadString(data, "CollectStart");
		questDefinition.CollectComplete = TFUtils.TryLoadString(data, "CollectComplete");
		List<string> list = (!data.ContainsKey("feature_unlocks")) ? new List<string>() : TFUtils.LoadList<string>(data, "feature_unlocks");
		List<int> list2 = (!data.ContainsKey("building_unlocks")) ? new List<int>() : TFUtils.LoadList<int>(data, "building_unlocks");
		List<int> list3 = (!data.ContainsKey("costume_unlocks")) ? new List<int>() : TFUtils.LoadList<int>(data, "costume_unlocks");
		uint dialogPackageId = TFUtils.LoadUint(data, "dialog_package_did");
		bool autoQuest = num >= 500001U && num <= 600000U;
		QuestBookendInfo questBookendInfo = QuestBookendInfo.FromDict(TFUtils.LoadDict(data, "start"), false, autoQuest);
		QuestBookendInfo questBookendInfo2 = QuestBookendInfo.FromDict(TFUtils.LoadDict(data, "end"), chunkQuest, autoQuest);
		QuestLineInfo questLineInfo = null;
		if (data.ContainsKey("quest_line"))
		{
			questLineInfo = QuestLineInfo.FromDict(TFUtils.LoadDict(data, "quest_line"));
		}
		RewardDefinition rewardDefinition = RewardDefinition.FromObject(data["reward"]);
		ICondition startingConditions;
		if (questBookendInfo.DialogSequenceId != null)
		{
			startingConditions = new CompleteDialogCondition(0U, questBookendInfo.DialogSequenceId.Value);
		}
		else
		{
			startingConditions = new ConstantCondition(0U, true);
		}
		SessionActionDefinition sessionActionDefinition = null;
		if (data.ContainsKey("session_actions"))
		{
			sessionActionDefinition = SessionActionFactory.Create((Dictionary<string, object>)data["session_actions"], startingConditions, num, 0U);
		}
		SessionActionDefinition sessionActionDefinition2 = null;
		if (data.ContainsKey("post_session_actions"))
		{
			sessionActionDefinition2 = SessionActionFactory.Create((Dictionary<string, object>)data["post_session_actions"], new ConstantCondition(0U, true), num, 0U);
		}
		questDefinition.Initialize(num, text2, chunkQuest, text5, text3, text7, text8, text9, text4, dialogPackageId, num3, num4, num2, questBookendInfo, questBookendInfo2, questLineInfo, sessionActionDefinition, sessionActionDefinition2, rewardDefinition, list, list2, list3, text6);
		TFUtils.DebugLog(string.Concat(new object[]
		{
			"Loaded Quest ",
			questDefinition.did,
			" (",
			questDefinition.name,
			")"
		}), TFUtils.LogFilter.Quests);
		return questDefinition;
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00059DE0 File Offset: 0x00057FE0
	public static Resource GetRandomRecipe(Game game)
	{
		List<int> list = game.resourceManager.ConsumableProducts(game.craftManager);
		HashSet<int> jellyBasedRecipesCopy = game.craftManager.JellyBasedRecipesCopy;
		foreach (int item in jellyBasedRecipesCopy)
		{
			if (list.Contains(item))
			{
				list.Remove(item);
			}
		}
		HashSet<int> ignoreRandomQuestRecipesCopy = game.craftManager.IgnoreRandomQuestRecipesCopy;
		foreach (int item2 in ignoreRandomQuestRecipesCopy)
		{
			if (list.Contains(item2))
			{
				list.Remove(item2);
			}
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		int key = list[index];
		return game.resourceManager.Resources[key];
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x00059F08 File Offset: 0x00058108
	public static string ParseResourceFieldString(Resource resource, string field)
	{
		if (field != null)
		{
			if (QuestDefinition.<>f__switch$mapF == null)
			{
				QuestDefinition.<>f__switch$mapF = new Dictionary<string, int>(2)
				{
					{
						"Name",
						0
					},
					{
						"Texture",
						1
					}
				};
			}
			int num;
			if (QuestDefinition.<>f__switch$mapF.TryGetValue(field, out num))
			{
				if (num == 0)
				{
					return resource.Name;
				}
				if (num == 1)
				{
					return resource.GetResourceTexture();
				}
			}
		}
		TFUtils.ErrorLog("Random resource does not support this field yet");
		return null;
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x00059F90 File Offset: 0x00058190
	public static int? ParseResourceFieldInt(Resource resource, string field)
	{
		if (field != null)
		{
			if (QuestDefinition.<>f__switch$map10 == null)
			{
				QuestDefinition.<>f__switch$map10 = new Dictionary<string, int>(1)
				{
					{
						"Did",
						0
					}
				};
			}
			int num;
			if (QuestDefinition.<>f__switch$map10.TryGetValue(field, out num))
			{
				if (num == 0)
				{
					return new int?(resource.Did);
				}
			}
		}
		TFUtils.ErrorLog("Random resource does not support this field yet");
		return null;
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x0005A00C File Offset: 0x0005820C
	public static QuestDefinition ParseAutoQuest(AutoQuest pAutoQuest, Game pGame)
	{
		if (pAutoQuest == null || pGame == null)
		{
			return null;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "quests");
		dictionary.Add("did", QuestDefinition.LastAutoQuestId);
		dictionary.Add("name", pAutoQuest.m_sName);
		dictionary.Add("dialog_heading", pAutoQuest.m_sName);
		dictionary.Add("dialog_body", pAutoQuest.m_sDescription);
		dictionary.Add("tag", "auto_quest_" + pAutoQuest.m_nDID.ToString());
		dictionary.Add("dialog_package_did", 1);
		dictionary.Add("chunk", true);
		dictionary.Add("auto_quest_id", pAutoQuest.m_nDID);
		dictionary.Add("auto_quest_char_id", pAutoQuest.m_nCharacterDID);
		dictionary.Add("portrait", "mrkrabsportrait_moneygrubbing.png");
		dictionary.Add("CollectStart", "Missing Dialog Text");
		dictionary.Add("CollectComplete", "Missing Dialog Text");
		dictionary.Add("icon", "sb_item_peanutbrittle.png");
		Simulated simulated = pGame.simulation.FindSimulated(new int?(pAutoQuest.m_nCharacterDID));
		if (simulated != null && simulated.HasEntity<ResidentEntity>())
		{
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			if (entity.DialogPortrait != null)
			{
				dictionary["portrait"] = entity.DialogPortrait;
			}
			AutoQuestData.DialogData dialogData = pGame.autoQuestDatabase.GetDialogData(pAutoQuest.m_nDID, entity.AutoQuestIntro);
			if (dialogData != null)
			{
				dictionary["CollectStart"] = dialogData.m_sIntroDialog;
			}
			dialogData = pGame.autoQuestDatabase.GetDialogData(pAutoQuest.m_nDID, entity.AutoQuestOutro);
			if (dialogData != null)
			{
				dictionary["CollectComplete"] = dialogData.m_sOutroDialog;
			}
			if (entity.QuestReminderIcon != null)
			{
				dictionary["icon"] = entity.QuestReminderIcon;
			}
		}
		dictionary.Add("start", new Dictionary<string, object>
		{
			{
				"conditions",
				new Dictionary<string, object>
				{
					{
						"id",
						1
					},
					{
						"type",
						"complete_quest"
					},
					{
						"quest_id",
						0
					}
				}
			},
			{
				"postpone",
				0
			},
			{
				"dialog_sequence_id",
				10002
			}
		});
		List<object> list = new List<object>();
		int num = 1;
		foreach (KeyValuePair<int, int> keyValuePair in pAutoQuest.m_pRecipes)
		{
			list.Add(new Dictionary<string, object>
			{
				{
					"conditions",
					new Dictionary<string, object>
					{
						{
							"id",
							num
						},
						{
							"type",
							"auto_quest_craft_collect"
						},
						{
							"resource_id",
							keyValuePair.Key
						},
						{
							"count",
							keyValuePair.Value
						}
					}
				},
				{
					"name",
					pGame.resourceManager.Resources[keyValuePair.Key].Name
				},
				{
					"icon",
					pGame.resourceManager.Resources[keyValuePair.Key].GetResourceTexture()
				}
			});
			num++;
		}
		list.Add(new Dictionary<string, object>
		{
			{
				"conditions",
				new Dictionary<string, object>
				{
					{
						"id",
						num
					},
					{
						"type",
						"auto_quest_all_done"
					},
					{
						"quest_id",
						QuestDefinition.LastAutoQuestId
					}
				}
			},
			{
				"name",
				string.Empty
			},
			{
				"icon",
				string.Empty
			}
		});
		dictionary.Add("end", new Dictionary<string, object>
		{
			{
				"array",
				list
			},
			{
				"dialog_sequence_id",
				10003
			}
		});
		dictionary.Add("reward", new Dictionary<string, object>
		{
			{
				"resources",
				new Dictionary<string, object>
				{
					{
						"3",
						pAutoQuest.m_nGoldReward
					},
					{
						"5",
						pAutoQuest.m_nXPReward
					}
				}
			},
			{
				"thought_icon",
				null
			}
		});
		return QuestDefinition.FromDict(dictionary);
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x0005A4D0 File Offset: 0x000586D0
	public static QuestDefinition ParseRandomTemplate(QuestTemplate template, Game game)
	{
		Dictionary<string, object> templateData = template.TemplateData;
		string text = (string)templateData["icon"];
		Dictionary<string, object> dictionary = (Dictionary<string, object>)templateData["variables"];
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)templateData["end"];
		Dictionary<string, object> dictionary3 = (Dictionary<string, object>)dictionary2["conditions"];
		Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
		Dictionary<string, object> o = (Dictionary<string, object>)templateData["reward"];
		Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
		foreach (string key in dictionary.Keys)
		{
			Dictionary<string, object> dictionary6 = (Dictionary<string, object>)dictionary[key];
			string text2 = (string)dictionary6["type"];
			string text3 = text2;
			switch (text3)
			{
			case "RandomRecipe":
				dictionary5.Add(key, QuestDefinition.GetRandomRecipe(game));
				continue;
			case "RandomInt":
			{
				int min = int.Parse(dictionary6["min"].ToString());
				int max = 0;
				string a = (string)dictionary6["max"];
				if (a == "$Level")
				{
					max = game.resourceManager.Resources[ResourceManager.LEVEL].Amount;
				}
				else
				{
					TFUtils.ErrorLog("RandomInt only supports playerlevel for max right now");
				}
				dictionary5.Add(key, UnityEngine.Random.Range(min, max));
				continue;
			}
			case "string":
				dictionary5.Add(key, dictionary6["value"] as string);
				continue;
			case "int":
				dictionary5.Add(key, int.Parse(dictionary6["value"].ToString()));
				continue;
			case "playerLevel":
				dictionary5.Add(key, game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				continue;
			}
			TFUtils.ErrorLog("This random template variable type is not implemented yet!");
		}
		string value = string.Empty;
		bool chunkQuest = false;
		if (templateData.ContainsKey("chunk"))
		{
			chunkQuest = TFUtils.LoadBool(templateData, "chunk");
		}
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string value2 = (string)text.Clone();
		string key2 = string.Empty;
		string field = string.Empty;
		string empty4 = string.Empty;
		string text4 = null;
		if (text.StartsWith("$"))
		{
			key2 = text.Substring(0, text.IndexOf('.'));
			field = text.Substring(text.IndexOf('.') + 1);
			if (dictionary5.ContainsKey(key2))
			{
				value2 = QuestDefinition.ParseResourceFieldString((Resource)dictionary5[key2], field);
			}
		}
		foreach (string key3 in dictionary3.Keys)
		{
			dictionary4.Add(key3, dictionary3[key3]);
		}
		string text5 = dictionary3["resource_id"] as string;
		key2 = text5.Substring(0, text5.IndexOf('.'));
		field = text5.Substring(text5.IndexOf('.') + 1);
		if (dictionary5.ContainsKey(key2))
		{
			dictionary4["resource_id"] = QuestDefinition.ParseResourceFieldInt((Resource)dictionary5[key2], field);
		}
		text5 = (string)dictionary3["count"];
		if (dictionary5.ContainsKey(text5))
		{
			dictionary4["count"] = int.Parse(dictionary5[text5].ToString());
		}
		QuestDefinition questDefinition = new QuestDefinition();
		questDefinition.CollectStart = (dictionary5["$CollectStart"] as string);
		questDefinition.CollectComplete = (dictionary5["$CollectComplete"] as string);
		uint lastRandomQuestId = QuestDefinition.LastRandomQuestId;
		string text6 = "misc_quest";
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		uint dialogPackageId = 1U;
		Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
		dictionary7["id"] = 1;
		dictionary7["type"] = "complete_quest";
		dictionary7["quest_id"] = 0;
		Dictionary<string, object> dictionary8 = new Dictionary<string, object>();
		dictionary8["conditions"] = dictionary7;
		dictionary8["dialog_sequence_id"] = 10000;
		QuestDefinition.StartInputPrompts[lastRandomQuestId] = new Dictionary<string, object>();
		QuestDefinition.CompleteInputPrompts[lastRandomQuestId] = new Dictionary<string, object>();
		QuestDefinition.StartInputPrompts[lastRandomQuestId]["type"] = "quest_start";
		QuestDefinition.CompleteInputPrompts[lastRandomQuestId]["type"] = "quest_complete";
		if ((string)dictionary4["type"] == "craft_collect")
		{
			int key4 = int.Parse(dictionary4["resource_id"].ToString());
			int num2 = int.Parse(dictionary4["count"].ToString());
			string name_Plural = game.resourceManager.Resources[key4].Name;
			if (num2 > 1)
			{
				name_Plural = game.resourceManager.Resources[key4].Name_Plural;
			}
			value = string.Format(Language.Get((string)dictionary5["$CollectStart"]), num2.ToString(), name_Plural);
			QuestDefinition.StartInputPrompts[lastRandomQuestId]["title"] = value;
			QuestDefinition.StartInputPrompts[lastRandomQuestId]["icon"] = value2;
			QuestDefinition.CompleteInputPrompts[lastRandomQuestId]["title"] = string.Format(Language.Get((string)dictionary5["$CollectComplete"]), num2.ToString(), name_Plural);
			QuestDefinition.CompleteInputPrompts[lastRandomQuestId]["icon"] = value2;
		}
		else
		{
			TFUtils.ErrorLog("Random Quest does not support this endCondition type yet");
		}
		Dictionary<string, object> dictionary9 = new Dictionary<string, object>();
		dictionary9["conditions"] = dictionary4;
		dictionary9["dialog_sequence_id"] = 10001;
		QuestBookendInfo questBookendInfo = QuestBookendInfo.FromDict(dictionary8, false, false);
		QuestBookendInfo questBookendInfo2 = QuestBookendInfo.FromDict(dictionary9, chunkQuest, false);
		RewardDefinition rewardDefinition = RewardDefinition.FromObject(o);
		SessionActionDefinition sessionActionDefinition = null;
		Dictionary<string, object> dictionary10 = new Dictionary<string, object>();
		dictionary10["type"] = "trigger_random_quest";
		List<object> list4 = new List<object>();
		list4.Add(dictionary10);
		Dictionary<string, object> dictionary11 = new Dictionary<string, object>();
		dictionary11["type"] = "array";
		dictionary11["actions"] = list4;
		SessionActionDefinition sessionActionDefinition2 = null;
		questDefinition.Initialize(lastRandomQuestId, value, chunkQuest, text6, value2, empty, empty2, empty3, empty4, dialogPackageId, -1, -1, null, questBookendInfo, questBookendInfo2, null, sessionActionDefinition, sessionActionDefinition2, rewardDefinition, list, list2, list3, text4);
		return questDefinition;
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x0005AC80 File Offset: 0x00058E80
	public static QuestDefinition CreateRandom(QuestManager questManager, Game game)
	{
		QuestTemplate randomQuestTemplate = questManager.GetRandomQuestTemplate();
		return QuestDefinition.ParseRandomTemplate(randomQuestTemplate, game);
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x0005ACA0 File Offset: 0x00058EA0
	public static QuestDefinition CreateAuto(Game pGame)
	{
		AutoQuest autoQuest = pGame.autoQuestDatabase.GenerateNextAutoQuest(pGame);
		if (autoQuest == null)
		{
			return null;
		}
		return QuestDefinition.ParseAutoQuest(autoQuest, pGame);
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x0005ACCC File Offset: 0x00058ECC
	public static QuestDialogInputData RecreateRandomQuestStartInputData(Game game, uint target)
	{
		QuestDefinition questDefinition = game.questManager.GetQuestDefinition(target);
		Dictionary<string, object> dictionary = questDefinition.end.Chunks[0].Condition.ToDict();
		QuestDefinition.StartInputPrompts[target] = new Dictionary<string, object>();
		QuestDefinition.StartInputPrompts[target]["type"] = "quest_start";
		if ((string)dictionary["type"] == "craft_collect")
		{
			int key = TFUtils.LoadInt(dictionary, "resource_id");
			int num = TFUtils.LoadInt(dictionary, "count");
			string resourceTexture = game.resourceManager.Resources[key].GetResourceTexture();
			QuestDefinition.StartInputPrompts[target]["title"] = string.Format(Language.Get(questDefinition.CollectStart), num.ToString(), game.resourceManager.Resources[key].Name);
			QuestDefinition.StartInputPrompts[target]["icon"] = resourceTexture;
		}
		List<object> value = new List<object>
		{
			questDefinition.Reward.ToDict()
		};
		Dictionary<string, object> contextData = new Dictionary<string, object>
		{
			{
				"rewards",
				value
			}
		};
		return new QuestStartDialogInputData(10000U, QuestDefinition.StartInputPrompts[target], contextData, new uint?(target));
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x0005AE2C File Offset: 0x0005902C
	public static QuestDialogInputData RecreateRandomQuestCompleteInputData(Game game, uint target)
	{
		QuestDefinition questDefinition = game.questManager.GetQuestDefinition(target);
		Dictionary<string, object> dictionary = questDefinition.end.Chunks[0].Condition.ToDict();
		QuestDefinition.CompleteInputPrompts[target] = new Dictionary<string, object>();
		QuestDefinition.CompleteInputPrompts[target]["type"] = "quest_complete";
		if ((string)dictionary["type"] == "craft_collect")
		{
			int key = TFUtils.LoadInt(dictionary, "resource_id");
			int num = TFUtils.LoadInt(dictionary, "count");
			string resourceTexture = game.resourceManager.Resources[key].GetResourceTexture();
			QuestDefinition.CompleteInputPrompts[target]["title"] = string.Format(Language.Get(questDefinition.CollectComplete), num.ToString(), game.resourceManager.Resources[key].Name);
			QuestDefinition.CompleteInputPrompts[target]["icon"] = resourceTexture;
		}
		List<object> value;
		if (questDefinition.Reward != null)
		{
			value = new List<object>
			{
				questDefinition.Reward.ToDict()
			};
		}
		else
		{
			value = new List<object>();
		}
		Dictionary<string, object> contextData = new Dictionary<string, object>
		{
			{
				"rewards",
				value
			}
		};
		return new QuestCompleteDialogInputData(10000U, QuestDefinition.CompleteInputPrompts[target], contextData, new uint?(target));
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x0005AFA8 File Offset: 0x000591A8
	public static CharacterDialogInputData RecreateAutoQuestIntroInputData(Game pGame, uint uTarget)
	{
		QuestDefinition questDefinition = pGame.questManager.GetQuestDefinition(uTarget);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "character");
		dictionary.Add("character_icon", questDefinition.Portrait);
		dictionary.Add("text", questDefinition.CollectStart);
		if (QuestDefinition.StartInputPrompts.ContainsKey(uTarget))
		{
			QuestDefinition.StartInputPrompts[uTarget] = dictionary;
		}
		else
		{
			QuestDefinition.StartInputPrompts.Add(uTarget, dictionary);
		}
		return new CharacterDialogInputData(10002U, dictionary);
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x0005B034 File Offset: 0x00059234
	public static CharacterDialogInputData RecreateAutoQuestOutroInputData(Game pGame, uint uTarget)
	{
		QuestDefinition questDefinition = pGame.questManager.GetQuestDefinition(uTarget);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "character");
		dictionary.Add("character_icon", questDefinition.Portrait);
		dictionary.Add("text", questDefinition.CollectComplete);
		if (QuestDefinition.CompleteInputPrompts.ContainsKey(uTarget))
		{
			QuestDefinition.CompleteInputPrompts[uTarget] = dictionary;
		}
		else
		{
			QuestDefinition.CompleteInputPrompts.Add(uTarget, dictionary);
		}
		return new CharacterDialogInputData(10003U, dictionary);
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x0005B0C0 File Offset: 0x000592C0
	public void Initialize(uint id, string name, bool chunk, string tag, string icon, string dialogHeading, string dialogBody, string portrait, string branch, uint dialogPackageId, int autoQuestID, int autoQuestCharacterID, int? microEventDID, QuestBookendInfo start, QuestBookendInfo end, QuestLineInfo questLine, SessionActionDefinition sessionActions, SessionActionDefinition postSessionActions, RewardDefinition rewardDefinition, List<string> featureUnlocks, List<int> buildingUnlocks, List<int> costumeUnlocks, string storeTab)
	{
		this.did = id;
		this.start = start;
		this.end = end;
		this.name = name;
		this.chunk = chunk;
		this.tag = tag;
		this.icon = icon;
		this.dialogHeading = dialogHeading;
		this.dialogBody = dialogBody;
		this.portrait = portrait;
		this.branch = branch;
		this.dialogPackageDid = dialogPackageId;
		this.sessionActions = sessionActions;
		this.postSessionActions = postSessionActions;
		this.rewardDefinition = rewardDefinition;
		this.featureUnlocks = featureUnlocks;
		this.buildingUnlocks = buildingUnlocks;
		this.costumeUnlocks = costumeUnlocks;
		this.questLine = questLine;
		this.autoQuestID = autoQuestID;
		this.autoQuestCharacterID = autoQuestCharacterID;
		this.microEventDID = microEventDID;
		this.storeTab = storeTab;
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x0005B184 File Offset: 0x00059384
	public override string ToString()
	{
		string text = "QuestDefinition(";
		text = text + "did=" + this.did;
		text = text + ", name=" + this.name;
		text = text + ", tag=" + this.tag;
		text = text + ", branch=" + this.branch;
		text += ", startingConditions=[";
		foreach (QuestBookendInfo.ChunkConditions chunkConditions in this.start.Chunks)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"( ",
				chunkConditions.Condition.Description(null),
				", ",
				chunkConditions.Name,
				", ",
				chunkConditions.Icon,
				" ), "
			});
		}
		text += "], endingConditions=[";
		foreach (QuestBookendInfo.ChunkConditions chunkConditions2 in this.end.Chunks)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"( ",
				chunkConditions2.Condition.Description(null),
				", ",
				chunkConditions2.Name,
				", ",
				chunkConditions2.Icon,
				" ), "
			});
		}
		text = text + "], sessionActions=" + ((this.sessionActions != null) ? this.sessionActions.ToString() : "null");
		text = text + ", postSessionActions=" + ((this.postSessionActions != null) ? this.postSessionActions.ToString() : "null");
		text += ")";
		return text;
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x0005B3B8 File Offset: 0x000595B8
	public Reward GenerateReward(Simulation simulation)
	{
		return this.rewardDefinition.GenerateReward(simulation, true);
	}

	// Token: 0x04000995 RID: 2453
	private const string DEFAULT_QUEST_TAG = "misc_quest";

	// Token: 0x04000996 RID: 2454
	public const uint RANDOM_QUEST_ID_START = 400000U;

	// Token: 0x04000997 RID: 2455
	public const uint RANDOM_QUEST_ID_END = 500000U;

	// Token: 0x04000998 RID: 2456
	public const int RANDOM_QUEST_START_DIALOG = 10000;

	// Token: 0x04000999 RID: 2457
	public const int RANDOM_QUEST_END_DIALOG = 10001;

	// Token: 0x0400099A RID: 2458
	public const uint AUTO_QUEST_ID_START = 500001U;

	// Token: 0x0400099B RID: 2459
	public const uint AUTO_QUEST_ID_END = 600000U;

	// Token: 0x0400099C RID: 2460
	public const int AUTO_QUEST_START_DIALOG = 10002;

	// Token: 0x0400099D RID: 2461
	public const int AUTO_QUEST_END_DIALOG = 10003;

	// Token: 0x0400099E RID: 2462
	public const uint COMMUNITY_EVENT_FAKE_QUEST_ID = 600001U;

	// Token: 0x0400099F RID: 2463
	private const string DID = "did";

	// Token: 0x040009A0 RID: 2464
	private const string NAME = "name";

	// Token: 0x040009A1 RID: 2465
	private const string CHUNK = "chunk";

	// Token: 0x040009A2 RID: 2466
	private const string TAG = "tag";

	// Token: 0x040009A3 RID: 2467
	private const string ICON = "icon";

	// Token: 0x040009A4 RID: 2468
	private const string DIALOG_HEADING = "dialog_heading";

	// Token: 0x040009A5 RID: 2469
	private const string DIALOG_BODY = "dialog_body";

	// Token: 0x040009A6 RID: 2470
	private const string PORTRAIT = "portrait";

	// Token: 0x040009A7 RID: 2471
	private const string FEATURE_UNLOCKS = "feature_unlocks";

	// Token: 0x040009A8 RID: 2472
	private const string BUILDING_UNLOCKS = "building_unlocks";

	// Token: 0x040009A9 RID: 2473
	private const string COSTUME_UNLOCKS = "costume_unlocks";

	// Token: 0x040009AA RID: 2474
	private const string DIALOG_PACKAGE_DID = "dialog_package_did";

	// Token: 0x040009AB RID: 2475
	private const string QUEST_LINE = "quest_line";

	// Token: 0x040009AC RID: 2476
	private const string SESSION_ACTIONS = "session_actions";

	// Token: 0x040009AD RID: 2477
	private const string POST_SESSION_ACTIONS = "post_session_actions";

	// Token: 0x040009AE RID: 2478
	private const string REWARD = "reward";

	// Token: 0x040009AF RID: 2479
	private const string START = "start";

	// Token: 0x040009B0 RID: 2480
	private const string END = "end";

	// Token: 0x040009B1 RID: 2481
	private const string AUTO_QUEST_ID = "auto_quest_id";

	// Token: 0x040009B2 RID: 2482
	private const string AUTO_QUEST_CHAR_ID = "auto_quest_char_id";

	// Token: 0x040009B3 RID: 2483
	private const string MICRO_EVENT_DID = "micro_event_id";

	// Token: 0x040009B4 RID: 2484
	private const string BRANCH = "branch";

	// Token: 0x040009B5 RID: 2485
	private const string STORE_TAB = "store_tab";

	// Token: 0x040009B6 RID: 2486
	private string storeTab;

	// Token: 0x040009B7 RID: 2487
	private uint did;

	// Token: 0x040009B8 RID: 2488
	private string name;

	// Token: 0x040009B9 RID: 2489
	private bool chunk;

	// Token: 0x040009BA RID: 2490
	private string tag;

	// Token: 0x040009BB RID: 2491
	private string icon;

	// Token: 0x040009BC RID: 2492
	private uint dialogPackageDid;

	// Token: 0x040009BD RID: 2493
	private string dialogHeading;

	// Token: 0x040009BE RID: 2494
	private string dialogBody;

	// Token: 0x040009BF RID: 2495
	private string portrait;

	// Token: 0x040009C0 RID: 2496
	private QuestBookendInfo start;

	// Token: 0x040009C1 RID: 2497
	private QuestBookendInfo end;

	// Token: 0x040009C2 RID: 2498
	private SessionActionDefinition sessionActions;

	// Token: 0x040009C3 RID: 2499
	private SessionActionDefinition postSessionActions;

	// Token: 0x040009C4 RID: 2500
	private RewardDefinition rewardDefinition;

	// Token: 0x040009C5 RID: 2501
	private List<string> featureUnlocks;

	// Token: 0x040009C6 RID: 2502
	private List<int> buildingUnlocks;

	// Token: 0x040009C7 RID: 2503
	private List<int> costumeUnlocks;

	// Token: 0x040009C8 RID: 2504
	private string collectStart;

	// Token: 0x040009C9 RID: 2505
	private string collectComplete;

	// Token: 0x040009CA RID: 2506
	private QuestLineInfo questLine;

	// Token: 0x040009CB RID: 2507
	private int autoQuestID;

	// Token: 0x040009CC RID: 2508
	private int autoQuestCharacterID;

	// Token: 0x040009CD RID: 2509
	private int? microEventDID;

	// Token: 0x040009CE RID: 2510
	private string branch;

	// Token: 0x040009CF RID: 2511
	public static uint LastRandomQuestId;

	// Token: 0x040009D0 RID: 2512
	public static uint LastAutoQuestId;

	// Token: 0x040009D1 RID: 2513
	public static Dictionary<uint, Dictionary<string, object>> StartInputPrompts = new Dictionary<uint, Dictionary<string, object>>();

	// Token: 0x040009D2 RID: 2514
	public static Dictionary<uint, Dictionary<string, object>> CompleteInputPrompts = new Dictionary<uint, Dictionary<string, object>>();
}
