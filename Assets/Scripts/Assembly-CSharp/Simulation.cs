using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class Simulation
{
	// Token: 0x0600176B RID: 5995 RVA: 0x0009B7F0 File Offset: 0x000999F0
	public Simulation(Simulation.ModifyGameStateFunction modifyGameState, Simulation.ModifyGameStateSimulatedFunction modifyGameStateSimulated, Action<Simulated> rushSimulated, Simulation.RecordBufferAction recordAction, Game game, EntityManager entityManager, TriggerRouter triggerRouter, ResourceManager resourceManager, ItemDropManager dropManager, SoundEffectManager soundEffectManager, ResourceCalculatorManager resourceCalculatorManager, CraftingManager craftManager, MovieManager movieManager, FeatureManager featureManager, Catalog catalog, RewardCap rewardCap, Camera camera, Terrain terrain, int depth, SBAnalytics analytics, SBGUIScreen scratchScreen, EnclosureManager enclosureManager)
	{
		this.game = game;
		this.analytics = analytics;
		this.soundEffectManager = soundEffectManager;
		this.triggerRouter = triggerRouter;
		this.resourceManager = resourceManager;
		this.resourceCalculatorManager = resourceCalculatorManager;
		this.craftManager = craftManager;
		this.movieManager = movieManager;
		this.dropManager = dropManager;
		this.featureManager = featureManager;
		this.catalog = catalog;
		this.ModifyGameState = modifyGameState;
		this.ModifyGameStateSimulated = modifyGameStateSimulated;
		this.RecordAction = recordAction;
		this.rewardCap = rewardCap;
		this.workerSpawners = new Dictionary<Identity, Simulated>();
		this.waypointDictionary = new Dictionary<string, Waypoint>();
		this.waypointList = new List<Waypoint>();
		this.waypointIndexer = new Simulation.WaypointIndexer(this.waypointDictionary, this.waypointList);
		this.entityManager = entityManager;
		this.terrain = terrain;
		this.camera = camera;
		this.simulateds = new List<Simulated>();
		this.simulatedsCopy = new List<Simulated>();
		this.scene = new Scene(this.terrain, depth);
		this.router = new CommandRouter();
		this.particleSystemManager = new ParticleSystemManager();
		this.enclosureManager = enclosureManager;
		this.rewardDropManager = new RewardDropManager();
		this.scratchScreen = scratchScreen;
		this.bounceInterpolator = new SplineInterpolator();
		this.bounceInterpolator.LoadData("bounce.json");
		this.bounceStartInterpolator = new SplineInterpolator();
		this.bounceStartInterpolator.LoadData("bounce_start.json");
		this.bounceEndInterpolator = new SplineInterpolator();
		this.bounceEndInterpolator.LoadData("bounce_end.json");
		this.whitelistedIdentities = new Dictionary<Identity, int>();
		this.whitelistedDefinitions = new Dictionary<int, int>();
		this.whitelistedExpansions = new Dictionary<int, int>();
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x0600176C RID: 5996 RVA: 0x0009B9A0 File Offset: 0x00099BA0
	public double Time
	{
		get
		{
			return this.timeSimulation;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x0600176D RID: 5997 RVA: 0x0009B9A8 File Offset: 0x00099BA8
	public float TimeStep
	{
		get
		{
			return 0.1f;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x0600176E RID: 5998 RVA: 0x0009B9B0 File Offset: 0x00099BB0
	public float Interpolant
	{
		get
		{
			return this.interpolant;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x0600176F RID: 5999 RVA: 0x0009B9B8 File Offset: 0x00099BB8
	public CommandRouter Router
	{
		get
		{
			return this.router;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06001770 RID: 6000 RVA: 0x0009B9C0 File Offset: 0x00099BC0
	public Terrain Terrain
	{
		get
		{
			return this.terrain;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06001771 RID: 6001 RVA: 0x0009B9C8 File Offset: 0x00099BC8
	public Scene Scene
	{
		get
		{
			return this.scene;
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06001772 RID: 6002 RVA: 0x0009B9D0 File Offset: 0x00099BD0
	public Simulation.WaypointIndexer Waypoint
	{
		get
		{
			return this.waypointIndexer;
		}
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06001773 RID: 6003 RVA: 0x0009B9D8 File Offset: 0x00099BD8
	public EntityManager EntityManager
	{
		get
		{
			return this.entityManager;
		}
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06001774 RID: 6004 RVA: 0x0009B9E0 File Offset: 0x00099BE0
	public ItemDropManager DropManager
	{
		get
		{
			return this.dropManager;
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06001775 RID: 6005 RVA: 0x0009B9E8 File Offset: 0x00099BE8
	public Camera TheCamera
	{
		get
		{
			return this.camera;
		}
	}

	// Token: 0x06001776 RID: 6006 RVA: 0x0009B9F0 File Offset: 0x00099BF0
	public Simulated CreateSimulated(Entity entity, Simulated.StateAction initialState, Vector2 position)
	{
		Simulated simulated = new Simulated(this, entity, position);
		this.AddSimulated(simulated);
		simulated.DisplayState("default");
		simulated.EnterInitialState(initialState, this);
		return simulated;
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x0009BA24 File Offset: 0x00099C24
	public Simulated CreateSimulated(string blueprint, Vector2 position)
	{
		return this.SetSimulated(new Simulated(this, this.entityManager.Create(blueprint, true), position));
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x0009BA40 File Offset: 0x00099C40
	public Simulated CreateSimulated(EntityType types, int did, Vector2 position)
	{
		return this.SetSimulated(new Simulated(this, this.entityManager.Create(types, did, true), position));
	}

	// Token: 0x06001779 RID: 6009 RVA: 0x0009BA60 File Offset: 0x00099C60
	private Simulated SetSimulated(Simulated simulated)
	{
		this.AddSimulated(simulated);
		simulated.EnterInitialState((Simulated.StateAction)simulated.Invariable["action"], this);
		return simulated;
	}

	// Token: 0x0600177A RID: 6010 RVA: 0x0009BA94 File Offset: 0x00099C94
	public void AddSimulated(Simulated simulated)
	{
		if (simulated.WorkerSpawner)
		{
			this.AddWorkerSpawner(simulated);
		}
		this.simulateds.Add(simulated);
		if (simulated.IsWaypoint)
		{
			this.AddWaypoint(simulated);
		}
		this.scene.Add(simulated);
		this.router.Register(simulated);
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x0009BAEC File Offset: 0x00099CEC
	public void RemoveSimulated(Simulated simulated)
	{
		if (this.waypointDictionary.ContainsKey(simulated.Id.Describe()))
		{
			this.RemoveWaypoint(simulated);
		}
		this.simulateds.Remove(simulated);
		this.router.Unregister(simulated);
		this.scene.Remove(simulated);
		simulated.Destroy(this);
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x0009BB48 File Offset: 0x00099D48
	public void SendPendingCommands()
	{
		foreach (Simulated simulated in this.simulateds)
		{
			simulated.SendPendingCommands(this);
		}
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x0009BBB0 File Offset: 0x00099DB0
	public Simulated FindSimulated(Identity id)
	{
		if (id.Equals(Identity.Null()))
		{
			return null;
		}
		int count = this.simulateds.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.identityComperer.Equals(this.simulateds[i].Id, id))
			{
				return this.simulateds[i];
			}
		}
		return null;
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x0009BC20 File Offset: 0x00099E20
	public Simulated FindSimulated(int? did)
	{
		if (did == null)
		{
			return null;
		}
		foreach (Simulated simulated in this.simulateds)
		{
			if (simulated.entity.DefinitionId == did.Value)
			{
				return simulated;
			}
		}
		return null;
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x0009BCB0 File Offset: 0x00099EB0
	public Simulated FindSimulated(int? did, EntityType type)
	{
		if (did == null)
		{
			return null;
		}
		foreach (Simulated simulated in this.simulateds)
		{
			if (simulated.entity.DefinitionId == did.Value && simulated.entity.Type == type)
			{
				return simulated;
			}
		}
		return null;
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x0009BD50 File Offset: 0x00099F50
	public List<Simulated> FindAllSimulateds(int did, EntityType? type = null)
	{
		if (type != null)
		{
			return this.simulateds.FindAll((Simulated sim) => sim.entity.DefinitionId == did && sim.entity.Type == type.GetValueOrDefault() && type != null);
		}
		return this.simulateds.FindAll((Simulated sim) => sim.entity.DefinitionId == did);
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x0009BDB0 File Offset: 0x00099FB0
	public IEnumerable<Simulated> GetSimulateds()
	{
		return this.simulateds;
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x0009BDB8 File Offset: 0x00099FB8
	public List<Simulated> GetSimulatedRaw()
	{
		return this.simulateds;
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x0009BDC0 File Offset: 0x00099FC0
	public Simulated SpawnWorker(Simulated simulated)
	{
		Simulated simulated2 = this.CreateSimulated("worker_0", Vector2.zero);
		Simulated closestWorkerSpawner = this.GetClosestWorkerSpawner(simulated.Position);
		if (closestWorkerSpawner != null)
		{
			simulated2.Warp(closestWorkerSpawner.PointOfInterest, null);
		}
		else
		{
			simulated2.Warp(simulated.PointOfInterest, null);
			this.Router.Send(SpawnCommand.Create(Identity.Null(), simulated.Id, "worker_0"));
		}
		simulated2.Visible = true;
		return simulated2;
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x0009BE38 File Offset: 0x0009A038
	public Waypoint GetRandomWaypoint()
	{
		int count = this.waypointList.Count;
		if (count == 0)
		{
			return null;
		}
		List<Waypoint> list = new List<Waypoint>();
		for (int i = 0; i < count; i++)
		{
			if (this.terrain.CheckIsPurchasedArea(this.waypointList[i].Position))
			{
				list.Add(this.waypointList[i]);
			}
		}
		count = list.Count;
		if (count == 0)
		{
			return null;
		}
		return list[UnityEngine.Random.Range(0, count)];
	}

	// Token: 0x06001785 RID: 6021 RVA: 0x0009BEC0 File Offset: 0x0009A0C0
	public void Clear()
	{
		foreach (Simulated simulated in this.simulateds)
		{
			simulated.Destroy(this);
		}
		SwarmManager.Instance.Cleanup();
		this.terrain.Destroy();
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x0009BF3C File Offset: 0x0009A13C
	public Vector2 ScreenPositionFromWorldPosition(Vector3 worldPosition)
	{
		Vector3 vector = this.camera.WorldToScreenPoint(worldPosition);
		return new Vector2(vector.x, SBGUI.GetScreenHeight() - vector.y);
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x0009BF70 File Offset: 0x0009A170
	public void OnUpdate(Session session)
	{
		float time = UnityEngine.Time.time;
		this.interpolant = this.timeAccum / 0.1f;
		this.Animate();
		float num = time - this.timeLast;
		this.timeAccum += num;
		this.timeLast = time;
		while (this.timeAccum >= 0.1f)
		{
			this.Simulate(session);
			this.particleSystemManager.OnUpdate();
			this.enclosureManager.OnUpdate(this);
			SwarmManager.Instance.OnUpdate(this, 0.1f);
			this.timeSimulation += 0.10000000149011612;
			this.timeAccum -= 0.1f;
		}
		if (!SBSettings.UseActionFile)
		{
			this.game.LocalSaveCheck(num);
		}
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x0009C03C File Offset: 0x0009A23C
	public void OnUpdateVisitParkState(Session session)
	{
		float time = UnityEngine.Time.time;
		this.timeLast = time - 0.1f;
		this.interpolant = this.timeAccum / 0.1f;
		this.Animate();
		float num = time - this.timeLast;
		this.timeAccum += num;
		this.timeLast = time;
		while (this.timeAccum >= 0.1f)
		{
			this.Simulate(session);
			this.particleSystemManager.OnUpdate();
			this.enclosureManager.OnUpdate(this);
			SwarmManager.Instance.OnUpdate(this, 0.1f);
			this.timeSimulation += 0.10000000149011612;
			this.timeAccum -= 0.1f;
		}
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x0009C100 File Offset: 0x0009A300
	public TerrainPathing CreatePathing(Vector2 start, Vector2 goal)
	{
		return new TerrainPathing(this.terrain, start, goal);
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x0009C110 File Offset: 0x0009A310
	public void ResetAllAffectedPaths(AlignedBox box)
	{
		GridPosition gridPosition = this.terrain.ComputeGridPosition(new Vector2(box.xmin, box.ymin));
		GridPosition gridPosition2 = this.terrain.ComputeGridPosition(new Vector2(box.xmax, box.ymax));
		gridPosition.MakeValid(this.terrain.GridHeight - 1, this.terrain.GridWidth - 1);
		gridPosition2.MakeValid(this.terrain.GridHeight - 1, this.terrain.GridWidth - 1);
		foreach (Simulated simulated in this.simulateds)
		{
			this.HandleIfShouldRecalculatePath(simulated, gridPosition, gridPosition2);
		}
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x0009C1F4 File Offset: 0x0009A3F4
	public void HandleIfShouldRecalculatePath(Simulated simulated, GridPosition min, GridPosition max)
	{
		object obj = null;
		Path<GridPosition> path = null;
		simulated.Variable.TryGetValue("pathing", out obj);
		if (obj == null)
		{
			if (simulated.Variable.TryGetValue("path", out obj))
			{
				path = (Path<GridPosition>)obj;
			}
			if (path == null || path.Done())
			{
				return;
			}
			bool flag = true;
			bool flag2 = false;
			foreach (GridPosition gridPosition in path)
			{
				if (flag)
				{
					if (gridPosition == path.Current)
					{
						flag = false;
					}
				}
				else if (gridPosition.Within(min, max))
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				return;
			}
		}
		GridPosition gridPosition2 = null;
		if (simulated.Variable.TryGetValue("pathGoal", out obj))
		{
			gridPosition2 = (obj as GridPosition);
		}
		if (gridPosition2 != null)
		{
			Vector2 goal = this.Terrain.ComputeWorldPosition(gridPosition2);
			simulated.Variable["pathing"] = this.CreatePathing(simulated.Position, goal);
			simulated.ClearPathInfo();
		}
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x0009C340 File Offset: 0x0009A540
	public Simulation.Placement.RESULT PlacementQuery(Simulated selected, ref List<Simulated> collisions, bool debrisOnly = false)
	{
		return this.PlacementQuery(selected.Box, ref collisions, selected.Id, debrisOnly);
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x0009C364 File Offset: 0x0009A564
	public Simulation.Placement.RESULT PlacementQuery(AlignedBox box, ref List<Simulated> collisions, Identity id = null, bool debrisOnly = false)
	{
		if (this.terrain.FootprintGuide != null && (this.terrain.FootprintGuide.xmin != box.xmin || this.terrain.FootprintGuide.ymin != box.ymin))
		{
			TFUtils.WarningLog("Terrain Debugging: PlacementQuery: Invalid Footprint Area");
			return Simulation.Placement.RESULT.INVALID;
		}
		if (!this.terrain.CheckIsPurchasedArea(box))
		{
			TFUtils.WarningLog("Terrain Debugging: PlacementQuery: Area Is Not Purchased");
			return Simulation.Placement.RESULT.INVALID;
		}
		this.scene.FindPlacementBlockers(box, ref collisions);
		if (debrisOnly)
		{
			foreach (Simulated simulated in collisions)
			{
				if (simulated.entity is DebrisEntity)
				{
					return Simulation.Placement.RESULT.INVALID;
				}
			}
			return Simulation.Placement.RESULT.VALID;
		}
		Simulated ourself = null;
		if (id != null)
		{
			ourself = this.FindSimulated(id);
		}
		if (ourself != null && ourself.HasEntity<StructureDecorator>() && ourself.GetEntity<StructureDecorator>().ShareableSpace)
		{
			collisions = collisions.FindAll((Simulated x) => !x.HasEntity<StructureDecorator>() || !x.GetEntity<StructureDecorator>().ShareableSpace);
		}
		else if (ourself != null && ourself.HasEntity<StructureDecorator>() && ourself.GetEntity<StructureDecorator>().ShareableSpaceSnap)
		{
			collisions = collisions.FindAll((Simulated x) => !x.HasEntity<StructureDecorator>() || !x.GetEntity<StructureDecorator>().ShareableSpaceSnap || x.Flip == ourself.Flip);
		}
		if (collisions.Count > 1 || (collisions.Count == 1 && collisions[0].entity.Id != id))
		{
			return Simulation.Placement.RESULT.INVALID;
		}
		return Simulation.Placement.RESULT.VALID;
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x0009C558 File Offset: 0x0009A758
	public Simulation.Placement.RESULT PlacementQuery(AlignedBox box, Identity id = null, bool debrisOnly = false)
	{
		List<Simulated> list = new List<Simulated>();
		return this.PlacementQuery(box, ref list, id, debrisOnly);
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x0009C578 File Offset: 0x0009A778
	public Simulation.Placement.RESULT PlacementQuery(Simulated selected, bool debrisOnly = false)
	{
		List<Simulated> list = new List<Simulated>();
		return this.PlacementQuery(selected.Box, ref list, selected.Id, debrisOnly);
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x0009C5A0 File Offset: 0x0009A7A0
	private void AddWorkerSpawner(Simulated workerSpawner)
	{
		if (!this.workerSpawners.ContainsKey(workerSpawner.Id))
		{
			this.workerSpawners.Add(workerSpawner.Id, workerSpawner);
		}
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x0009C5D8 File Offset: 0x0009A7D8
	public void TryWorkerSpawnerCleanup(Identity id)
	{
		if (this.workerSpawners.ContainsKey(id))
		{
			this.workerSpawners.Remove(id);
		}
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x0009C5F8 File Offset: 0x0009A7F8
	public Simulated GetClosestWorkerSpawner(Vector2 location)
	{
		Simulated result = null;
		float? num = null;
		foreach (Identity key in this.workerSpawners.Keys)
		{
			float num2 = Vector2.Distance(location, this.workerSpawners[key].PointOfInterest);
			if (num == null || num2 < num.Value)
			{
				result = this.workerSpawners[key];
				num = new float?(num2);
			}
		}
		return result;
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x0009C6B8 File Offset: 0x0009A8B8
	private void Animate()
	{
		Simulated.SimulatedFlags simulatedFlags = Simulated.SimulatedFlags.MOBILE | Simulated.SimulatedFlags.FORCE_ANIMATE_ACTION | Simulated.SimulatedFlags.FORCE_ANIMATE_FOOTPRINT | Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE | Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_START | Simulated.SimulatedFlags.FORCE_ANIMATE_BOUNCE_END;
		foreach (Simulated simulated in this.simulateds)
		{
			if ((simulated.simFlags & Simulated.SimulatedFlags.FIRST_ANIMATE) == (Simulated.SimulatedFlags)0)
			{
				if ((simulated.simFlags & simulatedFlags) != (Simulated.SimulatedFlags)0)
				{
					simulated.Animate(this);
				}
				IDisplayController displayController = simulated.DisplayController;
				DisplayControllerFlags flags = displayController.Flags;
				if ((flags & (DisplayControllerFlags.SWITCHED_STATE | DisplayControllerFlags.NEED_UPDATE)) != (DisplayControllerFlags)0)
				{
					displayController.OnUpdate(this.camera, this.particleSystemManager);
					displayController.Flags = (flags & ~DisplayControllerFlags.SWITCHED_STATE);
				}
				IDisplayController thoughtDisplayController = simulated.ThoughtDisplayController;
				if (thoughtDisplayController != null)
				{
					DisplayControllerFlags flags2 = thoughtDisplayController.Flags;
					if ((flags2 & DisplayControllerFlags.VISIBLE_AND_VALID_STATE) != (DisplayControllerFlags)0)
					{
						simulated.AnimateOtherControllers(this);
					}
				}
			}
			else
			{
				simulated.FirstAnimate(this);
				simulated.Animate(this);
				IDisplayController displayController2 = simulated.DisplayController;
				displayController2.OnUpdate(this.camera, this.particleSystemManager);
				IDisplayController thoughtDisplayController2 = simulated.ThoughtDisplayController;
				if (thoughtDisplayController2 != null)
				{
					DisplayControllerFlags flags3 = thoughtDisplayController2.Flags;
					if ((flags3 & DisplayControllerFlags.VISIBLE_AND_VALID_STATE) != (DisplayControllerFlags)0)
					{
						simulated.AnimateOtherControllers(this);
					}
				}
			}
		}
		if (Session.TheDebugManager.showHitBoxes || Session.TheDebugManager.showFootprints || Session.TheDebugManager.showExpansionBorders)
		{
			foreach (Simulated simulated2 in this.simulateds)
			{
				simulated2.AnimateDebugHitBox(this);
			}
		}
	}

	// Token: 0x06001794 RID: 6036 RVA: 0x0009C878 File Offset: 0x0009AA78
	private void Simulate(Session session)
	{
		this.router.Route();
		this.simulatedsCopy.Clear();
		this.simulatedsCopy.AddRange(this.simulateds);
		foreach (Simulated simulated in this.simulatedsCopy)
		{
			if (simulated.Simulate(this, session))
			{
				simulated.SetFootprint(this, false);
				this.RemoveSimulated(simulated);
			}
		}
		this.scene.OnUpdate(this.simulateds);
	}

	// Token: 0x06001795 RID: 6037 RVA: 0x0009C92C File Offset: 0x0009AB2C
	public void UpdateControls()
	{
		int count = this.simulateds.Count;
		for (int i = 0; i < count; i++)
		{
			this.simulateds[i].UpdateControls(this);
		}
	}

	// Token: 0x06001796 RID: 6038 RVA: 0x0009C96C File Offset: 0x0009AB6C
	public void UpdateDebugHitBoxes()
	{
		if (SBSettings.DebugDisplayControllers)
		{
			bool showHitBoxes = Session.TheDebugManager.showHitBoxes;
			foreach (Simulated simulated in this.simulateds)
			{
				simulated.DebugHitBoxesVisible = showHitBoxes;
			}
		}
	}

	// Token: 0x06001797 RID: 6039 RVA: 0x0009C9E8 File Offset: 0x0009ABE8
	public void UpdateDebugFootprints()
	{
		if (SBSettings.DebugDisplayControllers)
		{
			bool showFootprints = Session.TheDebugManager.showFootprints;
			foreach (Simulated simulated in this.simulateds)
			{
				simulated.DebugFootprintsVisible = showFootprints;
			}
		}
	}

	// Token: 0x06001798 RID: 6040 RVA: 0x0009CA64 File Offset: 0x0009AC64
	public void UpdateDebugExpansionBorders()
	{
		if (SBSettings.DebugDisplayControllers)
		{
			bool showExpansionBorders = Session.TheDebugManager.showExpansionBorders;
			if (showExpansionBorders)
			{
				this.terrain.OutlineAllExpansionSlots();
			}
			else
			{
				this.terrain.HideAllExpansionSlots();
			}
		}
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x0009CAA8 File Offset: 0x0009ACA8
	private void AddWaypoint(Simulated sim)
	{
		string key = sim.Id.Describe();
		if (!this.waypointDictionary.ContainsKey(key))
		{
			Waypoint waypoint = new Waypoint(sim);
			this.waypointDictionary.Add(key, waypoint);
			this.waypointList.Add(waypoint);
		}
		this.Waypoint[sim.Id.Describe()] = new Waypoint(sim);
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x0009CB10 File Offset: 0x0009AD10
	private void RemoveWaypoint(Simulated sim)
	{
		Waypoint item = this.waypointDictionary[sim.Id.Describe()];
		this.waypointList.Remove(item);
		this.waypointDictionary.Remove(sim.Id.Describe());
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x0009CB58 File Offset: 0x0009AD58
	public void ClearPendingTimebarsInSimulateds()
	{
		foreach (Simulated simulated in this.simulateds)
		{
			simulated.RemoveSimulateOnceAction("show_timebar");
		}
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x0009CBC4 File Offset: 0x0009ADC4
	public void ClearPendingNamebarsInSimulateds()
	{
		foreach (Simulated simulated in this.simulateds)
		{
			simulated.RemoveSimulateOnceAction("show_namebar");
		}
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x0009CC30 File Offset: 0x0009AE30
	public void WhitelistSimulated(Identity id)
	{
		if (this.whitelistedIdentities.ContainsKey(id))
		{
			Dictionary<Identity, int> dictionary2;
			Dictionary<Identity, int> dictionary = dictionary2 = this.whitelistedIdentities;
			int num = dictionary2[id];
			dictionary[id] = num + 1;
		}
		else
		{
			this.whitelistedIdentities[id] = 1;
		}
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x0009CC7C File Offset: 0x0009AE7C
	public void WhitelistSimulated(int definitionId)
	{
		if (this.whitelistedDefinitions.ContainsKey(definitionId))
		{
			Dictionary<int, int> dictionary2;
			Dictionary<int, int> dictionary = dictionary2 = this.whitelistedDefinitions;
			int num = dictionary2[definitionId];
			dictionary[definitionId] = num + 1;
		}
		else
		{
			this.whitelistedDefinitions[definitionId] = 1;
		}
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x0009CCC8 File Offset: 0x0009AEC8
	public void UnWhitelistSimulated(Identity id)
	{
		TFUtils.Assert(this.whitelistedIdentities.ContainsKey(id), "Trying to unrestrict a simulated by ID that is not tracked: " + id.Describe());
		Dictionary<Identity, int> dictionary2;
		Dictionary<Identity, int> dictionary = dictionary2 = this.whitelistedIdentities;
		int num = dictionary2[id];
		dictionary[id] = num - 1;
		if (this.whitelistedIdentities[id] <= 0)
		{
			this.whitelistedIdentities.Remove(id);
		}
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x0009CD30 File Offset: 0x0009AF30
	public void UnWhitelistSimulated(int definitionId)
	{
		TFUtils.Assert(this.whitelistedDefinitions.ContainsKey(definitionId), "Trying to unrestrict a simulated by defId that is not tracked: " + definitionId);
		Dictionary<int, int> dictionary2;
		Dictionary<int, int> dictionary = dictionary2 = this.whitelistedDefinitions;
		int num = dictionary2[definitionId];
		dictionary[definitionId] = num - 1;
		if (this.whitelistedDefinitions[definitionId] <= 0)
		{
			this.whitelistedDefinitions.Remove(definitionId);
		}
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x0009CD98 File Offset: 0x0009AF98
	public void WhitelistExpansion(int definitionId)
	{
		if (this.whitelistedExpansions.ContainsKey(definitionId))
		{
			Dictionary<int, int> dictionary2;
			Dictionary<int, int> dictionary = dictionary2 = this.whitelistedExpansions;
			int num = dictionary2[definitionId];
			dictionary[definitionId] = num + 1;
		}
		else
		{
			this.whitelistedExpansions[definitionId] = 1;
		}
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x0009CDE4 File Offset: 0x0009AFE4
	public void UnWhitelistExpansion(int definitionId)
	{
		TFUtils.Assert(this.whitelistedExpansions.ContainsKey(definitionId), "Trying to unrestrict an expansion that is not tracked: " + definitionId);
		Dictionary<int, int> dictionary2;
		Dictionary<int, int> dictionary = dictionary2 = this.whitelistedExpansions;
		int num = dictionary2[definitionId];
		dictionary[definitionId] = num - 1;
		if (this.whitelistedExpansions[definitionId] <= 0)
		{
			this.whitelistedExpansions.Remove(definitionId);
		}
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x0009CE4C File Offset: 0x0009B04C
	public void WhitelistSimulateds(ref List<Simulated> result)
	{
		if (this.whitelistedIdentities.Count == 0 && this.whitelistedDefinitions.Count == 0)
		{
			return;
		}
		List<Simulated> list = new List<Simulated>();
		foreach (Simulated simulated in result)
		{
			if (this.whitelistedIdentities.Count > 0 && this.whitelistedIdentities.ContainsKey(simulated.Id))
			{
				list.Add(simulated);
			}
			else if (this.whitelistedDefinitions.Count > 0 && this.whitelistedDefinitions.ContainsKey(simulated.entity.DefinitionId))
			{
				list.Add(simulated);
			}
		}
		result = list;
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x060017A4 RID: 6052 RVA: 0x0009CF38 File Offset: 0x0009B138
	public bool Whitelisted
	{
		get
		{
			return this.whitelistedIdentities.Count > 0 || this.whitelistedDefinitions.Count > 0;
		}
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x0009CF68 File Offset: 0x0009B168
	public bool CheckWhitelist(Simulated simulated)
	{
		return this.whitelistedIdentities.ContainsKey(simulated.entity.Id) || this.whitelistedDefinitions.ContainsKey(simulated.entity.DefinitionId);
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x0009CFAC File Offset: 0x0009B1AC
	private string PrintWhitelistedDefs()
	{
		string text = string.Empty;
		foreach (int key in this.whitelistedDefinitions.Keys)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"{",
				key.ToString(),
				":",
				this.whitelistedDefinitions[key].ToString(),
				"}"
			});
		}
		return text;
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x0009D064 File Offset: 0x0009B264
	private string PrintWhitelistedIds()
	{
		string text = string.Empty;
		foreach (Identity identity in this.whitelistedIdentities.Keys)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"{",
				identity.Describe(),
				":",
				this.whitelistedIdentities[identity].ToString(),
				"}"
			});
		}
		return text;
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x0009D11C File Offset: 0x0009B31C
	public bool CheckExpansionAllowed(int did)
	{
		return this.whitelistedExpansions.Count == 0 || this.whitelistedExpansions.ContainsKey(did);
	}

	// Token: 0x04000F80 RID: 3968
	private const float TIME_STEP = 0.1f;

	// Token: 0x04000F81 RID: 3969
	private const string WORKER = "worker_0";

	// Token: 0x04000F82 RID: 3970
	public Simulation.RecordBufferAction RecordAction;

	// Token: 0x04000F83 RID: 3971
	public Simulation.ModifyGameStateSimulatedFunction ModifyGameStateSimulated;

	// Token: 0x04000F84 RID: 3972
	public Simulation.ModifyGameStateFunction ModifyGameState;

	// Token: 0x04000F85 RID: 3973
	public Game game;

	// Token: 0x04000F86 RID: 3974
	public TriggerRouter triggerRouter;

	// Token: 0x04000F87 RID: 3975
	public ResourceManager resourceManager;

	// Token: 0x04000F88 RID: 3976
	public SBAnalytics analytics;

	// Token: 0x04000F89 RID: 3977
	public SoundEffectManager soundEffectManager;

	// Token: 0x04000F8A RID: 3978
	public ResourceCalculatorManager resourceCalculatorManager;

	// Token: 0x04000F8B RID: 3979
	public CraftingManager craftManager;

	// Token: 0x04000F8C RID: 3980
	public FeatureManager featureManager;

	// Token: 0x04000F8D RID: 3981
	public MovieManager movieManager;

	// Token: 0x04000F8E RID: 3982
	public ParticleSystemManager particleSystemManager;

	// Token: 0x04000F8F RID: 3983
	public EnclosureManager enclosureManager;

	// Token: 0x04000F90 RID: 3984
	public RewardDropManager rewardDropManager;

	// Token: 0x04000F91 RID: 3985
	public Catalog catalog;

	// Token: 0x04000F92 RID: 3986
	public SBGUIScreen scratchScreen;

	// Token: 0x04000F93 RID: 3987
	public RewardCap rewardCap;

	// Token: 0x04000F94 RID: 3988
	private Identity.Equality identityComperer = new Identity.Equality();

	// Token: 0x04000F95 RID: 3989
	private Dictionary<string, Waypoint> waypointDictionary;

	// Token: 0x04000F96 RID: 3990
	private List<Waypoint> waypointList;

	// Token: 0x04000F97 RID: 3991
	private Simulation.WaypointIndexer waypointIndexer;

	// Token: 0x04000F98 RID: 3992
	private EntityManager entityManager;

	// Token: 0x04000F99 RID: 3993
	private List<Simulated> simulateds;

	// Token: 0x04000F9A RID: 3994
	private List<Simulated> simulatedsCopy;

	// Token: 0x04000F9B RID: 3995
	private ItemDropManager dropManager;

	// Token: 0x04000F9C RID: 3996
	private Dictionary<int, int> whitelistedDefinitions;

	// Token: 0x04000F9D RID: 3997
	private Dictionary<Identity, int> whitelistedIdentities;

	// Token: 0x04000F9E RID: 3998
	private Dictionary<int, int> whitelistedExpansions;

	// Token: 0x04000F9F RID: 3999
	private Dictionary<Identity, Simulated> workerSpawners;

	// Token: 0x04000FA0 RID: 4000
	private Camera camera;

	// Token: 0x04000FA1 RID: 4001
	private Terrain terrain;

	// Token: 0x04000FA2 RID: 4002
	private Scene scene;

	// Token: 0x04000FA3 RID: 4003
	private CommandRouter router;

	// Token: 0x04000FA4 RID: 4004
	private float timeAccum;

	// Token: 0x04000FA5 RID: 4005
	private float timeLast;

	// Token: 0x04000FA6 RID: 4006
	private double timeSimulation;

	// Token: 0x04000FA7 RID: 4007
	private float interpolant;

	// Token: 0x04000FA8 RID: 4008
	public SplineInterpolator bounceInterpolator;

	// Token: 0x04000FA9 RID: 4009
	public SplineInterpolator bounceStartInterpolator;

	// Token: 0x04000FAA RID: 4010
	public SplineInterpolator bounceEndInterpolator;

	// Token: 0x02000327 RID: 807
	public class Indexer<Key, Value>
	{
		// Token: 0x060017AA RID: 6058 RVA: 0x0009D168 File Offset: 0x0009B368
		public Indexer(Dictionary<Key, Value> values)
		{
			this.values = values;
		}

		// Token: 0x1700032D RID: 813
		public Value this[Key key]
		{
			get
			{
				return this.values[key];
			}
			set
			{
				this.values[key] = value;
			}
		}

		// Token: 0x04000FAC RID: 4012
		private Dictionary<Key, Value> values;
	}

	// Token: 0x02000328 RID: 808
	public class WaypointIndexer
	{
		// Token: 0x060017AD RID: 6061 RVA: 0x0009D198 File Offset: 0x0009B398
		public WaypointIndexer(Dictionary<string, Waypoint> dictionary, List<Waypoint> list)
		{
			this.dictionary = dictionary;
			this.list = list;
		}

		// Token: 0x1700032E RID: 814
		public Waypoint this[string key]
		{
			get
			{
				return this.dictionary[key];
			}
			set
			{
				this.dictionary[key] = value;
				this.list.Remove(value);
				this.list.Add(value);
			}
		}

		// Token: 0x04000FAD RID: 4013
		private Dictionary<string, Waypoint> dictionary;

		// Token: 0x04000FAE RID: 4014
		private List<Waypoint> list;
	}

	// Token: 0x02000329 RID: 809
	public class Placement
	{
		// Token: 0x0200032A RID: 810
		public enum RESULT
		{
			// Token: 0x04000FB0 RID: 4016
			VALID,
			// Token: 0x04000FB1 RID: 4017
			INVALID,
			// Token: 0x04000FB2 RID: 4018
			CONDITIONAL
		}
	}

	// Token: 0x020004B0 RID: 1200
	// (Invoke) Token: 0x06002523 RID: 9507
	public delegate void RecordBufferAction(PersistedActionBuffer.PersistedAction action);

	// Token: 0x020004B1 RID: 1201
	// (Invoke) Token: 0x06002527 RID: 9511
	public delegate void ModifyGameStateSimulatedFunction(Simulated simulated, PersistedSimulatedAction action);

	// Token: 0x020004B2 RID: 1202
	// (Invoke) Token: 0x0600252B RID: 9515
	public delegate void ModifyGameStateFunction(PersistedTriggerableAction action);
}
