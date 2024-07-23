using System;
using System.Collections.Generic;

// Token: 0x0200016E RID: 366
public class TreasureDialogInputData : PersistedDialogInputData
{
	// Token: 0x06000C59 RID: 3161 RVA: 0x0004A5AC File Offset: 0x000487AC
	public TreasureDialogInputData(string title, string message, Reward reward, string soundBeat) : base(uint.MaxValue, "found_treasure", "Dialog_FoundItem", soundBeat)
	{
		this.title = title;
		this.message = message;
		this.reward = reward;
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000C5A RID: 3162 RVA: 0x0004A5E4 File Offset: 0x000487E4
	public string Title
	{
		get
		{
			return this.title;
		}
	}

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0004A5EC File Offset: 0x000487EC
	public string Message
	{
		get
		{
			return this.message;
		}
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0004A5F4 File Offset: 0x000487F4
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		base.BuildPersistenceDict(ref dictionary, "found_treasure");
		dictionary["title"] = this.title;
		dictionary["message"] = this.message;
		dictionary["reward"] = this.reward.ToDict();
		if (base.SoundBeat != null)
		{
			dictionary["sound"] = base.SoundBeat;
		}
		return dictionary;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x0004A66C File Offset: 0x0004886C
	public new static TreasureDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "title");
		string text2 = TFUtils.LoadString(dict, "message");
		Reward reward = Reward.FromDict(TFUtils.LoadDict(dict, "reward"));
		string soundBeat = TFUtils.TryLoadString(dict, "sound");
		return new TreasureDialogInputData(text, text2, reward, soundBeat);
	}

	// Token: 0x04000843 RID: 2115
	public const string DIALOG_TYPE = "found_treasure";

	// Token: 0x04000844 RID: 2116
	private const string TITLE = "title";

	// Token: 0x04000845 RID: 2117
	private const string MESSAGE = "message";

	// Token: 0x04000846 RID: 2118
	private const string REWARD = "reward";

	// Token: 0x04000847 RID: 2119
	private const string SOUND = "sound";

	// Token: 0x04000848 RID: 2120
	private string title;

	// Token: 0x04000849 RID: 2121
	private string message;

	// Token: 0x0400084A RID: 2122
	private Reward reward;
}
