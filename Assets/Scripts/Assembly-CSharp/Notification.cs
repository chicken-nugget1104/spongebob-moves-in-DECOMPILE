using System;
using System.Collections.Generic;

// Token: 0x020001A4 RID: 420
public class Notification
{
	// Token: 0x06000DF8 RID: 3576 RVA: 0x00054F20 File Offset: 0x00053120
	public Notification(string message, string sound, LoadableCondition loadableCondition)
	{
		this.message = message;
		this.sound = sound;
		this.loadableCondition = loadableCondition;
		this.Reset();
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x00054F44 File Offset: 0x00053144
	public static Notification FromDict(Dictionary<string, object> data)
	{
		LoadableCondition loadableCondition = (LoadableCondition)ConditionFactory.FromDict((Dictionary<string, object>)data["conditions"]);
		string text = Language.Get(TFUtils.LoadString(data, "message"));
		string text2 = TFUtils.TryLoadString(data, "notification_sound");
		return new Notification(text, text2, loadableCondition);
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x00054F94 File Offset: 0x00053194
	public void Reset()
	{
		this.conditions = new ConditionState(this.loadableCondition);
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x00054FA8 File Offset: 0x000531A8
	public int Send(DateTime fireDate, string label)
	{
		long delaySeconds = (fireDate.Ticks - DateTime.Now.Ticks) / 10000000L + 1L;
		return NotificationManager.SendNotification(this.message, delaySeconds, string.Empty, string.Empty);
	}

	// Token: 0x0400093C RID: 2364
	public string message;

	// Token: 0x0400093D RID: 2365
	public string sound;

	// Token: 0x0400093E RID: 2366
	public ConditionState conditions;

	// Token: 0x0400093F RID: 2367
	private LoadableCondition loadableCondition;
}
