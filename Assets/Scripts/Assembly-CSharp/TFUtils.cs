using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Ionic.Zlib;
using MiniJSON;
using MTools;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class TFUtils
{
	// Token: 0x06002289 RID: 8841 RVA: 0x000D3F94 File Offset: 0x000D2194
	// Note: this type is marked as 'beforefieldinit'.
	static TFUtils()
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		TFUtils.EPOCH = dateTime.ToUniversalTime();
		TFUtils.APP_START_TIME = TFUtils.EpochTime();
		TFUtils.LOG_LEVEL = TFUtils.LogLevel.INFO;
		TFUtils.LOG_FILTER = TFUtils.LogFilter.None;
		TFUtils.DebugTimeOffset = 0UL;
		TFUtils.isFastForwarding = false;
		TFUtils.timeMultiplier = 50f;
		TFUtils.FastForwardOffset = 0UL;
		TFUtils.AddTimeOffset = 0UL;
		TFUtils.seedUtcNow = new DateTime(1970, 1, 1);
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x0600228A RID: 8842 RVA: 0x000D400C File Offset: 0x000D220C
	// (set) Token: 0x0600228B RID: 8843 RVA: 0x000D4014 File Offset: 0x000D2214
	public static float TimeMultiplier
	{
		get
		{
			return TFUtils.timeMultiplier;
		}
		set
		{
			TFUtils.timeMultiplier = value;
		}
	}

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x0600228C RID: 8844 RVA: 0x000D401C File Offset: 0x000D221C
	// (set) Token: 0x0600228D RID: 8845 RVA: 0x000D4080 File Offset: 0x000D2280
	public static DateTime UtcNow
	{
		get
		{
			DateTime utcNow = DateTime.UtcNow;
			if (TFUtils.seedUtcNow.Equals(new DateTime(1970, 1, 1)))
			{
				TFUtils.seedUtcNow = utcNow;
			}
			TimeSpan value = TimeSpan.FromTicks((long)((float)utcNow.Subtract(TFUtils.seedUtcNow).Ticks * TFUtils.timeMultiplier));
			return TFUtils.seedUtcNow.Add(value);
		}
		set
		{
			TFUtils.seedUtcNow = value;
		}
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x000D4088 File Offset: 0x000D2288
	public static void Init()
	{
		TFUtils.ConsoleLog = new StringBuilder(string.Empty);
		TFUtils.PrevLog = null;
		TFUtils.ErrorConsoleLog = new StringBuilder(string.Empty);
		TFUtils.ApplicationDataPath = Application.dataPath;
		TFUtils.ApplicationPersistentDataPath = Application.persistentDataPath;
		TFUtils.DeviceId = TFUtils.GetDeviceId();
		TFUtils.DeviceName = SystemInfo.deviceName;
		TFUtils.DebugLog("This device is:" + TFUtils.DeviceId);
	}

	// Token: 0x0600228F RID: 8847 RVA: 0x000D40F8 File Offset: 0x000D22F8
	private static void WriteConsoleLog(string tx)
	{
		if (TFUtils.ConsoleLog.Length >= 131072)
		{
			TFUtils.PrevLog = TFUtils.ConsoleLog;
			TFUtils.ConsoleLog = new StringBuilder(string.Empty);
		}
		TFUtils.ConsoleLog.Append(tx);
	}

	// Token: 0x06002290 RID: 8848 RVA: 0x000D4134 File Offset: 0x000D2334
	public static ulong EpochTime()
	{
		if (TFUtils.isFastForwarding)
		{
			return TFUtils.EpochTime(TFUtils.UtcNow);
		}
		return TFUtils.EpochTime(DateTime.UtcNow);
	}

	// Token: 0x06002291 RID: 8849 RVA: 0x000D4158 File Offset: 0x000D2358
	public static ulong EpochTime(DateTime dt)
	{
		TimeSpan timeSpan = dt - TFUtils.EPOCH;
		if (SBSettings.SoaringProduction)
		{
			return (ulong)timeSpan.TotalSeconds;
		}
		return (ulong)timeSpan.TotalSeconds + TFUtils.FastForwardOffset + TFUtils.AddTimeOffset;
	}

	// Token: 0x06002292 RID: 8850 RVA: 0x000D4198 File Offset: 0x000D2398
	public static DateTime EpochToDateTime(ulong seconds)
	{
		return DateTime.SpecifyKind(TFUtils.EPOCH.AddSeconds(seconds), DateTimeKind.Utc);
	}

	// Token: 0x06002293 RID: 8851 RVA: 0x000D41B0 File Offset: 0x000D23B0
	public static string DurationToString(ulong duration)
	{
		return TFUtils.DurationToString(duration, true);
	}

	// Token: 0x06002294 RID: 8852 RVA: 0x000D41BC File Offset: 0x000D23BC
	public static string DurationToString(ulong duration, bool max0)
	{
		if (max0)
		{
			duration = Math.Max(duration, 0UL);
		}
		if (duration < 60UL)
		{
			return string.Format("{0}s", duration);
		}
		ulong num = duration % 60UL;
		duration -= num;
		ulong num2 = duration / 60UL;
		if (num2 < 60UL)
		{
			if (num == 0UL)
			{
				return string.Format("{0}m", num2);
			}
			return string.Format("{0}m {1}s", num2, num);
		}
		else
		{
			ulong num3 = num2 / 60UL;
			num2 %= 60UL;
			if (num3 < 24UL)
			{
				if (num2 == 0UL)
				{
					return string.Format("{0}h", num3);
				}
				return string.Format("{0}h {1}m", num3, num2);
			}
			else
			{
				ulong num4 = num3 / 24UL;
				num3 %= 24UL;
				if (num3 == 0UL)
				{
					return string.Format("{0}d", num4);
				}
				return string.Format("{0}d {1}h", num4, num3);
			}
		}
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x000D42B8 File Offset: 0x000D24B8
	public static Dictionary<KeyType, ValueType> CloneDictionary<KeyType, ValueType>(Dictionary<KeyType, ValueType> source)
	{
		Dictionary<KeyType, ValueType> dictionary = new Dictionary<KeyType, ValueType>();
		foreach (KeyType key in source.Keys)
		{
			dictionary[key] = source[key];
		}
		return dictionary;
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x000D432C File Offset: 0x000D252C
	public static void CloneDictionaryInPlace<KeyType, ValueType>(Dictionary<KeyType, ValueType> source, Dictionary<KeyType, ValueType> dest)
	{
		dest.Clear();
		foreach (KeyValuePair<KeyType, ValueType> keyValuePair in source)
		{
			dest.Add(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x06002297 RID: 8855 RVA: 0x000D43A0 File Offset: 0x000D25A0
	public static Dictionary<KeyType, ValueType> ConcatenateDictionaryInPlace<KeyType, ValueType>(Dictionary<KeyType, ValueType> dest, Dictionary<KeyType, ValueType> source)
	{
		foreach (KeyType key in source.Keys)
		{
			if (dest.ContainsKey(key))
			{
				throw new ArgumentException("Destination dictionary already contains key " + key.ToString());
			}
			dest[key] = source[key];
		}
		return dest;
	}

	// Token: 0x06002298 RID: 8856 RVA: 0x000D4438 File Offset: 0x000D2638
	public static List<To> CloneAndCastList<From, To>(List<From> list) where From : To
	{
		List<To> list2 = new List<To>(list.Count);
		foreach (From from in list)
		{
			list2.Add((To)((object)from));
		}
		return list2;
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x000D44B0 File Offset: 0x000D26B0
	private static T AssertCast<T>(Dictionary<string, object> dict, string key)
	{
		TFUtils.AssertKeyExists(dict, key);
		TFUtils.Assert(dict[key] is T, string.Format("Could not cast the key({0}) with value({1}) to type({2}) in dictionary{3}", new object[]
		{
			key,
			dict[key],
			typeof(T).ToString(),
			TFUtils.DebugDictToString(dict)
		}));
		return (T)((object)dict[key]);
	}

	// Token: 0x0600229A RID: 8858 RVA: 0x000D4520 File Offset: 0x000D2720
	public static Dictionary<string, object> DeserializeJsonFile(string filePath)
	{
		string json = TFUtils.ReadAllText(filePath);
		return (Dictionary<string, object>)Json.Deserialize(json);
	}

	// Token: 0x0600229B RID: 8859 RVA: 0x000D4540 File Offset: 0x000D2740
	public static string ReadAllText(string filePath)
	{
		string result = string.Empty;
		try
		{
			TFUtils.DebugLog("filePath " + filePath);
			if (filePath.Contains("://"))
			{
				result = TFUtils.LoadWWW(filePath);
			}
			else
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					result = streamReader.ReadToEnd();
				}
			}
		}
		catch (Exception ex)
		{
			TFUtils.WarningLog(ex.Message);
		}
		return result;
	}

	// Token: 0x0600229C RID: 8860 RVA: 0x000D45EC File Offset: 0x000D27EC
	private static string LoadWWW(string filePath)
	{
		TFUtils.DebugLog("------LoadWWW1-----------");
		WWW www = new WWW(filePath);
		TFUtils.DebugLog("------LoadWWW2-----------");
		while (!www.isDone)
		{
		}
		return www.text;
	}

	// Token: 0x0600229D RID: 8861 RVA: 0x000D462C File Offset: 0x000D282C
	public static void AssertKeyExists(Dictionary<string, object> dict, string key)
	{
		if (dict == null)
		{
			TFUtils.Assert(false, string.Format("Can't search for the key '{0}' in a null dictionary", key));
		}
		if (!dict.ContainsKey(key))
		{
			TFUtils.Assert(false, string.Format("Could not find the key '{0}' in the given dictionary:\n{1}", key, TFUtils.DebugDictToString(dict)));
		}
	}

	// Token: 0x0600229E RID: 8862 RVA: 0x000D4674 File Offset: 0x000D2874
	public static bool LoadBool(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertCast<bool>(d, key);
		}
		return (bool)d[key];
	}

	// Token: 0x0600229F RID: 8863 RVA: 0x000D4694 File Offset: 0x000D2894
	public static bool? LoadNullableBool(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(d, key);
		}
		return (bool?)d[key];
	}

	// Token: 0x060022A0 RID: 8864 RVA: 0x000D46B4 File Offset: 0x000D28B4
	public static List<T> TryLoadList<T>(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return TFUtils.LoadList<T>(data, key);
	}

	// Token: 0x060022A1 RID: 8865 RVA: 0x000D46CC File Offset: 0x000D28CC
	public static List<T> LoadList<T>(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(data, key);
		}
		if (data[key] is List<T>)
		{
			return (List<T>)data[key];
		}
		List<object> list = (List<object>)data[key];
		List<T> retval = new List<T>(data.Count);
		list.ForEach(delegate(object obj)
		{
			retval.Add((T)((object)Convert.ChangeType(obj, typeof(T))));
		});
		return retval;
	}

	// Token: 0x060022A2 RID: 8866 RVA: 0x000D4744 File Offset: 0x000D2944
	public static Dictionary<string, object> LoadDict(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(data, key);
		}
		return (Dictionary<string, object>)data[key];
	}

	// Token: 0x060022A3 RID: 8867 RVA: 0x000D4764 File Offset: 0x000D2964
	public static Dictionary<string, object> TryLoadDict(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return (Dictionary<string, object>)data[key];
	}

	// Token: 0x060022A4 RID: 8868 RVA: 0x000D4780 File Offset: 0x000D2980
	public static string LoadString(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertCast<string>(data, key);
		}
		return (string)data[key];
	}

	// Token: 0x060022A5 RID: 8869 RVA: 0x000D47A0 File Offset: 0x000D29A0
	public static string LoadString(Dictionary<string, object> data, string key, string default_val)
	{
		string text = TFUtils.TryLoadString(data, key);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return default_val;
	}

	// Token: 0x060022A6 RID: 8870 RVA: 0x000D47C4 File Offset: 0x000D29C4
	public static string TryLoadString(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		if (SBSettings.AssertDataValidity)
		{
			return TFUtils.AssertCast<string>(data, key);
		}
		return (string)data[key];
	}

	// Token: 0x060022A7 RID: 8871 RVA: 0x000D4800 File Offset: 0x000D2A00
	public static string LoadNullableString(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(data, key);
		}
		return (string)data[key];
	}

	// Token: 0x060022A8 RID: 8872 RVA: 0x000D4820 File Offset: 0x000D2A20
	public static string TryLoadNullableString(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			return (string)data[key];
		}
		return null;
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x000D483C File Offset: 0x000D2A3C
	public static int? LoadNullableInt(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(d, key);
		}
		object obj = d[key];
		if (obj == null)
		{
			return (int?)obj;
		}
		return new int?(TFUtils.LoadInt(d, key));
	}

	// Token: 0x060022AA RID: 8874 RVA: 0x000D487C File Offset: 0x000D2A7C
	public static uint? LoadNullableUInt(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(d, key);
		}
		object obj = d[key];
		if (obj == null)
		{
			return (uint?)obj;
		}
		return new uint?(TFUtils.LoadUint(d, key));
	}

	// Token: 0x060022AB RID: 8875 RVA: 0x000D48BC File Offset: 0x000D2ABC
	public static ulong? LoadNullableUlong(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(d, key);
		}
		object obj = d[key];
		if (obj == null)
		{
			return (ulong?)obj;
		}
		return new ulong?(TFUtils.LoadUlong(d, key, 0UL));
	}

	// Token: 0x060022AC RID: 8876 RVA: 0x000D4900 File Offset: 0x000D2B00
	public static int? TryLoadNullableInt(Dictionary<string, object> d, string key)
	{
		if (d.ContainsKey(key))
		{
			return TFUtils.LoadNullableInt(d, key);
		}
		return null;
	}

	// Token: 0x060022AD RID: 8877 RVA: 0x000D492C File Offset: 0x000D2B2C
	public static uint? TryLoadNullableUInt(Dictionary<string, object> d, string key)
	{
		if (d.ContainsKey(key))
		{
			return TFUtils.LoadNullableUInt(d, key);
		}
		return null;
	}

	// Token: 0x060022AE RID: 8878 RVA: 0x000D4958 File Offset: 0x000D2B58
	public static ulong? TryLoadNullableUlong(Dictionary<string, object> d, string key)
	{
		if (d.ContainsKey(key))
		{
			return TFUtils.LoadNullableUlong(d, key);
		}
		return null;
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x000D4984 File Offset: 0x000D2B84
	public static object NullableToObject(ulong? nullable)
	{
		if (nullable != null)
		{
			return nullable.Value;
		}
		return null;
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x000D49A0 File Offset: 0x000D2BA0
	public static int? TryLoadInt(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			return new int?(TFUtils.LoadIntHelper(data, key));
		}
		return null;
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x000D49D0 File Offset: 0x000D2BD0
	public static int LoadInt(Dictionary<string, object> data, string key, int default_val)
	{
		int? num = TFUtils.TryLoadInt(data, key);
		if (num != null)
		{
			return num.Value;
		}
		return default_val;
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x000D49FC File Offset: 0x000D2BFC
	public static long? TryLoadLong(Dictionary<string, object> data, string key)
	{
		if (data.ContainsKey(key))
		{
			return new long?(TFUtils.LoadLongHelper(data, key));
		}
		return null;
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x000D4A2C File Offset: 0x000D2C2C
	public static bool LoadBoolAsInt(Dictionary<string, object> d, string key)
	{
		return TFUtils.LoadInt(d, key) != 0;
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x000D4A50 File Offset: 0x000D2C50
	public static bool? TryLoadBool(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		if (SBSettings.AssertDataValidity)
		{
			return new bool?(TFUtils.AssertCast<bool>(data, key));
		}
		return new bool?((bool)data[key]);
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x000D4A9C File Offset: 0x000D2C9C
	public static bool LoadBool(Dictionary<string, object> data, string key, bool default_value)
	{
		bool? flag = TFUtils.TryLoadBool(data, key);
		if (flag != null)
		{
			return flag.Value;
		}
		return default_value;
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x000D4AC8 File Offset: 0x000D2CC8
	public static bool? LoadBoolObjectHelper(object obj)
	{
		if (obj != null)
		{
			return new bool?((bool)obj);
		}
		return null;
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x000D4AF0 File Offset: 0x000D2CF0
	public static long LoadLong(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(d, key);
		}
		return Convert.ToInt64(d[key]);
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x000D4B10 File Offset: 0x000D2D10
	public static int LoadInt(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(d, key);
		}
		return TFUtils.LoadIntHelper(d, key);
	}

	// Token: 0x060022B9 RID: 8889 RVA: 0x000D4B2C File Offset: 0x000D2D2C
	private static int LoadIntHelper(Dictionary<string, object> d, string key)
	{
		return Convert.ToInt32(d[key]);
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x000D4B3C File Offset: 0x000D2D3C
	public static int LoadIntObjectHelper(object obj)
	{
		return Convert.ToInt32(obj);
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000D4B44 File Offset: 0x000D2D44
	public static long LoadLongObjectHelper(object obj)
	{
		return Convert.ToInt64(obj);
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000D4B4C File Offset: 0x000D2D4C
	private static long LoadLongHelper(Dictionary<string, object> d, string key)
	{
		return Convert.ToInt64(d[key]);
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x000D4B5C File Offset: 0x000D2D5C
	public static uint LoadUint(Dictionary<string, object> data, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(data, key);
		}
		return TFUtils.LoadUintHelper(data, key);
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000D4B78 File Offset: 0x000D2D78
	public static uint? TryLoadUint(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return new uint?(TFUtils.LoadUintHelper(data, key));
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000D4BA8 File Offset: 0x000D2DA8
	private static uint LoadUintHelper(Dictionary<string, object> data, string key)
	{
		return Convert.ToUInt32(data[key]);
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000D4BB8 File Offset: 0x000D2DB8
	public static ulong LoadUlong(Dictionary<string, object> data, string key, ulong defaultValue = 0UL)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(data, key);
		}
		return TFUtils.LoadUlongHelper(data, key, defaultValue);
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000D4BD4 File Offset: 0x000D2DD4
	public static ulong? TryLoadUlong(Dictionary<string, object> data, string key, ulong defaultValue = 0UL)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		return new ulong?(TFUtils.LoadUlongHelper(data, key, defaultValue));
	}

	// Token: 0x060022C2 RID: 8898 RVA: 0x000D4C04 File Offset: 0x000D2E04
	private static ulong LoadUlongHelper(Dictionary<string, object> data, string key, ulong defaultValue)
	{
		ulong result;
		try
		{
			result = Convert.ToUInt64(data[key]);
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
			Debug.LogError(string.Format("Could not convert value to Uint64!\nKey={0}, Value={1}, Dictionary={2}", key, data[key], TFUtils.DebugDictToString(data)));
			result = defaultValue;
		}
		return result;
	}

	// Token: 0x060022C3 RID: 8899 RVA: 0x000D4C78 File Offset: 0x000D2E78
	public static float? TryLoadFloat(Dictionary<string, object> data, string key)
	{
		if (!data.ContainsKey(key))
		{
			return null;
		}
		if (SBSettings.AssertDataValidity)
		{
			return new float?((float)TFUtils.AssertCast<double>(data, key));
		}
		return new float?(Convert.ToSingle(data[key]));
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x000D4CC4 File Offset: 0x000D2EC4
	public static float? LoadFloatObjectHelper(object obj)
	{
		if (obj != null)
		{
			return new float?(Convert.ToSingle(obj));
		}
		return null;
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x000D4CEC File Offset: 0x000D2EEC
	public static float LoadFloat(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(d, key);
		}
		return Convert.ToSingle(d[key]);
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x000D4D0C File Offset: 0x000D2F0C
	public static double LoadDouble(Dictionary<string, object> d, string key)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.AssertKeyExists(d, key);
		}
		return Convert.ToDouble(d[key]);
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000D4D2C File Offset: 0x000D2F2C
	public static void LoadVector3(out Vector3 v3, Dictionary<string, object> d, float defaultValue)
	{
		v3.x = ((!d.ContainsKey("x")) ? defaultValue : TFUtils.LoadFloat(d, "x"));
		v3.y = ((!d.ContainsKey("y")) ? defaultValue : TFUtils.LoadFloat(d, "y"));
		v3.z = ((!d.ContainsKey("z")) ? defaultValue : TFUtils.LoadFloat(d, "z"));
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000D4DB0 File Offset: 0x000D2FB0
	public static void SaveVector3(Vector3 v3, string name, Dictionary<string, object> d)
	{
		d[name] = new Dictionary<string, object>
		{
			{
				"x",
				v3.x
			},
			{
				"y",
				v3.y
			},
			{
				"z",
				v3.z
			}
		};
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000D4E10 File Offset: 0x000D3010
	public static void LoadVector2(out Vector2 v2, Dictionary<string, object> d, float defaultValue)
	{
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.Assert(!d.ContainsKey("z"), "Don't call LoadVector2 on something that has a z value! (do you want to use LoadVector3?)");
		}
		v2.x = ((!d.ContainsKey("x")) ? defaultValue : TFUtils.LoadFloat(d, "x"));
		v2.y = ((!d.ContainsKey("y")) ? defaultValue : TFUtils.LoadFloat(d, "y"));
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000D4E90 File Offset: 0x000D3090
	public static void LoadVector3(out Vector3 v3, Dictionary<string, object> d)
	{
		TFUtils.LoadVector3(out v3, d, 0f);
	}

	// Token: 0x060022CB RID: 8907 RVA: 0x000D4EA0 File Offset: 0x000D30A0
	public static void LoadVector2(out Vector2 v2, Dictionary<string, object> d)
	{
		TFUtils.LoadVector2(out v2, d, 0f);
	}

	// Token: 0x060022CC RID: 8908 RVA: 0x000D4EB0 File Offset: 0x000D30B0
	public static Vector3 ExpandVector(Vector2 vector)
	{
		return TFUtils.ExpandVector(vector, 0f);
	}

	// Token: 0x060022CD RID: 8909 RVA: 0x000D4EC0 File Offset: 0x000D30C0
	public static Vector3 ExpandVector(Vector2 vector, float z)
	{
		return new Vector3(vector.x, vector.y, z);
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x000D4ED8 File Offset: 0x000D30D8
	public static Vector2 TruncateVector(Vector3 vector)
	{
		return new Vector2(vector.x, vector.y);
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x000D4EF0 File Offset: 0x000D30F0
	public static List<T> GetOrCreateList<T>(Dictionary<string, object> dict, string target)
	{
		if (!dict.ContainsKey(target))
		{
			List<T> list = new List<T>();
			dict.Add(target, list);
			return list;
		}
		if (SBSettings.AssertDataValidity)
		{
			TFUtils.Assert(dict[target] is List<T>, string.Format("Found data at '{0}' but it is not a List<{1}>", target, typeof(T).ToString()));
		}
		if (dict[target] is List<T>)
		{
			return (List<T>)dict[target];
		}
		return new List<T>();
	}

	// Token: 0x060022D0 RID: 8912 RVA: 0x000D4F78 File Offset: 0x000D3178
	public static void TruncateFile(string filePath)
	{
		TFUtils.DeleteFile(filePath);
		using (FileStream fileStream = File.Create(filePath))
		{
			fileStream.Close();
		}
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x000D4FC8 File Offset: 0x000D31C8
	public static void DeleteFile(string filePath)
	{
		if (TFUtils.FileIsExists(filePath))
		{
			File.Delete(filePath);
		}
	}

	// Token: 0x060022D2 RID: 8914 RVA: 0x000D4FDC File Offset: 0x000D31DC
	public static void DeleteExistingGameData()
	{
		if (Directory.Exists(TFUtils.GetPersistentAssetsPath()))
		{
			Directory.Delete(TFUtils.GetPersistentAssetsPath(), true);
		}
	}

	// Token: 0x060022D3 RID: 8915 RVA: 0x000D4FF8 File Offset: 0x000D31F8
	public static string GetPersistentAssetsPath()
	{
		return Path.Combine(TFUtils.ApplicationPersistentDataPath, "Contents");
	}

	// Token: 0x060022D4 RID: 8916 RVA: 0x000D500C File Offset: 0x000D320C
	public static string GetStreamingAssetsPath()
	{
		return string.Concat(new object[]
		{
			"jar:file://",
			TFUtils.ApplicationDataPath,
			Path.DirectorySeparatorChar,
			"!/assets"
		});
	}

	// Token: 0x060022D5 RID: 8917 RVA: 0x000D504C File Offset: 0x000D324C
	public static string GetStreamingAssetsSubfolder(string path)
	{
		return TFUtils.GetStreamingAssetsPath() + Path.DirectorySeparatorChar + path;
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000D5064 File Offset: 0x000D3264
	public static string GetStreamingAssetsFileInDirectory(string path, string filename)
	{
		return TFUtils.GetStreamingAssetsFile(path + Path.DirectorySeparatorChar + filename);
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000D507C File Offset: 0x000D327C
	public static void DeletePersistantFile(string fileName)
	{
		try
		{
			string text = TFUtils.GetPersistentAssetsPath() + Path.DirectorySeparatorChar + fileName;
			if (TFUtils.FileIsExists(text))
			{
				File.Delete(text);
			}
		}
		catch
		{
		}
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x000D50D8 File Offset: 0x000D32D8
	public static string GetStreamingAssetsFile(string fileName)
	{
		string text = TFUtils.GetPersistentAssetsPath() + Path.DirectorySeparatorChar + fileName;
		if (TFUtils.FileIsExists(text))
		{
			return text;
		}
		return TFUtils.GetStreamingAssetsPath() + Path.DirectorySeparatorChar + fileName;
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x000D5120 File Offset: 0x000D3320
	public static string GetStreamingAssetsFile_IgnorePersistant(string fileName)
	{
		return TFUtils.GetStreamingAssetsPath() + Path.DirectorySeparatorChar + fileName;
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x000D5138 File Offset: 0x000D3338
	public static string[] GetFilesInPath(string path, string searchPattern)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string streamingAssetsSubfolder = TFUtils.GetStreamingAssetsSubfolder(path);
		string streamingAssetsPath = TFUtils.GetStreamingAssetsPath();
		foreach (string text in Directory.GetFiles(streamingAssetsSubfolder, searchPattern, SearchOption.AllDirectories))
		{
			string key = text.Substring(streamingAssetsPath.Length);
			dictionary[key] = text;
		}
		string persistentAssetsPath = TFUtils.GetPersistentAssetsPath();
		string path2 = TFUtils.GetPersistentAssetsPath() + Path.DirectorySeparatorChar + path;
		if (Directory.Exists(path2))
		{
			foreach (string text2 in Directory.GetFiles(path2, searchPattern, SearchOption.AllDirectories))
			{
				string key2 = text2.Substring(persistentAssetsPath.Length);
				dictionary[key2] = text2;
			}
		}
		string[] array = new string[dictionary.Count];
		dictionary.Values.CopyTo(array, 0);
		return array;
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x000D5224 File Offset: 0x000D3424
	[Conditional("DEBUG")]
	public static void DebugDict(Dictionary<string, object> d)
	{
		TFUtils.DebugLog(TFUtils.DebugDictToString(d));
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000D5234 File Offset: 0x000D3434
	public static string DebugDictToString(Dictionary<string, object> d)
	{
		return "[Dictionary Debug View]\n" + TFUtils.PrintDict(d, string.Empty);
	}

	// Token: 0x060022DD RID: 8925 RVA: 0x000D524C File Offset: 0x000D344C
	public static string DebugListToString(List<object> l)
	{
		return "[List Debug View]\n" + TFUtils.PrintList(l, string.Empty);
	}

	// Token: 0x060022DE RID: 8926 RVA: 0x000D5264 File Offset: 0x000D3464
	public static string DebugListToString(List<Vector3> list)
	{
		return TFUtils.DebugListToString(list.ConvertAll<object>((Vector3 v) => string.Concat(new object[]
		{
			"\t(",
			v.x,
			",\t",
			v.y,
			",\t",
			v.z,
			")"
		})));
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x000D529C File Offset: 0x000D349C
	public static string DebugListToString(List<Vector2> list)
	{
		return TFUtils.DebugListToString(list.ConvertAll<Vector3>((Vector2 v) => TFUtils.ExpandVector(v)));
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x000D52D4 File Offset: 0x000D34D4
	private static string PrintDict(Dictionary<string, object> d, string lead)
	{
		if (d == null)
		{
			return "null";
		}
		string text = "{\n";
		foreach (string text2 in d.Keys)
		{
			if (d[text2] != null)
			{
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					lead,
					text2,
					":",
					TFUtils.PrintGenericValue(d[text2], lead + " "),
					",\n"
				});
			}
			else
			{
				string text3 = text;
				text = string.Concat(new object[]
				{
					text3,
					lead,
					text2,
					":",
					d[text2],
					",\n"
				});
			}
		}
		return text + lead + "}";
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x000D53DC File Offset: 0x000D35DC
	private static string PrintList(List<object> l, string lead)
	{
		if (l == null)
		{
			return "null";
		}
		string text = "[\n";
		for (int i = 0; i < l.Count; i++)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				lead,
				i,
				":",
				TFUtils.PrintGenericValue(l[i], lead + " "),
				",\n"
			});
		}
		return text + lead + "]";
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x000D5468 File Offset: 0x000D3668
	private static string PrintGenericValue(object v, string lead)
	{
		if (v is Dictionary<string, object>)
		{
			return TFUtils.PrintDict(v as Dictionary<string, object>, lead + " ");
		}
		if (v is Dictionary<int, int>)
		{
			string text = "{ ";
			foreach (KeyValuePair<int, int> keyValuePair in ((Dictionary<int, int>)v))
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"\n ",
					keyValuePair.Key,
					": ",
					keyValuePair.Value,
					","
				});
			}
			text = text.Substring(0, text.Length - 1);
			text += "\n }";
			return text;
		}
		if (v is List<object>)
		{
			return TFUtils.PrintList(v as List<object>, lead + " ");
		}
		if (v == null)
		{
			return "null\n";
		}
		return v.ToString();
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000D5594 File Offset: 0x000D3794
	public static void SetLogType(string settings)
	{
		if (string.IsNullOrEmpty(settings))
		{
			return;
		}
		settings = settings.ToLower();
		bool crashlytics = settings.Contains("crash");
		int logType = 2;
		if (settings.Contains("none"))
		{
			logType = -1;
		}
		else if (settings.Contains("error"))
		{
			logType = 0;
		}
		else if (settings.Contains("warning"))
		{
			logType = 1;
		}
		TFUtils.SetLogType(crashlytics, logType);
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000D560C File Offset: 0x000D380C
	public static void SetLogType(bool crashlytics, int logType)
	{
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000D5610 File Offset: 0x000D3810
	public static void DebugLog(object message, TFUtils.LogFilter filter)
	{
		if ((filter & TFUtils.LOG_FILTER) == filter || message == null)
		{
			return;
		}
		if (SBSettings.ConsoleLoggingEnabled)
		{
			Debug.Log(message);
		}
		if (TFUtils.LOG_LEVEL == TFUtils.LogLevel.INFO && SBSettings.ConsoleLoggingEnabled)
		{
			string text = message.ToString();
			if (text != null && TFUtils.ConsoleLog != null)
			{
				TFUtils.WriteConsoleLog(text);
			}
		}
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000D5674 File Offset: 0x000D3874
	public static void DebugLog(object message)
	{
		if (message == null)
		{
			return;
		}
		if (SBSettings.ConsoleLoggingEnabled)
		{
			Debug.Log(message);
		}
		if (TFUtils.LOG_LEVEL == TFUtils.LogLevel.INFO)
		{
			string text = message.ToString();
			if (text != null && TFUtils.ConsoleLog != null)
			{
				TFUtils.WriteConsoleLog(text);
			}
		}
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x000D56C0 File Offset: 0x000D38C0
	public static void DebugLogTimed(object message)
	{
		if (TFUtils.lastTimedMessage == null || TFUtils.lastTimedMessage != message || Time.realtimeSinceStartup - TFUtils.lastTimedMessageTime > 1f)
		{
			TFUtils.DebugLog(message);
			TFUtils.lastTimedMessage = message;
			TFUtils.lastTimedMessageTime = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x000D5710 File Offset: 0x000D3910
	public static void WarningLog(object message)
	{
		Debug.LogWarning(message);
		if (TFUtils.LOG_LEVEL <= TFUtils.LogLevel.WARN)
		{
			string text = message.ToString();
			if (text != null && TFUtils.ConsoleLog != null)
			{
				TFUtils.WriteConsoleLog(text);
			}
		}
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000D574C File Offset: 0x000D394C
	public static void ErrorLog(object message)
	{
		Debug.LogError(message);
		string text = message.ToString();
		if (TFUtils.LOG_LEVEL <= TFUtils.LogLevel.ERROR && text != null && TFUtils.ConsoleLog != null)
		{
			TFUtils.WriteConsoleLog(text);
		}
		if (text != null && TFUtils.ErrorConsoleLog != null)
		{
			TFUtils.ErrorConsoleLog.AppendLine(text);
		}
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000D57A4 File Offset: 0x000D39A4
	[Conditional("DEBUG")]
	public static void LogFormat(string format, params object[] args)
	{
		string text = string.Format(format, args);
		Debug.Log(text);
		if (TFUtils.LOG_LEVEL == TFUtils.LogLevel.INFO && TFUtils.ConsoleLog != null)
		{
			TFUtils.WriteConsoleLog(text);
		}
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x000D57DC File Offset: 0x000D39DC
	[Conditional("DEBUG")]
	public static void UnexpectedEntry()
	{
		throw new Exception("Unexpected path of code execution! You should not be here!");
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000D57E8 File Offset: 0x000D39E8
	[Conditional("DEBUG")]
	public static void NotYetImplemented()
	{
		throw new Exception("This function is not yet implemented.");
	}

	// Token: 0x060022ED RID: 8941 RVA: 0x000D57F4 File Offset: 0x000D39F4
	[Conditional("ASSERTS_ON")]
	public static void Assert(bool condition, string message)
	{
		if (!condition)
		{
			Exception ex = new Exception(message);
			if (SBSettings.DumpLogOnAssert)
			{
				TFUtils.LogDump(null, "Assert Error", ex, null);
			}
			throw ex;
		}
	}

	// Token: 0x060022EE RID: 8942 RVA: 0x000D5828 File Offset: 0x000D3A28
	public static GameObject FindGameObjectInHierarchy(GameObject root, string name)
	{
		if (root.name.Equals(name))
		{
			return root;
		}
		GameObject gameObject = null;
		int childCount = root.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			gameObject = TFUtils.FindGameObjectInHierarchy(root.transform.GetChild(i).gameObject, name);
			if (gameObject != null)
			{
				break;
			}
		}
		return gameObject;
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x000D5894 File Offset: 0x000D3A94
	public static GameObject FindParentGameObjectInHierarchy(GameObject root, string name)
	{
		Transform transform = root.transform;
		while (transform.parent != null)
		{
			if (transform.gameObject.name.Equals(name))
			{
				return transform.gameObject;
			}
			transform = transform.parent;
		}
		return null;
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000D58E4 File Offset: 0x000D3AE4
	public static void PlayMovie(string movie)
	{
		Handheld.PlayFullScreenMovie(movie, Color.black, FullScreenMovieControlMode.CancelOnInput);
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x000D58F4 File Offset: 0x000D3AF4
	public static byte[] Zip(string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		return TFUtils.Zip(bytes);
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000D5914 File Offset: 0x000D3B14
	public static byte[] Zip(byte[] bytedata)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
			{
				gzipStream.Write(bytedata, 0, bytedata.Length);
				gzipStream.Close();
			}
			result = memoryStream.ToArray();
		}
		return result;
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x000D59A8 File Offset: 0x000D3BA8
	public static byte[] UnzipToBytes(byte[] input)
	{
		MemoryStream stream = new MemoryStream(input);
		MemoryStream memoryStream = new MemoryStream();
		using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress))
		{
			byte[] array = new byte[1024];
			int count;
			while ((count = gzipStream.Read(array, 0, array.Length)) > 0)
			{
				memoryStream.Write(array, 0, count);
			}
		}
		return memoryStream.ToArray();
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x000D5A30 File Offset: 0x000D3C30
	public static string Unzip(byte[] input)
	{
		return Encoding.UTF8.GetString(TFUtils.UnzipToBytes(input));
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x000D5A44 File Offset: 0x000D3C44
	public static int BoolToInt(bool myBool)
	{
		if (myBool)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x000D5A50 File Offset: 0x000D3C50
	public static int KontagentCurrencyLevelIndex(int kRange)
	{
		if (kRange > 0 && kRange < 10)
		{
			return 1;
		}
		if (kRange > 10 && kRange < 100)
		{
			return 2;
		}
		if (kRange > 100 && kRange < 1000)
		{
			return 3;
		}
		if (kRange > 1000 && kRange < 10000)
		{
			return 4;
		}
		if (kRange > 10000 && kRange < 100000)
		{
			return 5;
		}
		if (kRange > 100000)
		{
			return 6;
		}
		return 0;
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000D5AD4 File Offset: 0x000D3CD4
	public static string GetOSVersion()
	{
		return SystemInfo.operatingSystem;
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000D5ADC File Offset: 0x000D3CDC
	public static string GetAndroidDeviceTypeString()
	{
		return SystemInfo.deviceModel;
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x000D5AE4 File Offset: 0x000D3CE4
	public static string GetDeviceLandscapeAspectRatio()
	{
		return Screen.width + ":" + Screen.height;
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x000D5B04 File Offset: 0x000D3D04
	private static int triggerIosUiMessage(string sTitle, string sText, string sOK, string sId)
	{
		EtceteraAndroid.showAlert(sTitle, sText, sOK);
		return 0;
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x000D5B10 File Offset: 0x000D3D10
	public static void TriggerPurchaseWarning()
	{
		string sText = TFUtils.AssignStorePlatformText("!!PURCHASE_WARNING_TEXT");
		string sTitle = Language.Get("!!PURCHASE_WARNING_TITLE");
		string sOK = Language.Get("!!PURCHASE_WARNING_OK");
		TFUtils.triggerIosUiMessage(sTitle, sText, sOK, "InApp");
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x000D5B4C File Offset: 0x000D3D4C
	public static void TriggerIAPDisabledWarning()
	{
	}

	// Token: 0x060022FD RID: 8957 RVA: 0x000D5B50 File Offset: 0x000D3D50
	public static void TriggerIAPOfflineWarning()
	{
		string sText = TFUtils.AssignStorePlatformText("!!IAP_OFFLINE_TEXT");
		string sTitle = Language.Get("!!IAP_OFFLINE_TITLE");
		string sOK = Language.Get("!!PURCHASE_WARNING_OK");
		TFUtils.triggerIosUiMessage(sTitle, sText, sOK, "InApp");
	}

	// Token: 0x060022FE RID: 8958 RVA: 0x000D5B8C File Offset: 0x000D3D8C
	public static string AssignStorePlatformText(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return key;
		}
		string text = Language.Get(key);
		if (text == null)
		{
			return text;
		}
		if (text.Contains("{0}"))
		{
			string arg;
			if (SoaringInternal.PlatformType == SoaringPlatformType.Amazon)
			{
				arg = Language.Get("!!PLATFORM_AMAZON");
			}
			else
			{
				arg = Language.Get("!!PLATFORM_GOOGLE");
			}
			text = string.Format(text, arg);
		}
		return text;
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x000D5BFC File Offset: 0x000D3DFC
	public static void TriggerEULAPopup()
	{
		string sText = Language.Get("!!PRIVACY_POLICY_POPUP_TEXT");
		string sTitle = Language.Get("!!PRIVACY_POLICY_POPUP_TITLE");
		string sOK = Language.Get("!!PURCHASE_WARNING_OK");
		TFUtils.triggerIosUiMessage(sTitle, sText, sOK, "PrivacyPolicy");
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x000D5C38 File Offset: 0x000D3E38
	private static int triggerIosUiError(string sTitle, string sText, string sOK)
	{
		EtceteraAndroid.showAlert(sTitle, sText, sOK);
		return 0;
	}

	// Token: 0x06002301 RID: 8961 RVA: 0x000D5C44 File Offset: 0x000D3E44
	public static void TriggerIosUiError(string title, string text)
	{
		string sOK = Language.Get("!!PREFAB_OK");
		TFUtils.triggerIosUiError(title, text, sOK);
	}

	// Token: 0x06002302 RID: 8962 RVA: 0x000D5C6C File Offset: 0x000D3E6C
	public static void TriggerIosUiChoice(string title, string message, string button1, string button2, string option1, string option2, string callbackId)
	{
	}

	// Token: 0x06002303 RID: 8963 RVA: 0x000D5C70 File Offset: 0x000D3E70
	public static string GetDeviceId()
	{
		return SystemInfo.deviceUniqueIdentifier;
	}

	// Token: 0x06002304 RID: 8964 RVA: 0x000D5C78 File Offset: 0x000D3E78
	private static string DumpLogPath()
	{
		return ResourceUtils.GetWritePath("LogDump.dmp", "LogDump", 1);
	}

	// Token: 0x06002305 RID: 8965 RVA: 0x000D5C8C File Offset: 0x000D3E8C
	public static void LogDump(Session session, string tag, Exception ex = null, SoaringDictionary logDataDictionary = null)
	{
		try
		{
			if (ex != null)
			{
				TFUtils.WarningLog("Exception: " + ex.Message);
				TFUtils.WarningLog("Stack Trace: " + ex.StackTrace);
			}
			if (!Soaring.IsInitialized)
			{
				TFUtils.ErrorLog("Failed to upload log dump to server because soaring isnt initialized");
			}
			else
			{
				SoaringDictionary soaringDictionary = null;
				if (logDataDictionary != null)
				{
					soaringDictionary = logDataDictionary;
				}
				else
				{
					soaringDictionary = new SoaringDictionary(4);
				}
				if (ex != null)
				{
					soaringDictionary.addValue(Convert.ToBase64String(Encoding.UTF8.GetBytes(ex.Message)), "exception");
					soaringDictionary.addValue(Convert.ToBase64String(Encoding.UTF8.GetBytes(ex.StackTrace)), "stack_trace");
					IDictionary dictionary = ex.Data;
					if (dictionary.Count == 0)
					{
						dictionary = null;
						if (ex.InnerException != null)
						{
							dictionary = ex.InnerException.Data;
							if (dictionary.Count == 0)
							{
								dictionary = null;
							}
						}
					}
					if (dictionary != null)
					{
						SoaringDictionary soaringDictionary2 = new SoaringDictionary(2);
						foreach (object obj in dictionary)
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
							soaringDictionary2.addValue(dictionaryEntry.Value.ToString(), dictionaryEntry.Key.ToString());
						}
						soaringDictionary.addValue(soaringDictionary2, "exception_keys");
					}
				}
				soaringDictionary.addValue(TFUtils.GetConsoleOutput(), "console_log");
				if (session != null)
				{
					string gameJsonFile = TFUtils.GetGameJsonFile(session.ThePlayer);
					if (gameJsonFile != null)
					{
						soaringDictionary.addValue(gameJsonFile, "game_json");
					}
				}
				soaringDictionary.addValue(TFUtils.GetErrorLog(), "error_log");
				soaringDictionary.addValue(Soaring.Player.UserTag, "session_username");
				if (!string.IsNullOrEmpty(tag))
				{
					soaringDictionary.addValue(tag, "error_tag");
				}
				soaringDictionary.addValue(TFUtils.EpochTime() - TFUtils.APP_START_TIME, "time_since_start");
				soaringDictionary.addValue(TFUtils.GetAndroidDeviceTypeString(), "device_type");
				soaringDictionary.addValue(TFUtils.GetOSVersion(), "os_version");
				soaringDictionary.addValue(SBSettings.BundleVersion, "bundle_version");
				if (SoaringInternal.IsProductionMode && !string.IsNullOrEmpty(SoaringDebug.DebugFileName))
				{
					string empty = string.Empty;
					string writePath = ResourceUtils.GetWritePath(SoaringDebug.DebugFileName, empty + "Soaring/Logs", 8);
					if (!string.IsNullOrEmpty(writePath))
					{
						string lastSoaringDebugFile = TFUtils.GetLastSoaringDebugFile(writePath);
						if (lastSoaringDebugFile != null)
						{
							soaringDictionary.addValue(lastSoaringDebugFile, "soaring_last_log");
						}
					}
				}
				if (logDataDictionary == null)
				{
					try
					{
						SoaringFileTools.WriteJsonToFile(TFUtils.DumpLogPath(), soaringDictionary);
						SoaringContext soaringContext = new SoaringContext();
						soaringContext.Responder = new TFUtils.SendLogDumpDelegate();
						soaringContext.addValue(true, "trIOff");
						Soaring.SendSessionData(soaringDictionary, soaringContext);
					}
					catch (Exception ex2)
					{
						TFUtils.ErrorLog("Exception: " + ex2.Message);
						TFUtils.ErrorLog("Failed to upload log dump to server due to Exception when sending.");
					}
				}
			}
		}
		catch (Exception ex3)
		{
			TFUtils.ErrorLog("Exception: " + ex3.Message);
		}
	}

	// Token: 0x06002306 RID: 8966 RVA: 0x000D6028 File Offset: 0x000D4228
	public static bool CheckForLogDumps(SoaringContextDelegate context_responder)
	{
		bool result = false;
		try
		{
			string text = TFUtils.DumpLogPath();
			if (File.Exists(text))
			{
				MBinaryReader mbinaryReader = new MBinaryReader(text);
				if (mbinaryReader != null)
				{
					if (mbinaryReader.IsOpen())
					{
						try
						{
							SoaringDictionary data = new SoaringDictionary(mbinaryReader.ReadAllBytes());
							SoaringContext soaringContext = new SoaringContext();
							soaringContext.Responder = new TFUtils.SendLogDumpDelegate();
							soaringContext.ContextResponder = context_responder;
							soaringContext.addValue(true, "trIOff");
							Soaring.SendSessionData(data, soaringContext);
							result = true;
						}
						catch (Exception ex)
						{
							TFUtils.ErrorLog("Exception: " + ex.Message);
							TFUtils.ErrorLog("Failed to upload log dump to server due to Exception when sending.");
						}
					}
					mbinaryReader.Close();
					mbinaryReader = null;
					if (context_responder == null)
					{
						File.Delete(text);
					}
				}
				result = true;
			}
		}
		catch (Exception ex2)
		{
			SoaringDebug.Log(ex2.Message, LogType.Error);
			result = false;
		}
		return result;
	}

	// Token: 0x06002307 RID: 8967 RVA: 0x000D6138 File Offset: 0x000D4338
	public static string GetConsoleOutput()
	{
		string result = null;
		try
		{
			int startIndex = 0;
			if (TFUtils.ConsoleLog.Length > 131072)
			{
				startIndex = TFUtils.ConsoleLog.Length - 131072;
			}
			result = Convert.ToBase64String(TFUtils.Zip(TFUtils.ConsoleLog.ToString(startIndex, TFUtils.ConsoleLog.Length)));
		}
		catch (Exception ex)
		{
			TFUtils.ErrorLog("GetGameJsonFile: Error: " + ex.Message);
		}
		return result;
	}

	// Token: 0x06002308 RID: 8968 RVA: 0x000D61CC File Offset: 0x000D43CC
	public static string GetLastSoaringDebugFile(string path)
	{
		string result = null;
		try
		{
			if (TFUtils.FileIsExists(path))
			{
				result = Convert.ToBase64String(TFUtils.Zip(File.ReadAllText(path)));
			}
		}
		catch (Exception ex)
		{
			TFUtils.ErrorLog("GetGameJsonFile: Error: " + ex.Message);
		}
		return result;
	}

	// Token: 0x06002309 RID: 8969 RVA: 0x000D6234 File Offset: 0x000D4434
	public static string GetGameJsonFile(Player p)
	{
		string result = null;
		try
		{
			string text = p.CacheFile("game.json");
			if (TFUtils.FileIsExists(text))
			{
				result = Convert.ToBase64String(TFUtils.Zip(File.ReadAllText(text)));
			}
		}
		catch (Exception ex)
		{
			TFUtils.ErrorLog("GetGameJsonFile: Error: " + ex.Message);
		}
		return result;
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000D62A8 File Offset: 0x000D44A8
	public static string GetErrorLog()
	{
		string result = null;
		try
		{
			int startIndex = 0;
			if (TFUtils.ConsoleLog.Length > 131072)
			{
				startIndex = TFUtils.ConsoleLog.Length - 131072;
			}
			result = Convert.ToBase64String(TFUtils.Zip(TFUtils.ErrorConsoleLog.ToString(startIndex, TFUtils.ErrorConsoleLog.Length)));
		}
		catch (Exception ex)
		{
			TFUtils.ErrorLog("GetErrorLog: Error: " + ex.Message);
		}
		return result;
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000D633C File Offset: 0x000D453C
	public static void GotoAppstore()
	{
		TFUtils.DebugLog("Going to " + SBSettings.STORE_APP_URL);
		if (!SoaringPlatform.OpenURL(SBSettings.STORE_APP_URL))
		{
			TFUtils.WarningLog("Failed to open " + SBSettings.STORE_APP_URL + " with native handler");
			Application.OpenURL(SBSettings.STORE_APP_URL);
		}
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000D6390 File Offset: 0x000D4590
	public static void SetDefaultHeaders(WebHeaderCollection wc)
	{
		wc.Add("SB-Date", DateTime.UtcNow.ToUniversalTime().ToString("r"));
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000D63C4 File Offset: 0x000D45C4
	public static string GetPlayerName(SoaringPlayer player, string format = "{0}")
	{
		string text = string.Empty;
		if (player.LoginType != SoaringLoginType.Device && player.LoginType != SoaringLoginType.Soaring)
		{
			text = SoaringPlatform.PlatformUserAlias;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = player.UserTag;
		}
		if (string.IsNullOrEmpty(text))
		{
			text = SoaringPlatform.DeviceID;
		}
		return string.Format(format, text);
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x000D6420 File Offset: 0x000D4620
	public static bool FileIsExists(string filePath)
	{
		TFUtils.DebugLog("judge_filePath " + filePath);
		if (filePath.Contains("://"))
		{
			return TFUtils.LoadWWWExist(filePath);
		}
		return File.Exists(filePath);
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000D6464 File Offset: 0x000D4664
	private static bool LoadWWWExist(string filePath)
	{
		WWW www = new WWW(filePath);
		while (!www.isDone)
		{
			if (www.error != null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000D6498 File Offset: 0x000D4698
	public static bool isAmazon()
	{
		return SystemInfo.deviceModel.StartsWith("Amazon");
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000D64AC File Offset: 0x000D46AC
	public static string ParseGameDetails(Dictionary<string, object> gameData, ref TFUtils.GameDetails details)
	{
		details = new TFUtils.GameDetails();
		try
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)gameData["playtime"];
			details.level = dictionary["level"].ToString();
			details.lastPlayTime = int.Parse(dictionary["last"].ToString());
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			details.dtLastPlayTime = dateTime.AddSeconds((double)details.lastPlayTime).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
			TFUtils.DebugLog("dtDateTime " + details.dtLastPlayTime);
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)gameData["farm"];
			List<object> resources = (List<object>)dictionary2["resources"];
			details.money = TFUtils.resourceValueByDid(3, resources);
			details.jelly = TFUtils.resourceValueByDid(2, resources);
			details.patties = TFUtils.resourceValueByDid(1, resources);
		}
		catch (Exception ex)
		{
			TFUtils.ErrorLog("Failed to parse game details: " + ex.Message + "\n" + ex.StackTrace);
		}
		return string.Format("time: {0} level: {1} money: {2} jelly: {3}", new object[]
		{
			details.lastPlayTime,
			details.level,
			details.money,
			details.jelly
		});
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000D662C File Offset: 0x000D482C
	private static string resourceValueByDid(int lookupDid, List<object> resources)
	{
		string result = string.Empty;
		foreach (object obj in resources)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			if (dictionary.ContainsKey("did"))
			{
				int num = int.Parse(dictionary["did"].ToString());
				if (lookupDid == num)
				{
					Dictionary<string, object> dictionary2 = dictionary;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					if (dictionary2.ContainsKey("amount_earned"))
					{
						num2 = int.Parse(dictionary2["amount_earned"].ToString());
					}
					if (dictionary2.ContainsKey("amount_purchased"))
					{
						num3 = int.Parse(dictionary2["amount_purchased"].ToString());
					}
					if (dictionary2.ContainsKey("amount_spent"))
					{
						num4 = int.Parse(dictionary2["amount_spent"].ToString());
					}
					result = (num2 + num3 - num4).ToString();
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x000D6764 File Offset: 0x000D4964
	public static string GetEULA_Address()
	{
		string text = Language.Get("!!EULA_URL");
		if (text != null && text.Contains("!!EULA_URL"))
		{
			text = null;
		}
		if (string.IsNullOrEmpty(text))
		{
			LanguageCode languageCode = Language.CurrentLanguage();
			if (languageCode != LanguageCode.DE)
			{
				if (languageCode != LanguageCode.ES)
				{
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
										if (languageCode == LanguageCode.LatAm)
										{
											text = "http://mx.mundonick.com/gsp/international/mundonick.com/AcuerdodeEntregadeContenidosPorPartedeUsuarios.pdf";
										}
									}
									else
									{
										text = "http://www.nickelodeon.ru/gsp/international/nickelodeon.ru/footer/terms-and-conditions.pdf";
									}
								}
								else
								{
									text = "http://mundonick.uol.com.br/gsp/international/mundonick.com.br/legal/termosdeuso.pdf";
								}
							}
							else
							{
								text = "http://www.nickelodeon.nl/static/info_algemene_voorwaarden";
							}
						}
						else
						{
							text = "http://www.nicktv.it/info/note-legali";
						}
					}
					else
					{
						text = "http://www.nickelodeon.fr/legal/";
					}
				}
				else
				{
					text = "http://www.nickelodeon.es/info/aviso-legal";
				}
			}
			else
			{
				text = "http://www.nick.de/static/info_contests";
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "http://www.nick.com/info/eula.html";
		}
		return text;
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000D685C File Offset: 0x000D4A5C
	public static string GetLegal_Address()
	{
		string text = Language.Get("!!LEGAL_URL");
		if (text != null && text.Contains("!!LEGAL_URL"))
		{
			text = null;
		}
		if (string.IsNullOrEmpty(text))
		{
			LanguageCode languageCode = Language.CurrentLanguage();
			if (languageCode != LanguageCode.DE)
			{
				if (languageCode != LanguageCode.ES)
				{
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
										if (languageCode == LanguageCode.LatAm)
										{
											text = "http://mx.mundonick.com/gsp/international/mundonick.com/AcuerdodeEntregadeContenidosPorPartedeUsuarios.pdf";
										}
									}
									else
									{
										text = "http://www.nickelodeon.ru/gsp/international/nickelodeon.ru/footer/terms-and-conditions.pdf";
									}
								}
								else
								{
									text = "http://mundonick.uol.com.br/gsp/international/mundonick.com.br/legal/termosdeuso.pdf";
								}
							}
							else
							{
								text = "http://www.nickelodeon.nl/static/info_algemene_voorwaarden";
							}
						}
						else
						{
							text = "http://www.nicktv.it/info/note-legali";
						}
					}
					else
					{
						text = "http://www.nickelodeon.fr/legal/";
					}
				}
				else
				{
					text = "http://www.nickelodeon.es/info/aviso-legal";
				}
			}
			else
			{
				text = "http://www.nick.de/static/info_contests";
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "http://www.nick.com/nick-assets/copy/legal.html";
		}
		return text;
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000D6954 File Offset: 0x000D4B54
	public static string GetPrivacy_Address()
	{
		string text = Language.Get("!!PRIVACY_URL");
		if (text != null && text.Contains("!!PRIVACY_URL"))
		{
			text = null;
		}
		if (string.IsNullOrEmpty(text))
		{
			LanguageCode languageCode = Language.CurrentLanguage();
			if (languageCode != LanguageCode.DE)
			{
				if (languageCode != LanguageCode.ES)
				{
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
										if (languageCode == LanguageCode.LatAm)
										{
											text = "http://mx.mundonick.com/gsp/international/mundonick.com/politica_de_privacidad_de_mundonick.pdf";
										}
									}
									else
									{
										text = "http://www.nickelodeon.ru/gsp/international/nickelodeon.ru/footer/privacy-policy.pdf";
									}
								}
								else
								{
									text = "http://mundonick.uol.com.br/gsp/international/mundonick.com.br/legal/politicadeprivacidade.pdf";
								}
							}
							else
							{
								text = "http://www.nickelodeon.nl/static/info_privacy";
							}
						}
						else
						{
							text = "http://www.nicktv.it/info/note-legali";
						}
					}
					else
					{
						text = "http://www.nickelodeon.fr/legal/";
					}
				}
				else
				{
					text = "Privacy: http://www.nickelodeon.es/info/privacidad";
				}
			}
			else
			{
				text = "http://www.nick.de/static/info_datenschutz";
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "http://www.nick.com/info/privacy-policy.html";
		}
		return text;
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x000D6A4C File Offset: 0x000D4C4C
	public static void RefreshSAFiles()
	{
		string filePath = TFUtils.GetStreamingAssetsPath() + Path.DirectorySeparatorChar + "sa_file.json";
		string filePath2 = TFUtils.GetPersistentAssetsPath() + "/sa_file.json";
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string text = null;
		if (TFUtils.FileIsExists(filePath2))
		{
			text = TFUtils.ReadAllText(filePath2);
		}
		else if (TFUtils.FileIsExists(filePath))
		{
			text = TFUtils.ReadAllText(filePath);
		}
		if (text != null)
		{
			dictionary = (Dictionary<string, object>)Json.Deserialize(text);
			for (int i = 0; i < Config.SA_FILES.Length; i++)
			{
				string text2 = Config.SA_FILES[i];
				if (dictionary.ContainsKey(text2))
				{
					string text3 = dictionary[text2] as string;
					string[] array = text3.Split(new char[]
					{
						','
					});
					string text4 = text2;
					switch (text4)
					{
					case "Crafting":
						Config.CRAFTING_PATH = array;
						break;
					case "Dialogs":
						Config.DIALOG_PACKAGES_PATH = array;
						break;
					case "Features":
						Config.FEATURE_DATA_PATH = array;
						break;
					case "Video":
						Config.MOVIE_PATH = array;
						break;
					case "Notifications":
						Config.NOTIFICATIONS_PATH = array;
						break;
					case "Quests":
						Config.QUESTS_PATH = array;
						break;
					case "BonusPaytables":
						Config.BONUS_PAYTABLES = array;
						break;
					case "Tasks":
						Config.TASKS_PATH = array;
						break;
					case "Treasure":
						Config.TREASURE_PATH = array;
						break;
					case "Vending":
						Config.VENDING_PATH = array;
						break;
					case "Blueprints":
						Config.BLUEPRINT_DIRECTORY_PATH = array;
						break;
					case "Terrain":
						Config.TERRAIN_PATH = array;
						break;
					}
				}
			}
			TFUtils.UpdateSAFilePathRefs(Config.CRAFTING_PATH, "Crafting");
			TFUtils.UpdateSAFilePathRefs(Config.DIALOG_PACKAGES_PATH, "Dialogs");
			TFUtils.UpdateSAFilePathRefs(Config.FEATURE_DATA_PATH, "Features");
			TFUtils.UpdateSAFilePathRefs(Config.MOVIE_PATH, "Video");
			TFUtils.UpdateSAFilePathRefs(Config.NOTIFICATIONS_PATH, "Notifications");
			TFUtils.UpdateSAFilePathRefs(Config.QUESTS_PATH, "Quests");
			TFUtils.UpdateSAFilePathRefs(Config.BONUS_PAYTABLES, "BonusPaytables");
			TFUtils.UpdateSAFilePathRefs(Config.TASKS_PATH, "Tasks");
			TFUtils.UpdateSAFilePathRefs(Config.TREASURE_PATH, "Treasure");
			TFUtils.UpdateSAFilePathRefs(Config.VENDING_PATH, "Vending");
			TFUtils.UpdateSAFilePathRefs(Config.BLUEPRINT_DIRECTORY_PATH, "Blueprints");
			TFUtils.UpdateSAFilePathRefs(Config.TERRAIN_PATH, "Terrain");
		}
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x000D6D78 File Offset: 0x000D4F78
	private static void UpdateSAFilePathRefs(string[] files, string directory)
	{
		if (files == null || string.IsNullOrEmpty(directory))
		{
			return;
		}
		int num = files.Length;
		string str = Path.DirectorySeparatorChar + directory + Path.DirectorySeparatorChar;
		string str2 = TFUtils.GetPersistentAssetsPath() + str;
		string str3 = TFUtils.GetStreamingAssetsPath() + str;
		for (int i = 0; i < num; i++)
		{
			string str4 = files[i];
			string text = str2 + str4;
			if (File.Exists(text))
			{
				files[i] = text;
			}
			else
			{
				files[i] = str3 + str4;
			}
		}
	}

	// Token: 0x0400154C RID: 5452
	public const int kMaxSaveLogLength = 131072;

	// Token: 0x0400154D RID: 5453
	private const float MESSAGE_TIME = 1f;

	// Token: 0x0400154E RID: 5454
	private const int LOG_TYPE_NONE = -1;

	// Token: 0x0400154F RID: 5455
	private const int LOG_TYPE_ERROR = 0;

	// Token: 0x04001550 RID: 5456
	private const int LOG_TYPE_WARNING = 1;

	// Token: 0x04001551 RID: 5457
	private const int LOG_TYPE_STANDARD = 2;

	// Token: 0x04001552 RID: 5458
	public static DateTime EPOCH;

	// Token: 0x04001553 RID: 5459
	public static ulong APP_START_TIME;

	// Token: 0x04001554 RID: 5460
	public static TFUtils.LogLevel LOG_LEVEL;

	// Token: 0x04001555 RID: 5461
	public static StringBuilder PrevLog;

	// Token: 0x04001556 RID: 5462
	public static StringBuilder ConsoleLog;

	// Token: 0x04001557 RID: 5463
	public static StringBuilder ErrorConsoleLog;

	// Token: 0x04001558 RID: 5464
	public static TFUtils.LogFilter LOG_FILTER;

	// Token: 0x04001559 RID: 5465
	public static string ApplicationDataPath;

	// Token: 0x0400155A RID: 5466
	public static string ApplicationPersistentDataPath;

	// Token: 0x0400155B RID: 5467
	public static string DeviceId;

	// Token: 0x0400155C RID: 5468
	public static string DeviceName;

	// Token: 0x0400155D RID: 5469
	public static ulong DebugTimeOffset;

	// Token: 0x0400155E RID: 5470
	private static object lastTimedMessage;

	// Token: 0x0400155F RID: 5471
	private static float lastTimedMessageTime;

	// Token: 0x04001560 RID: 5472
	public static bool isFastForwarding;

	// Token: 0x04001561 RID: 5473
	private static float timeMultiplier;

	// Token: 0x04001562 RID: 5474
	public static ulong FastForwardOffset;

	// Token: 0x04001563 RID: 5475
	public static ulong AddTimeOffset;

	// Token: 0x04001564 RID: 5476
	private static DateTime seedUtcNow;

	// Token: 0x04001565 RID: 5477
	public static string playerID;

	// Token: 0x04001566 RID: 5478
	public static string playerAlias;

	// Token: 0x0200045F RID: 1119
	public enum LogLevel
	{
		// Token: 0x0400156B RID: 5483
		INFO,
		// Token: 0x0400156C RID: 5484
		WARN,
		// Token: 0x0400156D RID: 5485
		ERROR
	}

	// Token: 0x02000460 RID: 1120
	public enum LogFilter
	{
		// Token: 0x0400156F RID: 5487
		All,
		// Token: 0x04001570 RID: 5488
		CraftingManager,
		// Token: 0x04001571 RID: 5489
		Resources,
		// Token: 0x04001572 RID: 5490
		Assets = 4,
		// Token: 0x04001573 RID: 5491
		Paytables = 8,
		// Token: 0x04001574 RID: 5492
		Features = 16,
		// Token: 0x04001575 RID: 5493
		Tasks = 32,
		// Token: 0x04001576 RID: 5494
		Terrain = 64,
		// Token: 0x04001577 RID: 5495
		Quests = 128,
		// Token: 0x04001578 RID: 5496
		Vending = 256,
		// Token: 0x04001579 RID: 5497
		Buildings = 512,
		// Token: 0x0400157A RID: 5498
		Residents = 1024,
		// Token: 0x0400157B RID: 5499
		None = 2147483647
	}

	// Token: 0x02000461 RID: 1121
	public class SendLogDumpDelegate : SoaringDelegate
	{
		// Token: 0x0600231B RID: 8987 RVA: 0x000D6E90 File Offset: 0x000D5090
		public override void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
			if (success && error == null)
			{
				string path = TFUtils.DumpLogPath();
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			if (context.ContextResponder != null)
			{
				context.ContextResponder(context);
			}
		}
	}

	// Token: 0x02000462 RID: 1122
	public class GameDetails
	{
		// Token: 0x0400157C RID: 5500
		public int lastPlayTime;

		// Token: 0x0400157D RID: 5501
		public string dtLastPlayTime = string.Empty;

		// Token: 0x0400157E RID: 5502
		public string money = string.Empty;

		// Token: 0x0400157F RID: 5503
		public string jelly = string.Empty;

		// Token: 0x04001580 RID: 5504
		public string patties = string.Empty;

		// Token: 0x04001581 RID: 5505
		public string level = string.Empty;
	}
}
