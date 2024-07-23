using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class PlayGameServicesUI : MonoBehaviourGUI
{
	// Token: 0x0600077C RID: 1916 RVA: 0x0001C034 File Offset: 0x0001A234
	private void Start()
	{
		PlayGameServices.enableDebugLog(true);
		PlayGameServices.init("160040154367.apps.googleusercontent.com", true, true, true);
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x0001C04C File Offset: 0x0001A24C
	private void OnGUI()
	{
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		base.beginColumn();
		GUILayout.Label("Authentication and Settings", new GUILayoutOption[0]);
		if (GUILayout.Button("Set Toasts on Bottom", new GUILayoutOption[0]))
		{
			PlayGameServices.setAchievementToastSettings(GPGToastPlacement.Bottom, 50);
		}
		if (GUILayout.Button("Authenticate", new GUILayoutOption[0]))
		{
			PlayGameServices.authenticate();
		}
		if (GUILayout.Button("Sign Out", new GUILayoutOption[0]))
		{
			PlayGameServices.signOut();
		}
		if (GUILayout.Button("Is Signed In", new GUILayoutOption[0]))
		{
			Debug.Log("is signed in? " + PlayGameServices.isSignedIn());
		}
		if (GUILayout.Button("Get Player Info", new GUILayoutOption[0]))
		{
			GPGPlayerInfo localPlayerInfo = PlayGameServices.getLocalPlayerInfo();
			Debug.Log(localPlayerInfo);
			if (Application.platform == RuntimePlatform.Android && localPlayerInfo.avatarUrl != null)
			{
				PlayGameServices.loadProfileImageForUri(localPlayerInfo.avatarUrl);
			}
		}
		GUILayout.Label("Achievements", new GUILayoutOption[0]);
		if (GUILayout.Button("Show Achievements", new GUILayoutOption[0]))
		{
			PlayGameServices.showAchievements();
		}
		if (GUILayout.Button("Increment Achievement", new GUILayoutOption[0]))
		{
			PlayGameServices.incrementAchievement("CgkI_-mLmdQEEAIQAQ", 2);
		}
		if (GUILayout.Button("Unlock Achievment", new GUILayoutOption[0]))
		{
			PlayGameServices.unlockAchievement("CgkI_-mLmdQEEAIQAw", true);
		}
		base.endColumn(true);
		if (base.toggleButtonState("Show Cloud Save Buttons"))
		{
			this.secondColumnButtions();
		}
		else
		{
			this.cloudSaveButtons();
		}
		base.toggleButton("Show Cloud Save Buttons", "Toggle Buttons");
		base.endColumn(false);
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0001C1EC File Offset: 0x0001A3EC
	private void secondColumnButtions()
	{
		GUILayout.Label("Leaderboards", new GUILayoutOption[0]);
		if (GUILayout.Button("Show Leaderboard", new GUILayoutOption[0]))
		{
			PlayGameServices.showLeaderboard("CgkI_-mLmdQEEAIQBQ", GPGLeaderboardTimeScope.AllTime);
		}
		if (GUILayout.Button("Show All Leaderboards", new GUILayoutOption[0]))
		{
			PlayGameServices.showLeaderboards();
		}
		if (GUILayout.Button("Submit Score", new GUILayoutOption[0]))
		{
			PlayGameServices.submitScore("CgkI_-mLmdQEEAIQBQ", 567L);
		}
		if (GUILayout.Button("Load Raw Score Data", new GUILayoutOption[0]))
		{
			PlayGameServices.loadScoresForLeaderboard("CgkI_-mLmdQEEAIQBQ", GPGLeaderboardTimeScope.AllTime, false, false);
		}
		if (GUILayout.Button("Get Leaderboard Metadata", new GUILayoutOption[0]))
		{
			List<GPGLeaderboardMetadata> allLeaderboardMetadata = PlayGameServices.getAllLeaderboardMetadata();
			Utils.logObject(allLeaderboardMetadata);
		}
		if (GUILayout.Button("Get Achievement Metadata", new GUILayoutOption[0]))
		{
			List<GPGAchievementMetadata> allAchievementMetadata = PlayGameServices.getAllAchievementMetadata();
			Utils.logObject(allAchievementMetadata);
		}
		if (GUILayout.Button("Reload All Metadata", new GUILayoutOption[0]))
		{
			PlayGameServices.reloadAchievementAndLeaderboardData();
		}
		if (GUILayout.Button("loading spongeBob", new GUILayoutOption[0]))
		{
			Application.LoadLevel("Scene0");
		}
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x0001C308 File Offset: 0x0001A508
	private void cloudSaveButtons()
	{
		GUILayout.Label("Cloud Data", new GUILayoutOption[0]);
		if (GUILayout.Button("Load Cloud Data", new GUILayoutOption[0]))
		{
			PlayGameServices.loadCloudDataForKey(0, true);
		}
		if (GUILayout.Button("Set Cloud Data", new GUILayoutOption[0]))
		{
			PlayGameServices.setStateData("I'm some data. I could be JSON or XML.", 0);
		}
		if (GUILayout.Button("Update Cloud Data", new GUILayoutOption[0]))
		{
			PlayGameServices.updateCloudDataForKey(0, true);
		}
		if (GUILayout.Button("Get Cloud Data", new GUILayoutOption[0]))
		{
			string message = PlayGameServices.stateDataForKey(0);
			Debug.Log(message);
		}
		if (GUILayout.Button("Delete Cloud Data", new GUILayoutOption[0]))
		{
			PlayGameServices.deleteCloudDataForKey(0, true);
		}
		if (GUILayout.Button("Clear Cloud Data", new GUILayoutOption[0]))
		{
			PlayGameServices.clearCloudDataForKey(0, true);
		}
	}
}
