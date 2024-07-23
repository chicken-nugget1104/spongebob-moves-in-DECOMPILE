using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class AmazonGameCircleExampleProfiles : AmazonGameCircleExampleBase
{
	// Token: 0x060001CB RID: 459 RVA: 0x0000902C File Offset: 0x0000722C
	public override string MenuTitle()
	{
		return "User Profile";
	}

	// Token: 0x060001CC RID: 460 RVA: 0x00009034 File Offset: 0x00007234
	public override void DrawMenu()
	{
		if (string.IsNullOrEmpty(this.playerProfileStatus))
		{
			if (GUILayout.Button("Retrieve local player data", new GUILayoutOption[0]))
			{
				this.RequestLocalPlayerData();
			}
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.playerProfileStatus, new GUILayoutOption[0]);
			if (!string.IsNullOrEmpty(this.playerProfileStatusMessage))
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.playerProfileStatusMessage, new GUILayoutOption[0]);
			}
			if (this.playerProfile != null)
			{
				string arg = string.IsNullOrEmpty(this.playerProfile.playerId) ? "null" : this.playerProfile.playerId;
				string arg2 = string.IsNullOrEmpty(this.playerProfile.alias) ? "null" : this.playerProfile.alias;
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("ID {0} : Alias {1}", arg, arg2), new GUILayoutOption[0]);
			}
		}
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000911C File Offset: 0x0000731C
	private void RequestLocalPlayerData()
	{
		this.SubscribeToProfileEvents();
		AGSProfilesClient.RequestLocalPlayerProfile();
		this.playerProfileStatus = "Retrieving local player data...";
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00009134 File Offset: 0x00007334
	private void SubscribeToProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent += this.PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent += this.PlayerAliasFailed;
	}

	// Token: 0x060001CF RID: 463 RVA: 0x00009164 File Offset: 0x00007364
	private void UnsubscribeFromProfileEvents()
	{
		AGSProfilesClient.PlayerAliasReceivedEvent -= this.PlayerAliasReceived;
		AGSProfilesClient.PlayerAliasFailedEvent -= this.PlayerAliasFailed;
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x00009194 File Offset: 0x00007394
	private void PlayerAliasReceived(AGSProfile profile)
	{
		this.playerProfileStatus = "Retrieved local player data";
		this.playerProfileStatusMessage = null;
		this.playerProfile = profile;
		this.UnsubscribeFromProfileEvents();
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x000091B8 File Offset: 0x000073B8
	private void PlayerAliasFailed(string errorMessage)
	{
		this.playerProfileStatus = "Failed to retrieve local player data";
		this.playerProfileStatusMessage = errorMessage;
		this.UnsubscribeFromProfileEvents();
	}

	// Token: 0x040000E6 RID: 230
	private const string profileMenuTitle = "User Profile";

	// Token: 0x040000E7 RID: 231
	private const string playerAliasReceivedLabel = "Retrieved local player data";

	// Token: 0x040000E8 RID: 232
	private const string playerAliasFailedLabel = "Failed to retrieve local player data";

	// Token: 0x040000E9 RID: 233
	private const string playerAliasRetrieveButtonLabel = "Retrieve local player data";

	// Token: 0x040000EA RID: 234
	private const string playerProfileLabel = "ID {0} : Alias {1}";

	// Token: 0x040000EB RID: 235
	private const string playerAliasRetrievingLabel = "Retrieving local player data...";

	// Token: 0x040000EC RID: 236
	private const string nullAsString = "null";

	// Token: 0x040000ED RID: 237
	private string playerProfileStatus;

	// Token: 0x040000EE RID: 238
	private string playerProfileStatusMessage;

	// Token: 0x040000EF RID: 239
	private AGSProfile playerProfile;
}
