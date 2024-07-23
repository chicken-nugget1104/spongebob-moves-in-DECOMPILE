using System;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class SBGUIQuestLineDialog : SBGUIModalDialog
{
	// Token: 0x060005B3 RID: 1459 RVA: 0x0002486C File Offset: 0x00022A6C
	protected override void Awake()
	{
		this.rewardWindow = base.FindChild("reward_window");
		base.Awake();
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x00024888 File Offset: 0x00022A88
	public override void SetParent(SBGUIElement element)
	{
		base.SetTransformParent(element);
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x00024894 File Offset: 0x00022A94
	public void SetupQuestLineDialogInfo(string dialogHeading, string dialogBody, string portrait, string rewardTexture, string rewardName)
	{
		SBGUIShadowedLabel sbguishadowedLabel = (SBGUIShadowedLabel)base.FindChild("dialog_heading");
		SBGUIShadowedLabel sbguishadowedLabel2 = (SBGUIShadowedLabel)base.FindChild("banner_label");
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("dialog_body");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("portrait");
		SBGUIAtlasImage sbguiatlasImage2 = (SBGUIAtlasImage)base.FindChild("portrait_shadow");
		SBGUIAtlasImage boundary = (SBGUIAtlasImage)base.FindChild("dialog_body_boundary");
		SBGUIAtlasImage sbguiatlasImage3 = (SBGUIAtlasImage)base.FindChild("reward_icon");
		int? num = this.prefabIconSize;
		if (num == null)
		{
			this.prefabIconSize = new int?((int)sbguiatlasImage3.Size.x);
		}
		sbguishadowedLabel.SetText(Language.Get(dialogHeading));
		sbguilabel.SetText(Language.Get(dialogBody));
		sbguilabel.AdjustText(boundary);
		if (rewardName != string.Empty)
		{
			string text = string.Format(Language.Get("!!EARNED_A_THING_DIALOG"), rewardName);
			text = sbguishadowedLabel2.textSprite.StripScalarDataFromString(text, false);
			sbguishadowedLabel2.SetText(text);
		}
		else
		{
			string text2 = Language.Get("!!YOU_WILL_EARN_A_THING_DIALOG");
			text2 = sbguishadowedLabel2.textSprite.StripScalarDataFromString(text2, false);
			sbguishadowedLabel2.SetText(text2);
		}
		sbguiatlasImage.SetTextureFromAtlas(portrait, true, false, true, false, false, 0);
		sbguiatlasImage2.renderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0.2f));
		sbguiatlasImage3.SetTextureFromAtlas(rewardTexture, true, false, true, false, false, 0);
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00024A24 File Offset: 0x00022C24
	public void ToggleRewardWindow(bool enabled)
	{
		this.rewardWindow.SetActive(enabled);
	}

	// Token: 0x0400045F RID: 1119
	private SBGUIElement rewardWindow;

	// Token: 0x04000460 RID: 1120
	private int? prefabIconSize;
}
