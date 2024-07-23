using System;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class SBGUITabButton : SBGUIAtlasButton
{
	// Token: 0x06000676 RID: 1654 RVA: 0x00029E90 File Offset: 0x00028090
	public virtual void Selected(bool selected)
	{
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00029E94 File Offset: 0x00028094
	private void SetupCategory(SBTabCategory cat)
	{
		this.category = cat;
		SBGUIAtlasImage component = base.FindChild("icon").gameObject.GetComponent<SBGUIAtlasImage>();
		if (!string.IsNullOrEmpty(this.category.Texture))
		{
			component.SetTextureFromAtlas(this.category.Texture);
			component.ScaleToMaxSize(58);
		}
		base.ClickEvent += delegate()
		{
			this.parentBar.TabClick(this);
		};
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x00029F00 File Offset: 0x00028100
	public static SBGUITabButton CreateTabButton(SBGUITabBar parent, SBTabCategory category, int index)
	{
		SBGUITabButton sbguitabButton = (SBGUITabButton)SBGUI.InstantiatePrefab("Prefabs/GUI/Widgets/MarketplaceTab");
		sbguitabButton.name = string.Format("tab_{0}", category.Name);
		sbguitabButton.SetParent(parent);
		sbguitabButton.parentBar = parent;
		YGAtlasSprite component = sbguitabButton.GetComponent<YGAtlasSprite>();
		float x = (component.size.x + -1f) * (float)index * 0.01f;
		sbguitabButton.tform.localPosition = new Vector3(x, 0f, 0f);
		sbguitabButton.SetupCategory(category);
		return sbguitabButton;
	}

	// Token: 0x040004ED RID: 1261
	private const int MAX_TAB_ICON_SIZE = 58;

	// Token: 0x040004EE RID: 1262
	private const int GAP_SIZE = -1;

	// Token: 0x040004EF RID: 1263
	public int tabIndex = -1;

	// Token: 0x040004F0 RID: 1264
	public SBGUITabBar parentBar;

	// Token: 0x040004F1 RID: 1265
	public SBTabCategory category;
}
