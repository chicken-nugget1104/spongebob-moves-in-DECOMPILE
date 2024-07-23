using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class SBGUILevelUpSlot : SBGUIElement
{
	// Token: 0x06000547 RID: 1351 RVA: 0x00021970 File Offset: 0x0001FB70
	public static SBGUILevelUpSlot Create(Session session, SBGUIScreen screen, SBGUIElement parent, Vector3 offset, string iconTexture)
	{
		SBGUILevelUpSlot sbguilevelUpSlot = (SBGUILevelUpSlot)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/LevelUpSlot");
		sbguilevelUpSlot.Setup(session, screen, parent, offset, iconTexture);
		return sbguilevelUpSlot;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0002199C File Offset: 0x0001FB9C
	public void Setup(Session session, SBGUIScreen screen, SBGUIElement parent, Vector3 offset, string iconTexture)
	{
		base.name = string.Format("Slot_{0}", iconTexture);
		this.SetParent(parent);
		base.transform.localPosition = offset;
		SBGUIAtlasImage sbguiatlasImage = (SBGUIAtlasImage)base.FindChild("icon");
		sbguiatlasImage.SetTextureFromAtlas(iconTexture, true, false, true, false, false, 0);
	}

	// Token: 0x040003FF RID: 1023
	public const int GAP_SIZE = 3;
}
