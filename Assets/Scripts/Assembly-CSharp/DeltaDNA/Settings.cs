using System;

namespace DeltaDNA
{
	// Token: 0x0200000B RID: 11
	public class Settings
	{
		// Token: 0x0600005B RID: 91 RVA: 0x00004318 File Offset: 0x00002518
		internal Settings()
		{
			this.DebugMode = false;
			this.OnFirstRunSendNewPlayerEvent = true;
			this.OnInitSendClientDeviceEvent = true;
			this.OnInitSendGameStartedEvent = true;
			this.HttpRequestRetryDelaySeconds = 2f;
			this.HttpRequestMaxRetries = 5;
			this.BackgroundEventUpload = true;
			this.BackgroundEventUploadStartDelaySeconds = 0;
			this.BackgroundEventUploadRepeatRateSeconds = 60;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005D RID: 93 RVA: 0x000043EC File Offset: 0x000025EC
		// (set) Token: 0x0600005E RID: 94 RVA: 0x000043F4 File Offset: 0x000025F4
		public bool OnFirstRunSendNewPlayerEvent { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00004400 File Offset: 0x00002600
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00004408 File Offset: 0x00002608
		public bool OnInitSendClientDeviceEvent { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00004414 File Offset: 0x00002614
		// (set) Token: 0x06000062 RID: 98 RVA: 0x0000441C File Offset: 0x0000261C
		public bool OnInitSendGameStartedEvent { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00004428 File Offset: 0x00002628
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00004430 File Offset: 0x00002630
		public bool DebugMode { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000443C File Offset: 0x0000263C
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00004444 File Offset: 0x00002644
		public float HttpRequestRetryDelaySeconds { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00004450 File Offset: 0x00002650
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00004458 File Offset: 0x00002658
		public int HttpRequestMaxRetries { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00004464 File Offset: 0x00002664
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000446C File Offset: 0x0000266C
		public bool BackgroundEventUpload { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004478 File Offset: 0x00002678
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00004480 File Offset: 0x00002680
		public int BackgroundEventUploadStartDelaySeconds { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000448C File Offset: 0x0000268C
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00004494 File Offset: 0x00002694
		public int BackgroundEventUploadRepeatRateSeconds { get; set; }

		// Token: 0x04000034 RID: 52
		internal static readonly string SDK_VERSION = "Unity SDK v3.2.1";

		// Token: 0x04000035 RID: 53
		internal static readonly string ENGAGE_API_VERSION = "4";

		// Token: 0x04000036 RID: 54
		internal static readonly string EVENT_STORAGE_PATH = "{persistent_path}/ddsdk/events/";

		// Token: 0x04000037 RID: 55
		internal static readonly string ENGAGE_STORAGE_PATH = "{persistent_path}/ddsdk/engage/";

		// Token: 0x04000038 RID: 56
		internal static readonly string LEGACY_SETTINGS_STORAGE_PATH = "{persistent_path}/GASettings.ini";

		// Token: 0x04000039 RID: 57
		internal static readonly string EVENT_TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

		// Token: 0x0400003A RID: 58
		internal static readonly string USERID_URL_PATTERN = "{host}/uuid";

		// Token: 0x0400003B RID: 59
		internal static readonly string COLLECT_URL_PATTERN = "{host}/{env_key}/bulk";

		// Token: 0x0400003C RID: 60
		internal static readonly string COLLECT_HASH_URL_PATTERN = "{host}/{env_key}/bulk/hash/{hash}";

		// Token: 0x0400003D RID: 61
		internal static readonly string ENGAGE_URL_PATTERN = "{host}/{env_key}";

		// Token: 0x0400003E RID: 62
		internal static readonly string ENGAGE_HASH_URL_PATTERN = "{host}/{env_key}/hash/{hash}";
	}
}
