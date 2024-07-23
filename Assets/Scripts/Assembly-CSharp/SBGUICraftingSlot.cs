using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class SBGUICraftingSlot : SBGUIScrollListElement
{
	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000461 RID: 1121 RVA: 0x0001BBC8 File Offset: 0x00019DC8
	// (set) Token: 0x06000462 RID: 1122 RVA: 0x0001BBD0 File Offset: 0x00019DD0
	public CraftingRecipe recipe { get; protected set; }

	// Token: 0x06000463 RID: 1123 RVA: 0x0001BBDC File Offset: 0x00019DDC
	public static SBGUICraftingSlot MakeCraftingSlot()
	{
		SBGUICraftingSlot sbguicraftingSlot = (SBGUICraftingSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CraftingSlot");
		sbguicraftingSlot.checkMark = (SBGUIAtlasImage)sbguicraftingSlot.FindChild("checkmark");
		sbguicraftingSlot.gameObject.transform.parent = GUIMainView.GetInstance().gameObject.transform;
		return sbguicraftingSlot;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001BC30 File Offset: 0x00019E30
	public static SBGUICraftingSlot Create(Session session, SBGUICraftingScreen craftingScreen, SBGUIElement anchor, CraftingCookbook cookbook, CraftingRecipe recipe, Vector3 offset, Action setSelected)
	{
		SBGUICraftingSlot sbguicraftingSlot = (SBGUICraftingSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CraftingSlot");
		sbguicraftingSlot.checkMark = (SBGUIAtlasImage)sbguicraftingSlot.FindChild("checkmark");
		sbguicraftingSlot.Setup(session, craftingScreen, anchor, cookbook, recipe, offset, setSelected);
		return sbguicraftingSlot;
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0001BC74 File Offset: 0x00019E74
	public void Setup(Session session, SBGUICraftingScreen craftingScreen, SBGUIElement anchor, CraftingCookbook cookbook, CraftingRecipe recipe, Vector3 offset, Action setSelected)
	{
		this.recipe = recipe;
		this.craftingScreen = craftingScreen;
		base.name = SBGUICraftingSlot.GetSessionActionId(recipe);
		this.SetParent(anchor);
		this.SetActive(true);
		base.transform.localPosition = offset;
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("icon");
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("name_label");
		this.numberOfProduct = (SBGUILabel)base.FindChild("number_label");
		SBGUIAtlasButton sbguiatlasButton = (SBGUIAtlasButton)base.FindChild("slot_background");
		sbguiatlasButton.SessionActionId = SBGUICraftingSlot.GetSessionActionId(recipe);
		sbguiatlasButton.SetTextureFromAtlas(cookbook.recipeSlotTexture);
		string text = Language.Get(recipe.recipeName);
		string text2 = text;
		if (text2.Length > 12)
		{
			text2 = text2.Substring(0, 9);
			text2 += "...";
		}
		sbguilabel.SetText(text2);
		this.resourceManager = session.TheGame.resourceManager;
		this.numberOfProduct.SetText(this.resourceManager.Query(recipe.productId).ToString());
		sbguiatlasImage.SetTextureFromAtlas(this.resourceManager.Resources[recipe.productId].GetResourceTexture());
		this.SetHighlight(false);
		base.AttachActionToButton("slot_background", delegate()
		{
			session.TheSoundEffectManager.PlaySound("HighlightItem");
			this.craftingScreen.HighlightSlot(session, recipe);
			setSelected();
		});
		sbguiatlasButton.UpdateCollider();
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001BE2C File Offset: 0x0001A02C
	public void SetHighlight(bool highlight)
	{
		this.checkMark.SetActive(highlight);
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0001BE3C File Offset: 0x0001A03C
	public override void Deactivate()
	{
		base.ClearButtonActions("slot_background");
		base.Deactivate();
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0001BE50 File Offset: 0x0001A050
	public static string GetSessionActionId(CraftingRecipe recipe)
	{
		return string.Format("Slot_{0}", recipe.productId);
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x0001BE68 File Offset: 0x0001A068
	public void Update()
	{
		if (this != null && this.numberOfProduct != null && this.recipe != null && this.resourceManager != null)
		{
			this.numberOfProduct.SetText(this.resourceManager.Query(this.recipe.productId).ToString());
		}
	}

	// Token: 0x04000369 RID: 873
	public const int GAP_SIZE = 6;

	// Token: 0x0400036A RID: 874
	public SBGUIAtlasImage checkMark;

	// Token: 0x0400036B RID: 875
	private SBGUICraftingScreen craftingScreen;

	// Token: 0x0400036C RID: 876
	private SBGUILabel numberOfProduct;

	// Token: 0x0400036D RID: 877
	private ResourceManager resourceManager;
}
