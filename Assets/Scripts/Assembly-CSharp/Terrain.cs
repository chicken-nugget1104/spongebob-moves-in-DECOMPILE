using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000464 RID: 1124
public class Terrain
{
	// Token: 0x06002322 RID: 8994 RVA: 0x000D6FB0 File Offset: 0x000D51B0
	public Terrain(int terrainSeed)
	{
		this.terrainSeed = terrainSeed;
		this.terrainTextures = new TerrainTextureLibrary();
		this.LoadTerrain();
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06002324 RID: 8996 RVA: 0x000D7054 File Offset: 0x000D5254
	public static Material TerrainMaterial
	{
		get
		{
			if (Terrain.mTerrainMaterial == null)
			{
				if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
				{
					Terrain.mTerrainMaterial = (Resources.Load("Materials/lod/terrainsheet_lr") as Material);
				}
				else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
				{
					Terrain.mTerrainMaterial = (Resources.Load("Materials/lod/terrainsheet_lr2") as Material);
				}
				else
				{
					Terrain.mTerrainMaterial = (Resources.Load("Materials/lod/terrainsheet") as Material);
				}
			}
			return Terrain.mTerrainMaterial;
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06002325 RID: 8997 RVA: 0x000D70D4 File Offset: 0x000D52D4
	public TerrainType BackgroundTerrainType
	{
		get
		{
			return this.terrainTypes[(int)this.backgroundTerrain];
		}
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06002326 RID: 8998 RVA: 0x000D70E8 File Offset: 0x000D52E8
	public int GridWidth
	{
		get
		{
			return this.sectorWidth * 6;
		}
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06002327 RID: 8999 RVA: 0x000D70F4 File Offset: 0x000D52F4
	public int GridHeight
	{
		get
		{
			return this.sectorHeight * 6;
		}
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06002328 RID: 9000 RVA: 0x000D7100 File Offset: 0x000D5300
	public int WorldWidth
	{
		get
		{
			return this.GridWidth * 20;
		}
	}

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06002329 RID: 9001 RVA: 0x000D710C File Offset: 0x000D530C
	public int WorldHeight
	{
		get
		{
			return this.GridHeight * 20;
		}
	}

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x0600232A RID: 9002 RVA: 0x000D7118 File Offset: 0x000D5318
	public AlignedBox WorldExtent
	{
		get
		{
			return this.worldExtent;
		}
	}

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x0600232B RID: 9003 RVA: 0x000D7120 File Offset: 0x000D5320
	public AlignedBox PurchasedExtent
	{
		get
		{
			return this.purchasedExtent;
		}
	}

	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x0600232C RID: 9004 RVA: 0x000D7128 File Offset: 0x000D5328
	public AlignedBox CameraExtents
	{
		get
		{
			return this.cameraExtents;
		}
	}

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x0600232E RID: 9006 RVA: 0x000D713C File Offset: 0x000D533C
	// (set) Token: 0x0600232D RID: 9005 RVA: 0x000D7130 File Offset: 0x000D5330
	public AlignedBox FootprintGuide
	{
		get
		{
			return this.footprintGuide;
		}
		set
		{
			this.footprintGuide = value;
		}
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x000D7144 File Offset: 0x000D5344
	private void LoadTerrain()
	{
		this.LoadTerrainFromSpread();
		this.LoadTerrainSlotsFromSpread();
		this.LoadTerrainTypesFromSpread();
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x000D7158 File Offset: 0x000D5358
	private void LoadTerrainFromSpread()
	{
		string text = "Terrain";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "terrain");
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "expansion gold costs");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "max width");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "max height");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "background terrain did");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "camera frame x min");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "camera frame x max");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "camera frame y min");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "camera frame y max");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "camera color r");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "camera color g");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "camera color b");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "camera alpha");
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				}
				dictionary.Add("max_height", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4));
				dictionary.Add("max_width", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet3));
				dictionary.Add("background_terrain", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
				dictionary.Add("camera_frame", new Dictionary<string, object>
				{
					{
						"xMin",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6)
					},
					{
						"xMax",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7)
					},
					{
						"yMin",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8)
					},
					{
						"yMax",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9)
					}
				});
				dictionary.Add("camera_color", new Dictionary<string, object>
				{
					{
						"r",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10)
					},
					{
						"g",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet11)
					},
					{
						"b",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12)
					},
					{
						"a",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13)
					}
				});
				dictionary.Add("expansion_costs", new List<object>());
				for (int j = 1; j <= num2; j++)
				{
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, "expansion gold cost " + j.ToString());
					if (intCell > 0)
					{
						((List<object>)dictionary["expansion_costs"]).Add(new Dictionary<string, object>
						{
							{
								"3",
								intCell
							}
						});
					}
				}
				dictionary.Add("terrain_dist", new Dictionary<string, object>());
				dictionary.Add("background_tiles", new List<object>());
				dictionary.Add("foreground_tiles", new List<object>());
				dictionary.Add("decal_tiles", new List<object>());
			}
		}
		text = "TerrainForeground";
		sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "terrain type did");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "position x");
		int columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "position y");
		for (int k = 0; k < num; k++)
		{
			string rowName = k.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				((List<object>)dictionary["foreground_tiles"]).Add(new Dictionary<string, object>(3)
				{
					{
						"did",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet14)
					},
					{
						"x",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15)
					},
					{
						"y",
						instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet16)
					}
				});
			}
		}
		this.LoadTerrain(dictionary);
	}

	// Token: 0x06002331 RID: 9009 RVA: 0x000D7660 File Offset: 0x000D5860
	private void LoadTerrainSlotsFromSpread()
	{
		string text = "TerrainSlots";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		Dictionary<int, Dictionary<string, object>> dictionary = new Dictionary<int, Dictionary<string, object>>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "sector row");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "sector column");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "cost multiplier");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "row");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "column");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "is boardwalk");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "cost jelly");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "cost gold");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "required slots");
		string b = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2);
				if (dictionary.ContainsKey(num2))
				{
					dictionary2 = dictionary[num2];
					num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet3);
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4);
					if (num2 >= 0 && intCell >= 0)
					{
						((List<object>)dictionary2["sectors"]).Add(new Dictionary<string, object>
						{
							{
								"row",
								num2
							},
							{
								"col",
								intCell
							}
						});
					}
				}
				else
				{
					dictionary2 = new Dictionary<string, object>();
					dictionary.Add(num2, dictionary2);
					dictionary2.Add("type", "slot");
					dictionary2.Add("did", num2);
					dictionary2.Add("tier", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5));
					dictionary2.Add("row", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6));
					dictionary2.Add("col", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7));
					dictionary2.Add("is_boardwalk", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8) == 1);
					dictionary2.Add("cost", new Dictionary<string, object>());
					num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9);
					if (num2 > 0)
					{
						((Dictionary<string, object>)dictionary2["cost"]).Add("2", num2);
					}
					num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10);
					if (num2 > 0)
					{
						((Dictionary<string, object>)dictionary2["cost"]).Add("3", num2);
					}
					dictionary2.Add("sectors", new List<object>());
					num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet3);
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet4);
					if (num2 >= 0 && intCell >= 0)
					{
						((List<object>)dictionary2["sectors"]).Add(new Dictionary<string, object>
						{
							{
								"row",
								num2
							},
							{
								"col",
								intCell
							}
						});
					}
					dictionary2.Add("outline", new List<object>());
					string stringCell;
					for (int j = 1; j < 5; j++)
					{
						stringCell = instance.GetStringCell(sheetIndex, rowIndex, "outline corner " + j.ToString());
						if (!string.IsNullOrEmpty(stringCell) && !(stringCell == b))
						{
							string[] array = stringCell.Split(new char[]
							{
								'|'
							});
							num2 = array.Length;
							if (num2 == 2)
							{
								bool flag = int.TryParse(array[0], out num2);
								if (flag)
								{
									flag = int.TryParse(array[1], out intCell);
								}
								if (flag)
								{
									((List<object>)dictionary2["outline"]).Add(new Dictionary<string, object>
									{
										{
											"row",
											num2
										},
										{
											"col",
											intCell
										}
									});
								}
							}
						}
					}
					dictionary2.Add("required_slots", new List<object>());
					stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet11);
					if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
					{
						string[] array = stringCell.Split(new char[]
						{
							'|'
						});
						num2 = array.Length;
						for (int k = 0; k < num2; k++)
						{
							if (int.TryParse(array[k], out intCell))
							{
								((List<object>)dictionary2["required_slots"]).Add(intCell);
							}
						}
					}
					dictionary2.Add("debris", new List<object>());
					dictionary2.Add("landmarks", new List<object>());
				}
			}
		}
		text = "DebrisPlacement";
		sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "slot did");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "debris did");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "label");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "position x");
		int columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "position y");
		for (int l = 0; l < num; l++)
		{
			string rowName = l.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12);
				if (dictionary.ContainsKey(num2))
				{
					dictionary2 = dictionary[num2];
					if (!dictionary2.ContainsKey("debris"))
					{
						dictionary2.Add("debris", new List<object>());
					}
					((List<object>)dictionary2["debris"]).Add(new Dictionary<string, object>(4)
					{
						{
							"did",
							instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet13)
						},
						{
							"label",
							instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet14)
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15)
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet16)
						}
					});
				}
			}
		}
		text = "LandmarkPlacement";
		sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "slot did");
		int columnIndexInSheet17 = instance.GetColumnIndexInSheet(sheetIndex, "landmark did");
		columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "label");
		columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "position x");
		columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "position y");
		for (int m = 0; m < num; m++)
		{
			string rowName = m.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, columnIndexInSheet).ToString());
				int num2 = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet12);
				if (dictionary.ContainsKey(num2))
				{
					dictionary2 = dictionary[num2];
					if (!dictionary2.ContainsKey("landmarks"))
					{
						dictionary2.Add("landmarks", new List<object>());
					}
					((List<object>)dictionary2["landmarks"]).Add(new Dictionary<string, object>(4)
					{
						{
							"did",
							instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet17)
						},
						{
							"label",
							instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet14)
						},
						{
							"x",
							instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet15)
						},
						{
							"y",
							instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet16)
						}
					});
				}
			}
		}
		foreach (KeyValuePair<int, Dictionary<string, object>> keyValuePair in dictionary)
		{
			TerrainSlot terrainSlot = new TerrainSlot(keyValuePair.Value);
			this.slots.Add(terrainSlot.Id, terrainSlot);
		}
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x000D7FAC File Offset: 0x000D61AC
	private void LoadTerrainTypesFromSpread()
	{
		string text = "TerrainTypes";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null)
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string b = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				dictionary.Clear();
				dictionary.Add("type", "type");
				dictionary.Add("id", instance.GetIntCell(text, rowName, "type did"));
				dictionary.Add("move_cost", instance.GetIntCell(text, rowName, "move cost"));
				dictionary.Add("name", instance.GetStringCell(text, rowName, "name"));
				dictionary.Add("can_pave", instance.GetIntCell(text, rowName, "can pave") == 1);
				int intCell = instance.GetIntCell(text, rowName, "derives from type did");
				if (intCell >= 0)
				{
					dictionary.Add("main_type", intCell);
				}
				string stringCell = instance.GetStringCell(text, rowName, "material");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("material", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "disabled material");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("disabled_material", stringCell);
				}
				TerrainType terrainType = new TerrainType(dictionary);
				this.terrainTypes.Add((int)terrainType.Id, terrainType);
			}
		}
	}

	// Token: 0x06002333 RID: 9011 RVA: 0x000D81BC File Offset: 0x000D63BC
	private void LoadTerrain(Dictionary<string, object> data)
	{
		this.sectorWidth = TFUtils.LoadInt(data, "max_width");
		this.sectorHeight = TFUtils.LoadInt(data, "max_height");
		this.backgroundTerrain = (byte)TFUtils.LoadInt(data, "background_terrain");
		this.sectorInset = new Rect(1f, 1f, (float)(this.sectorWidth - 1), (float)(this.sectorHeight - 1));
		if (data.ContainsKey("camera_frame"))
		{
			Dictionary<string, object> d = data["camera_frame"] as Dictionary<string, object>;
			this.sectorInset.xMin = (float)TFUtils.LoadInt(d, "xMin");
			this.sectorInset.xMax = (float)TFUtils.LoadInt(d, "xMax");
			this.sectorInset.yMin = (float)TFUtils.LoadInt(d, "yMin");
			this.sectorInset.yMax = (float)TFUtils.LoadInt(d, "yMax");
		}
		if (data.ContainsKey("camera_color"))
		{
			Color backgroundColor = default(Color);
			Dictionary<string, object> d2 = data["camera_color"] as Dictionary<string, object>;
			int num = TFUtils.LoadInt(d2, "r");
			if (num != 0)
			{
				backgroundColor.r = (float)num / 256f;
			}
			else
			{
				backgroundColor.r = 0f;
			}
			num = TFUtils.LoadInt(d2, "g");
			if (num != 0)
			{
				backgroundColor.g = (float)num / 256f;
			}
			else
			{
				backgroundColor.g = 0f;
			}
			num = TFUtils.LoadInt(d2, "b");
			if (num != 0)
			{
				backgroundColor.b = (float)num / 256f;
			}
			else
			{
				backgroundColor.b = 0f;
			}
			num = TFUtils.LoadInt(d2, "a");
			if (num != 0)
			{
				backgroundColor.a = (float)num / 256f;
			}
			else
			{
				backgroundColor.a = 0f;
			}
			Camera.main.backgroundColor = backgroundColor;
		}
		this.distribution = new List<KeyValuePair<int, float>>();
		Dictionary<string, object> dictionary = data["terrain_dist"] as Dictionary<string, object>;
		foreach (string text in dictionary.Keys)
		{
			int key = Convert.ToInt32(text);
			float value = TFUtils.LoadFloat(dictionary, text);
			this.distribution.Add(new KeyValuePair<int, float>(key, value));
		}
		this.expansionCosts = new List<Cost>();
		List<object> list = (List<object>)data["expansion_costs"];
		foreach (object obj in list)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)obj;
			this.expansionCosts.Add(Cost.FromDict(dict));
		}
		this.foregroundOverrides = Terrain.LoadTerrainNodeData((List<object>)data["foreground_tiles"]);
		this.worldExtent = new AlignedBox(0f, (float)this.WorldWidth, 0f, (float)this.WorldHeight);
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x000D84FC File Offset: 0x000D66FC
	public void Initialize()
	{
		this.tiles = new byte[this.GridHeight, this.GridWidth];
		this.nonPathTiles = new byte[this.GridHeight, this.GridWidth];
		this.obstacles = new bool[this.GridHeight, this.GridWidth];
		this.purchasedSectors = new bool[this.sectorHeight, this.sectorWidth];
		bool flag = this.distribution.Count == 0;
		for (int i = 0; i < this.GridHeight; i++)
		{
			for (int j = 0; j < this.GridWidth; j++)
			{
				if (flag)
				{
					this.tiles[i, j] = this.backgroundTerrain;
				}
				else
				{
					byte b = this.GenerateTerrainTile(i, j);
					if (b == 255)
					{
						this.tiles[i, j] = this.backgroundTerrain;
					}
				}
			}
		}
		this.ProcessOverrides();
		this.Decal();
		for (int k = 0; k < this.GridHeight; k++)
		{
			for (int l = 0; l < this.GridWidth; l++)
			{
				this.nonPathTiles[k, l] = this.tiles[k, l];
			}
		}
		this.sectors = new TerrainSector[this.sectorHeight, this.sectorWidth];
		for (int m = 0; m < this.sectorHeight; m++)
		{
			for (int n = 0; n < this.sectorWidth; n++)
			{
				this.sectors[m, n] = new TerrainSector(0, m, n);
			}
		}
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000D86B0 File Offset: 0x000D68B0
	public void CreateTerrainMeshes()
	{
		for (int i = 0; i < this.sectorHeight; i++)
		{
			for (int j = 0; j < this.sectorWidth; j++)
			{
				this.sectors[i, j].Initialize(this, i, j);
			}
		}
		this.meshesCreated = true;
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000D8708 File Offset: 0x000D6908
	private string[] GetFilesToLoad()
	{
		return Config.TERRAIN_PATH;
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000D8710 File Offset: 0x000D6910
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000D8714 File Offset: 0x000D6914
	public void Destroy()
	{
		TerrainSector[,] array = this.sectors;
		int length = array.GetLength(0);
		int length2 = array.GetLength(1);
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				TerrainSector terrainSector = array[i, j];
				if (terrainSector != null)
				{
					terrainSector.Destroy();
				}
			}
		}
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000D8778 File Offset: 0x000D6978
	public bool ChangePath(GridPosition gpos)
	{
		TerrainType terrainType = this.GetTerrainType(gpos);
		if (terrainType == null)
		{
			return false;
		}
		if (terrainType.IsPath())
		{
			this.tiles[gpos.row, gpos.col] = this.nonPathTiles[gpos.row, gpos.col];
		}
		else
		{
			byte b = this.tiles[gpos.row, gpos.col];
			if (b > 60 || b == 6)
			{
				return false;
			}
			this.tiles[gpos.row, gpos.col] = TerrainType.GetPathTypeId();
			byte b2 = this.GenerateDecal(gpos.row, gpos.col);
			if (b2 != 255)
			{
				this.tiles[gpos.row, gpos.col] = b2;
			}
		}
		this.UpdateSectors(gpos.row, gpos.col);
		if (PathFinder2.IsInitialized())
		{
			PathFinder2.UpdateCost(gpos.row, gpos.col, this.GetTerrainCost(gpos.row, gpos.col));
		}
		return true;
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000D8890 File Offset: 0x000D6A90
	public GridPosition ComputeGridPosition(Vector2 worldPosition)
	{
		return new GridPosition((int)(worldPosition.y / 20f), (int)(worldPosition.x / 20f));
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x000D88B4 File Offset: 0x000D6AB4
	public Vector2 ComputeWorldPosition(GridPosition gridPosition)
	{
		return new Vector2(((float)gridPosition.col + 0.5f) * 20f, ((float)gridPosition.row + 0.5f) * 20f);
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000D88E4 File Offset: 0x000D6AE4
	public Vector3 ConstrainToAlignedBox(Vector3 position, AlignedBox footprint)
	{
		if (position.x < this.worldExtent.xmin)
		{
			position.x = this.worldExtent.xmin;
		}
		else if (position.x > this.worldExtent.xmax - footprint.xmax)
		{
			position.x = this.worldExtent.xmax - footprint.xmax;
		}
		if (position.y < this.worldExtent.ymin)
		{
			position.y = this.worldExtent.ymin;
		}
		else if (position.y > this.worldExtent.ymax - footprint.ymax)
		{
			position.y = this.worldExtent.ymax - footprint.ymax;
		}
		return position;
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000D89BC File Offset: 0x000D6BBC
	public Vector3 CalculateNearestGridPosition(Vector3 position, AlignedBox footprint)
	{
		return this.ConstrainToAlignedBox(new Vector3(0f, 0f, 0f)
		{
			x = (float)Math.Round((double)(position.x / 20f)) * 20f,
			y = (float)Math.Round((double)(position.y / 20f)) * 20f
		}, footprint);
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000D8A2C File Offset: 0x000D6C2C
	public bool ComputeIntersection(Ray ray, out Vector3 point)
	{
		point = ray.origin;
		float num = Vector3.Dot(ray.direction, Terrain.UP);
		if (num * num < 1.0000001E-06f)
		{
			return false;
		}
		float num2 = -1f / num;
		float num3 = num2 * Vector3.Dot(ray.origin, Terrain.UP);
		if (num3 < 0f)
		{
			return false;
		}
		point = ray.origin + num3 * ray.direction;
		return true;
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x000D8AB4 File Offset: 0x000D6CB4
	public byte GetTerrainCost(int row, int col)
	{
		if (this.HasObstacle(row, col))
		{
			return 120;
		}
		TerrainType terrainType = this.GetTerrainType(row, col);
		if (terrainType == null)
		{
			return byte.MaxValue;
		}
		byte b = 0;
		if (!this.purchasedSectors[row / 6, col / 6])
		{
			b = 20;
		}
		return terrainType.Cost + b;
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x000D8B0C File Offset: 0x000D6D0C
	public float GetTerrainCost(GridPosition gridPosition)
	{
		return (float)this.GetTerrainCost(gridPosition.row, gridPosition.col);
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000D8B24 File Offset: 0x000D6D24
	public float GetTerrainCost(Vector2 worldPosition)
	{
		return this.GetTerrainCost(this.ComputeGridPosition(worldPosition));
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x000D8B34 File Offset: 0x000D6D34
	public void SetOrClearObstacle(AlignedBox box, bool isSet)
	{
		float num = (float)(20 * (int)(box.ymin / 20f));
		float num2 = box.ymin;
		bool flag = false;
		while (!flag)
		{
			float num3 = num + 20f;
			float num4;
			if (box.ymax > num3)
			{
				num4 = num3 - num2;
				num = num3;
			}
			else
			{
				num4 = box.ymax - num2;
				flag = true;
			}
			float num5 = (float)(20 * (int)(box.xmin / 20f));
			float num6 = box.xmin;
			for (;;)
			{
				float num7 = num5 + 20f;
				if (box.xmax <= num7)
				{
					break;
				}
				float num8 = num7 - num6;
				if (num8 * num4 >= 200f)
				{
					this.SetObstacleAtCoords(num6, num2, isSet);
				}
				num6 = num7;
				num5 = num7;
			}
			float num9 = box.xmax - num6;
			if (num9 * num4 >= 200f)
			{
				this.SetObstacleAtCoords(num6, num2, isSet);
			}
			num2 = num3;
		}
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000D8C30 File Offset: 0x000D6E30
	private void SetObstacleAtCoords(float x, float y, bool isSet)
	{
		GridPosition gridPosition = this.ComputeGridPosition(new Vector2(x, y));
		gridPosition.MakeValid(this.GridHeight - 1, this.GridWidth - 1);
		this.obstacles[gridPosition.row, gridPosition.col] = isSet;
		if (PathFinder2.IsInitialized())
		{
			PathFinder2.UpdateCost(gridPosition.row, gridPosition.col, this.GetTerrainCost(gridPosition.row, gridPosition.col));
		}
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000D8CA8 File Offset: 0x000D6EA8
	public bool CheckIsPurchasedArea(AlignedBox box)
	{
		int num = 6;
		GridPosition gridPosition = this.ComputeGridPosition(new Vector2(box.xmin, box.ymin));
		GridPosition gridPosition2 = this.ComputeGridPosition(new Vector2(box.xmax, box.ymax));
		gridPosition = new GridPosition(gridPosition.row / num, gridPosition.col / num);
		gridPosition2 = new GridPosition((gridPosition2.row - 1) / num, (gridPosition2.col - 1) / num);
		if (!this.ValidSectorIndex(gridPosition) || !this.ValidSectorIndex(gridPosition2))
		{
			return false;
		}
		for (int i = gridPosition.row; i <= gridPosition2.row; i++)
		{
			for (int j = gridPosition.col; j <= gridPosition2.col; j++)
			{
				if (!this.purchasedSectors[i, j])
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000D8D84 File Offset: 0x000D6F84
	public bool CheckIsPurchasedArea(Vector2 point)
	{
		int num = 120;
		int num2 = (int)point.y / num;
		int num3 = (int)point.x / num;
		return this.ValidSectorIndex(num2, num3) && this.purchasedSectors[num2, num3];
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000D8DC8 File Offset: 0x000D6FC8
	public bool CheckIsPurchasedArea(int row, int col)
	{
		return this.purchasedSectors[row / 6, col / 6];
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000D8DDC File Offset: 0x000D6FDC
	public void MarkPurchase(TerrainSlot slot)
	{
		foreach (GridPosition gridPosition in slot.sectors)
		{
			this.purchasedSectors[gridPosition.row, gridPosition.col] = true;
			if (this.ValidSectorIndex(gridPosition))
			{
				this.sectors[gridPosition.row, gridPosition.col].Highlighted = false;
				this.UpdateAllSurroundingSectors(gridPosition.row, gridPosition.col);
				if (this.purchasedExtent == null)
				{
					this.purchasedExtent = this.GetSectorBounds(gridPosition.row, gridPosition.col);
					this.cameraExtents = this.GetCameraBounds(gridPosition.row, gridPosition.col);
				}
				else
				{
					this.purchasedExtent = AlignedBox.Union(this.purchasedExtent, this.GetSectorBounds(gridPosition.row, gridPosition.col));
					this.cameraExtents = AlignedBox.Union(this.cameraExtents, this.GetCameraBounds(gridPosition.row, gridPosition.col));
				}
				if (PathFinder2.IsInitialized())
				{
					int num = gridPosition.row * 6;
					int num2 = num + 6;
					int num3 = gridPosition.col * 6;
					int num4 = num3 + 6;
					for (int i = num; i < num2; i++)
					{
						for (int j = num3; j < num4; j++)
						{
							PathFinder2.UpdateCost(i, j, this.GetTerrainCost(i, j));
						}
					}
				}
			}
		}
		this.selectedSlot = null;
		slot.ClearOutline();
		slot.ClearSign();
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000D8F90 File Offset: 0x000D7190
	private AlignedBox GetSectorBounds(int row, int col)
	{
		return new AlignedBox((float)(col * 6 * 20), (float)((col + 1) * 6 * 20), (float)(row * 6 * 20), (float)((row + 1) * 6 * 20));
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x000D8FB8 File Offset: 0x000D71B8
	private AlignedBox GetCameraBounds(int row, int col)
	{
		int num = (int)this.sectorInset.xMax;
		int num2 = (int)this.sectorInset.yMax;
		int num3 = (int)this.sectorInset.xMin;
		int num4 = (int)this.sectorInset.yMin;
		if (row > num2)
		{
			row = num2;
		}
		else if (row < num4)
		{
			row = num4;
		}
		if (col > num)
		{
			col = num;
		}
		else if (col < num3)
		{
			col = num3;
		}
		return new AlignedBox((float)(col * 6 * 20), (float)((col + 1) * 6 * 20), (float)(row * 6 * 20), (float)((row + 1) * 6 * 20));
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x000D9050 File Offset: 0x000D7250
	public AlignedBox GetGridBounds(int row, int col)
	{
		return new AlignedBox((float)(col * 20), (float)((col + 1) * 20), (float)(row * 20), (float)((row + 1) * 20));
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000D9070 File Offset: 0x000D7270
	public void AddExpansionSlot(int id)
	{
		if (this.slots.ContainsKey(id))
		{
			this.purchasedSlots.Add(id);
			this.MarkPurchase(this.slots[id]);
		}
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x000D90B0 File Offset: 0x000D72B0
	public void AddRandomAvailableSlot(Game game)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, TerrainSlot> keyValuePair in this.slots)
		{
			if (!this.purchasedSlots.Contains(keyValuePair.Key))
			{
				if (keyValuePair.Value.Available(this.purchasedSlots, game))
				{
					bool flag = false;
					foreach (GridPosition gridPosition in keyValuePair.Value.sectors)
					{
						flag = this.IsTerrainSectorDisabled(gridPosition.row, gridPosition.col);
						if (!flag)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						list.Add(keyValuePair.Key);
					}
				}
			}
		}
		if (list.Count < 0)
		{
			TFUtils.DebugLog("Warning: Add Random Available Slot has no available slot to add");
			return;
		}
		int num = list[UnityEngine.Random.Range(0, list.Count)];
		this.purchasedSlots.Add(num);
		TerrainSlot terrainSlot = this.slots[num];
		this.MarkPurchase(terrainSlot);
		if (game.featureManager.CheckFeature("purchase_expansions"))
		{
			game.terrain.UpdateRealtySigns(game.entities.DisplayControllerManager, new BillboardDelegate(SBCamera.BillboardDefinition), game);
		}
		if (game.terrain.IsBorderSlot(terrainSlot.Id))
		{
			game.border.UpdateTerrainBorderStrip(game.terrain);
		}
		foreach (TerrainSlotObject terrainSlotObject in terrainSlot.landmarks)
		{
			game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), terrainSlotObject.id));
		}
		foreach (TerrainSlotObject terrainSlotObject2 in terrainSlot.debris)
		{
			game.simulation.Router.Send(PurchaseCommand.Create(Identity.Null(), terrainSlotObject2.id));
		}
		game.ModifyGameState(new NewExpansionAction(terrainSlot.Id, new Cost(), terrainSlot.debris, terrainSlot.landmarks));
		AnalyticsWrapper.LogExpansion(game, terrainSlot.Id, null);
		TFUtils.DebugLog("Rewared Random Expansion Slot: " + terrainSlot.Id, TFUtils.LogFilter.Terrain);
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x000D93C0 File Offset: 0x000D75C0
	public void AddAndClearExpansionSlot(Game pGame, int nDID)
	{
		if (!this.slots.ContainsKey(nDID))
		{
			TFUtils.ErrorLog("AddAndClearExpansionSlot | no slot with id: " + nDID);
			return;
		}
		TerrainSlot terrainSlot = this.slots[nDID];
		if (!this.purchasedSlots.Contains(nDID))
		{
			this.purchasedSlots.Add(nDID);
			this.MarkPurchase(terrainSlot);
			if (pGame.featureManager.CheckFeature("purchase_expansions"))
			{
				pGame.terrain.UpdateRealtySigns(pGame.entities.DisplayControllerManager, new BillboardDelegate(SBCamera.BillboardDefinition), pGame);
			}
			if (pGame.terrain.IsBorderSlot(terrainSlot.Id))
			{
				pGame.border.UpdateTerrainBorderStrip(pGame.terrain);
			}
			pGame.ModifyGameState(new NewExpansionAction(terrainSlot.Id, new Cost(), new List<TerrainSlotObject>(), new List<TerrainSlotObject>()));
			AnalyticsWrapper.LogExpansion(pGame, terrainSlot.Id, null);
		}
		foreach (TerrainSlotObject terrainSlotObject in terrainSlot.landmarks)
		{
			pGame.entities.Destroy(terrainSlotObject.id);
			Simulated simulated = pGame.simulation.FindSimulated(terrainSlotObject.id);
			if (simulated != null)
			{
				simulated.SetFootprint(pGame.simulation, false);
				pGame.simulation.RemoveSimulated(simulated);
			}
		}
		foreach (TerrainSlotObject terrainSlotObject2 in terrainSlot.debris)
		{
			pGame.entities.Destroy(terrainSlotObject2.id);
			Simulated simulated = pGame.simulation.FindSimulated(terrainSlotObject2.id);
			if (simulated != null)
			{
				simulated.SetFootprint(pGame.simulation, false);
				pGame.simulation.RemoveSimulated(simulated);
			}
		}
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x000D95E0 File Offset: 0x000D77E0
	public bool IsBorderSlot(int id)
	{
		TerrainSlot terrainSlot;
		if (this.slots.TryGetValue(id, out terrainSlot))
		{
			foreach (GridPosition gridPosition in terrainSlot.sectors)
			{
				if (gridPosition.col == this.sectorWidth - 1)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000D9670 File Offset: 0x000D7870
	public void HighlightSelection(TerrainSlot slot)
	{
		TFUtils.Assert(!this.purchasedSlots.Contains(slot.Id), "Should not be selecting a purchased Slot");
		slot.DrawOutline();
		foreach (GridPosition gridPosition in slot.sectors)
		{
			if (this.ValidSectorIndex(gridPosition))
			{
				this.sectors[gridPosition.row, gridPosition.col].Highlighted = true;
				this.UpdateAllSurroundingSectors(gridPosition.row, gridPosition.col);
			}
		}
		this.selectedSlot = slot;
	}

	// Token: 0x06002350 RID: 9040 RVA: 0x000D9738 File Offset: 0x000D7938
	public void DropSelection(TerrainSlot slot)
	{
		TFUtils.Assert(!this.purchasedSlots.Contains(slot.Id), "Should not be deselecting a purchased Slot");
		foreach (GridPosition gridPosition in slot.sectors)
		{
			if (this.ValidSectorIndex(gridPosition))
			{
				this.sectors[gridPosition.row, gridPosition.col].Highlighted = false;
				this.UpdateAllSurroundingSectors(gridPosition.row, gridPosition.col);
			}
		}
		slot.ClearOutline();
		this.selectedSlot = null;
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x000D9800 File Offset: 0x000D7A00
	public void OutlineAvailableExpansionSlots(Game game)
	{
		foreach (TerrainSlot terrainSlot in this.slots.Values)
		{
			if (terrainSlot.Available(this.purchasedSlots, game))
			{
				terrainSlot.DrawOutline();
			}
		}
	}

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x06002352 RID: 9042 RVA: 0x000D987C File Offset: 0x000D7A7C
	public Dictionary<int, TerrainSlot> ExpansionSlots
	{
		get
		{
			return this.slots;
		}
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000D9884 File Offset: 0x000D7A84
	public void HideAvailableExpansionSlots()
	{
		foreach (TerrainSlot terrainSlot in this.slots.Values)
		{
			terrainSlot.ClearOutline();
		}
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x000D98F0 File Offset: 0x000D7AF0
	public void OutlineAllExpansionSlots()
	{
		foreach (TerrainSlot terrainSlot in this.slots.Values)
		{
			terrainSlot.DrawOutline();
		}
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000D995C File Offset: 0x000D7B5C
	public void HideAllExpansionSlots()
	{
		foreach (TerrainSlot terrainSlot in this.slots.Values)
		{
			terrainSlot.ClearOutline();
		}
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x000D99C8 File Offset: 0x000D7BC8
	public void UpdateRealtySigns(DisplayControllerManager dcm, BillboardDelegate billboard, Game game)
	{
		Camera main = Camera.main;
		foreach (TerrainSlot terrainSlot in this.slots.Values)
		{
			if (terrainSlot.Available(this.purchasedSlots, game))
			{
				terrainSlot.Display(dcm, billboard);
				terrainSlot.OnUpdate(main);
			}
		}
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x000D9A54 File Offset: 0x000D7C54
	public List<TerrainSlot> UnpurchasedExpansionSlots()
	{
		List<TerrainSlot> list = new List<TerrainSlot>();
		foreach (TerrainSlot terrainSlot in this.slots.Values)
		{
			if (!this.purchasedSlots.Contains(terrainSlot.Id))
			{
				list.Add(terrainSlot);
			}
		}
		return list;
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x000D9ADC File Offset: 0x000D7CDC
	public TerrainType GetTerrainType(int row, int col)
	{
		if (!this.ValidTileIndex(row, col))
		{
			return null;
		}
		byte key = this.tiles[row, col];
		return this.terrainTypes[(int)key];
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000D9B14 File Offset: 0x000D7D14
	public bool ProcessTap(Ray ray, Game game)
	{
		TerrainSlot terrainSlot = this.CheckTap(ray, game);
		if (terrainSlot != null)
		{
			if (this.selectedSlot != null)
			{
				this.DropSelection(this.selectedSlot);
			}
			this.selectedSlot = terrainSlot;
			this.selectedSlot.HandleSelection();
			return true;
		}
		return false;
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000D9B5C File Offset: 0x000D7D5C
	public TerrainSlot CheckTap(Ray ray, Game game)
	{
		if (!game.featureManager.CheckFeature("purchase_expansions"))
		{
			return null;
		}
		foreach (TerrainSlot terrainSlot in this.slots.Values)
		{
			if (terrainSlot.CheckTap(ray) && game.simulation.CheckExpansionAllowed(terrainSlot.Id))
			{
				return terrainSlot;
			}
		}
		return null;
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000D9C04 File Offset: 0x000D7E04
	public Cost GetExpansionCost(TerrainSlot slot)
	{
		if (slot.cost != null && slot.cost.ResourceAmounts.Count != 0)
		{
			return slot.cost;
		}
		Cost c = (this.purchasedSlots.Count < this.expansionCosts.Count) ? this.expansionCosts[this.purchasedSlots.Count] : this.expansionCosts[this.expansionCosts.Count - 1];
		Cost cost = new Cost();
		for (int i = 0; i < slot.Tier; i++)
		{
			cost += c;
		}
		return cost;
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000D9CB0 File Offset: 0x000D7EB0
	public bool IsTerrainSectorDisabled(int sectorRow, int sectorCol)
	{
		bool result = true;
		if (this.ValidSector(sectorRow, sectorCol))
		{
			result = !this.purchasedSectors[sectorRow, sectorCol];
		}
		return result;
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000D9CE0 File Offset: 0x000D7EE0
	public bool IsTerrainSectorBoardwalk(int sectorRow, int sectorCol)
	{
		bool result = true;
		if (this.ValidSector(sectorRow, sectorCol))
		{
		}
		return result;
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x000D9D00 File Offset: 0x000D7F00
	public TerrainType GetTerrainType(GridPosition gridPosition)
	{
		return this.GetTerrainType(gridPosition.row, gridPosition.col);
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x000D9D14 File Offset: 0x000D7F14
	public TerrainType GetTerrainType(Vector2 worldPosition)
	{
		return this.GetTerrainType(this.ComputeGridPosition(worldPosition));
	}

	// Token: 0x06002360 RID: 9056 RVA: 0x000D9D24 File Offset: 0x000D7F24
	public TerrainType GetTerrainType(int type)
	{
		if (this.terrainTypes.ContainsKey(type))
		{
			return this.terrainTypes[type];
		}
		return null;
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x000D9D48 File Offset: 0x000D7F48
	public int GetTerrainIdAt(int row, int col)
	{
		if (!this.ValidTileIndex(row, col))
		{
			return 255;
		}
		return (int)this.tiles[row, col];
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x000D9D78 File Offset: 0x000D7F78
	private void Decal()
	{
		for (int i = 0; i < this.GridHeight; i++)
		{
			for (int j = 0; j < this.GridWidth; j++)
			{
				byte b = this.GenerateDecal(i, j);
				if (b != 255)
				{
					this.tiles[i, j] = b;
				}
			}
		}
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x000D9DD8 File Offset: 0x000D7FD8
	private byte GenerateDecal(int row, int col)
	{
		TerrainType terrainType = this.GetTerrainType(row, col);
		if (terrainType == null)
		{
			return byte.MaxValue;
		}
		int seed = this.GetSeed(row, col);
		return terrainType.GenerateDecal(seed);
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000D9E0C File Offset: 0x000D800C
	private bool ValidTileIndex(int row, int col)
	{
		return row >= 0 && this.GridHeight > row && col >= 0 && this.GridWidth > col;
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000D9E3C File Offset: 0x000D803C
	private bool ValidSectorIndex(GridPosition pos)
	{
		return this.ValidSectorIndex(pos.row, pos.col);
	}

	// Token: 0x06002366 RID: 9062 RVA: 0x000D9E50 File Offset: 0x000D8050
	private bool ValidSectorIndex(int row, int col)
	{
		return row >= 0 && this.sectorHeight > row && col >= 0 && this.sectorWidth > col;
	}

	// Token: 0x06002367 RID: 9063 RVA: 0x000D9E80 File Offset: 0x000D8080
	private bool ValidSector(int sectorRow, int sectorCol)
	{
		return sectorRow >= 0 && sectorRow < this.sectorHeight && sectorCol >= 0 && sectorCol < this.sectorWidth;
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x000D9EAC File Offset: 0x000D80AC
	private bool ValidTileIndex(GridPosition pos)
	{
		return this.ValidTileIndex(pos.row, pos.col);
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x000D9EC0 File Offset: 0x000D80C0
	private bool HasObstacle(int row, int col)
	{
		return this.ValidTileIndex(row, col) && this.obstacles[row, col];
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000D9EE0 File Offset: 0x000D80E0
	private void UpdateSectors(int gridRow, int gridCol)
	{
		if (!this.meshesCreated)
		{
			return;
		}
		int num = gridRow / 6;
		int num2 = gridCol / 6;
		this.sectors[num, num2].Initialize(this, num, num2);
		bool flag = gridRow % 6 == 0 && gridRow > 0;
		bool flag2 = gridRow % 6 == 5 && num < this.sectorHeight - 1;
		bool flag3 = gridCol % 6 == 0 && gridCol > 0;
		bool flag4 = gridCol % 6 == 5 && num2 < this.sectorWidth - 1;
		if (flag)
		{
			this.sectors[num - 1, num2].Initialize(this, num - 1, num2);
		}
		else if (flag2)
		{
			this.sectors[num + 1, num2].Initialize(this, num + 1, num2);
		}
		if (flag3)
		{
			this.sectors[num, num2 - 1].Initialize(this, num, num2 - 1);
		}
		else if (flag4)
		{
			this.sectors[num, num2 + 1].Initialize(this, num, num2 + 1);
		}
		if (flag && flag3)
		{
			this.sectors[num - 1, num2 - 1].Initialize(this, num - 1, num2 - 1);
		}
		else if (flag2 && flag3)
		{
			this.sectors[num + 1, num2 - 1].Initialize(this, num + 1, num2 - 1);
		}
		else if (flag && flag4)
		{
			this.sectors[num - 1, num2 + 1].Initialize(this, num - 1, num2 + 1);
		}
		else if (flag2 && flag4)
		{
			this.sectors[num + 1, num2 + 1].Initialize(this, num + 1, num2 + 1);
		}
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x000DA0A8 File Offset: 0x000D82A8
	private void UpdateSingleSector(int sectorRow, int sectorCol)
	{
		if (this.ValidSector(sectorRow, sectorCol))
		{
			this.sectors[sectorRow, sectorCol].Initialize(this, sectorRow, sectorCol);
		}
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x000DA0D8 File Offset: 0x000D82D8
	private void UpdateAllSurroundingSectors(int sectorRow, int sectorCol)
	{
		if (!this.meshesCreated)
		{
			return;
		}
		this.UpdateSingleSector(sectorRow, sectorCol);
		this.UpdateSingleSector(sectorRow + 1, sectorCol);
		this.UpdateSingleSector(sectorRow - 1, sectorCol);
		this.UpdateSingleSector(sectorRow, sectorCol + 1);
		this.UpdateSingleSector(sectorRow + 1, sectorCol + 1);
		this.UpdateSingleSector(sectorRow - 1, sectorCol + 1);
		this.UpdateSingleSector(sectorRow, sectorCol - 1);
		this.UpdateSingleSector(sectorRow + 1, sectorCol - 1);
		this.UpdateSingleSector(sectorRow - 1, sectorCol - 1);
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x000DA154 File Offset: 0x000D8354
	private AlignedBox ComputeVisibleBounds()
	{
		Camera main = Camera.main;
		Vector3 a;
		this.ComputeIntersection(main.ViewportPointToRay(new Vector3(0f, 0f, 0f)), out a);
		a /= 20f;
		Vector3 a2;
		this.ComputeIntersection(main.ViewportPointToRay(new Vector3(0f, 1f, 0f)), out a2);
		a2 /= 20f;
		Vector3 a3;
		this.ComputeIntersection(main.ViewportPointToRay(new Vector3(1f, 0f, 0f)), out a3);
		a3 /= 20f;
		Vector3 a4;
		this.ComputeIntersection(main.ViewportPointToRay(new Vector3(1f, 1f, 0f)), out a4);
		a4 /= 20f;
		int num = (int)(Math.Min(a.x, Math.Min(a2.x, Math.Min(a3.x, a4.x))) - 0.5f);
		int num2 = (int)(Math.Max(a.x, Math.Max(a2.x, Math.Max(a3.x, a4.x))) - 0.5f);
		int num3 = (int)(Math.Min(a.y, Math.Min(a2.y, Math.Min(a3.y, a4.y))) - 0.5f);
		int num4 = (int)(Math.Max(a.y, Math.Max(a2.y, Math.Max(a3.y, a4.y))) - 0.5f);
		return new AlignedBox((float)num, (float)num2, (float)num3, (float)num4);
	}

	// Token: 0x0600236E RID: 9070 RVA: 0x000DA308 File Offset: 0x000D8508
	private byte GenerateTerrainTile(int row, int col)
	{
		UnityEngine.Random.seed = this.GetSeed(row, col);
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

	// Token: 0x0600236F RID: 9071 RVA: 0x000DA3A4 File Offset: 0x000D85A4
	private int GetSeed(int row, int col)
	{
		int num = this.terrainSeed * 127;
		num += row * 127;
		return num + col;
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x000DA3C8 File Offset: 0x000D85C8
	public static List<TerrainNode> LoadTerrainNodeData(List<object> data)
	{
		List<TerrainNode> list = new List<TerrainNode>();
		foreach (object obj in data)
		{
			Dictionary<string, object> d = (Dictionary<string, object>)obj;
			list.Add(new TerrainNode
			{
				did = TFUtils.LoadInt(d, "did"),
				x = TFUtils.LoadInt(d, "x"),
				y = TFUtils.LoadInt(d, "y")
			});
		}
		return list;
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x000DA474 File Offset: 0x000D8674
	private void ProcessOverrides()
	{
		foreach (TerrainNode terrainNode in this.foregroundOverrides)
		{
			this.tiles[terrainNode.x, terrainNode.y] = (byte)terrainNode.did;
		}
		this.foregroundOverrides = null;
	}

	// Token: 0x0400158C RID: 5516
	public const byte INVALID_TERRAIN_COST = 255;

	// Token: 0x0400158D RID: 5517
	public const byte OBSTACLE_COST = 120;

	// Token: 0x0400158E RID: 5518
	public const byte UNPURCHASED_COST = 20;

	// Token: 0x0400158F RID: 5519
	public const byte TERRAIN_TYPE_INVALID = 255;

	// Token: 0x04001590 RID: 5520
	public const int TERRAIN_TILE_WORLDSIZE = 20;

	// Token: 0x04001591 RID: 5521
	public static readonly string TERRAIN_PATH = "Terrain";

	// Token: 0x04001592 RID: 5522
	public static Vector3 UP = new Vector3(0f, 0f, 1f);

	// Token: 0x04001593 RID: 5523
	public static float terrainTextureScaleU = 0f;

	// Token: 0x04001594 RID: 5524
	public static float terrainTextureScaleV = 0f;

	// Token: 0x04001595 RID: 5525
	public static float terrainTextureInvScaleU = 0f;

	// Token: 0x04001596 RID: 5526
	public static float terrainTextureInvScaleV = 0f;

	// Token: 0x04001597 RID: 5527
	public List<Cost> expansionCosts;

	// Token: 0x04001598 RID: 5528
	public TerrainTextureLibrary terrainTextures;

	// Token: 0x04001599 RID: 5529
	public HashSet<int> purchasedSlots;

	// Token: 0x0400159A RID: 5530
	public TerrainSlot selectedSlot;

	// Token: 0x0400159B RID: 5531
	private Dictionary<int, TerrainType> terrainTypes = new Dictionary<int, TerrainType>();

	// Token: 0x0400159C RID: 5532
	private Dictionary<int, TerrainSlot> slots = new Dictionary<int, TerrainSlot>();

	// Token: 0x0400159D RID: 5533
	private byte[,] tiles;

	// Token: 0x0400159E RID: 5534
	private byte[,] nonPathTiles;

	// Token: 0x0400159F RID: 5535
	private TerrainSector[,] sectors;

	// Token: 0x040015A0 RID: 5536
	private int terrainSeed;

	// Token: 0x040015A1 RID: 5537
	private byte backgroundTerrain;

	// Token: 0x040015A2 RID: 5538
	private int sectorWidth;

	// Token: 0x040015A3 RID: 5539
	private int sectorHeight;

	// Token: 0x040015A4 RID: 5540
	private Rect sectorInset;

	// Token: 0x040015A5 RID: 5541
	private AlignedBox worldExtent;

	// Token: 0x040015A6 RID: 5542
	private AlignedBox purchasedExtent;

	// Token: 0x040015A7 RID: 5543
	private AlignedBox footprintGuide;

	// Token: 0x040015A8 RID: 5544
	private AlignedBox cameraExtents;

	// Token: 0x040015A9 RID: 5545
	private bool[,] obstacles;

	// Token: 0x040015AA RID: 5546
	private bool[,] purchasedSectors;

	// Token: 0x040015AB RID: 5547
	private List<TerrainNode> foregroundOverrides;

	// Token: 0x040015AC RID: 5548
	private List<KeyValuePair<int, float>> distribution;

	// Token: 0x040015AD RID: 5549
	private bool meshesCreated;

	// Token: 0x040015AE RID: 5550
	private static Material mTerrainMaterial = null;
}
