using System;
using System.Collections.Generic;

// Token: 0x0200010B RID: 267
public class VendingAction : PersistedSimulatedAction
{
	// Token: 0x06000970 RID: 2416 RVA: 0x0003B3A4 File Offset: 0x000395A4
	public VendingAction(Identity id, int slotId, bool special, Reward reward, Cost cost) : base("va", id, typeof(VendingAction).ToString())
	{
		this.slotId = slotId;
		this.cost = cost;
		this.reward = reward;
		this.special = special;
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000971 RID: 2417 RVA: 0x0003B3E0 File Offset: 0x000395E0
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0003B3E4 File Offset: 0x000395E4
	public new static VendingAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "slot_id");
		Cost cost = Cost.FromObject(data["cost"]);
		Reward reward = Reward.FromObject(data["reward"]);
		bool flag = TFUtils.LoadBool(data, "special");
		return new VendingAction(id, num, flag, reward, cost);
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0003B454 File Offset: 0x00039654
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["slot_id"] = this.slotId;
		dictionary["reward"] = this.reward.ToDict();
		dictionary["cost"] = this.cost.ToDict();
		dictionary["special"] = this.special;
		return dictionary;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0003B4C4 File Offset: 0x000396C4
	public override void Apply(Game game, ulong utcNow)
	{
		VendingInstance vendingInstance = (!this.special) ? game.vendingManager.GetVendingInstance(this.target, this.slotId) : game.vendingManager.GetSpecialInstance(this.target);
		if (vendingInstance.remaining > 0)
		{
			vendingInstance.remaining--;
			game.ApplyReward(this.reward, TFUtils.EpochTime(), true);
			game.resourceManager.Apply(this.cost, game);
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x0003B550 File Offset: 0x00039750
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["vending"];
		string targetString = this.target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> data = (Dictionary<string, object>)list.Find(match);
		Dictionary<string, object> data2 = (!this.special) ? TFUtils.LoadDict(data, "general_instances") : TFUtils.LoadDict(data, "special_instances");
		Dictionary<string, object> dictionary = TFUtils.LoadDict(data2, this.slotId.ToString());
		int num = TFUtils.LoadInt(dictionary, "remaining");
		if (num > 0)
		{
			RewardManager.ApplyToGameState(this.reward, TFUtils.EpochTime(), gameState);
			ResourceManager.ApplyCostToGameState(this.cost, gameState);
			dictionary["remaining"] = num - 1;
		}
		else
		{
			TFUtils.ErrorLog("Trying to confirm selling an item which is out of stock: " + TFUtils.DebugDictToString(dictionary));
		}
		base.Confirm(gameState);
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x0003B658 File Offset: 0x00039858
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		this.reward.AddDataToTrigger(ref data);
	}

	// Token: 0x0400068D RID: 1677
	public const string VENDING_ACTION = "va";

	// Token: 0x0400068E RID: 1678
	private int slotId;

	// Token: 0x0400068F RID: 1679
	private Reward reward;

	// Token: 0x04000690 RID: 1680
	private Cost cost;

	// Token: 0x04000691 RID: 1681
	private bool special;
}
