using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class SBGUIConfirmationDialog : SBGUIModalDialog
{
	// Token: 0x06000434 RID: 1076 RVA: 0x0001A260 File Offset: 0x00018460
	protected override void Awake()
	{
		base.Awake();
		this.messageLabel = (SBGUILabel)base.FindChild("message_label");
		this.messageLabelBoundary = (SBGUIAtlasImage)base.FindChild("message_label_boundary");
		this.titleLabel = (SBGUILabel)base.FindChild("title_label");
		this.acceptButtonLabel = (SBGUILabel)base.FindChild("okay_label");
		this.cancelButtonLabel = (SBGUILabel)base.FindChild("cancel_label");
		this.acceptButton = (SBGUIButton)base.FindChild("okay_button");
		this.cancelButton = (SBGUIButton)base.FindChild("cancel_button");
		this.originalAcceptButtonPosition = this.acceptButton.transform.localPosition;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x0001A324 File Offset: 0x00018524
	private void Start()
	{
		this.rewardCenter = this.rewardMarker.tform.position;
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x0001A33C File Offset: 0x0001853C
	public void SetUp(string title, string message, string acceptButtonLabel, string cancelButtonLabel, Dictionary<string, int> resources, string prefix)
	{
		this.titleLabel.SetText(title);
		this.messageLabel.SetText(message);
		this.messageLabel.AdjustText(this.messageLabelBoundary);
		this.acceptButtonLabel.SetText(acceptButtonLabel);
		if (cancelButtonLabel == null)
		{
			this.cancelButton.SetActive(false);
			this.acceptButton.tform.localPosition = new Vector3(0f, this.originalAcceptButtonPosition.y, this.originalAcceptButtonPosition.z);
		}
		else
		{
			this.cancelButtonLabel.SetText(cancelButtonLabel);
			this.cancelButton.SetActive(true);
			this.acceptButton.tform.localPosition = this.originalAcceptButtonPosition;
		}
		if (resources == null)
		{
			return;
		}
		foreach (KeyValuePair<string, int> keyValuePair in resources)
		{
			this.AddItem(keyValuePair.Key, keyValuePair.Value, prefix);
		}
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0001A468 File Offset: 0x00018668
	public override void AddItem(string texture, int amount, string prefix)
	{
		base.AddItem(texture, amount, prefix);
		base.View.RefreshEvent += this.CenterRewards;
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x0001A498 File Offset: 0x00018698
	private new void CenterRewards()
	{
		Vector3 vector = this.rewardMarker.TotalBounds.center - this.rewardCenter;
		Vector3 localPosition = this.rewardMarker.tform.localPosition;
		localPosition.x -= vector.x;
		this.rewardMarker.tform.localPosition = localPosition;
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0001A4FC File Offset: 0x000186FC
	public float GetMainWindowZ()
	{
		return base.gameObject.transform.FindChild("window").localPosition.z;
	}

	// Token: 0x04000332 RID: 818
	private SBGUILabel messageLabel;

	// Token: 0x04000333 RID: 819
	private SBGUIAtlasImage messageLabelBoundary;

	// Token: 0x04000334 RID: 820
	private SBGUILabel titleLabel;

	// Token: 0x04000335 RID: 821
	private SBGUILabel acceptButtonLabel;

	// Token: 0x04000336 RID: 822
	private SBGUILabel cancelButtonLabel;

	// Token: 0x04000337 RID: 823
	private SBGUIButton acceptButton;

	// Token: 0x04000338 RID: 824
	private SBGUIButton cancelButton;

	// Token: 0x04000339 RID: 825
	private Vector3 originalAcceptButtonPosition;

	// Token: 0x0400033A RID: 826
	private Vector3 rewardCenter;
}
