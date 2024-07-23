using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class AmazonLogging
{
	// Token: 0x06000164 RID: 356 RVA: 0x00006A7C File Offset: 0x00004C7C
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

	// Token: 0x06000165 RID: 357 RVA: 0x00006AD8 File Offset: 0x00004CD8
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

	// Token: 0x06000166 RID: 358 RVA: 0x00006B28 File Offset: 0x00004D28
	public static void Log(AmazonLogging.AmazonLoggingLevel reportLevel, string service, string message)
	{
		if (reportLevel != AmazonLogging.AmazonLoggingLevel.Verbose)
		{
			return;
		}
		Debug.Log(string.Format("{0}: {1}", service, message));
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00006B44 File Offset: 0x00004D44
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

	// Token: 0x0400005D RID: 93
	private const string errorMessage = "{0} error: {1}";

	// Token: 0x0400005E RID: 94
	private const string warningMessage = "{0} warning: {1}";

	// Token: 0x0400005F RID: 95
	private const string logMessage = "{0}: {1}";

	// Token: 0x02000028 RID: 40
	public enum AmazonLoggingLevel
	{
		// Token: 0x04000061 RID: 97
		Silent,
		// Token: 0x04000062 RID: 98
		Critical,
		// Token: 0x04000063 RID: 99
		ErrorsAsExceptions,
		// Token: 0x04000064 RID: 100
		Errors,
		// Token: 0x04000065 RID: 101
		Warnings,
		// Token: 0x04000066 RID: 102
		Verbose
	}

	// Token: 0x02000029 RID: 41
	public enum SDKLoggingLevel
	{
		// Token: 0x04000068 RID: 104
		LogOff,
		// Token: 0x04000069 RID: 105
		LogCritical,
		// Token: 0x0400006A RID: 106
		LogError,
		// Token: 0x0400006B RID: 107
		LogWarning
	}
}
