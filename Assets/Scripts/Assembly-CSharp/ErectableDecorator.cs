using System;
using System.Collections.Generic;

// Token: 0x0200040E RID: 1038
public class ErectableDecorator : EntityDecorator
{
	// Token: 0x06001FC8 RID: 8136 RVA: 0x000C23DC File Offset: 0x000C05DC
	public ErectableDecorator(Entity toDecorate) : base(toDecorate)
	{
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x000C23E8 File Offset: 0x000C05E8
	public Cost BuildRushCost
	{
		get
		{
			return (Cost)this.Invariable["build_rush_cost"];
		}
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06001FCA RID: 8138 RVA: 0x000C2400 File Offset: 0x000C0600
	public ulong ErectionTime
	{
		get
		{
			return (ulong)this.Invariable["time.build"];
		}
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06001FCB RID: 8139 RVA: 0x000C2418 File Offset: 0x000C0618
	public float ErectionTimerDuration
	{
		get
		{
			return (float)this.Invariable["build_timer_duration"];
		}
	}

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06001FCC RID: 8140 RVA: 0x000C2430 File Offset: 0x000C0630
	// (set) Token: 0x06001FCD RID: 8141 RVA: 0x000C2474 File Offset: 0x000C0674
	public ulong? ErectionCompleteTime
	{
		get
		{
			if (this.Variable.ContainsKey("buildCompleteTime"))
			{
				return (ulong?)this.Variable["buildCompleteTime"];
			}
			return null;
		}
		set
		{
			this.Variable["buildCompleteTime"] = value;
		}
	}

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x06001FCE RID: 8142 RVA: 0x000C248C File Offset: 0x000C068C
	// (set) Token: 0x06001FCF RID: 8143 RVA: 0x000C24D0 File Offset: 0x000C06D0
	public double RaisingTimeRemaining
	{
		get
		{
			if (this.Variable.ContainsKey("raising_time"))
			{
				return (double)this.Variable["raising_time"];
			}
			return 0.0;
		}
		set
		{
			this.Variable["raising_time"] = value;
		}
	}

	// Token: 0x06001FD0 RID: 8144 RVA: 0x000C24E8 File Offset: 0x000C06E8
	public bool IsErecting(ulong utcNow)
	{
		bool result;
		if (this.Variable.ContainsKey("buildCompleteTime"))
		{
			ulong? erectionCompleteTime = this.ErectionCompleteTime;
			result = (erectionCompleteTime != null && utcNow < erectionCompleteTime.Value);
		}
		else
		{
			result = false;
		}
		return result;
	}

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x000C2530 File Offset: 0x000C0730
	public RewardDefinition CompletionReward
	{
		get
		{
			return (RewardDefinition)this.Invariable["completion_reward"];
		}
	}

	// Token: 0x06001FD2 RID: 8146 RVA: 0x000C2548 File Offset: 0x000C0748
	public override void DeserializeDecorator(Dictionary<string, object> data)
	{
		if (data.ContainsKey("activated_time"))
		{
			this.ErectionCompleteTime = new ulong?(TFUtils.LoadUlong(data, "activated_time", 0UL));
			if (this.ErectionCompleteTime == 0UL)
			{
				this.ErectionCompleteTime = new ulong?(TFUtils.LoadUlong(data, "build_finish_time", 0UL));
			}
		}
		else if (data.ContainsKey("build_finish_time"))
		{
			this.ErectionCompleteTime = new ulong?(TFUtils.LoadUlong(data, "build_finish_time", 0UL));
		}
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x000C25E4 File Offset: 0x000C07E4
	public override void SerializeDecorator(ref Dictionary<string, object> data)
	{
		data["build_finish_time"] = this.ErectionCompleteTime;
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x000C2600 File Offset: 0x000C0800
	public static void Serialize(ref Dictionary<string, object> data, ulong completeTime)
	{
		data["build_finish_time"] = completeTime;
	}
}
