using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

// Token: 0x02000113 RID: 275
public class SBSettings
{
	// Token: 0x060009E1 RID: 2529 RVA: 0x0003DBB0 File Offset: 0x0003BDB0
	private SBSettings()
	{
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0003DC6C File Offset: 0x0003BE6C
	public static Version LOCAL_BUNDLE_VERSION
	{
		get
		{
			return SBSettings.localBundleVersion;
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0003DC74 File Offset: 0x0003BE74
	public static Version CURRENT_APPSTORE_BUNDLE_VERSION
	{
		get
		{
			return SBSettings.currentAppStoreBundleVersion;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0003DC7C File Offset: 0x0003BE7C
	public static string CDN_URL
	{
		get
		{
			return SBSettings.cdnUrl;
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0003DC84 File Offset: 0x0003BE84
	public static string MANIFEST_FILE
	{
		get
		{
			return SBSettings.manifestFile;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0003DC8C File Offset: 0x0003BE8C
	public static string MANIFEST_URL
	{
		get
		{
			return SBSettings.manifestUrl;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0003DC94 File Offset: 0x0003BE94
	public static string SERVER_URL
	{
		get
		{
			return SBSettings.serverUrl;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0003DC9C File Offset: 0x0003BE9C
	public static string STORE_APP_URL
	{
		get
		{
			return SBSettings.storeAppUrl;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x060009EA RID: 2538 RVA: 0x0003DCA4 File Offset: 0x0003BEA4
	public static int SAVE_INTERVAL
	{
		get
		{
			return SBSettings.saveInterval;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x060009EB RID: 2539 RVA: 0x0003DCAC File Offset: 0x0003BEAC
	public static int PATCHING_FILE_LIMIT
	{
		get
		{
			return SBSettings.patchingFileLimit;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x0003DCB4 File Offset: 0x0003BEB4
	public static int NETWORK_RETRY_COUNT
	{
		get
		{
			return SBSettings.retryCount;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x060009ED RID: 2541 RVA: 0x0003DCBC File Offset: 0x0003BEBC
	public static int? ANALAYTICS_BUFFER_SIZE
	{
		get
		{
			return SBSettings.analyticsBufferSize;
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x060009EE RID: 2542 RVA: 0x0003DCC4 File Offset: 0x0003BEC4
	public static string BundleIdentifier
	{
		get
		{
			return SBSettings.bundleIdentifier;
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x060009EF RID: 2543 RVA: 0x0003DCCC File Offset: 0x0003BECC
	public static string BundleVersion
	{
		get
		{
			return SBSettings.bundleVersion;
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0003DCD4 File Offset: 0x0003BED4
	public static string BundleShortVersion
	{
		get
		{
			return SBSettings.bundleShortVersion;
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0003DCDC File Offset: 0x0003BEDC
	public static string StoreName
	{
		get
		{
			return SBSettings.useStoreName;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0003DCE4 File Offset: 0x0003BEE4
	public static bool DebugDisplayControllers
	{
		get
		{
			return SBSettings.debugDisplayControllers;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0003DCEC File Offset: 0x0003BEEC
	public static bool TrackStatistics
	{
		get
		{
			return SBSettings.trackStatistics;
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0003DCF4 File Offset: 0x0003BEF4
	public static float StatisticsTrackingInterval
	{
		get
		{
			return SBSettings.statisticsTrackingInterval;
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0003DCFC File Offset: 0x0003BEFC
	public static bool ShowDebug
	{
		get
		{
			return SBSettings.showDebug;
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0003DD04 File Offset: 0x0003BF04
	public static bool EnableRandomQuests
	{
		get
		{
			return SBSettings.enableRandomQuests;
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x060009F7 RID: 2551 RVA: 0x0003DD0C File Offset: 0x0003BF0C
	public static bool EnableAutoQuests
	{
		get
		{
			return SBSettings.enableAutoQuests;
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0003DD14 File Offset: 0x0003BF14
	public static float CommunityEventBannerPing
	{
		get
		{
			return SBSettings.communityEventBannerPing;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x060009F9 RID: 2553 RVA: 0x0003DD1C File Offset: 0x0003BF1C
	public static bool EnableShakeLogDump
	{
		get
		{
			return SBSettings.enableShakeLogDump;
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x060009FA RID: 2554 RVA: 0x0003DD24 File Offset: 0x0003BF24
	public static bool AssertDataValidity
	{
		get
		{
			return SBSettings.assertDataValidity;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x060009FB RID: 2555 RVA: 0x0003DD2C File Offset: 0x0003BF2C
	public static bool ConsoleLoggingEnabled
	{
		get
		{
			return SBSettings.consoleLogging;
		}
	}

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x0003DD34 File Offset: 0x0003BF34
	public static bool BypassPatching
	{
		get
		{
			return SBSettings.bypassPatching;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x060009FD RID: 2557 RVA: 0x0003DD3C File Offset: 0x0003BF3C
	public static bool SoaringProduction
	{
		get
		{
			return SBSettings.soaringProduction;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x0003DD44 File Offset: 0x0003BF44
	public static bool SoaringQA
	{
		get
		{
			return SBSettings.soaringQA;
		}
	}

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x0003DD4C File Offset: 0x0003BF4C
	public static string BillingKey
	{
		get
		{
			return SBSettings.billingsKey;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0003DD54 File Offset: 0x0003BF54
	public static bool UseActionFile
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0003DD58 File Offset: 0x0003BF58
	public static bool UseLegacyGameLoad
	{
		get
		{
			return SBSettings.useLegacySaveLoad;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0003DD60 File Offset: 0x0003BF60
	public static bool RebootOnFocusChange
	{
		get
		{
			return SBSettings.rebootOnFocusChange;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0003DD68 File Offset: 0x0003BF68
	public static bool RebootOnConnectionChange
	{
		get
		{
			return SBSettings.rebootOnConnectionChange;
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0003DD70 File Offset: 0x0003BF70
	public static bool UseProductionIAP
	{
		get
		{
			return SBSettings.useProductionIAP;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0003DD78 File Offset: 0x0003BF78
	public static bool OfflineModeFriendParks
	{
		get
		{
			return SBSettings.offlineModeFriendPark;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0003DD80 File Offset: 0x0003BF80
	public static bool DisableFriendPark
	{
		get
		{
			return SBSettings.disableFriendPark;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0003DD88 File Offset: 0x0003BF88
	public static bool DumpLogOnAssert
	{
		get
		{
			return SBSettings.dumpLogOnAssert;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0003DD90 File Offset: 0x0003BF90
	public static string DeltaDNAEnvKey
	{
		get
		{
			return SBSettings.deltaDNAEnvKey;
		}
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x0003DD98 File Offset: 0x0003BF98
	public static void Init()
	{
		SBSettings._Init(false);
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0003DDA0 File Offset: 0x0003BFA0
	private static void _Init(bool isReload)
	{
		string streamingAssetsFile = TFUtils.GetStreamingAssetsFile("server_settings.json");
		string json = TFUtils.ReadAllText(streamingAssetsFile);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		SBSettings.serverUrl = (string)dictionary["server_url"];
		SBSettings.cdnUrl = (string)dictionary["cdn_url"];
		SBSettings.manifestFile = (string)dictionary["manifest_file"];
		SBSettings.manifestUrl = SBSettings.cdnUrl + SBSettings.manifestFile;
		if (dictionary.ContainsKey("bypass_patching"))
		{
			string text = (string)dictionary["bypass_patching"];
			if (!string.IsNullOrEmpty(text))
			{
				SBSettings.bypassPatching = (char.ToLower(text[0]) == 't');
			}
		}
		if (dictionary.ContainsKey("soaring_live"))
		{
			string text2 = (string)dictionary["soaring_live"];
			if (!string.IsNullOrEmpty(text2))
			{
				SBSettings.soaringProduction = (char.ToLower(text2[0]) == 't');
			}
		}
		if (dictionary.ContainsKey("soaring_qa"))
		{
			string text3 = (string)dictionary["soaring_qa"];
			if (!string.IsNullOrEmpty(text3))
			{
				SBSettings.soaringQA = (char.ToLower(text3[0]) == 't');
			}
		}
		if (dictionary.ContainsKey("deltaDNAEnv"))
		{
			SBSettings.deltaDNAEnvKey = (string)dictionary["deltaDNAEnv"];
		}
		SBSettings.bundleIdentifier = "com.nick.sbappstore";
		SBSettings.bundleVersion = "0.00.00";
		SBSettings.bundleShortVersion = "0.00.00";
		string text4 = "googleplay";
		if (TFUtils.isAmazon())
		{
			text4 = "amazon";
		}
		string json2;
		if (SBSettings.soaringProduction)
		{
			json2 = TFUtils.ReadAllText(TFUtils.GetStreamingAssetsFile_IgnorePersistant("android_version.json"));
		}
		else
		{
			json2 = TFUtils.ReadAllText(TFUtils.GetStreamingAssetsFile("android_version.json"));
		}
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)Json.Deserialize(json2);
		if (dictionary2.ContainsKey(text4 + "_bundle_identifier"))
		{
			SBSettings.bundleIdentifier = (string)dictionary2[text4 + "_bundle_identifier"];
		}
		if (dictionary2.ContainsKey(text4 + "_bundle_version"))
		{
			SBSettings.bundleVersion = (string)dictionary2[text4 + "_bundle_version"];
		}
		if (dictionary2.ContainsKey(text4 + "_bundle_short_version"))
		{
			SBSettings.bundleShortVersion = (string)dictionary2[text4 + "_bundle_short_version"];
		}
		if (dictionary2.ContainsKey(text4 + "_billing_key"))
		{
			SBSettings.billingsKey = (string)dictionary2[text4 + "_billing_key"];
		}
		if (SBSettings.bundleShortVersion.Contains("_"))
		{
			SBSettings.bundleShortVersion = SBSettings.bundleShortVersion.Substring(0, SBSettings.bundleShortVersion.IndexOf("_"));
		}
		if (SBSettings.bundleVersion.Contains("_"))
		{
			SBSettings.bundleVersion = SBSettings.bundleVersion.Substring(0, SBSettings.bundleVersion.IndexOf("_"));
		}
		SBSettings.localBundleVersion = new Version(SBSettings.bundleShortVersion);
		streamingAssetsFile = TFUtils.GetStreamingAssetsFile("global_settings.json");
		json = TFUtils.ReadAllText(streamingAssetsFile);
		dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		bool flag = SBSettings.LoadSettings(text4, dictionary);
		SBSettings.LoadAppMutableSettings(null);
		if (!flag)
		{
			Debug.LogError("Error: Failed to Load Settings!!! : " + isReload);
			if (!isReload)
			{
				TFUtils.DeletePersistantFile("global_settings.json");
				SBSettings._Init(true);
			}
			else
			{
				Debug.LogError("Critical: Failed to Load Settings!!! : " + isReload);
			}
		}
		streamingAssetsFile = TFUtils.GetStreamingAssetsFile("quality_settings.json");
		json = TFUtils.ReadAllText(streamingAssetsFile);
		dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		CommonUtils.Init(dictionary);
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x0003E174 File Offset: 0x0003C374
	private static bool LoadSettings(string name, Dictionary<string, object> data)
	{
		if (name == null || data == null)
		{
			Debug.LogError("Null Or Invalid Settings Data!");
			return false;
		}
		if (data.ContainsKey(name))
		{
			data = (Dictionary<string, object>)data[name];
		}
		else if (!data.ContainsKey("file_version"))
		{
			Debug.LogError("Invalid Global Settings File Version!");
			return false;
		}
		SBSettings.saveInterval = TFUtils.LoadInt(data, "save_interval");
		if (SBSettings.bypassPatching)
		{
			SBSettings.saveInterval = -1;
		}
		SBSettings.storeAppUrl = (string)data["store_url"];
		if (data.ContainsKey("log_settings"))
		{
			TFUtils.SetLogType((string)data["log_settings"]);
		}
		object obj;
		if (data.TryGetValue("debugDisplayControllers", out obj))
		{
			SBSettings.debugDisplayControllers = (bool)obj;
		}
		SBSettings.patchingFileLimit = TFUtils.LoadInt(data, "patching_file_limit", 10);
		SBSettings.retryCount = TFUtils.LoadInt(data, "network_retry_count", 2);
		float? num = TFUtils.TryLoadFloat(data, "statistics_tracking_interval");
		if (num != null)
		{
			SBSettings.statisticsTrackingInterval = num.Value;
		}
		else
		{
			SBSettings.statisticsTrackingInterval = 60f;
		}
		SBSettings.analyticsBufferSize = TFUtils.TryLoadInt(data, "analytics_buffer_size");
		SBSettings.trackStatistics = TFUtils.LoadBool(data, "track_statistics", false);
		SBSettings.useProductionIAP = TFUtils.LoadBool(data, "production_iap", true);
		SBSettings.consoleLogging = TFUtils.LoadBool(data, "console_logging", false);
		SBSettings.rebootOnFocusChange = TFUtils.LoadBool(data, "reboot_on_focus", false);
		SBSettings.rebootOnConnectionChange = TFUtils.LoadBool(data, "reboot_on_connection_change", false);
		SBSettings.offlineModeFriendPark = TFUtils.LoadBool(data, "offline_mode_visit_friends", false);
		SBSettings.disableFriendPark = TFUtils.LoadBool(data, "disable_visit_friends", false);
		SBSettings.assertDataValidity = TFUtils.LoadBool(data, "assert_valid_data", false);
		SBSettings.enableShakeLogDump = TFUtils.LoadBool(data, "shake_log_dump", false);
		SBSettings.dumpLogOnAssert = TFUtils.LoadBool(data, "assert_log_dump", true);
		SBSettings.showDebug = TFUtils.LoadBool(data, "show_debug", false);
		SBSettings.enableRandomQuests = TFUtils.LoadBool(data, "enable_random_quests", false);
		SBSettings.enableAutoQuests = TFUtils.LoadBool(data, "enable_auto_quests", true);
		num = TFUtils.TryLoadFloat(data, "community_event_banner_ping");
		if (num != null)
		{
			SBSettings.communityEventBannerPing = num.Value;
		}
		else
		{
			SBSettings.communityEventBannerPing = -1f;
		}
		SBSettings.enableShakeLogDump = TFUtils.LoadBool(data, "shake_log_dump", false);
		SBSettings.useLegacySaveLoad = TFUtils.LoadBool(data, "legacy_game_load", false);
		string key = "current_bundle_version";
		if (data.ContainsKey(key))
		{
			SBSettings.currentAppStoreBundleVersion = new Version((string)data[key]);
		}
		else
		{
			SBSettings.currentAppStoreBundleVersion = SBSettings.localBundleVersion;
		}
		string text = "store_name";
		if (TFUtils.isAmazon())
		{
			SBSettings.useStoreName = "amazon";
		}
		else
		{
			SBSettings.useStoreName = "google_play";
		}
		if (SBSettings.UseProductionIAP && !SBSettings.ConsoleLoggingEnabled)
		{
			TFUtils.LOG_LEVEL = TFUtils.LogLevel.WARN;
		}
		if (text != null && data.ContainsKey(text))
		{
			SBSettings.useStoreName = (string)data[text];
		}
		return true;
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0003E484 File Offset: 0x0003C684
	public static Version LastPromptedAppstoreVersion
	{
		get
		{
			return SBSettings.mutableLastPromptedAppstoreVersion;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0003E48C File Offset: 0x0003C68C
	public static bool IsLastRunVersion
	{
		get
		{
			if (SBSettings.mutableLastCheckedAppVersion == null || SBSettings.localBundleVersion == null)
			{
				return false;
			}
			if (SBSettings.mutableLastCheckedAppVersion != SBSettings.localBundleVersion)
			{
				SBSettings.mutableLastCheckedAppVersion = SBSettings.localBundleVersion;
				SBSettings.SaveMutableAppSetting("lastRAV1", SBSettings.mutableLastCheckedAppVersion.ToString());
				return false;
			}
			return true;
		}
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x0003E4F0 File Offset: 0x0003C6F0
	private static void LoadAppMutableSettings(Dictionary<string, object> mutableSettingsMap = null)
	{
		if (mutableSettingsMap == null && TFUtils.FileIsExists(SBSettings.MUTABLE_SETTINGS_PATH))
		{
			string json = TFUtils.ReadAllText(SBSettings.MUTABLE_SETTINGS_PATH);
			mutableSettingsMap = (Dictionary<string, object>)Json.Deserialize(json);
		}
		if (mutableSettingsMap != null)
		{
			object obj = null;
			if (mutableSettingsMap.TryGetValue("lastASV2", out obj))
			{
				SBSettings.mutableLastPromptedAppstoreVersion = new Version((string)obj);
			}
			else
			{
				SBSettings.mutableLastPromptedAppstoreVersion = new Version("0.0.0");
			}
			obj = null;
			if (mutableSettingsMap.TryGetValue("lastRAV1", out obj))
			{
				SBSettings.mutableLastCheckedAppVersion = new Version((string)obj);
			}
			else
			{
				SBSettings.mutableLastCheckedAppVersion = new Version("0.0.0");
			}
		}
		else
		{
			SBSettings.mutableLastPromptedAppstoreVersion = new Version("0.0.0");
			SBSettings.mutableLastCheckedAppVersion = new Version("0.0.0");
		}
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x0003E5C4 File Offset: 0x0003C7C4
	private static void SaveMutableAppSetting(string key, object value)
	{
		Dictionary<string, object> dictionary;
		if (TFUtils.FileIsExists(SBSettings.MUTABLE_SETTINGS_PATH))
		{
			string json = TFUtils.ReadAllText(SBSettings.MUTABLE_SETTINGS_PATH);
			dictionary = (Dictionary<string, object>)Json.Deserialize(json);
		}
		else
		{
			dictionary = new Dictionary<string, object>();
		}
		dictionary[key] = value;
		string contents = Json.Serialize(dictionary);
		File.WriteAllText(SBSettings.MUTABLE_SETTINGS_PATH, contents);
		SBSettings.LoadAppMutableSettings(dictionary);
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0003E624 File Offset: 0x0003C824
	public static void SaveLastPromptedAppstoreVersion()
	{
		SBSettings.SaveMutableAppSetting("lastASV2", SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.ToString());
	}

	// Token: 0x040006CE RID: 1742
	public const string LAST_PROMPTED_APPSTORE_VERSION_FIELD = "lastASV2";

	// Token: 0x040006CF RID: 1743
	private const string LAST_RUN_APP_VERSION = "lastRAV1";

	// Token: 0x040006D0 RID: 1744
	private const string MUTABLE_SETTINGS_FILE = "app_settings.json";

	// Token: 0x040006D1 RID: 1745
	private static string cdnUrl;

	// Token: 0x040006D2 RID: 1746
	private static string manifestUrl;

	// Token: 0x040006D3 RID: 1747
	private static string manifestFile;

	// Token: 0x040006D4 RID: 1748
	private static string serverUrl;

	// Token: 0x040006D5 RID: 1749
	private static int saveInterval;

	// Token: 0x040006D6 RID: 1750
	private static int patchingFileLimit;

	// Token: 0x040006D7 RID: 1751
	private static int retryCount;

	// Token: 0x040006D8 RID: 1752
	private static int? analyticsBufferSize;

	// Token: 0x040006D9 RID: 1753
	private static bool debugDisplayControllers = false;

	// Token: 0x040006DA RID: 1754
	private static string bundleIdentifier;

	// Token: 0x040006DB RID: 1755
	private static string bundleVersion;

	// Token: 0x040006DC RID: 1756
	private static Version localBundleVersion;

	// Token: 0x040006DD RID: 1757
	private static string bundleShortVersion;

	// Token: 0x040006DE RID: 1758
	private static string storeAppUrl;

	// Token: 0x040006DF RID: 1759
	private static bool trackStatistics = false;

	// Token: 0x040006E0 RID: 1760
	private static float statisticsTrackingInterval;

	// Token: 0x040006E1 RID: 1761
	private static bool showDebug = false;

	// Token: 0x040006E2 RID: 1762
	private static bool enableRandomQuests = false;

	// Token: 0x040006E3 RID: 1763
	private static bool enableAutoQuests = true;

	// Token: 0x040006E4 RID: 1764
	private static bool enableShakeLogDump = false;

	// Token: 0x040006E5 RID: 1765
	private static bool assertDataValidity = false;

	// Token: 0x040006E6 RID: 1766
	private static bool consoleLogging = false;

	// Token: 0x040006E7 RID: 1767
	private static bool bypassPatching = false;

	// Token: 0x040006E8 RID: 1768
	private static bool soaringProduction = false;

	// Token: 0x040006E9 RID: 1769
	private static bool soaringQA = false;

	// Token: 0x040006EA RID: 1770
	private static bool useLegacySaveLoad = false;

	// Token: 0x040006EB RID: 1771
	private static string useStoreName;

	// Token: 0x040006EC RID: 1772
	private static bool rebootOnFocusChange = false;

	// Token: 0x040006ED RID: 1773
	private static bool rebootOnConnectionChange = false;

	// Token: 0x040006EE RID: 1774
	private static bool useProductionIAP = true;

	// Token: 0x040006EF RID: 1775
	private static float communityEventBannerPing = -1f;

	// Token: 0x040006F0 RID: 1776
	private static bool dumpLogOnAssert = true;

	// Token: 0x040006F1 RID: 1777
	private static bool offlineModeFriendPark = false;

	// Token: 0x040006F2 RID: 1778
	private static bool disableFriendPark = false;

	// Token: 0x040006F3 RID: 1779
	private static string deltaDNAEnvKey = string.Empty;

	// Token: 0x040006F4 RID: 1780
	private static Version currentAppStoreBundleVersion;

	// Token: 0x040006F5 RID: 1781
	private static string billingsKey = null;

	// Token: 0x040006F6 RID: 1782
	private static string MUTABLE_SETTINGS_PATH = Application.persistentDataPath + Path.DirectorySeparatorChar + "app_settings.json";

	// Token: 0x040006F7 RID: 1783
	private static Version mutableLastPromptedAppstoreVersion;

	// Token: 0x040006F8 RID: 1784
	private static Version mutableLastCheckedAppVersion;
}
