using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class AGSWhispersyncClient : MonoBehaviour
{
	// Token: 0x06000270 RID: 624 RVA: 0x0000C178 File Offset: 0x0000A378
	static AGSWhispersyncClient()
	{
		AGSWhispersyncClient.javaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSWhispersyncClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() == IntPtr.Zero)
			{
				AGSClient.LogGameCircleWarning("No java class " + AGSWhispersyncClient.PROXY_CLASS_NAME + " present, can't use AGSWhispersyncClient");
			}
			else
			{
				AGSWhispersyncClient.javaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
		}
	}

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06000271 RID: 625 RVA: 0x0000C220 File Offset: 0x0000A420
	// (remove) Token: 0x06000272 RID: 626 RVA: 0x0000C238 File Offset: 0x0000A438
	public static event Action OnNewCloudDataEvent;

	// Token: 0x06000273 RID: 627 RVA: 0x0000C250 File Offset: 0x0000A450
	public static AGSGameDataMap GetGameData()
	{
		AndroidJavaObject androidJavaObject = AGSWhispersyncClient.javaObject.Call<AndroidJavaObject>("getGameData", new object[0]);
		if (androidJavaObject != null)
		{
			return new AGSGameDataMap(new AmazonJavaWrapper(androidJavaObject));
		}
		return null;
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000C288 File Offset: 0x0000A488
	public static void Synchronize()
	{
		AGSWhispersyncClient.javaObject.Call("synchronize", new object[0]);
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000C2A0 File Offset: 0x0000A4A0
	public static void Flush()
	{
		AGSWhispersyncClient.javaObject.Call("flush", new object[0]);
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000C2B8 File Offset: 0x0000A4B8
	public static void OnNewCloudData()
	{
		if (AGSWhispersyncClient.OnNewCloudDataEvent != null)
		{
			AGSWhispersyncClient.OnNewCloudDataEvent();
		}
	}

	// Token: 0x040001A1 RID: 417
	private static AmazonJavaWrapper javaObject;

	// Token: 0x040001A2 RID: 418
	private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.WhispersyncClientProxyImpl";
}
