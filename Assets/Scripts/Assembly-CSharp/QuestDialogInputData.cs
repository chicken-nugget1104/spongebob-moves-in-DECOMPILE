using System;
using System.Collections.Generic;

// Token: 0x02000169 RID: 361
public abstract class QuestDialogInputData : PersistedDialogInputData
{
	// Token: 0x06000C45 RID: 3141 RVA: 0x0004A0BC File Offset: 0x000482BC
	public QuestDialogInputData(uint sequenceId, string type, Dictionary<string, object> promptData, Dictionary<string, object> contextData, string soundImmediate, string soundBeat, uint? questId) : base(sequenceId, type, soundImmediate, soundBeat)
	{
		this.promptData = promptData;
		this.contextData = contextData;
		this.questId = questId;
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0004A0E4 File Offset: 0x000482E4
	public uint? QuestId
	{
		get
		{
			return this.questId;
		}
	}

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000C47 RID: 3143 RVA: 0x0004A0EC File Offset: 0x000482EC
	public Dictionary<string, object> PromptData
	{
		get
		{
			return this.promptData;
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0004A0F4 File Offset: 0x000482F4
	public Dictionary<string, object> ContextData
	{
		get
		{
			return this.contextData;
		}
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x0004A0FC File Offset: 0x000482FC
	protected override void BuildPersistenceDict(ref Dictionary<string, object> dict, string type)
	{
		base.BuildPersistenceDict(ref dict, type);
		dict["sequence_id"] = base.SequenceId;
		dict["prompt"] = this.promptData;
		dict["context_data"] = this.contextData;
		if (this.questId != null)
		{
			dict["quest_id"] = this.questId.Value;
		}
	}

	// Token: 0x0400083B RID: 2107
	private Dictionary<string, object> promptData;

	// Token: 0x0400083C RID: 2108
	private Dictionary<string, object> contextData;

	// Token: 0x0400083D RID: 2109
	private uint? questId;
}
