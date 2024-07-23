using System;

// Token: 0x0200040B RID: 1035
public class DebrisEntity : EntityDecorator
{
	// Token: 0x06001FB2 RID: 8114 RVA: 0x000C2100 File Offset: 0x000C0300
	public DebrisEntity(Entity toDecorate) : base(toDecorate)
	{
		new PurchasableDecorator(this);
		new ClearableDecorator(this);
		new StructureDecorator(this);
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06001FB3 RID: 8115 RVA: 0x000C2120 File Offset: 0x000C0320
	public override EntityType Type
	{
		get
		{
			return EntityType.DEBRIS;
		}
	}

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x000C2124 File Offset: 0x000C0324
	// (set) Token: 0x06001FB5 RID: 8117 RVA: 0x000C215C File Offset: 0x000C035C
	public int? ExpansionId
	{
		get
		{
			object obj = null;
			if (this.Variable.TryGetValue("expansionId", out obj))
			{
				return (int?)obj;
			}
			return null;
		}
		set
		{
			this.Variable["expansionId"] = value;
		}
	}
}
