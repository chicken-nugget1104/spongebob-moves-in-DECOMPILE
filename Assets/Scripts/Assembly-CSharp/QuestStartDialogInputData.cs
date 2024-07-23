using System;
using System.Collections.Generic;

// Token: 0x0200016C RID: 364
public class QuestStartDialogInputData : QuestDialogInputData
{
	// Token: 0x06000C50 RID: 3152 RVA: 0x0004A390 File Offset: 0x00048590
	public QuestStartDialogInputData(uint sequenceId, Dictionary<string, object> promptData, Dictionary<string, object> contextData, uint? questId) : base(sequenceId, "quest_start", promptData, contextData, null, null, questId)
	{
		string soundImmediate = "Dialog_QuestStart";
		string soundBeat = "Beat_QuestStart";
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

	// Token: 0x06000C51 RID: 3153 RVA: 0x0004A414 File Offset: 0x00048614
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> result = new Dictionary<string, object>();
		this.BuildPersistenceDict(ref result, "quest_start");
		return result;
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x0004A438 File Offset: 0x00048638
	public new static QuestStartDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		uint sequenceId = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			sequenceId = TFUtils.LoadUint(dict, "sequence_id");
		}
		Dictionary<string, object> promptData = (Dictionary<string, object>)dict["prompt"];
		Dictionary<string, object> contextData = (Dictionary<string, object>)dict["context_data"];
		uint? questId = TFUtils.TryLoadNullableUInt(dict, "quest_id");
		return new QuestStartDialogInputData(sequenceId, promptData, contextData, questId);
	}

	// Token: 0x04000840 RID: 2112
	public const string DIALOG_TYPE = "quest_start";
}
