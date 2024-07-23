using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class GameCenterBinding
{
	// Token: 0x06000597 RID: 1431 RVA: 0x000159D8 File Offset: 0x00013BD8
	public static bool isGameCenterAvailable()
	{
		return false;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x000159DC File Offset: 0x00013BDC
	public static void authenticateLocalPlayer()
	{
		if (GameCenterBinding.isAmazon())
		{
			AGSClient.Init(false, true, false);
		}
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x000159F8 File Offset: 0x00013BF8
	public static bool isPlayerAuthenticated()
	{
		return GameCenterBinding.isAmazon() && AGSClient.IsServiceReady();
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00015A0C File Offset: 0x00013C0C
	public static string playerAlias()
	{
		return string.Empty;
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00015A14 File Offset: 0x00013C14
	public static string playerIdentifier()
	{
		return string.Empty;
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x00015A1C File Offset: 0x00013C1C
	public static bool isUnderage()
	{
		return false;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00015A20 File Offset: 0x00013C20
	public static void retrieveFriends(bool loadProfileImages)
	{
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x00015A24 File Offset: 0x00013C24
	public static void loadPlayerData(string[] playerIdArray, bool loadProfileImages)
	{
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00015A28 File Offset: 0x00013C28
	public static void loadProfilePhotoForLocalPlayer()
	{
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x00015A2C File Offset: 0x00013C2C
	public static void loadLeaderboardTitles()
	{
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x00015A30 File Offset: 0x00013C30
	public static void reportScore(long score, string leaderboardId)
	{
		PlayGameServices.submitScore(leaderboardId, score);
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00015A3C File Offset: 0x00013C3C
	public static void showLeaderboardWithTimeScope(GameCenterLeaderboardTimeScope timeScope)
	{
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00015A40 File Offset: 0x00013C40
	public static void showLeaderboardWithTimeScopeAndLeaderboard(GameCenterLeaderboardTimeScope timeScope, string leaderboardId)
	{
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x00015A44 File Offset: 0x00013C44
	public static void retrieveScores(bool friendsOnly, GameCenterLeaderboardTimeScope timeScope, int start, int end)
	{
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x00015A48 File Offset: 0x00013C48
	public static void retrieveScores(bool friendsOnly, GameCenterLeaderboardTimeScope timeScope, int start, int end, string leaderboardId)
	{
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00015A4C File Offset: 0x00013C4C
	public static void retrieveScoresForPlayerId(string playerId)
	{
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00015A50 File Offset: 0x00013C50
	public static void retrieveScoresForPlayerId(string playerId, string leaderboardId)
	{
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x00015A54 File Offset: 0x00013C54
	public static void reportAchievement(string identifier, float percent)
	{
		if (GameCenterBinding.isAmazon())
		{
			AGSAchievementsClient.UpdateAchievementProgress(Config.ACHIEVEMENT_ID_DIC_AMAZON[identifier], percent);
		}
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00015A84 File Offset: 0x00013C84
	public static void getAchievements()
	{
		if (!GameCenterBinding.isAmazon())
		{
			PlayGameServices.getAllAchievementMetadata();
		}
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00015A98 File Offset: 0x00013C98
	public static void resetAchievements()
	{
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00015A9C File Offset: 0x00013C9C
	public static void showAchievements()
	{
		if (GameCenterBinding.isAmazon())
		{
			AGSAchievementsClient.ShowAchievementsOverlay();
		}
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x00015AB4 File Offset: 0x00013CB4
	public static void retrieveAchievementMetadata()
	{
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00015AB8 File Offset: 0x00013CB8
	public static void showCompletionBannerForAchievements()
	{
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00015ABC File Offset: 0x00013CBC
	public static bool isAmazon()
	{
		return SystemInfo.deviceModel.StartsWith("Amazon");
	}
}
