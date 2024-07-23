using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000063 RID: 99
public class SBGUICharacterArrowList : SBGUIArrowList
{
	// Token: 0x060003DE RID: 990 RVA: 0x000150E4 File Offset: 0x000132E4
	protected override void Awake()
	{
		base.Awake();
		this.m_pWishImages = new SBGUIAtlasImage[this.m_nNumAtlasImages];
		this.m_pWishBubbleImages = new SBGUIAtlasImage[this.m_nNumAtlasImages];
		this.m_pWishImagesSizes = new Vector2[this.m_nNumAtlasImages];
		for (int i = 0; i < this.m_nNumAtlasImages; i++)
		{
			SBGUIElement sbguielement = base.FindChild("wish_image_" + (i + 1).ToString());
			if (sbguielement != null)
			{
				this.m_pWishImages[i] = (SBGUIAtlasImage)sbguielement;
				this.m_pWishImagesSizes[i] = this.m_pWishImages[i].Size;
			}
			sbguielement = base.FindChild("wish_bubble_image_" + (i + 1).ToString());
			if (sbguielement != null)
			{
				this.m_pWishBubbleImages[i] = (SBGUIAtlasImage)sbguielement;
			}
		}
		this.m_pSingleWishBubbleImage = (SBGUIAtlasImage)base.FindChild("single_wish_bubble_image");
		this.m_pSingleWishImage = (SBGUIAtlasImage)base.FindChild("single_wish_image");
		this.m_pSingleWishImageSize = Vector2.zero;
		if (this.m_pSingleWishImage != null)
		{
			this.m_pSingleWishImageSize = this.m_pSingleWishImage.Size;
		}
	}

