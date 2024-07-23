using System;
using System.Collections.Generic;

// Token: 0x0200015F RID: 351
public abstract class DialogInputData
{
	// Token: 0x06000C08 RID: 3080 RVA: 0x00048844 File Offset: 0x00046A44
	public DialogInputData(uint sequenceId, string type, string soundImmediate, string soundBeat)
	{
		this.sequenceId = sequenceId;
		this.type = type;
		this.soundImmediate = soundImmediate;
		this.soundBeat = soundBeat;
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000C09 RID: 3081 RVA: 0x0004886C File Offset: 0x00046A6C
	public uint SequenceId
	{
		get
		{
			return this.sequenceId;
		}
	}

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x06000C0A RID: 3082 RVA: 0x00048874 File Offset: 0x00046A74
	public string SoundImmediate
	{
		get
		{
			return this.soundImmediate;
		}
	}

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000C0B RID: 3083 RVA: 0x0004887C File Offset: 0x00046A7C
	public string SoundBeat
	{
		get
		{
			return this.soundBeat;
		}
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x00048884 File Offset: 0x00046A84
	public static DialogInputData FromPromptDict(uint sequenceId, Dictionary<string, object> prompt, Dictionary<string, object> contextData, uint? associatedQuestId)
	{
		string text = TFUtils.LoadString(prompt, "type");
		DialogInputData dialogInputData;
		if (text.Equals("character"))
		{
			dialogInputData = new CharacterDialogInputData(sequenceId, prompt);
		}
		else if (text.Equals("quest_start"))
		{
			dialogInputData = new QuestStartDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else if (text.Equals("quest_complete"))
		{
			dialogInputData = new QuestCompleteDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else if (text.Equals("booty_quest_complete"))
		{
			dialogInputData = new BootyQuestCompleteDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else if (text.Equals("quest_line_start"))
		{
			dialogInputData = new QuestLineStartDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else if (text.Equals("quest_line_complete"))
		{
			dialogInputData = new QuestLineCompleteDialogInputData(sequenceId, prompt, contextData, associatedQuestId);
		}
		else
		{
			if (!text.Equals("found_item"))
			{
				TFUtils.Assert(false, "Unexpected prompt type:  " + text);
				return null;
			}
			dialogInputData = new FoundItemDialogInputData(sequenceId, prompt);
		}
		TFUtils.Assert(dialogInputData.type != null && dialogInputData.type != string.Empty, "Did not find a type on DialogInputData=" + dialogInputData.ToString());
		return dialogInputData;
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x000489BC File Offset: 0x00046BBC
	public ITrigger CreateTrigger(ulong utcTimeStamp)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["sequence_id"] = this.sequenceId;
		return new Trigger("dialogtrigger_" + this.type, dictionary, utcTimeStamp, null, null);
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00048A00 File Offset: 0x00046C00
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"DialogInputData(sequenceId=",
			this.sequenceId,
			", type=",
			this.type,
			")"
		});
	}

	// Token: 0x04000810 RID: 2064
	public const string TRIGGER_TYPE_PREFIX = "dialogtrigger_";

	// Token: 0x04000811 RID: 2065
	public const float STANDARD_BEAT_LENGTH = 1f;

	// Token: 0x04000812 RID: 2066
	public const uint NO_ID = 4294967295U;

	// Token: 0x04000813 RID: 2067
	protected const string SOUND_TO_PLAY = "sound_to_play";

	// Token: 0x04000814 RID: 2068
	protected const string TYPE = "type";

	// Token: 0x04000815 RID: 2069
	private uint sequenceId;

	// Token: 0x04000816 RID: 2070
	private string type;

	// Token: 0x04000817 RID: 2071
	protected string soundImmediate;

	// Token: 0x04000818 RID: 2072
	protected string soundBeat;
}
