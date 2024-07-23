using System;
using System.Collections.Generic;

// Token: 0x020000EB RID: 235
public class PurchaseCraftingSlotAction : PersistedSimulatedAction
{
	// Token: 0x06000890 RID: 2192 RVA: 0x00037304 File Offset: 0x00035504
	public PurchaseCraftingSlotAction(Identity id, Cost cost, int slots) : base("pcs", id, typeof(PurchaseCraftingSlotAction).ToString())
	{
		this.cost = cost;
		this.slots = slots;
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x06000891 RID: 2193 RVA: 0x00037330 File Offset: 0x00035530
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x00037334 File Offset: 0x00035534
	public new static PurchaseCraftingSlotAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromObject(data["cost"]);
		int num = TFUtils.LoadInt(data, "slots");
		return new PurchaseCraftingSlotAction(id, cost, num);
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x00037380 File Offset: 0x00035580
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["cost"] = this.cost.ToDict();
		dictionary["slots"] = this.slots;
		return dictionary;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x000373C4 File Offset: 0x000355C4
	public override void Apply(Game game, ulong utcNow)
	{
		game.resourceManager.Apply(this.cost, game);
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.GetEntity<BuildingEntity>().AddCraftingSlot();
		base.Apply(game, utcNow);
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00037418 File Offset: 0x00035618
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ResourceManager.ApplyCostToGameState(this.cost, gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = this.target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		dictionary["crafting_slots"] = this.slots;
		base.Confirm(gameState);
	}

	// Token: 0x04000624 RID: 1572
	public const string PURCHASE_CRAFTING_SLOT = "pcs";

	// Token: 0x04000625 RID: 1573
	private Cost cost;

	// Token: 0x04000626 RID: 1574
	private int slots;
}
