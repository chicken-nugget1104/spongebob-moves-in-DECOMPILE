using System;
using System.Collections.Generic;

// Token: 0x020000D9 RID: 217
public class HideWandererAction : PersistedSimulatedAction
{
	// Token: 0x06000815 RID: 2069 RVA: 0x000346E4 File Offset: 0x000328E4
	public HideWandererAction(Identity id, int did, ulong hideExpireAt) : base("hw", id, typeof(HideWandererAction).ToString())
	{
		this.dId = did;
		this.hideExpiresAt = hideExpireAt;
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00034710 File Offset: 0x00032910
	public HideWandererAction(Simulated simulated, ulong hideExpireAt) : base("hw", simulated.Id, typeof(HideWandererAction).ToString())
	{
		Entity entity = simulated.entity;
		this.dId = entity.DefinitionId;
		this.hideExpiresAt = hideExpireAt;
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000817 RID: 2071 RVA: 0x00034758 File Offset: 0x00032958
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x0003475C File Offset: 0x0003295C
	public new static HideWandererAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int did = TFUtils.LoadInt(data, "did");
		ulong hideExpireAt = TFUtils.LoadUlong(data, "hideExpiresAt", 0UL);
		return new HideWandererAction(id, did, hideExpireAt);
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x000347A4 File Offset: 0x000329A4
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = this.dId;
		dictionary["hideExpiresAt"] = this.hideExpiresAt;
		return dictionary;
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x000347E8 File Offset: 0x000329E8
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		simulated.ClearPendingCommands();
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.HideExpiresAt = new ulong?(this.hideExpiresAt);
		simulated.EnterInitialState(EntityManager.WandererActions["hidden"], game.simulation);
		base.Apply(game, utcNow);
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x00034858 File Offset: 0x00032A58
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> wandererGameState = ResidentEntity.GetWandererGameState(gameState, this.target);
		if (wandererGameState != null)
		{
			wandererGameState["hide_expires_at"] = this.hideExpiresAt;
		}
		base.Confirm(gameState);
	}

	// Token: 0x040005E3 RID: 1507
	public const string HIDE_WANDERER = "hw";

	// Token: 0x040005E4 RID: 1508
	public ulong hideExpiresAt;

	// Token: 0x040005E5 RID: 1509
	public int dId;
}
