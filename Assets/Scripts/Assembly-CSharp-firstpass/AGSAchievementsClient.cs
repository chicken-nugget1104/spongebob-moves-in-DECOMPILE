using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class AGSAchievementsClient : MonoBehaviour
{
	// Token: 0x06000217 RID: 535 RVA: 0x0000AE8C File Offset: 0x0000908C
	static AGSAchievementsClient()
	{
		AGSAchievementsClient.JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSAchievementsClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() == IntPtr.Zero)
			{
				AGSClient.LogGameCircleWarning(string.Format("No java class {0} present, can't use AGSAchievementsClient", AGSAchievementsClient.PROXY_CLASS_NAME));
			}
			else
			{
				AGSAchievementsClient.JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
		}
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000218 RID: 536 RVA: 0x0000AF30 File Offset: 0x00009130
	// (remove) Token: 0x06000219 RID: 537 RVA: 0x0000AF48 File Offset: 0x00009148
	public static event Action<string, string> UpdateAchievementFailedEvent;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600021A RID: 538 RVA: 0x0000AF60 File Offset: 0x00009160
	// (remove) Token: 0x0600021B RID: 539 RVA: 0x0000AF78 File Offset: 0x00009178
	public static event Action<string> UpdateAchievementSucceededEvent;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x0600021C RID: 540 RVA: 0x0000AF90 File Offset: 0x00009190
	// (remove) Token: 0x0600021D RID: 541 RVA: 0x0000AFA8 File Offset: 0x000091A8
	public static event Action<List<AGSAchievement>> RequestAchievementsSucceededEvent;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x0600021E RID: 542 RVA: 0x0000AFC0 File Offset: 0x000091C0
	// (remove) Token: 0x0600021F RID: 543 RVA: 0x0000AFD8 File Offset: 0x000091D8
	public static event Action<string> RequestAchievementsFailedEvent;

	// Token: 0x06000220 RID: 544 RVA: 0x0000AFF0 File Offset: 0x000091F0
	public static void UpdateAchievementProgress(string achievementId, float progress)
	{
		AGSAchievementsClient.JavaObject.Call("updateAchievementProgress", new object[]
		{
			achievementId,
			progress
		});
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000B020 File Offset: 0x00009220
	public static void RequestAchievements()
	{
		AGSAchievementsClient.JavaObject.Call("requestAchievements", new object[0]);
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000B038 File Offset: 0x00009238
	public static void ShowAchievementsOverlay()
	{
		AGSAchievementsClient.JavaObject.Call("showAchievementsOverlay", new object[0]);
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000B050 File Offset: 0x00009250
	public static void RequestAchievementsSucceeded(string json)
	{
		if (AGSAchievementsClient.RequestAchievementsSucceededEvent != null)
		{
			List<AGSAchievement> list = new List<AGSAchievement>();
			ArrayList arrayList = json.arrayListFromJson();
			foreach (object obj in arrayList)
			{
				Hashtable ht = (Hashtable)obj;
				list.Add(AGSAchievement.fromHashtable(ht));
			}
			AGSAchievementsClient.RequestAchievementsSucceededEvent(list);
		}
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000B0E4 File Offset: 0x000092E4
	public static void UpdateAchievementFailed(string json)
	{
		if (AGSAchievementsClient.UpdateAchievementFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = AGSAchievementsClient.GetStringFromHashtable(ht, "achievementId");
			string stringFromHashtable2 = AGSAchievementsClient.GetStringFromHashtable(ht, "error");
			AGSAchievementsClient.UpdateAchievementFailedEvent(stringFromHashtable, stringFromHashtable2);
		}
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000B128 File Offset: 0x00009328
	public static void UpdateAchievementSucceeded(string json)
	{
		if (AGSAchievementsClient.UpdateAchievementSucceededEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = AGSAchievementsClient.GetStringFromHashtable(ht, "achievementId");
			AGSAchievementsClient.UpdateAchievementSucceededEvent(stringFromHashtable);
		}
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000B160 File Offset: 0x00009360
	public static void RequestAchievementsFailed(string json)
	{
		if (AGSAchievementsClient.RequestAchievementsFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = AGSAchievementsClient.GetStringFromHashtable(ht, "error");
			AGSAchievementsClient.RequestAchievementsFailedEvent(stringFromHashtable);
		}
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000B198 File Offset: 0x00009398
	private static string GetStringFromHashtable(Hashtable ht, string key)
	{
		if (ht == null)
		{
			return null;
		}
		if (key == null)
		{
			return null;
		}
		string result = null;
		if (ht.Contains(key))
		{
			result = ht[key].ToString();
		}
		return result;
	}

	// Token: 0x04000175 RID: 373
	private static AmazonJavaWrapper JavaObject;

	// Token: 0x04000176 RID: 374
	private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.AchievementsClientProxyImpl";
}
