using System;
using System.Collections.Generic;

// Token: 0x02000162 RID: 354
public class ExplanationDialogInputData : PersistedDialogInputData
{
	// Token: 0x06000C24 RID: 3108 RVA: 0x000497F0 File Offset: 0x000479F0
	public ExplanationDialogInputData(string message, string soundBeat) : base(uint.MaxValue, "explanation", "Dialog_Explanation", soundBeat)
	{
		this.message = message;
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000C25 RID: 3109 RVA: 0x0004980C File Offset: 0x00047A0C
	public string Message
	{
		get
		{
			return this.message;
		}
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x00049814 File Offset: 0x00047A14
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.BuildPersistenceDict(ref dictionary, "explanation");
		dictionary["message"] = this.message;
		if (base.SoundBeat != null)
		{
			dictionary["soundBeat"] = base.SoundBeat;
		}
		return dictionary;
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x00049864 File Offset: 0x00047A64
	public new static ExplanationDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "message");
		string soundBeat = TFUtils.TryLoadString(dict, "soundBeat");
		return new ExplanationDialogInputData(text, soundBeat);
	}

	// Token: 0x0400081F RID: 2079
	public const string DIALOG_TYPE = "explanation";

	// Token: 0x04000820 RID: 2080
	private const string MESSAGE = "message";

	// Token: 0x04000821 RID: 2081
	private const string SOUND_BEAT = "soundBeat";

	// Token: 0x04000822 RID: 2082
	private string message;
}
