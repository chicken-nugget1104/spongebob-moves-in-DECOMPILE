using System;
using System.Collections.Generic;

// Token: 0x02000049 RID: 73
public class PushNotificationManager
{
	// Token: 0x060002DD RID: 733 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
	public PushNotificationManager(Session session)
	{
		session.RegisterExternalCallback("push_notification_action", new TFServer.JsonResponseHandler(this.HandlePushNotificationAction));
	}

	// Token: 0x060002DE RID: 734 RVA: 0x0000E7C0 File Offset: 0x0000C9C0
	public void HandlePushNotificationAction(Dictionary<string, object> dict, object userData)
	{
		TFUtils.DebugLog("Received push notification action");
		if (dict.ContainsKey("id"))
		{
			string a = (string)dict["id"];
			if (a == "1")
			{
				TFUtils.GotoAppstore();
			}
		}
	}

	// Token: 0x040001A1 RID: 417
	public const string PUSH_NOTIFICATION_ACTION_VALUE = "push_notification_action";
}
