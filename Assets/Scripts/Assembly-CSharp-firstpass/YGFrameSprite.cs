using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class YGFrameSprite : YGSprite
{
	// Token: 0x0600099F RID: 2463 RVA: 0x00025640 File Offset: 0x00023840
	protected override void OnEnable()
	{
		this.lockAspect = false;
		base.OnEnable();
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x00025650 File Offset: 0x00023850
	public new static int[] BuildTris()
	{
		return new int[]
		{
			1,
			4,
			0,
			5,
			4,
			1,
			2,
			5,
			1,
			6,
			5,
			2,
			3,
			6,
			2,
			7,
			6,
			3,
			5,
			8,
			4,
			9,
			8,
			5,
			6,
			9,
			5,
			10,
			9,
			6,
			7,
			10,
			6,
			11,
			10,
			7,
			9,
			12,
			8,
			13,
			12,
			9,
			10,
			13,
			9,
			14,
			13,
			10,
			11,
			14,
			10,
			15,
			14,
			11
		};
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x00025674 File Offset: 0x00023874
	public static Vector3[] BuildVerts(Vector2 size, RectOffset padding, Vector2 scale)
	{
		List<Vector3> list = new List<Vector3>();
		float[] array = new float[]
		{
			0f,
			(float)(-(float)padding.top),
			-size.y + (float)padding.bottom,
			-size.y
		};
		foreach (float num in array)
		{
			list.Add(new Vector2(0f, num * scale.y));
			list.Add(new Vector2((float)padding.left * scale.x, num * scale.y));
			list.Add(new Vector2((size.x - (float)padding.right) * scale.x, num * scale.y));
			list.Add(new Vector2(size.x * scale.x, num * scale.y));
		}
		return list.ToArray();
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x0002577C File Offset: 0x0002397C
	public static Vector2[] BuildUVs(Rect rect, RectOffset padding, Vector2 size)
	{
		List<Vector2> list = new List<Vector2>();
		float[] array = new float[]
		{
			1f - rect.yMin / size.y,
			1f - (rect.yMin + (float)padding.top) / size.y,
			1f - (rect.yMax - (float)padding.bottom) / size.y,
			1f - rect.yMax / size.y
		};
		foreach (float y in array)
		{
			list.Add(new Vector2(rect.xMin / size.x, y));
			list.Add(new Vector2((rect.xMin + (float)padding.left) / size.x, y));
			list.Add(new Vector2((rect.xMax - (float)padding.right) / size.x, y));
			list.Add(new Vector2(rect.xMax / size.x, y));
		}
		return list.ToArray();
	}

	// Token: 0x060009A3 RID: 2467 RVA: 0x000258A8 File Offset: 0x00023AA8
	public override void SetSize(Vector2 s)
	{
		this.size = s;
		this.update.verts = YGFrameSprite.BuildVerts(this.size, this.padding, this.scale);
		base.View.RefreshEvent += base.UpdateMesh;
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x000258F8 File Offset: 0x00023AF8
	public override void SetColor(Color c)
	{
		this.color = c;
		YGSprite.BuildColors(this.color, ref this.colors);
		this.update.colors = this.colors;
		base.View.RefreshEvent += base.UpdateMesh;
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00025948 File Offset: 0x00023B48
	public override void SetAlpha(float alpha)
	{
		this.color.a = alpha;
		this.SetColor(this.color);
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00025964 File Offset: 0x00023B64
	public override void AssembleMesh()
	{
		this.update.Reset();
		YGSprite.BuildColors(this.color, ref this.colors);
		this.update.verts = YGFrameSprite.BuildVerts(this.size, this.padding, this.scale);
		this.update.normals = YGSprite.BuildNormals(this.update.vertCount);
		this.update.tris = YGFrameSprite.BuildTris();
		this.update.colors = this.colors;
		this.update.uvs = YGFrameSprite.BuildUVs(new Rect(0f, 0f, this.textureSize.x, this.textureSize.y), this.padding, this.textureSize);
		this.UpdateMesh(this.update);
	}

	// Token: 0x04000625 RID: 1573
	public RectOffset padding;
}
