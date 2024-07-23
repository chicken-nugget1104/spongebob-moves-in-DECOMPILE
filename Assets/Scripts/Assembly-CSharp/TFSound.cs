using System;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class TFSound
{
	// Token: 0x0600221E RID: 8734 RVA: 0x000D23A8 File Offset: 0x000D05A8
	public TFSound(string file, string characterName)
	{
		this.fileName = file;
		this.characterName = characterName;
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x000D23C0 File Offset: 0x000D05C0
	public void Init()
	{
		if (Language.CurrentLanguage() != LanguageCode.EN)
		{
			this.fileName = this.LocalizeSoundFilename(this.fileName, false);
		}
		if (this.clip != null)
		{
			if (this.clip.name != this.fileName)
			{
				if (!this.bundleSound)
				{
					Resources.UnloadAsset(this.clip);
				}
				this.clip = (AudioClip)Resources.Load(this.fileName);
				if (this.clip == null)
				{
					this.clip = (AudioClip)Resources.Load(this.LocalizeSoundFilename(this.fileName, true));
				}
			}
		}
		else
		{
			this.clip = (AudioClip)Resources.Load(this.fileName);
			if (this.clip == null)
			{
				this.clip = (AudioClip)Resources.Load(this.LocalizeSoundFilename(this.fileName, true));
			}
		}
		this.bundleSound = false;
		if (this.clip == null)
		{
			UnityEngine.Object @object = FileSystemCoordinator.LoadAsset(this.fileName);
			if (@object == null)
			{
				@object = FileSystemCoordinator.LoadAsset(this.LocalizeSoundFilename(this.fileName, true));
			}
			if (@object != null)
			{
				this.clip = (AudioClip)@object;
				this.bundleSound = true;
			}
		}
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x000D2520 File Offset: 0x000D0720
	public void Clear()
	{
		if (this.characterName != null && this.clip != null)
		{
			Resources.UnloadAsset(this.clip);
		}
		this.clip = null;
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x000D255C File Offset: 0x000D075C
	public string LocalizeSoundFilename(string filename, bool altFile)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		LanguageCode languageCode = Language.CurrentLanguage();
		switch (languageCode)
		{
		case LanguageCode.DE:
			text = "/DE/";
			text2 = "_DE";
			break;
		default:
			if (languageCode != LanguageCode.FR)
			{
				if (languageCode != LanguageCode.IT)
				{
					if (languageCode != LanguageCode.NL)
					{
						if (languageCode != LanguageCode.PT)
						{
							if (languageCode != LanguageCode.RU)
							{
								if (languageCode != LanguageCode.LatAm)
								{
									text = "/EN/";
									text2 = "_EN";
								}
								else
								{
									text = "/LatAm/";
									text2 = "_LatAm";
								}
							}
							else
							{
								text = "/RU/";
								text2 = "_RU";
							}
						}
						else
						{
							text = "/PT/";
							text2 = "_PT";
						}
					}
					else
					{
						text = "/NL/";
						text2 = "_NL";
					}
				}
				else
				{
					text = "/IT/";
					text2 = "_IT";
				}
			}
			else
			{
				text = "/FR/";
				text2 = "_FR";
			}
			break;
		case LanguageCode.EN:
			text = "/EN/";
			text2 = "_EN";
			break;
		case LanguageCode.ES:
			text = "/ES/";
			text2 = "_ES";
			break;
		}
		string oldValue;
		string oldValue2;
		if (altFile)
		{
			oldValue = text;
			oldValue2 = text2;
			text = "EXT";
			text2 = string.Empty;
		}
		else
		{
			oldValue = "/EN/";
			oldValue2 = "_EN";
		}
		return filename.Replace(oldValue, text).Replace(oldValue2, text2);
	}

	// Token: 0x04001518 RID: 5400
	public string fileName;

	// Token: 0x04001519 RID: 5401
	public string characterName;

	// Token: 0x0400151A RID: 5402
	public AudioClip clip;

	// Token: 0x0400151B RID: 5403
	public bool bundleSound;
}
