using System;
using System.Collections.Generic;

// Token: 0x020000F1 RID: 241
public class QuestStartAction : QuestAction
{
	// Token: 0x060008BB RID: 2235 RVA: 0x00037F84 File Offset: 0x00036184
	private QuestStartAction(uint questId, ulong? startTime, ulong? completionTime) : base("qs", questId, startTime, completionTime)
	{
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00037F94 File Offset: 0x00036194
	public QuestStartAction(Quest quest) : this(quest.Did, quest.StartTime, quest.CompletionTime)
	{
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x00037FBC File Offset: 0x000361BC
	public new static QuestStartAction FromDict(Dictionary<string, object> data)
	{
		uint questId = TFUtils.LoadUint(data, "did");
		ulong? startTime = TFUtils.LoadNullableUlong(data, "start_time");
		ulong? completionTime = TFUtils.LoadNullableUlong(data, "completion_time");
		return new QuestStartAction(questId, startTime, completionTime);
	}

	// Token: 0x04000637 RID: 1591
	public const string QUEST_START = "qs";
}
