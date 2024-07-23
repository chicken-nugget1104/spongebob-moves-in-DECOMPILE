using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000048 RID: 72
public class AGSProfilesClient : MonoBehaviour
{
	// Token: 0x06000261 RID: 609 RVA: 0x0000BF14 File Offset: 0x0000A114
	static AGSProfilesClient()
	{
		AGSProfilesClient.JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSProfilesClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() == IntPtr.Zero)
			{
				AGSClient.LogGameCircleWarning("No java class " + AGSProfilesClient.PROXY_CLASS_NAME + " present, can't use AGSProfilesClient");
			}
			else
			{
				AGSProfilesClient.JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
		}
	}

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06000262 RID: 610 RVA: 0x0000BFBC File Offset: 0x0000A1BC
	// (remove) Token: 0x06000263 RID: 611 RVA: 0x0000BFD4 File Offset: 0x0000A1D4
	public static event Action<AGSProfile> PlayerAliasReceivedEvent;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000264 RID: 612 RVA: 0x0000BFEC File Offset: 0x0000A1EC
	// (remove) Token: 0x06000265 RID: 613 RVA: 0x0000C004 File Offset: 0x0000A204
	public static event Action<string> PlayerAliasFailedEvent;

	// Token: 0x06000266 RID: 614 RVA: 0x0000C01C File Offset: 0x0000A21C
	public static void RequestLocalPlayerProfile()
	{
		AGSProfilesClient.JavaObject.Call("requestLocalPlayerProfile", new object[0]);
	}

	// Token: 0x06000267 RID: 615 RVA: 0x0000C034 File Offset: 0x0000A234
	public static void PlayerAliasReceived(string json)
	{
		if (AGSProfilesClient.PlayerAliasReceivedEvent != null)
		{
			Hashtable profileDataAsHashtable = json.hashtableFromJson();
			AGSProfilesClient.PlayerAliasReceivedEvent(AGSProfile.fromHashtable(profileDataAsHashtable));
		}
	}

	// Token: 0x06000268 RID: 616 RVA: 0x0000C064 File Offset: 0x0000A264
	public static void PlayerAliasFailed(string json)
	{
		if (AGSProfilesClient.PlayerAliasFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = AGSProfilesClient.GetStringFromHashtable(ht, "error");
			AGSProfilesClient.PlayerAliasFailedEvent(stringFromHashtable);
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x0000C09C File Offset: 0x0000A29C
	private static string GetStringFromHashtable(Hashtable ht, string key)
	{
		string result = null;
		if (ht.Contains(key))
		{
			result = ht[key].ToString();
		}
		return result;
	}

	// Token: 0x0400019B RID: 411
	private static AmazonJavaWrapper JavaObject;

	// Token: 0x0400019C RID: 412
	private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.ProfilesClientProxyImpl";
}
