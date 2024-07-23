using System;
using System.Collections.Generic;

// Token: 0x0200016D RID: 365
public class SpongyGamesDialogInputData : PersistedDialogInputData
{
	// Token: 0x06000C53 RID: 3155 RVA: 0x0004A49C File Offset: 0x0004869C
	public SpongyGamesDialogInputData(Dictionary<string, object> inEventData) : base(uint.MaxValue, "spongy_games", null, null)
	{
		this.eventData = inEventData;
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x0004A4B4 File Offset: 0x000486B4
	public SpongyGamesDialogInputData(uint unSequenceID, Dictionary<string, object> inEventData) : base(unSequenceID, "spongy_games", null, null)
	{
		this.eventData = inEventData;
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000C55 RID: 3157 RVA: 0x0004A4CC File Offset: 0x000486CC
	public Dictionary<string, object> EventData
	{
		get
		{
			return this.eventData;
		}
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x0004A4D4 File Offset: 0x000486D4
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["type"] = "spongy_games";
		dictionary["event_data"] = this.eventData;
		dictionary["sequence_id"] = base.SequenceId;
		return dictionary;
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x0004A520 File Offset: 0x00048720
	public new static SpongyGamesDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		Dictionary<string, object> inEventData = (Dictionary<string, object>)dict["event_data"];
		uint unSequenceID = uint.MaxValue;
		if (dict.ContainsKey("sequence_id"))
		{
			unSequenceID = TFUtils.LoadUint(dict, "sequence_id");
		}
		return new SpongyGamesDialogInputData(unSequenceID, inEventData);
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0004A564 File Offset: 0x00048764
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"SpongyGamesDialogInputData(event_data=",
			TFUtils.DebugDictToString(this.eventData),
			",",
			base.ToString(),
			")"
		});
	}

	// Token: 0x04000841 RID: 2113
	public const string DIALOG_TYPE = "spongy_games";

	// Token: 0x04000842 RID: 2114
	private Dictionary<string, object> eventData;
}
