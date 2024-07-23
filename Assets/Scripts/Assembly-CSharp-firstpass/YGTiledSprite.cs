using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010F RID: 271
public class YGTiledSprite : YGSprite
{
	// Token: 0x06000A03 RID: 2563 RVA: 0x00027B84 File Offset: 0x00025D84
	protected override void OnEnable()
	{
		if (this.tileSize == Vector2.zero)
		{
			this.tileSize = this.GetMainTextureSize(true);
		}
		base.OnEnable();
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00027BBC File Offset: 0x00025DBC
	public static int[] BuildTris(Vector3[] verts)
	{
		List<int> list = new List<int>();
		List<int> list2 = list;
		int[] array = new int[3];
		array[0] = 1;
		array[1] = 4;
		list2.AddRange(array);
		list.AddRange(new int[]
		{
			5,
			4,
			1
		});
		list.AddRange(new int[]
		{
			2,
			5,
			1
		});
		list.AddRange(new int[]
		{
			6,
			5,
			2
		});
		list.AddRange(new int[]
		{
			3,
			6,
			2
		});
		list.AddRange(new int[]
		{
			7,
			6,
			3
		});
		list.AddRange(new int[]
		{
			5,
			8,
			4
		});
		list.AddRange(new int[]
		{
			9,
			8,
			5
		});
		list.AddRange(new int[]
		{
			6,
			9,
			5
		});
		list.AddRange(new int[]
		{
			10,
			9,
			6
		});
		list.AddRange(new int[]
		{
			7,
			10,
			6
		});
		list.AddRange(new int[]
		{
			11,
			10,
			7
		});
		list.AddRange(new int[]
		{
			9,
			12,
			8
		});
		list.AddRange(new int[]
		{
			13,
			12,
			9
		});
		list.AddRange(new int[]
		{
			10,
			13,
			9
		});
		list.AddRange(new int[]
		{
			14,
			13,
			10
		});
		list.AddRange(new int[]
		{
			11,
			14,
			10
		});
		list.AddRange(new int[]
		{
			15,
			14,
			11
		});
		return list.ToArray();
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00027D9C File Offset: 0x00025F9C
	public static Vector3[] BuildVerts(Vector2 size, Vector2 tileSize, Vector2 scale)
	{
		List<Vector3> list = new List<Vector3>();
		List<float> list2 = new List<float>();
		List<float> list3 = new List<float>();
		int num = Mathf.FloorToInt(size.x / tileSize.x);
		int num2 = Mathf.FloorToInt(size.y / tileSize.y);
		int num3 = 0;
		while ((float)num3 < size.x)
		{
			list2.Add((float)num3);
			num3 += num;
		}
		list2.Add(size.x);
		int num4 = 0;
		while ((float)num4 < size.y)
		{
			list3.Add((float)num4);
			num4 += num2;
		}
		list3.Add(size.y);
		foreach (float num5 in list2)
		{
			float x = num5;
			foreach (float num6 in list3)
			{
				float y = num6;
				list.Add(new Vector3(x, y));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x00027F00 File Offset: 0x00026100
	public static Vector2[] BuildUVs(Rect rect, Vector2 size, Vector3[] verts)
	{
		List<Vector2> list = new List<Vector2>();
		float[] array = new float[]
		{
			1f - rect.y / size.y,
			1f - rect.y / size.y,
			1f - (rect.y + rect.height) / size.y,
			1f - (rect.y + rect.height) / size.y
		};
		foreach (float y in array)
		{
			list.Add(new Vector2(rect.x / size.x, y));
			list.Add(new Vector2(rect.x / size.x, y));
			list.Add(new Vector2((rect.x + rect.width) / size.x, y));
			list.Add(new Vector2((rect.x + rect.width) / size.x, y));
		}
		return list.ToArray();
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x0002802C File Offset: 0x0002622C
	public override void AssembleMesh()
	{
		this.update.Reset();
		this.verts = YGTiledSprite.BuildVerts(this.size, this.tileSize, this.tileScale);
		this.update.verts = this.verts;
		this.update.tris = YGTiledSprite.BuildTris(this.verts);
		this.update.uvs = YGTiledSprite.BuildUVs(new Rect(0f, 0f, this.textureSize.x, this.textureSize.y), this.textureSize, this.verts);
		this.update.normals = YGSprite.BuildNormals(this.update.vertCount);
		this.colors = new Color[this.verts.Length];
		YGSprite.BuildColors(this.color, ref this.colors);
		this.update.colors = this.colors;
		this.UpdateMesh(this.update);
	}

	// Token: 0x0400066F RID: 1647
	public Vector2 tileSize;

	// Token: 0x04000670 RID: 1648
	public Vector2 tileScale = Vector2.one;

	// Token: 0x04000671 RID: 1649
	public Vector2 tileOffset = Vector2.one;
}
