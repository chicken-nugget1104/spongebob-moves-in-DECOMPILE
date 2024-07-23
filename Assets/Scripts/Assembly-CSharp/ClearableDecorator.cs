using System;

// Token: 0x0200040D RID: 1037
public class ClearableDecorator : EntityDecorator
{
	// Token: 0x06001FBC RID: 8124 RVA: 0x000C2220 File Offset: 0x000C0420
	public ClearableDecorator(Entity toDecorate) : base(toDecorate)
	{
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06001FBD RID: 8125 RVA: 0x000C222C File Offset: 0x000C042C
	public ulong ClearTime
	{
		get
		{
			return (ulong)this.Invariable["time.clear"];
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x06001FBE RID: 8126 RVA: 0x000C2244 File Offset: 0x000C0444
	public float ClearTimerDuration
	{
		get
		{
			return (float)this.Invariable["timer_duration"];
		}
	}

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x06001FBF RID: 8127 RVA: 0x000C225C File Offset: 0x000C045C
	// (set) Token: 0x06001FC0 RID: 8128 RVA: 0x000C22A0 File Offset: 0x000C04A0
	public ulong? ClearCompleteTime
	{
		get
		{
			if (!this.Variable.ContainsKey("clearCompleteTime"))
			{
				return null;
			}
			return (ulong?)this.Variable["clearCompleteTime"];
		}
		set
		{
			this.Variable["clearCompleteTime"] = value;
		}
	}

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x06001FC1 RID: 8129 RVA: 0x000C22B8 File Offset: 0x000C04B8
	public ulong ClearTimeRemaining
	{
		get
		{
			return (this.ClearCompleteTime == null) ? 0UL : (this.ClearCompleteTime.Value - TFUtils.EpochTime());
		}
	}

	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x06001FC2 RID: 8130 RVA: 0x000C22F4 File Offset: 0x000C04F4
	public Cost ClearCost
	{
		get
		{
			return (Cost)this.Invariable["cost"];
		}
	}

	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06001FC3 RID: 8131 RVA: 0x000C230C File Offset: 0x000C050C
	public Cost ClearingRushCost
	{
		get
		{
			return (Cost)this.Invariable["clear_rush_cost"];
		}
	}

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06001FC4 RID: 8132 RVA: 0x000C2324 File Offset: 0x000C0524
	public RewardDefinition ClearingReward
	{
		get
		{
			return (RewardDefinition)this.Invariable["clearing_reward"];
		}
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x000C233C File Offset: 0x000C053C
	public bool HasStartedClearing
	{
		get
		{
			return this.Variable.ContainsKey("clearCompleteTime") && this.ClearCompleteTime != 0UL;
		}
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000C2380 File Offset: 0x000C0580
	public bool IsClearing(ulong utcNow)
	{
		bool result;
		if (this.HasStartedClearing)
		{
			ulong? clearCompleteTime = this.ClearCompleteTime;
			result = (clearCompleteTime != null && utcNow < clearCompleteTime.Value);
		}
		else
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000C23BC File Offset: 0x000C05BC
	public ulong RemainingTime(ulong utcNow)
	{
		return this.ClearCompleteTime.Value - utcNow;
	}
}
