using System;
using System.Diagnostics;
using MTools;
using UnityEngine;

// Token: 0x02000396 RID: 918
public static class SoaringDebug
{
	// Token: 0x06001A32 RID: 6706 RVA: 0x000AB4BC File Offset: 0x000A96BC
	static SoaringDebug()
	{
		SoaringDebug.LogTypesName = new string[5];
		SoaringDebug.LogTypesName[1] = "Assert";
		SoaringDebug.LogTypesName[0] = "Error";
		SoaringDebug.LogTypesName[4] = "Exception";
		SoaringDebug.LogTypesName[3] = "Log";
		SoaringDebug.LogTypesName[2] = "Warning";
		if (SoaringInternal.IsProductionMode)
		{
			SoaringDebug.LogToConsole = false;
			SoaringDebug.LogToFile = false;
			SoaringDebug.LogToHandler = SoaringDebug.LogToHandlerType.none;
		}
		SoaringDebug.EnableLogToConsole(SoaringDebug.LogToConsole);
		SoaringDebug.EnableHandler(SoaringDebug.LogToHandler);
		SoaringDebug.EnableLogToFile(SoaringDebug.LogToFile);
		if (!SoaringInternal.IsProductionMode)
		{
			Debug.Log(string.Concat(new object[]
			{
				"SoaringDebug: LogToConsole ",
				SoaringDebug.LogToConsole,
				" LogToFile: ",
				SoaringDebug.LogToFile,
				" LogToHandler: ",
				SoaringDebug.LogToHandler.ToString()
			}));
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06001A33 RID: 6707 RVA: 0x000AB5C4 File Offset: 0x000A97C4
	public static bool IsLoggingToConsole
	{
		get
		{
			return SoaringDebug.LogToConsole;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06001A34 RID: 6708 RVA: 0x000AB5CC File Offset: 0x000A97CC
	public static bool IsLoggingToFile
	{
		get
		{
			return SoaringDebug.LogToFile;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06001A35 RID: 6709 RVA: 0x000AB5D4 File Offset: 0x000A97D4
	public static bool IsUsingLogToHandler
	{
		get
		{
			return SoaringDebug.LogToHandler != SoaringDebug.LogToHandlerType.none;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06001A36 RID: 6710 RVA: 0x000AB5E4 File Offset: 0x000A97E4
	public static string DebugFileName
	{
		get
		{
			return SoaringDebug.debugFileName;
		}
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x000AB5EC File Offset: 0x000A97EC
	public static void EnableLogToConsole(bool log)
	{
		SoaringDebug.LogToConsole = log;
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x000AB5F4 File Offset: 0x000A97F4
	public static void EnableHandler(SoaringDebug.LogToHandlerType log)
	{
		SoaringDebug.LogToHandler = log;
		if (SoaringDebug.LogToHandler != SoaringDebug.LogToHandlerType.none)
		{
			try
			{
				DateTime utcNow = DateTime.UtcNow;
				SoaringDebug.LogTimeStamp = "_" + utcNow.ToShortDateString() + "_" + utcNow.ToLongTimeString();
				SoaringDebug.LogTimeStamp = SoaringDebug.LogTimeStamp.Replace(' ', '_');
				SoaringDebug.LogTimeStamp = SoaringDebug.LogTimeStamp.Replace(',', '_');
				SoaringDebug.LogTimeStamp = SoaringDebug.LogTimeStamp.Replace('/', '_');
				SoaringDebug.LogTimeStamp = SoaringDebug.LogTimeStamp.Replace('\\', '_');
				SoaringDebug.LogTimeStamp = SoaringDebug.LogTimeStamp.Replace(':', '_');
				Application.RegisterLogCallback(new Application.LogCallback(SoaringDebug.WriteLoggedCallbackHandler));
			}
			catch
			{
				SoaringDebug.LogTimeStamp = string.Empty;
			}
		}
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x000AB6DC File Offset: 0x000A98DC
	public static void EnableLogToFile(bool log)
	{
		SoaringDebug.LogToFile = log;
		if (!SoaringDebug.LogToFile)
		{
			if (SoaringDebug.Writer != null)
			{
				SoaringDebug.Writer.Close();
			}
			SoaringDebug.Writer = null;
		}
		else
		{
			if (SoaringDebug.Writer != null)
			{
				SoaringDebug.Writer.Close();
			}
			SoaringDebug.Writer = null;
			string empty = string.Empty;
			if (SoaringDebug.LogTimeStamp == null)
			{
				SoaringDebug.LogTimeStamp = string.Empty;
			}
			SoaringDebug.debugFileName = "Soaring" + SoaringDebug.LogTimeStamp + ".log";
			string writePath = ResourceUtils.GetWritePath(SoaringDebug.debugFileName, empty + "Soaring/Logs", 8);
			MBinaryWriter mbinaryWriter = new MBinaryWriter();
			if (!mbinaryWriter.Open(writePath, true, true))
			{
				mbinaryWriter = null;
			}
			SoaringDebug.Writer = mbinaryWriter;
		}
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x000AB798 File Offset: 0x000A9998
	public static void Log(string text)
	{
		SoaringDebug.Log(text, LogType.Log);
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x000AB7A4 File Offset: 0x000A99A4
	public static void Log(string text, LogType lType)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (SoaringDebug.LogToFile && SoaringDebug.Writer != null && SoaringDebug.LogToHandler == SoaringDebug.LogToHandlerType.none)
		{
			SoaringDebug.Writer.WriteRawString(string.Concat(new string[]
			{
				"-",
				SoaringDebug.LogTypesName[(int)lType],
				"\nSoaring: ",
				text,
				"\n"
			}));
			SoaringDebug.Writer.Flush();
		}
		if (SoaringDebug.LogToConsole)
		{
			if (lType == LogType.Assert || lType == LogType.Error || lType == LogType.Exception)
			{
				Debug.LogError("Soaring: " + SoaringDebug.LogTypesName[(int)lType] + ": " + text);
			}
			else if (lType == LogType.Warning)
			{
				Debug.LogWarning("Soaring: " + SoaringDebug.LogTypesName[2] + ": " + text);
			}
			else
			{
				Debug.Log("Soaring: " + text);
			}
		}
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x000AB898 File Offset: 0x000A9A98
	private static void WriteLoggedCallbackHandler(string logString, string stackTrace, LogType type)
	{
		if (SoaringDebug.Writer == null)
		{
			return;
		}
		if (logString == null)
		{
			return;
		}
		string text = logString;
		if (SoaringDebug.LogToHandler == SoaringDebug.LogToHandlerType.verbose)
		{
			if (stackTrace != null)
			{
				text = text + "\n" + stackTrace;
			}
			else
			{
				try
				{
					StackTrace stackTrace2 = new StackTrace();
					text = text + "\n" + stackTrace2.ToString();
				}
				catch
				{
				}
			}
		}
		SoaringDebug.Writer.WriteRawString(string.Concat(new string[]
		{
			"-",
			SoaringDebug.LogTypesName[(int)type],
			": ",
			text,
			"\n"
		}));
		SoaringDebug.Writer.Flush();
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x000AB960 File Offset: 0x000A9B60
	public static void DebugListTextures(string stamp)
	{
	}

	// Token: 0x040010F7 RID: 4343
	private static string[] LogTypesName;

	// Token: 0x040010F8 RID: 4344
	private static bool LogToConsole = true;

	// Token: 0x040010F9 RID: 4345
	private static string debugFileName;

	// Token: 0x040010FA RID: 4346
	private static bool LogToFile = true;

	// Token: 0x040010FB RID: 4347
	private static string LogTimeStamp = string.Empty;

	// Token: 0x040010FC RID: 4348
	private static MBinaryWriter Writer;

	// Token: 0x040010FD RID: 4349
	private static SoaringDebug.LogToHandlerType LogToHandler = SoaringDebug.LogToHandlerType.verbose;

	// Token: 0x02000397 RID: 919
	public enum LogToHandlerType
	{
		// Token: 0x040010FF RID: 4351
		none,
		// Token: 0x04001100 RID: 4352
		verbose,
		// Token: 0x04001101 RID: 4353
		brief
	}
}
