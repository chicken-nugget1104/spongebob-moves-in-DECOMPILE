using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class SBGUICraftingScreen : SBGUISlottedScrollableDialog
{
	// Token: 0x06000449 RID: 1097 RVA: 0x0001B0B8 File Offset: 0x000192B8
	public void Setup(Session session, CraftingCookbook cookbook, Action<int> rushHandler, int productionSlots)
	{
		TFUtils.Assert(session != null, "There should be a session. Found null.");
		this.session = session;
		this.SessionActionId = cookbook.sessionActionId;
		this.rushHandler = rushHandler;
		this.closeButton = base.FindChild("close").GetComponent<SBGUIAtlasButton>();
		TFUtils.Assert(this.closeButton != null, "Could not find child closeButton button on crafting screen!");
		this.closeButton.SessionActionId = cookbook.sessionActionId + "_Close_Button";
		this.recipeDialog = (SBGUICraftingRecipeDialog)base.FindChild("recipe_dialog");
		TFUtils.Assert(this.recipeDialog != null, "Couldn't find recipe_dialog");
		this.recipeDialog.Init();
		this.makeButtonLabel = (SBGUILabel)base.FindChild("button_label");
		this.m_pTaskCharacterList = (SBGUICharacterArrowList)base.FindChild("character_portrait_parent");
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0001B19C File Offset: 0x0001939C
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

	// Token: 0x0600044B RID: 1099 RVA: 0x0001B2C0 File Offset: 0x000194C0
	public void CreateUI(CraftingCookbook cookbook, CraftingRecipe highlightedRecipe, int unlockedSlots, int maxSlots, Action<CraftingRecipe> setSelected)
	{
		base.PreLoadRegionContentInfo();
		this.SessionActionId = cookbook.sessionActionId;
		this.closeButton.SessionActionId = cookbook.sessionActionId + "_Close_Button";
		this.SetupProductionSlots(this.session, unlockedSlots, maxSlots);
		List<int> list = this.session.TheGame.craftManager.UnlockedRecipesCopy.Intersect(cookbook.GetRecipes()).ToList<int>();
		list = this.session.TheGame.resourceManager.SortRecipesByProductGroup(this.session.TheGame.craftManager, list);
		Action<CraftingRecipe> value = delegate(CraftingRecipe recipe)
		{
			this.UpdateProductionSlots();
		};
		this.MakeRecipeClickedEvent.ClearListeners();
		this.MakeRecipeClickedEvent.AddListener(value);
		this.UpdateProductionSlots();
		if (this.highlightedRecipe != highlightedRecipe)
		{
			this.highlightedRecipe = highlightedRecipe;
		}
		this.currentCookbook = cookbook.identity;
		this.LoadRecipes(list, this.session, cookbook, this.region.Marker, setSelected);
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x0001B3BC File Offset: 0x000195BC
	public void HighlightSlot(Session session, CraftingRecipe recipe)
	{
		this.highlightedRecipe = recipe;
		if (recipe == null)
		{
			this.recipeDialog.Deselect();
			return;
		}
		SBGUICraftingSlot sbguicraftingSlot = (SBGUICraftingSlot)this.region.subViewMarker.FindChild(SBGUICraftingSlot.GetSessionActionId(recipe));
		if (this.selectedSlot != null)
		{
			this.selectedSlot.SetHighlight(false);
		}
		this.selectedSlot = sbguicraftingSlot;
		if (this.selectedSlot != null)
		{
			this.selectedSlot.SetHighlight(true);
			this.lastSelectedByCookbook[this.currentCookbook] = this.selectedSlot.transform.localPosition.x;
		}
		this.recipeDialog.Setup(recipe, session.TheGame.resourceManager);
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0001B480 File Offset: 0x00019680
	protected override void OnSlotsVisible()
	{
		this.HighlightSlot(this.session, this.highlightedRecipe);
		base.OnSlotsVisible();
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0001B49C File Offset: 0x0001969C
	protected override int GetSlotIndex(Vector2 pos)
	{
		float num = this.GetSlotSize().x + 0.06f;
		float num2 = -num;
		int num3 = Mathf.FloorToInt(pos.x * 2f / num + num2);
		if (pos.y > 0f)
		{
			num3++;
		}
		return num3;
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0001B4F0 File Offset: 0x000196F0
	protected override Vector2 GetSlotOffset(int index)
	{
		Vector2 slotSize = this.GetSlotSize();
		return new Vector2(slotSize.x * ((float)(index / 2) + 0.06f), -1f * (slotSize.y * (float)(index % 2)));
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x0001B530 File Offset: 0x00019730
	protected override Vector2 GetSlotSize()
	{
		SBGUICraftingSlot component = this.rowPrefab.GetComponent<SBGUICraftingSlot>();
		SBGUIAtlasButton sbguiatlasButton = (SBGUIAtlasButton)component.FindChild("slot_background");
		return sbguiatlasButton.Size * 0.01f;
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x0001B56C File Offset: 0x0001976C
	private void LoadRecipes(List<int> recipes, Session session, CraftingCookbook cookbook, SBGUIElement anchor, Action<CraftingRecipe> setSelected)
	{
		int num = 0;
		this.region.SetupSlotActions.Clear();
		List<int> list = new List<int>();
		foreach (int num2 in recipes)
		{
			if (!list.Contains(num2))
			{
				list.Add(num2);
				CraftingRecipe recipeById = session.TheGame.craftManager.GetRecipeById(num2);
				if (recipeById == null)
				{
					TFUtils.WarningLog("null CraftingRecipe");
				}
				else
				{
					Action<CraftingRecipe> setSelected2 = delegate(CraftingRecipe recipeToSelect)
					{
						setSelected(recipeToSelect);
					};
					this.region.SetupSlotActions.Insert(num, this.SetupSlotClosure(session, anchor, cookbook, recipeById, this.GetSlotOffset(num), num, setSelected2));
					string sessionActionId = SBGUICraftingSlot.GetSessionActionId(recipeById);
					if (this.sessionActionIdSearchRequests.Contains(sessionActionId))
					{
						this.sessionActionSlotMap[sessionActionId] = num;
					}
					num++;
				}
			}
		}
		Vector3 zero = Vector3.zero;
		if (this.lastSelectedByCookbook.ContainsKey(cookbook.identity))
		{
			float num3 = this.lastSelectedByCookbook[cookbook.identity];
			float num4 = num3 - this.region.InitialMarkerPos.x;
			zero = new Vector3(-num4, this.region.InitialMarkerPos.y, this.region.InitialMarkerPos.z);
		}
		base.PostLoadRegionContentInfo(this.region.SetupSlotActions.Count, zero);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0001B72C File Offset: 0x0001992C
	public override void Deactivate()
	{
		base.ClearButtonActions("accept_button");
		SBGUICraftingScreen.prodSlotPool.Clear(delegate(SBGUIProductionSlot slot)
		{
			slot.Deactivate();
		});
		this.productionSlotShells = null;
		this.selectedSlot = null;
		this.recipeDialog.Deactivate();
		this.MakeRecipeClickedEvent.ClearListeners();
		base.Deactivate();
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0001B798 File Offset: 0x00019998
	public void ForceCycleProdSlots()
	{
		SBGUICraftingScreen.prodSlotPool.Clear(delegate(SBGUIProductionSlot slot)
		{
			slot.Deactivate();
		});
		this.productionSlotShells = null;
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0001B7D4 File Offset: 0x000199D4
	public override void Update()
	{
		base.Update();
		this.UpdateProductionSlots();
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0001B7E4 File Offset: 0x000199E4
	public void UpdateResources(Session session)
	{
		if (this.selectedSlot != null && this.recipeDialog != null)
		{
			this.recipeDialog.Setup(this.selectedSlot.recipe, session.TheGame.resourceManager);
		}
		this.UpdateProductionSlots();
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x0001B83C File Offset: 0x00019A3C
	public Vector2 GetHardSpendButtonPositionForSlot(int slotId)
	{
		return this.productionSlotShells.Find((ProductionSlotShell shell) => shell.SlotId == slotId).Position;
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0001B874 File Offset: 0x00019A74
	private void UpdateProductionSlots()
	{
		if (this.session == null || this.session.TheGame == null || this.productionSlotShells == null)
		{
			return;
		}
		TFUtils.Assert(this.session.TheGame.selected != null, "There should be a selected entity. Found null.");
		BuildingEntity entity = this.session.TheGame.selected.GetEntity<BuildingEntity>();
		TFUtils.Assert(entity.CanCraft, "Should not be viewing production slots on a non-craftable building");
		if (entity.ShuntsCrafting)
		{
			List<Entity> annexes = entity.Annexes;
			int i;
			for (i = 0; i < annexes.Count; i++)
			{
				this.productionSlotShells[i].UpdateInfo(entity, i, this.rushHandler, this.session.TheGame);
			}
			while (i < this.productionSlotShells.Count)
			{
				this.productionSlotShells[i].UpdateInfo(null, 0, null, null);
				i++;
			}
		}
		else
		{
			for (int j = 0; j < this.productionSlotShells.Count; j++)
			{
				this.productionSlotShells[j].UpdateInfo(entity, j, this.rushHandler, this.session.TheGame);
			}
		}
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0001B9B0 File Offset: 0x00019BB0
	protected override SBGUIScrollListElement MakeSlot()
	{
		return SBGUICraftingSlot.MakeCraftingSlot();
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0001B9B8 File Offset: 0x00019BB8
	private void SetupProductionSlots(Session session, int availableSlots, int unlockableSlots)
	{
		SBGUIElement sbguielement = base.FindChild("production_slots");
		SBGUIElement parent = sbguielement.FindChild("anchor");
		int definitionId = session.TheGame.selected.entity.DefinitionId;
		this.productionSlotShells = new List<ProductionSlotShell>(Math.Min(availableSlots, unlockableSlots));
		TFUtils.Assert(availableSlots <= 7, "We do not have screen space for more than " + 7 + " crafting production slots");
		float num = 0f;
		for (int i = 0; i < 7; i++)
		{
			SBGUIProductionSlot sbguiproductionSlot = SBGUICraftingScreen.prodSlotPool.Create(new Alloc<SBGUIProductionSlot>(SBGUIProductionSlot.Create));
			sbguiproductionSlot.SetParent(parent);
			ProductionSlotShell item;
			if (i < availableSlots)
			{
				item = new ProductionSlotShellAvailable(sbguiproductionSlot, i);
			}
			else if (i < unlockableSlots)
			{
				item = new ProductionSlotShellLocked(sbguiproductionSlot, session.TheGame.craftManager.GetSlotExpandCost(definitionId, i), i, session.TheGame);
			}
			else
			{
				item = new ProductionSlotShellUnavailable(sbguiproductionSlot, i);
			}
			this.productionSlotShells.Add(item);
			sbguiproductionSlot.transform.localPosition += new Vector3(num, 0f, 0f);
			num += 0.955f;
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0001BAF4 File Offset: 0x00019CF4
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0001BAFC File Offset: 0x00019CFC
	private Action<SBGUIScrollListElement> SetupSlotClosure(Session session, SBGUIElement anchor, CraftingCookbook cookbook, CraftingRecipe recipe, Vector3 offset, int slotId, Action<CraftingRecipe> setSelected)
	{
		Action action = delegate()
		{
			setSelected(recipe);
		};
		return delegate(SBGUIScrollListElement slot)
		{
			((SBGUICraftingSlot)slot).Setup(session, this, anchor, cookbook, recipe, offset, action);
		};
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x0001BB64 File Offset: 0x00019D64
	public Vector2 GetHardSpendPosition()
	{
		Vector2 result = base.View.WorldToScreen(this.makeButtonLabel.transform.position);
		result.y = (float)Screen.height - result.y;
		return result;
	}

	// Token: 0x04000355 RID: 853
	private const int NUM_ROWS = 2;

	// Token: 0x04000356 RID: 854
	private const int MAX_SLOTS = 7;

	// Token: 0x04000357 RID: 855
	private const float SLOT_DISPLACEMENT = 0.955f;

	// Token: 0x04000358 RID: 856
	private const float GAP_SIZE = 0.06f;

	// Token: 0x04000359 RID: 857
	public GameObject rowPrefab;

	// Token: 0x0400035A RID: 858
	public EventDispatcher<CraftingRecipe> MakeRecipeClickedEvent = new EventDispatcher<CraftingRecipe>();

	// Token: 0x0400035B RID: 859
	public SBGUICraftingSlot selectedSlot;

	// Token: 0x0400035C RID: 860
	public SBGUIAtlasButton closeButton;

	// Token: 0x0400035D RID: 861
	private static TFPool<SBGUIProductionSlot> prodSlotPool = new TFPool<SBGUIProductionSlot>();

	// Token: 0x0400035E RID: 862
	private SBGUICraftingRecipeDialog recipeDialog;

	// Token: 0x0400035F RID: 863
	private List<ProductionSlotShell> productionSlotShells;

	// Token: 0x04000360 RID: 864
	private Action<int> rushHandler;

	// Token: 0x04000361 RID: 865
	private CraftingRecipe highlightedRecipe;

	// Token: 0x04000362 RID: 866
	private int highlightedSlot;

	// Token: 0x04000363 RID: 867
	private int currentCookbook;

	// Token: 0x04000364 RID: 868
	private SBGUILabel makeButtonLabel;

	// Token: 0x04000365 RID: 869
	private Dictionary<int, float> lastSelectedByCookbook = new Dictionary<int, float>();

	// Token: 0x04000366 RID: 870
	private SBGUICharacterArrowList m_pTaskCharacterList;
}
