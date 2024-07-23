using System;

// Token: 0x02000419 RID: 1049
public class TreasureEntity : EntityDecorator
{
	// Token: 0x06002063 RID: 8291 RVA: 0x000C9F74 File Offset: 0x000C8174
	public TreasureEntity(Entity toDecorate) : base(toDecorate)
	{
		new StructureDecorator(this);
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06002064 RID: 8292 RVA: 0x000C9F84 File Offset: 0x000C8184
	public override EntityType Type
	{
		get
		{
			return EntityType.TREASURE;
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06002065 RID: 8293 RVA: 0x000C9F8C File Offset: 0x000C818C
	public ulong ClearTime
	{
		get
		{
			return (ulong)this.Invariable["time.clear"];
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06002066 RID: 8294 RVA: 0x000C9FA4 File Offset: 0x000C81A4
	public float ClearTimerDuration
	{
		get
		{
			return (float)this.Invariable["timer_duration"];
		}
	}

	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06002067 RID: 8295 RVA: 0x000C9FBC File Offset: 0x000C81BC
	// (set) Token: 0x06002068 RID: 8296 RVA: 0x000CA000 File Offset: 0x000C8200
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

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x06002069 RID: 8297 RVA: 0x000CA018 File Offset: 0x000C8218
	public bool Quickclear
	{
		get
		{
			return (bool)this.Invariable["quick_clear"];
		}
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x0600206A RID: 8298 RVA: 0x000CA030 File Offset: 0x000C8230
	// (set) Token: 0x0600206B RID: 8299 RVA: 0x000CA06C File Offset: 0x000C826C
	public TreasureSpawner TreasureTiming
	{
		get
		{
			if (!this.Variable.ContainsKey("treasure_timing"))
			{
				return null;
			}
			return (TreasureSpawner)this.Variable["treasure_timing"];
		}
		set
		{
			this.Variable["treasure_timing"] = value;
		}
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x0600206C RID: 8300 RVA: 0x000CA080 File Offset: 0x000C8280
	public ulong ClearTimeRemaining
	{
		get
		{
			return (this.ClearCompleteTime == null) ? 0UL : (this.ClearCompleteTime.Value - TFUtils.EpochTime());
		}
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x0600206D RID: 8301 RVA: 0x000CA0BC File Offset: 0x000C82BC
	// (set) Token: 0x0600206E RID: 8302 RVA: 0x000CA0FC File Offset: 0x000C82FC
	public float RaisingTimeRemaining
	{
		get
		{
			if (this.Variable.ContainsKey("raising_time"))
			{
				return (float)this.Variable["raising_time"];
			}
			return 0f;
		}
		set
		{
			this.Variable["raising_time"] = value;
		}
	}

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x0600206F RID: 8303 RVA: 0x000CA114 File Offset: 0x000C8314
	public RewardDefinition ClearingReward
	{
		get
		{
			return (RewardDefinition)this.Invariable["clearing_reward"];
		}
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06002070 RID: 8304 RVA: 0x000CA12C File Offset: 0x000C832C
	public bool HasStartedClearing
	{
		get
		{
			return this.Variable.ContainsKey("clearCompleteTime") && this.ClearCompleteTime != 0UL;
		}
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x000CA170 File Offset: 0x000C8370
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
}
