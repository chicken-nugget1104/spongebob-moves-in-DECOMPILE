using System;
using System.Collections.Generic;

// Token: 0x020000EC RID: 236
public class PurchaseResourcesAction : PersistedTriggerableAction
{
	// Token: 0x06000896 RID: 2198 RVA: 0x000374A0 File Offset: 0x000356A0
	private PurchaseResourcesAction(Identity id, Cost resources, Cost cost) : base("pr", id)
	{
		this.purchasedResources = resources;
		this.rmtCost = cost;
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x000374BC File Offset: 0x000356BC
	public PurchaseResourcesAction(Identity id, int rmtCost, Cost resources) : this(id, resources, new Cost(new Dictionary<int, int>
	{
		{
			ResourceManager.HARD_CURRENCY,
			rmtCost
		}
	}))
	{
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000898 RID: 2200 RVA: 0x000374EC File Offset: 0x000356EC
	public TriggerableMixin Triggerable
	{
		get
		{
			return this.triggerable;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000899 RID: 2201 RVA: 0x000374F4 File Offset: 0x000356F4
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x000374F8 File Offset: 0x000356F8
	public new static PurchaseResourcesAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost resources = Cost.FromObject(data["resources"]);
		Cost cost = Cost.FromObject(data["rmt_cost"]);
		return new PurchaseResourcesAction(id, resources, cost);
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00037548 File Offset: 0x00035748
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["resources"] = this.purchasedResources.ToDict();
		dictionary["rmt_cost"] = this.rmtCost.ToDict();
		return dictionary;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0003758C File Offset: 0x0003578C
	public override void Apply(Game game, ulong utcNow)
	{
		game.resourceManager.Apply(this.rmtCost, game);
		game.resourceManager.PurchaseResourcesWithHardCurrency(0, this.purchasedResources, game);
		base.Apply(game, utcNow);
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x000375BC File Offset: 0x000357BC
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(this.rmtCost, gameState);
		foreach (KeyValuePair<int, int> keyValuePair in this.purchasedResources.ResourceAmounts)
		{
			ResourceManager.AddAmountToGameState(keyValuePair.Key, keyValuePair.Value, gameState);
		}
		base.Confirm(gameState);
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x00037648 File Offset: 0x00035848
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0003764C File Offset: 0x0003584C
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x04000627 RID: 1575
	public const string PURCHASE_RESOURCES = "pr";

	// Token: 0x04000628 RID: 1576
	public Cost purchasedResources;

	// Token: 0x04000629 RID: 1577
	public Cost rmtCost;
}
