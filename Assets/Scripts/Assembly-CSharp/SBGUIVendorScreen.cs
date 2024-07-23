using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class SBGUIVendorScreen : SBGUIScreen
{
	// Token: 0x060006DE RID: 1758 RVA: 0x0002B45C File Offset: 0x0002965C
	public void Setup(Session session, VendorDefinition vendorDef)
	{
		this.session = session;
		Simulated selected = session.TheGame.selected;
		this.vendingEntity = selected.GetEntity<VendingDecorator>();
		this.itemDescription = (SBGUILabel)base.FindChild("item_description_label");
		this.itemName = (SBGUILabel)base.FindChild("item_name_label");
		this.itemCost = (SBGUILabel)base.FindChild("item_cost_label");
		this.itemCostIcon = (SBGUIAtlasImage)base.FindChild("item_cost_icon");
		this.itemIcon = (SBGUIAtlasImage)base.FindChild("item_icon");
		this.descriptionGroup = base.FindChild("description_group");
		this.skipButton = (SBGUIButton)base.FindChild("skip_button");
		this.restockTimer = (SBGUILabel)base.FindChild("restock_timer_label");
		this.stockLabel = (SBGUILabel)base.FindChild("stock_label");
		this.slotMarker = base.FindChild("slot_marker");
		this.buyButton = (SBGUIButton)base.FindChild("buy_button");
		this.m_pTaskCharacterList = (SBGUICharacterArrowList)base.FindChild("character_portrait_parent");
		int? num = this.descriptionIconSize;
		if (num == null)
		{
			this.descriptionIconSize = new int?((int)this.itemIcon.Size.x);
		}
		int? num2 = this.itemCostIconSize;
		if (num2 == null)
		{
			this.itemCostIconSize = new int?((int)this.itemCostIcon.Size.x);
		}
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("window");
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)base.FindChild("buy_tab");
		SBGUIAtlasImage sbguiatlasImage3 = (SBGUIAtlasImage)base.FindChild("_inset");
		List<int> backgroundColor = vendorDef.backgroundColor;
		if (vendorDef.backgroundColor != null)
		{
			sbguiatlasImage.SetColor(new Color((float)backgroundColor[0] / 255f, (float)backgroundColor[1] / 255f, (float)backgroundColor[2] / 255f));
			sbguiatlasImage2.SetColor(new Color((float)backgroundColor[0] / 255f, (float)backgroundColor[1] / 255f, (float)backgroundColor[2] / 255f));
			sbguiatlasImage3.SetColor(new Color((float)backgroundColor[0] / 255f, (float)backgroundColor[1] / 255f, (float)backgroundColor[2] / 255f));
		}
		else
		{
			TFUtils.WarningLog("VendorDefinition " + vendorDef.did + " does not have a background.color defined");
		}
		SBGUIAtlasImage sbguiatlasImage4 = (SBGUIAtlasImage)base.FindChild("title");
		sbguiatlasImage4.SetTextureFromAtlas(vendorDef.titleTexture);
		SBGUIAtlasImage sbguiatlasImage5 = (SBGUIAtlasImage)base.FindChild("title_icon");
		sbguiatlasImage5.SetTextureFromAtlas(vendorDef.titleIconTexture);
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("buy_label");
		sbguilabel.SetText(Language.Get(vendorDef.buttonLabel));
		SBGUIPulseButton sbguipulseButton = (SBGUIPulseButton)base.FindChild("close");
		sbguipulseButton.SetTextureFromAtlas(vendorDef.cancelButtonTexture);
		this.CreateVendingInstanceSlots(session);
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x0002B790 File Offset: 0x00029990
	public void CreateNonScrollUI(List<int> pTaskCharacterDIDs, Action<int> pTaskCharacterClicked)
	{
		List<SBGUIArrowList.ListItemData> list = new List<SBGUIArrowList.ListItemData>();
		int count = pTaskCharacterDIDs.Count;
		List<int> list2 = new List<int>();
		if (count <= 0)
		{
			this.m_pTaskCharacterList.SetActive(false);
			return;
		}
		this.m_pTaskCharacterList.SetActive(true);
		for (int i = 0; i < count; i++)
		{
			Simulated simulated = this.session.TheGame.simulation.FindSimulated(new int?(pTaskCharacterDIDs[i]));
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			list.Add(new SBGUIArrowList.ListItemData(entity.DefinitionId, entity.QuestReminderIcon, false));
			List<Task> activeTasksForSimulated = this.session.TheGame.taskManager.GetActiveTasksForSimulated(entity.DefinitionId, null, true);
			if (activeTasksForSimulated != null && activeTasksForSimulated.Count > 0 && activeTasksForSimulated[0].m_bMovingToTarget)
			{
				list2.Add(entity.DefinitionId);
			}
		}
		this.m_pTaskCharacterList.SetData(this.session, list, (list.Count <= 0) ? 0 : list[0].m_nID, list2, null, pTaskCharacterClicked);
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x0002B8B4 File Offset: 0x00029AB4
	private void CreateVendingInstanceSlots(Session session)
	{
		this.ClearVendingSlots();
		Vector3 zero = Vector3.zero;
		int num = 1;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < 12; i++)
		{
			SBGUIVendorSlot sbguivendorSlot = SBGUIVendorSlot.CreateVendorSlot(session, this);
			sbguivendorSlot.SlotID = i;
			this.slotRefs.Add(sbguivendorSlot);
			sbguivendorSlot.SetParent(this.slotMarker);
			sbguivendorSlot.tform.localPosition = Vector3.zero;
			int? num4 = this.slotIconSize;
			if (num4 == null)
			{
				this.slotIconSize = new int?((int)sbguivendorSlot.itemIcon.Size.x);
			}
			if (num > 4)
			{
				num2 -= (sbguivendorSlot.slotBackground.Size.y + 8f) * 0.01f;
				num3 = 0f;
				num = 1;
			}
			zero = new Vector3(num3, num2, 0f);
			sbguivendorSlot.tform.localPosition += zero;
			num3 += (sbguivendorSlot.slotBackground.Size.x + 8f) * 0.01f;
			num++;
		}
		SBGUIVendorSlot specialSlot = (SBGUIVendorSlot)base.FindChild("iod_slot");
		specialSlot.IsSpecial = true;
		specialSlot.SlotID = this.slotRefs.Count;
		specialSlot.AttachActionToButton("slot_background", delegate()
		{
			session.TheSoundEffectManager.PlaySound("HighlightItem");
			this.HighlightSlot(session, specialSlot);
		});
		int? num5 = this.specialSlotIconSize;
		if (num5 == null)
		{
			this.specialSlotIconSize = new int?((int)specialSlot.itemIcon.Size.x);
		}
		this.slotRefs.Add(specialSlot);
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x0002BAB0 File Offset: 0x00029CB0
	public void UpdateVendingInstanceSlots(Session session)
	{
		Dictionary<int, VendingInstance> vendingInstances = session.TheGame.vendingManager.GetVendingInstances(this.vendingEntity.Id);
		VendingInstance specialInstance = session.TheGame.vendingManager.GetSpecialInstance(this.vendingEntity.Id);
		foreach (SBGUIVendorSlot sbguivendorSlot in this.slotRefs)
		{
			VendingInstance vendingInstance;
			vendingInstances.TryGetValue(sbguivendorSlot.SlotID, out vendingInstance);
			if (vendingInstance == null)
			{
				vendingInstance = specialInstance;
			}
			if (vendingInstance == null)
			{
				sbguivendorSlot.SetEmpty(true, false);
			}
			else
			{
				VendorStock stock = session.TheGame.vendingManager.GetStock(vendingInstance.StockId);
				if (sbguivendorSlot.quantityLabel != null)
				{
					if (vendingInstance.remaining >= 2)
					{
						sbguivendorSlot.quantityCircle.SetActive(true);
						sbguivendorSlot.quantityLabel.SetText(vendingInstance.remaining.ToString());
					}
					else
					{
						sbguivendorSlot.quantityCircle.SetActive(false);
					}
				}
				if (vendingInstance.remaining <= 0)
				{
					if (vendingInstance == specialInstance)
					{
						sbguivendorSlot.SetEmpty(true, true);
					}
					else
					{
						sbguivendorSlot.SetEmpty(true, false);
					}
				}
				else if (vendingInstance == specialInstance)
				{
					sbguivendorSlot.SetEmpty(false, true);
				}
				else
				{
					sbguivendorSlot.SetEmpty(false, false);
				}
				if ((sbguivendorSlot.SlotID == this.lastSelectedSlotID || sbguivendorSlot == this.lastSelectedSlot) && !sbguivendorSlot.Empty)
				{
					sbguivendorSlot.SetHighlight(true, false);
					this.lastSelectedSlot = sbguivendorSlot;
					this.lastSelectedSlotID = sbguivendorSlot.SlotID;
				}
				else
				{
					sbguivendorSlot.SetHighlight(false, true);
				}
				sbguivendorSlot.itemIcon.SetTextureFromAtlas(stock.Icon);
				if (sbguivendorSlot == this.slotRefs[this.slotRefs.Count - 1])
				{
					SBGUIAtlasImage sbguiatlasImage = sbguivendorSlot.itemIcon;
					int? num = this.specialSlotIconSize;
					sbguiatlasImage.ScaleToMaxSize(num.Value);
				}
				else
				{
					SBGUIAtlasImage sbguiatlasImage2 = sbguivendorSlot.itemIcon;
					int? num2 = this.slotIconSize;
					sbguiatlasImage2.ScaleToMaxSize(num2.Value);
				}
			}
		}
		if (this.lastSelectedSlot != null)
		{
			VendingInstance vendingInstance2;
			vendingInstances.TryGetValue(this.lastSelectedSlot.SlotID, out vendingInstance2);
			if (vendingInstance2 == null)
			{
				vendingInstance2 = specialInstance;
			}
			VendorStock stock2 = session.TheGame.vendingManager.GetStock(vendingInstance2.StockId);
			this.UpdateItemDescription(session, stock2, vendingInstance2);
		}
		else
		{
			this.descriptionGroup.SetActive(false);
		}
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x0002BD58 File Offset: 0x00029F58
	public void HighlightSlot(Session session, SBGUIVendorSlot slot)
	{
		if (this.lastSelectedSlotID == slot.SlotID && this.lastSelectedSlot != null && this.lastSelectedSlot == slot)
		{
			return;
		}
		if (this.lastSelectedSlot != null)
		{
			this.lastSelectedSlot.SetHighlight(false, false);
		}
		this.lastSelectedSlot = slot;
		this.lastSelectedSlotID = slot.SlotID;
		if (this.lastSelectedSlot != null)
		{
			this.lastSelectedSlot.SetHighlight(true, false);
		}
		VendingInstance vendingInstance = session.TheGame.vendingManager.GetVendingInstance(this.vendingEntity.Id, slot.SlotID);
		if (vendingInstance == null)
		{
			vendingInstance = session.TheGame.vendingManager.GetSpecialInstance(this.vendingEntity.Id);
		}
		VendorStock stock = session.TheGame.vendingManager.GetStock(vendingInstance.StockId);
		this.UpdateItemDescription(session, stock, vendingInstance);
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x0002BE4C File Offset: 0x0002A04C
	public void UpdateItemDescription(Session session, VendorStock stock, VendingInstance instance)
	{
		if (!this.descriptionGroup.IsActive())
		{
			this.descriptionGroup.SetActive(true);
		}
		this.itemDescription.SetText(Language.Get(stock.Description));
		this.itemName.SetText(Language.Get(stock.Name));
		this.itemIcon.SetTextureFromAtlas(stock.Icon);
		SBGUIAtlasImage sbguiatlasImage = this.itemIcon;
		int? num = this.descriptionIconSize;
		sbguiatlasImage.ScaleToMaxSize(num.Value);
		this.stockLabel.SetText(string.Format(Language.Get("!!PREFAB_IN_STOCK"), instance.remaining));
		if (instance.Cost.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
		{
			int num2 = 0;
			instance.Cost.ResourceAmounts.TryGetValue(ResourceManager.SOFT_CURRENCY, out num2);
			this.itemCost.SetText(num2.ToString());
			this.itemCostIcon.SetTextureFromAtlas(session.TheGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].GetResourceTexture());
			SBGUIAtlasImage sbguiatlasImage2 = this.itemCostIcon;
			int? num3 = this.itemCostIconSize;
			sbguiatlasImage2.ScaleToMaxSize(num3.Value);
		}
		else if (instance.Cost.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
		{
			int num4 = 0;
			instance.Cost.ResourceAmounts.TryGetValue(ResourceManager.HARD_CURRENCY, out num4);
			this.itemCost.SetText(num4.ToString());
			this.itemCostIcon.SetTextureFromAtlas(session.TheGame.resourceManager.Resources[ResourceManager.HARD_CURRENCY].GetResourceTexture());
			SBGUIAtlasImage sbguiatlasImage3 = this.itemCostIcon;
			int? num5 = this.itemCostIconSize;
			sbguiatlasImage3.ScaleToMaxSize(num5.Value);
		}
		else
		{
			this.itemCost.SetText(Language.Get("!!PREFAB_FREE"));
			this.itemCostIcon.SetTextureFromAtlas("_blank_.png");
		}
		this.itemCostIcon.tform.localPosition = Vector3.zero + new Vector3(-((float)this.itemCost.Width + 5f) * 0.01f, 0f, 0f);
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x0002C080 File Offset: 0x0002A280
	public Vector2 GetRestockRushPosition()
	{
		Vector2 result = base.View.WorldToScreen(this.skipButton.transform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0002C0C4 File Offset: 0x0002A2C4
	public Vector2 GetBuyButtonPosition()
	{
		Vector2 result = base.View.WorldToScreen(this.buyButton.transform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0002C108 File Offset: 0x0002A308
	public override void Update()
	{
		this.restockTimer.SetText(TFUtils.DurationToString(this.vendingEntity.RestockTime - TFUtils.EpochTime()));
		base.Update();
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0002C140 File Offset: 0x0002A340
	public void ClearVendingSlots()
	{
		if (this.slotRefs != null)
		{
			int num = this.slotRefs.Count;
			for (int i = 0; i < num; i++)
			{
				SBGUIVendorSlot sbguivendorSlot = this.slotRefs[i];
				if (sbguivendorSlot.gameObject.name != "iod_slot")
				{
					if (sbguivendorSlot == this.lastSelectedSlot)
					{
						this.lastSelectedSlot = null;
					}
					UnityEngine.Object.Destroy(sbguivendorSlot.gameObject);
					this.slotRefs.RemoveAt(i);
					i--;
					num--;
				}
			}
		}
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x0002C1D4 File Offset: 0x0002A3D4
	public override void Deactivate()
	{
		this.ClearVendingSlots();
		base.Deactivate();
	}

	// Token: 0x0400052D RID: 1325
	private const float SLOT_GAP = 8f;

	// Token: 0x0400052E RID: 1326
	private const int SLOT_ROW_NUMBER_MAX = 4;

	// Token: 0x0400052F RID: 1327
	private const int MAX_VENDOR_SLOTS = 12;

	// Token: 0x04000530 RID: 1328
	private const float CURRENCY_ICON_GAP = 5f;

	// Token: 0x04000531 RID: 1329
	public int lastSelectedSlotID;

	// Token: 0x04000532 RID: 1330
	public SBGUIVendorSlot lastSelectedSlot;

	// Token: 0x04000533 RID: 1331
	private SBGUIButton skipButton;

	// Token: 0x04000534 RID: 1332
	private SBGUIElement slotMarker;

	// Token: 0x04000535 RID: 1333
	private List<SBGUIVendorSlot> slotRefs = new List<SBGUIVendorSlot>();

	// Token: 0x04000536 RID: 1334
	private SBGUILabel itemDescription;

	// Token: 0x04000537 RID: 1335
	private SBGUILabel itemName;

	// Token: 0x04000538 RID: 1336
	private SBGUILabel itemCost;

	// Token: 0x04000539 RID: 1337
	private SBGUILabel stockLabel;

	// Token: 0x0400053A RID: 1338
	private SBGUILabel restockTimer;

	// Token: 0x0400053B RID: 1339
	private SBGUIAtlasImage itemIcon;

	// Token: 0x0400053C RID: 1340
	private SBGUIAtlasImage itemCostIcon;

	// Token: 0x0400053D RID: 1341
	private SBGUIButton buyButton;

	// Token: 0x0400053E RID: 1342
	private SBGUIElement descriptionGroup;

	// Token: 0x0400053F RID: 1343
	private int? descriptionIconSize;

	// Token: 0x04000540 RID: 1344
	private int? slotIconSize;

	// Token: 0x04000541 RID: 1345
	private int? specialSlotIconSize;

	// Token: 0x04000542 RID: 1346
	private int? itemCostIconSize;

	// Token: 0x04000543 RID: 1347
	private VendingDecorator vendingEntity;

	// Token: 0x04000544 RID: 1348
	private SBGUICharacterArrowList m_pTaskCharacterList;
}
