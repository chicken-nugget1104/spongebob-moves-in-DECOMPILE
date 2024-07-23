using System;
using System.Collections.Generic;

// Token: 0x020000D1 RID: 209
public class CraftStartAction : PersistedSimulatedAction
{
	// Token: 0x060007D8 RID: 2008 RVA: 0x00033514 File Offset: 0x00031714
	public CraftStartAction(Identity id, int slotId, int recipeId, ulong readyTime, Reward reward, Cost cost) : base("cs", id, typeof(CraftStartAction).ToString())
	{
		this.recipeId = recipeId;
		this.readyTime = readyTime;
		this.craftingCost = cost;
		this.slotId = slotId;
		this.reward = reward;
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00033564 File Offset: 0x00031764
	public int RecipeId
	{
		get
		{
			return this.recipeId;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060007DA RID: 2010 RVA: 0x0003356C File Offset: 0x0003176C
	public ulong ReadyTime
	{
		get
		{
			return this.readyTime;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060007DB RID: 2011 RVA: 0x00033574 File Offset: 0x00031774
	protected Cost CraftingCost
	{
		get
		{
			return this.craftingCost;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060007DC RID: 2012 RVA: 0x0003357C File Offset: 0x0003177C
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00033580 File Offset: 0x00031780
	public new static CraftStartAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "slot_id");
		int num2 = TFUtils.LoadInt(data, "recipe_id");
		ulong num3 = TFUtils.LoadUlong(data, "ready_time", 0UL);
		Reward reward = Reward.FromObject(data["reward"]);
		Cost cost = Cost.FromObject(data["cost"]);
		return new CraftStartAction(id, num, num2, num3, reward, cost);
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x000335FC File Offset: 0x000317FC
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["ready_time"] = this.readyTime;
		dictionary["cost"] = this.craftingCost.ToDict();
		dictionary["recipe_id"] = this.recipeId;
		dictionary["slot_id"] = this.slotId;
		dictionary["reward"] = this.reward.ToDict();
		return dictionary;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00033680 File Offset: 0x00031880
	public override void Apply(Game game, ulong utcNow)
	{
		if (!game.craftManager.AddCraftingInstance(new CraftingInstance(this.target, this.recipeId, this.readyTime, this.reward, this.slotId)))
		{
			TFUtils.DebugLog("invalid action: " + this.ToString());
			TFUtils.DebugLog("crafting state: " + game.craftManager.GetCraftingInstance(this.target, this.slotId).ToString());
			TFUtils.ErrorLog("we are about to apply a crafting state that does not create a valid state");
		}
		game.resourceManager.Apply(this.craftingCost, game);
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.EnterInitialState(EntityManager.BuildingActions["reflecting"], game.simulation);
		if (utcNow < this.readyTime)
		{
			game.simulation.Router.Send(CraftedCommand.Create(this.target, this.target, this.slotId), this.readyTime - utcNow);
		}
		else
		{
			game.simulation.Router.Send(CraftedCommand.Create(this.target, this.target, this.slotId));
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x000337C8 File Offset: 0x000319C8
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(this.craftingCost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["crafts"];
		list.Add(new Dictionary<string, object>
		{
			{
				"building_label",
				this.target.Describe()
			},
			{
				"ready_time",
				this.readyTime
			},
			{
				"recipe_id",
				this.RecipeId
			},
			{
				"slot_id",
				this.slotId
			},
			{
				"reward",
				this.reward.ToDict()
			}
		});
		base.Confirm(gameState);
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00033884 File Offset: 0x00031A84
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		this.reward.AddDataToTrigger(ref data);
		data["recipe_id"] = this.recipeId;
		data["notification_time"] = TFUtils.EpochToDateTime(this.readyTime);
		data["notification_label"] = "craft:" + this.target.Describe();
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x000338F8 File Offset: 0x00031AF8
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		Simulated simulated = (Simulated)data["simulated"];
		this.entityId = simulated.entity.Id;
		this.definitionId = simulated.entity.DefinitionId;
		this.simType = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005CA RID: 1482
	public const string CRAFT_START = "cs";

	// Token: 0x040005CB RID: 1483
	private Reward reward;

	// Token: 0x040005CC RID: 1484
	private int recipeId;

	// Token: 0x040005CD RID: 1485
	private Cost craftingCost;

	// Token: 0x040005CE RID: 1486
	private ulong readyTime;

	// Token: 0x040005CF RID: 1487
	private int slotId;
}
