using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public static class Language
{
	// Token: 0x060006B4 RID: 1716 RVA: 0x000181CC File Offset: 0x000163CC
	private static void CreateAndroidLocal()
	{
		try
		{
			Language._pAndroidLocal = new AndroidJavaClass("com.fws.localization.localization");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					Language._sDeviceLocal = Language._pAndroidLocal.CallStatic<string>("GetDeviceCountry", new object[]
					{
						@static
					});
					Language._sDeviceLanguage = Language._pAndroidLocal.CallStatic<string>("GetDeviceLocal", new object[]
					{
						@static
					});
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log("CreateAndroidLocal: " + ex.Message);
		}
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x000182CC File Offset: 0x000164CC
	public static string getDeviceLanguage()
	{
		if (Language._pAndroidLocal == null)
		{
			Language.CreateAndroidLocal();
		}
		return Language._sDeviceLanguage;
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x000182E4 File Offset: 0x000164E4
	public static string getDeviceLocale()
	{
		if (Language._pAndroidLocal == null)
		{
			Language.CreateAndroidLocal();
		}
		return Language._sDeviceLocal;
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x000182FC File Offset: 0x000164FC
	public static void ResetHasInitialized()
	{
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x00018300 File Offset: 0x00016500
	public static void Init(string persistentPath)
	{
		if (!string.IsNullOrEmpty(persistentPath))
		{
			Language._persistentDataPath = persistentPath;
		}
		Language.LoadAvailableLanguages();
		bool useSystemLanguagePerDefault = Language.settings.useSystemLanguagePerDefault;
		LanguageCode code = LocalizationSettings.GetLanguageEnum(Language.settings.defaultLangCode);
		if (useSystemLanguagePerDefault)
		{
			LanguageCode languageCode = Language.LanguageNameToCode(Application.systemLanguage);
			string text = Language.getDeviceLocale().ToLower().Trim();
			string text2 = Language.getDeviceLanguage().ToLower().Trim();
			if (text2 != null)
			{
				text2 = text2.Replace('_', '-');
			}
			Debug.Log(string.Concat(new string[]
			{
				"Local: ",
				text,
				" : Language: ",
				text2,
				" : Code: ",
				languageCode.ToString()
			}));
			if ((text2.Equals("es") && !text.Equals("es")) || (text2.Contains("es-") && !text.Equals("es")))
			{
				languageCode = LanguageCode.LatAm;
			}
			if (languageCode == LanguageCode.N)
			{
				string twoLetterISOLanguageName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
				if (twoLetterISOLanguageName != "iv")
				{
					languageCode = LocalizationSettings.GetLanguageEnum(twoLetterISOLanguageName);
				}
			}
			if (Language.availableLanguages.Contains(languageCode + string.Empty))
			{
				code = languageCode;
			}
		}
		string @string = PlayerPrefs.GetString("M2H_lastLanguage", string.Empty);
		if (@string != string.Empty && Language.availableLanguages.Contains(@string))
		{
			Language.SwitchLanguage(@string);
		}
		else
		{
			Language.SwitchLanguage(code);
		}
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x000184A0 File Offset: 0x000166A0
	public static string LocalizedEnglishAssetName(string assetName)
	{
		if (Language.CurrentLanguage() != LanguageCode.EN)
		{
			string newValue = Language.CurrentLanguage().ToString().ToLower();
			assetName = assetName.Replace("en", newValue);
		}
		return assetName;
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x000184E0 File Offset: 0x000166E0
	private static void LoadAvailableLanguages()
	{
		Language.availableLanguages = new List<string>();
		Debug.Log(string.Concat(new object[]
		{
			"Language: Settings: ",
			Language.settings,
			" Backup: ",
			Language.backup_settings
		}));
		if (Language.settings.sheetTitles == null || Language.settings.sheetTitles.Length <= 0)
		{
			Debug.Log("------- First file not found -- Trying BackupLocalizationSettings.Asset file");
			Language.settings = Language.backup_settings;
			if (Language.backup_settings.sheetTitles == null || Language.backup_settings.sheetTitles.Length <= 0)
			{
				Debug.Log("------- None available -- Can't Even use BackupLocalizationSettings.Asset file");
				return;
			}
		}
		foreach (object obj in Enum.GetValues(typeof(LanguageCode)))
		{
			LanguageCode languageCode = (LanguageCode)((int)obj);
			if (Language.HasLanguageFile(languageCode + string.Empty, Language.settings.sheetTitles[0]))
			{
				Language.availableLanguages.Add(languageCode + string.Empty);
			}
		}
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x00018634 File Offset: 0x00016834
	public static string[] GetLanguages()
	{
		return Language.availableLanguages.ToArray();
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00018640 File Offset: 0x00016840
	public static bool ReloadLanguage(string persistantPath = null)
	{
		if (string.IsNullOrEmpty(Language._persistentDataPath) || Language.currentLanguage == LanguageCode.N)
		{
			Language.Init(persistantPath);
			return true;
		}
		return Language.SwitchLanguage(Language.currentLanguage);
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00018680 File Offset: 0x00016880
	public static bool SwitchLanguage(string langCode)
	{
		return Language.SwitchLanguage(LocalizationSettings.GetLanguageEnum(langCode));
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x00018690 File Offset: 0x00016890
	public static bool SwitchLanguage(LanguageCode code)
	{
		if (Language.availableLanguages.Contains(code + string.Empty))
		{
			Language.DoSwitch(code);
			return true;
		}
		Debug.LogError(string.Concat(new object[]
		{
			"Could not switch from language ",
			Language.currentLanguage,
			" to ",
			code
		}));
		if (Language.currentLanguage == LanguageCode.N)
		{
			if (Language.availableLanguages.Count > 0)
			{
				Language.DoSwitch(LocalizationSettings.GetLanguageEnum(Language.availableLanguages[0]));
				Debug.LogError("Switched to " + Language.currentLanguage + " instead");
			}
			else
			{
				Debug.LogError("Please verify that you have the file: Resources/Languages/" + code + string.Empty);
				Debug.Break();
			}
		}
		return false;
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0001876C File Offset: 0x0001696C
	private static void DoSwitch(LanguageCode newLang)
	{
		PlayerPrefs.GetString("M2H_lastLanguage", newLang + string.Empty);
		Language.currentLanguage = newLang;
		Language.currentEntrySheets = new Dictionary<string, Hashtable>();
		XMLParser xmlparser = new XMLParser();
		foreach (string text in Language.settings.sheetTitles)
		{
			Language.currentEntrySheets[text] = new Hashtable();
			Hashtable hashtable = (Hashtable)xmlparser.Parse(Language.GetLanguageFileContents(text));
			ArrayList arrayList = (ArrayList)(((ArrayList)hashtable["entries"])[0] as Hashtable)["entry"];
			foreach (object obj in arrayList)
			{
				Hashtable hashtable2 = (Hashtable)obj;
				string key = (string)hashtable2["@name"];
				string text2 = (hashtable2["_text"] + string.Empty).Trim();
				text2 = text2.UnescapeXML();
				Language.currentEntrySheets[text][key] = text2;
			}
		}
		LocalizedAsset[] array = (LocalizedAsset[])UnityEngine.Object.FindObjectsOfType(typeof(LocalizedAsset));
		foreach (LocalizedAsset localizedAsset in array)
		{
			localizedAsset.LocalizeAsset();
		}
		Language.SendMonoMessage("ChangedLanguage", new object[]
		{
			Language.currentLanguage
		});
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x00018928 File Offset: 0x00016B28
	public static UnityEngine.Object GetAsset(string name)
	{
		if (!Language.assets.ContainsKey(name))
		{
			Language.assets[name] = Resources.Load(string.Concat(new object[]
			{
				"Languages/Assets/",
				Language.CurrentLanguage(),
				"/",
				name
			}));
		}
		return Language.assets[name];
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0001898C File Offset: 0x00016B8C
	private static bool HasLanguageFile(string lang, string sheetTitle)
	{
		return (TextAsset)Resources.Load("Languages/" + lang + "_" + sheetTitle, typeof(TextAsset)) != null;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x000189BC File Offset: 0x00016BBC
	private static string GetLanguageFileContents(string sheetTitle)
	{
		string text = string.Concat(new object[]
		{
			"Languages/",
			Language.currentLanguage,
			"_",
			sheetTitle
		});
		string path = Language._persistentDataPath + "/" + text + ".xml";
		if (File.Exists(path))
		{
			return File.ReadAllText(path);
		}
		TextAsset textAsset = (TextAsset)Resources.Load(text, typeof(TextAsset));
		return textAsset.text;
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x00018A3C File Offset: 0x00016C3C
	public static LanguageCode CurrentLanguage()
	{
		return Language.currentLanguage;
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x00018A44 File Offset: 0x00016C44
	public static string Get(string key)
	{
		if (key.StartsWith("!!"))
		{
			return Language.Get(key, Language.settings.sheetTitles[0]);
		}
		return key;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x00018A78 File Offset: 0x00016C78
	public static string Get(string key, string sheetTitle)
	{
		if (Language.currentEntrySheets == null || !Language.currentEntrySheets.ContainsKey(sheetTitle))
		{
			Debug.LogError("The sheet with title \"" + sheetTitle + "\" does not exist!");
			return string.Empty;
		}
		if (Language.currentEntrySheets[sheetTitle].ContainsKey(key))
		{
			return (string)Language.currentEntrySheets[sheetTitle][key];
		}
		return "MISSING LANG:" + key;
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x00018AF4 File Offset: 0x00016CF4
	private static void SendMonoMessage(string methodString, params object[] parameters)
	{
		if (parameters != null && parameters.Length > 1)
		{
			Debug.LogError("We cannot pass more than one argument currently!");
		}
		GameObject[] array = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject gameObject in array)
		{
			if (gameObject && gameObject.transform.parent == null)
			{
				if (parameters != null && parameters.Length == 1)
				{
					gameObject.gameObject.BroadcastMessage(methodString, parameters[0], SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					gameObject.gameObject.BroadcastMessage(methodString, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x00018B9C File Offset: 0x00016D9C
	public static LanguageCode LanguageNameToCode(SystemLanguage name)
	{
		if (name == SystemLanguage.Afrikaans)
		{
			return LanguageCode.AF;
		}
		if (name == SystemLanguage.Arabic)
		{
			return LanguageCode.AR;
		}
		if (name == SystemLanguage.Basque)
		{
			return LanguageCode.BA;
		}
		if (name == SystemLanguage.Belarusian)
		{
			return LanguageCode.BE;
		}
		if (name == SystemLanguage.Bulgarian)
		{
			return LanguageCode.BG;
		}
		if (name == SystemLanguage.Catalan)
		{
			return LanguageCode.CA;
		}
		if (name == SystemLanguage.Chinese)
		{
			return LanguageCode.ZH;
		}
		if (name == SystemLanguage.Czech)
		{
			return LanguageCode.CS;
		}
		if (name == SystemLanguage.Danish)
		{
			return LanguageCode.DA;
		}
		if (name == SystemLanguage.Dutch)
		{
			return LanguageCode.NL;
		}
		if (name == SystemLanguage.English)
		{
			return LanguageCode.EN;
		}
		if (name == SystemLanguage.Estonian)
		{
			return LanguageCode.ET;
		}
		if (name == SystemLanguage.Faroese)
		{
			return LanguageCode.FA;
		}
		if (name == SystemLanguage.Finnish)
		{
			return LanguageCode.FI;
		}
		if (name == SystemLanguage.French)
		{
			return LanguageCode.FR;
		}
		if (name == SystemLanguage.German)
		{
			return LanguageCode.DE;
		}
		if (name == SystemLanguage.Greek)
		{
			return LanguageCode.EL;
		}
		if (name == SystemLanguage.Hebrew)
		{
			return LanguageCode.HE;
		}
		if (name == SystemLanguage.Hugarian)
		{
			return LanguageCode.HU;
		}
		if (name == SystemLanguage.Icelandic)
		{
			return LanguageCode.IS;
		}
		if (name == SystemLanguage.Indonesian)
		{
			return LanguageCode.ID;
		}
		if (name == SystemLanguage.Italian)
		{
			return LanguageCode.IT;
		}
		if (name == SystemLanguage.Japanese)
		{
			return LanguageCode.JA;
		}
		if (name == SystemLanguage.Korean)
		{
			return LanguageCode.KO;
		}
		if (name == SystemLanguage.Latvian)
		{
			return LanguageCode.LA;
		}
		if (name == SystemLanguage.Lithuanian)
		{
			return LanguageCode.LT;
		}
		if (name == SystemLanguage.Norwegian)
		{
			return LanguageCode.NO;
		}
		if (name == SystemLanguage.Polish)
		{
			return LanguageCode.PL;
		}
		if (name == SystemLanguage.Portuguese)
		{
			return LanguageCode.PT;
		}
		if (name == SystemLanguage.Romanian)
		{
			return LanguageCode.RO;
		}
		if (name == SystemLanguage.Russian)
		{
			return LanguageCode.RU;
		}
		if (name == SystemLanguage.SerboCroatian)
		{
			return LanguageCode.SH;
		}
		if (name == SystemLanguage.Slovak)
		{
			return LanguageCode.SK;
		}
		if (name == SystemLanguage.Slovenian)
		{
			return LanguageCode.SL;
		}
		if (name == SystemLanguage.Spanish)
		{
			return LanguageCode.ES;
		}
		if (name == SystemLanguage.Swedish)
		{
			return LanguageCode.SW;
		}
		if (name == SystemLanguage.Thai)
		{
			return LanguageCode.TH;
		}
		if (name == SystemLanguage.Turkish)
		{
			return LanguageCode.TR;
		}
		if (name == SystemLanguage.Ukrainian)
		{
			return LanguageCode.UK;
		}
		if (name == SystemLanguage.Vietnamese)
		{
			return LanguageCode.VI;
		}
		if (name == SystemLanguage.Hugarian)
		{
			return LanguageCode.HU;
		}
		if (name == SystemLanguage.Unknown)
		{
			return LanguageCode.N;
		}
		return LanguageCode.N;
	}

	// Token: 0x040003A2 RID: 930
	public static string settingsAssetPath = "Assets/Localization/Resources/Languages/LocalizationSettings.asset";

	// Token: 0x040003A3 RID: 931
	public static LocalizationSettings settings = (LocalizationSettings)Resources.Load("Languages/" + Path.GetFileNameWithoutExtension(Language.settingsAssetPath), typeof(LocalizationSettings));

	// Token: 0x040003A4 RID: 932
	public static string Backup_settingsAssetPath = "Assets/Localization/Resources/Languages/BackupLocalizationSettings.asset";

	// Token: 0x040003A5 RID: 933
	public static LocalizationSettings backup_settings = (LocalizationSettings)Resources.Load("Languages/" + Path.GetFileNameWithoutExtension(Language.Backup_settingsAssetPath), typeof(LocalizationSettings));

	// Token: 0x040003A6 RID: 934
	public static List<string> supportedLanguages = new List<string>(new string[]
	{
		"de",
		"en",
		"es",
		"fr",
		"it",
		"latam",
		"nl",
		"pt",
		"ru"
	});

	// Token: 0x040003A7 RID: 935
	private static Dictionary<string, UnityEngine.Object> assets = new Dictionary<string, UnityEngine.Object>();

	// Token: 0x040003A8 RID: 936
	private static List<string> availableLanguages;

	// Token: 0x040003A9 RID: 937
	private static LanguageCode currentLanguage = LanguageCode.N;

	// Token: 0x040003AA RID: 938
	private static Dictionary<string, Hashtable> currentEntrySheets;

	// Token: 0x040003AB RID: 939
	private static string _persistentDataPath = string.Empty;

	// Token: 0x040003AC RID: 940
	private static AndroidJavaClass _pAndroidLocal = null;

	// Token: 0x040003AD RID: 941
	private static string _sDeviceLocal = "EN";

	// Token: 0x040003AE RID: 942
	private static string _sDeviceLanguage = "EN";
}
