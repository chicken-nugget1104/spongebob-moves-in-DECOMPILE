using System;
using System.Collections.Generic;

// Token: 0x020001B1 RID: 433
public class QuestBookendInfo
{
	// Token: 0x06000E85 RID: 3717 RVA: 0x00059350 File Offset: 0x00057550
	public static QuestBookendInfo FromDict(Dictionary<string, object> data, bool chunkQuest, bool autoQuest)
	{
		QuestBookendInfo questBookendInfo = new QuestBookendInfo();
		questBookendInfo.Chunks = new List<QuestBookendInfo.ChunkConditions>();
		if (chunkQuest)
		{
			List<object> orCreateList = TFUtils.GetOrCreateList<object>(data, "array");
			foreach (object obj in orCreateList)
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
				LoadableCondition condition = (LoadableCondition)ConditionFactory.FromDict((Dictionary<string, object>)dictionary["conditions"]);
				string text = TFUtils.TryLoadString(dictionary, "name");
				string text2 = TFUtils.TryLoadString(dictionary, "icon");
				QuestBookendInfo.ChunkConditions item = new QuestBookendInfo.ChunkConditions(condition, (text != null) ? text : string.Empty, (text2 != null) ? text2 : string.Empty);
				questBookendInfo.Chunks.Add(item);
			}
			if (!autoQuest)
			{
				TFUtils.Assert(questBookendInfo.Chunks.Count > 1, "Chunk quests need to have at least 2 items");
			}
		}
		else
		{
			TFUtils.Assert(data.ContainsKey("conditions"), "This bookend should have a condition. Is this an array by mistake?");
			QuestBookendInfo.ChunkConditions item2 = new QuestBookendInfo.ChunkConditions((LoadableCondition)ConditionFactory.FromDict((Dictionary<string, object>)data["conditions"]), string.Empty, string.Empty);
			questBookendInfo.Chunks.Add(item2);
		}
		questBookendInfo.DialogSequenceId = ((!data.ContainsKey("dialog_sequence_id")) ? 0U : TFUtils.LoadNullableUInt(data, "dialog_sequence_id"));
		questBookendInfo.Postpone = ((!data.ContainsKey("postpone")) ? 0f : TFUtils.LoadFloat(data, "postpone"));
		return questBookendInfo;
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x00059514 File Offset: 0x00057714
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (this.Chunks.Count == 1)
		{
			dictionary["conditions"] = this.Chunks[0].Condition.ToDict();
		}
		else
		{
			List<object> list = new List<object>();
			foreach (QuestBookendInfo.ChunkConditions chunkConditions in this.Chunks)
			{
				list.Add(new Dictionary<string, object>
				{
					{
						"conditions",
						chunkConditions.Condition.ToDict()
					},
					{
						"name",
						chunkConditions.Name
					},
					{
						"icon",
						chunkConditions.Icon
					}
				});
			}
			dictionary["array"] = list;
		}
		dictionary["dialog_sequence_id"] = this.DialogSequenceId;
		dictionary["postpone"] = this.Postpone;
		return dictionary;
	}

	// Token: 0x0400098B RID: 2443
	private const string DIALOG_SEQUENCE_ID = "dialog_sequence_id";

	// Token: 0x0400098C RID: 2444
	private const string POSTPONE = "postpone";

	// Token: 0x0400098D RID: 2445
	private const string ARRAY = "array";

	// Token: 0x0400098E RID: 2446
	private const string CONDITIONS = "conditions";

	// Token: 0x0400098F RID: 2447
	public List<QuestBookendInfo.ChunkConditions> Chunks;

	// Token: 0x04000990 RID: 2448
	public uint? DialogSequenceId;

	// Token: 0x04000991 RID: 2449
	public float Postpone;

	// Token: 0x020001B2 RID: 434
	public class ChunkConditions
	{
		// Token: 0x06000E87 RID: 3719 RVA: 0x0005963C File Offset: 0x0005783C
		public ChunkConditions(LoadableCondition condition, string name, string icon)
		{
			this.Condition = condition;
			this.Name = name;
			this.Icon = icon;
		}

		// Token: 0x04000992 RID: 2450
		public LoadableCondition Condition;

		// Token: 0x04000993 RID: 2451
		public string Name;

		// Token: 0x04000994 RID: 2452
		public string Icon;
	}
}
