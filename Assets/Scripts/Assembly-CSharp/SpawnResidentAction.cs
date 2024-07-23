using System;
using System.Collections.Generic;

// Token: 0x020000FE RID: 254
public class SpawnResidentAction : PersistedTriggerableAction
{
	// Token: 0x06000919 RID: 2329 RVA: 0x00039A80 File Offset: 0x00037C80
	public SpawnResidentAction(int residentDID, string residentID, int buildingDID, string buildingID) : base("sr", Identity.Null())
	{
		this.residentDID = residentDID;
		this.residentID = residentID;
		this.buildingDID = buildingDID;
		this.buildingID = buildingID;
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x0600091A RID: 2330 RVA: 0x00039AB0 File Offset: 0x00037CB0
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x00039AB4 File Offset: 0x00037CB4
	public new static SpawnResidentAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "resident_did");
		string text = TFUtils.LoadString(data, "resident_id");
		int num2 = TFUtils.LoadInt(data, "building_did");
		string text2 = TFUtils.LoadString(data, "building_id");
		return new SpawnResidentAction(num, text, num2, text2);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x00039B00 File Offset: 0x00037D00
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["resident_did"] = this.residentDID;
		dictionary["resident_id"] = this.residentID;
		dictionary["building_did"] = this.buildingDID;
		dictionary["building_id"] = this.buildingID;
		return dictionary;
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x00039B64 File Offset: 0x00037D64
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(new Identity(this.buildingID));
		if (simulated == null)
		{
			return;
		}
		Simulated simulated2 = game.simulation.FindSimulated(new int?(this.residentDID));
		if (simulated2 == null)
		{
			ResidentEntity decorator = game.simulation.EntityManager.Create(EntityType.RESIDENT, this.residentDID, null, true).GetDecorator<ResidentEntity>();
			decorator.HungerResourceId = null;
			decorator.PreviousResourceId = null;
			decorator.WishExpiresAt = null;
			decorator.HungryAt = TFUtils.EpochTime();
			decorator.MatchBonus = null;
			simulated2 = Simulated.Resident.Load(decorator, simulated.Id, decorator.WishExpiresAt, decorator.HungerResourceId, decorator.PreviousResourceId, decorator.HungryAt, null, decorator.MatchBonus, game.simulation, TFUtils.EpochTime());
		}
		this.residentID = simulated2.Id.Describe();
		base.Apply(game, utcNow);
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x00039C68 File Offset: 0x00037E68
	public override void Confirm(Dictionary<string, object> gameState)
	{
		if (ResidentEntity.GetUnitGameState(gameState, this.residentDID) == null)
		{
			Simulated.Building.AddResidentToGameState(gameState, this.residentID, this.residentDID, this.buildingID, TFUtils.EpochTime());
		}
		base.Confirm(gameState);
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x00039CAC File Offset: 0x00037EAC
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00039CB0 File Offset: 0x00037EB0
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x04000663 RID: 1635
	public const string SPAWN_RESDIENT = "sr";

	// Token: 0x04000664 RID: 1636
	public int residentDID;

	// Token: 0x04000665 RID: 1637
	public string residentID;

	// Token: 0x04000666 RID: 1638
	public int buildingDID;

	// Token: 0x04000667 RID: 1639
	public string buildingID;
}
