using System;
using UnityEngine;

// Token: 0x020000A5 RID: 165
[RequireComponent(typeof(YGTextAtlasSprite))]
public class SBGUIShadowedLabel : SBGUILabel
{
	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000606 RID: 1542 RVA: 0x000263A8 File Offset: 0x000245A8
	// (set) Token: 0x06000607 RID: 1543 RVA: 0x000263B8 File Offset: 0x000245B8
	public override string Text
	{
		get
		{
			return base.textSprite.Text;
		}
		set
		{
			if (base.textSprite != null)
			{
				base.textSprite.Text = value;
				if (this.Shadow != null)
				{
					this.Shadow.SetText(value);
				}
			}
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00026400 File Offset: 0x00024600
	public override bool SetText(string s)
	{
		if (base.textSprite == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(s))
		{
			s = string.Empty;
		}
		if (base.textSprite.Text != s)
		{
			base.textSprite.Text = s;
			this.Text = s;
			if (base.textSprite.textScale != 0f)
			{
				base.textSprite.SetScale(new Vector2(base.textSprite.textScale, base.textSprite.textScale));
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0002649C File Offset: 0x0002469C
	protected override void Awake()
	{
		base.textSprite = base.gameObject.GetComponent<YGTextSprite>();
		if (base.textSprite.textScale != 0f)
		{
			base.textSprite.scale = new Vector2(base.textSprite.textScale, base.textSprite.textScale);
		}
		if (Language.CurrentLanguage() == LanguageCode.RU)
		{
			this.SwapFont("cyrillic", base.textSprite);
		}
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x00026514 File Offset: 0x00024714
	protected override void SwapFont(string desiredFontName, YGTextSprite textSprite)
	{
		int size = base.View.Library.fontAtlases[textSprite.fontIndex].info.size;
		string text = string.Concat(new object[]
		{
			desiredFontName,
			"-",
			size,
			".png"
		});
		for (int i = 0; i < base.View.Library.fontAtlases.Length; i++)
		{
			if (base.View.Library.fontAtlases[i].filename == text)
			{
				textSprite.fontIndex = i;
				textSprite.sprite.coords = YGTextureLibrary.GetAtlasCoords(text).atlasCoords.frame;
			}
		}
		if (base.View.Library.fontAtlases[textSprite.fontIndex].filename != text)
		{
			TFUtils.ErrorLog("Could not find " + text + " in the 'Font Atlases' list in the GUIMainView");
		}
	}

	// Token: 0x0400049D RID: 1181
	public SBGUILabel Shadow;
}
