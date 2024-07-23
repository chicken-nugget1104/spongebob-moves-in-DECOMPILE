using System;

// Token: 0x02000091 RID: 145
public class SBGUIMoveInDialog : SBGUIModalDialog
{
	// Token: 0x0600057D RID: 1405 RVA: 0x00023798 File Offset: 0x00021998
	public void Setup(string characterName, string buildingName, string portraitTexture)
	{
		this.characterMessage = (SBGUILabel)base.FindChild("character_message_label");
		this.characterMessage.SetText(string.Format(Language.Get("!!MOVE_IN_CHARACTER_MESSAGE"), characterName));
		this.buildingMessage = (SBGUILabel)base.FindChild("building_message_label");
		this.buildingMessage.SetText(string.Format(Language.Get("!!MOVE_IN_BUILDING_MESSAGE"), buildingName));
		this.portrait = (SBGUIAtlasImage)base.FindChild("icon");
		this.portrait.SetTextureFromAtlas(portraitTexture);
		this.portrait.ScaleToMaxSize(128);
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0002383C File Offset: 0x00021A3C
	public override void SetParent(SBGUIElement element)
	{
		base.SetTransformParent(element);
	}

	// Token: 0x0400043E RID: 1086
	private const int ICON_SIZE = 128;

	// Token: 0x0400043F RID: 1087
	private SBGUILabel characterMessage;

	// Token: 0x04000440 RID: 1088
	private SBGUILabel buildingMessage;

	// Token: 0x04000441 RID: 1089
	private SBGUIAtlasImage portrait;
}
