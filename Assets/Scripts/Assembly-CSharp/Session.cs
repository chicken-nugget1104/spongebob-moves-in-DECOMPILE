using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Helpshift;
using MiniJSON;
using MTools;
using UnityEngine;
using Yarg;

// Token: 0x020001CC RID: 460
public class Session
{
	// Token: 0x06000F99 RID: 3993 RVA: 0x00064958 File Offset: 0x00062B58
	public Session(int currentVersion)
	{
		this.callbackQueue = new CallbackQueue();
		this.lastOnlineState = true;
		this.canChangeState = true;
		this.soaringEvents = new SoaringArray();
		this.framerateWatcher = new Session.FramerateWatcher();
		this.queuedStateChanges = new List<Session.StateChangeRequest>();
		this.externalRequests = new Dictionary<string, TFServer.JsonResponseHandler>();
		this.properties = new Session.SessionProperties();
		base..ctor();
		TFUtils.Init();
		SBSettings.Init();
		this.analytics = new SBAnalytics();
		this.actions = new List<Session.GameloopAction>();
		this.webFileServer = new SBWebFileServer();
		this.auth = new SBAuth(Application.platform);
		this.currentVersion = currentVersion;
		this.camera = new SBCamera();
		this.musicManager = MusicManager.CreateMusicManager();
		this.soundEffectManager = SoundEffectManager.CreateSoundEffectManager();
		this.pushNotificationManager = new PushNotificationManager(this);
		if (Session.debugManager == null)
		{
			Session.debugManager = new DebugManager(this);
		}
		this.InitScreenSwipeEffects();
		this.simulationContext = SBUIBuilder.PushNewScreenContext();
		this.simulationScratchScreen = SBUIBuilder.MakeAndPushScratchLayer(this);
		this.simulationScratchScreen.name = "Simulation Scratch Screen Layer";
		this.simulationScratchScreen.SetPosition(new Vector3(0f, 0f, GUIMainView.GetInstance().camera.farClipPlane - 0.1f));
		this.currentGuiContext = SBUIBuilder.PushNewScreenContext();
		this.state = Session.states["Authorizing"];
		this.reinitializeSession = false;
		this.PlayHavenController = new PlayHavenController();
		this.lastResetTime = DateTime.UtcNow;
		this.state.OnEnter(this);
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x00064AFC File Offset: 0x00062CFC
	static Session()
	{
		Session.states = new Dictionary<string, Session.State>();
		Session.states["CheckPatching"] = new Session.CheckPatching();
		Session.states["Stopping"] = new Session.Stopping();
		Session.states["Authorizing"] = new Session.Authorizing();
		Session.states["GameStarting"] = new Session.GameStarting();
		Session.states["GameStopping"] = new Session.GameStopping();
		Session.states["Playing"] = new Session.Playing();
		Session.states["SelectedPlaying"] = new Session.SelectedPlaying();
		Session.states["Shopping"] = new Session.Shopping();
		Session.states["Inventory"] = new Session.Inventory();
		Session.states["CommunityEvent"] = new Session.CommunityEventSession();
		Session.states["BrowsingRecipes"] = new Session.BrowsingRecipes();
		Session.states["Paving"] = new Session.Paving();
		Session.states["Placing"] = new Session.Placing();
		Session.states["MoveBuildingInEdit"] = new Session.MoveBuildingInEdit();
		Session.states["MoveBuildingInPlacement"] = new Session.MoveBuildingInPlacement();
		Session.states["MoveBuildingPanningInEdit"] = new Session.MoveBuildingPanningInEdit();
		Session.states["MoveBuildingPanningInPlacement"] = new Session.MoveBuildingPanningInPlacement();
		Session.states["DragFeeding"] = new Session.DragFeeding();
		Session.states["Sync"] = new Session.Sync();
		Session.states["InAppPurchasing"] = new Session.InAppPurchasing();
		Session.states["ShowingDialog"] = new Session.ShowingDialog();
		Session.states["HardSpendConfirm"] = new Session.HardSpendConfirm();
		Session.states["HardSpendPassthrough"] = new Session.HardSpendPassthrough();
		Session.states["InsufficientDialog"] = new Session.InsufficientDialog();
		Session.states["Expansion"] = new Session.NewExpansion();
		Session.states["Expanding"] = new Session.Expanding();
		Session.states["Clearing"] = new Session.Clearing();
		Session.states["Options"] = new Session.Options();
		Session.states["Movie"] = new Session.Movie();
		Session.states["Debug"] = new Session.SessionDebug();
		Session.states["ErrorDialog"] = new Session.ErrorDialog();
		Session.states["GetJelly"] = new Session.GetJelly();
		Session.states["Editing"] = new Session.Editing();
		Session.states["Credits"] = new Session.Credits();
		Session.states["SellBuildingConfirmation"] = new Session.SellBuildingConfirmation();
		Session.states["StashBuildingConfirmation"] = new Session.StashBuildingConfirmation();
		Session.states["vending"] = new Session.Vending();
		Session.states["resolve_user"] = new Session.ResolveUser();
		Session.states["visit_friend"] = new Session.VisitGameStarting();
		Session.states["UnitBusy"] = new Session.UnitBusy();
		Session.states["UnitIdle"] = new Session.UnitIdle();
		Session.states["AgeGate"] = new Session.AgeGate();
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000F9B RID: 3995 RVA: 0x00064E68 File Offset: 0x00063068
	// (set) Token: 0x06000F9C RID: 3996 RVA: 0x00064E70 File Offset: 0x00063070
	public bool InFriendsGame
	{
		get
		{
			return Session.PatchyTownGame;
		}
		set
		{
			Session.PatchyTownGame = value;
		}
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x00064E78 File Offset: 0x00063078
	public bool IsOnline()
	{
		return Soaring.IsOnline;
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x00064E80 File Offset: 0x00063080
	public void ClearUserState()
	{
		if (this.gameSaver != null)
		{
			this.gameSaver.Stop();
			this.gameSaver = null;
		}
		this.GameInitialized(false);
		if (this.game != null && this.game.store != null)
		{
			this.game.store.Reset(this);
		}
		if (this.game != null)
		{
			this.game.Clear();
			this.game = null;
		}
		UnityGameResources.Reset();
		SBUIBuilder.UpdateGuiEventHandler(this, delegate(SBGUIEvent e, Session session)
		{
		});
		SBGUI.GetInstance().ResetWhiteList();
		this.Auth.ResetAuth();
		this.ClearAsyncRequests();
		this.player = null;
		this.soaringEvents = new SoaringArray();
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x00064F50 File Offset: 0x00063150
	public void SaveGame()
	{
		this.saveGame = true;
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x00064F5C File Offset: 0x0006315C
	public void GameInitialized(bool initialized)
	{
		if (initialized)
		{
			this.notifyOnDisconnect = true;
			if (this.gameSaver == null)
			{
				this.gameSaver = new SBGamePersister(this);
				this.gameSaver.Start();
			}
			if (SBSettings.TrackStatistics)
			{
				if (this.statisticsTracker == null)
				{
					this.statisticsTracker = new GameObject("statisticsTracker");
					this.tracker = this.statisticsTracker.AddComponent<SBStatisticsTracker>();
					this.tracker.TheSession = this;
				}
				else
				{
					this.tracker.Paused = false;
				}
			}
		}
		else if (SBSettings.TrackStatistics && this.statisticsTracker != null)
		{
			this.tracker.Paused = true;
		}
		this.gameInitialized = initialized;
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x00065024 File Offset: 0x00063224
	public void OnPause(bool paused)
	{
		ulong ulPauseTime = 0UL;
		if (paused)
		{
			this.m_ulPauseTimestamp = new ulong?(TFUtils.EpochTime());
		}
		else if (this.m_ulPauseTimestamp != null)
		{
			ulPauseTime = TFUtils.EpochTime() - this.m_ulPauseTimestamp.Value;
			this.m_ulPauseTimestamp = null;
		}
		if (this.state == Session.states["InAppPurchasing"])
		{
			TFUtils.DebugLog("Returning from OnPause as it was not a true backgrounding event");
			return;
		}
		if (this.game != null && this.gameInitialized)
		{
			if (this.InFriendsGame || this.WasInFriendsGame)
			{
				return;
			}
			if (paused && !SBSettings.UseActionFile)
			{
				this.game.SaveLocally(0UL, true, false, false);
			}
			if (this.gameSaver != null)
			{
				if (paused)
				{
					this.gameSaver.Stop();
				}
				else
				{
					this.gameSaver.Start();
					NotificationManager.CancelAllNotifications();
					NotificationManager.AddAnnoyingNotifications(this.TheGame);
					this.game.treasureManager.StartTreasureTimers();
					this.analytics.UpdateGameValues(this.game);
					this.PlayHavenController.RequestContent("app_resume");
					if (this.TheGame != null && this.TheGame.resourceManager != null)
					{
						SoaringDictionary soaringDictionary = new SoaringDictionary();
						soaringDictionary.addValue(this.TheGame.resourceManager.PlayerLevelAmount, "level");
						soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
						ulong num = this.TheGame.FirstPlayTime();
						if (num != 0UL)
						{
							soaringDictionary.addValue(num, "first_play_time");
						}
						Soaring.FireEvent("ResumeGame", soaringDictionary);
					}
					AnalyticsWrapper.LogSessionBegin(this.game, ulPauseTime);
					this.game.analytics.LogSessionBegin();
				}
				this.HandleReset(false);
			}
			if (!paused && this.game.store != null && this.game.store.rmtEnabled && !this.game.store.receivedProductInfo)
			{
				if (Soaring.IsOnline)
				{
					TFUtils.DebugLog("Attempting to fetch products again");
					RmtStore.PreloadRmtProducts(this);
				}
			}
			else if (!paused && this.game.store != null)
			{
				TFUtils.DebugLog(string.Format("game.store: {0}; game.store.receivedProductInfo: {1}; game.store.premiumSupported: {2}", this.game.store, this.game.store.receivedProductInfo, this.game.store.rmtEnabled));
			}
			else if (!paused)
			{
				TFUtils.DebugLog("Unpaused while game.store is null");
			}
		}
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x000652D8 File Offset: 0x000634D8
	public void PurchasePremiumProduct(string productIdentifier)
	{
		if (this.TheGame.store.rmtEnabled)
		{
			this.TheGame.store.OpenTransaction(productIdentifier);
			this.properties.iapBundleName = productIdentifier;
			this.ChangeState("InAppPurchasing", true);
		}
		else
		{
			TFUtils.DebugLog("Not purchasing premium item; the current application does not support premium purchases.");
			TFUtils.TriggerIAPDisabledWarning();
		}
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x00065338 File Offset: 0x00063538
	public void StopGameSaveTimer()
	{
		if (this.gameSaver != null)
		{
			this.gameSaver.Stop();
		}
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x00065350 File Offset: 0x00063550
	public void OnApplicationQuit()
	{
		if (this.game != null && !SBSettings.UseActionFile)
		{
			if (this.InFriendsGame || this.WasInFriendsGame)
			{
				return;
			}
			if (this.InFriendsGame || this.WasInFriendsGame)
			{
				return;
			}
			if (TFUtils.isFastForwarding)
			{
				this.game.FastForwardSimulationFinished();
			}
			this.game.SaveLocally(0UL, false, false, true);
		}
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x000653C8 File Offset: 0x000635C8
	public void OnApplicationFocus(bool bFocus)
	{
		Debug.Log("OnApplicationFocus: " + bFocus);
		if (this.game != null)
		{
			if (this.gameInitialized && !bFocus && !SBSettings.UseActionFile)
			{
				if (this.InFriendsGame || this.WasInFriendsGame)
				{
					return;
				}
				this.game.SaveLocally(0UL, false, false, true);
				this.HandleReset(false);
				return;
			}
			else
			{
				if (!this.gameInitialized)
				{
					return;
				}
				if (bFocus && ((this.game.store != null && this.game.store.rmtEnabled && !Soaring.IsAuthorized) || this.game.store == null))
				{
					Debug.Log("Testing Online");
					if (!Soaring.IsOnline)
					{
						SoaringInternal.instance.ClearOfflineMode();
					}
					if (Soaring.IsOnline)
					{
						Debug.Log("Are authorized");
						this.game.SaveLocally(0UL, false, false, true);
						bool flag = this.InFriendsGame || this.WasInFriendsGame;
						Session.GameStarting.SplashScreenState splashScreenState = (!flag) ? Session.GameStarting.SplashScreenState.Loading : Session.GameStarting.SplashScreenState.Patchy;
						Session.GameStarting.ResetShowSplashScreen(splashScreenState);
						this.reinitializeSession = true;
					}
					else
					{
						if (this.resyncConnection)
						{
							return;
						}
						this.ChangeState("Sync", true);
						this.resyncConnection = true;
					}
				}
			}
		}
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x00065528 File Offset: 0x00063728
	public void HandleReset(bool forceReset)
	{
		if ((this.game == null && !this.gameInitialized) || this.InFriendsGame)
		{
			return;
		}
		if (!Soaring.IsOnline && SBSettings.RebootOnConnectionChange)
		{
			SoaringInternal.instance.ClearOfflineMode();
		}
		if (this.state == Session.states["InAppPurchasing"] || RmtStore.IsPurchasing || !Soaring.IsOnline)
		{
			return;
		}
		DateTime utcNow = DateTime.UtcNow;
		if ((DateTime.UtcNow - this.lastResetTime).TotalSeconds > 5.0)
		{
			this.lastResetTime = utcNow;
			if (forceReset)
			{
				this.reinitializeSession = true;
			}
			if (SBSettings.RebootOnFocusChange)
			{
				bool flag = this.InFriendsGame || this.WasInFriendsGame;
				Session.GameStarting.SplashScreenState splashScreenState = (!flag) ? Session.GameStarting.SplashScreenState.Loading : Session.GameStarting.SplashScreenState.Patchy;
				Session.GameStarting.ResetShowSplashScreen(splashScreenState);
				this.reinitializeSession = true;
			}
			else
			{
				long num = this.player.ReadTimestamp();
				if (num <= 0L)
				{
					this.reinitializeSession = true;
				}
				else
				{
					Soaring.RequestSessionData("game", num, Game.CreateSoaringGameResponderContext(new SoaringContextDelegate(this.OnTestContextResponder)));
				}
			}
		}
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x00065660 File Offset: 0x00063860
	public void OnTestContextResponder(SoaringContext context)
	{
		if (context == null)
		{
			return;
		}
		SoaringArray soaringArray = (SoaringArray)context.objectWithKey("custom");
		if (soaringArray == null)
		{
			return;
		}
		if (soaringArray.count() == 0)
		{
			return;
		}
		this.reinitializeSession = true;
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x000656A0 File Offset: 0x000638A0
	public void CheckForPatching(bool checkForUpdates)
	{
		if (this.contentPatcher == null)
		{
			this.justCheckForUpdates = checkForUpdates;
			this.contentPatcher = new SBContentPatcher();
			this.contentPatcher.AddListener(new Action<string>(this.OnPatchingEvent));
			this.checkForPatching = true;
		}
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x000656E0 File Offset: 0x000638E0
	private void OnPatchingEvent(string eventStr)
	{
		if (eventStr == "patchingNecessary")
		{
			if (this.state != Session.states["CheckPatching"] && this.game != null)
			{
				SoaringDebug.Log("Request Restart", LogType.Warning);
				this.HandleReset(true);
				this.contentPatcher = null;
			}
		}
		else if (eventStr == "patchingDone")
		{
			if (SoaringInternal.instance.Versions != null)
			{
				string fileHash = SoaringInternal.instance.Versions.GetFileHash("Languages/EN_Sheet1.xml");
				Debug.LogError("HASH: " + fileHash);
				string @string = PlayerPrefs.GetString("Languages/EN_Sheet1.xml", string.Empty);
				if (@string != fileHash && !string.IsNullOrEmpty(fileHash))
				{
					PlayerPrefs.SetString("Languages/EN_Sheet1.xml", fileHash);
					PlayerPrefs.Save();
				}
				Language.ResetHasInitialized();
				Language.Init(TFUtils.GetPersistentAssetsPath());
				string[] array = new string[]
				{
					"Sound/SoundEffects.json",
					"Sound/Music.json"
				};
				for (int i = 0; i < array.Length; i++)
				{
					fileHash = SoaringInternal.instance.Versions.GetFileHash(array[i]);
					@string = PlayerPrefs.GetString(array[i], string.Empty);
					if (@string != fileHash && !string.IsNullOrEmpty(fileHash))
					{
						PlayerPrefs.SetString(array[i], fileHash);
						PlayerPrefs.Save();
						if (!string.IsNullOrEmpty(@string))
						{
							this.ChangeState("Authorizing", true);
							break;
						}
					}
				}
			}
			if (this.contentPatcher != null)
			{
				TFUtils.RefreshSAFiles();
			}
			this.contentPatcher = null;
		}
		else if (eventStr == "patchingNotNecessary")
		{
			if (this.gameSaver != null)
			{
				this.SaveGame();
			}
			this.contentPatcher = null;
		}
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x000658A4 File Offset: 0x00063AA4
	public bool IsPatchingInProgress()
	{
		return this.contentPatcher != null;
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x000658B4 File Offset: 0x00063AB4
	public void ChangeState(string state, bool newContext = true)
	{
		if (!this.canChangeState)
		{
			return;
		}
		if (state == "CommunityEvent" && this.game.communityEventManager.GetActiveEvent() == null)
		{
			return;
		}
		SoaringDebug.Log("Change State: " + state, LogType.Error);
		Session.StateChangeRequest item = default(Session.StateChangeRequest);
		item.state = state;
		item.changeContext = newContext;
		List<Session.StateChangeRequest> obj = this.queuedStateChanges;
		lock (obj)
		{
			this.queuedStateChanges.Add(item);
		}
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x00065960 File Offset: 0x00063B60
	private void SetState(Session.StateChangeRequest request)
	{
		if (!(request.state == "Shopping") || this.state == Session.states["Shopping"])
		{
		}
		this.state.OnLeave(this);
		TFUtils.DebugLog(string.Concat(new string[]
		{
			"Session Change State: ",
			request.state,
			"\n(from: ",
			this.state.GetType().ToString(),
			")"
		}));
		this.state = Session.states[request.state];
		if (request.changeContext)
		{
			this.currentGuiContext = SBUIBuilder.PushNewScreenContext();
		}
		this.state.OnEnter(this);
		if (this.game != null)
		{
			this.game.OnChangeSessionState();
		}
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x00065A40 File Offset: 0x00063C40
	public void ProcessStateChanges()
	{
		if (this.queuedStateChanges.Count > 0)
		{
			Session.StateChangeRequest[] array = null;
			List<Session.StateChangeRequest> obj = this.queuedStateChanges;
			lock (obj)
			{
				array = this.queuedStateChanges.ToArray();
				this.queuedStateChanges.Clear();
			}
			foreach (Session.StateChangeRequest stateChangeRequest in array)
			{
				this.SetState(stateChangeRequest);
			}
		}
	}

	// Token: 0x06000FAE RID: 4014 RVA: 0x00065ADC File Offset: 0x00063CDC
	public void CheckForPatchingUpdate()
	{
		if (this.checkForPatching)
		{
			if (this.contentPatcher != null)
			{
				Session.CheckPatching @object = (Session.CheckPatching)Session.states["CheckPatching"];
				this.contentPatcher.AddListener(new Action<string>(@object.PatchingEventListener));
				Session.GameStarting state = (Session.GameStarting)Session.states["GameStarting"];
				this.contentPatcher.AddListener(delegate(string eventName)
				{
					state.PatchingEventListener(eventName, this);
				});
				this.contentPatcher.LoadManifest(this.justCheckForUpdates);
			}
			this.checkForPatching = false;
		}
	}

	// Token: 0x06000FAF RID: 4015 RVA: 0x00065B84 File Offset: 0x00063D84
	public void OnUpdate()
	{
		try
		{
			if (this.saveGame && this.game != null)
			{
				TFUtils.DebugLog("Saving game onUpdate");
				this.game.SaveToServer(this, TFUtils.EpochTime(), false, false);
				this.saveGame = false;
			}
			this.framerateWatcher.OnUpdate();
			this.CheckForPatchingUpdate();
			if (this.currentGuiContext.next != this.simulationContext)
			{
				SBUIBuilder.ReleaseScreenContexts(this.currentGuiContext.next, this.simulationContext);
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			int count = this.actions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.actions[i] != null)
				{
					this.actions[i]();
				}
			}
			this.actions.Clear();
			if (this.reinitializeSession)
			{
				TFUtils.DebugLog("Reinitializing session and entering sync state");
				this.ChangeState("Sync", true);
				TFUtils.DebugLog("Reinitializing session " + this.reinitializeSession);
			}
			else
			{
				if (SBSettings.EnableShakeLogDump && Input.acceleration.magnitude > 2f && TFUtils.EpochTime() - Session.logDumpShake > 30UL)
				{
					Session.logDumpShake = TFUtils.EpochTime();
					TFUtils.DebugLog("Shake detected.  Dumping Log..");
					TFUtils.LogDump(this, "shake", null, null);
				}
				if (this.game != null && this.game.needsReloadErrorDialog && this.gameInitialized)
				{
					this.game.needsReloadErrorDialog = false;
					Action okAction = delegate()
					{
						this.TheGame.RequestReload();
					};
					this.ErrorMessageHandler(this, Language.Get("!!ERROR_SAVE_GAME_FAILED_TITLE"), Language.Get("!!ERROR_RELOAD_GAME_MESSAGE"), Language.Get("!!PREFAB_OK"), okAction, 0.6f);
				}
				if (this.game != null && this.gameInitialized && this.canChangeState)
				{
					if (this.state == "Shopping")
					{
						this.marketpalceActive = true;
					}
					else if (this.state != "Shopping")
					{
						this.marketpalceActive = false;
					}
					bool isOnline = Soaring.IsOnline;
					if ((this.game.needsNetworkDownErrorDialog && this.notifyOnDisconnect) || (!isOnline && !this.isShowingOfflineDialog && this.lastOnlineState != isOnline))
					{
						this.notifyOnDisconnect = false;
						this.isShowingOfflineDialog = true;
						Action okAction2 = delegate()
						{
							this.game.needsNetworkDownErrorDialog = false;
							this.isShowingOfflineDialog = false;
							this.ChangeState("Playing", true);
						};
						this.ErrorMessageHandler(this, Language.Get("!!ERROR_OFFLINE_MODE_TITLE"), Language.Get("!!NOTIFY_INGAME_OFFLINE_MODE"), Language.Get("!!PREFAB_OK"), okAction2, 0.75f);
					}
					this.lastOnlineState = isOnline;
					if (this.soaringEvents.count() != 0)
					{
						for (int j = 0; j < this.soaringEvents.count(); j++)
						{
							SoaringEvent soaringEvent = (SoaringEvent)this.soaringEvents.objectAtIndex(j);
							if (soaringEvent.HasDisplayBannerEvent() && soaringEvent.Requires != null)
							{
								bool flag = true;
								try
								{
									for (int k = 0; k < soaringEvent.Requires.Length; k++)
									{
										SoaringEvent.SoaringEventRequirements soaringEventRequirements = soaringEvent.Requires[k];
										if (soaringEventRequirements.Key == "level")
										{
											int num = 0;
											if (int.TryParse(soaringEventRequirements.Value, out num))
											{
												int playerLevelAmount = this.TheGame.resourceManager.PlayerLevelAmount;
												if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.equal && playerLevelAmount == num)
												{
													goto IL_640;
												}
												if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.greaterThen && playerLevelAmount > num)
												{
													goto IL_640;
												}
												if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.lessThen && playerLevelAmount < num)
												{
													goto IL_640;
												}
												if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.lessThenEquals && playerLevelAmount <= num)
												{
													goto IL_640;
												}
												if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.greaterThenEquals && playerLevelAmount >= num)
												{
													goto IL_640;
												}
											}
											flag = false;
										}
										else if (soaringEventRequirements.Key == "has_item" || soaringEventRequirements.Key == "has_no_item")
										{
											int num2 = 0;
											if (int.TryParse(soaringEventRequirements.Value, out num2))
											{
												int num3 = 0;
												List<Simulated> list = this.TheGame.simulation.FindAllSimulateds(num2, null);
												foreach (Simulated simulated in list)
												{
													if ((simulated.Entity.AllTypes & EntityType.BUILDING) != EntityType.INVALID)
													{
														num3++;
													}
												}
												foreach (SBInventoryItem sbinventoryItem in this.TheGame.inventory.GetItems())
												{
													if (sbinventoryItem.entity.DefinitionId == num2)
													{
														num3++;
													}
												}
												if (soaringEventRequirements.Key == "has_item")
												{
													SoaringValue soaringValue = null;
													if (soaringEventRequirements.Custom != null)
													{
														soaringEventRequirements.Custom.soaringValue("count");
													}
													if (soaringValue != null)
													{
														int num4 = soaringValue;
														if (soaringEventRequirements.Key == "has_item")
														{
															if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.equal && num3 == num4)
															{
																goto IL_640;
															}
															if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.greaterThen && num3 > num4)
															{
																goto IL_640;
															}
															if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.lessThen && num3 < num4)
															{
																goto IL_640;
															}
															if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.lessThenEquals && num3 <= num4)
															{
																goto IL_640;
															}
															if (soaringEventRequirements.Sign == SoaringEvent.Equivelency.greaterThenEquals && num3 >= num4)
															{
																goto IL_640;
															}
														}
													}
													else if (num3 > 0)
													{
														goto IL_640;
													}
												}
												else if (soaringEventRequirements.Key == "has_no_item" && num3 == 0)
												{
													goto IL_640;
												}
											}
											flag = false;
										}
										else if (soaringEventRequirements.Key == "client_version")
										{
											if (!(SBSettings.BundleVersion == soaringEventRequirements.Value))
											{
												flag = false;
											}
										}
										IL_640:;
									}
								}
								catch (Exception ex)
								{
									TFUtils.LogDump(this, "Event_Error_Banners", ex, null);
									SoaringDebug.Log(ex.Message, LogType.Error);
								}
								if (flag)
								{
									soaringEvent.Requires = null;
									SoaringInternal.instance.Events.AddBannerEvent(soaringEvent);
								}
							}
							else
							{
								int num5 = soaringEvent.Actions.Length;
								int l = 0;
								while (l < num5)
								{
									if (soaringEvent.Actions[l].Type == SoaringEvent.SoaringEventActionType.Custom)
									{
										if (soaringEvent.Actions[l].Key == "spongy_games_banner")
										{
											Dictionary<string, object> dictionary = new Dictionary<string, object>();
											List<DialogInputData> list2 = new List<DialogInputData>(1);
											SoaringDictionary custom = soaringEvent.Actions[l].Custom;
											SoaringArray soaringArray = (SoaringArray)custom.objectWithKey("characters");
											int num6 = soaringArray.count();
											List<int> list3 = new List<int>(num6);
											for (int m = 0; m < num6; m++)
											{
												list3.Add((SoaringValue)soaringArray.objectAtIndex(m));
											}
											dictionary.Add("characters", list3);
											dictionary.Add("event_name", (SoaringValue)custom.objectWithKey("event_name"));
											dictionary.Add("event_days", (SoaringValue)custom.objectWithKey("event_days"));
											dictionary.Add("day", (SoaringValue)custom.objectWithKey("day"));
											dictionary.Add("title", (SoaringValue)custom.objectWithKey("title"));
											dictionary.Add("description_one", (SoaringValue)custom.objectWithKey("description_one"));
											dictionary.Add("description_two", (SoaringValue)custom.objectWithKey("description_two"));
											dictionary.Add("event_portrait", (SoaringValue)custom.objectWithKey("event_portrait"));
											list2.Add(new SpongyGamesDialogInputData(dictionary));
											if ((int)dictionary["day"] == (int)dictionary["event_days"])
											{
												CommunityEventManager._pEventStatusDialogData = null;
												int num7 = 20410;
												if (this.TheGame.simulation.FindSimulated(new int?(num7)) == null && !this.TheGame.inventory.HasItem(new int?(num7)))
												{
													string arg = string.Empty;
													Blueprint blueprint;
													if (list3.Count > 0)
													{
														blueprint = EntityManager.GetBlueprint(EntityType.RESIDENT, list3[0], true);
														if (blueprint != null)
														{
															arg = Language.Get((string)blueprint.Invariable["name"]);
														}
													}
													list2.Add(new CharacterDialogInputData(uint.MaxValue, new Dictionary<string, object>
													{
														{
															"character_icon",
															"Portrait_Squilliam.png"
														},
														{
															"text",
															string.Format(Language.Get("!!QUEST_ID2304_TEXT1"), arg)
														}
													}));
													list2.Add(new CharacterDialogInputData(uint.MaxValue, new Dictionary<string, object>
													{
														{
															"character_icon",
															"Squidward_Annoyed.png"
														},
														{
															"text",
															"!!QUEST_ID2304_TEXT2"
														}
													}));
													list2.Add(new CharacterDialogInputData(uint.MaxValue, new Dictionary<string, object>
													{
														{
															"character_icon",
															"Portrait_Squilliam.png"
														},
														{
															"text",
															"!!QUEST_ID2304_TEXT3"
														}
													}));
													blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, 20410, true);
													list2.Add(new FoundItemDialogInputData(Language.Get("!!RECIPE_UNLOCKED_TITLE"), string.Format(Language.Get("!!RECIPE_UNLOCKED_DIALOG"), Language.Get((string)blueprint.Invariable["name"])), (string)blueprint.Invariable["portrait"], "Beat_FoundRecipe"));
													this.TheGame.dialogPackageManager.AddDialogInputBatch(this.TheGame, list2, uint.MaxValue);
													this.TheGame.questManager.AddDialogSequences(this.TheGame, 1U, 2305U, new List<Reward>(), 0U, true);
													Reward reward = new Reward(null, new Dictionary<int, int>
													{
														{
															num7,
															1
														}
													}, null, null, null, null, null, null, false, null);
													this.TheGame.ApplyReward(reward, TFUtils.EpochTime(), false);
													this.TheGame.ModifyGameState(new ReceiveRewardAction(reward, string.Empty));
												}
												else
												{
													this.TheGame.dialogPackageManager.AddDialogInputBatch(this.TheGame, list2, uint.MaxValue);
												}
											}
											else
											{
												CommunityEventManager._pEventStatusDialogData = dictionary;
												this.TheGame.dialogPackageManager.AddDialogInputBatch(this.TheGame, list2, uint.MaxValue);
											}
											if (this.TheState != Session.states["CommunityEvent"])
											{
												this.ChangeState("ShowingDialog", true);
											}
											else
											{
												this.AddAsyncResponse("dialogs_to_show", true);
											}
										}
										goto IL_B6F;
									}
									if (soaringEvent.Actions[l].Type != SoaringEvent.SoaringEventActionType.DisplayBanner)
									{
										goto IL_B6F;
									}
									IL_B99:
									l++;
									continue;
									try
									{
										IL_B6F:
										this.GiveSoaringReward(soaringEvent.Actions[l]);
									}
									catch (Exception ex2)
									{
										TFUtils.LogDump(this, "Reward Exception", ex2, null);
									}
									goto IL_B99;
								}
							}
						}
						this.soaringEvents.clear();
					}
				}
				if (this.game != null && this.game.PendingReload())
				{
					this.ChangeState("Sync", true);
					this.game.SetPendingReload(false);
				}
				else
				{
					this.state.OnUpdate(this);
					if (this.game != null && this.game.simulation != null)
					{
						this.game.sessionActionManager.OnUpdate(this.game);
					}
					this.camera.OnUpdate(realtimeSinceStartup - this.lastUpdateTime, this);
					this.lastUpdateTime = realtimeSinceStartup;
					this.callbackQueue.ProcessQueue();
				}
			}
		}
		catch (Exception ex3)
		{
			TFUtils.LogDump(this, "main_loop_error", ex3, null);
			throw;
		}
	}

	// Token: 0x06000FB0 RID: 4016 RVA: 0x00066890 File Offset: 0x00064A90
	public void GiveSoaringReward(SoaringEvent.SoaringEventAction reward)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
		Dictionary<string, string> dictionary6 = new Dictionary<string, string>();
		dictionary6["hard_currency"] = ResourceManager.HARD_CURRENCY.ToString();
		dictionary6["soft_currency"] = ResourceManager.SOFT_CURRENCY.ToString();
		dictionary6["xp"] = ResourceManager.XP.ToString();
		string text = reward.Key;
		if (dictionary6.ContainsKey(text))
		{
			string key = dictionary6[text];
			dictionary[key] = reward.Quantity;
		}
		else
		{
			text = reward.Value;
		}
		string text2 = string.Empty;
		string text3 = "costume_";
		int num = -1;
		string text4 = string.Empty;
		string text5 = "movie_";
		int num2 = -1;
		if (text.Contains(text3))
		{
			if (!int.TryParse(text.Substring(text3.Length), out num))
			{
				num = -1;
			}
			if (reward.Custom.containsKey("unlock_type"))
			{
				text2 = (SoaringValue)reward.Custom["unlock_type"];
			}
			if (string.IsNullOrEmpty(text2))
			{
				num = -1;
			}
		}
		if (text.Contains(text5))
		{
			if (!int.TryParse(text.Substring(text5.Length), out num2))
			{
				num2 = -1;
			}
			if (reward.Custom.containsKey("unlock_type"))
			{
				text4 = (SoaringValue)reward.Custom["unlock_type"];
			}
			if (string.IsNullOrEmpty(text4))
			{
				num2 = -1;
			}
		}
		this.PopulateRewardDict("building_", dictionary2, text, reward.Quantity);
		this.PopulateRewardDict("movie_", dictionary4, text, reward.Quantity);
		this.PopulateRewardDict("recipe_", dictionary3, text, reward.Quantity);
		if (num != -1 && text2.Equals("add"))
		{
			this.PopulateRewardDict("costume_", dictionary5, text, reward.Quantity);
		}
		if (num2 != -1 && text4.Equals("add"))
		{
			this.PopulateRewardDict("movie_", dictionary4, text, reward.Quantity);
		}
		Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
		dictionary7["resources"] = dictionary;
		dictionary7["recipes"] = dictionary3;
		dictionary7["movies"] = dictionary4;
		dictionary7["buildings"] = dictionary2;
		if (num != -1 && text2.Equals("add"))
		{
			dictionary7["costumes"] = dictionary5;
		}
		if (num2 != -1 && text4.Equals("add"))
		{
			dictionary7["movies"] = dictionary4;
		}
		if (this.TheGame != null)
		{
			RewardDefinition rewardDefinition = RewardDefinition.FromDict(dictionary7);
			Reward reward2 = rewardDefinition.GenerateReward(this.TheGame.simulation, false);
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, int> keyValuePair in reward2.BuildingAmounts)
			{
				int key2 = keyValuePair.Key;
				Blueprint blueprint = EntityManager.GetBlueprint("building", key2, true);
				int? num3 = null;
				if (blueprint != null)
				{
					num3 = blueprint.GetInstanceLimitByLevel(this.TheGame.resourceManager.PlayerLevelAmount);
				}
				if (num3 != null)
				{
					int num4 = 0;
					List<Simulated> list2 = this.TheGame.simulation.FindAllSimulateds(key2, null);
					foreach (Simulated simulated in list2)
					{
						if ((simulated.Entity.AllTypes & EntityType.BUILDING) != EntityType.INVALID)
						{
							num4++;
						}
					}
					foreach (SBInventoryItem sbinventoryItem in this.TheGame.inventory.GetItems())
					{
						if (sbinventoryItem.entity.DefinitionId == key2)
						{
							num4++;
						}
					}
					if (num4 > num3.Value)
					{
						Debug.LogError("Cannot add another instance of this building since instance limit of " + num3.Value + " has been reached!");
						list.Add(keyValuePair.Key);
					}
				}
			}
			foreach (int key3 in list)
			{
				if (reward2.BuildingAmounts.ContainsKey(key3))
				{
					reward2.BuildingAmounts[key3] = 0;
				}
			}
			if (num != -1)
			{
				if (text2.Equals("remove") && this.TheGame.costumeManager.IsCostumeUnlocked(num))
				{
					this.TheGame.costumeManager.RemoveCostume(num);
				}
				if (text2.Equals("unlock_store"))
				{
					this.TheGame.costumeManager.UnLockCostumeInStore(num);
				}
				if (text2.Equals("lock_store"))
				{
					this.TheGame.costumeManager.LockCostumeInStore(num);
				}
			}
			if (num2 != -1)
			{
				this.TheGame.movieManager.UnlockMovie(num2);
			}
			this.TheGame.ApplyReward(reward2, TFUtils.EpochTime(), false);
			this.TheGame.ModifyGameState(new ReceiveRewardAction(reward2, text));
		}
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x00066EA4 File Offset: 0x000650A4
	private void PopulateRewardDict(string prefix, Dictionary<string, object> dict, string rewardName, int quantity)
	{
		if (rewardName.StartsWith(prefix))
		{
			string key = rewardName.Substring(prefix.Length);
			dict[key] = quantity;
		}
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x00066ED8 File Offset: 0x000650D8
	public void SetupPlayer(SoaringContext context)
	{
		if (context == null)
		{
			TFUtils.ErrorLog("TFUtils: SetupPlayer: Error: No Valid Context");
			return;
		}
		SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)context.objectWithKey("user_data");
		this.ThePlayer = Player.LoadFromSoaringID(soaringPlayerData.userID);
		if (this.ThePlayer != null)
		{
			TFUtils.DebugLog(string.Format("The player is logged in with playerId {0}", this.ThePlayer.playerId));
			this.WebFileServer.SetPlayerInfo(this.ThePlayer);
			this.analytics.PlayerId = this.ThePlayer.playerId;
		}
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x00066F64 File Offset: 0x00065164
	public void AddAction(Session.GameloopAction action)
	{
		this.actions.Add(action);
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x00066F74 File Offset: 0x00065174
	public int GetLocalVersion()
	{
		return this.currentVersion;
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x00066F7C File Offset: 0x0006517C
	public SBWebFileServer WebFileServer
	{
		get
		{
			return this.webFileServer;
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x00066F84 File Offset: 0x00065184
	public SBAnalytics Analytics
	{
		get
		{
			return this.analytics;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x00066F8C File Offset: 0x0006518C
	public static DebugManager TheDebugManager
	{
		get
		{
			return Session.debugManager;
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x00066F94 File Offset: 0x00065194
	public SBAuth Auth
	{
		get
		{
			return this.auth;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x00066F9C File Offset: 0x0006519C
	public Game TheGame
	{
		get
		{
			return this.game;
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000FBA RID: 4026 RVA: 0x00066FA4 File Offset: 0x000651A4
	public CallbackQueue CallbackQueue
	{
		get
		{
			return this.callbackQueue;
		}
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x00066FAC File Offset: 0x000651AC
	public void DropGame()
	{
		if (this.game != null)
		{
			this.game = null;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000FBC RID: 4028 RVA: 0x00066FC0 File Offset: 0x000651C0
	public SBCamera TheCamera
	{
		get
		{
			return this.camera;
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000FBD RID: 4029 RVA: 0x00066FC8 File Offset: 0x000651C8
	// (set) Token: 0x06000FBE RID: 4030 RVA: 0x00066FD0 File Offset: 0x000651D0
	public Player ThePlayer
	{
		get
		{
			return this.player;
		}
		set
		{
			this.player = value;
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000FBF RID: 4031 RVA: 0x00066FDC File Offset: 0x000651DC
	public SoundEffectManager TheSoundEffectManager
	{
		get
		{
			return this.soundEffectManager;
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00066FE4 File Offset: 0x000651E4
	public Session.State TheState
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x06000FC1 RID: 4033 RVA: 0x00066FEC File Offset: 0x000651EC
	public bool PlayerIsLoggedIn()
	{
		return this.player != null;
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x00066FFC File Offset: 0x000651FC
	public SBGUIScreen SimulationSBGUIScreen
	{
		get
		{
			return this.simulationScratchScreen;
		}
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x00067004 File Offset: 0x00065204
	public void onExternalMessage(string msg)
	{
		TFUtils.DebugLog("decoding message: " + msg);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(msg);
		string text = dictionary["requestId"] as string;
		if (this.externalRequests.ContainsKey(text))
		{
			TFServer.JsonResponseHandler jsonResponseHandler = this.externalRequests[text];
			if (dictionary["data"] is Dictionary<string, object>)
			{
				jsonResponseHandler(dictionary["data"] as Dictionary<string, object>, null);
			}
			else
			{
				TFUtils.ErrorLog("Callback result is not a Dictionary<string, object>");
			}
		}
		else
		{
			TFUtils.DebugLog("No handler found for id: " + text);
		}
	}

	// Token: 0x06000FC4 RID: 4036 RVA: 0x000670AC File Offset: 0x000652AC
	public void RegisterExternalCallback(string requestId, TFServer.JsonResponseHandler callback)
	{
		TFUtils.DebugLog("Registering external callback for " + requestId);
		if (this.externalRequests.ContainsKey(requestId))
		{
			TFUtils.ErrorLog("Got duplicate registration for " + requestId + "; Clobbering existing callback");
		}
		this.externalRequests[requestId] = callback;
	}

	// Token: 0x06000FC5 RID: 4037 RVA: 0x000670FC File Offset: 0x000652FC
	public void unregisterExternalCallback(string requestId, TFServer.JsonResponseHandler callback)
	{
		TFUtils.DebugLog("Unregistering external callback for " + requestId);
		this.externalRequests.Remove(requestId);
	}

	// Token: 0x06000FC6 RID: 4038 RVA: 0x0006711C File Offset: 0x0006531C
	public AndroidJavaObject getAndroidActivity()
	{
		if (this.androidActivity == null)
		{
			int num = AndroidJNI.AttachCurrentThread();
			TFUtils.DebugLog("attach result: " + num);
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			this.androidActivity = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		return this.androidActivity;
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x00067174 File Offset: 0x00065374
	protected void CheckInventorySoftLock()
	{
		if (!this.game.featureManager.CheckFeature("inventory_soft"))
		{
			this.game.featureManager.UnlockFeature("inventory_soft");
			this.game.featureManager.ActivateFeatureActions(this.game, "inventory_soft");
			this.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string>
			{
				"inventory_soft"
			}));
			return;
		}
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x000671F8 File Offset: 0x000653F8
	public void AddAsyncResponse(string key, object val)
	{
		this.AddAsyncResponse(key, val, true);
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x00067204 File Offset: 0x00065404
	public void AddAsyncResponse(string key, object val, bool warnIfDuplicate)
	{
		Dictionary<string, object> obj = this.asyncRequests;
		lock (obj)
		{
			if (warnIfDuplicate && this.asyncRequests.ContainsKey(key))
			{
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"Warning: got second async response for ",
					key,
					"; Existing value was: ",
					this.asyncRequests[key]
				}));
			}
			this.asyncRequests[key] = val;
		}
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x000672A0 File Offset: 0x000654A0
	public object CheckAsyncRequest(string key)
	{
		object result = null;
		Dictionary<string, object> obj = this.asyncRequests;
		lock (obj)
		{
			if (this.asyncRequests.ContainsKey(key))
			{
				result = this.asyncRequests[key];
				this.asyncRequests.Remove(key);
			}
		}
		return result;
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x00067310 File Offset: 0x00065510
	public TFServer.JsonResponseHandler AsyncResponder(string key)
	{
		return delegate(Dictionary<string, object> response, object userData)
		{
			this.AddAsyncResponse(key, response);
		};
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x00067340 File Offset: 0x00065540
	public void AddAsyncFileResponse(string key, TFWebClient val)
	{
		Dictionary<string, TFWebClient> obj = this.asyncFileRequests;
		lock (obj)
		{
			this.asyncFileRequests[key] = val;
		}
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x00067390 File Offset: 0x00065590
	public TFWebClient CheckAsyncFileRequest(string key)
	{
		TFWebClient result = null;
		Dictionary<string, TFWebClient> obj = this.asyncFileRequests;
		lock (obj)
		{
			if (this.asyncFileRequests.ContainsKey(key))
			{
				result = this.asyncFileRequests[key];
				this.asyncFileRequests.Remove(key);
			}
		}
		return result;
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x00067400 File Offset: 0x00065600
	public TFWebClient.GetCallbackHandler AsyncFileResponder(string key)
	{
		return delegate(TFWebClient client)
		{
			this.AddAsyncFileResponse(key, client);
		};
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x00067430 File Offset: 0x00065630
	public void ClearAsyncRequests()
	{
		this.asyncRequests = new Dictionary<string, object>();
		this.asyncFileRequests = new Dictionary<string, TFWebClient>();
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x00067448 File Offset: 0x00065648
	public void PlayBubbleScreenSwipeEffect()
	{
		this.TheSoundEffectManager.PlaySound("BubbleWipe");
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Screen_Wipe", 0, 0, 0f, this.bubbleSwipeParticleSystemRequestDelegate);
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x00067490 File Offset: 0x00065690
	public void PlayConfettiScreenSwipeEffect()
	{
		this.TheSoundEffectManager.PlaySound("ConfettiPop");
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Confetti_Squares", 0, 0, 0f, this.confettiSwipeParticleSystemRequestDelegate);
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Confetti_Squiggles", 0, 0, 0f, this.confettiSwipeParticleSystemRequestDelegate);
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Confetti_Balloons_01", 0, 0, 0f, this.balloonSwipeParticleSystemRequestDelegate);
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Confetti_Balloons_02", 0, 0, 0f, this.balloonSwipeParticleSystemRequestDelegate);
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x00067550 File Offset: 0x00065750
	public void PlaySeaflowerAndBubbleScreenSwipeEffect()
	{
		this.TheSoundEffectManager.PlaySound("SeaflowerWipe");
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Seaflowers_Quest_Complete", 0, 0, 0f, this.seaFlowerSwipeParticleSystemRequestDelegate);
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Quest_Complete", 0, 0, 0f, this.seaFlowerSwipeParticleSystemRequestDelegate);
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x000675C0 File Offset: 0x000657C0
	public void PlayTapParticleEffect(Vector3 position)
	{
		this.tapFXParticleSystemRequestDelegate.Position = position;
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Bubble_Click", 0, 0, 0f, this.tapFXParticleSystemRequestDelegate);
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x00067604 File Offset: 0x00065804
	public void PlayFogParticleEffects()
	{
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog1_Drift", 0, 0, 40f, this.fogEffectRequestDelegate);
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog2_Drift", 0, 0, 40f, this.fogEffectRequestDelegate);
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog3_Drift", 0, 0, 40f, this.fogEffectRequestDelegate);
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog4_Drift", 0, 0, 40f, this.fogEffectRequestDelegate);
		this.game.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Fog5_Drift", 0, 0, 40f, this.fogEffectRequestDelegate);
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x000676DC File Offset: 0x000658DC
	private void InitScreenSwipeEffects()
	{
		this.bubbleSwipeParticleSystemRequestDelegate = new Session.BubbleSwipeParticleSystemRequestDelegate(this);
		this.confettiSwipeParticleSystemRequestDelegate = new Session.ConfettiSwipeParticleSystemRequestDelegate(this);
		this.balloonSwipeParticleSystemRequestDelegate = new Session.BalloonSwipeParticleSystemRequestDelegate(this);
		this.seaFlowerSwipeParticleSystemRequestDelegate = new Session.SeaflowerSwipeParticleSystemRequestDelegate(this);
		this.tapFXParticleSystemRequestDelegate = new Session.TapFXParticleSystemRequestDelegate(this);
		this.fogEffectRequestDelegate = new Session.FogEffectRequestDelegate(this);
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x00067734 File Offset: 0x00065934
	public void ErrorMessageHandler(Session session, string title, string message, string okButtonLabel, Action okAction, float messageScale = 1f)
	{
		session.AddAsyncResponse("error_message_title", title);
		session.AddAsyncResponse("error_message", message);
		session.AddAsyncResponse("error_message_ok_label", okButtonLabel);
		session.AddAsyncResponse("error_message_ok_action", okAction);
		session.AddAsyncResponse("error_message_scale", messageScale);
		session.ChangeState("ErrorDialog", true);
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x00067794 File Offset: 0x00065994
	public void GetJellyHandler(Session session, string title, string message, string question, string okButtonLabel, string cancelButtonLabel, Action okAction, Action cancelAction)
	{
		session.AddAsyncResponse("jelly_message_title", title);
		session.AddAsyncResponse("jelly_message", message);
		session.AddAsyncResponse("jelly_question", question);
		session.AddAsyncResponse("jelly_message_ok_label", okButtonLabel);
		session.AddAsyncResponse("jelly_message_cancel_label", cancelButtonLabel);
		session.AddAsyncResponse("jelly_message_ok_action", okAction);
		session.AddAsyncResponse("jelly_message_cancel_action", cancelAction);
		session.ChangeState("GetJelly", true);
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x00067808 File Offset: 0x00065A08
	public void InsufficientResourcesHandler(Session session, string itemName, int itemDID, Action okAction, Action cancelAction, Cost insufficientCost)
	{
		session.AddAsyncResponse("insufficient_item_did", itemDID);
		session.AddAsyncResponse("insufficient_itemname", itemName);
		session.AddAsyncResponse("insufficient_resources", insufficientCost);
		session.AddAsyncResponse("insufficient_cancel", cancelAction);
		session.AddAsyncResponse("insufficient_accept", okAction);
		session.ChangeState("InsufficientDialog", true);
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x00067868 File Offset: 0x00065A68
	public static Simulated FindBestSimulatedUnderPoint(Session.Prioritizer prioritizer, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast)
	{
		return Session.FindBestSimulatedUnderPoint(prioritizer, null, simulation, camera, screenPos, out rayCast);
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x00067878 File Offset: 0x00065A78
	public static Simulated FindBestSimulatedUnderPoint(Session.Prioritizer prioritizer, Predicate<Simulated> filterOutMatching, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast)
	{
		List<Simulated> list = Session.FindSimulatedsUnderPoint(filterOutMatching, simulation, camera, screenPos, out rayCast);
		list.ForEach(new Action<Simulated>(prioritizer.SelectBest));
		return prioritizer.Best;
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x000678AC File Offset: 0x00065AAC
	public static List<Simulated> FindSimulatedsUnderPoint(Predicate<Simulated> filterOutMatching, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast)
	{
		rayCast = camera.ScreenPointToRay(screenPos);
		List<Simulated> list = new List<Simulated>();
		simulation.Scene.Find(rayCast, ref list);
		simulation.WhitelistSimulateds(ref list);
		if (filterOutMatching != null)
		{
			list.RemoveAll(filterOutMatching);
		}
		return list;
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x000678F8 File Offset: 0x00065AF8
	public static Simulated FindAlreadySelected(Predicate<Simulated> filterOutMatching, Simulation simulation, SBCamera camera, Vector2 screenPos, out Ray rayCast, Simulated selected)
	{
		if (selected == null)
		{
			rayCast = camera.ScreenPointToRay(screenPos);
			return null;
		}
		List<Simulated> list = Session.FindSimulatedsUnderPoint(filterOutMatching, simulation, camera, screenPos, out rayCast);
		foreach (Simulated simulated in list)
		{
			if (simulated == selected)
			{
				return simulated;
			}
		}
		return null;
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x00067988 File Offset: 0x00065B88
	private static void ChangeToResolveSessionStateOnStartup(Session session)
	{
		string nextSession = "resolve_user";
		if (session.ThePlayer == null)
		{
			session.ChangeState(nextSession, true);
			return;
		}
		if (!Game.GameExists(session.ThePlayer))
		{
			session.PlayMovie("Video/1_intro.m4v", nextSession);
		}
		else
		{
			session.ChangeState(nextSession, true);
		}
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x000679D8 File Offset: 0x00065BD8
	private static void RegisterForLocalNotifications()
	{
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x000679DC File Offset: 0x00065BDC
	protected void PlayMovie(string movie, string nextSession)
	{
		Session.Movie movie2 = (Session.Movie)Session.states["Movie"];
		movie2.TheMovie = movie;
		movie2.NextSessionState = nextSession;
		this.ChangeState("Movie", true);
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x00067A18 File Offset: 0x00065C18
	public void PlayMovieFromInventory(string movie)
	{
		this.PlayMovie(movie, "Inventory");
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x00067A28 File Offset: 0x00065C28
	public void PlayMovieFromPlaying(string movie)
	{
		this.PlayMovie(movie, "Playing");
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x00067A38 File Offset: 0x00065C38
	public void PlayMovieFromShowingDialog(string movie)
	{
		this.PlayMovie(movie, "ShowingDialog");
	}

	// Token: 0x06000FE3 RID: 4067 RVA: 0x00067A48 File Offset: 0x00065C48
	public static void TryGrabSimulated(Session session, SBGUIEvent evt)
	{
		List<Simulated> candidateSimulateds = new List<Simulated>();
		session.game.simulation.Scene.Find(session.camera.ScreenPointToRay(evt.position), ref candidateSimulateds);
		Session.TryGrabSimulated(session, candidateSimulateds, evt);
	}

	// Token: 0x06000FE4 RID: 4068 RVA: 0x00067A8C File Offset: 0x00065C8C
	public static bool TryGrabSimulated(Session session, List<Simulated> candidateSimulateds, SBGUIEvent evt)
	{
		return candidateSimulateds.Count != 0 && Session.TryGrabSimulated(session, candidateSimulateds[0], evt);
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x00067AAC File Offset: 0x00065CAC
	public static bool TryGrabSimulated(Session session, Simulated candidateSimulated, SBGUIEvent evt)
	{
		if (candidateSimulated.InteractionState.IsGrabbable && !session.game.simulation.Whitelisted)
		{
			session.AddAsyncResponse("CurrentGuiEventInfo", evt);
			session.game.selected = candidateSimulated;
			Session.PushForPlacementHelper.PushPlacementConfirmation(session, candidateSimulated);
			return true;
		}
		return false;
	}

	// Token: 0x04000A7C RID: 2684
	private const string CHECK_PATCHING = "CheckPatching";

	// Token: 0x04000A7D RID: 2685
	private const string GAME_STARTING = "GameStarting";

	// Token: 0x04000A7E RID: 2686
	private const string GAME_STOPPING = "GameStopping";

	// Token: 0x04000A7F RID: 2687
	private const string AUTHORIZING = "Authorizing";

	// Token: 0x04000A80 RID: 2688
	public const string PLAYING = "Playing";

	// Token: 0x04000A81 RID: 2689
	public const string SELECTED_PLAYING = "SelectedPlaying";

	// Token: 0x04000A82 RID: 2690
	private const string EDITING = "Editing";

	// Token: 0x04000A83 RID: 2691
	private const string MOVE_IN_EDIT = "MoveBuildingInEdit";

	// Token: 0x04000A84 RID: 2692
	private const string MOVE_IN_PLACEMENT = "MoveBuildingInPlacement";

	// Token: 0x04000A85 RID: 2693
	private const string MOVE_PANNING_IN_EDIT = "MoveBuildingPanningInEdit";

	// Token: 0x04000A86 RID: 2694
	private const string MOVE_PANNING_IN_PLACEMENT = "MoveBuildingPanningInPlacement";

	// Token: 0x04000A87 RID: 2695
	private const string PLACING = "Placing";

	// Token: 0x04000A88 RID: 2696
	private const string PAVING = "Paving";

	// Token: 0x04000A89 RID: 2697
	private const string DRAG_FEEDING = "DragFeeding";

	// Token: 0x04000A8A RID: 2698
	private const string SHOPPING = "Shopping";

	// Token: 0x04000A8B RID: 2699
	private const string INVENTORY = "Inventory";

	// Token: 0x04000A8C RID: 2700
	private const string COMMUNITY_EVENT = "CommunityEvent";

	// Token: 0x04000A8D RID: 2701
	public const string BROWSING_RECIPES = "BrowsingRecipes";

	// Token: 0x04000A8E RID: 2702
	private const string SYNC = "Sync";

	// Token: 0x04000A8F RID: 2703
	private const string STOPPING = "Stopping";

	// Token: 0x04000A90 RID: 2704
	private const string IN_APP_PURCHASING = "InAppPurchasing";

	// Token: 0x04000A91 RID: 2705
	private const string SHOWING_DIALOG = "ShowingDialog";

	// Token: 0x04000A92 RID: 2706
	private const string HARD_SPEND_CONFIRM = "HardSpendConfirm";

	// Token: 0x04000A93 RID: 2707
	private const string HARD_SPEND_PASSTHROUGH = "HardSpendPassthrough";

	// Token: 0x04000A94 RID: 2708
	private const string INSUFFICIENT_DIALOG = "InsufficientDialog";

	// Token: 0x04000A95 RID: 2709
	private const string EXPANSION = "Expansion";

	// Token: 0x04000A96 RID: 2710
	private const string EXPANDING = "Expanding";

	// Token: 0x04000A97 RID: 2711
	private const string CLEARING = "Clearing";

	// Token: 0x04000A98 RID: 2712
	private const string OPTIONS = "Options";

	// Token: 0x04000A99 RID: 2713
	private const string MOVIE = "Movie";

	// Token: 0x04000A9A RID: 2714
	private const string DEBUG = "Debug";

	// Token: 0x04000A9B RID: 2715
	private const string ERROR_DIALOG = "ErrorDialog";

	// Token: 0x04000A9C RID: 2716
	private const string GET_JELLY = "GetJelly";

	// Token: 0x04000A9D RID: 2717
	private const string CREDITS = "Credits";

	// Token: 0x04000A9E RID: 2718
	private const string MOVIE_START_TIME = "MovieStartTime";

	// Token: 0x04000A9F RID: 2719
	private const string SELL_BUILDING_CONFIRMATION = "SellBuildingConfirmation";

	// Token: 0x04000AA0 RID: 2720
	private const string STASH_BUILDING_CONFIRMATION = "StashBuildingConfirmation";

	// Token: 0x04000AA1 RID: 2721
	public const string VENDING = "vending";

	// Token: 0x04000AA2 RID: 2722
	public const string UNIT_IDLE = "UnitIdle";

	// Token: 0x04000AA3 RID: 2723
	public const string UNIT_BUSY = "UnitBusy";

	// Token: 0x04000AA4 RID: 2724
	public const string AGE_GATE = "AgeGate";

	// Token: 0x04000AA5 RID: 2725
	protected const string ERROR_MESSAGE_TITLE = "error_message_title";

	// Token: 0x04000AA6 RID: 2726
	protected const string ERROR_MESSAGE = "error_message";

	// Token: 0x04000AA7 RID: 2727
	protected const string ERROR_MESSAGE_OK_LABEL = "error_message_ok_label";

	// Token: 0x04000AA8 RID: 2728
	protected const string ERROR_MESSAGE_OK_ACTION = "error_message_ok_action";

	// Token: 0x04000AA9 RID: 2729
	protected const string ERROR_MESSAGE_SCALE = "error_message_scale";

	// Token: 0x04000AAA RID: 2730
	protected const string JELLY_MESSAGE_TITLE = "jelly_message_title";

	// Token: 0x04000AAB RID: 2731
	protected const string JELLY_MESSAGE = "jelly_message";

	// Token: 0x04000AAC RID: 2732
	protected const string JELLY_QUESTION = "jelly_question";

	// Token: 0x04000AAD RID: 2733
	protected const string JELLY_MESSAGE_OK_LABEL = "jelly_message_ok_label";

	// Token: 0x04000AAE RID: 2734
	protected const string JELLY_MESSAGE_CANCEL_LABEL = "jelly_message_cancel_label";

	// Token: 0x04000AAF RID: 2735
	protected const string JELLY_MESSAGE_OK_ACTION = "jelly_message_ok_action";

	// Token: 0x04000AB0 RID: 2736
	protected const string JELLY_MESSAGE_CANCEL_ACTION = "jelly_message_cancel_action";

	// Token: 0x04000AB1 RID: 2737
	protected const string INSUFFICIENT_ITEM_DID = "insufficient_item_did";

	// Token: 0x04000AB2 RID: 2738
	protected const string INSUFFICIENT_ITEMNAME = "insufficient_itemname";

	// Token: 0x04000AB3 RID: 2739
	protected const string INSUFFICIENT_RESOURCES = "insufficient_resources";

	// Token: 0x04000AB4 RID: 2740
	protected const string INSUFFICIENT_CANCEL = "insufficient_cancel";

	// Token: 0x04000AB5 RID: 2741
	protected const string INSUFFICIENT_ACCEPT = "insufficient_accept";

	// Token: 0x04000AB6 RID: 2742
	private const string USER_LOGIN = "userLogin";

	// Token: 0x04000AB7 RID: 2743
	private const string EXPANDING_UI_HANDLER = "expanding_ui";

	// Token: 0x04000AB8 RID: 2744
	protected const string EXPANSION_SCREEN = "expansion";

	// Token: 0x04000AB9 RID: 2745
	private const string EXPANSION_UI_HANDLER = "expansion_ui";

	// Token: 0x04000ABA RID: 2746
	private const string STARTING_PROGRESS = "starting_progress";

	// Token: 0x04000ABB RID: 2747
	private const int TERRAIN_DEPTH = 5;

	// Token: 0x04000ABC RID: 2748
	public const string TARGET_STORE_TAB = "target_store_tab";

	// Token: 0x04000ABD RID: 2749
	public const string TARGET_STORE_DID = "target_store_did";

	// Token: 0x04000ABE RID: 2750
	public const string CURRENT_UI_EVENT = "CurrentGuiEventInfo";

	// Token: 0x04000ABF RID: 2751
	protected const string IN_STATE_MOVE_IN_EDIT = "in_state_move_in_edit";

	// Token: 0x04000AC0 RID: 2752
	private const string DIALOGS_TO_SHOW = "dialogs_to_show";

	// Token: 0x04000AC1 RID: 2753
	private const string PLAYING_UI_HANDLER = "playing_ui";

	// Token: 0x04000AC2 RID: 2754
	public const string STANDARD_SCREEN = "standard_screen";

	// Token: 0x04000AC3 RID: 2755
	public const string LEVELUP_SCREEN = "levelup_screen";

	// Token: 0x04000AC4 RID: 2756
	public const string CLEAR_PURCHASE_ON_MOVEMENT = "clear_purchase_on_movement";

	// Token: 0x04000AC5 RID: 2757
	private const string RESOLVE_USER = "resolve_user";

	// Token: 0x04000AC6 RID: 2758
	public const string TRANSACTION_OFFER = "transaction_offer";

	// Token: 0x04000AC7 RID: 2759
	public const string STORE_OPEN_TYPE = "store_open_type";

	// Token: 0x04000AC8 RID: 2760
	public const bool DEBUG_LOG = true;

	// Token: 0x04000AC9 RID: 2761
	private const string VISIT_FRIEND_STARTING = "visit_friend";

	// Token: 0x04000ACA RID: 2762
	protected const string TO_SELL = "to_sell";

	// Token: 0x04000ACB RID: 2763
	protected const string SELL_ERROR = "sell_error";

	// Token: 0x04000ACC RID: 2764
	protected const string TO_STASH = "to_stash";

	// Token: 0x04000ACD RID: 2765
	protected const string STASH_ERROR = "stash_error";

	// Token: 0x04000ACE RID: 2766
	private static Dictionary<string, Session.State> states;

	// Token: 0x04000ACF RID: 2767
	private SBGamePersister gameSaver;

	// Token: 0x04000AD0 RID: 2768
	private CallbackQueue callbackQueue;

	// Token: 0x04000AD1 RID: 2769
	public PlayHavenController PlayHavenController;

	// Token: 0x04000AD2 RID: 2770
	public SBAnalytics analytics;

	// Token: 0x04000AD3 RID: 2771
	public SBContentPatcher contentPatcher;

	// Token: 0x04000AD4 RID: 2772
	public bool notifyOnDisconnect;

	// Token: 0x04000AD5 RID: 2773
	public bool gameInitialized;

	// Token: 0x04000AD6 RID: 2774
	public bool reinitializeSession;

	// Token: 0x04000AD7 RID: 2775
	public bool resyncConnection;

	// Token: 0x04000AD8 RID: 2776
	public bool gameIsReloading;

	// Token: 0x04000AD9 RID: 2777
	public static bool PatchyTownGame;

	// Token: 0x04000ADA RID: 2778
	public bool WasInFriendsGame;

	// Token: 0x04000ADB RID: 2779
	public bool musicStateBeforePT;

	// Token: 0x04000ADC RID: 2780
	public bool sfxStateBeforePT;

	// Token: 0x04000ADD RID: 2781
	public bool haveReloaded;

	// Token: 0x04000ADE RID: 2782
	public PushNotificationManager pushNotificationManager;

	// Token: 0x04000ADF RID: 2783
	public GameObject statisticsTracker;

	// Token: 0x04000AE0 RID: 2784
	public SBStatisticsTracker tracker;

	// Token: 0x04000AE1 RID: 2785
	private static ulong logDumpShake;

	// Token: 0x04000AE2 RID: 2786
	private bool lastOnlineState;

	// Token: 0x04000AE3 RID: 2787
	private bool isShowingOfflineDialog;

	// Token: 0x04000AE4 RID: 2788
	public bool canChangeState;

	// Token: 0x04000AE5 RID: 2789
	private bool checkForPatching;

	// Token: 0x04000AE6 RID: 2790
	private bool justCheckForUpdates;

	// Token: 0x04000AE7 RID: 2791
	public SoaringArray soaringEvents;

	// Token: 0x04000AE8 RID: 2792
	public DateTime lastResetTime;

	// Token: 0x04000AE9 RID: 2793
	private ulong? m_ulPauseTimestamp;

	// Token: 0x04000AEA RID: 2794
	private AndroidJavaObject androidActivity;

	// Token: 0x04000AEB RID: 2795
	public Session.FramerateWatcher framerateWatcher;

	// Token: 0x04000AEC RID: 2796
	private Session.State state;

	// Token: 0x04000AED RID: 2797
	private bool saveGame;

	// Token: 0x04000AEE RID: 2798
	private Player player;

	// Token: 0x04000AEF RID: 2799
	private Game game;

	// Token: 0x04000AF0 RID: 2800
	private SBCamera camera;

	// Token: 0x04000AF1 RID: 2801
	private SBWebFileServer webFileServer;

	// Token: 0x04000AF2 RID: 2802
	private SBAuth auth;

	// Token: 0x04000AF3 RID: 2803
	private static DebugManager debugManager;

	// Token: 0x04000AF4 RID: 2804
	private float lastUpdateTime;

	// Token: 0x04000AF5 RID: 2805
	private int currentVersion;

	// Token: 0x04000AF6 RID: 2806
	private List<Session.GameloopAction> actions;

	// Token: 0x04000AF7 RID: 2807
	private MusicManager musicManager;

	// Token: 0x04000AF8 RID: 2808
	private SoundEffectManager soundEffectManager;

	// Token: 0x04000AF9 RID: 2809
	private SBUIBuilder.ScreenContext simulationContext;

	// Token: 0x04000AFA RID: 2810
	private SBGUIScreen simulationScratchScreen;

	// Token: 0x04000AFB RID: 2811
	private SBUIBuilder.ScreenContext currentGuiContext;

	// Token: 0x04000AFC RID: 2812
	private List<Session.StateChangeRequest> queuedStateChanges;

	// Token: 0x04000AFD RID: 2813
	private Dictionary<string, TFServer.JsonResponseHandler> externalRequests;

	// Token: 0x04000AFE RID: 2814
	private Session.SessionProperties properties;

	// Token: 0x04000AFF RID: 2815
	private Dictionary<string, object> asyncRequests = new Dictionary<string, object>();

	// Token: 0x04000B00 RID: 2816
	private Dictionary<string, TFWebClient> asyncFileRequests = new Dictionary<string, TFWebClient>();

	// Token: 0x04000B01 RID: 2817
	private Session.BubbleSwipeParticleSystemRequestDelegate bubbleSwipeParticleSystemRequestDelegate;

	// Token: 0x04000B02 RID: 2818
	private Session.ConfettiSwipeParticleSystemRequestDelegate confettiSwipeParticleSystemRequestDelegate;

	// Token: 0x04000B03 RID: 2819
	private Session.BalloonSwipeParticleSystemRequestDelegate balloonSwipeParticleSystemRequestDelegate;

	// Token: 0x04000B04 RID: 2820
	private Session.SeaflowerSwipeParticleSystemRequestDelegate seaFlowerSwipeParticleSystemRequestDelegate;

	// Token: 0x04000B05 RID: 2821
	private Session.FogEffectRequestDelegate fogEffectRequestDelegate;

	// Token: 0x04000B06 RID: 2822
	private Session.TapFXParticleSystemRequestDelegate tapFXParticleSystemRequestDelegate;

	// Token: 0x04000B07 RID: 2823
	private static readonly object expandLock = new object();

	// Token: 0x04000B08 RID: 2824
	public static bool didRegisterNotifications;

	// Token: 0x04000B09 RID: 2825
	private static bool loggingTimedDependents;

	// Token: 0x04000B0A RID: 2826
	private static bool draggingCamera;

	// Token: 0x04000B0B RID: 2827
	public bool marketpalceActive;

	// Token: 0x020001CD RID: 461
	public class SoaringSessionRestartDelegate : SoaringDelegate
	{
		// Token: 0x06000FEA RID: 4074 RVA: 0x00067B40 File Offset: 0x00065D40
		public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
		{
			if (!success || error != null || sessions == null)
			{
				return;
			}
		}
	}

	// Token: 0x020001CE RID: 462
	public class FramerateWatcher
	{
		// Token: 0x06000FEC RID: 4076 RVA: 0x00067B6C File Offset: 0x00065D6C
		public void OnUpdate()
		{
			this.accum += Time.timeScale / Time.deltaTime;
			this.frames++;
			this.waitTime += Time.deltaTime;
			if (this.waitTime > this.frequency)
			{
				this.waitTime = 0f;
				this.prevWindowsFPS = this.accum / (float)this.frames;
				this.accum = 0f;
				this.frames = 0;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x00067BF4 File Offset: 0x00065DF4
		public float Framerate
		{
			get
			{
				return this.prevWindowsFPS;
			}
		}

		// Token: 0x04000B0D RID: 2829
		public float frequency = 0.5f;

		// Token: 0x04000B0E RID: 2830
		private float accum;

		// Token: 0x04000B0F RID: 2831
		private int frames;

		// Token: 0x04000B10 RID: 2832
		private float waitTime;

		// Token: 0x04000B11 RID: 2833
		private float prevWindowsFPS;
	}

	// Token: 0x020001CF RID: 463
	private class SessionProperties
	{
		// Token: 0x04000B12 RID: 2834
		public SBGUIStandardScreen playingHud;

		// Token: 0x04000B13 RID: 2835
		public SBGUIStandardScreen ageGateHud;

		// Token: 0x04000B14 RID: 2836
		public bool transitionSilently;

		// Token: 0x04000B15 RID: 2837
		public SBGUIStandardScreen recipesHud;

		// Token: 0x04000B16 RID: 2838
		public SBGUICraftingScreen recipesWindow;

		// Token: 0x04000B17 RID: 2839
		public Dictionary<CraftingCookbook, CraftingRecipe> lastSelectedRecipe = new Dictionary<CraftingCookbook, CraftingRecipe>();

		// Token: 0x04000B18 RID: 2840
		public Simulated m_pTaskSimulated;

		// Token: 0x04000B19 RID: 2841
		public bool m_bAutoPanToSimulatedOnLeave;

		// Token: 0x04000B1A RID: 2842
		public SBGUIStandardScreen communityEventHud;

		// Token: 0x04000B1B RID: 2843
		public SBGUICommunityEventScreen communityEventScreen;

		// Token: 0x04000B1C RID: 2844
		public SBGUIStandardScreen dragFeedHud;

		// Token: 0x04000B1D RID: 2845
		public Session.SessionProperties.DraggedGood draggedGood;

		// Token: 0x04000B1E RID: 2846
		public Simulated candidateSimulated;

		// Token: 0x04000B1F RID: 2847
		public YGEvent carriedUiEvent;

		// Token: 0x04000B20 RID: 2848
		public int playDelayCounter;

		// Token: 0x04000B21 RID: 2849
		public SBGUIMicroConfirmDialog microConfirmDialog;

		// Token: 0x04000B22 RID: 2850
		public Action denialActions;

		// Token: 0x04000B23 RID: 2851
		public Action cleanUp;

		// Token: 0x04000B24 RID: 2852
		public Session.HardSpendActions hardSpendActions;

		// Token: 0x04000B25 RID: 2853
		public Simulated overrideSimulatedToRush;

		// Token: 0x04000B26 RID: 2854
		public string iapBundleName;

		// Token: 0x04000B27 RID: 2855
		public SBGUIInsufficientResourcesDialog insufficientDialog;

		// Token: 0x04000B28 RID: 2856
		public SBGUIStandardScreen inventoryHud;

		// Token: 0x04000B29 RID: 2857
		public SBGUIScreen editingHud;

		// Token: 0x04000B2A RID: 2858
		public bool waitToDecidePlacement;

		// Token: 0x04000B2B RID: 2859
		public Vector2 preMovePosition;

		// Token: 0x04000B2C RID: 2860
		public bool preMoveFlip;

		// Token: 0x04000B2D RID: 2861
		public bool preMovePositionSet;

		// Token: 0x04000B2E RID: 2862
		public bool isInteractionStripActive;

		// Token: 0x04000B2F RID: 2863
		public bool isDraggingBuilding;

		// Token: 0x04000B30 RID: 2864
		public bool isDraggingBuildingAndScreen;

		// Token: 0x04000B31 RID: 2865
		public bool firstEntered;

		// Token: 0x04000B32 RID: 2866
		public bool startedTouchOnEmptySpace;

		// Token: 0x04000B33 RID: 2867
		public SBGUIStandardScreen optionsHud;

		// Token: 0x04000B34 RID: 2868
		public bool cameFromMarketplace;

		// Token: 0x04000B35 RID: 2869
		public Simulated touchingSim;

		// Token: 0x04000B36 RID: 2870
		public Simulated queuedClickedSim;

		// Token: 0x04000B37 RID: 2871
		public Vector2 moveDragStart;

		// Token: 0x04000B38 RID: 2872
		public IDisplayController tappedDisplayController;

		// Token: 0x04000B39 RID: 2873
		public SBGUIStandardScreen shoppingHud;

		// Token: 0x04000B3A RID: 2874
		public string marketplaceSessionActionID;

		// Token: 0x04000B3B RID: 2875
		public string m_sLeaveType;

		// Token: 0x04000B3C RID: 2876
		public bool reducedBuffer;

		// Token: 0x04000B3D RID: 2877
		public int storeVisitSinceLastPurchase;

		// Token: 0x04000B3E RID: 2878
		public SBGUIStandardScreen dialogHud;

		// Token: 0x04000B3F RID: 2879
		public SBGUIStandardScreen unitBusyHud;

		// Token: 0x04000B40 RID: 2880
		public SBGUICharacterBusyScreen unitBusyWindow;

		// Token: 0x04000B41 RID: 2881
		public Task unitBusyTask;

		// Token: 0x04000B42 RID: 2882
		public SBGUIStandardScreen unitIdleHud;

		// Token: 0x04000B43 RID: 2883
		public SBGUICharacterIdleScreen unitIdleWindow;

		// Token: 0x04000B44 RID: 2884
		public SBGUIVendorScreen vendorScreen;

		// Token: 0x04000B45 RID: 2885
		public Reward reward;

		// Token: 0x020001D0 RID: 464
		public class DraggedGood
		{
			// Token: 0x06000FEF RID: 4079 RVA: 0x00067C10 File Offset: 0x00065E10
			public DraggedGood(int productId, Resource resource)
			{
				this.productId = productId;
				this.resource = resource;
			}

			// Token: 0x04000B46 RID: 2886
			public int productId;

			// Token: 0x04000B47 RID: 2887
			public Resource resource;
		}
	}

	// Token: 0x020001D1 RID: 465
	private struct StateChangeRequest
	{
		// Token: 0x04000B48 RID: 2888
		public string state;

		// Token: 0x04000B49 RID: 2889
		public bool changeContext;
	}

	// Token: 0x020001D2 RID: 466
	private class BubbleSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x06000FF0 RID: 4080 RVA: 0x00067C28 File Offset: 0x00065E28
		public BubbleSwipeParticleSystemRequestDelegate(Session s)
		{
			this.session = s;
			this.viewportPosition = new Vector3(0.5f, 0f, this.session.camera.UnityCamera.nearClipPlane);
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x00067C6C File Offset: 0x00065E6C
		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x00067C70 File Offset: 0x00065E70
		public Vector3 Position
		{
			get
			{
				return this.session.camera.UnityCamera.ViewportToWorldPoint(this.viewportPosition);
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x00067C90 File Offset: 0x00065E90
		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000B4A RID: 2890
		protected Session session;

		// Token: 0x04000B4B RID: 2891
		protected Vector3 viewportPosition;
	}

	// Token: 0x020001D3 RID: 467
	private class ConfettiSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x06000FF4 RID: 4084 RVA: 0x00067C94 File Offset: 0x00065E94
		public ConfettiSwipeParticleSystemRequestDelegate(Session s)
		{
			this.session = s;
			this.viewportPosition = new Vector3(0.5f, 1.25f, this.session.camera.UnityCamera.nearClipPlane);
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x00067CD8 File Offset: 0x00065ED8
		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x00067CDC File Offset: 0x00065EDC
		public Vector3 Position
		{
			get
			{
				return this.session.camera.UnityCamera.ViewportToWorldPoint(this.viewportPosition);
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x00067CFC File Offset: 0x00065EFC
		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000B4C RID: 2892
		protected Session session;

		// Token: 0x04000B4D RID: 2893
		protected Vector3 viewportPosition;
	}

	// Token: 0x020001D4 RID: 468
	private class BalloonSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x06000FF8 RID: 4088 RVA: 0x00067D00 File Offset: 0x00065F00
		public BalloonSwipeParticleSystemRequestDelegate(Session s)
		{
			this.session = s;
			this.viewportPosition = new Vector3(0.5f, -0.25f, this.session.camera.UnityCamera.nearClipPlane + 25f);
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x00067D4C File Offset: 0x00065F4C
		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00067D50 File Offset: 0x00065F50
		public Vector3 Position
		{
			get
			{
				return this.session.camera.UnityCamera.ViewportToWorldPoint(this.viewportPosition);
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x00067D70 File Offset: 0x00065F70
		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000B4E RID: 2894
		protected Session session;

		// Token: 0x04000B4F RID: 2895
		protected Vector3 viewportPosition;
	}

	// Token: 0x020001D5 RID: 469
	private class SeaflowerSwipeParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x06000FFC RID: 4092 RVA: 0x00067D74 File Offset: 0x00065F74
		public SeaflowerSwipeParticleSystemRequestDelegate(Session s)
		{
			this.session = s;
			this.viewportPosition = new Vector3(0.15f, -0.25f, this.session.camera.UnityCamera.nearClipPlane + 25f);
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x00067DC0 File Offset: 0x00065FC0
		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00067DC4 File Offset: 0x00065FC4
		public Vector3 Position
		{
			get
			{
				return this.session.camera.UnityCamera.ViewportToWorldPoint(this.viewportPosition);
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000FFF RID: 4095 RVA: 0x00067DE4 File Offset: 0x00065FE4
		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000B50 RID: 2896
		protected Session session;

		// Token: 0x04000B51 RID: 2897
		protected Vector3 viewportPosition;
	}

	// Token: 0x020001D6 RID: 470
	private class FogEffectRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x06001000 RID: 4096 RVA: 0x00067DE8 File Offset: 0x00065FE8
		public FogEffectRequestDelegate(Session s)
		{
			this.position = new Vector3(0f, 0f, 100f);
			this.session = s;
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x00067E14 File Offset: 0x00066014
		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x00067E24 File Offset: 0x00066024
		// (set) Token: 0x06001002 RID: 4098 RVA: 0x00067E18 File Offset: 0x00066018
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001004 RID: 4100 RVA: 0x00067E2C File Offset: 0x0006602C
		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000B52 RID: 2898
		protected Session session;

		// Token: 0x04000B53 RID: 2899
		protected Vector3 position;
	}

	// Token: 0x020001D7 RID: 471
	private class TapFXParticleSystemRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x06001005 RID: 4101 RVA: 0x00067E30 File Offset: 0x00066030
		public TapFXParticleSystemRequestDelegate(Session s)
		{
			this.session = s;
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x00067E40 File Offset: 0x00066040
		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x00067E50 File Offset: 0x00066050
		// (set) Token: 0x06001007 RID: 4103 RVA: 0x00067E44 File Offset: 0x00066044
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x00067E58 File Offset: 0x00066058
		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000B54 RID: 2900
		protected Session session;

		// Token: 0x04000B55 RID: 2901
		protected Vector3 position;
	}

	// Token: 0x020001D8 RID: 472
	public class InteractionStripMixin
	{
		// Token: 0x0600100B RID: 4107 RVA: 0x00067E64 File Offset: 0x00066064
		public void ActivateOnSelected(Session session)
		{
			Simulated selected = session.game.selected;
			TFUtils.Assert(selected != null, "Cannot enable interaction strip unless there is a selected simulated");
			TFUtils.Assert(selected.InteractionState.Controls != null && selected.InteractionState.Controls.Count > 0, "Trying to activate interation strip on a simulated that has no control bindings.Sim=" + selected.ToString());
			ICollection<IControlBinding> controls = selected.InteractionState.Controls;
			SBGUIElement val = SBUIBuilder.MakeAndAddInteractionStrip(session, (uint)selected.entity.DefinitionId, session.SimulationSBGUIScreen, controls);
			session.AddAsyncResponse("InteractionStrip", val);
			session.AddAsyncResponse("InteractionControls", controls);
			this.MoveSubUiWithSelected(session);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00067F10 File Offset: 0x00066110
		public void Deactivate(Session session)
		{
			session.CheckAsyncRequest("InteractionStrip_AcceptCallback");
			session.CheckAsyncRequest("InteractionStrip_RejectCallback");
			session.CheckAsyncRequest("InteractionControls");
			SBGUIElement sbguielement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (sbguielement != null)
			{
				SBUIBuilder.ReleaseInteractionStrip(sbguielement);
			}
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00067F64 File Offset: 0x00066164
		public void EnableRejectButton(Session session, bool enable)
		{
			SBGUIElement sbguielement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (sbguielement != null)
			{
				sbguielement.EnableRejectButton(enable);
				session.AddAsyncResponse("InteractionStrip", sbguielement);
			}
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00067FA4 File Offset: 0x000661A4
		public void EnableButtons(Session session, bool enable)
		{
			SBGUIElement sbguielement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (sbguielement != null)
			{
				sbguielement.EnableButtons(enable);
				session.AddAsyncResponse("InteractionStrip", sbguielement);
			}
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00067FE4 File Offset: 0x000661E4
		public void OnUpdate(Session session)
		{
			this.MoveSubUiWithSelected(session);
			List<IControlBinding> list = (List<IControlBinding>)session.CheckAsyncRequest("InteractionControls");
			if (list != null)
			{
				list.ForEach(delegate(IControlBinding control)
				{
					control.DynamicUpdate(session);
				});
				session.AddAsyncResponse("InteractionControls", list);
			}
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0006804C File Offset: 0x0006624C
		public void SetAcceptHandler(Session session, Action<Session> handler)
		{
			session.CheckAsyncRequest("InteractionStrip_AcceptCallback");
			session.AddAsyncResponse("InteractionStrip_AcceptCallback", handler);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00068068 File Offset: 0x00066268
		public void SetRejectHandler(Session session, Action<Session> handler)
		{
			session.CheckAsyncRequest("InteractionStrip_RejectCallback");
			session.AddAsyncResponse("InteractionStrip_RejectCallback", handler);
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001012 RID: 4114 RVA: 0x00068084 File Offset: 0x00066284
		// (set) Token: 0x06001013 RID: 4115 RVA: 0x0006808C File Offset: 0x0006628C
		public Vector3 StripPosition { get; set; }

		// Token: 0x06001014 RID: 4116 RVA: 0x00068098 File Offset: 0x00066298
		public bool FindTutorialPointer(Session session)
		{
			SBGUIElement sbguielement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (null == sbguielement)
			{
				return false;
			}
			session.AddAsyncResponse("InteractionStrip", sbguielement);
			foreach (SBGUIElement sbguielement2 in sbguielement.GetComponentsInChildren<SBGUIElement>())
			{
				if (sbguielement2.name.Contains("TutorialPointer"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00068108 File Offset: 0x00066308
		private void MoveSubUiWithSelected(Session session)
		{
			SBGUIElement sbguielement = (SBGUIElement)session.CheckAsyncRequest("InteractionStrip");
			if (null == sbguielement)
			{
				return;
			}
			if (session.game.selected == null)
			{
				session.AddAsyncResponse("InteractionStrip", sbguielement);
				this.Deactivate(session);
			}
			else
			{
				Vector2 position = session.game.selected.Position;
				Vector2 screenPosition = session.TheCamera.WorldPointToScreenPoint(new Vector3(position.x, position.y));
				sbguielement.SetScreenPosition(screenPosition);
				this.StripPosition = position;
				sbguielement.GUIUpdate();
				session.AddAsyncResponse("InteractionStrip", sbguielement);
			}
		}

		// Token: 0x04000B56 RID: 2902
		private const string INTERACTION_STRIP = "InteractionStrip";

		// Token: 0x04000B57 RID: 2903
		private const string INTERACTION_CONTROLS = "InteractionControls";

		// Token: 0x04000B58 RID: 2904
		public const string ACCEPT_CALLBACK = "InteractionStrip_AcceptCallback";

		// Token: 0x04000B59 RID: 2905
		public const string REJECT_CALLBACK = "InteractionStrip_RejectCallback";
	}

	// Token: 0x020001D9 RID: 473
	public class NamebarMixin
	{
		// Token: 0x06001017 RID: 4119 RVA: 0x000681B8 File Offset: 0x000663B8
		public bool ActivateOnSelected(Session pSession, Simulated pSimulated, float fYOffset = 20f)
		{
			bool result = false;
			Simulated selected = pSession.game.selected;
			TFUtils.Assert(pSimulated != null, "Cannot enable Namebar unless there is a simulated");
			if (pSimulated.m_pNamebarMixinArgs.m_bHasNamebar)
			{
				SBGUINamebar.HostPosition hPosition = () => pSession.game.simulation.ScreenPositionFromWorldPosition(selected.Position);
				Action onFinish = delegate()
				{
					pSession.ChangeState("Playing", true);
					pSession.game.selected = null;
				};
				List<int> list = null;
				Action<int> pTaskCharacterClicked = null;
				if (pSimulated.m_pNamebarMixinArgs.m_bCheckForTaskCharacters)
				{
					list = pSession.TheGame.taskManager.GetActiveSourcesForTarget(pSimulated.Id);
					int num = list.Count;
					for (int i = 0; i < num; i++)
					{
						List<Task> activeTasksForSimulated = pSession.TheGame.taskManager.GetActiveTasksForSimulated(list[i], null, true);
						if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() <= 0UL)
						{
							list.RemoveAt(i);
							i--;
							num--;
						}
					}
					if (list != null && list.Count > 0)
					{
						pTaskCharacterClicked = delegate(int nDID)
						{
							pSession.CheckAsyncRequest(Session.SelectedPlaying.TASK_CHARACTER_SELECT);
							pSession.AddAsyncResponse(Session.SelectedPlaying.TASK_CHARACTER_SELECT, nDID, false);
						};
					}
				}
				SBGUINamebar sbguinamebar = SBUIBuilder.MakeAndAddNamebar(pSession, pSession.SimulationSBGUIScreen, pSimulated.m_pNamebarMixinArgs.m_sName, hPosition, onFinish, list, pTaskCharacterClicked);
				this.m_sGameObjectID = sbguinamebar.gameObject.name;
				this.m_pNamebarGUI = sbguinamebar;
				result = true;
			}
			return result;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00068340 File Offset: 0x00066540
		public void Deactivate(Session pSession)
		{
			if (this.m_sGameObjectID != null && this.m_pNamebarGUI != null)
			{
				this.m_pNamebarGUI.Close();
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001019 RID: 4121 RVA: 0x0006836C File Offset: 0x0006656C
		public bool IsActive
		{
			get
			{
				return this.m_pNamebarGUI != null && this.m_pNamebarGUI.IsActive();
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00068390 File Offset: 0x00066590
		public void Extend()
		{
			if (this.m_pNamebarGUI != null)
			{
				this.m_pNamebarGUI.elapsed = 0f;
			}
		}

		// Token: 0x04000B5B RID: 2907
		public const int YOFFSET = 20;

		// Token: 0x04000B5C RID: 2908
		public const int HEIGHT = 100;

		// Token: 0x04000B5D RID: 2909
		private const string _sNAMEBAR = "Namebar";

		// Token: 0x04000B5E RID: 2910
		private SBGUINamebar m_pNamebarGUI;

		// Token: 0x04000B5F RID: 2911
		private string m_sGameObjectID;
	}

	// Token: 0x020001DA RID: 474
	public class NamebarGroup
	{
		// Token: 0x0600101C RID: 4124 RVA: 0x000683D4 File Offset: 0x000665D4
		public void ActivateOnSelected(Session pSession)
		{
			if (pSession.game.selected.Variable.ContainsKey("TaskSrcUnit"))
			{
				this.m_pTaskAtBuildingNamebar.ActivateOnSelected(pSession, (Simulated)pSession.game.selected.Variable["TaskSrcUnit"], 120f);
			}
			this.m_pNamebar.ActivateOnSelected(pSession, pSession.game.selected, 20f);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00068450 File Offset: 0x00066650
		public void Deactivate(Session pSession)
		{
			this.m_pNamebar.Deactivate(pSession);
			if (pSession.game.selected != null && pSession.game.selected.Variable.ContainsKey("TaskSrcUnit"))
			{
				this.m_pTaskAtBuildingNamebar.Deactivate(pSession);
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x000684A4 File Offset: 0x000666A4
		public bool IsActive
		{
			get
			{
				return this.m_pNamebar != null && this.m_pNamebar.IsActive;
			}
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x000684C0 File Offset: 0x000666C0
		public void Extend()
		{
			this.m_pNamebar.Extend();
		}

		// Token: 0x04000B60 RID: 2912
		public const string TASK_SRC_UNIT = "TaskSrcUnit";

		// Token: 0x04000B61 RID: 2913
		private Session.NamebarMixin m_pTaskAtBuildingNamebar = new Session.NamebarMixin();

		// Token: 0x04000B62 RID: 2914
		private Session.NamebarMixin m_pNamebar = new Session.NamebarMixin();
	}

	// Token: 0x020001DB RID: 475
	public abstract class Prioritizer
	{
		// Token: 0x06001020 RID: 4128 RVA: 0x000684D0 File Offset: 0x000666D0
		public Prioritizer(Camera camera)
		{
			this.camera = camera;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001021 RID: 4129 RVA: 0x000684E0 File Offset: 0x000666E0
		public Simulated Best
		{
			get
			{
				return this.best;
			}
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x000684E8 File Offset: 0x000666E8
		public void SelectBest(Simulated simulated)
		{
			if (this.best == null)
			{
				this.best = simulated;
			}
			else if (this.Compare(simulated, this.best) < 0)
			{
				this.best = simulated;
			}
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0006851C File Offset: 0x0006671C
		public float distanceToCamera(Simulated simulated, Camera camera)
		{
			return (camera.transform.position - new Vector3(simulated.Position.x, simulated.Position.y, 0f)).sqrMagnitude;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00068568 File Offset: 0x00066768
		protected int CompareByDistanceToCamera(Simulated a, Simulated b)
		{
			float num = this.distanceToCamera(a, this.camera);
			float num2 = this.distanceToCamera(b, this.camera);
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06001025 RID: 4133
		protected abstract int Compare(Simulated a, Simulated b);

		// Token: 0x04000B63 RID: 2915
		protected Simulated best;

		// Token: 0x04000B64 RID: 2916
		protected Camera camera;
	}

	// Token: 0x020001DC RID: 476
	public class SelectionPrioritizer : Session.Prioritizer
	{
		// Token: 0x06001026 RID: 4134 RVA: 0x000685A4 File Offset: 0x000667A4
		public SelectionPrioritizer(Camera camera) : base(camera)
		{
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x000685B0 File Offset: 0x000667B0
		protected override int Compare(Simulated a, Simulated b)
		{
			int selectionPriority = a.SelectionPriority;
			int selectionPriority2 = b.SelectionPriority;
			if (selectionPriority < selectionPriority2)
			{
				return -1;
			}
			if (selectionPriority > selectionPriority2)
			{
				return 1;
			}
			return base.CompareByDistanceToCamera(a, b);
		}
	}

	// Token: 0x020001DD RID: 477
	public class TemptationPrioritizer : Session.Prioritizer
	{
		// Token: 0x06001028 RID: 4136 RVA: 0x000685E8 File Offset: 0x000667E8
		public TemptationPrioritizer(Camera camera) : base(camera)
		{
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x000685F4 File Offset: 0x000667F4
		protected override int Compare(Simulated a, Simulated b)
		{
			if (a.TemptationPriority < b.TemptationPriority)
			{
				return -1;
			}
			if (a.TemptationPriority > b.TemptationPriority)
			{
				return 1;
			}
			return base.CompareByDistanceToCamera(a, b);
		}
	}

	// Token: 0x020001DE RID: 478
	public class AgeGate : Session.State
	{
		// Token: 0x0600102B RID: 4139 RVA: 0x00068644 File Offset: 0x00066844
		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.PlaySeaflowerAndBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false, false, default(Vector3));
			string deviceLanguage = Language.getDeviceLanguage();
			HelpshiftSdk.getInstance().setNameAndEmail(Soaring.Player.UserTag, Soaring.Player.UserTag + "@example.com");
			HelpshiftSdk.getInstance().setUserIdentifier(Soaring.Player.UserTag);
			Dictionary<string, object> configMap = new Dictionary<string, object>();
			configMap.Add("enableContactUs", "always");
			if (!string.IsNullOrEmpty(deviceLanguage) && !deviceLanguage.Contains("EN") && !deviceLanguage.Contains("en"))
			{
				configMap["enableContactUs"] = "never";
			}
			configMap.Add("gotoConversationAfterContactUs", "no");
			configMap.Add("showConversationResolutionQuestion", "yes");
			configMap.Add("requireEmail", false);
			configMap.Add("hideNameAndEmail", true);
			configMap.Add("enableFullPrivacy", "yes");
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("playerLevel", session.TheGame.resourceManager.Query(ResourceManager.LEVEL).ToString());
			dictionary.Add("playerCoins", session.TheGame.resourceManager.Query(ResourceManager.SOFT_CURRENCY).ToString());
			dictionary.Add("playerJelly", session.TheGame.resourceManager.Query(ResourceManager.HARD_CURRENCY).ToString());
			dictionary.Add("playerXP", session.TheGame.resourceManager.Query(ResourceManager.XP).ToString());
			dictionary.Add("gameCenterLoggedIn", GameCenterBinding.isPlayerAuthenticated().ToString());
			dictionary.Add("playerLanguageCountry", deviceLanguage);
			if (!session.InFriendsGame)
			{
				List<string> list = new List<string>();
				session.TheGame.store.GetPurchases(session);
				int totalMoneySpent = SoaringRetrievePurchasesModule.TotalMoneySpent;
				dictionary.Add("money spent", totalMoneySpent.ToString());
				if (totalMoneySpent > 9)
				{
					list.Add("$10");
					if (totalMoneySpent > 99)
					{
						list.Add("whale");
					}
				}
				else if (totalMoneySpent > 0)
				{
					list.Add("lessthan$10");
				}
				else
				{
					list.Add("nomoneyspent");
				}
				if (list.Count > 0)
				{
					string[] value = list.ToArray();
					dictionary.Add("hs-tags", value);
				}
			}
			HelpshiftSdk.getInstance().updateMetaData(dictionary);
			Action action = delegate()
			{
				session.ChangeState("Options", true);
				if (this.keyboard != null && this.keyboard.active)
				{
					this.keyboard.active = false;
				}
				AndroidBack.getInstance().pop();
			};
			Action action2 = delegate()
			{
			};
			Action submitHandler = delegate()
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_NEED_NETWORK_TITLE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
					{
						item
					}, uint.MaxValue);
					session.ChangeState("ShowingDialog", true);
				}
				else if (this.SubmitCheck(session))
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					HelpshiftSdk.getInstance().showFAQs(configMap);
					session.ChangeState("Options", true);
				}
			};
			Action cancelHandler = delegate()
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				session.ChangeState("Options", true);
			};
			Action inputHandler = delegate()
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				if (this.ageGate != null)
				{
					this.keyboard = TouchScreenKeyboard.Open(this.inputString, TouchScreenKeyboardType.NumberPad);
				}
			};
			session.properties.ageGateHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, action2, action2, action, action2, null, Session.DragFeeding.SwitchToFn(session), null, action2, action2, action2, action2, false);
			session.properties.ageGateHud.SetActive(true);
			session.properties.ageGateHud.SetVisibleNonEssentialElements(false, true);
			SBGUIPulseButton sbguipulseButton = (SBGUIPulseButton)session.properties.ageGateHud.FindChild("marketplace");
			sbguipulseButton.SetActive(false);
			SBGUIPulseButton sbguipulseButton2 = (SBGUIPulseButton)session.properties.ageGateHud.FindChild("community_event");
			sbguipulseButton2.SetActive(false);
			if (session.InFriendsGame)
			{
				SBGUIElement sbguielement = session.properties.ageGateHud.FindChild("happyface");
				sbguielement.SetActive(false);
				SBGUIElement sbguielement2 = session.properties.ageGateHud.FindChild("quest_marker");
				sbguielement2.SetActive(false);
				SBGUIElement sbguielement3 = session.properties.ageGateHud.FindChild("jj_bar");
				sbguielement3.SetActive(false);
				SBGUIElement sbguielement4 = session.properties.ageGateHud.FindChild("money_bar");
				sbguielement4.SetActive(false);
				SBGUIElement sbguielement5 = session.properties.ageGateHud.FindChild("special_bar");
				if (sbguielement5)
				{
					sbguielement5.SetActive(false);
				}
			}
			this.ageGate = SBUIBuilder.MakeAndPushAgeGateDialog(action, submitHandler, cancelHandler, inputHandler);
			this.inputLabel = (SBGUILabel)this.ageGate.FindChild("input_label");
			this.invalidAnswer = (SBGUILabel)this.ageGate.FindChild("invalid_answer");
			this.invalidAnswer.SetActive(false);
			this.equationLabel = (SBGUILabel)this.ageGate.FindChild("equation");
			this.GenerateRandomEquation();
			this.equationLabel.SetText(string.Concat(new object[]
			{
				this.number1,
				" X ",
				this.number2,
				" = ?"
			}));
			session.TheCamera.TurnOnScreenBuffer();
			this.closeButton = (this.ageGate.FindChild("close") as SBGUIButton);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x00068C40 File Offset: 0x00066E40
		public bool SubmitCheck(Session session)
		{
			int num;
			bool flag = int.TryParse(this.inputString, out num);
			if (this.ageGate != null && flag && num == this.correctAnswer)
			{
				this.ageGate.FindChild("invalid_answer").SetActive(false);
				this.inputString = string.Empty;
				this.inputLabel.SetText(string.Empty);
				return true;
			}
			if (this.ageGate != null)
			{
				this.ageGate.FindChild("invalid_answer").SetActive(true);
				this.GenerateRandomEquation();
				this.inputString = string.Empty;
				this.inputLabel.SetText(string.Empty);
				if (this.equationLabel == null)
				{
					this.equationLabel = (SBGUILabel)this.ageGate.FindChild("equation");
				}
				this.equationLabel.SetText(string.Concat(new object[]
				{
					this.number1,
					" x ",
					this.number2,
					" = ?"
				}));
			}
			return false;
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00068D6C File Offset: 0x00066F6C
		public void GenerateRandomEquation()
		{
			this.number1 = UnityEngine.Random.Range(4, 10);
			this.number2 = UnityEngine.Random.Range(4, 10);
			this.correctAnswer = this.number1 * this.number2;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00068DA0 File Offset: 0x00066FA0
		public void OnLeave(Session session)
		{
			this.inputString = string.Empty;
			this.inputLabel.SetText(string.Empty);
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.properties.ageGateHud.SetVisibleNonEssentialElements(true);
			session.properties.ageGateHud = null;
			session.TheCamera.TurnOffScreenBuffer();
			if (this.keyboard != null && this.keyboard.active)
			{
				this.keyboard.active = false;
			}
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00068E2C File Offset: 0x0006702C
		public void OnUpdate(Session session = null)
		{
			if (this.keyboard != null && this.keyboard.active && this.keyboard.text.Length > 3)
			{
				this.keyboard.text = this.keyboard.text.Substring(0, 3);
			}
			if (this.keyboard != null && this.keyboard.done)
			{
				this.inputString = this.keyboard.text.Substring(0, this.keyboard.text.Length);
				this.inputLabel.SetText(this.inputString);
				this.keyboard.text = string.Empty;
				this.keyboard = null;
			}
			session.game.simulation.OnUpdate(session);
		}

		// Token: 0x04000B65 RID: 2917
		private int number1;

		// Token: 0x04000B66 RID: 2918
		private int number2;

		// Token: 0x04000B67 RID: 2919
		private int correctAnswer;

		// Token: 0x04000B68 RID: 2920
		private string inputString = string.Empty;

		// Token: 0x04000B69 RID: 2921
		private SBGUIScreen ageGate;

		// Token: 0x04000B6A RID: 2922
		private SBGUILabel equationLabel;

		// Token: 0x04000B6B RID: 2923
		private SBGUILabel invalidAnswer;

		// Token: 0x04000B6C RID: 2924
		private SBGUILabel inputLabel;

		// Token: 0x04000B6D RID: 2925
		private TouchScreenKeyboard keyboard;

		// Token: 0x04000B6E RID: 2926
		private SBGUIButton closeButton;
	}

	// Token: 0x020001DF RID: 479
	public class Authorizing : Session.State
	{
		// Token: 0x06001032 RID: 4146 RVA: 0x00068F10 File Offset: 0x00067110
		public void OnEnter(Session session)
		{
			Session.GameStarting.CreateLoadingScreen(session, false, "starting_progress", true);
			if (Application.platform == RuntimePlatform.Android)
			{
				Screen.sleepTimeout = -1;
			}
			session.InFriendsGame = false;
			session.Auth.SoaringAuthorizing = true;
			Player.Init();
			if (!Soaring.IsInitialized)
			{
				SBMISoaring.Initialize(session.Auth);
			}
			else
			{
				if (!Soaring.IsOnline || !Soaring.IsAuthorized)
				{
					SoaringInternal.instance.ClearOfflineMode();
					SoaringInternal.instance.BeginHandshake(new SoaringContextDelegate(this.HandshakeResponder));
				}
				else
				{
					this.HandshakeResponder(null);
				}
				Upsight.requestAppOpen();
			}
			session.analytics.LogLoadingFunnelStep("Authorizing");
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x00068FC4 File Offset: 0x000671C4
		public void HandshakeResponder(SoaringContext c)
		{
			SessionDriver.session_ref.auth.FindAndMigrateLoginID();
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x00068FD8 File Offset: 0x000671D8
		public void OnLeave(Session session)
		{
			SBUIBuilder.ReleaseTopScreen();
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00068FE0 File Offset: 0x000671E0
		public void OnUpdate(Session session)
		{
			if (session.Auth.SoaringAuthorizing || this.errorScreenShown)
			{
				return;
			}
			if (session.PlayerIsLoggedIn())
			{
				this.AddAdditionalCredentials();
				TFUtils.CheckForLogDumps(null);
				Session.ChangeToResolveSessionStateOnStartup(session);
			}
			else
			{
				if (!Soaring.IsOnline && string.IsNullOrEmpty(Soaring.Player.UserID))
				{
					SoaringDictionary soaringDictionary = new SoaringDictionary(4);
					soaringDictionary.addValue(SoaringPlatform.DeviceID, "tag");
					soaringDictionary.addValue(SoaringPlatform.DeviceID, "userId");
					soaringDictionary.addValue(string.Empty, "authToken");
					SoaringInternal.instance.UpdatePlayerData(soaringDictionary);
					Soaring.Player.LoginType = SoaringLoginType.Device;
					Soaring.Player.IsLocalAuthorized = true;
					session.ThePlayer = new Player(SoaringPlatform.DeviceID);
				}
				else
				{
					session.ThePlayer = new Player(Soaring.Player.UserTag);
				}
				if (session.ThePlayer != null)
				{
					TFUtils.DebugLog(string.Format("The player is logged in with playerId {0}", session.ThePlayer.playerId));
					session.WebFileServer.SetPlayerInfo(session.ThePlayer);
					session.analytics.PlayerId = session.ThePlayer.playerId;
					session.analytics.IsOffline = !Soaring.IsOnline;
				}
			}
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0006913C File Offset: 0x0006733C
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x00069140 File Offset: 0x00067340
		public void AddAdditionalCredentials()
		{
		}

		// Token: 0x04000B70 RID: 2928
		private bool errorScreenShown;
	}

	// Token: 0x020001E0 RID: 480
	public class BrowsingRecipes : Session.State
	{
		// Token: 0x06001039 RID: 4153 RVA: 0x0006914C File Offset: 0x0006734C
		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			Simulated sim = session.game.selected;
			if (sim == null)
			{
				TFUtils.DebugLog("attempted to transition to browsing recipes without a selected entity");
				session.ChangeState("Playing", true);
				return;
			}
			BuildingEntity buildingEntity = sim.GetEntity<BuildingEntity>();
			TFUtils.Assert(buildingEntity != null, "Did not select a building for Crafting!");
			TFUtils.Assert(session.game.craftManager != null, "CraftingManager was not setup for this game!");
			CraftingCookbook cookbook = session.game.craftManager.GetCookbookById(buildingEntity.CraftMenu);
			if (!session.properties.lastSelectedRecipe.ContainsKey(cookbook))
			{
				session.properties.lastSelectedRecipe[cookbook] = null;
			}
			int num;
			int effectiveSlotCount;
			if (buildingEntity.ShuntsCrafting)
			{
				TFUtils.Assert(buildingEntity.Slots == 0, "The UI is not equipped to handle buildings that have production slots AND shunt crafting!");
				num = buildingEntity.Annexes.Count;
				effectiveSlotCount = num;
			}
			else
			{
				effectiveSlotCount = buildingEntity.Slots;
				num = session.TheGame.craftManager.GetMaxSlots(session.TheGame.selected.entity.DefinitionId);
			}
			session.TheSoundEffectManager.PlaySound(cookbook.openSound);
			Action closeClickHandler = delegate()
			{
				session.ChangeState("Playing", true);
				AndroidBack.getInstance().pop();
			};
			Action<SBGUICraftingScreen, CraftingRecipe> craftRecipeHandler = delegate(SBGUICraftingScreen screen, CraftingRecipe recipe)
			{
				if (session.game.craftManager.HasCapacity(sim.Id, buildingEntity.Slots))
				{
					this.CheckRecipeForJelly(session, screen, recipe);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Error");
				}
			};
			Action<CraftingRecipe> setSelected = delegate(CraftingRecipe recipe)
			{
				session.properties.lastSelectedRecipe[cookbook] = recipe;
			};
			Action<int> rushCraftHandler = delegate(int slotId)
			{
				this.CraftProductionRush(session, slotId);
			};
			session.properties.recipesHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory", true);
			}, delegate
			{
				session.ChangeState("Options", true);
			}, delegate
			{
				session.ChangeState("Editing", true);
			}, null, null, null, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.ChangeState("CommunityEvent", true);
			}, null, false);
			bool flag = session.properties.recipesHud.ShowInventoryWidget();
			session.properties.recipesHud.SetVisibleNonEssentialElements(false);
			session.AddAsyncResponse("KeepInventoryOpen", !flag);
			session.properties.m_pTaskSimulated = null;
			session.properties.m_bAutoPanToSimulatedOnLeave = false;
			Action<int> pTaskCharacterClicked = delegate(int nDID)
			{
				Simulated simulated = session.TheGame.simulation.FindSimulated(new int?(nDID));
				if (simulated != null)
				{
					session.properties.m_pTaskSimulated = simulated;
					TaskManager taskManager = session.TheGame.taskManager;
					List<Task> activeTasksForSimulated2 = session.TheGame.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
					if (activeTasksForSimulated2.Count > 0 && activeTasksForSimulated2[0].GetTimeLeft() > 0UL)
					{
						if (taskManager.GetTaskingStateForSimulated(session.TheGame.simulation, nDID, simulated.Id) == TaskManager._eBlueprintTaskingState.eNone)
						{
							session.ChangeState("UnitIdle", true);
						}
						else
						{
							session.ChangeState("UnitBusy", true);
						}
					}
					else
					{
						session.properties.m_bAutoPanToSimulatedOnLeave = true;
						session.ChangeState("Playing", true);
					}
				}
			};
			List<int> activeSourcesForTarget = session.TheGame.taskManager.GetActiveSourcesForTarget(sim.Id);
			int num2 = activeSourcesForTarget.Count;
			for (int i = 0; i < num2; i++)
			{
				List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(activeSourcesForTarget[i], null, true);
				if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() <= 0UL)
				{
					activeSourcesForTarget.RemoveAt(i);
					i--;
					num2--;
				}
			}
			session.properties.recipesWindow = SBUIBuilder.MakeAndPushCraftingUI(session, null, closeClickHandler, craftRecipeHandler, rushCraftHandler, setSelected, cookbook, session.properties.lastSelectedRecipe[cookbook], activeSourcesForTarget, pTaskCharacterClicked, effectiveSlotCount, num);
			session.properties.transitionSilently = false;
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.sessionActionManager.SetActionHandler("browsing_ui", session, new List<SBGUIScreen>
			{
				session.properties.recipesHud,
				session.properties.recipesWindow
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
			session.TheCamera.TurnOnScreenBuffer();
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x0006964C File Offset: 0x0006784C
		public void OnLeave(Session session)
		{
			session.game.sessionActionManager.ClearActionHandler("browsing_ui", session);
			if (!session.properties.transitionSilently)
			{
				if (session.game.selected != null)
				{
					BuildingEntity entity = session.game.selected.GetEntity<BuildingEntity>();
					CraftingCookbook cookbookById = session.game.craftManager.GetCookbookById(entity.CraftMenu);
					session.TheSoundEffectManager.PlaySound(cookbookById.closeSound);
				}
				if (session.properties.recipesHud != null)
				{
					session.properties.recipesHud.SetVisibleNonEssentialElements(true);
					if (!(bool)session.CheckAsyncRequest("KeepInventoryOpen"))
					{
						session.properties.recipesHud.CloseInventoryWidget();
					}
				}
				if (session.properties.recipesWindow != null)
				{
					session.properties.recipesWindow.ForceCycleProdSlots();
				}
				if (session.TheCamera.ScreenBufferOn)
				{
					session.TheCamera.TurnOffScreenBuffer();
				}
				session.properties.recipesHud = null;
				session.properties.recipesWindow = null;
			}
			if (session.properties.m_pTaskSimulated != null)
			{
				session.game.selected = session.properties.m_pTaskSimulated;
			}
			if (session.game.selected != null && session.properties.m_bAutoPanToSimulatedOnLeave)
			{
				session.TheCamera.AutoPanToPosition(session.game.selected.PositionCenter, 0.75f);
			}
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x000697D8 File Offset: 0x000679D8
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x0006983C File Offset: 0x00067A3C
		public void CheckRecipeForJelly(Session session, SBGUICraftingScreen screen, CraftingRecipe recipe)
		{
			if (recipe.cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY) && session.game.resourceManager.CanPay(recipe.cost))
			{
				Action execute = delegate()
				{
					this.CraftRecipe(session, screen, recipe);
				};
				Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
				{
					session.analytics.LogPremiumCrafting(recipe.recipeName, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount, cost, canAfford);
				};
				int jellyCost = 0;
				recipe.cost.ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out jellyCost);
				Action complete = delegate()
				{
					session.ChangeState("BrowsingRecipes", true);
					session.properties.recipesWindow.ForceCycleProdSlots();
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, recipe.identity, jellyCost, recipe.recipeName, "craft", "instant_purchase", string.Empty, "confirm");
				};
				Action cancel = delegate()
				{
					session.ChangeState("BrowsingRecipes", true);
					session.properties.recipesWindow.ForceCycleProdSlots();
				};
				session.properties.transitionSilently = true;
				session.properties.hardSpendActions = new Session.HardSpendActions(execute, (ulong time) => new Cost(new Dictionary<int, int>
				{
					{
						ResourceManager.HARD_CURRENCY,
						jellyCost
					}
				}), string.Empty, recipe.identity, complete, cancel, logSpend, (!(session.properties.recipesWindow == null)) ? session.properties.recipesWindow.GetHardSpendPosition() : session.TheCamera.ScreenCenter);
				session.properties.recipesWindow.ShowScrollRegion(false);
				session.ChangeState("HardSpendConfirm", false);
			}
			else
			{
				this.CraftRecipe(session, screen, recipe);
			}
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00069A10 File Offset: 0x00067C10
		public void CraftRecipe(Session session, SBGUICraftingScreen screen, CraftingRecipe recipe)
		{
			Simulated simulated = session.game.selected;
			BuildingEntity buildingEntity = simulated.GetEntity<BuildingEntity>();
			int? num = null;
			SessionActionTracker sessionActionTracker = (SessionActionTracker)session.CheckAsyncRequest("force_crafting_instance_slot_sessionaction");
			if (sessionActionTracker != null)
			{
				ForceCraftingInstanceSlot forceCraftingInstanceSlot = (ForceCraftingInstanceSlot)sessionActionTracker.Definition;
				if (forceCraftingInstanceSlot != null)
				{
					num = new int?(forceCraftingInstanceSlot.SlotID);
				}
			}
			Entity entity = null;
			if (simulated.GetEntity<BuildingEntity>().ShuntsCrafting)
			{
				List<Entity> annexes = buildingEntity.Annexes;
				if (num != null)
				{
					entity = annexes[num.Value];
					if (session.TheGame.craftManager.Crafting(entity.Id))
					{
						TFUtils.WarningLog("Tried to force crafting into annex " + num.Value + ", but that annex is already crafting!");
						entity = null;
					}
				}
				if (entity == null)
				{
					entity = annexes.Find((Entity annex) => !session.TheGame.craftManager.Crafting(annex.Id));
				}
				if (entity == null)
				{
					TFUtils.WarningLog("Trying to craft but there are no free annexes for this building to shunt onto.");
					session.TheSoundEffectManager.PlaySound("Error");
					return;
				}
				buildingEntity = entity.GetDecorator<BuildingEntity>();
				simulated = session.game.simulation.FindSimulated(buildingEntity.Id);
			}
			if (session.game.resourceManager.CanPay(recipe.cost) && session.game.craftManager.HasCapacity(simulated.Id, buildingEntity.Slots))
			{
				session.game.resourceManager.Apply(recipe.cost, session.game);
				session.game.simulation.Router.Send(CraftCommand.Create(simulated.Id, simulated.Id));
				int num2 = session.game.craftManager.GetNextSlot(simulated.Id, buildingEntity.Slots);
				if (num != null && entity == null)
				{
					num2 = num.Value;
					if (session.TheGame.craftManager.GetCraftingInstance(simulated.Id, num2) != null)
					{
						TFUtils.WarningLog("Tried to force crafting into annex " + num.Value + ", but that annex is already crafting!");
						num2 = session.game.craftManager.GetNextSlot(simulated.Id, buildingEntity.Slots);
					}
				}
				TFUtils.Assert(num2 != -1, "Session check for HasCapacity did not catch a filled crafting state, or GetNextSlot is broken");
				CraftingInstance craftingInstance = new CraftingInstance(buildingEntity.Id, recipe.identity, TFUtils.EpochTime() + recipe.craftTime, recipe.rewardDefinition.GenerateReward(session.TheGame.simulation, true, false), num2);
				if (session.game.craftManager.AddCraftingInstance(craftingInstance))
				{
					CraftStartAction action = new CraftStartAction(buildingEntity.Id, num2, recipe.identity, craftingInstance.ReadyTimeUtc, recipe.rewardDefinition.GenerateReward(session.TheGame.simulation, true, false), recipe.cost);
					session.game.simulation.ModifyGameStateSimulated(simulated, action);
					session.game.simulation.Router.Send(CraftedCommand.Create(simulated.Id, simulated.Id, num2), recipe.craftTime);
					session.TheSoundEffectManager.PlaySound(recipe.startSoundImmediate);
					session.TheSoundEffectManager.PlaySound(recipe.startSoundBeat, recipe.beatLength);
					session.TheSoundEffectManager.PlaySound("CloseGalleyGrubMenu");
				}
			}
			else
			{
				Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(recipe.cost.ResourceAmounts, session.game.resourceManager);
				if (resourcesStillRequired.Count > 0)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					Action okAction = delegate()
					{
						this.CraftRecipe(session, screen, recipe);
						session.ChangeState("BrowsingRecipes", true);
					};
					Action cancelAction = delegate()
					{
						session.ChangeState("BrowsingRecipes", true);
					};
					session.InsufficientResourcesHandler(session, recipe.recipeName, recipe.identity, okAction, cancelAction, recipe.cost);
				}
				else
				{
					TFUtils.ErrorLog("Was not able to craft but had enough resources... something else bad has probably happened.");
				}
			}
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00069F20 File Offset: 0x00068120
		private void CraftProductionRush(Session session, int slotId)
		{
			TFUtils.Assert(session != null && session.game != null && session.game.selected != null, "Trying to update a Rush Slot in an invalid game state");
			int slotId2 = slotId;
			Simulated selected = session.game.selected;
			Simulated simToRush = selected;
			BuildingEntity entityToRush = simToRush.GetEntity<BuildingEntity>();
			if (entityToRush.ShuntsCrafting)
			{
				List<Entity> annexes = entityToRush.Annexes;
				entityToRush = annexes[slotId].GetDecorator<BuildingEntity>();
				slotId = 0;
				TFUtils.Assert(entityToRush != null && session.TheGame.craftManager.GetCraftingInstance(entityToRush.Id, slotId) != null, "Could not find an annex that was crafting from this list of annexes!");
				simToRush = session.game.simulation.FindSimulated(entityToRush.Id);
			}
			Cost.CostAtTime costAtTime;
			string subjectText;
			int subjectDID;
			Action execute;
			Action<bool, Cost> logSpend;
			Action complete;
			Action cancel;
			if (slotId < entityToRush.Slots)
			{
				CraftingInstance instance = session.game.craftManager.GetCraftingInstance(entityToRush.Id, slotId);
				CraftingRecipe recipe = session.game.craftManager.GetRecipeById(instance.recipeId);
				Cost fullCost = recipe.rushCost;
				ulong craftStartTime = instance.ReadyTimeUtc - recipe.craftTime;
				costAtTime = ((ulong ts) => Cost.Prorate(fullCost, craftStartTime, instance.ReadyTimeUtc, ts));
				subjectText = recipe.recipeName;
				subjectDID = recipe.identity;
				Cost pFinalCost = Cost.Prorate(fullCost, craftStartTime, instance.ReadyTimeUtc, TFUtils.EpochTime());
				execute = delegate()
				{
					instance.rushed = true;
					session.game.simulation.Router.Send(RushCommand.Create(entityToRush.Id, instance.slotId));
				};
				logSpend = delegate(bool canAfford, Cost cost)
				{
					session.analytics.LogRushCraft(recipe.recipeName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
				};
				complete = delegate()
				{
					session.ChangeState("BrowsingRecipes", true);
					session.properties.recipesWindow.ForceCycleProdSlots();
					string sItemName = "Accelerate_craft_" + recipe.recipeName;
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, recipe.identity, pFinalCost.ResourceAmounts[ResourceManager.HARD_CURRENCY], sItemName, "craft", "speedup", "craft", "confirm");
				};
				cancel = delegate()
				{
					session.ChangeState("BrowsingRecipes", true);
					session.properties.recipesWindow.ForceCycleProdSlots();
				};
			}
			else
			{
				Cost expandCost = session.game.craftManager.GetSlotExpandCost(entityToRush.DefinitionId, entityToRush.Slots);
				costAtTime = ((ulong ts) => expandCost);
				TFUtils.Assert(costAtTime != null, "Got back a new slot cost of null. This needs to be assigned somewhere!");
				subjectText = string.Empty;
				subjectDID = entityToRush.DefinitionId;
				execute = delegate()
				{
					entityToRush.AddCraftingSlot();
					session.game.resourceManager.Spend(expandCost, session.game);
					session.game.simulation.ModifyGameStateSimulated(simToRush, new PurchaseCraftingSlotAction(entityToRush.Id, expandCost, entityToRush.Slots));
				};
				logSpend = delegate(bool canAfford, Cost cost)
				{
					session.analytics.LogPurchaseProductionSlot(entityToRush.BlueprintName, entityToRush.Slots, cost, canAfford, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				};
				complete = delegate()
				{
					session.ChangeState("BrowsingRecipes", true);
					session.properties.recipesWindow.ForceCycleProdSlots();
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, entityToRush.DefinitionId, expandCost.ResourceAmounts[ResourceManager.HARD_CURRENCY], entityToRush.Name, entityToRush.Name, "unlock", string.Empty, "confirm");
				};
				cancel = delegate()
				{
					session.ChangeState("BrowsingRecipes", true);
					session.properties.recipesWindow.ForceCycleProdSlots();
				};
			}
			session.properties.transitionSilently = true;
			session.properties.hardSpendActions = new Session.HardSpendActions(execute, costAtTime, subjectText, subjectDID, complete, cancel, logSpend, session.properties.recipesWindow.GetHardSpendButtonPositionForSlot(slotId2));
			session.properties.recipesWindow.ShowScrollRegion(false);
			session.ChangeState("HardSpendConfirm", false);
		}

		// Token: 0x04000B71 RID: 2929
		private const string BROWSING_UI_HANDLER = "browsing_ui";

		// Token: 0x04000B72 RID: 2930
		private const string KEEP_INVENTORY_OPEN = "KeepInventoryOpen";
	}

	// Token: 0x020001E1 RID: 481
	public class CheckPatching : Session.State
	{
		// Token: 0x06001040 RID: 4160 RVA: 0x0006A2BC File Offset: 0x000684BC
		public void OnEnter(Session session)
		{
			TFUtils.DebugLog("Starting to Patch content");
			this._doneChecking = false;
			session.CheckForPatching(true);
			session.analytics.LogLoadingFunnelStep("CheckPatching");
			Session.GameStarting.CreateLoadingScreen(session, false, "starting_progress", true);
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0006A300 File Offset: 0x00068500
		public void OnLeave(Session session)
		{
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0006A304 File Offset: 0x00068504
		public void OnUpdate(Session session)
		{
			if (this._doneChecking)
			{
				session.ChangeState("GameStarting", true);
			}
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0006A320 File Offset: 0x00068520
		public void PatchingEventListener(string patchingEvent)
		{
			this._doneChecking = true;
		}

		// Token: 0x04000B73 RID: 2931
		private bool _doneChecking;
	}

	// Token: 0x020001E2 RID: 482
	public class Clearing : Session.State
	{
		// Token: 0x06001045 RID: 4165 RVA: 0x0006A334 File Offset: 0x00068534
		public void Purchase(Session session, DebrisEntity debrisEntity)
		{
			ClearableDecorator decorator = debrisEntity.GetDecorator<ClearableDecorator>();
			Cost clearCost = decorator.ClearCost;
			bool flag = session.game.resourceManager.CanPay(clearCost);
			if (flag)
			{
				Simulated selected = session.game.selected;
				session.game.resourceManager.Apply(clearCost, session.game);
				session.game.simulation.Router.Send(ClearCommand.Create(Identity.Null(), selected.Id));
				session.game.simulation.ModifyGameStateSimulated(selected, new DebrisStartAction(selected.Id, TFUtils.EpochTime() + decorator.ClearTime, decorator.ClearCost));
				session.ChangeState("Playing", false);
				session.analytics.LogClearDebris(selected.entity.BlueprintName, clearCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
			}
			else
			{
				Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(clearCost.ResourceAmounts, session.game.resourceManager);
				TFUtils.Assert(resourcesStillRequired.Count > 0, "Error occurred, we have enough resources to apply cost.");
				session.TheSoundEffectManager.PlaySound("Error");
				Action okAction = delegate()
				{
					this.Purchase(session, debrisEntity);
					session.ChangeState("Playing", true);
				};
				Action cancelAction = delegate()
				{
					session.ChangeState("Playing", true);
				};
				session.InsufficientResourcesHandler(session, "clear " + session.game.selected.entity.BlueprintName, session.game.selected.entity.DefinitionId, okAction, cancelAction, clearCost);
			}
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0006A544 File Offset: 0x00068744
		public void OnEnter(Session session)
		{
			Simulated selected = session.game.selected;
			DebrisEntity entity = selected.GetEntity<DebrisEntity>();
			TFUtils.Assert(entity != null, "Did not select a debris for Clearing!");
			this.Purchase(session, entity);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0006A580 File Offset: 0x00068780
		public void OnLeave(Session session)
		{
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0006A584 File Offset: 0x00068784
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0006A5E8 File Offset: 0x000687E8
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	// Token: 0x020001E3 RID: 483
	public class CommunityEventSession : Session.State
	{
		// Token: 0x0600104B RID: 4171 RVA: 0x0006A5F4 File Offset: 0x000687F4
		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.camera.SetEnableUserInput(false, false, default(Vector3));
			session.TheCamera.TurnOnScreenBuffer();
			Action openIAPTabHandlerSoft = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping", true);
			};
			Action openIAPTabHandlerHard = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping", true);
			};
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.properties.communityEventHud = SBUIBuilder.MakeAndPushStandardUI(session, false, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.ChangeState("Inventory", true);
			}, delegate
			{
				session.ChangeState("Options", true);
			}, delegate
			{
				session.ChangeState("Editing", true);
			}, null, Session.DragFeeding.SwitchToFn(session), null, openIAPTabHandlerSoft, openIAPTabHandlerHard, delegate
			{
				session.ChangeState("Playing", true);
			}, null, false);
			session.properties.communityEventHud.SetVisibleNonEssentialElements(false, true);
			SBGUIButton sbguibutton = (SBGUIButton)session.properties.communityEventHud.FindChild("inventory");
			sbguibutton.SetActive(false);
			SBGUIButton sbguibutton2 = (SBGUIButton)session.properties.communityEventHud.FindChild("marketplace");
			sbguibutton2.SetActive(false);
			SBGUIButton sbguibutton3 = (SBGUIButton)session.properties.communityEventHud.FindChild("community_event");
			sbguibutton3.SetActive(false);
			session.properties.communityEventHud.HideAllElements();
			session.properties.transitionSilently = false;
			Action<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward> purchaseHandler = delegate(CommunityEvent pEvent, SoaringCommunityEvent pSoaringEvent, SoaringCommunityEvent.Reward pReward)
			{
				int amount = session.TheGame.resourceManager.Resources[pEvent.m_nValueID].Amount;
				int nCost = pReward.m_nValue - amount;
				string sRewardName = string.Empty;
				if (pReward != null)
				{
					Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, pReward.m_nID, true);
					if (blueprint != null)
					{
						sRewardName = (string)blueprint.Invariable["name"];
					}
				}
				if (nCost > 0)
				{
					Dictionary<int, int> resourceAmounts = new Dictionary<int, int>
					{
						{
							pEvent.m_nValueID,
							nCost
						}
					};
					nCost = session.TheGame.resourceManager.GetResourcesPackageCostInHardCurrencyValue(new Cost(resourceAmounts));
					Dictionary<int, int> jellyRequired = new Dictionary<int, int>
					{
						{
							ResourceManager.HARD_CURRENCY,
							nCost
						}
					};
					Cost.CostAtTime cost2 = (ulong ulUtcNow) => new Cost(jellyRequired);
					Action cancel = delegate()
					{
						session.ChangeState("CommunityEvent", true);
						session.properties.communityEventScreen.BuyRewardCancel();
						AnalyticsWrapper.LogJellyConfirmation(session.game, pReward.m_nID, nCost, sRewardName, "buildings", "community_event_purchase", string.Empty, "cancel");
					};
					Action execute = delegate()
					{
						session.properties.communityEventScreen.BuyRewardConfirm(nCost);
						AnalyticsWrapper.LogJellyConfirmation(session.game, pReward.m_nID, nCost, sRewardName, "buildings", "community_event_purchase", string.Empty, "confirm");
					};
					Action complete = delegate()
					{
						session.ChangeState("CommunityEvent", true);
					};
					Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
					{
					};
					session.properties.hardSpendActions = new Session.HardSpendActions(execute, cost2, sRewardName, pReward.m_nID, complete, cancel, logSpend, session.properties.communityEventScreen.GetHardSpendButtonPosition());
					session.properties.transitionSilently = true;
					session.ChangeState("HardSpendConfirm", false);
				}
				else
				{
					session.properties.communityEventScreen.BuyRewardConfirm(0);
				}
			};
			SBGUIScreen sbguiscreen = SBUIBuilder.MakeAndPushCommunityEventDialog(session, delegate
			{
				session.ChangeState("Playing", true);
			}, purchaseHandler);
			session.properties.communityEventScreen = sbguiscreen.GetComponent<SBGUICommunityEventScreen>();
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0006A844 File Offset: 0x00068A44
		public void OnLeave(Session session)
		{
			if (!session.properties.transitionSilently)
			{
				session.TheSoundEffectManager.PlaySound("CloseMenu");
				session.properties.communityEventHud.SetVisibleNonEssentialElements(true);
				session.properties.communityEventHud = null;
				session.TheCamera.TurnOffScreenBuffer();
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0006A89C File Offset: 0x00068A9C
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0006A8D4 File Offset: 0x00068AD4
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	// Token: 0x020001E4 RID: 484
	public class Credits : Session.State
	{
		// Token: 0x06001050 RID: 4176 RVA: 0x0006A8E0 File Offset: 0x00068AE0
		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.PlayBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false, false, default(Vector3));
			session.TheCamera.TurnOnScreenBuffer();
			Action closeClickHandler = delegate()
			{
				session.ChangeState("Options", true);
				AndroidBack.getInstance().pop();
			};
			SBUIBuilder.MakeAndPushEmptyUI(session, null);
			SBUIBuilder.MakeAndPushCreditsUI(session, closeClickHandler);
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x0006A970 File Offset: 0x00068B70
		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.TheCamera.TurnOffScreenBuffer();
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x0006A990 File Offset: 0x00068B90
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}
	}

	// Token: 0x020001E5 RID: 485
	public class SessionDebug : Session.State
	{
		// Token: 0x06001054 RID: 4180 RVA: 0x0006A9D8 File Offset: 0x00068BD8
		public void OnEnter(Session session)
		{
			Session.SessionDebug.<OnEnter>c__AnonStoreyA0 <OnEnter>c__AnonStoreyA = new Session.SessionDebug.<OnEnter>c__AnonStoreyA0();
			<OnEnter>c__AnonStoreyA.session = session;
			<OnEnter>c__AnonStoreyA.<>f__this = this;
			<OnEnter>c__AnonStoreyA.debugScreen = null;
			<OnEnter>c__AnonStoreyA.session.TheSoundEffectManager.PlaySound("OpenMenu");
			<OnEnter>c__AnonStoreyA.session.PlayBubbleScreenSwipeEffect();
			<OnEnter>c__AnonStoreyA.session.camera.SetEnableUserInput(false, false, default(Vector3));
			<OnEnter>c__AnonStoreyA.session.TheCamera.TurnOnScreenBuffer();
			Action closeClickHandler = delegate()
			{
				<OnEnter>c__AnonStoreyA.session.ChangeState("Playing", true);
				AndroidBack.getInstance().pop();
			};
			<OnEnter>c__AnonStoreyA.commonToggleStuff = delegate(string output)
			{
				TFUtils.DebugLog(output);
				<OnEnter>c__AnonStoreyA.session.TheSoundEffectManager.PlaySound("Accept");
				<OnEnter>c__AnonStoreyA.debugScreen = (SBGUIDebugScreen)<OnEnter>c__AnonStoreyA.session.CheckAsyncRequest("DEBUG_SCREEN");
				<OnEnter>c__AnonStoreyA.debugScreen.Refresh();
				<OnEnter>c__AnonStoreyA.session.AddAsyncResponse("DEBUG_SCREEN", <OnEnter>c__AnonStoreyA.debugScreen);
			};
			Action toggleFreeEditMode = delegate()
			{
				Session.TheDebugManager.ToggleDebugPlaceObjects(<OnEnter>c__AnonStoreyA.session);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Toggling Free Edit Mode");
			};
			Action saveFreeEditProgress = delegate()
			{
				TFUtils.DebugLog("Saving Free Edit Progress is only supported in the editor");
				<OnEnter>c__AnonStoreyA.session.TheSoundEffectManager.PlaySound("Error");
			};
			Action toggleFramerateCounter = delegate()
			{
				Session.TheDebugManager.ToggleFramerateCounter(<OnEnter>c__AnonStoreyA.session);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Toggle Framerate Counter");
			};
			Action toggleHitBoxes = delegate()
			{
				Session.TheDebugManager.ToggleHitBoxes(<OnEnter>c__AnonStoreyA.session.game.simulation);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Toggle hit boxes");
			};
			Action toggleFootprints = delegate()
			{
				Session.TheDebugManager.ToggleFootprints(<OnEnter>c__AnonStoreyA.session.game.simulation);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Toggle footprints");
			};
			Action toggleExpansionBorders = delegate()
			{
				Session.TheDebugManager.ToggleExpansionBorders(<OnEnter>c__AnonStoreyA.session.game.simulation);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Toggle expansion borders");
			};
			Action addMoney = delegate()
			{
				int amount = 100000;
				<OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Add(ResourceManager.SOFT_CURRENCY, amount, <OnEnter>c__AnonStoreyA.session.game);
				Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
				{
					ResourceManager.AddAmountToGameState(ResourceManager.SOFT_CURRENCY, amount, gameState);
				};
				<OnEnter>c__AnonStoreyA.session.TheGame.LockedGameStateChange(writer);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Add money");
			};
			Action addJJ = delegate()
			{
				int amount = 1000;
				<OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Add(ResourceManager.HARD_CURRENCY, amount, <OnEnter>c__AnonStoreyA.session.game);
				Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
				{
					ResourceManager.AddAmountToGameState(ResourceManager.HARD_CURRENCY, amount, gameState);
				};
				<OnEnter>c__AnonStoreyA.session.TheGame.LockedGameStateChange(writer);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Add JJ");
			};
			Action addSpecialCurrency = delegate()
			{
				int amount = 100;
				if (ResourceManager.SPECIAL_CURRENCY >= 0)
				{
					<OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Add(ResourceManager.SPECIAL_CURRENCY, amount, <OnEnter>c__AnonStoreyA.session.game);
					Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
					{
						ResourceManager.AddAmountToGameState(ResourceManager.SPECIAL_CURRENCY, amount, gameState);
					};
					<OnEnter>c__AnonStoreyA.session.TheGame.LockedGameStateChange(writer);
					<OnEnter>c__AnonStoreyA.commonToggleStuff("Add Special Currency");
				}
			};
			Action addFoods = delegate()
			{
				Session.SessionDebug.<OnEnter>c__AnonStoreyA0.<OnEnter>c__AnonStoreyA5 <OnEnter>c__AnonStoreyA2 = new Session.SessionDebug.<OnEnter>c__AnonStoreyA0.<OnEnter>c__AnonStoreyA5();
				<OnEnter>c__AnonStoreyA2.<>f__ref$160 = <OnEnter>c__AnonStoreyA;
				<OnEnter>c__AnonStoreyA2.amount = 1000;
				KeyValuePair<int, Resource> kvp;
				foreach (KeyValuePair<int, Resource> kvp2 in <OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Resources)
				{
					kvp = kvp2;
					if (kvp.Key != 4 && kvp.Key != 5 && kvp.Key != 2 && kvp.Key != 3 && kvp.Key != 51 && kvp.Key != 52)
					{
						<OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Add(kvp.Key, <OnEnter>c__AnonStoreyA2.amount, <OnEnter>c__AnonStoreyA.session.game);
						Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
						{
							ResourceManager.AddAmountToGameState(kvp.Key, <OnEnter>c__AnonStoreyA2.amount, gameState);
						};
						<OnEnter>c__AnonStoreyA.session.TheGame.LockedGameStateChange(writer);
					}
				}
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Add Foods");
			};
			Action toggleRMT = delegate()
			{
				Session.TheDebugManager.ToggleRMT();
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Toggle RMT");
			};
			Action deleteServerGame = delegate()
			{
				Session.TheDebugManager.DeleteGameData();
			};
			Action resetEventItems = delegate()
			{
				CommunityEvent activeEvent = <OnEnter>c__AnonStoreyA.session.TheGame.communityEventManager.GetActiveEvent();
				if (activeEvent != null)
				{
					SBMISoaring.ResetEventGifts(<OnEnter>c__AnonStoreyA.session, int.Parse(activeEvent.m_sID), null);
					int nValueID = activeEvent.m_nValueID;
					if (<OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Resources.ContainsKey(nValueID))
					{
						Resource resource = <OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Resources[nValueID];
						Reward reward = new Reward(new Dictionary<int, int>
						{
							{
								nValueID,
								-resource.Amount
							}
						}, null, null, null, null, null, null, null, false, null);
						<OnEnter>c__AnonStoreyA.session.TheGame.ApplyReward(reward, TFUtils.EpochTime(), false);
						<OnEnter>c__AnonStoreyA.session.TheGame.ModifyGameState(new ReceiveRewardAction(reward, string.Empty));
						SBMISoaring.SetEventValue(<OnEnter>c__AnonStoreyA.session, int.Parse(activeEvent.m_sID), 0, null);
					}
				}
			};
			Action toggleFreeCameraMode = delegate()
			{
				Session.TheDebugManager.ToggleFreeCameraMode(<OnEnter>c__AnonStoreyA.session.camera);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Toggle Free Camera Mode");
			};
			Action completeAllQuests = delegate()
			{
				Session.debugManager.CompleteAllQuests();
				<OnEnter>c__AnonStoreyA.session.ChangeState("Playing", true);
			};
			Action levelUp = delegate()
			{
				int num = <OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Query(ResourceManager.LEVEL);
				int levelAmount = <OnEnter>c__AnonStoreyA.session.TheGame.levelingManager.MaxLevel - num;
				<OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Add(ResourceManager.LEVEL, levelAmount, <OnEnter>c__AnonStoreyA.session.game);
				<OnEnter>c__AnonStoreyA.session.TheGame.featureManager.UnlockAllFeatures();
				<OnEnter>c__AnonStoreyA.session.TheGame.craftManager.UnlockAllRecipes(<OnEnter>c__AnonStoreyA.session.TheGame);
				<OnEnter>c__AnonStoreyA.session.TheGame.movieManager.UnlockAllMovies();
				<OnEnter>c__AnonStoreyA.session.TheGame.costumeManager.UnlockAllCostumes();
				Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
				{
					ResourceManager.AddAmountToGameState(ResourceManager.LEVEL, levelAmount, gameState);
					<OnEnter>c__AnonStoreyA.session.TheGame.featureManager.UnlockAllFeaturesToGamestate(gameState);
					<OnEnter>c__AnonStoreyA.session.TheGame.craftManager.UnlockAllRecipesToGamestate(gameState);
					<OnEnter>c__AnonStoreyA.session.TheGame.movieManager.UnlockAllMoviesToGamestate(gameState);
					<OnEnter>c__AnonStoreyA.session.TheGame.costumeManager.UnlockAllCostumesToGamestate(gameState);
				};
				<OnEnter>c__AnonStoreyA.session.TheGame.LockedGameStateChange(writer);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Level up");
			};
			Action logDump = delegate()
			{
				Action okHandler = delegate()
				{
					TFUtils.LogDump(<OnEnter>c__AnonStoreyA.session, "button_log_dump", null, null);
					<OnEnter>c__AnonStoreyA.commonToggleStuff("Log Dump");
					SBUIBuilder.ReleaseTopScreen();
				};
				Action cancelHandler = delegate()
				{
					SoaringDictionary soaringDictionary = new SoaringDictionary(4);
					TFUtils.LogDump(<OnEnter>c__AnonStoreyA.session, "button_log_dump", null, soaringDictionary);
					SoaringPlatform.SendEmail(Soaring.Player.UserID + "-Debug-Log", soaringDictionary.ToString(), "bbethel@flyingwisdomstudios.com");
					<OnEnter>c__AnonStoreyA.commonToggleStuff("Log Dump");
					SBUIBuilder.ReleaseTopScreen();
				};
				SBUIBuilder.CreateErrorDialog(<OnEnter>c__AnonStoreyA.session, "DUMP LOG", "Would you like to just dump the log or send an email?", Language.Get("!!PREFAB_OK"), okHandler, Language.Get("!!SEND_EMAIL"), cancelHandler, 0.85f, 0.45f);
			};
			Action unlockDecos = delegate()
			{
				TFUtils.DebugLog("Moving all decorations and buildings to inventory");
				List<string> allBuildingBlueprintKeys = EntityManager.GetAllBuildingBlueprintKeys();
				foreach (string blueprint in allBuildingBlueprintKeys)
				{
					BuildingEntity decorator = <OnEnter>c__AnonStoreyA.session.game.entities.Create(blueprint, false).GetDecorator<BuildingEntity>();
					<OnEnter>c__AnonStoreyA.session.game.inventory.AddItem(decorator, null);
				}
			};
			Action addHourSimulation = delegate()
			{
				<OnEnter>c__AnonStoreyA.session.TheGame.AddTimeToSimulation(3600UL);
			};
			Action action = delegate()
			{
				SoaringDebug.DebugListTextures("DEBUG");
			};
			Action incrementDailyBonus = delegate()
			{
				SoaringContext soaringContext = new SoaringContext();
				soaringContext.Name = "DailyBonus";
				soaringContext.Responder = new SBMISoaring.SMBICacheDelegate();
				soaringContext.addValue(new SoaringObject(<OnEnter>c__AnonStoreyA.session), "session");
				SBMISoaring.RetrieveDailyBonuseCalendar(<OnEnter>c__AnonStoreyA.<>f__this.dailyBonusDay, soaringContext, new SoaringContextDelegate(<OnEnter>c__AnonStoreyA.<>f__this.DisplayDailyBonus));
			};
			Action fastFoward = delegate()
			{
				TFUtils.isFastForwarding = !TFUtils.isFastForwarding;
				if (TFUtils.isFastForwarding)
				{
					<OnEnter>c__AnonStoreyA.session.TheGame.FastForwardSimulationBegun();
				}
				else
				{
					<OnEnter>c__AnonStoreyA.session.TheGame.FastForwardSimulationFinished();
				}
			};
			Action addOneLevel = delegate()
			{
				<OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.Add(ResourceManager.LEVEL, 1, <OnEnter>c__AnonStoreyA.session.game);
				<OnEnter>c__AnonStoreyA.session.TheGame.resourceManager.UpdateLevelExpToMilestone(<OnEnter>c__AnonStoreyA.session.TheGame.levelingManager);
				Game.GamestateWriter writer = delegate(Dictionary<string, object> gameState)
				{
					ResourceManager.AddAmountToGameState(ResourceManager.LEVEL, 1, gameState);
				};
				<OnEnter>c__AnonStoreyA.session.TheGame.LockedGameStateChange(writer);
				<OnEnter>c__AnonStoreyA.commonToggleStuff("Add One Level");
				ResourceManager resourceManager = <OnEnter>c__AnonStoreyA.session.TheGame.resourceManager;
				List<Reward> rewards = null;
				IResourceProgressCalculator resourceCalculator = <OnEnter>c__AnonStoreyA.session.TheGame.simulation.resourceCalculatorManager.GetResourceCalculator(ResourceManager.XP);
				if (resourceCalculator != null)
				{
					resourceCalculator.GetRewardsForIncreasingResource(<OnEnter>c__AnonStoreyA.session.TheGame.simulation, resourceManager.Resources, 0, out rewards);
				}
				LevelUpDialogInputData item = new LevelUpDialogInputData(resourceManager.Query(ResourceManager.LEVEL), rewards);
				<OnEnter>c__AnonStoreyA.session.TheGame.dialogPackageManager.AddDialogInputBatch(<OnEnter>c__AnonStoreyA.session.TheGame, new List<DialogInputData>
				{
					item
				}, uint.MaxValue);
				<OnEnter>c__AnonStoreyA.session.ChangeState("ShowingDialog", true);
			};
			Action reset_device_id = delegate()
			{
				Session.debugManager.ResetDeviceID();
			};
			SBGUIScreen parent = SBUIBuilder.MakeAndPushEmptyUI(<OnEnter>c__AnonStoreyA.session, null);
			<OnEnter>c__AnonStoreyA.debugScreen = SBUIBuilder.MakeAndParentDebugUI(<OnEnter>c__AnonStoreyA.session, parent, closeClickHandler, toggleFramerateCounter, toggleFreeEditMode, saveFreeEditProgress, toggleHitBoxes, toggleFootprints, toggleExpansionBorders, addMoney, addJJ, addSpecialCurrency, addFoods, toggleRMT, deleteServerGame, resetEventItems, toggleFreeCameraMode, completeAllQuests, levelUp, logDump, unlockDecos, addHourSimulation, incrementDailyBonus, fastFoward, addOneLevel, reset_device_id);
			<OnEnter>c__AnonStoreyA.session.AddAsyncResponse("DEBUG_SCREEN", <OnEnter>c__AnonStoreyA.debugScreen);
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0006AC74 File Offset: 0x00068E74
		private void DisplayDailyBonus(SoaringContext context)
		{
			Session session = null;
			SoaringError soaringError = null;
			bool flag = false;
			if (Soaring.IsOnline && context != null)
			{
				flag = context.soaringValue("query");
				if (flag)
				{
					SoaringObjectBase soaringObjectBase = context.objectWithKey("session");
					if (soaringObjectBase != null)
					{
						session = (Session)((SoaringObject)soaringObjectBase).Object;
					}
					else
					{
						flag = false;
					}
				}
			}
			if (context != null)
			{
				SoaringObjectBase soaringObjectBase2 = context.objectWithKey("error_code");
				if (soaringObjectBase2 != null)
				{
					soaringError = (SoaringError)soaringObjectBase2;
				}
			}
			if (flag)
			{
				DailyBonusDialogInputData dailyBonusDialogInputData = new DailyBonusDialogInputData();
				this.dailyBonusDay = dailyBonusDialogInputData.CurrentDay + 1;
				if (!dailyBonusDialogInputData.AlreadyCollected)
				{
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
					{
						dailyBonusDialogInputData
					}, uint.MaxValue);
					session.ChangeState("ShowingDialog", true);
				}
			}
			else
			{
				int num = -1;
				if (soaringError != null)
				{
					num = soaringError.ErrorCode;
				}
				Debug.Log("TODO: HANDLE THE ERROR CODE: " + num.ToString());
			}
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0006AD84 File Offset: 0x00068F84
		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.TheCamera.TurnOffScreenBuffer();
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0006ADA4 File Offset: 0x00068FA4
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}

		// Token: 0x04000B74 RID: 2932
		private const string DEBUG_SCREEN = "DEBUG_SCREEN";

		// Token: 0x04000B75 RID: 2933
		private int dailyBonusDay = 1;
	}

	// Token: 0x020001E6 RID: 486
	public class DragFeeding : Session.State
	{
		// Token: 0x0600105D RID: 4189 RVA: 0x0006AE44 File Offset: 0x00069044
		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("grab_wish");
			session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
			SBGUIStandardScreen sbguistandardScreen = SBUIBuilder.MakeAndPushStandardUI(session, false, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), null, null, null, null, null, delegate(int i, YGEvent evt)
			{
			}, delegate(YGEvent evt)
			{
				this.HandleSBGUIEvent(new SBGUIEvent(evt), session);
			}, null, null, null, null, false);
			sbguistandardScreen.SetPatchyHudIconVisible();
			session.properties.dragFeedHud = sbguistandardScreen;
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.sessionActionManager.SetActionHandler("dragfeeding_ui_handler", session, new List<SBGUIScreen>
			{
				sbguistandardScreen
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			if (this.icon == null)
			{
				this.icon = SBGUIPulseImage.Create(sbguistandardScreen, session.properties.draggedGood.resource.GetResourceTexture(), new Vector2(100f, 100f), 0.75f, 0.4f, null);
				this.icon.transform.rotation = Session.DragFeeding.ICON_ANGLE;
			}
			this.icon.SetTextureFromAtlas(session.properties.draggedGood.resource.GetResourceTexture());
			this.SetIconToEventPosition(session.properties.carriedUiEvent);
			session.properties.carriedUiEvent = null;
			this.icon.SetVisible(true);
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x0006B044 File Offset: 0x00069244
		public void OnLeave(Session session)
		{
			session.properties.dragFeedHud.inventory.Tidy();
			session.game.sessionActionManager.ClearActionHandler("dragfeeding_ui_handler", session);
			session.properties.dragFeedHud = null;
			session.properties.draggedGood = null;
			if (session.properties.candidateSimulated != null)
			{
				this.CancelTempt(session);
			}
			this.icon.SetVisible(false);
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0006B0B8 File Offset: 0x000692B8
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			Predicate<Simulated> filterOutMatching = (Simulated simulated) => simulated.TemptationPriority >= 0;
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
			{
				bool flag = this.TryFeedTempted(session);
				int productId = session.properties.draggedGood.productId;
				if (!flag)
				{
					session.AddAsyncResponse("GoodReturnRequest", new GoodReturnRequest(this.icon.GetScreenPosition(), productId, session.game.resourceManager.Resources[productId].GetResourceTexture()));
					session.soundEffectManager.PlaySound("FailDragFeed");
				}
				session.ChangeState("Playing", true);
				session.CheckAsyncRequest("OriginalDragEvent");
				break;
			}
			case YGEvent.TYPE.TOUCH_STAY:
			case YGEvent.TYPE.TOUCH_MOVE:
				this.SetIconToEventPosition(evt);
				if (session.properties.candidateSimulated != null)
				{
					Ray ray;
					List<Simulated> list = Session.FindSimulatedsUnderPoint(filterOutMatching, session.game.simulation, session.TheCamera, evt.position - Session.DragFeeding.FINGER_OFFSET, out ray);
					if (!list.Contains(session.properties.candidateSimulated))
					{
						this.CancelTempt(session);
					}
				}
				if (session.properties.candidateSimulated == null)
				{
					Ray ray;
					Simulated simulated2 = Session.FindBestSimulatedUnderPoint(new Session.TemptationPrioritizer(session.camera.UnityCamera), filterOutMatching, session.game.simulation, session.TheCamera, evt.position - Session.DragFeeding.FINGER_OFFSET, out ray);
					if (simulated2 != null)
					{
						this.Tempt(simulated2, session);
					}
				}
				break;
			}
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0006B250 File Offset: 0x00069450
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			session.properties.dragFeedHud.inventory.IncrementDeductionsForTick(session.properties.draggedGood.productId);
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0006B2D8 File Offset: 0x000694D8
		public static void SwitchTo(Session session, int productId, YGEvent triggeringEvent)
		{
			Session.DragFeeding.SwitchToFn(session)(productId, triggeringEvent);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0006B2E8 File Offset: 0x000694E8
		public static Action<int, YGEvent> SwitchToFn(Session session)
		{
			return delegate(int productId, YGEvent evt)
			{
				TFUtils.Assert(session.TheGame.resourceManager.Resources.ContainsKey(productId), "Was given a productId that does not exist in the resource manager! ProductId=" + productId);
				session.properties.draggedGood = new Session.SessionProperties.DraggedGood(productId, session.TheGame.resourceManager.Resources[productId]);
				session.properties.carriedUiEvent = evt;
				session.ChangeState("DragFeeding", true);
			};
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0006B310 File Offset: 0x00069510
		private void SetIconToEventPosition(YGEvent evt)
		{
			this.icon.SetScreenPosition(new Vector2(evt.position.x, (float)Screen.height - evt.position.y) + Session.DragFeeding.FINGER_OFFSET);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0006B34C File Offset: 0x0006954C
		private void Tempt(Simulated simulated, Session session)
		{
			session.properties.candidateSimulated = simulated;
			session.game.simulation.Router.Send(TemptCommand.Create(Identity.Null(), session.properties.candidateSimulated.Id, new int?(session.properties.draggedGood.productId)));
			if (simulated.GetEntity<ResidentEntity>().HungerResourceId != null)
			{
				this.icon.Pulser.PulseStartLoop();
			}
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0006B3D4 File Offset: 0x000695D4
		private void CancelTempt(Session session)
		{
			session.game.simulation.Router.Send(AbortCommand.Create(Identity.Null(), session.properties.candidateSimulated.Id));
			session.properties.candidateSimulated = null;
			this.icon.Pulser.PulseStopLoop();
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0006B42C File Offset: 0x0006962C
		private bool TryFeedTempted(Session session)
		{
			Simulated candidateSimulated = session.properties.candidateSimulated;
			if (candidateSimulated != null)
			{
				int? num = new int?(candidateSimulated.GetEntity<ResidentEntity>().ForbiddenItemsWishTableDID);
				int? temptingID = new int?(session.properties.draggedGood.productId);
				bool flag = false;
				if (num != null)
				{
					CdfDictionary<int> cdfDictionary = session.game.wishTableManager.GetWishTable(num.Value);
					if (cdfDictionary != null)
					{
						cdfDictionary = cdfDictionary.Where((int productID) => productID == temptingID.GetValueOrDefault() && temptingID != null, true);
					}
					if (cdfDictionary != null && cdfDictionary.Count > 0)
					{
						flag = true;
					}
				}
				if (flag)
				{
					session.game.simulation.Router.Send(AbortCommand.Create(Identity.Null(), session.properties.candidateSimulated.Id));
					int? num2 = null;
					return false;
				}
			}
			if (session.properties.candidateSimulated == null)
			{
				return false;
			}
			session.game.simulation.Router.Send(OfferFoodCommand.Create(Identity.Null(), session.properties.candidateSimulated.Id, session.properties.draggedGood.productId));
			session.properties.candidateSimulated = null;
			return true;
		}

		// Token: 0x04000B79 RID: 2937
		private const string DRAGFEEDING_UI_HANDLER = "dragfeeding_ui_handler";

		// Token: 0x04000B7A RID: 2938
		private SBGUIPulseImage icon;

		// Token: 0x04000B7B RID: 2939
		private static readonly Vector2 FINGER_OFFSET = new Vector2(-15f, -45f);

		// Token: 0x04000B7C RID: 2940
		private static readonly Quaternion ICON_ANGLE = Quaternion.AngleAxis(-12f, new Vector3(0f, 0f, -1f));
	}

	// Token: 0x020001E7 RID: 487
	public class Editing : Session.Playing
	{
		// Token: 0x0600106A RID: 4202 RVA: 0x0006B594 File Offset: 0x00069794
		public override void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			Action inventoryClickHandler = delegate()
			{
				if (!session.game.featureManager.CheckFeature("inventory_soft"))
				{
					session.game.featureManager.UnlockFeature("inventory_soft");
					session.game.featureManager.ActivateFeatureActions(session.game, "inventory_soft");
					session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string>
					{
						"inventory_soft"
					}));
					return;
				}
				session.AddAsyncResponse("FromEdit", true);
				session.ChangeState("Inventory", true);
			};
			Action optionsHandler = delegate()
			{
				session.ChangeState("Options", true);
			};
			Action editClickHandler = delegate()
			{
				session.ChangeState("Playing", true);
			};
			Action openIAPTabHandlerSoft = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping", true);
			};
			Action openIAPTabHandlerHard = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping", true);
			};
			SBGUIStandardScreen sbguistandardScreen = SBUIBuilder.MakeAndPushStandardUI(session, true, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), null, inventoryClickHandler, optionsHandler, editClickHandler, delegate
			{
				session.ChangeState("Paving", true);
			}, null, null, openIAPTabHandlerSoft, openIAPTabHandlerHard, null, null, true);
			session.properties.playingHud = sbguistandardScreen;
			Action action = delegate()
			{
				session.ChangeState("ShowingDialog", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.camera.SetEnableUserInput(true, false, default(Vector3));
			session.musicManager.PlayTrack("InGame");
			SBGUIAtlasButton sbguiatlasButton = (SBGUIAtlasButton)sbguistandardScreen.FindChild("quest");
			sbguiatlasButton.SetActive(true);
			SBGUIButton sbguibutton = (SBGUIButton)session.properties.playingHud.FindChild("community_event");
			sbguibutton.SetActive(false);
			SBGUIAtlasButton sbguiatlasButton2 = (SBGUIAtlasButton)sbguistandardScreen.FindChild("editpath_toggle");
			if (sbguiatlasButton2 != null)
			{
				sbguiatlasButton2.SetActive(true);
			}
			SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)sbguistandardScreen.FindChild("edit_mode_fish_person");
			if (sbguiatlasImage != null)
			{
				sbguiatlasImage.SetActive(true);
				sbguiatlasImage.SetTextureFromAtlas("EditMode_FishPerson", true, false, true, false, false, 0);
			}
			session.game.sessionActionManager.SetActionHandler("playing_ui", session, new List<SBGUIScreen>
			{
				sbguistandardScreen
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			SessionActionSimulationHelper.EnableHandler(session, true);
			session.game.dropManager.MarkForClearCurrentDrops();
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0006B7EC File Offset: 0x000699EC
		public override void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.properties.playingHud = null;
			base.OnLeave(session);
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x0006B820 File Offset: 0x00069A20
		public override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			session.properties.playingHud.EnableUI(true);
			Predicate<Simulated> filterOutMatching = (Simulated simulated) => !simulated.InteractionState.IsEditable && !Session.TheDebugManager.debugPlaceObjects;
			Ray ray;
			Simulated simulated2 = Session.FindBestSimulatedUnderPoint(new Session.SelectionPrioritizer(session.camera.UnityCamera), filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out ray);
			YGEvent.TYPE type = evt.type;
			switch (type)
			{
			case YGEvent.TYPE.TOUCH_MOVE:
			case YGEvent.TYPE.DRAG:
				break;
			default:
				if (type == YGEvent.TYPE.TOUCH_BEGIN)
				{
					if (simulated2 != null)
					{
						if (simulated2.InteractionState.IsEditable || Session.TheDebugManager.debugPlaceObjects)
						{
							if (simulated2.entity == null)
							{
								TFUtils.ErrorLog("Session.Editing.HandleSBGUIEvent - clickedSim.entity is null");
							}
							else
							{
								session.TheSoundEffectManager.PlaySound(simulated2.entity.SoundOnSelect);
							}
							session.game.selected = simulated2;
							session.game.selected.Bounce();
							session.AddAsyncResponse("override_drag", session.game.selected);
							session.ChangeState("MoveBuildingInEdit", true);
						}
						foreach (Action action in simulated2.ClickListeners)
						{
							action();
						}
					}
				}
				break;
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x0006B9B4 File Offset: 0x00069BB4
		public override void OnUpdate(Session session)
		{
			base.OnUpdate(session);
		}

		// Token: 0x04000B7F RID: 2943
		public const string FROM_EDIT = "FromEdit";
	}

	// Token: 0x020001E8 RID: 488
	public class ErrorDialog : Session.State
	{
		// Token: 0x06001070 RID: 4208 RVA: 0x0006B9F8 File Offset: 0x00069BF8
		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			string title = (string)session.CheckAsyncRequest("error_message_title");
			string message = (string)session.CheckAsyncRequest("error_message");
			string acceptLabel = (string)session.CheckAsyncRequest("error_message_ok_label");
			Action okButtonHandler = (Action)session.CheckAsyncRequest("error_message_ok_action");
			float num = (float)session.CheckAsyncRequest("error_message_scale");
			SBGUIConfirmationDialog sbguiconfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), title, message, acceptLabel, null, null, okButtonHandler, null, true);
			SBGUILabel sbguilabel = (SBGUILabel)sbguiconfirmationDialog.FindChild("message_label");
			YGTextAtlasSprite component = sbguilabel.GetComponent<YGTextAtlasSprite>();
			component.scale = new Vector2(num, num);
			sbguiconfirmationDialog.tform.parent = GUIMainView.GetInstance().transform;
			sbguiconfirmationDialog.tform.localPosition = Vector3.zero;
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x0006BADC File Offset: 0x00069CDC
		public void OnLeave(Session session)
		{
			SBUIBuilder.ReleaseTopScreen();
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x0006BAE4 File Offset: 0x00069CE4
		public void OnUpdate(Session session)
		{
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x0006BAE8 File Offset: 0x00069CE8
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x04000B81 RID: 2945
		private const string ERROR_DIALOG = "ERROR_DIALOG";
	}

	// Token: 0x020001E9 RID: 489
	public class Expanding : Session.State
	{
		// Token: 0x06001075 RID: 4213 RVA: 0x0006BAF4 File Offset: 0x00069CF4
		public void OnEnter(Session session)
		{
			Action acceptButtonClickHandler = delegate()
			{
				session.ChangeState("Playing", true);
			};
			SBUIBuilder.MakeAndPushAcceptUI(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), acceptButtonClickHandler);
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.terrain.OutlineAvailableExpansionSlots(session.game);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x0006BBA4 File Offset: 0x00069DA4
		public void OnLeave(Session session)
		{
			session.game.terrain.HideAvailableExpansionSlots();
			session.game.sessionActionManager.ClearActionHandler("expanding_ui", session);
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x0006BBD8 File Offset: 0x00069DD8
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			YGEvent.TYPE type = evt.type;
			switch (type)
			{
			case YGEvent.TYPE.TOUCH_END:
				break;
			default:
				if (type == YGEvent.TYPE.TAP)
				{
					Ray ray = session.TheCamera.ScreenPointToRay(evt.position);
					TerrainSlot selectedSlot = session.game.terrain.selectedSlot;
					if (session.game.terrain.ProcessTap(ray, session.game))
					{
						if (selectedSlot != null)
						{
							selectedSlot.DrawOutline();
						}
						this.ShowDialog(session);
						return;
					}
				}
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				break;
			}
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x0006BC74 File Offset: 0x00069E74
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x0006BCD8 File Offset: 0x00069ED8
		private void ShowDialog(Session session)
		{
			SBGUIConfirmationDialog sbguiconfirmationDialog = (SBGUIConfirmationDialog)session.CheckAsyncRequest("expansion");
			if (null != sbguiconfirmationDialog)
			{
				SBUIBuilder.ReleaseTopScreen();
			}
			TerrainSlot slot = session.game.terrain.selectedSlot;
			TFUtils.Assert(slot != null, "Did not select an Expansion Slot properly!");
			session.game.terrain.HighlightSelection(slot);
			Cost expansionCost = session.game.terrain.GetExpansionCost(slot);
			Action cancelButtonHandler = delegate()
			{
				SBUIBuilder.ReleaseTopScreen();
				session.game.terrain.DropSelection(slot);
				slot.DrawOutline();
			};
			Action okButtonHandler = delegate()
			{
				SBUIBuilder.ReleaseTopScreen();
				this.PurchaseExpansion(session);
				session.ChangeState("Playing", true);
			};
			sbguiconfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), Language.Get("!!EXPANSION_TITLE"), Language.Get("!!EXPANSION_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_CANCEL"), Cost.DisplayDictionary(expansionCost.ResourceAmounts, session.TheGame.resourceManager), okButtonHandler, cancelButtonHandler, false);
			session.AddAsyncResponse("expansion", sbguiconfirmationDialog);
			session.game.sessionActionManager.SetActionHandler("expanding_ui", session, new List<SBGUIScreen>
			{
				sbguiconfirmationDialog
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			SessionActionSimulationHelper.EnableHandler(session, true);
			session.soundEffectManager.PlaySound("Dialog_Expansion");
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0006BE88 File Offset: 0x0006A088
		public void PurchaseExpansion(Session session)
		{
			TerrainSlot slot = session.game.terrain.selectedSlot;
			Cost expansionCost = session.game.terrain.GetExpansionCost(slot);
			bool flag = session.game.resourceManager.CanPay(expansionCost);
			if (flag)
			{
				session.TheSoundEffectManager.PlaySound("PurchaseExpansion");
				session.game.resourceManager.Apply(expansionCost, session.game);
				session.game.terrain.AddExpansionSlot(slot.Id);
				if (session.game.featureManager.CheckFeature("purchase_expansions"))
				{
					session.game.terrain.UpdateRealtySigns(session.game.entities.DisplayControllerManager, new BillboardDelegate(SBCamera.BillboardDefinition), session.game);
				}
				if (session.game.terrain.IsBorderSlot(slot.Id))
				{
					session.game.border.UpdateTerrainBorderStrip(session.game.terrain);
				}
				foreach (TerrainSlotObject terrainSlotObject in slot.landmarks)
				{
					session.game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), terrainSlotObject.id));
				}
				foreach (TerrainSlotObject terrainSlotObject2 in slot.debris)
				{
					session.game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), terrainSlotObject2.id));
				}
				session.game.ModifyGameState(new NewExpansionAction(slot.Id, expansionCost, slot.debris, slot.landmarks));
				AnalyticsWrapper.LogExpansion(session.game, slot.Id, expansionCost);
				TFUtils.DebugLog("Purchased Expansion Slot: " + slot.Id);
				session.TheSoundEffectManager.PlaySound("Purchase");
				session.analytics.LogExpansion(slot.Id, expansionCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				session.ChangeState("Playing", true);
			}
			else
			{
				Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(expansionCost.ResourceAmounts, session.game.resourceManager);
				TFUtils.Assert(resourcesStillRequired.Count > 0, "Error occurred, we have enough resources to apply cost.");
				session.TheSoundEffectManager.PlaySound("Error");
				Action action = delegate()
				{
					session.game.terrain.DropSelection(slot);
					session.ChangeState("Expanding", true);
				};
				Action action2 = delegate()
				{
					this.PurchaseExpansion(session);
				};
				Action cancelAction = action;
				Action okAction = action2;
				SBGUIConfirmationDialog sbguiconfirmationDialog = (SBGUIConfirmationDialog)session.CheckAsyncRequest("expansion");
				session.AddAsyncResponse("expansion", sbguiconfirmationDialog);
				sbguiconfirmationDialog.SetActive(false);
				session.InsufficientResourcesHandler(session, "Expansion " + slot.Id, slot.Id, okAction, cancelAction, expansionCost);
			}
		}
	}

	// Token: 0x020001EA RID: 490
	public class NewExpansion : Session.State
	{
		// Token: 0x0600107C RID: 4220 RVA: 0x0006C2F0 File Offset: 0x0006A4F0
		public void OnEnter(Session session)
		{
			session.camera.SetEnableUserInput(false, false, default(Vector3));
			this.ShowDialog(session);
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0006C380 File Offset: 0x0006A580
		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.CheckAsyncRequest("expansion");
			session.game.sessionActionManager.ClearActionHandler("expansion_ui", session);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0006C3C0 File Offset: 0x0006A5C0
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0006C424 File Offset: 0x0006A624
		public void PurchaseExpansion(Session session)
		{
			TerrainSlot slot = session.game.terrain.selectedSlot;
			bool flag = false;
			object expandLock = Session.expandLock;
			lock (expandLock)
			{
				if (slot == null || slot.inUse)
				{
					flag = true;
				}
				if (slot != null)
				{
					slot.inUse = true;
				}
			}
			if (flag)
			{
				session.ChangeState("Playing", true);
				return;
			}
			Cost expansionCost = session.game.terrain.GetExpansionCost(slot);
			bool flag2 = session.game.resourceManager.CanPay(expansionCost);
			if (flag2)
			{
				session.TheSoundEffectManager.PlaySound("PurchaseExpansion");
				session.game.resourceManager.Apply(expansionCost, session.game);
				session.game.terrain.AddExpansionSlot(slot.Id);
				if (session.game.featureManager.CheckFeature("purchase_expansions"))
				{
					session.game.terrain.UpdateRealtySigns(session.game.entities.DisplayControllerManager, new BillboardDelegate(SBCamera.BillboardDefinition), session.game);
				}
				if (session.game.terrain.IsBorderSlot(slot.Id))
				{
					session.game.border.UpdateTerrainBorderStrip(session.game.terrain);
				}
				foreach (TerrainSlotObject terrainSlotObject in slot.landmarks)
				{
					session.game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), terrainSlotObject.id));
				}
				foreach (TerrainSlotObject terrainSlotObject2 in slot.debris)
				{
					session.game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), terrainSlotObject2.id));
				}
				session.game.ModifyGameState(new NewExpansionAction(slot.Id, expansionCost, slot.debris, slot.landmarks));
				AnalyticsWrapper.LogExpansion(session.game, slot.Id, expansionCost);
				TFUtils.DebugLog("Purchased Expansion Slot: " + slot.Id);
				session.analytics.LogExpansion(slot.Id, expansionCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				session.ChangeState("Playing", true);
			}
			else
			{
				Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(expansionCost.ResourceAmounts, session.game.resourceManager);
				TFUtils.Assert(resourcesStillRequired.Count > 0, "Error occurred, we have enough resources to apply cost.");
				session.TheSoundEffectManager.PlaySound("Error");
				Action action = delegate()
				{
					session.game.terrain.DropSelection(slot);
					object expandLock2 = Session.expandLock;
					lock (expandLock2)
					{
						if (slot != null)
						{
							slot.inUse = false;
						}
					}
					session.ChangeState("Playing", true);
				};
				Action action2 = delegate()
				{
					object expandLock2 = Session.expandLock;
					lock (expandLock2)
					{
						if (slot != null)
						{
							slot.inUse = false;
						}
					}
					this.PurchaseExpansion(session);
				};
				Action cancelAction = action;
				Action okAction = action2;
				SBGUIConfirmationDialog sbguiconfirmationDialog = (SBGUIConfirmationDialog)session.CheckAsyncRequest("expansion");
				session.AddAsyncResponse("expansion", sbguiconfirmationDialog);
				sbguiconfirmationDialog.SetActive(false);
				session.InsufficientResourcesHandler(session, "Expansion " + slot.Id, slot.Id, okAction, cancelAction, expansionCost);
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0006C8F4 File Offset: 0x0006AAF4
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0006C8F8 File Offset: 0x0006AAF8
		private void ShowDialog(Session session)
		{
			SBGUIConfirmationDialog sbguiconfirmationDialog = (SBGUIConfirmationDialog)session.CheckAsyncRequest("expansion");
			if (null != sbguiconfirmationDialog)
			{
				SBUIBuilder.ReleaseTopScreen();
			}
			TerrainSlot slot = session.game.terrain.selectedSlot;
			TFUtils.Assert(slot != null, "Did not select an Expansion Slot properly!");
			session.game.terrain.HighlightSelection(slot);
			Cost expansionCost = session.game.terrain.GetExpansionCost(slot);
			Action cancelButtonHandler = delegate()
			{
				session.game.terrain.DropSelection(slot);
				session.ChangeState("Playing", true);
			};
			Action okButtonHandler = delegate()
			{
				this.PurchaseExpansion(session);
			};
			SBUIBuilder.MakeAndPushEmptyUI(session, null);
			sbguiconfirmationDialog = SBUIBuilder.MakeAndPushExpansionDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), Language.Get("!!EXPANSION_TITLE"), Language.Get("!!EXPANSION_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_CANCEL"), Cost.DisplayDictionary(expansionCost.ResourceAmounts, session.TheGame.resourceManager), okButtonHandler, cancelButtonHandler, false);
			sbguiconfirmationDialog.SetPosition(session.TheCamera.ScreenCenter);
			session.AddAsyncResponse("expansion", sbguiconfirmationDialog);
			session.game.sessionActionManager.SetActionHandler("expansion_ui", session, new List<SBGUIScreen>
			{
				sbguiconfirmationDialog
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			SessionActionSimulationHelper.EnableHandler(session, true);
			session.soundEffectManager.PlaySound("Dialog_Expansion");
		}
	}

	// Token: 0x020001EB RID: 491
	public class GameStarting : Session.State
	{
		// Token: 0x06001084 RID: 4228 RVA: 0x0006CB0C File Offset: 0x0006AD0C
		private void OnGameCreated(Session session)
		{
			if (this.performedMigration == 3)
			{
				Action okHandler = delegate()
				{
					TFUtils.GotoAppstore();
				};
				this.CreateErrorDialog(session, Language.Get("!!ERROR_ENCOUNTERED_NEWER_PROTOCOL_TITLE"), Language.Get("!!ERROR_ENCOUNTERED_NEWER_PROTOCOL_MESSAGE"), Language.Get("!!PREFAB_OK"), okHandler, this.errorMessageScale, this.errorTitleScale);
				return;
			}
			this.DeferDialogs(session);
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0006CB80 File Offset: 0x0006AD80
		private void DeferDialogs(Session session)
		{
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			this.AdvanceState(session);
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0006CBE0 File Offset: 0x0006ADE0
		public void OnLoadGameDelegate(SoaringContext context)
		{
			this.LOAD_GAME_CONTEXT = context;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x0006CBEC File Offset: 0x0006ADEC
		private void LoadEntityBlueprints(Session session)
		{
			if (!this.contentLoader.LoadNextBlueprint())
			{
				this.CallLoadFromNetwork(session, false);
				this.AdvanceState(session);
			}
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x0006CC10 File Offset: 0x0006AE10
		private void CallLoadFromNetwork(Session session, bool isRetryAttempt = false)
		{
			SoaringContext soaringContext = Game.CreateSoaringGameResponderContext(new SoaringContextDelegate(this.OnLoadGameDelegate));
			if (isRetryAttempt)
			{
				soaringContext.addValue(isRetryAttempt, "retry");
			}
			Game.LoadFromNetwork(session.ThePlayer.playerId, session.ThePlayer.ReadTimestamp(), soaringContext, session);
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x0006CC64 File Offset: 0x0006AE64
		public static void ResetShowSplashScreen(Session.GameStarting.SplashScreenState state)
		{
			YGTextureLibrary library = GUIMainView.GetInstance().Library;
			switch (state)
			{
			case Session.GameStarting.SplashScreenState.Loading:
				library.UnloadAtlasResources("splash_bg_patchy");
				library.LoadAtlasResources("splash_bg");
				library.LoadAtlasResources("localized_logo_en");
				break;
			case Session.GameStarting.SplashScreenState.Patchy:
				library.LoadAtlasResources("splash_bg_patchy");
				library.UnloadAtlasResources("splash_bg");
				if (!library.UnloadAtlasResources("localized_logo_en"))
				{
					library.UnloadAtlasResources(Language.LocalizedEnglishAssetName("localized_logo_en"));
				}
				break;
			case Session.GameStarting.SplashScreenState.None:
				library.UnloadAtlasResources("splash_bg_patchy");
				library.UnloadAtlasResources("splash_bg");
				if (!library.UnloadAtlasResources("localized_logo_en"))
				{
					library.UnloadAtlasResources(Language.LocalizedEnglishAssetName("localized_logo_en"));
				}
				break;
			}
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0006CD38 File Offset: 0x0006AF38
		public static void UnloadSaveGameAtlas()
		{
			GUIMainView.GetInstance().Library.UnloadAtlasResources("SaveGameUI");
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x0006CD50 File Offset: 0x0006AF50
		public static SBGUIScreen CreateLoadingScreen(Session session, bool makeLoadingBar = false, string starting_progress = "starting_progress", bool changeInitState = true)
		{
			bool flag = session.InFriendsGame || session.WasInFriendsGame;
			Session.GameStarting.SplashScreenState splashScreenState = (!flag) ? Session.GameStarting.SplashScreenState.Loading : Session.GameStarting.SplashScreenState.Patchy;
			Session.GameStarting.ResetShowSplashScreen(splashScreenState);
			Action privacyHandler = delegate()
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				Application.OpenURL(TFUtils.GetLegal_Address());
			};
			SBGUIScreen sbguiscreen = SBUIBuilder.MakeAndPushStartingProgress(session, privacyHandler, new Action<SBGUIEvent, Session>(Session.GameStarting.HandleSBGUIEvent), makeLoadingBar, flag);
			if (changeInitState)
			{
				session.GameInitialized(false);
			}
			session.AddAsyncResponse(starting_progress, sbguiscreen);
			SBGUIButton sbguibutton = (SBGUIButton)sbguiscreen.FindChild("privacy_policy");
			sbguibutton.SetActive(makeLoadingBar);
			SBGUILabel sbguilabel = (SBGUILabel)sbguibutton.FindChild("privacy_policy_label");
			sbguilabel.SetText(Language.Get("!!PRIVACY_POLICY"));
			session.AddAsyncResponse("policy_button", sbguibutton);
			SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)sbguiscreen.FindChild("reloading_window");
			SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)sbguiscreen.FindChild("logo");
			SBGUIAtlasImage sbguiatlasImage3 = (SBGUIAtlasImage)sbguiscreen.FindChild("splash");
			SBGUILabel sbguilabel2 = (SBGUILabel)sbguiscreen.FindChild("playerID_label");
			sbguilabel2.SetText(Soaring.Player.UserTag);
			sbguilabel2.SetVisible(!flag && makeLoadingBar);
			if (sbguiatlasImage2 != null)
			{
				sbguiatlasImage2.SetVisible(sbguilabel2.Visible);
			}
			if (makeLoadingBar && !flag)
			{
				Vector2 screenPosition = new Vector2(SBGUI.GetScreenWidth() * 0.15f, SBGUI.GetScreenHeight() * 0.05f);
				sbguilabel2.SetScreenPosition(screenPosition);
			}
			if (splashScreenState == Session.GameStarting.SplashScreenState.Patchy)
			{
				SBGUIAtlasImage sbguiatlasImage4 = (SBGUIAtlasImage)sbguiscreen.FindChild("patchy_splash");
				SBGUILabel sbguilabel3 = (SBGUILabel)sbguiscreen.FindChild("destination_label");
				SBGUIProgressMeter sbguiprogressMeter = (SBGUIProgressMeter)sbguiscreen.FindChild("progress_meter");
				sbguilabel3.SetActive(false);
				float num = 2f / GUIView.ResolutionScaleFactor();
				sbguiatlasImage4.Size = new Vector2(2048f, 1536f);
				Vector2 size = new Vector2(SBGUI.GetScreenWidth(), SBGUI.GetScreenHeight()) * num;
				Vector2 screenPosition2 = sbguiatlasImage4.GetScreenPosition();
				float num2 = size.x / sbguiatlasImage4.Size.x;
				float num3 = 0.24f;
				size.y = sbguiatlasImage4.Size.y * num2;
				float num4 = size.y - SBGUI.GetScreenHeight() * num;
				screenPosition2.y += num4 * num3;
				sbguiatlasImage4.SetScreenPosition(screenPosition2);
				sbguiatlasImage4.Size = size;
				sbguiatlasImage4.SetActive(flag);
				if (session.WasInFriendsGame)
				{
					sbguilabel3.SetActive(flag && sbguiprogressMeter.renderer.enabled);
					sbguilabel3.SetText("           " + Language.Get("!!PATCHY_EXIT"));
				}
				if (session.InFriendsGame)
				{
					sbguilabel3.SetActive(flag && sbguiprogressMeter.renderer.enabled);
					sbguilabel3.SetText("              " + Language.Get("!!PATCHY_ENTER"));
					AnalyticsWrapper.LogUIInteraction(session.TheGame, "ui_visit_patchy", "button", "tap");
				}
				sbguibutton.SetActive(false);
				sbguilabel.SetActive(false);
			}
			if (!flag)
			{
				sbguiatlasImage.SetActive(session.gameIsReloading);
			}
			session.camera.OnUpdate(0.0001f, session);
			return sbguiscreen;
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x0006D0F4 File Offset: 0x0006B2F4
		public void OnEnter(Session session)
		{
			session.canChangeState = false;
			session.InFriendsGame = false;
			session.gameIsReloading = false;
			this.LOAD_GAME_CONTEXT = null;
			Session.GameStarting._CommunityEventSession = null;
			if (!Game.GameExists(session.ThePlayer))
			{
				session.PlayHavenController.RequestContent("first_time_app_start");
			}
			else
			{
				session.PlayHavenController.RequestContent("app_start");
			}
			if (session.properties.playingHud != null)
			{
				session.properties.playingHud.Deactivate();
			}
			SBGUIScreen sbguiscreen = Session.GameStarting.CreateLoadingScreen(session, true, "starting_progress", true);
			this.policyButton = (SBGUIButton)sbguiscreen.FindChild("privacy_policy");
			this.policy_Label = (SBGUILabel)this.policyButton.FindChild("privacy_policy_label");
			this.loadingSpinner = sbguiscreen.FindChild("loading_spinner");
			session.properties.playDelayCounter = 0;
			if (!Soaring.IsOnline)
			{
				SoaringInternal.instance.ClearOfflineMode();
			}
			this.precacheGUIState = 0;
			this.loadTimeDependentsState = 0;
			this.performedMigration = 0;
			int num = 0;
			this.processes = new Session.GameStarting.ProcessStartingProgressState[21];
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.PatchContent);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.AssembleGameState);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.LoadEntityBlueprints);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.CreateGame);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.LoadAssets);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.FetchProductInfo);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.AwaitProductInfo);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.FetchPurchaseInfo);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.AwaitPurchaseInfo);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.StartStore);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.LoadLocalAssetsTerrain);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.LoadLocalAssetsCreateSimulation);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.PrecacheGUI);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.LoadLocalAssetsLoadTimeDependents);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.LoadLocalAssetsSendPendingCommands);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.CreateTerrainMeshes);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.LoadLocalAssetsActivateQuests);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.ProcessTriggers);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.SetupSimulation);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.LoadSoaringCommunityEvents);
			this.processes[num++] = new Session.GameStarting.ProcessStartingProgressState(this.AnalyticsBookkeeping);
			SBGUIActivityIndicator sbguiactivityIndicator = (SBGUIActivityIndicator)sbguiscreen.FindChildSessionActionId("ActivityIndicator", false);
			TFUtils.Assert(sbguiactivityIndicator != null, "ActivityIndicator expected to be valid.");
			sbguiactivityIndicator.Center = new Vector3(4f, -2.7f, 3.2f);
			Application.targetFrameRate = 60;
			sbguiactivityIndicator.StartActivityIndicator();
			this.currentState = -1;
			this.AdvanceState(session);
			if (!Session.didRegisterNotifications)
			{
				try
				{
					Session.RegisterForLocalNotifications();
				}
				catch (Exception ex)
				{
					SoaringDebug.Log("Failed to register user Notifications: " + ex.Message, LogType.Error);
				}
				Session.didRegisterNotifications = true;
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0006D4B4 File Offset: 0x0006B6B4
		public void OnLeave(Session session)
		{
			this.contentLoader = null;
			this.LOAD_GAME_CONTEXT = null;
			if (session.game != null)
			{
				session.game.CanSave = true;
			}
			SBGUIScreen sbguiscreen = (SBGUIScreen)session.CheckAsyncRequest("starting_progress");
			if (sbguiscreen != null)
			{
				SBGUIActivityIndicator sbguiactivityIndicator = (SBGUIActivityIndicator)sbguiscreen.FindChildSessionActionId("ActivityIndicator", false);
				TFUtils.Assert(sbguiactivityIndicator != null, "ActivityIndicator expected to be valid.");
				sbguiactivityIndicator.StopActivityIndicator();
			}
			if (session.gameInitialized)
			{
				NotificationManager.CancelAllNotifications();
				NotificationManager.AddAnnoyingNotifications(session.game);
				session.game.simulation.ClearPendingTimebarsInSimulateds();
				session.game.simulation.ClearPendingNamebarsInSimulateds();
				session.TheCamera.StartCamera();
				session.TheSoundEffectManager.StartSoundEffectsManager();
				session.musicManager.PlayTrack("InGame");
				SBUIBuilder.ReleaseTopScreen();
				session.analytics.UpdateGameValues(session.game);
				AnalyticsWrapper.LogSessionBegin(session.game, 0UL);
				session.TheGame.analytics.LogSessionBegin();
				if (!session.game.featureManager.CheckFeature("unrestrict_clicks"))
				{
					RestrictInteraction.AddWhitelistElement(RestrictInteraction.RESTRICT_ALL_UI_ELEMENT);
					RestrictInteraction.AddWhitelistExpansion(session.game.simulation, int.MinValue);
					RestrictInteraction.AddWhitelistSimulated(session.game.simulation, int.MinValue);
					session.game.tutorialLocked = true;
				}
				if (session.game.featureManager.CheckFeature("purchase_expansions"))
				{
					session.game.terrain.UpdateRealtySigns(session.game.entities.DisplayControllerManager, new BillboardDelegate(SBCamera.BillboardDefinition), session.game);
				}
				if (!session.game.tutorialLocked)
				{
					if (session.game.simulation.FindSimulated(PlayHavenController.PIRATE_BOOTY_SHIP_DID) == null && !session.game.inventory.HasItem(PlayHavenController.PIRATE_BOOTY_SHIP_DID))
					{
						session.PlayHavenController.RequestContent("loading_screen_end_existingplayer_no_ship");
					}
					else
					{
						session.PlayHavenController.RequestContent("loading_screen_end_existingplayer_with_ship");
					}
				}
				this.saveGameScreen = null;
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0006D6E0 File Offset: 0x0006B8E0
		public static void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x0006D6E4 File Offset: 0x0006B8E4
		public void OnUpdate(Session session)
		{
			this.currentAdvance = 1;
			if (this.currentState == 21)
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(session.TheGame.resourceManager.PlayerLevelAmount, "level");
				ulong num = session.TheGame.FirstPlayTime();
				if (num != 0UL)
				{
					soaringDictionary.addValue(num, "first_play_time");
				}
				soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
				Soaring.FireEvent("StartGame", soaringDictionary);
				CommunityEvent activeEvent = session.TheGame.communityEventManager.GetActiveEvent();
				if (activeEvent != null && (activeEvent.m_sID == CommunityEventManager._sSpongyGamesEventID || activeEvent.m_sID == CommunityEventManager._sSpongyGamesLastDayEventID) && (activeEvent.m_nQuestPrereqID < 0 || (session.TheGame.questManager.IsQuestCompleted((uint)activeEvent.m_nQuestPrereqID) && (session.TheGame.simulation.FindSimulated(new int?(CommunityEventManager._nColiseumDID)) != null || session.TheGame.inventory.HasItem(new int?(CommunityEventManager._nColiseumDID))))))
				{
					Soaring.FireEvent("spongy_games_banner", null);
				}
				if (Soaring.Player.PrivateData_Safe != null)
				{
					SoaringArray soaringArray = (SoaringArray)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_completed_quest_key");
					if (soaringArray != null)
					{
						try
						{
							for (int i = 0; i < soaringArray.count(); i++)
							{
								uint did = (uint)((SoaringValue)soaringArray.objectAtIndex(i));
								QuestManager questManager = session.TheGame.questManager;
								if (!questManager.IsQuestCompleted(did) && questManager.IsQuestActive(did))
								{
									questManager.CompleteQuest(questManager.GetQuest(did), session.TheGame);
								}
							}
							soaringArray.clear();
						}
						catch
						{
							SoaringDebug.Log("Failed To Apply Completed Quests: " + soaringArray.ToJsonString());
						}
					}
				}
				if (Soaring.Player.PrivateData_Safe != null)
				{
					SoaringDictionary soaringDictionary2 = (SoaringDictionary)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_reward_key");
					if (soaringDictionary2 != null)
					{
						Dictionary<int, int> dictionary = new Dictionary<int, int>();
						string[] array = soaringDictionary2.allKeys();
						SoaringObjectBase[] array2 = soaringDictionary2.allValues();
						for (int j = 0; j < array.Length; j++)
						{
							if (string.Compare(array[j], "SBMI_friends_coinreward_key") == 0)
							{
								dictionary.Add(ResourceManager.SOFT_CURRENCY, (SoaringValue)array2[j]);
								ResourceManager.AddAmountToGameState(ResourceManager.SOFT_CURRENCY, (SoaringValue)array2[j], session.TheGame.gameState);
							}
							else if (string.Compare(array[j], "SBMI_friends_jellyreward_key") == 0)
							{
								dictionary.Add(ResourceManager.HARD_CURRENCY, (SoaringValue)array2[j]);
								ResourceManager.AddAmountToGameState(ResourceManager.HARD_CURRENCY, (SoaringValue)array2[j], session.TheGame.gameState);
							}
							else if (string.Compare(array[j], "SBMI_friends_xpreward_key") == 0)
							{
								dictionary.Add(ResourceManager.XP, (SoaringValue)array2[j]);
								ResourceManager.AddAmountToGameState(ResourceManager.XP, (SoaringValue)array2[j], session.TheGame.gameState);
							}
						}
						Reward reward = new Reward(dictionary, null, null, null, null, null, null, null, false, null);
						session.TheGame.ApplyReward(reward, TFUtils.EpochTime(), false);
						Soaring.Player.PrivateData_Safe.setValue(new SoaringDictionary(), "SBMI_friends_reward_key");
					}
				}
				SoaringContext soaringContext = new SoaringContext();
				soaringContext.Name = "DailyBonus";
				soaringContext.Responder = new SBMISoaring.SMBICacheDelegate();
				soaringContext.addValue(new SoaringObject(session), "session");
				SBMISoaring.RetrieveDailyBonuseCalendar(-1, soaringContext, new SoaringContextDelegate(this.DisplayDailyBonus));
				session.canChangeState = true;
				session.ChangeState("Playing", true);
				Dictionary<string, object> gameState = session.TheGame.gameState;
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)gameState["farm"];
				List<object> list = null;
				Simulation simulation = session.TheGame.simulation;
				if (dictionary2.ContainsKey("launch_dialogs_shown"))
				{
					list = (List<object>)dictionary2["launch_dialogs_shown"];
				}
				if ((list == null || !list.Contains("christmas_event_over_2013_dialog")) && simulation.FindSimulated(new int?(9042)) != null)
				{
					if (list == null)
					{
						list = new List<object>();
					}
					list.Add("christmas_event_over_2013_dialog");
					Simulated simulated = simulation.FindSimulated(new int?(3049));
					ResidentEntity residentEntity = null;
					if (simulated != null)
					{
						residentEntity = simulated.GetEntity<ResidentEntity>();
					}
					uint sequenceId = 2105U;
					if (residentEntity != null && residentEntity.DisableFlee == true)
					{
						sequenceId = 2104U;
					}
					DialogPackage dialogPackage = session.TheGame.dialogPackageManager.GetDialogPackage(1U);
					List<DialogInputData> dialogInputsInSequence = dialogPackage.GetDialogInputsInSequence(sequenceId, null, null);
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence, sequenceId);
					session.ChangeState("ShowingDialog", true);
					if (dictionary2.ContainsKey("launch_dialogs_shown"))
					{
						dictionary2["launch_dialogs_shown"] = list;
					}
					else
					{
						dictionary2.Add("launch_dialogs_shown", list);
					}
					session.TheGame.CanSave = !session.WasInFriendsGame;
					session.TheGame.SaveLocally(0UL, false, false, true);
				}
				if (dictionary2.ContainsKey("recipes") && (activeEvent == null || (activeEvent.m_sID != CommunityEventManager._sSpongyGamesEventID && activeEvent.m_sID != CommunityEventManager._sSpongyGamesLastDayEventID)))
				{
					Simulated simulated2 = session.TheGame.simulation.FindSimulated(new int?(20400));
					Simulated simulated3 = session.TheGame.simulation.FindSimulated(new int?(20410));
					bool flag = false;
					if (simulated2 != null && simulated3 == null && !session.TheGame.inventory.HasItem(new int?(20410)))
					{
						if (list == null)
						{
							list = new List<object>();
						}
						if (!list.Contains("spongy_games_event_over_2014_dialog"))
						{
							list.Add("spongy_games_event_over_2014_dialog");
							DialogPackage dialogPackage2 = session.TheGame.dialogPackageManager.GetDialogPackage(1U);
							if (dialogPackage2 != null)
							{
								List<DialogInputData> dialogInputsInSequence2 = dialogPackage2.GetDialogInputsInSequence(2306U, null, null);
								if (dialogInputsInSequence2 != null)
								{
									session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence2, 2306U);
									session.ChangeState("ShowingDialog", true);
									if (dictionary2.ContainsKey("launch_dialogs_shown"))
									{
										dictionary2["launch_dialogs_shown"] = list;
									}
									else
									{
										dictionary2.Add("launch_dialogs_shown", list);
									}
									flag = true;
								}
							}
						}
					}
					if (simulated2 != null)
					{
						int[] array3 = new int[]
						{
							9200,
							9300,
							9301,
							9302,
							9303,
							9304
						};
						int num2 = array3.Length;
						List<object> list2 = (List<object>)dictionary2["recipes"];
						for (int k = 0; k < num2; k++)
						{
							if (!session.TheGame.craftManager.IsRecipeUnlocked(array3[k]))
							{
								flag = true;
								session.TheGame.craftManager.UnlockRecipe(array3[k], session.TheGame);
								list2.Add(array3[k]);
							}
						}
					}
					if (flag)
					{
						session.TheGame.CanSave = !session.WasInFriendsGame;
						session.TheGame.SaveLocally(0UL, false, false, true);
					}
				}
				if ((activeEvent == null || activeEvent.m_sID != CommunityEventManager._sChrismas14EventID) && (list == null || !list.Contains("christmas_event_over_2014_dialog")))
				{
					bool flag2 = false;
					if (session.TheGame.questManager.IsQuestCompleted(2800U) && !session.TheGame.questManager.IsQuestCompleted(2842U))
					{
						if (list == null)
						{
							list = new List<object>();
						}
						list.Add("christmas_event_over_2014_dialog");
						flag2 = true;
						DialogPackage dialogPackage3 = session.TheGame.dialogPackageManager.GetDialogPackage(1U);
						List<DialogInputData> dialogInputsInSequence3 = dialogPackage3.GetDialogInputsInSequence(2848U, null, null);
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence3, 2848U);
						session.ChangeState("ShowingDialog", true);
					}
					else if (session.TheGame.questManager.IsQuestCompleted(2842U))
					{
						if (list == null)
						{
							list = new List<object>();
						}
						list.Add("christmas_event_over_2014_dialog");
						flag2 = true;
						DialogPackage dialogPackage4 = session.TheGame.dialogPackageManager.GetDialogPackage(1U);
						List<DialogInputData> dialogInputsInSequence4 = dialogPackage4.GetDialogInputsInSequence(2846U, null, null);
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence4, 2846U);
						session.ChangeState("ShowingDialog", true);
					}
					if (dictionary2.ContainsKey("launch_dialogs_shown"))
					{
						dictionary2["launch_dialogs_shown"] = list;
					}
					else
					{
						dictionary2.Add("launch_dialogs_shown", list);
					}
					if (flag2)
					{
						session.TheGame.CanSave = !session.WasInFriendsGame;
						session.TheGame.SaveLocally(0UL, false, false, true);
					}
				}
				if ((activeEvent == null || activeEvent.m_sID != CommunityEventManager._sValentines15EventID) && (list == null || !list.Contains("valentines_event_over_2015_dialog")))
				{
					bool flag3 = false;
					if (session.TheGame.questManager.IsQuestCompleted(3100U) && !session.TheGame.questManager.IsQuestCompleted(3134U))
					{
						if (list == null)
						{
							list = new List<object>();
						}
						list.Add("valentines_event_over_2015_dialog");
						flag3 = true;
						DialogPackage dialogPackage5 = session.TheGame.dialogPackageManager.GetDialogPackage(1U);
						List<DialogInputData> dialogInputsInSequence5 = dialogPackage5.GetDialogInputsInSequence(3140U, null, null);
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence5, 3140U);
						session.ChangeState("ShowingDialog", true);
					}
					else if (session.TheGame.questManager.IsQuestCompleted(3134U))
					{
						if (list == null)
						{
							list = new List<object>();
						}
						list.Add("valentines_event_over_2015_dialog");
						flag3 = true;
						DialogPackage dialogPackage6 = session.TheGame.dialogPackageManager.GetDialogPackage(1U);
						List<DialogInputData> dialogInputsInSequence6 = dialogPackage6.GetDialogInputsInSequence(3138U, null, null);
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence6, 3138U);
						session.ChangeState("ShowingDialog", true);
					}
					if (dictionary2.ContainsKey("launch_dialogs_shown"))
					{
						dictionary2["launch_dialogs_shown"] = list;
					}
					else
					{
						dictionary2.Add("launch_dialogs_shown", list);
					}
					if (flag3)
					{
						session.TheGame.CanSave = !session.WasInFriendsGame;
						session.TheGame.SaveLocally(0UL, false, false, true);
					}
				}
				return;
			}
			if (this.currentState == 22)
			{
				return;
			}
			SBGUIScreen sbguiscreen = (SBGUIScreen)session.CheckAsyncRequest("starting_progress");
			session.AddAsyncResponse("starting_progress", sbguiscreen);
			this.loadingSpinner.gameObject.transform.Rotate(new Vector3(0f, 0f, 1f), -10f * Time.deltaTime);
			float num3 = ((float)this.currentState + 1f) / 21f;
			if (this.currentState == 0)
			{
				num3 += 0.04761905f * SoaringInternal.instance.Versions.CurrentUpdateProgress();
			}
			sbguiscreen.dynamicMeters["loading"].Progress = num3;
			sbguiscreen.dynamicLabels["progress"].SetText(string.Format("{0}%", ((int)(100f * num3)).ToString()));
			try
			{
				this.processes[this.currentState](session);
			}
			catch (Exception ex)
			{
				if (session.haveReloaded)
				{
					TFUtils.DebugLog(ex.Message + " at: " + ex.StackTrace);
					SoaringDebug.Log("GAME MUST RELOAD CRITICAL: " + ex.Message + " at: " + ex.StackTrace, LogType.Error);
					int errorCode = TFError.GetErrorCode(ex, 200 + this.currentState);
					if (errorCode != 302)
					{
						this.CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), string.Concat(new object[]
						{
							Language.Get("!!ERROR_CORRUPTED_GAMESTATE_MESSAGE"),
							"\n",
							Soaring.Player.UserTag,
							"-",
							errorCode
						}), Language.Get("!!PREFAB_OK"), delegate()
						{
							SBUIBuilder.ReleaseTopScreen();
						}, this.errorMessageScale, this.errorTitleScale);
					}
					else
					{
						this.CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), Language.Get("!!ERROR_302_MESSAGE") + " " + Soaring.Player.UserTag + "!", Language.Get("!!PREFAB_OK"), delegate()
						{
							SBUIBuilder.ReleaseTopScreen();
						}, this.errorMessageScale, this.errorTitleScale);
					}
					this.currentState = 22;
					TFUtils.LogDump(session, "loading_error_" + errorCode, ex, null);
				}
				else
				{
					SoaringDebug.Log("GAME MUST RELOAD: " + ex.Message + " at: " + ex.StackTrace, LogType.Error);
					session.haveReloaded = true;
					if (session.game != null && session.game.actionBuffer != null)
					{
						session.game.actionBuffer.DestroyCache();
					}
					if (SoaringInternal.instance.Versions != null)
					{
						SoaringInternal.instance.Versions.ClearAllContent();
						TFUtils.RefreshSAFiles();
					}
					else if (Directory.Exists(TFUtils.GetPersistentAssetsPath()))
					{
						Directory.Delete(TFUtils.GetPersistentAssetsPath(), true);
					}
					session.canChangeState = true;
					session.ChangeState("Authorizing", true);
				}
			}
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x0006E5B0 File Offset: 0x0006C7B0
		private void DisplayDailyBonus(SoaringContext context)
		{
			Session session = null;
			SoaringError soaringError = null;
			bool flag = false;
			if (Soaring.IsOnline && context != null)
			{
				flag = context.soaringValue("query");
				if (flag)
				{
					SoaringObjectBase soaringObjectBase = context.objectWithKey("session");
					if (soaringObjectBase != null)
					{
						session = (Session)((SoaringObject)soaringObjectBase).Object;
					}
					else
					{
						flag = false;
					}
				}
			}
			if (context != null)
			{
				SoaringObjectBase soaringObjectBase2 = context.objectWithKey("error_code");
				if (soaringObjectBase2 != null)
				{
					soaringError = (SoaringError)soaringObjectBase2;
				}
			}
			if (flag)
			{
				DailyBonusDialogInputData dailyBonusDialogInputData = new DailyBonusDialogInputData();
				if (!dailyBonusDialogInputData.AlreadyCollected && session.TheGame != null && session.TheGame.dialogPackageManager != null)
				{
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
					{
						dailyBonusDialogInputData
					}, uint.MaxValue);
					session.ChangeState("ShowingDialog", true);
				}
			}
			else
			{
				int num = -1;
				if (soaringError != null)
				{
					num = soaringError.ErrorCode;
				}
				Debug.Log("TODO: HANDLE THE ERROR CODE: " + num.ToString());
			}
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x0006E6CC File Offset: 0x0006C8CC
		private void AdvanceState(Session session)
		{
			lock (this)
			{
				this.currentState += this.currentAdvance;
				this.currentAdvance = 0;
			}
			session.analytics.LogLoadingFunnelStep("LoadingScreen_" + this.currentState.ToString());
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x0006E744 File Offset: 0x0006C944
		public void CRITICAL_ERROR_ALL_GAMES_CORRUPTED(Session session, Exception e)
		{
			this.LOAD_GAME_CONTEXT = null;
			Action okHandler = delegate()
			{
				SBUIBuilder.ReleaseTopScreen();
			};
			int game_error_code = TFError.GetErrorCode(e, 250 + this.currentState);
			Action cancelHandler = delegate()
			{
				SoaringPlatform.SendEmail(Soaring.Player.UserID + "-" + game_error_code, string.Empty, Language.Get("!!CUSTOMER_SERVICE_EMAIL"));
			};
			if (game_error_code != 302)
			{
				this.CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), this.WithErrorID(Language.Get("!!ERROR_CORRUPTED_GAMESTATE_MESSAGE") + "\n" + Soaring.Player.UserTag, game_error_code), Language.Get("!!PREFAB_OK"), okHandler, Language.Get("!!SEND_EMAIL"), cancelHandler, this.errorMessageScale, this.errorTitleScale);
			}
			else
			{
				this.CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), this.WithErrorID(Language.Get("!!ERROR_302_MESSAGE") + " " + Soaring.Player.UserTag + "!", game_error_code), Language.Get("!!PREFAB_OK"), okHandler, Language.Get("!!SEND_EMAIL"), cancelHandler, this.errorMessageScale, this.errorTitleScale);
			}
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0006E878 File Offset: 0x0006CA78
		public string WithErrorID(string message, int errorID)
		{
			return message + " - " + errorID.ToString();
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x0006E88C File Offset: 0x0006CA8C
		private bool CheckServerGameWithSession(Session session, bool canSave)
		{
			bool result = true;
			if (session.game != null)
			{
				session.game.ClearActionBuffer();
				bool canSave2 = session.game.CanSave;
				session.game.CanSave = (canSave && !session.WasInFriendsGame);
				session.game.SaveLocally(0UL, false, false, true);
				session.TheGame.CanSave = canSave2;
			}
			else
			{
				session.player.DeleteTimestamp();
				result = false;
			}
			return result;
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x0006E90C File Offset: 0x0006CB0C
		private void CreateGame(Session session)
		{
			if (this.LOAD_GAME_CONTEXT != null)
			{
				bool flag = this.LOAD_GAME_CONTEXT.soaringValue("status");
				SoaringArray soaringArray = (SoaringArray)this.LOAD_GAME_CONTEXT.objectWithKey("custom");
				bool flag2 = this.LOAD_GAME_CONTEXT.soaringValue("retry");
				this.LOAD_GAME_CONTEXT = null;
				SoaringDictionary soaringDictionary = null;
				bool flag3 = false;
				if (Game.GameCacheExists(SoaringPlatform.DeviceID))
				{
					flag3 = true;
				}
				if (soaringArray != null && soaringArray.count() != 0)
				{
					soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(0);
				}
				if (soaringDictionary != null)
				{
					Dictionary<string, object> gameServer = null;
					if (SBSettings.UseLegacyGameLoad)
					{
						TFUtils.DebugLog("Loading Game In Legacy Mode");
						gameServer = (Dictionary<string, object>)Json.Deserialize(soaringDictionary.ToJsonString());
					}
					else
					{
						gameServer = SBMISoaring.ConvertDictionaryToGeneric(soaringDictionary);
					}
					Game gameLocal = null;
					try
					{
						if (flag3)
						{
							try
							{
								string text = Game.GameCachePath(SoaringPlatform.DeviceID);
								string text2 = Game.GamePath(session.ThePlayer);
								string directoryName = Path.GetDirectoryName(text2);
								if (!Directory.Exists(directoryName))
								{
									Directory.CreateDirectory(directoryName);
								}
								File.Copy(text, text2, true);
								File.Delete(text);
							}
							catch (Exception ex)
							{
								Debug.LogError("Data Failed Copy Offline Game: " + ex.Message + "\n" + ex.StackTrace);
							}
						}
						gameLocal = Game.LoadFromCache(session.ThePlayer, session.Analytics, this.contentLoader, out this.performedMigration, session.PlayHavenController);
					}
					catch (Exception ex2)
					{
						Debug.Log("Data Failed To Load Local Game: " + ex2.Message);
						gameLocal = null;
					}
					bool flag4 = session.InFriendsGame || session.WasInFriendsGame;
					if (gameLocal != null && !flag4)
					{
						TFUtils.GameDetails gameDetails = null;
						TFUtils.GameDetails gameDetails2 = null;
						TFUtils.DebugLog("ServerGame: " + TFUtils.ParseGameDetails(gameServer, ref gameDetails));
						TFUtils.DebugLog("LocalGame: " + TFUtils.ParseGameDetails(gameLocal.gameState, ref gameDetails2));
						if (gameDetails.lastPlayTime == gameDetails2.lastPlayTime)
						{
							Debug.Log("Games are Identical, Use Local");
							session.game = gameLocal;
							session.player.SaveStagedTimestamp();
							this.OnGameCreated(session);
							return;
						}
						Action server = delegate()
						{
							this.saveGameScreen.SetActive(false);
							Action okButtonHandler = delegate()
							{
								SBUIBuilder.ReleaseTopScreen();
								this.saveGameScreen.Deactivate();
								try
								{
									session.game = Game.LoadFromDataDict(gameServer, session.Analytics, session.ThePlayer, this.contentLoader, out this.performedMigration, session.PlayHavenController);
									this.CheckServerGameWithSession(session, true);
								}
								catch (Exception ex8)
								{
									Debug.Log("Data Failed To Load: Using ServerGame: " + ex8.Message);
								}
								this.policyButton.SetActive(true);
								this.policy_Label.SetText(Language.Get("!!PRIVACY_POLICY"));
								this.OnGameCreated(session);
							};
							Action cancelButtonHandler = delegate()
							{
								SBUIBuilder.ReleaseTopScreen();
								this.saveGameScreen.SetActive(true);
							};
							string message = Language.Get("!!SAVE_ALERT_REPLACE_1");
							SBGUIConfirmationDialog sbguiconfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), message, Language.Get("!!SAVE_GAME_YES"), Language.Get("!!SAVE_GAME_NO"), null, okButtonHandler, cancelButtonHandler, false);
							sbguiconfirmationDialog.FindChild("title_label").SetActive(false);
							sbguiconfirmationDialog.FindChild("message_label").transform.localScale *= 1.5f;
							Vector3 localPosition = sbguiconfirmationDialog.FindChild("message_label").transform.localPosition;
							sbguiconfirmationDialog.FindChild("message_label").transform.localPosition = new Vector3(localPosition.x, localPosition.y - 0.5f, localPosition.z);
							sbguiconfirmationDialog.transform.FindChild("window").FindChild("titlebackground").gameObject.SetActive(false);
						};
						Action local = delegate()
						{
							this.saveGameScreen.SetActive(false);
							Action okButtonHandler = delegate()
							{
								SBUIBuilder.ReleaseTopScreen();
								this.saveGameScreen.Deactivate();
								if (gameLocal != null)
								{
									session.game = gameLocal;
								}
								else
								{
									try
									{
										session.game = Game.LoadFromCache(session.ThePlayer, session.Analytics, this.contentLoader, out this.performedMigration, session.PlayHavenController);
									}
									catch (Exception ex8)
									{
										Debug.LogError("Data Failed To Load: Using ServerGame: " + ex8.Message);
									}
								}
								this.policyButton.SetActive(true);
								this.policy_Label.SetText(Language.Get("!!PRIVACY_POLICY"));
								this.OnGameCreated(session);
							};
							Action cancelButtonHandler = delegate()
							{
								SBUIBuilder.ReleaseTopScreen();
								this.saveGameScreen.SetActive(true);
							};
							string message = Language.Get("!!SAVE_ALERT_REPLACE_2");
							SBGUIConfirmationDialog sbguiconfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), message, Language.Get("!!SAVE_GAME_YES"), Language.Get("!!SAVE_GAME_NO"), null, okButtonHandler, cancelButtonHandler, false);
							sbguiconfirmationDialog.FindChild("title_label").SetActive(false);
							sbguiconfirmationDialog.FindChild("message_label").transform.localScale *= 1.5f;
							Vector3 localPosition = sbguiconfirmationDialog.FindChild("message_label").transform.localPosition;
							sbguiconfirmationDialog.FindChild("message_label").transform.localPosition = new Vector3(localPosition.x, localPosition.y - 0.5f, localPosition.z);
							sbguiconfirmationDialog.transform.FindChild("window").FindChild("titlebackground").gameObject.SetActive(false);
						};
						if (this.saveGameScreen == null)
						{
							session.player.DeleteTimestamp();
							this.saveGameScreen = (SaveGameScreen)SBGUI.InstantiatePrefab("Prefabs/SaveGame");
							this.saveGameScreen.SetUp(TFUtils.GetPlayerName(Soaring.Player, "{0} ") + Language.Get("!!SAVE_GAME_TEXT1"), Language.Get("!!SAVE_GAME_TEXT2"), "                    " + Language.Get("!!SAVE_GAME_ALERT1"), Language.Get("!!SAVE_GAME_SAVE_ON_SERVER"), gameDetails.level, gameDetails.money, gameDetails.jelly, gameDetails.patties, gameDetails.dtLastPlayTime, Language.Get("!!SAVE_GAME_KEEP_THIS_GAME"), Language.Get("!!SAVE_GAME_SAVE_ON_DEVICE"), gameDetails2.level, gameDetails2.money, gameDetails2.jelly, gameDetails2.patties, gameDetails2.dtLastPlayTime, Language.Get("!!SAVE_GAME_KEEP_THIS_GAME"), server, local, session);
							this.policyButton.SetActive(false);
						}
					}
					else
					{
						try
						{
							session.game = Game.LoadFromDataDict(gameServer, session.Analytics, session.ThePlayer, this.contentLoader, out this.performedMigration, session.PlayHavenController);
							this.CheckServerGameWithSession(session, true);
							this.OnGameCreated(session);
						}
						catch (Exception ex3)
						{
							Debug.LogError("Data Failed To Load: Using ServerGame: " + ex3.Message + "\n" + ex3.StackTrace);
							this.CRITICAL_ERROR_ALL_GAMES_CORRUPTED(session, ex3);
							TFUtils.LogDump(session, "save_error", ex3, null);
						}
					}
				}
				else
				{
					bool flag5 = false;
					Exception ex4 = null;
					if (Game.GameExists(session.ThePlayer))
					{
						TFUtils.DebugLog("Creating game from local file");
						try
						{
							session.game = Game.LoadFromCache(session.ThePlayer, session.Analytics, this.contentLoader, out this.performedMigration, session.PlayHavenController);
							this.OnGameCreated(session);
						}
						catch (Exception ex5)
						{
							TFUtils.DebugLog(ex5);
							if (Soaring.IsOnline)
							{
								session.player.DeleteTimestamp();
								if (flag2)
								{
									flag5 = true;
								}
								else
								{
									this.CallLoadFromNetwork(session, true);
								}
							}
							else
							{
								flag5 = true;
							}
							ex4 = ex5;
						}
					}
					else if (flag || Soaring.Player.IsLocalAuthorized)
					{
						TFUtils.WarningLog("No Save Game Found");
						if (!Player.ValidTimeStamp(session.player.ReadTimestamp()))
						{
							bool flag6 = false;
							if (flag3)
							{
								try
								{
									string text3 = Game.GameCachePath(SoaringPlatform.DeviceID);
									string text4 = Game.GamePath(session.ThePlayer);
									string directoryName2 = Path.GetDirectoryName(text4);
									if (!Directory.Exists(directoryName2))
									{
										Directory.CreateDirectory(directoryName2);
									}
									File.Copy(text3, text4, true);
									File.Delete(text3);
								}
								catch (Exception ex6)
								{
									Debug.LogError("Data Failed Copy Offline Game: " + ex6.Message + "\n" + ex6.StackTrace);
								}
								try
								{
									session.game = Game.LoadFromCache(session.ThePlayer, session.Analytics, this.contentLoader, out this.performedMigration, session.PlayHavenController);
									this.OnGameCreated(session);
									flag6 = true;
								}
								catch (Exception ex7)
								{
									Debug.LogError("Data Failed Load Copied Offline Game: " + ex7.Message + "\n" + ex7.StackTrace);
								}
							}
							if (!flag6)
							{
								session.game = Game.CreateNew(session.analytics, session.player, this.contentLoader, out this.performedMigration, session.PlayHavenController);
								this.OnGameCreated(session);
							}
						}
						else
						{
							session.player.DeleteTimestamp();
							if (flag2)
							{
								flag5 = true;
							}
							else
							{
								this.CallLoadFromNetwork(session, true);
							}
						}
					}
					else if (!Soaring.IsOnline)
					{
						TFUtils.WarningLog("Local Authorized: " + Soaring.Player.IsLocalAuthorized);
						TFUtils.DebugLog("Critical Game Is Offling, No Local Data");
						flag5 = true;
					}
					else
					{
						TFUtils.DebugLog("Critical Error No Games to Load, How did we get here!!!!");
					}
					if (flag5)
					{
						Action okHandler = delegate()
						{
							SBUIBuilder.ReleaseTopScreen();
							this.CallLoadFromNetwork(session, true);
						};
						int num = 302;
						if (ex4 != null)
						{
							num = TFError.GetErrorCode(ex4, num);
						}
						if (num != 302)
						{
							this.CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), this.WithErrorID(Language.Get("!!ERROR_CORRUPTED_GAMESTATE_MESSAGE") + "\n" + Soaring.Player.UserTag, num), Language.Get("!!PREFAB_OK"), okHandler, this.errorMessageScale, this.errorTitleScale);
						}
						else
						{
							this.CreateErrorDialog(session, Language.Get("!!ERROR_CORRUPTED_GAMESTATE_TITLE"), this.WithErrorID(Language.Get("!!ERROR_302_MESSAGE") + " " + Soaring.Player.UserTag + "!", num), Language.Get("!!PREFAB_OK"), okHandler, this.errorMessageScale, this.errorTitleScale);
						}
					}
				}
			}
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0006F21C File Offset: 0x0006D41C
		private void LoadSoaringCommunityEvents(Session session)
		{
			if (Session.GameStarting._CommunityEventSession != null)
			{
				return;
			}
			Session.GameStarting._CommunityEventSession = session;
			Session.GameStarting._CommunityEventIndex = 0;
			Session.GameStarting._CommunityEvents = session.game.communityEventManager.GetEvents();
			int num = Session.GameStarting._CommunityEvents.Length;
			if (num <= 0)
			{
				this.AdvanceState(session);
				return;
			}
			CommunityEvent communityEvent = Session.GameStarting._CommunityEvents[Session.GameStarting._CommunityEventIndex];
			SoaringCommunityEventManager.SetValueFinished += this.HandleSetValueFinished;
			SBMISoaring.SetEventValue(session, int.Parse(communityEvent.m_sID), session.TheGame.resourceManager.Resources[communityEvent.m_nValueID].Amount, null);
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x0006F2C8 File Offset: 0x0006D4C8
		private void HandleSetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
		{
			int num = 0;
			if (Session.GameStarting._CommunityEvents != null)
			{
				num = Session.GameStarting._CommunityEvents.Length;
			}
			Session.GameStarting._CommunityEventIndex++;
			if (Session.GameStarting._CommunityEventIndex < num)
			{
				CommunityEvent communityEvent = Session.GameStarting._CommunityEvents[Session.GameStarting._CommunityEventIndex];
				SBMISoaring.SetEventValue(Session.GameStarting._CommunityEventSession, int.Parse(communityEvent.m_sID), Session.GameStarting._CommunityEventSession.TheGame.resourceManager.Resources[communityEvent.m_nValueID].Amount, null);
			}
			else
			{
				SoaringCommunityEventManager.SetValueFinished -= this.HandleSetValueFinished;
				Session.GameStarting._CommunityEventIndex = 0;
				Session.GameStarting._CommunityEvents = null;
				this.AdvanceState(Session.GameStarting._CommunityEventSession);
				Session.GameStarting._CommunityEventSession = null;
			}
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0006F384 File Offset: 0x0006D584
		private bool dataIsChange(string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local)
		{
			return level_server != level_local || money_server != money_local || jelly_server != jelly_local || patty_server != patty_local || timeStamp_server != timeStamp_local;
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x0006F3D4 File Offset: 0x0006D5D4
		private void LoadAssets(Session session)
		{
			TFUtils.Assert(session.game != null, "SessionGameStarting.LoadAssets() expects session.game to not be null");
			this.contentLoader.TheEntityManager.LoadBlueprintResources();
			bool flag = false;
			if (session.game.store == null)
			{
				flag = true;
			}
			else if (!session.game.store.receivedProductInfo)
			{
				flag = true;
				session.game.store.Reset(session);
				session.game.store = null;
			}
			if (flag)
			{
				this.currentState = 5;
			}
			else
			{
				this.currentState = 17;
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0006F470 File Offset: 0x0006D670
		private void CreateTerrainMeshes(Session session)
		{
			session.game.terrain.CreateTerrainMeshes();
			this.AdvanceState(session);
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0006F48C File Offset: 0x0006D68C
		private void FetchProductInfo(Session session)
		{
			session.game.store = RmtStore.LoadFromFilesystem(true);
			session.game.store.Init(session);
			if (!RmtStore.PreloadRmtProducts(session))
			{
				session.game.store.rmtEnabled = false;
			}
			session.PlayHavenController.Initialize(session);
			this.AdvanceState(session);
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0006F4EC File Offset: 0x0006D6EC
		private void AwaitProductInfo(Session session)
		{
			if (session.TheGame.store.receivedProductInfo)
			{
				this.AdvanceState(session);
			}
			else if (!session.game.store.rmtEnabled)
			{
				TFUtils.DebugLog("Skipping process product info, since premium is not supported");
				this.AdvanceState(session);
			}
			else
			{
				TFUtils.DebugLogTimed("Waiting for product info");
				this.elapsedProductInfoTime += Time.deltaTime;
				if (this.elapsedProductInfoTime > 15f || !Soaring.IsOnline)
				{
					TFUtils.WarningLog("Timed out on store request. Disabling store");
					this.AdvanceState(session);
				}
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x0006F58C File Offset: 0x0006D78C
		private void ProcessTriggers(Session session)
		{
			session.game.dropManager.ExecuteAllPickupTriggers(session.game);
			this.AdvanceState(session);
		}

		// Token: 0x0600109E RID: 4254 RVA: 0x0006F5AC File Offset: 0x0006D7AC
		private void SetupSimulation(Session session)
		{
			if (!session.gameInitialized)
			{
				Resources.UnloadUnusedAssets();
				Session.GameStarting.UnloadSaveGameAtlas();
				session.GameInitialized(true);
				session.PlayHavenController.RequestContent("loading_screen_end");
				session.game.simulation.OnUpdate(session);
				session.game.treasureManager.StartTreasureTimers();
			}
			else if (!session.game.needsNetworkDownErrorDialog)
			{
				this.AdvanceState(session);
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0006F628 File Offset: 0x0006D828
		private void AnalyticsBookkeeping(Session session)
		{
			session.game.playtimeRegistrar.UpdatePlaytime(TFUtils.EpochTime());
			session.analytics.LogStartedPlaying(session.game.resourceManager.PlayerLevelAmount);
			uint item = 2000U;
			if (session.TheGame.questManager.CompletedQuestDids.Contains(item))
			{
				session.analytics.LogEligiblePromoEvent(session.game.resourceManager.PlayerLevelAmount, "2013_Halloween");
			}
			if (Application.platform == RuntimePlatform.Android)
			{
				Screen.sleepTimeout = -2;
			}
			this.AdvanceState(session);
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0006F6C0 File Offset: 0x0006D8C0
		private void PatchContent(Session session)
		{
			if (!session.IsPatchingInProgress())
			{
				this.AdvanceState(session);
			}
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0006F6D4 File Offset: 0x0006D8D4
		public void PatchingEventListener(string patchingEvent, Session session)
		{
			if (this.currentState == 0)
			{
				if ("patchingDone" == patchingEvent)
				{
					TFUtils.DebugLog("Patching is done. Proceeding.");
					this.AdvanceState(session);
				}
				else
				{
					TFUtils.DebugLog("Patching is in progress with status " + patchingEvent);
				}
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0006F724 File Offset: 0x0006D924
		private void AssembleGameState(Session session)
		{
			SBSettings.Init();
			SBUIBuilder.ClearScreenCache();
			if (Language.CurrentLanguage() == LanguageCode.N)
			{
				Language.Init(TFUtils.GetPersistentAssetsPath());
			}
			if (!Session.GameStarting.didOpenUpdateDialog)
			{
				if (SBSettings.LOCAL_BUNDLE_VERSION.Major < SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Major || (SBSettings.LOCAL_BUNDLE_VERSION.Major == SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Major && SBSettings.LOCAL_BUNDLE_VERSION.Minor < SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Minor))
				{
					Session.GameStarting.didOpenUpdateDialog = true;
					TFUtils.DebugLog("Will force app update");
					Action okHandler = delegate()
					{
						TFUtils.GotoAppstore();
					};
					this.CreateErrorDialog(session, Language.Get("!!NEW_APP_AVAILABLE_TITLE"), Language.Get("!!NEW_APP_FORCE_UPGRADE_MESSAGE"), Language.Get("!!PREFAB_OK"), okHandler, 1f, 0.5f);
				}
				else if (SBSettings.LOCAL_BUNDLE_VERSION.Major == SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Major && SBSettings.LOCAL_BUNDLE_VERSION.Minor == SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Minor && SBSettings.LOCAL_BUNDLE_VERSION.Build < SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION.Build)
				{
					Version lastPromptedAppstoreVersion = SBSettings.LastPromptedAppstoreVersion;
					if (!lastPromptedAppstoreVersion.Equals(SBSettings.CURRENT_APPSTORE_BUNDLE_VERSION))
					{
						Session.GameStarting.didOpenUpdateDialog = true;
						Action okButtonHandler = delegate()
						{
							SBSettings.SaveLastPromptedAppstoreVersion();
							SBUIBuilder.ReleaseTopScreen();
							TFUtils.GotoAppstore();
							Session.GameStarting.didOpenUpdateDialog = false;
						};
						Action cancelButtonHandler = delegate()
						{
							SBSettings.SaveLastPromptedAppstoreVersion();
							SBUIBuilder.ReleaseTopScreen();
							Session.GameStarting.didOpenUpdateDialog = false;
						};
						TFUtils.DebugLog("Will suggest app update");
						SBGUIConfirmationDialog sbguiconfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!NEW_APP_AVAILABLE_TITLE"), Language.Get("!!NEW_APP_SUGGEST_UPGRADE_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_CANCEL"), new Dictionary<string, int>(), okButtonHandler, cancelButtonHandler, false);
						sbguiconfirmationDialog.tform.parent = GUIMainView.GetInstance().transform;
						sbguiconfirmationDialog.tform.localPosition = Vector3.zero;
					}
				}
				SBGUIButton sbguibutton = (SBGUIButton)session.CheckAsyncRequest("policy_button");
				if (sbguibutton != null)
				{
					SBGUILabel sbguilabel = (SBGUILabel)sbguibutton.FindChild("privacy_policy_label");
					sbguilabel.SetText(Language.Get("!!PRIVACY_POLICY"));
					float num = 4.5f;
					((SBGUIImage)sbguibutton.FindChild("hyperlink_image")).Size = new Vector2(((float)sbguilabel.Text.Length + 1f) * num, 2f);
					if (!session.InFriendsGame && !session.WasInFriendsGame)
					{
						sbguibutton.SetActive(true);
					}
				}
			}
			if (Session.GameStarting.didOpenUpdateDialog)
			{
				return;
			}
			this.contentLoader = new StaticContentLoader();
			this.contentLoader.LoadContent(session);
			session.DropGame();
			this.AdvanceState(session);
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0006F9E4 File Offset: 0x0006DBE4
		private void FetchPurchaseInfo(Session session)
		{
			TFUtils.DebugLog("FetchPurchaseInfo");
			session.game.store.GetPurchases(session);
			this.AdvanceState(session);
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0006FA14 File Offset: 0x0006DC14
		private void AwaitPurchaseInfo(Session session)
		{
			if (session.game.store.receivedPurchaseInfo)
			{
				this.AdvanceState(session);
			}
			else
			{
				this.elapsedPurchaseInfoTime += Time.deltaTime;
				if (this.elapsedPurchaseInfoTime > 15f || !Soaring.IsOnline)
				{
					TFUtils.WarningLog("Timed out on store request.");
					this.AdvanceState(session);
				}
			}
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0006FA80 File Offset: 0x0006DC80
		private void StartStore(Session session)
		{
			session.game.store.Start();
			this.AdvanceState(session);
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0006FA9C File Offset: 0x0006DC9C
		private void LoadLocalAssetsTerrain(Session session)
		{
			this.contentLoader.Initialize();
			this.AdvanceState(session);
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0006FAB0 File Offset: 0x0006DCB0
		private void LoadLocalAssetsCreateSimulation(Session session)
		{
			TFUtils.DebugLog("Creating simulation");
			Action<Simulated> rushSimulated = delegate(Simulated sim)
			{
				session.game.selected = sim;
				session.ChangeState("SelectedPlaying", false);
				session.ChangeState("HardSpendConfirm", false);
			};
			session.game.simulation = new Simulation(new Simulation.ModifyGameStateFunction(session.game.ModifyGameState), new Simulation.ModifyGameStateSimulatedFunction(session.game.ModifyGameStateSimulated), rushSimulated, new Simulation.RecordBufferAction(session.game.actionBuffer.Record), session.game, session.game.entities, session.game.triggerRouter, session.game.resourceManager, session.game.dropManager, session.TheSoundEffectManager, session.game.resourceCalculatorManager, session.game.craftManager, session.game.movieManager, session.game.featureManager, session.game.catalog, session.game.rewardCap, session.camera.UnityCamera, session.game.terrain, 5, session.analytics, session.simulationScratchScreen, this.contentLoader.TheEnclosureManager);
			this.AdvanceState(session);
		}

		// Token: 0x060010A8 RID: 4264 RVA: 0x0006FC44 File Offset: 0x0006DE44
		private void PrecacheGUI(Session session)
		{
			TFUtils.DebugLogTimed("In PrecacheGUI");
			int num = this.precacheGUIState;
			if (num != 0)
			{
				this.AdvanceState(session);
			}
			else
			{
				SBGUIRewardWidget.MakeRewardWidgetPool();
				this.precacheGUIState++;
			}
		}

		// Token: 0x060010A9 RID: 4265 RVA: 0x0006FC94 File Offset: 0x0006DE94
		private void LoadLocalAssetsLoadTimeDependents(Session session)
		{
			if (Session.loggingTimedDependents)
			{
				TFUtils.DebugLogTimed("In LoadLocalAssetsLoadTimeDependents. State=" + this.loadTimeDependentsState);
			}
			ulong utcNow = TFUtils.EpochTime();
			switch (this.loadTimeDependentsState)
			{
			case 0:
				session.game.LoadSimulation(utcNow);
				this.loadTimeDependentsState++;
				break;
			case 1:
				if (session.game.IterateLoadSimulation())
				{
					this.loadTimeDependentsState++;
				}
				break;
			case 2:
				session.game.LoadExpansions(utcNow);
				this.loadTimeDependentsState++;
				break;
			case 3:
				if (session.game.IterateLoadExpansions())
				{
					this.loadTimeDependentsState++;
				}
				break;
			default:
			{
				bool canSave = session.game.CanSave;
				if (!session.WasInFriendsGame && !session.InFriendsGame)
				{
					session.game.CanSave = true;
				}
				session.game.SaveToServer(session, TFUtils.EpochTime(), true, this.performedMigration == 2);
				session.game.CanSave = canSave;
				this.AdvanceState(session);
				break;
			}
			}
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0006FDD4 File Offset: 0x0006DFD4
		private void LoadLocalAssetsSendPendingCommands(Session session)
		{
			session.game.simulation.SendPendingCommands();
			this.AdvanceState(session);
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0006FDF0 File Offset: 0x0006DFF0
		private void LoadLocalAssetsActivateQuests(Session session)
		{
			TFUtils.DebugLog("GAME INITIALIZED");
			session.game.questManager.Activate(session.game);
			if (session.game.dialogPackageManager.GetNumQueuedDialogInputs() > 0)
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			}
			this.AdvanceState(session);
		}

		// Token: 0x060010AC RID: 4268 RVA: 0x0006FE4C File Offset: 0x0006E04C
		private void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, float messageScale, float titleScale)
		{
			this.CreateErrorDialog(session, title, message, okButtonLabel, okHandler, null, null, messageScale, titleScale);
		}

		// Token: 0x060010AD RID: 4269 RVA: 0x0006FE6C File Offset: 0x0006E06C
		private void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, string cancelButtonLabel, Action cancelHandler, float messageScale, float titleScale)
		{
			Action androidOk = delegate()
			{
				okHandler();
			};
			Action okButtonHandler = delegate()
			{
				okHandler();
				AndroidBack.getInstance().pop(androidOk);
			};
			Action cancelButtonHandler = delegate()
			{
				cancelHandler();
				AndroidBack.getInstance().pop(androidOk);
			};
			if (cancelHandler == null || cancelButtonLabel == null)
			{
				cancelButtonHandler = null;
				cancelButtonLabel = null;
			}
			SBGUIConfirmationDialog sbguiconfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, title, message, okButtonLabel, cancelButtonLabel, null, okButtonHandler, cancelButtonHandler, false);
			AndroidBack.getInstance().push(androidOk, sbguiconfirmationDialog);
			SBGUILabel sbguilabel = (SBGUILabel)sbguiconfirmationDialog.FindChild("message_label");
			YGTextAtlasSprite component = sbguilabel.GetComponent<YGTextAtlasSprite>();
			component.scale = new Vector2(messageScale, messageScale);
			SBGUILabel sbguilabel2 = (SBGUILabel)sbguiconfirmationDialog.FindChild("title_label");
			YGTextAtlasSprite component2 = sbguilabel2.GetComponent<YGTextAtlasSprite>();
			component2.scale = new Vector2(titleScale, titleScale);
			sbguiconfirmationDialog.tform.parent = GUIMainView.GetInstance().transform;
			sbguiconfirmationDialog.tform.localPosition = Vector3.zero;
		}

		// Token: 0x04000B82 RID: 2946
		private const string STARTING_PROGRESS = "starting_progress";

		// Token: 0x04000B83 RID: 2947
		private const string POLICY_BUTTON = "policy_button";

		// Token: 0x04000B84 RID: 2948
		private SoaringContext LOAD_GAME_CONTEXT;

		// Token: 0x04000B85 RID: 2949
		private SaveGameScreen saveGameScreen;

		// Token: 0x04000B86 RID: 2950
		private float elapsedProductInfoTime;

		// Token: 0x04000B87 RID: 2951
		private float elapsedPurchaseInfoTime;

		// Token: 0x04000B88 RID: 2952
		private int currentState = -1;

		// Token: 0x04000B89 RID: 2953
		private Session.GameStarting.ProcessStartingProgressState[] processes;

		// Token: 0x04000B8A RID: 2954
		private float errorMessageScale = 0.85f;

		// Token: 0x04000B8B RID: 2955
		private float errorTitleScale = 0.45f;

		// Token: 0x04000B8C RID: 2956
		private int currentAdvance = 1;

		// Token: 0x04000B8D RID: 2957
		private SBGUIButton policyButton;

		// Token: 0x04000B8E RID: 2958
		private SBGUILabel policy_Label;

		// Token: 0x04000B8F RID: 2959
		private int precacheGUIState;

		// Token: 0x04000B90 RID: 2960
		private int loadTimeDependentsState;

		// Token: 0x04000B91 RID: 2961
		private StaticContentLoader contentLoader;

		// Token: 0x04000B92 RID: 2962
		private int performedMigration;

		// Token: 0x04000B93 RID: 2963
		private SBGUIElement loadingSpinner;

		// Token: 0x04000B94 RID: 2964
		private static int _CommunityEventIndex;

		// Token: 0x04000B95 RID: 2965
		private static CommunityEvent[] _CommunityEvents;

		// Token: 0x04000B96 RID: 2966
		private static Session _CommunityEventSession;

		// Token: 0x04000B97 RID: 2967
		private static bool didOpenUpdateDialog;

		// Token: 0x020001EC RID: 492
		private enum GameStartingState
		{
			// Token: 0x04000BA0 RID: 2976
			STATE_FIRST = -1,
			// Token: 0x04000BA1 RID: 2977
			STATE_PATCHING_CONTENT,
			// Token: 0x04000BA2 RID: 2978
			STATE_ASSEMBLE_GAME_STATE,
			// Token: 0x04000BA3 RID: 2979
			STATE_LOAD_ENTITY_BLUEPRINTS,
			// Token: 0x04000BA4 RID: 2980
			STATE_CREATE_GAME,
			// Token: 0x04000BA5 RID: 2981
			STATE_LOAD_ASSETS,
			// Token: 0x04000BA6 RID: 2982
			STATE_FETCH_PRODUCT_INFO,
			// Token: 0x04000BA7 RID: 2983
			STATE_AWAIT_PRODUCT_INFO,
			// Token: 0x04000BA8 RID: 2984
			STATE_FETCH_PURCHASE_INFO,
			// Token: 0x04000BA9 RID: 2985
			STATE_AWAIT_PURCHASE_INFO,
			// Token: 0x04000BAA RID: 2986
			STATE_START_STORE,
			// Token: 0x04000BAB RID: 2987
			STATE_LOAD_ASSETS_TERRAIN,
			// Token: 0x04000BAC RID: 2988
			STATE_LOAD_ASSETS_SIMULATION,
			// Token: 0x04000BAD RID: 2989
			STATE_PRECACHE_GUI,
			// Token: 0x04000BAE RID: 2990
			STATE_LOAD_ASSETS_TIME_DEPENDENTS,
			// Token: 0x04000BAF RID: 2991
			STATE_LOAD_ASSETS_SEND_COMMANDS,
			// Token: 0x04000BB0 RID: 2992
			STATE_CREATE_TERRAIN_MESHES,
			// Token: 0x04000BB1 RID: 2993
			STATE_LOAD_ASSETS_ACTIVATE_QUESTS,
			// Token: 0x04000BB2 RID: 2994
			STATE_PROCESS_PENDING,
			// Token: 0x04000BB3 RID: 2995
			STATE_SETUP_SIMULATION,
			// Token: 0x04000BB4 RID: 2996
			STATE_LOAD_SOARING_COMMUNITY_EVENTS,
			// Token: 0x04000BB5 RID: 2997
			STATE_ANALYTICS_BOOKKEPING,
			// Token: 0x04000BB6 RID: 2998
			STATE_LAST,
			// Token: 0x04000BB7 RID: 2999
			STATE_ERROR
		}

		// Token: 0x020001ED RID: 493
		public enum SplashScreenState
		{
			// Token: 0x04000BB9 RID: 3001
			Loading,
			// Token: 0x04000BBA RID: 3002
			Patchy,
			// Token: 0x04000BBB RID: 3003
			None
		}

		// Token: 0x020004A7 RID: 1191
		// (Invoke) Token: 0x060024FF RID: 9471
		private delegate void ProcessStartingProgressState(Session session);
	}

	// Token: 0x020001EE RID: 494
	public class GameStopping : Session.State
	{
		// Token: 0x060010B6 RID: 4278 RVA: 0x0006FFD4 File Offset: 0x0006E1D4
		public void OnEnter(Session session)
		{
			session.game = null;
			UnityGameResources.Reset();
			session.ChangeState("Authorizing", true);
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0006FFF0 File Offset: 0x0006E1F0
		public void OnLeave(Session session)
		{
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0006FFF4 File Offset: 0x0006E1F4
		public void OnUpdate(Session session)
		{
		}
	}

	// Token: 0x020001EF RID: 495
	public class GetJelly : Session.State
	{
		// Token: 0x060010BA RID: 4282 RVA: 0x00070000 File Offset: 0x0006E200
		public void OnEnter(Session session)
		{
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			string title = (string)session.CheckAsyncRequest("jelly_message_title");
			string message = (string)session.CheckAsyncRequest("jelly_message");
			string question = (string)session.CheckAsyncRequest("jelly_question");
			string acceptLabel = (string)session.CheckAsyncRequest("jelly_message_ok_label");
			string cancelLabel = (string)session.CheckAsyncRequest("jelly_message_cancel_label");
			Action okButtonHandler = (Action)session.CheckAsyncRequest("jelly_message_ok_action");
			Action cancelButtonHandler = (Action)session.CheckAsyncRequest("jelly_message_cancel_action");
			SBGUIGetJellyDialog sbguigetJellyDialog = SBUIBuilder.MakeAndPushGetJellyDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), title, message, question, acceptLabel, cancelLabel, null, okButtonHandler, cancelButtonHandler, true);
			sbguigetJellyDialog.tform.parent = GUIMainView.GetInstance().transform;
			sbguigetJellyDialog.tform.localPosition = Vector3.zero;
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0007017C File Offset: 0x0006E37C
		public void OnLeave(Session session)
		{
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x00070180 File Offset: 0x0006E380
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x000701E4 File Offset: 0x0006E3E4
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x04000BBC RID: 3004
		private const string GET_JELLY = "GetJelly";
	}

	// Token: 0x020001F0 RID: 496
	public class HardSpendConfirm : Session.State
	{
		// Token: 0x060010BF RID: 4287 RVA: 0x000701F0 File Offset: 0x0006E3F0
		public void OnEnter(Session session)
		{
			TFUtils.Assert(session.properties.hardSpendActions != null, "You need to first set the hardSpendActions session property before trying to rush!");
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			Action acceptHandler = delegate()
			{
				session.ChangeState("HardSpendPassthrough", false);
			};
			Action denyStep1 = session.properties.hardSpendActions.cancel;
			if (denyStep1 == null)
			{
				denyStep1 = delegate()
				{
					session.ChangeState("Playing", true);
				};
			}
			session.properties.denialActions = delegate()
			{
				denyStep1();
				session.analytics.LogPlayerRejectHardSpend(session.properties.hardSpendActions.cost(TFUtils.EpochTime()).ResourceAmounts[ResourceManager.HARD_CURRENCY], session.game.resourceManager.PlayerLevelAmount);
				session.properties.cleanUp = delegate()
				{
					Session.HardSpendPassthrough.ClearSpendProperties(session);
				};
			};
			Vector2 screenPosition = session.properties.hardSpendActions.screenPosition;
			session.properties.microConfirmDialog = SBUIBuilder.MakeAndPushJjMicroConfirmDialog(session, null, Language.Get("!!PREFAB_CONFIRM_PURCHASE_SHORT"), session.properties.hardSpendActions.cost, acceptHandler, session.properties.denialActions, screenPosition);
			session.game.sessionActionManager.SetActionHandler("hard_spend_confirm_ui", session, new List<SBGUIScreen>
			{
				session.properties.microConfirmDialog
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			SessionActionSimulationHelper.EnableHandler(session, true);
			Cost cost = session.properties.hardSpendActions.cost(TFUtils.EpochTime());
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(cost.ResourceAmounts, session.game.resourceManager);
			if (resourcesStillRequired.Count > 0)
			{
				session.properties.hardSpendActions.logSpend(false, cost);
				session.analytics.LogPlayerConfirmHardSpend(cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], false, session.game.resourceManager.PlayerLevelAmount);
				session.TheSoundEffectManager.PlaySound("Error");
			}
			else
			{
				session.TheSoundEffectManager.PlaySound("HighlightItem");
			}
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00070484 File Offset: 0x0006E684
		public void OnLeave(Session session)
		{
			SBUIBuilder.ReleaseTopScreen();
			if (session.properties.cleanUp != null)
			{
				session.properties.cleanUp();
				session.properties.cleanUp = null;
			}
			session.properties.microConfirmDialog = null;
			session.properties.denialActions = null;
			session.game.sessionActionManager.ClearActionHandler("hard_spend_confirm_ui", session);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x000704F4 File Offset: 0x0006E6F4
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			if (session.properties.hardSpendActions != null)
			{
				TFUtils.Assert(session.properties.hardSpendActions.complete != null, "Must set the hard spend complete action before attempting to rush!");
				Cost cost = session.properties.hardSpendActions.cost(TFUtils.EpochTime());
				int num = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
				session.properties.microConfirmDialog.SetHardAmount(num);
				if (num <= 0)
				{
					session.properties.denialActions();
				}
			}
			else
			{
				session.properties.denialActions();
			}
		}

		// Token: 0x04000BBD RID: 3005
		private const string HARD_SPEND_CONFIRM_HANDLER = "hard_spend_confirm_ui";
	}

	// Token: 0x020001F1 RID: 497
	public class HardSpendPassthrough : Session.State
	{
		// Token: 0x060010C3 RID: 4291 RVA: 0x000705F8 File Offset: 0x0006E7F8
		public void OnEnter(Session session)
		{
			ulong time = TFUtils.EpochTime();
			if (session.properties.hardSpendActions == null)
			{
				TFUtils.DebugLog("we should not be rushing without a cost");
				session.ChangeState("Playing", true);
				return;
			}
			Cost cost = session.properties.hardSpendActions.cost(time);
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(cost.ResourceAmounts, session.game.resourceManager);
			if (resourcesStillRequired.Count > 0)
			{
				session.InsufficientResourcesHandler(session, session.properties.hardSpendActions.subjectText, session.properties.hardSpendActions.subjectDID, session.properties.hardSpendActions.complete, session.properties.hardSpendActions.cancel, cost);
			}
			else
			{
				Action action = delegate()
				{
					session.AddAsyncResponse("dialogs_to_show", true);
				};
				session.game.dropManager.DialogNeededCallback = action;
				session.game.questManager.OnShowDialogCallback = action;
				session.game.communityEventManager.DialogNeededCallback = action;
				TFUtils.Assert(session.properties.hardSpendActions != null, "You must set the hardSpendActions before trying to rush something!");
				TFUtils.Assert(session.properties.hardSpendActions.logSpend != null, "You must set the hardSpendActions.log Action before trying to hard spend on something!");
				if (session.properties.hardSpendActions == null)
				{
					session.ChangeState("Playing", true);
				}
				else if (cost == null || cost.ResourceAmounts.Count < 1)
				{
					TFUtils.ErrorLog("Could not figure out rush cost. Something has gone wrong.");
					session.properties.hardSpendActions.cancel();
				}
				else
				{
					session.properties.hardSpendActions.logSpend(true, cost);
					session.analytics.LogPlayerConfirmHardSpend(cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], true, session.game.resourceManager.PlayerLevelAmount);
					session.TheSoundEffectManager.PlaySound("Rush");
					session.properties.hardSpendActions.execute();
					session.properties.hardSpendActions.complete();
				}
			}
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x000708B0 File Offset: 0x0006EAB0
		public void OnLeave(Session session)
		{
			Session.HardSpendPassthrough.ClearSpendProperties(session);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x000708B8 File Offset: 0x0006EAB8
		public void OnUpdate(Session session)
		{
		}

		// Token: 0x060010C6 RID: 4294 RVA: 0x000708BC File Offset: 0x0006EABC
		public static void ClearSpendProperties(Session session)
		{
			session.properties.overrideSimulatedToRush = null;
			session.properties.hardSpendActions = null;
		}
	}

	// Token: 0x020001F2 RID: 498
	public class HardSpendActions
	{
		// Token: 0x060010C7 RID: 4295 RVA: 0x000708D8 File Offset: 0x0006EAD8
		public HardSpendActions(Action execute, Cost.CostAtTime cost, string subjectText, int subjectDID, Action complete, Action<bool, Cost> logSpend, Vector2 screenPosition) : this(execute, cost, subjectText, subjectDID, complete, complete, logSpend, screenPosition)
		{
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x000708F8 File Offset: 0x0006EAF8
		public HardSpendActions(Action execute, Cost.CostAtTime cost, string subjectText, int subjectDID, Action complete, Action cancel, Action<bool, Cost> logSpend, Vector2 screenPosition)
		{
			this.cost = cost;
			this.subjectText = subjectText;
			this.execute = execute;
			this.complete = complete;
			this.cancel = cancel;
			this.logSpend = logSpend;
			this.screenPosition = screenPosition;
			this.subjectDID = subjectDID;
		}

		// Token: 0x04000BBE RID: 3006
		public Cost.CostAtTime cost;

		// Token: 0x04000BBF RID: 3007
		public string subjectText;

		// Token: 0x04000BC0 RID: 3008
		public int subjectDID;

		// Token: 0x04000BC1 RID: 3009
		public Action execute;

		// Token: 0x04000BC2 RID: 3010
		public Action complete;

		// Token: 0x04000BC3 RID: 3011
		public Action cancel;

		// Token: 0x04000BC4 RID: 3012
		public Action<bool, Cost> logSpend;

		// Token: 0x04000BC5 RID: 3013
		public Vector2 screenPosition;
	}

	// Token: 0x020001F3 RID: 499
	public class InAppPurchasing : Session.State
	{
		// Token: 0x060010CA RID: 4298 RVA: 0x00070950 File Offset: 0x0006EB50
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			if (this.receivedError)
			{
				Action okAction = delegate()
				{
					session.ChangeState("Playing", true);
				};
				TFUtils.DebugLog("Error out on store request");
				session.game.analytics.LogFailInAppPurchase(session.properties.iapBundleName, session.game.resourceManager.PlayerLevelAmount);
				session.ErrorMessageHandler(session, this.errorTitle, this.errorMessage, Language.Get("!!PREFAB_OK"), okAction, 1f);
			}
			else if (this.canceledTransaction)
			{
				session.game.analytics.LogCancelInAppPurchase(session.properties.iapBundleName, session.game.resourceManager.PlayerLevelAmount);
				session.ChangeState("Playing", true);
			}
			else if (this.receivedProduct)
			{
				session.game.analytics.LogCompleteInAppPurchase(session.properties.iapBundleName, session.game.resourceManager.PlayerLevelAmount);
				session.ChangeState("Playing", true);
			}
			else if (this.elapsedTime > 15f)
			{
				Action okAction2 = delegate()
				{
					session.ChangeState("Playing", true);
				};
				TFUtils.DebugLog("Timed out on store request");
				session.ErrorMessageHandler(session, Language.Get("!!NOTIFY_STORE_TIMEOUT_TITLE"), Language.Get("!!NOTIFY_STORE_TIMEOUT_MESSAGE"), Language.Get("!!PREFAB_OK"), okAction2, 1f);
			}
			else
			{
				string title = Language.Get("!!NOTIFY_STORE_WAITING_TITLE");
				string message = Language.Get("!!NOTIFY_STORE_WAITING_MESSAGE");
				string acceptLabel = Language.Get("!!PREFAB_OK");
				Action okButtonHandler = delegate()
				{
					session.ChangeState("Playing", true);
				};
				SBUIBuilder.MakeAndPushConfirmationDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), title, message, acceptLabel, null, null, okButtonHandler, null, false);
				this.elapsedTime += Time.deltaTime;
				TFUtils.DebugLogTimed("Awaiting purchase completion");
			}
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00070C20 File Offset: 0x0006EE20
		public void OnEnter(Session session)
		{
			session.camera.SetEnableUserInput(false, false, default(Vector3));
			session.game.analytics.LogRequestInAppPurchase(session.properties.iapBundleName, session.game.resourceManager.PlayerLevelAmount);
			this.elapsedTime = 0f;
			this.receivedProduct = false;
			this.receivedError = false;
			this.canceledTransaction = false;
			session.game.store.PurchaseUpdateReceived += this.OnPurchaseUpdate;
			session.game.store.PurchaseResponseReceived += this.OnPurchaseResponse;
			session.game.store.PurchaseError += this.OnPurchaseError;
			session.game.store.StartRmtPurchase(session);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00070CF4 File Offset: 0x0006EEF4
		public void OnLeave(Session session)
		{
			session.properties.iapBundleName = null;
			session.game.store.PurchaseUpdateReceived -= this.OnPurchaseUpdate;
			session.game.store.PurchaseResponseReceived -= this.OnPurchaseResponse;
			session.game.store.PurchaseError -= this.OnPurchaseError;
			session.camera.SetEnableUserInput(true, false, default(Vector3));
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x00070D78 File Offset: 0x0006EF78
		public void OnPurchaseUpdate(object sender, RmtStore.StoreEventArgs args)
		{
			this.elapsedTime = 0f;
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x00070D88 File Offset: 0x0006EF88
		public void OnPurchaseResponse(object sender, RmtStore.StoreEventArgs args)
		{
			if (!TFServer.IsNetworkError(args.results))
			{
				this.receivedProduct = true;
			}
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00070DA4 File Offset: 0x0006EFA4
		public static void OnPurchaseDefered(object sender, RmtStore.StoreEventArgs args)
		{
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00070DA8 File Offset: 0x0006EFA8
		public void OnPurchaseError(object sender, RmtStore.StoreEventArgs args)
		{
			object obj;
			object obj2;
			object obj3;
			if (args.results.TryGetValue("state", out obj) && args.results.TryGetValue("reason", out obj2))
			{
				string a = (string)obj;
				string a2 = (string)obj2;
				if (a2 == "userCancelled")
				{
					TFUtils.DebugLog("User canceled purchase");
					this.canceledTransaction = true;
					return;
				}
				if (a == "failed")
				{
					this.errorTitle = Language.Get("!!ERROR_STORE_FAILED_TITLE");
					this.errorMessage = (string)args.results["description"];
				}
			}
			else if (TFServer.IsNetworkError(args.results))
			{
				this.errorTitle = Language.Get("!!ERROR_STORE_NETWORK_TITLE");
				this.errorMessage = Language.Get("!!ERROR_STORE_NETWORK_MESSAGE");
			}
			else if (args.results.TryGetValue("purchase", out obj3))
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)obj3;
				this.errorTitle = Language.Get("!!ERROR_STORE_UNCONFIRMED_TITLE");
				this.errorMessage = Language.Get("!!ERROR_STORE_UNCONFIRMED_MESSAGE");
			}
			this.receivedError = true;
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00070ED0 File Offset: 0x0006F0D0
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x04000BC6 RID: 3014
		private bool receivedProduct;

		// Token: 0x04000BC7 RID: 3015
		private bool receivedError;

		// Token: 0x04000BC8 RID: 3016
		private bool canceledTransaction;

		// Token: 0x04000BC9 RID: 3017
		private float elapsedTime;

		// Token: 0x04000BCA RID: 3018
		private string errorTitle;

		// Token: 0x04000BCB RID: 3019
		private string errorMessage;
	}

	// Token: 0x020001F4 RID: 500
	public class InsufficientDialog : Session.State
	{
		// Token: 0x060010D3 RID: 4307 RVA: 0x00070EDC File Offset: 0x0006F0DC
		public void OnEnter(Session session)
		{
			this.Setup(session);
			session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00070F08 File Offset: 0x0006F108
		public void Setup(Session session)
		{
			Action okAction = (Action)session.CheckAsyncRequest("insufficient_accept");
			Action cancelAction = (Action)session.CheckAsyncRequest("insufficient_cancel");
			Cost cost2 = (Cost)session.CheckAsyncRequest("insufficient_resources");
			string purchaseName = (string)session.CheckAsyncRequest("insufficient_itemname");
			int purchaseDID = (int)session.CheckAsyncRequest("insufficient_item_did");
			if (cost2 == null)
			{
				TFUtils.DebugLog("Error occurred, we are failing out of the insufficient dialog due to a missing cost");
				session.ChangeState("Playing", true);
				return;
			}
			Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(cost2.ResourceAmounts, session.game.resourceManager);
			TFUtils.Assert(resourcesStillRequired.Count > 0, "Error occurred, we have enough resources to apply cost.");
			Cost resourcesToPurchase = Cost.GetResourcesToPurchase(cost2.ResourceAmounts, session.game.resourceManager);
			TFUtils.Assert(cost2.ResourceAmounts.Count > 0, "Error occurred, we appear to have enough resources to apply cost.");
			int jjCost = session.game.resourceManager.GetResourcesPackageCostInHardCurrencyValue(resourcesToPurchase);
			Action pNewCancelAction = null;
			if (resourcesToPurchase.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
			{
				Action okAction2 = delegate()
				{
					this.PrepForStoreUI(session, "rmt");
					session.AddAsyncResponse("store_open_type", "store_open_need_currency_redirect");
					cancelAction();
					session.ChangeState("Shopping", true);
				};
				Action cancelAction2 = delegate()
				{
					cancelAction();
				};
				session.GetJellyHandler(session, Language.Get("!!PREFAB_GET") + " " + Language.Get(session.TheGame.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Name), Language.Get("!!PREFAB_NO_JELLY"), Language.Get("!!PREFAB_WANT_JELLY"), Language.Get("!!PREFAB_BUY_JELLY"), Language.Get("!!PREFAB_CANCEL"), okAction2, cancelAction2);
			}
			else
			{
				string acceptLabel;
				Action okButtonHandler;
				if (session.game.resourceManager.HasEnough(ResourceManager.HARD_CURRENCY, jjCost))
				{
					acceptLabel = Language.Get("!!PREFAB_OK");
					Action confirmAction = delegate()
					{
						session.analytics.LogInsufficientDialog(purchaseName, jjCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
						session.game.resourceManager.PurchaseResourcesWithHardCurrency(jjCost, resourcesToPurchase, session.game);
						session.game.simulation.ModifyGameState(new PurchaseResourcesAction(new Identity(), jjCost, resourcesToPurchase));
						session.TheSoundEffectManager.PlaySound("Purchase");
						string sItemName = "Not_enough_money_" + purchaseName;
						if (session.TheGame.resourceManager.Resources.ContainsKey(purchaseDID) || session.TheGame.craftManager.ContainsRecipe(purchaseDID))
						{
							AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "craft", "instant_purchase", string.Empty, "confirm");
						}
						else
						{
							AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "buildings", "instant_purchase", string.Empty, "confirm");
						}
					};
					Action<bool, Cost> log = delegate(bool canAfford, Cost cost)
					{
					};
					pNewCancelAction = delegate()
					{
						cancelAction();
					};
					okButtonHandler = delegate()
					{
						session.properties.hardSpendActions = new Session.HardSpendActions(confirmAction, (ulong time) => new Cost(new Dictionary<int, int>
						{
							{
								ResourceManager.HARD_CURRENCY,
								jjCost
							}
						}), string.Empty, purchaseDID, okAction, pNewCancelAction, log, (!(session.properties.insufficientDialog == null)) ? session.properties.insufficientDialog.GetHardSpendPosition() : session.TheCamera.ScreenCenter);
						session.ChangeState("HardSpendConfirm", false);
					};
				}
				else
				{
					acceptLabel = Language.Get("!!PREFAB_OK");
					Action internalOk = delegate()
					{
						TFUtils.Assert(false, "We don't support RMT Store functions yet!");
						session.game.resourceManager.PurchaseResourcesWithHardCurrency(jjCost, resourcesToPurchase, session.game);
						session.game.simulation.ModifyGameState(new PurchaseResourcesAction(new Identity(), jjCost, resourcesToPurchase));
						session.TheSoundEffectManager.PlaySound("Purchase");
						string sItemName = "Not_enough_money_" + purchaseName;
						if (session.TheGame.buildingUnlockManager.CheckBuildingUnlock(purchaseDID))
						{
							AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "buildings", "instant_purchase", string.Empty, "confirm");
						}
						else if (session.TheGame.resourceManager.Resources.ContainsKey(purchaseDID) || (session.TheGame.craftManager.ContainsRecipe(purchaseDID) && !session.TheGame.buildingUnlockManager.CheckBuildingUnlock(purchaseDID)))
						{
							AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "craft", "instant_purchase", string.Empty, "confirm");
						}
						else
						{
							AnalyticsWrapper.LogJellyConfirmation(session.TheGame, purchaseDID, jjCost, sItemName, "buildings", "instant_purchase", string.Empty, "confirm");
						}
						okAction();
					};
					pNewCancelAction = delegate()
					{
						cancelAction();
					};
					okButtonHandler = delegate()
					{
						session.InsufficientResourcesHandler(session, purchaseName, purchaseDID, internalOk, pNewCancelAction, new Cost(new Dictionary<int, int>
						{
							{
								ResourceManager.HARD_CURRENCY,
								jjCost
							}
						}));
					};
				}
				Action cancelButtonHandler = delegate()
				{
					session.TheSoundEffectManager.PlaySound("CloseMenu");
					if (pNewCancelAction != null)
					{
						pNewCancelAction();
					}
					else
					{
						cancelAction();
					}
				};
				session.properties.insufficientDialog = SBUIBuilder.MakeAndPushInsufficientResourcesDialog(session, cost2.ResourceAmounts, resourcesStillRequired, new int?(jjCost), session.game.resourceManager.Resources[ResourceManager.HARD_CURRENCY].GetResourceTexture(), acceptLabel, okButtonHandler, cancelButtonHandler);
			}
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0007128C File Offset: 0x0006F48C
		public void OnLeave(Session session)
		{
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00071290 File Offset: 0x0006F490
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x000712C8 File Offset: 0x0006F4C8
		private void PrepForStoreUI(Session session, string tabToOpen)
		{
			session.AddAsyncResponse("target_store_tab", tabToOpen);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x000712D8 File Offset: 0x0006F4D8
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}
	}

	// Token: 0x020001F5 RID: 501
	public class Inventory : Session.State
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x000712E8 File Offset: 0x0006F4E8
		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.PlayBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false, false, default(Vector3));
			session.TheCamera.TurnOnScreenBuffer();
			Action action = delegate()
			{
				AndroidBack.getInstance().pop();
				bool? flag = (bool?)session.CheckAsyncRequest("FromEdit");
				if (flag != null && flag.Value)
				{
					session.ChangeState("Editing", true);
				}
				else
				{
					session.ChangeState("Playing", true);
				}
			};
			Action shopClickHandler = delegate()
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping", true);
			};
			Action optionsHandler = delegate()
			{
				session.ChangeState("Options", true);
			};
			Action editClickHandler = delegate()
			{
				session.ChangeState("Editing", true);
			};
			Action<SBInventoryItem> buildingClickHandler = delegate(SBInventoryItem item)
			{
				TFUtils.DebugLog("inventory clicked: " + item.ToString());
				Identity id = item.entity.Id;
				List<KeyValuePair<int, Identity>> val;
				Entity entity = session.game.inventory.RemoveEntity(id, out val);
				Ray ray = session.camera.ScreenPointToRay(session.camera.ScreenCenter);
				Vector3 vector;
				session.game.terrain.ComputeIntersection(ray, out vector);
				Simulated simulated = session.game.simulation.CreateSimulated(entity, EntityManager.BuildingActions["replacing"], new Vector2(vector.x, vector.y));
				simulated.Warp(simulated.Position, null);
				session.game.selected = simulated;
				session.game.selected.Visible = true;
				session.AddAsyncResponse("FromInventory", true);
				session.AddAsyncResponse("AssociatedEntities", val);
				session.ChangeState("MoveBuildingInPlacement", true);
			};
			Action<SBInventoryItem> inventoryClickHandler = delegate(SBInventoryItem item)
			{
				string movieFileName = item.movieFileName;
				session.PlayMovie(movieFileName, "Inventory");
			};
			Action openIAPTabHandlerSoft = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping", true);
			};
			Action openIAPTabHandlerHard = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping", true);
			};
			session.properties.inventoryHud = SBUIBuilder.MakeAndPushStandardUI(session, false, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), shopClickHandler, action, optionsHandler, editClickHandler, null, Session.DragFeeding.SwitchToFn(session), null, openIAPTabHandlerSoft, openIAPTabHandlerHard, null, null, false);
			session.properties.inventoryHud.SetVisibleNonEssentialElements(false, true);
			SBGUIButton sbguibutton = (SBGUIButton)session.properties.inventoryHud.FindChild("inventory");
			sbguibutton.SetActive(true);
			SBGUIButton sbguibutton2 = (SBGUIButton)session.properties.inventoryHud.FindChild("marketplace");
			sbguibutton2.SetActive(false);
			SBGUIButton sbguibutton3 = (SBGUIButton)session.properties.inventoryHud.FindChild("community_event");
			sbguibutton3.SetActive(false);
			SBGUIInventoryScreen item2 = SBUIBuilder.MakeAndPushInventoryDialog(session, session.game.entities, session.TheSoundEffectManager, action, buildingClickHandler, inventoryClickHandler);
			Action action2 = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action2;
			session.game.questManager.OnShowDialogCallback = action2;
			session.game.communityEventManager.DialogNeededCallback = action2;
			session.game.sessionActionManager.SetActionHandler("inventory_ui", session, new List<SBGUIScreen>
			{
				session.properties.inventoryHud,
				item2
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x000715A4 File Offset: 0x0006F7A4
		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.game.sessionActionManager.ClearActionHandler("inventory_ui", session);
			session.properties.inventoryHud.SetVisibleNonEssentialElements(true);
			session.properties.inventoryHud = null;
			session.TheCamera.TurnOffScreenBuffer();
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00071600 File Offset: 0x0006F800
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00071604 File Offset: 0x0006F804
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x04000BCD RID: 3021
		public const string FROM_INVENTORY = "FromInventory";

		// Token: 0x04000BCE RID: 3022
		public const string ASSOCIATED_ENTITIES = "AssociatedEntities";

		// Token: 0x04000BCF RID: 3023
		private const string INVENTORY_UI_HANDLER = "inventory_ui";
	}

	// Token: 0x020001F6 RID: 502
	public abstract class MoveBuilding : Session.State
	{
		// Token: 0x060010E0 RID: 4320 RVA: 0x0007167C File Offset: 0x0006F87C
		public virtual void OnEnter(Session session)
		{
			TFUtils.DebugLog("Selected building did=" + session.game.selected.entity.DefinitionId);
			TFUtils.Assert(session.game.selected != null, "How is there no selected building!?");
			if (session.CheckAsyncRequest("silent_enter") == null)
			{
				session.TheSoundEffectManager.PlaySound("EditModePickup");
			}
			session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
			List<Simulated> list = (List<Simulated>)session.CheckAsyncRequest("blocking_sims");
			if (list != null)
			{
				foreach (Simulated simulated in list)
				{
					simulated.ClearBlockerHighlight();
				}
			}
			if (!session.properties.preMovePositionSet)
			{
				session.properties.preMovePosition = session.game.selected.Position;
				session.properties.preMoveFlip = session.game.selected.Flip;
				session.properties.preMovePositionSet = true;
			}
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			SBGUIEvent sbguievent = (SBGUIEvent)session.CheckAsyncRequest("CurrentGuiEventInfo");
			if (sbguievent != null)
			{
				this.HandleSBGUIEvent(sbguievent, session);
			}
			session.game.sessionActionManager.SetActionHandler("movedragging_ui", session, new List<SBGUIScreen>
			{
				session.properties.editingHud
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x000718E8 File Offset: 0x0006FAE8
		public virtual void OnLeave(Session session)
		{
			if (!session.properties.waitToDecidePlacement)
			{
				this.DecideForSelectedBuilding(session);
			}
			session.properties.waitToDecidePlacement = false;
			this.CleanupMovementVisuals(session);
			session.game.sessionActionManager.ClearActionHandler("movedragging_ui", session);
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00071938 File Offset: 0x0006FB38
		protected void DecideForSelectedBuilding(Session session)
		{
			if (session.game.selected == null)
			{
				return;
			}
			if (this.IsValidLocationForSelected(session))
			{
				this.AcceptPlacement(session);
			}
			else
			{
				this.DenyPlacement(session);
			}
		}

		// Token: 0x060010E3 RID: 4323
		protected abstract void HandleSBGUIEvent(SBGUIEvent evt, Session session);

		// Token: 0x060010E4 RID: 4324 RVA: 0x00071978 File Offset: 0x0006FB78
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			this.ColorSelectedByOccupation(session);
			this.interactionStrip.OnUpdate(session);
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x000719F0 File Offset: 0x0006FBF0
		protected void SnapSelectedToInputPosition(Session session, Vector2 position, bool snapObject, bool updatePaths = false)
		{
			if (session.game.selected == null)
			{
				TFUtils.ErrorLog("SessionMoveBuilding.SnapSelectedToInputPosition - selected is null");
				return;
			}
			Simulated selected = session.game.selected;
			Ray ray = session.TheCamera.ScreenPointToRay(position);
			Vector3 position2;
			session.game.terrain.ComputeIntersection(ray, out position2);
			position2.x -= selected.Box.Width * 0.5f;
			position2.y -= selected.Box.Height * 0.5f;
			Vector2 vector = session.game.terrain.CalculateNearestGridPosition(position2, selected.Footprint);
			if (snapObject && !selected.GetEntity<StructureDecorator>().ShareableSpace)
			{
				if (!Session.TheDebugManager.debugPlaceObjects)
				{
					position2 = vector;
				}
			}
			else if (snapObject && !selected.GetEntity<StructureDecorator>().ShareableSpaceSnap)
			{
				if (!Session.TheDebugManager.debugPlaceObjects)
				{
					position2 = vector;
				}
			}
			else
			{
				position2 = session.game.terrain.ConstrainToAlignedBox(position2, selected.Footprint);
			}
			if (updatePaths)
			{
				selected.Warp(new Vector2(position2.x, position2.y), session.game.simulation);
			}
			else
			{
				selected.Warp(new Vector2(position2.x, position2.y), null);
			}
			Simulated worker = this.getWorker(selected, session.game.simulation);
			if (worker != null)
			{
				if (worker.GetDisplayState() == "work")
				{
					worker.Warp(selected.PointOfInterest, null);
				}
				else if (worker.GetDisplayState() != "idle")
				{
					session.game.simulation.Router.Send(CompleteCommand.Create(worker.Id, worker.Id));
				}
			}
			if (!Session.TheDebugManager.debugPlaceObjects)
			{
				selected.SnapPosition = vector;
			}
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x00071BFC File Offset: 0x0006FDFC
		protected bool IsValidLocationForSelected(Session session)
		{
			return Session.TheDebugManager.debugPlaceObjects || session.game.simulation.PlacementQuery(session.game.selected, false) != Simulation.Placement.RESULT.INVALID;
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00071C40 File Offset: 0x0006FE40
		protected virtual void AcceptPlacement(Session session)
		{
			TFUtils.Assert(this.IsValidLocationForSelected(session), "Invalid placement! Why is accept being called?");
			this.ResetMoveDecorationsOnSelected(session);
			session.properties.cameFromMarketplace = false;
			Simulated selected = session.game.selected;
			Terrain terrain = (session.game.simulation == null) ? null : session.game.simulation.Terrain;
			bool flag = selected.HasEntity<StructureDecorator>() && selected.GetEntity<StructureDecorator>().IsObstacle;
			Vector2 offset = session.properties.preMovePosition - selected.Position;
			AlignedBox alignedBox = selected.Box.OffsetByVector(offset);
			if (terrain != null && alignedBox.xmin >= 0f && flag)
			{
				terrain.SetOrClearObstacle(alignedBox, false);
				terrain.SetOrClearObstacle(selected.Box, true);
				session.game.simulation.ResetAllAffectedPaths(selected.Box);
			}
			Simulated worker = this.getWorker(session.game.selected, session.game.simulation);
			if (worker != null && worker.GetDisplayState() == "idle")
			{
				session.game.simulation.Router.Send(MoveCommand.Create(session.game.selected.Id, worker.Id, session.game.selected.PointOfInterest, session.game.selected.Flip));
			}
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00071DB8 File Offset: 0x0006FFB8
		protected void DenyPlacement(Session session)
		{
			TFUtils.Assert(session.game.selected != null, "Selected simulated cannot be null.");
			session.TheSoundEffectManager.PlaySound("Cancel");
			this.ResetMoveDecorationsOnSelected(session);
			if (session.properties.cameFromMarketplace)
			{
				session.game.simulation.RemoveSimulated(session.game.selected);
				session.game.entities.Destroy(session.game.selected.Id);
				session.game.selected = null;
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				session.properties.cameFromMarketplace = false;
				session.ChangeState("Shopping", true);
			}
			else if (this.WasFromInventory(session))
			{
				session.game.simulation.RemoveSimulated(session.game.selected);
				List<KeyValuePair<int, Identity>> associatedEntities = (List<KeyValuePair<int, Identity>>)session.CheckAsyncRequest("AssociatedEntities");
				session.game.inventory.AddItem(session.game.selected.GetEntity<BuildingEntity>(), associatedEntities);
				session.CheckAsyncRequest("FromInventory");
				session.game.selected = null;
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				session.ChangeState("Inventory", true);
			}
			else
			{
				session.game.selected.Warp(session.properties.preMovePosition, null);
				if (session.game.selected.Flip != session.properties.preMoveFlip)
				{
					session.game.selected.Flip = session.properties.preMoveFlip;
					session.game.selected.FlipWarp(session.game.simulation);
				}
				Simulated worker = this.getWorker(session.game.selected, session.game.simulation);
				if (worker != null)
				{
					if (worker.GetDisplayState() == "work")
					{
						worker.Warp(session.game.selected.PointOfInterest, null);
					}
					else if (worker.GetDisplayState() == "idle")
					{
						session.game.simulation.Router.Send(MoveCommand.Create(session.game.selected.Id, worker.Id, session.game.selected.PointOfInterest, session.game.selected.Flip));
					}
				}
				session.game.selected.Animate(session.game.simulation);
			}
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00072064 File Offset: 0x00070264
		protected void ResetMoveDecorationsOnSelected(Session session)
		{
			session.game.selected.Alpha = 1f;
			session.game.selected.FootprintVisible = false;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00072098 File Offset: 0x00070298
		protected Simulated getWorker(Simulated buildingSim, Simulation simulation)
		{
			if (buildingSim.HasEntity<ErectableDecorator>())
			{
				ErectableDecorator entity = buildingSim.GetEntity<ErectableDecorator>();
				ulong? erectionCompleteTime = entity.ErectionCompleteTime;
				if (erectionCompleteTime != null && erectionCompleteTime.Value > TFUtils.EpochTime() && buildingSim.Variable.ContainsKey("employee"))
				{
					Identity identity = buildingSim.Variable["employee"] as Identity;
					if (identity != null)
					{
						Simulated simulated = simulation.FindSimulated(identity);
						if (simulated != null)
						{
							return simulated;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00072124 File Offset: 0x00070324
		protected void ResetPlacement(Session session)
		{
			session.game.selected.Warp(session.game.selected.Position, session.game.simulation);
			this.ResetMoveDecorationsOnSelected(session);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00072164 File Offset: 0x00070364
		private bool CheckFlag(Session session, string flagKey)
		{
			bool? flag = (bool?)session.CheckAsyncRequest(flagKey);
			if (flag != null)
			{
				session.AddAsyncResponse(flagKey, flag.Value);
			}
			return flag != null && flag.Value;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x000721B4 File Offset: 0x000703B4
		protected bool WasFromInventory(Session session)
		{
			return this.CheckFlag(session, "FromInventory");
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x000721C4 File Offset: 0x000703C4
		protected bool WasFromEdit(Session session)
		{
			return this.CheckFlag(session, "FromEdit");
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x000721D4 File Offset: 0x000703D4
		protected void ColorSelectedByOccupation(Session session)
		{
			this.UnmarkBlockers(session, false);
			if (session.game.selected != null)
			{
				Simulated selected = session.game.selected;
				selected.Alpha = 0.5f;
				AlignedBox box = selected.SnapBox;
				if (selected.HasEntity<StructureDecorator>() && selected.GetEntity<StructureDecorator>().ShareableSpace)
				{
					box = selected.Box;
				}
				List<Simulated> list = new List<Simulated>();
				if (Session.TheDebugManager.debugPlaceObjects || session.game.simulation.PlacementQuery(box, ref list, session.game.selected.Id, false) != Simulation.Placement.RESULT.INVALID)
				{
					selected.FootprintColor = Simulated.COLOR_FOOTPRINT_FREE;
				}
				else
				{
					list.Remove(selected);
					session.AddAsyncResponse("blocking_sims", list);
					this.MarkBlockers(session, true);
					selected.FootprintColor = Simulated.COLOR_FOOTPRINT_BLOCKED;
				}
				selected.FootprintVisible = true;
			}
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x000722B8 File Offset: 0x000704B8
		protected void MarkBlockers(Session session, bool persist = true)
		{
			List<Simulated> list = (List<Simulated>)session.CheckAsyncRequest("blocking_sims");
			if (list != null)
			{
				foreach (Simulated simulated in list)
				{
					simulated.BlockerHighlight();
				}
			}
			if (persist)
			{
				session.AddAsyncResponse("blocking_sims", list);
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x00072344 File Offset: 0x00070544
		protected void UnmarkBlockers(Session session, bool persist = false)
		{
			List<Simulated> list = (List<Simulated>)session.CheckAsyncRequest("blocking_sims");
			if (list != null)
			{
				foreach (Simulated simulated in list)
				{
					simulated.ClearBlockerHighlight();
				}
			}
			if (persist)
			{
				session.AddAsyncResponse("blocking_sims", list);
			}
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x000723D0 File Offset: 0x000705D0
		protected void CleanupMovementVisuals(Session session)
		{
			this.interactionStrip.Deactivate(session);
			this.UnmarkBlockers(session, false);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x000723E8 File Offset: 0x000705E8
		protected void AdornMovementVisuals(Session session)
		{
			if (session.game.selected.InteractionState.Controls != null && session.game.selected.InteractionState.Controls.Count > 0)
			{
				this.interactionStrip.ActivateOnSelected(session);
				this.interactionStrip.OnUpdate(session);
			}
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00072448 File Offset: 0x00070648
		protected void UpdateMovementBookkeeping(Session session)
		{
			if (this.IsValidLocationForSelected(session))
			{
				session.properties.preMovePosition = session.game.selected.Position;
				session.properties.preMoveFlip = session.game.selected.Flip;
			}
			else
			{
				TFUtils.ErrorLog("You shouldn't be trying to update movement bookkeping into an invalid location!");
			}
		}

		// Token: 0x04000BD0 RID: 3024
		public const string OVERRIDE_DRAG = "override_drag";

		// Token: 0x04000BD1 RID: 3025
		public const string PANNING_EVENT = "panning_event";

		// Token: 0x04000BD2 RID: 3026
		public const string SILENT_ENTER = "silent_enter";

		// Token: 0x04000BD3 RID: 3027
		protected const string BLOCKING_SIMULATEDS = "blocking_sims";

		// Token: 0x04000BD4 RID: 3028
		private const string MOVEDRAGGING_UI_HANDLER = "movedragging_ui";

		// Token: 0x04000BD5 RID: 3029
		protected Session.InteractionStripMixin interactionStrip = new Session.InteractionStripMixin();

		// Token: 0x04000BD6 RID: 3030
		protected bool? savedFlippedState;
	}

	// Token: 0x020001F7 RID: 503
	public class MoveBuildingInEdit : Session.MoveBuilding
	{
		// Token: 0x060010F6 RID: 4342 RVA: 0x000724B0 File Offset: 0x000706B0
		public override void OnEnter(Session session)
		{
			session.properties.firstEntered = true;
			this.m_bTouchBegan = false;
			Action inventoryClickHandler = delegate()
			{
				if (!session.game.featureManager.CheckFeature("inventory_soft"))
				{
					session.game.featureManager.UnlockFeature("inventory_soft");
					session.game.featureManager.ActivateFeatureActions(session.game, "inventory_soft");
					session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string>
					{
						"inventory_soft"
					}));
					return;
				}
				session.AddAsyncResponse("FromEdit", true);
				session.ChangeState("Inventory", true);
			};
			Action optionsHandler = delegate()
			{
				session.ChangeState("Options", true);
			};
			Action editClickHandler = delegate()
			{
				session.ChangeState("Playing", true);
			};
			Action openIAPTabHandlerSoft = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping", true);
			};
			Action openIAPTabHandlerHard = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping", true);
			};
			session.properties.editingHud = SBUIBuilder.MakeAndPushStandardUI(session, true, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), null, inventoryClickHandler, optionsHandler, editClickHandler, delegate
			{
				session.ChangeState("Paving", true);
			}, null, null, openIAPTabHandlerSoft, openIAPTabHandlerHard, null, null, true);
			((SBGUIStandardScreen)session.properties.editingHud).EnableFullHiding(false);
			SBGUIPulseButton sbguipulseButton = (SBGUIPulseButton)session.properties.editingHud.FindChild("community_event");
			sbguipulseButton.SetActive(false);
			base.AdornMovementVisuals(session);
			base.UpdateMovementBookkeeping(session);
			session.properties.startedTouchOnEmptySpace = false;
			this.LoadInteractionStrip(session);
			if (!session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
			{
				this.interactionStrip.EnableRejectButton(session, false);
			}
			else
			{
				this.interactionStrip.EnableRejectButton(session, true);
			}
			base.OnEnter(session);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0007264C File Offset: 0x0007084C
		public void Update()
		{
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00072650 File Offset: 0x00070850
		public override void OnLeave(Session session)
		{
			((SBGUIStandardScreen)session.properties.editingHud).EnableFullHiding(true);
			base.OnLeave(session);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00072670 File Offset: 0x00070870
		public void LoadInteractionStrip(Session session)
		{
			Action<Session> handler = delegate(Session sesh)
			{
				this.AcceptPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Playing", true);
			};
			Action<Session> handler2 = delegate(Session sesh)
			{
				this.AcceptPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Editing", true);
			};
			Action<Session> handler3 = delegate(Session sesh)
			{
				this.DenyPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Editing", true);
			};
			Action<Session> handler4 = delegate(Session sesh)
			{
				this.DenyPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Playing", true);
			};
			if (base.WasFromInventory(session) || session.properties.cameFromMarketplace)
			{
				this.interactionStrip.SetAcceptHandler(session, handler);
				this.interactionStrip.SetRejectHandler(session, handler4);
			}
			else
			{
				this.interactionStrip.SetAcceptHandler(session, handler2);
				this.interactionStrip.SetRejectHandler(session, handler3);
			}
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00072748 File Offset: 0x00070948
		protected override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_BEGIN:
			{
				session.properties.isDraggingBuildingAndScreen = false;
				session.properties.isInteractionStripActive = true;
				this.m_bTouchBegan = true;
				Predicate<Simulated> filterOutMatching = (Simulated simulated) => !simulated.InteractionState.IsEditable && !Session.TheDebugManager.debugPlaceObjects;
				Ray ray;
				Simulated simulated2 = Session.FindAlreadySelected(filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out ray, session.game.selected);
				if (simulated2 == null)
				{
					simulated2 = Session.FindBestSimulatedUnderPoint(new Session.SelectionPrioritizer(session.camera.UnityCamera), filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out ray);
				}
				if (simulated2 == null)
				{
					session.properties.startedTouchOnEmptySpace = true;
				}
				else
				{
					session.properties.startedTouchOnEmptySpace = false;
					this.clickedSim = simulated2;
				}
				if (simulated2 == session.game.selected && !Input.GetMouseButtonDown(1))
				{
					session.properties.isDraggingBuilding = true;
					session.AddAsyncResponse("override_drag", new object());
				}
				else
				{
					session.properties.isDraggingBuilding = false;
				}
				session.AddAsyncResponse("panning_event", evt);
				break;
			}
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
			case YGEvent.TYPE.TAP:
			case YGEvent.TYPE.RESET:
			case YGEvent.TYPE.DISABLE:
				if (this.m_bTouchBegan && !this.userCameraActive)
				{
					this.m_bTouchBegan = false;
					if (session.properties.startedTouchOnEmptySpace)
					{
						session.CheckAsyncRequest("override_drag");
						session.CheckAsyncRequest("panning_event");
						session.ChangeState("Editing", true);
					}
					else if (!session.properties.isDraggingBuilding && !session.properties.startedTouchOnEmptySpace)
					{
						this.interactionStrip.EnableButtons(session, true);
						if (!session.properties.waitToDecidePlacement)
						{
							base.DecideForSelectedBuilding(session);
						}
						session.properties.waitToDecidePlacement = false;
						base.CleanupMovementVisuals(session);
						session.game.selected.Alpha = 1f;
						session.game.selected.FootprintVisible = false;
						session.game.selected = this.clickedSim;
						session.TheSoundEffectManager.PlaySound(this.clickedSim.entity.SoundOnSelect);
						session.game.selected.Bounce();
						base.AdornMovementVisuals(session);
						base.UpdateMovementBookkeeping(session);
						this.LoadInteractionStrip(session);
						session.properties.firstEntered = true;
					}
				}
				if (this.userCameraActive || session.properties.firstEntered)
				{
					this.userCameraActive = false;
					if (session.properties.isDraggingBuilding || session.properties.firstEntered)
					{
						session.properties.isDraggingBuilding = true;
						session.camera.SetEnableUserInput(this.userCameraActive, session.properties.isDraggingBuilding, this.interactionStrip.StripPosition);
						if (session.properties.firstEntered)
						{
							session.camera.ChangeState(SBCamera.State.Dragging);
						}
					}
					else
					{
						session.camera.SetEnableUserInput(this.userCameraActive, false, default(Vector3));
						if (!session.properties.isInteractionStripActive)
						{
							session.properties.isInteractionStripActive = true;
							this.interactionStrip.EnableButtons(session, true);
						}
						this.interactionStrip.OnUpdate(session);
					}
				}
				session.properties.waitToDecidePlacement = false;
				session.properties.isDraggingBuildingAndScreen = false;
				session.properties.isDraggingBuilding = false;
				session.properties.firstEntered = false;
				break;
			case YGEvent.TYPE.TOUCH_STAY:
			{
				object obj = null;
				if (!session.properties.startedTouchOnEmptySpace && session.properties.isDraggingBuilding)
				{
					obj = session.CheckAsyncRequest("override_drag");
				}
				if (obj != null)
				{
					session.AddAsyncResponse("override_drag", obj);
					base.SnapSelectedToInputPosition(session, evt.position, true, true);
					this.userCameraActive = true;
					session.camera.SetEnableUserInput(this.userCameraActive, session.properties.isDraggingBuilding, default(Vector3));
					if (!session.properties.isDraggingBuildingAndScreen)
					{
						session.camera.ChangeState(SBCamera.State.Dragging);
						session.properties.isDraggingBuildingAndScreen = true;
					}
				}
				break;
			}
			case YGEvent.TYPE.TOUCH_MOVE:
			{
				if (!this.m_bTouchBegan)
				{
					session.properties.isDraggingBuildingAndScreen = false;
					session.properties.isInteractionStripActive = true;
					this.m_bTouchBegan = true;
					if (session.game.selected != null)
					{
						session.properties.startedTouchOnEmptySpace = false;
						this.clickedSim = session.game.selected;
						session.properties.isDraggingBuilding = true;
						session.AddAsyncResponse("override_drag", new object());
					}
				}
				object obj = null;
				if (!session.properties.startedTouchOnEmptySpace && session.properties.isDraggingBuilding)
				{
					obj = session.CheckAsyncRequest("override_drag");
				}
				if (obj != null)
				{
					session.AddAsyncResponse("override_drag", obj);
					base.SnapSelectedToInputPosition(session, evt.position, true, true);
					this.userCameraActive = true;
					session.camera.SetEnableUserInput(this.userCameraActive, session.properties.isDraggingBuilding, default(Vector3));
					if (!session.properties.isDraggingBuildingAndScreen)
					{
						session.camera.ChangeState(SBCamera.State.Dragging);
						session.properties.isDraggingBuildingAndScreen = true;
					}
				}
				else if ((evt.position - evt.startPosition).SqrMagnitude() > 400f)
				{
					session.properties.waitToDecidePlacement = true;
					if (session.game.selected.InteractionState.Controls != null && session.game.selected.InteractionState.Controls.Count > 0 && !session.game.selected.InteractionState.IsEditable)
					{
						this.interactionStrip.ActivateOnSelected(session);
					}
					session.game.selected.FootprintVisible = true;
					this.userCameraActive = true;
					session.camera.SetEnableUserInput(this.userCameraActive, session.properties.isDraggingBuilding, default(Vector3));
					object obj2 = session.CheckAsyncRequest("panning_event");
					if (obj2 != null)
					{
						session.camera.ProcessExtraGuiEvent((SBGUIEvent)obj2);
					}
					if (session.properties.isInteractionStripActive)
					{
						this.interactionStrip.EnableButtons(session, false);
						session.properties.isInteractionStripActive = false;
					}
					if (session.properties.firstEntered)
					{
						session.properties.firstEntered = false;
					}
				}
				break;
			}
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00072E20 File Offset: 0x00071020
		protected override void AcceptPlacement(Session session)
		{
			session.TheSoundEffectManager.PlaySound("EditModePlace");
			if (base.WasFromInventory(session))
			{
				session.game.selected.entity.Variable["associated_entities"] = session.CheckAsyncRequest("AssociatedEntities");
				session.game.simulation.Router.Send(CompleteCommand.Create(Identity.Null(), session.game.selected.Id));
			}
			else if (session.game.selected.Position != session.properties.preMovePosition || session.game.selected.Flip != session.properties.preMoveFlip)
			{
				session.game.simulation.ModifyGameStateSimulated(session.game.selected, new MoveAction(session.game.selected, null));
				session.game.selected.Animate(session.game.simulation);
				GridPosition rhs = session.game.terrain.ComputeGridPosition(session.properties.preMovePosition);
				GridPosition lhs = session.game.terrain.ComputeGridPosition(session.game.selected.Position);
				float magnitude = (lhs - rhs).ToVector2().magnitude;
				if (session.game.selected.HasEntity<StructureDecorator>() && session.game.selected.GetEntity<StructureDecorator>().IsObstacle)
				{
					Vector2 offset = session.properties.preMovePosition - session.game.selected.Position;
					AlignedBox box = session.game.selected.Box.OffsetByVector(offset);
					session.game.terrain.SetOrClearObstacle(box, false);
				}
				session.analytics.LogMoveObject(session.game.selected.entity.BlueprintName, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount, magnitude, session.framerateWatcher.Framerate);
			}
			base.AcceptPlacement(session);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00073060 File Offset: 0x00071260
		public void DeactivateInteractionStrip(Session session)
		{
			this.interactionStrip.Deactivate(session);
		}

		// Token: 0x04000BD7 RID: 3031
		private bool m_bTouchBegan;

		// Token: 0x04000BD8 RID: 3032
		private bool userCameraActive;

		// Token: 0x04000BD9 RID: 3033
		private Simulated clickedSim;
	}

	// Token: 0x020001F8 RID: 504
	public class MoveBuildingInPlacement : Session.MoveBuilding
	{
		// Token: 0x060010FF RID: 4351 RVA: 0x000730A8 File Offset: 0x000712A8
		public override void OnEnter(Session session)
		{
			this.m_bTouchBegan = false;
			Action<Session> handler = delegate(Session sesh)
			{
				this.AcceptPlacement(sesh);
				session.properties.waitToDecidePlacement = true;
				session.properties.preMovePositionSet = false;
				sesh.ChangeState("Playing", true);
			};
			Action<Session> handler2 = delegate(Session sesh)
			{
				this.DenyPlacement(sesh);
			};
			session.properties.editingHud = SBUIBuilder.MakeAndPushEmptyUI(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent));
			SBGUIEvent sbguievent = (SBGUIEvent)session.CheckAsyncRequest("currentUiEvt");
			if (sbguievent != null)
			{
				this.HandleSBGUIEvent(sbguievent, session);
			}
			this.interactionStrip.ActivateOnSelected(session);
			this.interactionStrip.SetAcceptHandler(session, handler);
			this.interactionStrip.SetRejectHandler(session, handler2);
			if (!session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
			{
				this.interactionStrip.EnableRejectButton(session, false);
			}
			session.properties.preMovePosition = session.game.selected.Position;
			session.properties.preMoveFlip = session.game.selected.Flip;
			this.m_bTouchBegan = true;
			base.OnEnter(session);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00073204 File Offset: 0x00071404
		protected override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			TFUtils.Assert(session.game != null, "Should not be in MoveBuildingInPlacement w/out a game!");
			if (session.game == null)
			{
				TFUtils.ErrorLog("Should not be in MoveBuildingInPlacement w/out a game!");
				TFUtils.LogDump(session, "error", null, null);
				return;
			}
			TFUtils.Assert(session.game.selected != null, "Should not be in MoveBuildingInPlacement w/out a selected sim!");
			if (session.game.selected == null)
			{
				TFUtils.ErrorLog("Should not be in MoveBuildingInPlacement w/out a selected sim!");
				TFUtils.LogDump(session, "error", null, null);
				return;
			}
			TFUtils.Assert(session.game.selected.Id != null, "Should not be in MoveBuildingInPlacement w/out a selected sim id!");
			if (session.game.selected.Id == null)
			{
				TFUtils.ErrorLog("Should not be in MoveBuildingInPlacement w/out a selected sim id!");
				TFUtils.LogDump(session, "error", null, null);
				return;
			}
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_BEGIN:
			{
				Predicate<Simulated> filterOutMatching = (Simulated simulated) => !simulated.InteractionState.IsEditable && !Session.TheDebugManager.debugPlaceObjects;
				Ray ray;
				Simulated simulated2 = Session.FindAlreadySelected(filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out ray, session.game.selected);
				if (simulated2 == null)
				{
					simulated2 = Session.FindBestSimulatedUnderPoint(new Session.SelectionPrioritizer(session.camera.UnityCamera), filterOutMatching, session.game.simulation, session.TheCamera, evt.position, out ray);
				}
				session.properties.isInteractionStripActive = true;
				session.properties.isDraggingBuilding = false;
				this.m_bTouchBegan = true;
				if (simulated2 == session.game.selected && !Input.GetMouseButtonDown(1))
				{
					session.properties.isDraggingBuilding = true;
					session.AddAsyncResponse("override_drag", new object());
				}
				else
				{
					session.properties.isDraggingBuilding = false;
				}
				session.AddAsyncResponse("panning_event", evt);
				break;
			}
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
			case YGEvent.TYPE.TAP:
			case YGEvent.TYPE.RESET:
			case YGEvent.TYPE.DISABLE:
				if (this.m_bTouchBegan)
				{
					if (session.properties.isDraggingBuilding)
					{
						session.CheckAsyncRequest("override_drag");
						base.SnapSelectedToInputPosition(session, evt.position, true, true);
					}
					session.CheckAsyncRequest("panning_event");
					this.m_bTouchBegan = false;
				}
				if (this.userCameraActive)
				{
					this.userCameraActive = false;
					if (session.properties.isDraggingBuilding)
					{
						session.camera.SetEnableUserInput(this.userCameraActive, session.properties.isDraggingBuilding, this.interactionStrip.StripPosition);
					}
					else
					{
						session.camera.SetEnableUserInput(this.userCameraActive, false, default(Vector3));
						if (!session.properties.isInteractionStripActive)
						{
							session.properties.isInteractionStripActive = true;
							this.interactionStrip.EnableButtons(session, true);
						}
						this.interactionStrip.OnUpdate(session);
					}
				}
				session.properties.waitToDecidePlacement = false;
				session.properties.isDraggingBuildingAndScreen = false;
				session.properties.isDraggingBuilding = false;
				break;
			case YGEvent.TYPE.TOUCH_STAY:
			{
				object obj = null;
				if (!session.properties.startedTouchOnEmptySpace && session.properties.isDraggingBuilding)
				{
					obj = session.CheckAsyncRequest("override_drag");
				}
				if (obj != null)
				{
					session.AddAsyncResponse("override_drag", obj);
					base.SnapSelectedToInputPosition(session, evt.position, true, true);
					this.userCameraActive = true;
					session.camera.SetEnableUserInput(this.userCameraActive, session.properties.isDraggingBuilding, default(Vector3));
					if (!session.properties.isDraggingBuildingAndScreen && session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
					{
						session.camera.ChangeState(SBCamera.State.Dragging);
						session.properties.isDraggingBuildingAndScreen = true;
					}
				}
				break;
			}
			case YGEvent.TYPE.TOUCH_MOVE:
				if (this.m_bTouchBegan)
				{
					object obj = session.CheckAsyncRequest("override_drag");
					if (obj != null && session.properties.isDraggingBuilding)
					{
						this.userCameraActive = true;
						session.camera.SetEnableUserInput(this.userCameraActive, session.properties.isDraggingBuilding, default(Vector3));
						if (!session.properties.isDraggingBuildingAndScreen && session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
						{
							session.camera.ChangeState(SBCamera.State.Dragging);
							session.properties.isDraggingBuildingAndScreen = true;
						}
						session.AddAsyncResponse("override_drag", obj);
						base.SnapSelectedToInputPosition(session, evt.position, true, true);
					}
					else if ((evt.position - evt.startPosition).SqrMagnitude() > 400f && session.game.simulation.featureManager.CheckFeature("move_reject_lock"))
					{
						session.properties.waitToDecidePlacement = true;
						session.ChangeState("MoveBuildingPanningInPlacement", true);
						if (session.properties.isInteractionStripActive)
						{
							this.interactionStrip.EnableButtons(session, false);
							session.properties.isInteractionStripActive = false;
						}
					}
				}
				break;
			}
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00073748 File Offset: 0x00071948
		protected override void AcceptPlacement(Session session)
		{
			BuildingEntity entity = session.game.selected.GetEntity<BuildingEntity>();
			if (entity.Invariable.ContainsKey("accept_placement_sound"))
			{
				session.game.simulation.soundEffectManager.PlaySound((string)entity.Invariable["accept_placement_sound"]);
			}
			else
			{
				session.game.simulation.soundEffectManager.PlaySound("PlaceBuilding");
			}
			if (session.properties.cameFromMarketplace)
			{
				Simulated simulated = session.game.simulation.SpawnWorker(session.game.selected);
				ErectableDecorator decorator = entity.GetDecorator<ErectableDecorator>();
				session.game.simulation.Router.Send(EmployCommand.Create(Identity.Null(), session.game.selected.Id, simulated.Id));
				session.game.simulation.Router.Send(CompleteCommand.Create(Identity.Null(), session.game.selected.Id), decorator.ErectionTime);
				session.game.simulation.Router.Send(CompleteCommand.Create(session.game.selected.Id, simulated.Id), decorator.ErectionTime);
				session.game.simulation.Router.Send(ReturnCommand.Create(session.game.selected.Id, simulated.Id), decorator.ErectionTime);
				Cost cost = session.game.catalog.GetCost(entity.DefinitionId);
				TFUtils.Assert(cost != null, "Expected building to have a cost when placing after purchase");
				session.game.resourceManager.Apply(cost, session.game);
				bool decoration = entity.DefinitionId >= 2000 && entity.DefinitionId < 3000;
				bool premium = cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY);
				session.analytics.LogPlacement(entity.BlueprintName, decoration, premium, cost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount, session.framerateWatcher.Framerate);
				AnalyticsWrapper.LogItemPlacement(session.TheGame, session.game.selected.Entity, false, true);
				session.properties.storeVisitSinceLastPurchase = 0;
				Session.Shopping.FireFinishShoppingEvent(session);
			}
			else if (Session.TheDebugManager.debugPlaceObjects || session.game.simulation.PlacementQuery(session.game.selected, false) != Simulation.Placement.RESULT.INVALID)
			{
				if (base.WasFromInventory(session))
				{
					AnalyticsWrapper.LogItemPlacement(session.TheGame, session.game.selected.Entity, true, true);
					session.game.selected.entity.Variable["associated_entities"] = session.CheckAsyncRequest("AssociatedEntities");
					session.game.simulation.Router.Send(CompleteCommand.Create(Identity.Null(), session.game.selected.Id));
					session.CheckAsyncRequest("FromInventory");
					session.analytics.LogPlacementFromInventory(entity.BlueprintName, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
				}
				else
				{
					TFUtils.Assert(!session.properties.cameFromMarketplace, "You should not get here if you came from the marketplace!");
					session.game.simulation.ModifyGameStateSimulated(session.game.selected, new MoveAction(session.game.selected, null));
				}
			}
			base.AcceptPlacement(session);
		}

		// Token: 0x04000BDB RID: 3035
		public const string CURRENT_UI_EVENT = "currentUiEvt";

		// Token: 0x04000BDC RID: 3036
		private bool isTutorialPointerOnStrip;

		// Token: 0x04000BDD RID: 3037
		private bool m_bTouchBegan;

		// Token: 0x04000BDE RID: 3038
		private bool userCameraActive;
	}

	// Token: 0x020001F9 RID: 505
	public class MoveBuildingPanningInEdit : Session.State
	{
		// Token: 0x06001104 RID: 4356 RVA: 0x00073B44 File Offset: 0x00071D44
		public void OnEnter(Session session)
		{
			if (session.game.selected == null)
			{
				TFUtils.ErrorLog("MoveBuildingPanningInEdit.OnEnter - selected is null");
				session.ChangeState("Playing", true);
				return;
			}
			Action inventoryClickHandler = delegate()
			{
				if (!session.game.featureManager.CheckFeature("inventory_soft"))
				{
					session.game.featureManager.UnlockFeature("inventory_soft");
					session.game.featureManager.ActivateFeatureActions(session.game, "inventory_soft");
					session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string>
					{
						"inventory_soft"
					}));
					return;
				}
				session.ChangeState("Inventory", true);
			};
			Action optionsHandler = delegate()
			{
				session.ChangeState("Options", true);
			};
			Action editClickHandler = delegate()
			{
				session.ChangeState("Playing", true);
			};
			SBGUIStandardScreen val = SBUIBuilder.MakeAndPushStandardUI(session, true, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), null, inventoryClickHandler, optionsHandler, editClickHandler, null, null, null, null, null, null, null, true);
			session.AddAsyncResponse("standard_screen", val);
			if (session.game.selected.InteractionState.Controls != null && session.game.selected.InteractionState.Controls.Count > 0 && !session.game.selected.InteractionState.IsEditable)
			{
				this.interactionStrip.ActivateOnSelected(session);
			}
			session.camera.SetEnableUserInput(true, false, default(Vector3));
			object obj = session.CheckAsyncRequest("panning_event");
			if (obj != null)
			{
				session.camera.ProcessExtraGuiEvent((SBGUIEvent)obj);
			}
			session.game.selected.FootprintVisible = true;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00073CD4 File Offset: 0x00071ED4
		public void OnLeave(Session session)
		{
			this.interactionStrip.Deactivate(session);
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00073CE4 File Offset: 0x00071EE4
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00073D48 File Offset: 0x00071F48
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
				session.ChangeState("MoveBuildingInEdit", true);
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				this.interactionStrip.OnUpdate(session);
				break;
			}
		}

		// Token: 0x04000BE0 RID: 3040
		private Session.InteractionStripMixin interactionStrip = new Session.InteractionStripMixin();
	}

	// Token: 0x020001FA RID: 506
	public class MoveBuildingPanningInPlacement : Session.State
	{
		// Token: 0x06001109 RID: 4361 RVA: 0x00073DB0 File Offset: 0x00071FB0
		public void OnEnter(Session session)
		{
			SBGUIScreen item = SBUIBuilder.MakeAndPushEmptyUI(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent));
			this.interactionStrip.ActivateOnSelected(session);
			session.camera.SetEnableUserInput(true, false, default(Vector3));
			session.game.sessionActionManager.SetActionHandler("movebuildingpanninginplacement_ui", session, new List<SBGUIScreen>
			{
				item
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			SessionActionSimulationHelper.EnableHandler(session, true);
			object obj = session.CheckAsyncRequest("panning_event");
			if (obj != null)
			{
				session.camera.ProcessExtraGuiEvent((SBGUIEvent)obj);
			}
			session.game.selected.FootprintVisible = true;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00073E60 File Offset: 0x00072060
		public void OnLeave(Session session)
		{
			this.interactionStrip.Deactivate(session);
			SBUIBuilder.ReleaseTopScreen();
			session.game.sessionActionManager.ClearActionHandler("movebuildingpanninginplacement_ui", session);
			session.AddAsyncResponse("silent_enter", new object());
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00073EA8 File Offset: 0x000720A8
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00073F0C File Offset: 0x0007210C
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_END:
			case YGEvent.TYPE.TOUCH_CANCEL:
				if (!session.properties.isInteractionStripActive)
				{
					session.properties.isInteractionStripActive = true;
					this.interactionStrip.EnableButtons(session, true);
				}
				session.ChangeState("MoveBuildingInPlacement", true);
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				this.interactionStrip.OnUpdate(session);
				break;
			}
		}

		// Token: 0x04000BE1 RID: 3041
		private const string MOVEBUILDING_UI_HANDLER = "movebuildingpanninginplacement_ui";

		// Token: 0x04000BE2 RID: 3042
		private Session.InteractionStripMixin interactionStrip = new Session.InteractionStripMixin();
	}

	// Token: 0x020001FB RID: 507
	public class Movie : Session.State
	{
		// Token: 0x0600110E RID: 4366 RVA: 0x00073F90 File Offset: 0x00072190
		public void OnEnter(Session session)
		{
			TFUtils.PlayMovie(this.movie);
			session.AddAsyncResponse("MovieStartTime", TFUtils.EpochTime());
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00073FC0 File Offset: 0x000721C0
		public void OnLeave(Session session)
		{
			Thread.Sleep(0);
			AudioSettings.speakerMode = AudioSettings.driverCaps;
			session.soundEffectManager.Clear();
			int playerLevel = 0;
			if (session.game != null)
			{
				playerLevel = session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount;
			}
			session.analytics.LogPlayMovie(this.movie, TFUtils.EpochTime() - (ulong)session.CheckAsyncRequest("MovieStartTime"), playerLevel);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00074040 File Offset: 0x00072240
		public void OnUpdate(Session session)
		{
			session.ChangeState(this.nextSession, true);
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x00074050 File Offset: 0x00072250
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x00074058 File Offset: 0x00072258
		public string TheMovie
		{
			get
			{
				return this.movie;
			}
			set
			{
				this.movie = value;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x00074064 File Offset: 0x00072264
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x0007406C File Offset: 0x0007226C
		public string NextSessionState
		{
			get
			{
				return this.nextSession;
			}
			set
			{
				this.nextSession = value;
			}
		}

		// Token: 0x04000BE3 RID: 3043
		private string movie;

		// Token: 0x04000BE4 RID: 3044
		private string nextSession;
	}

	// Token: 0x020001FC RID: 508
	public class Options : Session.State
	{
		// Token: 0x06001116 RID: 4374 RVA: 0x00074080 File Offset: 0x00072280
		public void OnEnter(Session session)
		{
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.PlaySeaflowerAndBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false, false, default(Vector3));
			TFUtils.ErrorLog(string.Empty);
			Action action = delegate()
			{
				session.ChangeState("Playing", true);
				AndroidBack.getInstance().pop();
			};
			Action action2 = delegate()
			{
			};
			Action parentsHandler = delegate()
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_NEED_NETWORK_TITLE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
					{
						item
					}, uint.MaxValue);
					session.ChangeState("ShowingDialog", true);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					session.ChangeState("AgeGate", true);
				}
			};
			Action moreNickHandler = delegate()
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_MORE_NICK_MESSAGE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
					{
						item
					}, uint.MaxValue);
					session.ChangeState("ShowingDialog", true);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					session.PlayHavenController.RequestContent("more_nick_click");
				}
			};
			Action toggleSFXHandler = delegate()
			{
				if (session.soundEffectManager.Enabled)
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					session.soundEffectManager.ToggleOnOff();
				}
				else
				{
					session.soundEffectManager.ToggleOnOff();
					session.TheSoundEffectManager.PlaySound("Accept");
				}
			};
			Action toggleMusicHandler = delegate()
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				session.musicManager.ToggleOnOff();
			};
			Action creditsHandler = delegate()
			{
				session.TheSoundEffectManager.PlaySound("tutorial_arrow");
				if (session.InFriendsGame && session.properties.optionsHud != null)
				{
					session.properties.optionsHud.SetActive(false);
				}
				session.ChangeState("Credits", true);
			};
			Action achievementsHandler = delegate()
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				if (GameCenterBinding.isPlayerAuthenticated())
				{
					GameCenterBinding.showAchievements();
				}
				else
				{
					GameCenterBinding.authenticateLocalPlayer();
				}
			};
			Action privacyHandler = delegate()
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_NEED_NETWORK_TITLE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
					{
						item
					}, uint.MaxValue);
					session.ChangeState("ShowingDialog", true);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					Application.OpenURL(TFUtils.GetPrivacy_Address());
				}
			};
			Action eulaHandler = delegate()
			{
				if (Application.internetReachability == NetworkReachability.NotReachable)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!ERROR_NEED_NETWORK_TITLE"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
					{
						item
					}, uint.MaxValue);
					session.ChangeState("ShowingDialog", true);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Accept");
					Application.OpenURL(TFUtils.GetEULA_Address());
				}
			};
			Action debugHandler = delegate()
			{
				session.TheSoundEffectManager.PlaySound("Accept");
				session.ChangeState("Debug", true);
			};
			session.properties.optionsHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, action2, action2, action, action2, null, Session.DragFeeding.SwitchToFn(session), null, action2, action2, action2, action2, false);
			session.properties.optionsHud.SetActive(true);
			session.properties.optionsHud.SetVisibleNonEssentialElements(false, true);
			SBGUIPulseButton sbguipulseButton = (SBGUIPulseButton)session.properties.optionsHud.FindChild("marketplace");
			sbguipulseButton.SetActive(false);
			SBGUIPulseButton sbguipulseButton2 = (SBGUIPulseButton)session.properties.optionsHud.FindChild("community_event");
			sbguipulseButton2.SetActive(false);
			if (session.InFriendsGame)
			{
				SBGUIElement sbguielement = session.properties.optionsHud.FindChild("happyface");
				sbguielement.SetActive(false);
				SBGUIElement sbguielement2 = session.properties.optionsHud.FindChild("quest_marker");
				sbguielement2.SetActive(false);
				SBGUIElement sbguielement3 = session.properties.optionsHud.FindChild("jj_bar");
				sbguielement3.SetActive(false);
				SBGUIElement sbguielement4 = session.properties.optionsHud.FindChild("money_bar");
				sbguielement4.SetActive(false);
				SBGUIElement sbguielement5 = session.properties.optionsHud.FindChild("special_bar");
				if (sbguielement5)
				{
					sbguielement5.SetActive(false);
				}
			}
			SBGUIScreen sbguiscreen = SBUIBuilder.MakeAndPushOptionsDialog(action, moreNickHandler, toggleSFXHandler, toggleMusicHandler, achievementsHandler, creditsHandler, privacyHandler, eulaHandler, debugHandler, parentsHandler);
			YGTextAtlasSprite component = sbguiscreen.FindChild("player_id_label").GetComponent<YGTextAtlasSprite>();
			component.Text = Soaring.Player.UserTag;
			session.TheCamera.TurnOnScreenBuffer();
			SBGUIElement sbguielement6 = sbguiscreen.FindChild("red_counter");
			SBGUILabel sbguilabel = (SBGUILabel)sbguiscreen.FindChild("red_counter_label");
			int notificationCount = HelpshiftSdk.getInstance().getNotificationCount(false);
			HelpshiftSdk.getInstance().getNotificationCount(true);
			sbguilabel.SetText(notificationCount.ToString());
			if (notificationCount > 0)
			{
				sbguilabel.SetActive(true);
				sbguielement6.SetActive(true);
			}
			else
			{
				sbguilabel.SetActive(false);
				sbguielement6.SetActive(false);
			}
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x000743F4 File Offset: 0x000725F4
		public void OnLeave(Session session)
		{
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.properties.optionsHud.SetVisibleNonEssentialElements(true);
			session.properties.optionsHud = null;
			session.TheCamera.TurnOffScreenBuffer();
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0007443C File Offset: 0x0007263C
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
		}
	}

	// Token: 0x020001FD RID: 509
	public class Paving : Session.State
	{
		// Token: 0x0600111A RID: 4378 RVA: 0x00074478 File Offset: 0x00072678
		public Paving()
		{
			this.workingList = new List<PaveAction.PaveElement>();
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0007448C File Offset: 0x0007268C
		public void OnEnter(Session session)
		{
			this.segmentCost = Cost.FromDict(new Dictionary<string, object>
			{
				{
					"3",
					5
				}
			});
			this.totalCost = new Cost();
			Action inventoryHandler = delegate()
			{
				if (!session.TheGame.featureManager.CheckFeature("inventory_soft"))
				{
					session.TheGame.featureManager.UnlockFeature("inventory_soft");
					session.TheGame.featureManager.ActivateFeatureActions(session.TheGame, "inventory_soft");
					session.TheGame.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string>
					{
						"inventory_soft"
					}));
					return;
				}
				session.AddAsyncResponse("FromEdit", true);
				session.ChangeState("Inventory", true);
			};
			SBUIBuilder.MakeAndPushPavingUI(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), delegate
			{
				session.ChangeState("Editing", true);
			}, delegate
			{
				session.ChangeState("Playing", true);
			}, inventoryHandler);
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.camera.SetEnableUserInput(true, false, default(Vector3));
			this.cannotPay = 0;
			this.placed = 0;
			this.removed = 0;
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00074594 File Offset: 0x00072794
		public void OnLeave(Session session)
		{
			if (this.workingList != null && this.workingList.Count > 0)
			{
				List<PaveAction.PaveElement> path = this.workingList;
				this.workingList = new List<PaveAction.PaveElement>();
				session.game.simulation.ModifyGameState(new PaveAction(new Identity(), path, this.totalCost));
			}
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x000745F8 File Offset: 0x000727F8
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			YGEvent.TYPE type = evt.type;
			if (type == YGEvent.TYPE.TAP)
			{
				Ray ray = Camera.main.ScreenPointToRay(evt.position);
				Vector3 vector;
				session.game.terrain.ComputeIntersection(ray, out vector);
				Vector2 vector2;
				vector2.x = vector.x;
				vector2.y = vector.y;
				GridPosition gpos = session.game.terrain.ComputeGridPosition(vector2);
				TerrainType terrainType = session.game.terrain.GetTerrainType(gpos);
				if ((terrainType == null || terrainType.CanPave()) && session.game.terrain.CheckIsPurchasedArea(vector2))
				{
					bool flag = terrainType != null && !terrainType.IsPath();
					if (flag && !Session.TheDebugManager.debugPlaceObjects)
					{
						if (session.game.simulation.PlacementQuery(session.game.terrain.GetGridBounds(gpos.row, gpos.col), null, true) == Simulation.Placement.RESULT.INVALID)
						{
							session.TheSoundEffectManager.PlaySound("Cancel");
							return;
						}
						if (!session.game.resourceManager.CanPay(this.segmentCost))
						{
							this.cannotPay++;
							session.TheSoundEffectManager.PlaySound("Cancel");
							return;
						}
					}
					if (session.game.terrain.ChangePath(gpos))
					{
						if (flag)
						{
							if (!Session.TheDebugManager.debugPlaceObjects)
							{
								session.game.resourceManager.Apply(this.segmentCost, session.game);
								this.totalCost += this.segmentCost;
							}
							session.TheSoundEffectManager.PlaySound("PlacePavement");
							this.placed++;
						}
						else
						{
							if (!Session.TheDebugManager.debugPlaceObjects)
							{
								session.game.resourceManager.SellFor(this.segmentCost, session.game);
								this.totalCost -= this.segmentCost;
							}
							session.TheSoundEffectManager.PlaySound("RemovePavement");
							this.removed++;
						}
						int num = this.workingList.FindIndex((PaveAction.PaveElement p) => p.position == gpos);
						if (num < 0)
						{
							this.workingList.Add(new PaveAction.PaveElement(gpos));
						}
						else
						{
							this.workingList.RemoveAt(num);
						}
					}
				}
			}
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x000748C4 File Offset: 0x00072AC4
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x04000BE6 RID: 3046
		private List<PaveAction.PaveElement> workingList;

		// Token: 0x04000BE7 RID: 3047
		private Cost segmentCost;

		// Token: 0x04000BE8 RID: 3048
		private Cost totalCost;

		// Token: 0x04000BE9 RID: 3049
		private int placed;

		// Token: 0x04000BEA RID: 3050
		private int removed;

		// Token: 0x04000BEB RID: 3051
		private int cannotPay;
	}

	// Token: 0x020001FE RID: 510
	public class Placing : Session.State
	{
		// Token: 0x06001120 RID: 4384 RVA: 0x00074930 File Offset: 0x00072B30
		public void OnEnter(Session session)
		{
			SBUIBuilder.MakeAndPushEmptyUI(session, null);
			SBUIBuilder.ReleaseTimebars();
			SBUIBuilder.ReleaseNamebars();
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x000749BC File Offset: 0x00072BBC
		public void OnLeave(Session session)
		{
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x000749C0 File Offset: 0x00072BC0
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			if (!this.PlayerCanAfford(session, session.game.selected))
			{
				TFUtils.DebugLog("You cannot afford that!");
				session.game.selected.SetFootprint(session.game.simulation, false);
				session.game.simulation.RemoveSimulated(session.game.selected);
				session.game.entities.Destroy(session.game.selected.Id);
				session.ChangeState("Shopping", true);
				session.game.selected = null;
			}
			else
			{
				Ray ray = session.camera.ScreenPointToRay(session.camera.ScreenCenter);
				Vector3 vector = new Vector3(0f, 0f, 0f);
				Dictionary<int, Vector2> dictionary = (Dictionary<int, Vector2>)session.CheckAsyncRequest("preplace_request_dict");
				if (dictionary != null)
				{
					if (dictionary.ContainsKey(session.game.selected.entity.DefinitionId))
					{
						vector.x = dictionary[session.game.selected.entity.DefinitionId].x;
						vector.y = dictionary[session.game.selected.entity.DefinitionId].y;
						dictionary.Remove(session.game.selected.entity.DefinitionId);
					}
					else
					{
						session.game.terrain.ComputeIntersection(ray, out vector);
					}
					if (dictionary.Count > 0)
					{
						session.AddAsyncResponse("preplace_request_dict", dictionary);
					}
				}
				else
				{
					session.game.terrain.ComputeIntersection(ray, out vector);
				}
				session.game.selected.Warp(new Vector2(vector.x, vector.y), session.game.simulation);
				session.properties.cameFromMarketplace = true;
				session.ChangeState("MoveBuildingInPlacement", true);
			}
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00074C20 File Offset: 0x00072E20
		private bool PlayerCanAfford(Session session, Simulated simulated)
		{
			BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
			Cost cost = session.game.catalog.GetCost(entity.DefinitionId);
			return cost != null && session.game.resourceManager.CanPay(cost);
		}

		// Token: 0x04000BEC RID: 3052
		public const string FRESHLY_PURCHASED = "FreshlyPurchased";
	}

	// Token: 0x020001FF RID: 511
	public class Playing : Session.State
	{
		// Token: 0x06001126 RID: 4390 RVA: 0x00074C7C File Offset: 0x00072E7C
		public virtual void OnEnter(Session session)
		{
			Action shopClickHandler = delegate()
			{
				string storeTabValue = session.TheGame.questManager.GetStoreTabValue();
				if (!string.IsNullOrEmpty(storeTabValue))
				{
					session.AddAsyncResponse("target_store_tab", storeTabValue);
				}
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping", true);
			};
			Action inventoryClickHandler = delegate()
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory", true);
			};
			Action optionsHandler = delegate()
			{
				session.ChangeState("Options", true);
			};
			Action editClickHandler = delegate()
			{
				session.ChangeState("Editing", true);
			};
			Action openIAPTabHandlerSoft = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping", true);
			};
			Action openIAPTabHandlerHard = delegate()
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping", true);
			};
			Action communityEventClickHandler = delegate()
			{
				session.ChangeState("CommunityEvent", true);
			};
			Action patchyClickHandler = delegate()
			{
				session.TheSoundEffectManager.PlaySound("OpenMenu");
				SBUIBuilder.ReleaseTimebars();
				SBUIBuilder.ReleaseNamebars();
				if (session.InFriendsGame)
				{
					if (session.properties.playingHud != null)
					{
						session.properties.playingHud.gameObject.SetActiveRecursively(false);
					}
					else
					{
						Debug.LogError("Hud Not Set!!!!");
					}
					TFUtils.DebugLog("Switching To Park");
					session.InFriendsGame = false;
					session.TheGame.RequestReload();
				}
				else
				{
					if (!Soaring.IsOnline || SBSettings.DisableFriendPark)
					{
						Action okHandler = delegate()
						{
							SBUIBuilder.ReleaseTopScreen();
						};
						SBUIBuilder.CreateErrorDialog(session, "Error", Language.Get("!!NOTIFY_INGAME_PATCHYS_NOT_AVAILABLE"), Language.Get("!!PREFAB_OK"), okHandler, 0.85f, 0.45f);
						return;
					}
					if (session.properties.playingHud != null)
					{
						session.properties.playingHud.gameObject.SetActiveRecursively(false);
					}
					else
					{
						Debug.LogError("Hud Not Set!!!!");
					}
					session.InFriendsGame = true;
					TFUtils.DebugLog("Switching To Patchy Town");
					if (session.gameInitialized)
					{
						session.game.analytics.LogVisitPark();
						AnalyticsWrapper.LogVisitPark(session.game);
						QuestManager questManager2 = session.TheGame.questManager;
						if (Soaring.Player.CustomData == null)
						{
							SoaringDictionary soaringDictionary = new SoaringDictionary();
							soaringDictionary.addValue(new SoaringDictionary(), "custom");
							Soaring.Player.SetUserData(soaringDictionary, false);
						}
						SoaringArray soaringArray = (SoaringArray)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_fdk");
						if (soaringArray != null)
						{
							soaringArray.clear();
						}
						if (!questManager2.IsQuestCompleted(2400U) && questManager2.IsQuestActive(2400U))
						{
							soaringArray = new SoaringArray();
							Soaring.Player.PrivateData_Safe.setValue(soaringArray, "SBMI_fdk");
							soaringArray.addObject(2401L);
						}
						session.TheGame.SaveLocally(0UL, false, false, false);
						session.TheGame.RequestLoadFriendPark(null);
					}
				}
			};
			SBGUIStandardScreen screen = SBUIBuilder.MakeAndPushStandardUI(session, true, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), shopClickHandler, inventoryClickHandler, optionsHandler, editClickHandler, null, Session.DragFeeding.SwitchToFn(session), null, openIAPTabHandlerSoft, openIAPTabHandlerHard, communityEventClickHandler, patchyClickHandler, false);
			screen.EnableUI(true);
			session.AddAsyncResponse("standard_screen", screen);
			session.properties.playingHud = screen;
			Action action = delegate()
			{
				screen.RefreshQuestTrackers(session);
			};
			if (screen.ReadyEvent.IsReady)
			{
				action();
			}
			else
			{
				screen.ReadyEvent.AddListener(action);
			}
			session.properties.playingHud.ShowInventoryWidget();
			Action action2 = delegate()
			{
				session.ChangeState("ShowingDialog", true);
			};
			session.game.dropManager.DialogNeededCallback = action2;
			session.game.questManager.OnShowDialogCallback = action2;
			session.game.communityEventManager.DialogNeededCallback = action2;
			session.camera.SetEnableUserInput(true, false, default(Vector3));
			session.musicManager.PlayTrack("InGame");
			SBGUIElement sbguielement = screen.FindChild("quest");
			SBGUIElement sbguielement2 = session.properties.playingHud.FindChild("marketplace");
			SBGUIElement sbguielement3 = session.properties.playingHud.FindChild("inventory_widget");
			SBGUIElement sbguielement4 = session.properties.playingHud.FindChild("patchy_title_bg");
			SBGUIElement sbguielement5 = session.properties.playingHud.FindChild("patchy_title_icon");
			SBGUIElement sbguielement6 = session.properties.playingHud.FindChild("patchy_title_label");
			SBGUIPulseButton sbguipulseButton = (SBGUIPulseButton)session.properties.playingHud.FindChild("patchy");
			sbguipulseButton.SetActive(false);
			Vector3 localPosition = sbguipulseButton.gameObject.transform.localPosition;
			sbguipulseButton.gameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, 0.9f);
			QuestManager questManager = session.game.questManager;
			SBGUIElement sbguielement7 = session.properties.playingHud.FindChild("inventory");
			SBGUIElement sbguielement8 = session.properties.playingHud.FindChild("edit");
			SBGUIElement sbguielement9 = session.properties.playingHud.FindChild("settings");
			SBGUIElement sbguielement10 = session.properties.playingHud.FindChild("red_counter");
			if (sbguielement9 != null)
			{
				sbguielement9.SetActive(true);
			}
			session.properties.playingHud.ShowCurrencies();
			if (session.InFriendsGame)
			{
				SBGUIElement sbguielement11 = session.properties.playingHud.FindChild("happyface");
				SBGUIElement sbguielement12 = session.properties.playingHud.FindChild("jj_bar");
				SBGUIElement sbguielement13 = session.properties.playingHud.FindChild("money_bar");
				SBGUIElement sbguielement14 = session.properties.playingHud.FindChild("special_bar");
				SBGUIElement sbguielement15 = session.properties.playingHud.FindChild("community_event");
				SBGUIElement sbguielement16 = session.properties.playingHud.FindChild("quest_scroll_down");
				SBGUIElement sbguielement17 = session.properties.playingHud.FindChild("quest_marker");
				SBGUIElement sbguielement18 = session.properties.playingHud.FindChild("quest_scroll_up");
				sbguipulseButton.SetActive(true);
				sbguipulseButton.tform.localPosition = new Vector3(1.538977f, 0.65f, 1f);
				sbguipulseButton.tform.localScale = new Vector3(1f, 1f, 1f);
				sbguipulseButton.SetTextureFromAtlas("GoHomeMenuButton.png", true, false, false, 0);
				sbguielement2.SetActive(false);
				sbguielement.SetActive(false);
				sbguielement3.SetActive(false);
				if (sbguielement12 != null)
				{
					sbguielement12.SetActive(false);
				}
				if (sbguielement13 != null)
				{
					sbguielement13.SetActive(false);
				}
				if (sbguielement14 != null)
				{
					sbguielement14.SetActive(false);
				}
				if (sbguielement15 != null)
				{
					sbguielement15.SetActive(false);
				}
				if (sbguielement7 != null)
				{
					sbguielement7.SetActive(false);
				}
				if (sbguielement8 != null)
				{
					sbguielement8.SetActive(false);
				}
				if (sbguielement16 != null)
				{
					sbguielement16.SetActive(false);
					sbguielement16.SetVisible(false);
				}
				if (sbguielement17 != null)
				{
					sbguielement17.SetActive(false);
				}
				if (sbguielement18 != null)
				{
					sbguielement18.SetActive(false);
					sbguielement18.SetVisible(false);
				}
				if (sbguielement4 != null)
				{
					sbguielement4.SetActive(true);
				}
				if (sbguielement5 != null)
				{
					sbguielement5.SetActive(true);
				}
				if (sbguielement6 != null)
				{
					sbguielement6.SetActive(true);
				}
				if (sbguielement11 != null)
				{
					sbguielement11.SetActive(false);
				}
			}
			else
			{
				sbguielement.SetActive(true);
				sbguielement2.SetActive(true);
				sbguielement3.SetActive(true);
				sbguielement4.SetActive(false);
				sbguielement5.SetActive(false);
				sbguielement6.SetActive(false);
				if (sbguielement7 != null)
				{
					sbguielement7.SetActive(true);
				}
				if (sbguielement8 != null)
				{
					sbguielement8.SetActive(true);
				}
				bool flag = questManager.IsQuestActive(2400U);
				if (!flag)
				{
					flag = questManager.IsQuestCompleted(2400U);
				}
				if (SBSettings.DisableFriendPark)
				{
					flag = false;
				}
				sbguipulseButton.SetActive(flag);
				if (session.WasInFriendsGame)
				{
					session.TheCamera.ResetCameraPosition();
					session.musicManager.Enabled = session.musicStateBeforePT;
					if (!session.musicStateBeforePT)
					{
						session.musicManager.StopTrack();
					}
					session.soundEffectManager.Enabled = session.sfxStateBeforePT;
					session.WasInFriendsGame = false;
				}
				if (SBGUIStandardScreen.userClosedWishList)
				{
					session.properties.playingHud.CloseInventoryWidget();
				}
			}
			int notificationCount = HelpshiftSdk.getInstance().getNotificationCount(false);
			HelpshiftSdk.getInstance().getNotificationCount(true);
			session.properties.playingHud.HelpshiftNotificationCount = notificationCount;
			session.properties.playingHud.ShowHelpshiftNotification();
			session.game.sessionActionManager.SetActionHandler("playing_ui", session, new List<SBGUIScreen>
			{
				screen
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			SessionActionSimulationHelper.EnableHandler(session, true);
			Session.GameStarting.ResetShowSplashScreen(Session.GameStarting.SplashScreenState.None);
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00075494 File Offset: 0x00073694
		public virtual void OnLeave(Session session)
		{
			session.properties.playingHud = null;
			if (session.InFriendsGame && !session.WasInFriendsGame)
			{
				session.sfxStateBeforePT = session.soundEffectManager.Enabled;
				session.musicStateBeforePT = session.musicManager.Enabled;
			}
			session.CheckAsyncRequest("standard_screen");
			session.game.sessionActionManager.ClearActionHandler("playing_ui", session);
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00075508 File Offset: 0x00073708
		public virtual void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			if (session.properties.playingHud != null)
			{
				session.properties.playingHud.EnableUI(true);
			}
			session.camera.SetEnableUserInput(true, false, default(Vector3));
			YGEvent.TYPE type = evt.type;
			switch (type)
			{
			case YGEvent.TYPE.TOUCH_BEGIN:
				Session.draggingCamera = false;
				if (!Session.draggingCamera)
				{
					Ray ray;
					List<Simulated> list = Session.FindSimulatedsUnderPoint(null, session.game.simulation, session.TheCamera, evt.position, out ray);
					Session.SelectionPrioritizer selectionPrioritizer = new Session.SelectionPrioritizer(session.TheCamera.UnityCamera);
					list.ForEach(new Action<Simulated>(selectionPrioritizer.SelectBest));
					Simulated best = selectionPrioritizer.Best;
					if (!session.game.dropManager.ProcessTap(session, ray))
					{
						TerrainSlot terrainSlot = session.game.terrain.CheckTap(ray, session.TheGame);
						if (best != null)
						{
							if (best.HasEntity<ResidentEntity>() && best.ThoughtDisplayController.isVisible && best.ThoughtDisplayController.Intersects(ray))
							{
								session.properties.tappedDisplayController = best.ThoughtDisplayController;
							}
							session.properties.touchingSim = best;
							session.properties.touchingSim.BounceStart();
						}
						session.properties.moveDragStart = evt.position;
						session.PlayTapParticleEffect(session.TheCamera.ScreenPointToWorldPoint(session.game.terrain, evt.position));
						if (terrainSlot != null)
						{
							session.CheckAsyncRequest("clear_purchase_on_movement");
							session.AddAsyncResponse("clear_purchase_on_movement", terrainSlot);
						}
					}
				}
				break;
			case YGEvent.TYPE.TOUCH_END:
			{
				Ray ray;
				List<Simulated> list = Session.FindSimulatedsUnderPoint(null, session.game.simulation, session.TheCamera, evt.position, out ray);
				Session.SelectionPrioritizer selectionPrioritizer = new Session.SelectionPrioritizer(session.TheCamera.UnityCamera);
				list.ForEach(new Action<Simulated>(selectionPrioritizer.SelectBest));
				Simulated best = selectionPrioritizer.Best;
				if (Session.draggingCamera && session.properties.touchingSim != null && session.properties.touchingSim.DisplayController != null && session.properties.touchingSim.DisplayController.Transform != null)
				{
					session.properties.touchingSim.AnimateScaleAndFlip(new Vector3(1f, 1f, 1f));
				}
				else if (!Session.draggingCamera)
				{
					TerrainSlot terrainSlot2 = (TerrainSlot)session.CheckAsyncRequest("clear_purchase_on_movement");
					if (terrainSlot2 != null && session.game.terrain.ProcessTap(ray, session.TheGame))
					{
						session.ChangeState("Expansion", true);
						break;
					}
					if (list.Contains(session.properties.touchingSim))
					{
						if (session.properties.tappedDisplayController != null)
						{
							if (string.Compare(session.properties.touchingSim.GetDisplayState(), "acceptable") == 0 || string.Compare(session.properties.touchingSim.ThoughtDisplayController.GetDisplayState(), "task_collect") == 0)
							{
								session.properties.queuedClickedSim = session.properties.touchingSim;
							}
							else
							{
								ResidentEntity entity = session.properties.touchingSim.GetEntity<ResidentEntity>();
								if (entity.HungerResourceId != null)
								{
									int value = entity.HungerResourceId.Value;
									session.game.simulation.Router.Send(OfferFoodCommand.Create(Identity.Null(), session.properties.touchingSim.Id, value));
								}
							}
						}
						else
						{
							session.properties.queuedClickedSim = session.properties.touchingSim;
						}
					}
					this.CleanupTouchingSim(session);
				}
				this.CleanupTouchingBubble(session);
				Session.draggingCamera = false;
				break;
			}
			default:
				if (type != YGEvent.TYPE.HOLD)
				{
					this.CleanupTouchingSim(session);
					this.CleanupTouchingBubble(session);
				}
				else
				{
					Ray ray;
					List<Simulated> list = Session.FindSimulatedsUnderPoint(null, session.game.simulation, session.TheCamera, evt.position, out ray);
					Session.SelectionPrioritizer selectionPrioritizer = new Session.SelectionPrioritizer(session.TheCamera.UnityCamera);
					list.ForEach(new Action<Simulated>(selectionPrioritizer.SelectBest));
					Simulated best = selectionPrioritizer.Best;
					if (session.properties.touchingSim != null)
					{
						session.properties.touchingSim.BounceEnd();
						if (session.properties.touchingSim == best || list.Contains(session.properties.touchingSim))
						{
							session.AddAsyncResponse("override_drag", true);
							Session.TryGrabSimulated(session, session.properties.touchingSim, evt);
						}
					}
				}
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				Session.draggingCamera = true;
				if (!Session.draggingCamera)
				{
					Ray ray;
					List<Simulated> list = Session.FindSimulatedsUnderPoint(null, session.game.simulation, session.TheCamera, evt.position, out ray);
					Session.SelectionPrioritizer selectionPrioritizer = new Session.SelectionPrioritizer(session.TheCamera.UnityCamera);
					list.ForEach(new Action<Simulated>(selectionPrioritizer.SelectBest));
					Simulated best = selectionPrioritizer.Best;
					session.CheckAsyncRequest("clear_purchase_on_movement");
					if (session.CheckAsyncRequest("ResetSimulationDrag") != null)
					{
						session.properties.moveDragStart = evt.position;
						session.camera.ResetCurrentState();
					}
					if (session.properties.touchingSim != null && !list.Contains(session.properties.touchingSim))
					{
						this.CleanupTouchingSim(session);
						this.CleanupTouchingBubble(session);
						TFUtils.ErrorLog("cleanuptouchingsim " + session);
					}
				}
				break;
			}
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00075AB0 File Offset: 0x00073CB0
		public virtual void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			if (!session.InFriendsGame)
			{
				session.game.questManager.OnUpdate(session.game);
			}
			session.game.treasureManager.OnUpdate(session);
			object obj = session.CheckAsyncRequest(Session.Playing.INVENTORY_ENTITY);
			if (obj != null)
			{
				SBGUIScreen sbguiscreen = SBGUI.GetInstance().PopGUIScreen();
				UnityEngine.Object.Destroy(sbguiscreen.gameObject);
				Simulated selected = (Simulated)obj;
				session.game.selected = selected;
				session.game.selected.Bounce();
				session.game.selected.Warp(new Vector2(100f, 100f), session.game.simulation);
				session.game.selected.Visible = true;
				session.ChangeState("MoveBuildingInPlacement", true);
			}
			Simulated simulated = (Simulated)session.CheckAsyncRequest("RequestEntityInterface");
			if (simulated != null && simulated.InteractionState != null && simulated.InteractionState.SelectedTransition != null)
			{
				session.soundEffectManager.PlaySound(simulated.entity.SoundOnSelect);
				session.game.selected = simulated;
				simulated.InteractionState.SelectedTransition.Apply(session);
			}
			bool? flag = (bool?)session.CheckAsyncRequest("ignore_request_rush_sim");
			if (flag == null || !flag.Value)
			{
				Simulated simulated2 = (Simulated)session.CheckAsyncRequest("request_rush_sim");
				if (simulated2 != null && simulated2.rushParameters != null)
				{
					Simulated.RushParameters rushParams = simulated2.rushParameters;
					Cost.CostAtTime cost2 = rushParams.cost;
					string subject = rushParams.subject;
					int did = rushParams.did;
					Action transition = delegate()
					{
						session.ChangeState("Playing", true);
					};
					Action execute = delegate()
					{
						rushParams.execute(session);
					};
					Action cancel = delegate()
					{
						rushParams.cancel(session);
						transition();
					};
					Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
					{
						rushParams.log(session, cost, canAfford);
					};
					Vector2 screenPosition = rushParams.screenPosition;
					session.properties.hardSpendActions = new Session.HardSpendActions(execute, cost2, subject, did, transition, cancel, logSpend, screenPosition);
					session.game.selected = simulated2;
					session.ChangeState("HardSpendConfirm", false);
				}
			}
			else
			{
				session.AddAsyncResponse("ignore_request_rush_sim", true);
			}
			if (session.CheckAsyncRequest("ShowInventoryHudWidget") != null)
			{
				session.properties.playingHud.ShowInventoryWidget();
			}
			Simulated simulated3 = (Simulated)session.CheckAsyncRequest("mock_click_sessionaction");
			if (simulated3 != null)
			{
				this.SimulatedClick(simulated3, session);
			}
			if (session.properties.queuedClickedSim != null)
			{
				this.SimulatedClick(session.properties.queuedClickedSim, session);
			}
			session.properties.queuedClickedSim = null;
			int? num = (int?)session.CheckAsyncRequest("PulseResourceError");
			if (num != null)
			{
				session.properties.playingHud.TryPulseResourceError(num.Value);
			}
			GoodToSimulatedDeliveryRequest goodToSimulatedDeliveryRequest = (GoodToSimulatedDeliveryRequest)session.CheckAsyncRequest("GoodDeliveryRequest");
			if (goodToSimulatedDeliveryRequest != null)
			{
				session.properties.playingHud.DeliverGood(goodToSimulatedDeliveryRequest);
			}
			GoodReturnRequest goodReturnRequest = (GoodReturnRequest)session.CheckAsyncRequest("GoodReturnRequest");
			if (goodReturnRequest != null)
			{
				session.properties.playingHud.ReturnGood(goodReturnRequest);
			}
			bool? flag2 = (bool?)session.CheckAsyncRequest("dialogs_to_show");
			session.AddAsyncResponse("dialogs_to_show", flag2);
			if (flag2 != null && flag2.Value)
			{
				session.ChangeState("ShowingDialog", true);
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00075FEC File Offset: 0x000741EC
		public void DisappearingResourceAmount(Vector2 screenPosition, int amount)
		{
			SBGUISlidingLabel sbguislidingLabel = (SBGUISlidingLabel)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/DisappearingResourceAmount");
			sbguislidingLabel.SetParent(null);
			sbguislidingLabel.SetText(string.Format("+{0}", amount));
			sbguislidingLabel.SetScreenPosition(screenPosition);
			sbguislidingLabel.SetColor(new Color32(80, 40, 0, byte.MaxValue));
			sbguislidingLabel.AnimatedSliding(new Vector2(0f, -50f), 0f, 1f, true, null);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x0007606C File Offset: 0x0007426C
		public void SimulatedClick(Simulated clickedSim, Session session)
		{
			if (clickedSim != null)
			{
				clickedSim.BounceEnd();
				foreach (Action action in clickedSim.ClickListeners)
				{
					action();
				}
				string soundId = clickedSim.entity.SoundOnTouch;
				if (clickedSim.InteractionState.HasClickCommandFunctionality)
				{
					soundId = null;
					session.game.simulation.Router.Send(ClickedCommand.Create(Identity.Null(), clickedSim.Id), null);
					if (session.game.selected != null)
					{
						session.ChangeState("Playing", true);
						session.game.selected = null;
					}
				}
				else if (clickedSim.InteractionState.SelectedTransition != null)
				{
					soundId = clickedSim.entity.SoundOnSelect;
					session.game.selected = clickedSim;
					clickedSim.InteractionState.SelectedTransition.Apply(session);
				}
				else if (clickedSim.InteractionState.IsSelectable)
				{
					soundId = clickedSim.entity.SoundOnSelect;
					session.game.selected = clickedSim;
					session.ChangeState("SelectedPlaying", true);
				}
				else if ((int)clickedSim.Invariable["did"] == 5014)
				{
					ExplanationDialogInputData item = new ExplanationDialogInputData(Language.Get("!!JELLYFISH_FIELDS_COMING_SOON"), "Beat_JellyfishFields_ComingSoon");
					session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
					{
						item
					}, uint.MaxValue);
					session.AddAsyncResponse("dialogs_to_show", true);
				}
				session.soundEffectManager.PlaySound(soundId);
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00076240 File Offset: 0x00074440
		protected virtual void CleanupTouchingSim(Session session)
		{
			if (session.properties.touchingSim != null)
			{
				session.properties.touchingSim.BounceCleanup();
			}
			session.properties.touchingSim = null;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x0007627C File Offset: 0x0007447C
		protected virtual void CleanupTouchingBubble(Session session)
		{
			if (session.properties.tappedDisplayController != null)
			{
				session.properties.tappedDisplayController = null;
			}
		}

		// Token: 0x04000BED RID: 3053
		public static string INVENTORY_ENTITY = "InventoryEntity";
	}

	// Token: 0x02000200 RID: 512
	public class ResolveUser : Session.State
	{
		// Token: 0x0600112F RID: 4399 RVA: 0x000762A4 File Offset: 0x000744A4
		public void OnEnter(Session session)
		{
			Session.GameStarting.CreateLoadingScreen(session, false, "starting_progress", true);
			this.LOAD_GAME_RESOLVE_CONTEXT = null;
			this.platform_account = null;
			this.last_account = null;
			this.device_account = null;
			if (Language.CurrentLanguage() == LanguageCode.N)
			{
				Language.Init(TFUtils.GetPersistentAssetsPath());
			}
			if (session.Auth.AccountResolveRequired())
			{
				SoaringPlayerResolver soaringPlayerResolver = session.Auth.AccountResolver();
				this.platform_account = soaringPlayerResolver.ResolvePlatformData;
				this.last_account = soaringPlayerResolver.ResolveLastUserData;
				this.device_account = soaringPlayerResolver.ResolveDeviceData;
				Action okButtonHandler = delegate()
				{
					SBUIBuilder.ReleaseTopScreen();
					session.TheSoundEffectManager.PlaySound("Accept");
					this.UserResolutionComplete(session);
				};
				if (TFUtils.isAmazon())
				{
					this.SocialNetworkMediaName = Language.Get("!!SAVE_GAME_AMAZON_GAME_CIRCLE");
				}
				else
				{
					this.SocialNetworkMediaName = Language.Get("!!SAVE_GAME_GOOGLE_PLAY");
				}
				if (this.platform_account == null)
				{
					if (Soaring.IsOnline)
					{
						TFUtils.DebugLog("Here we show a dialog to alert the user that they can log in with GC if they want to play the previous game played on this device.");
						string message = string.Empty;
						if (Game.GameExists(session.ThePlayer))
						{
							message = Language.Get("!!SAVE_GAME_ALERT2_1") + " " + this.SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT2_2");
						}
						else
						{
							message = Language.Get("!!SAVE_GAME_ALERT2_1") + " " + this.SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT2_3");
						}
						SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), message, Language.Get("!!SAVE_GAME_PLAY"), null, null, okButtonHandler, null, false);
					}
					else
					{
						string message2 = string.Empty;
						if (Game.GameExists(session.ThePlayer))
						{
							message2 = Language.Get("!!SAVE_GAME_ALERT3_1") + " " + this.SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT3_2");
						}
						else
						{
							message2 = Language.Get("!!SAVE_GAME_ALERT3_1") + " " + this.SocialNetworkMediaName + Language.Get("!!SAVE_GAME_ALERT3_3");
						}
						SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), message2, Language.Get("!!SAVE_GAME_PLAY"), null, null, okButtonHandler, null, false);
						TFUtils.DebugLog("Here we show a dialog to alert the user that they can go online and log in with GC if they want to play the previous game played on this device.");
					}
				}
				else if (this.platform_account.loginType != this.device_account.loginType && this.last_account.loginType != this.device_account.loginType)
				{
					string playerName = TFUtils.GetPlayerName(Soaring.Player, "{0}");
					SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), string.Concat(new string[]
					{
						playerName,
						Language.Get("!!SAVE_GAME_ALERT6_1"),
						Language.Get("!!SAVE_GAME_ALERT6_2"),
						" ",
						playerName,
						Language.Get("!!SAVE_GAME_ALERT6_3"),
						" ",
						this.SocialNetworkMediaName,
						" ",
						Language.Get("!!SAVE_GAME_ALERT6_4")
					}), Language.Get("!!SAVE_GAME_PLAY"), null, null, okButtonHandler, null, false);
					TFUtils.DebugLog("Here we show a dialog to alert the user that they will be playing with a different game.");
				}
				else
				{
					TFUtils.DebugLog("-----------------!lastPlayerIsGC------------------");
					SoaringContext context = Game.CreateSoaringGameResponderContext(new SoaringContextDelegate(this.OnLoadRemoteGame));
					Game.LoadFromNetwork(null, session.player.ReadTimestamp(), context, session);
				}
			}
			else
			{
				TFUtils.DebugLog("Last player is the same as current. Continue unobtrusively");
				this.UserResolutionComplete(session);
			}
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00076630 File Offset: 0x00074830
		public void OnLoadRemoteGame(SoaringContext context)
		{
			this.LOAD_GAME_RESOLVE_CONTEXT = context;
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x0007663C File Offset: 0x0007483C
		public void OnUpdate(Session session)
		{
			if (this.LOAD_GAME_RESOLVE_CONTEXT != null)
			{
				this.RemoteGameReturned(session, this.LOAD_GAME_RESOLVE_CONTEXT);
				this.LOAD_GAME_RESOLVE_CONTEXT = null;
			}
			if (this.MIGRATION_CONTEXT != null)
			{
				this.ProcessMigrationResults(session, this.MIGRATION_CONTEXT);
				this.MIGRATION_CONTEXT = null;
			}
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00076688 File Offset: 0x00074888
		private void RemoteGameReturned(Session session, SoaringContext context)
		{
			SoaringError soaringError = (SoaringError)context.objectWithKey("error_message");
			SoaringArray soaringArray = (SoaringArray)context.objectWithKey("custom");
			SoaringDictionary soaringDictionary = null;
			bool flag = !Soaring.IsOnline;
			if (soaringError != null)
			{
				flag = true;
			}
			else if (soaringArray != null && soaringArray.count() != 0)
			{
				soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(0);
			}
			if (flag)
			{
				if (Game.GameExists(session.ThePlayer))
				{
					TFUtils.DebugLog("Here we show a dialog to alert the user that they will be playing with a different game than the previous game.");
					Action okButtonHandler = delegate()
					{
						SBUIBuilder.ReleaseTopScreen();
						session.TheSoundEffectManager.PlaySound("Accept");
						this.UserResolutionComplete(session);
					};
					SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), Language.Get("!!SAVE_GAME_ALERT5"), Language.Get("!!SAVE_GAME_PLAY"), null, null, okButtonHandler, null, false);
				}
				else
				{
					Action okButtonHandler2 = delegate()
					{
						SBUIBuilder.ReleaseTopScreen();
						session.TheSoundEffectManager.PlaySound("Accept");
					};
					SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), Language.Get("!!SAVE_GAME_ALERT_FIRST_OFFLINE"), Language.Get("!!SAVE_GAME_OK"), null, null, okButtonHandler2, null, false);
				}
			}
			else if (Game.GameExists(session.ThePlayer) || soaringDictionary != null)
			{
				TFUtils.DebugLog("Different player, can migrate device to local or use remote or use local gc game");
				this.LoadAndPresentAllPossibleGamestates(session, soaringDictionary);
			}
			else
			{
				Action okButtonHandler3 = delegate()
				{
					SBUIBuilder.ReleaseTopScreen();
					session.TheSoundEffectManager.PlaySound("Accept");
					this.MigratePlayer(session, this.last_account, null);
				};
				SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), string.Concat(new string[]
				{
					TFUtils.GetPlayerName(Soaring.Player, "{0}"),
					Language.Get("!!SAVE_GAME_ALERT45_1"),
					" ",
					this.SocialNetworkMediaName,
					Language.Get("!!SAVE_GAME_ALERT45_2"),
					" ",
					this.SocialNetworkMediaName,
					Language.Get("!!SAVE_GAME_ALERT45_3")
				}), Language.Get("!!SAVE_GAME_PLAY"), null, null, okButtonHandler3, null, false);
				TFUtils.DebugLog("Different player, no existing game. Auto migrate.");
			}
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00076898 File Offset: 0x00074A98
		private void LoadAndPresentAllPossibleGamestates(Session session, SoaringDictionary sessionSaveData)
		{
			if (sessionSaveData != null)
			{
				TFUtils.DebugLog("There is a later version of the user's save game on the server");
				if (SBSettings.UseLegacyGameLoad)
				{
					TFUtils.DebugLog("Loading Game In Legacy Mode");
					this.serverSave = (Dictionary<string, object>)Json.Deserialize(sessionSaveData.ToJsonString());
				}
				else
				{
					this.serverSave = SBMISoaring.ConvertDictionaryToGeneric(sessionSaveData);
				}
			}
			else
			{
				TFUtils.DebugLog("There is a NOT a later version of the user's save game on the server");
			}
			if (Game.GameExists(session.ThePlayer))
			{
				string json = TFUtils.ReadAllText(session.ThePlayer.CacheFile("game.json"));
				this.localSave = (Dictionary<string, object>)Json.Deserialize(json);
			}
			this.deviceGameSaves = this.gatherLocalDeviceSaves();
			this.PresentGameOptions(session);
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x0007694C File Offset: 0x00074B4C
		private void PresentGameOptions(Session session)
		{
			this.alert(session);
			TFUtils.DebugLog("These are the options for games:");
			if (this.serverSave != null)
			{
				TFUtils.DebugLog("----------serverSave != null----------");
				Dictionary<string, object> dictionary = (Dictionary<string, object>)this.serverSave["playtime"];
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"Found server game that is different than the local game ",
					session.ThePlayer.playerId,
					" with server game level ",
					dictionary["level"]
				}));
			}
			else
			{
				TFUtils.DebugLog("The server save is either not found or the same as the local player game.");
			}
			if (this.localSave != null)
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)this.localSave["playtime"];
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"Found local game ",
					session.ThePlayer.playerId,
					" with local game level ",
					dictionary2["level"]
				}));
			}
			else
			{
				TFUtils.DebugLog("There is no local game for this player. It is either loading one from the server or creating a new game");
			}
			foreach (KeyValuePair<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> keyValuePair in this.deviceGameSaves)
			{
				Dictionary<string, object> dictionary3 = (Dictionary<string, object>)keyValuePair.Value["playtime"];
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"Found device player ",
					keyValuePair.Key.soaringTag,
					" with local game level ",
					dictionary3["level"]
				}));
			}
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00076AF4 File Offset: 0x00074CF4
		private void alert(Session session)
		{
			Dictionary<string, object> gcSave = null;
			if (this.serverSave != null)
			{
				gcSave = this.serverSave;
			}
			else
			{
				gcSave = this.localSave;
			}
			TFUtils.GameDetails gameDetails = null;
			TFUtils.GameDetails lastDetails = null;
			TFUtils.DebugLog("GCGame: " + TFUtils.ParseGameDetails(gcSave, ref gameDetails));
			string json = TFUtils.ReadAllText(string.Concat(new object[]
			{
				Application.persistentDataPath,
				Path.DirectorySeparatorChar,
				this.last_account.soaringTag,
				Path.DirectorySeparatorChar,
				"game.json"
			}));
			Dictionary<string, object> gameData = (Dictionary<string, object>)Json.Deserialize(json);
			TFUtils.DebugLog("LastGame: " + TFUtils.ParseGameDetails(gameData, ref lastDetails));
			if (gameDetails.lastPlayTime == lastDetails.lastPlayTime)
			{
				TFUtils.DebugLog("Games are Identical, Use Server");
				this.SelectSaveGame(session, gcSave);
				return;
			}
			this.saveGameScreen = (SaveGameScreen)SBGUI.InstantiatePrefab("Prefabs/SaveGame");
			Action local = delegate()
			{
				this.saveGameScreen.SetActive(false);
				Action okButtonHandler = delegate()
				{
					SBUIBuilder.ReleaseTopScreen();
					this.saveGameScreen.Deactivate();
					session.TheSoundEffectManager.PlaySound("Accept");
					this.MigratePlayer(session, this.last_account, null);
				};
				Action cancelButtonHandler = delegate()
				{
					SBUIBuilder.ReleaseTopScreen();
					this.saveGameScreen.SetActive(true);
				};
				string format = "||Level:{0} Jelly:{1} Money:{2}";
				string text = string.Concat(new string[]
				{
					Language.Get("!!SAVE_GAME_ALERT4_LINK_1"),
					TFUtils.GetPlayerName(Soaring.Player, " {0} "),
					Language.Get("!!SAVE_GAME_ALERT4_LINK_2"),
					" ",
					this.SocialNetworkMediaName,
					Language.Get("!!SAVE_GAME_ALERT4_LINK_3")
				});
				text += string.Format(format, lastDetails.level, lastDetails.jelly, lastDetails.money);
				SBUIBuilder.MakeAndPushConfirmationDialog(session, null, Language.Get("!!SAVE_GAME_TITLE"), text, Language.Get("!!SAVE_GAME_YES"), Language.Get("!!SAVE_GAME_NO"), null, okButtonHandler, cancelButtonHandler, false);
			};
			Action server = delegate()
			{
				this.saveGameScreen.Deactivate();
				session.TheSoundEffectManager.PlaySound("Accept");
				this.SelectSaveGame(session, gcSave);
			};
			this.saveGameScreen.SetUp(TFUtils.GetPlayerName(Soaring.Player, "{0} ") + Language.Get("!!SAVE_GAME_TEXT1"), Language.Get("!!SAVE_GAME_TEXT2"), Language.Get("!!SAVE_GAME_ALERT4"), Language.Get("!!SAVE_GAME_SAVE_ON_SERVER"), gameDetails.level, gameDetails.money, gameDetails.jelly, gameDetails.patties, gameDetails.dtLastPlayTime, Language.Get("!!SAVE_GAME_KEEP_AND_PLAY"), Language.Get("!!SAVE_GAME_SAVE_ON_DEVICE"), lastDetails.level, lastDetails.money, lastDetails.jelly, lastDetails.jelly, lastDetails.dtLastPlayTime, Language.Get("!!SAVE_GAME_LINK_AND_PLAY"), server, local, session);
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00076D14 File Offset: 0x00074F14
		private void SelectSaveGame(Session session, Dictionary<string, object> selectedGame)
		{
			TFUtils.DebugLog("Saving selected game");
			if (this.deviceGameSaves.ContainsValue(selectedGame))
			{
				TFUtils.DebugLog("Attempting to migrate local games");
				foreach (KeyValuePair<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> keyValuePair in this.deviceGameSaves)
				{
					if (keyValuePair.Value == selectedGame)
					{
						TFUtils.DebugLog("Migrating player games: " + keyValuePair.Key);
						this.MigratePlayer(session, keyValuePair.Key, null);
						break;
					}
				}
			}
			else if (selectedGame == this.serverSave)
			{
				string path = session.ThePlayer.CacheFile(PersistedActionBuffer.ACTION_LIST_FILE);
				if (File.Exists(path))
				{
					File.WriteAllText(path, string.Empty);
				}
				string contents = Json.Serialize(selectedGame);
				File.WriteAllText(session.ThePlayer.CacheFile("game.json"), contents);
				TFUtils.DebugLog("Overwriting local game with the server game.");
				this.UserResolutionComplete(session);
			}
			else if (selectedGame == this.localSave && this.serverSave != null)
			{
				TFUtils.DebugLog("Overwriting server game with the local game.");
				string gameData = Json.Serialize(selectedGame);
				session.ThePlayer.DeleteTimestamp();
				SoaringContext soaringContext = Game.CreateSoaringGameResponderContext(new SoaringContextDelegate(this.OnSaveComplete));
				soaringContext.addValue(new SoaringObject(session), "session");
				session.WebFileServer.SaveGameData(gameData, soaringContext);
			}
			else if (selectedGame == this.localSave)
			{
				TFUtils.DebugLog("No action - user selected local save (and it is the same as the server) so just will continue.");
				this.UserResolutionComplete(session);
			}
			else
			{
				TFUtils.ErrorLog("Attempting to save an unknown gamestate.");
			}
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00076ED4 File Offset: 0x000750D4
		private Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> gatherLocalDeviceSaves()
		{
			Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> dictionary = new Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>>();
			SoaringArray usersArray = SoaringPlayerResolver.UsersArray;
			if (usersArray == null)
			{
				return dictionary;
			}
			int num = usersArray.count();
			for (int i = 0; i < num; i++)
			{
				SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)usersArray.objectAtIndex(i);
				if (soaringPlayerData.loginType == SoaringLoginType.Soaring || (soaringPlayerData.loginType == SoaringLoginType.Device && !string.IsNullOrEmpty(soaringPlayerData.userID)))
				{
					Player player = new Player(soaringPlayerData.soaringTag);
					string text = player.CacheFile("game.json");
					if (File.Exists(text))
					{
						string json = TFUtils.ReadAllText(text);
						Dictionary<string, object> value = (Dictionary<string, object>)Json.Deserialize(json);
						dictionary[soaringPlayerData] = value;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00076F90 File Offset: 0x00075190
		public void OnLeave(Session session)
		{
			this.LOAD_GAME_RESOLVE_CONTEXT = null;
			this.localSave = null;
			this.deviceGameSaves = null;
			this.serverSave = null;
			this.platform_account = null;
			this.last_account = null;
			this.device_account = null;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00076FC4 File Offset: 0x000751C4
		private void MigratePlayer(Session session, SoaringPlayerResolver.SoaringPlayerData sourceAccount, SoaringPlayerResolver.SoaringPlayerData targetAccount)
		{
			SoaringContext soaringContext = Game.CreateSoaringGameResponderContext(new SoaringContextDelegate(this.OnMigrationComplete));
			SoaringLoginType soaringLoginType = SoaringLoginType.Device;
			SoaringLoginType soaringLoginType2 = SoaringLoginType.Device;
			string srcPlayerID = null;
			string targetPlayerID = null;
			if (sourceAccount != null)
			{
				srcPlayerID = sourceAccount.platformID;
				soaringLoginType = sourceAccount.loginType;
				if (soaringLoginType == SoaringLoginType.Soaring)
				{
					soaringLoginType = SoaringLoginType.Device;
				}
				soaringContext.addValue(sourceAccount, "user_data");
			}
			else if (targetAccount != null)
			{
				targetPlayerID = targetAccount.platformID;
				soaringLoginType2 = targetAccount.loginType;
				if (soaringLoginType2 == SoaringLoginType.Soaring)
				{
					soaringLoginType2 = SoaringLoginType.Device;
				}
				soaringContext.addValue(targetAccount, "user_data");
			}
			SBMISoaring.MigratePlayerToNewPlayer(srcPlayerID, soaringLoginType, targetPlayerID, soaringLoginType2, soaringContext);
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00077050 File Offset: 0x00075250
		public void OnMigrationComplete(SoaringContext context)
		{
			this.MIGRATION_CONTEXT = context;
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x0007705C File Offset: 0x0007525C
		public void ProcessMigrationResults(Session session, SoaringContext context)
		{
			session.auth.AccountResolved();
			SoaringError soaringError = (SoaringError)context.objectWithKey("error_message");
			if (soaringError != null || !Soaring.IsOnline)
			{
				TFUtils.DebugLog("Show dialog saying that the migration failed and you must be online (if the user is offline)");
				TFUtils.ErrorLog("ProcessMigrationResults: " + soaringError);
				Action okButtonHandler = delegate()
				{
					SBUIBuilder.ReleaseTopScreen();
					this.UserResolutionComplete(session);
				};
				SBUIBuilder.MakeAndPushConfirmationDialog(session, null, "Migration Error", "An error occured in the migrations", Language.Get("!!PREFAB_OK"), null, null, okButtonHandler, null, false);
				TFUtils.LogDump(session, "migration_error", null, null);
			}
			else
			{
				SoaringPlayerResolver.SoaringPlayerData soaringPlayerData = (SoaringPlayerResolver.SoaringPlayerData)context.objectWithKey("user_data");
				bool flag = soaringPlayerData.soaringTag != Soaring.Player.UserTag;
				if (flag)
				{
					SoaringPlayerResolver.RemovePlayer(soaringPlayerData);
					try
					{
						string text = string.Concat(new object[]
						{
							Application.persistentDataPath,
							Path.DirectorySeparatorChar,
							Soaring.Player.UserTag,
							Path.DirectorySeparatorChar
						});
						if (Directory.Exists(text))
						{
							Directory.Delete(text, true);
						}
						string text2 = string.Concat(new object[]
						{
							Application.persistentDataPath,
							Path.DirectorySeparatorChar,
							soaringPlayerData.soaringTag,
							Path.DirectorySeparatorChar
						});
						if (Directory.Exists(text2))
						{
							Directory.Move(text2, text);
						}
						if (Directory.Exists(text2))
						{
							Directory.Delete(text2, true);
						}
					}
					catch (Exception ex)
					{
						SoaringDebug.Log("MigrationError: " + ex.Message);
					}
				}
				TFUtils.DebugLog(string.Concat(new string[]
				{
					"Migrating Player ",
					Soaring.Player.UserTag,
					" : ",
					session.ThePlayer.playerId,
					"\nwith ",
					soaringPlayerData.soaringTag,
					" : ",
					soaringPlayerData.userID
				}));
				SoaringPlayerResolver.Save(Soaring.Player.UserTag);
				session.ChangeState("Authorizing", true);
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000772C4 File Offset: 0x000754C4
		private void OnSaveComplete(SoaringContext context)
		{
			bool flag = context.soaringValue("status");
			SoaringError soaringError = (SoaringError)context.objectWithKey("error_message");
			SoaringDictionary soaringDictionary = (SoaringDictionary)context.objectWithKey("custom");
			Session session = (Session)((SoaringObject)context.objectWithKey("session")).Object;
			if (flag)
			{
				if (soaringDictionary != null)
				{
					long num = soaringDictionary.soaringValue("datetime");
					if (Player.ValidTimeStamp(num))
					{
						session.player.SetStagedTimestamp(num);
					}
				}
			}
			else if (soaringError != null)
			{
				SoaringDebug.Log(soaringError, LogType.Error);
			}
			this.UserResolutionComplete(session);
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00077374 File Offset: 0x00075574
		private void UserResolutionComplete(Session session)
		{
			session.ChangeState("CheckPatching", true);
		}

		// Token: 0x04000BEE RID: 3054
		private SoaringContext LOAD_GAME_RESOLVE_CONTEXT;

		// Token: 0x04000BEF RID: 3055
		private SoaringContext MIGRATION_CONTEXT;

		// Token: 0x04000BF0 RID: 3056
		private Dictionary<string, object> serverSave;

		// Token: 0x04000BF1 RID: 3057
		private Dictionary<string, object> localSave;

		// Token: 0x04000BF2 RID: 3058
		private SoaringPlayerResolver.SoaringPlayerData platform_account;

		// Token: 0x04000BF3 RID: 3059
		private SoaringPlayerResolver.SoaringPlayerData last_account;

		// Token: 0x04000BF4 RID: 3060
		private SoaringPlayerResolver.SoaringPlayerData device_account;

		// Token: 0x04000BF5 RID: 3061
		private SaveGameScrollScreen saveGameScrollScreen;

		// Token: 0x04000BF6 RID: 3062
		private SaveGameScreen saveGameScreen;

		// Token: 0x04000BF7 RID: 3063
		private SaveGameScreen1 saveGameScreen1;

		// Token: 0x04000BF8 RID: 3064
		private Dictionary<SoaringPlayerResolver.SoaringPlayerData, Dictionary<string, object>> deviceGameSaves;

		// Token: 0x04000BF9 RID: 3065
		private string SocialNetworkMediaName;
	}

	// Token: 0x02000201 RID: 513
	public class SelectedPlaying : Session.Playing
	{
		// Token: 0x06001140 RID: 4416 RVA: 0x000773BC File Offset: 0x000755BC
		public override void OnEnter(Session session)
		{
			base.OnEnter(session);
			SBUIBuilder.MakeAndPushEmptyUI(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent));
			Simulated selected = session.game.selected;
			TFUtils.DebugLog("Selected simulated with did " + selected.entity.DefinitionId);
			if (selected.InteractionState.Controls != null && selected.InteractionState.Controls.Count > 0 && !selected.InteractionState.IsEditable)
			{
				this.interactionStrip.ActivateOnSelected(session);
				this.interactionStrip.OnUpdate(session);
			}
			this.timebarGroup.ActivateOnSelected(session);
			this.m_pNamebarGroup.ActivateOnSelected(session);
			session.properties.m_pTaskSimulated = null;
			session.properties.m_bAutoPanToSimulatedOnLeave = false;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00077490 File Offset: 0x00075690
		public override void OnLeave(Session session)
		{
			this.DeactivateTimeBarAndInteractionStrip(session);
			SBUIBuilder.ReleaseTopScreen();
			if (session.properties.m_pTaskSimulated != null)
			{
				session.game.selected = session.properties.m_pTaskSimulated;
			}
			if (session.game.selected != null && session.properties.m_bAutoPanToSimulatedOnLeave)
			{
				session.TheCamera.AutoPanToPosition(session.game.selected.PositionCenter, 0.75f);
			}
			base.OnLeave(session);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00077518 File Offset: 0x00075718
		public override void OnUpdate(Session session)
		{
			base.OnUpdate(session);
			if (session.game.selected != null && session.game.selected.InteractionState.Controls != null && session.game.selected.InteractionState.Controls.Count > 0)
			{
				this.interactionStrip.OnUpdate(session);
			}
			int? num = (int?)session.CheckAsyncRequest(Session.SelectedPlaying.TASK_CHARACTER_SELECT);
			if (num != null)
			{
				Simulated simulated = session.TheGame.simulation.FindSimulated(new int?(num.Value));
				if (simulated != null)
				{
					session.properties.m_pTaskSimulated = simulated;
					TaskManager taskManager = session.TheGame.taskManager;
					List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
					if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() > 0UL)
					{
						if (taskManager.GetTaskingStateForSimulated(session.TheGame.simulation, num.Value, simulated.Id) == TaskManager._eBlueprintTaskingState.eNone)
						{
							session.ChangeState("UnitIdle", true);
						}
						else
						{
							session.ChangeState("UnitBusy", true);
						}
					}
					else
					{
						session.properties.m_bAutoPanToSimulatedOnLeave = true;
						session.ChangeState("Playing", true);
					}
				}
			}
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00077680 File Offset: 0x00075880
		public override void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
			bool flag = true;
			YGEvent.TYPE type = evt.type;
			if (type == YGEvent.TYPE.TOUCH_END)
			{
				if (session.game.simulation.Whitelisted)
				{
					return;
				}
				if (session.properties.touchingSim == null || session.properties.touchingSim == session.game.selected)
				{
					flag = false;
					session.ChangeState("Playing", true);
					this.CleanupTouchingSim(session);
					session.game.selected = null;
				}
			}
			if (flag)
			{
				base.HandleSBGUIEvent(evt, session);
			}
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00077718 File Offset: 0x00075918
		private void DeactivateTimeBarAndInteractionStrip(Session session)
		{
			this.timebarGroup.Deactivate(session);
			this.m_pNamebarGroup.Deactivate(session);
			this.interactionStrip.Deactivate(session);
		}

		// Token: 0x04000BFA RID: 3066
		public static string TASK_CHARACTER_SELECT = "task_character_select";

		// Token: 0x04000BFB RID: 3067
		protected Session.TimebarGroup timebarGroup = new Session.TimebarGroup();

		// Token: 0x04000BFC RID: 3068
		protected Session.NamebarGroup m_pNamebarGroup = new Session.NamebarGroup();

		// Token: 0x04000BFD RID: 3069
		protected Session.InteractionStripMixin interactionStrip = new Session.InteractionStripMixin();
	}

	// Token: 0x02000202 RID: 514
	public class SellBuildingConfirmation : Session.State
	{
		// Token: 0x06001146 RID: 4422 RVA: 0x00077754 File Offset: 0x00075954
		public void OnEnter(Session session)
		{
			this.Setup(session);
			session.camera.SetEnableUserInput(false, false, default(Vector3));
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00077780 File Offset: 0x00075980
		public void Setup(Session session)
		{
			string text = null;
			object obj = session.CheckAsyncRequest("sell_error");
			if (obj != null)
			{
				text = (string)obj;
			}
			Simulated toSell = (Simulated)session.CheckAsyncRequest("to_sell");
			object obj2 = session.CheckAsyncRequest("in_state_move_in_edit");
			bool bMoveInEdit = false;
			if (obj2 != null)
			{
				bMoveInEdit = (bool)obj2;
			}
			if (session.TheState.GetType().Equals(typeof(Session.MoveBuildingInEdit)))
			{
				Session.MoveBuildingInEdit moveBuildingInEdit = (Session.MoveBuildingInEdit)session.TheState;
				moveBuildingInEdit.DeactivateInteractionStrip(session);
			}
			BuildingEntity entity = toSell.GetEntity<BuildingEntity>();
			if (string.IsNullOrEmpty(text))
			{
				Action okButtonHandler = delegate()
				{
					this.SellSimulated(session, toSell);
				};
				Action cancelButtonHandler = delegate()
				{
					session.TheSoundEffectManager.PlaySound("CloseMenu");
					session.ChangeState((!bMoveInEdit) ? "MoveBuildingInPlacement" : "MoveBuildingInEdit", true);
				};
				Cost sellCost = session.game.catalog.GetSellCost(entity.DefinitionId);
				session.TheSoundEffectManager.PlaySound("OpenMenu");
				SBUIBuilder.MakeAndPushConfirmationDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), Language.Get("!!SELL_CONFIRMATION_TITLE"), Language.Get("!!SELL_CONFIRMATION_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_NEVERMIND"), Cost.DisplayDictionary(sellCost.ResourceAmounts, session.TheGame.resourceManager), okButtonHandler, cancelButtonHandler, false);
			}
			else
			{
				Action okButtonHandler2 = delegate()
				{
					session.TheSoundEffectManager.PlaySound("CloseMenu");
					session.ChangeState((!bMoveInEdit) ? "MoveBuildingInPlacement" : "MoveBuildingInEdit", true);
				};
				SBUIBuilder.MakeAndPushAcknowledgeDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), Language.Get("!!CANNOT_SELL_DIALOG_TITLE"), Language.Get(text), (string)entity.Invariable["portrait"], Language.Get("!!PREFAB_OK"), okButtonHandler2);
			}
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x00077984 File Offset: 0x00075B84
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00077988 File Offset: 0x00075B88
		public void OnLeave(Session session)
		{
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0007798C File Offset: 0x00075B8C
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x000779F0 File Offset: 0x00075BF0
		private void SellSimulated(Session session, Simulated toSell)
		{
			bool? flag = (bool?)session.CheckAsyncRequest("FromInventory");
			if (toSell.entity is DebrisEntity)
			{
				TFUtils.Assert(Session.debugManager.debugPlaceObjects, "You shouldn't be here...");
				DebrisEntity debris = (DebrisEntity)toSell.entity;
				if (debris.ExpansionId != null)
				{
					Dictionary<int, TerrainSlot> expansionSlots = session.TheGame.terrain.ExpansionSlots;
					TerrainSlot terrainSlot = expansionSlots[debris.ExpansionId.Value];
					int num = terrainSlot.debris.FindIndex((TerrainSlotObject obj) => obj.id == debris.Id);
					if (num >= 0)
					{
						terrainSlot.debris.RemoveAt(num);
					}
				}
				toSell.SetFootprint(session.game.simulation, false);
				session.game.simulation.RemoveSimulated(toSell);
				if (flag != null)
				{
					session.AddAsyncResponse("FromInventory", flag.Value);
				}
				if ((flag != null && flag.Value) || session.properties.cameFromMarketplace)
				{
					session.ChangeState("Playing", true);
				}
				else
				{
					session.ChangeState("Editing", true);
				}
				session.game.selected = null;
				return;
			}
			session.TheSoundEffectManager.PlaySound("Sell");
			session.game.selected.FootprintVisible = false;
			BuildingEntity entity = toSell.GetEntity<BuildingEntity>();
			SwarmManager.Instance.RestoreResidents(session.game.simulation, toSell);
			Cost sellCost = session.game.catalog.GetSellCost(entity.DefinitionId);
			session.game.simulation.ModifyGameStateSimulated(toSell, new SellAction(toSell, sellCost));
			session.game.resourceManager.SellFor(sellCost, session.game);
			Identity id = toSell.Id;
			session.game.simulation.TryWorkerSpawnerCleanup(id);
			List<Simulated> list = Simulated.Building.FindResidents(session.game.simulation, toSell);
			foreach (Simulated simulated in list)
			{
				SwarmManager.Instance.RemoveResident(simulated.GetEntity<ResidentEntity>(), toSell);
				session.game.simulation.RemoveSimulated(simulated);
			}
			session.game.simulation.RemoveSimulated(toSell);
			session.game.entities.Destroy(id);
			session.analytics.LogSell(toSell.entity.BlueprintName, toSell.entity.DefinitionId >= 2000 && toSell.entity.DefinitionId < 3000, sellCost, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount);
			session.game.selected = null;
			if (flag != null)
			{
				session.AddAsyncResponse("FromInventory", flag.Value);
			}
			if ((flag != null && flag.Value) || session.properties.cameFromMarketplace)
			{
				session.ChangeState("Playing", true);
			}
			else
			{
				session.ChangeState("Editing", true);
			}
		}
	}

	// Token: 0x02000203 RID: 515
	public class Shopping : Session.State
	{
		// Token: 0x0600114D RID: 4429 RVA: 0x00077D88 File Offset: 0x00075F88
		public void OnEnter(Session session)
		{
			bool isJellyPurchase = false;
			session.game.dropManager.MarkForClearCurrentDrops();
			session.marketpalceActive = true;
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			session.musicManager.PlayTrack("InGame");
			session.PlayBubbleScreenSwipeEffect();
			session.camera.SetEnableUserInput(false, false, default(Vector3));
			session.TheCamera.TurnOnScreenBuffer();
			Action action = delegate()
			{
				session.ChangeState("Playing", true);
				session.properties.m_sLeaveType = "store_close_back_button";
				AndroidBack.getInstance().pop();
			};
			Action inventoryClickHandler = delegate()
			{
				if (!session.game.featureManager.CheckFeature("inventory_soft"))
				{
					session.game.featureManager.UnlockFeature("inventory_soft");
					session.game.featureManager.ActivateFeatureActions(session.game, "inventory_soft");
					session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string>
					{
						"inventory_soft"
					}));
					return;
				}
				session.ChangeState("Inventory", true);
			};
			Action optionsHandler = delegate()
			{
				session.ChangeState("Options", true);
			};
			Action editClickHandler = delegate()
			{
				session.ChangeState("Editing", true);
			};
			Action<SBMarketOffer> purchaseClickHandler = delegate(SBMarketOffer offer)
			{
				string type2 = offer.type;
				switch (type2)
				{
				case "rmt":
					if (Application.internetReachability == NetworkReachability.NotReachable || !Soaring.IsOnline)
					{
						session.TheSoundEffectManager.PlaySound("Error");
						string message = TFUtils.AssignStorePlatformText("!!ERROR_NETWORK_NEEDED_FOR_RMT");
						ExplanationDialogInputData item2 = new ExplanationDialogInputData(message, "Beat_JellyfishFields_ComingSoon");
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, new List<DialogInputData>
						{
							item2
						}, uint.MaxValue);
						session.ChangeState("ShowingDialog", true);
						session.properties.m_sLeaveType = "store_close_known_error_iap";
						return;
					}
					if (RmtStore.IsPurchasing)
					{
						Action okAction = delegate()
						{
							session.AddAsyncResponse("store_open_type", "store_open_iap_error_return");
							session.ChangeState("Shopping", true);
						};
						session.ErrorMessageHandler(session, Language.Get("!!PRODUCT_IAP_IS_BUSY_TITLE"), Language.Get("!!PRODUCT_IAP_IS_BUSY_TEXT"), Language.Get("!!PREFAB_OK"), okAction, 0.75f);
						return;
					}
					if (session.game.store.RmtReady)
					{
						if (!session.game.store.rmtProducts.ContainsKey(offer.innerOffer))
						{
							TFUtils.DebugLog("Failed to find product for " + offer.innerOffer);
						}
						string innerOffer = offer.innerOffer;
						session.properties.iapBundleName = innerOffer;
						session.game.store.OpenTransaction(innerOffer);
						session.AddAsyncResponse("transaction_offer", offer);
						session.ChangeState("InAppPurchasing", true);
						session.properties.m_sLeaveType = "store_close_purchase_iap";
					}
					else
					{
						session.ChangeState("Playing", true);
						session.properties.m_sLeaveType = "store_close_unknown_error_iap";
					}
					return;
				case "path":
					session.ChangeState("Paving", true);
					session.properties.m_sLeaveType = "store_close_road_purchase_start";
					return;
				case "expansion":
					session.ChangeState("Expanding", true);
					session.properties.m_sLeaveType = "expanding";
					return;
				case "resource":
				{
					SBUIBuilder.ReleaseTopScreen();
					int rmtCost = offer.cost[ResourceManager.HARD_CURRENCY];
					if (session.game.resourceManager.HasEnough(ResourceManager.HARD_CURRENCY, rmtCost))
					{
						session.TheSoundEffectManager.PlaySound("OpenMenu");
						Action cancelButtonHandler = delegate()
						{
							session.TheSoundEffectManager.PlaySound("Error");
							SBUIBuilder.ReleaseTopScreen();
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping", true);
						};
						Action okButtonHandler = delegate()
						{
							session.TheSoundEffectManager.PlaySound("Purchase");
							SBUIBuilder.ReleaseTopScreen();
							TFUtils.Assert(offer.cost.ContainsKey(ResourceManager.HARD_CURRENCY), "Anything purchased in the Marketplace needs RMT for now");
							TFUtils.Assert(offer.data != null && offer.data.Count > 0, "Missing resource data for this offer: " + offer.itemName);
							session.TheGame.resourceManager.PurchaseResourcesWithHardCurrency(rmtCost, new Cost(offer.data), session.game);
							session.game.simulation.ModifyGameState(new PurchaseResourcesAction(new Identity(), rmtCost, new Cost(offer.data)));
							session.ChangeState("Playing", true);
							session.properties.m_sLeaveType = "store_close_purchase_iap";
						};
						SBUIBuilder.MakeAndPushConfirmationDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), Language.Get("!!PREFAB_CONFIRM_PURCHASE_TITLE"), Language.Get("!!PREFAB_CONFIRM_PURCHASE_MESSAGE"), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_NEVERMIND"), Cost.DisplayDictionary(offer.cost, session.TheGame.resourceManager), okButtonHandler, cancelButtonHandler, false);
					}
					else
					{
						Action action4 = delegate()
						{
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping", true);
						};
						string title = (string)session.CheckAsyncRequest("jelly_message_title");
						string message2 = (string)session.CheckAsyncRequest("jelly_message");
						string question = (string)session.CheckAsyncRequest("jelly_question");
						string acceptLabel = (string)session.CheckAsyncRequest("jelly_message_ok_label");
						string cancelLabel = (string)session.CheckAsyncRequest("jelly_message_cancel_label");
						Action okButtonHandler2 = (Action)session.CheckAsyncRequest("jelly_message_ok_action");
						Action cancelButtonHandler2 = (Action)session.CheckAsyncRequest("jelly_message_cancel_action");
						SBGUIGetJellyDialog sbguigetJellyDialog = SBUIBuilder.MakeAndPushGetJellyDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), title, message2, question, acceptLabel, cancelLabel, null, okButtonHandler2, cancelButtonHandler2, true);
					}
					return;
				}
				case "building":
				case "annex":
				{
					EntityType type = EntityType.BUILDING;
					if (offer.type == "annex")
					{
						type = EntityType.ANNEX;
					}
					Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(offer.cost, session.game.resourceManager);
					if (resourcesStillRequired.Count > 0)
					{
						session.TheSoundEffectManager.PlaySound("Error");
						Session temp = session;
						Action okAction2 = delegate()
						{
							if (!session.game.simulation.Whitelisted || Session.Shopping.hackLastOffer != offer)
							{
								Session.Shopping.hackLastOffer = offer;
								temp.TheSoundEffectManager.PlaySound("Purchase");
								temp.game.selected = temp.game.simulation.CreateSimulated(type, offer.identity, Vector2.zero);
								if (type == EntityType.BUILDING)
								{
									temp.game.selected.GetEntity<BuildingEntity>().Slots = temp.game.craftManager.GetInitialSlots(offer.identity);
								}
								temp.game.selected.Visible = true;
								temp.ChangeState("Placing", true);
								session.properties.m_sLeaveType = "store_close_item_purchase_start";
							}
						};
						Action cancelAction = delegate()
						{
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping", true);
						};
						string itemName;
						if (offer.itemName != null)
						{
							itemName = "purchase " + offer.itemName;
						}
						else
						{
							itemName = string.Format("purchase {0} {1}", offer.type, offer.identity);
						}
						session.properties.m_sLeaveType = "store_close_im_broke";
						session.InsufficientResourcesHandler(session, itemName, offer.identity, okAction2, cancelAction, new Cost(offer.cost));
					}
					else if (!session.game.simulation.Whitelisted || Session.Shopping.hackLastOffer != offer)
					{
						Session.Shopping.hackLastOffer = offer;
						session.TheSoundEffectManager.PlaySound("Purchase");
						EntityType types = EntityTypeNamingHelper.StringToType(offer.type);
						session.game.selected = session.game.simulation.CreateSimulated(types, offer.identity, Vector2.zero);
						session.game.selected.Visible = true;
						session.ChangeState("Placing", true);
						session.properties.m_sLeaveType = "store_close_item_purchase_start";
					}
					return;
				}
				case "costume":
				{
					int currency = 0;
					int costumeCost = 0;
					Simulated pSimulated = session.TheGame.simulation.FindSimulated(new int?(session.TheGame.costumeManager.GetCostume(offer.identity).m_nUnitDID));
					if (offer.cost.ContainsKey(ResourceManager.SOFT_CURRENCY))
					{
						costumeCost = offer.cost[ResourceManager.SOFT_CURRENCY];
						currency = ResourceManager.SOFT_CURRENCY;
						isJellyPurchase = false;
					}
					else if (offer.cost.ContainsKey(ResourceManager.HARD_CURRENCY))
					{
						costumeCost = offer.cost[ResourceManager.HARD_CURRENCY];
						currency = ResourceManager.HARD_CURRENCY;
						isJellyPurchase = true;
					}
					if (session.game.resourceManager.HasEnough(currency, costumeCost))
					{
						SBUIBuilder.ReleaseTopScreen();
						bool isInInventory = false;
						session.TheSoundEffectManager.PlaySound("OpenMenu");
						Action cancelButtonHandler3 = delegate()
						{
							session.TheSoundEffectManager.PlaySound("Error");
							SBUIBuilder.ReleaseTopScreen();
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping", true);
							if (isJellyPurchase)
							{
							}
						};
						Action okButtonHandler3 = delegate()
						{
							session.TheSoundEffectManager.PlaySound("Purchase");
							SBUIBuilder.ReleaseTopScreen();
							if (!session.TheGame.costumeManager.IsCostumeUnlocked(offer.identity))
							{
								session.TheGame.resourceManager.Spend(currency, costumeCost, session.game);
								ResourceManager.ApplyCostToGameState(currency, costumeCost, session.TheGame.gameState);
								session.TheGame.costumeManager.UnlockCostume(offer.identity);
								session.TheGame.simulation.ModifyGameState(new UnlockCostumeAction(offer.identity));
								CostumeManager.Costume costume = session.TheGame.costumeManager.GetCostume(offer.identity);
								if (costume != null)
								{
									AnalyticsWrapper.LogCostumePurchased(session.game, costume, currency, costumeCost);
									session.game.analytics.LogCostumeUnlocked(costume.m_nDID);
									AnalyticsWrapper.LogCostumeUnlocked(session.game, costume);
								}
								if (isJellyPurchase)
								{
								}
								if ((!session.game.inventory.HasItem(new int?(1015)) && (offer.identity == 2 || offer.identity == 21)) || (!session.game.inventory.HasItem(new int?(1017)) && (offer.identity == 7 || offer.identity == 24)) || (!session.game.inventory.HasItem(new int?(1016)) && offer.identity == 9) || (!session.game.inventory.HasItem(new int?(1013)) && offer.identity == 23))
								{
									isInInventory = false;
									session.game.selected = pSimulated;
									session.game.selected = pSimulated;
									session.AddAsyncResponse("purchasedCostume", offer.identity);
									session.properties.m_sLeaveType = "store_close_purchase_iap";
									session.ChangeState("UnitIdle", true);
								}
								else
								{
									isInInventory = true;
									Action okAction4 = delegate()
									{
										session.ChangeState("Playing", true);
									};
									session.ErrorMessageHandler(session, Language.Get("!!ERROR_CHARACTER_INVENTORY_TITLE"), Language.Get("!!ERROR_CHARACTER_INVENTORY"), Language.Get("!!PREFAB_OK"), okAction4, 0.9f);
								}
							}
							session.properties.storeVisitSinceLastPurchase = 0;
						};
						if (!isInInventory)
						{
							string text = string.Empty;
							if (offer.identity == 2 || offer.identity == 21)
							{
								text = "SpongeBob";
							}
							else if (offer.identity == 7 || offer.identity == 24)
							{
								text = "Patrick";
							}
							else if (offer.identity == 9)
							{
								text = "Squidward";
							}
							else if (offer.identity == 23)
							{
								text = "Mr. Krabs";
							}
							SBUIBuilder.MakeAndPushConfirmationDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), Language.Get(session.TheGame.costumeManager.GetCostume(offer.identity).m_sName), Language.Get("!!PREFAB_CONFIRM_COSTUME_PURCHASE_MESSAGE") + "|      " + Language.Get(text.ToString()), Language.Get("!!PREFAB_OK"), Language.Get("!!PREFAB_NEVERMIND"), Cost.DisplayDictionary(offer.cost, session.TheGame.resourceManager), okButtonHandler3, cancelButtonHandler3, false);
						}
					}
					else
					{
						Action okAction3 = delegate()
						{
							session.AddAsyncResponse("store_open_type", "store_open_too_poor_return");
							session.ChangeState("Shopping", true);
						};
						session.ErrorMessageHandler(session, Language.Get("!!PREFAB_NOT_ENOUGH") + " " + Language.Get(session.TheGame.resourceManager.Resources[currency].Name), Language.Get("!!PREFAB_NOT_ENOUGH_JJ_FOR_COINS"), Language.Get("!!PREFAB_OK"), okAction3, 1.1f);
					}
					return;
				}
				}
				TFUtils.Assert(false, "Unsupported Marketplace type: " + offer.type);
			};
			Action action2 = delegate()
			{
			};
			session.properties.shoppingHud = SBUIBuilder.MakeAndPushStandardUI(session, false, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), action, inventoryClickHandler, optionsHandler, editClickHandler, null, Session.DragFeeding.SwitchToFn(session), null, action2, action2, delegate
			{
				session.ChangeState("CommunityEvent", true);
			}, null, false);
			session.properties.shoppingHud.SetVisibleNonEssentialElements(false, true);
			SBGUIPulseButton sbguipulseButton = (SBGUIPulseButton)session.properties.shoppingHud.FindChild("marketplace");
			session.properties.marketplaceSessionActionID = sbguipulseButton.SessionActionId;
			sbguipulseButton.SessionActionId = session.properties.marketplaceSessionActionID + "_in_store";
			SBGUIMarketplaceScreen item = SBUIBuilder.MakeAndPushMarketplaceDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), action, purchaseClickHandler, session.game.entities, session.game.resourceManager, session.TheSoundEffectManager, session.game.catalog);
			Action action3 = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action3;
			session.game.questManager.OnShowDialogCallback = action3;
			session.game.communityEventManager.DialogNeededCallback = action3;
			session.game.sessionActionManager.SetActionHandler("shopping_ui", session, new List<SBGUIScreen>
			{
				session.properties.shoppingHud,
				item
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			session.PlayHavenController.RequestContent("shop_open");
			session.properties.m_sLeaveType = null;
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			soaringDictionary.addValue(session.TheGame.resourceManager.PlayerLevelAmount, "level");
			soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
			Soaring.FireEvent("OpenStore", soaringDictionary);
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x000780EC File Offset: 0x000762EC
		public void OnLeave(Session session)
		{
			SBGUIPulseButton sbguipulseButton = (SBGUIPulseButton)session.properties.shoppingHud.FindChild("marketplace");
			sbguipulseButton.SessionActionId = session.properties.marketplaceSessionActionID;
			object obj = session.CheckAsyncRequest("store_open_type");
			string sOpenType = null;
			if (obj != null)
			{
				sOpenType = (string)obj;
			}
			AnalyticsWrapper.LogMarketplaceUI(session.TheGame, "open", sOpenType, null);
			AnalyticsWrapper.LogMarketplaceUI(session.TheGame, "close", null, session.properties.m_sLeaveType);
			Debug.LogError(session.properties.m_sLeaveType);
			if (session.properties.m_sLeaveType == "store_open_too_poor_return" || session.properties.m_sLeaveType == "store_close_back_button")
			{
				Session.Shopping.FireFinishShoppingEvent(session);
				session.properties.storeVisitSinceLastPurchase++;
			}
			session.TheSoundEffectManager.PlaySound("CloseMenu");
			session.game.sessionActionManager.ClearActionHandler("shopping_ui", session);
			session.properties.shoppingHud.SetVisibleNonEssentialElements(true);
			session.properties.shoppingHud = null;
			session.TheCamera.TurnOffScreenBuffer();
			session.marketpalceActive = false;
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x00078224 File Offset: 0x00076424
		public static void FireFinishShoppingEvent(Session session)
		{
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			soaringDictionary.addValue(session.TheGame.resourceManager.PlayerLevelAmount, "level");
			soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
			Soaring.FireEvent("LeaveStore", soaringDictionary);
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00078278 File Offset: 0x00076478
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x0007827C File Offset: 0x0007647C
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x04000BFE RID: 3070
		private const string SHOPPING_UI_HANDLER = "shopping_ui";

		// Token: 0x04000BFF RID: 3071
		private static SBMarketOffer hackLastOffer;
	}

	// Token: 0x02000204 RID: 516
	public class ShowingDialog : Session.State
	{
		// Token: 0x06001154 RID: 4436 RVA: 0x000782EC File Offset: 0x000764EC
		private void AdvanceToNextDialog(SBGUIScreen screen, Session session, SBGUIScreen dialog)
		{
			session.game.dialogPackageManager.RemoveCurrentDialogInput(session.game);
			if (dialog != null)
			{
				dialog.Close();
			}
			int numQueuedDialogInputs = session.game.dialogPackageManager.GetNumQueuedDialogInputs();
			if (numQueuedDialogInputs == 0)
			{
				TFUtils.DebugLog("SessionShowingDialog.AdvanceToNextDialog - No more dialogs remaining to show, going to go back to Session Playing");
				session.AddAsyncResponse("dialogs_to_show", false);
				session.ChangeState("Playing", true);
			}
			else
			{
				DialogInputData dialogInputData = session.game.dialogPackageManager.PeekCurrentDialogInput();
				TFUtils.DebugLog(string.Format("SessionShowingDialog.AdvanceToNextDialog - There are {0} dialogs remaining to show, going to create the next dialog with inputdata of type {1}", numQueuedDialogInputs, dialogInputData.GetType()));
				this.CreateOrAdvanceDialog(dialogInputData, screen, session);
			}
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x0007839C File Offset: 0x0007659C
		private SBGUIScreen CreateCharacterDialog(CharacterDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateCharacterDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUICharacterDialog dialog = SBUIBuilder.MakeAndAddDialogSequence(screen, session, inputData.PromptsData, null);
			TFUtils.DebugLog("SessionShowingDialog.CreateCharacterDialog - dialog created.");
			dialog.DialogChange.AddListener(delegate(int page)
			{
				if (page < 0)
				{
					this.AdvanceToNextDialog(screen, session, dialog);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("OpenMenu");
				}
			});
			return dialog;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00078418 File Offset: 0x00076618
		private SBGUIScreen CreateQuestDialog(QuestDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			if (inputData.PromptData == null)
			{
				if (inputData.SequenceId == 10000U)
				{
					inputData = QuestDefinition.RecreateRandomQuestStartInputData(session.TheGame, QuestDefinition.LastRandomQuestId);
				}
				else if (inputData.SequenceId == 10001U)
				{
					inputData = QuestDefinition.RecreateRandomQuestCompleteInputData(session.TheGame, QuestDefinition.LastRandomQuestId);
				}
			}
			List<ConditionDescription> steps = new List<ConditionDescription>();
			Dictionary<string, object> promptData = inputData.PromptData;
			string key = string.Empty;
			string text = string.Empty;
			string text2 = string.Empty;
			if (promptData.ContainsKey("title"))
			{
				key = TFUtils.LoadString(promptData, "title");
			}
			if (promptData.ContainsKey("icon"))
			{
				text = TFUtils.LoadString(promptData, "icon");
			}
			if (promptData.ContainsKey("type"))
			{
				text2 = TFUtils.LoadString(promptData, "type");
			}
			TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - deciding which quest dialog to create. Type is: " + text2);
			List<Reward> list = new List<Reward>();
			if (inputData.ContextData != null)
			{
				List<object> list2 = TFUtils.LoadList<object>(inputData.ContextData, "rewards");
				foreach (object o in list2)
				{
					list.Add(Reward.FromObject(o));
				}
			}
			TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - generated rewards list");
			string title = Language.Get(key);
			SBGUIScreen dialog = null;
			if (text2.Equals("quest_start"))
			{
				uint questDid = session.TheGame.questManager.ActiveQuestDids.Last();
				if (inputData.QuestId != null)
				{
					questDid = inputData.QuestId.Value;
				}
				QuestDefinition questDefinition = this.RetrieveQuestDefinition(session, questDid);
				if (questDefinition == null)
				{
					this.AdvanceToNextDialog(screen, session, dialog);
					return dialog;
				}
				steps = this.RetrieveQuestConditionDescriptions(session, questDid);
				if (questDefinition.Chunk)
				{
					if (questDefinition.Did == QuestDefinition.LastAutoQuestId)
					{
						Action allDoneButton = delegate()
						{
							session.TheSoundEffectManager.PlaySound("CloseQuestDialog");
							this.AdvanceToNextDialog(screen, session, dialog);
							session.TheGame.simulation.ModifyGameState(new AutoQuestAllDoneAction(QuestDefinition.LastAutoQuestId));
						};
						Action makeButton = delegate()
						{
							session.TheSoundEffectManager.PlaySound("CloseQuestDialog");
							this.AdvanceToNextDialog(screen, session, dialog);
						};
						dialog = SBUIBuilder.MakeAndAddAutoQuestStartDialog(screen, session.properties.dialogHud, session, list, questDefinition, steps, allDoneButton, makeButton);
					}
					else
					{
						Action findButton = delegate()
						{
							session.TheSoundEffectManager.PlaySound("CloseQuestDialog");
							this.AdvanceToNextDialog(screen, session, dialog);
						};
						TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - going to make Quest CHUNK Start Dialog, sending it through to SBUIBuilder");
						dialog = SBUIBuilder.MakeAndAddQuestChunkStartDialog(screen, session.properties.dialogHud, session, list, questDefinition, steps, findButton);
					}
				}
				else
				{
					TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - going to make Quest NORMAL Start Dialog, sending it through to SBUIBuilder");
					dialog = SBUIBuilder.MakeAndAddQuestStartDialog(screen, session, list, title, text);
				}
			}
			else if (text2.Equals("quest_complete"))
			{
				uint num = session.TheGame.questManager.CompletedQuestDids.Last();
				if (inputData.QuestId != null)
				{
					num = inputData.QuestId.Value;
				}
				QuestDefinition questDefinition = this.RetrieveQuestDefinition(session, num);
				if (questDefinition == null)
				{
					TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - Is QuestCompleteDialogInputData, tried gettings questDefinition but null was returned when questDid: " + num + " was passed to RetriveQuestDefinition.");
					this.AdvanceToNextDialog(screen, session, dialog);
					return dialog;
				}
				steps = this.RetrieveQuestConditionDescriptions(session, num);
				TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - Is QuestCompleteDialogInputData, conditionDescriptions retrieved.");
				if (questDefinition.Chunk)
				{
					TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - going to make Quest CHUNK Complete Dialog, sending it through to SBUIBuilder");
					if (questDefinition.Did == QuestDefinition.LastAutoQuestId)
					{
						dialog = SBUIBuilder.MakeAndAddAutoQuestCompleteDialog(screen, null, session, list, questDefinition);
					}
					else
					{
						dialog = SBUIBuilder.MakeAndAddQuestChunkCompleteDialog(screen, null, session, list, questDefinition, steps);
					}
				}
				else
				{
					TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - going to make Quest NORMAL Complete Dialog, sending it through to SBUIBuilder");
					dialog = SBUIBuilder.MakeAndAddQuestCompleteDialog(screen, session, list, title, text);
				}
			}
			else if (text2.Equals("booty_quest_complete"))
			{
				dialog = SBUIBuilder.MakeAndAddBootyQuestCompleteDialog(screen, session, list, title, text);
			}
			else if (text2.Equals("quest_line_start") || text2.Equals("quest_line_complete"))
			{
				string text3 = string.Empty;
				string text4 = string.Empty;
				string text5 = string.Empty;
				string text6 = string.Empty;
				string text7 = string.Empty;
				List<Reward> list3 = new List<Reward>();
				TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, going to get heading, body, portrait, and reward", text2));
				if (promptData.ContainsKey("heading") && promptData.ContainsKey("body") && promptData.ContainsKey("portrait") && promptData.ContainsKey("reward"))
				{
					text3 = TFUtils.LoadString(promptData, "heading");
					TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, got heading: {1}", text2, text3));
					text4 = TFUtils.LoadString(promptData, "body");
					TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, got body: {1}", text2, text4));
					text5 = TFUtils.LoadString(promptData, "portrait");
					TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, got portrait: {1}", text2, text5));
					Dictionary<string, object> dictionary = TFUtils.LoadDict(promptData, "reward");
					TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, got reward.", text2));
					if (dictionary.ContainsKey("buildings"))
					{
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'buildings' key, going to get texture and name.", text2));
						Dictionary<string, object> dictionary2 = TFUtils.LoadDict(dictionary, "buildings");
						int num2 = int.Parse(dictionary2.Keys.First<string>());
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'buildings' key, did is {1}", text2, num2));
						Blueprint blueprint = EntityManager.GetBlueprint("building", num2, false);
						if (blueprint == null)
						{
							TFUtils.ErrorLog("SessionShowingDialog.CreateQuestDialog - blueprint is null");
						}
						text7 = (string)blueprint.Invariable["portrait"];
						text6 = Language.Get((string)blueprint.Invariable["name"]);
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'buildings' key, name is {1}, textures is {2}", text2, text6, text7));
					}
					else if (dictionary.ContainsKey("recipes"))
					{
						Dictionary<string, object> dictionary3 = TFUtils.LoadDict(dictionary, "buildings");
						int id = int.Parse(dictionary3.Keys.First<string>());
						int productId = session.TheGame.craftManager.GetRecipeById(id).productId;
						Resource resource = session.TheGame.resourceManager.Resources[productId];
						text7 = resource.GetResourceTexture();
						text6 = Language.Get(resource.Name);
					}
					if (dictionary.ContainsKey("resources"))
					{
						if (dictionary.ContainsKey("buildings"))
						{
							dictionary.Remove("buildings");
						}
						if (dictionary.ContainsKey("recipes"))
						{
							dictionary.Remove("recipes");
						}
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'resources' key, getting promptReward from reward", text2));
						Reward reward = Reward.FromDict(dictionary);
						TFUtils.DebugLog(string.Format("SessionShowingDialog.CreateQuestDialog - Is {0}, reward has 'resources' key, loaded promptReward", text2));
						if (reward == null)
						{
							TFUtils.DebugLog("SessionShowingDialog.CreateQuestDialog - promptReward is null");
						}
						list3.Add(reward);
					}
				}
				else
				{
					TFUtils.ErrorLog(string.Format("Dialog sequenceId {0} prompt type '{1}' needs to contain all of the following fields: 'heading', 'body', 'portrait', 'reward'", inputData.SequenceId, text2));
				}
				if (!string.IsNullOrEmpty(text))
				{
					text7 = text;
				}
				if (text2.Equals("quest_line_start"))
				{
					dialog = SBUIBuilder.MakeAndAddQuestLineStartDialog(screen, session, list3, text3, text4, text5, text7, string.Empty);
				}
				else
				{
					dialog = SBUIBuilder.MakeAndAddQuestLineCompleteDialog(screen, session, list3, text3, text4, text5, text7, text6);
				}
			}
			Action okHandler = delegate()
			{
				session.TheSoundEffectManager.PlaySound("CloseQuestDialog");
				this.AdvanceToNextDialog(screen, session, dialog);
				AndroidBack.getInstance().pop();
			};
			Action action = delegate()
			{
				okHandler();
			};
			AndroidBack.getInstance().push(action, dialog);
			dialog.AttachActionToButton("okay", okHandler);
			if (text2.Equals("quest_line_start") || text2.Equals("quest_line_complete"))
			{
				dialog.AttachActionToButton("TouchableBackground", delegate()
				{
				});
			}
			else
			{
				dialog.AttachActionToButton("TouchableBackground", okHandler);
			}
			return dialog;
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00078D44 File Offset: 0x00076F44
		private SBGUIScreen CreateLevelUpDialog(LevelUpDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateLevelUpDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUILevelUpScreen dialog = SBUIBuilder.MakeAndAddLevelUpDialog(screen, session, inputData);
			TFUtils.DebugLog("SessionShowingDialog.CreateLevelUpDialog - dialog created.");
			session.TheCamera.TurnOnScreenBuffer();
			Action action = delegate()
			{
				session.PlayHavenController.RequestContent("level_" + inputData.NewLevel);
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(inputData.NewLevel, "level");
				soaringDictionary.addValue(inputData.NewLevel - 1, "old_level");
				soaringDictionary.addValue(SBSettings.BundleVersion, "client_version");
				Soaring.FireEvent("LevelUp", soaringDictionary);
				session.TheSoundEffectManager.PlaySound("CloseLevelUpDialog");
				this.AdvanceToNextDialog(screen, session, dialog);
				session.TheCamera.TurnOffScreenBuffer();
				Renderer component = dialog.FindChild("windows").GetComponent<Renderer>();
				Renderer component2 = dialog.FindChild("spinning_paper").GetComponent<Renderer>();
				Renderer component3 = dialog.FindChild("headline_image").GetComponent<Renderer>();
				Resources.UnloadAsset(component.material.mainTexture);
				Resources.UnloadAsset(component2.material.mainTexture);
				Resources.UnloadAsset(component3.material.mainTexture);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate()
			{
			});
			return dialog;
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00078E08 File Offset: 0x00077008
		private SBGUIScreen CreateFoundItemDialog(FoundItemDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateFoundItemDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIFoundItemScreen dialog = SBUIBuilder.MakeAndAddFoundItemScreen(session, screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateFoundItemDialog - dialog created.");
			Action action = delegate()
			{
				session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
				this.AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate()
			{
			});
			dialog.Setup(inputData.Title, inputData.Message, inputData.Icon, false, string.Empty);
			session.TheSoundEffectManager.PlaySound("Error");
			return dialog;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00078EE8 File Offset: 0x000770E8
		private SBGUIScreen CreateFoundMovieDialog(FoundMovieDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateFoundMovieDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIFoundItemScreen dialog = SBUIBuilder.MakeAndAddFoundItemScreen(session, screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateFoundMovieDialog - dialog created.");
			Action action = delegate()
			{
				session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
				this.AdvanceToNextDialog(screen, session, dialog);
			};
			Action action2 = delegate()
			{
				session.PlayMovieFromShowingDialog(inputData.Movie);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("extra_button", action2);
			dialog.AttachActionToButton("TouchableBackground", delegate()
			{
			});
			dialog.Setup(inputData.Title, inputData.Message, inputData.Icon, true, Language.Get("!!MOVIE_WATCH_NOW"));
			session.TheSoundEffectManager.PlaySound("Error");
			return dialog;
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00079004 File Offset: 0x00077204
		private SBGUIScreen CreateExplanationDialog(ExplanationDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateExplanationDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIExplanationDialog dialog = SBUIBuilder.MakeAndAddExplanationDialog(screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateExplanationDialog - dialog created.");
			Action action = delegate()
			{
				this.AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("skip_button", action);
			dialog.Setup(inputData.Message);
			session.TheSoundEffectManager.PlaySound("Error");
			return dialog;
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x000790A0 File Offset: 0x000772A0
		private SBGUIScreen CreateMoveInDialog(MoveInDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateMoveInDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIMoveInDialog dialog = SBUIBuilder.MakeAndAddMoveInDialog(screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateMoveInDialog - dialog created.");
			Action action = delegate()
			{
				this.AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate()
			{
			});
			dialog.Setup(inputData.CharacterName, inputData.BuildingName, inputData.PortraitTexture);
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			return dialog;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00079174 File Offset: 0x00077374
		private SBGUIScreen CreateTreasureDialog(TreasureDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateTreasureDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUIFoundItemScreen dialog = SBUIBuilder.MakeAndAddFoundItemScreen(session, screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateTreasureDialog - dialog created.");
			Action action = delegate()
			{
				session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
				this.AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate()
			{
			});
			dialog.Setup(inputData.Title, inputData.Message, "TreasureChest_Closed.png", false, string.Empty);
			session.TheSoundEffectManager.PlaySound("Error");
			return dialog;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00079254 File Offset: 0x00077454
		private SBGUIScreen CreateSpongyGamesDialog(SpongyGamesDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateSpongyGamesDialog - passing inputData to SBUIBuilder to create screen.");
			SBGUISpongyGamesDialog dialog = SBUIBuilder.MakeAndAddSpongyGamesDialog(screen);
			TFUtils.DebugLog("SessionShowingDialog.CreateSpongyGamesDialog - dialog created.");
			Action action = delegate()
			{
				session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
				this.AdvanceToNextDialog(screen, session, dialog);
			};
			dialog.AttachActionToButton("okay", action);
			dialog.AttachActionToButton("TouchableBackground", delegate()
			{
			});
			dialog.Setup(inputData);
			session.TheSoundEffectManager.PlaySound("OpenMenu");
			return dialog;
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00079318 File Offset: 0x00077518
		private SBGUIScreen CreateDailyBonusDialog(DailyBonusDialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.DebugLog("SessionShowingDialog.CreateDailyBonusDialog - passing inputData to SBUIBuilder to create screen.");
			if (inputData.DailyBonusData != null)
			{
				SBGUIDailyBonusDialog dialog = SBUIBuilder.MakeAndAddDailyBonusDialog(screen);
				TFUtils.DebugLog("SessionShowingDialog.CreateDailyBonusDialog - dialog created.");
				Action action = delegate()
				{
					session.TheSoundEffectManager.PlaySound("CloseFoundItemDialog");
					this.AdvanceToNextDialog(screen, session, dialog);
					dialog.applyReward(session);
				};
				dialog.AttachActionToButton("okay", action);
				dialog.AttachActionToButton("TouchableBackground", delegate()
				{
				});
				dialog.Setup(inputData, session);
				session.TheSoundEffectManager.PlaySound("OpenMenu");
				return dialog;
			}
			Debug.LogError("Viet's Debug - Daily Bonus Disabled");
			this.AdvanceToNextDialog(screen, session, null);
			return null;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x00079420 File Offset: 0x00077620
		private void CreateOrAdvanceDialog(DialogInputData inputData, SBGUIScreen screen, Session session)
		{
			SBGUIScreen sbguiscreen = null;
			try
			{
				sbguiscreen = this.CreateDialog(inputData, screen, session);
			}
			catch (Exception ex)
			{
				TFUtils.LogDump(session, "failed_dialog", ex, null);
				SBUIBuilder.ReleaseTopScreen();
				screen = SBUIBuilder.MakeAndPushScratchLayer(session);
			}
			if (sbguiscreen == null)
			{
				this.AdvanceToNextDialog(screen, session, sbguiscreen);
			}
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x00079490 File Offset: 0x00077690
		private SBGUIScreen CreateDialog(DialogInputData inputData, SBGUIScreen screen, Session session)
		{
			TFUtils.Assert(inputData != null, "Don't call CreateDialog with null inputData.");
			SBGUIScreen result = null;
			if (inputData is CharacterDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is CharacterDialogInputData, going to create dialog.");
				result = this.CreateCharacterDialog((CharacterDialogInputData)inputData, screen, session);
			}
			else if (inputData is QuestDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is QuestDialogInputData, going to create dialog.");
				result = this.CreateQuestDialog((QuestDialogInputData)inputData, screen, session);
			}
			else if (inputData is LevelUpDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is LevelUpDialogInputData, going to create dialog.");
				result = this.CreateLevelUpDialog((LevelUpDialogInputData)inputData, screen, session);
			}
			else if (inputData is FoundMovieDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is FoundMovieDialogInputData, going to create dialog.");
				result = this.CreateFoundMovieDialog((FoundMovieDialogInputData)inputData, screen, session);
			}
			else if (inputData is FoundItemDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is FoundItemDialogInputData, going to create dialog.");
				result = this.CreateFoundItemDialog((FoundItemDialogInputData)inputData, screen, session);
			}
			else if (inputData is ExplanationDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is ExplanationDialogInputData, going to create dialog.");
				result = this.CreateExplanationDialog((ExplanationDialogInputData)inputData, screen, session);
			}
			else if (inputData is MoveInDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is MoveInDialogInputData, ggoing to create dialog.");
				result = this.CreateMoveInDialog((MoveInDialogInputData)inputData, screen, session);
			}
			else if (inputData is TreasureDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is TreasureDialogInputData, going to create dialog.");
				result = this.CreateTreasureDialog((TreasureDialogInputData)inputData, screen, session);
			}
			else if (inputData is SpongyGamesDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is SpongyGamesDialogInputData, going to create dialog.");
				result = this.CreateSpongyGamesDialog((SpongyGamesDialogInputData)inputData, screen, session);
			}
			else if (inputData is DailyBonusDialogInputData)
			{
				TFUtils.DebugLog("SessionShowingDialog.CreateDialog - InputData is DailyBonusDialogInputData, going to create dialog.");
				result = this.CreateDailyBonusDialog((DailyBonusDialogInputData)inputData, screen, session);
			}
			session.soundEffectManager.PlaySound(inputData.SoundImmediate);
			session.soundEffectManager.PlaySound(inputData.SoundBeat, 1f);
			return result;
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00079670 File Offset: 0x00077870
		public void OnEnter(Session session)
		{
			GUIMainView.GetInstance().Library.bShowingDialog = true;
			session.game.dropManager.MarkForClearCurrentDrops();
			Action shopClickHandler = delegate()
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping", true);
			};
			SBGUIStandardScreen dialogHud = SBUIBuilder.MakeAndPushStandardUI(session, true, null, shopClickHandler, null, null, null, null, null, null, null, null, null, null, false);
			TFUtils.DebugLog("SessionShowingDialog.OnEnter - created dialogHud");
			session.properties.dialogHud = dialogHud;
			session.properties.dialogHud.EnableUI(false);
			SBGUIScreen screen = SBUIBuilder.MakeAndPushScratchLayer(session);
			int numQueuedDialogInputs = session.game.dialogPackageManager.GetNumQueuedDialogInputs();
			TFUtils.Assert(numQueuedDialogInputs > 0, "Should not get into the ShowingDialog Session state unless there are some dialogs!");
			DialogInputData inputData = session.game.dialogPackageManager.PeekCurrentDialogInput();
			this.CreateOrAdvanceDialog(inputData, screen, session);
			session.camera.SetEnableUserInput(false, false, default(Vector3));
			SessionActionSimulationHelper.EnableHandler(session, false);
			RestrictInteraction.AddWhitelistSimulated(session.game.simulation, int.MinValue);
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x000797B0 File Offset: 0x000779B0
		public void OnLeave(Session session)
		{
			session.properties.dialogHud.EnableUI(true);
			session.properties.dialogHud = null;
			SessionActionSimulationHelper.EnableHandler(session, true);
			RestrictInteraction.RemoveWhitelistSimulated(session.game.simulation, int.MinValue);
			GUIMainView.GetInstance().Library.bShowingDialog = false;
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00079808 File Offset: 0x00077A08
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x0007986C File Offset: 0x00077A6C
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x00079870 File Offset: 0x00077A70
		private QuestDefinition RetrieveQuestDefinition(Session session, uint questDid)
		{
			Quest quest = session.TheGame.questManager.GetQuest(questDid);
			if (quest == null)
			{
				return null;
			}
			return session.TheGame.questManager.GetQuestDefinition(quest.Did);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000798B0 File Offset: 0x00077AB0
		private List<ConditionDescription> RetrieveQuestConditionDescriptions(Session session, uint questDid)
		{
			Quest quest = session.TheGame.questManager.GetQuest(questDid);
			if (quest == null)
			{
				return null;
			}
			List<ConditionDescription> list = new List<ConditionDescription>();
			foreach (ConditionState conditionState in quest.EndConditions)
			{
				list.AddRange(conditionState.Describe(session.TheGame));
			}
			return list;
		}
	}

	// Token: 0x02000205 RID: 517
	public class StashBuildingConfirmation : Session.State
	{
		// Token: 0x06001170 RID: 4464 RVA: 0x0007996C File Offset: 0x00077B6C
		public void OnEnter(Session session)
		{
			this.Setup(session);
			session.camera.SetEnableUserInput(false, false, default(Vector3));
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00079998 File Offset: 0x00077B98
		public void Setup(Session session)
		{
			string text = null;
			object obj = session.CheckAsyncRequest("stash_error");
			if (obj != null)
			{
				text = (string)obj;
			}
			Simulated simulated = (Simulated)session.CheckAsyncRequest("to_stash");
			object obj2 = session.CheckAsyncRequest("in_state_move_in_edit");
			bool bMoveInEdit = false;
			if (obj2 != null)
			{
				bMoveInEdit = (bool)obj2;
			}
			if (session.TheState.GetType().Equals(typeof(Session.MoveBuildingInEdit)))
			{
				Session.MoveBuildingInEdit moveBuildingInEdit = (Session.MoveBuildingInEdit)session.TheState;
				moveBuildingInEdit.DeactivateInteractionStrip(session);
			}
			BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
			if (!string.IsNullOrEmpty(text))
			{
				Action okButtonHandler = delegate()
				{
					session.TheSoundEffectManager.PlaySound("CloseMenu");
					session.ChangeState((!bMoveInEdit) ? "MoveBuildingInPlacement" : "MoveBuildingInEdit", true);
				};
				session.TheSoundEffectManager.PlaySound("OpenMenu");
				SBUIBuilder.MakeAndPushAcknowledgeDialog(session, new Action<SBGUIEvent, Session>(this.HandleSBGUIEvent), Language.Get("!!CANNOT_STOW_DIALOG_TITLE"), Language.Get(text), (string)entity.Invariable["portrait"], Language.Get("!!PREFAB_OK"), okButtonHandler);
			}
			else
			{
				session.ChangeState((!bMoveInEdit) ? "MoveBuildingInPlacement" : "MoveBuildingInEdit", true);
			}
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x00079B10 File Offset: 0x00077D10
		public void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x00079B14 File Offset: 0x00077D14
		public void OnLeave(Session session)
		{
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00079B18 File Offset: 0x00077D18
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, false);
		}
	}

	// Token: 0x02000206 RID: 518
	public interface State
	{
		// Token: 0x06001175 RID: 4469
		void OnEnter(Session session);

		// Token: 0x06001176 RID: 4470
		void OnLeave(Session session);

		// Token: 0x06001177 RID: 4471
		void OnUpdate(Session session);
	}

	// Token: 0x02000207 RID: 519
	public class Stopping : Session.State
	{
		// Token: 0x06001179 RID: 4473 RVA: 0x00079B84 File Offset: 0x00077D84
		public void OnEnter(Session session)
		{
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00079B88 File Offset: 0x00077D88
		public void OnLeave(Session session)
		{
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00079B8C File Offset: 0x00077D8C
		public void OnUpdate(Session session)
		{
		}
	}

	// Token: 0x02000208 RID: 520
	public class Sync : Session.State
	{
		// Token: 0x0600117D RID: 4477 RVA: 0x00079B98 File Offset: 0x00077D98
		public void OnEnter(Session session)
		{
			session.camera.StopCamera();
			session.TheSoundEffectManager.Enabled = false;
			if (session.reinitializeSession)
			{
				TFUtils.DebugLog("session.reinitializeSession");
				this.Reauthenticate(session);
			}
			else if (session.TheGame.ReloadToFriendPark())
			{
				TFUtils.DebugLog("session.TheGame.ReloadToFriendPark()");
				session.TheGame.ClearLoadFriendPark();
				this.ReloadToFriendsSession(session);
			}
			else if (session.TheGame.RequiresReload())
			{
				TFUtils.DebugLog("session.TheGame.RequiresReinitialize()");
				session.TheGame.ClearReloadRequest();
				this.ReloadFromDisk(session);
			}
			else if (session.resyncConnection)
			{
				Session.GameStarting.CreateLoadingScreen(session, false, "starting_progress", false);
				this.mWasResync = true;
			}
			else
			{
				TFUtils.DebugLog("ReloadFromNetwork");
				this.ReloadFromNetwork(session);
			}
			this.mResyncStartTime = Time.realtimeSinceStartup;
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00079C80 File Offset: 0x00077E80
		public void OnLeave(Session session)
		{
			this.mWasResync = false;
			session.resyncConnection = false;
			session.camera.StartCamera();
			session.TheSoundEffectManager.StartSoundEffectsManager();
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00079CB4 File Offset: 0x00077EB4
		public void OnUpdate(Session session)
		{
			if (session.resyncConnection)
			{
				if (Time.realtimeSinceStartup - this.mResyncStartTime > 2f)
				{
					session.ChangeState("Playing", true);
					session.resyncConnection = false;
				}
				else
				{
					if (!Soaring.IsOnline)
					{
						SoaringInternal.instance.ClearOfflineMode();
					}
					if (Soaring.IsOnline)
					{
						session.resyncConnection = false;
						session.reinitializeSession = true;
						this.Reauthenticate(session);
					}
				}
			}
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00079D30 File Offset: 0x00077F30
		private void Reauthenticate(Session session)
		{
			TFUtils.DebugLog("Logging in with possibly different credentials.");
			session.gameIsReloading = true;
			session.reinitializeSession = false;
			session.ClearUserState();
			session.InFriendsGame = false;
			session.ChangeState("Authorizing", true);
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00079D70 File Offset: 0x00077F70
		private void ReloadFromDisk(Session session)
		{
			this.CleanUp(session);
			session.InFriendsGame = false;
			session.ChangeState("GameStarting", true);
			Session.GameStarting.CreateLoadingScreen(session, false, "visit_starting_progress", true);
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00079DA8 File Offset: 0x00077FA8
		private void ReloadToFriendsSession(Session session)
		{
			this.CleanUp(session);
			session.ChangeState("visit_friend", true);
			Session.GameStarting.CreateLoadingScreen(session, false, "visit_starting_progress", true);
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x00079DD8 File Offset: 0x00077FD8
		private void ReloadFromNetwork(Session session)
		{
			this.CleanUp(session);
			session.InFriendsGame = false;
			session.TheGame.DestroyCache();
			session.WebFileServer.DeleteETagFile();
			session.ChangeState("GameStarting", true);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x00079E18 File Offset: 0x00078018
		private void CleanUp(Session session)
		{
			SBUIBuilder.UpdateGuiEventHandler(session, delegate(SBGUIEvent e, Session s)
			{
			});
			session.ClearAsyncRequests();
			SoaringInternal.instance.ClearSoaringWebQueue();
			session.game.Clear();
		}

		// Token: 0x04000C0A RID: 3082
		private float mResyncStartTime;

		// Token: 0x04000C0B RID: 3083
		private bool mWasResync;
	}

	// Token: 0x02000209 RID: 521
	public class UnitBusy : Session.State
	{
		// Token: 0x06001187 RID: 4487 RVA: 0x00079E70 File Offset: 0x00078070
		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			Simulated sim = session.game.selected;
			if (sim == null)
			{
				TFUtils.DebugLog("attempted to transition to unit busy without a selected entity");
				session.ChangeState("Playing", true);
				return;
			}
			Action closeButton = delegate()
			{
				session.ChangeState("Playing", true);
				AndroidBack.getInstance().pop();
			};
			session.properties.unitBusyHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory", true);
			}, delegate
			{
				session.ChangeState("Options", true);
			}, delegate
			{
				session.ChangeState("Editing", true);
			}, null, null, null, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.ChangeState("CommunityEvent", true);
			}, null, false);
			session.properties.unitBusyHud.HideAllElements();
			Action pFeedWishAction = delegate()
			{
				ResidentEntity entity = sim.GetEntity<ResidentEntity>();
				if (entity != null && entity.HungerResourceId != null)
				{
					session.TheGame.simulation.Router.Send(OfferFoodCommand.Create(sim.Id, sim.Id, entity.HungerResourceId.Value));
					session.ChangeState("Playing", true);
				}
			};
			Action pRushWishAction = delegate()
			{
				ResidentEntity pEntity = sim.GetEntity<ResidentEntity>();
				if (pEntity != null && pEntity.HungerResourceId == null)
				{
					Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
					{
						session.analytics.LogRushFullness(sim.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
					};
					Action execute = delegate()
					{
						session.TheGame.simulation.Router.Send(RushCommand.Create(sim.Id));
						session.ChangeState("UnitBusy", true);
						session.properties.transitionSilently = false;
						session.ChangeState("Playing", true);
					};
					Action complete = delegate()
					{
						int nJellyCost = 0;
						pEntity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out nJellyCost);
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, pEntity.DefinitionId, nJellyCost, pEntity.Name, "characters", "speedup", "fullness", "confirm");
					};
					Action cancel = delegate()
					{
						session.TheGame.simulation.Router.Send(AbortCommand.Create(sim.Id, sim.Id));
						session.ChangeState("UnitBusy", true);
						int num = 0;
						pEntity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out num);
					};
					session.properties.transitionSilently = true;
					session.properties.hardSpendActions = new Session.HardSpendActions(execute, (ulong ts) => pEntity.FullnessRushCostNow(), pEntity.BlueprintName, pEntity.DefinitionId, complete, cancel, logSpend, session.properties.unitBusyWindow.GetWishWidgetRushButtonPosition());
					session.ChangeState("HardSpendConfirm", false);
				}
			};
			Task pTask = null;
			List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(sim.entity.DefinitionId, sim.Id, true);
			if (activeTasksForSimulated.Count > 0)
			{
				pTask = activeTasksForSimulated[0];
			}
			if (pTask != null)
			{
				session.properties.unitBusyTask = pTask;
			}
			else
			{
				pTask = session.properties.unitBusyTask;
			}
			Action pRushTaskAction = delegate()
			{
				if (pTask != null)
				{
					Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
					{
					};
					Action execute = delegate()
					{
						session.ChangeState("UnitBusy", true);
						session.properties.transitionSilently = false;
						session.ChangeState("Playing", true);
						Cost cost = pTask.RushCostNow();
						session.TheGame.resourceManager.Apply(cost, session.TheGame);
						pTask.m_ulCompleteTime = TFUtils.EpochTime();
						session.TheGame.simulation.ModifyGameState(new TaskRushAction(pTask, cost));
						session.TheGame.simulation.Router.Send(RushTaskCommand.Create(sim.Id, sim.Id));
					};
					Action complete = delegate()
					{
						Cost cost = pTask.RushCostNow();
						int nJellyCost;
						if (cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
						{
							nJellyCost = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
						}
						nJellyCost = session.properties.unitBusyWindow.taskRushCost;
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, pTask.m_pTaskData.m_nDID, nJellyCost, pTask.m_pTaskData.m_sName, "task", "speedup", "task", "confirm");
					};
					Action cancel = delegate()
					{
						Cost cost = pTask.RushCostNow();
						if (cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
						{
							int num = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
						}
						session.ChangeState("UnitBusy", true);
					};
					session.properties.transitionSilently = true;
					session.properties.hardSpendActions = new Session.HardSpendActions(execute, (ulong ts) => pTask.RushCostNow(), pTask.m_pTaskData.m_sName, pTask.m_pTaskData.m_nDID, complete, cancel, logSpend, session.properties.unitBusyWindow.GetTaskRushButtonPosition());
					session.ChangeState("HardSpendConfirm", false);
				}
			};
			session.properties.unitBusyWindow = SBUIBuilder.MakeAndPushUnitBusyUI(session.properties.unitBusyHud, session, sim, pTask, pFeedWishAction, pRushWishAction, pRushTaskAction, closeButton);
			session.properties.transitionSilently = false;
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.sessionActionManager.SetActionHandler("unit_busy_ui", session, new List<SBGUIScreen>
			{
				session.properties.unitBusyHud,
				session.properties.unitBusyWindow
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0007A184 File Offset: 0x00078384
		public void OnLeave(Session session)
		{
			session.game.sessionActionManager.ClearActionHandler("unit_busy_ui", session);
			if (!session.properties.transitionSilently)
			{
				if (session.TheCamera.ScreenBufferOn)
				{
					session.TheCamera.TurnOffScreenBuffer();
				}
				session.properties.unitBusyHud = null;
				session.properties.unitBusyWindow = null;
			}
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0007A1EC File Offset: 0x000783EC
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
			Simulated selected = session.game.selected;
			Task task = null;
			List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(selected.entity.DefinitionId, selected.Id, true);
			if (activeTasksForSimulated.Count > 0)
			{
				task = activeTasksForSimulated[0];
			}
			if (task == null || task.GetTimeLeft() <= 0UL)
			{
				session.ChangeState("Playing", true);
			}
		}

		// Token: 0x04000C0D RID: 3085
		private const string UNIT_BUSY_UI_HANDLER = "unit_busy_ui";
	}

	// Token: 0x0200020A RID: 522
	public class UnitIdle : Session.State
	{
		// Token: 0x0600118B RID: 4491 RVA: 0x0007A2BC File Offset: 0x000784BC
		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			Simulated sim = session.game.selected;
			if (sim == null)
			{
				TFUtils.DebugLog("attempted to transition to unit busy without a selected entity");
				session.ChangeState("Playing", true);
				return;
			}
			Action closeButton = delegate()
			{
				if (session.properties.unitIdleWindow != null)
				{
					session.properties.unitIdleWindow.ClearList();
				}
				session.ChangeState("Playing", true);
				AndroidBack.getInstance().pop();
			};
			session.properties.unitIdleHud = SBUIBuilder.MakeAndPushStandardUI(session, false, null, delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory", true);
			}, delegate
			{
				session.ChangeState("Options", true);
			}, delegate
			{
				session.ChangeState("Editing", true);
			}, null, null, null, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.ChangeState("CommunityEvent", true);
			}, null, false);
			session.properties.unitIdleHud.HideAllElements();
			Action pFeedWishAction = delegate()
			{
				ResidentEntity entity = sim.GetEntity<ResidentEntity>();
				if (entity != null && entity.HungerResourceId != null)
				{
					session.TheGame.simulation.Router.Send(OfferFoodCommand.Create(sim.Id, sim.Id, entity.HungerResourceId.Value));
					session.ChangeState("Playing", true);
				}
			};
			Action pRushWishAction = delegate()
			{
				ResidentEntity pEntity = sim.GetEntity<ResidentEntity>();
				if (pEntity != null && pEntity.HungerResourceId == null)
				{
					Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
					{
						session.analytics.LogRushFullness(sim.entity.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
					};
					Action execute = delegate()
					{
						session.TheGame.simulation.Router.Send(RushCommand.Create(sim.Id));
						session.ChangeState("UnitIdle", true);
						session.properties.transitionSilently = false;
						session.ChangeState("Playing", true);
					};
					Action complete = delegate()
					{
						int nJellyCost = 0;
						pEntity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out nJellyCost);
						AnalyticsWrapper.LogJellyConfirmation(session.TheGame, pEntity.DefinitionId, nJellyCost, pEntity.Name, "characters", "speedup", "fullness", "confirm");
					};
					Action cancel = delegate()
					{
						session.TheGame.simulation.Router.Send(AbortCommand.Create(sim.Id, sim.Id));
						session.ChangeState("UnitIdle", true);
						int num = 0;
						pEntity.FullnessRushCostNow().ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out num);
					};
					session.properties.transitionSilently = true;
					session.properties.hardSpendActions = new Session.HardSpendActions(execute, (ulong ts) => pEntity.FullnessRushCostNow(), pEntity.BlueprintName, pEntity.DefinitionId, complete, cancel, logSpend, session.properties.unitIdleWindow.GetWishWidgetRushButtonPosition());
					session.ChangeState("HardSpendConfirm", false);
				}
			};
			Action<int> pDoTaskAction = delegate(int nTaskDID)
			{
				int? nCostumeDID = session.properties.unitIdleWindow.m_nCostumeDID;
				if (nCostumeDID != null && session.TheGame.costumeManager.IsCostumeUnlocked(nCostumeDID.Value))
				{
					ResidentEntity entity = sim.GetEntity<ResidentEntity>();
					if (entity.CostumeDID == null || entity.CostumeDID.Value != nCostumeDID.Value)
					{
						CostumeManager.Costume costume = session.TheGame.costumeManager.GetCostume(nCostumeDID.Value);
						CostumeManager.Costume pOldCostume = null;
						if (entity.CostumeDID != null || entity.DefaultCostumeDID != null)
						{
							pOldCostume = session.TheGame.costumeManager.GetCostume((entity.CostumeDID == null) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value);
						}
						if (costume != null)
						{
							session.game.analytics.LogCostumeChanged(costume.m_nDID);
							AnalyticsWrapper.LogCostumeChanged(session.TheGame, entity, pOldCostume, costume);
							session.TheGame.simulation.ModifyGameStateSimulated(sim, new ChangeCostumeAction(sim.Id, nCostumeDID.Value));
							entity.CostumeDID = nCostumeDID;
							sim.SetCostume(costume);
						}
					}
				}
				TaskManager taskManager = session.TheGame.taskManager;
				Task task = taskManager.CreateActiveTask(session.TheGame, nTaskDID);
				if (task != null)
				{
					session.TheGame.simulation.UpdateControls();
					session.TheGame.analytics.LogTaskStarted(task.m_pTaskData.m_nDID);
					AnalyticsWrapper.LogTaskStarted(session.TheGame, task);
					session.TheGame.simulation.ModifyGameState(new TaskStartAction(task));
					session.TheGame.simulation.soundEffectManager.PlaySound(task.m_pTaskData.m_sStartVO);
					session.TheGame.simulation.soundEffectManager.PlaySound(task.m_pTaskData.m_sStartSound);
					session.ChangeState("Playing", true);
				}
			};
			session.properties.unitIdleWindow = SBUIBuilder.MakeAndPushUnitIdleUI(session.properties.unitIdleHud, session, sim, session.TheGame.taskManager.GetTaskDatasForSource(sim.entity.DefinitionId, false), pFeedWishAction, pRushWishAction, pDoTaskAction, closeButton);
			session.properties.transitionSilently = false;
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.game.sessionActionManager.SetActionHandler("unit_idle_ui", session, new List<SBGUIScreen>
			{
				session.properties.unitIdleHud,
				session.properties.unitIdleWindow
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0007A558 File Offset: 0x00078758
		public void OnLeave(Session session)
		{
			session.game.sessionActionManager.ClearActionHandler("unit_idle_ui", session);
			if (session.properties.unitIdleWindow == null)
			{
				TFUtils.DebugLog("unitIdleWindow is null");
				return;
			}
			int? nCostumeDID = session.properties.unitIdleWindow.m_nCostumeDID;
			if (nCostumeDID != null && session.game.costumeManager.IsCostumeUnlocked(nCostumeDID.Value))
			{
				Simulated selected = session.game.selected;
				ResidentEntity entity = selected.GetEntity<ResidentEntity>();
				if (entity.CostumeDID == null || entity.CostumeDID.Value != nCostumeDID.Value)
				{
					CostumeManager.Costume pOldCostume = null;
					if (entity.CostumeDID != null || entity.DefaultCostumeDID != null)
					{
						pOldCostume = session.TheGame.costumeManager.GetCostume((entity.CostumeDID == null) ? entity.DefaultCostumeDID.Value : entity.CostumeDID.Value);
					}
					CostumeManager.Costume costume = session.TheGame.costumeManager.GetCostume(nCostumeDID.Value);
					if (costume != null)
					{
						session.game.analytics.LogCostumeChanged(costume.m_nDID);
						AnalyticsWrapper.LogCostumeChanged(session.TheGame, entity, pOldCostume, costume);
						session.TheGame.simulation.ModifyGameStateSimulated(selected, new ChangeCostumeAction(selected.Id, nCostumeDID.Value));
						entity.CostumeDID = nCostumeDID;
						selected.SetCostume(costume);
					}
				}
			}
			if (!session.properties.transitionSilently)
			{
				if (session.TheCamera.ScreenBufferOn)
				{
					session.TheCamera.TurnOffScreenBuffer();
				}
				session.properties.unitIdleHud = null;
				session.properties.unitIdleWindow = null;
			}
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0007A74C File Offset: 0x0007894C
		public void OnUpdate(Session session)
		{
			session.game.simulation.OnUpdate(session);
			session.game.communityEventManager.OnUpdate(session);
			session.game.microEventManager.OnUpdate(session);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x04000C0E RID: 3086
		private const string UNIT_IDLE_UI_HANDLER = "unit_idle_ui";
	}

	// Token: 0x0200020B RID: 523
	public class Vending : Session.State
	{
		// Token: 0x0600118F RID: 4495 RVA: 0x0007A7B8 File Offset: 0x000789B8
		public void OnEnter(Session session)
		{
			session.game.dropManager.MarkForClearCurrentDrops();
			Simulated selected = session.game.selected;
			if (selected == null)
			{
				TFUtils.DebugLog("attempted to transition to Vending without a selected entity");
				session.ChangeState("Playing", true);
				return;
			}
			VendingDecorator entity = selected.GetEntity<VendingDecorator>();
			TFUtils.Assert(entity != null, "Did not select a valid building for Vending!");
			TFUtils.Assert(session.game.vendingManager != null, "VendingManager is not setup for this game!");
			VendorDefinition vendorDefinition = session.game.vendingManager.GetVendorDefinition(entity.VendorId);
			if (vendorDefinition == null)
			{
				TFUtils.DebugLog("attempted to transition to Vending without a valid vendor target " + entity.VendorId);
				session.ChangeState("Playing", true);
				return;
			}
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.dropManager.DialogNeededCallback = action;
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			session.TheSoundEffectManager.PlaySound(vendorDefinition.music);
			session.TheSoundEffectManager.PlaySound(vendorDefinition.openSound);
			Action backHandler = delegate()
			{
				AndroidBack.getInstance().pop();
				session.ChangeState("Playing", true);
			};
			Dictionary<int, VendingInstance> vendingInstances = session.game.vendingManager.GetVendingInstances(entity.Id);
			VendingInstance specialInstance = session.game.vendingManager.GetSpecialInstance(entity.Id);
			if (vendingInstances == null || specialInstance == null)
			{
				this.Restock(session, selected, false);
				vendingInstances = session.game.vendingManager.GetVendingInstances(entity.Id);
				specialInstance = session.game.vendingManager.GetSpecialInstance(entity.Id);
			}
			Action<VendingInstance> vendorInstanceHandler = delegate(VendingInstance instance)
			{
				if (instance.remaining > 0)
				{
					this.CheckInstanceForJelly(session, instance);
					session.properties.vendorScreen.UpdateVendingInstanceSlots(session);
				}
				else
				{
					session.TheSoundEffectManager.PlaySound("Error");
				}
			};
			Action rushHandler = delegate()
			{
				this.VendorRestockRush(session);
			};
			SBGUIStandardScreen sbguistandardScreen = SBUIBuilder.MakeAndPushStandardUI(session, false, null, delegate
			{
				session.AddAsyncResponse("store_open_type", "store_open_button");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.CheckInventorySoftLock();
				session.ChangeState("Inventory", true);
			}, delegate
			{
				session.ChangeState("Options", true);
			}, delegate
			{
				session.ChangeState("Editing", true);
			}, null, null, null, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_gold");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.AddAsyncResponse("target_store_tab", "rmt");
				session.AddAsyncResponse("store_open_type", "store_open_plus_buy_jelly");
				session.ChangeState("Shopping", true);
			}, delegate
			{
				session.ChangeState("CommunityEvent", true);
			}, null, false);
			sbguistandardScreen.SetVisibleNonEssentialElements(false);
			session.properties.m_pTaskSimulated = null;
			session.properties.m_bAutoPanToSimulatedOnLeave = false;
			Action<int> pTaskCharacterClicked = delegate(int nDID)
			{
				Simulated simulated = session.TheGame.simulation.FindSimulated(new int?(nDID));
				if (simulated != null)
				{
					session.properties.m_pTaskSimulated = simulated;
					TaskManager taskManager = session.TheGame.taskManager;
					List<Task> activeTasksForSimulated2 = session.TheGame.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, simulated.Id, true);
					if (activeTasksForSimulated2.Count > 0 && activeTasksForSimulated2[0].GetTimeLeft() > 0UL)
					{
						if (taskManager.GetTaskingStateForSimulated(session.TheGame.simulation, nDID, simulated.Id) == TaskManager._eBlueprintTaskingState.eNone)
						{
							session.ChangeState("UnitIdle", true);
						}
						else
						{
							session.ChangeState("UnitBusy", true);
						}
					}
					else
					{
						session.properties.m_bAutoPanToSimulatedOnLeave = true;
						session.ChangeState("Playing", true);
					}
				}
			};
			List<int> activeSourcesForTarget = session.TheGame.taskManager.GetActiveSourcesForTarget(selected.Id);
			int num = activeSourcesForTarget.Count;
			for (int i = 0; i < num; i++)
			{
				List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(activeSourcesForTarget[i], null, true);
				if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() <= 0UL)
				{
					activeSourcesForTarget.RemoveAt(i);
					i--;
					num--;
				}
			}
			session.properties.vendorScreen = SBUIBuilder.MakeAndPushVendorUI(session, null, backHandler, vendorInstanceHandler, rushHandler, vendorDefinition, vendingInstances, specialInstance, entity, activeSourcesForTarget, pTaskCharacterClicked);
			session.properties.transitionSilently = false;
			session.game.sessionActionManager.SetActionHandler("vending_ui", session, new List<SBGUIScreen>
			{
				sbguistandardScreen,
				session.properties.vendorScreen
			}, new SessionActionManager.Handler(SessionActionUiHelper.HandleCommonSessionActions));
			session.TheCamera.SetEnableUserInput(false, false, default(Vector3));
			session.TheCamera.TurnOnScreenBuffer();
			SessionActionSimulationHelper.EnableHandler(session, true);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0007AC0C File Offset: 0x00078E0C
		public void OnLeave(Session session)
		{
			session.game.sessionActionManager.ClearActionHandler("vending_ui", session);
			if (!session.properties.transitionSilently)
			{
				if (session.game.selected != null)
				{
					Simulated selected = session.game.selected;
					VendingDecorator entity = selected.GetEntity<VendingDecorator>();
					if (entity != null)
					{
						session.TheSoundEffectManager.PlaySound(session.game.vendingManager.GetVendorDefinition(entity.VendorId).closeSound);
					}
					if (session.properties.reward != null)
					{
						ulong utcNow = TFUtils.EpochTime();
						RewardManager.GenerateRewardDrops(session.properties.reward, session.game.simulation, selected.DisplayController.Position + selected.ThoughtDisplayOffsetWorld, utcNow, false);
						session.game.simulation.analytics.LogCollectVendedReward(entity.DefinitionId, session.game.resourceManager.PlayerLevelAmount);
					}
					session.game.selected = null;
				}
				else if (session.properties.reward != null)
				{
					session.game.ApplyReward(session.properties.reward, TFUtils.EpochTime(), false);
				}
				if (session.TheCamera.ScreenBufferOn)
				{
					session.TheCamera.TurnOffScreenBuffer();
				}
				session.properties.reward = null;
			}
			if (session.properties.m_pTaskSimulated != null)
			{
				session.game.selected = session.properties.m_pTaskSimulated;
			}
			if (session.game.selected != null && session.properties.m_bAutoPanToSimulatedOnLeave)
			{
				session.TheCamera.AutoPanToPosition(session.game.selected.PositionCenter, 0.75f);
			}
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0007ADD0 File Offset: 0x00078FD0
		public void OnUpdate(Session session)
		{
			TFUtils.Assert(session != null && session.game != null && session.game.selected != null, "Trying to find Selected failed");
			if (session.game.selected.GetEntity<VendingDecorator>() == null)
			{
				TFUtils.Assert(false, "we have switched an entity to a non-Vendor!");
				session.ChangeState("Playing", true);
			}
			this.Restock(session, session.game.selected, true);
			session.game.dropManager.OnUpdate(session, session.game.simulation.TheCamera, true);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0007AE70 File Offset: 0x00079070
		private void CheckInstanceForJelly(Session session, VendingInstance instance)
		{
			if (instance.Cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY) && session.game.resourceManager.CanPay(instance.Cost))
			{
				Action execute = delegate()
				{
					this.Purchase(session, instance);
				};
				VendorStock stock = session.game.vendingManager.GetStock(instance.StockId);
				Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
				{
					session.analytics.LogPremiumVending(stock.Name, session.game.resourceManager.Resources[ResourceManager.LEVEL].Amount, cost, canAfford);
				};
				int jellyCost = 0;
				instance.Cost.ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out jellyCost);
				Action complete = delegate()
				{
					session.ChangeState("vending", true);
					AnalyticsWrapper.LogJellyConfirmation(session.TheGame, stock.Did, jellyCost, stock.Name, "craft", "instant_purchase", string.Empty, "confirm");
				};
				Action cancel = delegate()
				{
					session.ChangeState("vending", true);
				};
				session.properties.transitionSilently = true;
				session.properties.hardSpendActions = new Session.HardSpendActions(execute, (ulong time) => new Cost(new Dictionary<int, int>
				{
					{
						ResourceManager.HARD_CURRENCY,
						jellyCost
					}
				}), string.Empty, stock.Did, complete, cancel, logSpend, session.properties.vendorScreen.GetBuyButtonPosition());
				session.ChangeState("HardSpendConfirm", false);
			}
			else
			{
				this.Purchase(session, instance);
			}
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0007B014 File Offset: 0x00079214
		private void VendorRestockRush(Session session)
		{
			TFUtils.Assert(session != null && session.game != null && session.game.selected != null, "Trying to Rush Vendor restock in an invalid game state");
			VendingDecorator entityToRush = session.game.selected.GetEntity<VendingDecorator>();
			if (entityToRush == null)
			{
				TFUtils.Assert(false, "we have switched an entity to a non-Vendor!");
				session.ChangeState("Playing", true);
			}
			Cost fullCost = session.game.vendingManager.GetVendorDefinition(entityToRush.VendorId).RushCost;
			Cost lastCost = null;
			ulong startTime = entityToRush.RestockTime - entityToRush.RestockPeriod;
			int jellyCost = Cost.Prorate(fullCost, startTime, entityToRush.RestockTime, TFUtils.EpochTime()).ResourceAmounts[ResourceManager.HARD_CURRENCY];
			Cost.CostAtTime cost2 = delegate(ulong ts)
			{
				lastCost = Cost.Prorate(fullCost, startTime, entityToRush.RestockTime, ts);
				return lastCost;
			};
			Action execute = delegate()
			{
				entityToRush.RestockTime = TFUtils.EpochTime();
				session.game.resourceManager.Spend(lastCost, session.game);
				RushRestockAction action = new RushRestockAction(session.game.selected.Id, lastCost);
				session.game.simulation.ModifyGameStateSimulated(session.game.selected, action);
				AnalyticsWrapper.LogJellyConfirmation(session.TheGame, entityToRush.DefinitionId, jellyCost, entityToRush.Name, "shops", "speedup", "store_restocking", "confirm");
			};
			Action<bool, Cost> logSpend = delegate(bool canAfford, Cost cost)
			{
				session.analytics.LogRushRestock(entityToRush.BlueprintName, cost.ResourceAmounts[ResourceManager.HARD_CURRENCY], canAfford);
			};
			Action cancel = delegate()
			{
				session.ChangeState("vending", true);
			};
			session.properties.transitionSilently = true;
			session.properties.hardSpendActions = new Session.HardSpendActions(execute, cost2, "!!RESTOCK_STORE", entityToRush.DefinitionId, delegate()
			{
				session.ChangeState("vending", true);
			}, cancel, logSpend, session.properties.vendorScreen.GetRestockRushPosition());
			session.ChangeState("HardSpendConfirm", false);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0007B1F4 File Offset: 0x000793F4
		private void Purchase(Session session, VendingInstance instance)
		{
			if (session.game.resourceManager.CanPay(instance.Cost))
			{
				session.TheSoundEffectManager.PlaySound("Purchase");
				session.game.resourceManager.Apply(instance.Cost, session.game);
				Reward reward = session.game.vendingManager.GetStock(instance.StockId).GenerateReward(session.game.simulation);
				VendingAction action = new VendingAction(session.game.selected.Id, instance.SlotId, instance.Special, reward, instance.Cost);
				session.game.simulation.ModifyGameStateSimulated(session.game.selected, action);
				if (session.properties.reward == null)
				{
					session.properties.reward = reward;
				}
				else
				{
					session.properties.reward += reward;
				}
				instance.remaining--;
			}
			else
			{
				session.TheSoundEffectManager.PlaySound("Error");
				Dictionary<string, int> resourcesStillRequired = Cost.GetResourcesStillRequired(instance.Cost.ResourceAmounts, session.game.resourceManager);
				if (resourcesStillRequired.Count > 0)
				{
					session.TheSoundEffectManager.PlaySound("Error");
					session.properties.transitionSilently = true;
					Action okAction = delegate()
					{
						this.Purchase(session, instance);
						session.properties.transitionSilently = true;
						session.ChangeState("vending", true);
					};
					Action cancelAction = delegate()
					{
						session.properties.transitionSilently = true;
						session.ChangeState("vending", true);
					};
					session.InsufficientResourcesHandler(session, session.game.vendingManager.GetStock(instance.StockId).Name, session.game.vendingManager.GetStock(instance.StockId).Did, okAction, cancelAction, instance.Cost);
				}
				else
				{
					TFUtils.ErrorLog("Was not able to purchase something but had enough resources in SessionVending.");
				}
			}
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0007B4A8 File Offset: 0x000796A8
		private void Restock(Session session, Simulated simulated, bool refresh)
		{
			VendingDecorator entity = simulated.GetEntity<VendingDecorator>();
			bool flag = false;
			if (TFUtils.EpochTime() >= entity.RestockTime)
			{
				flag = true;
				session.game.vendingManager.GenerateNewGeneralInstances(entity);
				entity.RestockTime = TFUtils.EpochTime() + entity.RestockPeriod;
			}
			if (TFUtils.EpochTime() >= entity.SpecialRestockTime)
			{
				flag = true;
				session.game.vendingManager.GenerateNewSpecialInstances(entity);
				entity.SpecialRestockTime = TFUtils.EpochTime() + entity.SpecialRestockPeriod;
			}
			if (flag)
			{
				RestockVendorAction action = RestockVendorAction.Create(entity.Id, entity.RestockTime, entity.SpecialRestockTime, session.game.vendingManager.GetVendingInstances(entity.Id), session.game.vendingManager.GetSpecialInstances(entity.Id));
				session.game.simulation.ModifyGameStateSimulated(simulated, action);
				if (refresh)
				{
					session.properties.vendorScreen.UpdateVendingInstanceSlots(session);
				}
			}
		}

		// Token: 0x04000C0F RID: 3087
		private const string VENDING_UI_HANDLER = "vending_ui";
	}

	// Token: 0x0200020C RID: 524
	public class VisitGameStarting : Session.State
	{
		// Token: 0x06001197 RID: 4503 RVA: 0x0007B5C0 File Offset: 0x000797C0
		private void OnGameCreated(Session session)
		{
			this.DeferDialogs(session);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0007B5CC File Offset: 0x000797CC
		private void DeferDialogs(Session session)
		{
			Action action = delegate()
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			};
			session.game.questManager.OnShowDialogCallback = action;
			session.game.communityEventManager.DialogNeededCallback = action;
			this.AdvanceState(session);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0007B62C File Offset: 0x0007982C
		public void OnLoadGameDelegate(SoaringContext context)
		{
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0007B630 File Offset: 0x00079830
		private void LoadEntityBlueprints(Session session)
		{
			if (!this.contentLoader.LoadNextBlueprint())
			{
				this.CallLoadFromNetwork(session, false);
				this.AdvanceState(session);
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0007B654 File Offset: 0x00079854
		private void CallLoadFromNetwork(Session session, bool isRetryAttempt = false)
		{
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0007B658 File Offset: 0x00079858
		public void OnEnter(Session session)
		{
			session.InFriendsGame = true;
			EntityManager.MustRegenerateStates = true;
			this.attempLoadPatchTown = true;
			this.blockUpdates = false;
			session.canChangeState = false;
			if (session.properties.playingHud != null)
			{
				session.properties.playingHud.Deactivate();
			}
			SBGUIScreen sbguiscreen = Session.GameStarting.CreateLoadingScreen(session, true, "visit_starting_progress", true);
			this.loadingSpinner = sbguiscreen.FindChild("loading_spinner");
			session.properties.playDelayCounter = 0;
			this.precacheGUIState = 0;
			this.loadTimeDependentsState = 0;
			int num = 0;
			this.processes = new Session.VisitGameStarting.ProcessStartingProgressState[15];
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.AssembleGameState);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.RequestGameState);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.LoadEntityBlueprints);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.CreateGame);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.LoadAssets);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.LoadLocalAssetsTerrain);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.LoadLocalAssetsCreateSimulation);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.PrecacheGUI);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.LoadLocalAssetsLoadTimeDependents);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.LoadLocalAssetsSendPendingCommands);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.CreateTerrainMeshes);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.LoadLocalAssetsActivateQuests);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.ProcessTriggers);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.HandleUnusedAssets);
			this.processes[num++] = new Session.VisitGameStarting.ProcessStartingProgressState(this.SetupSimulation);
			SBGUIActivityIndicator sbguiactivityIndicator = (SBGUIActivityIndicator)sbguiscreen.FindChildSessionActionId("ActivityIndicator", false);
			sbguiactivityIndicator.Center = new Vector3(4f, -2.7f, 3.2f);
			sbguiactivityIndicator.StartActivityIndicator();
			session.soundEffectManager.Enabled = (PlayerPrefs.GetInt(SoundEffectManager.SOUND_ENABLED) == 1);
			this.currentState = -1;
			this.AdvanceState(session);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0007B8C0 File Offset: 0x00079AC0
		public void OnLeave(Session session)
		{
			session.WasInFriendsGame = true;
			if (session.game != null)
			{
				session.game.CanSave = false;
			}
			EntityManager.MustRegenerateStates = true;
			SBGUIScreen sbguiscreen = (SBGUIScreen)session.CheckAsyncRequest("visit_starting_progress");
			if (sbguiscreen != null)
			{
				SBGUIActivityIndicator sbguiactivityIndicator = (SBGUIActivityIndicator)sbguiscreen.FindChildSessionActionId("ActivityIndicator", false);
				TFUtils.Assert(sbguiactivityIndicator != null, "ActivityIndicator expected to be valid.");
				sbguiactivityIndicator.StopActivityIndicator();
			}
			if (session.gameInitialized)
			{
				session.game.simulation.ClearPendingTimebarsInSimulateds();
				session.game.simulation.ClearPendingNamebarsInSimulateds();
				session.TheCamera.StartCamera();
				session.TheSoundEffectManager.StartSoundEffectsManager();
				session.musicManager.PlayTrack("InGame");
				SBUIBuilder.ReleaseTopScreen();
				RestrictInteraction.AddWhitelistExpansion(session.game.simulation, int.MinValue);
			}
			Session.GameStarting.ResetShowSplashScreen(Session.GameStarting.SplashScreenState.None);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0007B9AC File Offset: 0x00079BAC
		public static void HandleSBGUIEvent(SBGUIEvent evt, Session session)
		{
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0007B9B0 File Offset: 0x00079BB0
		public void OnUpdate(Session session)
		{
			this.currentAdvance = 1;
			if (this.currentState == 15)
			{
				this.SaveFriendGameTimeStamp();
				session.canChangeState = true;
				session.ChangeState("Playing", true);
				if ((SoaringDictionary)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_friends_reward_key") == null)
				{
					SoaringDictionary val = new SoaringDictionary();
					Soaring.Player.PrivateData_Safe.setValue(val, "SBMI_friends_reward_key");
				}
				SoaringArray soaringArray = (SoaringArray)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_fdk");
				if (soaringArray != null)
				{
					for (int i = 0; i < soaringArray.count(); i++)
					{
						SoaringValue b = (SoaringValue)soaringArray.objectAtIndex(i);
						uint sequenceId = (uint)b;
						DialogPackage dialogPackage = session.TheGame.dialogPackageManager.GetDialogPackage(1U);
						List<DialogInputData> dialogInputsInSequence = dialogPackage.GetDialogInputsInSequence(sequenceId, null, null);
						session.TheGame.dialogPackageManager.AddDialogInputBatch(session.TheGame, dialogInputsInSequence, sequenceId);
						session.ChangeState("ShowingDialog", true);
					}
					soaringArray.clear();
				}
				SoaringArray soaringArray2 = (SoaringArray)Soaring.Player.PrivateData_Safe.objectWithKey("SBMI_completed_quest_key");
				if (soaringArray2 == null)
				{
					soaringArray2 = new SoaringArray();
				}
				soaringArray2.addObject(2400L);
				Soaring.Player.PrivateData_Safe.setValue(soaringArray2, "SBMI_completed_quest_key");
				Soaring.UpdateUserProfile(Soaring.Player.CustomData, null);
				return;
			}
			if (this.currentState == 16)
			{
				return;
			}
			SBGUIScreen sbguiscreen = (SBGUIScreen)session.CheckAsyncRequest("visit_starting_progress");
			session.AddAsyncResponse("visit_starting_progress", sbguiscreen);
			this.loadingSpinner.gameObject.transform.Rotate(new Vector3(0f, 0f, 1f), -250f * Time.deltaTime);
			float num = ((float)this.currentState + 1f) / 15f;
			sbguiscreen.dynamicMeters["loading"].Progress = num;
			sbguiscreen.dynamicLabels["progress"].SetText(string.Format("{0}%", ((int)(100f * num)).ToString()));
			try
			{
				if (!this.blockUpdates)
				{
					this.processes[this.currentState](session);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message + "\n" + ex.StackTrace);
				this.DisplayFailedToLoadDialog(session);
			}
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0007BC58 File Offset: 0x00079E58
		private void AdvanceState(Session session)
		{
			lock (this)
			{
				this.currentState += this.currentAdvance;
				this.currentAdvance = 0;
			}
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0007BCB0 File Offset: 0x00079EB0
		private void RequestGameState(Session session)
		{
			if (this.attempLoadPatchTown)
			{
				SoaringContext soaringContext = new SoaringContext();
				soaringContext.Name = "RetrieveFriendGame";
				soaringContext.addValue(new SoaringObject(session), "session");
				soaringContext.Responder = new Session.VisitGameStarting.VisitFriendSoaringDelegate();
				soaringContext.ContextResponder = new SoaringContextDelegate(this.GameRetrieved);
				this.FRIEND_SAVE_GAME = null;
				if (this.CheckFriendGameTimestamp())
				{
					this.GameRetrieved(soaringContext);
				}
				else
				{
					SBMISoaring.RetrieveUsersSession(soaringContext);
				}
				this.attempLoadPatchTown = false;
			}
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0007BD34 File Offset: 0x00079F34
		private void SaveFriendGameTimeStamp()
		{
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0007BD38 File Offset: 0x00079F38
		private bool CheckFriendGameTimestamp()
		{
			return false;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0007BD3C File Offset: 0x00079F3C
		private void GameRetrieved(SoaringContext context)
		{
			Session session = null;
			if (context != null)
			{
				SoaringObject soaringObject = (SoaringObject)context.objectWithKey("session");
				if (soaringObject != null)
				{
					session = (Session)soaringObject.Object;
				}
				bool flag = context.soaringValue("success");
				if (flag)
				{
					this.FRIEND_SAVE_GAME = (SoaringDictionary)context.objectWithKey("game_session");
					if (this.FRIEND_SAVE_GAME != null && SBSettings.OfflineModeFriendParks)
					{
						try
						{
							string writePath = ResourceUtils.GetWritePath("game.json", "PatchyTown", 1);
							MBinaryWriter mbinaryWriter = new MBinaryWriter();
							if (mbinaryWriter.Open(writePath, true, true) && mbinaryWriter.IsOpen())
							{
								mbinaryWriter.Write(this.FRIEND_SAVE_GAME.ToJsonString());
								mbinaryWriter.Close();
							}
						}
						catch (Exception ex)
						{
							TFUtils.ErrorLog(ex.Message + "\n" + ex.StackTrace);
						}
					}
				}
			}
			this.AdvanceState(session);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0007BE54 File Offset: 0x0007A054
		private void CreateGame(Session session)
		{
			SoaringDictionary soaringDictionary = this.FRIEND_SAVE_GAME;
			bool flag = false;
			if (soaringDictionary != null)
			{
				Dictionary<string, object> data = SBMISoaring.ConvertDictionaryToGeneric(soaringDictionary);
				try
				{
					int num = 0;
					session.game = Game.LoadFromDataDict(data, session.Analytics, session.ThePlayer, this.contentLoader, out num, session.PlayHavenController);
					session.game.CanSave = false;
					this.OnGameCreated(session);
					flag = true;
				}
				catch (Exception ex)
				{
					Debug.LogError("Data Failed To Load: Using ServerGame: " + ex.Message + "\n" + ex.StackTrace);
					TFUtils.LogDump(session, "friend_save_error", ex, null);
				}
			}
			if (!flag && SBSettings.OfflineModeFriendParks)
			{
				try
				{
					MBinaryReader fileStream = ResourceUtils.GetFileStream("game", "PatchyTown", "json", 5);
					if (fileStream != null && fileStream.IsOpen())
					{
						soaringDictionary = new SoaringDictionary(fileStream.ReadAllBytes());
						Dictionary<string, object> data2 = SBMISoaring.ConvertDictionaryToGeneric(soaringDictionary);
						int num2 = 0;
						session.game = Game.LoadFromDataDict(data2, session.Analytics, session.ThePlayer, this.contentLoader, out num2, session.PlayHavenController);
						session.game.CanSave = false;
						this.OnGameCreated(session);
						flag = true;
					}
				}
				catch (Exception ex2)
				{
					Debug.LogError("Data Failed To Load: Using Local Game: " + ex2.Message + "\n" + ex2.StackTrace);
					TFUtils.LogDump(session, "friend_save_error", ex2, null);
				}
			}
			if (!flag)
			{
				this.DisplayFailedToLoadDialog(session);
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0007BFFC File Offset: 0x0007A1FC
		public void DisplayFailedToLoadDialog(Session session)
		{
			if (this.blockUpdates)
			{
				return;
			}
			Action okHandler = delegate()
			{
				session.canChangeState = true;
				session.ChangeState("GameStarting", true);
			};
			SBUIBuilder.CreateErrorDialog(session, "Error", "Opps, We were unable to load Patchy Town\nAt this time.\nCome Back Later.", Language.Get("!!PREFAB_OK"), okHandler, 0.85f, 0.45f);
			this.blockUpdates = true;
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0007C060 File Offset: 0x0007A260
		private void LoadAssets(Session session)
		{
			TFUtils.Assert(session.game != null, "VisitGameStarting.LoadAssets() expects session.game to not be null");
			this.contentLoader.TheEntityManager.LoadBlueprintResources();
			this.AdvanceState(session);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0007C090 File Offset: 0x0007A290
		private void CreateTerrainMeshes(Session session)
		{
			session.game.terrain.CreateTerrainMeshes();
			this.AdvanceState(session);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0007C0AC File Offset: 0x0007A2AC
		private void AwaitProductInfo(Session session)
		{
			if (session.TheGame.store.receivedProductInfo)
			{
				this.AdvanceState(session);
			}
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0007C0CC File Offset: 0x0007A2CC
		private void ProcessTriggers(Session session)
		{
			this.AdvanceState(session);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0007C0D8 File Offset: 0x0007A2D8
		private void HandleUnusedAssets(Session session)
		{
			if (this.mUnloadAssetMonitor == null)
			{
				this.mUnloadAssetMonitor = AssetServices.CreateUnloadUnusedAssetService(null);
			}
			else if (this.mUnloadAssetMonitor.IsCompleted)
			{
				this.AdvanceState(session);
				this.mUnloadAssetMonitor = null;
			}
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0007C124 File Offset: 0x0007A324
		private void SetupSimulation(Session session)
		{
			if (!session.gameInitialized)
			{
				session.GameInitialized(true);
				session.game.simulation.OnUpdateVisitParkState(session);
				session.game.treasureManager.StartTreasureTimers();
				session.game.playtimeRegistrar.UpdatePlaytime(TFUtils.EpochTime());
			}
			else if (!session.game.needsNetworkDownErrorDialog)
			{
				this.AdvanceState(session);
			}
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0007C198 File Offset: 0x0007A398
		private void AssembleGameState(Session session)
		{
			SBSettings.Init();
			SBUIBuilder.ClearScreenCache();
			if (Language.CurrentLanguage() == LanguageCode.N)
			{
				Language.Init(TFUtils.GetPersistentAssetsPath());
			}
			this.contentLoader = new StaticContentLoader();
			this.contentLoader.LoadContent(session);
			session.DropGame();
			this.AdvanceState(session);
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x0007C1E8 File Offset: 0x0007A3E8
		private void LoadLocalAssetsTerrain(Session session)
		{
			this.contentLoader.Initialize();
			this.AdvanceState(session);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0007C1FC File Offset: 0x0007A3FC
		private void LoadLocalAssetsCreateSimulation(Session session)
		{
			TFUtils.DebugLog("Creating simulation");
			session.game.simulation = new Simulation(new Simulation.ModifyGameStateFunction(session.game.NULL_ModifyGameState), new Simulation.ModifyGameStateSimulatedFunction(session.game.NULL_ModifyStateSimulated), null, new Simulation.RecordBufferAction(session.game.actionBuffer.Record), session.game, session.game.entities, session.game.triggerRouter, session.game.resourceManager, session.game.dropManager, session.TheSoundEffectManager, session.game.resourceCalculatorManager, session.game.craftManager, session.game.movieManager, session.game.featureManager, session.game.catalog, session.game.rewardCap, session.camera.UnityCamera, session.game.terrain, 5, session.analytics, session.simulationScratchScreen, this.contentLoader.TheEnclosureManager);
			this.AdvanceState(session);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0007C30C File Offset: 0x0007A50C
		private void PrecacheGUI(Session session)
		{
			TFUtils.DebugLogTimed("In PrecacheGUI");
			int num = this.precacheGUIState;
			if (num != 0)
			{
				this.AdvanceState(session);
			}
			else
			{
				SBGUIRewardWidget.MakeRewardWidgetPool();
				this.precacheGUIState++;
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0007C35C File Offset: 0x0007A55C
		private void LoadLocalAssetsLoadTimeDependents(Session session)
		{
			ulong utcNow = TFUtils.EpochTime();
			switch (this.loadTimeDependentsState)
			{
			case 0:
				session.game.LoadSimulation(utcNow);
				this.loadTimeDependentsState++;
				break;
			case 1:
				if (session.game.IterateLoadSimulation())
				{
					this.loadTimeDependentsState++;
				}
				break;
			case 2:
				session.game.LoadExpansions(utcNow);
				this.loadTimeDependentsState++;
				break;
			case 3:
				if (session.game.IterateLoadExpansions())
				{
					this.loadTimeDependentsState++;
				}
				break;
			default:
				this.AdvanceState(session);
				break;
			}
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0007C424 File Offset: 0x0007A624
		private void LoadLocalAssetsSendPendingCommands(Session session)
		{
			session.game.simulation.SendPendingCommands();
			this.AdvanceState(session);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0007C440 File Offset: 0x0007A640
		private void LoadLocalAssetsActivateQuests(Session session)
		{
			TFUtils.DebugLog("GAME INITIALIZED");
			if (session.game.dialogPackageManager.GetNumQueuedDialogInputs() > 0)
			{
				session.AddAsyncResponse("dialogs_to_show", true);
			}
			this.AdvanceState(session);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x0007C488 File Offset: 0x0007A688
		private void CreateErrorDialog(Session session, string title, string message, string okButtonLabel, Action okHandler, float messageScale, float titleScale)
		{
			Action androidOk = delegate()
			{
				okHandler();
			};
			Action okButtonHandler = delegate()
			{
				okHandler();
				AndroidBack.getInstance().pop(androidOk);
			};
			SBGUIConfirmationDialog sbguiconfirmationDialog = SBUIBuilder.MakeAndPushConfirmationDialog(session, null, title, message, okButtonLabel, null, null, okButtonHandler, null, false);
			AndroidBack.getInstance().push(androidOk, sbguiconfirmationDialog);
			SBGUILabel sbguilabel = (SBGUILabel)sbguiconfirmationDialog.FindChild("message_label");
			YGTextAtlasSprite component = sbguilabel.GetComponent<YGTextAtlasSprite>();
			component.scale = new Vector2(messageScale, messageScale);
			SBGUILabel sbguilabel2 = (SBGUILabel)sbguiconfirmationDialog.FindChild("title_label");
			YGTextAtlasSprite component2 = sbguilabel2.GetComponent<YGTextAtlasSprite>();
			component2.scale = new Vector2(titleScale, titleScale);
			sbguiconfirmationDialog.tform.parent = GUIMainView.GetInstance().transform;
			sbguiconfirmationDialog.tform.localPosition = Vector3.zero;
		}

		// Token: 0x04000C10 RID: 3088
		public const uint VISIT_FRIEND_QUEST_ID = 2400U;

		// Token: 0x04000C11 RID: 3089
		public const uint VISIT_FRIEND_DIALOG_ID = 2401U;

		// Token: 0x04000C12 RID: 3090
		public const string VISIT_STARTING_PROGRESS = "visit_starting_progress";

		// Token: 0x04000C13 RID: 3091
		private const string POLICY_BUTTON = "policy_button";

		// Token: 0x04000C14 RID: 3092
		private SoaringDictionary FRIEND_SAVE_GAME;

		// Token: 0x04000C15 RID: 3093
		private int currentState = -1;

		// Token: 0x04000C16 RID: 3094
		private Session.VisitGameStarting.ProcessStartingProgressState[] processes;

		// Token: 0x04000C17 RID: 3095
		private int currentAdvance = 1;

		// Token: 0x04000C18 RID: 3096
		private int precacheGUIState;

		// Token: 0x04000C19 RID: 3097
		private int loadTimeDependentsState;

		// Token: 0x04000C1A RID: 3098
		private StaticContentLoader contentLoader;

		// Token: 0x04000C1B RID: 3099
		public bool blockUpdates;

		// Token: 0x04000C1C RID: 3100
		private SBGUIElement loadingSpinner;

		// Token: 0x04000C1D RID: 3101
		public bool attempLoadPatchTown = true;

		// Token: 0x04000C1E RID: 3102
		private AssetServices.AssetServicesMonitor mUnloadAssetMonitor;

		// Token: 0x0200020D RID: 525
		private enum VisitStartingState
		{
			// Token: 0x04000C20 RID: 3104
			STATE_FIRST = -1,
			// Token: 0x04000C21 RID: 3105
			STATE_ASSEMBLE_GAME_STATE,
			// Token: 0x04000C22 RID: 3106
			STATE_RETRIEVE_GAME_SAVE,
			// Token: 0x04000C23 RID: 3107
			STATE_LOAD_ENTITY_BLUEPRINTS,
			// Token: 0x04000C24 RID: 3108
			STATE_CREATE_GAME,
			// Token: 0x04000C25 RID: 3109
			STATE_LOAD_ASSETS,
			// Token: 0x04000C26 RID: 3110
			STATE_LOAD_ASSETS_TERRAIN,
			// Token: 0x04000C27 RID: 3111
			STATE_LOAD_ASSETS_SIMULATION,
			// Token: 0x04000C28 RID: 3112
			STATE_PRECACHE_GUI,
			// Token: 0x04000C29 RID: 3113
			STATE_LOAD_ASSETS_TIME_DEPENDENTS,
			// Token: 0x04000C2A RID: 3114
			STATE_LOAD_ASSETS_SEND_COMMANDS,
			// Token: 0x04000C2B RID: 3115
			STATE_CREATE_TERRAIN_MESHES,
			// Token: 0x04000C2C RID: 3116
			STATE_LOAD_ASSETS_ACTIVATE_QUESTS,
			// Token: 0x04000C2D RID: 3117
			STATE_PROCESS_PENDING,
			// Token: 0x04000C2E RID: 3118
			STATE_UNLOAD_UNUSED_ASSETS,
			// Token: 0x04000C2F RID: 3119
			STATE_SETUP_SIMULATION,
			// Token: 0x04000C30 RID: 3120
			STATE_LAST,
			// Token: 0x04000C31 RID: 3121
			STATE_ERROR
		}

		// Token: 0x0200020E RID: 526
		private class VisitFriendSoaringDelegate : SoaringDelegate
		{
			// Token: 0x060011B6 RID: 4534 RVA: 0x0007C56C File Offset: 0x0007A76C
			public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
			{
				if (string.IsNullOrEmpty(module) || context == null)
				{
					return;
				}
				if (module == "retrieveSessionFromUser")
				{
					context.setValue(success, "success");
					if (error != null || !success)
					{
						context.ContextResponder(context);
					}
					else
					{
						SoaringArray soaringArray = new SoaringArray(1);
						SoaringDictionary soaringDictionary = new SoaringDictionary(1);
						soaringDictionary.addValue(data.soaringValue("gameSessionId"), "gameSessionId");
						soaringArray.addObject(soaringDictionary);
						Soaring.RequestSessionData(soaringArray, null, context);
					}
				}
			}

			// Token: 0x060011B7 RID: 4535 RVA: 0x0007C604 File Offset: 0x0007A804
			public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
			{
				if (!success || error != null || context == null || sessions == null)
				{
					success = false;
				}
				else if (sessions.count() == 0)
				{
					success = false;
				}
				if (success)
				{
					context.setValue(sessions.objectAtIndex(0), "game_session");
				}
				if (context != null)
				{
					context.setValue(success, "success");
					context.ContextResponder(context);
				}
			}
		}

		// Token: 0x020004A8 RID: 1192
		// (Invoke) Token: 0x06002503 RID: 9475
		private delegate void ProcessStartingProgressState(Session session);
	}

	// Token: 0x0200020F RID: 527
	public class TimebarMixin
	{
		// Token: 0x060011B9 RID: 4537 RVA: 0x0007C688 File Offset: 0x0007A888
		public bool ActivateOnSelected(Session session, Simulated simulated, float yOffset = 20f)
		{
			bool result = false;
			Simulated selected = session.game.selected;
			TFUtils.Assert(simulated != null, "Cannot enable Timebar unless there is a simulated");
			if (simulated.timebarMixinArgs.hasTimebar && simulated.timebarMixinArgs.duration > 0f)
			{
				SBGUITimebar.HostPosition hPosition = delegate()
				{
					if (session.game == null)
					{
						return Vector3.zero;
					}
					if (session.game.simulation == null)
					{
						return Vector3.zero;
					}
					return session.game.simulation.ScreenPositionFromWorldPosition(selected.Position);
				};
				Action goBackToPlaying = delegate()
				{
					session.ChangeState("Playing", true);
					session.game.selected = null;
				};
				Action goBackToPlayingCancel = delegate()
				{
					session.ChangeState("Playing", true);
					if (session.game.selected.rushParameters != null && session.game.selected.rushParameters.cancel != null)
					{
						session.game.selected.rushParameters.cancel(session);
					}
					session.game.selected = null;
				};
				List<int> list = null;
				Action<int> pTaskCharacterClicked = null;
				if (simulated.timebarMixinArgs.m_bCheckForTaskCharacters)
				{
					list = session.TheGame.taskManager.GetActiveSourcesForTarget(simulated.Id);
					int num = list.Count;
					for (int i = 0; i < num; i++)
					{
						List<Task> activeTasksForSimulated = session.TheGame.taskManager.GetActiveTasksForSimulated(list[i], null, true);
						if (activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].GetTimeLeft() <= 0UL)
						{
							list.RemoveAt(i);
							i--;
							num--;
						}
					}
					if (list != null && list.Count > 0)
					{
						pTaskCharacterClicked = delegate(int nDID)
						{
							session.CheckAsyncRequest(Session.SelectedPlaying.TASK_CHARACTER_SELECT);
							session.AddAsyncResponse(Session.SelectedPlaying.TASK_CHARACTER_SELECT, nDID, false);
						};
					}
				}
				SBGUITimebar timebar = null;
				timebar = SBUIBuilder.MakeAndAddTimebar(session, session.SimulationSBGUIScreen, (uint)simulated.entity.DefinitionId, simulated.timebarMixinArgs.description, simulated.timebarMixinArgs.completeTime, simulated.timebarMixinArgs.totalTime, simulated.timebarMixinArgs.duration, simulated.timebarMixinArgs.rushCost, delegate
				{
					if (!session.game.simulation.Whitelisted || session.game.simulation.CheckWhitelist(simulated) || timebar == null || SBGUI.GetInstance().CheckWhitelisted(timebar.RushButton))
					{
						this.DoRush(session, simulated, goBackToPlaying, goBackToPlayingCancel);
						timebar.RemoveCompleteAction();
						timebar.Close();
					}
				}, hPosition, delegate
				{
					if (selected == session.game.selected && (!session.game.simulation.Whitelisted || session.game.simulation.CheckWhitelist(simulated) || timebar == null || SBGUI.GetInstance().CheckWhitelisted(timebar.RushButton)))
					{
						goBackToPlaying();
					}
				}, list, pTaskCharacterClicked);
				this.gameObjectID = timebar.gameObject.name;
				this.timebarGUI = timebar;
				result = true;
			}
			return result;
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0007C908 File Offset: 0x0007AB08
		public void DoRush(Session session, Simulated simulated, Action goBackToPlaying, Action goBackToPlayingCancel)
		{
			session.properties.hardSpendActions = new Session.HardSpendActions(delegate()
			{
				simulated.rushParameters.execute(session);
			}, (ulong ts) => Cost.Prorate(simulated.timebarMixinArgs.rushCost, simulated.timebarMixinArgs.completeTime - simulated.timebarMixinArgs.totalTime, simulated.timebarMixinArgs.completeTime, ts), simulated.rushParameters.subject, simulated.rushParameters.did, goBackToPlaying, goBackToPlayingCancel, delegate(bool canAfford, Cost cost)
			{
				simulated.rushParameters.log(session, cost, canAfford);
			}, this.GetRushButtonScreenPosition());
			session.game.selected = simulated;
			session.ChangeState("HardSpendConfirm", false);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0007C9B4 File Offset: 0x0007ABB4
		public void Deactivate(Session session)
		{
			if (this.gameObjectID != null && this.timebarGUI != null)
			{
				this.timebarGUI.Close();
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x0007C9E0 File Offset: 0x0007ABE0
		public bool IsActive
		{
			get
			{
				return this.timebarGUI != null && this.timebarGUI.IsActive();
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x0007CA04 File Offset: 0x0007AC04
		public void Extend()
		{
			if (this.timebarGUI != null)
			{
				this.timebarGUI.elapsed = 0f;
			}
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x0007CA28 File Offset: 0x0007AC28
		private Vector2 GetRushButtonScreenPosition()
		{
			return this.timebarGUI.GetRushButtonScreenPosition();
		}

		// Token: 0x04000C32 RID: 3122
		public const int YOFFSET = 20;

		// Token: 0x04000C33 RID: 3123
		public const int HEIGHT = 100;

		// Token: 0x04000C34 RID: 3124
		private const string TIMEBAR = "Timebar";

		// Token: 0x04000C35 RID: 3125
		private SBGUITimebar timebarGUI;

		// Token: 0x04000C36 RID: 3126
		private string gameObjectID;
	}

	// Token: 0x02000210 RID: 528
	public class TimebarGroup
	{
		// Token: 0x060011C0 RID: 4544 RVA: 0x0007CA58 File Offset: 0x0007AC58
		public void ActivateOnSelected(Session session)
		{
			if (session.game.selected.Variable.ContainsKey("TaskSrcUnit"))
			{
				this.taskAtBuildingTimebar.ActivateOnSelected(session, (Simulated)session.game.selected.Variable["TaskSrcUnit"], 120f);
			}
			this.timebar.ActivateOnSelected(session, session.game.selected, 20f);
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x0007CAD4 File Offset: 0x0007ACD4
		public void Deactivate(Session session)
		{
			this.timebar.Deactivate(session);
			if (session.game.selected != null && session.game.selected.Variable.ContainsKey("TaskSrcUnit"))
			{
				this.taskAtBuildingTimebar.Deactivate(session);
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060011C2 RID: 4546 RVA: 0x0007CB28 File Offset: 0x0007AD28
		public bool IsActive
		{
			get
			{
				return this.timebar != null && this.timebar.IsActive;
			}
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0007CB44 File Offset: 0x0007AD44
		public void Extend()
		{
			this.timebar.Extend();
		}

		// Token: 0x04000C37 RID: 3127
		public const string TASK_SRC_UNIT = "TaskSrcUnit";

		// Token: 0x04000C38 RID: 3128
		private Session.TimebarMixin taskAtBuildingTimebar = new Session.TimebarMixin();

		// Token: 0x04000C39 RID: 3129
		private Session.TimebarMixin timebar = new Session.TimebarMixin();
	}

	// Token: 0x02000211 RID: 529
	public class AcceptPlacementControl : BaseControlBinding
	{
		// Token: 0x060011C4 RID: 4548 RVA: 0x0007CB54 File Offset: 0x0007AD54
		public AcceptPlacementControl() : this(null)
		{
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0007CB60 File Offset: 0x0007AD60
		public AcceptPlacementControl(Action callback)
		{
			base.Initialize(delegate(Session session)
			{
				this.OnClick(session);
			}, callback, "Accept");
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0007CB80 File Offset: 0x0007AD80
		public override void DynamicUpdate(Session session)
		{
			SBUIBuilder.UpdateAcceptPlacementButton(base.DynamicButton, session);
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0007CB90 File Offset: 0x0007AD90
		private void OnClick(Session session)
		{
			if (!Session.TheDebugManager.debugPlaceObjects && session.TheGame.simulation.PlacementQuery(session.TheGame.selected, false) == Simulation.Placement.RESULT.INVALID)
			{
				session.TheSoundEffectManager.PlaySound("Cancel");
				return;
			}
			Action<Session> action = (Action<Session>)session.CheckAsyncRequest("InteractionStrip_AcceptCallback");
			if (action != null)
			{
				session.AddAsyncResponse("InteractionStrip_AcceptCallback", action);
				action(session);
			}
			if (base.Callback != null)
			{
				base.Callback();
			}
		}
	}

	// Token: 0x02000212 RID: 530
	public class BrowseRecipesControl : BaseControlBinding
	{
		// Token: 0x060011C9 RID: 4553 RVA: 0x0007CC2C File Offset: 0x0007AE2C
		public BrowseRecipesControl(Simulated toBrowse)
		{
			Session.BrowseRecipesControl <>f__this = this;
			base.Initialize(delegate(Session session)
			{
				<>f__this.OnClick(session, toBrowse);
			}, null, "Browse");
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x0007CC6C File Offset: 0x0007AE6C
		private void OnClick(Session session, Simulated toBrowse)
		{
			TFUtils.Assert(toBrowse == session.game.selected, "Trying to open a simulated other than the selected one. This is probably wrong");
			session.ChangeState("BrowsingRecipes", true);
		}
	}

	// Token: 0x02000213 RID: 531
	public class ClearDebrisControl : BaseControlBinding
	{
		// Token: 0x060011CB RID: 4555 RVA: 0x0007CCA0 File Offset: 0x0007AEA0
		public ClearDebrisControl(Simulated toClear)
		{
			Session.ClearDebrisControl <>f__this = this;
			base.Initialize(delegate(Session session)
			{
				<>f__this.OnClick(session, toClear);
			}, null, "Clear");
			ClearableDecorator entity = toClear.GetEntity<ClearableDecorator>();
			TFUtils.Assert(entity != null, "Null clearable pointer.  Shouldn't happen if you just touched a clearable entity.");
			this.Label = entity.ClearCost.ResourceAmounts[ResourceManager.SOFT_CURRENCY].ToString();
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0007CD20 File Offset: 0x0007AF20
		private void OnClick(Session session, Simulated toClear)
		{
			if (!session.game.featureManager.CheckFeature("debris_clearing"))
			{
				return;
			}
			TFUtils.Assert(toClear == session.game.selected, "Trying to clear a simulated other than the selected one. This is a bad idea");
			session.ChangeState("Clearing", false);
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0007CD6C File Offset: 0x0007AF6C
		public override void DynamicUpdate(Session session)
		{
			bool enabled = session.game.featureManager.CheckFeature("debris_clearing");
			SBUIBuilder.UpdateButton(base.DynamicButton, enabled);
		}
	}

	// Token: 0x02000214 RID: 532
	public static class PushForPlacementHelper
	{
		// Token: 0x060011CE RID: 4558 RVA: 0x0007CD9C File Offset: 0x0007AF9C
		public static void PushPlacementConfirmation(Session session, Simulated subject)
		{
			session.ChangeState("MoveBuildingInEdit", true);
		}
	}

	// Token: 0x02000215 RID: 533
	public class RejectControl : BaseControlBinding
	{
		// Token: 0x060011CF RID: 4559 RVA: 0x0007CDAC File Offset: 0x0007AFAC
		public RejectControl() : this(null)
		{
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x0007CDB8 File Offset: 0x0007AFB8
		public RejectControl(Action callback)
		{
			base.Initialize(delegate(Session session)
			{
				this.OnClick(session);
			}, callback, "Reject");
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x0007CDD8 File Offset: 0x0007AFD8
		private void OnClick(Session session)
		{
			Action<Session> action = (Action<Session>)session.CheckAsyncRequest("InteractionStrip_RejectCallback");
			if (action != null)
			{
				session.AddAsyncResponse("InteractionStrip_RejectCallback", action);
				action(session);
			}
			if (base.Callback != null)
			{
				base.Callback();
			}
		}
	}

	// Token: 0x02000216 RID: 534
	public class RotateControl : BaseControlBinding
	{
		// Token: 0x060011D3 RID: 4563 RVA: 0x0007CE34 File Offset: 0x0007B034
		public RotateControl(Simulated toRotate, bool isEnabled, Simulation simulation = null)
		{
			Session.RotateControl <>f__this = this;
			base.Initialize(delegate(Session session)
			{
				<>f__this.OnClick(session, toRotate, simulation);
			}, null, "Rotate");
			this.isEnabled = isEnabled;
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x0007CE84 File Offset: 0x0007B084
		private void OnClick(Session session, Simulated toRotate, Simulation simulation)
		{
			if (this.isEnabled)
			{
				session.TheSoundEffectManager.PlaySound("Rotate");
				if (simulation != null)
				{
					toRotate.RemoveFence(simulation);
					toRotate.RemoveScaffolding(simulation);
					toRotate.Flip = !toRotate.Flip;
					toRotate.Animate(simulation);
					toRotate.DisplayController.SetMaskPercentage(0f);
					toRotate.AddFence(simulation);
					toRotate.AddScaffolding(simulation);
				}
				else
				{
					toRotate.Flip = !toRotate.Flip;
					toRotate.FlipWarp(session.TheGame.simulation);
				}
			}
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x0007CF1C File Offset: 0x0007B11C
		public override void DynamicUpdate(Session session)
		{
			YGAtlasSprite component = base.DynamicButton.GetComponent<YGAtlasSprite>();
			if (this.isEnabled)
			{
				component.SetAlpha(1f);
			}
			else
			{
				component.SetAlpha(0.25f);
			}
		}

		// Token: 0x04000C3A RID: 3130
		private bool isEnabled;
	}

	// Token: 0x02000217 RID: 535
	public class RushControl : BaseControlBinding
	{
		// Token: 0x060011D6 RID: 4566 RVA: 0x0007CF5C File Offset: 0x0007B15C
		public RushControl(Simulated toRush)
		{
			Session.RushControl <>f__this = this;
			base.Initialize(delegate(Session session)
			{
				<>f__this.OnClick(session, toRush);
			}, null, "Rush");
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x0007CF9C File Offset: 0x0007B19C
		private void OnClick(Session session, Simulated toSell)
		{
			session.ChangeState("SelectedPlaying", true);
		}
	}

	// Token: 0x02000218 RID: 536
	public class SellControl : BaseControlBinding
	{
		// Token: 0x060011D8 RID: 4568 RVA: 0x0007CFAC File Offset: 0x0007B1AC
		public SellControl(Simulated toSell, bool isEnabled, string sellError)
		{
			Session.SellControl <>f__this = this;
			Simulated pToSell = toSell;
			string sSellError = sellError;
			base.Initialize(delegate(Session session)
			{
				<>f__this.OnClick(session, pToSell, sSellError);
			}, null, "Sell");
			this.isEnabled = isEnabled;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x0007CFFC File Offset: 0x0007B1FC
		private void OnClick(Session session, Simulated toSell, string sellError)
		{
			if (!this.isEnabled && string.IsNullOrEmpty(sellError))
			{
				return;
			}
			if (!string.IsNullOrEmpty(sellError))
			{
				session.AddAsyncResponse("sell_error", sellError);
			}
			session.AddAsyncResponse("to_sell", toSell);
			TFUtils.Assert(toSell == session.game.selected, "Trying to sell a simulated other than the selected one. This is probably wrong");
			session.properties.waitToDecidePlacement = true;
			session.AddAsyncResponse("in_state_move_in_edit", session.TheState.GetType().Equals(typeof(Session.MoveBuildingInEdit)));
			session.ChangeState("SellBuildingConfirmation", true);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0007D0A0 File Offset: 0x0007B2A0
		public override void DynamicUpdate(Session session)
		{
			YGAtlasSprite component = base.DynamicButton.GetComponent<YGAtlasSprite>();
			if (this.isEnabled)
			{
				component.SetAlpha(1f);
			}
			else
			{
				component.SetAlpha(0.25f);
			}
		}

		// Token: 0x04000C3B RID: 3131
		private bool isEnabled;
	}

	// Token: 0x02000219 RID: 537
	public class StashControl : BaseControlBinding
	{
		// Token: 0x060011DB RID: 4571 RVA: 0x0007D0E0 File Offset: 0x0007B2E0
		public StashControl(Simulated toStash, bool isEnabled, string stashError)
		{
			Session.StashControl <>f__this = this;
			string sStashError = stashError;
			base.Initialize(delegate(Session session)
			{
				<>f__this.OnClick(session, toStash, sStashError);
			}, null, "Stash");
			this.isEnabled = isEnabled;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x0007D130 File Offset: 0x0007B330
		private void OnClick(Session session, Simulated toStash, string stashError)
		{
			if (!this.isEnabled && string.IsNullOrEmpty(stashError))
			{
				return;
			}
			if (!session.game.featureManager.CheckFeature("stash_soft"))
			{
				session.game.featureManager.UnlockFeature("stash_soft");
				session.game.featureManager.ActivateFeatureActions(session.game, "stash_soft");
				session.game.simulation.ModifyGameState(new FeatureUnlocksAction(new List<string>
				{
					"stash_soft"
				}));
				return;
			}
			session.TheSoundEffectManager.PlaySound("Stash");
			if (!string.IsNullOrEmpty(stashError))
			{
				session.AddAsyncResponse("stash_error", stashError);
				session.AddAsyncResponse("to_stash", toStash);
				session.AddAsyncResponse("in_state_move_in_edit", session.TheState.GetType().Equals(typeof(Session.MoveBuildingInEdit)));
				session.properties.waitToDecidePlacement = true;
				session.ChangeState("StashBuildingConfirmation", true);
				return;
			}
			BuildingEntity entity = toStash.GetEntity<BuildingEntity>();
			SwarmManager.Instance.RestoreResidents(session.game.simulation, toStash);
			List<Simulated> list = Simulated.Building.FindResidents(session.game.simulation, toStash);
			List<KeyValuePair<int, Identity>> associatedEntities = list.ConvertAll<KeyValuePair<int, Identity>>((Simulated simulated) => new KeyValuePair<int, Identity>(simulated.entity.DefinitionId, simulated.Id));
			session.game.inventory.AddItem(entity, associatedEntities);
			foreach (Simulated simulated3 in list)
			{
				ResidentEntity entity2 = simulated3.GetEntity<ResidentEntity>();
				ulong hungryAt = entity2.HungryAt;
				ulong num = TFUtils.EpochTime();
				ulong hungryAt2 = hungryAt - num;
				entity2.HungryAt = hungryAt2;
			}
			session.game.simulation.ModifyGameStateSimulated(toStash, new MoveAction(toStash.Id, null, null, null, list));
			foreach (Simulated simulated2 in list)
			{
				SwarmManager.Instance.RemoveResident(simulated2.GetEntity<ResidentEntity>(), toStash);
				session.game.simulation.RemoveSimulated(simulated2);
			}
			session.game.simulation.TryWorkerSpawnerCleanup(toStash.Id);
			session.game.selected.FootprintVisible = false;
			toStash.SetFootprint(session.game.simulation, false);
			session.game.simulation.RemoveSimulated(toStash);
			session.game.selected = null;
			bool? flag = (bool?)session.CheckAsyncRequest("FromInventory");
			if (flag != null)
			{
				session.AddAsyncResponse("FromInventory", flag.Value);
			}
			if ((flag != null && flag.Value) || session.properties.cameFromMarketplace)
			{
				session.ChangeState("Playing", true);
			}
			else
			{
				session.ChangeState("Editing", true);
			}
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x0007D4A4 File Offset: 0x0007B6A4
		public override void DynamicUpdate(Session session)
		{
			YGAtlasSprite component = base.DynamicButton.GetComponent<YGAtlasSprite>();
			if (this.isEnabled)
			{
				component.SetAlpha(1f);
			}
			else
			{
				component.SetAlpha(0.25f);
			}
		}

		// Token: 0x04000C3C RID: 3132
		private bool isEnabled;
	}

	// Token: 0x0200021A RID: 538
	public class SelectedStateTransition : BaseTransitionBinding
	{
		// Token: 0x060011DF RID: 4575 RVA: 0x0007D4FC File Offset: 0x0007B6FC
		public SelectedStateTransition(Simulated targetSim, string state)
		{
			Session.SelectedStateTransition <>f__this = this;
			this.targetState = state;
			base.Initialize(delegate(Session session)
			{
				<>f__this.OnClick(session, targetSim);
			});
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0007D53C File Offset: 0x0007B73C
		private void OnClick(Session session, Simulated targetSim)
		{
			TFUtils.Assert(targetSim == session.game.selected, "Trying to open a simulated other than the selected one. This is probably wrong");
			session.ChangeState(this.targetState, true);
		}

		// Token: 0x04000C3E RID: 3134
		private string targetState;
	}

	// Token: 0x0200021B RID: 539
	public class BrowseRecipesTransition : Session.SelectedStateTransition
	{
		// Token: 0x060011E1 RID: 4577 RVA: 0x0007D564 File Offset: 0x0007B764
		public BrowseRecipesTransition(Simulated targetSim) : base(targetSim, "BrowsingRecipes")
		{
		}
	}

	// Token: 0x0200021C RID: 540
	public class VendingTransition : Session.SelectedStateTransition
	{
		// Token: 0x060011E2 RID: 4578 RVA: 0x0007D574 File Offset: 0x0007B774
		public VendingTransition(Simulated targetSim) : base(targetSim, "vending")
		{
		}
	}

	// Token: 0x0200021D RID: 541
	public class UnitIdleTransition : Session.SelectedStateTransition
	{
		// Token: 0x060011E3 RID: 4579 RVA: 0x0007D584 File Offset: 0x0007B784
		public UnitIdleTransition(Simulated targetSim) : base(targetSim, "UnitIdle")
		{
		}
	}

	// Token: 0x0200021E RID: 542
	public class UnitBusyTransition : Session.SelectedStateTransition
	{
		// Token: 0x060011E4 RID: 4580 RVA: 0x0007D594 File Offset: 0x0007B794
		public UnitBusyTransition(Simulated targetSim) : base(targetSim, "UnitBusy")
		{
		}
	}

	// Token: 0x0200021F RID: 543
	public class ShowTreasureRewardTransition : BaseTransitionBinding
	{
		// Token: 0x060011E5 RID: 4581 RVA: 0x0007D5A4 File Offset: 0x0007B7A4
		public ShowTreasureRewardTransition(Simulated toShow)
		{
			Session.ShowTreasureRewardTransition <>f__this = this;
			base.Initialize(delegate(Session session)
			{
				<>f__this.OnClick(session, toShow);
			});
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x0007D5E0 File Offset: 0x0007B7E0
		private void OnClick(Session session, Simulated toShow)
		{
			session.game.simulation.Router.Send(ClickedCommand.Create(toShow.Id, toShow.Id));
		}
	}

	// Token: 0x020004A9 RID: 1193
	// (Invoke) Token: 0x06002507 RID: 9479
	public delegate void GameloopAction();

	// Token: 0x020004AA RID: 1194
	// (Invoke) Token: 0x0600250B RID: 9483
	public delegate void AsyncAction();
}
