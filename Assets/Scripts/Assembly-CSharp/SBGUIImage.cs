using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
[RequireComponent(typeof(YGSprite))]
public class SBGUIImage : SBGUIElement
{
	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060004D0 RID: 1232 RVA: 0x0001E4D8 File Offset: 0x0001C6D8
	// (set) Token: 0x060004D1 RID: 1233 RVA: 0x0001E500 File Offset: 0x0001C700
	protected YGSprite sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<YGSprite>();
			}
			return this._sprite;
		}
		set
		{
			this._sprite = value;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0001E50C File Offset: 0x0001C70C
	// (set) Token: 0x060004D3 RID: 1235 RVA: 0x0001E51C File Offset: 0x0001C71C
	public Vector2 Size
	{
		get
		{
			return this.sprite.size;
		}
		set
		{
			this.sprite.SetSize(value);
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001E52C File Offset: 0x0001C72C
	public virtual int Width
	{
		get
		{
			return Mathf.CeilToInt(this.sprite.size.x);
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001E544 File Offset: 0x0001C744
	public virtual int Height
	{
		get
		{
			return Mathf.CeilToInt(this.sprite.size.y);
		}
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x0001E55C File Offset: 0x0001C75C
	protected override void Awake()
	{
		this.sprite = base.GetComponent<YGSprite>();
		base.Awake();
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x0001E570 File Offset: 0x0001C770
	public static SBGUIImage Create(SBGUIElement parent, string name, string texture, Vector3 offset)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.localPosition = offset;
		SBGUIImage sbguiimage = gameObject.AddComponent<SBGUIImage>();
		sbguiimage.sprite = gameObject.AddComponent<YGSprite>();
		sbguiimage.SetParent(parent);
		sbguiimage.SetTextureFromMaterialPath(texture);
		return sbguiimage;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0001E5C4 File Offset: 0x0001C7C4
	public static SBGUIImage Create(SBGUIElement parent, Rect size, string asset)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIImage_{0}", SBGUIElement.InstanceID));
		SBGUIImage sbguiimage = gameObject.AddComponent<SBGUIImage>();
		sbguiimage.Initialize(parent, size, asset);
		return sbguiimage;
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x0001E5FC File Offset: 0x0001C7FC
	public virtual Vector2 ScaleToMaxSize(int pixels)
	{
		Texture mainTexture = this.sprite.renderer.material.mainTexture;
		if (mainTexture == null)
		{
			return Vector2.zero;
		}
		Vector2 vector = new Vector2((float)mainTexture.width, (float)mainTexture.height);
		float num = Mathf.Max(vector.x, vector.y);
		if (num > (float)pixels)
		{
			float d = (float)pixels / num;
			vector *= d;
		}
		this.sprite.SetSize(vector);
		return vector;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x0001E67C File Offset: 0x0001C87C
	public void SetSizeNoRebuild(Vector2 newSize)
	{
		this.sprite.size = newSize;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0001E68C File Offset: 0x0001C88C
	protected virtual void Initialize(SBGUIElement parent, Rect rect, string asset)
	{
		this.SetParent(parent);
		TFUtils.DebugLog(string.Format("SBGUIImage Initialize: {0} sprite: {1}", base.gameObject.name, asset));
		this.sprite = base.gameObject.AddComponent<YGSprite>();
		this.sprite.SetPosition((int)rect.x, (int)rect.y);
		if (asset != null)
		{
			this.SetTextureFromMaterialPath(asset);
		}
		if (rect.width != -1f && rect.height != -1f)
		{
			this.sprite.size = new Vector2(rect.width, rect.height);
		}
		this.sprite.ResetSize();
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x0001E740 File Offset: 0x0001C940
	public void SetAlpha(float a)
	{
		this.sprite.SetAlpha(a);
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x0001E750 File Offset: 0x0001C950
	public void SetColor(Color c)
	{
		this.sprite.SetColor(c);
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0001E760 File Offset: 0x0001C960
	public Rect GetWorldRect()
	{
		return this.sprite.GetBounds().ToRect();
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x0001E774 File Offset: 0x0001C974
	public virtual void SetTextureFromTexturePath(string path)
	{
		if (!path.ToLower().StartsWith("textures/"))
		{
			TFUtils.DebugLog(string.Format("Are you sure this is a texture? '{0}'", path));
		}
		Material material = base.renderer.material;
		Texture texture = (Texture)Resources.Load(path);
		TFUtils.Assert(texture != null, "unknown texture: " + path);
		material.mainTexture = texture;
		this.sprite.RefreshTextureSize();
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0001E7E8 File Offset: 0x0001C9E8
	public virtual void SetMaterial(Material mat)
	{
		TFUtils.Assert(mat != null, "unknown material: " + mat.name);
		base.renderer.material = mat;
		this.sprite.RefreshTextureSize();
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0001E828 File Offset: 0x0001CA28
	public virtual void SetTextureFromMaterialPath(string path)
	{
		TFUtils.Assert(!string.IsNullOrEmpty(path), "SetTextureFromMaterialPath: empty path!");
		Material material = Resources.Load(path) as Material;
		TFUtils.Assert(material != null, "unknown material: " + path);
		base.renderer.material = material;
		this.sprite.RefreshTextureSize();
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0001E884 File Offset: 0x0001CA84
	public virtual void SetTexture(Texture t)
	{
		this.sprite.renderer.material.mainTexture = t;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0001E8A8 File Offset: 0x0001CAA8
	public virtual void Unload()
	{
	}

	// Token: 0x0400039D RID: 925
	private YGSprite _sprite;
}
