using System;

// Token: 0x02000409 RID: 1033
public class EntityCtor : Ctor<Entity>
{
	// Token: 0x06001F9C RID: 8092 RVA: 0x000C1CE8 File Offset: 0x000BFEE8
	public EntityCtor(Blueprint blueprint)
	{
		this.blueprint = blueprint;
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x000C1CF8 File Offset: 0x000BFEF8
	public Entity Create()
	{
		return this.Create(new Identity());
	}

	// Token: 0x06001F9E RID: 8094 RVA: 0x000C1D08 File Offset: 0x000BFF08
	public Entity Create(Identity id)
	{
		EntityType primaryType = this.blueprint.PrimaryType;
		Entity entity = new CoreEntity(id, this.blueprint);
		if (primaryType == EntityType.BUILDING)
		{
			entity = new BuildingEntity(entity);
		}
		else if (primaryType == EntityType.ANNEX)
		{
			entity = new BuildingEntity(entity);
			entity = new AnnexEntity(entity);
		}
		else if (primaryType == EntityType.DEBRIS)
		{
			entity = new DebrisEntity(entity);
		}
		else if (primaryType == EntityType.RESIDENT || primaryType == EntityType.WORKER || primaryType == EntityType.WANDERER)
		{
			entity = new ResidentEntity(entity);
		}
		else if (primaryType == EntityType.LANDMARK)
		{
			entity = new LandmarkEntity(entity);
		}
		else if (primaryType == EntityType.TREASURE)
		{
			entity = new TreasureEntity(entity);
		}
		else
		{
			TFUtils.ErrorLog("Unexpected entity type (" + primaryType + ")");
		}
		return entity;
	}

	// Token: 0x040013B5 RID: 5045
	private Blueprint blueprint;
}
