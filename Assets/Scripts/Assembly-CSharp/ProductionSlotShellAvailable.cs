using System;

// Token: 0x02000051 RID: 81
public class ProductionSlotShellAvailable : ProductionSlotShell
{
	// Token: 0x06000351 RID: 849 RVA: 0x0001097C File Offset: 0x0000EB7C
	public ProductionSlotShellAvailable(SBGUIProductionSlot core, int slotId) : base(core, slotId)
	{
		this.core.rushButton.SetVisible(true);
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00010998 File Offset: 0x0000EB98
	public override void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game)
	{
		CraftingInstance craftingInstance = null;
		if (producer != null)
		{
			if (producer.ShuntsCrafting)
			{
				craftingInstance = game.craftManager.GetCraftingInstance(producer.Annexes[slot].Id, 0);
			}
			else
			{
				craftingInstance = game.craftManager.GetCraftingInstance(producer.Id, slot);
			}
		}
		if (craftingInstance == null || craftingInstance.rushed)
		{
			this.core.label.Text = string.Empty;
			this.core.rushButton.SessionActionId = string.Empty;
			this.core.rushCostLabel.Text = string.Empty;
			this.core.rushButton.SetActive(false);
			this.core.icon.SetTextureFromAtlas("empty.png");
			this.core.background.SetVisible(false);
			this.core.ClearButtonActions(this.core.rushButton.name);
			this.activated = false;
		}
		else if (this.activated)
		{
			CraftingRecipe recipeById = game.craftManager.GetRecipeById(craftingInstance.recipeId);
			float num = (float)Math.Max(0.0, craftingInstance.ReadyTimeFromNow / recipeById.craftTime);
			if (num < 0f)
			{
				this.core.label.Text = string.Empty;
				this.core.rushButton.SessionActionId = string.Empty;
				this.core.rushCostLabel.Text = string.Empty;
				this.core.rushButton.SetActive(false);
				this.core.icon.SetTextureFromAtlas("empty.png");
				this.core.background.SetVisible(false);
				this.core.ClearButtonActions(this.core.rushButton.name);
				this.activated = false;
			}
			Cost cost = Cost.Prorate(recipeById.rushCost, num);
			this.core.label.Text = TFUtils.DurationToString(craftingInstance.ReadyTimeFromNow);
			this.core.rushCostLabel.Text = cost.ResourceAmounts[ResourceManager.HARD_CURRENCY].ToString();
		}
		else
		{
			CraftingRecipe recipeById2 = game.craftManager.GetRecipeById(craftingInstance.recipeId);
			int num2 = recipeById2.rushCost.ResourceAmounts[ResourceManager.HARD_CURRENCY];
			string resourceTexture = game.resourceManager.Resources[recipeById2.productId].GetResourceTexture();
			TFUtils.Assert(resourceTexture != null, "This craft rewards thought icon is null! Need to know what to show!");
			this.core.label.Text = TFUtils.DurationToString(craftingInstance.ReadyTimeFromNow);
			this.core.icon.SetTextureFromAtlas(resourceTexture);
			this.core.background.SetTextureFromAtlas("ProductionSlotInUse.png");
			this.core.background.SetVisible(true);
			Action rushButtonHandler = null;
			rushButtonHandler = delegate()
			{
				rushHandler(slot);
				this.core.rushButton.ClickEvent -= rushButtonHandler;
			};
			this.core.rushButton.SetActive(true);
			this.core.rushButton.SessionActionId = string.Concat(new object[]
			{
				"ProductionRush_",
				slot,
				"_",
				producer.BlueprintName
			});
			this.core.ClearButtonActions(this.core.rushButton.name);
			this.core.AttachActionToButton(this.core.rushButton.name, rushButtonHandler);
			this.core.rushCostLabel.Text = num2.ToString();
			this.activated = true;
		}
	}
}
