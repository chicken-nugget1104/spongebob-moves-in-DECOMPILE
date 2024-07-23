using System;
using System.IO;
using UnityEngine;
using Yarg;

// Token: 0x0200005F RID: 95
[RequireComponent(typeof(YGAtlasSprite))]
public class SBGUIAtlasImage : SBGUIImage
{
	// Token: 0x060003BB RID: 955 RVA: 0x00013610 File Offset: 0x00011810
	public new static SBGUIAtlasImage Create(SBGUIElement parent, Rect size, string asset)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIAtlasImage_{0}", SBGUIElement.InstanceID));
		SBGUIAtlasImage sbguiatlasImage = gameObject.AddComponent<SBGUIAtlasImage>();
		sbguiatlasImage.Initialize(parent, size, asset);
		return sbguiatlasImage;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00013648 File Offset: 0x00011848
	protected override void Initialize(SBGUIElement parent, Rect rect, string asset)
	{
		this.SetParent(parent);
		TFUtils.DebugLog(string.Format("SBGUIImage Initialize: {0} sprite: {1}", base.gameObject.name, asset));
		base.sprite = base.gameObject.AddComponent<YGAtlasSprite>();
		base.sprite.SetPosition((int)rect.x, (int)rect.y);
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.sprite;
		if (ygatlasSprite.GetAtlas().useSingleTexture)
		{
			this.SetTextureFromLibrary(asset, null);
		}
		else
		{
			this.SetTextureFromAtlas(asset);
		}
		if (rect.width != -1f && rect.height != -1f)
		{
			base.sprite.size = new Vector2(rect.width, rect.height);
		}
		base.sprite.ResetSize();
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00013720 File Offset: 0x00011920
	public void SetTextureFromSearch(string path)
	{
		string fileName = Path.GetFileName(path);
		YGTextureLibrary.FoundMaterial foundMaterial = base.View.Library.FindSpriteMaterial(fileName);
		if (foundMaterial.index >= 0)
		{
			YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.sprite;
			ygatlasSprite.renderer.sharedMaterial = foundMaterial.material;
			ygatlasSprite.sprite = new SpriteCoordinates(foundMaterial.name);
			ygatlasSprite.atlasIndex = foundMaterial.index;
			ygatlasSprite.Load();
		}
		else if (path.ToLower().StartsWith("texture"))
		{
			this.SetTextureFromTexturePath(path);
		}
		else
		{
			this.SetTextureFromMaterialPath(path);
		}
	}

	// Token: 0x060003BE RID: 958 RVA: 0x000137C4 File Offset: 0x000119C4
	public override void SetTextureFromMaterialPath(string path)
	{
		base.SetTextureFromMaterialPath(path);
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.sprite;
		ygatlasSprite.sprite = new SpriteCoordinates();
		ygatlasSprite.Load();
	}

	// Token: 0x060003BF RID: 959 RVA: 0x000137F8 File Offset: 0x000119F8
	public override void SetTextureFromTexturePath(string path)
	{
		base.SetTextureFromTexturePath(path);
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.sprite;
		ygatlasSprite.sprite = new SpriteCoordinates();
		ygatlasSprite.Load();
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001382C File Offset: 0x00011A2C
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

	// Token: 0x060003C1 RID: 961 RVA: 0x00013900 File Offset: 0x00011B00
	public void SetTextureFromAtlas(string name)
	{
		this.SetTextureFromAtlas(name, false, false, false, false, false, 0);
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0001391C File Offset: 0x00011B1C
	public void SetTextureFromAtlas(string name, bool resize, bool resizeToTrimmed = false, bool resizeToFit = false, bool keepSmallSize = false, bool explanationDialog = false, int scalePixel = 0)
	{
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.sprite;
		if (ygatlasSprite != null && ygatlasSprite.sprite != null && name == ygatlasSprite.sprite.name)
		{
			return;
		}
		YGTextureLibrary.FoundMaterial foundMaterial = base.View.Library.FindSpriteMaterial(name);
		if (foundMaterial.index >= 0)
		{
			ygatlasSprite.sprite = new SpriteCoordinates(foundMaterial.name);
			ygatlasSprite.sprite.coords = foundMaterial.coords.frame;
			if (ygatlasSprite.sprite.coords.width * ygatlasSprite.sprite.coords.height > 2500f)
			{
				keepSmallSize = false;
			}
			if (resize)
			{
				if (resizeToTrimmed)
				{
					ygatlasSprite.size = new Vector2(ygatlasSprite.size.x * foundMaterial.coords.spriteSize.width / foundMaterial.coords.spriteSourceSize.x, ygatlasSprite.size.y * foundMaterial.coords.spriteSize.height / foundMaterial.coords.spriteSourceSize.y);
				}
				else if (resizeToFit && !keepSmallSize)
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
			if (ygatlasSprite.GetAtlas().useSingleTexture && !explanationDialog)
			{
				this.SetTextureFromLibrary(foundMaterial.name, null);
				ygatlasSprite.SetNonAtlasName(foundMaterial.name);
			}
			else
			{
				ygatlasSprite.SetMaterial(foundMaterial.material);
				ygatlasSprite.Load();
			}
		}
		else
		{
			ygatlasSprite.sprite = new SpriteCoordinates(name);
			string str = YGTextureLibrary.ActualName(name);
			string path = "Textures/Atlases/Portraits/" + str;
			Texture2D texture2D = (Texture2D)Resources.Load(path, typeof(Texture));
			if (texture2D == null)
			{
				path = "Textures/Atlases/Obj_Portraits/" + str;
				texture2D = (Texture2D)Resources.Load(path, typeof(Texture));
				if (texture2D == null)
				{
					TFUtils.Assert(false, "Image doesn't exist in Textures/Atlases/Portraits/ or /Obj_Portraits or in atlas, inquired image:  " + name);
				}
			}
			if (ygatlasSprite.sprite.coords.width * ygatlasSprite.sprite.coords.height > 2500f)
			{
				keepSmallSize = false;
			}
			if (resize)
			{
				if (resizeToTrimmed)
				{
					ygatlasSprite.size = new Vector2(ygatlasSprite.size.x, ygatlasSprite.size.y);
				}
				else if (resizeToFit && !keepSmallSize)
				{
					float num5 = ygatlasSprite.size.x / (float)texture2D.width;
					float num6 = ygatlasSprite.size.y / (float)texture2D.height;
					if (num6 < num5)
					{
						num5 = num6;
					}
					ygatlasSprite.size = new Vector2(num5 * (float)texture2D.width, num5 * (float)texture2D.height);
				}
				else
				{
					ygatlasSprite.size = new Vector2((float)texture2D.width, (float)texture2D.height);
				}
				this.SetTextureFromLibrary(name, texture2D);
				ygatlasSprite.SetNonAtlasName(name);
			}
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00013D4C File Offset: 0x00011F4C
	public virtual void ResetSize()
	{
		YGAtlasSprite ygatlasSprite = (YGAtlasSprite)base.sprite;
		ygatlasSprite.ResetSize();
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00013D6C File Offset: 0x00011F6C
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

	// Token: 0x060003C5 RID: 965 RVA: 0x00013E00 File Offset: 0x00012000
	public override void SetTexture(Texture t)
	{
		throw new NotImplementedException();
	}

	// Token: 0x04000278 RID: 632
	private string loadedTexture;
}
