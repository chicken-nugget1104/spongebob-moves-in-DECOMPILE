using System;
using UnityEngine;
using Yarg;

// Token: 0x02000105 RID: 261
public class YGAtlasSprite : YGSprite
{
	// Token: 0x0600098A RID: 2442 RVA: 0x00025060 File Offset: 0x00023260
	protected override void OnEnable()
	{
		YGTextureLibrary library = base.View.Library;
		TextureAtlas textureAtlas = library.textureAtlases[this.atlasIndex];
		Material atlasMaterial = textureAtlas.GetAtlasMaterial();
		if (base.renderer.sharedMaterial != atlasMaterial)
		{
			base.SetMaterial(atlasMaterial);
		}
		base.OnEnable();
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x000250B4 File Offset: 0x000232B4
	public override Vector2 ResetSize()
	{
		this.size.Set(this.sprite.coords.width, this.sprite.coords.height);
		YGSprite.BuildVerts(this.size, this.scale, ref this.verts);
		this.sprite.verts = this.verts;
		this.update.verts = this.sprite.verts;
		base.UpdateMesh();
		return this.size;
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x00025138 File Offset: 0x00023338
	public virtual void SetUVs(SpriteCoordinates coords)
	{
		this.sprite = this.LoadSprite(coords.name, coords.coords);
		base.View.RefreshEvent += this.AssembleMesh;
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00025178 File Offset: 0x00023378
	protected virtual void LoadSprite()
	{
		this.nonAtlasName = string.Empty;
		this.sprite = this.LoadSprite(this.sprite.name, this.sprite.coords);
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x000251A8 File Offset: 0x000233A8
	public virtual SpriteCoordinates LoadEmptySprite()
	{
		return this.LoadSprite(null, new Rect(0f, 0f, this.textureSize.x, this.textureSize.y));
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x000251E4 File Offset: 0x000233E4
	public virtual SpriteCoordinates LoadSprite(string name, Rect frame)
	{
		SpriteCoordinates spriteCoordinates = new SpriteCoordinates(name);
		spriteCoordinates.coords = frame;
		YGSprite.BuildVerts(this.size, this.scale, ref this.verts);
		YGSprite.BuildColors(this.color, ref this.colors);
		YGSprite.BuildUVs(spriteCoordinates.coords, this.textureSize, ref this.uvs);
		spriteCoordinates.verts = this.verts;
		spriteCoordinates.normals = this.normals;
		spriteCoordinates.tris = this.tris;
		spriteCoordinates.color = this.colors;
		spriteCoordinates.uvs = this.uvs;
		return spriteCoordinates;
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x0002527C File Offset: 0x0002347C
	public TextureAtlas GetAtlas()
	{
		return base.View.Library.textureAtlases[this.atlasIndex];
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x000252A4 File Offset: 0x000234A4
	public virtual SpriteCoordinates LoadSpriteFromAtlas(string name, int atlasIndex)
	{
		if (base.View == null)
		{
			Debug.LogError("No GUIView in scene");
			return new SpriteCoordinates(name);
		}
		return this.LoadSpriteFromAtlas(name, atlasIndex, base.View.Library);
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x000252E8 File Offset: 0x000234E8
	public virtual SpriteCoordinates LoadSpriteFromAtlas(string name, int atlasIndex, YGTextureLibrary library)
	{
		name = name.Trim();
		TextureAtlas textureAtlas = library.textureAtlases[atlasIndex];
		if (textureAtlas == null)
		{
			Debug.LogError("Texture Atlas is null");
			SpriteCoordinates spriteCoordinates = this.LoadEmptySprite();
			spriteCoordinates.name = name;
			return spriteCoordinates;
		}
		AtlasCoords atlasCoords = textureAtlas[name];
		if (atlasCoords == null)
		{
			Debug.LogError(string.Format("Texture Atlas '{0}' does not contain '{1}'", textureAtlas.name, name));
			SpriteCoordinates spriteCoordinates2 = this.LoadEmptySprite();
			spriteCoordinates2.name = name;
			return spriteCoordinates2;
		}
		return this.LoadSprite(name, atlasCoords.frame);
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0002536C File Offset: 0x0002356C
	public override void AssembleMesh()
	{
		if (this.sprite == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.sprite.name))
		{
			this.sprite = this.LoadEmptySprite();
			base.AssembleMesh();
			return;
		}
		this.LoadSprite();
		this.UpdateMesh(this.sprite.MeshUpdate);
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x000253C4 File Offset: 0x000235C4
	protected override Vector2 GetMainTextureSize(bool fromShared)
	{
		TextureAtlas textureAtlas = base.View.Library.textureAtlases[this.atlasIndex];
		return new Vector2(textureAtlas.meta.size.width, textureAtlas.meta.size.height);
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x00025410 File Offset: 0x00023610
	public void SetNonAtlasName(string nonAtlasName)
	{
		this.nonAtlasName = nonAtlasName;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0002541C File Offset: 0x0002361C
	protected override void OnDisable()
	{
		if (!string.IsNullOrEmpty(this.nonAtlasName))
		{
			base.View.Library.UnLoadTexture(this.nonAtlasName);
		}
		base.OnDisable();
	}

	// Token: 0x04000621 RID: 1569
	public SpriteCoordinates sprite;

	// Token: 0x04000622 RID: 1570
	public string nonAtlasName;

	// Token: 0x04000623 RID: 1571
	[HideInInspector]
	public int atlasIndex;
}
