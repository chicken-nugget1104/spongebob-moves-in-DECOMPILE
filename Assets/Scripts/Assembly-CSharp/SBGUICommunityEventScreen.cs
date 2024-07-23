using System;
using System.Collections;
using System.Collections.Generic;
using MTools;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class SBGUICommunityEventScreen : SBGUITabbedDialog
{
	// Token: 0x06000421 RID: 1057 RVA: 0x00019024 File Offset: 0x00017224
	protected override void Awake()
	{
		this.m_pRewardCostLabel = (SBGUILabel)base.FindChild("get_next_item_label");
		this.m_pRewardCostTitleLabel = (SBGUILabel)base.FindChild("get_next_item_label_title");
		this.m_pTabOneDescriptionOne = (SBGUILabel)base.FindChild("tab_one_description_label_one");
		this.m_pBuyRewardGO = this.m_pRewardCostLabel.transform.parent.gameObject;
		this.m_pNextRecipeCostLabel = (SBGUILabel)base.FindChild("next_recipe_cost_label");
		this.m_pNextRecipeIconLabel = (SBGUILabel)base.FindChild("next_recipe_icon_label");
		this.m_pNextRecipeIconImage = (SBGUIAtlasImage)base.FindChild("next_recipe_icon");
		this.m_pNextRecipeCostIconImage = (SBGUIAtlasImage)base.FindChild("next_recipe_cost_icon");
		this.m_pNextRecipeLabel = (SBGUILabel)base.FindChild("next_recipe_title_label");
		this.m_pNextRecipeGO = this.m_pNextRecipeIconImage.transform.parent.gameObject;
		this.m_pLeftBannerImage = (SBGUIAtlasImage)base.FindChild("banner_image");
		this.m_pRightBannerImage = base.FindChild("banner_image_two").GetComponent<SBGUIAtlasImage>();
		this.m_pBannerTitle = (SBGUILabel)base.FindChild("banner_image_two_label");
		this.m_pNextItemButton = (SBGUIButton)base.FindChild("get_next_item_button");
		this.m_pSpecialCurrencyIcon = (SBGUIAtlasImage)base.FindChild("special_currency_icon");
		this.m_pSpecialCurrencyLabel = (SBGUILabel)base.FindChild("special_currency_label");
		this.m_pHardCurrencyLabel = (SBGUILabel)base.FindChild("hard_currency_label");
		this.m_pTabTwoDescriptionLabelOne = (SBGUILabel)base.FindChild("tab_two_description_label_one");
		this.m_pTabTwoDescriptionLabelTwo = (SBGUILabel)base.FindChild("tab_two_description_label_two");
		this.m_pTabTwoFooterImage = (SBGUIAtlasImage)base.FindChild("tab_two_footer_image");
		if (this.m_pTabTwoFooterImage != null)
		{
			UnityEngine.Object.Destroy(this.m_pTabTwoFooterImage.gameObject);
		}
		this.m_pCommunityCountLabel = (SBGUILabel)base.FindChild("community_count_label");
		this.m_pCommunityTotalLabel = (SBGUILabel)base.FindChild("community_total_label");
		this.m_pCommunityProgressMeter = (SBGUIProgressMeter)base.FindChild("community_bar_meter");
		this.m_pCommunityProgressBarGO = this.m_pCommunityProgressMeter.transform.parent.parent.gameObject;
		this.m_pOfflineLabel = (SBGUILabel)base.FindChild("offline_label");
		base.Awake();
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00019294 File Offset: 0x00017494
	public void SetupButtons()
	{
		base.AttachActionToButton("get_next_item_button", new Action(this.BuyNextRewardButtonClick));
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x000192B0 File Offset: 0x000174B0
	public Vector2 GetHardSpendButtonPosition()
	{
		if (this.m_pNextItemButton != null)
		{
			return this.m_pNextItemButton.GetScreenPosition();
		}
		return Vector2.zero;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x000192E0 File Offset: 0x000174E0
	protected override void LoadCategories(Session pSession)
	{
		this.categories = new Dictionary<string, SBTabCategory>();
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		SBTabCategory value = new SBCommunityEventCategory(new Dictionary<string, object>
		{
			{
				"name",
				SBGUICommunityEventScreen._sTab1Name
			},
			{
				"label",
				SBGUICommunityEventScreen._sTab1Name
			},
			{
				"display.material",
				activeEvent.m_sTabOneTexture
			},
			{
				"type",
				"buildings"
			}
		});
		this.categories.Add("Tab1", value);
		value = new SBCommunityEventCategory(new Dictionary<string, object>
		{
			{
				"name",
				SBGUICommunityEventScreen._sTab2Name
			},
			{
				"label",
				SBGUICommunityEventScreen._sTab2Name
			},
			{
				"display.material",
				activeEvent.m_sTabTwoTexture
			},
			{
				"type",
				"buildings"
			}
		});
		this.categories.Add("Tab2", value);
		this.m_pLeftBannerImage.SetTextureFromAtlas(activeEvent.m_sLeftBannerTexture);
		this.m_pLeftBannerImage.ResetSize();
		this.m_pRightBannerImage.SetTextureFromAtlas(activeEvent.m_sRightBannerTexture);
		this.m_pBannerTitle.SetText(Language.Get(activeEvent.m_sRightBannerTitle));
		this.m_pTabOneDescriptionOne.SetText(Language.Get(activeEvent.m_sIndividualFooterText));
		this.m_pTabTwoDescriptionLabelOne.SetText(Language.Get(activeEvent.m_sCommunityHeaderText));
		this.m_pTabTwoDescriptionLabelTwo.SetText(Language.Get(activeEvent.m_sCommunityFooterText));
		this.m_pNextRecipeCostIconImage.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[activeEvent.m_nValueID].GetResourceTexture());
		this.m_pNextRecipeLabel.SetText(Language.Get("!!EVENT_NEXT_RECIPE"));
		this.m_pCommunityTotalLabel.SetText(Language.Get("!!EVENT_COMMUNITY_TOTAL"));
		this.m_pOfflineLabel.SetText(Language.Get("!!EVENT_OFFLINE"));
		this.m_pRewardCostTitleLabel.SetText(Language.Get("!!EVENT_BUY_ITEM_NOW"));
		if (this.session.TheGame.resourceManager.Resources.ContainsKey(activeEvent.m_nValueID))
		{
			this.m_pSpecialCurrencyIcon.SetTextureFromAtlas(this.session.TheGame.resourceManager.Resources[activeEvent.m_nValueID].GetResourceTexture(), true, false, true, false, false, 0);
		}
		this.UpdateCurrency();
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x00019540 File Offset: 0x00017740
	private void UpdateCurrency()
	{
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		if (this.session.TheGame.resourceManager.Resources.ContainsKey(activeEvent.m_nValueID))
		{
			this.m_pSpecialCurrencyLabel.SetText(this.session.TheGame.resourceManager.Resources[activeEvent.m_nValueID].Amount.ToString());
		}
		this.m_pHardCurrencyLabel.SetText(this.session.TheGame.resourceManager.Resources[ResourceManager.HARD_CURRENCY].Amount.ToString());
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x00019600 File Offset: 0x00017800
	private void HideTabTwoHack()
	{
		this.m_pTabTwoDescriptionLabelOne.SetActive(false);
		this.m_pTabTwoDescriptionLabelTwo.SetActive(false);
		this.m_pCommunityCountLabel.SetActive(false);
		this.m_pCommunityTotalLabel.SetActive(false);
		this.m_pCommunityProgressBarGO.SetActive(false);
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0001964C File Offset: 0x0001784C
	protected override IEnumerator BuildTabCoroutine(string sTabName)
	{
		if (!this.tabContents.TryGetValue(sTabName, out this.currentTab))
		{
			SBGUIElement pAnchor = SBGUIElement.Create(base.FindChild("window"));
			pAnchor.name = string.Format(sTabName, new object[0]);
			pAnchor.transform.localPosition = Vector3.zero;
			this.tabContents[sTabName] = pAnchor;
			this.currentTab = pAnchor;
			if (sTabName == SBGUICommunityEventScreen._sTab1Name)
			{
				this.m_pTabOneDescriptionOne.transform.parent = this.currentTab.transform;
				this.m_pBuyRewardGO.transform.parent = this.currentTab.transform;
				this.m_pNextRecipeGO.transform.parent = this.currentTab.transform;
				this.HideTabTwoHack();
			}
			else
			{
				this.m_pTabTwoDescriptionLabelOne.transform.parent = this.currentTab.transform;
				this.m_pTabTwoDescriptionLabelTwo.transform.parent = this.currentTab.transform;
				this.m_pCommunityCountLabel.transform.parent = this.currentTab.transform;
				this.m_pCommunityTotalLabel.transform.parent = this.currentTab.transform;
				this.m_pCommunityProgressBarGO.transform.parent = this.currentTab.transform;
			}
		}
		CommunityEvent pEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (pEvent != null)
		{
			if (sTabName == SBGUICommunityEventScreen._sTab1Name)
			{
				if (this.session.IsOnline())
				{
					this.m_pOfflineLabel.SetActive(false);
					this.currentTab.SetActive(true);
					this.HideTabTwoHack();
					this.RefreshIndividualRewardTab();
				}
				else
				{
					this.currentTab.SetActive(false);
					this.m_pOfflineLabel.SetActive(true);
				}
			}
			else if (this.session.IsOnline())
			{
				this.m_pOfflineLabel.SetActive(false);
				this.m_bWaitingOnServer = true;
				SoaringCommunityEventManager.SetValueFinished += this.HandleSoaringSetValueFinished;
				SBMISoaring.SetEventValue(this.session, int.Parse(pEvent.m_sID), this.session.TheGame.resourceManager.Resources[pEvent.m_nValueID].Amount, null);
				while (this.m_bWaitingOnServer)
				{
					yield return null;
				}
				SoaringCommunityEventManager.SetValueFinished -= this.HandleSoaringSetValueFinished;
				this.currentTab.SetActive(true);
				this.RefreshCommunityRewardTab();
			}
			else
			{
				this.currentTab.SetActive(false);
				this.m_pOfflineLabel.SetActive(true);
				this.m_pTabTwoDescriptionLabelTwo.SetActive(true);
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00019678 File Offset: 0x00017878
	private void RefreshIndividualRewardTab()
	{
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		SoaringCommunityEvent @event = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (@event == null)
		{
			return;
		}
		if (this.m_pGUIIndividualRewards == null)
		{
			this.m_pGUIIndividualRewards = new MArray();
		}
		if (this.TabOneRewardTransforms == null)
		{
			return;
		}
		int num = this.TabOneRewardTransforms.Length;
		int num2 = this.m_pGUIIndividualRewards.count();
		SoaringCommunityEvent.Reward[] individualRewards = @event.IndividualRewards;
		int num3 = individualRewards.Length;
		int nextReward = this.GetNextReward("building");
		int nextReward2 = this.GetNextReward("recipe");
		int i = 0;
		int num4 = 0;
		while (i < num3)
		{
			if (num4 >= num)
			{
				break;
			}
			SoaringCommunityEvent.Reward reward = individualRewards[i];
			CommunityEvent.Reward reward2 = activeEvent.GetReward(reward.m_nID);
			if (!(reward2.m_sType == "recipe"))
			{
				SBGUICommunityEventReward sbguicommunityEventReward;
				if (num4 < num2)
				{
					sbguicommunityEventReward = (SBGUICommunityEventReward)this.m_pGUIIndividualRewards.objectAtIndex(num4);
				}
				else
				{
					this.TabOneRewardTransforms[num4].transform.parent = this.currentTab.transform;
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.m_pRewardPrefab);
					Transform transform = gameObject.transform;
					transform.parent = this.TabOneRewardTransforms[num4].transform;
					transform.localPosition = Vector3.zero;
					sbguicommunityEventReward = (SBGUICommunityEventReward)gameObject.GetComponent(typeof(SBGUICommunityEventReward));
				}
				sbguicommunityEventReward.SetActive(true);
				sbguicommunityEventReward.SetData(this.session, activeEvent, reward2, reward, nextReward == reward.m_nID, false);
				this.m_pGUIIndividualRewards.addObject(sbguicommunityEventReward);
				num4++;
			}
			i++;
		}
		int amount = this.session.TheGame.resourceManager.Resources[activeEvent.m_nValueID].Amount;
		this.m_pBuyRewardGO.SetActiveRecursively(nextReward != -1);
		if (nextReward != -1)
		{
			SoaringCommunityEvent.Reward reward = @event.GetReward(nextReward);
			int num5 = reward.m_nValue - amount;
			if (num5 > 0)
			{
				Dictionary<int, int> resourceAmounts = new Dictionary<int, int>
				{
					{
						activeEvent.m_nValueID,
						num5
					}
				};
				num5 = this.session.TheGame.resourceManager.GetResourcesPackageCostInHardCurrencyValue(new Cost(resourceAmounts));
			}
			else
			{
				num5 = 0;
			}
			this.m_pRewardCostLabel.SetText(string.Format("{0:n0}", num5));
		}
		this.m_pNextRecipeGO.SetActiveRecursively(nextReward2 != -1);
		if (nextReward2 != -1)
		{
			SoaringCommunityEvent.Reward reward = @event.GetReward(nextReward2);
			string str = string.Format("{0:n0}", amount);
			string str2 = string.Format("{0:n0}", reward.m_nValue);
			this.m_pNextRecipeCostLabel.SetText(str + "/" + str2);
			CommunityEvent.Reward reward2 = activeEvent.GetReward(nextReward2);
			this.m_pNextRecipeIconImage.SetSizeNoRebuild(new Vector2((float)reward2.m_nWidth, (float)reward2.m_nHeight));
			this.m_pNextRecipeIconImage.SetTextureFromAtlas(reward2.m_sTexture, true, false, true, false, false, 0);
			this.m_pNextRecipeIconLabel.SetText(Language.Get(this.session.TheGame.resourceManager.Resources[nextReward2].Name));
		}
		this.m_pNextRecipeGO.SetActiveRecursively(nextReward2 != -1);
		if (nextReward2 != -1)
		{
			SoaringCommunityEvent.Reward reward = @event.GetReward(nextReward2);
			string str3 = string.Format("{0:n0}", amount);
			string str4 = string.Format("{0:n0}", reward.m_nValue);
			this.m_pNextRecipeCostLabel.SetText(str3 + "/" + str4);
			CommunityEvent.Reward reward2 = activeEvent.GetReward(nextReward2);
			this.m_pNextRecipeIconImage.SetSizeNoRebuild(new Vector2((float)reward2.m_nWidth, (float)reward2.m_nHeight));
			this.m_pNextRecipeIconImage.SetTextureFromAtlas(reward2.m_sTexture, true, false, true, false, false, 0);
			this.m_pNextRecipeIconLabel.SetText(Language.Get(this.session.TheGame.resourceManager.Resources[nextReward2].Name));
		}
		this.UpdateCurrency();
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00019ABC File Offset: 0x00017CBC
	private void RefreshCommunityRewardTab()
	{
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		SoaringCommunityEvent @event = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (@event == null)
		{
			return;
		}
		if (this.TabTwoRewardTransforms == null)
		{
			return;
		}
		int num = this.TabTwoRewardTransforms.Length;
		if (this.m_pGUICommunityRewards == null)
		{
			this.m_pGUICommunityRewards = new MArray();
		}
		int num2 = this.m_pGUICommunityRewards.count();
		SoaringCommunityEvent.Reward[] communityRewards = @event.CommunityRewards;
		int num3 = communityRewards.Length;
		int num4 = 0;
		int nextCommunityReward = this.GetNextCommunityReward("building");
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int num7 = 0;
		while (i < num3)
		{
			SoaringCommunityEvent.Reward reward = communityRewards[i];
			CommunityEvent.Reward reward2 = activeEvent.GetReward(reward.m_nID);
			if (!(reward2.m_sType == "recipe"))
			{
				num5++;
				if (reward.m_nValue > num4)
				{
					num4 = reward.m_nValue;
				}
				if (num7 >= num)
				{
					break;
				}
				SBGUICommunityEventReward sbguicommunityEventReward;
				if (num7 < num2)
				{
					sbguicommunityEventReward = (SBGUICommunityEventReward)this.m_pGUICommunityRewards.objectAtIndex(num7);
				}
				else
				{
					this.TabTwoRewardTransforms[num7].transform.parent = this.currentTab.transform;
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.m_pRewardPrefab);
					Transform transform = gameObject.transform;
					transform.parent = this.TabTwoRewardTransforms[num7].transform;
					transform.localPosition = Vector3.zero;
					sbguicommunityEventReward = (SBGUICommunityEventReward)gameObject.GetComponent(typeof(SBGUICommunityEventReward));
				}
				if (nextCommunityReward == reward.m_nID)
				{
					num6 = num7;
				}
				sbguicommunityEventReward.SetActive(true);
				sbguicommunityEventReward.SetData(this.session, activeEvent, reward2, reward, nextCommunityReward == reward.m_nID, true);
				this.m_pGUICommunityRewards.addObject(sbguicommunityEventReward);
				num7++;
			}
			i++;
		}
		if (nextCommunityReward == -1)
		{
			this.m_pCommunityProgressMeter.Progress = 1f;
		}
		else
		{
			SoaringCommunityEvent.Reward reward = @event.GetReward(nextCommunityReward);
			float num8 = (float)num6 * (1f / (float)num5);
			int currentCommunityReward = this.GetCurrentCommunityReward("building");
			SoaringCommunityEvent.Reward reward3 = null;
			if (currentCommunityReward != -1)
			{
				reward3 = @event.GetReward(currentCommunityReward);
			}
			int num9;
			int num10;
			if (reward3 == null)
			{
				num9 = reward.m_nValue;
				num10 = @event.m_nCommunityValue;
			}
			else
			{
				num9 = reward.m_nValue - reward3.m_nValue;
				num10 = @event.m_nCommunityValue - reward3.m_nValue;
			}
			num8 += (float)num10 / (float)num9 * (1f / (float)num5);
			this.m_pCommunityProgressMeter.Progress = Mathf.Clamp01(num8);
		}
		string str = string.Format("{0:n0}", @event.m_nCommunityValue);
		string str2 = string.Format("{0:n0}", (nextCommunityReward != -1) ? @event.GetReward(nextCommunityReward).m_nValue : num4);
		this.m_pCommunityCountLabel.SetText(str + "/" + str2);
		if (this.m_pCommunityProgressMeter.Progress != 1f)
		{
			this.m_pTabTwoDescriptionLabelTwo.SetText(Language.Get(activeEvent.m_sCommunityFooterText));
			this.m_pTabTwoDescriptionLabelOne.SetActive(true);
		}
		else
		{
			this.m_pTabTwoDescriptionLabelTwo.SetText(Language.Get(activeEvent.m_sCommunityFooterAllUnlocksText));
			this.m_pTabTwoDescriptionLabelOne.SetActive(false);
		}
		this.UpdateCurrency();
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00019E30 File Offset: 0x00018030
	private int GetNextReward(string sType)
	{
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return -1;
		}
		SoaringCommunityEvent @event = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (@event == null)
		{
			return -1;
		}
		SoaringCommunityEvent.Reward[] individualRewards = @event.IndividualRewards;
		int num = individualRewards.Length;
		for (int i = 0; i < num; i++)
		{
			SoaringCommunityEvent.Reward reward = individualRewards[i];
			if (!(activeEvent.GetReward(reward.m_nID).m_sType != sType))
			{
				if (!reward.m_bAcquired)
				{
					return reward.m_nID;
				}
			}
		}
		return -1;
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00019ED4 File Offset: 0x000180D4
	private int GetNextCommunityReward(string sType)
	{
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return -1;
		}
		SoaringCommunityEvent @event = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (@event == null)
		{
			return -1;
		}
		SoaringCommunityEvent.Reward[] communityRewards = @event.CommunityRewards;
		int num = communityRewards.Length;
		for (int i = 0; i < num; i++)
		{
			SoaringCommunityEvent.Reward reward = communityRewards[i];
			if (!(activeEvent.GetReward(reward.m_nID).m_sType != sType))
			{
				if (!reward.m_bAcquired)
				{
					return reward.m_nID;
				}
			}
		}
		return -1;
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x00019F78 File Offset: 0x00018178
	private int GetCurrentCommunityReward(string sType)
	{
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return -1;
		}
		SoaringCommunityEvent @event = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (@event == null)
		{
			return -1;
		}
		SoaringCommunityEvent.Reward[] communityRewards = @event.CommunityRewards;
		int num = communityRewards.Length;
		SoaringCommunityEvent.Reward reward = null;
		for (int i = 0; i < num; i++)
		{
			SoaringCommunityEvent.Reward reward2 = communityRewards[i];
			if (!(activeEvent.GetReward(reward2.m_nID).m_sType != sType))
			{
				if (!reward2.m_bAcquired)
				{
					if (reward != null)
					{
						return reward.m_nID;
					}
					return -1;
				}
				else
				{
					reward = reward2;
				}
			}
		}
		return -1;
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x0001A02C File Offset: 0x0001822C
	public bool IsBuyingReward()
	{
		return SBGUICommunityEventScreen._nBuyRewardBuildingID != -1 || SBGUICommunityEventScreen._nBuyRewardRecipeID != -1;
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0001A048 File Offset: 0x00018248
	public void BuyRewardCancel()
	{
		SBGUICommunityEventScreen._nBuyRewardBuildingID = (SBGUICommunityEventScreen._nBuyRewardRecipeID = -1);
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x0001A058 File Offset: 0x00018258
	public void BuyRewardConfirm(int nCost)
	{
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			this.BuyRewardCancel();
			return;
		}
		SoaringCommunityEventManager.AquireGiftFinished += this.HandleSoaringAquireGiftFinished;
		SBMISoaring.AquireEventGift(this.session, int.Parse(activeEvent.m_sID), SBGUICommunityEventScreen._nBuyRewardBuildingID, nCost, true, null);
		if (SBGUICommunityEventScreen._nBuyRewardRecipeID != -1)
		{
			SBMISoaring.AquireEventGift(this.session, int.Parse(activeEvent.m_sID), SBGUICommunityEventScreen._nBuyRewardRecipeID, nCost, true, null);
		}
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0001A0F4 File Offset: 0x000182F4
	private void BuyNextRewardButtonClick()
	{
		if (this.IsBuyingReward())
		{
			return;
		}
		CommunityEvent activeEvent = this.session.TheGame.communityEventManager.GetActiveEvent();
		if (activeEvent == null)
		{
			return;
		}
		SBGUICommunityEventScreen._nBuyRewardBuildingID = this.GetNextReward("building");
		if (SBGUICommunityEventScreen._nBuyRewardBuildingID == -1)
		{
			return;
		}
		SBGUICommunityEventScreen._nBuyRewardRecipeID = this.GetNextReward("recipe");
		SoaringCommunityEvent @event = Soaring.CommunityEventManager.GetEvent(activeEvent.m_sID);
		if (SBGUICommunityEventScreen._nBuyRewardRecipeID != -1 && @event.GetReward(SBGUICommunityEventScreen._nBuyRewardRecipeID).m_nValue != @event.GetReward(SBGUICommunityEventScreen._nBuyRewardBuildingID).m_nValue)
		{
			SBGUICommunityEventScreen._nBuyRewardRecipeID = -1;
		}
		if (this.BuyButtonClickedEvent != null)
		{
			this.BuyButtonClickedEvent.FireEvent(activeEvent, @event, @event.GetReward(SBGUICommunityEventScreen._nBuyRewardBuildingID));
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0001A1C0 File Offset: 0x000183C0
	private void HandleSoaringAquireGiftFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		int num = pContext.soaringValue("giftDid");
		if (num == SBGUICommunityEventScreen._nBuyRewardBuildingID)
		{
			SBGUICommunityEventScreen._nBuyRewardBuildingID = -1;
			if (SBGUICommunityEventScreen._nBuyRewardRecipeID != -1)
			{
				return;
			}
			SoaringCommunityEventManager.AquireGiftFinished -= this.HandleSoaringAquireGiftFinished;
			this.RefreshIndividualRewardTab();
		}
		else if (num == SBGUICommunityEventScreen._nBuyRewardRecipeID)
		{
			SBGUICommunityEventScreen._nBuyRewardRecipeID = -1;
			if (SBGUICommunityEventScreen._nBuyRewardBuildingID != -1)
			{
				return;
			}
			SoaringCommunityEventManager.AquireGiftFinished -= this.HandleSoaringAquireGiftFinished;
			this.RefreshIndividualRewardTab();
		}
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0001A24C File Offset: 0x0001844C
	private void HandleSoaringSetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		this.m_bWaitingOnServer = false;
	}

	// Token: 0x0400030E RID: 782
	public GameObject m_pRewardPrefab;

	// Token: 0x0400030F RID: 783
	public GameObject[] TabOneRewardTransforms;

	// Token: 0x04000310 RID: 784
	public GameObject[] TabTwoRewardTransforms;

	// Token: 0x04000311 RID: 785
	private static string _sTab1Name = "Tab1";

	// Token: 0x04000312 RID: 786
	private static string _sTab2Name = "Tab2";

	// Token: 0x04000313 RID: 787
	private MArray m_pGUIIndividualRewards;

	// Token: 0x04000314 RID: 788
	private MArray m_pGUICommunityRewards;

	// Token: 0x04000315 RID: 789
	private GameObject m_pBuyRewardGO;

	// Token: 0x04000316 RID: 790
	private SBGUILabel m_pRewardCostLabel;

	// Token: 0x04000317 RID: 791
	private SBGUILabel m_pRewardCostTitleLabel;

	// Token: 0x04000318 RID: 792
	private SBGUILabel m_pTabOneDescriptionOne;

	// Token: 0x04000319 RID: 793
	private GameObject m_pNextRecipeGO;

	// Token: 0x0400031A RID: 794
	private SBGUILabel m_pNextRecipeLabel;

	// Token: 0x0400031B RID: 795
	private SBGUILabel m_pNextRecipeCostLabel;

	// Token: 0x0400031C RID: 796
	private SBGUIAtlasImage m_pNextRecipeIconImage;

	// Token: 0x0400031D RID: 797
	private SBGUIAtlasImage m_pNextRecipeCostIconImage;

	// Token: 0x0400031E RID: 798
	private SBGUILabel m_pNextRecipeIconLabel;

	// Token: 0x0400031F RID: 799
	private SBGUIAtlasImage m_pSpecialCurrencyIcon;

	// Token: 0x04000320 RID: 800
	private SBGUILabel m_pSpecialCurrencyLabel;

	// Token: 0x04000321 RID: 801
	private SBGUILabel m_pHardCurrencyLabel;

	// Token: 0x04000322 RID: 802
	private SBGUIAtlasImage m_pLeftBannerImage;

	// Token: 0x04000323 RID: 803
	private SBGUIAtlasImage m_pRightBannerImage;

	// Token: 0x04000324 RID: 804
	private SBGUILabel m_pBannerTitle;

	// Token: 0x04000325 RID: 805
	private SBGUILabel m_pTabTwoDescriptionLabelOne;

	// Token: 0x04000326 RID: 806
	private SBGUILabel m_pTabTwoDescriptionLabelTwo;

	// Token: 0x04000327 RID: 807
	private SBGUIAtlasImage m_pTabTwoFooterImage;

	// Token: 0x04000328 RID: 808
	private SBGUILabel m_pCommunityCountLabel;

	// Token: 0x04000329 RID: 809
	private SBGUILabel m_pCommunityTotalLabel;

	// Token: 0x0400032A RID: 810
	private GameObject m_pCommunityProgressBarGO;

	// Token: 0x0400032B RID: 811
	private SBGUIProgressMeter m_pCommunityProgressMeter;

	// Token: 0x0400032C RID: 812
	private SBGUILabel m_pOfflineLabel;

	// Token: 0x0400032D RID: 813
	private SBGUIButton m_pNextItemButton;

	// Token: 0x0400032E RID: 814
	private bool m_bWaitingOnServer;

	// Token: 0x0400032F RID: 815
	public EventDispatcher<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward> BuyButtonClickedEvent = new EventDispatcher<CommunityEvent, SoaringCommunityEvent, SoaringCommunityEvent.Reward>();

	// Token: 0x04000330 RID: 816
	private static int _nBuyRewardBuildingID = -1;

	// Token: 0x04000331 RID: 817
	private static int _nBuyRewardRecipeID = -1;
}
