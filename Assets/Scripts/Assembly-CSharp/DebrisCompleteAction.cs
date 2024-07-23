using System;
using System.Collections.Generic;

// Token: 0x020000D2 RID: 210
public class DebrisCompleteAction : PersistedSimulatedAction
{
	// Token: 0x060007E3 RID: 2019 RVA: 0x00033974 File Offset: 0x00031B74
	public DebrisCompleteAction(Identity id, Reward reward) : base("dc", id, typeof(DebrisCompleteAction).ToString())
	{
		this.reward = reward;
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060007E4 RID: 2020 RVA: 0x000339A4 File Offset: 0x00031BA4
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x000339A8 File Offset: 0x00031BA8
	public new static DebrisCompleteAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = Reward.FromObject(data["reward"]);
		DebrisCompleteAction debrisCompleteAction = new DebrisCompleteAction(id, reward);
		debrisCompleteAction.DropTargetDataFromDict(data);
		return debrisCompleteAction;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x000339EC File Offset: 0x00031BEC
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = this.reward.ToDict();
		base.DropTargetDataToDict(dictionary);
		return dictionary;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00033A20 File Offset: 0x00031C20
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.SetFootprint(game.simulation, false);
		game.simulation.RemoveSimulated(simulated);
		game.entities.Destroy(this.target);
		game.ApplyReward(this.reward, base.GetTime(), true);
		base.AddPickup(game.simulation);
		simulated.ClearPendingCommands();
		base.Apply(game, utcNow);
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00033AA8 File Offset: 0x00031CA8
	public override void Confirm(Dictionary<string, object> gameState)
	{
		try
		{
			List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["debris"];
			string targetString = this.target.Describe();
			Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
			object obj = list.Find(match);
			if (obj == null)
			{
				string message = "DebrisCompleteAction.Confirm - No Debris Found: " + targetString;
				TFUtils.ErrorLog(message);
				throw new Exception(message);
			}
			list.Remove(obj);
			RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
			base.AddPickupToGameState(gameState);
			base.Confirm(gameState);
		}
		catch (Exception message2)
		{
			TFUtils.ErrorLog(message2);
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x00033B80 File Offset: 0x00031D80
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
	}

	// Token: 0x040005D0 RID: 1488
	public const string DEBRIS_COMPLETE = "dc";

	// Token: 0x040005D1 RID: 1489
	private Reward reward;
}
