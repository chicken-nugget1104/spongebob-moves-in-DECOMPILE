using System;
using UnityEngine;

// Token: 0x02000466 RID: 1126
public class TerrainSector
{
	// Token: 0x06002376 RID: 9078 RVA: 0x000DA580 File Offset: 0x000D8780
	public TerrainSector(int renderOrder, int row, int col)
	{
		int num = 144;
		this.vertexCount = 0;
		this.vertex = new TerrainVertex[num];
		int num2 = 216;
		this.indexCount = 0;
		this.index = new int[num2];
		this.isHighlighted = false;
		this.gameObject = new GameObject(string.Concat(new object[]
		{
			"TerrainSector",
			row,
			"x",
			col
		}));
		UnityGameResources.AddGameObject(this.gameObject);
		MeshRenderer meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
		MeshFilter meshFilter = this.gameObject.AddComponent<MeshFilter>();
		meshRenderer.sharedMaterial = Terrain.TerrainMaterial;
		this.mesh = new Mesh();
		meshFilter.mesh = this.mesh;
		if (Terrain.terrainTextureScaleU == 0f)
		{
			Texture mainTexture = meshRenderer.sharedMaterial.mainTexture;
			if (mainTexture.width > mainTexture.height)
			{
				Terrain.terrainTextureScaleU = 1f;
				Terrain.terrainTextureScaleV = (float)(mainTexture.width / mainTexture.height);
			}
			else
			{
				Terrain.terrainTextureScaleU = (float)(mainTexture.height / mainTexture.width);
				Terrain.terrainTextureScaleV = 1f;
			}
			Terrain.terrainTextureInvScaleU = 1f / Terrain.terrainTextureScaleU;
			Terrain.terrainTextureInvScaleV = 1f / Terrain.terrainTextureScaleV;
		}
		meshRenderer.material.renderQueue = renderOrder;
	}

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06002378 RID: 9080 RVA: 0x000DA728 File Offset: 0x000D8928
	// (set) Token: 0x06002379 RID: 9081 RVA: 0x000DA730 File Offset: 0x000D8930
	public bool Highlighted
	{
		get
		{
			return this.isHighlighted;
		}
		set
		{
			this.isHighlighted = value;
			if (this.isHighlighted)
			{
				this.gameObject.renderer.material.color = new Color(0.75f, 0.75f, 0.75f);
			}
			else
			{
				this.gameObject.renderer.material.color = new Color(1f, 1f, 1f);
			}
		}
	}

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x0600237A RID: 9082 RVA: 0x000DA7A8 File Offset: 0x000D89A8
	public static int TileMaximum
	{
		get
		{
			return 36;
		}
	}

	// Token: 0x0600237B RID: 9083 RVA: 0x000DA7AC File Offset: 0x000D89AC
	public void Destroy()
	{
		this.mesh.Clear();
	}

