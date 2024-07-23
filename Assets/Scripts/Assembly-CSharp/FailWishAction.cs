using System;
using System.Collections.Generic;

// Token: 0x020000D6 RID: 214
public class FailWishAction : PersistedSimulatedAction
{
	// Token: 0x060007FE RID: 2046 RVA: 0x00034080 File Offset: 0x00032280
	public FailWishAction(Simulated unit) : this(unit.Id)
	{
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00034090 File Offset: 0x00032290
	private FailWishAction(Identity id) : base("fw", id, typeof(FailWishAction).ToString())
	{
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06000800 RID: 2048 RVA: 0x000340B0 File Offset: 0x000322B0
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x000340B4 File Offset: 0x000322B4
	public new static FailWishAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		return new FailWishAction(id);
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x000340E0 File Offset: 0x000322E0
	public override Dictionary<string, object> ToDict()
	{
		return base.ToDict();
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x000340F8 File Offset: 0x000322F8
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
		entity.PreviousResourceId = entity.HungerResourceId;
		entity.HungerResourceId = null;
		entity.WishExpiresAt = new ulong?(utcNow);
		simulated.EnterInitialState(EntityManager.ResidentActions["wander_hungry"], game.simulation);
		base.Apply(game, utcNow);
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00034180 File Offset: 0x00032380
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, this.target);
		if (unitGameState.ContainsKey("wish_product_id"))
		{
			unitGameState["prev_wish_product_id"] = unitGameState["wish_product_id"];
		}
		unitGameState.Remove("wish_product_id");
		unitGameState.Remove("wish_expires_at");
		base.Confirm(gameState);
	}

	// Token: 0x040005DA RID: 1498
	public const string FAIL_WISH = "fw";
}
