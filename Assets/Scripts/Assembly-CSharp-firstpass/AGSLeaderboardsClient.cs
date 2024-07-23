using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class AGSLeaderboardsClient : MonoBehaviour
{
	// Token: 0x06000241 RID: 577 RVA: 0x0000B7F8 File Offset: 0x000099F8
	static AGSLeaderboardsClient()
	{
		AGSLeaderboardsClient.JavaObject = new AmazonJavaWrapper();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AGSLeaderboardsClient.PROXY_CLASS_NAME))
		{
			if (androidJavaClass.GetRawClass() == IntPtr.Zero)
			{
				AGSClient.LogGameCircleWarning("No java class " + AGSLeaderboardsClient.PROXY_CLASS_NAME + " present, can't use AGSLeaderboardsClient");
			}
			else
			{
				AGSLeaderboardsClient.JavaObject.setAndroidJavaObject(androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]));
			}
		}
	}

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000242 RID: 578 RVA: 0x0000B8A0 File Offset: 0x00009AA0
	// (remove) Token: 0x06000243 RID: 579 RVA: 0x0000B8B8 File Offset: 0x00009AB8
	public static event Action<string, string> SubmitScoreFailedEvent;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000244 RID: 580 RVA: 0x0000B8D0 File Offset: 0x00009AD0
	// (remove) Token: 0x06000245 RID: 581 RVA: 0x0000B8E8 File Offset: 0x00009AE8
	public static event Action<string> SubmitScoreSucceededEvent;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000246 RID: 582 RVA: 0x0000B900 File Offset: 0x00009B00
	// (remove) Token: 0x06000247 RID: 583 RVA: 0x0000B918 File Offset: 0x00009B18
	public static event Action<string> RequestLeaderboardsFailedEvent;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000248 RID: 584 RVA: 0x0000B930 File Offset: 0x00009B30
	// (remove) Token: 0x06000249 RID: 585 RVA: 0x0000B948 File Offset: 0x00009B48
	public static event Action<List<AGSLeaderboard>> RequestLeaderboardsSucceededEvent;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x0600024A RID: 586 RVA: 0x0000B960 File Offset: 0x00009B60
	// (remove) Token: 0x0600024B RID: 587 RVA: 0x0000B978 File Offset: 0x00009B78
	public static event Action<string, string> RequestLocalPlayerScoreFailedEvent;

	// Token: 0x1400000D RID: 13
	// (add) Token: 0x0600024C RID: 588 RVA: 0x0000B990 File Offset: 0x00009B90
	// (remove) Token: 0x0600024D RID: 589 RVA: 0x0000B9A8 File Offset: 0x00009BA8
	public static event Action<string, int, long> RequestLocalPlayerScoreSucceededEvent;

	// Token: 0x0600024E RID: 590 RVA: 0x0000B9C0 File Offset: 0x00009BC0
	public static void SubmitScore(string leaderboardId, long score)
	{
		AGSLeaderboardsClient.JavaObject.Call("submitScore", new object[]
		{
			leaderboardId,
			score
		});
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0000B9F0 File Offset: 0x00009BF0
	public static void ShowLeaderboardsOverlay()
	{
		AGSLeaderboardsClient.JavaObject.Call("showLeaderboardsOverlay", new object[0]);
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0000BA08 File Offset: 0x00009C08
	public static void RequestLeaderboards()
	{
		AGSLeaderboardsClient.JavaObject.Call("requestLeaderboards", new object[0]);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0000BA20 File Offset: 0x00009C20
	public static void RequestLocalPlayerScore(string leaderboardId, LeaderboardScope scope)
	{
		AGSLeaderboardsClient.JavaObject.Call("requestLocalPlayerScore", new object[]
		{
			leaderboardId,
			(int)scope
		});
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000BA50 File Offset: 0x00009C50
	public static void SubmitScoreFailed(string json)
	{
		if (AGSLeaderboardsClient.SubmitScoreFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = AGSLeaderboardsClient.GetStringFromHashtable(ht, "leaderboardId");
			string stringFromHashtable2 = AGSLeaderboardsClient.GetStringFromHashtable(ht, "error");
			AGSLeaderboardsClient.SubmitScoreFailedEvent(stringFromHashtable, stringFromHashtable2);
		}
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000BA94 File Offset: 0x00009C94
	public static void SubmitScoreSucceeded(string json)
	{
		if (AGSLeaderboardsClient.SubmitScoreSucceededEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = AGSLeaderboardsClient.GetStringFromHashtable(ht, "leaderboardId");
			AGSLeaderboardsClient.SubmitScoreSucceededEvent(stringFromHashtable);
		}
	}

	// Token: 0x06000254 RID: 596 RVA: 0x0000BACC File Offset: 0x00009CCC
	public static void RequestLeaderboardsFailed(string json)
	{
		if (AGSLeaderboardsClient.RequestLeaderboardsFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = AGSLeaderboardsClient.GetStringFromHashtable(ht, "error");
			AGSLeaderboardsClient.RequestLeaderboardsFailedEvent(stringFromHashtable);
		}
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0000BB04 File Offset: 0x00009D04
	public static void RequestLeaderboardsSucceeded(string json)
	{
		if (AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent != null)
		{
			List<AGSLeaderboard> list = new List<AGSLeaderboard>();
			ArrayList arrayList = json.arrayListFromJson();
			foreach (object obj in arrayList)
			{
				Hashtable ht = (Hashtable)obj;
				list.Add(AGSLeaderboard.fromHashtable(ht));
			}
			AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent(list);
		}
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0000BB98 File Offset: 0x00009D98
	public static void RequestLocalPlayerScoreFailed(string json)
	{
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent != null)
		{
			Hashtable ht = json.hashtableFromJson();
			string stringFromHashtable = AGSLeaderboardsClient.GetStringFromHashtable(ht, "leaderboardId");
			string stringFromHashtable2 = AGSLeaderboardsClient.GetStringFromHashtable(ht, "error");
			AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent(stringFromHashtable, stringFromHashtable2);
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000BBDC File Offset: 0x00009DDC
	public static void RequestLocalPlayerScoreSucceeded(string json)
	{
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent != null)
		{
			Hashtable hashtable = json.hashtableFromJson();
			int arg = 0;
			long arg2 = 0L;
			string arg3 = null;
			try
			{
				if (hashtable.Contains("leaderboardId"))
				{
					arg3 = hashtable["leaderboardId"].ToString();
				}
				if (hashtable.Contains("rank"))
				{
					arg = int.Parse(hashtable["rank"].ToString());
				}
				if (hashtable.Contains("score"))
				{
					arg2 = long.Parse(hashtable["score"].ToString());
				}
			}
			catch (FormatException ex)
			{
				AGSClient.Log("unable to parse score " + ex.Message);
			}
			AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent(arg3, arg, arg2);
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000BCBC File Offset: 0x00009EBC
	private static string GetStringFromHashtable(Hashtable ht, string key)
	{
		string result = null;
		if (ht.Contains(key))
		{
			result = ht[key].ToString();
		}
		return result;
	}

	// Token: 0x0400018A RID: 394
	private static AmazonJavaWrapper JavaObject;

	// Token: 0x0400018B RID: 395
	private static readonly string PROXY_CLASS_NAME = "com.amazon.ags.api.unity.LeaderboardsClientProxyImpl";
}
