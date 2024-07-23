using System;
using System.Collections.Generic;

// Token: 0x020000FD RID: 253
public class SellAction : PersistedSimulatedAction
{
	// Token: 0x06000913 RID: 2323 RVA: 0x000398E8 File Offset: 0x00037AE8
	public SellAction(Identity id, Cost cost) : base("s", id, typeof(SellAction).ToString())
	{
		TFUtils.Assert(cost != null, "Cannot create a sell action with a null selling cost");
		this.cost = cost;
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00039920 File Offset: 0x00037B20
	public SellAction(Simulated simulated, Cost cost) : this(simulated.Id, cost)
	{
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x00039930 File Offset: 0x00037B30
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00039934 File Offset: 0x00037B34
	public new static SellAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Cost cost = Cost.FromDict((Dictionary<string, object>)data["cost"]);
		return new SellAction(id, cost);
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00039978 File Offset: 0x00037B78
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["cost"] = this.cost.ToDict();
		return dictionary;
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x000399A4 File Offset: 0x00037BA4
	public override void Confirm(Dictionary<string, object> gameState)
	{
		string targetString = this.target.Describe();
		Simulated.Building.RemoveResidentsFromGameState(gameState, targetString);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		foreach (KeyValuePair<int, int> keyValuePair in this.cost.ResourceAmounts)
		{
			ResourceManager.AddAmountToGameState(keyValuePair.Key, keyValuePair.Value, gameState);
		}
		list.RemoveAll(match);
		base.Confirm(gameState);
	}

	// Token: 0x04000661 RID: 1633
	public const string SELL = "s";

	// Token: 0x04000662 RID: 1634
	public Cost cost;
}
