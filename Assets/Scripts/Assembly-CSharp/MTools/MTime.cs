using System;

namespace MTools
{
	// Token: 0x020003BE RID: 958
	public static class MTime
	{
		// Token: 0x06001C42 RID: 7234 RVA: 0x000B4AA8 File Offset: 0x000B2CA8
		public static void LoadCurrentTime()
		{
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000B4AAC File Offset: 0x000B2CAC
		public static void SetCurrentTime(ulong cTime)
		{
			DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			d.AddMilliseconds(MTime.sCurrentTime);
			double num = (d - MTime.sOriginTimeStamp).TotalSeconds;
			if (num > MTime.sCurrentTime)
			{
				MTime.sCurrentTime = num;
			}
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000B4B00 File Offset: 0x000B2D00
		public static bool ConstantTimestampWithinDays(int days, long ts)
		{
			long num = MTime.ConstantTimeStamp();
			return ts - 86400L * (long)days <= num && ts >= num;
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000B4B30 File Offset: 0x000B2D30
		public static long CurrentTimeSinceEpoch()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x000B4B64 File Offset: 0x000B2D64
		public static long TimeSinceEpoch(string time)
		{
			return (long)(DateTime.Parse(time) - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x000B4B98 File Offset: 0x000B2D98
		public static long TimeSinceEpoch(DateTime time)
		{
			return (long)(time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x000B4BC8 File Offset: 0x000B2DC8
		public static long ParseForEasternTime(string time)
		{
			return MTime.ParseForTimeZone(time, -5);
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000B4BD4 File Offset: 0x000B2DD4
		public static long ParseForTimeZone(string parsetime, int timezone)
		{
			DateTime dateTime = DateTime.Parse(parsetime);
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, DateTimeKind.Utc);
			}
			long num = MTime.ConstantTimeStampFromDate(dateTime);
			return num + 3600L * (long)timezone;
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x000B4C3C File Offset: 0x000B2E3C
		public static long ParseForTimeZoneSinceEpoch(string parsetime, int timezone)
		{
			DateTime time = DateTime.Parse(parsetime);
			if (time.Kind != DateTimeKind.Utc)
			{
				time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Utc);
			}
			long num = MTime.TimeSinceEpoch(time);
			return num + 3600L * (long)timezone;
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x000B4CA4 File Offset: 0x000B2EA4
		public static long ConstantTimeStamp()
		{
			return (long)(DateTime.UtcNow - MTime.sOriginTimeStamp).TotalSeconds;
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000B4CCC File Offset: 0x000B2ECC
		public static double ConstantTimeStampPrecise()
		{
			return (DateTime.UtcNow - MTime.sOriginTimeStamp).TotalSeconds;
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000B4CF0 File Offset: 0x000B2EF0
		public static long ConstantTimeStampFromTime(int year, int month, int day)
		{
			return (long)(new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc) - MTime.sOriginTimeStamp).TotalSeconds;
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000B4D1C File Offset: 0x000B2F1C
		public static double ConstantTimeStampFromTimePrecise(int year, int month, int day)
		{
			return (new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc) - MTime.sOriginTimeStamp).TotalSeconds;
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000B4D48 File Offset: 0x000B2F48
		public static long ConstantTimeStampFromDate(DateTime dateTime)
		{
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
			}
			return (long)(dateTime - MTime.sOriginTimeStamp).TotalSeconds;
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000B4D88 File Offset: 0x000B2F88
		public static double ConstantTimeStampFromDatePrecise(DateTime dateTime)
		{
			if (dateTime.Kind != DateTimeKind.Utc)
			{
				dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
			}
			return (dateTime - MTime.sOriginTimeStamp).TotalSeconds;
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x000B4DC4 File Offset: 0x000B2FC4
		public static long LocalTimeStamp()
		{
			return (long)(DateTime.UtcNow - MTime.sLocalOriginTimestamp).TotalSeconds;
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x000B4DEC File Offset: 0x000B2FEC
		public static double LocalTimeStampPrecise()
		{
			return (DateTime.UtcNow - MTime.sLocalOriginTimestamp).TotalSeconds;
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x000B4E10 File Offset: 0x000B3010
		public static long GenerateTimestampForSave(long timestamp)
		{
			long num = timestamp + (long)((ulong)MTime.sTimeStampAdjust);
			long num2 = (timestamp & 255L) >> 24;
			long num3 = timestamp & 255L;
			num = (timestamp & 72057594037927680L);
			num |= num2;
			return num | num3 << 24;
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x000B4E54 File Offset: 0x000B3054
		public static long GenerateTimestampForSave()
		{
			long num = MTime.ConstantTimeStamp() + (long)((ulong)MTime.sTimeStampAdjust);
			long num2 = (num & 255L) >> 24;
			long num3 = num & 255L;
			num &= 72057594037927680L;
			num |= num2;
			return num | num3 << 24;
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000B4E9C File Offset: 0x000B309C
		public static long ExtractTimestampFromSave(long timestamp)
		{
			long num = (timestamp & 255L) >> 24;
			long num2 = timestamp & 255L;
			long num3 = timestamp & 72057594037927680L;
			num3 |= num;
			num3 |= num2 << 24;
			return num3 - (long)((ulong)MTime.sTimeStampAdjust);
		}

		// Token: 0x04001239 RID: 4665
		public const uint cSecondsInDay = 86400U;

		// Token: 0x0400123A RID: 4666
		public const uint cSecondsInHour = 3600U;

		// Token: 0x0400123B RID: 4667
		public const int TimeZone_UTC = 0;

		// Token: 0x0400123C RID: 4668
		public const int TimeZone_Eastern = -5;

		// Token: 0x0400123D RID: 4669
		public const int TimeZone_Central = -6;

		// Token: 0x0400123E RID: 4670
		public const int TimeZone_Mountain = -7;

		// Token: 0x0400123F RID: 4671
		public const int TimeZone_Pacific = -7;

		// Token: 0x04001240 RID: 4672
		private static DateTime sOriginTimeStamp = new DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04001241 RID: 4673
		private static DateTime sLocalOriginTimestamp = DateTime.UtcNow;

		// Token: 0x04001242 RID: 4674
		private static uint sTimeStampAdjust = 11411671U;

		// Token: 0x04001243 RID: 4675
		private static double sCurrentTime = 0.0;
	}
}
