using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class SBGUIDailyBonusDialog : SBGUIScreen
{
	// Token: 0x06000478 RID: 1144 RVA: 0x0001C17C File Offset: 0x0001A37C
	public void Setup(DailyBonusDialogInputData pInputData, Session pSession)
	{
		this.window = (SBGUIAtlasImage)base.FindChild("window");
		this.titleBackground = (SBGUIAtlasImage)base.FindChild("titleBackground");
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("window_title");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("tab_icon");
		SBGUILabel sbguilabel2 = (SBGUILabel)base.FindChild("info");
		this.okayButton = (SBGUIPulseButton)base.FindChild("okay");
		SBGUILabel sbguilabel3 = (SBGUILabel)base.FindChild("okay_label");
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)base.FindChild("reward1_icon");
		SBGUIAtlasImage sbguiatlasImage3 = (SBGUIAtlasImage)base.FindChild("reward2_icon");
		SBGUIAtlasImage sbguiatlasImage4 = (SBGUIAtlasImage)base.FindChild("reward3_icon");
		SBGUIAtlasImage sbguiatlasImage5 = (SBGUIAtlasImage)base.FindChild("reward4_icon");
		SBGUIAtlasImage sbguiatlasImage6 = (SBGUIAtlasImage)base.FindChild("reward5_icon");
		this.pReward6Image = (SBGUIAtlasImage)base.FindChild("reward6_icon");
		SBGUILabel sbguilabel4 = (SBGUILabel)base.FindChild("reward1_label");
		SBGUILabel sbguilabel5 = (SBGUILabel)base.FindChild("reward2_label");
		SBGUILabel sbguilabel6 = (SBGUILabel)base.FindChild("reward3_label");
		SBGUILabel sbguilabel7 = (SBGUILabel)base.FindChild("reward4_label");
		SBGUILabel sbguilabel8 = (SBGUILabel)base.FindChild("reward5_label");
		this.pReward6Label = (SBGUILabel)base.FindChild("reward6_label");
		this.pRewardTodayLabel = (SBGUILabel)base.FindChild("rewardToday_label");
		SBGUILabel sbguilabel9 = (SBGUILabel)base.FindChild("reward1Day_label");
		this.pReward2DayLabel = (SBGUILabel)base.FindChild("reward2Day_label");
		SBGUILabel sbguilabel10 = (SBGUILabel)base.FindChild("reward3Day_label");
		SBGUILabel sbguilabel11 = (SBGUILabel)base.FindChild("reward4Day_label");
		SBGUILabel sbguilabel12 = (SBGUILabel)base.FindChild("reward5Day_label");
		this.pReward6DayLabel = (SBGUILabel)base.FindChild("reward6Day_label");
		this.pDailyBonusData = pInputData.DailyBonusData;
		this.currentDay = pInputData.CurrentDay;
		this.alreadyCollected = pInputData.AlreadyCollected;
		if (this.pReward6DayLabel == null || this.pReward6Image == null || this.pReward6Label == null || sbguilabel12 == null || sbguiatlasImage6 == null || sbguilabel8 == null || this.pDailyBonusData.count() < 5)
		{
			Debug.LogError("Missing items for daily bonus");
			pSession.ChangeState("Playing", true);
			return;
		}
		sbguiatlasImage3.tform.localScale = new Vector3(1f, 1f, 1f);
		sbguiatlasImage4.tform.localScale = new Vector3(1f, 1f, 1f);
		sbguiatlasImage5.tform.localScale = new Vector3(1f, 1f, 1f);
		sbguiatlasImage6.tform.localScale = new Vector3(1f, 1f, 1f);
		this.pReward6Image.tform.localScale = new Vector3(1f, 1f, 1f);
		sbguiatlasImage2.tform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		sbguilabel9.SetActive(false);
		this.pReward6DayLabel.SetActive(false);
		this.pReward6Image.SetActive(false);
		this.pReward6Label.SetActive(false);
		this.pRewardTodayLabel.SetActive(false);
		this.okayButton.SetActive(false);
		if (this.pDailyBonusData.count() == 5)
		{
			sbguilabel9.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[0].Day.ToString());
			this.pReward2DayLabel.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[1].Day.ToString());
			sbguilabel10.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[2].Day.ToString());
			sbguilabel11.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[3].Day.ToString());
			sbguilabel12.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[4].Day.ToString());
			sbguilabel4.SetText(this.pDailyBonusData[0].CurrencyAmount.ToString());
			sbguilabel5.SetText(this.pDailyBonusData[1].CurrencyAmount.ToString());
			sbguilabel6.SetText(this.pDailyBonusData[2].CurrencyAmount.ToString());
			sbguilabel7.SetText(this.pDailyBonusData[3].CurrencyAmount.ToString());
			sbguilabel8.SetText(this.pDailyBonusData[4].CurrencyAmount.ToString());
			sbguiatlasImage2.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[0].CurrencyDID].GetResourceTexture());
			sbguiatlasImage3.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[1].CurrencyDID].GetResourceTexture());
			sbguiatlasImage4.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[2].CurrencyDID].GetResourceTexture());
			sbguiatlasImage5.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[3].CurrencyDID].GetResourceTexture());
			sbguiatlasImage6.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[4].CurrencyDID].GetResourceTexture());
			float duration = 0.5f;
			base.StartCoroutine(this.EnlargeTodayEffects(duration));
		}
		else if (this.currentDay == 1)
		{
			sbguilabel9.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[1].Day.ToString());
			this.pReward2DayLabel.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[2].Day.ToString());
			sbguilabel10.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[3].Day.ToString());
			sbguilabel11.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[4].Day.ToString());
			sbguilabel12.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[5].Day.ToString());
			sbguilabel4.SetText(this.pDailyBonusData[1].CurrencyAmount.ToString());
			sbguilabel5.SetText(this.pDailyBonusData[2].CurrencyAmount.ToString());
			sbguilabel6.SetText(this.pDailyBonusData[3].CurrencyAmount.ToString());
			sbguilabel7.SetText(this.pDailyBonusData[4].CurrencyAmount.ToString());
			sbguilabel8.SetText(this.pDailyBonusData[5].CurrencyAmount.ToString());
			sbguiatlasImage2.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[1].CurrencyDID].GetResourceTexture());
			sbguiatlasImage3.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[2].CurrencyDID].GetResourceTexture());
			sbguiatlasImage4.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[3].CurrencyDID].GetResourceTexture());
			sbguiatlasImage5.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[4].CurrencyDID].GetResourceTexture());
			sbguiatlasImage6.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[5].CurrencyDID].GetResourceTexture());
			float duration2 = 0.5f;
			base.StartCoroutine(this.EnlargeTodayEffects(duration2));
		}
		else
		{
			if (this.pDailyBonusData.count() < 6)
			{
				Debug.LogError("Missing items for daily bonus");
				pSession.ChangeState("Playing", true);
				return;
			}
			sbguilabel9.SetActive(true);
			sbguilabel9.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[0].Day.ToString());
			this.pReward2DayLabel.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[1].Day.ToString());
			sbguilabel10.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[2].Day.ToString());
			sbguilabel11.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[3].Day.ToString());
			sbguilabel12.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[4].Day.ToString());
			sbguilabel4.SetText(this.pDailyBonusData[0].CurrencyAmount.ToString());
			sbguilabel5.SetText(this.pDailyBonusData[1].CurrencyAmount.ToString());
			sbguilabel6.SetText(this.pDailyBonusData[2].CurrencyAmount.ToString());
			sbguilabel7.SetText(this.pDailyBonusData[3].CurrencyAmount.ToString());
			sbguilabel8.SetText(this.pDailyBonusData[4].CurrencyAmount.ToString());
			this.pReward6Label.SetText(this.pDailyBonusData[5].CurrencyAmount.ToString());
			sbguiatlasImage2.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[0].CurrencyDID].GetResourceTexture());
			sbguiatlasImage3.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[1].CurrencyDID].GetResourceTexture());
			sbguiatlasImage4.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[2].CurrencyDID].GetResourceTexture());
			sbguiatlasImage5.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[3].CurrencyDID].GetResourceTexture());
			sbguiatlasImage6.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[4].CurrencyDID].GetResourceTexture());
			this.pReward6Image.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[this.pDailyBonusData[5].CurrencyDID].GetResourceTexture());
			this.elementsList.Clear();
			this.elementsToPosition.Clear();
			this.elementsList.Add(this.pReward2DayLabel);
			this.elementsList.Add(sbguilabel10);
			this.elementsList.Add(sbguilabel11);
			this.elementsList.Add(sbguilabel12);
			this.elementsList.Add(sbguiatlasImage3);
			this.elementsList.Add(sbguiatlasImage4);
			this.elementsList.Add(sbguiatlasImage5);
			this.elementsList.Add(sbguiatlasImage6);
			this.elementsList.Add(sbguilabel5);
			this.elementsList.Add(sbguilabel6);
			this.elementsList.Add(sbguilabel7);
			this.elementsList.Add(sbguilabel8);
			foreach (SBGUIElement sbguielement in this.elementsList)
			{
				Vector3 localPosition = sbguielement.tform.localPosition;
				Vector3 target = new Vector3(localPosition.x - 1.8f, localPosition.y, localPosition.z);
				this.elementsToPosition.Add(sbguielement.name, new SBGUIDailyBonusDialog.Positioning(sbguielement, localPosition, target));
			}
			this.elementsToShrink.Add(sbguiatlasImage2);
			this.elementsToShrink.Add(sbguilabel4);
			this.elementsToShrink.Add(sbguilabel9);
			float duration3 = 0.6f;
			base.StartCoroutine(this.shrinkFirstItem(duration3));
		}
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0001CFC0 File Offset: 0x0001B1C0
	private IEnumerator shrinkFirstItem(float duration)
	{
		yield return new WaitForSeconds(0.3f);
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			this.elementsToShrink[0].tform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, normalizedTime);
			this.elementsToShrink[1].tform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, normalizedTime);
			this.elementsToShrink[2].tform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, normalizedTime);
			yield return null;
		}
		float shiftDuration = 0.7f;
		base.StartCoroutine(this.ShiftLeftCoroutine(shiftDuration));
		float fadeDuration = 0.55f;
		base.StartCoroutine(this.FadeOutSecondDayLabel(fadeDuration));
		yield break;
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0001CFEC File Offset: 0x0001B1EC
	private IEnumerator FadeOutSecondDayLabel(float duration)
	{
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			this.pReward2DayLabel.SetAlpha(1f - normalizedTime);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0001D018 File Offset: 0x0001B218
	private IEnumerator ShiftLeftCoroutine(float duration)
	{
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			this.shiftLeftTransform(normalizedTime);
			yield return null;
		}
		float enlargeLastItemDuration = 0.5f;
		float enlargeTodayEffectsDuration = 0.2f;
		base.StartCoroutine(this.EnlargeLastItem(enlargeLastItemDuration));
		base.StartCoroutine(this.EnlargeTodayEffects(enlargeTodayEffectsDuration));
		yield break;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0001D044 File Offset: 0x0001B244
	private void shiftLeftTransform(float normalizedTime)
	{
		foreach (string text in this.elementsToPosition.Keys)
		{
			this.elementsToPosition[text].element.tform.localPosition = Vector3.Lerp(this.elementsToPosition[text].origin, this.elementsToPosition[text].target, normalizedTime);
			this.elementsToPosition[text].element.tform.localRotation = Quaternion.identity;
			if (text == "reward2_icon")
			{
				this.elementsToPosition[text].element.tform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.3f, 1.3f, 1.3f), normalizedTime);
			}
		}
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0001D158 File Offset: 0x0001B358
	private IEnumerator EnlargeLastItem(float duration)
	{
		float normalizedTime = 0f;
		this.pReward6Image.SetActive(true);
		this.pReward6Label.SetActive(true);
		this.pReward6DayLabel.SetActive(true);
		this.pReward6DayLabel.SetText(Language.Get("!!DIALOG_DAY_ID2104") + " " + this.pDailyBonusData[5].Day.ToString());
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			this.pReward6Image.tform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			this.pReward6Label.tform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			this.pReward6DayLabel.tform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0001D184 File Offset: 0x0001B384
	private IEnumerator EnlargeTodayEffects(float duration)
	{
		this.pRewardTodayLabel.SetActive(true);
		float normalizedTime = 0f;
		while (normalizedTime <= 1f)
		{
			normalizedTime += Time.deltaTime / duration;
			this.pRewardTodayLabel.tform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, normalizedTime);
			yield return null;
		}
		this.okayButton.SetActive(true);
		yield break;
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0001D1B0 File Offset: 0x0001B3B0
	public void applyReward(Session session)
	{
		int index = 1;
		Reward reward = new Reward(new Dictionary<int, int>
		{
			{
				this.pDailyBonusData[index].CurrencyDID,
				this.pDailyBonusData[index].CurrencyAmount
			}
		}, null, null, null, null, null, null, null, false, null);
		session.TheGame.ApplyReward(reward, TFUtils.EpochTime(), false);
		session.TheGame.ModifyGameState(new ReceiveRewardAction(reward, string.Empty));
		session.TheGame.analytics.LogDailyReward(this.currentDay);
		AnalyticsWrapper.LogDailyReward(session.TheGame, this.currentDay, reward);
		Ray ray = session.TheCamera.ScreenPointToRay(Input.mousePosition);
		if (this.pDailyBonusData[index].CurrencyDID == 3)
		{
			session.TheSoundEffectManager.PlaySound("coin_big");
			for (int i = 0; i < 5; i++)
			{
				session.TheGame.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Coin_Shower", 0, 0, 0f, new SBGUIDailyBonusDialog.RewardCoinShowerRequestDelegate(ray.origin));
			}
		}
		else
		{
			session.TheSoundEffectManager.PlaySound("ItemCollected");
			session.TheGame.simulation.particleSystemManager.RequestParticles("Prefabs/FX/Fx_Jelly_Shower", 0, 0, 0f, new SBGUIDailyBonusDialog.RewardCoinShowerRequestDelegate(ray.origin));
		}
	}

	// Token: 0x04000372 RID: 882
	private SBGUIPulseButton okayButton;

	// Token: 0x04000373 RID: 883
	private SBGUIAtlasImage window;

	// Token: 0x04000374 RID: 884
	private SBGUIAtlasImage titleBackground;

	// Token: 0x04000375 RID: 885
	private SBGUILabel pRewardTodayLabel;

	// Token: 0x04000376 RID: 886
	private SBGUILabel pReward2DayLabel;

	// Token: 0x04000377 RID: 887
	private SBGUILabel pReward6DayLabel;

	// Token: 0x04000378 RID: 888
	private SBGUILabel pReward6Label;

	// Token: 0x04000379 RID: 889
	private SBGUIAtlasImage pReward6Image;

	// Token: 0x0400037A RID: 890
	private SoaringArray<SBMISoaring.SBMIDailyBonusDay> pDailyBonusData;

	// Token: 0x0400037B RID: 891
	private int currentDay;

	// Token: 0x0400037C RID: 892
	private bool alreadyCollected;

	// Token: 0x0400037D RID: 893
	private List<SBGUIElement> elementsList = new List<SBGUIElement>();

	// Token: 0x0400037E RID: 894
	private Dictionary<string, SBGUIDailyBonusDialog.Positioning> elementsToPosition = new Dictionary<string, SBGUIDailyBonusDialog.Positioning>();

	// Token: 0x0400037F RID: 895
	private List<SBGUIElement> elementsToShrink = new List<SBGUIElement>();

	// Token: 0x02000076 RID: 118
	public class Positioning
	{
		// Token: 0x06000480 RID: 1152 RVA: 0x0001D314 File Offset: 0x0001B514
		public Positioning(SBGUIElement element, Vector3 origin, Vector3 target)
		{
			this.element = element;
			this.origin = origin;
			this.target = target;
		}

		// Token: 0x04000380 RID: 896
		public SBGUIElement element;

		// Token: 0x04000381 RID: 897
		public Vector3 origin;

		// Token: 0x04000382 RID: 898
		public Vector3 target;
	}

	// Token: 0x02000077 RID: 119
	private class RewardCoinShowerRequestDelegate : ParticleSystemManager.Request.IDelegate
	{
		// Token: 0x06000481 RID: 1153 RVA: 0x0001D334 File Offset: 0x0001B534
		public RewardCoinShowerRequestDelegate(Vector3 particleLocation)
		{
			this.particleLocation = particleLocation;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0001D344 File Offset: 0x0001B544
		public Transform ParentTransform
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0001D348 File Offset: 0x0001B548
		public Vector3 Position
		{
			get
			{
				return this.particleLocation;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0001D350 File Offset: 0x0001B550
		public bool isVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000383 RID: 899
		protected Vector3 particleLocation;
	}
}
