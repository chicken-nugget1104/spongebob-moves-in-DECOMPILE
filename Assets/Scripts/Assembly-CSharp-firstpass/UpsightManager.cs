using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class UpsightManager : MonoBehaviour
{
	// Token: 0x060007FB RID: 2043 RVA: 0x0001E750 File Offset: 0x0001C950
	static UpsightManager()
	{
		try
		{
			GameObject gameObject = new GameObject("UpsightManager");
			gameObject.AddComponent<UpsightManager>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
		catch (UnityException)
		{
			Debug.LogWarning("It looks like you have the UpsightManager on a GameObject in your scene. Please remove the script from your scene.");
		}
	}

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x060007FC RID: 2044 RVA: 0x0001E7A8 File Offset: 0x0001C9A8
	// (remove) Token: 0x060007FD RID: 2045 RVA: 0x0001E7C0 File Offset: 0x0001C9C0
	public static event Action<Dictionary<string, object>> openRequestSucceededEvent;

	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x060007FE RID: 2046 RVA: 0x0001E7D8 File Offset: 0x0001C9D8
	// (remove) Token: 0x060007FF RID: 2047 RVA: 0x0001E7F0 File Offset: 0x0001C9F0
	public static event Action<string> openRequestFailedEvent;

	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x06000800 RID: 2048 RVA: 0x0001E808 File Offset: 0x0001CA08
	// (remove) Token: 0x06000801 RID: 2049 RVA: 0x0001E820 File Offset: 0x0001CA20
	public static event Action<string> contentWillDisplayEvent;

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x06000802 RID: 2050 RVA: 0x0001E838 File Offset: 0x0001CA38
	// (remove) Token: 0x06000803 RID: 2051 RVA: 0x0001E850 File Offset: 0x0001CA50
	public static event Action<string> contentDidDisplayEvent;

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x06000804 RID: 2052 RVA: 0x0001E868 File Offset: 0x0001CA68
	// (remove) Token: 0x06000805 RID: 2053 RVA: 0x0001E880 File Offset: 0x0001CA80
	public static event Action<string> contentRequestLoadedEvent;

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x06000806 RID: 2054 RVA: 0x0001E898 File Offset: 0x0001CA98
	// (remove) Token: 0x06000807 RID: 2055 RVA: 0x0001E8B0 File Offset: 0x0001CAB0
	public static event Action<string, string> contentRequestFailedEvent;

	// Token: 0x140000C8 RID: 200
	// (add) Token: 0x06000808 RID: 2056 RVA: 0x0001E8C8 File Offset: 0x0001CAC8
	// (remove) Token: 0x06000809 RID: 2057 RVA: 0x0001E8E0 File Offset: 0x0001CAE0
	public static event Action<string> contentPreloadSucceededEvent;

	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x0600080A RID: 2058 RVA: 0x0001E8F8 File Offset: 0x0001CAF8
	// (remove) Token: 0x0600080B RID: 2059 RVA: 0x0001E910 File Offset: 0x0001CB10
	public static event Action<string, string> contentPreloadFailedEvent;

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x0600080C RID: 2060 RVA: 0x0001E928 File Offset: 0x0001CB28
	// (remove) Token: 0x0600080D RID: 2061 RVA: 0x0001E940 File Offset: 0x0001CB40
	public static event Action<int> badgeCountRequestSucceededEvent;

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x0600080E RID: 2062 RVA: 0x0001E958 File Offset: 0x0001CB58
	// (remove) Token: 0x0600080F RID: 2063 RVA: 0x0001E970 File Offset: 0x0001CB70
	public static event Action<string> badgeCountRequestFailedEvent;

	// Token: 0x140000CC RID: 204
	// (add) Token: 0x06000810 RID: 2064 RVA: 0x0001E988 File Offset: 0x0001CB88
	// (remove) Token: 0x06000811 RID: 2065 RVA: 0x0001E9A0 File Offset: 0x0001CBA0
	public static event Action trackInAppPurchaseSucceededEvent;

	// Token: 0x140000CD RID: 205
	// (add) Token: 0x06000812 RID: 2066 RVA: 0x0001E9B8 File Offset: 0x0001CBB8
	// (remove) Token: 0x06000813 RID: 2067 RVA: 0x0001E9D0 File Offset: 0x0001CBD0
	public static event Action<string> trackInAppPurchaseFailedEvent;

	// Token: 0x140000CE RID: 206
	// (add) Token: 0x06000814 RID: 2068 RVA: 0x0001E9E8 File Offset: 0x0001CBE8
	// (remove) Token: 0x06000815 RID: 2069 RVA: 0x0001EA00 File Offset: 0x0001CC00
	public static event Action reportCustomEventSucceededEvent;

	// Token: 0x140000CF RID: 207
	// (add) Token: 0x06000816 RID: 2070 RVA: 0x0001EA18 File Offset: 0x0001CC18
	// (remove) Token: 0x06000817 RID: 2071 RVA: 0x0001EA30 File Offset: 0x0001CC30
	public static event Action<string> reportCustomEventFailedEvent;

	// Token: 0x140000D0 RID: 208
	// (add) Token: 0x06000818 RID: 2072 RVA: 0x0001EA48 File Offset: 0x0001CC48
	// (remove) Token: 0x06000819 RID: 2073 RVA: 0x0001EA60 File Offset: 0x0001CC60
	public static event Action<string, string> contentDismissedEvent;

	// Token: 0x140000D1 RID: 209
	// (add) Token: 0x0600081A RID: 2074 RVA: 0x0001EA78 File Offset: 0x0001CC78
	// (remove) Token: 0x0600081B RID: 2075 RVA: 0x0001EA90 File Offset: 0x0001CC90
	public static event Action<UpsightPurchase> makePurchaseEvent;

	// Token: 0x140000D2 RID: 210
	// (add) Token: 0x0600081C RID: 2076 RVA: 0x0001EAA8 File Offset: 0x0001CCA8
	// (remove) Token: 0x0600081D RID: 2077 RVA: 0x0001EAC0 File Offset: 0x0001CCC0
	public static event Action<Dictionary<string, object>> dataOptInEvent;

	// Token: 0x140000D3 RID: 211
	// (add) Token: 0x0600081E RID: 2078 RVA: 0x0001EAD8 File Offset: 0x0001CCD8
	// (remove) Token: 0x0600081F RID: 2079 RVA: 0x0001EAF0 File Offset: 0x0001CCF0
	public static event Action<UpsightReward> unlockedRewardEvent;

	// Token: 0x140000D4 RID: 212
	// (add) Token: 0x06000820 RID: 2080 RVA: 0x0001EB08 File Offset: 0x0001CD08
	// (remove) Token: 0x06000821 RID: 2081 RVA: 0x0001EB20 File Offset: 0x0001CD20
	public static event Action<string, string, string> pushNotificationWithContentReceivedEvent;

	// Token: 0x140000D5 RID: 213
	// (add) Token: 0x06000822 RID: 2082 RVA: 0x0001EB38 File Offset: 0x0001CD38
	// (remove) Token: 0x06000823 RID: 2083 RVA: 0x0001EB50 File Offset: 0x0001CD50
	public static event Action<string> pushNotificationWithUrlReceivedEvent;

	// Token: 0x06000824 RID: 2084 RVA: 0x0001EB68 File Offset: 0x0001CD68
	public static void noop()
	{
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x0001EB6C File Offset: 0x0001CD6C
	private void openRequestSucceeded(string json)
	{
		if (UpsightManager.openRequestSucceededEvent != null)
		{
			UpsightManager.openRequestSucceededEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0001EB90 File Offset: 0x0001CD90
	private void openRequestFailed(string error)
	{
		if (UpsightManager.openRequestFailedEvent != null)
		{
			UpsightManager.openRequestFailedEvent(error);
		}
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x0001EBA8 File Offset: 0x0001CDA8
	private void contentWillDisplay(string placementID)
	{
		if (UpsightManager.contentWillDisplayEvent != null)
		{
			UpsightManager.contentWillDisplayEvent(placementID);
		}
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x0001EBC0 File Offset: 0x0001CDC0
	private void contentDidDisplay(string placementID)
	{
		if (UpsightManager.contentDidDisplayEvent != null)
		{
			UpsightManager.contentDidDisplayEvent(placementID);
		}
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x0001EBD8 File Offset: 0x0001CDD8
	private void contentRequestLoaded(string placementID)
	{
		if (UpsightManager.contentRequestLoadedEvent != null)
		{
			UpsightManager.contentRequestLoadedEvent(placementID);
		}
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0001EBF0 File Offset: 0x0001CDF0
	private void contentRequestFailed(string json)
	{
		if (UpsightManager.contentRequestFailedEvent != null)
		{
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("error") && dictionary.ContainsKey("placement"))
			{
				UpsightManager.contentRequestFailedEvent(dictionary["placement"].ToString(), dictionary["error"].ToString());
			}
		}
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x0001EC64 File Offset: 0x0001CE64
	private void contentPreloadSucceeded(string placementID)
	{
		if (UpsightManager.contentPreloadSucceededEvent != null)
		{
			UpsightManager.contentPreloadSucceededEvent(placementID);
		}
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x0001EC7C File Offset: 0x0001CE7C
	private void contentPreloadFailed(string json)
	{
		if (UpsightManager.contentPreloadFailedEvent != null)
		{
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("error") && dictionary.ContainsKey("placement"))
			{
				UpsightManager.contentPreloadFailedEvent(dictionary["placement"].ToString(), dictionary["error"].ToString());
			}
		}
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0001ECF0 File Offset: 0x0001CEF0
	private void metadataRequestSucceeded(string json)
	{
		if (UpsightManager.badgeCountRequestSucceededEvent != null)
		{
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("notification"))
			{
				Dictionary<string, object> dictionary2 = dictionary["notification"] as Dictionary<string, object>;
				if (dictionary2.ContainsKey("type") && dictionary2.ContainsKey("value"))
				{
					UpsightManager.badgeCountRequestSucceededEvent(int.Parse(dictionary2["value"].ToString()));
					return;
				}
			}
		}
		UpsightManager.badgeCountRequestFailedEvent("No badge count could be found for the placement");
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x0001ED8C File Offset: 0x0001CF8C
	private void metadataRequestFailed(string error)
	{
		if (UpsightManager.badgeCountRequestFailedEvent != null)
		{
			UpsightManager.badgeCountRequestFailedEvent(error);
		}
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x0001EDA4 File Offset: 0x0001CFA4
	private void trackInAppPurchaseSucceeded(string empty)
	{
		if (UpsightManager.trackInAppPurchaseSucceededEvent != null)
		{
			UpsightManager.trackInAppPurchaseSucceededEvent();
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x0001EDBC File Offset: 0x0001CFBC
	private void trackInAppPurchaseFailed(string error)
	{
		if (UpsightManager.trackInAppPurchaseFailedEvent != null)
		{
			UpsightManager.trackInAppPurchaseFailedEvent(error);
		}
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x0001EDD4 File Offset: 0x0001CFD4
	private void reportCustomEventSucceeded(string empty)
	{
		if (UpsightManager.reportCustomEventSucceededEvent != null)
		{
			UpsightManager.reportCustomEventSucceededEvent();
		}
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0001EDEC File Offset: 0x0001CFEC
	private void reportCustomEventFailed(string error)
	{
		if (UpsightManager.reportCustomEventFailedEvent != null)
		{
			UpsightManager.reportCustomEventFailedEvent(error);
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0001EE04 File Offset: 0x0001D004
	private void contentDismissed(string json)
	{
		if (UpsightManager.contentDismissedEvent != null)
		{
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("dismissType") && dictionary.ContainsKey("placement"))
			{
				UpsightManager.contentDismissedEvent(dictionary["placement"].ToString(), dictionary["dismissType"].ToString());
			}
		}
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0001EE78 File Offset: 0x0001D078
	private void makePurchase(string json)
	{
		if (UpsightManager.makePurchaseEvent != null)
		{
			UpsightManager.makePurchaseEvent(UpsightPurchase.purchaseFromJson(json));
		}
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0001EE94 File Offset: 0x0001D094
	private void dataOptIn(string json)
	{
		if (UpsightManager.dataOptInEvent != null)
		{
			UpsightManager.dataOptInEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0001EEB8 File Offset: 0x0001D0B8
	private void unlockedReward(string json)
	{
		if (UpsightManager.unlockedRewardEvent != null)
		{
			UpsightManager.unlockedRewardEvent(UpsightReward.rewardFromJson(json));
		}
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0001EED4 File Offset: 0x0001D0D4
	private void pushNotificationWithContentReceived(string json)
	{
		if (UpsightManager.pushNotificationWithContentReceivedEvent != null)
		{
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			if (dictionary != null && dictionary.ContainsKey("messageID") && dictionary.ContainsKey("contentUnitID"))
			{
				string arg = string.Empty;
				if (dictionary.ContainsKey("campaignID") && dictionary["campaignID"] != null)
				{
					arg = dictionary["campaignID"].ToString();
				}
				UpsightManager.pushNotificationWithContentReceivedEvent(dictionary["messageID"].ToString(), dictionary["contentUnitID"].ToString(), arg);
			}
		}
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0001EF80 File Offset: 0x0001D180
	private void pushNotificationWithUrlReceived(string url)
	{
		if (UpsightManager.pushNotificationWithUrlReceivedEvent != null)
		{
			UpsightManager.pushNotificationWithUrlReceivedEvent(url);
		}
	}
}
