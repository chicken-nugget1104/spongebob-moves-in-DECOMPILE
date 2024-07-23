using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class GoogleIAB
{
	// Token: 0x06000658 RID: 1624 RVA: 0x00017150 File Offset: 0x00015350
	static GoogleIAB()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.GoogleIABPlugin"))
		{
			GoogleIAB._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x000171BC File Offset: 0x000153BC
	public static void enableLogging(bool shouldEnable)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (GoogleIAB._plugin == null)
		{
			return;
		}
		if (shouldEnable)
		{
			Debug.LogWarning("YOU HAVE ENABLED HIGH DETAIL LOGS. DO NOT DISTRIBUTE THE GENERATED APK PUBLICLY. IT WILL DUMP SENSITIVE INFORMATION TO THE CONSOLE!");
		}
		GoogleIAB._plugin.Call("enableLogging", new object[]
		{
			shouldEnable
		});
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00017210 File Offset: 0x00015410
	public static void setAutoVerifySignatures(bool shouldVerify)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (GoogleIAB._plugin == null)
		{
			return;
		}
		GoogleIAB._plugin.Call("setAutoVerifySignatures", new object[]
		{
			shouldVerify
		});
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00017254 File Offset: 0x00015454
	public static void init(string publicKey)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (GoogleIAB._plugin == null)
		{
			return;
		}
		GoogleIAB._plugin.Call("init", new object[]
		{
			publicKey
		});
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00017288 File Offset: 0x00015488
	public static void unbindService()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (GoogleIAB._plugin == null)
		{
			return;
		}
		GoogleIAB._plugin.Call("unbindService", new object[0]);
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x000172B8 File Offset: 0x000154B8
	public static bool areSubscriptionsSupported()
	{
		return Application.platform == RuntimePlatform.Android && GoogleIAB._plugin != null && GoogleIAB._plugin.Call<bool>("areSubscriptionsSupported", new object[0]);
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x000172EC File Offset: 0x000154EC
	public static void queryInventory(string[] skus)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		GoogleIAB._plugin.Call("queryInventory", new object[]
		{
			skus
		});
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x00017320 File Offset: 0x00015520
	public static void purchaseProduct(string sku)
	{
		GoogleIAB.purchaseProduct(sku, string.Empty);
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x00017330 File Offset: 0x00015530
	public static void purchaseProduct(string sku, string developerPayload)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (GoogleIAB._plugin == null)
		{
			return;
		}
		GoogleIAB._plugin.Call("purchaseProduct", new object[]
		{
			sku,
			developerPayload
		});
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00017368 File Offset: 0x00015568
	public static void consumeProduct(string sku)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (GoogleIAB._plugin == null)
		{
			return;
		}
		GoogleIAB._plugin.Call("consumeProduct", new object[]
		{
			sku
		});
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0001739C File Offset: 0x0001559C
	public static void consumeProducts(string[] skus)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (GoogleIAB._plugin == null)
		{
			return;
		}
		GoogleIAB._plugin.Call("consumeProducts", new object[]
		{
			skus
		});
	}

	// Token: 0x04000384 RID: 900
	private static AndroidJavaObject _plugin;
}
