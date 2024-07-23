using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarg;

// Token: 0x02000087 RID: 135
[RequireComponent(typeof(TapButton))]
[RequireComponent(typeof(YG2DRectangle))]
public class SBGUIInventoryWidgetTab : SBGUIButton
{
	// Token: 0x06000520 RID: 1312 RVA: 0x00020620 File Offset: 0x0001E820
	public void Setup(Game game, CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr, SoundEffectManager sfxMgr, Action<YGEvent> onUiEventCallback, Action<int, YGEvent> onDragCallback, SBGUIInventoryWidgetTab.GetNewRow nextRowDelegate, SBGUIInventoryWidgetTab.CloseRows closeRowsCallback, SBGUIInventoryWidgetTab.OpenAllTabs openAllTabsCallback)
	{
		TFUtils.Assert(this != null, "This Inventory Widget Tab Unity Object has not yet been able to set itself up.");
		this.nextRowDelegate = nextRowDelegate;
		this.closeRowsCallback = closeRowsCallback;
		this.openAllTabsCallback = openAllTabsCallback;
		this.openRows = delegate(bool closeFirst)
		{
			if (closeFirst)
			{
				this.InternalClose();
			}
			this.Open(resourceMgr, craftMgr, sfxMgr, onUiEventCallback, onDragCallback);
		};
		Action value = delegate()
		{
			if (this.rowsVisible)
			{
				sfxMgr.PlaySound("CloseIngredientWidget");
				SBGUIStandardScreen.userClosedWishList = true;
				this.InternalClose();
				AnalyticsWrapper.LogUIInteraction(game, "ui_hide_wishes", "button", "tap");
			}
			else
			{
				sfxMgr.PlaySound("OpenIngredientWidget");
				SBGUIStandardScreen.userClosedWishList = false;
				openAllTabsCallback();
				AnalyticsWrapper.LogUIInteraction(game, "ui_display_wishes", "button", "tap");
			}
		};
		this.button.TapEvent.AddListener(value);
		this.UpdateRecipes(craftMgr, vendingMgr, resourceMgr);
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x000206E0 File Offset: 0x0001E8E0
	public bool ActivateTab(bool closeExisting)
	{
		bool result = !this.rowsVisible;
		this.openRows(closeExisting);
		return result;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00020704 File Offset: 0x0001E904
	public bool TryActivateTab(int cookbookId, bool isVendor, bool closeExisting)
	{
		if (this.CookbookId == cookbookId && isVendor == this.isVendor)
		{
			this.ActivateTab(closeExisting);
		}
		return false;
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00020728 File Offset: 0x0001E928
	public void UpdateRecipes(CraftingManager craftMgr, VendingManager vendingMgr, ResourceManager resourceMgr)
	{
		this.productsMade.Clear();
		int[] array = null;
		if (this.isVendor)
		{
			VendorDefinition vendorDefinition = vendingMgr.GetVendorDefinition(this.CookbookId);
			if (vendorDefinition != null)
			{
				List<int> generalStock = vendorDefinition.generalStock;
				generalStock.AddRange(vendorDefinition.specialStock);
				array = craftMgr.UnlockedRecipesCopy.Intersect(generalStock).ToArray<int>();
			}
		}
		else
		{
			array = craftMgr.UnlockedRecipesCopy.Intersect(craftMgr.GetCookbookById(this.CookbookId).GetRecipes()).ToArray<int>();
		}
		if (array != null)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				int productId = craftMgr.GetRecipeById(array[i]).productId;
				if (!this.productsMade.Contains(productId))
				{
					this.productsMade.Add(productId);
				}
			}
		}
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x000207FC File Offset: 0x0001E9FC
	public void Close()
	{
		this.rowsVisible = false;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00020808 File Offset: 0x0001EA08
	private void Open(ResourceManager resourceMgr, CraftingManager craftMgr, SoundEffectManager sfxMgr, Action<YGEvent> onUiEventCallback, Action<int, YGEvent> onDragCallback)
	{
		TFUtils.Assert(resourceMgr != null, "Must call Setup before opening!");
		List<int> consumables = resourceMgr.ConsumableProducts(craftMgr);
		List<int> list = (from pid in this.productsMade
		where consumables.Contains(pid)
		select pid).ToList<int>();
		foreach (int num in list)
		{
			SBGUIInventoryWidgetRow sbguiinventoryWidgetRow = this.nextRowDelegate(this.Row, this.CookbookId, this.isVendor);
			string resourceTexture = resourceMgr.Resources[num].GetResourceTexture();
			TFUtils.Assert(resourceTexture != null && resourceTexture != string.Empty, "Did not find texture for resource with resourceId=" + num);
			sbguiinventoryWidgetRow.Initialize(sfxMgr, onUiEventCallback, onDragCallback, resourceTexture);
			sbguiinventoryWidgetRow.SetProductToTrack(num);
		}
		this.rowsVisible = true;
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00020920 File Offset: 0x0001EB20
	private void InternalClose()
	{
		this.closeRowsCallback();
		this.rowsVisible = false;
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00020934 File Offset: 0x0001EB34
	public override void MockClick()
	{
		this.openAllTabsCallback();
	}

	// Token: 0x040003E4 RID: 996
	public int CookbookId;

	// Token: 0x040003E5 RID: 997
	public bool isVendor;

	// Token: 0x040003E6 RID: 998
	public SBGUIInventoryWidgetRow Row;

	// Token: 0x040003E7 RID: 999
	public Mesh RowMesh;

	// Token: 0x040003E8 RID: 1000
	private List<int> productsMade = new List<int>();

	// Token: 0x040003E9 RID: 1001
	private SBGUIInventoryWidgetTab.GetNewRow nextRowDelegate;

	// Token: 0x040003EA RID: 1002
	private SBGUIInventoryWidgetTab.CloseRows closeRowsCallback;

	// Token: 0x040003EB RID: 1003
	private SBGUIInventoryWidgetTab.OpenAllTabs openAllTabsCallback;

	// Token: 0x040003EC RID: 1004
	private Action<bool> openRows;

	// Token: 0x040003ED RID: 1005
	private bool rowsVisible;

	// Token: 0x02000492 RID: 1170
	// (Invoke) Token: 0x060024AB RID: 9387
	public delegate SBGUIInventoryWidgetRow GetNewRow(SBGUIInventoryWidgetRow tabRow, int fromCookbookId, bool fromIsVendor);

	// Token: 0x02000493 RID: 1171
	// (Invoke) Token: 0x060024AF RID: 9391
	public delegate void CloseRows();

	// Token: 0x02000494 RID: 1172
	// (Invoke) Token: 0x060024B3 RID: 9395
	public delegate bool OpenAllTabs();
}
