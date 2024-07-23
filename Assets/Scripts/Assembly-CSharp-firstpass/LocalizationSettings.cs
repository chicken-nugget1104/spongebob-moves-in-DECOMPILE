using System;
using UnityEngine;

// Token: 0x020000AF RID: 175
[Serializable]
public class LocalizationSettings : ScriptableObject
{
	// Token: 0x060006D4 RID: 1748 RVA: 0x000198D4 File Offset: 0x00017AD4
	public static LanguageCode GetLanguageEnum(string langCode)
	{
		langCode = langCode.ToUpper();
		foreach (object obj in Enum.GetValues(typeof(LanguageCode)))
		{
			LanguageCode languageCode = (LanguageCode)((int)obj);
			if (languageCode + string.Empty == langCode)
			{
				return languageCode;
			}
		}
		Debug.LogError("ERORR: There is no language: [" + langCode + "]");
		return LanguageCode.EN;
	}

	// Token: 0x0400044A RID: 1098
	public string[] sheetTitles;

	// Token: 0x0400044B RID: 1099
	public bool useSystemLanguagePerDefault = true;

	// Token: 0x0400044C RID: 1100
	public string defaultLangCode = "EN";
}
