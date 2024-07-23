using System;

// Token: 0x020003F1 RID: 1009
public class AnnexEntity : EntityDecorator
{
	// Token: 0x06001EBA RID: 7866 RVA: 0x000BD344 File Offset: 0x000BB544
	public AnnexEntity(Entity toDecorate) : base(toDecorate)
	{
		new StructureDecorator(this);
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06001EBB RID: 7867 RVA: 0x000BD354 File Offset: 0x000BB554
	public override EntityType Type
	{
		get
		{
			return EntityType.ANNEX;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06001EBC RID: 7868 RVA: 0x000BD358 File Offset: 0x000BB558
	public Identity HubId
	{
		get
		{
			if (this.Invariable.ContainsKey("hub_id"))
			{
				return (Identity)this.Invariable["hub_id"];
			}
			return null;
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06001EBD RID: 7869 RVA: 0x000BD394 File Offset: 0x000BB594
	public uint? HubDid
	{
		get
		{
			if (this.Invariable.ContainsKey("hub_did"))
			{
				return (uint?)this.Invariable["hub_did"];
			}
			return null;
		}
	}

	// Token: 0x06001EBE RID: 7870 RVA: 0x000BD3D8 File Offset: 0x000BB5D8
	public override void PatchReferences(Game game)
	{
		if (this.Invariable.ContainsKey("hub_id"))
		{
			Identity hubId = this.HubId;
			BuildingEntity decorator = game.entities.GetEntity(hubId).GetDecorator<BuildingEntity>();
			decorator.RegisterAnnex(this);
		}
		else if (this.Invariable.ContainsKey("hub_did"))
		{
			Simulated simulated = game.simulation.FindSimulated(new int?((int)this.HubDid.Value));
			if (simulated != null)
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				entity.RegisterAnnex(this);
			}
		}
		base.PatchReferences(game);
	}

	// Token: 0x04001311 RID: 4881
	public const string TYPE = "annex";

	// Token: 0x04001312 RID: 4882
	public const string HUB_ID = "hub_id";

	// Token: 0x04001313 RID: 4883
	public const string HUB_DID = "hub_did";
}
