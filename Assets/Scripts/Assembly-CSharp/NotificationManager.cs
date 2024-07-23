using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class NotificationManager : ITriggerObserver
{
	// Token: 0x06000DFC RID: 3580 RVA: 0x00054FEC File Offset: 0x000531EC
	public NotificationManager()
	{
		this.LoadNotificationsFromSpread();
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x0005501C File Offset: 0x0005321C
	private string[] GetFilesToLoad()
	{
		return Config.NOTIFICATIONS_PATH;
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x00055024 File Offset: 0x00053224
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x00055028 File Offset: 0x00053228
	private Notification LoadNotificationFromFile(string filePath)
	{
		TFUtils.DebugLog("Loading Notification from file " + filePath);
		string json = TFUtils.ReadAllText(filePath);
		Dictionary<string, object> data = (Dictionary<string, object>)Json.Deserialize(json);
		return Notification.FromDict(data);
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x00055060 File Offset: 0x00053260
	private void LoadNotificationsFromSpread()
	{
		string text = "Notifications";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				dictionary.Add("message", instance.GetStringCell(text, rowName, "message"));
				dictionary.Add("notification_sound", instance.GetStringCell(text, rowName, "notification sound"));
				dictionary.Add("conditions", new Dictionary<string, object>
				{
					{
						"id",
						1
					},
					{
						"type",
						instance.GetStringCell(text, rowName, "condition type")
					},
					{
						"simulated_type",
						instance.GetStringCell(text, rowName, "condition simulated type")
					}
				});
				this.notificationList.Add(Notification.FromDict(dictionary));
			}
		}
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x000551CC File Offset: 0x000533CC
	public void ProcessTrigger(ITrigger trigger, Game game)
	{
		bool flag = !SBSettings.SoaringProduction;
		foreach (Notification notification in this.notificationList)
		{
			notification.conditions.Recalculate(game, trigger, null);
			ConditionResult conditionResult = notification.conditions.Examine();
			if (conditionResult == ConditionResult.PASS)
			{
				Trigger trigger2 = (Trigger)trigger;
				if (trigger2.Data.ContainsKey("notification_time") && trigger2.Data.ContainsKey("notification_label"))
				{
					DateTime fireDate = (DateTime)trigger2.Data["notification_time"];
					string text = (string)trigger2.Data["notification_label"];
					if (flag)
					{
						SoaringDebug.Log(string.Concat(new string[]
						{
							text,
							": ",
							fireDate.ToLongDateString(),
							" ",
							fireDate.ToShortTimeString()
						}));
					}
					int value = notification.Send(fireDate, text);
					this.sentNotifications[text] = value;
					notification.Reset();
					if (this.sentNotifications.Count >= 64 && flag)
					{
						SoaringDebug.Log("Warning: Local Notification Overflow. Can Not exceed 64", LogType.Error);
					}
				}
			}
		}
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x00055340 File Offset: 0x00053540
	public static int SendNotification(string body, long delaySeconds, string label, string sound)
	{
		return EtceteraAndroid.scheduleNotification(delaySeconds, "spongebob", body, body, string.Empty);
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x00055354 File Offset: 0x00053554
	public static long ConvertDateTimeToTicks(DateTime dtInput)
	{
		return dtInput.Ticks;
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00055370 File Offset: 0x00053570
	public void CancelNotification(string label)
	{
		if (!this.sentNotifications.ContainsKey(label))
		{
			return;
		}
		int notificationId = this.sentNotifications[label];
		EtceteraAndroid.cancelNotification(notificationId);
		this.sentNotifications.Remove(label);
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x000553B0 File Offset: 0x000535B0
	public static void CancelAllNotifications()
	{
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x000553B4 File Offset: 0x000535B4
	public static void AddAnnoyingNotifications(Game game)
	{
		int num = 3;
		for (int i = 0; i < 4; i++)
		{
			NotificationManager.SendNotification(Language.Get("!!NOTIFY_DAILY_MESSAGE"), NotificationManager.ConvertDateTimeToTicks(DateTime.Now.AddHours(2.5)) + NotificationManager.ConvertDateTimeToTicks(DateTime.Now.AddDays((double)i)), string.Empty, null);
			TFUtils.ErrorLog("\nNOTIFY_DAILY_MESSAGE triggered properly");
		}
		NotificationManager.SendNotification(Language.Get("!!NOTIFY_SB_MISSES_YOU"), NotificationManager.ConvertDateTimeToTicks(DateTime.Now.AddDays(1.0)), string.Empty, null);
		NotificationManager.SendNotification(Language.Get("!!NOTIFY_SB_MISSES_YOU"), NotificationManager.ConvertDateTimeToTicks(DateTime.Now.AddDays(3.0)), string.Empty, null);
		NotificationManager.SendNotification(Language.Get("!!NOTIFY_SB_MISSES_YOU"), NotificationManager.ConvertDateTimeToTicks(DateTime.Now.AddDays(7.0)), string.Empty, null);
		if (game.resourceManager.Resources[ResourceManager.LEVEL].Amount >= num || game.resourceManager.Query(ResourceManager.LEVEL) >= num)
		{
			NotificationManager.SendNotification(Language.Get("!!NOTIFY_FEED_RESIDENTS"), NotificationManager.ConvertDateTimeToTicks(DateTime.Now.AddDays(3.0)), string.Empty, null);
			TFUtils.ErrorLog("\nNOTIFY_FEED_RESIDENTS triggered properly");
		}
		if (game.costumeManager.IsCostumeUnlocked(21) || game.questManager.IsQuestCompleted(3422U))
		{
		}
		if (game.resourceManager.HasEnough(9020, 1) || game.questManager.IsQuestCompleted(2428U))
		{
		}
	}

	// Token: 0x04000940 RID: 2368
	public const string NOTIFICATION_TIME = "notification_time";

	// Token: 0x04000941 RID: 2369
	public const string NOTIFICATION_LABEL = "notification_label";

	// Token: 0x04000942 RID: 2370
	private const string NOTIFICATIONS_PATH = "Notifications";

	// Token: 0x04000943 RID: 2371
	private List<Notification> notificationList = new List<Notification>();

	// Token: 0x04000944 RID: 2372
	private Dictionary<string, int> sentNotifications = new Dictionary<string, int>();
}
