using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public abstract class ProductionSlotShell
{
	// Token: 0x0600034D RID: 845 RVA: 0x000108A4 File Offset: 0x0000EAA4
	public ProductionSlotShell(SBGUIProductionSlot core, int slotId)
	{
		this.slotId = slotId;
		this.core = core;
		this.core.SetActive(true);
		this.core.icon.SetTextureFromAtlas("empty.png");
		this.core.background.SetVisible(false);
		this.core.label.Text = string.Empty;
		this.core.rushButton.SessionActionId = string.Empty;
		this.core.rushButton.SetVisible(false);
		this.core.rushCostLabel.Text = string.Empty;
		this.core.transform.localPosition = Vector3.zero;
		this.activated = false;
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x0600034E RID: 846 RVA: 0x00010964 File Offset: 0x0000EB64
	public int SlotId
	{
		get
		{
			return this.slotId;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600034F RID: 847 RVA: 0x0001096C File Offset: 0x0000EB6C
	public Vector2 Position
	{
		get
		{
			return this.core.GetScreenPosition();
		}
	}

	// Token: 0x06000350 RID: 848
	public abstract void UpdateInfo(BuildingEntity producer, int slot, Action<int> rushHandler, Game game);

	// Token: 0x04000234 RID: 564
	protected SBGUIProductionSlot core;

	// Token: 0x04000235 RID: 565
	protected bool activated;

	// Token: 0x04000236 RID: 566
	private int slotId;
}
