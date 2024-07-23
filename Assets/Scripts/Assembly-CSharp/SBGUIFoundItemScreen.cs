using System;

// Token: 0x0200007D RID: 125
public class SBGUIFoundItemScreen : SBGUIModalDialog
{
	// Token: 0x060004C7 RID: 1223 RVA: 0x0001E290 File Offset: 0x0001C490
	protected override void Awake()
	{
		this.rewardMarker = base.FindChild("reward_marker");
		base.Awake();
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x0001E2AC File Offset: 0x0001C4AC
	public override void SetParent(SBGUIElement element)
	{
		base.SetTransformParent(element);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0001E2B8 File Offset: 0x0001C4B8
	public void Setup(string title, string message, string texture, bool useExtraButton = false, string extraButtonText = "")
	{
		SBGUILabel sbguilabel = (SBGUILabel)base.FindChild("title");
		SBGUILabel sbguilabel2 = (SBGUILabel)base.FindChild("message_label");
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("icon");
		SBGUIButton sbguibutton = (SBGUIButton)base.FindChild("extra_button");
		if (useExtraButton)
		{
			sbguibutton.SetActive(true);
			SBGUILabel sbguilabel3 = (SBGUILabel)sbguibutton.FindChild("extra_label");
			sbguilabel3.SetText(extraButtonText);
		}
		else
		{
			sbguibutton.SetActive(false);
		}
		sbguilabel.SetText(title);
		sbguilabel2.SetText(message);
		sbguiatlasImage.SetTextureFromAtlas(texture, true, false, true, false, false, 0);
	}
}
