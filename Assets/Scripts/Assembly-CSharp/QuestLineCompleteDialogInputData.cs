using System;
using System.Collections.Generic;

// Token: 0x0200016A RID: 362
public class QuestLineCompleteDialogInputData : QuestDialogInputData
{
	// Token: 0x06000C4A RID: 3146 RVA: 0x0004A178 File Offset: 0x00048378
	public QuestLineCompleteDialogInputData(uint sequenceId, Dictionary<string, object> promptData, Dictionary<string, object> contextData, uint? questId) : base(sequenceId, "quest_line_complete", promptData, contextData, null, null, questId)
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

	// Token: 0x06000C4B RID: 3147 RVA: 0x0004A1FC File Offset: 0x000483FC
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> result = new Dictionary<string, object>();
		this.BuildPersistenceDict(ref result, "quest_line_complete");
		return result;
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0004A220 File Offset: 0x00048420
	public new static QuestLineCompleteDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		uint sequenceId = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			sequenceId = TFUtils.LoadUint(dict, "sequence_id");
		}
		Dictionary<string, object> promptData = (Dictionary<string, object>)dict["prompt"];
		Dictionary<string, object> contextData = (Dictionary<string, object>)dict["context_data"];
		uint? questId = TFUtils.TryLoadNullableUInt(dict, "quest_id");
		return new QuestLineCompleteDialogInputData(sequenceId, promptData, contextData, questId);
	}

	// Token: 0x0400083E RID: 2110
	public const string DIALOG_TYPE = "quest_line_complete";
}
