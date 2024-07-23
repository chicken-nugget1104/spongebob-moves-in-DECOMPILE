using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DeltaDNA;
using MiniJSON;
using UnityEngine;

// Token: 0x02000172 RID: 370
public class Game
{
	// Token: 0x06000C6D RID: 3181 RVA: 0x0004AB4C File Offset: 0x00048D4C
	public Game(SBAnalytics analytics, Dictionary<string, object> gameState, Player p, StaticContentLoader contentLoader, PersistedActionBuffer actBuffer, PlayHavenController phController)
	{
		TFUtils.Assert(Game.IsValidState(gameState), "Game.ctor received invalid gamestate. Try to catch where the invalid state was created and fix it!");
		this.playHavenController = phController;
		this.gameState = gameState;
		this.actionBuffer = actBuffer;
		this.gameFile = p.CacheFile("game.json");
		this.player = p;
		this.resourceManager = contentLoader.TheResourceManager;
		this.analytics = analytics;
		this.levelingManager = contentLoader.TheLevelingManager;
		this.SaveVersionInfo();
		this.LoadResources();
		this.LoadPlaytime(this.gameState, this.resourceManager);
		this.inventory = new PlayerInventory();
		this.rewardCap = new RewardCap();
		this.entities = contentLoader.TheEntityManager;
		this.dialogPackageManager = new DialogPackageManager(gameState);
		this.sessionActionManager = new SessionActionManager();
		this.craftManager = contentLoader.TheCraftingManager;
		this.vendingManager = contentLoader.TheVendingManager;
		this.paytableManager = contentLoader.ThePaytableManager;
		this.featureManager = contentLoader.TheFeatureManager;
		this.buildingUnlockManager = contentLoader.TheBuildingUnlockManager;
		this.treasureManager = contentLoader.TheTreasureManager;
		this.communityEventManager = contentLoader.TheCommunityEventManager;
		this.taskManager = contentLoader.TheTaskManager;
		this.microEventManager = contentLoader.TheMicroEventManager;
		this.costumeManager = contentLoader.TheCostumeManager;
		this.wishTableManager = contentLoader.TheWishTableManager;
		this.movieManager = contentLoader.TheMovieManager;
		this.dropManager = new ItemDropManager();
		this.catalog = contentLoader.TheCatalog;
		this.autoQuestDatabase = contentLoader.TheAutoQuestDatabase;
		this.questManager = contentLoader.TheQuestManager;
		this.questManager.SetDialogManager(this.dialogPackageManager);
		this.notificationManager = new NotificationManager();
		this.triggerRouter = new TriggerRouter(new List<ITriggerObserver>
		{
			this.questManager,
			this.notificationManager,
			this.sessionActionManager
		});
		this.sessionStateChangeObservers = new List<Action<Game>>();
		this.sessionStateChangeObservers.Add(new Action<Game>(this.sessionActionManager.RequestProcess));
		this.terrain = contentLoader.TheTerrain;
		this.border = contentLoader.TheBorder;
		this.resourceManager.UpdateProductGroups(this.craftManager);
		TFUtils.DebugTimeOffset = 0UL;
		TFUtils.FastForwardOffset = 0UL;
		TFUtils.AddTimeOffset = 0UL;
		if (gameState.ContainsKey("playtime"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["playtime"];
			object obj = null;
			if (dictionary.TryGetValue("adjusted_debug_time", out obj) && obj != null)
			{
				ulong num = 0UL;
				Type type = obj.GetType();
				if (type == typeof(ulong))
				{
					num = (ulong)obj;
				}
				else if (type == typeof(double))
				{
					num = Convert.ToUInt64((double)obj);
				}
				else if (type == typeof(uint))
				{
					num = Convert.ToUInt64((uint)obj);
				}
				else if (type == typeof(long))
				{
					num = Convert.ToUInt64((long)obj);
				}
				TFUtils.DebugTimeOffset = num;
				TFUtils.FastForwardOffset = num;
			}
			object obj2 = null;
			if (dictionary.TryGetValue("debug_add_time", out obj2) && obj2 != null)
			{
				ulong addTimeOffset = 0UL;
				Type type2 = obj2.GetType();
				if (type2 == typeof(ulong))
				{
					addTimeOffset = (ulong)obj2;
				}
				else if (type2 == typeof(double))
				{
					addTimeOffset = Convert.ToUInt64((double)obj2);
				}
				else if (type2 == typeof(uint))
				{
					addTimeOffset = Convert.ToUInt64((uint)obj2);
				}
				else if (type2 == typeof(long))
				{
					addTimeOffset = Convert.ToUInt64((long)obj2);
				}
				TFUtils.AddTimeOffset = addTimeOffset;
			}
		}
		this.LoadVariables();
		AutoQuestDatabase.SetPreviousAutoQuestDataFramGameState(gameState);
		this.m_fLocalSaveTimer = 0f;
		this.m_bNeedsLocalSave = false;
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x0004AF44 File Offset: 0x00049144
	public static bool GameExists(Player p)
	{
		return TFUtils.FileIsExists(p.CacheFile("game.json"));
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x0004AF58 File Offset: 0x00049158
	public static bool GameCacheExists(string playerName)
	{
		return TFUtils.FileIsExists(Player.PlayerCacheFile(playerName, "game.json"));
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x0004AF6C File Offset: 0x0004916C
	public static string GamePath(Player p)
	{
		return p.CacheFile("game.json");
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0004AF7C File Offset: 0x0004917C
	public static string GameCachePath(string playerName)
	{
		return Player.PlayerCacheFile(playerName, "game.json");
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0004AF8C File Offset: 0x0004918C
	public static Game CreateNew(SBAnalytics analytics, Player p, StaticContentLoader contentLoader, out int performedMigration, PlayHavenController phController)
	{
		TFUtils.DebugLog("Creating new game");
		Dictionary<string, object> data = InitialGame.Generate(contentLoader.TheEntityManager, contentLoader.TheResourceManager);
		Game game = Game.LoadFromDataDict(data, analytics, p, contentLoader, out performedMigration, phController);
		game.actionBuffer.Record(new InitializeAction());
		game.analytics.InitGameValues(game);
		TFUtils.DebugTimeOffset = 0UL;
		TFUtils.AddTimeOffset = 0UL;
		TFUtils.TriggerPurchaseWarning();
		return game;
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x0004AFF4 File Offset: 0x000491F4
	public static Game LoadFromCache(Player p, SBAnalytics analytics, StaticContentLoader contentLoader, out int performedMigration, PlayHavenController phController)
	{
		string json = TFUtils.ReadAllText(p.CacheFile("game.json"));
		Dictionary<string, object> data = (Dictionary<string, object>)Json.Deserialize(json);
		return Game.LoadFromDataDict(data, analytics, p, contentLoader, out performedMigration, phController);
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x0004B02C File Offset: 0x0004922C
	public static SoaringContext CreateSoaringGameResponderContext(SoaringContextDelegate del)
	{
		return new SoaringContext
		{
			Responder = new Game.GameSoaringResponder(),
			ContextResponder = del
		};
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0004B054 File Offset: 0x00049254
	public static void LoadFromNetwork(string userID, long timestamp, SoaringContext context, Session session)
	{
		if (context != null && context.Responder == null)
		{
			context.Responder = new SoaringDelegate();
		}
		session.WebFileServer.GetGameData(userID, timestamp, context);
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0004B08C File Offset: 0x0004928C
	public static Game LoadFromDataDict(Dictionary<string, object> data, SBAnalytics analytics, Player p, StaticContentLoader contentLoader, out int performedMigration, PlayHavenController phController)
	{
		if (!Game.IsValidState(data))
		{
			string message = "Encountered invalid data! Creating a new local state.";
			TFUtils.DebugLog(message);
			throw new Exception(message);
		}
		List<Dictionary<string, object>> actionList = PersistedActionBuffer.LoadActionList(p);
		GamestateMigrator gamestateMigrator = new GamestateMigrator();
		gamestateMigrator.Migrate(data, actionList, contentLoader, p, out performedMigration);
		if (performedMigration == 3)
		{
			return null;
		}
		PersistedActionBuffer actBuffer = new PersistedActionBuffer(p, actionList);
		return new Game(analytics, data, p, contentLoader, actBuffer, phController);
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x0004B0F0 File Offset: 0x000492F0
	public static bool IsValidState(Dictionary<string, object> data)
	{
		TFUtils.AssertKeyExists(data, "farm");
		return (!data.ContainsKey("error") || !((string)data["error"]).Equals("new_user")) && data.ContainsKey("farm");
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x0004B148 File Offset: 0x00049348
	public void DestroyCache()
	{
		Dictionary<string, object> obj = this.gameState;
		lock (obj)
		{
			TFUtils.DeleteFile(this.gameFile);
			this.actionBuffer.Flush();
		}
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x0004B1A0 File Offset: 0x000493A0
	public void Clear()
	{
		this.sessionActionManager.ClearActions();
		if (this.simulation != null)
		{
			this.simulation.Clear();
		}
		SBGUI.GetInstance().ResetWhiteList();
		UnityGameResources.Reset();
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x0004B1E0 File Offset: 0x000493E0
	private void LoadVariables()
	{
		if (this.gameState.ContainsKey("variables"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["variables"];
			if (dictionary.ContainsKey("auto_quest_completion_time"))
			{
				this.questManager.m_uQuestCompletionTimestamp = new ulong?(TFUtils.LoadUlong(dictionary, "auto_quest_completion_time", 0UL));
			}
			if (dictionary.ContainsKey("current_auto_quest_completion_count"))
			{
				this.questManager.m_autoQuestCount = TFUtils.LoadInt(dictionary, "current_auto_quest_completion_count");
			}
			if (dictionary.ContainsKey("first_auto_quest_completion_timestamp"))
			{
				this.questManager.m_uAutoQuestStartTime = new ulong?(TFUtils.LoadUlong(dictionary, "first_auto_quest_completion_timestamp", 0UL));
			}
		}
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x0004B298 File Offset: 0x00049498
	public void LoadSimulation(ulong utcNow)
	{
		this.loadSimulationActions = new Action[]
		{
			delegate()
			{
				this.LoadMicroEvents(utcNow);
			},
			delegate()
			{
				this.LoadTasks(utcNow);
			},
			delegate()
			{
				this.LoadTaskCompletions();
			},
			delegate()
			{
				this.LoadCostumes();
			},
			delegate()
			{
				this.LoadCraftings(utcNow);
			},
			delegate()
			{
				this.LoadVending(utcNow);
			},
			delegate()
			{
				this.LoadBuildings(utcNow);
			},
			delegate()
			{
				this.LoadDebris(utcNow);
			},
			delegate()
			{
				this.LoadLandmarks(utcNow);
			},
			delegate()
			{
				this.LoadTerrain();
			},
			delegate()
			{
				this.LoadTreasures(utcNow);
			},
			delegate()
			{
				this.LoadUnits(utcNow);
			},
			delegate()
			{
				this.LoadWanderers(utcNow);
			},
			delegate()
			{
				this.LoadLastRandomQuestId();
			},
			delegate()
			{
				this.LoadLastAutoQuestId();
			},
			delegate()
			{
				this.LoadQuestDefinitions(utcNow);
			},
			delegate()
			{
				this.LoadQuests(utcNow);
			},
			delegate()
			{
				this.LoadRecipes();
			},
			delegate()
			{
				this.LoadFeatureUnlocks();
			},
			delegate()
			{
				this.LoadBuildingUnlocks();
			},
			delegate()
			{
				this.LoadTreasureState();
			},
			delegate()
			{
				this.LoadRewardCaps();
			},
			delegate()
			{
				this.LoadMovies();
			},
			delegate()
			{
				this.LoadDropPickups();
			},
			delegate()
			{
				this.taskManager.RemoveUnsafeActiveTasks(this);
			},
			delegate()
			{
				this.PatchReferences();
			}
		};
		this.loadSimulationActionsEnumerator = this.loadSimulationActions.GetEnumerator();
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x0004B470 File Offset: 0x00049670
	public bool IterateLoadSimulation()
	{
		if (this.loadSimulationActionsEnumerator.MoveNext())
		{
			Action action = (Action)this.loadSimulationActionsEnumerator.Current;
			if (action != null)
			{
				action();
			}
			else
			{
				TFUtils.WarningLog("missing loaded action!");
			}
			return false;
		}
		return true;
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x0004B4BC File Offset: 0x000496BC
	public string LoadActions(ulong utcNow, bool applyAction, bool forceSave)
	{
		string result = null;
		Dictionary<string, object> obj = this.gameState;
		lock (obj)
		{
			List<PersistedActionBuffer.PersistedAction> allUnackedActions = this.actionBuffer.GetAllUnackedActions();
			TFUtils.DebugLog("Applying " + allUnackedActions.Count + " actions to gamestate.");
			if (allUnackedActions.Count > 0 || forceSave)
			{
				this.LoadActionsFromList(allUnackedActions, utcNow, applyAction);
				this.actionBuffer.Flush();
			}
			result = this.SaveLocally(0UL, true, true, false);
		}
		return result;
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x0004B560 File Offset: 0x00049760
	public void ClearActionBuffer()
	{
		Dictionary<string, object> obj = this.gameState;
		lock (obj)
		{
			this.actionBuffer.Flush();
		}
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x0004B5B0 File Offset: 0x000497B0
	public void SaveToServer(Session session, ulong utcNow, bool applyActions, bool forceSave)
	{
		string text = null;
		if (!this.CanSave)
		{
			return;
		}
		try
		{
			text = this.LoadActions(utcNow, applyActions, forceSave);
		}
		catch (Exception ex)
		{
			TFUtils.ErrorLog("Needs reload error dialog");
			SoaringDebug.Log("Needs reload error dialog: " + ex.Message, LogType.Error);
			TFUtils.ErrorLog(ex);
			this.needsReloadErrorDialog = true;
			this.RequestReload();
			text = null;
		}
		if (text != null)
		{
			TFUtils.DebugLog("Saving gamedata to server");
			SoaringContext context = Game.CreateSoaringGameResponderContext(new SoaringContextDelegate(this.OnSaveGameData));
			session.WebFileServer.SaveGameData(text, context);
		}
		else
		{
			TFUtils.DebugLog("Gamedata has not changed. Not saving to server");
		}
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x0004B674 File Offset: 0x00049874
	public void OnSaveGameData(SoaringContext context)
	{
		if (context == null)
		{
			return;
		}
		bool flag = context.soaringValue("status");
		SoaringError soaringError = (SoaringError)context.objectWithKey("error_message");
		SoaringDictionary soaringDictionary = (SoaringDictionary)context.objectWithKey("custom");
		if (soaringError != null || !flag || !Soaring.IsOnline)
		{
			if (soaringDictionary == null)
			{
				this.needsNetworkDownErrorDialog = false;
			}
			SoaringDebug.Log(soaringError, LogType.Error);
		}
		else if (soaringDictionary != null)
		{
			SBWebFileServer.LastSuccessfulSave = DateTime.UtcNow;
			long num = soaringDictionary.soaringValue("datetime");
			if (Player.ValidTimeStamp(num))
			{
				this.player.SetStagedTimestamp(num);
				this.player.SaveStagedTimestamp();
			}
		}
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x0004B734 File Offset: 0x00049934
	public void AddTimeToSimulation(ulong nSeconds)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["playtime"];
		object value = null;
		if (dictionary.TryGetValue("debug_add_time", out value))
		{
			dictionary["debug_add_time"] = Convert.ToUInt64(value) + nSeconds;
		}
		else
		{
			dictionary.Add("debug_add_time", nSeconds);
		}
		this.gameState["playtime"] = dictionary;
		TFUtils.AddTimeOffset = (ulong)dictionary["debug_add_time"];
		this.SaveLocally(0UL, true, false, false);
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0004B7CC File Offset: 0x000499CC
	public void FastForwardSimulationBegun()
	{
		TFUtils.UtcNow = new DateTime(1970, 1, 1);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["playtime"];
		if (dictionary.ContainsKey("fast_forward_start_time"))
		{
			dictionary["fast_forward_start_time"] = DateTime.UtcNow;
		}
		else
		{
			dictionary.Add("fast_forward_start_time", DateTime.UtcNow);
		}
		this.gameState["playtime"] = dictionary;
		this.SaveLocally(0UL, true, false, false);
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0004B85C File Offset: 0x00049A5C
	public void FastForwardSimulationFinished()
	{
		TFUtils.isFastForwarding = false;
		Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["playtime"];
		DateTime utcNow = TFUtils.UtcNow;
		DateTime value = utcNow;
		if (dictionary.ContainsKey("fast_forward_start_time"))
		{
			value = (DateTime)dictionary["fast_forward_start_time"];
		}
		object value2 = null;
		if (dictionary.TryGetValue("adjusted_debug_time", out value2))
		{
			dictionary["adjusted_debug_time"] = Convert.ToUInt64(value2) + Convert.ToUInt64(utcNow.Subtract(value).TotalSeconds);
		}
		else
		{
			dictionary.Add("adjusted_debug_time", Convert.ToUInt64(utcNow.Subtract(value).TotalSeconds));
		}
		this.gameState["playtime"] = dictionary;
		TFUtils.FastForwardOffset = (ulong)dictionary["adjusted_debug_time"];
		this.SaveLocally(0UL, true, false, false);
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x0004B94C File Offset: 0x00049B4C
	private void LoadDebrisSlotObject(TerrainSlot expansion, TerrainSlotObject debrisObject, ulong utcNow)
	{
		DebrisEntity decorator = this.entities.Create(EntityType.DEBRIS, debrisObject.did, debrisObject.id, true).GetDecorator<DebrisEntity>();
		PurchasableDecorator decorator2 = decorator.GetDecorator<PurchasableDecorator>();
		decorator2.Purchased = false;
		decorator.ExpansionId = new int?(expansion.Id);
		Simulated.Debris.Load(decorator, this.simulation, debrisObject.position.ToVector2(), utcNow);
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0004B9B4 File Offset: 0x00049BB4
	private void LoadLandmarkSlotObject(TerrainSlot expansion, TerrainSlotObject landmarkObject, ulong utcNow)
	{
		LandmarkEntity decorator = this.entities.Create(EntityType.LANDMARK, landmarkObject.did, landmarkObject.id, true).GetDecorator<LandmarkEntity>();
		decorator.GetDecorator<PurchasableDecorator>().Purchased = false;
		Simulated.Landmark.Load(decorator, this.simulation, landmarkObject.position.ToVector2(), utcNow);
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x0004BA0C File Offset: 0x00049C0C
	public void LoadExpansions(ulong utcNow)
	{
		List<Game.LoadSlotObjectData> list = new List<Game.LoadSlotObjectData>();
		foreach (TerrainSlot terrainSlot in this.terrain.UnpurchasedExpansionSlots())
		{
			foreach (TerrainSlotObject o in terrainSlot.debris)
			{
				list.Add(new Game.LoadSlotObjectData(terrainSlot, o, utcNow, new Game.LoadSlotObject(this.LoadDebrisSlotObject)));
			}
			foreach (TerrainSlotObject o2 in terrainSlot.landmarks)
			{
				list.Add(new Game.LoadSlotObjectData(terrainSlot, o2, utcNow, new Game.LoadSlotObject(this.LoadLandmarkSlotObject)));
			}
		}
		this.loadExpansionSlotObjectsEnumerator = list.GetEnumerator();
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0004BB60 File Offset: 0x00049D60
	public bool IterateLoadExpansions()
	{
		float num = 10f;
		DateTime utcNow = DateTime.UtcNow;
		while (this.loadExpansionSlotObjectsEnumerator.MoveNext())
		{
			Game.LoadSlotObjectData loadSlotObjectData = (Game.LoadSlotObjectData)this.loadExpansionSlotObjectsEnumerator.Current;
			if (loadSlotObjectData != null)
			{
				loadSlotObjectData.Load();
			}
			else
			{
				TFUtils.WarningLog("missing slot object loading expansions!");
			}
			if ((DateTime.UtcNow - utcNow).TotalMilliseconds > (double)num)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x0004BBD8 File Offset: 0x00049DD8
	public void LocalSaveCheck(float fDeltaTime)
	{
		this.m_fLocalSaveTimer += fDeltaTime;
		if (this.m_bNeedsLocalSave && this.m_fLocalSaveTimer >= 30f)
		{
			this.SaveLocally(0UL, false, false, true);
		}
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x0004BC10 File Offset: 0x00049E10
	public string SaveLocally(ulong timestamp, bool skipSave = false, bool skipWrite = false, bool useStaged = false)
	{
		Dictionary<string, object> obj = this.gameState;
		lock (obj)
		{
			if (this.CanSave)
			{
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"Game Saving: ",
					timestamp,
					", ",
					skipSave,
					", ",
					skipSave,
					", ",
					useStaged
				}));
				string text = Json.Serialize(this.gameState);
				if (skipWrite)
				{
					return text;
				}
				string directoryName = Path.GetDirectoryName(this.gameFile);
				Debug.Log(directoryName);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllText(this.gameFile, text);
				this.m_fLocalSaveTimer = 0f;
				this.m_bNeedsLocalSave = false;
				if (skipSave)
				{
					return text;
				}
				if (useStaged)
				{
					this.player.SaveStagedTimestamp();
					return text;
				}
				if (Player.ValidTimeStamp((long)timestamp))
				{
					this.player.SaveTimestamp((long)timestamp);
				}
				return text;
			}
			else
			{
				TFUtils.DebugLog("Game Is Locked Cannot Save Locally");
			}
		}
		return null;
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x0004BD60 File Offset: 0x00049F60
	public string LastAction()
	{
		return (string)((Dictionary<string, object>)this.gameState["farm"])["last_action"];
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x0004BD94 File Offset: 0x00049F94
	public void LockedGameStateChange(Game.GamestateWriter writer)
	{
		Dictionary<string, object> obj = this.gameState;
		lock (obj)
		{
			writer(this.gameState);
			this.SaveLocally(0UL, true, false, false);
		}
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x0004BDF0 File Offset: 0x00049FF0
	public void RequestLoadFriendPark(string park)
	{
		this.loadFriendGame = true;
		this.SetPendingReload(true);
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x0004BE04 File Offset: 0x0004A004
	public bool ReloadToFriendPark()
	{
		return this.loadFriendGame;
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x0004BE10 File Offset: 0x0004A010
	public void ClearLoadFriendPark()
	{
		this.loadFriendGame = false;
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x0004BE1C File Offset: 0x0004A01C
	public void RequestReload()
	{
		this.needsReload = true;
		this.SetPendingReload(true);
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x0004BE30 File Offset: 0x0004A030
	public bool RequiresReload()
	{
		return this.needsReload;
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x0004BE3C File Offset: 0x0004A03C
	public void ClearReloadRequest()
	{
		this.needsReload = false;
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x0004BE48 File Offset: 0x0004A048
	public void SetPendingReload(bool rr)
	{
		this.pendingReload = rr;
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x0004BE54 File Offset: 0x0004A054
	public bool PendingReload()
	{
		return this.pendingReload;
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x0004BE60 File Offset: 0x0004A060
	public void NULL_ModifyStateSimulated(Simulated simulated, PersistedSimulatedAction action)
	{
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x0004BE64 File Offset: 0x0004A064
	public void ModifyGameStateSimulated(Simulated simulated, PersistedSimulatedAction action)
	{
		this.ModifyGameStateHelper(action, new Dictionary<string, object>
		{
			{
				"simulated",
				simulated
			}
		});
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x0004BE8C File Offset: 0x0004A08C
	public void NULL_ModifyGameState(PersistedTriggerableAction action)
	{
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x0004BE90 File Offset: 0x0004A090
	public void ModifyGameState(PersistedTriggerableAction action)
	{
		this.ModifyGameStateHelper(action, new Dictionary<string, object>());
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x0004BEA0 File Offset: 0x0004A0A0
	public void LoadLastRandomQuestId()
	{
		uint lastRandomQuestId = 400000U;
		if (((Dictionary<string, object>)this.gameState["farm"]).ContainsKey("random_quest_id"))
		{
			lastRandomQuestId = uint.Parse(((Dictionary<string, object>)this.gameState["farm"])["random_quest_id"].ToString());
		}
		QuestDefinition.LastRandomQuestId = lastRandomQuestId;
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x0004BF08 File Offset: 0x0004A108
	public void LoadLastAutoQuestId()
	{
		uint lastAutoQuestId = 500001U;
		if (((Dictionary<string, object>)this.gameState["farm"]).ContainsKey("auto_quest_id"))
		{
			lastAutoQuestId = uint.Parse(((Dictionary<string, object>)this.gameState["farm"])["auto_quest_id"].ToString());
		}
		QuestDefinition.LastAutoQuestId = lastAutoQuestId;
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x0004BF70 File Offset: 0x0004A170
	public int GetResidentPopulation()
	{
		List<object> list = (List<object>)((Dictionary<string, object>)this.gameState["farm"])["units"];
		return list.Count;
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x0004BFA8 File Offset: 0x0004A1A8
	private void ModifyGameStateHelper(PersistedTriggerableAction action, Dictionary<string, object> data)
	{
		action.Process(this);
		this.simulation.RecordAction(action);
		if (!SBSettings.UseActionFile)
		{
			action.Confirm(this.gameState);
			this.m_bNeedsLocalSave = true;
		}
		this.playtimeRegistrar.Process(action, this.resourceManager.Query(ResourceManager.LEVEL), this.analytics);
		if (!Session.TheDebugManager.debugPlaceObjects)
		{
			ITrigger trigger = action.CreateTrigger(data);
			this.triggerRouter.RouteTrigger(trigger, this);
		}
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x0004C030 File Offset: 0x0004A230
	public void Record(PersistedTriggerableAction action)
	{
		if (action == null)
		{
			return;
		}
		if (SBSettings.UseActionFile)
		{
			this.actionBuffer.Record(action);
		}
		else
		{
			action.Confirm(this.gameState);
			this.m_bNeedsLocalSave = true;
		}
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x0004C068 File Offset: 0x0004A268
	public void ApplyReward(Reward reward, ulong buildingCompleteTime, bool bDoAnalytics = true)
	{
		List<int> clearedLands = reward.ClearedLands;
		int num = 0;
		if (clearedLands != null)
		{
			num = clearedLands.Count;
		}
		for (int i = 0; i < num; i++)
		{
			this.simulation.game.terrain.AddAndClearExpansionSlot(this, clearedLands[i]);
		}
		foreach (KeyValuePair<int, int> keyValuePair in reward.ResourceAmounts)
		{
			this.simulation.resourceManager.Add(keyValuePair.Key, keyValuePair.Value, this);
		}
		Dictionary<int, Vector2> buildingPositions = reward.BuildingPositions;
		foreach (KeyValuePair<int, int> keyValuePair2 in reward.BuildingAmounts)
		{
			int key = keyValuePair2.Key;
			int value = keyValuePair2.Value;
			List<object> list = (List<object>)reward.BuildingLabels[key.ToString()];
			for (int j = 0; j < value; j++)
			{
				Identity id = new Identity((string)list[j]);
				if (buildingPositions.ContainsKey(key))
				{
					Entity entity = this.entities.Create(EntityType.BUILDING, key, id, true);
					BuildingEntity decorator = entity.GetDecorator<BuildingEntity>();
					decorator.Slots = this.craftManager.GetInitialSlots(decorator.DefinitionId);
					ErectableDecorator decorator2 = decorator.GetDecorator<ErectableDecorator>();
					decorator2.ErectionCompleteTime = new ulong?(TFUtils.EpochTime() + decorator2.ErectionTime);
					Simulated simulated = Simulated.Building.Load(decorator, this.simulation, buildingPositions[key], false, TFUtils.EpochTime());
					NewBuildingAction action = new NewBuildingAction(id, entity.BlueprintName, key, EntityType.BUILDING, false, decorator2.ErectionCompleteTime.Value, buildingPositions[key], false, new Cost());
					this.ModifyGameStateSimulated(simulated, action);
					this.communityEventManager.GetSession().TheCamera.AutoPanToPosition(simulated.PositionCenter, 0.75f);
				}
				else
				{
					BuildingEntity decorator3 = this.entities.Create(EntityType.BUILDING, key, id, true).GetDecorator<BuildingEntity>();
					decorator3.GetDecorator<ErectableDecorator>().ErectionCompleteTime = new ulong?(buildingCompleteTime);
					decorator3.GetDecorator<ActivatableDecorator>().Activated = buildingCompleteTime;
					this.inventory.AddItem(decorator3, null);
					if (bDoAnalytics)
					{
						this.analytics.LogBuildingAddToInventory(decorator3.BlueprintName, this.resourceManager.Resources[ResourceManager.LEVEL].Amount);
					}
				}
			}
		}
		foreach (int id2 in reward.RecipeUnlocks)
		{
			this.simulation.craftManager.UnlockRecipe(id2, this);
		}
		foreach (int id3 in reward.MovieUnlocks)
		{
			this.simulation.movieManager.UnlockMovie(id3);
		}
		foreach (int nCostumeDID in reward.CostumeUnlocks)
		{
			this.costumeManager.UnlockCostume(nCostumeDID);
		}
		foreach (int buildingDid in reward.BuildingUnlocks)
		{
			this.buildingUnlockManager.UnlockBuilding(buildingDid);
		}
		if (reward.RandomLand)
		{
			this.simulation.game.terrain.AddRandomAvailableSlot(this.simulation.game);
		}
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x0004C4EC File Offset: 0x0004A6EC
	public void OnChangeSessionState()
	{
		foreach (Action<Game> action in this.sessionStateChangeObservers)
		{
			action(this);
		}
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x0004C554 File Offset: 0x0004A754
	public SBMIAnalytics.CommonData GetAnalyticsCommonData()
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["farm"];
		List<object> list = (List<object>)dictionary["buildings"];
		List<object> list2 = (List<object>)dictionary["units"];
		int num = 0;
		int num2 = 1;
		List<Simulated> simulatedRaw = this.simulation.GetSimulatedRaw();
		int count = simulatedRaw.Count;
		for (int i = 0; i < count; i++)
		{
			Simulated simulated = simulatedRaw[i];
			if (simulated.entity.Type == EntityType.BUILDING)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity != null)
				{
					if (entity.ResidentDids.Count > 0)
					{
						num++;
					}
				}
			}
			else if (simulated.entity.Type == EntityType.RESIDENT)
			{
				num2++;
			}
		}
		int nCharacters = num2;
		int nHouses = num;
		bool bIsEligibleForSpongyGames = false;
		CommunityEvent activeEvent = this.communityEventManager.GetActiveEvent();
		if (activeEvent != null && (activeEvent.m_sID == CommunityEventManager._sSpongyGamesEventID || activeEvent.m_sID == CommunityEventManager._sSpongyGamesLastDayEventID))
		{
			if (activeEvent.m_nQuestPrereqID >= 0)
			{
				if (this.questManager.IsQuestCompleted((uint)activeEvent.m_nQuestPrereqID))
				{
					bIsEligibleForSpongyGames = true;
				}
			}
			else
			{
				bIsEligibleForSpongyGames = true;
			}
		}
		int nSpongyCurrency = 0;
		if (this.resourceManager.Resources.ContainsKey(ResourceManager.SPONGY_GAMES_CURRENCY))
		{
			nSpongyCurrency = this.resourceManager.Resources[ResourceManager.SPONGY_GAMES_CURRENCY].Amount;
		}
		SBMIAnalytics.CommonData result;
		result.ulDateTime = SoaringAnalytics.AnalyticTime();
		result.ulFirstPlayTime = TFUtils.LoadUlong((Dictionary<string, object>)list[0], "build_finish_time", 0UL);
		result.nPlayerLevel = this.resourceManager.PlayerLevelAmount;
		result.nSoftCurrency = this.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount;
		result.nHardCurreny = this.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount;
		result.nCharacters = nCharacters;
		result.nHouses = nHouses;
		result.nLandExpansions = this.terrain.purchasedSlots.Count - 1;
		result.nSpongyCurrency = nSpongyCurrency;
		result.bIsEligibleForSpongyGames = bIsEligibleForSpongyGames;
		result.sPlayerID = Soaring.Player.UserID;
		result.sPlatform = SoaringPlatform.PrimaryPlatformName;
		result.sDeviceName = SystemInfo.deviceModel;
		result.sBinaryVersion = SoaringInternal.GameVersion.ToString();
		result.sCampaignData = SBAuth.campaigns;
		result.sOSVersion = SystemInfo.operatingSystem;
		result.sManifest = SBSettings.MANIFEST_FILE;
		result.sGUID = SoaringAnalytics.GenerateGUID();
		result.sDeviceGUID = SoaringAnalytics.DeviceGUID;
		result.ulSequence = SoaringAnalytics.DeviceSequenceID;
		return result;
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x0004C82C File Offset: 0x0004AA2C
	public SBMIAnalytics.PlayerObject GetAnalyticsPlayerObject()
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["farm"];
		List<object> list = (List<object>)dictionary["buildings"];
		List<object> list2 = (List<object>)dictionary["units"];
		int num = 0;
		int num2 = 1;
		List<Simulated> simulatedRaw = this.simulation.GetSimulatedRaw();
		int count = simulatedRaw.Count;
		for (int i = 0; i < count; i++)
		{
			Simulated simulated = simulatedRaw[i];
			if (simulated.entity.Type == EntityType.BUILDING)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity != null)
				{
					if (entity.ResidentDids.Count > 0)
					{
						num++;
					}
				}
			}
			else if (simulated.entity.Type == EntityType.RESIDENT)
			{
				num2++;
			}
		}
		int nNumCharacters = num2;
		int nNumHouses = num;
		CommunityEvent activeEvent = this.communityEventManager.GetActiveEvent();
		string sLiveEventName = (activeEvent != null) ? activeEvent.m_sName : null;
		int num3 = (activeEvent != null) ? activeEvent.m_nValueID : -1;
		int nSpecialCurrencyAmount = (num3 >= 0) ? this.resourceManager.Resources[num3].Amount : -1;
		return new SBMIAnalytics.PlayerObject("player", Soaring.Player.UserID, sLiveEventName, TFUtils.LoadUlong((Dictionary<string, object>)list[0], "build_finish_time", 0UL), this.resourceManager.PlayerLevelAmount, nNumCharacters, nNumHouses, this.terrain.purchasedSlots.Count - 1, this.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount, this.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount, num3, nSpecialCurrencyAmount, SBAuth.campaigns);
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x0004CA04 File Offset: 0x0004AC04
	public ulong FirstPlayTime()
	{
		ulong result = 0UL;
		try
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["farm"];
			List<object> list = (List<object>)dictionary["buildings"];
			result = TFUtils.LoadUlong((Dictionary<string, object>)list[0], "build_finish_time", 0UL);
		}
		catch
		{
		}
		return result;
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x0004CA7C File Offset: 0x0004AC7C
	public SBMIAnalytics.MetaObject GetAnalyticsMetaObject(string sEventName, int nOverrideTrackingVersion = -1)
	{
		return new SBMIAnalytics.MetaObject("meta", sEventName, SystemInfo.deviceModel, SoaringInternal.GameVersion.ToString(), SystemInfo.operatingSystem.ToString(), SBSettings.MANIFEST_FILE, SoaringPlatform.PrimaryPlatformName, SoaringAnalytics.GenerateGUID(), SoaringAnalytics.DeviceGUID, (nOverrideTrackingVersion < 0) ? SBMIAnalytics._nTRACKING_VERSION : nOverrideTrackingVersion, SoaringAnalytics.DeviceSequenceID, SoaringAnalytics.AnalyticTime());
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x0004CAE0 File Offset: 0x0004ACE0
	public static SBMIDeltaDNA.DeviceObject GetDeltaDNADeviceObject()
	{
		return new SBMIDeltaDNA.DeviceObject("device_data", ClientInfo.DeviceName, ClientInfo.DeviceType, ClientInfo.DeviceModel, ClientInfo.OperatingSystem, ClientInfo.OperatingSystemVersion, ClientInfo.Manufacturer, ClientInfo.TimezoneOffset, ClientInfo.LanguageCode);
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x0004CB20 File Offset: 0x0004AD20
	public SBMIDeltaDNA.PlayerObject GetDeltaDNAPlayerObject()
	{
		return new SBMIDeltaDNA.PlayerObject("player", this.resourceManager.PlayerLevelAmount, this.resourceManager.Resources[ResourceManager.XP].Amount, this.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount, this.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].Amount);
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x0004CB90 File Offset: 0x0004AD90
	private void LoadTerrain()
	{
		List<object> list = (List<object>)((Dictionary<string, object>)this.gameState["farm"])["pavement"];
		if (list.Count != 0)
		{
			foreach (object obj in list)
			{
				Dictionary<string, object> d = (Dictionary<string, object>)obj;
				GridPosition gpos = new GridPosition(TFUtils.LoadInt(d, "row"), TFUtils.LoadInt(d, "col"));
				this.terrain.ChangePath(gpos);
			}
		}
		List<object> list2 = (List<object>)((Dictionary<string, object>)this.gameState["farm"])["expansions"];
		if (list2.Count != 0)
		{
			this.terrain.purchasedSlots = new HashSet<int>(list2.ConvertAll<int>((object x) => Convert.ToInt32(x)));
			foreach (int id in this.terrain.purchasedSlots)
			{
				this.terrain.AddExpansionSlot(id);
			}
		}
		if (this.featureManager.CheckFeature("purchase_expansions"))
		{
			this.terrain.UpdateRealtySigns(this.entities.DisplayControllerManager, new BillboardDelegate(SBCamera.BillboardDefinition), this);
		}
		this.border.UpdateTerrainBorderStrip(this.terrain);
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x0004CD5C File Offset: 0x0004AF5C
	private void LoadRecipes()
	{
		TFUtils.AssertKeyExists((Dictionary<string, object>)this.gameState["farm"], "recipes");
		List<int> list = TFUtils.LoadList<int>((Dictionary<string, object>)this.gameState["farm"], "recipes");
		if (list.Count != 0)
		{
			foreach (int id in list)
			{
				this.craftManager.UnlockRecipe(id, this);
			}
		}
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x0004CE10 File Offset: 0x0004B010
	private void LoadFeatureUnlocks()
	{
		if (((Dictionary<string, object>)this.gameState["farm"]).ContainsKey("features"))
		{
			List<string> list = TFUtils.LoadList<string>((Dictionary<string, object>)this.gameState["farm"], "features");
			if (list.Count != 0)
			{
				foreach (string feature in list)
				{
					this.featureManager.UnlockFeature(feature);
				}
			}
		}
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x0004CEC8 File Offset: 0x0004B0C8
	private void LoadBuildingUnlocks()
	{
		if (((Dictionary<string, object>)this.gameState["farm"]).ContainsKey("building_unlocks"))
		{
			List<int> list = TFUtils.LoadList<int>((Dictionary<string, object>)this.gameState["farm"], "building_unlocks");
			if (list.Count != 0)
			{
				foreach (int buildingDid in list)
				{
					this.buildingUnlockManager.UnlockBuilding(buildingDid);
				}
			}
		}
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x0004CF80 File Offset: 0x0004B180
	private void LoadTreasureState()
	{
		if (((Dictionary<string, object>)this.gameState["farm"]).ContainsKey("treasure_state"))
		{
			Dictionary<string, object> dict = TFUtils.LoadDict((Dictionary<string, object>)this.gameState["farm"], "treasure_state");
			this.treasureManager.InitializeTreasureTimers(dict);
		}
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x0004CFE0 File Offset: 0x0004B1E0
	private void LoadRewardCaps()
	{
		if (((Dictionary<string, object>)this.gameState["farm"]).ContainsKey("caps"))
		{
			Dictionary<string, object> dictionary = TFUtils.LoadDict((Dictionary<string, object>)this.gameState["farm"], "caps");
			ulong expiration = TFUtils.LoadUlong(dictionary, "expiration", 0UL);
			int recipes = TFUtils.LoadInt(dictionary, "recipe_count");
			int jelly = TFUtils.LoadInt(dictionary, "jelly_count");
			this.rewardCap.Reset(jelly, recipes, expiration);
		}
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x0004D068 File Offset: 0x0004B268
	private void LoadMovies()
	{
		TFUtils.AssertKeyExists((Dictionary<string, object>)this.gameState["farm"], "movies");
		List<object> list = (List<object>)((Dictionary<string, object>)this.gameState["farm"])["movies"];
		if (list.Count != 0)
		{
			HashSet<int> hashSet = new HashSet<int>(list.ConvertAll<int>((object x) => Convert.ToInt32(x)));
			foreach (int id in hashSet)
			{
				this.movieManager.UnlockMovie(id);
			}
		}
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x0004D148 File Offset: 0x0004B348
	private void LoadDropPickups()
	{
		TFUtils.AssertKeyExists((Dictionary<string, object>)this.gameState["farm"], "drop_pickups");
		List<object> list = (List<object>)((Dictionary<string, object>)this.gameState["farm"])["drop_pickups"];
		if (list.Count != 0)
		{
			foreach (object obj in list)
			{
				Dictionary<string, object> newTrigger = (Dictionary<string, object>)obj;
				this.dropManager.AddPickupTrigger(newTrigger);
			}
		}
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x0004D204 File Offset: 0x0004B404
	private void SaveVersionInfo()
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["farm"];
		dictionary["game_version"] = SBSettings.BundleVersion;
		dictionary["bundle_id"] = SBSettings.BundleIdentifier;
		dictionary["manifest_url"] = SBSettings.MANIFEST_URL;
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x0004D258 File Offset: 0x0004B458
	private void LoadResources()
	{
		TFUtils.AssertKeyExists((Dictionary<string, object>)this.gameState["farm"], "resources");
		List<object> resources = (List<object>)((Dictionary<string, object>)this.gameState["farm"])["resources"];
		this.resourceManager.LoadResources(resources);
		this.resourceCalculatorManager = new ResourceCalculatorManager(this.levelingManager);
		this.resourceManager.UpdateLevelExpToMilestone(this.levelingManager);
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x0004D2D8 File Offset: 0x0004B4D8
	private void LoadPlaytime(Dictionary<string, object> gameState, ResourceManager resourceMgr)
	{
		TFUtils.AssertKeyExists(gameState, "playtime");
		Dictionary<string, object> data = TFUtils.LoadDict(gameState, "playtime");
		this.playtimeRegistrar = PlaytimeRegistrar.FromDict(data);
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x0004D308 File Offset: 0x0004B508
	private void LoadActionsFromList(List<PersistedActionBuffer.PersistedAction> actions, ulong utcNow, bool applyAction)
	{
		foreach (PersistedActionBuffer.PersistedAction persistedAction in actions)
		{
			TFUtils.DebugLog("Applying action: " + persistedAction.type);
			if (applyAction)
			{
				persistedAction.Apply(this, utcNow);
			}
			persistedAction.Confirm(this.gameState);
		}
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x0004D394 File Offset: 0x0004B594
	private void LoadUnits(ulong utcNow)
	{
		this.LoadObjects("units", new Game.ObjectLoaderFn(this.LoadUnit), utcNow);
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x0004D3B0 File Offset: 0x0004B5B0
	private void LoadWanderers(ulong utcNow)
	{
		this.LoadObjects("wanderers", new Game.ObjectLoaderFn(this.LoadWanderer), utcNow);
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x0004D3CC File Offset: 0x0004B5CC
	private void LoadBuildings(ulong utcNow)
	{
		this.LoadObjects("buildings", new Game.ObjectLoaderFn(this.LoadBuilding), utcNow);
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x0004D3E8 File Offset: 0x0004B5E8
	private void LoadDebris(ulong utcNow)
	{
		this.LoadObjects("debris", new Game.ObjectLoaderFn(this.LoadDebris), utcNow);
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x0004D404 File Offset: 0x0004B604
	private void LoadTreasures(ulong utcNow)
	{
		this.LoadObjects("treasure", new Game.ObjectLoaderFn(this.LoadTreasure), utcNow);
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x0004D420 File Offset: 0x0004B620
	private void LoadLandmarks(ulong utcNow)
	{
		this.LoadObjects("landmarks", new Game.ObjectLoaderFn(this.LoadLandmark), utcNow);
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x0004D43C File Offset: 0x0004B63C
	private void LoadTasks(ulong utcNow)
	{
		this.LoadObjects("tasks_v2", new Game.ObjectLoaderFn(this.LoadTask), utcNow);
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x0004D458 File Offset: 0x0004B658
	private void LoadTaskCompletions()
	{
		TFUtils.AssertKeyExists(this.gameState, "farm");
		Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["farm"];
		if (dictionary.ContainsKey("task_completion_counts"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["task_completion_counts"];
			int nDID = 0;
			foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
			{
				if (int.TryParse(keyValuePair.Key, out nDID))
				{
					this.taskManager.SetTaskCompletionCount(nDID, TFUtils.LoadInt(dictionary2, keyValuePair.Key));
				}
			}
		}
		else
		{
			dictionary.Add("task_completion_counts", new Dictionary<string, object>());
		}
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x0004D540 File Offset: 0x0004B740
	private void LoadMicroEvents(ulong utcNow)
	{
		this.LoadObjects("micro_events", new Game.ObjectLoaderFn(this.LoadMicroEvent), utcNow);
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x0004D55C File Offset: 0x0004B75C
	private void LoadQuests(ulong utcNow)
	{
		this.LoadObjects("quests", new Game.ObjectLoaderFn(this.LoadQuest), utcNow);
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x0004D578 File Offset: 0x0004B778
	private void LoadQuestDefinitions(ulong utcNow)
	{
		this.LoadObjects("generated_quest_definition", new Game.ObjectLoaderFn(this.LoadQuestDefinition), utcNow);
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0004D594 File Offset: 0x0004B794
	private void LoadCraftings(ulong utcNow)
	{
		this.LoadObjects("crafts", new Game.ObjectLoaderFn(this.LoadCrafting), utcNow);
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x0004D5B0 File Offset: 0x0004B7B0
	private void LoadVending(ulong utcNow)
	{
		this.LoadObjects("vending", new Game.ObjectLoaderFn(this.LoadVendor), utcNow);
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x0004D5CC File Offset: 0x0004B7CC
	private void LoadObjects(string key, Game.ObjectLoaderFn objectLoader, ulong utcNow)
	{
		TFUtils.AssertKeyExists(this.gameState, "farm");
		Dictionary<string, object> dictionary = (Dictionary<string, object>)this.gameState["farm"];
		if (!dictionary.ContainsKey(key))
		{
			dictionary[key] = new List<object>();
		}
		else
		{
			foreach (object obj in (dictionary[key] as List<object>))
			{
				Dictionary<string, object> data = (Dictionary<string, object>)obj;
				objectLoader(data, utcNow);
			}
		}
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x0004D684 File Offset: 0x0004B884
	private void LoadUnit(Dictionary<string, object> dict, ulong utcNow)
	{
		int num = TFUtils.LoadInt(dict, "did");
		Identity identity = new Identity((string)dict["label"]);
		Identity identity2 = new Identity((string)dict["residence"]);
		bool flag = (bool)dict["active"];
		ResidentEntity decorator = this.entities.Create(EntityType.RESIDENT, num, identity, true).GetDecorator<ResidentEntity>();
		decorator.Residence = identity2;
		decorator.HungryAt = TFUtils.LoadUlong(dict, "feed_ready_time", utcNow);
		if (decorator.HungryAt - utcNow > 1209600UL)
		{
			decorator.HungryAt = utcNow;
		}
		decorator.HungerResourceId = TFUtils.TryLoadInt(dict, "wish_product_id");
		decorator.PreviousResourceId = TFUtils.TryLoadInt(dict, "prev_wish_product_id");
		decorator.CostumeDID = TFUtils.TryLoadInt(dict, "costume_did");
		if (decorator.CostumeDID == null && decorator.DefaultCostumeDID != null)
		{
			decorator.CostumeDID = decorator.DefaultCostumeDID;
		}
		ulong? num2 = TFUtils.TryLoadUlong(dict, "fullness_length", 0UL);
		if (num2 == null)
		{
			decorator.FullnessLength = 90UL;
		}
		else
		{
			decorator.FullnessLength = num2.Value;
		}
		decorator.WishExpiresAt = TFUtils.TryLoadUlong(dict, "wish_expires_at", 0UL);
		if (decorator.HungerResourceId != null && decorator.WishExpiresAt == null)
		{
			TFUtils.WarningLog("Did not find required field wish_expires_at using default of now instead.");
			decorator.WishExpiresAt = new ulong?(utcNow);
		}
		decorator.MatchBonus = Reward.FromDict(TFUtils.TryLoadDict(dict, "match_bonus"));
		if (flag && !decorator.Disabled)
		{
			Simulated.Resident.Load(decorator, identity2, decorator.WishExpiresAt, decorator.HungerResourceId, decorator.PreviousResourceId, decorator.HungryAt, new ulong?(decorator.FullnessLength), decorator.MatchBonus, this.simulation, utcNow);
		}
		else
		{
			this.inventory.AddAssociatedEntities(identity2, new List<KeyValuePair<int, Identity>>
			{
				new KeyValuePair<int, Identity>(num, identity)
			});
		}
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x0004D8B8 File Offset: 0x0004BAB8
	private void LoadWanderer(Dictionary<string, object> dict, ulong utcNow)
	{
		int did = TFUtils.LoadInt(dict, "did");
		Identity id = new Identity((string)dict["label"]);
		ResidentEntity decorator = this.entities.Create(EntityType.WANDERER, did, id, true).GetDecorator<ResidentEntity>();
		decorator.HideExpiresAt = TFUtils.TryLoadUlong(dict, "hide_expires_at", 0UL);
		decorator.DisableFlee = TFUtils.TryLoadBool(dict, "disable_flee");
		decorator.Wanderer = true;
		if (decorator.DisableIfWillFlee.Value && !decorator.DisableFlee.Value)
		{
			return;
		}
		Simulated.Wanderer.Load(decorator, decorator.HideExpiresAt, decorator.DisableFlee, this.simulation, utcNow);
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x0004D970 File Offset: 0x0004BB70
	private bool BuildingIsInventory(Dictionary<string, object> dict)
	{
		return dict["x"] == null || dict["y"] == null;
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x0004D994 File Offset: 0x0004BB94
	private void LoadBuilding(Dictionary<string, object> dict, ulong utcNow)
	{
		int did = TFUtils.LoadInt(dict, "did");
		EntityType primaryType = (EntityType)TFUtils.LoadUint(dict, "extensions");
		Entity entity = this.entities.Create(EntityTypeNamingHelper.GetBlueprintName(primaryType, did), new Identity((string)dict["label"]), true);
		BuildingEntity decorator = entity.GetDecorator<BuildingEntity>();
		decorator.Deserialize(dict);
		object obj = null;
		dict.TryGetValue("rent_ready_time", out obj);
		if (obj != null && decorator.HasDecorator<PeriodicProductionDecorator>())
		{
			decorator.GetDecorator<PeriodicProductionDecorator>().ProductReadyTime = TFUtils.LoadUlong(dict, "rent_ready_time", 0UL);
		}
		if (dict.ContainsKey("craft_rewards"))
		{
			decorator.CraftRewards = Reward.FromObject(dict["craft_rewards"]);
		}
		else if (dict.ContainsKey("craft.rewards"))
		{
			decorator.CraftRewards = Reward.FromObject(dict["craft.rewards"]);
		}
		if (dict.ContainsKey("crafting_slots"))
		{
			decorator.Slots = TFUtils.LoadInt(dict, "crafting_slots");
		}
		else if (this.craftManager.HasInitialSlots(decorator.DefinitionId))
		{
			decorator.Slots = this.craftManager.GetInitialSlots(decorator.DefinitionId);
		}
		if (decorator.CanVend)
		{
			if (dict.ContainsKey("general_restock"))
			{
				decorator.GetDecorator<VendingDecorator>().RestockTime = TFUtils.LoadUlong(dict, "general_restock", 0UL);
			}
			if (dict.ContainsKey("special_restock"))
			{
				decorator.GetDecorator<VendingDecorator>().SpecialRestockTime = TFUtils.LoadUlong(dict, "special_restock", 0UL);
			}
		}
		if (this.BuildingIsInventory(dict))
		{
			this.inventory.AddItem(decorator, null);
		}
		else
		{
			Vector2 position = new Vector2((float)TFUtils.LoadInt(dict, "x"), (float)TFUtils.LoadInt(dict, "y"));
			bool? flag = TFUtils.LoadNullableBool(dict, "flip");
			Simulated simulated = Simulated.Building.Load(decorator, this.simulation, position, flag != null && flag.Value, utcNow);
			if ((entity.AllTypes & EntityType.ANNEX) != EntityType.INVALID)
			{
				Simulated.Annex.Extend(simulated, this.simulation);
			}
		}
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x0004DBBC File Offset: 0x0004BDBC
	private void LoadLandmark(Dictionary<string, object> dict, ulong utcNow)
	{
		LandmarkEntity decorator = this.entities.Create(EntityType.LANDMARK, TFUtils.LoadInt(dict, "did"), new Identity(TFUtils.LoadString(dict, "label")), true).GetDecorator<LandmarkEntity>();
		decorator.Deserialize(dict);
		Vector2 position = new Vector2((float)TFUtils.LoadInt(dict, "x"), (float)TFUtils.LoadInt(dict, "y"));
		Simulated.Landmark.Load(decorator, this.simulation, position, utcNow);
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x0004DC30 File Offset: 0x0004BE30
	private void LoadDebris(Dictionary<string, object> dict, ulong utcNow)
	{
		DebrisEntity decorator = this.entities.Create(EntityType.DEBRIS, TFUtils.LoadInt(dict, "did"), new Identity((string)dict["label"]), true).GetDecorator<DebrisEntity>();
		if (dict.ContainsKey("clear_complete_time"))
		{
			PurchasableDecorator decorator2 = decorator.GetDecorator<PurchasableDecorator>();
			decorator2.Purchased = true;
			ClearableDecorator decorator3 = decorator.GetDecorator<ClearableDecorator>();
			decorator3.ClearCompleteTime = TFUtils.LoadNullableUlong(dict, "clear_complete_time");
		}
		Vector2 position = new Vector2((float)TFUtils.LoadInt(dict, "x"), (float)TFUtils.LoadInt(dict, "y"));
		Simulated.Debris.Load(decorator, this.simulation, position, utcNow);
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x0004DCD4 File Offset: 0x0004BED4
	private void LoadTreasure(Dictionary<string, object> dict, ulong utcNow)
	{
		if (Session.PatchyTownGame)
		{
			return;
		}
		TreasureEntity decorator = this.entities.Create(EntityType.TREASURE, TFUtils.LoadInt(dict, "did"), new Identity((string)dict["label"]), true).GetDecorator<TreasureEntity>();
		if (dict.ContainsKey("clear_complete_time"))
		{
			decorator.ClearCompleteTime = TFUtils.LoadNullableUlong(dict, "clear_complete_time");
		}
		string text = TFUtils.TryLoadNullableString(dict, "treasure_spawner_name");
		if (text == null)
		{
			text = "time_to_spawn";
		}
		if (decorator == null)
		{
			Debug.LogError("Game: Null Or Invalid Tressure Entity: " + text);
			return;
		}
		decorator.TreasureTiming = this.treasureManager.FindTreasureSpawner(text);
		if (decorator.TreasureTiming == null)
		{
			Debug.LogError("Game: Null Or Invalid Tressure Timing: " + text);
			return;
		}
		Vector2 position = decorator.TreasureTiming.GenerateLocation();
		Simulated.Treasure.Load(decorator, this.simulation, position, utcNow);
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x0004DDBC File Offset: 0x0004BFBC
	private void LoadTask(Dictionary<string, object> pDict, ulong utcNow)
	{
		this.taskManager.AddActiveTask(this, new Task(this, pDict, false), true);
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x0004DDD4 File Offset: 0x0004BFD4
	private void LoadMicroEvent(Dictionary<string, object> pDict, ulong utcNow)
	{
		this.microEventManager.AddMicroEvent(this, new MicroEvent(this, pDict, false), true);
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x0004DDEC File Offset: 0x0004BFEC
	private void LoadCostumes()
	{
		if (!((Dictionary<string, object>)this.gameState["farm"]).ContainsKey("costumes"))
		{
			((Dictionary<string, object>)this.gameState["farm"]).Add("costumes", new List<object>());
		}
		List<int> list = TFUtils.LoadList<int>((Dictionary<string, object>)this.gameState["farm"], "costumes");
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			this.costumeManager.UnlockCostume(list[i]);
		}
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0004DE8C File Offset: 0x0004C08C
	private void LoadQuest(Dictionary<string, object> dict, ulong utcNow)
	{
		Quest quest = Quest.FromDict(dict);
		if (quest == null)
		{
			return;
		}
		this.questManager.RegisterQuest(this, quest);
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x0004DEB4 File Offset: 0x0004C0B4
	private void LoadQuestDefinition(Dictionary<string, object> pDict, ulong nUtcNow)
	{
		QuestDefinition questDefinition = QuestDefinition.FromDict(pDict);
		Quest quest = this.questManager.AddQuestDefinition(questDefinition);
		if (quest != null)
		{
			if (quest.Did == QuestDefinition.LastRandomQuestId)
			{
				QuestDefinition.RecreateRandomQuestStartInputData(this, questDefinition.Did);
				QuestDefinition.RecreateRandomQuestCompleteInputData(this, questDefinition.Did);
			}
			else if (quest.Did == QuestDefinition.LastAutoQuestId)
			{
				QuestDefinition.RecreateAutoQuestIntroInputData(this, questDefinition.Did);
				QuestDefinition.RecreateAutoQuestOutroInputData(this, questDefinition.Did);
			}
		}
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x0004DF34 File Offset: 0x0004C134
	private void LoadCrafting(Dictionary<string, object> dict, ulong utcNow)
	{
		CraftingInstance craftingInstance = new CraftingInstance(dict);
		this.craftManager.AddCraftingInstance(craftingInstance);
		if (craftingInstance.ReadyTimeUtc > utcNow)
		{
			this.simulation.Router.Send(CraftedCommand.Create(craftingInstance.buildingLabel, craftingInstance.buildingLabel, craftingInstance.slotId), craftingInstance.ReadyTimeUtc - utcNow);
		}
		else
		{
			this.simulation.Router.Send(CraftedCommand.Create(craftingInstance.buildingLabel, craftingInstance.buildingLabel, craftingInstance.slotId));
		}
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x0004DFBC File Offset: 0x0004C1BC
	private void LoadVendor(Dictionary<string, object> dict, ulong utcNow)
	{
		Identity target = new Identity((string)dict["label"]);
		Dictionary<string, object> generalInstances;
		if (dict.ContainsKey("general_instances"))
		{
			generalInstances = TFUtils.LoadDict(dict, "general_instances");
		}
		else
		{
			generalInstances = new Dictionary<string, object>();
		}
		Dictionary<string, object> specialInstances;
		if (dict.ContainsKey("special_instances"))
		{
			specialInstances = TFUtils.LoadDict(dict, "special_instances");
		}
		else
		{
			specialInstances = new Dictionary<string, object>();
		}
		this.vendingManager.LoadVendorInstances(target, generalInstances, specialInstances);
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x0004E03C File Offset: 0x0004C23C
	private void PatchReferences()
	{
		foreach (Entity entity in this.entities.GetEntities())
		{
			entity.PatchReferences(this);
		}
	}

	// Token: 0x04000865 RID: 2149
	public const string PLAYTIME = "playtime";

	// Token: 0x04000866 RID: 2150
	public const string GAME_FILE = "game.json";

	// Token: 0x04000867 RID: 2151
	private const float m_fLocalSaveTimeLength = 30f;

	// Token: 0x04000868 RID: 2152
	public EntityManager entities;

	// Token: 0x04000869 RID: 2153
	public ItemDropManager dropManager;

	// Token: 0x0400086A RID: 2154
	public CraftingManager craftManager;

	// Token: 0x0400086B RID: 2155
	public VendingManager vendingManager;

	// Token: 0x0400086C RID: 2156
	public TreasureManager treasureManager;

	// Token: 0x0400086D RID: 2157
	public PaytableManager paytableManager;

	// Token: 0x0400086E RID: 2158
	public FeatureManager featureManager;

	// Token: 0x0400086F RID: 2159
	public BuildingUnlockManager buildingUnlockManager;

	// Token: 0x04000870 RID: 2160
	public MovieManager movieManager;

	// Token: 0x04000871 RID: 2161
	public CommunityEventManager communityEventManager;

	// Token: 0x04000872 RID: 2162
	public TaskManager taskManager;

	// Token: 0x04000873 RID: 2163
	public MicroEventManager microEventManager;

	// Token: 0x04000874 RID: 2164
	public CostumeManager costumeManager;

	// Token: 0x04000875 RID: 2165
	public WishTableManager wishTableManager;

	// Token: 0x04000876 RID: 2166
	public Terrain terrain;

	// Token: 0x04000877 RID: 2167
	public Border border;

	// Token: 0x04000878 RID: 2168
	public Simulation simulation;

	// Token: 0x04000879 RID: 2169
	public Simulated selected;

	// Token: 0x0400087A RID: 2170
	public PersistedActionBuffer actionBuffer;

	// Token: 0x0400087B RID: 2171
	public Player player;

	// Token: 0x0400087C RID: 2172
	public ResourceManager resourceManager;

	// Token: 0x0400087D RID: 2173
	public LevelingManager levelingManager;

	// Token: 0x0400087E RID: 2174
	public ResourceCalculatorManager resourceCalculatorManager;

	// Token: 0x0400087F RID: 2175
	public PlayerInventory inventory;

	// Token: 0x04000880 RID: 2176
	public SessionActionManager sessionActionManager;

	// Token: 0x04000881 RID: 2177
	public TriggerRouter triggerRouter;

	// Token: 0x04000882 RID: 2178
	public DialogPackageManager dialogPackageManager;

	// Token: 0x04000883 RID: 2179
	public QuestManager questManager;

	// Token: 0x04000884 RID: 2180
	public AutoQuestDatabase autoQuestDatabase;

	// Token: 0x04000885 RID: 2181
	public NotificationManager notificationManager;

	// Token: 0x04000886 RID: 2182
	public Catalog catalog;

	// Token: 0x04000887 RID: 2183
	public RewardCap rewardCap;

	// Token: 0x04000888 RID: 2184
	public RmtStore store;

	// Token: 0x04000889 RID: 2185
	public SBAnalytics analytics;

	// Token: 0x0400088A RID: 2186
	public PlaytimeRegistrar playtimeRegistrar;

	// Token: 0x0400088B RID: 2187
	public PlayHavenController playHavenController;

	// Token: 0x0400088C RID: 2188
	public bool CanSave;

	// Token: 0x0400088D RID: 2189
	public volatile bool needsReloadErrorDialog;

	// Token: 0x0400088E RID: 2190
	public volatile bool needsNetworkDownErrorDialog;

	// Token: 0x0400088F RID: 2191
	public volatile bool tutorialLocked;

	// Token: 0x04000890 RID: 2192
	public Dictionary<string, object> gameState;

	// Token: 0x04000891 RID: 2193
	private List<Action<Game>> sessionStateChangeObservers;

	// Token: 0x04000892 RID: 2194
	private string gameFile;

	// Token: 0x04000893 RID: 2195
	private volatile bool needsReload;

	// Token: 0x04000894 RID: 2196
	private volatile bool loadFriendGame;

	// Token: 0x04000895 RID: 2197
	private volatile bool pendingReload;

	// Token: 0x04000896 RID: 2198
	private Action[] loadSimulationActions;

	// Token: 0x04000897 RID: 2199
	private IEnumerator loadSimulationActionsEnumerator;

	// Token: 0x04000898 RID: 2200
	private IEnumerator loadExpansionSlotObjectsEnumerator;

	// Token: 0x04000899 RID: 2201
	private float m_fLocalSaveTimer;

	// Token: 0x0400089A RID: 2202
	private bool m_bNeedsLocalSave;

	// Token: 0x02000173 RID: 371
	public class GameSoaringResponder : SoaringDelegate
	{
		// Token: 0x06000CD1 RID: 3281 RVA: 0x0004E0BC File Offset: 0x0004C2BC
		public override void OnRequestingSessionData(bool success, SoaringError error, SoaringArray sessions, SoaringDictionary raw_data, SoaringContext context)
		{
			if (context.ContextResponder != null)
			{
				context.addValue(success, "status");
				if (error != null)
				{
					context.addValue(error, "error_message");
				}
				if (sessions != null)
				{
					context.addValue(sessions, "custom");
				}
				context.ContextResponder(context);
			}
			else
			{
				SoaringDebug.Log("Game: OnRequestingSessionData: No Return Context");
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0004E12C File Offset: 0x0004C32C
		public override void OnSavingSessionData(bool success, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
			if (context != null && context.ContextResponder != null)
			{
				context.addValue(success, "status");
				if (error != null)
				{
					context.addValue(error, "error_message");
				}
				if (data != null)
				{
					context.addValue(data, "custom");
				}
				context.ContextResponder(context);
			}
			SoaringDebug.Log("Game: OnSavingSessionData: " + success);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0004E1A8 File Offset: 0x0004C3A8
		public override void OnComponentFinished(bool success, string module, SoaringError error, SoaringDictionary data, SoaringContext context)
		{
			if (context != null)
			{
				context.addValue(success, "status");
				context.addValue(module, "module");
				if (error != null)
				{
					context.addValue(error, "error_message");
				}
				if (data != null)
				{
					context.addValue(data, "custom");
				}
				context.ContextResponder(context);
			}
			SoaringDebug.Log("Game: OnComponentFinished: " + success);
		}
	}

	// Token: 0x02000174 RID: 372
	private class LoadSlotObjectData
	{
		// Token: 0x06000CD4 RID: 3284 RVA: 0x0004E22C File Offset: 0x0004C42C
		public LoadSlotObjectData(TerrainSlot s, TerrainSlotObject o, ulong u, Game.LoadSlotObject l)
		{
			this.expansion = s;
			this.slotObject = o;
			this.utcNow = u;
			this.loader = l;
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0004E254 File Offset: 0x0004C454
		public void Load()
		{
			this.loader(this.expansion, this.slotObject, this.utcNow);
		}

		// Token: 0x0400089D RID: 2205
		public TerrainSlot expansion;

		// Token: 0x0400089E RID: 2206
		public TerrainSlotObject slotObject;

		// Token: 0x0400089F RID: 2207
		public ulong utcNow;

		// Token: 0x040008A0 RID: 2208
		public Game.LoadSlotObject loader;
	}

	// Token: 0x020004A2 RID: 1186
	// (Invoke) Token: 0x060024EB RID: 9451
	public delegate void GamestateWriter(Dictionary<string, object> gameState);

	// Token: 0x020004A3 RID: 1187
	// (Invoke) Token: 0x060024EF RID: 9455
	private delegate void ObjectLoaderFn(Dictionary<string, object> data, ulong utcNow);

	// Token: 0x020004A4 RID: 1188
	// (Invoke) Token: 0x060024F3 RID: 9459
	private delegate void LoadSlotObject(TerrainSlot expansion, TerrainSlotObject slotObject, ulong utcNow);
}
