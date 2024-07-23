using System;

// Token: 0x02000052 RID: 82
public class ProductionSlotShellLocked : ProductionSlotShell
{
	// Token: 0x06000353 RID: 851 RVA: 0x00010D98 File Offset: 0x0000EF98
	public ProductionSlotShellLocked(SBGUIProductionSlot core, Cost purchaseCost, int slotId, Game game) : base(core, slotId)
	{
		this.core.background.SetTextureFromAtlas("StoreLock.png");
		this.core.background.SetVisible(true);
		bool flag = game.featureManager.CheckFeature("allow_production_slot_purchase");
		if (flag)
		{
			this.core.rushButton.SetVisible(true);
			this.core.rushButton.SetActive(true);
			this.core.rushCostLabel.Text = purchaseCost.ResourceAmounts[ResourceManager.HARD_CURRENCY].ToString();
		}
		this.activated = false;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00010E3C File Offset: 0x0000F03C
	public override void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game)
	{
		if (!this.activated)
		{
			Action rushButtonHandler = null;
			rushButtonHandler = delegate()
			{
				rushHandler(slot);
				this.core.rushButton.ClickEvent -= rushButtonHandler;
			};
			this.core.ClearButtonActions(this.core.rushButton.name);
			bool flag = game.featureManager.CheckFeature("allow_production_slot_purchase");
			if (flag)
			{
				this.core.AttachActionToButton(this.core.rushButton.name, rushButtonHandler);
				this.core.rushButton.SetActive(true);
			}
			this.core.rushButton.SessionActionId = string.Concat(new object[]
			{
				"ProductionRush_",
				slot,
				"_",
				producer.BlueprintName
			});
			this.activated = true;
		}
	}
}
