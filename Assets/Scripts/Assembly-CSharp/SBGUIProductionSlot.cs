using System;

// Token: 0x02000093 RID: 147
public class SBGUIProductionSlot : SBGUIElement
{
	// Token: 0x0600058A RID: 1418 RVA: 0x00023B5C File Offset: 0x00021D5C
	public new static SBGUIProductionSlot Create()
	{
		SBGUIProductionSlot sbguiproductionSlot = (SBGUIProductionSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/CraftProductionSlot");
		sbguiproductionSlot.gameObject.name = "ProductionSlot";
		return sbguiproductionSlot;
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x00023B8C File Offset: 0x00021D8C
	protected override void Awake()
	{
		this.label = base.FindChild("time_label").GetComponent<SBGUILabel>();
		this.icon = base.FindChild("icon").GetComponent<SBGUIAtlasImage>();
		this.background = base.FindChild("background").GetComponent<SBGUIAtlasImage>();
		this.rushButton = base.FindChild("rush_button").GetComponent<SBGUIAtlasButton>();
		this.rushCostLabel = base.FindChild("rush_cost_label").GetComponent<SBGUILabel>();
		TFUtils.Assert(this.label != null, "Could not find child label on production slot!");
		TFUtils.Assert(this.icon != null, "Could not find child icon on production slot!");
		TFUtils.Assert(this.rushButton != null, "Could not find child rush button on production slot!");
		TFUtils.Assert(this.rushCostLabel != null, "Could not find rush cost label on production slot!");
		this.label.Text = string.Empty;
		this.icon.SetTextureFromAtlas("empty.png");
		this.background.SetVisible(false);
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00023C8C File Offset: 0x00021E8C
	public void Deactivate()
	{
		this.MuteButtons(false);
		this.SetParent(null);
		this.SetActive(false);
		base.ClearButtonActions(this.rushButton.name);
	}

	// Token: 0x04000448 RID: 1096
	public SBGUILabel label;

	// Token: 0x04000449 RID: 1097
	public SBGUIAtlasImage icon;

	// Token: 0x0400044A RID: 1098
	public SBGUIAtlasImage background;

	// Token: 0x0400044B RID: 1099
	public SBGUIAtlasButton rushButton;

	// Token: 0x0400044C RID: 1100
	public SBGUILabel rushCostLabel;
}
