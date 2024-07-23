using System;
using Prime31;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class EtceteraUIManagerTwo : MonoBehaviourGUI
{
	// Token: 0x0600039D RID: 925 RVA: 0x0000FDDC File Offset: 0x0000DFDC
	private void OnGUI()
	{
		base.beginColumn();
		if (GUILayout.Button("Show Inline Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.inlineWebViewShow("http://prime31.com/", 160, 430, Screen.width - 160, Screen.height - 100);
		}
		if (GUILayout.Button("Close Inline Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.inlineWebViewClose();
		}
		if (GUILayout.Button("Set Url of Inline Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.inlineWebViewSetUrl("http://google.com");
		}
		if (GUILayout.Button("Set Frame of Inline Web View", new GUILayoutOption[0]))
		{
			EtceteraAndroid.inlineWebViewSetFrame(80, 50, 300, 400);
		}
		base.endColumn(true);
		if (GUILayout.Button("Schedule Notification in 5 Seconds", new GUILayoutOption[0]))
		{
			EtceteraAndroid.scheduleNotification(5L, "Notiifcation Title", "The subtitle of the notification", "Ticker text gets ticked", "my-special-data");
		}
		if (GUILayout.Button("Schedule Notification in 10 Seconds", new GUILayoutOption[0]))
		{
			EtceteraAndroid.scheduleNotification(10L, "Notiifcation Title", "The subtitle of the notification", "Ticker text gets ticked", "my-special-data");
		}
		if (GUILayout.Button("Check for Noitifications", new GUILayoutOption[0]))
		{
			EtceteraAndroid.checkForNotifications();
		}
		if (GUILayout.Button("Quit App", new GUILayoutOption[0]))
		{
			Application.Quit();
		}
		base.endColumn();
		if (base.bottomRightButton("Previous Scene", 150f))
		{
			Application.LoadLevel("EtceteraTestScene");
		}
	}
}
