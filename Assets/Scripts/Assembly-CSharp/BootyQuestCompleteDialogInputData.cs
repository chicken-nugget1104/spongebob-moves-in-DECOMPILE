using System;
using System.Collections.Generic;

// Token: 0x0200015C RID: 348
public class BootyQuestCompleteDialogInputData : QuestDialogInputData
{
	// Token: 0x06000BF9 RID: 3065 RVA: 0x000485C0 File Offset: 0x000467C0
	public BootyQuestCompleteDialogInputData(uint sequenceId, Dictionary<string, object> promptData, Dictionary<string, object> contextData, uint? questId) : base(sequenceId, "booty_quest_complete", promptData, contextData, "Dialog_QuestComplete", "Beat_QuestComplete", questId)
	{
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x000485E8 File Offset: 0x000467E8
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> result = new Dictionary<string, object>();
		this.BuildPersistenceDict(ref result, "booty_quest_complete");
		return result;
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0004860C File Offset: 0x0004680C
	public new static BootyQuestCompleteDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		uint sequenceId = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			sequenceId = TFUtils.LoadUint(dict, "sequence_id");
		}
		Dictionary<string, object> promptData = (Dictionary<string, object>)dict["prompt"];
		Dictionary<string, object> contextData = (Dictionary<string, object>)dict["context_data"];
		uint? questId = TFUtils.TryLoadNullableUInt(dict, "quest_id");
		return new BootyQuestCompleteDialogInputData(sequenceId, promptData, contextData, questId);
	}

	// Token: 0x04000809 RID: 2057
	public const string DIALOG_TYPE = "booty_quest_complete";
}
