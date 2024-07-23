using System;
using UnityEngine;

// Token: 0x0200034C RID: 844
public class SaveGameScreen : SBGUIScreen
{
	// Token: 0x06001849 RID: 6217 RVA: 0x000A06D4 File Offset: 0x0009E8D4
	protected override void Awake()
	{
		base.Awake();
		if (this.info1 == null || this.info2 == null || this.messageLabelBoundary1 == null)
		{
		}
		this.messageLabel3 = (SBGUILabel)base.FindChild("message3");
		this.messageLabel3_bottom = (SBGUILabel)base.FindChild("message3_bottom");
		this.messageLabel2 = (SBGUILabel)base.FindChild("message2");
		this.messageLabel2_bottom = (SBGUILabel)base.FindChild("message2_bottom");
		this.messageLabelBoundary1 = (SBGUIAtlasImage)base.FindChild("message_label_boundary");
		this.info1 = (SBGUILabel)base.FindChild("infoText1");
		this.title_server = (SBGUILabel)base.FindChild("title_server");
		this.level_server = (SBGUILabel)base.FindChild("level_server");
		this.money_server = (SBGUILabel)base.FindChild("money_server");
		this.jelly_server = (SBGUILabel)base.FindChild("jelly_server");
		this.timeStamp_server = (SBGUILabel)base.FindChild("timeStamp_server");
		this.info2 = (SBGUILabel)base.FindChild("infoText2");
		this.title_local = (SBGUILabel)base.FindChild("title_local");
		this.level_local = (SBGUILabel)base.FindChild("level_local");
		this.money_local = (SBGUILabel)base.FindChild("money_local");
		this.jelly_local = (SBGUILabel)base.FindChild("jelly_local");
		this.timeStamp_local = (SBGUILabel)base.FindChild("timeStamp_local");
		this.localBtn = (SBGUIButton)base.FindChild("local_button");
		this.serverBtn = (SBGUIButton)base.FindChild("server_button");
		this.btnName_local = (SBGUILabel)base.FindChild("local_label");
		this.btnName_server = (SBGUILabel)base.FindChild("server_label");
		this.highLight = (SBGUIAtlasImage)base.FindChild("highLight");
		this.saveGameArrow = (SBGUIAtlasImage)base.FindChild("saveGameArrow");
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x000A0914 File Offset: 0x0009EB14
	private void Start()
	{
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x000A0918 File Offset: 0x0009EB18
	public void SetUp(string message1, string message3, string message2, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string btnName_server, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string btnName_local, Action server, Action local, Session session)
	{
		this.messageLabel3.SetText(message3);
		this.messageLabel3_bottom.SetText(message3);
		this.messageLabel2.SetText(message2);
		this.messageLabel2_bottom.SetText(message2);
		this.title_server.SetText(title_server);
		this.level_server.SetText(level_server);
		this.money_server.SetText(money_server);
		this.jelly_server.SetText(jelly_server);
		this.btnName_server.SetText(btnName_server);
		this.title_local.SetText(title_local);
		this.level_local.SetText(level_local);
		this.money_local.SetText(money_local);
		this.jelly_local.SetText(jelly_local);
		this.btnName_local.SetText(btnName_local);
		DateTime dateTime = Convert.ToDateTime(timeStamp_server);
		string text = string.Concat(new object[]
		{
			"   ",
			dateTime.Month,
			"/",
			dateTime.Day,
			"/",
			dateTime.Year.ToString().Substring(Math.Max(0, dateTime.Year.ToString().Length - 2)),
			"|at ",
			(dateTime.Hour <= 12) ? dateTime.Hour : (dateTime.Hour - 12),
			":",
			dateTime.Minute,
			" ",
			(dateTime.Hour <= 12) ? "AM" : "PM"
		});
		DateTime dateTime2 = Convert.ToDateTime(timeStamp_local);
		string text2 = string.Concat(new object[]
		{
			"  ",
			dateTime2.Month,
			"/",
			dateTime2.Day,
			"/",
			dateTime2.Year.ToString().Substring(Math.Max(0, dateTime2.Year.ToString().Length - 2)),
			"|at ",
			(dateTime2.Hour <= 12) ? dateTime2.Hour : (dateTime2.Hour - 12),
			":",
			dateTime2.Minute,
			" ",
			(dateTime2.Hour <= 12) ? "AM" : "PM"
		});
		this.timeStamp_server.SetText(text);
		this.timeStamp_local.SetText(text2);
		GameObject gameObject = GameObject.Find("local");
		GameObject gameObject2 = GameObject.Find("server");
		bool flag;
		if (Convert.ToInt32(level_local) == Convert.ToInt32(level_server))
		{
			flag = (DateTime.Compare(Convert.ToDateTime(timeStamp_local), Convert.ToDateTime(timeStamp_server)) > 0);
		}
		else
		{
			flag = (Convert.ToInt32(level_local) > Convert.ToInt32(level_server));
		}
		if (flag)
		{
			this.highLight.transform.parent = gameObject.transform;
			this.highLight.transform.localPosition = Vector3.zero;
			this.highLight.transform.localPosition = new Vector3(0f, 0f, -0.01f);
			this.saveGameArrow.transform.parent = gameObject.transform;
			this.saveGameArrow.transform.localPosition = Vector3.zero;
			this.saveGameArrow.transform.localPosition = this.saveArrowOffset;
		}
		else
		{
			this.highLight.transform.parent = gameObject2.transform;
			this.highLight.transform.localPosition = Vector3.zero;
			this.highLight.transform.localPosition = new Vector3(0f, 0f, -0.01f);
			this.saveGameArrow.transform.parent = gameObject2.transform;
			this.saveGameArrow.transform.localPosition = Vector3.zero;
			this.saveGameArrow.transform.localPosition = this.saveArrowOffset;
		}
		base.AttachActionToButton(this.serverBtn, server);
		base.AttachActionToButton(this.localBtn, local);
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x000A0DB0 File Offset: 0x0009EFB0
	public float GetMainWindowZ()
	{
		return base.gameObject.transform.FindChild("window").localPosition.z;
	}

	// Token: 0x0400101A RID: 4122
	private SBGUILabel messageLabel1;

	// Token: 0x0400101B RID: 4123
	private SBGUILabel messageLabel1_bottom;

	// Token: 0x0400101C RID: 4124
	private SBGUILabel messageLabel3;

	// Token: 0x0400101D RID: 4125
	private SBGUILabel messageLabel3_bottom;

	// Token: 0x0400101E RID: 4126
	private SBGUILabel messageLabel2;

	// Token: 0x0400101F RID: 4127
	private SBGUILabel messageLabel2_bottom;

	// Token: 0x04001020 RID: 4128
	private SBGUIAtlasImage messageLabelBoundary1;

	// Token: 0x04001021 RID: 4129
	private SBGUIButton localBtn;

	// Token: 0x04001022 RID: 4130
	private SBGUIButton serverBtn;

	// Token: 0x04001023 RID: 4131
	private SBGUILabel btnName_local;

	// Token: 0x04001024 RID: 4132
	private SBGUILabel btnName_server;

	// Token: 0x04001025 RID: 4133
	private SBGUILabel info1;

	// Token: 0x04001026 RID: 4134
	private SBGUILabel title_server;

	// Token: 0x04001027 RID: 4135
	private SBGUILabel level_server;

	// Token: 0x04001028 RID: 4136
	private SBGUILabel money_server;

	// Token: 0x04001029 RID: 4137
	private SBGUILabel jelly_server;

	// Token: 0x0400102A RID: 4138
	private SBGUILabel patty_server;

	// Token: 0x0400102B RID: 4139
	private SBGUILabel timeStamp_server;

	// Token: 0x0400102C RID: 4140
	private SBGUILabel info2;

	// Token: 0x0400102D RID: 4141
	private SBGUILabel title_local;

	// Token: 0x0400102E RID: 4142
	private SBGUILabel level_local;

	// Token: 0x0400102F RID: 4143
	private SBGUILabel money_local;

	// Token: 0x04001030 RID: 4144
	private SBGUILabel jelly_local;

	// Token: 0x04001031 RID: 4145
	private SBGUILabel patty_local;

	// Token: 0x04001032 RID: 4146
	private SBGUILabel timeStamp_local;

	// Token: 0x04001033 RID: 4147
	private SBGUIAtlasImage pattySprite;

	// Token: 0x04001034 RID: 4148
	private SBGUIAtlasImage highLight;

	// Token: 0x04001035 RID: 4149
	private SBGUIAtlasImage saveGameArrow;

	// Token: 0x04001036 RID: 4150
	private Vector3 rewardCenter;

	// Token: 0x04001037 RID: 4151
	private Vector3 saveArrowOffset = new Vector3(1.55f, 0.8f, -0.3f);
}
