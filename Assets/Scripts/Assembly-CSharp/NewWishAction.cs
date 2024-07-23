using System;
using System.Collections.Generic;

// Token: 0x020000E5 RID: 229
public class NewWishAction : PersistedSimulatedAction
{
	// Token: 0x0600086A RID: 2154 RVA: 0x00036794 File Offset: 0x00034994
	public NewWishAction(Identity id, int wishProductId, int? prevWishProductId, ulong expiresAt) : base("nw", id, typeof(NewWishAction).ToString())
	{
		this.wishProductId = wishProductId;
		this.expiresAt = expiresAt;
		this.prevWishProductId = prevWishProductId;
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x0600086B RID: 2155 RVA: 0x000367C8 File Offset: 0x000349C8
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x000367CC File Offset: 0x000349CC
	public new static NewWishAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int num = TFUtils.LoadInt(data, "wish_product_id");
		ulong num2 = TFUtils.LoadUlong(data, "wish_expires_at", 0UL);
		int? num3 = TFUtils.TryLoadInt(data, "prev_wish_product_id");
		return new NewWishAction(id, num, num3, num2);
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x00036824 File Offset: 0x00034A24
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["wish_product_id"] = this.wishProductId;
		dictionary["wish_expires_at"] = this.expiresAt;
		if (this.prevWishProductId != null)
		{
			dictionary["prev_wish_product_id"] = this.prevWishProductId;
		}
		return dictionary;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0003688C File Offset: 0x00034A8C
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			base.Apply(game, utcNow);
			return;
		}
		ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
		entity.PreviousResourceId = this.prevWishProductId;
		entity.HungerResourceId = new int?(this.wishProductId);
		entity.WishExpiresAt = new ulong?(this.expiresAt);
		simulated.ClearPendingCommands();
		simulated.EnterInitialState(EntityManager.ResidentActions["wishing"], game.simulation);
		base.Apply(game, utcNow);
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x00036918 File Offset: 0x00034B18
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(gameState, this.target);
		if (this.prevWishProductId != null)
		{
			unitGameState["prev_wish_product_id"] = this.prevWishProductId;
		}
		if (unitGameState.ContainsKey("wish_product_id"))
		{
			unitGameState["wish_product_id"] = this.wishProductId;
		}
		else
		{
			unitGameState.Add("wish_product_id", this.wishProductId);
		}
		unitGameState["wish_expires_at"] = this.expiresAt;
		base.Confirm(gameState);
	}

	// Token: 0x04000612 RID: 1554
	public const string NEW_WISH = "nw";

	// Token: 0x04000613 RID: 1555
	public const ulong INVALID_ULONG = 18446744073709551615UL;

	// Token: 0x04000614 RID: 1556
	public int wishProductId;

	// Token: 0x04000615 RID: 1557
	public int? prevWishProductId;

	// Token: 0x04000616 RID: 1558
	public ulong expiresAt;
}
