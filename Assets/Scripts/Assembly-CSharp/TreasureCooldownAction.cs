using System;
using System.Collections.Generic;

// Token: 0x02000106 RID: 262
public class TreasureCooldownAction : PersistedTriggerableAction
{
	// Token: 0x06000951 RID: 2385 RVA: 0x0003A998 File Offset: 0x00038B98
	public TreasureCooldownAction(ulong nextTime, string persistName) : base("tt", Identity.Null())
	{
		this.nextTreasureTime = nextTime;
		this.persistName = persistName;
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000952 RID: 2386 RVA: 0x0003A9B8 File Offset: 0x00038BB8
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0003A9BC File Offset: 0x00038BBC
	public new static TreasureCooldownAction FromDict(Dictionary<string, object> data)
	{
		ulong nextTime = TFUtils.LoadUlong(data, "time", 0UL);
		string text = TFUtils.TryLoadString(data, "persistName");
		if (text == null)
		{
			text = "time_to_spawn";
		}
		return new TreasureCooldownAction(nextTime, text);
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0003A9F8 File Offset: 0x00038BF8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["time"] = this.nextTreasureTime;
		dictionary["persistName"] = this.persistName;
		return dictionary;
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0003AA34 File Offset: 0x00038C34
	public override void Apply(Game game, ulong utcNow)
	{
		TreasureSpawner treasureSpawner = game.treasureManager.FindTreasureSpawner(this.persistName);
		treasureSpawner.Reset(new ulong?(this.nextTreasureTime));
		base.Apply(game, utcNow);
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0003AA6C File Offset: 0x00038C6C
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (dictionary.ContainsKey("treasure_state"))
		{
			((Dictionary<string, object>)dictionary["treasure_state"])[this.persistName] = this.nextTreasureTime;
		}
		else
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2[this.persistName] = this.nextTreasureTime;
			((Dictionary<string, object>)gameState["farm"])["treasure_state"] = dictionary2;
		}
		base.Confirm(gameState);
	}

	// Token: 0x0400067D RID: 1661
	public const string TREASURE_TIME = "tt";

	// Token: 0x0400067E RID: 1662
	private ulong nextTreasureTime;

	// Token: 0x0400067F RID: 1663
	private string persistName;
}
