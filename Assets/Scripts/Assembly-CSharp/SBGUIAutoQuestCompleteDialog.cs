using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class SBGUIAutoQuestCompleteDialog : SBGUIScreen
{
	// Token: 0x060003C7 RID: 967 RVA: 0x00013E10 File Offset: 0x00012010
	public void SetupDialogInfo(string sDialogHeading, string sDialogBody, string sPortrait, List<Reward> pRewards, QuestDefinition pQuestDef)
	{
		SBGUIShadowedLabel sbguishadowedLabel = (SBGUIShadowedLabel)base.FindChild("dialog_heading");
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("dialog_body");
		SBGUIAtlasImage boundary = (SBGUIAtlasImage)base.FindChild("dialog_body_boundary");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("portrait");
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)base.FindChild("portrait_shadow");
		SBGUILabel sbguilabel2 = (SBGUILabel)base.FindChild("reward_label");
		SBGUILabel sbguilabel3 = (SBGUILabel)base.FindChild("reward_gold_label");
		SBGUILabel sbguilabel4 = (SBGUILabel)base.FindChild("reward_xp_label");
		this.window = (SBGUIAtlasImage)base.FindChild("window");
		this.okayButton = (SBGUIPulseButton)base.FindChild("okay");
		int num = 0;
		int num2 = 0;
		int count = pRewards.Count;
		for (int i = 0; i < count; i++)
		{
			Reward reward = pRewards[i];
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
			{
				num += reward.ResourceAmounts[ResourceManager.SOFT_CURRENCY];
			}
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.XP))
			{
				num2 += reward.ResourceAmounts[ResourceManager.XP];
			}
		}
		string text = Language.Get(sDialogBody);
		if (text.Contains("{0}") && pQuestDef.AutoQuestCharacterID >= 0)
		{
			Simulated simulated = this.session.TheGame.simulation.FindSimulated(new int?(pQuestDef.AutoQuestCharacterID));
			if (simulated != null && simulated.HasEntity<ResidentEntity>())
			{
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				text = string.Format(text, Language.Get(entity.Name));
			}
		}
		sbguilabel3.SetText(num.ToString());
		sbguilabel4.SetText(num2.ToString());
		sbguishadowedLabel.SetText(Language.Get(sDialogHeading));
		sbguilabel.SetText(text);
		sbguilabel.AdjustText(boundary);
		sbguilabel2.SetText(Language.Get("!!PREFAB_REWARD") + ":");
		sbguiatlasImage.SetTextureFromAtlas(sPortrait, true, false, false, false, false, 0);
		sbguiatlasImage2.renderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.2f));
	}

	// Token: 0x04000279 RID: 633
	private SBGUIPulseButton okayButton;

	// Token: 0x0400027A RID: 634
	private SBGUIAtlasImage window;
}
