using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yarg
{
	// Token: 0x020000ED RID: 237
	[Serializable]
	public class SpriteCoordinates
	{
		// Token: 0x060008F9 RID: 2297 RVA: 0x00022408 File Offset: 0x00020608
		public SpriteCoordinates()
		{
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00022410 File Offset: 0x00020610
		public SpriteCoordinates(string asset)
		{
			this.name = asset;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x00022420 File Offset: 0x00020620
		public YGSprite.MeshUpdate MeshUpdate
		{
			get
			{
				return new YGSprite.MeshUpdate(this);
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00022428 File Offset: 0x00020628
		public bool Reload(Dictionary<string, AtlasCoords> frames)
		{
			if (!frames.ContainsKey(this.name))
			{
				Debug.LogError("couldn't reload " + this.name);
				return false;
			}
			this.coords = frames[this.name].frame;
			return true;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00022478 File Offset: 0x00020678
		public void SetMesh(Mesh mesh)
		{
			this.verts = mesh.vertices;
			this.normals = mesh.normals;
			this.color = mesh.colors;
			this.tris = mesh.triangles;
			this.uvs = mesh.uv;
		}

		// Token: 0x0400059B RID: 1435
		public string name;

		// Token: 0x0400059C RID: 1436
		[HideInInspector]
		public Rect coords;

		// Token: 0x0400059D RID: 1437
		[HideInInspector]
		public Vector3[] verts;

		// Token: 0x0400059E RID: 1438
		[HideInInspector]
		public Vector3[] normals;

		// Token: 0x0400059F RID: 1439
		[HideInInspector]
		public Color[] color;

		// Token: 0x040005A0 RID: 1440
		[HideInInspector]
		public int[] tris;

		// Token: 0x040005A1 RID: 1441
		[HideInInspector]
		public Vector2[] uvs;
	}
}