	// Token: 0x0600237C RID: 9084 RVA: 0x000DA7BC File Offset: 0x000D89BC
	private void CreateQuad(float resolution, int row, int col, Rect? coords, byte rotationIndex)
	{
		if (coords == null)
		{
			return;
		}
		Rect value = coords.Value;
		if (this.vertexCount + 4 > this.vertex.Length)
		{
			return;
		}
		if (rotationIndex == 5)
		{
			TerrainSector.rotatedUs[0] = value.xMin;
			TerrainSector.rotatedVs[0] = value.yMax;
			TerrainSector.rotatedUs[1] = value.xMin;
			TerrainSector.rotatedVs[1] = value.yMin;
			TerrainSector.rotatedUs[2] = value.xMax;
			TerrainSector.rotatedVs[2] = value.yMax;
			TerrainSector.rotatedUs[3] = value.xMax;
			TerrainSector.rotatedVs[3] = value.yMin;
		}
		else if (rotationIndex == 6)
		{
			TerrainSector.rotatedUs[0] = value.xMax;
			TerrainSector.rotatedVs[0] = value.yMin;
			TerrainSector.rotatedUs[1] = value.xMax;
			TerrainSector.rotatedVs[1] = value.yMax;
			TerrainSector.rotatedUs[2] = value.xMin;
			TerrainSector.rotatedVs[2] = value.yMin;
			TerrainSector.rotatedUs[3] = value.xMin;
			TerrainSector.rotatedVs[3] = value.yMax;
		}
		else if (rotationIndex == 7)
		{
			TerrainSector.rotatedUs[0] = value.xMax;
			TerrainSector.rotatedVs[0] = value.yMax;
			TerrainSector.rotatedUs[1] = value.xMax;
			TerrainSector.rotatedVs[1] = value.yMin;
			TerrainSector.rotatedUs[2] = value.xMin;
			TerrainSector.rotatedVs[2] = value.yMax;
			TerrainSector.rotatedUs[3] = value.xMin;
			TerrainSector.rotatedVs[3] = value.yMin;
		}
		else if (rotationIndex > 0)
		{
			float x = value.center.x;
			float y = value.center.y;
			TerrainSector.originalUs[0] = (value.xMin - x) * Terrain.terrainTextureInvScaleU;
			TerrainSector.originalVs[0] = (value.yMin - y) * Terrain.terrainTextureInvScaleV;
			TerrainSector.originalUs[1] = (value.xMin - x) * Terrain.terrainTextureInvScaleU;
			TerrainSector.originalVs[1] = (value.yMax - y) * Terrain.terrainTextureInvScaleV;
			TerrainSector.originalUs[2] = (value.xMax - x) * Terrain.terrainTextureInvScaleU;
			TerrainSector.originalVs[2] = (value.yMin - y) * Terrain.terrainTextureInvScaleV;
			TerrainSector.originalUs[3] = (value.xMax - x) * Terrain.terrainTextureInvScaleU;
			TerrainSector.originalVs[3] = (value.yMax - y) * Terrain.terrainTextureInvScaleV;
			float num;
			float num2;
			float num3;
			float num4;
			switch (rotationIndex)
			{
			case 1:
				num = 0f;
				num2 = Terrain.terrainTextureScaleV;
				num3 = -Terrain.terrainTextureScaleU;
				num4 = 0f;
				break;
			case 2:
				num = -Terrain.terrainTextureScaleU;
				num2 = 0f;
				num3 = 0f;
				num4 = -Terrain.terrainTextureScaleV;
				break;
			case 3:
				num = 0f;
				num2 = -Terrain.terrainTextureScaleV;
				num3 = Terrain.terrainTextureScaleU;
				num4 = 0f;
				break;
			default:
				num = 1f;
				num2 = 0f;
				num3 = 0f;
				num4 = 1f;
				break;
			}
			for (int i = 0; i < 4; i++)
			{
				TerrainSector.rotatedUs[i] = TerrainSector.originalUs[i] * num + TerrainSector.originalVs[i] * num3 + x;
				TerrainSector.rotatedVs[i] = TerrainSector.originalUs[i] * num2 + TerrainSector.originalVs[i] * num4 + y;
			}
		}
		else
		{
			TerrainSector.rotatedUs[0] = value.xMin;
			TerrainSector.rotatedVs[0] = value.yMin;
			TerrainSector.rotatedUs[1] = value.xMin;
			TerrainSector.rotatedVs[1] = value.yMax;
			TerrainSector.rotatedUs[2] = value.xMax;
			TerrainSector.rotatedVs[2] = value.yMin;
			TerrainSector.rotatedUs[3] = value.xMax;
			TerrainSector.rotatedVs[3] = value.yMax;
		}
		int num5 = this.CreateVertex((float)col * resolution, (float)row * resolution, TerrainSector.rotatedUs[0], TerrainSector.rotatedVs[0]);
		int num6 = this.CreateVertex((float)col * resolution, (float)(row + 1) * resolution, TerrainSector.rotatedUs[1], TerrainSector.rotatedVs[1]);
		int num7 = this.CreateVertex((float)(col + 1) * resolution, (float)row * resolution, TerrainSector.rotatedUs[2], TerrainSector.rotatedVs[2]);
		int num8 = this.CreateVertex((float)(col + 1) * resolution, (float)(row + 1) * resolution, TerrainSector.rotatedUs[3], TerrainSector.rotatedVs[3]);
		this.index[this.indexCount++] = num7;
		this.index[this.indexCount++] = num6;
		this.index[this.indexCount++] = num5;
		this.index[this.indexCount++] = num6;
		this.index[this.indexCount++] = num7;
		this.index[this.indexCount++] = num8;
	}

	// Token: 0x0600237D RID: 9085 RVA: 0x000DACE0 File Offset: 0x000D8EE0
	private int CreateVertex(float x, float y, float u, float v)
	{
		int num = this.vertexCount;
		this.vertex[num].position.x = x;
		this.vertex[num].position.y = y;
		this.vertex[num].position.z = 0f;
		this.vertex[num].texcoord.x = u;
		this.vertex[num].texcoord.y = v;
		this.vertexCount++;
		return num;
	}

