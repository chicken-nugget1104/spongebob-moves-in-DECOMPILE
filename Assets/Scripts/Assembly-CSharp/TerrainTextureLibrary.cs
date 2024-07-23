using System;
using UnityEngine;
using Yarg;

// Token: 0x02000468 RID: 1128
public class TerrainTextureLibrary
{
	// Token: 0x06002394 RID: 9108 RVA: 0x000DBBA8 File Offset: 0x000D9DA8
	public TerrainTextureLibrary()
	{
		TFUtils.DebugLog("Loading Terrain Texture file: " + TerrainTextureLibrary.terrainAtlasFile);
		string json = TFUtils.ReadAllText(TerrainTextureLibrary.terrainAtlasFile);
		this.atlas = TextureAtlas.LoadJsonAtlas(json);
		this.tileEdges[0] = 0;
		this.tileEdges[1] = 11;
		this.tileEdges[2] = 10;
		this.tileEdges[3] = 18;
		this.tileEdges[4] = 9;
		this.tileEdges[5] = this.tileEdges[0];
		this.tileEdges[6] = 17;
		this.tileEdges[7] = this.tileEdges[0];
		this.tileEdges[8] = 8;
		this.tileEdges[9] = 19;
		this.tileEdges[10] = this.tileEdges[0];
		this.tileEdges[11] = this.tileEdges[0];
		this.tileEdges[12] = 16;
		this.tileEdges[13] = this.tileEdges[0];
		this.tileEdges[14] = this.tileEdges[0];
		this.tileEdges[15] = this.tileEdges[0];
	}

	// Token: 0x06002396 RID: 9110 RVA: 0x000DBCDC File Offset: 0x000D9EDC
	public Rect? GetMaterialUVs(string material)
	{
		AtlasCoords atlasCoords = this.atlas[material];
		if (atlasCoords != null)
		{
			float x = this.atlas.meta.invScale.x;
			float y = this.atlas.meta.invScale.y;
			return new Rect?(new Rect
			{
				x = atlasCoords.frame.x * x,
				width = atlasCoords.frame.width * x,
				y = (this.atlas.meta.size.height - (atlasCoords.frame.y + atlasCoords.frame.height)) * y,
				height = atlasCoords.frame.height * y
			});
		}
		return null;
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x000DBDB4 File Offset: 0x000D9FB4
	public byte GetTileTypeAndRotation(byte index)
	{
		return this.tileEdges[(int)index];
	}

	// Token: 0x040015D5 RID: 5589
	public const byte TT_INTERNAL = 0;

	// Token: 0x040015D6 RID: 5590
	public const byte TT_EDGE = 1;

	// Token: 0x040015D7 RID: 5591
	public const byte TT_CORNER = 2;

	// Token: 0x040015D8 RID: 5592
	public const byte TT_TYPE_SHIFT = 3;

	// Token: 0x040015D9 RID: 5593
	public const byte TT_ROTATION_MASK = 7;

	// Token: 0x040015DA RID: 5594
	private byte[] tileEdges = new byte[16];

	// Token: 0x040015DB RID: 5595
	private TextureAtlas atlas;

	// Token: 0x040015DC RID: 5596
	private static string terrainAtlasFile = TFUtils.GetStreamingAssetsFileInDirectory("Terrain", "terrainsheet.txt");
}
