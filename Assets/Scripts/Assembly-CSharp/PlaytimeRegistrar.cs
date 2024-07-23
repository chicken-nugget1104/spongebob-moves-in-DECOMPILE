using System;
using System.Collections.Generic;

// Token: 0x020001AA RID: 426
public class PlaytimeRegistrar
{
	// Token: 0x06000E3C RID: 3644 RVA: 0x00056D2C File Offset: 0x00054F2C
	public PlaytimeRegistrar(int level, ulong walltimeStartUtc, ulong lastTouchedUtc, ulong accruedPlaytimeAtLevelSeconds)
	{
		this.playtimeAtLevel = accruedPlaytimeAtLevelSeconds;
		this.walltimeLevelStart = walltimeStartUtc;
		this.lastPlaytimeCheckin = lastTouchedUtc;
		this.level = level;
	}

	// Token: 0x06000E3D RID: 3645 RVA: 0x00056D54 File Offset: 0x00054F54
	public ulong GetWalltimeLevelStartUtc(int level)
	{
		TFUtils.Assert(this.level == level, "You can only get walltime for the current/last level! Querying level=" + level);
		return this.walltimeLevelStart;
	}

	// Token: 0x06000E3E RID: 3646 RVA: 0x00056D88 File Offset: 0x00054F88
	public ulong GetPlaytimeAtLevelSeconds(int level)
	{
		TFUtils.Assert(this.level == level, "You can only get playtime at the current/last level!");
		return this.playtimeAtLevel;
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00056DA4 File Offset: 0x00054FA4
	public static bool IsTimeout(ulong utcLast, ulong utcNow, out ulong delta)
	{
		delta = utcNow - utcLast;
		return delta > 300UL;
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x00056DC0 File Offset: 0x00054FC0
	public void Process(PersistedTriggerableAction action, int levelNow, SBAnalytics analytics)
	{
		ulong num = TFUtils.EpochTime();
		bool flag = action is LevelUpAction;
		if (action.IsUserInitiated || flag)
		{
			this.UpdatePlaytime(num);
		}
		while (flag && levelNow > this.level)
		{
			ulong num2 = num - this.walltimeLevelStart;
			ulong walltimeMinutes = Convert.ToUInt64(num2 / 60.0);
			ulong playtimeMinutes = Convert.ToUInt64(this.playtimeAtLevel / 60.0);
			analytics.LogLevelPlaytime(this.level, walltimeMinutes, playtimeMinutes);
			this.level++;
			this.playtimeAtLevel = 0UL;
			this.walltimeLevelStart = num;
		}
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x00056E70 File Offset: 0x00055070
	public void UpdateLevel(int level, ulong startUtc)
	{
		this.level = level;
		this.walltimeLevelStart = startUtc;
		this.playtimeAtLevel = 0UL;
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x00056E88 File Offset: 0x00055088
	public void UpdatePlaytime(ulong nowUtc)
	{
		ulong num;
		if (!PlaytimeRegistrar.IsTimeout(this.lastPlaytimeCheckin, nowUtc, out num))
		{
			this.playtimeAtLevel += num;
		}
		this.lastPlaytimeCheckin = nowUtc;
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x00056EC0 File Offset: 0x000550C0
	public static PlaytimeRegistrar FromDict(Dictionary<string, object> data)
	{
		ulong walltimeStartUtc = TFUtils.LoadUlong(data, "wts_start", 0UL);
		ulong lastTouchedUtc = TFUtils.LoadUlong(data, "last", 0UL);
		ulong accruedPlaytimeAtLevelSeconds = TFUtils.LoadUlong(data, "time_at", 0UL);
		int num = TFUtils.LoadInt(data, "level");
		return new PlaytimeRegistrar(num, walltimeStartUtc, lastTouchedUtc, accruedPlaytimeAtLevelSeconds);
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x00056F10 File Offset: 0x00055110
	public static void ApplyToGameState(ref Dictionary<string, object> gamestate, int level, ulong walltimeLevelStartUtc, ulong lastTouchedUtc, ulong playtimeAtLevelSeconds)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["level"] = level;
		dictionary["wts_start"] = walltimeLevelStartUtc;
		dictionary["last"] = lastTouchedUtc;
		dictionary["time_at"] = playtimeAtLevelSeconds;
		gamestate["playtime"] = dictionary;
	}

	// Token: 0x0400095C RID: 2396
	public const string PLAYTIME = "playtime";

	// Token: 0x0400095D RID: 2397
	public const string LEVEL = "level";

	// Token: 0x0400095E RID: 2398
	public const string WALLTIME_START = "wts_start";

	// Token: 0x0400095F RID: 2399
	public const string LAST_TOUCHED = "last";

	// Token: 0x04000960 RID: 2400
	public const string PLAYTIME_AT_LEVEL = "time_at";

	// Token: 0x04000961 RID: 2401
	private ulong playtimeAtLevel;

	// Token: 0x04000962 RID: 2402
	private ulong lastPlaytimeCheckin;

	// Token: 0x04000963 RID: 2403
	private ulong walltimeLevelStart;

	// Token: 0x04000964 RID: 2404
	private int level;
}
