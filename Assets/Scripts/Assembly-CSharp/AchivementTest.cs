using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class AchivementTest : MonoBehaviour
{
	// Token: 0x060001FA RID: 506 RVA: 0x00009A2C File Offset: 0x00007C2C
	private void Start()
	{
	}

	// Token: 0x060001FB RID: 507 RVA: 0x00009A30 File Offset: 0x00007C30
	private void Update()
	{
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00009A34 File Offset: 0x00007C34
	private void OnGUI()
	{
		float num = 5f;
		float left = 5f;
		float width = (float)((Screen.width < 800 && Screen.height < 800) ? 160 : 320);
		float num2 = (float)((Screen.width < 800 && Screen.height < 800) ? 40 : 80);
		float num3 = num2 + 10f;
		if (GUI.Button(new Rect(left, num, width, num2), "Init"))
		{
			this.ServiceReadyEvent();
			AGSClient.Init(false, true, false);
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "show achivement"))
		{
			AGSAchievementsClient.ShowAchievementsOverlay();
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "update achiveent"))
		{
			AGSAchievementsClient.UpdateAchievementProgress("achievement1", 50f);
		}
		if (GUI.Button(new Rect(left, num += num3, width, num2), "get userId"))
		{
			AGSProfilesClient.PlayerAliasReceivedEvent += this.PlayerAliasReceived;
			AGSProfilesClient.PlayerAliasFailedEvent += this.PlayerAliasFailed;
			AGSProfilesClient.RequestLocalPlayerProfile();
		}
		if (GUI.Button(new Rect(left, num + num3, width, num2), "load"))
		{
			Application.LoadLevel("Scene0");
		}
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00009B90 File Offset: 0x00007D90
	private void ServiceReadyHandler()
	{
		Debug.LogError("ServiceReadyHandler");
		this.UnServiceReadyEvent();
		this.SubscribeToProfileEvents();
		AGSProfilesClient.RequestLocalPlayerProfile();
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00009BB0 File Offset: 0x00007DB0
	private void ServiceNotReadyHandler(string error)
	{
		Debug.LogError("ServiceNotReadyHandler");
		this.UnServiceReadyEvent();
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00009BC4 File Offset: 0x00007DC4
	private void ServiceReadyEvent()
	{
		AGSClient.ServiceReadyEvent += this.ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent += this.ServiceNotReadyHandler;
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00009BF4 File Offset: 0x00007DF4
	private void UnServiceReadyEvent()
	{
		AGSClient.ServiceReadyEvent -= this.ServiceReadyHandler;
		AGSClient.ServiceNotReadyEvent -= this.ServiceNotReadyHandler;
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00009C24 File Offset: 0x00007E24
	private void PlayerAliasReceived(AGSProfile profile)
	{
		Debug.LogError("PlayerAliasReceived");
		Debug.LogError("profile.playerId " + profile.playerId);
		this.UnsubscribeFromProfileEvents();
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00009C4C File Offset: 0x00007E4C
	private void PlayerAliasFailed(string errorMessage)
	{
		Debug.LogError("PlayerAliasFailed " + errorMessage);
		this.UnsubscribeFromProfileEvents();
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00009C64 File Offset: 0x00007E64
	private void SubscribeToProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent += this.PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent += this.PlayerAliasFailed;
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00009C94 File Offset: 0x00007E94
	private void UnsubscribeFromProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent -= this.PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent -= this.PlayerAliasFailed;
	}
}
