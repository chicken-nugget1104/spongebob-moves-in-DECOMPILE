using System;

// Token: 0x0200040F RID: 1039
public class PeriodicProductionDecorator : EntityDecorator
{
	// Token: 0x06001FD5 RID: 8149 RVA: 0x000C2614 File Offset: 0x000C0814
	public PeriodicProductionDecorator(Entity toDecorate) : base(toDecorate)
	{
	}

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x000C2620 File Offset: 0x000C0820
	public bool RentRushable
	{
		get
		{
			this.RequireProduction();
			return (bool)this.Invariable["rent_rushable"];
		}
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x000C2640 File Offset: 0x000C0840
	public ulong RentProductionTime
	{
		get
		{
			this.RequireProduction();
			return (ulong)this.Invariable["time.production"];
		}
	}

	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x06001FD8 RID: 8152 RVA: 0x000C2660 File Offset: 0x000C0860
	public float RentTimerDuration
	{
		get
		{
			this.RequireProduction();
			return (float)this.Invariable["rent_timer_duration"];
		}
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x000C2680 File Offset: 0x000C0880
	public Cost RentRushCost
	{
		get
		{
			return (Cost)this.Invariable["rent_rush_cost"];
		}
	}

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x06001FDA RID: 8154 RVA: 0x000C2698 File Offset: 0x000C0898
	// (set) Token: 0x06001FDB RID: 8155 RVA: 0x000C26F8 File Offset: 0x000C08F8
	public ulong ProductReadyTime
	{
		get
		{
			this.RequireProduction();
			if (!this.Variable.ContainsKey("product.ready"))
			{
				this.Variable["product.ready"] = TFUtils.EpochTime() + this.RentProductionTime;
			}
			return (ulong)this.Variable["product.ready"];
		}
		set
		{
			this.RequireProduction();
			this.Variable["product.ready"] = value;
		}
	}

	// Token: 0x06001FDC RID: 8156 RVA: 0x000C2718 File Offset: 0x000C0918
	private void RequireProduction()
	{
		if (this.Invariable["product"] == null)
		{
			throw new InvalidOperationException("Building does not produce rent");
		}
	}

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x06001FDD RID: 8157 RVA: 0x000C2748 File Offset: 0x000C0948
	public RewardDefinition Product
	{
		get
		{
			this.RequireProduction();
			return (RewardDefinition)this.Invariable["product"];
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x06001FDE RID: 8158 RVA: 0x000C2768 File Offset: 0x000C0968
	public bool HasProduct
	{
		get
		{
			return this.Invariable["product"] != null;
		}
	}

	// Token: 0x040013BE RID: 5054
	public const string PRODUCTION_RUSHABLE = "rent_rushable";
}
