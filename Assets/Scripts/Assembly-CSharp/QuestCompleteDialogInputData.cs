using System;
using System.Collections.Generic;

// Token: 0x02000168 RID: 360
public class QuestCompleteDialogInputData : QuestDialogInputData
{
	// Token: 0x06000C42 RID: 3138 RVA: 0x00049FB0 File Offset: 0x000481B0
	public QuestCompleteDialogInputData(uint sequenceId, Dictionary<string, object> promptData, Dictionary<string, object> contextData, uint? questId) : base(sequenceId, "quest_complete", promptData, contextData, null, null, questId)
	{
		string soundImmediate = "Dialog_QuestComplete";
		string soundBeat = "Beat_QuestComplete";
		if (promptData != null)
		{
			if (promptData.ContainsKey("voiceover"))
			{
				soundBeat = (string)promptData["voiceover"];
			}
			if (promptData.ContainsKey("effect"))
			{
				soundImmediate = (string)promptData["effect"];
			}
		}
		this.soundImmediate = soundImmediate;
		this.soundBeat = soundBeat;
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x0004A034 File Offset: 0x00048234
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> result = new Dictionary<string, object>();
		this.BuildPersistenceDict(ref result, "quest_complete");
		return result;
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0004A058 File Offset: 0x00048258
	public new static QuestCompleteDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		uint sequenceId = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			sequenceId = TFUtils.LoadUint(dict, "sequence_id");
		}
		Dictionary<string, object> promptData = (Dictionary<string, object>)dict["prompt"];
		Dictionary<string, object> contextData = (Dictionary<string, object>)dict["context_data"];
		uint? questId = TFUtils.TryLoadNullableUInt(dict, "quest_id");
		return new QuestCompleteDialogInputData(sequenceId, promptData, contextData, questId);
	}

	// Token: 0x0400083A RID: 2106
	public const string DIALOG_TYPE = "quest_complete";
}
