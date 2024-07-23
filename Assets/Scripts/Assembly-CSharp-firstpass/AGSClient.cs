using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class AGSClient : MonoBehaviour
{
	// Token: 0x06000205 RID: 517 RVA: 0x0000AC20 File Offset: 0x00008E20
	static AGSClient()
	{
		if (Application.isEditor)
		{
			return;
		}
		AGSClient.JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() == IntPtr.Zero)
			{
				AGSClient.LogGameCircleWarning("No java class " + AGSClient.PROXY_CLASS_NAME + " present, can't use AGSClient");
			}
			else
			{
				AGSClient.JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000206 RID: 518 RVA: 0x0000ACD4 File Offset: 0x00008ED4
	// (remove) Token: 0x06000207 RID: 519 RVA: 0x0000ACEC File Offset: 0x00008EEC
	public static event Action ServiceReadyEvent;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000208 RID: 520 RVA: 0x0000AD04 File Offset: 0x00008F04
	// (remove) Token: 0x06000209 RID: 521 RVA: 0x0000AD1C File Offset: 0x00008F1C
	public static event Action<string> ServiceNotReadyEvent;

	// Token: 0x0600020A RID: 522 RVA: 0x0000AD34 File Offset: 0x00008F34
	public static void Init()
	{
		AGSClient.Init(false, false, false);
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000AD40 File Offset: 0x00008F40
	public static void Init(bool supportsLeaderboards, bool supportsAchievements, bool supportsWhispersync)
	{
		AGSClient.JavaObject.Call("init", new object[]
		{
			supportsLeaderboards,
			supportsAchievements,
			supportsWhispersync
		});
	}

	// Token: 0x0600020C RID: 524 RVA: 0x0000AD80 File Offset: 0x00008F80
	public static void SetPopUpEnabled(bool enabled)
	{
		AGSClient.JavaObject.Call("setPopupEnabled", new object[]
		{
			enabled
		});
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000ADA0 File Offset: 0x00008FA0
	public static void SetPopUpLocation(GameCirclePopupLocation location)
	{
		AGSClient.JavaObject.Call("setPopUpLocation", new object[]
		{
			location.ToString()
		});
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000ADC8 File Offset: 0x00008FC8
	public static void ServiceReady(string empty)
	{
		AGSClient.Log("Client GameCircle - Service is ready");
		AGSClient.IsReady = true;
		if (AGSClient.ServiceReadyEvent != null)
		{
			AGSClient.ServiceReadyEvent();
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x0000ADFC File Offset: 0x00008FFC
	public static bool IsServiceReady()
	{
		return AGSClient.IsReady;
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000AE04 File Offset: 0x00009004
	public static void release()
	{
		AGSClient.JavaObject.Call("release", new object[0]);
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000AE1C File Offset: 0x0000901C
	public static void ServiceNotReady(string param)
	{
		AGSClient.IsReady = false;
		if (AGSClient.ServiceNotReadyEvent != null)
		{
			AGSClient.ServiceNotReadyEvent(param);
		}
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000AE3C File Offset: 0x0000903C
	public static void ShowGameCircleOverlay()
	{
		AGSClient.JavaObject.Call("showGameCircleOverlay", new object[0]);
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000AE54 File Offset: 0x00009054
	public static void LogGameCircleError(string errorMessage)
	{
		AmazonLogging.LogError(AmazonLogging.AmazonLoggingLevel.Verbose, "AmazonGameCircle", errorMessage);
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000AE64 File Offset: 0x00009064
	public static void LogGameCircleWarning(string errorMessage)
	{
		AmazonLogging.LogWarning(AmazonLogging.AmazonLoggingLevel.Verbose, "AmazonGameCircle", errorMessage);
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000AE74 File Offset: 0x00009074
	public static void Log(string message)
	{
		AmazonLogging.Log(AmazonLogging.AmazonLoggingLevel.Verbose, "AmazonGameCircle", message);
	}

	// Token: 0x0400016E RID: 366
	public const string serviceName = "AmazonGameCircle";

	// Token: 0x0400016F RID: 367
	public const AmazonLogging.AmazonLoggingLevel errorLevel = AmazonLogging.AmazonLoggingLevel.Verbose;

	// Token: 0x04000170 RID: 368
	private static bool IsReady;

	// Token: 0x04000171 RID: 369
	private static AmazonJavaWrapper JavaObject;

	// Token: 0x04000172 RID: 370
	private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.AmazonGamesClientProxyImpl";
}
