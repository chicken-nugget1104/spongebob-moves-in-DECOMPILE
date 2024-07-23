using System;
using System.Collections.Generic;

// Token: 0x0200016B RID: 363
public class QuestLineStartDialogInputData : QuestDialogInputData
{
	// Token: 0x06000C4D RID: 3149 RVA: 0x0004A284 File Offset: 0x00048484
	public QuestLineStartDialogInputData(uint sequenceId, Dictionary<string, object> promptData, Dictionary<string, object> contextData, uint? questId) : base(sequenceId, "quest_line_start", promptData, contextData, null, null, questId)
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

	// Token: 0x06000C4E RID: 3150 RVA: 0x0004A308 File Offset: 0x00048508
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> result = new Dictionary<string, object>();
		this.BuildPersistenceDict(ref result, "quest_line_start");
		return result;
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x0004A32C File Offset: 0x0004852C
	public new static QuestLineStartDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		uint sequenceId = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			sequenceId = TFUtils.LoadUint(dict, "sequence_id");
		}
		Dictionary<string, object> promptData = (Dictionary<string, object>)dict["prompt"];
		Dictionary<string, object> contextData = (Dictionary<string, object>)dict["context_data"];
		uint? questId = TFUtils.TryLoadNullableUInt(dict, "quest_id");
		return new QuestLineStartDialogInputData(sequenceId, promptData, contextData, questId);
	}

	// Token: 0x0400083F RID: 2111
	public const string DIALOG_TYPE = "quest_line_start";
}
