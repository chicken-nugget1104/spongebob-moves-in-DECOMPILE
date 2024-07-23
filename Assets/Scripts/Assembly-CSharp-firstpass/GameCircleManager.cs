using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class GameCircleManager : MonoBehaviour
{
	// Token: 0x0600022E RID: 558 RVA: 0x0000B64C File Offset: 0x0000984C
	public static GameCircleManager getInstance()
	{
		if (GameCircleManager.instance == null)
		{
			GameCircleManager.instance = new GameObject
			{
				name = "GameCircleManager"
			}.AddComponent<GameCircleManager>();
		}
		return GameCircleManager.instance;
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000B68C File Offset: 0x0000988C
	private void Awake()
	{
		base.gameObject.name = base.GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000B6B8 File Offset: 0x000098B8
	public void serviceReady(string empty)
	{
		AGSClient.Log("GameCircleManager - serviceReady");
		AGSClient.ServiceReady(empty);
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000B6CC File Offset: 0x000098CC
	public void serviceNotReady(string param)
	{
		AGSClient.Log("GameCircleManager - serviceNotReady");
		AGSClient.ServiceNotReady(param);
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000B6E0 File Offset: 0x000098E0
	public void playerAliasReceived(string json)
	{
		AGSClient.Log("GameCircleManager - playerAliasReceived");
		AGSProfilesClient.PlayerAliasReceived(json);
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000B6F4 File Offset: 0x000098F4
	public void playerAliasFailed(string json)
	{
		AGSClient.Log("GameCircleManager - playerAliasFailed");
		AGSProfilesClient.PlayerAliasFailed(json);
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000B708 File Offset: 0x00009908
	public void submitScoreFailed(string json)
	{
		AGSClient.Log("GameCircleManager - submitScoreFailed");
		AGSLeaderboardsClient.SubmitScoreFailed(json);
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000B71C File Offset: 0x0000991C
	public void submitScoreSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - submitScoreSucceeded");
		AGSLeaderboardsClient.SubmitScoreSucceeded(json);
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000B730 File Offset: 0x00009930
	public void requestLeaderboardsFailed(string json)
	{
		AGSClient.Log("GameCircleManager - requestLeaderboardsFailed");
		AGSLeaderboardsClient.RequestLeaderboardsFailed(json);
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000B744 File Offset: 0x00009944
	public void requestLeaderboardsSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - requestLeaderboardsSucceeded");
		AGSLeaderboardsClient.RequestLeaderboardsSucceeded(json);
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000B758 File Offset: 0x00009958
	public void requestLocalPlayerScoreFailed(string json)
	{
		AGSClient.Log("GameCircleManager - requestLocalPlayerScoreFailed");
		AGSLeaderboardsClient.RequestLocalPlayerScoreFailed(json);
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000B76C File Offset: 0x0000996C
	public void requestLocalPlayerScoreSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - requestLocalPlayerScoreSucceeded");
		AGSLeaderboardsClient.RequestLocalPlayerScoreSucceeded(json);
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000B780 File Offset: 0x00009980
	public void updateAchievementSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - updateAchievementSucceeded");
		AGSAchievementsClient.UpdateAchievementSucceeded(json);
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000B794 File Offset: 0x00009994
	public void updateAchievementFailed(string json)
	{
		AGSClient.Log("GameCircleManager - updateAchievementsFailed");
		AGSAchievementsClient.UpdateAchievementFailed(json);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000B7A8 File Offset: 0x000099A8
	public void requestAchievementsSucceeded(string json)
	{
		AGSClient.Log("GameCircleManager - requestAchievementsSucceeded");
		AGSAchievementsClient.RequestAchievementsSucceeded(json);
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0000B7BC File Offset: 0x000099BC
	public void requestAchievementsFailed(string json)
	{
		AGSClient.Log("GameCircleManager -  requestAchievementsFailed");
		AGSAchievementsClient.RequestAchievementsFailed(json);
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000B7D0 File Offset: 0x000099D0
	public void onNewCloudData(string empty)
	{
		AGSWhispersyncClient.OnNewCloudData();
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000B7D8 File Offset: 0x000099D8
	public void OnApplicationFocus(bool focusStatus)
	{
		if (!AGSClient.IsServiceReady())
		{
			return;
		}
		if (!focusStatus)
		{
			AGSClient.release();
		}
	}

	// Token: 0x04000184 RID: 388
	public static GameCircleManager instance;
}
