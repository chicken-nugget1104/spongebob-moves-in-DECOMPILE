using System;

// Token: 0x02000095 RID: 149
public class SBGUIProgressDialog : SBGUIModalDialog
{
	// Token: 0x0600058F RID: 1423 RVA: 0x00023CD8 File Offset: 0x00021ED8
	protected override void Awake()
	{
		this.rewardMarker = base.FindChild("reward_marker");
		this.meter = (SBGUIProgressMeter)base.FindChild("progress_meter");
		this.durationLabel = (SBGUILabel)base.FindChild("duration_label");
		base.Awake();
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x00023D28 File Offset: 0x00021F28
	public void Setup(string title, string description, Action onClose)
	{
		Cost cost = new Cost();
		cost.ResourceAmounts[0] = 0;
		this.Setup(title, description, onClose, false, cost, null);
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00023D54 File Offset: 0x00021F54
	public void Setup(string title, string description, Action onClose, bool rewardVisible, Cost rushCost, Action onRush)
	{
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("building_label");
		SBGUILabel sbguilabel2 = (SBGUILabel)base.FindChild("task_label");
		sbguilabel.SetText(title);
		sbguilabel2.SetText(description);
		SBGUIElement sbguielement = base.FindChild("close");
		SBGUIButton component = sbguielement.GetComponent<SBGUIButton>();
		this.AttachAnalyticsToButton("close", component);
		component.ClickEvent += delegate()
		{
			this.Close();
		};
		component.ClickEvent += onClose;
		SBGUIElement sbguielement2 = base.FindChild("rush_button");
		if (onRush == null)
		{
			sbguielement2.SetActive(false);
		}
		else
		{
			SBGUIButton component2 = sbguielement2.GetComponent<SBGUIButton>();
			this.AttachAnalyticsToButton("rush", component2);
			component2.ClickEvent += delegate()
			{
				this.Close();
			};
			component2.ClickEvent += onRush;
			this.rushLabel = (SBGUILabel)base.FindChild("rush_cost_label");
			this.rushLabel.SetText(rushCost.ResourceAmounts[rushCost.GetOnlyCostKey()].ToString());
			this.maxJellyCost = rushCost.ResourceAmounts[rushCost.GetOnlyCostKey()];
		}
		if (!rewardVisible)
		{
			SBGUIElement sbguielement3 = base.FindChild("reward");
			sbguielement3.SetActive(false);
		}
	}

	// Token: 0x0400044D RID: 1101
	private SBGUIProgressMeter meter;

	// Token: 0x0400044E RID: 1102
	private SBGUILabel durationLabel;

	// Token: 0x0400044F RID: 1103
	private SBGUILabel rushLabel;

	// Token: 0x04000450 RID: 1104
	private int maxJellyCost = 1;
}
