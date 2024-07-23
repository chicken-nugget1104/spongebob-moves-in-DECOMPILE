using System;
using UnityEngine;

// Token: 0x0200034D RID: 845
public class SaveGameScreen1 : SBGUIScreen
{
	// Token: 0x0600184E RID: 6222 RVA: 0x000A0DE8 File Offset: 0x0009EFE8
	protected override void Awake()
	{
		base.Awake();
		this.messageLabel1 = (SBGUILabel)base.FindChild("message1");
		this.messageLabel1_bottom = (SBGUILabel)base.FindChild("message1_bottom");
		this.messageLabel2 = (SBGUILabel)base.FindChild("message2");
		this.messageLabel2_bottom = (SBGUILabel)base.FindChild("message2_bottom");
		this.info1 = (SBGUILabel)base.FindChild("info1");
		this.info2 = (SBGUILabel)base.FindChild("info2");
		this.btnLabel1 = (SBGUILabel)base.FindChild("btnLabel1");
		this.btnLabel2 = (SBGUILabel)base.FindChild("btnLabel2");
		this.btn1 = (SBGUIButton)base.FindChild("button1");
		this.btn2 = (SBGUIButton)base.FindChild("button2");
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x000A0ED8 File Offset: 0x0009F0D8
	private void Start()
	{
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x000A0EDC File Offset: 0x0009F0DC
	public void SetUp(string message1, string message2, string info1, string info2, string btnLabel1, string btnLabel2, Action action1, Action action2)
	{
		this.messageLabel1.SetText(message1);
		this.messageLabel1_bottom.SetText(message1);
		this.messageLabel2.SetText(message2);
		this.messageLabel2_bottom.SetText(message2);
		this.info1.SetText(info1);
		this.info2.SetText(info2);
		this.btnLabel1.SetText(btnLabel1);
		this.btnLabel2.SetText(btnLabel2);
		base.AttachActionToButton(this.btn1, action1);
		base.AttachActionToButton(this.btn2, action2);
	}

	// Token: 0x04001038 RID: 4152
	private SBGUILabel messageLabel1;

	// Token: 0x04001039 RID: 4153
	private SBGUILabel messageLabel1_bottom;

	// Token: 0x0400103A RID: 4154
	private SBGUILabel messageLabel2;

	// Token: 0x0400103B RID: 4155
	private SBGUILabel messageLabel2_bottom;

	// Token: 0x0400103C RID: 4156
	private SBGUILabel info1;

	// Token: 0x0400103D RID: 4157
	private SBGUILabel info2;

	// Token: 0x0400103E RID: 4158
	private SBGUILabel btnLabel1;

	// Token: 0x0400103F RID: 4159
	private SBGUILabel btnLabel2;

	// Token: 0x04001040 RID: 4160
	private SBGUIButton btn1;

	// Token: 0x04001041 RID: 4161
	private SBGUIButton btn2;

	// Token: 0x04001042 RID: 4162
	private Vector3 rewardCenter;
}
