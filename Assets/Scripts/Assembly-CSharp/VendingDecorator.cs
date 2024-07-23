using System;

// Token: 0x02000412 RID: 1042
public class VendingDecorator : EntityDecorator
{
	// Token: 0x06001FEA RID: 8170 RVA: 0x000C28C4 File Offset: 0x000C0AC4
	public VendingDecorator(Entity toDecorate) : base(toDecorate)
	{
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x06001FEB RID: 8171 RVA: 0x000C28D0 File Offset: 0x000C0AD0
	public int VendorId
	{
		get
		{
			return (int)this.Invariable["vendor_id"];
		}
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x06001FEC RID: 8172 RVA: 0x000C28E8 File Offset: 0x000C0AE8
	public ulong RestockPeriod
	{
		get
		{
			return (ulong)this.Invariable["restock_time"];
		}
	}

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x06001FED RID: 8173 RVA: 0x000C2900 File Offset: 0x000C0B00
	// (set) Token: 0x06001FEE RID: 8174 RVA: 0x000C2954 File Offset: 0x000C0B54
	public ulong RestockTime
	{
		get
		{
			if (!this.Variable.ContainsKey("restock_time"))
			{
				this.Variable["restock_time"] = TFUtils.EpochTime();
			}
			return (ulong)this.Variable["restock_time"];
		}
		set
		{
			this.Variable["restock_time"] = value;
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x06001FEF RID: 8175 RVA: 0x000C296C File Offset: 0x000C0B6C
	public ulong SpecialRestockPeriod
	{
		get
		{
			return (ulong)this.Invariable["special_time"];
		}
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x06001FF0 RID: 8176 RVA: 0x000C2984 File Offset: 0x000C0B84
	// (set) Token: 0x06001FF1 RID: 8177 RVA: 0x000C29D8 File Offset: 0x000C0BD8
	public ulong SpecialRestockTime
	{
		get
		{
			if (!this.Variable.ContainsKey("special_time"))
			{
				this.Variable["special_time"] = TFUtils.EpochTime();
			}
			return (ulong)this.Variable["special_time"];
		}
		set
		{
			this.Variable["special_time"] = value;
		}
	}
}
