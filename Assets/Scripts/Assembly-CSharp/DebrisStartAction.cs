using System;
using System.Collections.Generic;

// Token: 0x020000D3 RID: 211
public class DebrisStartAction : PersistedSimulatedAction
{
	// Token: 0x060007EA RID: 2026 RVA: 0x00033B8C File Offset: 0x00031D8C
	public DebrisStartAction(Identity target, ulong completeTime, Cost cost) : base("ds", target, typeof(DebrisStartAction).ToString())
	{
		this.cost = cost;
		this.completionTime = completeTime;
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060007EB RID: 2027 RVA: 0x00033BB8 File Offset: 0x00031DB8
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00033BBC File Offset: 0x00031DBC
	public new static DebrisStartAction FromDict(Dictionary<string, object> data)
	{
		Identity target = new Identity((string)data["target"]);
		ulong completeTime = TFUtils.LoadUlong(data, "completion_time", 0UL);
		Cost cost = Cost.FromObject(data["cost"]);
		return new DebrisStartAction(target, completeTime, cost);
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00033C08 File Offset: 0x00031E08
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["completion_time"] = this.completionTime;
		dictionary["cost"] = this.cost.ToDict();
		return dictionary;
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00033C4C File Offset: 0x00031E4C
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ClearableDecorator entity = simulated.GetEntity<ClearableDecorator>();
		simulated.ClearPendingCommands();
		game.resourceManager.Apply(this.cost, game);
		simulated.EnterInitialState(EntityManager.DebrisActions["clearing"], game.simulation);
		entity.ClearCompleteTime = new ulong?(this.completionTime);
		base.Apply(game, utcNow);
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00033CD0 File Offset: 0x00031ED0
	public override void Confirm(Dictionary<string, object> gameState)
	{
		try
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["debris"];
			string targetString = this.target.Describe();
			Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
			if (dictionary == null)
			{
				string message = "DebrisStartAction.Confirm - No Debris Found: " + targetString;
				TFUtils.ErrorLog(message);
				throw new Exception(message);
			}
			dictionary["clear_complete_time"] = this.completionTime;
			ResourceManager.ApplyCostToGameState(this.cost, gameState);
			base.Confirm(gameState);
		}
		catch (Exception message2)
		{
			TFUtils.ErrorLog(message2);
		}
	}

	// Token: 0x040005D2 RID: 1490
	public const string DEBRIS_START = "ds";

	// Token: 0x040005D3 RID: 1491
	private ulong completionTime;

	// Token: 0x040005D4 RID: 1492
	private Cost cost;
}
