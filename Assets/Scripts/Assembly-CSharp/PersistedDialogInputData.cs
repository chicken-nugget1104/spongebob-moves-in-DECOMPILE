using System;
using System.Collections.Generic;

// Token: 0x02000167 RID: 359
public abstract class PersistedDialogInputData : DialogInputData
{
	// Token: 0x06000C3E RID: 3134 RVA: 0x00049DE0 File Offset: 0x00047FE0
	public PersistedDialogInputData(uint sequenceId, string type, string soundImmediate, string soundBeat) : base(sequenceId, type, soundImmediate, soundBeat)
	{
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x00049DF0 File Offset: 0x00047FF0
	public static PersistedDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "type");
		string text2 = text;
		switch (text2)
		{
		case "character":
			return CharacterDialogInputData.FromPersistenceDict(dict);
		case "quest_start":
			return QuestStartDialogInputData.FromPersistenceDict(dict);
		case "quest_complete":
			return QuestCompleteDialogInputData.FromPersistenceDict(dict);
		case "quest_line_start":
			return QuestLineStartDialogInputData.FromPersistenceDict(dict);
		case "quest_line_complete":
			return QuestLineCompleteDialogInputData.FromPersistenceDict(dict);
		case "booty_quest_complete":
			return BootyQuestCompleteDialogInputData.FromPersistenceDict(dict);
		case "level_up":
			return LevelUpDialogInputData.FromPersistenceDict(dict);
		case "found_movie":
			return FoundMovieDialogInputData.FromPersistenceDict(dict);
		case "found_item":
			return FoundItemDialogInputData.FromPersistenceDict(dict);
		case "explanation":
			return ExplanationDialogInputData.FromPersistenceDict(dict);
		case "movein":
			return MoveInDialogInputData.FromPersistenceDict(dict);
		case "found_treasure":
			return TreasureDialogInputData.FromPersistenceDict(dict);
		case "spongy_games":
			return SpongyGamesDialogInputData.FromPersistenceDict(dict);
		case "daily_bonus":
			return DailyBonusDialogInputData.FromPersistenceDict(dict);
		}
		TFUtils.Assert(false, "Unexpected dialog type:  " + text);
		return null;
	}

	// Token: 0x06000C40 RID: 3136
	public abstract Dictionary<string, object> ToPersistenceDict();

	// Token: 0x06000C41 RID: 3137 RVA: 0x00049FA0 File Offset: 0x000481A0
	protected virtual void BuildPersistenceDict(ref Dictionary<string, object> dict, string dialogType)
	{
		dict["type"] = dialogType;
	}
}
