using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Yarg;

// Token: 0x0200005E RID: 94
[RequireComponent(typeof(YGAtlasSprite))]
public class SBGUIAtlasButton : SBGUIButton
{
	// Token: 0x17000074 RID: 116
	// (get) Token: 0x060003AF RID: 943 RVA: 0x00012F44 File Offset: 0x00011144
	// (set) Token: 0x060003B0 RID: 944 RVA: 0x00012F54 File Offset: 0x00011154
	private YGAtlasSprite atlasSprite
	{
		get
		{
			return (YGAtlasSprite)base.sprite;
		}
		set
		{
			base.sprite = value;
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00012F60 File Offset: 0x00011160
	public static SBGUIAtlasButton Create(SBGUIElement parent, float x, float y, float w, float h, string imageAsset)
	{
		GameObject gameObject = new GameObject(string.Format("SBAtlasButton_{0}", SBGUIElement.InstanceID));
		Rect rect = new Rect(x, y, w, h);
		SBGUIAtlasButton sbguiatlasButton = gameObject.AddComponent<SBGUIAtlasButton>();
		sbguiatlasButton.Initialize(parent, rect, imageAsset);
		return sbguiatlasButton;
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00012FA8 File Offset: 0x000111A8
	public void SetTextureFromFound(YGTextureLibrary.FoundMaterial found)
	{
		this.atlasSprite.renderer.sharedMaterial = found.material;
		this.atlasSprite.sprite = new SpriteCoordinates(found.name);
		this.atlasSprite.sprite.coords = found.coords.frame;
		this.atlasSprite.atlasIndex = found.index;
		this.atlasSprite.Load();
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0001301C File Offset: 0x0001121C
	public void SetTexture(string t)
	{
		YGTextureLibrary.FoundMaterial textureFromFound = base.View.Library.FindSpriteMaterial(t);
		if (textureFromFound.index >= 0)
		{
			this.SetTextureFromFound(textureFromFound);
		}
		else
		{
			TFUtils.Assert(false, "unknown image: " + t);
		}
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00013068 File Offset: 0x00011268
	protected override void Initialize(SBGUIElement parent, Rect rect, string imageAsset)
	{
		this.SetParent(parent);
		YG2DRectangle yg2DRectangle = base.gameObject.AddComponent<YG2DRectangle>();
		yg2DRectangle.size = new Vector2(rect.width, rect.height);
		this.button = base.gameObject.AddComponent<TapButton>();
		this.button.SetPosition((int)rect.x, (int)rect.y);
		this.AttachImage(imageAsset);
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x000130D8 File Offset: 0x000112D8
	public SBGUIAtlasImage AttachImage(string asset)
	{
		SBGUIAtlasImage sbguiatlasImage = SBGUIAtlasImage.Create(this, new Rect(0f, 0f, -1f, -1f), asset);
		sbguiatlasImage.transform.localPosition = Vector3.zero;
		this.images[asset] = sbguiatlasImage;
		return sbguiatlasImage;
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00013124 File Offset: 0x00011324
	public void SetTextureFromAtlas(string name)
	{
		this.SetTextureFromAtlas(name, false, false, false, 0);
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00013134 File Offset: 0x00011334
	public void SetTextureFromAtlas(string name, bool resize, bool resizeToTrimmed = false, bool resizeToFit = false, int scalePixel = 0)
	{
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.sprite;
		if (name == ygatlasSprite.sprite.name)
		{
			return;
		}
		YGTextureLibrary.FoundMaterial foundMaterial = base.View.Library.FindSpriteMaterial(name);
		if (foundMaterial.index >= 0)
		{
			ygatlasSprite.sprite = new SpriteCoordinates(foundMaterial.name);
			ygatlasSprite.sprite.coords = foundMaterial.coords.frame;
			if (resize)
			{
				if (resizeToTrimmed)
				{
					ygatlasSprite.size = new Vector2(ygatlasSprite.size.x * foundMaterial.coords.spriteSize.width / foundMaterial.coords.spriteSourceSize.x, ygatlasSprite.size.y * foundMaterial.coords.spriteSize.height / foundMaterial.coords.spriteSourceSize.y);
				}
				else if (resizeToFit)
				{
					float num = ygatlasSprite.size.x / foundMaterial.coords.spriteSize.width;
					float num2 = ygatlasSprite.size.y / foundMaterial.coords.spriteSize.height;
					float num3 = 1f;
					if (num2 < num)
					{
						num = num2;
					}
					if (scalePixel > 0)
					{
						float num4 = Mathf.Max(num * foundMaterial.coords.spriteSize.width, num * foundMaterial.coords.spriteSize.height);
						if (num4 > (float)scalePixel)
						{
							num3 = num4 / (float)scalePixel;
						}
						else
						{
							num3 = (float)scalePixel / num4;
						}
					}
					ygatlasSprite.size = new Vector2(num * foundMaterial.coords.spriteSize.width * num3, num * foundMaterial.coords.spriteSize.height * num3);
				}
				else
				{
					ygatlasSprite.size = new Vector2(foundMaterial.coords.spriteSize.width, foundMaterial.coords.spriteSize.height);
				}
			}
			ygatlasSprite.atlasIndex = foundMaterial.index;
			if (ygatlasSprite.GetAtlas().useSingleTexture)
			{
				this.SetTextureFromLibrary(foundMaterial.name, null);
				ygatlasSprite.SetNonAtlasName(foundMaterial.name);
			}
			else
			{
				ygatlasSprite.SetMaterial(foundMaterial.material);
				ygatlasSprite.Load();
			}
			ygatlasSprite.Load();
		}
		else if (ygatlasSprite.GetAtlas().useSingleTexture)
		{
			ygatlasSprite.sprite = new SpriteCoordinates(name);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);
			string path = "Textures/Atlases/Obj_Portraits/" + fileNameWithoutExtension;
			Texture texture = (Texture)Resources.Load(path, typeof(Texture));
			if (texture == null)
			{
				path = "Textures/Atlases/Portraits/" + fileNameWithoutExtension;
				texture = (Texture)Resources.Load(path, typeof(Texture));
				if (texture == null)
				{
					TFUtils.Assert(false, "Image doesn't exist in Textures/Atlases/Portraits/ or in atlas, inquired image:  " + name);
				}
			}
			if (resize)
			{
				if (resizeToTrimmed)
				{
					ygatlasSprite.size = new Vector2(ygatlasSprite.size.x, ygatlasSprite.size.y);
				}
				else
				{
					ygatlasSprite.size = new Vector2((float)texture.width, (float)texture.height);
				}
			}
			this.SetTextureFromLibrary(name, texture);
			ygatlasSprite.SetNonAtlasName(name);
		}
		else
		{
			TFUtils.Assert(false, "unknown image: " + name);
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x000134A0 File Offset: 0x000116A0
	public void SetTextureFromLibrary(string name, Texture texture = null)
	{
		if (texture == null)
		{
			texture = base.View.Library.LoadTexture(name);
		}
		Material materialPrototype = base.View.Library.materialPrototype;
		Material material = materialPrototype;
		base.renderer.material = material;
		Color color = base.renderer.material.GetColor("_TintColor");
		if (texture != null)
		{
			color.a = 1f;
			base.renderer.material.SetTexture("_MainTex", texture);
		}
		else
		{
			color.a = 0f;
		}
		base.renderer.material.SetColor("_TintColor", color);
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.sprite;
		ygatlasSprite.sprite = new SpriteCoordinates();
		ygatlasSprite.Load();
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00013574 File Offset: 0x00011774
	public override Vector2 ScaleToMaxSize(int pixels)
	{
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.sprite;
		if (string.IsNullOrEmpty(ygatlasSprite.sprite.name))
		{
			return base.ScaleToMaxSize(pixels);
		}
		Vector2 vector = new Vector2(ygatlasSprite.sprite.coords.width, ygatlasSprite.sprite.coords.height);
		float num = Mathf.Max(vector.x, vector.y);
		if (num > (float)pixels)
		{
			float d = (float)pixels / num;
			vector *= d;
		}
		base.sprite.SetSize(vector);
		return vector;
	}

	// Token: 0x04000276 RID: 630
	private Dictionary<string, SBGUIAtlasImage> images = new Dictionary<string, SBGUIAtlasImage>();

	// Token: 0x04000277 RID: 631
	private string loadedTexture;
}
