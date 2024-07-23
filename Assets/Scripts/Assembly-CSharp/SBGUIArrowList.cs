using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class SBGUIArrowList : SBGUIElement
{
	// Token: 0x0600039E RID: 926 RVA: 0x00011FF8 File Offset: 0x000101F8
	public virtual void SetData(Session pSession, List<SBGUIArrowList.ListItemData> pListItems, int nSelectedID, List<int> pIgnoreListItemIDs, Action<int> pSelectedItemChanged, Action<int> pItemClick)
	{
		this.m_pSession = pSession;
		this.m_nSelectedAtlasImageIndex = Mathf.Clamp(this.m_nSelectedAtlasImageIndex, 0, this.m_nNumAtlasImages - 1);
		this.m_pSelectedItemChanged = pSelectedItemChanged;
		this.m_pIgnoreListItemIDs = ((pIgnoreListItemIDs != null) ? pIgnoreListItemIDs : new List<int>());
		this.m_pListItems = pListItems;
		this.m_nNumListItems = this.m_pListItems.Count;
		this.m_nSelectedListItemIndex = 0;
		for (int i = 0; i < this.m_nNumListItems; i++)
		{
			if (pListItems[i].m_nID == nSelectedID)
			{
				this.m_nSelectedListItemIndex = i;
				break;
			}
		}
		this.m_pUpButton.ClearClickEvents();
		base.AttachActionToButton(this.m_pUpButton, delegate()
		{
			this.UpButtonPressed();
		});
		this.m_pDownButton.ClearClickEvents();
		base.AttachActionToButton(this.m_pDownButton, delegate()
		{
			this.DownButtonPressed();
		});
		this.m_pItemClicked = pItemClick;
		this.UpdateVisuals();
		this.UpdateItemClicks();
	}

	// Token: 0x0600039F RID: 927 RVA: 0x000120F8 File Offset: 0x000102F8
	public void SetSelectedID(int nSelectedID)
	{
		if (this.m_pListItems == null)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < this.m_nNumListItems; i++)
		{
			if (this.m_pListItems[i].m_nID == nSelectedID)
			{
				num = i;
				break;
			}
		}
		if (this.m_nSelectedListItemIndex != num)
		{
			this.m_nSelectedListItemIndex = num;
			this.UpdateVisuals();
			this.UpdateItemClicks();
			if (this.m_pSelectedItemChanged != null)
			{
				this.m_pSelectedItemChanged(this.m_pListItems[this.m_nSelectedListItemIndex].m_nID);
			}
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00012194 File Offset: 0x00010394
	protected override void Awake()
	{
		base.Awake();
		this.m_nNumAtlasImages = this.m_pAtlasImages.Length;
		this.m_pAtlasImageSizes = new Vector2[this.m_nNumAtlasImages];
		for (int i = 0; i < this.m_nNumAtlasImages; i++)
		{
			this.m_pAtlasImageSizes[i] = this.m_pAtlasImages[i].Size;
		}
		this.m_pItemButtons = new SBGUIButton[this.m_nNumAtlasImages];
		this.m_pLockedImages = new SBGUIAtlasImage[this.m_nNumAtlasImages];
		SBGUIElement sbguielement;
		for (int j = 0; j < this.m_nNumAtlasImages; j++)
		{
			sbguielement = base.FindChild("button_" + (j + 1).ToString());
			if (sbguielement != null)
			{
				this.m_pItemButtons[j] = sbguielement.GetComponent<SBGUIButton>();
			}
			else
			{
				this.m_pItemButtons[j] = null;
			}
			sbguielement = base.FindChild("locked_icon_" + (j + 1).ToString());
			if (sbguielement != null)
			{
				this.m_pLockedImages[j] = sbguielement.GetComponent<SBGUIAtlasImage>();
			}
			else
			{
				this.m_pLockedImages[j] = null;
			}
		}
		this.m_pSingleItemImageSize = Vector2.zero;
		if (this.m_pSingleItemImage != null)
		{
			this.m_pSingleItemImageSize = this.m_pSingleItemImage.Size;
		}
		this.m_pSingleItemLockedImage = null;
		sbguielement = base.FindChild("single_locked_icon");
		if (sbguielement != null)
		{
			this.m_pSingleItemLockedImage = sbguielement.GetComponent<SBGUIAtlasImage>();
		}
		this.m_pSingleItemButton = null;
		sbguielement = base.FindChild("single_button");
		if (sbguielement != null)
		{
			this.m_pSingleItemButton = sbguielement.GetComponent<SBGUIButton>();
		}
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00012340 File Offset: 0x00010540
	protected virtual void UpdateVisuals()
	{
		List<string> list = new List<string>();
		int num = this.m_nSelectedListItemIndex;
		while (num >= 0 && num < this.m_nNumListItems && this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[num].m_nID))
		{
			num--;
		}
		if (num < 0)
		{
			num = 0;
			while (num >= 0 && num < this.m_nNumListItems && this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[num].m_nID))
			{
				num++;
			}
		}
		if (num >= this.m_nNumListItems)
		{
			num = this.m_nNumListItems - 1;
		}
		if (num < 0)
		{
			num = 0;
		}
		this.m_nSelectedListItemIndex = num;
		int num2 = this.m_nSelectedAtlasImageIndex;
		string sessionActionId = string.Empty;
		SBGUIAtlasImage sbguiatlasImage = this.m_pAtlasImages[this.m_nSelectedAtlasImageIndex];
		YGAtlasSprite component = sbguiatlasImage.GetComponent<YGAtlasSprite>();
		if (this.m_nNumListItems > 0 && !this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[this.m_nSelectedListItemIndex].m_nID))
		{
			if (!sbguiatlasImage.active)
			{
				sbguiatlasImage.SetActive(true);
			}
			sessionActionId = this.m_pListItems[this.m_nSelectedListItemIndex].m_sTexture;
			SBGUIElement sbguielement = base.FindChildSessionActionId(sessionActionId, true);
			if (sbguielement != null)
			{
				sbguielement.SessionActionId = string.Empty;
			}
			if (string.IsNullOrEmpty(component.nonAtlasName))
			{
				if (!list.Contains(component.nonAtlasName))
				{
					list.Add(component.nonAtlasName);
				}
				else
				{
					base.View.Library.UnLoadTexture(component.nonAtlasName);
				}
			}
			sbguiatlasImage.SetSizeNoRebuild(this.m_pAtlasImageSizes[this.m_nSelectedAtlasImageIndex]);
			sbguiatlasImage.SetTextureFromAtlas(this.m_pListItems[this.m_nSelectedListItemIndex].m_sTexture, true, false, true, false, false, 0);
			sbguiatlasImage.SessionActionId = sessionActionId;
			if (this.m_pListItems[this.m_nSelectedListItemIndex].m_bLocked)
			{
				sbguiatlasImage.SetColor(this.m_pNonSelectedColor);
				if (this.m_pLockedImages[this.m_nSelectedAtlasImageIndex] != null)
				{
					this.m_pLockedImages[this.m_nSelectedAtlasImageIndex].SetActive(true);
				}
			}
			else
			{
				if (this.m_pLockedImages[this.m_nSelectedAtlasImageIndex] != null)
				{
					this.m_pLockedImages[this.m_nSelectedAtlasImageIndex].SetActive(false);
				}
				sbguiatlasImage.SetColor(Color.white);
			}
		}
		else if (sbguiatlasImage.active)
		{
			if (!string.IsNullOrEmpty(component.nonAtlasName))
			{
				base.View.Library.incrementTextureDuplicates(component.nonAtlasName);
			}
			sbguiatlasImage.SetActive(false);
		}
		bool flag = true;
		do
		{
			num2--;
			num--;
			if (num >= 0 && this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[num].m_nID))
			{
				num2++;
			}
			else
			{
				sbguiatlasImage = null;
				if (num2 >= 0)
				{
					sbguiatlasImage = this.m_pAtlasImages[num2];
				}
				if (num < 0)
				{
					flag = true;
					if (sbguiatlasImage != null && sbguiatlasImage.active)
					{
						if (!string.IsNullOrEmpty(component.nonAtlasName))
						{
							base.View.Library.incrementTextureDuplicates(component.nonAtlasName);
						}
						sbguiatlasImage.SetActive(false);
					}
				}
				else
				{
					flag = false;
					if (sbguiatlasImage != null)
					{
						if (!sbguiatlasImage.active)
						{
							sbguiatlasImage.SetActive(true);
						}
						sessionActionId = this.m_pListItems[num].m_sTexture;
						SBGUIElement sbguielement = base.FindChildSessionActionId(sessionActionId, true);
						if (sbguielement != null)
						{
							sbguielement.SessionActionId = string.Empty;
						}
						if (string.IsNullOrEmpty(component.nonAtlasName))
						{
							if (!list.Contains(component.nonAtlasName))
							{
								list.Add(component.nonAtlasName);
							}
							else
							{
								base.View.Library.UnLoadTexture(component.nonAtlasName);
							}
						}
						sbguiatlasImage.SetSizeNoRebuild(this.m_pAtlasImageSizes[num2]);
						sbguiatlasImage.SetTextureFromAtlas(this.m_pListItems[num].m_sTexture, true, false, true, false, false, 0);
						sbguiatlasImage.SessionActionId = sessionActionId;
						if (this.m_pListItems[num].m_bLocked)
						{
							sbguiatlasImage.SetColor(this.m_pNonSelectedColor);
							if (this.m_pLockedImages[num2] != null)
							{
								this.m_pLockedImages[num2].SetActive(true);
							}
						}
						else
						{
							sbguiatlasImage.SetColor(Color.white);
							if (this.m_pLockedImages[num2] != null)
							{
								this.m_pLockedImages[num2].SetActive(false);
							}
						}
					}
				}
			}
		}
		while (num2 > 0);
		num2 = this.m_nSelectedAtlasImageIndex;
		num = this.m_nSelectedListItemIndex;
		bool flag2 = true;
		do
		{
			num2++;
			num++;
			if (num < this.m_nNumListItems && this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[num].m_nID))
			{
				num2--;
			}
			else
			{
				sbguiatlasImage = null;
				if (num2 < this.m_nNumAtlasImages)
				{
					sbguiatlasImage = this.m_pAtlasImages[num2];
				}
				if (num >= this.m_nNumListItems)
				{
					flag2 = true;
					if (sbguiatlasImage != null && sbguiatlasImage.active)
					{
						if (!string.IsNullOrEmpty(component.nonAtlasName))
						{
							base.View.Library.incrementTextureDuplicates(component.nonAtlasName);
						}
						sbguiatlasImage.SetActive(false);
					}
				}
				else
				{
					flag2 = false;
					if (sbguiatlasImage != null)
					{
						if (!sbguiatlasImage.active)
						{
							sbguiatlasImage.SetActive(true);
						}
						sessionActionId = this.m_pListItems[num].m_sTexture;
						SBGUIElement sbguielement = base.FindChildSessionActionId(sessionActionId, true);
						if (sbguielement != null)
						{
							sbguielement.SessionActionId = string.Empty;
						}
						if (string.IsNullOrEmpty(component.nonAtlasName))
						{
							if (!list.Contains(component.nonAtlasName))
							{
								list.Add(component.nonAtlasName);
							}
							else
							{
								base.View.Library.UnLoadTexture(component.nonAtlasName);
							}
						}
						sbguiatlasImage.SetSizeNoRebuild(this.m_pAtlasImageSizes[num2]);
						sbguiatlasImage.SetTextureFromAtlas(this.m_pListItems[num].m_sTexture, true, false, true, false, false, 0);
						sbguiatlasImage.SessionActionId = sessionActionId;
						if (this.m_pListItems[num].m_bLocked)
						{
							sbguiatlasImage.SetColor(this.m_pNonSelectedColor);
							if (this.m_pLockedImages[num2] != null)
							{
								this.m_pLockedImages[num2].SetActive(true);
							}
						}
						else
						{
							sbguiatlasImage.SetColor(Color.white);
							if (this.m_pLockedImages[num2] != null)
							{
								this.m_pLockedImages[num2].SetActive(false);
							}
						}
					}
				}
			}
		}
		while (num2 < this.m_nNumAtlasImages - 1);
		if (flag)
		{
			if (this.m_pUpButton.active)
			{
				this.m_pUpButton.SetActive(false);
			}
		}
		else if (!this.m_pUpButton.active)
		{
			this.m_pUpButton.SetActive(true);
		}
		if (flag2)
		{
			if (this.m_pDownButton.active)
			{
				this.m_pDownButton.SetActive(false);
			}
		}
		else if (!this.m_pDownButton.active)
		{
			this.m_pDownButton.SetActive(true);
		}
		if (flag && flag2)
		{
			bool flag3 = this.m_nNumListItems <= 0 || this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[this.m_nSelectedListItemIndex].m_nID);
			if (this.m_pSingleItemParent != null)
			{
				if (flag3)
				{
					this.m_pSingleItemParent.SetActive(false);
				}
				else
				{
					this.m_pSingleItemParent.SetActive(true);
				}
			}
			if (this.m_pMultipleItemParent != null)
			{
				this.m_pMultipleItemParent.SetActive(false);
			}
			if (!flag3 && this.m_pSingleItemImage != null)
			{
				sessionActionId = this.m_pListItems[this.m_nSelectedListItemIndex].m_sTexture;
				SBGUIElement sbguielement = base.FindChildSessionActionId(sessionActionId, true);
				if (sbguielement != null)
				{
					sbguielement.SessionActionId = string.Empty;
				}
				this.m_pSingleItemImage.SetSizeNoRebuild(this.m_pSingleItemImageSize);
				this.m_pSingleItemImage.SetTextureFromAtlas(this.m_pListItems[this.m_nSelectedListItemIndex].m_sTexture, true, false, true, false, false, 0);
				this.m_pSingleItemImage.SessionActionId = sessionActionId;
				if (this.m_pListItems[this.m_nSelectedListItemIndex].m_bLocked)
				{
					this.m_pSingleItemImage.SetColor(this.m_pNonSelectedColor);
					if (this.m_pSingleItemLockedImage != null)
					{
						this.m_pSingleItemLockedImage.SetActive(true);
					}
				}
				else
				{
					this.m_pSingleItemImage.SetColor(Color.white);
					if (this.m_pSingleItemLockedImage != null)
					{
						this.m_pSingleItemLockedImage.SetActive(false);
					}
				}
			}
		}
		else
		{
			if (this.m_pSingleItemParent != null)
			{
				this.m_pSingleItemParent.SetActive(false);
			}
			if (this.m_pMultipleItemParent != null)
			{
				this.m_pMultipleItemParent.SetActive(true);
			}
		}
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00012CAC File Offset: 0x00010EAC
	protected void UpdateItemClicks()
	{
		if (this.m_pItemClicked != null)
		{
			for (int i = 0; i < this.m_nNumAtlasImages; i++)
			{
				SBGUIButton sbguibutton = this.m_pItemButtons[i];
				if (sbguibutton != null)
				{
					sbguibutton.ClearClickEvents();
					int nLoopIndex = this.m_nSelectedListItemIndex - (this.m_nSelectedAtlasImageIndex - i);
					if (nLoopIndex >= 0 && nLoopIndex < this.m_nNumListItems)
					{
						Action action = delegate()
						{
							SBGUIArrowList.ListItemData listItemData = this.m_pListItems[nLoopIndex];
							this.m_pItemClicked(listItemData.m_nID);
						};
						base.AttachActionToButton(sbguibutton, action);
					}
				}
			}
			if (this.m_pSingleItemButton != null)
			{
				for (int j = 0; j < this.m_nNumListItems; j++)
				{
					if (!this.m_pIgnoreListItemIDs.Contains(this.m_pListItems[j].m_nID))
					{
						this.m_pSingleItemButton.ClearClickEvents();
						int nLoopIndex = j;
						Action action = delegate()
						{
							SBGUIArrowList.ListItemData listItemData = this.m_pListItems[nLoopIndex];
							this.m_pItemClicked(listItemData.m_nID);
						};
						base.AttachActionToButton(this.m_pSingleItemButton, action);
						break;
					}
				}
			}
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00012DE0 File Offset: 0x00010FE0
	private void DownButtonPressed()
	{
		if (this.m_nSelectedListItemIndex < this.m_nNumListItems - 1)
		{
			this.m_nSelectedListItemIndex++;
			this.UpdateVisuals();
			this.UpdateItemClicks();
			if (this.m_pSelectedItemChanged != null)
			{
				this.m_pSelectedItemChanged(this.m_pListItems[this.m_nSelectedListItemIndex].m_nID);
			}
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00012E48 File Offset: 0x00011048
	private void UpButtonPressed()
	{
		if (this.m_nSelectedListItemIndex > 0)
		{
			this.m_nSelectedListItemIndex--;
			this.UpdateVisuals();
			this.UpdateItemClicks();
			if (this.m_pSelectedItemChanged != null)
			{
				this.m_pSelectedItemChanged(this.m_pListItems[this.m_nSelectedListItemIndex].m_nID);
			}
		}
	}

	// Token: 0x0400025D RID: 605
	public SBGUIAtlasImage[] m_pAtlasImages;

	// Token: 0x0400025E RID: 606
	public int m_nSelectedAtlasImageIndex;

	// Token: 0x0400025F RID: 607
	public SBGUIButton m_pUpButton;

	// Token: 0x04000260 RID: 608
	public SBGUIButton m_pDownButton;

	// Token: 0x04000261 RID: 609
	public Color m_pNonSelectedColor = Color.white;

	// Token: 0x04000262 RID: 610
	public GameObject m_pSingleItemParent;

	// Token: 0x04000263 RID: 611
	public GameObject m_pMultipleItemParent;

	// Token: 0x04000264 RID: 612
	public SBGUIAtlasImage m_pSingleItemImage;

	// Token: 0x04000265 RID: 613
	protected int m_nNumAtlasImages;

	// Token: 0x04000266 RID: 614
	protected List<SBGUIArrowList.ListItemData> m_pListItems;

	// Token: 0x04000267 RID: 615
	protected int m_nNumListItems;

	// Token: 0x04000268 RID: 616
	protected int m_nSelectedListItemIndex;

	// Token: 0x04000269 RID: 617
	protected Action<int> m_pSelectedItemChanged;

	// Token: 0x0400026A RID: 618
	protected Action<int> m_pItemClicked;

	// Token: 0x0400026B RID: 619
	protected Vector2[] m_pAtlasImageSizes;

	// Token: 0x0400026C RID: 620
	protected Vector2 m_pSingleItemImageSize;

	// Token: 0x0400026D RID: 621
	protected Session m_pSession;

	// Token: 0x0400026E RID: 622
	protected List<int> m_pIgnoreListItemIDs;

	// Token: 0x0400026F RID: 623
	private SBGUIAtlasImage[] m_pLockedImages;

	// Token: 0x04000270 RID: 624
	private SBGUIAtlasImage m_pSingleItemLockedImage;

	// Token: 0x04000271 RID: 625
	private SBGUIButton[] m_pItemButtons;

	// Token: 0x04000272 RID: 626
	private SBGUIButton m_pSingleItemButton;

	// Token: 0x0200005D RID: 93
	public class ListItemData
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x00012EB8 File Offset: 0x000110B8
		public ListItemData(int nID, string sTexture, bool bLocked = false)
		{
			this.m_nID = nID;
			if (string.IsNullOrEmpty(sTexture))
			{
				sTexture = "_blank_.png";
			}
			this.m_sTexture = sTexture;
			this.m_bLocked = bLocked;
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00012EF4 File Offset: 0x000110F4
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00012EFC File Offset: 0x000110FC
		public int m_nID { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00012F08 File Offset: 0x00011108
		// (set) Token: 0x060003AB RID: 939 RVA: 0x00012F10 File Offset: 0x00011110
		public string m_sTexture { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00012F1C File Offset: 0x0001111C
		// (set) Token: 0x060003AD RID: 941 RVA: 0x00012F24 File Offset: 0x00011124
		public bool m_bLocked { get; private set; }
	}
}
