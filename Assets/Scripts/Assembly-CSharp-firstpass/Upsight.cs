using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class Upsight
{
	// Token: 0x060007E1 RID: 2017 RVA: 0x0001E2A0 File Offset: 0x0001C4A0
	static Upsight()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		UpsightManager.noop();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.upsight.unity.UpsightPlugin"))
		{
			Upsight._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0001E310 File Offset: 0x0001C510
	public static void setLogLevel(UpsightLogLevel logLevel)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		Upsight._plugin.Call("setLogLevel", new object[]
		{
			logLevel.ToString()
		});
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0001E350 File Offset: 0x0001C550
	public static string getPluginVersion()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return "UnityEditor";
		}
		return Upsight._plugin.Call<string>("getPluginVersion", new object[0]);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0001E37C File Offset: 0x0001C57C
	public static void init(string appToken, string appSecret, string gcmProjectNumber = null)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		Upsight._plugin.Call("init", new object[]
		{
			appToken,
			appSecret,
			gcmProjectNumber
		});
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0001E3B8 File Offset: 0x0001C5B8
	public static void requestAppOpen()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		Upsight._plugin.Call("requestAppOpen", new object[0]);
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x0001E3E8 File Offset: 0x0001C5E8
	public static void sendContentRequest(string placementID, bool showsOverlayImmediately, bool shouldAnimate = true, Dictionary<string, object> dimensions = null)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		AndroidJavaObject androidJavaObject = Upsight.dictionaryToJavaHashMap(dimensions);
		Upsight._plugin.Call("sendContentRequest", new object[]
		{
			placementID,
			showsOverlayImmediately,
			shouldAnimate,
			androidJavaObject
		});
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0001E438 File Offset: 0x0001C638
	public static AndroidJavaObject dictionaryToJavaHashMap(Dictionary<string, object> dictionary)
	{
		AndroidJavaObject result = null;
		if (dictionary != null)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("net.minidev.json.parser.JSONParser");
			int @static = androidJavaClass.GetStatic<int>("MODE_JSON_SIMPLE");
			string text = Json.Serialize(dictionary);
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("net.minidev.json.parser.JSONParser", new object[]
			{
				@static
			});
			result = androidJavaObject.Call<AndroidJavaObject>("parse", new object[]
			{
				text
			});
		}
		return result;
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0001E4A0 File Offset: 0x0001C6A0
	public static void preloadContentRequest(string placementID, Dictionary<string, object> dimensions = null)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		AndroidJavaObject androidJavaObject = Upsight.dictionaryToJavaHashMap(dimensions);
		Upsight._plugin.Call("preloadContentRequest", new object[]
		{
			placementID,
			androidJavaObject
		});
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0001E4E0 File Offset: 0x0001C6E0
	public static void getContentBadgeNumber(string placementID)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		Upsight._plugin.Call("sendMetadataRequest", new object[]
		{
			placementID
		});
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x0001E514 File Offset: 0x0001C714
	public static bool getOptOutStatus()
	{
		return Application.platform == RuntimePlatform.Android && Upsight._plugin.Call<bool>("getOptOutStatus", new object[0]);
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x0001E53C File Offset: 0x0001C73C
	public static void setOptOutStatus(bool optOutStatus)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		Upsight._plugin.Call("setOptOutStatus", new object[]
		{
			optOutStatus
		});
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x0001E56C File Offset: 0x0001C76C
	public static void trackInAppPurchase(string sku, int quantity, UpsightAndroidPurchaseResolution resolutionType, double price, string orderId, string store)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		Upsight._plugin.Call("trackInAppPurchase", new object[]
		{
			sku,
			quantity,
			(int)resolutionType,
			price,
			orderId,
			store
		});
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x0001E5C4 File Offset: 0x0001C7C4
	public static void reportCustomEvent(Dictionary<string, object> properties)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		Upsight._plugin.Call("reportCustomEvent", new object[]
		{
			Json.Serialize(properties)
		});
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x0001E5F4 File Offset: 0x0001C7F4
	public static void registerForPushNotifications()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		Upsight._plugin.Call("registerForPushNotifications", new object[0]);
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x0001E624 File Offset: 0x0001C824
	public static void deregisterForPushNotifications()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		Upsight._plugin.Call("deregisterForPushNotifications", new object[0]);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x0001E654 File Offset: 0x0001C854
	public static void setShouldOpenContentRequestsFromPushNotifications(bool shouldOpen)
	{
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x0001E658 File Offset: 0x0001C858
	public static void setShouldOpenUrlsFromPushNotifications(bool shouldOpen)
	{
	}

	// Token: 0x040004EC RID: 1260
	private static AndroidJavaObject _plugin;
}
