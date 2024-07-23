using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MiniJSON;
using UnityEngine;

namespace DeltaDNA
{
	// Token: 0x02000010 RID: 16
	public sealed class SDK : Singleton<SDK>
	{
		// Token: 0x06000082 RID: 130 RVA: 0x000049EC File Offset: 0x00002BEC
		private SDK()
		{
			this.Settings = new Settings();
			this.Transaction = new TransactionBuilder(this);
			this.eventStore = new EventStore(Settings.EVENT_STORAGE_PATH.Replace("{persistent_path}", Application.persistentDataPath), this.Settings.DebugMode);
			this.engageArchive = new EngageArchive(Settings.ENGAGE_STORAGE_PATH.Replace("{persistent_path}", Application.persistentDataPath));
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004AF0 File Offset: 0x00002CF0
		[Obsolete("Prefer 'StartSDK' instead, Init will be removed in a future update.")]
		public void Init(string envKey, string collectURL, string engageURL, string userID)
		{
			this.StartSDK(envKey, collectURL, engageURL, userID);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004B00 File Offset: 0x00002D00
		public void StartSDK(string envKey, string collectURL, string engageURL, string userID)
		{
			this.SetUserID(userID);
			this.EnvironmentKey = envKey;
			this.CollectURL = collectURL;
			this.EngageURL = engageURL;
			this.Platform = ClientInfo.Platform;
			this.SessionID = this.GetSessionID();
			this.initialised = true;
			this.TriggerDefaultEvents();
			if (this.Settings.BackgroundEventUpload && !base.IsInvoking("Upload"))
			{
				base.InvokeRepeating("Upload", (float)this.Settings.BackgroundEventUploadStartDelaySeconds, (float)this.Settings.BackgroundEventUploadRepeatRateSeconds);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004B94 File Offset: 0x00002D94
		public void NewSession()
		{
			this.SessionID = this.GetSessionID();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004BA4 File Offset: 0x00002DA4
		public void StopSDK()
		{
			this.LogDebug("Stopping SDK");
			this.RecordEvent("gameEnded");
			base.CancelInvoke();
			this.Upload();
			this.initialised = false;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004BDC File Offset: 0x00002DDC
		[Obsolete("Prefer 'RecordEvent' instead, Trigger will be removed in a future update.")]
		public void TriggerEvent(string eventName)
		{
			this.RecordEvent(eventName, new Dictionary<string, object>());
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004BEC File Offset: 0x00002DEC
		public void RecordEvent(string eventName)
		{
			this.RecordEvent(eventName, new Dictionary<string, object>());
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00004BFC File Offset: 0x00002DFC
		[Obsolete("Prefer 'RecordEvent' instead, Trigger will be removed in a future update.")]
		public void TriggerEvent(string eventName, EventBuilder eventParams)
		{
			this.RecordEvent(eventName, (eventParams != null) ? eventParams.ToDictionary() : new Dictionary<string, object>());
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004C1C File Offset: 0x00002E1C
		public void RecordEvent(string eventName, EventBuilder eventParams)
		{
			this.RecordEvent(eventName, (eventParams != null) ? eventParams.ToDictionary() : new Dictionary<string, object>());
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004C3C File Offset: 0x00002E3C
		[Obsolete("Prefer 'RecordEvent' instead, Trigger will be removed in a future update.")]
		public void TriggerEvent(string eventName, Dictionary<string, object> eventParams)
		{
			this.RecordEvent(eventName, eventParams);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004C48 File Offset: 0x00002E48
		public void RecordEvent(string eventName, Dictionary<string, object> eventParams)
		{
			if (!this.initialised)
			{
				throw new NotStartedException("You must first start the SDK via the StartSDK method");
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary[SDK.EV_KEY_NAME] = eventName;
			dictionary[SDK.EV_KEY_USER_ID] = this.UserID;
			dictionary[SDK.EV_KEY_SESSION_ID] = this.SessionID;
			dictionary[SDK.EV_KEY_TIMESTAMP] = this.GetCurrentTimestamp();
			if (!eventParams.ContainsKey(SDK.EP_KEY_PLATFORM))
			{
				eventParams.Add(SDK.EP_KEY_PLATFORM, this.Platform);
			}
			if (!eventParams.ContainsKey(SDK.EP_KEY_SDK_VERSION))
			{
				eventParams.Add(SDK.EP_KEY_SDK_VERSION, Settings.SDK_VERSION);
			}
			dictionary[SDK.EV_KEY_PARAMS] = eventParams;
			Debug.Log("[DDSDK eventRecord] " + Json.Serialize(dictionary));
			if (!string.IsNullOrEmpty(this.UserID))
			{
				if (!this.eventStore.Push(Json.Serialize(dictionary)))
				{
					this.LogWarning("Event Store full, unable to handle event");
				}
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004D48 File Offset: 0x00002F48
		public void RequestEngagement(string decisionPoint, Dictionary<string, object> engageParams, Action<Dictionary<string, object>> callback)
		{
			if (!this.initialised)
			{
				throw new NotStartedException("You must first start the SDK via the StartSDK method");
			}
			if (string.IsNullOrEmpty(this.EngageURL))
			{
				this.LogWarning("Engage URL not configured, can not make engagement.");
				return;
			}
			if (string.IsNullOrEmpty(decisionPoint))
			{
				this.LogWarning("No decision point set, can not make engagement.");
				return;
			}
			base.StartCoroutine(this.EngageCoroutine(decisionPoint, engageParams, callback));
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public void Upload()
		{
			if (!this.initialised)
			{
				throw new NotStartedException("You must first start the SDK via the StartSDK method");
			}
			if (this.IsUploading)
			{
				this.LogWarning("Event upload already in progress, aborting");
				return;
			}
			base.StartCoroutine(this.UploadCoroutine());
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004DF8 File Offset: 0x00002FF8
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00004E00 File Offset: 0x00003000
		public Settings Settings { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004E0C File Offset: 0x0000300C
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00004E14 File Offset: 0x00003014
		public TransactionBuilder Transaction { get; private set; }

		// Token: 0x06000094 RID: 148 RVA: 0x00004E20 File Offset: 0x00003020
		public void ClearPersistentData()
		{
			PlayerPrefs.DeleteKey(SDK.PF_KEY_USER_ID);
			PlayerPrefs.DeleteKey(SDK.PF_KEY_FIRST_RUN);
			PlayerPrefs.DeleteKey(SDK.PF_KEY_HASH_SECRET);
			PlayerPrefs.DeleteKey(SDK.PF_KEY_CLIENT_VERSION);
			PlayerPrefs.DeleteKey(SDK.PF_KEY_PUSH_NOTIFICATION_TOKEN);
			PlayerPrefs.DeleteKey(SDK.PF_KEY_ANDROID_REGISTRATION_ID);
			this.eventStore.Clear();
			this.engageArchive.Clear();
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00004E80 File Offset: 0x00003080
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00004E88 File Offset: 0x00003088
		public string EnvironmentKey { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004E94 File Offset: 0x00003094
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00004E9C File Offset: 0x0000309C
		public string CollectURL { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00004EA8 File Offset: 0x000030A8
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00004EB0 File Offset: 0x000030B0
		public string EngageURL { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004EBC File Offset: 0x000030BC
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00004EC4 File Offset: 0x000030C4
		public string SessionID { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00004ED0 File Offset: 0x000030D0
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00004ED8 File Offset: 0x000030D8
		public string Platform { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00004EE4 File Offset: 0x000030E4
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00004F18 File Offset: 0x00003118
		public string UserID
		{
			get
			{
				string @string = PlayerPrefs.GetString(SDK.PF_KEY_USER_ID, null);
				if (string.IsNullOrEmpty(@string))
				{
					this.LogDebug("No existing User ID found.");
					return null;
				}
				return @string;
			}
			private set
			{
				if (!string.IsNullOrEmpty(value))
				{
					PlayerPrefs.SetString(SDK.PF_KEY_USER_ID, value);
					PlayerPrefs.Save();
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00004F38 File Offset: 0x00003138
		public bool IsInitialised
		{
			get
			{
				return this.initialised;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004F40 File Offset: 0x00003140
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00004F48 File Offset: 0x00003148
		public bool IsUploading { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004F54 File Offset: 0x00003154
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x00004F88 File Offset: 0x00003188
		public string HashSecret
		{
			get
			{
				string @string = PlayerPrefs.GetString(SDK.PF_KEY_HASH_SECRET, null);
				if (string.IsNullOrEmpty(@string))
				{
					this.LogDebug("Event hashing not enabled.");
					return null;
				}
				return @string;
			}
			set
			{
				this.LogDebug("Setting Hash Secret '" + value + "'");
				PlayerPrefs.SetString(SDK.PF_KEY_HASH_SECRET, value);
				PlayerPrefs.Save();
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004FBC File Offset: 0x000031BC
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x00004FF0 File Offset: 0x000031F0
		public string ClientVersion
		{
			get
			{
				string @string = PlayerPrefs.GetString(SDK.PF_KEY_CLIENT_VERSION, null);
				if (string.IsNullOrEmpty(@string))
				{
					this.LogWarning("No client game version set.");
					return null;
				}
				return @string;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this.LogDebug("Setting ClientVersion '" + value + "'");
					PlayerPrefs.SetString(SDK.PF_KEY_CLIENT_VERSION, value);
					PlayerPrefs.Save();
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00005024 File Offset: 0x00003224
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x0000506C File Offset: 0x0000326C
		public string PushNotificationToken
		{
			get
			{
				string @string = PlayerPrefs.GetString(SDK.PF_KEY_PUSH_NOTIFICATION_TOKEN, null);
				if (string.IsNullOrEmpty(@string))
				{
					if (ClientInfo.Platform.Contains("IOS"))
					{
						this.LogWarning("No Apple push notification token set, sending push notifications to iOS devices will be unavailable.");
					}
					return null;
				}
				return @string;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					PlayerPrefs.SetString(SDK.PF_KEY_PUSH_NOTIFICATION_TOKEN, value);
					PlayerPrefs.Save();
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000508C File Offset: 0x0000328C
		// (set) Token: 0x060000AB RID: 171 RVA: 0x000050D4 File Offset: 0x000032D4
		public string AndroidRegistrationID
		{
			get
			{
				string @string = PlayerPrefs.GetString(SDK.PF_KEY_ANDROID_REGISTRATION_ID, null);
				if (string.IsNullOrEmpty(@string))
				{
					if (ClientInfo.Platform.Contains("ANDROID"))
					{
						this.LogWarning("No Android registration id set, sending push notifications to Android devices will be unavailable.");
					}
					return null;
				}
				return @string;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					PlayerPrefs.SetString(SDK.PF_KEY_ANDROID_REGISTRATION_ID, value);
					PlayerPrefs.Save();
				}
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000050F4 File Offset: 0x000032F4
		public override void OnDestroy()
		{
			if (this.eventStore != null && this.eventStore.GetType() == typeof(EventStore))
			{
				this.eventStore.Dispose();
			}
			if (this.engageArchive != null)
			{
				this.engageArchive.Save();
			}
			base.OnDestroy();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005150 File Offset: 0x00003350
		private void LogDebug(string message)
		{
			if (this.Settings.DebugMode)
			{
				Debug.Log("[DDSDK] " + message);
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005180 File Offset: 0x00003380
		private void LogWarning(string message)
		{
			Debug.LogWarning("[DDSDK] " + message);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005194 File Offset: 0x00003394
		private string GetSessionID()
		{
			return Guid.NewGuid().ToString();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000051B0 File Offset: 0x000033B0
		private string GetUserID()
		{
			string text = Settings.LEGACY_SETTINGS_STORAGE_PATH.Replace("{persistent_path}", Application.persistentDataPath);
			if (File.Exists(text))
			{
				this.LogDebug("Found a legacy file in " + text);
				using (FileStream fileStream = new FileStream(text, FileMode.Open, FileAccess.Read))
				{
					try
					{
						List<byte> list = new List<byte>();
						byte[] array = new byte[1024];
						while (fileStream.Read(array, 0, array.Length) > 0)
						{
							list.AddRange(array);
						}
						byte[] array2 = list.ToArray();
						string @string = Encoding.UTF8.GetString(array2, 0, array2.Length);
						Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
						if (dictionary.ContainsKey("userID"))
						{
							this.LogDebug("Found a legacy user id for player");
							return dictionary["userID"] as string;
						}
					}
					catch (Exception ex)
					{
						this.LogWarning("Problem reading legacy user id: " + ex.Message);
					}
				}
			}
			this.LogDebug("Creating a new user id for player");
			return Guid.NewGuid().ToString();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005308 File Offset: 0x00003508
		private string GetCurrentTimestamp()
		{
			return DateTime.UtcNow.ToString(Settings.EVENT_TIMESTAMP_FORMAT, CultureInfo.InvariantCulture);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000532C File Offset: 0x0000352C
		private IEnumerator UploadCoroutine()
		{
			this.IsUploading = true;
			try
			{
				this.eventStore.Swap();
				List<string> events = this.eventStore.Read();
				if (events.Count > 0)
				{
					this.LogDebug("Starting event upload");
					yield return base.StartCoroutine(this.PostEvents(events.ToArray(), delegate(bool succeeded)
					{
						if (succeeded)
						{
							this.LogDebug("Event upload successful");
							this.eventStore.Clear();
						}
						else
						{
							this.LogWarning("Event upload failed - try again later");
						}
					}));
				}
			}
			finally
			{
				this.IsUploading = false;
			}
			yield break;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005348 File Offset: 0x00003548
		private IEnumerator EngageCoroutine(string decisionPoint, Dictionary<string, object> engageParams, Action<Dictionary<string, object>> callback)
		{
			this.LogDebug("Starting engagement for '" + decisionPoint + "'");
			Dictionary<string, object> engageRequest = new Dictionary<string, object>
			{
				{
					"userID",
					this.UserID
				},
				{
					"decisionPoint",
					decisionPoint
				},
				{
					"sessionID",
					this.SessionID
				},
				{
					"version",
					Settings.ENGAGE_API_VERSION
				},
				{
					"sdkVersion",
					Settings.SDK_VERSION
				},
				{
					"platform",
					this.Platform
				},
				{
					"timezoneOffset",
					Convert.ToInt32(ClientInfo.TimezoneOffset)
				}
			};
			if (ClientInfo.Locale != null)
			{
				engageRequest.Add("locale", ClientInfo.Locale);
			}
			if (engageParams != null)
			{
				engageRequest.Add("parameters", engageParams);
			}
			string engageJSON = null;
			try
			{
				engageJSON = Json.Serialize(engageRequest);
			}
			catch (Exception ex)
			{
				Exception e = ex;
				this.LogWarning("Problem serialising engage request data: " + e.Message);
				yield break;
			}
			yield return base.StartCoroutine(this.EngageRequest(engageJSON, delegate(string response)
			{
				bool flag = false;
				if (response != null)
				{
					this.LogDebug("Using live engagement: " + response);
					this.engageArchive[decisionPoint] = response;
				}
				else if (this.engageArchive.Contains(decisionPoint))
				{
					this.LogWarning("Engage request failed, using cached response.");
					flag = true;
					response = this.engageArchive[decisionPoint];
				}
				else
				{
					this.LogWarning("Engage request failed");
				}
				Dictionary<string, object> dictionary = Json.Deserialize(response) as Dictionary<string, object>;
				if (flag)
				{
					dictionary["isCachedResponse"] = flag;
				}
				callback(dictionary);
			}));
			yield break;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005390 File Offset: 0x00003590
		private IEnumerator PostEvents(string[] events, Action<bool> resultCallback)
		{
			string bulkEvent = "{\"eventList\":[" + string.Join(",", events) + "]}";
			Debug.Log("[DDSDK bulkEvents: ]" + bulkEvent);
			string url;
			if (this.HashSecret != null)
			{
				string md5Hash = SDK.GenerateHash(bulkEvent, this.HashSecret);
				url = SDK.FormatURI(Settings.COLLECT_HASH_URL_PATTERN, this.CollectURL, this.EnvironmentKey, md5Hash);
			}
			else
			{
				url = SDK.FormatURI(Settings.COLLECT_URL_PATTERN, this.CollectURL, this.EnvironmentKey, null);
			}
			int attempts = 0;
			bool succeeded = false;
			do
			{
				yield return base.StartCoroutine(this.HttpPOST(url, bulkEvent, delegate(int status, string response)
				{
					if (status == 200 || status == 204)
					{
						succeeded = true;
					}
					else if (status == 100 && string.IsNullOrEmpty(response))
					{
						succeeded = true;
					}
					else
					{
						this.LogDebug(string.Concat(new object[]
						{
							"Error uploading events, Collect returned: ",
							status,
							" ",
							response
						}));
					}
				}));
				yield return new WaitForSeconds(this.Settings.HttpRequestRetryDelaySeconds);
			}
			while (!succeeded && ++attempts < this.Settings.HttpRequestMaxRetries);
			resultCallback(succeeded);
			yield break;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000053C8 File Offset: 0x000035C8
		private IEnumerator EngageRequest(string engagement, Action<string> callback)
		{
			string url;
			if (this.HashSecret != null)
			{
				string md5Hash = SDK.GenerateHash(engagement, this.HashSecret);
				url = SDK.FormatURI(Settings.ENGAGE_HASH_URL_PATTERN, this.EngageURL, this.EnvironmentKey, md5Hash);
			}
			else
			{
				url = SDK.FormatURI(Settings.ENGAGE_URL_PATTERN, this.EngageURL, this.EnvironmentKey, null);
			}
			yield return base.StartCoroutine(this.HttpPOST(url, engagement, delegate(int status, string response)
			{
				if (status == 200 || status == 100)
				{
					callback(response);
				}
				else
				{
					this.LogDebug("Error requesting engagement, Engage returned: " + status);
					callback(null);
				}
			}));
			yield break;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005400 File Offset: 0x00003600
		private IEnumerator HttpGET(string url, Action<int, string> responseCallback = null)
		{
			this.LogDebug("HttpGET " + url);
			WWW www = new WWW(url);
			yield return www;
			int statusCode = 0;
			if (www.error == null)
			{
				statusCode = 200;
				if (responseCallback != null)
				{
					responseCallback(statusCode, www.text);
				}
			}
			else
			{
				statusCode = SDK.ReadWWWResponse(www.error);
				if (responseCallback != null)
				{
					responseCallback(statusCode, null);
				}
			}
			yield break;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005438 File Offset: 0x00003638
		private IEnumerator HttpPOST(string url, string json, Action<int, string> responseCallback = null)
		{
			this.LogDebug("HttpPOST " + url + " " + json);
			WWWForm form = new WWWForm();
			Hashtable headers = form.headers;
			headers["Content-Type"] = "application/json";
			byte[] bytes = Encoding.UTF8.GetBytes(json);
			WWW www = new WWW(url, bytes, headers);
			yield return www;
			int statusCode = this.ReadWWWStatusCode(www);
			if (www.error == null)
			{
				if (responseCallback != null)
				{
					responseCallback(statusCode, www.text);
				}
			}
			else
			{
				this.LogDebug("WWW.error: " + www.error);
				if (responseCallback != null)
				{
					responseCallback(statusCode, null);
				}
			}
			yield break;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005480 File Offset: 0x00003680
		private static int ReadWWWResponse(string response)
		{
			MatchCollection matchCollection = Regex.Matches(response, "^.*\\s(\\d{3})\\s.*$");
			if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 0)
			{
				return Convert.ToInt32(matchCollection[0].Groups[1].Value);
			}
			return 0;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000054DC File Offset: 0x000036DC
		private int ReadWWWStatusCode(WWW www)
		{
			int result = 0;
			string key = "NULL";
			if (www.responseHeaders.ContainsKey(key))
			{
				string input = www.responseHeaders[key];
				MatchCollection matchCollection = Regex.Matches(input, "^HTTP.*\\s(\\d{3})\\s.*$");
				if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 0)
				{
					result = Convert.ToInt32(matchCollection[0].Groups[1].Value);
				}
			}
			else if (string.IsNullOrEmpty(www.error))
			{
				result = 200;
			}
			else
			{
				result = SDK.ReadWWWResponse(www.error);
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005588 File Offset: 0x00003788
		private static string FormatURI(string uriPattern, string apiHost, string envKey, string hash = null)
		{
			string text = uriPattern.Replace("{host}", apiHost);
			text = text.Replace("{env_key}", envKey);
			return text.Replace("{hash}", hash);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000055C0 File Offset: 0x000037C0
		private static string GenerateHash(string data, string secret)
		{
			MD5 md = MD5.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(data + secret);
			byte[] array = md.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000562C File Offset: 0x0000382C
		private void SetUserID(string userID)
		{
			if (string.IsNullOrEmpty(userID))
			{
				if (string.IsNullOrEmpty(this.UserID))
				{
					this.UserID = this.GetUserID();
				}
			}
			else if (this.UserID != userID)
			{
				PlayerPrefs.DeleteKey(SDK.PF_KEY_FIRST_RUN);
				this.UserID = userID;
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005694 File Offset: 0x00003894
		private void TriggerDefaultEvents()
		{
			if (this.Settings.OnFirstRunSendNewPlayerEvent && PlayerPrefs.GetInt(SDK.PF_KEY_FIRST_RUN, 1) > 0)
			{
				this.LogDebug("Sending 'newPlayer' event");
				EventBuilder eventParams = new EventBuilder().AddParam("userCountry", ClientInfo.CountryCode);
				this.RecordEvent("newPlayer", eventParams);
				PlayerPrefs.SetInt(SDK.PF_KEY_FIRST_RUN, 0);
			}
			if (this.Settings.OnInitSendGameStartedEvent)
			{
				this.LogDebug("Sending 'gameStarted' event");
				EventBuilder eventParams2 = new EventBuilder().AddParam("clientVersion", this.ClientVersion).AddParam("pushNotificationToken", this.PushNotificationToken).AddParam("androidRegistrationID", this.AndroidRegistrationID);
				this.RecordEvent("gameStarted", eventParams2);
			}
			if (this.Settings.OnInitSendClientDeviceEvent)
			{
				this.LogDebug("Sending 'clientDevice' event");
				EventBuilder eventParams3 = new EventBuilder().AddParam("deviceName", ClientInfo.DeviceName).AddParam("deviceType", ClientInfo.DeviceType).AddParam("hardwareVersion", ClientInfo.DeviceModel).AddParam("operatingSystem", ClientInfo.OperatingSystem).AddParam("operatingSystemVersion", ClientInfo.OperatingSystemVersion).AddParam("manufacturer", ClientInfo.Manufacturer).AddParam("timezoneOffset", ClientInfo.TimezoneOffset).AddParam("userLanguage", ClientInfo.LanguageCode);
				this.RecordEvent("clientDevice", eventParams3);
			}
		}

		// Token: 0x04000051 RID: 81
		private static readonly string PF_KEY_USER_ID = "DDSDK_USER_ID";

		// Token: 0x04000052 RID: 82
		private static readonly string PF_KEY_FIRST_RUN = "DDSDK_FIRST_RUN";

		// Token: 0x04000053 RID: 83
		private static readonly string PF_KEY_HASH_SECRET = "DDSDK_HASH_SECRET";

		// Token: 0x04000054 RID: 84
		private static readonly string PF_KEY_CLIENT_VERSION = "DDSDK_CLIENT_VERSION";

		// Token: 0x04000055 RID: 85
		private static readonly string PF_KEY_PUSH_NOTIFICATION_TOKEN = "DDSDK_PUSH_NOTIFICATION_TOKEN";

		// Token: 0x04000056 RID: 86
		private static readonly string PF_KEY_ANDROID_REGISTRATION_ID = "DDSDK_ANDROID_REGISTRATION_ID";

		// Token: 0x04000057 RID: 87
		private static readonly string EV_KEY_NAME = "eventName";

		// Token: 0x04000058 RID: 88
		private static readonly string EV_KEY_USER_ID = "userID";

		// Token: 0x04000059 RID: 89
		private static readonly string EV_KEY_SESSION_ID = "sessionID";

		// Token: 0x0400005A RID: 90
		private static readonly string EV_KEY_TIMESTAMP = "eventTimestamp";

		// Token: 0x0400005B RID: 91
		private static readonly string EV_KEY_PARAMS = "eventParams";

		// Token: 0x0400005C RID: 92
		private static readonly string EP_KEY_PLATFORM = "platform";

		// Token: 0x0400005D RID: 93
		private static readonly string EP_KEY_SDK_VERSION = "sdkVersion";

		// Token: 0x0400005E RID: 94
		public static readonly string AUTO_GENERATED_USER_ID;

		// Token: 0x0400005F RID: 95
		private bool initialised;

		// Token: 0x04000060 RID: 96
		private IEventStore eventStore;

		// Token: 0x04000061 RID: 97
		private EngageArchive engageArchive;
	}
}
