using System;
using System.Collections.Generic;

// Token: 0x02000194 RID: 404
public class SimulatedQuerier : Matcher
{
	// Token: 0x06000D8B RID: 3467 RVA: 0x00052A60 File Offset: 0x00050C60
	public static SimulatedQuerier FromDict(Dictionary<string, object> dict)
	{
		SimulatedQuerier simulatedQuerier = new SimulatedQuerier();
		simulatedQuerier.simFinder = null;
		simulatedQuerier.taskFinder = null;
		simulatedQuerier.m_bIncludeInventory = false;
		if (dict.ContainsKey("include_inventory"))
		{
			simulatedQuerier.m_bIncludeInventory = (TFUtils.LoadInt(dict, "include_inventory") == 1);
		}
		if (dict.ContainsKey("complete_buildings_only"))
		{
			simulatedQuerier.buildingComplete = TFUtils.LoadBool(dict, "complete_buildings_only");
		}
		else
		{
			simulatedQuerier.buildingComplete = false;
		}
		if (dict.ContainsKey("simulated_guid"))
		{
			simulatedQuerier.simFinder = new SimulatedQuerier.InstanceFinder(new Identity(TFUtils.LoadString(dict, "simulated_guid")), simulatedQuerier.buildingComplete);
		}
		else if (dict.ContainsKey("simulated_id"))
		{
			simulatedQuerier.simFinder = new SimulatedQuerier.TypeFinder(TFUtils.LoadInt(dict, "simulated_id"), simulatedQuerier.buildingComplete);
		}
		else if (dict.ContainsKey("task_id"))
		{
			simulatedQuerier.taskFinder = new SimulatedQuerier.TaskFinder(TFUtils.LoadInt(dict, "task_id"));
		}
		else if (dict.ContainsKey("resource_id"))
		{
			simulatedQuerier.resourceFinder = new SimulatedQuerier.ResourceFinder(TFUtils.LoadInt(dict, "resource_id"));
		}
		else
		{
			TFUtils.ErrorLog(string.Format("Not enough information to find a simulated to query. Requires either '{0}' or '{1}' or '{2}'\nData={3}", new object[]
			{
				"simulated_guid",
				"simulated_id",
				"task_id",
				TFUtils.DebugDictToString(dict)
			}));
		}
		if (dict.ContainsKey("instance_count"))
		{
			simulatedQuerier.RegisterProperty("instance_count", dict, new MatchableProperty.MatchFn(simulatedQuerier.MatchCount));
		}
		if (dict.ContainsKey("task_count"))
		{
			simulatedQuerier.RegisterProperty("task_count", dict, new MatchableProperty.MatchFn(simulatedQuerier.MatchTaskCount));
		}
		else if (simulatedQuerier.taskFinder != null)
		{
			simulatedQuerier.RegisterProperty("task_count", new Dictionary<string, object>
			{
				{
					"task_count",
					1
				}
			}, new MatchableProperty.MatchFn(simulatedQuerier.MatchTaskCount));
		}
		if (dict.ContainsKey("ready_to_collect"))
		{
			simulatedQuerier.collectReady = (TFUtils.LoadInt(dict, "ready_to_collect") == 1);
		}
		else
		{
			simulatedQuerier.collectReady = false;
		}
		if (dict.ContainsKey("craft_reward"))
		{
			simulatedQuerier.RegisterProperty("craft_reward", dict, new MatchableProperty.MatchFn(simulatedQuerier.MatchCraftReward));
			simulatedQuerier.resourceSubMatcher = ResourceMatcher.FromDict(TFUtils.LoadDict(dict, "craft_reward"));
		}
		if (dict.ContainsKey("costume_id"))
		{
			simulatedQuerier.RegisterProperty("costume_id", dict, new MatchableProperty.MatchFn(simulatedQuerier.MatchCostume));
		}
		return simulatedQuerier;
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x00052CFC File Offset: 0x00050EFC
	public override uint MatchAmount(Game game, Dictionary<string, object> data)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (this.taskFinder != null)
		{
			dictionary["task_count"] = 0;
			TaskData taskData = this.taskFinder.FindTaskToQuery(game);
			if (taskData != null)
			{
				Task activeTask = game.taskManager.GetActiveTask(taskData.m_nDID);
				if (activeTask != null)
				{
					if (this.collectReady)
					{
						if (activeTask.GetTimeLeft() <= 0UL)
						{
							dictionary["task_count"] = 1;
						}
					}
					else
					{
						dictionary["task_count"] = 1;
					}
				}
			}
			return base.MatchAmount(game, dictionary);
		}
		if (this.resourceFinder != null)
		{
			dictionary["instance_count"] = 0;
			Resource resource = this.resourceFinder.FindResourceToQuery(game);
			if (resource != null)
			{
				dictionary["instance_count"] = resource.Amount;
			}
			return base.MatchAmount(game, dictionary);
		}
		List<Simulated> list = this.simFinder.FindCandidatesToQuery(game);
		dictionary["instance_count"] = list.Count;
		int num = 0;
		foreach (Simulated simulated in list)
		{
			num += game.taskManager.GetActiveTasksForSimulated(simulated.entity.DefinitionId, null, true).Count;
		}
		dictionary["task_count"] = num;
		uint num2 = 0U;
		foreach (Simulated value in list)
		{
			dictionary["simulated_candidate"] = value;
			num2 = base.MatchAmount(game, dictionary);
			if (num2 > 0U)
			{
				break;
			}
		}
		if (num2 <= 0U)
		{
			int num3 = list.Count;
			if (num3 <= 0)
			{
				num3 = this.simFinder.FindCandidates(game).Count;
			}
			if (this.m_bIncludeInventory)
			{
				num3 += this.simFinder.GetNumInventoryItems(game);
			}
			dictionary["simulated_candidate"] = null;
			dictionary["instance_count"] = num3;
			num2 = base.MatchAmount(game, dictionary);
		}
		return num2;
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x00052F80 File Offset: 0x00051180
	public override string DescribeSubject(Game game)
	{
		return string.Empty;
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x00052F88 File Offset: 0x00051188
	private uint MatchCount(MatchableProperty idProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		int amount = (int)candidateWrapper["instance_count"];
		return base.CompareOperandRangesToAmount(idProperty.Target, amount);
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x00052FB4 File Offset: 0x000511B4
	private uint MatchTaskCount(MatchableProperty idProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		int amount = (int)candidateWrapper["task_count"];
		return base.CompareOperandRangesToAmount(idProperty.Target, amount);
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00052FE0 File Offset: 0x000511E0
	private uint MatchCostume(MatchableProperty idProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		Simulated simulated = (Simulated)candidateWrapper["simulated_candidate"];
		if (simulated == null)
		{
			return 0U;
		}
		if (!simulated.HasEntity<ResidentEntity>())
		{
			return 0U;
		}
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		if (entity.CostumeDID != null)
		{
			return base.CompareOperandRangesToAmount(idProperty.Target, entity.CostumeDID.Value);
		}
		return 0U;
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x0005304C File Offset: 0x0005124C
	private uint MatchCraftReward(MatchableProperty craftRewardProperty, Dictionary<string, object> candidateWrapper, Game game)
	{
		Simulated simulated = (Simulated)candidateWrapper["simulated_candidate"];
		if (simulated == null)
		{
			return 0U;
		}
		TFUtils.Assert((simulated.entity.AllTypes & EntityType.BUILDING) != EntityType.INVALID, "Was expecting to be examining a building entity! Instead found: " + simulated.entity.AllTypes);
		BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
		if (entity.CraftRewards == null)
		{
			return 0U;
		}
		Dictionary<string, object> data = new Dictionary<string, object>();
		entity.CraftRewards.AddDataToTrigger(ref data);
		return this.resourceSubMatcher.MatchAmount(game, data);
	}

	// Token: 0x040008FB RID: 2299
	public const string INSTANCE_ID = "simulated_guid";

	// Token: 0x040008FC RID: 2300
	public const string DEFINITION_ID = "simulated_id";

	// Token: 0x040008FD RID: 2301
	public const string INCLUDE_INVENTORY = "include_inventory";

	// Token: 0x040008FE RID: 2302
	public const string COSTUME_ID = "costume_id";

	// Token: 0x040008FF RID: 2303
	public const string TASK_ID = "task_id";

	// Token: 0x04000900 RID: 2304
	public const string INSTANCE_COUNT = "instance_count";

	// Token: 0x04000901 RID: 2305
	public const string TASK_COUNT = "task_count";

	// Token: 0x04000902 RID: 2306
	public const string CRAFT_REWARD = "craft_reward";

	// Token: 0x04000903 RID: 2307
	public const string COLLECT_READY = "ready_to_collect";

	// Token: 0x04000904 RID: 2308
	public const string RESOURCE_ID = "resource_id";

	// Token: 0x04000905 RID: 2309
	public const string BUILDING_COMPLETE = "complete_buildings_only";

	// Token: 0x04000906 RID: 2310
	private const string SIMULATED_CANDIDATE = "simulated_candidate";

	// Token: 0x04000907 RID: 2311
	private SimulatedQuerier.QuerySimulatedFinder simFinder;

	// Token: 0x04000908 RID: 2312
	private SimulatedQuerier.TaskFinder taskFinder;

	// Token: 0x04000909 RID: 2313
	private SimulatedQuerier.ResourceFinder resourceFinder;

	// Token: 0x0400090A RID: 2314
	private ResourceMatcher resourceSubMatcher;

	// Token: 0x0400090B RID: 2315
	private bool collectReady;

	// Token: 0x0400090C RID: 2316
	private bool m_bIncludeInventory;

	// Token: 0x0400090D RID: 2317
	private bool buildingComplete;

	// Token: 0x02000195 RID: 405
	private abstract class QuerySimulatedFinder
	{
		// Token: 0x06000D93 RID: 3475
		public abstract List<Simulated> FindCandidatesToQuery(Game game);

		// Token: 0x06000D94 RID: 3476
		public abstract List<Simulated> FindCandidates(Game game);

		// Token: 0x06000D95 RID: 3477
		public abstract int GetNumInventoryItems(Game game);
	}

	// Token: 0x02000196 RID: 406
	private class InstanceFinder : SimulatedQuerier.QuerySimulatedFinder
	{
		// Token: 0x06000D96 RID: 3478 RVA: 0x000530E4 File Offset: 0x000512E4
		public InstanceFinder(Identity id, bool bOnlyCompleteBuildings)
		{
			this.id = id;
			this.m_bOnlyCompleteBuildings = bOnlyCompleteBuildings;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x000530FC File Offset: 0x000512FC
		public override List<Simulated> FindCandidatesToQuery(Game game)
		{
			List<Simulated> list = new List<Simulated>();
			Simulated simulated = game.simulation.FindSimulated(this.id);
			if (this.m_bOnlyCompleteBuildings && simulated.HasEntity<BuildingEntity>())
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
				if (decorator.Activated == 0UL)
				{
					list.Remove(simulated);
				}
				else if (simulated != null && simulated.SimulatedQueryable)
				{
					list.Add(simulated);
				}
			}
			return list;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00053178 File Offset: 0x00051378
		public override List<Simulated> FindCandidates(Game game)
		{
			List<Simulated> list = new List<Simulated>();
			Simulated simulated = game.simulation.FindSimulated(this.id);
			if (this.m_bOnlyCompleteBuildings && simulated.HasEntity<BuildingEntity>())
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
				if (decorator.Activated == 0UL)
				{
					list.Remove(simulated);
				}
				else if (simulated != null && simulated.SimulatedQueryable)
				{
					list.Add(simulated);
				}
			}
			return list;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x000531F4 File Offset: 0x000513F4
		public override int GetNumInventoryItems(Game game)
		{
			return game.inventory.GetNumItems(this.id);
		}

		// Token: 0x0400090E RID: 2318
		private Identity id;

		// Token: 0x0400090F RID: 2319
		private bool m_bOnlyCompleteBuildings;
	}

	// Token: 0x02000197 RID: 407
	private class TypeFinder : SimulatedQuerier.QuerySimulatedFinder
	{
		// Token: 0x06000D9A RID: 3482 RVA: 0x00053208 File Offset: 0x00051408
		public TypeFinder(int definitionId, bool bOnlyCompleteBuildings)
		{
			this.definitionId = definitionId;
			this.m_bOnlyCompleteBuildings = bOnlyCompleteBuildings;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00053220 File Offset: 0x00051420
		public override List<Simulated> FindCandidatesToQuery(Game game)
		{
			List<Simulated> list = game.simulation.FindAllSimulateds(this.definitionId, null);
			if (this.m_bOnlyCompleteBuildings)
			{
				int num = list.Count;
				for (int i = 0; i < num; i++)
				{
					if (list[i].HasEntity<BuildingEntity>())
					{
						BuildingEntity entity = list[i].GetEntity<BuildingEntity>();
						ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
						if (decorator.Activated == 0UL)
						{
							list.RemoveAt(i);
							i--;
							num--;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x000532B0 File Offset: 0x000514B0
		public override List<Simulated> FindCandidates(Game game)
		{
			List<Simulated> list = game.simulation.FindAllSimulateds(this.definitionId, null);
			list = list.FindAll((Simulated candidate) => candidate.SimulatedQueryable);
			if (this.m_bOnlyCompleteBuildings)
			{
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					if (list[i].HasEntity<BuildingEntity>())
					{
						BuildingEntity entity = list[i].GetEntity<BuildingEntity>();
						ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
						if (decorator.Activated == 0UL)
						{
							list.RemoveAt(i);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0005335C File Offset: 0x0005155C
		public override int GetNumInventoryItems(Game game)
		{
			return game.inventory.GetNumItems(new int?(this.definitionId));
		}

		// Token: 0x04000910 RID: 2320
		private int definitionId;

		// Token: 0x04000911 RID: 2321
		private bool m_bOnlyCompleteBuildings;
	}

	// Token: 0x02000198 RID: 408
	private class TaskFinder
	{
		// Token: 0x06000D9F RID: 3487 RVA: 0x0005337C File Offset: 0x0005157C
		public TaskFinder(int definitionId)
		{
			this.definitionId = definitionId;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0005338C File Offset: 0x0005158C
		public TaskData FindTaskToQuery(Game game)
		{
			return game.taskManager.GetTaskData(this.definitionId, false);
		}

		// Token: 0x04000913 RID: 2323
		private int definitionId;
	}

	// Token: 0x02000199 RID: 409
	private class ResourceFinder
	{
		// Token: 0x06000DA1 RID: 3489 RVA: 0x000533A0 File Offset: 0x000515A0
		public ResourceFinder(int definitionId)
		{
			this.definitionId = definitionId;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x000533B0 File Offset: 0x000515B0
		public Resource FindResourceToQuery(Game game)
		{
			if (game.resourceManager.Resources.ContainsKey(this.definitionId))
			{
				return game.resourceManager.Resources[this.definitionId];
			}
			return null;
		}

		// Token: 0x04000914 RID: 2324
		private int definitionId;
	}
}
