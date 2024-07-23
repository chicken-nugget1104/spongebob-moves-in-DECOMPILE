using System;
using System.Collections.Generic;

// Token: 0x0200040C RID: 1036
public class ActivatableDecorator : EntityDecorator
{
	// Token: 0x06001FB6 RID: 8118 RVA: 0x000C2174 File Offset: 0x000C0374
	public ActivatableDecorator(Entity toDecorate) : base(toDecorate)
	{
	}

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06001FB7 RID: 8119 RVA: 0x000C2180 File Offset: 0x000C0380
	// (set) Token: 0x06001FB8 RID: 8120 RVA: 0x000C21B0 File Offset: 0x000C03B0
	public ulong Activated
	{
		get
		{
			object obj;
			if (this.Variable.TryGetValue("activatedTime", out obj))
			{
				return (ulong)obj;
			}
			return 0UL;
		}
		set
		{
			this.Variable["activatedTime"] = value;
		}
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000C21C8 File Offset: 0x000C03C8
	public override void DeserializeDecorator(Dictionary<string, object> data)
	{
		if (data.ContainsKey("activated_time"))
		{
			this.Activated = TFUtils.LoadUlong(data, "activated_time", 0UL);
		}
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x000C21F0 File Offset: 0x000C03F0
	public override void SerializeDecorator(ref Dictionary<string, object> data)
	{
		data["activated_time"] = this.Activated;
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x000C220C File Offset: 0x000C040C
	public static void Serialize(ref Dictionary<string, object> data, ulong startTime)
	{
		data["activated_time"] = startTime;
	}
}
