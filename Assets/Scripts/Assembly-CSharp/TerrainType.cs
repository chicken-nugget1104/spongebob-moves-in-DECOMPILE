using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000469 RID: 1129
public class TerrainType
{
	// Token: 0x06002398 RID: 9112 RVA: 0x000DBDC0 File Offset: 0x000D9FC0
	public TerrainType(Dictionary<string, object> data)
	{
		this.id = (byte)TFUtils.LoadInt(data, "id");
		this.cost = (byte)TFUtils.LoadInt(data, "move_cost");
		if (data.ContainsKey("material"))
		{
			this.material = (string)data["material"];
		}
		if (data.ContainsKey("disabled_material"))
		{
			this.disabledMaterial = (string)data["disabled_material"];
		}
		this.distribution = new List<KeyValuePair<int, float>>();
		if (data.ContainsKey("decal_distribution"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["decal_distribution"];
			foreach (string text in dictionary.Keys)
			{
				int key = Convert.ToInt32(text);
				float value = TFUtils.LoadFloat(dictionary, text);
				this.distribution.Add(new KeyValuePair<int, float>(key, value));
			}
		}
		if (data.ContainsKey("can_pave"))
		{
			this.canPave = TFUtils.LoadBool(data, "can_pave");
		}
		else
		{
			this.canPave = true;
		}
		object value2;
		if (data.TryGetValue("main_type", out value2))
		{
			this.mainTypeId = (byte)Convert.ToInt32(value2);
			if (this.mainTypeId == 0)
			{
				this.mainTypeId = this.id;
			}
		}
		else
		{
			this.mainTypeId = this.id;
		}
		int num = this.material.LastIndexOf(".png");
		TFUtils.Assert(num > 0, string.Format("can't find .png in Terrain material name \"{0}\"", this.material));
		string str = this.material.Substring(0, num);
		if (this.IsPath())
		{
			this.borderTypeMaterialNames[0] = str + "XX.png";
			this.borderTypeMaterialNames[1] = str + "UX.png";
			this.borderTypeMaterialNames[2] = str + "XR.png";
			this.borderTypeMaterialNames[3] = str + "UR.png";
			this.borderTypeMaterialNames[4] = str + "OuterCorner.png";
		}
		else if (this.IsGrass())
		{
			for (int i = 0; i < 16; i++)
			{
				this.grassTypeMaterialNames[i] = str + this.grassTypeMaterialNames[i] + ".png";
			}
		}
	}

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06002399 RID: 9113 RVA: 0x000DC0EC File Offset: 0x000DA2EC
	public byte Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x0600239A RID: 9114 RVA: 0x000DC0F4 File Offset: 0x000DA2F4
	public byte Cost
	{
		get
		{
			return this.cost;
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x0600239B RID: 9115 RVA: 0x000DC0FC File Offset: 0x000DA2FC
	public string Material
	{
		get
		{
			return this.material;
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x0600239C RID: 9116 RVA: 0x000DC104 File Offset: 0x000DA304
	public byte MainTypeId
	{
		get
		{
			return this.mainTypeId;
		}
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x000DC10C File Offset: 0x000DA30C
	public bool CanPave()
	{
		return this.canPave;
	}

	// Token: 0x0600239E RID: 9118 RVA: 0x000DC114 File Offset: 0x000DA314
	public bool IsPath()
	{
		return this.mainTypeId == 1;
	}

	// Token: 0x0600239F RID: 9119 RVA: 0x000DC120 File Offset: 0x000DA320
	public bool IsSand()
	{
		return this.mainTypeId == 2;
	}

	// Token: 0x060023A0 RID: 9120 RVA: 0x000DC12C File Offset: 0x000DA32C
	public bool IsMud()
	{
		return this.mainTypeId == 3;
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x000DC138 File Offset: 0x000DA338
	public bool IsGrass()
	{
		return this.mainTypeId == 4;
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x000DC144 File Offset: 0x000DA344
	public bool IsGoo()
	{
		return this.mainTypeId == 5;
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x000DC150 File Offset: 0x000DA350
	public static byte GetPathTypeId()
	{
		return 1;
	}

	// Token: 0x060023A4 RID: 9124 RVA: 0x000DC154 File Offset: 0x000DA354
	public string GetBorderMaterial(TerrainType.TileBorderType borderType)
	{
		return this.borderTypeMaterialNames[(int)borderType];
	}

	// Token: 0x060023A5 RID: 9125 RVA: 0x000DC160 File Offset: 0x000DA360
	public string GetGrassMaterial(TerrainType.GrassBorderType borderType)
	{
		return this.grassTypeMaterialNames[(int)borderType];
	}

	// Token: 0x060023A6 RID: 9126 RVA: 0x000DC16C File Offset: 0x000DA36C
	public string GetPathMaterial(int offset)
	{
		int length = this.material.LastIndexOf(".png");
		string arg = this.material.Substring(0, length);
		return arg + offset + ".png";
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x000DC1AC File Offset: 0x000DA3AC
	public string GetDisabledMaterial()
	{
		return this.disabledMaterial;
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000DC1B4 File Offset: 0x000DA3B4
	public byte GenerateDecal(int seed)
	{
		UnityEngine.Random.seed = seed;
		float num = UnityEngine.Random.value;
		foreach (KeyValuePair<int, float> keyValuePair in this.distribution)
		{
			num -= keyValuePair.Value;
			if (num < 0f)
			{
				return (byte)keyValuePair.Key;
			}
		}
		return byte.MaxValue;
	}

	// Token: 0x040015DD RID: 5597
	private const byte TERRAIN_TYPE_PATH = 1;

	// Token: 0x040015DE RID: 5598
	private const byte TERRAIN_TYPE_SAND = 2;

	// Token: 0x040015DF RID: 5599
	private const byte TERRAIN_TYPE_MUD = 3;

	// Token: 0x040015E0 RID: 5600
	private const byte TERRAIN_TYPE_GRASS = 4;

	// Token: 0x040015E1 RID: 5601
	private const byte TERRAIN_TYPE_GOO = 5;

	// Token: 0x040015E2 RID: 5602
	private readonly string[] grassTypeMaterialNames = new string[]
	{
		"XXXX",
		"XLXL",
		"LXUX",
		"LLUL",
		"XXXX",
		"XDXL",
		"LXUX",
		"LDUL",
		"XXXX",
		"XLXL",
		"DXUX",
		"DLUL",
		"XXXX",
		"XDXL",
		"DXUX",
		"DDUL"
	};

	// Token: 0x040015E3 RID: 5603
	private byte id;

	// Token: 0x040015E4 RID: 5604
	private byte cost;

	// Token: 0x040015E5 RID: 5605
	private byte mainTypeId;

	// Token: 0x040015E6 RID: 5606
	private string material;

	// Token: 0x040015E7 RID: 5607
	private string disabledMaterial;

	// Token: 0x040015E8 RID: 5608
	private List<KeyValuePair<int, float>> distribution;

	// Token: 0x040015E9 RID: 5609
	private string[] borderTypeMaterialNames = new string[5];

	// Token: 0x040015EA RID: 5610
	private bool canPave;

	// Token: 0x0200046A RID: 1130
	[Flags]
	public enum TileBorderType
	{
		// Token: 0x040015EC RID: 5612
		XX = 0,
		// Token: 0x040015ED RID: 5613
		UX = 1,
		// Token: 0x040015EE RID: 5614
		XR = 2,
		// Token: 0x040015EF RID: 5615
		UR = 3,
		// Token: 0x040015F0 RID: 5616
		OuterCorner = 4,
		// Token: 0x040015F1 RID: 5617
		MAX = 5
	}

	// Token: 0x0200046B RID: 1131
	public enum GrassBorderType
	{
		// Token: 0x040015F3 RID: 5619
		LLXX,
		// Token: 0x040015F4 RID: 5620
		LLXL,
		// Token: 0x040015F5 RID: 5621
		LLUX,
		// Token: 0x040015F6 RID: 5622
		LLUL,
		// Token: 0x040015F7 RID: 5623
		LDXX,
		// Token: 0x040015F8 RID: 5624
		LDXL,
		// Token: 0x040015F9 RID: 5625
		LDUX,
		// Token: 0x040015FA RID: 5626
		LDUL,
		// Token: 0x040015FB RID: 5627
		DLXX,
		// Token: 0x040015FC RID: 5628
		DLXL,
		// Token: 0x040015FD RID: 5629
		DLUX,
		// Token: 0x040015FE RID: 5630
		DLUL,
		// Token: 0x040015FF RID: 5631
		DDXX,
		// Token: 0x04001600 RID: 5632
		DDXL,
		// Token: 0x04001601 RID: 5633
		DDUX,
		// Token: 0x04001602 RID: 5634
		DDUL,
		// Token: 0x04001603 RID: 5635
		MAX
	}
}
