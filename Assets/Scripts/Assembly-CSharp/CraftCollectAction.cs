using System;
using System.Collections.Generic;

// Token: 0x020000CF RID: 207
public class CraftCollectAction : PersistedSimulatedAction
{
	// Token: 0x060007CA RID: 1994 RVA: 0x00033040 File Offset: 0x00031240
	public CraftCollectAction(Identity id, Reward reward) : base("cc", id, "CraftPickup")
	{
		this.reward = reward;
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060007CB RID: 1995 RVA: 0x0003305C File Offset: 0x0003125C
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00033060 File Offset: 0x00031260
	public new static CraftCollectAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Reward reward = (!data.ContainsKey("reward")) ? null : Reward.FromObject(data["reward"]);
		CraftCollectAction craftCollectAction = new CraftCollectAction(id, reward);
		craftCollectAction.DropTargetDataFromDict(data);
		return craftCollectAction;
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x000330BC File Offset: 0x000312BC
	public override void Apply(Game game, ulong utcNow)
	{
		game.ApplyReward(this.reward, base.GetTime(), true);
		base.AddPickup(game.simulation);
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
		entity.ClearCraftingRewards();
		simulated.EnterInitialState(EntityManager.BuildingActions["reflecting"], game.simulation);
		base.Apply(game, utcNow);
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x0003313C File Offset: 0x0003133C
	public override void Confirm(Dictionary<string, object> gameState)
	{
		RewardManager.ApplyToGameState(this.reward, base.GetTime(), gameState);
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		string targetString = this.target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		if (dictionary.ContainsKey("craft.rewards"))
		{
			dictionary.Remove("craft.rewards");
		}
		else
		{
			dictionary.Remove("craft_rewards");
		}
		base.AddPickupToGameState(gameState);
		base.Confirm(gameState);
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x000331E8 File Offset: 0x000313E8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = this.reward.ToDict();
		base.DropTargetDataToDict(dictionary);
		return dictionary;
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0003321C File Offset: 0x0003141C
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		this.reward.AddDataToTrigger(ref data);
	}

	// Token: 0x040005C4 RID: 1476
	public const string CRAFT_COLLECT = "cc";

	// Token: 0x040005C5 RID: 1477
	public const string PICKUP_TRIGGERTYPE = "CraftPickup";

	// Token: 0x040005C6 RID: 1478
	private Reward reward;
}
