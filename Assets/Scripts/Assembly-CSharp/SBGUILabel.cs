using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
[RequireComponent(typeof(YGTextAtlasSprite))]
public class SBGUILabel : SBGUIImage
{
	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000529 RID: 1321 RVA: 0x0002094C File Offset: 0x0001EB4C
	// (set) Token: 0x0600052A RID: 1322 RVA: 0x0002095C File Offset: 0x0001EB5C
	public YGTextSprite textSprite
	{
		get
		{
			return (YGTextSprite)base.sprite;
		}
		set
		{
			base.sprite = value;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x0600052B RID: 1323 RVA: 0x00020968 File Offset: 0x0001EB68
	public override int Width
	{
		get
		{
			return Mathf.CeilToInt(this.textSprite.textSize.x);
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600052C RID: 1324 RVA: 0x00020980 File Offset: 0x0001EB80
	public override int Height
	{
		get
		{
			return Mathf.CeilToInt(this.textSprite.textSize.y);
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00020998 File Offset: 0x0001EB98
	protected override void Awake()
	{
		if (this.textSprite.textScale != 0f)
		{
			this.textSprite.SetScale(new Vector2(this.textSprite.textScale, this.textSprite.textScale));
		}
		if (Language.CurrentLanguage() == LanguageCode.RU)
		{
			this.SwapFont("cyrillic", this.textSprite);
		}
		base.Awake();
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00020A04 File Offset: 0x0001EC04
	public static SBGUILabel Create(SBGUIElement parent, float x, float y, float w, float h, string text)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUILabel_{0}", SBGUIElement.InstanceID));
		Rect rect = new Rect(x, y, w, h);
		SBGUILabel sbguilabel = gameObject.AddComponent<SBGUILabel>();
		sbguilabel.Initialize(parent, rect, text);
		return sbguilabel;
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00020A4C File Offset: 0x0001EC4C
	protected override void Initialize(SBGUIElement parent, Rect rect, string text)
	{
		base.SetTransformParent(parent);
		this.textSprite = base.gameObject.AddComponent<YGTextSprite>();
		this.textSprite.SetPosition((int)rect.x, (int)rect.y);
		this.textSprite.Text = text;
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00020A98 File Offset: 0x0001EC98
	protected virtual void SwapFont(string desiredFontName, YGTextSprite textSprite)
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

	// Token: 0x06000531 RID: 1329 RVA: 0x00020B94 File Offset: 0x0001ED94
	public virtual bool SetText(string s)
	{
		if (this.textSprite == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(s))
		{
			s = string.Empty;
		}
		if (this.textSprite.Text != s)
		{
			this.textSprite.Text = s;
			if (this.textSprite.textScale != 0f)
			{
				this.textSprite.SetScale(new Vector2(this.textSprite.textScale, this.textSprite.textScale));
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00020C28 File Offset: 0x0001EE28
	public void AdjustText(SBGUIAtlasImage boundary)
	{
		string text = Language.Get(this.Text);
		this.SetText(string.Empty);
		int i = 0;
		while (i < text.Length)
		{
			this.Text += text[i++];
			if ((float)this.Width > boundary.Size.x)
			{
				string text2 = this.Text;
				for (int j = i - 1; j >= 0; j--)
				{
					if (text2[j] == ' ')
					{
						text = text.Remove(j, 1);
						text = text.Insert(j, "|");
						this.SetText(text.Substring(0, i));
						break;
					}
				}
			}
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000533 RID: 1331 RVA: 0x00020CF0 File Offset: 0x0001EEF0
	// (set) Token: 0x06000534 RID: 1332 RVA: 0x00020D00 File Offset: 0x0001EF00
	public virtual string Text
	{
		get
		{
			return this.textSprite.Text;
		}
		set
		{
			if (this.textSprite != null)
			{
				this.textSprite.Text = value;
			}
		}
	}
}