	// Token: 0x0600237E RID: 9086 RVA: 0x000DAD7C File Offset: 0x000D8F7C
	private void UpdateMesh()
	{
		this.mesh.Clear();
		Vector3[] array = new Vector3[this.vertexCount];
		Vector2[] array2 = new Vector2[this.vertexCount];
		int[] array3 = new int[this.indexCount];
		for (int i = 0; i < this.vertexCount; i++)
		{
			array[i] = this.vertex[i].position;
			array2[i] = this.vertex[i].texcoord;
		}
		for (int j = 0; j < this.indexCount; j++)
		{
			array3[j] = this.index[j];
		}
		this.mesh.vertices = array;
		this.mesh.uv = array2;
		this.mesh.triangles = array3;
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x000DAE58 File Offset: 0x000D9058
	public void Initialize(Terrain terrain, int sectorRow, int sectorCol)
	{
		this.indexCount = 0;
		this.vertexCount = 0;
		int num = sectorRow * 6;
		int num2 = sectorCol * 6;
		this.gameObject.transform.position = new Vector3((float)(20 * num2), (float)(20 * num), 0f);
		bool flag = terrain.IsTerrainSectorDisabled(sectorRow, sectorCol);
		for (int i = 0; i < 6; i++)
		{
			int num3 = num + i;
			for (int j = 0; j < 6; j++)
			{
				int terrainIdAt = terrain.GetTerrainIdAt(num + i, num2 + j);
				TerrainType terrainType = terrain.GetTerrainType(terrainIdAt);
				TFUtils.Assert(terrainType != null, "Missing terrain type" + terrainIdAt);
				int num4 = num2 + j;
				string material = terrainType.Material;
				byte b = 0;
				if (terrainType.IsPath())
				{
					byte b2 = 0;
					if (terrain.GetTerrainType(num3 + 1, num4) != null && terrain.GetTerrainType(num3 + 1, num4).IsPath())
					{
						b2 |= 8;
					}
					if (terrain.GetTerrainType(num3, num4 + 1) != null && terrain.GetTerrainType(num3, num4 + 1).IsPath())
					{
						b2 |= 4;
					}
					if (terrain.GetTerrainType(num3 - 1, num4) != null && terrain.GetTerrainType(num3 - 1, num4).IsPath())
					{
						b2 |= 2;
					}
					if (terrain.GetTerrainType(num3, num4 - 1) != null && terrain.GetTerrainType(num3, num4 - 1).IsPath())
					{
						b2 |= 1;
					}
					material = terrainType.GetPathMaterial((int)b2);
				}
				else if (terrainType.IsGrass())
				{
					byte b3 = 0;
					b = 0;
					if (i == 0)
					{
						TerrainType terrainType2 = terrain.GetTerrainType(num3 - 1, num4);
						if (terrainType2 == null || !terrainType2.IsGrass())
						{
							b3 |= 1;
							if (terrain.IsTerrainSectorDisabled(sectorRow - 1, sectorCol))
							{
								b3 |= 4;
							}
						}
					}
					else if (i == 5)
					{
						TerrainType terrainType3 = terrain.GetTerrainType(num3 + 1, num4);
						if (terrainType3 == null || !terrainType3.IsGrass())
						{
							b3 |= 1;
							b |= 5;
							if (terrain.IsTerrainSectorDisabled(sectorRow + 1, sectorCol))
							{
								b3 |= 4;
							}
						}
					}
					if (j == 0)
					{
						TerrainType terrainType4 = terrain.GetTerrainType(num3, num4 - 1);
						if (terrainType4 == null || !terrainType4.IsGrass())
						{
							b3 |= 2;
							if (terrain.IsTerrainSectorDisabled(sectorRow, sectorCol - 1))
							{
								b3 |= 8;
							}
						}
					}
					else if (j == 5)
					{
						TerrainType terrainType5 = terrain.GetTerrainType(num3, num4 + 1);
						if (terrainType5 == null || !terrainType5.IsGrass())
						{
							b3 |= 2;
							b |= 6;
							if (terrain.IsTerrainSectorDisabled(sectorRow, sectorCol + 1))
							{
								b3 |= 8;
							}
						}
					}
					material = terrainType.GetGrassMaterial((TerrainType.GrassBorderType)b3);
				}
				else if (terrainType.IsSand() && flag)
				{
					if (TerrainSector.useRotatedTiles)
					{
						byte b4 = 0;
						if (i == 0 && !terrain.IsTerrainSectorDisabled(sectorRow - 1, sectorCol))
						{
							b4 = 1;
						}
						else if (i == 5 && !terrain.IsTerrainSectorDisabled(sectorRow + 1, sectorCol))
						{
							b4 = 4;
						}
						if (j == 0 && !terrain.IsTerrainSectorDisabled(sectorRow, sectorCol - 1))
						{
							b4 |= 8;
						}
						else if (j == 5 && !terrain.IsTerrainSectorDisabled(sectorRow, sectorCol + 1))
						{
							b4 |= 2;
						}
						byte tileTypeAndRotation = terrain.terrainTextures.GetTileTypeAndRotation(b4);
						byte b5 = (byte)(tileTypeAndRotation >> 3);
						if (b5 == 0 || terrain.GetTerrainType(num3, num4 + 1).Material.ToString().Equals("grass.png") || terrain.GetTerrainType(num3, num4 - 1).Material.ToString().Equals("grass.png") || terrain.GetTerrainType(num3 + 1, num4).Material.ToString().Equals("grass.png") || terrain.GetTerrainType(num3 - 1, num4).Material.ToString().Equals("grass.png"))
						{
							material = "disabled_terrainXX";
						}
						else if (b5 == 1)
						{
							material = "disabled_terrainXL";
						}
						else
						{
							material = "disabled_terrainUL";
						}
						b = (tileTypeAndRotation & 7);
					}
					else
					{
						char c = 'X';
						char c2 = 'X';
						if (i == 0 && !terrain.IsTerrainSectorDisabled(sectorRow - 1, sectorCol))
						{
							c = 'D';
						}
						else if (i == 5 && !terrain.IsTerrainSectorDisabled(sectorRow + 1, sectorCol))
						{
							c = 'U';
						}
						if (j == 0 && !terrain.IsTerrainSectorDisabled(sectorRow, sectorCol - 1))
						{
							c2 = 'L';
						}
						else if (j == 5 && !terrain.IsTerrainSectorDisabled(sectorRow, sectorCol + 1))
						{
							c2 = 'R';
						}
						material = "disabled_terrain" + c + c2;
					}
				}
				else if (terrainType.GetDisabledMaterial() != null && flag)
				{
					material = terrainType.GetDisabledMaterial();
				}
				this.CreateQuad(20f, i, j, terrain.terrainTextures.GetMaterialUVs(material), b);
			}
		}
		this.UpdateMesh();
	}

	// Token: 0x040015B2 RID: 5554
	public const int SECTOR_TILE_SIZE = 6;

	// Token: 0x040015B3 RID: 5555
	private const int INVALID_IMAGE_INDEX = -1;

	// Token: 0x040015B4 RID: 5556
	private const int TILE_VERTEX_COUNT = 4;

	// Token: 0x040015B5 RID: 5557
	private const int TILE_INDEX_COUNT = 6;

	// Token: 0x040015B6 RID: 5558
	private const int SECTOR_TILE_MAXIMUM = 36;

	// Token: 0x040015B7 RID: 5559
	private static TerrainType defaultTerrain;

	// Token: 0x040015B8 RID: 5560
	private GridPosition position;

	// Token: 0x040015B9 RID: 5561
	private int vertexCount;

	// Token: 0x040015BA RID: 5562
	private TerrainVertex[] vertex;

	// Token: 0x040015BB RID: 5563
	private bool isHighlighted;

	// Token: 0x040015BC RID: 5564
	private int indexCount;

	// Token: 0x040015BD RID: 5565
	private int[] index;

	// Token: 0x040015BE RID: 5566
	private GameObject gameObject;

	// Token: 0x040015BF RID: 5567
	private Mesh mesh;

	// Token: 0x040015C0 RID: 5568
	private static bool useRotatedTiles = true;

	// Token: 0x040015C1 RID: 5569
	private static float[] originalUs = new float[4];

	// Token: 0x040015C2 RID: 5570
	private static float[] originalVs = new float[4];

	// Token: 0x040015C3 RID: 5571
	private static float[] rotatedUs = new float[4];

	// Token: 0x040015C4 RID: 5572
	private static float[] rotatedVs = new float[4];
}
