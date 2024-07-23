using System;

// Token: 0x02000053 RID: 83
public class ProductionSlotShellUnavailable : ProductionSlotShell
{
	// Token: 0x06000355 RID: 853 RVA: 0x00010F4C File Offset: 0x0000F14C
	public ProductionSlotShellUnavailable(SBGUIProductionSlot core, int slotId) : base(core, slotId)
	{
		this.core.rushButton.SetVisible(false);
		this.core.background.SetTextureFromAtlas("StoreLock.png");
		this.core.background.SetVisible(true);
		this.activated = false;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00010FA0 File Offset: 0x0000F1A0
	public override void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game)
	{
	}
}
