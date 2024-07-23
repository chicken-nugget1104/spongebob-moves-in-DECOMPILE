using System;
using System.Collections.Generic;
using Prime31;
using UnityEngine;

// Token: 0x020000BE RID: 190
public class GPGSEventListener : MonoBehaviour
{
	// Token: 0x06000760 RID: 1888 RVA: 0x0001BA70 File Offset: 0x00019C70
	private void OnEnable()
	{
		GPGManager.authenticationSucceededEvent += this.authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent += this.authenticationFailedEvent;
		GPGManager.licenseCheckFailedEvent += this.licenseCheckFailedEvent;
		GPGManager.profileImageLoadedAtPathEvent += this.profileImageLoadedAtPathEvent;
		GPGManager.userSignedOutEvent += this.userSignedOutEvent;
		GPGManager.reloadDataForKeyFailedEvent += this.reloadDataForKeyFailedEvent;
		GPGManager.reloadDataForKeySucceededEvent += this.reloadDataForKeySucceededEvent;
		GPGManager.loadCloudDataForKeyFailedEvent += this.loadCloudDataForKeyFailedEvent;
		GPGManager.loadCloudDataForKeySucceededEvent += this.loadCloudDataForKeySucceededEvent;
		GPGManager.updateCloudDataForKeyFailedEvent += this.updateCloudDataForKeyFailedEvent;
		GPGManager.updateCloudDataForKeySucceededEvent += this.updateCloudDataForKeySucceededEvent;
		GPGManager.clearCloudDataForKeyFailedEvent += this.clearCloudDataForKeyFailedEvent;
		GPGManager.clearCloudDataForKeySucceededEvent += this.clearCloudDataForKeySucceededEvent;
		GPGManager.deleteCloudDataForKeyFailedEvent += this.deleteCloudDataForKeyFailedEvent;
		GPGManager.deleteCloudDataForKeySucceededEvent += this.deleteCloudDataForKeySucceededEvent;
		GPGManager.unlockAchievementFailedEvent += this.unlockAchievementFailedEvent;
		GPGManager.unlockAchievementSucceededEvent += this.unlockAchievementSucceededEvent;
		GPGManager.incrementAchievementFailedEvent += this.incrementAchievementFailedEvent;
		GPGManager.incrementAchievementSucceededEvent += this.incrementAchievementSucceededEvent;
		GPGManager.revealAchievementFailedEvent += this.revealAchievementFailedEvent;
		GPGManager.revealAchievementSucceededEvent += this.revealAchievementSucceededEvent;
		GPGManager.submitScoreFailedEvent += this.submitScoreFailedEvent;
		GPGManager.submitScoreSucceededEvent += this.submitScoreSucceededEvent;
		GPGManager.loadScoresFailedEvent += this.loadScoresFailedEvent;
		GPGManager.loadScoresSucceededEvent += this.loadScoresSucceededEvent;
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x0001BC28 File Offset: 0x00019E28
	private void OnDisable()
	{
		GPGManager.authenticationSucceededEvent -= this.authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent -= this.authenticationFailedEvent;
		GPGManager.licenseCheckFailedEvent -= this.licenseCheckFailedEvent;
		GPGManager.profileImageLoadedAtPathEvent -= this.profileImageLoadedAtPathEvent;
		GPGManager.userSignedOutEvent -= this.userSignedOutEvent;
		GPGManager.reloadDataForKeyFailedEvent -= this.reloadDataForKeyFailedEvent;
		GPGManager.reloadDataForKeySucceededEvent -= this.reloadDataForKeySucceededEvent;
		GPGManager.loadCloudDataForKeyFailedEvent -= this.loadCloudDataForKeyFailedEvent;
		GPGManager.loadCloudDataForKeySucceededEvent -= this.loadCloudDataForKeySucceededEvent;
		GPGManager.updateCloudDataForKeyFailedEvent -= this.updateCloudDataForKeyFailedEvent;
		GPGManager.updateCloudDataForKeySucceededEvent -= this.updateCloudDataForKeySucceededEvent;
		GPGManager.clearCloudDataForKeyFailedEvent -= this.clearCloudDataForKeyFailedEvent;
		GPGManager.clearCloudDataForKeySucceededEvent -= this.clearCloudDataForKeySucceededEvent;
		GPGManager.deleteCloudDataForKeyFailedEvent -= this.deleteCloudDataForKeyFailedEvent;
		GPGManager.deleteCloudDataForKeySucceededEvent -= this.deleteCloudDataForKeySucceededEvent;
		GPGManager.unlockAchievementFailedEvent -= this.unlockAchievementFailedEvent;
		GPGManager.unlockAchievementSucceededEvent -= this.unlockAchievementSucceededEvent;
		GPGManager.incrementAchievementFailedEvent -= this.incrementAchievementFailedEvent;
		GPGManager.incrementAchievementSucceededEvent -= this.incrementAchievementSucceededEvent;
		GPGManager.revealAchievementFailedEvent -= this.revealAchievementFailedEvent;
		GPGManager.revealAchievementSucceededEvent -= this.revealAchievementSucceededEvent;
		GPGManager.submitScoreFailedEvent -= this.submitScoreFailedEvent;
		GPGManager.submitScoreSucceededEvent -= this.submitScoreSucceededEvent;
		GPGManager.loadScoresFailedEvent -= this.loadScoresFailedEvent;
		GPGManager.loadScoresSucceededEvent -= this.loadScoresSucceededEvent;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0001BDE0 File Offset: 0x00019FE0
	private void authenticationSucceededEvent(string param)
	{
		Debug.Log("authenticationSucceededEvent: " + param);
		Debug.Log("Player Authenticated - Attempting to load achievements and leaderboards");
		GameCenterBinding.loadProfilePhotoForLocalPlayer();
		GameCenterBinding.getAchievements();
		GameCenterBinding.retrieveAchievementMetadata();
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0001BE0C File Offset: 0x0001A00C
	private void authenticationFailedEvent(string error)
	{
		Debug.Log("authenticationFailedEvent: " + error);
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x0001BE20 File Offset: 0x0001A020
	private void licenseCheckFailedEvent()
	{
		Debug.Log("licenseCheckFailedEvent");
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x0001BE2C File Offset: 0x0001A02C
	private void profileImageLoadedAtPathEvent(string path)
	{
		Debug.Log("profileImageLoadedAtPathEvent: " + path);
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0001BE40 File Offset: 0x0001A040
	private void userSignedOutEvent()
	{
		Debug.Log("userSignedOutEvent");
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0001BE4C File Offset: 0x0001A04C
	private void reloadDataForKeyFailedEvent(string error)
	{
		Debug.Log("reloadDataForKeyFailedEvent: " + error);
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0001BE60 File Offset: 0x0001A060
	private void reloadDataForKeySucceededEvent(string param)
	{
		Debug.Log("reloadDataForKeySucceededEvent: " + param);
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0001BE74 File Offset: 0x0001A074
	private void loadCloudDataForKeyFailedEvent(string error)
	{
		Debug.Log("loadCloudDataForKeyFailedEvent: " + error);
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x0001BE88 File Offset: 0x0001A088
	private void loadCloudDataForKeySucceededEvent(int key, string data)
	{
		Debug.Log("loadCloudDataForKeySucceededEvent:" + data);
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x0001BE9C File Offset: 0x0001A09C
	private void updateCloudDataForKeyFailedEvent(string error)
	{
		Debug.Log("updateCloudDataForKeyFailedEvent: " + error);
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x0001BEB0 File Offset: 0x0001A0B0
	private void updateCloudDataForKeySucceededEvent(int key, string data)
	{
		Debug.Log("updateCloudDataForKeySucceededEvent: " + data);
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x0001BEC4 File Offset: 0x0001A0C4
	private void clearCloudDataForKeyFailedEvent(string error)
	{
		Debug.Log("clearCloudDataForKeyFailedEvent: " + error);
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x0001BED8 File Offset: 0x0001A0D8
	private void clearCloudDataForKeySucceededEvent(string param)
	{
		Debug.Log("clearCloudDataForKeySucceededEvent: " + param);
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0001BEEC File Offset: 0x0001A0EC
	private void deleteCloudDataForKeyFailedEvent(string error)
	{
		Debug.Log("deleteCloudDataForKeyFailedEvent: " + error);
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0001BF00 File Offset: 0x0001A100
	private void deleteCloudDataForKeySucceededEvent(string param)
	{
		Debug.Log("deleteCloudDataForKeySucceededEvent: " + param);
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0001BF14 File Offset: 0x0001A114
	private void unlockAchievementFailedEvent(string achievementId, string error)
	{
		Debug.Log("unlockAchievementFailedEvent. achievementId: " + achievementId + ", error: " + error);
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x0001BF2C File Offset: 0x0001A12C
	private void unlockAchievementSucceededEvent(string achievementId, bool newlyUnlocked)
	{
		Debug.Log(string.Concat(new object[]
		{
			"unlockAchievementSucceededEvent. achievementId: ",
			achievementId,
			", newlyUnlocked: ",
			newlyUnlocked
		}));
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0001BF5C File Offset: 0x0001A15C
	private void incrementAchievementFailedEvent(string achievementId, string error)
	{
		Debug.Log("incrementAchievementFailedEvent. achievementId: " + achievementId + ", error: " + error);
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0001BF74 File Offset: 0x0001A174
	private void incrementAchievementSucceededEvent(string achievementId, bool newlyUnlocked)
	{
		Debug.Log(string.Concat(new object[]
		{
			"incrementAchievementSucceededEvent. achievementId: ",
			achievementId,
			", newlyUnlocked: ",
			newlyUnlocked
		}));
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0001BFA4 File Offset: 0x0001A1A4
	private void revealAchievementFailedEvent(string achievementId, string error)
	{
		Debug.Log("revealAchievementFailedEvent. achievementId: " + achievementId + ", error: " + error);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0001BFBC File Offset: 0x0001A1BC
	private void revealAchievementSucceededEvent(string achievementId)
	{
		Debug.Log("revealAchievementSucceededEvent: " + achievementId);
		GameCenterBinding.getAchievements();
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x0001BFD4 File Offset: 0x0001A1D4
	private void submitScoreFailedEvent(string leaderboardId, string error)
	{
		Debug.Log("submitScoreFailedEvent. leaderboardId: " + leaderboardId + ", error: " + error);
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x0001BFEC File Offset: 0x0001A1EC
	private void submitScoreSucceededEvent(string leaderboardId, Dictionary<string, object> scoreReport)
	{
		Debug.Log("submitScoreSucceededEvent");
		Utils.logObject(scoreReport);
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x0001C000 File Offset: 0x0001A200
	private void loadScoresFailedEvent(string leaderboardId, string error)
	{
		Debug.Log("loadScoresFailedEvent. leaderboardId: " + leaderboardId + ", error: " + error);
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0001C018 File Offset: 0x0001A218
	private void loadScoresSucceededEvent(List<GPGScore> scores)
	{
		Debug.Log("loadScoresSucceededEvent");
		Utils.logObject(scores);
	}
}
