using System;
using System.Collections.Generic;

// Token: 0x02000108 RID: 264
public class TreasureUncoverAction : PersistedSimulatedAction
{
	// Token: 0x0600095E RID: 2398 RVA: 0x0003AE80 File Offset: 0x00039080
	public TreasureUncoverAction(Identity id, ulong completionTime) : base("tu", id, typeof(TreasureUncoverAction).ToString())
	{
		this.completionTime = completionTime;
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x0600095F RID: 2399 RVA: 0x0003AEB0 File Offset: 0x000390B0
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0003AEB4 File Offset: 0x000390B4
	public new static TreasureUncoverAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		ulong num = TFUtils.LoadUlong(data, "completion_time", 0UL);
		return new TreasureUncoverAction(id, num);
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0003AEF0 File Offset: 0x000390F0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["completion_time"] = this.completionTime;
		return dictionary;
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0003AF1C File Offset: 0x0003911C
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		TreasureEntity entity = simulated.GetEntity<TreasureEntity>();
		simulated.ClearPendingCommands();
		entity.ClearCompleteTime = new ulong?(this.completionTime);
		simulated.EnterInitialState(EntityManager.TreasureActions["uncovering"], game.simulation);
		base.Apply(game, utcNow);
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0003AF8C File Offset: 0x0003918C
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> orCreateList = TFUtils.GetOrCreateList<object>((Dictionary<string, object>)gameState["farm"], "treasure");
		string targetString = this.target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)orCreateList.Find(match);
		dictionary["clear_complete_time"] = this.completionTime;
		base.Confirm(gameState);
	}

	// Token: 0x04000686 RID: 1670
	public const string TREASURE_UNCOVER = "tu";

	// Token: 0x04000687 RID: 1671
	private ulong completionTime;
}
