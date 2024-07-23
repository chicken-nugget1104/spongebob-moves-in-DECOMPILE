using System;
using System.Collections.Generic;

// Token: 0x020001B0 RID: 432
public class Quest
{
	// Token: 0x06000E70 RID: 3696 RVA: 0x00058F68 File Offset: 0x00057168
	public Quest(uint did, ConditionalProgress startProgress, ConditionalProgress endProgress, ulong? startTime, ulong? completionTime, bool triggeredAlready)
	{
		this.did = did;
		this.startTime = startTime;
		this.completionTime = completionTime;
		this.startProgress = startProgress;
		this.endProgress = endProgress;
		this.triggeredReminder = triggeredAlready;
		this.endConditions = new List<ConditionState>();
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000E71 RID: 3697 RVA: 0x00058FB4 File Offset: 0x000571B4
	public uint Did
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000E72 RID: 3698 RVA: 0x00058FBC File Offset: 0x000571BC
	// (set) Token: 0x06000E73 RID: 3699 RVA: 0x00058FC4 File Offset: 0x000571C4
	public ConditionState StartConditions
	{
		get
		{
			return this.startConditions;
		}
		set
		{
			this.startConditions = value;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000E74 RID: 3700 RVA: 0x00058FD0 File Offset: 0x000571D0
	public List<ConditionState> EndConditions
	{
		get
		{
			return this.endConditions;
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00058FD8 File Offset: 0x000571D8
	// (set) Token: 0x06000E76 RID: 3702 RVA: 0x00058FE0 File Offset: 0x000571E0
	public ConditionalProgress StartProgress
	{
		get
		{
			return this.startProgress;
		}
		set
		{
			this.startProgress = value;
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00058FEC File Offset: 0x000571EC
	// (set) Token: 0x06000E78 RID: 3704 RVA: 0x00058FF4 File Offset: 0x000571F4
	public ConditionalProgress EndProgress
	{
		get
		{
			return this.endProgress;
		}
		set
		{
			this.endProgress = value;
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00059000 File Offset: 0x00057200
	public ulong? StartTime
	{
		get
		{
			return this.startTime;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06000E7A RID: 3706 RVA: 0x00059008 File Offset: 0x00057208
	public ulong? CompletionTime
	{
		get
		{
			return this.completionTime;
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000E7B RID: 3707 RVA: 0x00059010 File Offset: 0x00057210
	// (set) Token: 0x06000E7C RID: 3708 RVA: 0x00059018 File Offset: 0x00057218
	public bool TriggeredReminder
	{
		get
		{
			return this.triggeredReminder;
		}
		set
		{
			this.triggeredReminder = value;
		}
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x00059024 File Offset: 0x00057224
	public void Start(ulong utcTime)
	{
		this.startTime = new ulong?(utcTime);
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x00059034 File Offset: 0x00057234
	public void Complete(ulong utcTime)
	{
		this.completionTime = new ulong?(utcTime);
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000E7F RID: 3711 RVA: 0x00059044 File Offset: 0x00057244
	public string TrackerTag
	{
		get
		{
			return "quest_" + this.did;
		}
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0005905C File Offset: 0x0005725C
	public static Quest FromDict(Dictionary<string, object> data)
	{
		uint num = TFUtils.LoadUint(data, "did");
		Dictionary<string, object> dictionary = TFUtils.LoadDict(data, "conditions");
		if (dictionary.Count == 0)
		{
			return null;
		}
		ConditionalProgressSerializer conditionalProgressSerializer = new ConditionalProgressSerializer();
		ConditionalProgress conditionalProgress = conditionalProgressSerializer.DeserializeProgress(TFUtils.LoadList<object>(dictionary, "met_start_condition_ids"));
		ConditionalProgress conditionalProgress2 = conditionalProgressSerializer.DeserializeProgress(TFUtils.LoadList<object>(dictionary, "met_end_condition_ids"));
		ulong? num2 = TFUtils.LoadNullableUlong(data, "start_time");
		ulong? num3 = TFUtils.LoadNullableUlong(data, "completion_time");
		bool triggeredAlready = false;
		bool? flag = TFUtils.TryLoadBool(data, "reminded");
		if (flag != null)
		{
			triggeredAlready = flag.Value;
		}
		if (num2 == null && num3 != null)
		{
			num2 = num3;
		}
		return new Quest(num, conditionalProgress, conditionalProgress2, num2, num3, triggeredAlready);
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x00059128 File Offset: 0x00057328
	public Dictionary<string, object> ToDict()
	{
		TFUtils.Assert(this.startConditions != null && this.endConditions != null, "Quest object not valid. Cannot hydrate properly.");
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		ConditionalProgressSerializer conditionalProgressSerializer = new ConditionalProgressSerializer();
		ConditionalProgress progress = this.startConditions.Dehydrate();
		dictionary2["met_start_condition_ids"] = conditionalProgressSerializer.SerializeProgress(progress);
		ConditionalProgress progress2 = ConditionState.DehydrateChunks(this.endConditions);
		dictionary2["met_end_condition_ids"] = conditionalProgressSerializer.SerializeProgress(progress2);
		dictionary["conditions"] = dictionary2;
		dictionary["start_time"] = TFUtils.NullableToObject(this.startTime);
		dictionary["completion_time"] = TFUtils.NullableToObject(this.completionTime);
		dictionary["did"] = this.did;
		dictionary["reminded"] = this.triggeredReminder;
		return dictionary;
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x00059210 File Offset: 0x00057410
	public SessionActionTracker InstantiateSessionAction(SessionActionDefinition definition)
	{
		return new SessionActionTracker(definition);
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x00059218 File Offset: 0x00057418
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"[Quest (did=",
			this.did,
			", reminded=",
			this.triggeredReminder,
			", startTime=",
			this.startTime.ToString(),
			", completeTime=",
			this.completionTime.ToString(),
			", startProgress=",
			(this.startProgress == null) ? "null" : this.startProgress.ToString(),
			", endProgress=",
			(this.endProgress == null) ? "null" : this.endProgress.ToString(),
			", startConditions=",
			(this.startConditions == null) ? "null" : this.startConditions.ToString(),
			", endConditions=",
			(this.endConditions == null) ? "null" : this.endConditions.ToString(),
			")]"
		});
	}

	// Token: 0x04000983 RID: 2435
	private uint did;

	// Token: 0x04000984 RID: 2436
	private ConditionalProgress startProgress;

	// Token: 0x04000985 RID: 2437
	private ConditionalProgress endProgress;

	// Token: 0x04000986 RID: 2438
	private ConditionState startConditions;

	// Token: 0x04000987 RID: 2439
	private List<ConditionState> endConditions;

	// Token: 0x04000988 RID: 2440
	private ulong? startTime;

	// Token: 0x04000989 RID: 2441
	private ulong? completionTime;

	// Token: 0x0400098A RID: 2442
	private bool triggeredReminder;
}
