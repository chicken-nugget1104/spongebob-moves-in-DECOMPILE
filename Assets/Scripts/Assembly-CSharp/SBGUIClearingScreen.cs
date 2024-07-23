using System;

// Token: 0x0200006B RID: 107
public class SBGUIClearingScreen : SBGUIModalDialog
{
	// Token: 0x0600041A RID: 1050 RVA: 0x00018D54 File Offset: 0x00016F54
	protected override void Awake()
	{
		base.Awake();
		this.messageLabel = (SBGUILabel)base.FindChild("message_label");
		this.titleLabel = (SBGUILabel)base.FindChild("title_label");
		this.costLabel = (SBGUILabel)base.FindChild("cost_label");
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00018DAC File Offset: 0x00016FAC
	public void SetUp(string title, string message, string cost)
	{
		this.titleLabel.SetText(title);
		this.messageLabel.SetText(message);
		this.costLabel.SetText(cost);
	}

	// Token: 0x04000302 RID: 770
	private SBGUILabel messageLabel;

	// Token: 0x04000303 RID: 771
	private SBGUILabel titleLabel;

	// Token: 0x04000304 RID: 772
	private SBGUILabel costLabel;
}
