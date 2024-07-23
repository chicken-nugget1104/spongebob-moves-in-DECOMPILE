using System;
using System.Collections.Generic;

// Token: 0x020000EF RID: 239
public class QuestProgressAction : QuestAction
{
	// Token: 0x060008B2 RID: 2226 RVA: 0x00037AD4 File Offset: 0x00035CD4
	private QuestProgressAction(uint questId, ulong? startTime, ulong? completionTime, QuestProgressAction.ConditionType conditionType, List<uint> conditionIds) : base("qp", questId, startTime, completionTime)
	{
		this.conditionType = conditionType;
		this.conditionIds = conditionIds;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x00037AF4 File Offset: 0x00035CF4
	private QuestProgressAction(uint questId, ulong? startTime, ulong? completionTime, QuestProgressAction.ConditionType conditionType, ICollection<uint> conditionIds) : base("qp", questId, startTime, completionTime)
	{
		this.conditionType = conditionType;
		this.conditionIds = new List<uint>();
		foreach (uint item in conditionIds)
		{
			this.conditionIds.Add(item);
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00037B7C File Offset: 0x00035D7C
	public QuestProgressAction(Quest quest, QuestProgressAction.ConditionType conditionType, ICollection<uint> conditionIds) : this(quest.Did, quest.StartTime, quest.CompletionTime, conditionType, conditionIds)
	{
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00037BA4 File Offset: 0x00035DA4
	public new static QuestProgressAction FromDict(Dictionary<string, object> data)
	{
		QuestProgressAction.ConditionType conditionType = QuestProgressAction.ConditionTypeFromString(TFUtils.LoadString(data, "condition_type"));
		List<uint> list = TFUtils.LoadList<uint>(data, "condition_ids");
		uint questId = TFUtils.LoadUint(data, "did");
		ulong? startTime = TFUtils.LoadNullableUlong(data, "start_time");
		ulong? completionTime = TFUtils.LoadNullableUlong(data, "completion_time");
		return new QuestProgressAction(questId, startTime, completionTime, conditionType, list);
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00037C04 File Offset: 0x00035E04
	private static string ConditionTypeToString(QuestProgressAction.ConditionType conditionType)
	{
		if (conditionType == QuestProgressAction.ConditionType.START)
		{
			return "s";
		}
		if (conditionType != QuestProgressAction.ConditionType.END)
		{
			throw new ArgumentException("Unrecognized condition type:  " + conditionType.ToString());
		}
		return "e";
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00037C4C File Offset: 0x00035E4C
	private static QuestProgressAction.ConditionType ConditionTypeFromString(string s)
	{
		if (s != null)
		{
			if (QuestProgressAction.<>f__switch$map6 == null)
			{
				QuestProgressAction.<>f__switch$map6 = new Dictionary<string, int>(2)
				{
					{
						"s",
						0
					},
					{
						"e",
						1
					}
				};
			}
			int num;
			if (QuestProgressAction.<>f__switch$map6.TryGetValue(s, out num))
			{
				if (num == 0)
				{
					return QuestProgressAction.ConditionType.START;
				}
				if (num == 1)
				{
					return QuestProgressAction.ConditionType.END;
				}
			}
		}
		throw new ArgumentException("Unrecognized condition type string:  " + s);
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00037CC8 File Offset: 0x00035EC8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["condition_type"] = QuestProgressAction.ConditionTypeToString(this.conditionType);
		dictionary["condition_ids"] = TFUtils.CloneAndCastList<uint, object>(this.conditionIds);
		return dictionary;
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x00037D0C File Offset: 0x00035F0C
	public override void Apply(Game game, ulong utcNow)
	{
		Quest quest = game.questManager.GetQuest(this.questId);
		if (quest != null)
		{
			if (this.startTime != null)
			{
				quest.Start(this.startTime.Value);
			}
			if (this.completionTime != null)
			{
				quest.Complete(this.completionTime.Value);
			}
			if (this.conditionType == QuestProgressAction.ConditionType.START)
			{
				quest.StartProgress = new ConditionalProgress(this.conditionIds);
			}
			else if (this.conditionType == QuestProgressAction.ConditionType.END)
			{
				quest.EndProgress = new ConditionalProgress(this.conditionIds);
			}
			game.questManager.RegisterQuest(game, quest);
		}
		else
		{
			TFUtils.WarningLog("QuestProgressAction.Apply - Quest " + this.questId + " does not exist in the questList.");
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x00037DF0 File Offset: 0x00035FF0
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		List<object> list = (List<object>)dictionary["quests"];
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)list[i];
			uint num2 = TFUtils.LoadUint(data, "did");
			if (num2 == this.questId)
			{
				num++;
				if (num == 1)
				{
					Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list[i];
					Dictionary<string, object> dictionary3 = (Dictionary<string, object>)dictionary2["conditions"];
					if (this.conditionType == QuestProgressAction.ConditionType.START)
					{
						dictionary3["met_start_condition_ids"] = TFUtils.CloneAndCastList<uint, object>(this.conditionIds);
					}
					if (this.conditionType == QuestProgressAction.ConditionType.END)
					{
						dictionary3["met_end_condition_ids"] = TFUtils.CloneAndCastList<uint, object>(this.conditionIds);
					}
				}
			}
		}
		if (num == 0)
		{
			Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
			Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
			TFUtils.Assert(this.conditionType == QuestProgressAction.ConditionType.START, "Can't process End Conditions - No Quest Starting progress has been made yet.");
			dictionary5["met_start_condition_ids"] = TFUtils.CloneAndCastList<uint, object>(this.conditionIds);
			dictionary5["met_end_condition_ids"] = new List<object>();
			dictionary4["did"] = this.questId;
			dictionary4["start_time"] = null;
			dictionary4["conditions"] = dictionary5;
			list.Add(dictionary4);
		}
		TFUtils.Assert(num <= 1, "Too many quests match did " + this.questId);
		base.Confirm(gameState);
	}

	// Token: 0x04000630 RID: 1584
	public const string QUEST_PROGRESS = "qp";

	// Token: 0x04000631 RID: 1585
	private QuestProgressAction.ConditionType conditionType;

	// Token: 0x04000632 RID: 1586
	private List<uint> conditionIds;

	// Token: 0x020000F0 RID: 240
	public enum ConditionType
	{
		// Token: 0x04000635 RID: 1589
		START,
		// Token: 0x04000636 RID: 1590
		END
	}
}
