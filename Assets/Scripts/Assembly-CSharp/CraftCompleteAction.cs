using System;
using System.Collections.Generic;

// Token: 0x020000D0 RID: 208
public class CraftCompleteAction : PersistedSimulatedAction
{
	// Token: 0x060007D1 RID: 2001 RVA: 0x00033234 File Offset: 0x00031434
	public CraftCompleteAction(Identity id, int slotId, Reward reward) : base("cf", id, "cf")
	{
		this.reward = reward;
		this.slotId = slotId;
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00033258 File Offset: 0x00031458
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x0003325C File Offset: 0x0003145C
	public new static CraftCompleteAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "slot_id");
		Reward reward = (!data.ContainsKey("reward")) ? null : Reward.FromObject(data["reward"]);
		return new CraftCompleteAction(id, num, reward);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x000332BC File Offset: 0x000314BC
	public override void Apply(Game game, ulong utcNow)
	{
		game.craftManager.RemoveCraftingInstance(this.target, this.slotId);
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		game.simulation.Router.CancelMatching(Command.TYPE.CRAFTED, this.target, this.target, new Dictionary<string, object>
		{
			{
				"slot_id",
				this.slotId
			}
		});
		BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
		entity.CraftingComplete(this.reward);
		simulated.EnterInitialState(EntityManager.BuildingActions["reflecting"], game.simulation);
		base.Apply(game, utcNow);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00033374 File Offset: 0x00031574
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["crafts"];
		string targetString = this.target.Describe();
		Predicate<object> match = (object c) => ((string)((Dictionary<string, object>)c)["building_label"]).Equals(targetString) && TFUtils.LoadInt((Dictionary<string, object>)c, "slot_id") == this.slotId;
		list.Remove((Dictionary<string, object>)list.Find(match));
		List<object> list2 = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		match = ((object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString));
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list2.Find(match);
		Reward reward = new Reward(null, null, null, null, null, null, null, null, false, this.reward.ThoughtIcon);
		reward += this.reward;
		if (dictionary.ContainsKey("craft_rewards"))
		{
			reward += Reward.FromObject(dictionary["craft_rewards"]);
		}
		else if (dictionary.ContainsKey("craft.rewards"))
		{
			reward += Reward.FromObject(dictionary["craft.rewards"]);
		}
		dictionary["craft_rewards"] = reward.ToDict();
		base.Confirm(gameState);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x000334B8 File Offset: 0x000316B8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["reward"] = this.reward.ToDict();
		dictionary["slot_id"] = this.slotId;
		return dictionary;
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x000334FC File Offset: 0x000316FC
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		base.AddMoreDataToTrigger(ref data);
		this.reward.AddDataToTrigger(ref data);
	}

	// Token: 0x040005C7 RID: 1479
	public const string CRAFT_FINISHED = "cf";

	// Token: 0x040005C8 RID: 1480
	private Reward reward;

	// Token: 0x040005C9 RID: 1481
	private int slotId;
}
