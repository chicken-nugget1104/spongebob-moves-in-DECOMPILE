using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

// Token: 0x020000C0 RID: 192
public class PlayGameServices
{
	// Token: 0x06000781 RID: 1921 RVA: 0x0001C3E0 File Offset: 0x0001A5E0
	static PlayGameServices()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.prime31.PlayGameServicesPlugin"))
		{
			PlayGameServices._plugin = androidJavaClass.CallStatic<AndroidJavaObject>("instance", new object[0]);
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0001C44C File Offset: 0x0001A64C
	public static void setAchievementToastSettings(GPGToastPlacement placement, int offset)
	{
		PlayGameServices.setToastSettings(placement);
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x0001C454 File Offset: 0x0001A654
	public static void setWelcomeBackToastSettings(GPGToastPlacement placement, int offset)
	{
		PlayGameServices.setToastSettings(placement);
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x0001C45C File Offset: 0x0001A65C
	public static void enableDebugLog(bool shouldEnable)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("enableDebugLog", new object[]
		{
			shouldEnable
		});
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x0001C48C File Offset: 0x0001A68C
	public static void setToastSettings(GPGToastPlacement placement)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("setToastSettings", new object[]
		{
			(int)placement
		});
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x0001C4BC File Offset: 0x0001A6BC
	public static void init(string clientId, bool requestAppStateScope, bool fetchMetadataAfterAuthentication = true, bool pauseUnityWhileShowingFullScreenViews = true)
	{
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x0001C4C0 File Offset: 0x0001A6C0
	public static void authenticate()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("authenticate", new object[0]);
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0001C4F0 File Offset: 0x0001A6F0
	public static void signOut()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("signOut", new object[0]);
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x0001C520 File Offset: 0x0001A720
	public static bool isSignedIn()
	{
		return Application.platform == RuntimePlatform.Android && PlayGameServices._plugin.Call<bool>("isSignedIn", new object[0]);
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x0001C548 File Offset: 0x0001A748
	public static GPGPlayerInfo getLocalPlayerInfo()
	{
		GPGPlayerInfo gpgplayerInfo = new GPGPlayerInfo();
		if (Application.platform != RuntimePlatform.Android)
		{
			return gpgplayerInfo;
		}
		gpgplayerInfo.setDataFromJson(PlayGameServices._plugin.Call<string>("getLocalPlayerInfo", new object[0]));
		return gpgplayerInfo;
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x0001C588 File Offset: 0x0001A788
	public static void reloadAchievementAndLeaderboardData()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			PlayGameServices._plugin.Call("loadBasicModelData", new object[0]);
		}
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x0001C5AC File Offset: 0x0001A7AC
	public static void loadProfileImageForUri(string uri)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			PlayGameServices._plugin.Call("loadProfileImageForUri", new object[]
			{
				uri
			});
		}
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x0001C5D4 File Offset: 0x0001A7D4
	public static void setStateData(string data, int key)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("setStateData", new object[]
		{
			data,
			key
		});
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0001C608 File Offset: 0x0001A808
	public static string stateDataForKey(int key)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return null;
		}
		return PlayGameServices._plugin.Call<string>("stateDataForKey", new object[]
		{
			key
		});
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x0001C644 File Offset: 0x0001A844
	public static void loadCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("loadCloudDataForKey", new object[]
		{
			key,
			useRemoteDataForConflictResolution
		});
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0001C688 File Offset: 0x0001A888
	public static void deleteCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("deleteCloudDataForKey", new object[]
		{
			key,
			useRemoteDataForConflictResolution
		});
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0001C6CC File Offset: 0x0001A8CC
	public static void clearCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0001C6D0 File Offset: 0x0001A8D0
	public static void updateCloudDataForKey(int key, bool useRemoteDataForConflictResolution = true)
	{
		PlayGameServices.loadCloudDataForKey(key, useRemoteDataForConflictResolution);
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x0001C6DC File Offset: 0x0001A8DC
	public static void showAchievements()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("showAchievements", new object[0]);
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x0001C70C File Offset: 0x0001A90C
	public static void revealAchievement(string achievementId)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("revealAchievement", new object[]
		{
			achievementId
		});
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0001C740 File Offset: 0x0001A940
	public static void unlockAchievement(string achievementId, bool showsCompletionNotification = true)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("unlockAchievement", new object[]
		{
			achievementId,
			showsCompletionNotification
		});
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x0001C774 File Offset: 0x0001A974
	public static void incrementAchievement(string achievementId, int numSteps)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("incrementAchievement", new object[]
		{
			achievementId,
			numSteps
		});
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0001C7A8 File Offset: 0x0001A9A8
	public static List<GPGAchievementMetadata> getAllAchievementMetadata()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return new List<GPGAchievementMetadata>();
		}
		string json = PlayGameServices._plugin.Call<string>("getAllAchievementMetadata", new object[0]);
		return DTOBase.listFromJson<GPGAchievementMetadata>(json);
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x0001C7E4 File Offset: 0x0001A9E4
	public static void showLeaderboard(string leaderboardId, GPGLeaderboardTimeScope timeScope = GPGLeaderboardTimeScope.AllTime)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("showLeaderboard", new object[]
		{
			leaderboardId
		});
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x0001C818 File Offset: 0x0001AA18
	public static void showLeaderboards()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("showLeaderboards", new object[0]);
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x0001C848 File Offset: 0x0001AA48
	public static void submitScore(string leaderboardId, long score)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("submitScore", new object[]
		{
			leaderboardId,
			score
		});
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0001C87C File Offset: 0x0001AA7C
	public static void loadScoresForLeaderboard(string leaderboardId, GPGLeaderboardTimeScope timeScope, bool isSocial, bool personalWindow)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		PlayGameServices._plugin.Call("loadScoresForLeaderboard", new object[]
		{
			leaderboardId,
			(int)timeScope,
			isSocial,
			personalWindow
		});
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0001C8CC File Offset: 0x0001AACC
	public static List<GPGLeaderboardMetadata> getAllLeaderboardMetadata()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return new List<GPGLeaderboardMetadata>();
		}
		string json = PlayGameServices._plugin.Call<string>("getAllLeaderboardMetadata", new object[0]);
		return DTOBase.listFromJson<GPGLeaderboardMetadata>(json);
	}

	// Token: 0x040004AF RID: 1199
	private static AndroidJavaObject _plugin;
}
