using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class SBGUIGetJellyDialog : SBGUIModalDialog
{
	// Token: 0x060004CB RID: 1227 RVA: 0x0001E364 File Offset: 0x0001C564
	protected override void Awake()
	{
		base.Awake();
		this.messageLabel = (SBGUILabel)base.FindChild("message_label");
		this.questionLabel = (SBGUILabel)base.FindChild("question_label");
		this.titleLabel = (SBGUILabel)base.FindChild("title_label");
		this.acceptButtonLabel = (SBGUILabel)base.FindChild("okay_label");
		this.cancelButtonLabel = (SBGUILabel)base.FindChild("cancel_label");
		this.acceptButton = (SBGUIButton)base.FindChild("okay_button");
		this.cancelButton = (SBGUIButton)base.FindChild("cancel_button");
		this.originalAcceptButtonPosition = this.acceptButton.transform.localPosition;
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x0001E428 File Offset: 0x0001C628
	private void Start()
	{
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0001E42C File Offset: 0x0001C62C
	public void SetUp(string title, string message, string question, string acceptButtonLabel, string cancelButtonLabel, Dictionary<string, int> resources)
	{
		this.titleLabel.SetText(title);
		this.messageLabel.SetText(message);
		this.questionLabel.SetText(question);
		this.acceptButtonLabel.SetText(acceptButtonLabel);
		this.cancelButtonLabel.SetText(cancelButtonLabel);
		this.cancelButton.SetActive(true);
		this.acceptButton.tform.localPosition = this.originalAcceptButtonPosition;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0001E4A0 File Offset: 0x0001C6A0
	public float GetMainWindowZ()
	{
		return base.gameObject.transform.FindChild("window").localPosition.z;
	}

	// Token: 0x04000395 RID: 917
	private SBGUILabel messageLabel;

	// Token: 0x04000396 RID: 918
	private SBGUILabel questionLabel;

	// Token: 0x04000397 RID: 919
	private SBGUILabel titleLabel;

	// Token: 0x04000398 RID: 920
	private SBGUILabel acceptButtonLabel;

	// Token: 0x04000399 RID: 921
	private SBGUILabel cancelButtonLabel;

	// Token: 0x0400039A RID: 922
	private SBGUIButton acceptButton;

	// Token: 0x0400039B RID: 923
	private SBGUIButton cancelButton;

	// Token: 0x0400039C RID: 924
	private Vector3 originalAcceptButtonPosition;
}
