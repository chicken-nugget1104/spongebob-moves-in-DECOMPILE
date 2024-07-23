using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class AmazonGameCircleExampleLeaderboards : AmazonGameCircleExampleBase
{
	// Token: 0x060001B5 RID: 437 RVA: 0x0000885C File Offset: 0x00006A5C
	public AmazonGameCircleExampleLeaderboards()
	{
		this.invalidLeaderboard = new AGSLeaderboard();
		this.invalidLeaderboard.id = "Invalid Leaderboard ID";
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x000088D4 File Offset: 0x00006AD4
	public override string MenuTitle()
	{
		return "Leaderboards";
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x000088DC File Offset: 0x00006ADC
	public override void DrawMenu()
	{
		if (GUILayout.Button("Leaderboards Overlay", new GUILayoutOption[0]))
		{
			AGSLeaderboardsClient.ShowLeaderboardsOverlay();
		}
		if (string.IsNullOrEmpty(this.requestLeaderboardsStatus))
		{
			if (GUILayout.Button("Request Leaderboards", new GUILayoutOption[0]))
			{
				this.RequestLeaderboards();
			}
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.requestLeaderboardsStatus, new GUILayoutOption[0]);
			if (!string.IsNullOrEmpty(this.requestLeaderboardsStatusMessage))
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.requestLeaderboardsStatusMessage, new GUILayoutOption[0]);
			}
			if (!this.leaderboardsReady)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("{0,5:N1} seconds", (DateTime.Now - this.leaderboardsRequestTime).TotalSeconds), new GUILayoutOption[0]);
			}
			else
			{
				if (this.leaderboardList != null && this.leaderboardList.Count > 0)
				{
					foreach (AGSLeaderboard leaderboard in this.leaderboardList)
					{
						this.DisplayLeaderboard(leaderboard);
					}
				}
				else
				{
					AmazonGameCircleExampleGUIHelpers.CenteredLabel("No Leaderboards Available", new GUILayoutOption[0]);
				}
				if (this.invalidLeaderboard != null)
				{
					this.DisplayLeaderboard(this.invalidLeaderboard);
				}
			}
		}
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00008A48 File Offset: 0x00006C48
	private void DisplayLeaderboard(AGSLeaderboard leaderboard)
	{
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		if (!this.leaderboardsFoldout.ContainsKey(leaderboard.id))
		{
			this.leaderboardsFoldout.Add(leaderboard.id, false);
		}
		this.leaderboardsFoldout[leaderboard.id] = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.leaderboardsFoldout[leaderboard.id], string.Format("Leaderboard \"{0}\"", leaderboard.id));
		if (this.leaderboardsFoldout[leaderboard.id])
		{
			AmazonGameCircleExampleGUIHelpers.AnchoredLabel(this.AddNewlineEverySecondComma(leaderboard.ToString()), TextAnchor.UpperCenter, new GUILayoutOption[0]);
			this.leaderboardScoreValue = (long)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider((float)this.leaderboardScoreValue, -10000f, 10000f, "{0} score units");
			if (this.leaderboardsSubmissionStatus.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(this.leaderboardsSubmissionStatus[leaderboard.id]))
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.leaderboardsSubmissionStatus[leaderboard.id], new GUILayoutOption[0]);
				if (this.leaderboardsSubmissionStatusMessage.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(this.leaderboardsSubmissionStatusMessage[leaderboard.id]))
				{
					AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.leaderboardsSubmissionStatusMessage[leaderboard.id], new GUILayoutOption[0]);
				}
			}
			if (GUILayout.Button("Submit Score", new GUILayoutOption[0]))
			{
				this.SubmitScoreToLeaderboard(leaderboard.id, this.leaderboardScoreValue);
			}
			if (this.leaderboardsLocalScoreStatus.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(this.leaderboardsLocalScoreStatus[leaderboard.id]))
			{
				AmazonGameCircleExampleGUIHelpers.AnchoredLabel(this.leaderboardsLocalScoreStatus[leaderboard.id], TextAnchor.UpperCenter, new GUILayoutOption[0]);
				if (this.leaderboardsLocalScoreStatusMessage.ContainsKey(leaderboard.id) && !string.IsNullOrEmpty(this.leaderboardsLocalScoreStatusMessage[leaderboard.id]))
				{
					AmazonGameCircleExampleGUIHelpers.AnchoredLabel(this.leaderboardsLocalScoreStatusMessage[leaderboard.id], TextAnchor.UpperCenter, new GUILayoutOption[0]);
				}
			}
			if (GUILayout.Button("Request Score", new GUILayoutOption[0]))
			{
				this.RequestLocalPlayerScore(leaderboard.id);
			}
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00008C9C File Offset: 0x00006E9C
	private string AddNewlineEverySecondComma(string stringToChange)
	{
		return this.addNewlineEverySecondCommaRegex.Replace(stringToChange, (Match regexMatchEvaluator) => "," + regexMatchEvaluator.Groups[2].Value + ",\n");
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00008CC8 File Offset: 0x00006EC8
	private void RequestLeaderboards()
	{
		this.leaderboardsRequestTime = DateTime.Now;
		this.SubscribeToLeaderboardRequestEvents();
		AGSLeaderboardsClient.RequestLeaderboards();
		this.requestLeaderboardsStatus = "Requesting Leaderboards...";
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00008CEC File Offset: 0x00006EEC
	private void SubmitScoreToLeaderboard(string leaderboardId, long scoreValue)
	{
		this.SubscribeToScoreSubmissionEvents();
		AGSLeaderboardsClient.SubmitScore(leaderboardId, scoreValue);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00008CFC File Offset: 0x00006EFC
	private void RequestLocalPlayerScore(string leaderboardId)
	{
		this.SubscribeToLocalPlayerScoreRequestEvents();
		AGSLeaderboardsClient.RequestLocalPlayerScore(leaderboardId, this.leaderboardScoreScope);
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00008D10 File Offset: 0x00006F10
	private void SubscribeToLeaderboardRequestEvents()
	{
		AGSLeaderboardsClient.RequestLeaderboardsFailedEvent += this.RequestLeaderboardsFailed;
		AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent += this.RequestLeaderboardsSucceeded;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00008D40 File Offset: 0x00006F40
	private void UnsubscribeFromLeaderboardRequestEvents()
	{
		AGSLeaderboardsClient.RequestLeaderboardsFailedEvent -= this.RequestLeaderboardsFailed;
		AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent -= this.RequestLeaderboardsSucceeded;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00008D70 File Offset: 0x00006F70
	private void SubscribeToScoreSubmissionEvents()
	{
		AGSLeaderboardsClient.SubmitScoreFailedEvent += this.SubmitScoreFailed;
		AGSLeaderboardsClient.SubmitScoreSucceededEvent += this.SubmitScoreSucceeded;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00008DA0 File Offset: 0x00006FA0
	private void UnsubscribeFromScoreSubmissionEvents()
	{
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= this.SubmitScoreFailed;
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= this.SubmitScoreSucceeded;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00008DD0 File Offset: 0x00006FD0
	private void SubscribeToLocalPlayerScoreRequestEvents()
	{
		AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent += this.RequestLocalPlayerScoreFailed;
		AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent += this.RequestLocalPlayerScoreSucceeded;
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00008E00 File Offset: 0x00007000
	private void UnsubscribeFromLocalPlayerScoreRequestEvents()
	{
		AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent -= this.RequestLocalPlayerScoreFailed;
		AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent -= this.RequestLocalPlayerScoreSucceeded;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00008E30 File Offset: 0x00007030
	private void RequestLeaderboardsFailed(string error)
	{
		this.requestLeaderboardsStatus = "Request Leaderboards failed with error:";
		this.requestLeaderboardsStatusMessage = error;
		this.UnsubscribeFromLeaderboardRequestEvents();
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00008E4C File Offset: 0x0000704C
	private void RequestLeaderboardsSucceeded(List<AGSLeaderboard> leaderboards)
	{
		this.requestLeaderboardsStatus = "Available Leaderboards";
		this.leaderboardList = leaderboards;
		this.leaderboardsReady = true;
		this.UnsubscribeFromLeaderboardRequestEvents();
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00008E70 File Offset: 0x00007070
	private void SubmitScoreFailed(string leaderboardId, string error)
	{
		if (!this.leaderboardsSubmissionStatus.ContainsKey(leaderboardId))
		{
			this.leaderboardsSubmissionStatus.Add(leaderboardId, null);
		}
		if (!this.leaderboardsSubmissionStatusMessage.ContainsKey(leaderboardId))
		{
			this.leaderboardsSubmissionStatusMessage.Add(leaderboardId, null);
		}
		this.leaderboardsSubmissionStatus[leaderboardId] = string.Format("Leaderboard \"{0}\" failed with error:", leaderboardId);
		this.leaderboardsSubmissionStatusMessage[leaderboardId] = error;
		this.UnsubscribeFromScoreSubmissionEvents();
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00008EE4 File Offset: 0x000070E4
	private void SubmitScoreSucceeded(string leaderboardId)
	{
		if (!this.leaderboardsSubmissionStatus.ContainsKey(leaderboardId))
		{
			this.leaderboardsSubmissionStatus.Add(leaderboardId, null);
		}
		this.leaderboardsSubmissionStatus[leaderboardId] = string.Format("Score uploaded to \"{0}\" successfully.", leaderboardId);
		this.UnsubscribeFromScoreSubmissionEvents();
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00008F2C File Offset: 0x0000712C
	private void RequestLocalPlayerScoreFailed(string leaderboardId, string error)
	{
		if (!this.leaderboardsLocalScoreStatus.ContainsKey(leaderboardId))
		{
			this.leaderboardsLocalScoreStatus.Add(leaderboardId, null);
		}
		if (!this.leaderboardsLocalScoreStatusMessage.ContainsKey(leaderboardId))
		{
			this.leaderboardsLocalScoreStatusMessage.Add(leaderboardId, null);
		}
		this.leaderboardsLocalScoreStatus[leaderboardId] = string.Format("\"{0}\" score request failed with error:", leaderboardId);
		this.leaderboardsLocalScoreStatusMessage[leaderboardId] = error;
		this.UnsubscribeFromLocalPlayerScoreRequestEvents();
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x00008FA0 File Offset: 0x000071A0
	private void RequestLocalPlayerScoreSucceeded(string leaderboardId, int rank, long score)
	{
		if (!this.leaderboardsLocalScoreStatus.ContainsKey(leaderboardId))
		{
			this.leaderboardsLocalScoreStatus.Add(leaderboardId, null);
		}
		this.leaderboardsLocalScoreStatus[leaderboardId] = string.Format("Rank {0} with score of {1,5:N1}", rank, score);
		this.UnsubscribeFromLocalPlayerScoreRequestEvents();
	}

	// Token: 0x040000C4 RID: 196
	private const int betweenCommaRegexGroup = 2;

	// Token: 0x040000C5 RID: 197
	private const string leaderboardsMenuTitle = "Leaderboards";

	// Token: 0x040000C6 RID: 198
	private const string DisplayLeaderboardOverlayButtonLabel = "Leaderboards Overlay";

	// Token: 0x040000C7 RID: 199
	private const string requestLeaderboardsButtonLabel = "Request Leaderboards";

	// Token: 0x040000C8 RID: 200
	private const string requestingLeaderboardsLabel = "Requesting Leaderboards...";

	// Token: 0x040000C9 RID: 201
	private const string requestLeaderboardsFailedLabel = "Request Leaderboards failed with error:";

	// Token: 0x040000CA RID: 202
	private const string requestLeaderboardsSucceededLabel = "Available Leaderboards";

	// Token: 0x040000CB RID: 203
	private const string noLeaderboardsAvailableLabel = "No Leaderboards Available";

	// Token: 0x040000CC RID: 204
	private const string leaderboardIDLabel = "Leaderboard \"{0}\"";

	// Token: 0x040000CD RID: 205
	private const string leaderboardRequestTimeLabel = "{0,5:N1} seconds";

	// Token: 0x040000CE RID: 206
	private const string leaderboardScoreDisplayLabel = "{0} score units";

	// Token: 0x040000CF RID: 207
	private const string submitLeaderboardButtonLabel = "Submit Score";

	// Token: 0x040000D0 RID: 208
	private const string leaderboardFailed = "Leaderboard \"{0}\" failed with error:";

	// Token: 0x040000D1 RID: 209
	private const string leaderboardSucceeded = "Score uploaded to \"{0}\" successfully.";

	// Token: 0x040000D2 RID: 210
	private const string requestLeaderboardScoreButtonLabel = "Request Score";

	// Token: 0x040000D3 RID: 211
	private const string leaderboardRankScoreLabel = "Rank {0} with score of {1,5:N1}";

	// Token: 0x040000D4 RID: 212
	private const string leaderboardScoreFailed = "\"{0}\" score request failed with error:";

	// Token: 0x040000D5 RID: 213
	private const float leaderboardMinValue = -10000f;

	// Token: 0x040000D6 RID: 214
	private const float leaderboardMaxValue = 10000f;

	// Token: 0x040000D7 RID: 215
	private Dictionary<string, string> leaderboardsSubmissionStatus = new Dictionary<string, string>();

	// Token: 0x040000D8 RID: 216
	private Dictionary<string, string> leaderboardsSubmissionStatusMessage = new Dictionary<string, string>();

	// Token: 0x040000D9 RID: 217
	private Dictionary<string, string> leaderboardsLocalScoreStatus = new Dictionary<string, string>();

	// Token: 0x040000DA RID: 218
	private Dictionary<string, string> leaderboardsLocalScoreStatusMessage = new Dictionary<string, string>();

	// Token: 0x040000DB RID: 219
	private Dictionary<string, bool> leaderboardsFoldout = new Dictionary<string, bool>();

	// Token: 0x040000DC RID: 220
	private long leaderboardScoreValue;

	// Token: 0x040000DD RID: 221
	private string requestLeaderboardsStatus;

	// Token: 0x040000DE RID: 222
	private string requestLeaderboardsStatusMessage;

	// Token: 0x040000DF RID: 223
	private List<AGSLeaderboard> leaderboardList;

	// Token: 0x040000E0 RID: 224
	private bool leaderboardsReady;

	// Token: 0x040000E1 RID: 225
	private DateTime leaderboardsRequestTime;

	// Token: 0x040000E2 RID: 226
	private LeaderboardScope leaderboardScoreScope;

	// Token: 0x040000E3 RID: 227
	private AGSLeaderboard invalidLeaderboard;

	// Token: 0x040000E4 RID: 228
	private readonly Regex addNewlineEverySecondCommaRegex = new Regex("(,([^,]+),)");
}