	// Token: 0x060003DF RID: 991 RVA: 0x00015224 File Offset: 0x00013424
	protected override void UpdateVisuals()
	{
		if (this.m_pSession == null)
		{
			return;
		}
		base.UpdateVisuals();
		this.UpdateWishIcons();
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x00015240 File Offset: 0x00013440
	private void Update()
	{
		if (this.m_pSession == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.m_nNumListItems; i++)
		{
			SBGUIArrowList.ListItemData listItemData = this.m_pListItems[i];
			List<Task> activeTasksForSimulated = this.m_pSession.TheGame.taskManager.GetActiveTasksForSimulated(listItemData.m_nID, null, true);
			if (activeTasksForSimulated == null || activeTasksForSimulated.Count <= 0 || activeTasksForSimulated[0].GetTimeLeft() <= 0UL)
			{
				this.m_pListItems.RemoveAt(i);
				if (i == this.m_nSelectedListItemIndex)
				{
					this.m_nSelectedListItemIndex = Mathf.Clamp(this.m_nSelectedListItemIndex - 1, 0, this.m_nSelectedListItemIndex);
				}
				i--;
				this.m_nNumListItems--;
				flag = true;
			}
		}
		int num = this.m_pIgnoreListItemIDs.Count;
		for (int j = 0; j < num; j++)
		{
			int num2 = -1;
			SBGUIArrowList.ListItemData listItemData = null;
			for (int k = 0; k < this.m_nNumListItems; k++)
			{
				listItemData = this.m_pListItems[k];
				if (listItemData.m_nID == this.m_pIgnoreListItemIDs[j])
				{
					num2 = k;
					break;
				}
			}
			if (num2 >= 0)
			{
				List<Task> activeTasksForSimulated = this.m_pSession.TheGame.taskManager.GetActiveTasksForSimulated(listItemData.m_nID, null, true);
				if (activeTasksForSimulated != null && activeTasksForSimulated.Count > 0 && !activeTasksForSimulated[0].m_bMovingToTarget)
				{
					this.m_pIgnoreListItemIDs.RemoveAt(j);
					num--;
					j--;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.UpdateVisuals();
			base.UpdateItemClicks();
		}
		else
		{
			this.UpdateWishIcons();
		}
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x000153FC File Offset: 0x000135FC
	private void UpdateWishIcons()
	{
		int? nHungerResourceDID = null;
		if (this.m_pSingleItemParent != null && this.m_pSingleItemParent.activeSelf)
		{
			int nID = this.m_pListItems[this.m_nSelectedListItemIndex].m_nID;
			Simulated simulated = this.m_pSession.TheGame.simulation.FindSimulated(new int?(nID));
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			nHungerResourceDID = entity.HungerResourceId;
			this.SetWishImagesForHungerDID(this.m_pSingleWishImage, this.m_pSingleWishImageSize, this.m_pSingleWishBubbleImage, nHungerResourceDID);
		}
		else
		{
			int i = this.m_nSelectedAtlasImageIndex;
			int num = this.m_nSelectedListItemIndex;
			if (this.m_nNumListItems > 0 && !this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[this.m_nSelectedListItemIndex].m_nID))
			{
				int nID = this.m_pListItems[num].m_nID;
				Simulated simulated = this.m_pSession.TheGame.simulation.FindSimulated(new int?(nID));
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				nHungerResourceDID = entity.HungerResourceId;
			}
			if (i >= 0 && i < this.m_pWishImages.Length)
			{
				this.SetWishImagesForHungerDID(this.m_pWishImages[i], this.m_pWishImagesSizes[i], this.m_pWishBubbleImages[i], nHungerResourceDID);
			}
			while (i > 0)
			{
				i--;
				num--;
				if (num >= 0 && this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[num].m_nID))
				{
					i++;
				}
				else
				{
					if (num < 0)
					{
						nHungerResourceDID = null;
					}
					else
					{
						int nID = this.m_pListItems[num].m_nID;
						Simulated simulated = this.m_pSession.TheGame.simulation.FindSimulated(new int?(nID));
						ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
						nHungerResourceDID = entity.HungerResourceId;
					}
					if (i >= 0 && i < this.m_pWishImages.Length)
					{
						this.SetWishImagesForHungerDID(this.m_pWishImages[i], this.m_pWishImagesSizes[i], this.m_pWishBubbleImages[i], nHungerResourceDID);
					}
				}
			}
			i = this.m_nSelectedAtlasImageIndex;
			num = this.m_nSelectedListItemIndex;
			while (i < this.m_nNumAtlasImages - 1)
			{
				i++;
				num++;
				if (num < this.m_nNumListItems && this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[num].m_nID))
				{
					i--;
				}
				else
				{
					if (num >= this.m_nNumListItems)
					{
						nHungerResourceDID = null;
					}
					else
					{
						int nID = this.m_pListItems[num].m_nID;
						Simulated simulated = this.m_pSession.TheGame.simulation.FindSimulated(new int?(nID));
						ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
						nHungerResourceDID = entity.HungerResourceId;
					}
					if (i >= 0 && i < this.m_pWishImages.Length)
					{
						this.SetWishImagesForHungerDID(this.m_pWishImages[i], this.m_pWishImagesSizes[i], this.m_pWishBubbleImages[i], nHungerResourceDID);
					}
				}
			}
		}
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00015744 File Offset: 0x00013944
	private void SetWishImagesForHungerDID(SBGUIAtlasImage pWishImage, Vector2 pWishImageSize, SBGUIAtlasImage pWishBubbleImage, int? nHungerResourceDID)
	{
		if (pWishImage != null)
		{
			if (nHungerResourceDID != null)
			{
				Resource resource = this.m_pSession.TheGame.resourceManager.Resources[nHungerResourceDID.Value];
				if (!pWishImage.active)
				{
					pWishImage.SetActive(true);
				}
				if (pWishImage.name != resource.GetResourceTexture())
				{
					pWishImage.SetSizeNoRebuild(pWishImageSize);
					pWishImage.SetTextureFromAtlas(resource.GetResourceTexture(), true, false, true, false, false, 0);
				}
			}
			else if (pWishImage.active)
			{
				pWishImage.SetActive(false);
			}
		}
		if (pWishBubbleImage != null)
		{
			if (nHungerResourceDID != null)
			{
				if (!pWishBubbleImage.active)
				{
					pWishBubbleImage.SetActive(true);
				}
			}
			else if (pWishBubbleImage.active)
			{
				pWishBubbleImage.SetActive(false);
			}
		}
	}

	// Token: 0x0400028B RID: 651
	private SBGUIAtlasImage[] m_pWishImages;

	// Token: 0x0400028C RID: 652
	private SBGUIAtlasImage[] m_pWishBubbleImages;

	// Token: 0x0400028D RID: 653
	private Vector2[] m_pWishImagesSizes;

	// Token: 0x0400028E RID: 654
	private SBGUIAtlasImage m_pSingleWishImage;

	// Token: 0x0400028F RID: 655
	private SBGUIAtlasImage m_pSingleWishBubbleImage;

	// Token: 0x04000290 RID: 656
	private Vector2 m_pSingleWishImageSize;
}
