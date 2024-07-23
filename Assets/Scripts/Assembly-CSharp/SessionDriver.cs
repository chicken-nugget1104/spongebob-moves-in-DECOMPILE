using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt;
using DeltaDNA;
using Helpshift;
using UnityEngine;

// Token: 0x0200025E RID: 606
public class SessionDriver : MonoBehaviour
{
	// Token: 0x0600136C RID: 4972 RVA: 0x00085BC0 File Offset: 0x00083DC0
	private void Start()
	{
		if (TFUtils.isAmazon())
		{
			Upsight.init(this.amazonAppToken, this.amazonAppSecret, this.gcmProjectNumber);
			Upsight.requestAppOpen();
		}
		else
		{
			Upsight.init(this.androidAppToken, this.androidAppSecret, this.gcmProjectNumber);
			Upsight.requestAppOpen();
		}
		TFUtils.Init();
		TFUtils.RefreshSAFiles();
		new GameObject
		{
			name = "googleServiceListener"
		}.AddComponent<GPGSEventListener>();
		if (TFUtils.isAmazon())
		{
			AmazonIAPEventListener.getInstance();
			if (AmazonIapV2Impl.Instance == null)
			{
			}
		}
		this.session = new Session(1);
		SessionDriver.session_ref = this.session;
		AndroidBack.getInstance().addSession(this.session);
		SoaringPlatformType platform;
		if (TFUtils.isAmazon())
		{
			platform = SoaringPlatformType.Amazon;
		}
		else
		{
			platform = SoaringPlatformType.Android;
		}
		if (!Singleton<SDK>.Instance.IsInitialised)
		{
			Singleton<SDK>.Instance.ClientVersion = SBSettings.BundleVersion;
			Singleton<SDK>.Instance.Settings.DebugMode = SBSettings.ShowDebug;
			Singleton<SDK>.Instance.StartSDK(SBSettings.DeltaDNAEnvKey, "http://collect3106sbmvg.deltadna.net/collect/api", "http://engage3106sbmvg.deltadna.net", SDK.AUTO_GENERATED_USER_ID);
		}
		SoaringPlatform.Init(platform);
		SoaringTime.Load();
		Soaring.SaveAnonymousStat("anonymous_start", SoaringAnalytics.StampDeviceMetadata());
		if (Language.CurrentLanguage() == LanguageCode.N)
		{
			Language.Init(TFUtils.GetPersistentAssetsPath());
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("unityGameObject", "SessionDriver");
		dictionary.Add("enableInAppNotification", "yes");
		dictionary.Add("enableDialogUIForTablets", "no");
		dictionary.Add("presentFullScreenOniPad", "no");
		HelpshiftSdk.getInstance().install("9acc31b4614cba52a05eadb41240f3e5", "nick.helpshift.com", "nick_platform_20150814162842188-e0b3f9614ea849b", dictionary);
		base.gameObject.AddComponent<HelpshiftNotificationCount>();
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x00085D78 File Offset: 0x00083F78
	private void Update()
	{
		if (this.session != null)
		{
			this.session.ProcessStateChanges();
			this.session.OnUpdate();
		}
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x00085D9C File Offset: 0x00083F9C
	public void OnApplicationPause(bool paused)
	{
		TFUtils.DebugLog("Application pausing " + paused);
		if (this.session != null)
		{
			TFUtils.DebugLog("SessionState=" + this.session.TheState);
			this.session.OnPause(paused);
		}
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x00085DF0 File Offset: 0x00083FF0
	public void OnApplicationQuit()
	{
		TFUtils.DebugLog("Application quitting...");
		if (this.session != null)
		{
			TFUtils.DebugLog("SessionState=" + this.session.TheState);
			this.session.OnApplicationQuit();
		}
		Singleton<SDK>.Instance.StopSDK();
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x00085E44 File Offset: 0x00084044
	public void OnApplicationFocus(bool bFocus)
	{
		TFUtils.DebugLog("Application lost focus...");
		if (this.session != null)
		{
			TFUtils.DebugLog("SessionState=" + this.session.TheState);
			this.session.OnApplicationFocus(bFocus);
		}
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x00085E84 File Offset: 0x00084084
	public void OnMemoryWarning(string msg)
	{
		SoaringDebug.Log("OnMemoryWarning: Recieved from: " + msg, LogType.Warning);
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x00085E98 File Offset: 0x00084098
	private void onExternalMessage(string msg)
	{
		this.session.onExternalMessage(msg);
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06001373 RID: 4979 RVA: 0x00085EA8 File Offset: 0x000840A8
	// (set) Token: 0x06001374 RID: 4980 RVA: 0x00085EB0 File Offset: 0x000840B0
	public static Session session_ref { get; private set; }

	// Token: 0x06001375 RID: 4981 RVA: 0x00085EB8 File Offset: 0x000840B8
	private void LoginAndroid()
	{
		Debug.Log("---------------------loginAndroid");
		if (TFUtils.isAmazon())
		{
			this.ServiceEvent();
		}
		else
		{
			this.AuthenticationEvent();
		}
		if (TFUtils.isAmazon())
		{
			GameCenterBinding.authenticateLocalPlayer();
		}
		else
		{
			this.authenticationFailedEvent("NOT VALUE");
		}
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x00085F0C File Offset: 0x0008410C
	private void ServiceReadyHandler()
	{
		Debug.Log("ServiceReadyHandler");
		this.UnServiceEvent();
		this.SubscribeToProfileEvents();
		AGSProfilesClient.RequestLocalPlayerProfile();
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x00085F2C File Offset: 0x0008412C
	private void ServiceNotReadyHandler(string error)
	{
		Debug.Log("ServiceNotReadyHandler");
		this.UnServiceEvent();
		this.session = new Session(1);
		SessionDriver.session_ref = this.session;
		AndroidBack.getInstance().addSession(this.session);
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x00085F68 File Offset: 0x00084168
	private void ServiceEvent()
	{
		AGSClient.ServiceReadyEvent += this.ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent += this.ServiceNotReadyHandler;
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x00085F98 File Offset: 0x00084198
	private void UnServiceEvent()
	{
		AGSClient.ServiceReadyEvent -= this.ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent -= this.ServiceNotReadyHandler;
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x00085FC8 File Offset: 0x000841C8
	private void PlayerAliasReceived(AGSProfile profile)
	{
		Debug.Log("PlayerAliasReceived");
		Debug.Log("profile.playerId " + profile.playerId);
		this.UnsubscribeFromProfileEvents();
		this.session = new Session(1);
		SessionDriver.session_ref = this.session;
		AndroidBack.getInstance().addSession(this.session);
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x00086024 File Offset: 0x00084224
	private void PlayerAliasFailed(string errorMessage)
	{
		Debug.Log("PlayerAliasFailed " + errorMessage);
		this.UnsubscribeFromProfileEvents();
		this.session = new Session(1);
		SessionDriver.session_ref = this.session;
		AndroidBack.getInstance().addSession(this.session);
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x00086070 File Offset: 0x00084270
	private void SubscribeToProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent += this.PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent += this.PlayerAliasFailed;
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x000860A0 File Offset: 0x000842A0
	private void UnsubscribeFromProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent -= this.PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent -= this.PlayerAliasFailed;
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x000860D0 File Offset: 0x000842D0
	private void AuthenticationEvent()
	{
		GPGManager.authenticationSucceededEvent += this.authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent += this.authenticationFailedEvent;
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x00086100 File Offset: 0x00084300
	private void UnAuthenticationEvent()
	{
		GPGManager.authenticationSucceededEvent -= this.authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent -= this.authenticationFailedEvent;
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x00086130 File Offset: 0x00084330
	private void authenticationSucceededEvent(string param)
	{
		Debug.Log("authenticationSucceededEvent11: " + param);
		GPGPlayerInfo localPlayerInfo = PlayGameServices.getLocalPlayerInfo();
		this.UnAuthenticationEvent();
		this.session = new Session(1);
		SessionDriver.session_ref = this.session;
		AndroidBack.getInstance().addSession(this.session);
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x00086180 File Offset: 0x00084380
	private void authenticationFailedEvent(string error)
	{
		Debug.Log("authenticationFailedEvent: " + error);
		if ("Unknown error".Equals(error))
		{
			return;
		}
		this.UnAuthenticationEvent();
		this.session = new Session(1);
		SessionDriver.session_ref = this.session;
		AndroidBack.getInstance().addSession(this.session);
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x000861DC File Offset: 0x000843DC
	private void OnGUI()
	{
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.content.pm.ActivityInfo");
		int static2 = androidJavaClass2.GetStatic<int>("SCREEN_ORIENTATION_SENSOR_LANDSCAPE");
		@static.Call("setRequestedOrientation", new object[]
		{
			static2
		});
	}

	// Token: 0x04000D7D RID: 3453
	public const string KontagentAndroidKey = "eb9066e818664a89923bf70626a50288";

	// Token: 0x04000D7E RID: 3454
	public const string deltaDNACollectKey = "http://collect3106sbmvg.deltadna.net/collect/api";

	// Token: 0x04000D7F RID: 3455
	public const string deltaDNAEngageURL = "http://engage3106sbmvg.deltadna.net";

	// Token: 0x04000D80 RID: 3456
	public const string helpshiftAPIKey = "9acc31b4614cba52a05eadb41240f3e5";

	// Token: 0x04000D81 RID: 3457
	public const string helpshiftDomain = "nick.helpshift.com";

	// Token: 0x04000D82 RID: 3458
	public const string helpshiftAppID = "nick_platform_20150814162842188-e0b3f9614ea849b";

	// Token: 0x04000D83 RID: 3459
	private const int currentVersion = 1;

	// Token: 0x04000D84 RID: 3460
	[NonSerialized]
	public string androidAppToken = "6b8c7716edc0427b85dc87a0b6efad7f";

	// Token: 0x04000D85 RID: 3461
	[NonSerialized]
	public string androidAppSecret = "bf364b5161a642a6847264f2c9804b6d";

	// Token: 0x04000D86 RID: 3462
	[NonSerialized]
	public string gcmProjectNumber = string.Empty;

	// Token: 0x04000D87 RID: 3463
	[NonSerialized]
	public string iosAppToken = "e4e51f061d1c456e987ff997299ca4f8";

	// Token: 0x04000D88 RID: 3464
	[NonSerialized]
	public string iosAppSecret = "fa95d77b2246444eabf3877890e5ebf7";

	// Token: 0x04000D89 RID: 3465
	[NonSerialized]
	public string amazonAppToken = "d69c26b6915443a88f3d87d817e68bd9";

	// Token: 0x04000D8A RID: 3466
	[NonSerialized]
	public string amazonAppSecret = "d16c6edbcb2544cdb692418553d43b6a";

	// Token: 0x04000D8B RID: 3467
	public bool registerForPushNotifications;

	// Token: 0x04000D8C RID: 3468
	private Session session;
}
