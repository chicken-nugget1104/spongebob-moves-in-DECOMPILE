using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034E RID: 846
public class SaveGameScrollScreen : SBGUIScrollableDialog
{
	// Token: 0x06001853 RID: 6227 RVA: 0x000A0F88 File Offset: 0x0009F188
	public void Setup(string message1, string message2)
	{
		if (this.info1 == null || this.info2 == null || this.info3 == null)
		{
		}
		this.messageLabel1 = (SBGUILabel)base.FindChild("message1");
		this.messageLabel1_bottom = (SBGUILabel)base.FindChild("message1_bottom");
		this.messageLabel2 = (SBGUILabel)base.FindChild("message2");
		this.messageLabel2_bottom = (SBGUILabel)base.FindChild("message2_bottom");
		this.messageLabel1.SetText(message1);
		this.messageLabel1_bottom.SetText(message1);
		this.messageLabel2.SetText(message2);
		this.messageLabel2_bottom.SetText(message2);
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x000A1054 File Offset: 0x0009F254
	public void CreateUI1(string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, Action local, Action server)
	{
	}

	// Token: 0x06001855 RID: 6229 RVA: 0x000A1058 File Offset: 0x0009F258
	public void CreateUI(string info1, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string info2, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string info3, string title_offline, string level_offline, string money_offline, string jelly_offline, string patty_offline, string timeStamp_offline, Action local, Action server, Action offline)
	{
		SBGUICreditsSlot component = this.slotPrefab.GetComponent<SBGUICreditsSlot>();
		SBGUIImage sbguiimage = (SBGUIImage)component.FindChild("slot_boundary");
		Vector2 vector = sbguiimage.Size * 0.01f;
		Rect scrollSize = new Rect(0f, 0f, vector.x, vector.y);
		this.region.ResetScroll(scrollSize);
		this.region.ResetToMinScroll();
		this.CreateCreditsSlot(this.session, this.region.Marker, Vector3.zero, info1, title_server, level_server, money_server, jelly_server, patty_server, timeStamp_server, info2, title_local, level_local, money_local, jelly_local, patty_local, timeStamp_local, info3, title_offline, level_offline, money_offline, jelly_offline, patty_offline, timeStamp_offline, local, server, offline);
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x000A1114 File Offset: 0x0009F314
	private IEnumerator ScrollingCredits()
	{
		yield return null;
		bool keepScrolling = true;
		while (keepScrolling)
		{
			if (this.region.WasRecentlyTouched)
			{
				keepScrolling = false;
			}
			this.region.momentum.TrackForSmoothing(this.region.subViewMarker.tform.position + new Vector3(0f, 0.005f, 0f));
			this.region.momentum.CalculateSmoothVelocity();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x000A1130 File Offset: 0x0009F330
	public override void Deactivate()
	{
		base.StopCoroutine("ScrollingCredits");
		SaveGameScrollScreen.slotPool.Clear(delegate(SBGUICreditsSlot slot)
		{
			slot.Deactivate();
		});
		base.Deactivate();
	}

	// Token: 0x06001858 RID: 6232 RVA: 0x000A1178 File Offset: 0x0009F378
	private SBGUICreditsSlot CreateCreditsSlot(Session session, SBGUIElement anchor, Vector3 offset, string info1, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string info2, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string info3, string title_offline, string level_offline, string money_offline, string jelly_offline, string patty_offline, string timeStamp_offline, Action local, Action server, Action offline)
	{
		SBGUICreditsSlot sbguicreditsSlot;
		if (title_offline != null)
		{
			sbguicreditsSlot = SaveGameScrollScreen.slotPool.Create(new Alloc<SBGUICreditsSlot>(SBGUICreditsSlot.MakeCreditsSlot2));
		}
		else
		{
			sbguicreditsSlot = SaveGameScrollScreen.slotPool.Create(new Alloc<SBGUICreditsSlot>(SBGUICreditsSlot.MakeCreditsSlot1));
		}
		this.setUpChild(sbguicreditsSlot, title_server, level_server, money_server, jelly_server, patty_server, timeStamp_server, info1, title_local, level_local, money_local, jelly_local, patty_local, timeStamp_local, info2, title_offline, level_offline, money_offline, jelly_offline, patty_offline, timeStamp_offline, info3, local, server, offline);
		sbguicreditsSlot.SetActive(true);
		sbguicreditsSlot.Setup(session, this, anchor, offset);
		return sbguicreditsSlot;
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x000A1208 File Offset: 0x0009F408
	public void setUpChild(SBGUICreditsSlot slot, string info1, string title_server, string level_server, string money_server, string jelly_server, string patty_server, string timeStamp_server, string info2, string title_local, string level_local, string money_local, string jelly_local, string patty_local, string timeStamp_local, string info3, string title_offline, string level_offline, string money_offline, string jelly_offline, string patty_offline, string timeStamp_offline, Action local, Action server, Action offline)
	{
		this.info1 = (SBGUILabel)slot.FindChild("infoText1");
		this.title_server = (SBGUILabel)slot.FindChild("title_server");
		this.level_server = (SBGUILabel)slot.FindChild("level_server");
		this.money_server = (SBGUILabel)slot.FindChild("money_server");
		this.jelly_server = (SBGUILabel)slot.FindChild("jelly_server");
		this.patty_server = (SBGUILabel)slot.FindChild("patty_server");
		this.timeStamp_server = (SBGUILabel)slot.FindChild("timeStamp_server");
		this.info2 = (SBGUILabel)slot.FindChild("infoText2");
		this.title_local = (SBGUILabel)slot.FindChild("title_local");
		this.level_local = (SBGUILabel)slot.FindChild("level_local");
		this.money_local = (SBGUILabel)slot.FindChild("money_local");
		this.jelly_local = (SBGUILabel)slot.FindChild("jelly_local");
		this.patty_local = (SBGUILabel)slot.FindChild("patty_local");
		this.timeStamp_local = (SBGUILabel)slot.FindChild("timeStamp_local");
		this.title_server.SetText(title_server);
		this.level_server.SetText(level_server);
		this.money_server.SetText(money_server);
		this.jelly_server.SetText(jelly_server);
		this.patty_server.SetText(patty_server);
		this.timeStamp_server.SetText(timeStamp_server);
		this.title_local.SetText(title_local);
		this.level_local.SetText(level_local);
		this.money_local.SetText(money_local);
		this.jelly_local.SetText(jelly_local);
		this.patty_local.SetText(patty_local);
		this.timeStamp_local.SetText(timeStamp_local);
		this.localBtn = (SBGUIButton)slot.FindChild("local_button");
		this.serverBtn = (SBGUIButton)slot.FindChild("server_button");
		base.AttachActionToButton(this.localBtn, local);
		base.AttachActionToButton(this.serverBtn, server);
		if (title_offline != null)
		{
			this.info3 = (SBGUILabel)slot.FindChild("infoText3");
			this.title_offline = (SBGUILabel)slot.FindChild("title_offline");
			this.level_offline = (SBGUILabel)slot.FindChild("level_offline");
			this.money_offline = (SBGUILabel)slot.FindChild("money_offline");
			this.jelly_offline = (SBGUILabel)slot.FindChild("jelly_offline");
			this.patty_offline = (SBGUILabel)slot.FindChild("patty_offline");
			this.timeStamp_offline = (SBGUILabel)slot.FindChild("timeStamp_offline");
			this.title_offline.SetText(title_offline);
			this.level_offline.SetText(level_offline);
			this.money_offline.SetText(money_offline);
			this.jelly_offline.SetText(jelly_offline);
			this.patty_offline.SetText(patty_offline);
			this.timeStamp_offline.SetText(timeStamp_offline);
			this.offlineBtn = (SBGUIButton)slot.FindChild("offline_button");
			base.AttachActionToButton(this.offlineBtn, offline);
			base.StartCoroutine(this.serSubView2());
		}
		else
		{
			base.StartCoroutine(this.serSubView1());
		}
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x000A1574 File Offset: 0x0009F774
	private IEnumerator serSubView1()
	{
		yield return new WaitForSeconds(0.2f);
		GameObject go = GameObject.Find("CreditsSlot1");
		GUISubView subView = go.transform.parent.parent.GetComponent<GUISubView>();
		subView.transform.localPosition = new Vector3(0f, -1.1f, 21f);
		Vector3 pos = go.transform.parent.transform.localPosition;
		go.transform.parent.transform.localPosition = new Vector3(0f, 1.3f, 0f) + pos;
		yield break;
	}

	// Token: 0x0600185B RID: 6235 RVA: 0x000A1588 File Offset: 0x0009F788
	private IEnumerator serSubView2()
	{
		yield return new WaitForSeconds(0.2f);
		GameObject go = GameObject.Find("CreditsSlot2");
		GUISubView subView = go.transform.parent.parent.GetComponent<GUISubView>();
		subView.transform.localPosition = new Vector3(0f, -1.1f, 21f);
		Vector3 pos = go.transform.parent.transform.localPosition;
		go.transform.parent.transform.localPosition = new Vector3(0f, 1.4f, 0f) + pos;
		yield break;
	}

	// Token: 0x04001043 RID: 4163
	public GameObject slotPrefab;

	// Token: 0x04001044 RID: 4164
	private SBGUILabel messageLabel1;

	// Token: 0x04001045 RID: 4165
	private SBGUILabel messageLabel1_bottom;

	// Token: 0x04001046 RID: 4166
	private SBGUILabel messageLabel2;

	// Token: 0x04001047 RID: 4167
	private SBGUILabel messageLabel2_bottom;

	// Token: 0x04001048 RID: 4168
	private SBGUIButton localBtn;

	// Token: 0x04001049 RID: 4169
	private SBGUIButton serverBtn;

	// Token: 0x0400104A RID: 4170
	private SBGUIButton offlineBtn;

	// Token: 0x0400104B RID: 4171
	private SBGUILabel info1;

	// Token: 0x0400104C RID: 4172
	private SBGUILabel title_server;

	// Token: 0x0400104D RID: 4173
	private SBGUILabel level_server;

	// Token: 0x0400104E RID: 4174
	private SBGUILabel money_server;

	// Token: 0x0400104F RID: 4175
	private SBGUILabel jelly_server;

	// Token: 0x04001050 RID: 4176
	private SBGUILabel patty_server;

	// Token: 0x04001051 RID: 4177
	private SBGUILabel timeStamp_server;

	// Token: 0x04001052 RID: 4178
	private SBGUILabel info2;

	// Token: 0x04001053 RID: 4179
	private SBGUILabel title_local;

	// Token: 0x04001054 RID: 4180
	private SBGUILabel level_local;

	// Token: 0x04001055 RID: 4181
	private SBGUILabel money_local;

	// Token: 0x04001056 RID: 4182
	private SBGUILabel jelly_local;

	// Token: 0x04001057 RID: 4183
	private SBGUILabel patty_local;

	// Token: 0x04001058 RID: 4184
	private SBGUILabel timeStamp_local;

	// Token: 0x04001059 RID: 4185
	private SBGUILabel info3;

	// Token: 0x0400105A RID: 4186
	private SBGUILabel title_offline;

	// Token: 0x0400105B RID: 4187
	private SBGUILabel level_offline;

	// Token: 0x0400105C RID: 4188
	private SBGUILabel money_offline;

	// Token: 0x0400105D RID: 4189
	private SBGUILabel jelly_offline;

	// Token: 0x0400105E RID: 4190
	private SBGUILabel patty_offline;

	// Token: 0x0400105F RID: 4191
	private SBGUILabel timeStamp_offline;

	// Token: 0x04001060 RID: 4192
	protected static TFPool<SBGUICreditsSlot> slotPool = new TFPool<SBGUICreditsSlot>();
}
