using System;
using System.Collections.Generic;

// Token: 0x020000E9 RID: 233
public abstract class PersistedTriggerableAction : PersistedActionBuffer.PersistedAction, ITriggerable
{
	// Token: 0x06000883 RID: 2179 RVA: 0x00037154 File Offset: 0x00035354
	public PersistedTriggerableAction(string type, Identity target) : base(type, target)
	{
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000884 RID: 2180
	public abstract bool IsUserInitiated { get; }

	// Token: 0x06000885 RID: 2181 RVA: 0x0003716C File Offset: 0x0003536C
	public override void Process(Game game)
	{
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00037170 File Offset: 0x00035370
	public override void Apply(Game game, ulong utcNow)
	{
		if (this.IsUserInitiated)
		{
			game.playtimeRegistrar.UpdatePlaytime(base.GetTime());
		}
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00037190 File Offset: 0x00035390
	public override void Confirm(Dictionary<string, object> gameState)
	{
		if (this.IsUserInitiated)
		{
			Dictionary<string, object> dictionary = TFUtils.LoadDict(gameState, "playtime");
			ulong num = TFUtils.LoadUlong(dictionary, "time_at", 0UL);
			ulong utcLast = TFUtils.LoadUlong(dictionary, "last", 0UL);
			ulong num2;
			if (!PlaytimeRegistrar.IsTimeout(utcLast, base.GetTime(), out num2))
			{
				num += num2;
				dictionary["time_at"] = num;
			}
			dictionary["last"] = base.GetTime();
		}
		base.Confirm(gameState);
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00037218 File Offset: 0x00035418
	public virtual ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return Trigger.Null;
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00037220 File Offset: 0x00035420
	public virtual ITrigger CreateTrigger(string type)
	{
		return Trigger.Null;
	}

	// Token: 0x04000621 RID: 1569
	protected TriggerableMixin triggerable = new TriggerableMixin();
}
