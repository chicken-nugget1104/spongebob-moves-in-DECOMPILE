using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class AmazonGameCircleExampleAchievements : AmazonGameCircleExampleBase
{
	// Token: 0x0600018C RID: 396 RVA: 0x00007A70 File Offset: 0x00005C70
	public AmazonGameCircleExampleAchievements()
	{
		this.invalidAchievement = new AGSAchievement();
		this.invalidAchievement.title = "Invalid Achievement Title";
		this.invalidAchievement.id = "Invalid Achievement ID";
		this.invalidAchievement.progress = 0f;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x00007AF0 File Offset: 0x00005CF0
	public override string MenuTitle()
	{
		return "Achievements";
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00007AF8 File Offset: 0x00005CF8
	public override void DrawMenu()
	{
		if (GUILayout.Button("Achievements Overlay", new GUILayoutOption[0]))
		{
			AGSAchievementsClient.ShowAchievementsOverlay();
		}
		if (string.IsNullOrEmpty(this.requestAchievementsStatus))
		{
			if (GUILayout.Button("Request Achievements", new GUILayoutOption[0]))
			{
				this.RequestAchievements();
			}
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.requestAchievementsStatus, new GUILayoutOption[0]);
			if (!string.IsNullOrEmpty(this.requestAchievementsStatusMessage))
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.requestAchievementsStatusMessage, new GUILayoutOption[0]);
			}
			if (!this.achievementsReady)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("{0,5:N1} seconds", (DateTime.Now - this.achievementsRequestTime).TotalSeconds), new GUILayoutOption[0]);
			}
			else
			{
				if (this.achievementList != null && this.achievementList.Count > 0)
				{
					foreach (AGSAchievement achievement in this.achievementList)
					{
						this.DisplayAchievement(achievement);
					}
				}
				else
				{
					AmazonGameCircleExampleGUIHelpers.CenteredLabel("No Achievements Available", new GUILayoutOption[0]);
				}
				if (this.invalidAchievement != null)
				{
					this.DisplayAchievement(this.invalidAchievement);
				}
			}
		}
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00007C64 File Offset: 0x00005E64
	private void DisplayAchievement(AGSAchievement achievement)
	{
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		if (!this.achievementsFoldout.ContainsKey(achievement.id))
		{
			this.achievementsFoldout.Add(achievement.id, false);
		}
		this.achievementsFoldout[achievement.id] = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.achievementsFoldout[achievement.id], string.Format("Achievement \"{0}\"", achievement.id));
		if (this.achievementsFoldout[achievement.id])
		{
			AmazonGameCircleExampleGUIHelpers.AnchoredLabel(this.AddNewlineEveryThirdComma(achievement.ToString()), TextAnchor.UpperCenter, new GUILayoutOption[0]);
			if (!this.achievementsSubmissionStatus.ContainsKey(achievement.id) || string.IsNullOrEmpty(this.achievementsSubmissionStatus[achievement.id]))
			{
				achievement.progress = AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider(achievement.progress, -200f, 200f, "{0}%");
				if (GUILayout.Button("Submit Achievement Progress", new GUILayoutOption[0]))
				{
					this.SubmitAchievement(achievement.id, achievement.progress);
				}
			}
			else
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.achievementsSubmissionStatus[achievement.id], new GUILayoutOption[0]);
				if (this.achievementsSubmissionStatusMessage.ContainsKey(achievement.id) && !string.IsNullOrEmpty(this.achievementsSubmissionStatusMessage[achievement.id]))
				{
					AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.achievementsSubmissionStatusMessage[achievement.id], new GUILayoutOption[0]);
				}
			}
		}
		GUILayout.EndVertical();
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00007E00 File Offset: 0x00006000
	private string AddNewlineEveryThirdComma(string stringToChange)
	{
		return this.addNewlineEveryThirdCommaRegex.Replace(stringToChange, (Match regexMatchEvaluator) => "," + regexMatchEvaluator.Groups[2].Value + ",\n");
	}

	// Token: 0x06000191 RID: 401 RVA: 0x00007E2C File Offset: 0x0000602C
	private void RequestAchievements()
	{
		this.achievementsRequestTime = DateTime.Now;
		this.SubscribeToAchievementRequestEvents();
		AGSAchievementsClient.RequestAchievements();
		this.requestAchievementsStatus = "Requesting Achievements...";
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00007E50 File Offset: 0x00006050
	private void SubmitAchievement(string achievementId, float progress)
	{
		this.SubscribeToSubmitAchievementEvents();
		AGSAchievementsClient.UpdateAchievementProgress(achievementId, progress);
		if (!this.achievementsSubmissionStatus.ContainsKey(achievementId))
		{
			this.achievementsSubmissionStatus.Add(achievementId, null);
		}
		this.achievementsSubmissionStatus[achievementId] = string.Format("Submitting Achievement...", new object[0]);
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00007EA4 File Offset: 0x000060A4
	private void SubscribeToAchievementRequestEvents()
	{
		AGSAchievementsClient.RequestAchievementsFailedEvent += this.RequestAchievementsFailed;
		AGSAchievementsClient.RequestAchievementsSucceededEvent += this.RequestAchievementsSucceeded;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x00007ED4 File Offset: 0x000060D4
	private void UnsubscribeFromAchievementRequestEvents()
	{
		AGSAchievementsClient.RequestAchievementsFailedEvent -= this.RequestAchievementsFailed;
		AGSAchievementsClient.RequestAchievementsSucceededEvent -= this.RequestAchievementsSucceeded;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00007F04 File Offset: 0x00006104
	private void SubscribeToSubmitAchievementEvents()
	{
		AGSAchievementsClient.UpdateAchievementFailedEvent += this.UpdateAchievementsFailed;
		AGSAchievementsClient.UpdateAchievementSucceededEvent += this.UpdateAchievementsSucceeded;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00007F34 File Offset: 0x00006134
	private void UnsubscribeFromSubmitAchievementEvents()
	{
		AGSAchievementsClient.UpdateAchievementFailedEvent -= this.UpdateAchievementsFailed;
		AGSAchievementsClient.UpdateAchievementSucceededEvent -= this.UpdateAchievementsSucceeded;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x00007F64 File Offset: 0x00006164
	private void RequestAchievementsFailed(string error)
	{
		this.requestAchievementsStatus = "Request Achievements failed with error:";
		this.requestAchievementsStatusMessage = error;
		this.UnsubscribeFromAchievementRequestEvents();
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00007F80 File Offset: 0x00006180
	private void RequestAchievementsSucceeded(List<AGSAchievement> achievements)
	{
		this.requestAchievementsStatus = "Available Achievements";
		this.achievementList = achievements;
		this.achievementsReady = true;
		this.UnsubscribeFromAchievementRequestEvents();
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00007FA4 File Offset: 0x000061A4
	private void UpdateAchievementsFailed(string achievementId, string error)
	{
		if (string.IsNullOrEmpty(achievementId))
		{
			Debug.LogError("AmazonGameCircleExampleAchievements received GameCircle plugin callback with invalid achievement ID.");
			return;
		}
		if (string.IsNullOrEmpty(error))
		{
			error = "MISSING ERROR STRING";
		}
		if (!this.achievementsSubmissionStatus.ContainsKey(achievementId))
		{
			this.achievementsSubmissionStatus.Add(achievementId, null);
		}
		if (!this.achievementsSubmissionStatusMessage.ContainsKey(achievementId))
		{
			this.achievementsSubmissionStatusMessage.Add(achievementId, null);
		}
		this.achievementsSubmissionStatus[achievementId] = string.Format("Achievement \"{0}\" failed with error:", achievementId);
		this.achievementsSubmissionStatusMessage[achievementId] = error;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000803C File Offset: 0x0000623C
	private void UpdateAchievementsSucceeded(string achievementId)
	{
		if (!this.achievementsSubmissionStatus.ContainsKey(achievementId))
		{
			this.achievementsSubmissionStatus.Add(achievementId, null);
		}
		this.achievementsSubmissionStatus[achievementId] = string.Format("Achievement \"{0}\" uploaded successfully.", achievementId);
	}

	// Token: 0x04000082 RID: 130
	private const int betweenCommaRegexGroup = 2;

	// Token: 0x04000083 RID: 131
	private const string achievementsMenuTitle = "Achievements";

	// Token: 0x04000084 RID: 132
	private const string displayAchievementOverlayButtonLabel = "Achievements Overlay";

	// Token: 0x04000085 RID: 133
	private const string achievementProgressLabel = "Achievement \"{0}\"";

	// Token: 0x04000086 RID: 134
	private const string submitAchievementButtonLabel = "Submit Achievement Progress";

	// Token: 0x04000087 RID: 135
	private const string achievementFailedLabel = "Achievement \"{0}\" failed with error:";

	// Token: 0x04000088 RID: 136
	private const string achievementSucceededLabel = "Achievement \"{0}\" uploaded successfully.";

	// Token: 0x04000089 RID: 137
	private const string achievementPercent = "{0}%";

	// Token: 0x0400008A RID: 138
	private const string requestAchievementsButtonLabel = "Request Achievements";

	// Token: 0x0400008B RID: 139
	private const string requestingAchievementsLabel = "Requesting Achievements...";

	// Token: 0x0400008C RID: 140
	private const string requestAchievementsFailedLabel = "Request Achievements failed with error:";

	// Token: 0x0400008D RID: 141
	private const string requestAchievementsSucceededLabel = "Available Achievements";

	// Token: 0x0400008E RID: 142
	private const string noAchievementsAvailableLabel = "No Achievements Available";

	// Token: 0x0400008F RID: 143
	private const string achievementRequestTimeLabel = "{0,5:N1} seconds";

	// Token: 0x04000090 RID: 144
	private const string submittingInformationString = "Submitting Achievement...";

	// Token: 0x04000091 RID: 145
	private const string updateAchievementsReturnedMissingAchievementId = "AmazonGameCircleExampleAchievements received GameCircle plugin callback with invalid achievement ID.";

	// Token: 0x04000092 RID: 146
	private const string noErrorMessageReceived = "MISSING ERROR STRING";

	// Token: 0x04000093 RID: 147
	private const float achievementMinValue = -200f;

	// Token: 0x04000094 RID: 148
	private const float achievementMaxValue = 200f;

	// Token: 0x04000095 RID: 149
	private Dictionary<string, string> achievementsSubmissionStatus = new Dictionary<string, string>();

	// Token: 0x04000096 RID: 150
	private Dictionary<string, string> achievementsSubmissionStatusMessage = new Dictionary<string, string>();

	// Token: 0x04000097 RID: 151
	private Dictionary<string, bool> achievementsFoldout = new Dictionary<string, bool>();

	// Token: 0x04000098 RID: 152
	private string requestAchievementsStatus;

	// Token: 0x04000099 RID: 153
	private string requestAchievementsStatusMessage;

	// Token: 0x0400009A RID: 154
	private List<AGSAchievement> achievementList;

	// Token: 0x0400009B RID: 155
	private bool achievementsReady;

	// Token: 0x0400009C RID: 156
	private DateTime achievementsRequestTime;

	// Token: 0x0400009D RID: 157
	private AGSAchievement invalidAchievement;

	// Token: 0x0400009E RID: 158
	private readonly Regex addNewlineEveryThirdCommaRegex = new Regex("(,([^,]+,[^,]+),)");
}
