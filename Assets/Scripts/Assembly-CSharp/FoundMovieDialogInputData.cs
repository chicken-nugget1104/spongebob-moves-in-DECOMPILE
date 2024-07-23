using System;
using System.Collections.Generic;

// Token: 0x02000164 RID: 356
public class FoundMovieDialogInputData : FoundItemDialogInputData
{
	// Token: 0x06000C2F RID: 3119 RVA: 0x00049A5C File Offset: 0x00047C5C
	public FoundMovieDialogInputData(string title, string message, string icon, string movie, string soundBeat) : base(title, message, icon, soundBeat)
	{
		this.movie = movie;
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00049A74 File Offset: 0x00047C74
	public string Movie
	{
		get
		{
			return this.movie;
		}
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00049A7C File Offset: 0x00047C7C
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		base.BuildPersistenceDict(ref dictionary, "found_movie");
		dictionary["title"] = this.title;
		dictionary["message"] = this.message;
		dictionary["icon"] = this.icon;
		dictionary["movie"] = this.movie;
		if (base.SoundBeat != null)
		{
			dictionary["sound_beat"] = base.SoundBeat;
		}
		return dictionary;
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00049B00 File Offset: 0x00047D00
	public new static FoundMovieDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string title = TFUtils.LoadString(dict, "title");
		string message = TFUtils.LoadString(dict, "message");
		string icon = TFUtils.LoadString(dict, "icon");
		string text = TFUtils.LoadString(dict, "movie");
		string soundBeat = TFUtils.TryLoadString(dict, "sound_beat");
		return new FoundMovieDialogInputData(title, message, icon, text, soundBeat);
	}

	// Token: 0x0400082B RID: 2091
	public new const string DIALOG_TYPE = "found_movie";

	// Token: 0x0400082C RID: 2092
	protected const string MOVIE = "movie";

	// Token: 0x0400082D RID: 2093
	protected string movie;
}
