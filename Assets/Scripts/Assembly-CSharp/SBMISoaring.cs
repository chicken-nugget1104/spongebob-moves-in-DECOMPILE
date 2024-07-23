using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000345 RID: 837
public class SBMISoaring
{
	// Token: 0x17000330 RID: 816
	// (get) Token: 0x060017F9 RID: 6137 RVA: 0x0009EBBC File Offset: 0x0009CDBC
	// (set) Token: 0x060017FA RID: 6138 RVA: 0x0009EBF4 File Offset: 0x0009CDF4
	public static long PatchTownTimestamp
	{
		get
		{
			SoaringValue soaringValue = (SoaringValue)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_timestamp_key");
			if (soaringValue == null)
			{
				return 0L;
			}
			return soaringValue;
		}
		set
		{
			Soaring.Player.PrivateData_Safe.setValue(value, "SBMI_friends_timestamp_key");
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x060017FB RID: 6139 RVA: 0x0009EC10 File Offset: 0x0009CE10
	// (set) Token: 0x060017FC RID: 6140 RVA: 0x0009EC48 File Offset: 0x0009CE48
	public static long PatchTownTreasureSpawnTimestamp
	{
		get
		{
			SoaringValue soaringValue = (SoaringValue)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_treasurespawntimestamp_key");
			if (soaringValue == null)
			{
				return 0L;
			}
			return soaringValue;
		}
		set
		{
			Soaring.Player.PrivateData_Safe.setValue(value, "SBMI_friends_treasurespawntimestamp_key");
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x060017FD RID: 6141 RVA: 0x0009EC64 File Offset: 0x0009CE64
	// (set) Token: 0x060017FE RID: 6142 RVA: 0x0009EC94 File Offset: 0x0009CE94
	public static int PatchTownTreasureCollected
	{
		get
		{
			SoaringValue soaringValue = Soaring.Player.PrivateData_Safe.soaringValue("SBMI_friends_chestscollected_key");
			if (soaringValue == null)
			{
				return 0;
			}
			return soaringValue;
		}
		set
		{
			Soaring.Player.PrivateData_Safe.setValue(value, "SBMI_friends_chestscollected_key");
		}
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x0009ECB0 File Offset: 0x0009CEB0
	public static void Initialize(SoaringDelegate del)
	{
		if (Soaring.IsInitialized)
		{
			return;
		}
		SoaringPlatformType soaringPlatformType;
		if (TFUtils.isAmazon())
		{
			soaringPlatformType = SoaringPlatformType.Amazon;
		}
		else
		{
			soaringPlatformType = SoaringPlatformType.Android;
		}
		SoaringMode mode = SoaringMode.Development;
		if (SBSettings.SoaringProduction)
		{
			mode = SoaringMode.Production;
		}
		else if (SBSettings.SoaringQA)
		{
			mode = SoaringMode.Testing;
		}
		Soaring.StartSoaring("5239a9edbecd6a054f000001", del, mode, soaringPlatformType);
		try
		{
			if (soaringPlatformType == SoaringPlatformType.Amazon)
			{
				CommonUtils.SetMemoryLevel(768);
			}
			else
			{
				SoaringPlatformAndroid soaringPlatformAndroid = (SoaringPlatformAndroid)SoaringPlatform.GetDelegate();
				CommonUtils.SetMemoryLevel((int)soaringPlatformAndroid.TotalMemory);
				SoaringDebug.Log("MEMORY: " + soaringPlatformAndroid.TotalMemory.ToString());
			}
		}
		catch
		{
		}
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x0009ED7C File Offset: 0x0009CF7C
	public void ResetCachedData()
	{
		SBMISoaring.mDailyBonusCalendar = null;
		SBMISoaring.mCurrentDailyBonusDay = -1;
		SBMISoaring.mAlreadyCollected = false;
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x0009ED90 File Offset: 0x0009CF90
	public static void OnInitializeSoaring()
	{
		string serverContentUrl = Soaring.ServerContentUrl;
		if (!SBSettings.IsLastRunVersion)
		{
			SoaringInternal.instance.Versions.ClearAllContent();
			SBSettings.Init();
		}
		SoaringInternal.instance.mSoaringLanguage = Language.CurrentLanguage();
		Soaring.SetGameVersion(SBSettings.LOCAL_BUNDLE_VERSION);
		Soaring.SetAdServerURL(serverContentUrl);
		string versionName = "manifest.json";
		Soaring.SetVersionedFileRepo(serverContentUrl + SBSettings.MANIFEST_FILE, serverContentUrl, TFUtils.GetPersistentAssetsPath(), versionName);
		SoaringInternal.instance.Versions.MaxActiveConnections = SBSettings.PATCHING_FILE_LIMIT;
		string b = CommonUtils.TextureLod().ToString();
		SoaringInternal.instance.Versions.SubContentCategories.addObject(b);
		SBMISoaring.RegisterModules();
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x0009EE40 File Offset: 0x0009D040
	private static void RegisterModules()
	{
		SoaringInternal.instance.RegisterModule(new SBMIAquireEventGiftModule());
		SoaringInternal.instance.RegisterModule(new SBMISetEventValueModule());
		SoaringInternal.instance.RegisterModule(new SBMIMoveAccountModule());
		SoaringInternal.instance.RegisterModule(new SBMIFinalizeMigrationModule());
		SoaringInternal.instance.RegisterModule(new SBMIRetrieveDailyBonusCalendarModule());
		SoaringInternal.instance.RegisterModule(new SBMIRetrieveSessionFromUserModule());
		SoaringInternal.instance.RegisterModule(new SBMIAddCredentialsToUserModule());
		SoaringInternal.instance.RegisterModule(new SBMIAddCharacterFoodModule());
		if (!SoaringInternal.IsProductionMode)
		{
			SoaringInternal.instance.RegisterModule(new SBMIResetEventValueModule());
		}
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x0009EEE0 File Offset: 0x0009D0E0
	public static SoaringDictionary ConvertDictionary(Dictionary<string, object> dict)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (dict == null)
		{
			return soaringDictionary;
		}
		foreach (KeyValuePair<string, object> keyValuePair in dict)
		{
			object value = keyValuePair.Value;
			Type type = value.GetType();
			if (type == typeof(string))
			{
				soaringDictionary.addValue(TFUtils.TryLoadString(dict, keyValuePair.Key), keyValuePair.Key);
			}
			else if (type == typeof(int))
			{
				SoaringDictionary soaringDictionary2 = soaringDictionary;
				int? num = TFUtils.TryLoadInt(dict, keyValuePair.Key);
				soaringDictionary2.addValue((num == null) ? null : num.Value, keyValuePair.Key);
			}
			else if (type == typeof(float))
			{
				SoaringDictionary soaringDictionary3 = soaringDictionary;
				float? num2 = TFUtils.TryLoadFloat(dict, keyValuePair.Key);
				soaringDictionary3.addValue((num2 == null) ? null : num2.Value, keyValuePair.Key);
			}
			else if (type == typeof(long) || type == typeof(ulong))
			{
				SoaringDictionary soaringDictionary4 = soaringDictionary;
				long? num3 = TFUtils.TryLoadLong(dict, keyValuePair.Key);
				soaringDictionary4.addValue((num3 == null) ? null : num3.Value, keyValuePair.Key);
			}
			else if (type == typeof(bool))
			{
				SoaringDictionary soaringDictionary5 = soaringDictionary;
				bool? flag = TFUtils.TryLoadBool(dict, keyValuePair.Key);
				soaringDictionary5.addValue((flag == null) ? null : flag.Value, keyValuePair.Key);
			}
			else if (type == typeof(Dictionary<string, object>))
			{
				soaringDictionary.addValue(SBMISoaring.ConvertDictionary((Dictionary<string, object>)keyValuePair.Value), keyValuePair.Key);
			}
			else if (type == typeof(List<object>))
			{
				soaringDictionary.addValue(SBMISoaring.ConvertArray((List<object>)keyValuePair.Value), keyValuePair.Key);
			}
			else if (type == typeof(List<Dictionary<string, object>>))
			{
				soaringDictionary.addValue(SBMISoaring.ConvertArray((List<Dictionary<string, object>>)keyValuePair.Value), keyValuePair.Key);
			}
			else
			{
				TFUtils.ErrorLog("ConvertDictionary: Unknown Type: " + keyValuePair.Key + " : " + type.ToString());
			}
		}
		return soaringDictionary;
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x0009F1A0 File Offset: 0x0009D3A0
	private static SoaringArray ConvertArray(List<object> list)
	{
		SoaringArray soaringArray = new SoaringArray();
		if (list == null)
		{
			return soaringArray;
		}
		foreach (object obj in list)
		{
			Type type = obj.GetType();
			if (type == typeof(string))
			{
				soaringArray.addObject((string)obj);
			}
			else if (type == typeof(int))
			{
				soaringArray.addObject(TFUtils.LoadIntObjectHelper(obj));
			}
			else if (type == typeof(float))
			{
				SoaringArray soaringArray2 = soaringArray;
				float? num = TFUtils.LoadFloatObjectHelper(obj);
				soaringArray2.addObject((num == null) ? null : num.Value);
			}
			else if (type == typeof(long) || type == typeof(ulong))
			{
				soaringArray.addObject(TFUtils.LoadLongObjectHelper(obj));
			}
			else if (type == typeof(bool))
			{
				SoaringArray soaringArray3 = soaringArray;
				bool? flag = TFUtils.LoadBoolObjectHelper(obj);
				soaringArray3.addObject((flag == null) ? null : flag.Value);
			}
			else if (type == typeof(Dictionary<string, object>))
			{
				soaringArray.addObject(SBMISoaring.ConvertDictionary((Dictionary<string, object>)obj));
			}
			else if (type == typeof(List<object>))
			{
				soaringArray.addObject(SBMISoaring.ConvertArray((List<object>)obj));
			}
			else
			{
				TFUtils.ErrorLog(string.Concat(new object[]
				{
					"ConvertArray: Unknown Type: ",
					obj,
					" : ",
					type.ToString()
				}));
			}
		}
		return soaringArray;
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x0009F390 File Offset: 0x0009D590
	private static SoaringArray ConvertArray(List<Dictionary<string, object>> list)
	{
		SoaringArray soaringArray = new SoaringArray();
		if (list == null)
		{
			return soaringArray;
		}
		foreach (Dictionary<string, object> dict in list)
		{
			soaringArray.addObject(SBMISoaring.ConvertDictionary(dict));
		}
		return soaringArray;
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x0009F408 File Offset: 0x0009D608
	public static Dictionary<string, object> ConvertDictionaryToGeneric(SoaringDictionary dict)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (dict == null)
		{
			return dictionary;
		}
		SoaringObjectBase[] array = dict.allValues();
		string[] array2 = dict.allKeys();
		int i = 0;
		while (i < dict.count())
		{
			SoaringObjectBase soaringObjectBase = array[i];
			SoaringObjectBase.IsType type = soaringObjectBase.Type;
			switch (type)
			{
			case SoaringObjectBase.IsType.Int:
				dictionary.Add(array2[i], (SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.Float:
				dictionary.Add(array2[i], (SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.String:
				dictionary.Add(array2[i], (SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.Array:
				dictionary.Add(array2[i], SBMISoaring.ConvertArrayToGeneric((SoaringArray)soaringObjectBase));
				break;
			case SoaringObjectBase.IsType.Dictionary:
				dictionary.Add(array2[i], SBMISoaring.ConvertDictionaryToGeneric((SoaringDictionary)soaringObjectBase));
				break;
			case SoaringObjectBase.IsType.Object:
			case SoaringObjectBase.IsType.Module:
			case SoaringObjectBase.IsType.Management:
				goto IL_123;
			case SoaringObjectBase.IsType.Boolean:
				dictionary.Add(array2[i], (SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.Null:
				dictionary.Add(array2[i], null);
				break;
			default:
				goto IL_123;
			}
			IL_14B:
			i++;
			continue;
			IL_123:
			TFUtils.ErrorLog("ConvertDictionary: Unknown Type: " + array2[i] + " : " + type.ToString());
			goto IL_14B;
		}
		return dictionary;
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x0009F574 File Offset: 0x0009D774
	private static List<object> ConvertArrayToGeneric(SoaringArray list)
	{
		List<object> list2 = new List<object>();
		if (list == null)
		{
			return list2;
		}
		int i = 0;
		while (i < list.count())
		{
			SoaringObjectBase soaringObjectBase = list.objectAtIndex(i);
			SoaringObjectBase.IsType type = soaringObjectBase.Type;
			switch (type)
			{
			case SoaringObjectBase.IsType.Int:
				list2.Add((SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.Float:
				list2.Add((SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.String:
				list2.Add((SoaringValue)soaringObjectBase);
				break;
			case SoaringObjectBase.IsType.Array:
				list2.Add(SBMISoaring.ConvertArrayToGeneric((SoaringArray)soaringObjectBase));
				break;
			case SoaringObjectBase.IsType.Dictionary:
				list2.Add(SBMISoaring.ConvertDictionaryToGeneric((SoaringDictionary)soaringObjectBase));
				break;
			case SoaringObjectBase.IsType.Object:
			case SoaringObjectBase.IsType.Module:
			case SoaringObjectBase.IsType.Management:
				goto IL_EA;
			case SoaringObjectBase.IsType.Boolean:
				list2.Add((SoaringValue)soaringObjectBase);
				break;
			default:
				goto IL_EA;
			}
			IL_121:
			i++;
			continue;
			IL_EA:
			TFUtils.ErrorLog(string.Concat(new object[]
			{
				"ConvertArray: Unknown Type: ",
				soaringObjectBase,
				" : ",
				type.ToString()
			}));
			goto IL_121;
		}
		return list2;
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x0009F6B4 File Offset: 0x0009D8B4
	public static void SetEventValue(Session session, SoaringValue event_id, SoaringValue event_value, SoaringContext context = null)
	{
		if (context == null)
		{
			context = SBMISoaring.CheckContext();
		}
		context.addValue(event_id, "eventDid");
		if (!Soaring.IsAuthorized)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidAuthCodeError(), context, "setEventValue");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(event_id, "eventDid");
		soaringDictionary.addValue(event_value, "value");
		SoaringInternal.instance.CallModule("setEventValue", soaringDictionary, context);
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x0009F728 File Offset: 0x0009D928
	public static void GetEventValue(Session session, SoaringValue event_id, SoaringContext context = null)
	{
		if (context == null)
		{
			context = SBMISoaring.CheckContext();
		}
		context.addValue(event_id, "eventDid");
		if (!Soaring.IsAuthorized)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidAuthCodeError(), context, "getEventValue");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(event_id, "eventDid");
		SoaringInternal.instance.CallModule("getEventValue", soaringDictionary, context);
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x0009F790 File Offset: 0x0009D990
	public static void AddFoodToCharacter(SoaringValue value, SoaringValue characterDID, int day = -1, SoaringContext context = null)
	{
		if (context == null)
		{
			context = SBMISoaring.CheckContext();
		}
		context.addValue(value, "value");
		context.addValue(characterDID, "characterDid");
		if (!Soaring.IsAuthorized)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidAuthCodeError(), context, "addFoodToCharacter");
			return;
		}
		if (value == null)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidParametersError("value"), context, "addFoodToCharacter");
			return;
		}
		if (characterDID == null)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidParametersError("characterDid"), context, "addFoodToCharacter");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(value, "value");
		soaringDictionary.addValue(characterDID, "characterDid");
		if (!SBSettings.SoaringProduction && day != -1)
		{
			soaringDictionary.addValue(day, "__day__");
		}
		SoaringInternal.instance.CallModule("addFoodToCharacter", soaringDictionary, context);
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x0009F868 File Offset: 0x0009DA68
	public static void AquireEventGift(Session session, SoaringValue event_id, SoaringValue gift_id, int purchaseCost, bool purchased = false, SoaringContext context = null)
	{
		if (context == null)
		{
			context = SBMISoaring.CheckContext();
		}
		context.addValue(event_id, "eventDid");
		context.addValue(gift_id, "giftDid");
		context.addValue(purchased, "purchased");
		context.addValue(purchaseCost, "purchaseCost");
		if (!Soaring.IsAuthorized)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidAuthCodeError(), context, "acquireEventGift");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(event_id, "eventDid");
		soaringDictionary.addValue(gift_id, "giftDid");
		if (purchased)
		{
			soaringDictionary.addValue(purchased, "purchased");
		}
		SoaringInternal.instance.CallModule("acquireEventGift", soaringDictionary, context);
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x0009F928 File Offset: 0x0009DB28
	public static void ResetEventGifts(Session session, SoaringValue event_id, SoaringContext context = null)
	{
		if (context == null)
		{
			context = SBMISoaring.CheckContext();
		}
		context.addValue(event_id, "eventDid");
		if (!Soaring.IsAuthorized)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidAuthCodeError(), context, "resetEventGifts");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(event_id, "eventDid");
		SoaringInternal.instance.CallModule("resetEventGifts", soaringDictionary, context);
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x0009F990 File Offset: 0x0009DB90
	public static void FinalizeMigration(string playerID, SoaringLoginType type, SoaringContext context)
	{
		if (string.IsNullOrEmpty(playerID))
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidCredentialsError("Invalid Player ID"), context, "finalizeMigration");
			return;
		}
		string b = "device";
		if (type != SoaringLoginType.Soaring && type != SoaringLoginType.Device && TFUtils.isAmazon())
		{
			b = "amazon";
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(playerID, "playerId");
		soaringDictionary.addValue(b, "accountType");
		playerID = SystemInfo.deviceUniqueIdentifier;
		soaringDictionary.addValue(playerID, "croppedPlayerId");
		soaringDictionary.addValue(SoaringInternal.instance.GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary.addValue(SoaringInternal.instance.GenerateAppDataDictionary(), "appInfo");
		SoaringInternal.instance.CallModule("finalizeMigration", soaringDictionary, context);
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x0009FA60 File Offset: 0x0009DC60
	public static void MigratePlayerToNewPlayer(string srcPlayerID, SoaringLoginType srcType, string targetPlayerID, SoaringLoginType targetType, SoaringContext context)
	{
		if (!Soaring.IsAuthorized)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidAuthCodeError(), context, "moveAccount");
			return;
		}
		if (string.IsNullOrEmpty(srcPlayerID) && string.IsNullOrEmpty(targetPlayerID))
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidCredentialsError("Invalid User Id"), context, "moveAccount");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!string.IsNullOrEmpty(targetPlayerID))
		{
			SoaringDictionary soaringDictionary2 = new SoaringDictionary();
			soaringDictionary.addValue(soaringDictionary2, "target");
			soaringDictionary2.addValue(targetPlayerID, SoaringInternal.instance.PlatformKeyWithLoginType(targetType, true));
		}
		if (!string.IsNullOrEmpty(srcPlayerID))
		{
			SoaringDictionary soaringDictionary3 = new SoaringDictionary();
			soaringDictionary.addValue(soaringDictionary3, "source");
			soaringDictionary3.addValue(srcPlayerID, SoaringInternal.instance.PlatformKeyWithLoginType(srcType, true));
		}
		SoaringInternal.instance.CallModule("moveAccount", soaringDictionary, context);
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x0009FB3C File Offset: 0x0009DD3C
	public static void RetrieveDailyBonuseCalendar(int day = -1, SoaringContext context = null, SoaringContextDelegate context_delegate = null)
	{
		if (context == null)
		{
			context = new SoaringContext();
			context.Name = "DailyBonus";
			context.Responder = new SBMISoaring.SMBICacheDelegate();
		}
		if (context_delegate != null)
		{
			context.ContextResponder = context_delegate;
		}
		if (!Soaring.IsAuthorized)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidAuthCodeError(), context, "retrieveDailyBonusCalendar");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!SBSettings.SoaringProduction && day != -1)
		{
			soaringDictionary.addValue(day, "__day__");
		}
		SoaringInternal.instance.CallModule("retrieveDailyBonusCalendar", soaringDictionary, context);
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x0009FBD0 File Offset: 0x0009DDD0
	public static void RetrieveUsersSession(SoaringContext context = null)
	{
		if (context == null)
		{
			context = new SoaringContext();
			context.Name = "RetrieveUsersSession";
			context.Responder = new SBMISoaring.SMBICacheDelegate();
		}
		if (!Soaring.IsAuthorized)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidAuthCodeError(), context, "retrieveSessionFromUser");
			return;
		}
		SoaringDictionary data = new SoaringDictionary();
		SoaringInternal.instance.CallModule("retrieveSessionFromUser", data, context);
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x0009FC34 File Offset: 0x0009DE34
	public static void AddCredentialsToUsers(SoaringArray identifiers, SoaringContext context = null)
	{
		if (context == null)
		{
			context = new SoaringContext();
			context.Name = "RetrieveUsersSession";
			context.Responder = new SBMISoaring.SMBICacheDelegate();
		}
		if (!Soaring.IsAuthorized)
		{
			SBMISoaring.CallbackFailedModule(SBMISoaring.CreateInvalidAuthCodeError(), context, "addCredentialsToUser");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(identifiers, "identifiers");
		SoaringInternal.instance.CallModule("addCredentialsToUser", soaringDictionary, context);
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x0009FCA4 File Offset: 0x0009DEA4
	public static SoaringArray<SBMISoaring.SBMIDailyBonusDay> GetCachedDailyBonus(ref int day, ref bool alreadyCollected)
	{
		alreadyCollected = SBMISoaring.mAlreadyCollected;
		day = SBMISoaring.mCurrentDailyBonusDay;
		return SBMISoaring.mDailyBonusCalendar;
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x0009FCBC File Offset: 0x0009DEBC
	private static void CallbackFailedModule(SoaringError error, SoaringContext context, string moduleName)
	{
		if (context == null)
		{
			context = SBMISoaring.CheckContext();
		}
		if (context != null && context.Responder != null)
		{
			context.Responder.OnComponentFinished(false, moduleName, error, null, context);
			return;
		}
		Soaring.Delegate.OnComponentFinished(false, moduleName, error, null, context);
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x0009FD08 File Offset: 0x0009DF08
	private static SoaringError CreateInvalidAuthCodeError()
	{
		return new SoaringError("Invalid User Auth Code.", -2);
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x0009FD18 File Offset: 0x0009DF18
	private static SoaringError CreateInvalidCredentialsError(string str)
	{
		return new SoaringError(str, -3);
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x0009FD24 File Offset: 0x0009DF24
	private static SoaringError CreateInvalidParametersError(string param)
	{
		return new SoaringError("Invalid Parameter: " + param, -9);
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x0009FD38 File Offset: 0x0009DF38
	private static SoaringContext CheckContext()
	{
		SoaringContext soaringContext = "CommunityEvents";
		soaringContext.Responder = new SoaringCommunityEventDelegate();
		return soaringContext;
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x0009FD5C File Offset: 0x0009DF5C
	private static SoaringError CheckForError(string error)
	{
		if (error == null)
		{
			return null;
		}
		string text = error.ToLower();
		SoaringError result;
		if (text.Contains("is not defined"))
		{
			result = new SoaringError(error, 404);
		}
		else if (text.Contains("is not active"))
		{
			result = new SoaringError(error, 404);
		}
		else
		{
			result = new SoaringError(error, -1);
		}
		return result;
	}

	// Token: 0x04000FFD RID: 4093
	public const string SBMI_Friends_Dialog_Key = "SBMI_fdk";

	// Token: 0x04000FFE RID: 4094
	public const string SBMI_CompletedQuest_Key = "SBMI_completed_quest_key";

	// Token: 0x04000FFF RID: 4095
	public const string SBMI_Friends_Reward_Key = "SBMI_friends_reward_key";

	// Token: 0x04001000 RID: 4096
	public const string SBMI_Friends_CoinReward_Key = "SBMI_friends_coinreward_key";

	// Token: 0x04001001 RID: 4097
	public const string SBMI_Friends_JellyReward_Key = "SBMI_friends_jellyreward_key";

	// Token: 0x04001002 RID: 4098
	public const string SBMI_Friends_XPReward_Key = "SBMI_friends_xpreward_key";

	// Token: 0x04001003 RID: 4099
	public const string SBMI_Friends_TimeStampReward_Key = "SBMI_friends_timestampreward_key";

	// Token: 0x04001004 RID: 4100
	public const string SBMI_Friends_ChestsCollected_Key = "SBMI_friends_chestscollected_key";

	// Token: 0x04001005 RID: 4101
	public const string SBMI_Friends_TimeStamp_Key = "SBMI_friends_timestamp_key";

	// Token: 0x04001006 RID: 4102
	public const string SBMI_Friends_TreasureSpawnTimeStamp_Key = "SBMI_friends_treasurespawntimestamp_key";

	// Token: 0x04001007 RID: 4103
	private static int mCurrentDailyBonusDay = -1;

	// Token: 0x04001008 RID: 4104
	private static SoaringArray<SBMISoaring.SBMIDailyBonusDay> mDailyBonusCalendar;

	// Token: 0x04001009 RID: 4105
	private static bool mAlreadyCollected;

	// Token: 0x02000346 RID: 838
	public class SBMIDailyBonusDay : SoaringObjectBase
	{
		// Token: 0x06001819 RID: 6169 RVA: 0x0009FDC8 File Offset: 0x0009DFC8
		public SBMIDailyBonusDay(SoaringDictionary data) : base(SoaringObjectBase.IsType.Object)
		{
			this.mData = data;
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0009FDD8 File Offset: 0x0009DFD8
		private int GetSoaringValue(string str)
		{
			if (this.mData == null || str == null)
			{
				return -1;
			}
			return this.mData.soaringValue(str);
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600181B RID: 6171 RVA: 0x0009FE0C File Offset: 0x0009E00C
		public int Day
		{
			get
			{
				return this.GetSoaringValue("day");
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600181C RID: 6172 RVA: 0x0009FE1C File Offset: 0x0009E01C
		public int CurrencyDID
		{
			get
			{
				return this.GetSoaringValue("did");
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x0600181D RID: 6173 RVA: 0x0009FE2C File Offset: 0x0009E02C
		public int CurrencyAmount
		{
			get
			{
				return this.GetSoaringValue("value");
			}
		}

		// Token: 0x0400100A RID: 4106
		private SoaringDictionary mData;
	}

	// Token: 0x02000347 RID: 839
	public class SMBICacheDelegate : SoaringDelegate
	{
		// Token: 0x0600181F RID: 6175 RVA: 0x0009FE44 File Offset: 0x0009E044
		public bool IsError(bool success, SoaringError err, SoaringDictionary data)
		{
			return !success || err != null || data == null;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0009FE5C File Offset: 0x0009E05C
		public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
			if (string.IsNullOrEmpty(module))
			{
				return;
			}
			if (module == "retrieveDailyBonusCalendar")
			{
				SBMISoaring.mAlreadyCollected = false;
				SBMISoaring.mCurrentDailyBonusDay = -1;
				SBMISoaring.mDailyBonusCalendar = null;
				bool b = false;
				if (!this.IsError(success, error, data))
				{
					SoaringObjectBase soaringObjectBase = data.objectWithKey("days");
					if (soaringObjectBase != null)
					{
						SoaringArray soaringArray = (SoaringArray)soaringObjectBase;
						int num = soaringArray.count();
						SBMISoaring.mDailyBonusCalendar = new SoaringArray<SBMISoaring.SBMIDailyBonusDay>(num);
						for (int i = 0; i < num; i++)
						{
							SBMISoaring.SBMIDailyBonusDay obj = new SBMISoaring.SBMIDailyBonusDay((SoaringDictionary)soaringArray.objectAtIndex(i));
							SBMISoaring.mDailyBonusCalendar.addObject(obj);
						}
					}
					soaringObjectBase = data.objectWithKey("currentDay");
					if (soaringObjectBase != null)
					{
						SBMISoaring.mCurrentDailyBonusDay = (SoaringValue)soaringObjectBase;
						if (SBMISoaring.mDailyBonusCalendar != null)
						{
							b = true;
						}
					}
					soaringObjectBase = data.objectWithKey("alreadyCollected");
					if (soaringObjectBase != null)
					{
						SBMISoaring.mAlreadyCollected = (SoaringValue)soaringObjectBase;
					}
				}
				else
				{
					SoaringDebug.Log(string.Concat(new object[]
					{
						"Failed to retrieve daily bonus calendar: ",
						error,
						" : Code: ",
						error.ErrorCode
					}));
				}
				if (context != null && context.ContextResponder != null)
				{
					context.addValue(b, "query");
					if (error != null)
					{
						context.addValue(error, "error_code");
					}
					context.ContextResponder(context);
				}
			}
		}
	}
}
