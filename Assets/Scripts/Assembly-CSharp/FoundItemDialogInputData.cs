using System;
using System.Collections.Generic;

// Token: 0x02000163 RID: 355
public class FoundItemDialogInputData : PersistedDialogInputData
{
	// Token: 0x06000C28 RID: 3112 RVA: 0x00049890 File Offset: 0x00047A90
	public FoundItemDialogInputData(uint sequenceId, Dictionary<string, object> prompt) : base(sequenceId, "found_item", "Dialog_FoundItem", null)
	{
		if (prompt.ContainsKey("title"))
		{
			this.title = Language.Get((string)prompt["title"]);
		}
		if (prompt.ContainsKey("body"))
		{
			this.message = Language.Get((string)prompt["body"]);
		}
		if (prompt.ContainsKey("effect"))
		{
			this.soundBeat = (string)prompt["effect"];
		}
		if (prompt.ContainsKey("icon"))
		{
			this.icon = (string)prompt["icon"];
		}
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x00049954 File Offset: 0x00047B54
	public FoundItemDialogInputData(string title, string message, string icon, string soundBeat) : base(uint.MaxValue, "found_item", "Dialog_FoundItem", soundBeat)
	{
		this.title = title;
		this.message = message;
		this.icon = icon;
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000C2A RID: 3114 RVA: 0x0004998C File Offset: 0x00047B8C
	public string Title
	{
		get
		{
			return this.title;
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00049994 File Offset: 0x00047B94
	public string Message
	{
		get
		{
			return this.message;
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0004999C File Offset: 0x00047B9C
	public string Icon
	{
		get
		{
			return this.icon;
		}
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x000499A4 File Offset: 0x00047BA4
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		base.BuildPersistenceDict(ref dictionary, "found_item");
		dictionary["title"] = this.title;
		dictionary["message"] = this.message;
		dictionary["icon"] = this.icon;
		if (base.SoundBeat != null)
		{
			dictionary["sound_beat"] = base.SoundBeat;
		}
		return dictionary;
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x00049A14 File Offset: 0x00047C14
	public new static FoundItemDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "title");
		string text2 = TFUtils.LoadString(dict, "message");
		string text3 = TFUtils.LoadString(dict, "icon");
		string soundBeat = TFUtils.TryLoadString(dict, "sound_beat");
		return new FoundItemDialogInputData(text, text2, text3, soundBeat);
	}

	// Token: 0x04000823 RID: 2083
	public const string DIALOG_TYPE = "found_item";

	// Token: 0x04000824 RID: 2084
	protected const string TITLE = "title";

	// Token: 0x04000825 RID: 2085
	protected const string MESSAGE = "message";

	// Token: 0x04000826 RID: 2086
	protected const string ICON = "icon";

	// Token: 0x04000827 RID: 2087
	protected const string SOUND_BEAT = "sound_beat";

	// Token: 0x04000828 RID: 2088
	protected string title;

	// Token: 0x04000829 RID: 2089
	protected string message;

	// Token: 0x0400082A RID: 2090
	protected string icon;
}
