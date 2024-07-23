using System;
using UnityEngine;

namespace com.amazon.device.iap.cpt.log
{
	// Token: 0x02000002 RID: 2
	public class AmazonLogging
	{
		// Token: 0x06000002 RID: 2 RVA: 0x000020F4 File Offset: 0x000002F4
		public static void LogError(AmazonLogging.AmazonLoggingLevel reportLevel, string service, string message)
		{
			if (reportLevel == AmazonLogging.AmazonLoggingLevel.Silent)
			{
				return;
			}
			string message2 = string.Format("{0} error: {1}", service, message);
			switch (reportLevel)
			{
			case AmazonLogging.AmazonLoggingLevel.Critical:
			case AmazonLogging.AmazonLoggingLevel.Errors:
			case AmazonLogging.AmazonLoggingLevel.Warnings:
			case AmazonLogging.AmazonLoggingLevel.Verbose:
				Debug.LogError(message2);
				break;
			case AmazonLogging.AmazonLoggingLevel.ErrorsAsExceptions:
				throw new Exception(message2);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002150 File Offset: 0x00000350
		public static void LogWarning(AmazonLogging.AmazonLoggingLevel reportLevel, string service, string message)
		{
			switch (reportLevel)
			{
			case AmazonLogging.AmazonLoggingLevel.Warnings:
			case AmazonLogging.AmazonLoggingLevel.Verbose:
				Debug.LogWarning(string.Format("{0} warning: {1}", service, message));
				break;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021A0 File Offset: 0x000003A0
		public static void Log(AmazonLogging.AmazonLoggingLevel reportLevel, string service, string message)
		{
			if (reportLevel != AmazonLogging.AmazonLoggingLevel.Verbose)
			{
				return;
			}
			Debug.Log(string.Format("{0}: {1}", service, message));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021BC File Offset: 0x000003BC
		public static AmazonLogging.SDKLoggingLevel pluginToSDKLoggingLevel(AmazonLogging.AmazonLoggingLevel pluginLoggingLevel)
		{
			switch (pluginLoggingLevel)
			{
			case AmazonLogging.AmazonLoggingLevel.Silent:
				return AmazonLogging.SDKLoggingLevel.LogOff;
			case AmazonLogging.AmazonLoggingLevel.Critical:
				return AmazonLogging.SDKLoggingLevel.LogCritical;
			case AmazonLogging.AmazonLoggingLevel.ErrorsAsExceptions:
			case AmazonLogging.AmazonLoggingLevel.Errors:
				return AmazonLogging.SDKLoggingLevel.LogError;
			case AmazonLogging.AmazonLoggingLevel.Warnings:
			case AmazonLogging.AmazonLoggingLevel.Verbose:
				return AmazonLogging.SDKLoggingLevel.LogWarning;
			default:
				return AmazonLogging.SDKLoggingLevel.LogWarning;
			}
		}

		// Token: 0x04000001 RID: 1
		private const string errorMessage = "{0} error: {1}";

		// Token: 0x04000002 RID: 2
		private const string warningMessage = "{0} warning: {1}";

		// Token: 0x04000003 RID: 3
		private const string logMessage = "{0}: {1}";

		// Token: 0x02000003 RID: 3
		public enum AmazonLoggingLevel
		{
			// Token: 0x04000005 RID: 5
			Silent,
			// Token: 0x04000006 RID: 6
			Critical,
			// Token: 0x04000007 RID: 7
			ErrorsAsExceptions,
			// Token: 0x04000008 RID: 8
			Errors,
			// Token: 0x04000009 RID: 9
			Warnings,
			// Token: 0x0400000A RID: 10
			Verbose
		}

		// Token: 0x02000004 RID: 4
		public enum SDKLoggingLevel
		{
			// Token: 0x0400000C RID: 12
			LogOff,
			// Token: 0x0400000D RID: 13
			LogCritical,
			// Token: 0x0400000E RID: 14
			LogError,
			// Token: 0x0400000F RID: 15
			LogWarning
		}
	}
}
