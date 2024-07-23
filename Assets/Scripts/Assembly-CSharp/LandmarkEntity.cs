using System;

// Token: 0x02000420 RID: 1056
public class LandmarkEntity : EntityDecorator
{
	// Token: 0x060020B3 RID: 8371 RVA: 0x000CA36C File Offset: 0x000C856C
	public LandmarkEntity(Entity toDecorate) : base(new PurchasableDecorator(toDecorate))
	{
		new StructureDecorator(this);
		new ActivatableDecorator(this);
	}

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x060020B4 RID: 8372 RVA: 0x000CA388 File Offset: 0x000C8588
	public override EntityType Type
	{
		get
		{
			return EntityType.LANDMARK;
		}
	}
}
