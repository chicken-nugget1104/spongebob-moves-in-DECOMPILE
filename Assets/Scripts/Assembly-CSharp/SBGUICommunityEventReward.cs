using System;
using UnityEngine;

// Token: 0x0200006C RID: 108
public class SBGUICommunityEventReward : SBGUIElement
{
	// Token: 0x0600041D RID: 1053 RVA: 0x00018DE0 File Offset: 0x00016FE0
	protected override void Awake()
	{
		base.Awake();
		this.m_pRewardSize = this.m_pRewardImage.Size;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00018DFC File Offset: 0x00016FFC
	public void SetData(Session pSession, CommunityEvent pEvent, CommunityEvent.Reward pReward, SoaringCommunityEvent.Reward pSoaringReward, bool bIsPurchasable, bool bHideCost = false)
	{
		bool flag = pSoaringReward.m_bUnlocked || pSoaringReward.m_bAcquired;
		Blueprint blueprint = EntityManager.GetBlueprint(EntityType.BUILDING, pReward.m_nID, true);
		this.m_pRewardImage.SetSizeNoRebuild(this.m_pRewardSize);
		if (!flag && !string.IsNullOrEmpty(pReward.m_sLockedTexture))
		{
			this.m_pRewardImage.SetTextureFromAtlas(pReward.m_sLockedTexture, true, false, true, false, false, 0);
			this.m_pRewardImage.SetColor(Color.white);
			this.m_pRewardLabel.SetText(string.Empty);
		}
		else
		{
			this.m_pRewardImage.SetTextureFromAtlas(pReward.m_sTexture, true, false, true, false, false, 0);
			this.m_pRewardImage.SetColor(flag ? Color.white : this.m_pLockedColor);
			this.m_pRewardLabel.SetText(Language.Get((string)blueprint.Invariable["name"]));
		}
		this.m_pLockedImage.SetActive(!flag);
		this.m_pCurrencyImage.SetTextureFromAtlas(pSession.TheGame.resourceManager.Resources[pEvent.m_nValueID].GetResourceTexture());
		if (flag)
		{
			this.m_pLockedImage.SetActive(false);
		}
		else
		{
			this.m_pLockedImage.SetActive(true);
			if (pReward.m_bHideNameWhenLocked)
			{
				this.m_pRewardLabel.SetText(string.Empty);
			}
		}
		if (bIsPurchasable)
		{
			if (!bHideCost)
			{
				this.m_pCurrencyGO.SetActiveRecursively(true);
				this.m_pValueLabel.SetText(string.Format("{0:n0}", pSoaringReward.m_nValue));
			}
			else
			{
				this.m_pCurrencyGO.SetActiveRecursively(false);
				this.m_pNextImage.SetActive(true);
			}
		}
		else
		{
			this.m_pCurrencyGO.SetActiveRecursively(false);
			this.m_pNextImage.SetActive(false);
		}
	}

	// Token: 0x04000305 RID: 773
	public SBGUIAtlasImage m_pRewardImage;

	// Token: 0x04000306 RID: 774
	public SBGUIAtlasImage m_pLockedImage;

	// Token: 0x04000307 RID: 775
	public Color m_pLockedColor;

	// Token: 0x04000308 RID: 776
	public SBGUILabel m_pRewardLabel;

	// Token: 0x04000309 RID: 777
	public GameObject m_pCurrencyGO;

	// Token: 0x0400030A RID: 778
	public SBGUILabel m_pValueLabel;

	// Token: 0x0400030B RID: 779
	public SBGUIAtlasImage m_pNextImage;

	// Token: 0x0400030C RID: 780
	public SBGUIAtlasImage m_pCurrencyImage;

	// Token: 0x0400030D RID: 781
	private Vector2 m_pRewardSize;
}
