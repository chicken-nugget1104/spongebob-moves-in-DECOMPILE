using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x020003E1 RID: 993
public class TFAnalytics
{
	// Token: 0x06001E64 RID: 7780 RVA: 0x000BBB84 File Offset: 0x000B9D84
	public static void LogEvent(string eventName, Dictionary<string, object> eventData)
	{
		eventName = TFAnalytics.ValidateEventName(eventName);
		string text = Json.Serialize(eventData);
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x000BBBA0 File Offset: 0x000B9DA0
	public static void LogRevenueTracking(int valueCents)
	{
		Debug.Log("-------------------valueCents " + valueCents);
		TFAnalytics.logRevenueTracking(valueCents);
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x000BBBC0 File Offset: 0x000B9DC0
	private static void ThreadedLogEventData(object obj)
	{
		List<string> list = (List<string>)obj;
		TFAnalytics.logEventWithData(list[0], list[1]);
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x000BBBE8 File Offset: 0x000B9DE8
	private static void ThreadedLogRevenue(object obj)
	{
		TFAnalytics.logRevenueTracking((int)obj);
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x000BBBF8 File Offset: 0x000B9DF8
	private static void logEventWithData(string eventName, string eventData)
	{
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x000BBBFC File Offset: 0x000B9DFC
	private static void logRevenueTracking(int valueCents)
	{
		Upsight.reportCustomEvent(new Dictionary<string, object>
		{
			{
				"revenue_tracking",
				valueCents
			}
		});
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x000BBC28 File Offset: 0x000B9E28
	private static string ValidateEventName(string eventName)
	{
		if (eventName.Length > 32)
		{
			TFUtils.WarningLog("Requesting analytics event that exceeds maximum character limit of 32!\neventName=" + eventName);
			eventName = eventName.Substring(0, 32);
		}
		return eventName;
	}

	// Token: 0x040012CC RID: 4812
	private const bool LOG_ANALYTICS = false;

	// Token: 0x040012CD RID: 4813
	private const int MAX_NAME_CHARACTERS = 32;
}
