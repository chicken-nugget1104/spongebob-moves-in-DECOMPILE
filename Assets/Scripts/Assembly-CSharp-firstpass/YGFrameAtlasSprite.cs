using System;
using UnityEngine;
using Yarg;

// Token: 0x02000106 RID: 262
public class YGFrameAtlasSprite : YGAtlasSprite
{
	// Token: 0x06000998 RID: 2456 RVA: 0x00025460 File Offset: 0x00023660
	private void Awake()
	{
		this.verts = YGFrameSprite.BuildVerts(this.size, this.padding, this.scale);
		this.colors = new Color[this.verts.Length];
	}

	// Token: 0x06000999 RID: 2457 RVA: 0x000254A0 File Offset: 0x000236A0
	protected override void OnEnable()
	{
		this.lockAspect = false;
		base.OnEnable();
	}

	// Token: 0x0600099A RID: 2458 RVA: 0x000254B0 File Offset: 0x000236B0
	public override void SetSize(Vector2 s)
	{
		this.size = s;
		this.update.verts = YGFrameSprite.BuildVerts(this.size, this.padding, this.scale);
		base.View.RefreshEvent += base.UpdateMesh;
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x00025500 File Offset: 0x00023700
	public override void SetColor(Color c)
	{
		this.color = c;
		YGSprite.BuildColors(this.color, ref this.colors);
		this.update.colors = this.colors;
		base.View.RefreshEvent += base.UpdateMesh;
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x00025550 File Offset: 0x00023750
	public override void SetAlpha(float alpha)
	{
		this.color.a = alpha;
		YGSprite.BuildColors(this.color, ref this.colors);
		this.update.colors = this.colors;
		base.View.RefreshEvent += base.UpdateMesh;
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x000255A4 File Offset: 0x000237A4
	public override SpriteCoordinates LoadSprite(string name, Rect frame)
	{
		SpriteCoordinates spriteCoordinates = new SpriteCoordinates(name);
		spriteCoordinates.coords = frame;
		spriteCoordinates.verts = YGFrameSprite.BuildVerts(this.size, this.padding, this.scale);
		YGSprite.BuildColors(this.color, ref this.colors);
		spriteCoordinates.normals = YGSprite.BuildNormals(spriteCoordinates.verts.Length);
		spriteCoordinates.tris = YGFrameSprite.BuildTris();
		spriteCoordinates.color = this.colors;
		spriteCoordinates.uvs = YGFrameSprite.BuildUVs(spriteCoordinates.coords, this.padding, this.textureSize);
		return spriteCoordinates;
	}

	// Token: 0x04000624 RID: 1572
	public RectOffset padding;
}
