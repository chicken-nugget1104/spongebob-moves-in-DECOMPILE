using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class SBGUICharacterWishWidget : SBGUIElement
{
	// Token: 0x06000407 RID: 1031 RVA: 0x0001725C File Offset: 0x0001545C
	public void SetData(Session pSession, Simulated pSimulated, Action pFeedWishAction, Action pRushWishAction)
	{
		this.m_pSession = pSession;
		this.m_pSimulated = pSimulated;
		this.m_pFeedWishAction = pFeedWishAction;
		this.m_pRushWishAction = pRushWishAction;
		this.m_eWishState = SBGUICharacterWishWidget._eWishState.eNone;
		this.m_nHungerResourceID = null;
		this.m_pEntity = this.m_pSimulated.GetEntity<ResidentEntity>();
		if (this.m_pEntity.HungerResourceId != null)
		{
			this.m_nHungerResourceID = new int?(this.m_pEntity.HungerResourceId.Value);
			if (this.m_eWishState != SBGUICharacterWishWidget._eWishState.eHungry)
			{
				this.m_pHungryParent.SetActive(true);
				this.m_pFullParent.SetActive(false);
				this.SetVisualsForHungerResourceID(this.m_nHungerResourceID.Value);
				this.m_pGrantWishButton.ClearClickEvents();
				base.AttachActionToButton(this.m_pGrantWishButton, this.m_pFeedWishAction);
				this.m_eWishState = SBGUICharacterWishWidget._eWishState.eHungry;
			}
		}
		else
		{
			double num = this.m_pEntity.HungryAt - TFUtils.EpochTime();
			if (num < 0.0)
			{
				num = 0.0;
			}
			this.m_pWishProgressLabel.SetText(TFUtils.DurationToString((ulong)num));
			this.m_pWishProgressMeter.Progress = this.m_pEntity.FullnessPercentage();
			Cost cost = this.m_pEntity.FullnessRushCostNow();
			this.m_pWishRushCostLabel.SetText(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString());
			if (this.m_eWishState != SBGUICharacterWishWidget._eWishState.eFull)
			{
				this.m_pHungryParent.SetActive(false);
				this.m_pFullParent.SetActive(true);
				this.m_pRushWishButton.ClearClickEvents();
				base.AttachActionToButton(this.m_pRushWishButton, this.m_pRushWishAction);
				this.m_eWishState = SBGUICharacterWishWidget._eWishState.eFull;
			}
		}
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x00017414 File Offset: 0x00015614
	public void UpdateData()
	{
		if (this.m_eWishState == SBGUICharacterWishWidget._eWishState.eFull)
		{
			if (this.m_pEntity.HungerResourceId != null)
			{
				this.m_nHungerResourceID = new int?(this.m_pEntity.HungerResourceId.Value);
				this.m_pHungryParent.SetActive(true);
				this.m_pFullParent.SetActive(false);
				this.SetVisualsForHungerResourceID(this.m_nHungerResourceID.Value);
				this.m_pGrantWishButton.ClearClickEvents();
				base.AttachActionToButton(this.m_pGrantWishButton, this.m_pFeedWishAction);
				this.m_eWishState = SBGUICharacterWishWidget._eWishState.eHungry;
				return;
			}
			double num = this.m_pEntity.HungryAt - TFUtils.EpochTime();
			if (num < 0.0)
			{
				num = 0.0;
			}
			this.m_pWishProgressLabel.SetText(TFUtils.DurationToString((ulong)num));
			this.m_pWishProgressMeter.Progress = this.m_pEntity.FullnessPercentage();
			Cost cost = this.m_pEntity.FullnessRushCostNow();
			this.m_pWishRushCostLabel.SetText(cost.ResourceAmounts[cost.GetOnlyCostKey()].ToString());
		}
		else if (this.m_pEntity.HungerResourceId != null && this.m_pEntity.HungerResourceId.Value != this.m_nHungerResourceID.Value)
		{
			this.m_nHungerResourceID = new int?(this.m_pEntity.HungerResourceId.Value);
			this.SetVisualsForHungerResourceID(this.m_nHungerResourceID.Value);
			this.m_eWishState = SBGUICharacterWishWidget._eWishState.eHungry;
		}
		if (this.m_nHungerResourceID != null && this.m_pSession.TheGame.resourceManager.Resources[this.m_nHungerResourceID.Value].Amount <= 0)
		{
			if (this.m_pGrantWishButton.enabled)
			{
				this.m_pGrantWishButton.enabled = false;
				Color color = new Color(0.4509804f, 0.4509804f, 0.4509804f, 0.4509804f);
				this.m_pGrantWishButton.SetColor(color);
				this.m_pGrantWishButtonLabel.SetColor(color);
			}
		}
		else if (!this.m_pGrantWishButton.enabled)
		{
			this.m_pGrantWishButton.enabled = true;
			this.m_pGrantWishButton.SetColor(Color.white);
			this.m_pGrantWishButtonLabel.SetColor(new Color(0.043137256f, 0.2509804f, 0f, 255f));
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00017698 File Offset: 0x00015898
	public Vector2 GetRushWishButtonPosition()
	{
		return this.m_pRushWishButton.GetScreenPosition();
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x000176A8 File Offset: 0x000158A8
	private void SetVisualsForHungerResourceID(int nHungerResourceID)
	{
		Resource resource = this.m_pSession.TheGame.resourceManager.Resources[this.m_nHungerResourceID.Value];
		this.m_pWishIcon.SetSizeNoRebuild(this.m_pWishIconSize);
		this.m_pWishIcon.SetTextureFromAtlas(resource.GetResourceTexture(), true, false, true, false, false, 0);
		this.m_pWishNameLabel.SetText(Language.Get(resource.Name));
		this.m_pWishFullTimeLabel.SetText(TFUtils.DurationToString((ulong)((long)resource.FullnessTime)));
		this.m_pWishXPRewardLabel.SetText(resource.Reward.LowestResourceValue(ResourceManager.XP).ToString());
		this.m_pWishCountLabel.SetText(resource.Amount.ToString());
		int num = resource.Reward.LowestResourceValue(ResourceManager.HARD_CURRENCY);
		if (num > 0)
		{
			this.m_pWishSoftRewardLabel.SetText(num.ToString());
			this.m_pCurrencyImage.SetTextureFromAtlas(this.m_pSession.TheGame.resourceManager.Resources[ResourceManager.HARD_CURRENCY].GetResourceTexture());
		}
		else
		{
			num = resource.Reward.LowestResourceValue(ResourceManager.SOFT_CURRENCY);
			this.m_pWishSoftRewardLabel.SetText(num.ToString());
			this.m_pCurrencyImage.SetTextureFromAtlas(this.m_pSession.TheGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
		}
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00017824 File Offset: 0x00015A24
	protected override void Awake()
	{
		this.m_pWishIcon = (SBGUIAtlasImage)base.FindChild("wish_icon");
		this.m_pCurrencyImage = (SBGUIAtlasImage)base.FindChild("soft_currency_icon");
		this.m_pWishNameLabel = (SBGUILabel)base.FindChild("wish_name_label");
		this.m_pWishProgressLabel = (SBGUILabel)base.FindChild("wish_progress_label");
		this.m_pWishFullTimeLabel = (SBGUILabel)base.FindChild("wish_full_time_label");
		this.m_pWishSoftRewardLabel = (SBGUILabel)base.FindChild("wish_soft_reward_label");
		this.m_pWishXPRewardLabel = (SBGUILabel)base.FindChild("wish_xp_reward_label");
		this.m_pWishRushCostLabel = (SBGUILabel)base.FindChild("wish_rush_cost_label");
		this.m_pGrantWishButtonLabel = (SBGUILabel)base.FindChild("grant_wish_label");
		this.m_pWishCountLabel = (SBGUILabel)base.FindChild("wish_count_label");
		this.m_pWishProgressMeter = (SBGUIProgressMeter)base.FindChild("wish_progress_meter");
		this.m_pGrantWishButton = (SBGUIPulseButton)base.FindChild("grant_wish_button");
		this.m_pRushWishButton = (SBGUIButton)base.FindChild("rush_wish_button");
		this.m_pFullParent = this.m_pRushWishButton.transform.parent.parent.gameObject;
		this.m_pHungryParent = this.m_pWishFullTimeLabel.transform.parent.gameObject;
		this.m_eWishState = SBGUICharacterWishWidget._eWishState.eNone;
		this.m_pWishIconSize = this.m_pWishIcon.Size;
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("reward_title_label");
		sbguilabel.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		base.Awake();
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x000179D4 File Offset: 0x00015BD4
	private void Update()
	{
		this.UpdateData();
	}

	// Token: 0x040002CE RID: 718
	private SBGUIAtlasImage m_pWishIcon;

	// Token: 0x040002CF RID: 719
	private SBGUIAtlasImage m_pCurrencyImage;

	// Token: 0x040002D0 RID: 720
	private SBGUILabel m_pWishNameLabel;

	// Token: 0x040002D1 RID: 721
	private SBGUILabel m_pWishProgressLabel;

	// Token: 0x040002D2 RID: 722
	private SBGUILabel m_pWishFullTimeLabel;

	// Token: 0x040002D3 RID: 723
	private SBGUILabel m_pWishSoftRewardLabel;

	// Token: 0x040002D4 RID: 724
	private SBGUILabel m_pWishXPRewardLabel;

	// Token: 0x040002D5 RID: 725
	private SBGUILabel m_pWishRushCostLabel;

	// Token: 0x040002D6 RID: 726
	private SBGUILabel m_pGrantWishButtonLabel;

	// Token: 0x040002D7 RID: 727
	private SBGUILabel m_pWishCountLabel;

	// Token: 0x040002D8 RID: 728
	private SBGUIProgressMeter m_pWishProgressMeter;

	// Token: 0x040002D9 RID: 729
	private SBGUIPulseButton m_pGrantWishButton;

	// Token: 0x040002DA RID: 730
	private SBGUIButton m_pRushWishButton;

	// Token: 0x040002DB RID: 731
	private GameObject m_pFullParent;

	// Token: 0x040002DC RID: 732
	private GameObject m_pHungryParent;

	// Token: 0x040002DD RID: 733
	private Simulated m_pSimulated;

	// Token: 0x040002DE RID: 734
	private ResidentEntity m_pEntity;

	// Token: 0x040002DF RID: 735
	private Session m_pSession;

	// Token: 0x040002E0 RID: 736
	private Action m_pFeedWishAction;

	// Token: 0x040002E1 RID: 737
	private Action m_pRushWishAction;

	// Token: 0x040002E2 RID: 738
	private int? m_nHungerResourceID;

	// Token: 0x040002E3 RID: 739
	private Vector2 m_pWishIconSize;

	// Token: 0x040002E4 RID: 740
	private SBGUICharacterWishWidget._eWishState m_eWishState;

	// Token: 0x02000069 RID: 105
	private enum _eWishState
	{
		// Token: 0x040002E6 RID: 742
		eFull,
		// Token: 0x040002E7 RID: 743
		eHungry,
		// Token: 0x040002E8 RID: 744
		eNone
	}
}
