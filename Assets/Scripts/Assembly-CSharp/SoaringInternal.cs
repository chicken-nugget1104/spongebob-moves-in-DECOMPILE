using System;
using MTools;
using UnityEngine;

// Token: 0x020003A3 RID: 931
public class SoaringInternal : SoaringObjectBase
{
	// Token: 0x06001A4D RID: 6733 RVA: 0x000AC0A8 File Offset: 0x000AA2A8
	private SoaringInternal() : base(SoaringObjectBase.IsType.Management)
	{
		this.mPlayerData = new SoaringPlayer();
		this.mSoaringModules = new SoaringDictionary();
		this.mSoaringData = new SoaringDictionary();
		this.mGamePurchasables = new SoaringDictionary();
		this.mSoaringData.addValue(new SoaringInternal.SoaringPlayerValue("authToken"), "authToken");
		this.mSoaringData.addValue(this, "SCInternal");
		this.mCommunityEventManager = new SoaringCommunityEventManager();
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06001A4F RID: 6735 RVA: 0x000AC178 File Offset: 0x000AA378
	public static Version GameVersion
	{
		get
		{
			return SoaringInternal.GAME_VERSION;
		}
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x000AC180 File Offset: 0x000AA380
	internal static void SetGameVersion(Version version)
	{
		SoaringInternal.GAME_VERSION = version;
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06001A51 RID: 6737 RVA: 0x000AC188 File Offset: 0x000AA388
	public static SoaringLoginType LoginType
	{
		get
		{
			return SoaringInternal.Login_Type;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001A52 RID: 6738 RVA: 0x000AC190 File Offset: 0x000AA390
	public static SoaringPlatformType PlatformType
	{
		get
		{
			return SoaringPlatform.Platform;
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001A53 RID: 6739 RVA: 0x000AC198 File Offset: 0x000AA398
	public static bool IsProductionMode
	{
		get
		{
			return SoaringInternal.SOARING_MODE == SoaringMode.Production;
		}
	}

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06001A54 RID: 6740 RVA: 0x000AC1A4 File Offset: 0x000AA3A4
	public string CurrentServer
	{
		get
		{
			return SoaringInternal.WEB_SDK;
		}
	}

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06001A55 RID: 6741 RVA: 0x000AC1AC File Offset: 0x000AA3AC
	public string CurrentContentURL
	{
		get
		{
			return SoaringInternal.WEB_CDN;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06001A56 RID: 6742 RVA: 0x000AC1B4 File Offset: 0x000AA3B4
	internal static SoaringDelegateArray Delegate
	{
		get
		{
			if (SoaringInternal.instance.mSoaringDelegate == null)
			{
				SoaringInternal.instance.mSoaringDelegate = new SoaringDelegateArray();
			}
			return SoaringInternal.instance.mSoaringDelegate;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06001A57 RID: 6743 RVA: 0x000AC1EC File Offset: 0x000AA3EC
	// (set) Token: 0x06001A58 RID: 6744 RVA: 0x000AC1F8 File Offset: 0x000AA3F8
	public static SoaringEncryption Encryption
	{
		get
		{
			return SoaringInternal.instance.mSoaringEncryption;
		}
		set
		{
			SoaringInternal.instance.mSoaringEncryption = value;
		}
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06001A59 RID: 6745 RVA: 0x000AC208 File Offset: 0x000AA408
	public static string GameID
	{
		get
		{
			return SoaringInternal.instance.mGameID;
		}
	}

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06001A5A RID: 6746 RVA: 0x000AC214 File Offset: 0x000AA414
	public static bool IsOnline
	{
		get
		{
			return SoaringInternal.UpdateConnectionStatus();
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06001A5B RID: 6747 RVA: 0x000AC21C File Offset: 0x000AA41C
	public static SoaringInternal instance
	{
		get
		{
			if (SoaringInternal.gInstance == null)
			{
				SoaringInternal.gInstance = new SoaringInternal();
			}
			return SoaringInternal.gInstance;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06001A5C RID: 6748 RVA: 0x000AC238 File Offset: 0x000AA438
	public SoaringEvents Events
	{
		get
		{
			return this.mSoaringEvents;
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06001A5D RID: 6749 RVA: 0x000AC240 File Offset: 0x000AA440
	// (set) Token: 0x06001A5E RID: 6750 RVA: 0x000AC24C File Offset: 0x000AA44C
	public static SoaringCampaign Campaign
	{
		get
		{
			return SoaringInternal.instance.mCampaign;
		}
		set
		{
			SoaringInternal.instance.mCampaign = value;
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06001A5F RID: 6751 RVA: 0x000AC25C File Offset: 0x000AA45C
	public SoaringPlayer Player
	{
		get
		{
			return this.mPlayerData;
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06001A60 RID: 6752 RVA: 0x000AC264 File Offset: 0x000AA464
	public SoaringAdServer AdServer
	{
		get
		{
			return this.mAdServer;
		}
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x06001A61 RID: 6753 RVA: 0x000AC26C File Offset: 0x000AA46C
	public SoaringCommunityEventManager CommunityEventManager
	{
		get
		{
			return this.mCommunityEventManager;
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06001A62 RID: 6754 RVA: 0x000AC274 File Offset: 0x000AA474
	public SoaringAnalytics Analytics
	{
		get
		{
			return this.mAnalytics;
		}
	}

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06001A63 RID: 6755 RVA: 0x000AC27C File Offset: 0x000AA47C
	public SoaringDictionary Purchasables
	{
		get
		{
			return this.mGamePurchasables;
		}
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x000AC284 File Offset: 0x000AA484
	public void RegisterDelegate(SoaringDelegate deleg)
	{
		if (deleg == null)
		{
			return;
		}
		if (this.mSoaringDelegate == null)
		{
			this.mSoaringDelegate = new SoaringDelegateArray();
		}
		this.mSoaringDelegate.RegisterDelegate(deleg);
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000AC2B0 File Offset: 0x000AA4B0
	public void UnregisterDelegate(SoaringDelegate deleg)
	{
		if (deleg == null)
		{
			return;
		}
		if (this.mSoaringDelegate == null)
		{
			this.mSoaringDelegate = new SoaringDelegateArray();
		}
		this.mSoaringDelegate.UnregisterDelegate(deleg);
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x000AC2DC File Offset: 0x000AA4DC
	public void UnregisterDelegate(Type type)
	{
		if (this.mSoaringDelegate == null)
		{
			this.mSoaringDelegate = new SoaringDelegateArray();
		}
		this.mSoaringDelegate.UnregisterDelegate(type);
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x000AC30C File Offset: 0x000AA50C
	public bool IsInitialized()
	{
		return this.mIsInitialized;
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x000AC314 File Offset: 0x000AA514
	public bool Initialize(string gameID, SoaringDelegate deleg, SoaringMode mode)
	{
		return this.Initialize(gameID, deleg, mode, SoaringPlatformType.System);
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x000AC320 File Offset: 0x000AA520
	public bool Initialize(string gameID, SoaringDelegate deleg, SoaringMode mode, SoaringPlatformType platform)
	{
		try
		{
			this.RegisterDelegate(deleg);
			if (string.IsNullOrEmpty(gameID))
			{
				SoaringInternal.Delegate.OnInitializing(false, "Invalid Game ID", null);
				return false;
			}
			if (!string.IsNullOrEmpty(this.mGameID))
			{
				SoaringInternal.Delegate.OnInitializing(false, "Soaring is Already Initialized", null);
				return false;
			}
			SoaringInternalProperties.Load();
			if (SoaringInternalProperties.ForceOfflineModeUser)
			{
				this.TriggerOfflineMode(true);
			}
			SoaringPlatform.Init(platform);
			SoaringInternal.SOARING_MODE = mode;
			if (SoaringInternal.SOARING_MODE == SoaringMode.Production)
			{
				SoaringInternal.WEB_SDK = SoaringInternalProperties.SoaringProductionURL;
				SoaringInternal.WEB_CDN = SoaringInternalProperties.SoaringProductionCDN;
			}
			else if (SoaringInternal.SOARING_MODE == SoaringMode.Testing)
			{
				SoaringInternal.WEB_SDK = SoaringInternalProperties.SoaringTestingURL;
				SoaringInternal.WEB_CDN = SoaringInternalProperties.SoaringTestingCDN;
			}
			else
			{
				SoaringInternal.WEB_SDK = SoaringInternalProperties.SoaringDevelopmentURL;
				SoaringInternal.WEB_CDN = SoaringInternalProperties.SoaringDevelopmentCDN;
			}
			if (string.IsNullOrEmpty(SoaringInternal.WEB_SDK))
			{
				SoaringInternal.Delegate.OnInitializing(false, "Invalid Web URL", null);
				return false;
			}
			this.mPlayerData.CanSaveUserCredentials = SoaringInternalProperties.SaveUserCredentials;
			this.mGameID = gameID;
			this.mSoaringData.addValue(gameID, "gameId");
			if (this.mSoaringObject == null)
			{
				Camera main = Camera.main;
				if (main != null)
				{
					this.mSoaringObject = main.gameObject;
				}
				if (this.mSoaringObject == null)
				{
					this.mSoaringObject = new GameObject("Soaring");
					UnityEngine.Object.DontDestroyOnLoad(this.mSoaringObject);
				}
			}
			this.mWebQueue = this.mSoaringObject.GetComponent<SCWebQueue>();
			if (this.mWebQueue == null)
			{
				this.mWebQueue = this.mSoaringObject.AddComponent<SCWebQueue>();
				this.mWebQueue.Initialize("2.1.0");
			}
			if (this.ForceLoginWithSaveCredentials())
			{
				this.mPlayerData.ClearSavedCredentials();
			}
			this.RegisterModules();
			this.mSoaringEvents = new SoaringEvents();
			if (SoaringInternalProperties.EnableVersions)
			{
				this.mVersions = new SoaringVersions(SoaringInternal.WEB_CDN);
			}
			if (SoaringInternalProperties.EnableAddressKeeper)
			{
				this.mAddressKeeper = new SoaringAddressKeeper();
				this.CheckForSoaringAddresses();
			}
			if (SoaringInternalProperties.EnableAdServer)
			{
				this.mAdServer = new SoaringAdServer();
				this.mAdServer.SetAdServerURL(SoaringInternal.WEB_CDN);
			}
			if (SoaringInternalProperties.EnableServerTimeVersions)
			{
				SoaringTime.Register();
			}
			if (SoaringInternalProperties.EnableAnalytics && this.mAnalytics == null)
			{
				this.mAnalytics = new SoaringAnalytics();
				this.mAnalytics.Initialize();
			}
			if (SoaringInternalProperties.SecureCommunication && !SoaringInternalProperties.ForceOfflineModeUser)
			{
				if (this.mSoaringStashedCall == null)
				{
					this.mSoaringStashedCall = new SoaringArray();
				}
				this.BeginHandshake();
			}
			else
			{
				this.HandleFinalGameInitialization(true);
			}
		}
		catch (Exception ex)
		{
			string text = ex.Message;
			if (text == null)
			{
				text = string.Empty;
			}
			SoaringDebug.Log("Initialization Failed: " + text + "\n" + ex.StackTrace, LogType.Error);
			this.TriggerOfflineMode(true);
			SoaringInternal.Delegate.OnInitializing(false, null, null);
		}
		return true;
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x000AC664 File Offset: 0x000AA864
	internal void HandleFinalGameInitialization(bool success)
	{
		this.mIsInitialized = true;
		SoaringInternal.Delegate.OnInitializing(success, null, null);
		if (SoaringInternalProperties.LoginOnInitialize)
		{
			this.mPlayerData.Load(null);
		}
		if (success)
		{
			this.HandleStashedCalls();
		}
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x000AC6A0 File Offset: 0x000AA8A0
	internal bool HasAuthorizedCredentials()
	{
		if (this.mPlayerData == null)
		{
			return false;
		}
		SoaringDebug.Log(string.Concat(new object[]
		{
			"HasAuthorizedCredentials: ",
			SoaringPlayer.ValidCredentials,
			" Save: ",
			this.mPlayerData.CanSaveUserCredentials
		}));
		return SoaringPlayer.ValidCredentials && this.mPlayerData.CanSaveUserCredentials;
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x000AC714 File Offset: 0x000AA914
	internal void ClearSoaringWebQueue()
	{
		if (this.mWebQueue != null)
		{
			this.mWebQueue.ClearConnections();
		}
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x000AC734 File Offset: 0x000AA934
	private void RestartSoaring()
	{
		this.ClearSoaringWebQueue();
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x000AC73C File Offset: 0x000AA93C
	private void RegisterModules()
	{
		if (SoaringInternalProperties.SecureCommunication)
		{
			this.RegisterModule(new SoaringHandshakeGetKeyModule(), false);
			this.RegisterModule(new SoaringHandshakeTestKeyModule(), false);
			this.RegisterModule(new SoaringHandshakeFinishKeyModule(), false);
		}
		this.RegisterModule(new SoaringLoginUserModule(), false);
		this.RegisterModule(new SoaringCreateUserModule(), false);
		this.RegisterModule(new SoaringGenerateUserModule(), false);
		this.RegisterModule(new SoaringGenerateInviteCodeModule(), false);
		this.RegisterModule(new SoaringUpdateUserModule(), false);
		this.RegisterModule(new SoaringApplyInviteCodeModule(), false);
		this.RegisterModule(new SoaringSearchUsersModule(), false);
		this.RegisterModule(new SoaringRetrieveFriendsModule(), false);
		this.RegisterModule(new SoaringSaveSessionModule(), false);
		this.RegisterModule(new SoaringRetrieveSessionModule(), false);
		this.RegisterModule(new SoaringRemoveFriendModule(), false);
		this.RegisterModule(new SoaringAddFriendModule(), false);
		this.RegisterModule(new SoaringCheckRewardModule(), false);
		this.RegisterModule(new SoaringRedeemRewardModule(), false);
		this.RegisterModule(new SoaringRetrieveMessagesModule(), false);
		this.RegisterModule(new SoaringAddFriendWithTagModule(), false);
		this.RegisterModule(new SoaringSendMessageModule(), false);
		this.RegisterModule(new SoaringMarkMessageReadModule(), false);
		this.RegisterModule(new SoaringRemoveFriendWithTagModule(), false);
		this.RegisterModule(new SoaringRetrieveUserProfileModule(), false);
		this.RegisterModule(new SoaringDownloadModule(), false);
		this.RegisterModule(new SoaringResetPasswordModule(), false);
		this.RegisterModule(new SoaringChangePasswordModule(), false);
		this.RegisterModule(new SoaringRenewPasswordModule(), false);
		this.RegisterModule(new SoaringRegisterDeviceModule(), false);
		this.RegisterModule(new SoaringLookupUserModule(), false);
		if (SoaringInternalProperties.EnableAnalytics)
		{
			this.RegisterModule(new SoaringSaveStatModule(), false);
			this.RegisterModule(new SoaringAnonymousSaveStatModule(), false);
		}
		this.RegisterModule(new SoaringVerifyServerRecieptModule(), false);
		this.RegisterModule(new SoaringRetrieveProductModule(), false);
		this.RegisterModule(new SoaringRetrievePurchasesModule(), false);
		this.RegisterModule(new SoaringRetrieveABCampaigndModule(), false);
		this.RegisterModule(new SoaringFireEventModule(), false);
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x000AC910 File Offset: 0x000AAB10
	public void RegisterModule(SoaringModule module)
	{
		this.RegisterModule(module, true);
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x000AC91C File Offset: 0x000AAB1C
	public void RegisterModule(SoaringModule module, bool safe)
	{
		if (module == null)
		{
			return;
		}
		string text = module.ModuleName();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (safe)
		{
			this.mSoaringModules.addValue(module, text);
		}
		else
		{
			this.mSoaringModules.addValue_unsafe(module, text);
		}
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x000AC968 File Offset: 0x000AAB68
	public void ClearOfflineMode()
	{
		SoaringInternal.S_CheckUpdateTimer = -1f;
		SoaringInternal.sIsOffline = false;
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x000AC97C File Offset: 0x000AAB7C
	private static bool UpdateConnectionStatus()
	{
		if (SoaringInternal.sIsOffline)
		{
			return false;
		}
		if (SoaringInternal.S_CheckUpdateTimer <= 0f || Time.realtimeSinceStartup - SoaringInternal.S_CheckUpdateTimer > 5f)
		{
			SoaringInternal.S_CacheIsOnline = true;
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				SoaringInternal.S_CacheIsOnline = false;
			}
			else if (Application.internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork && Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
			{
				SoaringInternal.S_CacheIsOnline = false;
			}
			else
			{
				NetworkPlayer player = Network.player;
				if (player.ipAddress == "127.0.0.1" || player.ipAddress == "0.0.0.0")
				{
					SoaringInternal.S_CacheIsOnline = false;
				}
			}
			SoaringInternal.S_CheckUpdateTimer = Time.realtimeSinceStartup;
		}
		return SoaringInternal.S_CacheIsOnline;
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x000ACA3C File Offset: 0x000AAC3C
	public void TriggerOfflineMode(bool trigger)
	{
		if (SoaringInternal.sIsOffline != trigger && !SoaringInternalProperties.ForceOfflineModeUser)
		{
			SoaringDebug.Log("Soaring: TriggerOfflineMode: " + trigger, LogType.Warning);
			SoaringInternal.sIsOffline = trigger;
			Soaring.Delegate.InternetStateChange(SoaringInternal.sIsOffline);
		}
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x000ACA8C File Offset: 0x000AAC8C
	private void CheckForSoaringAddresses()
	{
		if (string.IsNullOrEmpty(this.mGameID))
		{
			return;
		}
		SoaringDictionary data = new SoaringDictionary();
		this.CallModule("retrieveGameLinks", data, null);
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x000ACAC0 File Offset: 0x000AACC0
	public void Update(float deltaTime)
	{
		if (this.mAnalytics != null)
		{
			this.mAnalytics.Update(deltaTime);
		}
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x000ACADC File Offset: 0x000AACDC
	public void HandleOnApplicationPaused(bool paused)
	{
		try
		{
			if (paused)
			{
				SoaringInternal.S_CheckUpdateTimer = 0f;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringInternal: HandleOnApplicationPaused: " + ex.Message);
		}
	}

	// Token: 0x06001A77 RID: 6775 RVA: 0x000ACB3C File Offset: 0x000AAD3C
	public void HandleOnApplicationQuit()
	{
		if (this.mAnalytics != null)
		{
			this.mAnalytics.Shutdown();
		}
	}

	// Token: 0x06001A78 RID: 6776 RVA: 0x000ACB54 File Offset: 0x000AAD54
	public bool IsAuthorized()
	{
		if (this.mPlayerData == null)
		{
			return false;
		}
		string authToken = this.mPlayerData.AuthToken;
		return !string.IsNullOrEmpty(authToken);
	}

	// Token: 0x06001A79 RID: 6777 RVA: 0x000ACB88 File Offset: 0x000AAD88
	public bool CallModule(string moduleName, SoaringDictionary data, SoaringContext context)
	{
		if (context == null)
		{
			context = new SoaringContext();
		}
		if (string.IsNullOrEmpty(moduleName))
		{
			SoaringInternal.Delegate.OnComponentFinished(false, moduleName, "No Module Name Found", null, context);
			return false;
		}
		SoaringModule soaringModule = (SoaringModule)this.mSoaringModules.objectWithKey(moduleName);
		if (soaringModule == null)
		{
			SoaringInternal.Delegate.OnComponentFinished(false, moduleName, "No Module Found", null, context);
			return false;
		}
		if (soaringModule.ShouldEncryptCall() && SoaringInternal.Encryption != null && SoaringInternal.Encryption.HasExpired())
		{
			if (SoaringEncryption.IsEncryptionAvailable())
			{
				this.BeginHandshake();
			}
			this.mSoaringStashedCall.addObject(new SoaringInternal.SoaringStashedCall(moduleName, data, context));
			return true;
		}
		soaringModule.CallModule(this.mSoaringData, data, context);
		return true;
	}

	// Token: 0x06001A7A RID: 6778 RVA: 0x000ACC54 File Offset: 0x000AAE54
	internal bool ValidateUserNameLength(string userName)
	{
		return !string.IsNullOrEmpty(userName) && userName.Length >= 6 && userName.Length < 32;
	}

	// Token: 0x06001A7B RID: 6779 RVA: 0x000ACC80 File Offset: 0x000AAE80
	internal bool ValidateUserName(string userName, SoaringLoginType type)
	{
		int length = userName.Length;
		for (int i = 0; i < length; i++)
		{
			char c = userName[i];
			if (c < '!' || c > 'z' || c == '*' || (c == '@' && type != SoaringLoginType.Email) || (c == '\n' || c == '\0' || c == '$' || c == ' ' || c == '~' || c == ',' || (c == '.' && type != SoaringLoginType.Email)) || c == '\'' || c == '"' || c == '&' || c == '-')
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001A7C RID: 6780 RVA: 0x000ACD38 File Offset: 0x000AAF38
	internal SoaringDictionary GenerateAppDataDictionary()
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(SoaringInternal.GameVersion.ToString(), "version");
		return soaringDictionary;
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x000ACD68 File Offset: 0x000AAF68
	internal SoaringDictionary GenerateDeviceDataDictionary()
	{
		SoaringDictionary soaringDictionary;
		if (SoaringInternalProperties.EnableDeviceData)
		{
			soaringDictionary = SoaringPlatform.GenerateDeviceDictionary();
		}
		else
		{
			soaringDictionary = new SoaringDictionary(1);
			soaringDictionary.addValue(SoaringInternal.PlatformType.ToString().ToLower(), "platform");
		}
		return soaringDictionary;
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x000ACDB8 File Offset: 0x000AAFB8
	private void BeginHandshake()
	{
		if (SoaringInternal.Encryption != null)
		{
			SoaringInternal.Encryption.SetEncryptionKey(null);
		}
		SoaringDictionary data = new SoaringDictionary();
		this.CallModule("handshake_pt1", data, null);
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x000ACDF0 File Offset: 0x000AAFF0
	public void BeginHandshake(SoaringContextDelegate responder)
	{
		if (SoaringInternal.Encryption != null)
		{
			SoaringInternal.Encryption.SetEncryptionKey(null);
		}
		SoaringContext soaringContext = new SoaringContext();
		soaringContext.ContextResponder = responder;
		SoaringDictionary data = new SoaringDictionary();
		this.CallModule("handshake_pt1", data, soaringContext);
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x000ACE34 File Offset: 0x000AB034
	internal void RegisterUser(string userName, string password, string platformID, bool liteUser, SoaringLoginType type, bool internalRegister, SoaringContext context)
	{
		if (!this.IsInitialized())
		{
			return;
		}
		if (this.ForceLoginWithSaveCredentials())
		{
			this.Login(null, null, null, SoaringLoginType.Soaring, false, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		if (string.IsNullOrEmpty(userName))
		{
			userName = string.Empty;
		}
		if (string.IsNullOrEmpty(password))
		{
			password = string.Empty;
		}
		if (!this.ValidateUserNameLength(userName))
		{
			SoaringInternal.Delegate.OnRegisterUser(false, this.CreateInvalidCredentialsError("Invalid User Name: Must be between 6 and 32 characters"), null, context);
			return;
		}
		if (!this.ValidateUserName(userName, type))
		{
			SoaringInternal.Delegate.OnRegisterUser(false, this.CreateInvalidCredentialsError("Invalid User Name: Must be composed of A-Z, a-z, 0-9, or _"), null, context);
			return;
		}
		string text = this.PlatformKeyWithLoginType(type, false);
		if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(platformID))
		{
			soaringDictionary.addValue(platformID, text);
		}
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		soaringDictionary.addValue(liteUser, "autoCreated");
		soaringDictionary.addValue(userName, "tag");
		if (liteUser && type == SoaringLoginType.Soaring && string.IsNullOrEmpty(platformID) && string.IsNullOrEmpty(password))
		{
			password = Application.platform.ToString()[0].ToString();
			int length = userName.Length;
			for (int i = 0; i < length; i++)
			{
				int num = length - 1 - i;
				if (num >= 0)
				{
					char c = (char)((int)userName[num] + i);
					password += ((!this._IsAsciiLetterOrDigit(c)) ? string.Empty : c.ToString());
				}
			}
			if (password.Length < 8)
			{
				for (int j = password.Length; j < 8; j++)
				{
					password += "s";
				}
			}
		}
		if (!liteUser || type == SoaringLoginType.Soaring)
		{
			soaringDictionary.addValue(password, "password");
		}
		if (internalRegister)
		{
			soaringDictionary2.addValue("1", "tregister");
		}
		SoaringDictionary soaringDictionary3 = new SoaringDictionary();
		soaringDictionary.addValue(soaringDictionary3, "custom");
		soaringDictionary3.addValue(this.GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary3.addValue(this.GenerateAppDataDictionary(), "appInfo");
		SoaringPlayer.ValidCredentials = false;
		this.mPlayerData.Save();
		this.CallModule("registerUser", soaringDictionary, context);
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x000AD0A8 File Offset: 0x000AB2A8
	private bool _IsAsciiLetterOrDigit(char c)
	{
		return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9');
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x000AD0EC File Offset: 0x000AB2EC
	internal void Login(string userName, string password, string platformID, SoaringLoginType type, bool forceInternalRegister, SoaringContext context)
	{
		if (!this.IsInitialized())
		{
			return;
		}
		if (this.ForceLoginWithSaveCredentials())
		{
			userName = SoaringInternalProperties.DeveloperLoginTag;
			password = SoaringInternalProperties.DeveloperLoginPassword;
		}
		string text = this.PlatformKeyWithLoginType(type, false);
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		if (type == SoaringLoginType.Soaring)
		{
			if (string.IsNullOrEmpty(userName))
			{
				userName = string.Empty;
			}
			soaringDictionary.addValue(userName, "tag");
		}
		if (!string.IsNullOrEmpty(platformID) && text != null)
		{
			soaringDictionary.addValue(platformID, text);
		}
		if (!string.IsNullOrEmpty(password))
		{
			soaringDictionary.addValue(password, "password");
		}
		if (!SoaringPlayer.ValidCredentials || Soaring.Player.UserID != userName || Soaring.Player.Password != password)
		{
			SoaringPlayer.ValidCredentials = false;
			this.mPlayerData.Save();
		}
		SoaringDictionary soaringDictionary2 = new SoaringDictionary();
		soaringDictionary.addValue(soaringDictionary2, "custom");
		soaringDictionary2.addValue(this.GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary2.addValue(this.GenerateAppDataDictionary(), "appInfo");
		SoaringInternal.Login_Type = type;
		this.CallModule("loginUser", soaringDictionary, context);
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x000AD224 File Offset: 0x000AB424
	internal void LookupUser(string platformID, SoaringLoginType loginType, SoaringContext context)
	{
		SoaringDebug.Log("LookupUser: " + platformID, LogType.Warning);
		if (string.IsNullOrEmpty(platformID))
		{
			SoaringInternal.Delegate.OnLookupUser(false, this.CreateInvalidCredentialsError("No PlatformID"), context);
			return;
		}
		string key = "deviceId";
		if (loginType != SoaringLoginType.Soaring && loginType != SoaringLoginType.Device)
		{
			key = this.PlatformKeyWithLoginType(loginType, false);
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(platformID, key);
		SoaringDictionary soaringDictionary2 = new SoaringDictionary(2);
		soaringDictionary.addValue(soaringDictionary2, "custom");
		soaringDictionary2.addValue(this.GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary2.addValue(this.GenerateAppDataDictionary(), "appInfo");
		this.CallModule("lookupUser", soaringDictionary, context);
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x000AD2D8 File Offset: 0x000AB4D8
	internal void LookupUser(SoaringArray identifiers, SoaringContext context)
	{
		if (identifiers == null)
		{
			SoaringInternal.Delegate.OnLookupUser(false, this.CreateInvalidCredentialsError("No PlatformIDs"), context);
			return;
		}
		SoaringDebug.Log("LookupUser: " + identifiers.ToJsonString(), LogType.Warning);
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		SoaringDictionary soaringDictionary2 = new SoaringDictionary(2);
		soaringDictionary.addValue(soaringDictionary2, "custom");
		soaringDictionary.addValue(identifiers, "identifiers");
		soaringDictionary2.addValue(this.GenerateDeviceDataDictionary(), "deviceInfo");
		soaringDictionary2.addValue(this.GenerateAppDataDictionary(), "appInfo");
		this.CallModule("lookupUser", soaringDictionary, context);
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x000AD370 File Offset: 0x000AB570
	private bool ForceLoginWithSaveCredentials()
	{
		return !string.IsNullOrEmpty(SoaringInternalProperties.DeveloperLoginTag) && !string.IsNullOrEmpty(SoaringInternalProperties.DeveloperLoginPassword) && SoaringInternalProperties.EnableDeveloperLogin;
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x000AD3A0 File Offset: 0x000AB5A0
	internal void Login(SoaringContext context)
	{
		if (!this.IsInitialized())
		{
			return;
		}
		if (this.ForceLoginWithSaveCredentials())
		{
			this.Login(null, null, null, SoaringLoginType.Soaring, false, context);
		}
		else if (this.HasAuthorizedCredentials())
		{
			this.Login(this.mPlayerData.UserTag, this.mPlayerData.Password, null, SoaringLoginType.Soaring, false, context);
		}
		else
		{
			bool flag = true;
			if (flag)
			{
				this.GenerateUniqueNewUserName(true, context);
			}
		}
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x000AD418 File Offset: 0x000AB618
	internal void HandleLogin(SoaringLoginType type, bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
	{
		SoaringInternal.Delegate.OnAuthorize(success, error, Soaring.Player, context);
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x000AD430 File Offset: 0x000AB630
	internal string GeneratePassword()
	{
		string str = UnityEngine.Random.Range(1, 9).ToString();
		str += SystemInfo.deviceType.ToString()[0].ToString();
		str += UnityEngine.Random.Range(11, 99).ToString();
		str += ((UnityEngine.Random.Range(0, 1) != 1) ? "!" : "#");
		str += ((UnityEngine.Random.Range(0, 1) != 1) ? "T" : "s");
		return str + UnityEngine.Random.Range(0, 9).ToString();
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x000AD4EC File Offset: 0x000AB6EC
	internal void GenerateUniqueNewUserName(bool internalRegister, SoaringContext context)
	{
		if (!this.IsInitialized())
		{
			return;
		}
		SoaringInternal.Login_Type = SoaringLoginType.Soaring;
		SoaringDictionary data = null;
		if (internalRegister)
		{
			data = new SoaringDictionary();
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			if (context == null)
			{
				context = new SoaringContext();
			}
			soaringDictionary.addValue(this.GeneratePassword(), "password");
			soaringDictionary.addValue((int)SoaringInternal.Login_Type, "loginType");
			context.addValue(soaringDictionary, "tregister");
		}
		this.CallModule("retrieveNextAutogeneratedUserTag", data, context);
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x000AD574 File Offset: 0x000AB774
	internal void RetrieveUserProfile(string userID, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRetrieveUserProfile(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		if (!string.IsNullOrEmpty(userID))
		{
			soaringDictionary.addValue(userID, "userId");
		}
		this.CallModule("retrieveUserProfile", soaringDictionary, context);
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x000AD5EC File Offset: 0x000AB7EC
	internal void GenerateInviteCode()
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRetrieveInvitationCode(false, this.CreateInvalidAuthCodeError(), null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("retrieveInvitationCode", soaringDictionary, null);
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x000AD648 File Offset: 0x000AB848
	internal void ApplyInviteCode(string code, SoaringContext context)
	{
		if (!this.IsAuthorized() || string.IsNullOrEmpty(code))
		{
			SoaringInternal.Delegate.OnApplyInviteCode(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		if (!code.Contains("-"))
		{
			SoaringInternal.Delegate.OnApplyInviteCode(false, "Invalid Invite Code Format.", null, context);
			return;
		}
		if (code.Length != 9)
		{
			SoaringInternal.Delegate.OnApplyInviteCode(false, "Invalid Invite Code Length.", null, context);
			return;
		}
		code = code.ToUpper();
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(code, "invitationCode");
		this.CallModule("applyInvitationCode", soaringDictionary, null);
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x000AD718 File Offset: 0x000AB918
	internal void UpdatePlayerProfile(SoaringDictionary custom, SoaringContext context)
	{
		this.UpdatePlayerProfile(null, custom, context);
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x000AD724 File Offset: 0x000AB924
	internal void UpdatePlayerProfile(string tag, string status, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!string.IsNullOrEmpty(tag))
		{
			soaringDictionary.addValue(tag, "tag");
		}
		if (!string.IsNullOrEmpty(status))
		{
			soaringDictionary.addValue(status, "status");
		}
		this.UpdatePlayerProfile(soaringDictionary, null, context);
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x000AD778 File Offset: 0x000AB978
	internal void UpdatePlayerProfile(SoaringDictionary userData, SoaringDictionary custom, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnUpdatingUserProfile(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (userData != null)
		{
			string text = userData.soaringValue("emails");
			if (!string.IsNullOrEmpty(text))
			{
				if (!this.ValidateEmailFormat(text))
				{
					SoaringInternal.Delegate.OnUpdatingUserProfile(false, "Invalid Email Format", null, context);
					return;
				}
				SoaringArray soaringArray = new SoaringArray();
				soaringArray.addObject(text);
				soaringDictionary.addValue(soaringArray, "emails");
				userData.removeObjectWithKey("emails");
			}
			for (int i = 0; i < userData.count(); i++)
			{
				SoaringObjectBase val = userData.allValues()[i];
				string key = userData.allKeys()[i];
				soaringDictionary.addValue(val, key);
			}
		}
		if (custom != null)
		{
			soaringDictionary.addValue(custom, "custom");
		}
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("updateUserProfile", soaringDictionary, context);
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x000AD88C File Offset: 0x000ABA8C
	internal void UpdatePlayerFacebookID(string facebookID, string icon, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnUpdatingUserProfile(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (string.IsNullOrEmpty(facebookID))
		{
			SoaringInternal.Delegate.OnUpdatingUserProfile(false, "Invalid Facebook ID.", null, context);
			return;
		}
		if (!string.IsNullOrEmpty(icon))
		{
			soaringDictionary.addValue(icon, "icon");
		}
		soaringDictionary.addValue(facebookID, "facebookId");
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("updateUserProfile", soaringDictionary, context);
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x000AD938 File Offset: 0x000ABB38
	internal void FindUser(string tag, string email, string userId, string facebookId, SoaringContext context)
	{
		SoaringValue email2 = null;
		if (!string.IsNullOrEmpty(email))
		{
			email2 = new SoaringValue(email);
		}
		SoaringValue userId2 = null;
		if (!string.IsNullOrEmpty(userId))
		{
			userId2 = new SoaringValue(userId);
		}
		SoaringValue facebookId2 = null;
		if (!string.IsNullOrEmpty(facebookId))
		{
			facebookId2 = new SoaringValue(facebookId);
		}
		SoaringValue tag2 = null;
		if (!string.IsNullOrEmpty(tag))
		{
			tag2 = new SoaringValue(tag);
		}
		this.FindUserWithData(tag2, email2, userId2, facebookId2, context);
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x000AD9A4 File Offset: 0x000ABBA4
	internal void FindUsers(SoaringArray tag, SoaringArray email, SoaringArray userId, SoaringArray facebookId, SoaringContext context)
	{
		this.FindUserWithData(tag, email, userId, facebookId, context);
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x000AD9B4 File Offset: 0x000ABBB4
	internal void FindUserWithData(SoaringObjectBase tag, SoaringObjectBase email, SoaringObjectBase userId, SoaringObjectBase facebookId, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnFindUser(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (tag != null)
		{
			soaringDictionary.addValue("tag", "type");
			soaringDictionary.addValue(tag, "value");
		}
		else if (email != null)
		{
			soaringDictionary.addValue("email", "type");
			soaringDictionary.addValue(email, "value");
		}
		else if (userId != null)
		{
			if (userId.Type == SoaringObjectBase.IsType.Array)
			{
				soaringDictionary.addValue("userIds", "type");
			}
			else
			{
				soaringDictionary.addValue("userId", "type");
			}
			soaringDictionary.addValue(userId, "value");
		}
		else if (facebookId != null)
		{
			if (facebookId.Type == SoaringObjectBase.IsType.Array)
			{
				soaringDictionary.addValue("facebookIds", "type");
			}
			else
			{
				soaringDictionary.addValue("facebookId", "type");
			}
			soaringDictionary.addValue(facebookId, "value");
		}
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("searchUsers", soaringDictionary, context);
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x000ADB0C File Offset: 0x000ABD0C
	internal void RequestFriendships(SoaringArray userId, SoaringContext context)
	{
		this.RequestFriendship(null, null, userId, null, context);
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x000ADB1C File Offset: 0x000ABD1C
	internal void RequestFriendship(string tag, string email, string userId, SoaringContext context)
	{
		SoaringValue userId2 = null;
		if (!string.IsNullOrEmpty(userId))
		{
			userId2 = new SoaringValue(userId);
		}
		this.RequestFriendship(tag, email, userId2, null, context);
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x000ADB4C File Offset: 0x000ABD4C
	private void RequestFriendship(string tag, string email, SoaringObjectBase userId, object phld, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRequestFriend(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		string moduleName = "requestFriendship";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!string.IsNullOrEmpty(tag))
		{
			moduleName = "requestFriendshipWithTag";
			soaringDictionary.addValue(tag, "tag");
		}
		else if (!string.IsNullOrEmpty(email))
		{
			soaringDictionary.addValue(email, "email");
		}
		else if (userId != null)
		{
			soaringDictionary.addValue(userId, "userId");
		}
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule(moduleName, soaringDictionary, context);
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x000ADC08 File Offset: 0x000ABE08
	internal void RequestFriendshipWithCode(string code, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRequestFriend(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		if (string.IsNullOrEmpty(code))
		{
			SoaringInternal.Delegate.OnRequestFriend(false, "Invalid User Code", null, context);
			return;
		}
		if (code.Contains("-"))
		{
			this.ApplyInviteCode(code, context);
		}
		else
		{
			this.RequestFriendship(code, null, null, context);
		}
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x000ADC80 File Offset: 0x000ABE80
	internal void RemoveFriendship(string tag, string email, string userId, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRemoveFriend(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		string moduleName = "removeFriendship";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (!string.IsNullOrEmpty(tag))
		{
			moduleName = "removeFriendshipWithTag";
			soaringDictionary.addValue(tag, "tag");
		}
		else if (!string.IsNullOrEmpty(email))
		{
			soaringDictionary.addValue(email, "email");
		}
		else if (!string.IsNullOrEmpty(userId))
		{
			soaringDictionary.addValue(userId, "userId");
		}
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule(moduleName, soaringDictionary, context);
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x000ADD44 File Offset: 0x000ABF44
	internal void SendSessionData(SoaringDictionary data, SoaringContext context)
	{
		this.SendSessionData(SoaringSession.SessionType.OneWay, null, data, context);
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x000ADD50 File Offset: 0x000ABF50
	internal void SendSessionData(string tag, SoaringSession.SessionType sessionType, SoaringDictionary data, SoaringContext context)
	{
		if (!this.IsAuthorized() || data == null)
		{
			SoaringInternal.Delegate.OnSavingSessionData(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(data, "custom");
		soaringDictionary.addValue(SoaringSession.GetSoaringSessionTypeString(sessionType), "sessionType");
		if (!string.IsNullOrEmpty(tag))
		{
			soaringDictionary.addValue(tag, "label");
		}
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("saveGameSession", soaringDictionary, context);
	}

	// Token: 0x06001A9B RID: 6811 RVA: 0x000ADDF4 File Offset: 0x000ABFF4
	internal void SendSessionData(SoaringSession.SessionType sessionType, string sessionID, SoaringDictionary data, SoaringContext context)
	{
		if (!this.IsAuthorized() || data == null)
		{
			SoaringInternal.Delegate.OnSavingSessionData(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(data, "custom");
		soaringDictionary.addValue(SoaringSession.GetSoaringSessionTypeString(sessionType), "sessionType");
		if (!string.IsNullOrEmpty(sessionID))
		{
			soaringDictionary.addValue(sessionID, "gameSessionId");
		}
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("saveGameSession", soaringDictionary, context);
	}

	// Token: 0x06001A9C RID: 6812 RVA: 0x000ADE98 File Offset: 0x000AC098
	internal void RequestSessionData(string searchLabel, long timestamp, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(searchLabel, "label");
		if (timestamp > 0L)
		{
			soaringDictionary.addValue(timestamp, "datetime");
		}
		SoaringArray soaringArray = new SoaringArray(1);
		soaringArray.addObject(soaringDictionary);
		this.RequestSessionData(soaringArray, null, context);
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x000ADEF0 File Offset: 0x000AC0F0
	internal void RequestSessionData(SoaringArray identifiers, SoaringDictionary sort, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRequestingSessionData(false, this.CreateInvalidAuthCodeError(), null, null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (sort != null)
		{
			soaringDictionary.addValue(sort, "sort");
		}
		if (identifiers != null)
		{
			soaringDictionary.addValue(identifiers, "identifiers");
		}
		soaringDictionary.addValue(SoaringSession.GetSoaringSessionQueryTypeString(SoaringSession.QueryType.List2), "queryType");
		soaringDictionary.addValue(SoaringSession.GetSoaringSessionTypeString(SoaringSession.SessionType.PersistantOneWay), "sessionType");
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("retrieveGameSession", soaringDictionary, context);
	}

	// Token: 0x06001A9E RID: 6814 RVA: 0x000ADF9C File Offset: 0x000AC19C
	internal void ValidatePurchaseReciept(string reciept, SoaringPurchasable purchasable, string storeName, string userID, bool isProduction, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRecieptValidated(false, this.CreateInvalidAuthCodeError(), context);
			return;
		}
		if (string.IsNullOrEmpty(reciept))
		{
			SoaringInternal.Delegate.OnRecieptValidated(false, "Invalid Reciept: " + reciept, context);
			return;
		}
		if (string.IsNullOrEmpty(purchasable.ProductID))
		{
			SoaringInternal.Delegate.OnRecieptValidated(false, "Invalid Product ID: " + purchasable.ProductID, context);
			return;
		}
		if (context == null)
		{
			context = "ValidatePurchasableReciept";
		}
		context.addValue(purchasable, "purchasable");
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		if (string.IsNullOrEmpty(storeName))
		{
			storeName = this.PrimaryPlatformName();
		}
		soaringDictionary.addValue(storeName, "storeName");
		if (SoaringPlatform.Platform == SoaringPlatformType.iPhone)
		{
			soaringDictionary.addValue(reciept, "receipt");
		}
		else
		{
			soaringDictionary.addValue(reciept, "token");
			if (SoaringPlatform.Platform == SoaringPlatformType.Amazon)
			{
				soaringDictionary.addValue(userID, "userId");
				soaringDictionary.addValue(2, "version");
			}
		}
		soaringDictionary.addValue(purchasable.ProductID, "productId");
		soaringDictionary.addValue(purchasable.Amount, "amount");
		if (SoaringInternal.IsProductionMode && !isProduction)
		{
			soaringDictionary.addValue("staging", "__hostType__");
		}
		this.CallModule("validateIapReceipt", soaringDictionary, context);
	}

	// Token: 0x06001A9F RID: 6815 RVA: 0x000AE130 File Offset: 0x000AC330
	internal void RequestPurchasables(string store, string language, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRetrieveProducts(false, this.CreateInvalidAuthCodeError(), SoaringRetrieveProductModule.LoadCachedProductData(), context);
			return;
		}
		if (string.IsNullOrEmpty(language))
		{
			SoaringInternal.Delegate.OnRetrieveProducts(false, "Invalid language", SoaringRetrieveProductModule.LoadCachedProductData(), context);
			return;
		}
		if (string.IsNullOrEmpty(store))
		{
			store = this.PrimaryPlatformName();
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(store, "storeName");
		soaringDictionary.addValue(language, "language");
		this.CallModule("retrieveIapProducts", soaringDictionary, context);
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x000AE1D4 File Offset: 0x000AC3D4
	internal void RequestPurchases(string store, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRetrievePurchases(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		if (string.IsNullOrEmpty(store))
		{
			store = this.PrimaryPlatformName();
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(store, "storeName");
		this.CallModule("retrieveIapPurchases", soaringDictionary, context);
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x000AE23C File Offset: 0x000AC43C
	internal void CheckUserRewards()
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnCheckUserRewards(false, this.CreateInvalidAuthCodeError(), null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("retrieveVirtualGoodCoupons", soaringDictionary, null);
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x000AE298 File Offset: 0x000AC498
	internal void UpdateFriendsListWithLastSettings(int startRange, int endRange, SoaringContext context)
	{
		this.UpdateFriendsList(startRange, endRange, this.mFriendsLastOrder, this.mFriendsLastMode, context);
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x000AE2B0 File Offset: 0x000AC4B0
	internal void UpdateFriendsList(int startRange, int endRange, string order, string mode, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnUpdateFriendList(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		this.mFriendsLastOrder = order;
		this.mFriendsLastMode = mode;
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		if (!string.IsNullOrEmpty(order) && !string.IsNullOrEmpty(mode))
		{
			SoaringDictionary soaringDictionary2 = new SoaringDictionary();
			soaringDictionary2.addValue(mode, "field");
			soaringDictionary2.addValue(order, "mode");
			soaringDictionary.addValue(soaringDictionary2, "sort");
		}
		if (startRange != -1 && endRange != -1)
		{
			SoaringArray soaringArray = new SoaringArray(2);
			soaringArray.addObject(startRange);
			soaringArray.addObject(endRange);
			soaringDictionary.addValue(soaringArray, "range");
		}
		this.CallModule("retrieveFriendsList", soaringDictionary, context);
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x000AE3A4 File Offset: 0x000AC5A4
	internal void UpdateServerTime(SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnServerTimeUpdated(false, this.CreateInvalidAuthCodeError(), 0L, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("retrieveServerTime", soaringDictionary, context);
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x000AE400 File Offset: 0x000AC600
	internal void RedeemRewardCoupons(SoaringCoupon coupons)
	{
		SoaringArray soaringArray = null;
		if (coupons != null)
		{
			soaringArray = new SoaringArray(1);
			soaringArray.addObject(coupons);
		}
		this.RedeemRewardCoupons(soaringArray);
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x000AE42C File Offset: 0x000AC62C
	internal void RegisterDevice(string device_token, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnDeviceRegistered(false, this.CreateInvalidAuthCodeError(), context);
			return;
		}
		if (string.IsNullOrEmpty(device_token))
		{
			SoaringInternal.Delegate.OnDeviceRegistered(false, "Invalid Device Token", context);
			return;
		}
		string pushNotificationsProtocol = SoaringPlatform.PushNotificationsProtocol;
		if (string.IsNullOrEmpty(pushNotificationsProtocol))
		{
			SoaringInternal.Delegate.OnDeviceRegistered(false, "Unsupported Protocal", context);
			return;
		}
		string text = null;
		string @string = PlayerPrefs.GetString("trToken", string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			text = this.mPlayerData.UserID + "_" + device_token;
			text = MCommon.CreateStringHash(text);
			if (!string.IsNullOrEmpty(text) && text == @string)
			{
				SoaringInternal.Delegate.OnDeviceRegistered(true, null, context);
				return;
			}
			text = @string;
		}
		if (string.IsNullOrEmpty(text))
		{
			if (context == null)
			{
				context = new SoaringContext();
			}
			context.addValue(text, "trToken");
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(device_token, "token");
		soaringDictionary.addValue(pushNotificationsProtocol, "protocol");
		this.CallModule("registerDevice", soaringDictionary, context);
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x000AE580 File Offset: 0x000AC780
	internal void RedeemRewardCoupons(SoaringArray coupons)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRedeemUserReward(false, this.CreateInvalidAuthCodeError(), null);
			return;
		}
		if (coupons == null)
		{
			SoaringInternal.Delegate.OnRedeemUserReward(false, "Invalid Coupon", null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(coupons, "coupons");
		this.CallModule("tearVirtualGoodCoupons", soaringDictionary, null);
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x000AE604 File Offset: 0x000AC804
	internal void CheckUnreadMessages()
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnCheckMessages(false, this.CreateInvalidAuthCodeError(), null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		this.CallModule("retrieveUnreadMessages", soaringDictionary, null);
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000AE660 File Offset: 0x000AC860
	internal void SendMessage(SoaringMessage message)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnSendMessage(false, this.CreateInvalidAuthCodeError(), null);
			return;
		}
		if (message == null)
		{
			SoaringInternal.Delegate.OnSendMessage(false, "Invalid Messages", null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(message, "messages");
		this.CallModule("sendMessage", soaringDictionary, null);
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x000AE6E4 File Offset: 0x000AC8E4
	internal void FireEvent(string eventName, SoaringDictionary custom, SoaringContext context = null)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnSendMessage(false, this.CreateInvalidAuthCodeError(), null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(eventName, "event_name");
		soaringDictionary.addValue(custom, "custom");
		this.CallModule("fireEvent", soaringDictionary, context);
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x000AE75C File Offset: 0x000AC95C
	internal void MarkMessageAsRead(SoaringMessage message)
	{
		if (message == null)
		{
			SoaringInternal.Delegate.OnMessageStateChanged(false, "Invalid Messages", null);
			return;
		}
		SoaringArray soaringArray = new SoaringArray(1);
		soaringArray.addObject(message.MessageID);
		this.MarkMessageAsRead(soaringArray);
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x000AE7A8 File Offset: 0x000AC9A8
	internal void MarkMessageAsRead(SoaringArray message)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnMessageStateChanged(false, this.CreateInvalidAuthCodeError(), null);
			return;
		}
		if (message == null)
		{
			SoaringInternal.Delegate.OnMessageStateChanged(false, "Invalid Messages", null);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mPlayerData.AuthToken, "authToken");
		soaringDictionary.addValue(message, "messages");
		this.CallModule("markMessagesAsRead", soaringDictionary, null);
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x000AE82C File Offset: 0x000ACA2C
	internal void ResetPassword(string username, string email)
	{
		if (string.IsNullOrEmpty(username))
		{
			SoaringInternal.Delegate.OnPasswordReset(false, this.CreateInvalidCredentialsError("Invaid Username"));
			return;
		}
		if (!this.ValidateUserName(username, SoaringLoginType.Soaring))
		{
			SoaringInternal.Delegate.OnPasswordReset(false, this.CreateInvalidCredentialsError("Invaid Username"));
			return;
		}
		if (string.IsNullOrEmpty(email))
		{
			SoaringInternal.Delegate.OnPasswordReset(false, this.CreateInvalidCredentialsError("Invaid Email"));
			return;
		}
		if (!this.ValidateEmailFormat(email))
		{
			SoaringInternal.Delegate.OnPasswordReset(false, this.CreateInvalidCredentialsError("Invaid Email"));
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mGameID, "gameId");
		soaringDictionary.addValue(username, "tag");
		soaringDictionary.addValue(email, "email");
		this.CallModule("resetUserPassword", soaringDictionary, null);
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x000AE910 File Offset: 0x000ACB10
	internal void ResetPasswordConfirm(string username, string confirmCode, string password)
	{
		if (string.IsNullOrEmpty(username))
		{
			SoaringInternal.Delegate.OnPasswordResetConfirmed(false, this.CreateInvalidCredentialsError("Invaid Username"));
			return;
		}
		if (!this.ValidateUserName(username, SoaringLoginType.Soaring))
		{
			SoaringInternal.Delegate.OnPasswordResetConfirmed(false, this.CreateInvalidCredentialsError("Invaid Username"));
			return;
		}
		if (string.IsNullOrEmpty(confirmCode))
		{
			SoaringInternal.Delegate.OnPasswordResetConfirmed(false, "Invaid Confirm Code");
			return;
		}
		if (string.IsNullOrEmpty(password))
		{
			SoaringInternal.Delegate.OnPasswordResetConfirmed(false, "Invaid Confirm Code");
			return;
		}
		if (password.Length < 6 || password.Length > 16)
		{
			SoaringInternal.Delegate.OnPasswordResetConfirmed(false, "Invaid Confirm Code");
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(SoaringInternal.GameID, "gameId");
		soaringDictionary.addValue(username, "tag");
		soaringDictionary.addValue(password, "password");
		soaringDictionary.addValue(confirmCode, "resetToken");
		this.CallModule("renewUserPassword", soaringDictionary, null);
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000AEA30 File Offset: 0x000ACC30
	internal void ChangePassword(string oldPassword, string newPassword, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnPasswordChanged(false, this.CreateInvalidAuthCodeError(), context);
			return;
		}
		if (string.IsNullOrEmpty(oldPassword))
		{
			SoaringInternal.Delegate.OnPasswordChanged(false, this.CreateInvalidCredentialsError("Invaid Password"), context);
			return;
		}
		if (string.IsNullOrEmpty(newPassword))
		{
			SoaringInternal.Delegate.OnPasswordChanged(false, this.CreateInvalidCredentialsError("Invaid Password"), context);
			return;
		}
		if (newPassword.Length < 6 || newPassword.Length > 16)
		{
			SoaringInternal.Delegate.OnPasswordChanged(false, this.CreateInvalidCredentialsError("Password must be between 6 and 16 characters in length"), context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mAuthorizationToken, "authToken");
		soaringDictionary.addValue(oldPassword, "oldPassword");
		soaringDictionary.addValue(newPassword, "newPassword");
		this.CallModule("changeUserPassword", soaringDictionary, context);
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000AEB20 File Offset: 0x000ACD20
	internal void RequestCampaign(SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnRetrieveCampaign(false, this.CreateInvalidAuthCodeError(), null, context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(this.mAuthorizationToken, "authToken");
		this.CallModule("retrieveABCampaignData", soaringDictionary, context);
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000AEB78 File Offset: 0x000ACD78
	internal void SaveStat(string key, SoaringObjectBase value)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && this.mAnalytics == null)
		{
			this.mAnalytics = new SoaringAnalytics();
			this.mAnalytics.Initialize();
		}
		if (this.mAnalytics == null)
		{
			return;
		}
		this.mAnalytics.LogEvent(key, value, 0);
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000AEBDC File Offset: 0x000ACDDC
	internal void SaveStat(SoaringArray entries)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && this.mAnalytics == null)
		{
			this.mAnalytics = new SoaringAnalytics();
			this.mAnalytics.Initialize();
		}
		if (this.mAnalytics == null)
		{
			return;
		}
		this.mAnalytics.LogEvents(entries, 0);
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x000AEC3C File Offset: 0x000ACE3C
	internal void SaveAnonymousStat(string key, SoaringObjectBase value)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && this.mAnalytics == null)
		{
			this.mAnalytics = new SoaringAnalytics();
			this.mAnalytics.Initialize();
		}
		if (this.mAnalytics == null)
		{
			return;
		}
		this.mAnalytics.LogAnonymousEvent(key, value);
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x000AEC9C File Offset: 0x000ACE9C
	internal void SaveAnonymousStat(SoaringArray entries)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && this.mAnalytics == null)
		{
			this.mAnalytics = new SoaringAnalytics();
			this.mAnalytics.Initialize();
		}
		if (this.mAnalytics == null)
		{
			return;
		}
		this.mAnalytics.LogAnonymousEvents(entries);
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x000AECFC File Offset: 0x000ACEFC
	internal void internal_SaveStat(string key, SoaringObjectBase value, SoaringContext context)
	{
		SoaringArray soaringArray = new SoaringArray(1);
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringArray.addObject(soaringDictionary);
		soaringDictionary.addValue(key, "key");
		soaringDictionary.addValue(value, "value");
		this.internal_SaveStat(soaringArray, context);
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x000AED44 File Offset: 0x000ACF44
	internal void internal_SaveStat(SoaringArray entries, SoaringContext context)
	{
		if (!this.IsAuthorized())
		{
			SoaringInternal.Delegate.OnSaveStat(false, false, this.CreateInvalidAuthCodeError(), context);
			return;
		}
		if (entries == null)
		{
			SoaringInternal.Delegate.OnSaveStat(false, false, "Invalid Entries", context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(entries, "entries");
		this.CallModule("saveStat", soaringDictionary, context);
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x000AEDB0 File Offset: 0x000ACFB0
	internal void internal_SaveAnonymousStat(string key, SoaringDictionary value, SoaringContext context)
	{
		SoaringArray soaringArray = new SoaringArray(1);
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringArray.addObject(soaringDictionary);
		soaringDictionary.addValue(key, "key");
		soaringDictionary.addValue(value, "value");
		this.internal_SaveAnonymousStat(soaringArray, context);
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x000AEDF8 File Offset: 0x000ACFF8
	internal void internal_SaveAnonymousStat(SoaringArray entries, SoaringContext context)
	{
		if (!SoaringInternalProperties.IsLoaded)
		{
			SoaringInternalProperties.Load();
		}
		if (SoaringInternalProperties.EnableAnalytics && this.mAnalytics == null)
		{
			this.mAnalytics = new SoaringAnalytics();
			this.mAnalytics.Initialize();
		}
		if (entries == null)
		{
			SoaringInternal.Delegate.OnSaveStat(false, true, "Invalid Entries", context);
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(entries, "entries");
		if (!this.IsAuthorized())
		{
			if (this.mSoaringStashedCall == null)
			{
				this.mSoaringStashedCall = new SoaringArray();
			}
			this.mSoaringStashedCall.addObject(new SoaringInternal.SoaringStashedCall("saveAnonymousStat", soaringDictionary, context));
		}
		else
		{
			this.CallModule("saveAnonymousStat", soaringDictionary, context);
		}
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x000AEEBC File Offset: 0x000AD0BC
	internal bool ValidateEmailFormat(string email)
	{
		if (email == null)
		{
			return false;
		}
		if (email.Length >= 256 || email.Length < 3)
		{
			return false;
		}
		int num = email.IndexOf('@');
		if (num <= 0)
		{
			return false;
		}
		int num2 = email.LastIndexOf('.');
		return num2 > 0 && num2 >= num && num + 1 <= num2 && num2 + 1 < email.Length;
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x000AEF34 File Offset: 0x000AD134
	internal void CheckFilesForUpdates(bool updateFiles)
	{
		this.mVersions.CheckFilesForUpdates(updateFiles);
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x000AEF44 File Offset: 0x000AD144
	internal bool PushCall(SoaringDictionary callData)
	{
		if (!Soaring.IsOnline)
		{
			bool flag = false;
			if (callData != null && callData.containsKey("trIOff"))
			{
				flag = true;
			}
			if (flag)
			{
				SoaringDebug.Log("PushCall: Game is offline", LogType.Error);
				return false;
			}
		}
		if (callData == null)
		{
			SoaringDebug.Log("PushCall: Invalid Call Data", LogType.Error);
			return false;
		}
		SCWebQueue.SCWebCallbackObject scwebCallbackObject = (SCWebQueue.SCWebCallbackObject)callData.objectWithKey("tcallback");
		if (scwebCallbackObject == null)
		{
			SoaringDebug.Log("PushCall: Invalid Callback", LogType.Error);
			return false;
		}
		SCWebQueue.SCWebCallbackObject scwebCallbackObject2 = (SCWebQueue.SCWebCallbackObject)callData.objectWithKey("tvcallback");
		SCWebQueue.SCWebQueueCallback verifyCallback = null;
		if (scwebCallbackObject2 != null)
		{
			verifyCallback = scwebCallbackObject2.callback;
		}
		string text = callData.soaringValue("turl");
		if (string.IsNullOrEmpty(text))
		{
			text = SoaringInternal.WEB_SDK;
		}
		int num = callData.soaringValue("tchannel");
		if (num < 0)
		{
			num = 0;
		}
		string saveData = callData.soaringValue("tsave");
		SoaringDictionary postData = (SoaringDictionary)callData.objectWithKey("tposts");
		SoaringDictionary urlData = (SoaringDictionary)callData.objectWithKey("tgets");
		object userData = callData.objectWithKey("tobject");
		return this.mWebQueue.StartConnection(num, userData, text, saveData, postData, urlData, scwebCallbackObject.callback, verifyCallback);
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x000AF084 File Offset: 0x000AD284
	public void PushContextEvent(SoaringContext context)
	{
		if (context == null)
		{
			return;
		}
		this.mWebQueue.RegisterEventMessage(context);
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x000AF09C File Offset: 0x000AD29C
	internal void UpdatePlayerData(SoaringDictionary data)
	{
		this.UpdatePlayerData(data, false);
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x000AF0A8 File Offset: 0x000AD2A8
	internal void UpdatePlayerData(SoaringDictionary data, bool clearData)
	{
		if (this.mPlayerData == null)
		{
			this.mPlayerData = new SoaringPlayer();
		}
		this.mPlayerData.SetUserData(data, clearData);
		this.mAuthorizationToken = this.mPlayerData.AuthToken;
		this.mPlayerData.Save();
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x000AF0F4 File Offset: 0x000AD2F4
	internal void SetSoaringInternalData(SoaringDictionary data)
	{
		if (data == null)
		{
			return;
		}
		SoaringDictionary soaringDictionary = (SoaringDictionary)data.objectWithKey("settings");
		if (soaringDictionary != null)
		{
			SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("encrypted");
			if (soaringDictionary2 != null)
			{
				this.mEncryptedModules = soaringDictionary2.makeCopy();
				this.CheckModulesForSecureConnection();
			}
			if (this.mSoaringEvents != null)
			{
				SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectWithKey("events");
				if (soaringArray != null)
				{
					this.mSoaringEvents.LoadEvents(soaringArray);
				}
			}
		}
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x000AF178 File Offset: 0x000AD378
	private void CheckModulesForSecureConnection()
	{
		if (this.mEncryptedModules == null)
		{
			return;
		}
		if (this.mSoaringModules == null)
		{
			return;
		}
		int num = this.mSoaringModules.count();
		for (int i = 0; i < num; i++)
		{
			SoaringModule soaringModule = (SoaringModule)this.mSoaringModules.objectAtIndex(i);
			if (soaringModule != null)
			{
				SoaringValue soaringValue = this.mEncryptedModules.soaringValue(soaringModule.ModuleName());
				if (soaringValue != null)
				{
					soaringModule.encryptedCall = soaringValue;
				}
			}
		}
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x000AF1FC File Offset: 0x000AD3FC
	internal string GetSoaringAddress(string key)
	{
		return this.mAddressKeeper.Address(key);
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x000AF20C File Offset: 0x000AD40C
	internal SoaringAddressKeeper AddressesKeeper
	{
		get
		{
			return this.mAddressKeeper;
		}
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x000AF214 File Offset: 0x000AD414
	internal SoaringVersions Versions
	{
		get
		{
			return this.mVersions;
		}
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x000AF21C File Offset: 0x000AD41C
	internal void DownloadFileWithSoaring(string name, string url, string path, SoaringContext context)
	{
		this.DownloadFileWithSoaring(name, url, path, null, context);
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x000AF22C File Offset: 0x000AD42C
	internal void DownloadFileWithSoaring(string name, string url, string path, SCWebQueue.SCDownloadCallback callback)
	{
		this.DownloadFileWithSoaring(name, url, path, callback, null);
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000AF23C File Offset: 0x000AD43C
	internal void DownloadFileWithSoaring(string name, string url, string path, SCWebQueue.SCDownloadCallback callback, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(name, "tobject");
		soaringDictionary.addValue(url, "turl");
		soaringDictionary.addValue(path, "tsave");
		if (callback != null)
		{
			soaringDictionary.addValue(new SCWebQueue.SCDownloadCallbackObject(callback), "tcallback");
		}
		this.CallModule("downloadFiles", soaringDictionary, context);
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x000AF2AC File Offset: 0x000AD4AC
	internal void HandleStashedCalls()
	{
		SoaringArray soaringArray = this.mSoaringStashedCall;
		if (soaringArray == null)
		{
			return;
		}
		if (soaringArray.count() == 0)
		{
			return;
		}
		this.mSoaringStashedCall = new SoaringArray();
		for (int i = 0; i < soaringArray.count(); i++)
		{
			SoaringInternal.SoaringStashedCall soaringStashedCall = null;
			try
			{
				soaringStashedCall = (SoaringInternal.SoaringStashedCall)soaringArray.objectAtIndex(i);
				this.CallModule(soaringStashedCall.ModuleName, soaringStashedCall.CallData, soaringStashedCall.Contex);
			}
			catch (Exception ex)
			{
				SoaringDebug.Log(string.Concat(new string[]
				{
					"Error In Module: ",
					soaringStashedCall.ModuleName,
					" : ",
					ex.Message,
					"\n",
					ex.StackTrace
				}), LogType.Error);
				throw ex;
			}
		}
		soaringArray.clear();
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x000AF390 File Offset: 0x000AD590
	internal string PlatformKeyWithLoginType(SoaringLoginType type, bool soaringPlatformDefault)
	{
		string result = (!soaringPlatformDefault) ? null : "tag";
		switch (type)
		{
		case SoaringLoginType.Device:
			result = "deviceId";
			break;
		case SoaringLoginType.GameCenter:
			result = "gamecenterId";
			break;
		case SoaringLoginType.Facebook:
			result = "facebookId";
			break;
		case SoaringLoginType.GoogleGS:
			result = "googleId";
			break;
		case SoaringLoginType.Amazon:
			result = "amazonId";
			break;
		}
		return result;
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x000AF40C File Offset: 0x000AD60C
	public static string PlatformKeyAbriviationWithLoginType(SoaringLoginType type, bool soaringPlatformDefault)
	{
		string result = (!soaringPlatformDefault) ? null : "tag";
		switch (type)
		{
		case SoaringLoginType.GameCenter:
			result = "gc1";
			break;
		case SoaringLoginType.Facebook:
			result = "fb1";
			break;
		case SoaringLoginType.GoogleGS:
			result = "gs1";
			break;
		case SoaringLoginType.Amazon:
			result = "ac1";
			break;
		}
		return result;
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x000AF478 File Offset: 0x000AD678
	public static SoaringLoginType PlatformKeyAbriviationWithTag(string userID)
	{
		if (string.IsNullOrEmpty(userID))
		{
			return SoaringLoginType.Soaring;
		}
		char[] separator = new char[]
		{
			':'
		};
		string[] array = userID.Split(separator);
		if (array == null)
		{
			return SoaringLoginType.Soaring;
		}
		if (array.Length == 0)
		{
			return SoaringLoginType.Soaring;
		}
		string a = array[0];
		SoaringLoginType result = SoaringLoginType.Soaring;
		if (a == "fb1")
		{
			return SoaringLoginType.Facebook;
		}
		if (a == "gc1")
		{
			return SoaringLoginType.GameCenter;
		}
		if (a == "ac1")
		{
			return SoaringLoginType.Amazon;
		}
		if (a == "gs1")
		{
			return SoaringLoginType.GoogleGS;
		}
		return result;
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x000AF508 File Offset: 0x000AD708
	private string PrimaryPlatformName()
	{
		return SoaringPlatform.PrimaryPlatformName;
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x000AF510 File Offset: 0x000AD710
	private SoaringError CreateInvalidAuthCodeError()
	{
		return new SoaringError("Invalid User Auth Code.", -2);
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x000AF520 File Offset: 0x000AD720
	private SoaringError CreateInvalidCredentialsError(string str)
	{
		return new SoaringError(str, -3);
	}

	// Token: 0x0400113C RID: 4412
	private const string SDK_VERSION = "2.1.0";

	// Token: 0x0400113D RID: 4413
	private static SoaringMode SOARING_MODE = SoaringMode.Development;

	// Token: 0x0400113E RID: 4414
	private static string WEB_SDK = null;

	// Token: 0x0400113F RID: 4415
	private static string WEB_CDN = null;

	// Token: 0x04001140 RID: 4416
	private static Version GAME_VERSION = new Version(0, 0, 0);

	// Token: 0x04001141 RID: 4417
	private static SoaringLoginType Login_Type = SoaringLoginType.Soaring;

	// Token: 0x04001142 RID: 4418
	private string mFriendsLastMode;

	// Token: 0x04001143 RID: 4419
	private string mFriendsLastOrder;

	// Token: 0x04001144 RID: 4420
	private SoaringDictionary mSoaringModules;

	// Token: 0x04001145 RID: 4421
	private SoaringDictionary mSoaringData;

	// Token: 0x04001146 RID: 4422
	private SoaringDelegateArray mSoaringDelegate;

	// Token: 0x04001147 RID: 4423
	private SoaringDictionary mEncryptedModules;

	// Token: 0x04001148 RID: 4424
	private SoaringEncryption mSoaringEncryption;

	// Token: 0x04001149 RID: 4425
	private SoaringPlayer mPlayerData;

	// Token: 0x0400114A RID: 4426
	private string mAuthorizationToken;

	// Token: 0x0400114B RID: 4427
	private string mGameID;

	// Token: 0x0400114C RID: 4428
	private SoaringArray mSoaringStashedCall;

	// Token: 0x0400114D RID: 4429
	private static SoaringInternal gInstance = null;

	// Token: 0x0400114E RID: 4430
	private GameObject mSoaringObject;

	// Token: 0x0400114F RID: 4431
	public SCWebQueue mWebQueue;

	// Token: 0x04001150 RID: 4432
	public LanguageCode mSoaringLanguage = LanguageCode.EN;

	// Token: 0x04001151 RID: 4433
	private SoaringVersions mVersions;

	// Token: 0x04001152 RID: 4434
	private SoaringAddressKeeper mAddressKeeper;

	// Token: 0x04001153 RID: 4435
	private SoaringCommunityEventManager mCommunityEventManager;

	// Token: 0x04001154 RID: 4436
	private SoaringAnalytics mAnalytics;

	// Token: 0x04001155 RID: 4437
	private SoaringAdServer mAdServer;

	// Token: 0x04001156 RID: 4438
	private SoaringEvents mSoaringEvents;

	// Token: 0x04001157 RID: 4439
	private SoaringDictionary mGamePurchasables;

	// Token: 0x04001158 RID: 4440
	private SoaringCampaign mCampaign;

	// Token: 0x04001159 RID: 4441
	private bool mIsInitialized;

	// Token: 0x0400115A RID: 4442
	private static bool sIsOffline = false;

	// Token: 0x0400115B RID: 4443
	private static bool S_CacheIsOnline = false;

	// Token: 0x0400115C RID: 4444
	private static float S_CheckUpdateTimer = 0f;

	// Token: 0x020003A4 RID: 932
	private class SoaringPlayerValue : SoaringValue
	{
		// Token: 0x06001ACE RID: 6862 RVA: 0x000AF52C File Offset: 0x000AD72C
		public SoaringPlayerValue(string key) : base(key)
		{
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x000AF538 File Offset: 0x000AD738
		public override string ToJsonString()
		{
			return "\"" + this.ToString() + "\"";
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x000AF550 File Offset: 0x000AD750
		public override string ToString()
		{
			return Soaring.Player.GetUserInfo(this.StringVal);
		}
	}

	// Token: 0x020003A5 RID: 933
	private class SoaringStashedCall : SoaringObjectBase
	{
		// Token: 0x06001AD1 RID: 6865 RVA: 0x000AF568 File Offset: 0x000AD768
		public SoaringStashedCall(string name, SoaringDictionary data, SoaringContext context) : base(SoaringObjectBase.IsType.Object)
		{
			this.ModuleName = name;
			this.CallData = data;
			this.Contex = context;
		}

		// Token: 0x0400115D RID: 4445
		public string ModuleName;

		// Token: 0x0400115E RID: 4446
		public SoaringDictionary CallData;

		// Token: 0x0400115F RID: 4447
		public SoaringContext Contex;
	}
}
