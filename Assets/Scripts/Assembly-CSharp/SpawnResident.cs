using System;
using System.Collections.Generic;

// Token: 0x02000253 RID: 595
public class SpawnResident : SessionActionDefinition
{
	// Token: 0x0600132D RID: 4909 RVA: 0x00084550 File Offset: 0x00082750
	public static SpawnResident Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		SpawnResident spawnResident = new SpawnResident();
		spawnResident.Parse(data, id, startConditions, originatedFromQuest);
		return spawnResident;
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x00084570 File Offset: 0x00082770
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.residentID = TFUtils.TryLoadInt(data, "resident_id");
		this.buildingID = TFUtils.TryLoadInt(data, "building_id");
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x000845A8 File Offset: 0x000827A8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		Dictionary<string, object> dictionary2 = dictionary;
		string key = "resident_id";
		int? num = this.residentID;
		dictionary2[key] = ((num != null) ? this.residentID : new int?(-1));
		Dictionary<string, object> dictionary3 = dictionary;
		string key2 = "building_id";
		int? num2 = this.buildingID;
		dictionary3[key2] = ((num2 != null) ? this.buildingID : new int?(-1));
		return dictionary;
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x0008462C File Offset: 0x0008282C
	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		int? num = this.residentID;
		if (num != null)
		{
			int? num2 = this.residentID;
			if (num2 == null || num2.Value >= 0)
			{
				int? num3 = this.buildingID;
				if (num3 != null)
				{
					int? num4 = this.buildingID;
					if (num4 == null || num4.Value >= 0)
					{
						Simulated simulated = session.TheGame.simulation.FindSimulated(new int?(this.buildingID.Value));
						if (simulated == null)
						{
							action.MarkFailed();
							return;
						}
						if (session.TheGame.simulation.FindSimulated(new int?(this.residentID.Value)) == null)
						{
							ResidentEntity decorator = session.TheGame.simulation.EntityManager.Create(EntityType.RESIDENT, this.residentID.Value, null, true).GetDecorator<ResidentEntity>();
							if (decorator.Disabled)
							{
								action.MarkFailed();
								return;
							}
							decorator.HungerResourceId = null;
							decorator.PreviousResourceId = null;
							decorator.WishExpiresAt = null;
							decorator.HungryAt = TFUtils.EpochTime();
							decorator.MatchBonus = null;
							Simulated simulated2 = Simulated.Resident.Load(decorator, simulated.Id, decorator.WishExpiresAt, decorator.HungerResourceId, decorator.PreviousResourceId, decorator.HungryAt, null, decorator.MatchBonus, session.TheGame.simulation, TFUtils.EpochTime());
							session.TheGame.simulation.ModifyGameState(new SpawnResidentAction(this.residentID.Value, simulated2.Id.Describe(), this.buildingID.Value, simulated.Id.Describe()));
							action.MarkSucceeded();
						}
						else
						{
							action.MarkFailed();
						}
						return;
					}
				}
			}
		}
		action.MarkFailed();
	}

	// Token: 0x04000D41 RID: 3393
	public const string TYPE = "spawn_resident";

	// Token: 0x04000D42 RID: 3394
	public const string RESIDENT_ID = "resident_id";

	// Token: 0x04000D43 RID: 3395
	public const string BUILDING_ID = "building_id";

	// Token: 0x04000D44 RID: 3396
	private int? residentID;

	// Token: 0x04000D45 RID: 3397
	private int? buildingID;
}
