using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class SBGameCenterManager : MonoBehaviour
{
	// Token: 0x06000270 RID: 624 RVA: 0x0000C284 File Offset: 0x0000A484
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x0000C28C File Offset: 0x0000A48C
	private void Start()
	{
		GameCenterBinding.showCompletionBannerForAchievements();
		GameCenterManager.loadPlayerDataFailedEvent += this.LoadPlayerDataFailed;
		GameCenterManager.playerAuthenticatedEvent += this.PlayerAuthenticated;
		GameCenterManager.playerFailedToAuthenticateEvent += this.PlayerFailedToAuthenticate;
		GameCenterManager.playerLoggedOutEvent += this.PlayerLoggedOut;
		GameCenterManager.profilePhotoLoadedEvent += this.ProfilePhotoLoaded;
		GameCenterManager.profilePhotoFailedEvent += this.ProfilePhotoFailed;
		GameCenterManager.reportAchievementFailedEvent += this.ReportAchievementFailed;
		GameCenterManager.reportAchievementFinishedEvent += this.ReportAchievementFinished;
		GameCenterManager.loadAchievementsFailedEvent += this.LoadAchievementsFailed;
		GameCenterManager.achievementsLoadedEvent += this.AchievementsLoaded;
		GameCenterManager.resetAchievementsFailedEvent += this.ResetAchievementsFailed;
		GameCenterManager.resetAchievementsFinishedEvent += this.ResetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailedEvent += this.RetrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoadedEvent += this.AchievementMetadataLoaded;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000C38C File Offset: 0x0000A58C
	private void OnDisable()
	{
		GameCenterManager.loadPlayerDataFailedEvent -= this.LoadPlayerDataFailed;
		GameCenterManager.playerAuthenticatedEvent -= this.PlayerAuthenticated;
		GameCenterManager.playerLoggedOutEvent -= this.PlayerLoggedOut;
		GameCenterManager.profilePhotoLoadedEvent -= this.ProfilePhotoLoaded;
		GameCenterManager.profilePhotoFailedEvent -= this.ProfilePhotoFailed;
		GameCenterManager.reportAchievementFailedEvent -= this.ReportAchievementFailed;
		GameCenterManager.reportAchievementFinishedEvent -= this.ReportAchievementFinished;
		GameCenterManager.loadAchievementsFailedEvent -= this.LoadAchievementsFailed;
		GameCenterManager.achievementsLoadedEvent -= this.AchievementsLoaded;
		GameCenterManager.resetAchievementsFailedEvent -= this.ResetAchievementsFailed;
		GameCenterManager.resetAchievementsFinishedEvent -= this.ResetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailedEvent -= this.RetrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoadedEvent -= this.AchievementMetadataLoaded;
		GameCenterBinding.isPlayerAuthenticated();
	}

	// Token: 0x06000273 RID: 627 RVA: 0x0000C47C File Offset: 0x0000A67C
	private void ResetLocalPlayer()
	{
		this.achievementMetadata.Clear();
		this.achievements.Clear();
	}

	// Token: 0x06000274 RID: 628 RVA: 0x0000C494 File Offset: 0x0000A694
	public void ReportAchievement(string achievementId, float percentComplete)
	{
		if (GameCenterBinding.isPlayerAuthenticated())
		{
			foreach (GameCenterAchievement gameCenterAchievement in this.achievements)
			{
				if (gameCenterAchievement.identifier == achievementId)
				{
					TFUtils.DebugLog("Achievement Already Earned");
					return;
				}
			}
			TFUtils.DebugLog("Achievement Earned - " + achievementId);
			GameCenterBinding.reportAchievement(achievementId, percentComplete);
		}
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0000C534 File Offset: 0x0000A734
	public void ResetAchievements()
	{
		if (GameCenterBinding.isPlayerAuthenticated())
		{
			GameCenterBinding.resetAchievements();
			this.achievements.Clear();
		}
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000C550 File Offset: 0x0000A750
	public void PlayerAuthenticated()
	{
		TFUtils.DebugLog("Player Authenticated - Attempting to load achievements and leaderboards");
		GameCenterBinding.loadProfilePhotoForLocalPlayer();
		GameCenterBinding.getAchievements();
		GameCenterBinding.retrieveAchievementMetadata();
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000C56C File Offset: 0x0000A76C
	private void PlayerFailedToAuthenticate(string error)
	{
		TFUtils.DebugLog("PlayerFailedToAuthenticate: " + error);
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000C580 File Offset: 0x0000A780
	private void PlayerLoggedOut()
	{
		TFUtils.DebugLog("playerLoggedOut");
		this.ResetLocalPlayer();
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000C594 File Offset: 0x0000A794
	private void LoadPlayerDataFailed(string error)
	{
		TFUtils.DebugLog("LoadPlayerDataFailed: " + error);
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000C5A8 File Offset: 0x0000A7A8
	private void ProfilePhotoLoaded(string path)
	{
		TFUtils.DebugLog("ProfilePhotoLoaded: " + path);
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000C5BC File Offset: 0x0000A7BC
	private void ProfilePhotoFailed(string error)
	{
		TFUtils.DebugLog("ProfilePhotoFailed: " + error);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0000C5D0 File Offset: 0x0000A7D0
	private void AchievementMetadataLoaded(List<GameCenterAchievementMetadata> achievementMetadata)
	{
		this.achievementMetadata = achievementMetadata;
		TFUtils.DebugLog("achievementMetadatLoaded");
		foreach (GameCenterAchievementMetadata message in achievementMetadata)
		{
			TFUtils.DebugLog(message);
		}
	}

	// Token: 0x0600027D RID: 637 RVA: 0x0000C644 File Offset: 0x0000A844
	private void RetrieveAchievementMetadataFailed(string error)
	{
		TFUtils.DebugLog("RetrieveAchievementMetadataFailed: " + error);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x0000C658 File Offset: 0x0000A858
	private void ResetAchievementsFinished()
	{
		TFUtils.DebugLog("resetAchievmenetsFinished");
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0000C664 File Offset: 0x0000A864
	private void ResetAchievementsFailed(string error)
	{
		TFUtils.DebugLog("ResetAchievementsFailed: " + error);
	}

	// Token: 0x06000280 RID: 640 RVA: 0x0000C678 File Offset: 0x0000A878
	private void AchievementsLoaded(List<GameCenterAchievement> achievements)
	{
		this.achievements = achievements;
		TFUtils.DebugLog("AchievementsLoaded");
		foreach (GameCenterAchievement message in achievements)
		{
			TFUtils.DebugLog(message);
		}
	}

	// Token: 0x06000281 RID: 641 RVA: 0x0000C6EC File Offset: 0x0000A8EC
	private void LoadAchievementsFailed(string error)
	{
		TFUtils.DebugLog("LoadAchievementsFailed: " + error);
	}

	// Token: 0x06000282 RID: 642 RVA: 0x0000C700 File Offset: 0x0000A900
	private void ReportAchievementFinished(string identifier)
	{
		TFUtils.DebugLog("ReportAchievementFinished: " + identifier);
		GameCenterBinding.getAchievements();
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0000C718 File Offset: 0x0000A918
	private void ReportAchievementFailed(string error)
	{
		TFUtils.DebugLog("ReportAchievementFailed: " + error);
	}

	// Token: 0x0400013F RID: 319
	public List<GameCenterAchievementMetadata> achievementMetadata = new List<GameCenterAchievementMetadata>();

	// Token: 0x04000140 RID: 320
	public List<GameCenterAchievement> achievements = new List<GameCenterAchievement>();
}
