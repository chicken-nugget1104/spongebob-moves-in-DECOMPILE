using System;
using MTools;
using UnityEngine;

// Token: 0x020003AB RID: 939
public static class SoaringTime
{
	// Token: 0x17000397 RID: 919
	// (get) Token: 0x06001B17 RID: 6935 RVA: 0x000AFF1C File Offset: 0x000AE11C
	public static long LastServerTime
	{
		get
		{
			return SoaringTime.sSoaring_LastServerTime + SoaringTime.sSoaring_ServerTimeOffset;
		}
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x06001B18 RID: 6936 RVA: 0x000AFF2C File Offset: 0x000AE12C
	public static long AdjustedServerTime
	{
		get
		{
			long lastServerTime = SoaringTime.LastServerTime;
			if (lastServerTime == 0L)
			{
				return SoaringTime.UserCurrentUTCUnixTime;
			}
			long num = SoaringTime.CurrentDeviceTimeSinceBoot - SoaringTime.sRelative_LastServerUpdateTime;
			return num + lastServerTime + SoaringTime.sDevice_SystemTimeDiff;
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x06001B19 RID: 6937 RVA: 0x000AFF60 File Offset: 0x000AE160
	public static DateTime Epoch
	{
		get
		{
			return SoaringTime.EpochTime;
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06001B1A RID: 6938 RVA: 0x000AFF68 File Offset: 0x000AE168
	public static long UserCurrentUTCUnixTime
	{
		get
		{
			return (long)(DateTime.UtcNow - SoaringTime.EpochTime).TotalSeconds;
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06001B1B RID: 6939 RVA: 0x000AFF90 File Offset: 0x000AE190
	public static long CurrentDeviceTimeSinceBoot
	{
		get
		{
			return SoaringPlatform.SystemTimeSinceBootTime();
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06001B1C RID: 6940 RVA: 0x000AFF98 File Offset: 0x000AE198
	public static long DeviceBootTime
	{
		get
		{
			return SoaringTime.sDevice_SystemBootTime + SoaringTime.sDevice_SystemTimeDiff;
		}
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x000AFFA8 File Offset: 0x000AE1A8
	private static DateTime ServerAdjustedDateTime()
	{
		return SoaringTime.EpochTime.AddSeconds((double)SoaringTime.AdjustedServerTime);
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x000AFFBC File Offset: 0x000AE1BC
	internal static void Register()
	{
		SoaringInternal.instance.RegisterModule(new SoaringServerTimeModule());
	}

	// Token: 0x06001B1F RID: 6943 RVA: 0x000AFFD0 File Offset: 0x000AE1D0
	internal static void UpdateServerTime(long l)
	{
		if ((SoaringTime.sDevice_SystemTimeDiff != 0L || SoaringTime.sDevice_SystemBootTime == 0L) && l > 0L)
		{
			SoaringTime.sDevice_SystemBootTime = SoaringPlatform.SystemBootTime();
			SoaringTime.sDevice_SystemTimeDiff = 0L;
		}
		SoaringTime.sRelative_LastServerUpdateTime = SoaringTime.CurrentDeviceTimeSinceBoot;
		SoaringTime.sSoaring_LastServerTime = l - SoaringTime.sSoaring_ServerTimeOffset;
		SoaringTime.Save();
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x000B0028 File Offset: 0x000AE228
	internal static void Load()
	{
		bool flag = false;
		try
		{
			MBinaryReader fileStream = ResourceUtils.GetFileStream("SoaringTime", "Soaring", "dat", 1);
			if (fileStream != null && fileStream.IsOpen())
			{
				int num = fileStream.ReadInt();
				if (num == 3)
				{
					SoaringTime.mTimeHackProbability = 0f;
					SoaringTime.sDevice_SystemTimeDiff = 0L;
					SoaringTime.sSoaring_LastServerTime = fileStream.ReadLong();
					SoaringTime.sRelative_LastServerUpdateTime = fileStream.ReadLong();
					SoaringTime.sDevice_SystemBootTime = fileStream.ReadLong();
					SoaringTime.mUTCOffset = fileStream.ReadInt();
					long num2 = fileStream.ReadLong();
					TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
					int num3 = (int)currentTimeZone.GetUtcOffset(DateTime.Now).TotalHours;
					if (num3 != SoaringTime.mUTCOffset)
					{
					}
					long num4 = SoaringPlatform.SystemBootTime();
					if (SoaringTime.sDevice_SystemBootTime == 0L)
					{
						SoaringTime.sDevice_SystemBootTime = num4;
					}
					SoaringTime.sDevice_SystemTimeDiff = num4 - SoaringTime.sDevice_SystemBootTime;
					if (SoaringTime.sDevice_SystemTimeDiff - 240L < 0L)
					{
						SoaringDebug.Log(string.Concat(new object[]
						{
							"Timestamp Missmatch: Time: Difference: ",
							SoaringTime.sDevice_SystemTimeDiff,
							" | ",
							num4,
							" | ",
							SoaringTime.sDevice_SystemBootTime
						}), LogType.Error);
						SoaringTime.mTimeHackProbability += (float)(5L * (SoaringTime.sDevice_SystemTimeDiff / 240L));
					}
					if (SoaringTime.AdjustedServerTime < num2)
					{
						SoaringDebug.Log(string.Concat(new object[]
						{
							"Timestamp Missmatch: Ajusted Time: ",
							SoaringTime.AdjustedServerTime,
							" < ",
							num2
						}), LogType.Error);
						SoaringTime.mTimeHackProbability += 10f;
					}
					fileStream.Close();
					flag = true;
				}
			}
		}
		catch
		{
		}
		if (!flag)
		{
			SoaringTime.SetDefaults();
		}
		SoaringDebug.Log(string.Concat(new object[]
		{
			"Timestamp: Diff: ",
			SoaringTime.sDevice_SystemTimeDiff,
			" Last: ",
			SoaringTime.sSoaring_LastServerTime,
			" Device Boot: ",
			SoaringTime.sDevice_SystemBootTime,
			" Current Since: ",
			SoaringTime.CurrentDeviceTimeSinceBoot,
			" Adjusted: ",
			SoaringTime.AdjustedServerTime,
			" Loaded: ",
			flag
		}), LogType.Log);
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x000B0298 File Offset: 0x000AE498
	private static void Save()
	{
		try
		{
			string writePath = ResourceUtils.GetWritePath("SoaringTime.dat", "Soaring", 1);
			MBinaryWriter mbinaryWriter = new MBinaryWriter();
			if (mbinaryWriter.Open(writePath, true, true, "bak") && mbinaryWriter.IsOpen())
			{
				mbinaryWriter.Write(3);
				mbinaryWriter.Write(SoaringTime.sSoaring_LastServerTime);
				mbinaryWriter.Write(SoaringTime.sRelative_LastServerUpdateTime);
				mbinaryWriter.Write(SoaringTime.sDevice_SystemBootTime);
				mbinaryWriter.Write(SoaringTime.mUTCOffset);
				mbinaryWriter.Write(SoaringTime.AdjustedServerTime);
				mbinaryWriter.Flush();
				mbinaryWriter.Close();
			}
		}
		catch
		{
		}
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x000B0350 File Offset: 0x000AE550
	private static void SetDefaults()
	{
		SoaringTime.UpdateServerTime(SoaringTime.UserCurrentUTCUnixTime);
	}

	// Token: 0x040011E4 RID: 4580
	private const int cTimeVariance = 240;

	// Token: 0x040011E5 RID: 4581
	private const int cTimezoneVariance = 1;

	// Token: 0x040011E6 RID: 4582
	private const int cTimehackTolerance = 25;

	// Token: 0x040011E7 RID: 4583
	private static DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

	// Token: 0x040011E8 RID: 4584
	private static long sSoaring_ServerTimeOffset = 0L;

	// Token: 0x040011E9 RID: 4585
	private static long sSoaring_LastServerTime = 0L;

	// Token: 0x040011EA RID: 4586
	private static long sRelative_LastServerUpdateTime = 0L;

	// Token: 0x040011EB RID: 4587
	private static long sDevice_SystemBootTime = 0L;

	// Token: 0x040011EC RID: 4588
	private static long sDevice_SystemTimeDiff = 0L;

	// Token: 0x040011ED RID: 4589
	private static int mUTCOffset = 0;

	// Token: 0x040011EE RID: 4590
	private static float mTimeHackProbability = 0f;
}
