using System;
using System.Collections.Generic;

// Token: 0x02000166 RID: 358
public class MoveInDialogInputData : PersistedDialogInputData
{
	// Token: 0x06000C38 RID: 3128 RVA: 0x00049CD8 File Offset: 0x00047ED8
	public MoveInDialogInputData(string characterName, string buildingName, string portraitTexture, string soundBeat) : base(uint.MaxValue, "movein", "Dialog_Explanation", soundBeat)
	{
		this.characterName = characterName;
		this.buildingName = buildingName;
		this.portraitTexture = portraitTexture;
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000C39 RID: 3129 RVA: 0x00049D10 File Offset: 0x00047F10
	public string CharacterName
	{
		get
		{
			return this.characterName;
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00049D18 File Offset: 0x00047F18
	public string BuildingName
	{
		get
		{
			return this.buildingName;
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00049D20 File Offset: 0x00047F20
	public string PortraitTexture
	{
		get
		{
			return this.portraitTexture;
		}
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x00049D28 File Offset: 0x00047F28
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.BuildPersistenceDict(ref dictionary, "movein");
		dictionary["charactername"] = this.characterName;
		dictionary["buildingname"] = this.buildingName;
		dictionary["portraittexture"] = this.portraitTexture;
		if (base.SoundBeat != null)
		{
			dictionary["soundBeat"] = base.SoundBeat;
		}
		return dictionary;
	}

	// Token: 0x06000C3D RID: 3133 RVA: 0x00049D98 File Offset: 0x00047F98
	public new static MoveInDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		string text = TFUtils.LoadString(dict, "charactername");
		string text2 = TFUtils.LoadString(dict, "buildingname");
		string text3 = TFUtils.LoadString(dict, "portraittexture");
		string soundBeat = TFUtils.TryLoadString(dict, "soundBeat");
		return new MoveInDialogInputData(text, text2, text3, soundBeat);
	}

	// Token: 0x04000831 RID: 2097
	public const string DIALOG_TYPE = "movein";

	// Token: 0x04000832 RID: 2098
	private const string CHARACTER_NAME = "charactername";

	// Token: 0x04000833 RID: 2099
	private const string BUILDING_NAME = "buildingname";

	// Token: 0x04000834 RID: 2100
	private const string PORTRAIT_TEXTURE = "portraittexture";

	// Token: 0x04000835 RID: 2101
	private const string SOUND_BEAT = "soundBeat";

	// Token: 0x04000836 RID: 2102
	private string characterName;

	// Token: 0x04000837 RID: 2103
	private string buildingName;

	// Token: 0x04000838 RID: 2104
	private string portraitTexture;
}
