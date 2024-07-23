using System;
using UnityEngine;

// Token: 0x0200045D RID: 1117
public class TFQuad
{
	// Token: 0x06002286 RID: 8838 RVA: 0x000D3CEC File Offset: 0x000D1EEC
	public static void SetupQuadMesh(Mesh mesh, float width, float height, Vector2 center, bool resizeOnly = false, Rect? uvs = null)
	{
		float num = 0.5f * width;
		float num2 = 0.5f * height;
		float x = center.x;
		float y = center.y;
		Vector3[] vertices = new Vector3[]
		{
			new Vector3(-num - x, -num2 - y, 0f),
			new Vector3(-num - x, num2 - y, 0f),
			new Vector3(num - x, -num2 - y, 0f),
			new Vector3(num - x, num2 - y, 0f)
		};
		mesh.vertices = vertices;
		if (!resizeOnly)
		{
			if (uvs != null)
			{
				mesh.uv = new Vector2[]
				{
					new Vector2(uvs.Value.xMax, uvs.Value.yMin),
					new Vector2(uvs.Value.xMax, uvs.Value.yMax),
					new Vector2(uvs.Value.xMin, uvs.Value.yMin),
					new Vector2(uvs.Value.xMin, uvs.Value.yMax)
				};
			}
			else
			{
				mesh.uv = new Vector2[]
				{
					new Vector2(1f, 0f),
					new Vector2(1f, 1f),
					new Vector2(0f, 0f),
					new Vector2(0f, 1f)
				};
			}
			int[] triangles = new int[]
			{
				2,
				1,
				0,
				1,
				2,
				3
			};
			mesh.triangles = triangles;
		}
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x000D3F18 File Offset: 0x000D2118
	public static void SetupQuad(GameObject go, Material material, float width, float height, Vector2 center, Rect? uvs = null, Mesh hitMesh = null)
	{
		go.AddComponent<MeshFilter>();
		MeshFilter component = go.GetComponent<MeshFilter>();
		go.AddComponent<MeshRenderer>();
		MeshRenderer component2 = go.GetComponent<MeshRenderer>();
		component2.castShadows = false;
		component2.receiveShadows = false;
		component2.material = material;
		if (hitMesh == null)
		{
			Mesh mesh = new Mesh();
			component.mesh = mesh;
			TFQuad.SetupQuadMesh(mesh, width, height, center, false, uvs);
		}
		else
		{
			component.mesh = hitMesh;
		}
	}
}
