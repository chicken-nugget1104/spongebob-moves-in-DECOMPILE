using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using Yarg;

// Token: 0x020003F5 RID: 1013
public class Border
{
	// Token: 0x06001F0B RID: 7947 RVA: 0x000BE680 File Offset: 0x000BC880
	private void CreateTerrainBorder(Terrain terrain)
	{
		if (!Border.BorderEnabled)
		{
			return;
		}
		GameObject gameObject = UnityGameResources.Create("Prefabs/TerrainBorder");
		gameObject.name = "BorderFlatTerrain";
		if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
		{
			Material sharedMaterial = gameObject.renderer.sharedMaterial;
			gameObject.renderer.material = (Resources.Load("Materials/lod/terrainBorder_lr") as Material);
			Resources.UnloadAsset(sharedMaterial);
		}
		else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
		{
			Material sharedMaterial2 = gameObject.renderer.sharedMaterial;
			gameObject.renderer.material = (Resources.Load("Materials/lod/terrainBorder_lr2") as Material);
			Resources.UnloadAsset(sharedMaterial2);
		}
		Mesh mesh = new Mesh();
		gameObject.GetComponent<MeshFilter>().mesh = mesh;
		int num = 12;
		this.terrVertices = new Vector3[num];
		this.terrUVs = new Vector2[num];
		float num2 = this.nonTopBorderWidth;
		float num3 = (float)terrain.WorldWidth;
		float num4 = (float)terrain.WorldHeight;
		float num5 = 20f;
		float uvScale = 0.05f;
		this.terrVertCount = 0;
		this.AddTerrBorderVertex(num3 + num5 - this.borderEpsilon, -num2, uvScale);
		this.AddTerrBorderVertex(num3 + num5 - this.borderEpsilon, this.borderEpsilon, uvScale);
		this.AddTerrBorderVertex(this.borderEpsilon, -num2, uvScale);
		this.AddTerrBorderVertex(this.borderEpsilon, this.borderEpsilon, uvScale);
		this.AddTerrBorderVertex(-num2, -num2, uvScale);
		this.AddTerrBorderVertex(-num2, this.borderEpsilon, uvScale);
		this.AddTerrBorderVertex(-num2, num4 - this.borderEpsilon, uvScale);
		this.AddTerrBorderVertex(-num2, num4 + num2, uvScale);
		this.AddTerrBorderVertex(this.borderEpsilon, num4 - this.borderEpsilon, uvScale);
		this.AddTerrBorderVertex(this.borderEpsilon, num4 + num2, uvScale);
		this.AddTerrBorderVertex(num3 + num5 - this.borderEpsilon, num4 - this.borderEpsilon, uvScale);
		this.AddTerrBorderVertex(num3 + num5 - this.borderEpsilon, num4 + num2, uvScale);
		int[] triangles = new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			5,
			3,
			6,
			8,
			8,
			7,
			7,
			6,
			9,
			8,
			11,
			10
		};
		mesh.vertices = this.terrVertices;
		mesh.uv = this.terrUVs;
		mesh.SetTriangles(triangles, 0);
		this.terrVertices = null;
		this.terrUVs = null;
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x000BE8BC File Offset: 0x000BCABC
	private void AddTerrBorderVertex(float x, float y, float uvScale)
	{
		this.terrVertices[this.terrVertCount].x = x;
		this.terrVertices[this.terrVertCount].y = y;
		this.terrVertices[this.terrVertCount].z = 0f;
		this.terrUVs[this.terrVertCount].x = x * uvScale;
		this.terrUVs[this.terrVertCount].y = y * uvScale;
		this.terrVertCount++;
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x000BE954 File Offset: 0x000BCB54
	public void UpdateTerrainBorderStrip(Terrain terrain)
	{
		if (!Border.BorderEnabled)
		{
			return;
		}
		Mesh mesh;
		if (this.stripBorderGO == null)
		{
			this.stripBorderGO = new GameObject("BorderStrip");
			MeshFilter meshFilter = this.stripBorderGO.AddComponent<MeshFilter>();
			mesh = new Mesh();
			meshFilter.mesh = mesh;
			MeshRenderer meshRenderer = this.stripBorderGO.AddComponent<MeshRenderer>();
			meshRenderer.castShadows = false;
			meshRenderer.receiveShadows = false;
			meshRenderer.material = Terrain.TerrainMaterial;
			meshRenderer.material.renderQueue = 2998;
			this.stripBorderGO.transform.localPosition = new Vector3((float)terrain.WorldWidth, 0f, 0f);
		}
		else
		{
			mesh = this.stripBorderGO.GetComponent<MeshFilter>().mesh;
			mesh.Clear();
		}
		int gridHeight = terrain.GridHeight;
		int col = terrain.GridWidth - 1;
		int num = gridHeight * 4;
		int num2 = gridHeight * 6;
		this.terrStripVertices = new Vector3[num];
		this.terrStripUVs = new Vector2[num];
		int[] array = new int[num2];
		Rect? materialUVs = terrain.terrainTextures.GetMaterialUVs("disabled_terrainXL.png");
		Rect? materialUVs2 = terrain.terrainTextures.GetMaterialUVs("disabled_terrainXX.png");
		int num3 = 0;
		float num4 = 0f;
		int num5 = 0;
		this.terrStripVertCount = 0;
		for (int i = 0; i < gridHeight; i++)
		{
			Rect rect = (!terrain.CheckIsPurchasedArea(i, col)) ? materialUVs2.Value : materialUVs.Value;
			this.AddBorderStripVertex(0f, num4, rect.xMin, rect.yMin);
			this.AddBorderStripVertex(20f, num4, rect.xMax, rect.yMin);
			this.AddBorderStripVertex(20f, num4 + 20f, rect.xMax, rect.yMax);
			this.AddBorderStripVertex(0f, num4 + 20f, rect.xMin, rect.yMax);
			num4 += 20f;
			array[num5++] = num3;
			array[num5++] = num3 + 1;
			array[num5++] = num3 + 2;
			array[num5++] = num3;
			array[num5++] = num3 + 2;
			array[num5++] = num3 + 3;
			num3 += 4;
		}
		TFUtils.Assert(num5 == num2, "bad indices in terrain strip");
		mesh.vertices = this.terrStripVertices;
		mesh.uv = this.terrStripUVs;
		mesh.triangles = array;
		this.terrStripVertices = null;
		this.terrStripUVs = null;
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x000BEBE8 File Offset: 0x000BCDE8
	private void AddBorderStripVertex(float x, float y, float u, float v)
	{
		this.terrStripVertices[this.terrStripVertCount].x = x;
		this.terrStripVertices[this.terrStripVertCount].y = y;
		this.terrStripVertices[this.terrStripVertCount].z = 0f;
		this.terrStripUVs[this.terrStripVertCount].x = u;
		this.terrStripUVs[this.terrStripVertCount].y = v;
		this.terrStripVertCount++;
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x000BEC7C File Offset: 0x000BCE7C
	public void CreateTerrainTopBorder(Terrain terrain, float tileSize, int numRows, bool front)
	{
		if (!Border.BorderEnabled)
		{
			return;
		}
		GameObject gameObject = new GameObject((!front) ? "BorderTopBack" : "BorderTopFront");
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.castShadows = false;
		meshRenderer.receiveShadows = false;
		float num = (float)terrain.WorldHeight + this.nonTopBorderWidth;
		float num2 = num + this.nonTopBorderWidth;
		float num3 = (float)terrain.WorldWidth + this.topBorderXOffset;
		if (front)
		{
			if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
			{
				meshRenderer.material = (Resources.Load("Materials/lod/TerrainBorderTopFront_lr") as Material);
			}
			else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				meshRenderer.material = (Resources.Load("Materials/lod/TerrainBorderTopFront_lr2") as Material);
			}
			else
			{
				meshRenderer.material = (Resources.Load("Materials/lod/TerrainBorderTopFront") as Material);
			}
			meshRenderer.material.renderQueue = 2999;
		}
		else
		{
			if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
			{
				meshRenderer.material = (Resources.Load("Materials/lod/TerrainBorderTopBack_lr") as Material);
			}
			else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				meshRenderer.material = (Resources.Load("Materials/lod/TerrainBorderTopBack_lr2") as Material);
			}
			else
			{
				meshRenderer.material = (Resources.Load("Materials/lod/TerrainBorderTopBack") as Material);
			}
			num3 += tileSize;
		}
		gameObject.transform.localPosition = new Vector3(num3, num, 0.3f);
		int num4 = (int)((num2 + 0.99f * tileSize) / tileSize);
		int num5 = num4 * 4;
		int num6 = num4 * 6;
		this.terrStripVertices = new Vector3[num5];
		this.terrStripUVs = new Vector2[num5];
		int[] array = new int[num6];
		int num7 = 0;
		float num8 = 0f;
		int num9 = 0;
		float x = tileSize * (float)numRows;
		this.terrStripVertCount = 0;
		for (int i = 0; i < num4; i++)
		{
			this.AddBorderStripVertex(0f, num8 - tileSize, 0f, 0f);
			this.AddBorderStripVertex(x, num8 - tileSize, 0f, (float)numRows);
			this.AddBorderStripVertex(x, num8, 1f, (float)numRows);
			this.AddBorderStripVertex(0f, num8, 1f, 0f);
			num8 -= tileSize;
			array[num9++] = num7;
			array[num9++] = num7 + 1;
			array[num9++] = num7 + 2;
			array[num9++] = num7;
			array[num9++] = num7 + 2;
			array[num9++] = num7 + 3;
			num7 += 4;
		}
		TFUtils.Assert(num9 == num6, "bad indices in terrain strip");
		mesh.vertices = this.terrStripVertices;
		mesh.uv = this.terrStripUVs;
		mesh.triangles = array;
		this.terrStripVertices = null;
		this.terrStripUVs = null;
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x000BEF60 File Offset: 0x000BD160
	private void CreateBorderObjects()
	{
		if (!Border.BorderEnabled)
		{
			return;
		}
		string json = TFUtils.ReadAllText(Border.borderDecorFile);
		List<object> list = (List<object>)Json.Deserialize(json);
		Camera main = Camera.main;
		Vector3 zero = Vector3.zero;
		int num = 0;
		Vector3 zero2 = Vector3.zero;
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			float width = TFUtils.LoadFloat(dictionary, "width");
			float num2 = TFUtils.LoadFloat(dictionary, "height");
			string sprite = (string)dictionary["sprite"];
			TFUtils.LoadVector3(out zero2, (Dictionary<string, object>)dictionary["position"]);
			GameObject gameObject = UnityGameResources.CreateEmpty("borderObj");
			zero.y = -num2 * 0.5f;
			Rect? uvs = null;
			Material meterialAndUVs = Border.GetMeterialAndUVs(sprite, ref uvs);
			TFQuad.SetupQuad(gameObject, meterialAndUVs, width, num2, zero, uvs, null);
			gameObject.transform.localPosition = zero2;
			gameObject.transform.LookAt(zero2 - main.transform.forward, main.transform.up);
			num++;
		}
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x000BF0CC File Offset: 0x000BD2CC
	public static void UpdateBorderObjects()
	{
		if (!Border.BorderEnabled)
		{
			return;
		}
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x000BF0DC File Offset: 0x000BD2DC
	public static void SaveBorderObjects()
	{
		if (!Border.BorderEnabled)
		{
			return;
		}
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x000BF0EC File Offset: 0x000BD2EC
	private static Material GetMeterialAndUVs(string sprite, ref Rect? rect)
	{
		int num = sprite.LastIndexOf('/');
		Material result;
		if (num >= 0)
		{
			result = TextureLibrarian.LookUp(sprite);
		}
		else
		{
			AtlasAndCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(sprite);
			result = TextureLibrarian.LookUp("Materials/lod/" + atlasCoords.atlas.name);
			Rect value = default(Rect);
			atlasCoords.atlas.GetUVs(atlasCoords.atlasCoords, ref value);
			rect = new Rect?(value);
		}
		return result;
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x000BF160 File Offset: 0x000BD360
	public void Initialize(Terrain terrain)
	{
		if (!Border.BorderEnabled)
		{
			return;
		}
		this.CreateTerrainBorder(terrain);
		this.CreateTerrainTopBorder(terrain, this.topBorderTileSize, 1, true);
		this.CreateTerrainTopBorder(terrain, this.topBorderTileSize, this.topBackBorderRows, false);
		this.CreateBorderObjects();
	}

	// Token: 0x04001331 RID: 4913
	private const int TransparentRenderQueueStart = 3000;

	// Token: 0x04001332 RID: 4914
	private const string BORDER_OBJ_NAME = "borderObj";

	// Token: 0x04001333 RID: 4915
	public static bool BorderEnabled = false;

	// Token: 0x04001334 RID: 4916
	private static string borderDecorFile = TFUtils.GetStreamingAssetsFileInDirectory("Border", "border_decor.json");

	// Token: 0x04001335 RID: 4917
	private Vector3[] terrVertices;

	// Token: 0x04001336 RID: 4918
	private Vector2[] terrUVs;

	// Token: 0x04001337 RID: 4919
	private int terrVertCount;

	// Token: 0x04001338 RID: 4920
	private GameObject stripBorderGO;

	// Token: 0x04001339 RID: 4921
	private Vector3[] terrStripVertices;

	// Token: 0x0400133A RID: 4922
	private Vector2[] terrStripUVs;

	// Token: 0x0400133B RID: 4923
	private int terrStripVertCount;

	// Token: 0x0400133C RID: 4924
	private float borderEpsilon = 0.01f;

	// Token: 0x0400133D RID: 4925
	private float topBorderXOffset = 5f;

	// Token: 0x0400133E RID: 4926
	private float nonTopBorderWidth = 400f;

	// Token: 0x0400133F RID: 4927
	private float topBorderTileSize = 160f;

	// Token: 0x04001340 RID: 4928
	private int topBackBorderRows = 2;
}
