using System;
using System.Collections.Generic;

// Token: 0x02000065 RID: 101
[Serializable]
public class DialogPrompt
{
	// Token: 0x060003E9 RID: 1001 RVA: 0x00015E08 File Offset: 0x00014008
	public DialogPrompt(string _texture, string _text, string _vo)
	{
		this.texture = _texture;
		this.text = _text;
		this.voiceover = _vo;
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x00015E28 File Offset: 0x00014028
	public DialogPrompt(Dictionary<string, object> dict)
	{
		this.texture = (string)dict["character_icon"];
		this.text = (string)dict["text"];
		if (dict.ContainsKey("voiceover"))
		{
			this.voiceover = (string)dict["voiceover"];
		}
	}

	// Token: 0x040002A5 RID: 677
	public string texture;

	// Token: 0x040002A6 RID: 678
	public string text;

	// Token: 0x040002A7 RID: 679
	public string voiceover;

	// Token: 0x040002A8 RID: 680
	public bool atlased;
}
