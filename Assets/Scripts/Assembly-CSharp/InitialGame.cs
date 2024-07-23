using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

// Token: 0x02000176 RID: 374
public static class InitialGame
{
	// Token: 0x06000CDA RID: 3290 RVA: 0x0004E274 File Offset: 0x0004C474
	public static Dictionary<string, object> Generate(EntityManager entityManager, ResourceManager resourceManager)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary3 = InitialGame.LoadInitialGameFromSpreads();
		ulong num = TFUtils.EpochTime();
		PlaytimeRegistrar.ApplyToGameState(ref dictionary, 1, num, num, 0UL);
		List<object> list = new List<object>();
		foreach (object obj in (dictionary3["buildings"] as List<object>))
		{
			Dictionary<string, object> dictionary4 = (Dictionary<string, object>)obj;
			int num2 = TFUtils.LoadInt(dictionary4, "did");
			string value = (string)dictionary4["label"];
			EntityType entityType = EntityType.BUILDING;
			if (dictionary4.ContainsKey("extensions"))
			{
				entityType |= (EntityType)TFUtils.LoadUint(dictionary4, "extensions");
			}
			BuildingEntity decorator = entityManager.Create(entityType, num2, new Identity(value), false).GetDecorator<BuildingEntity>();
			Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
			dictionary5["did"] = num2;
			dictionary5["extensions"] = (uint)entityType;
			dictionary5["label"] = value;
			dictionary5["x"] = TFUtils.LoadInt(dictionary4, "x");
			dictionary5["y"] = TFUtils.LoadInt(dictionary4, "y");
			dictionary5["flip"] = (bool)dictionary4["flip"];
			ActivatableDecorator.Serialize(ref dictionary5, num);
			dictionary5["build_finish_time"] = num;
			if (decorator.HasDecorator<PeriodicProductionDecorator>())
			{
				dictionary5["rent_ready_time"] = num + decorator.GetDecorator<PeriodicProductionDecorator>().RentProductionTime;
			}
			else
			{
				dictionary5["rent_ready_time"] = null;
			}
			list.Add(dictionary5);
		}
		List<object> list2 = new List<object>();
		if (dictionary3.ContainsKey("debris"))
		{
			foreach (object obj2 in (dictionary3["debris"] as List<object>))
			{
				Dictionary<string, object> dictionary6 = (Dictionary<string, object>)obj2;
				int num3 = TFUtils.LoadInt(dictionary6, "did");
				string value2 = (string)dictionary6["label"];
				Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
				dictionary7["did"] = num3;
				dictionary7["label"] = value2;
				dictionary7["x"] = TFUtils.LoadInt(dictionary6, "x");
				dictionary7["y"] = TFUtils.LoadInt(dictionary6, "y");
				list2.Add(dictionary7);
			}
		}
		List<object> list3 = new List<object>();
		if (dictionary3.ContainsKey("units"))
		{
			foreach (object obj3 in (dictionary3["units"] as List<object>))
			{
				Dictionary<string, object> dictionary8 = (Dictionary<string, object>)obj3;
				string value3 = (string)dictionary8["label"];
				Dictionary<string, object> dictionary9 = new Dictionary<string, object>();
				dictionary9["did"] = TFUtils.LoadInt(dictionary8, "did");
				dictionary9["label"] = value3;
				dictionary9["residence"] = dictionary8["residence"];
				dictionary9["feed_ready_time"] = num;
				dictionary9["fullness_length"] = 90;
				dictionary9["waiting"] = ((!dictionary8.ContainsKey("waiting")) ? false : dictionary8["waiting"]);
				dictionary9["active"] = true;
				list3.Add(dictionary9);
			}
		}
		List<object> list4 = new List<object>();
		foreach (object obj4 in (dictionary3["resources"] as List<object>))
		{
			Dictionary<string, object> d = (Dictionary<string, object>)obj4;
			int num4 = TFUtils.LoadInt(d, "id");
			int amountEarned = TFUtils.LoadInt(d, "amount_earned");
			list4.Add(InitialGame.MakeResource(num4, resourceManager.Resources[num4].Name, amountEarned, 0, 0));
		}
		List<object> list5 = new List<object>();
		if (dictionary3.ContainsKey("paths"))
		{
			foreach (object obj5 in (dictionary3["paths"] as List<object>))
			{
				Dictionary<string, object> item = (Dictionary<string, object>)obj5;
				list5.Add(item);
			}
		}
		List<object> list6 = new List<object>();
		foreach (object item2 in (dictionary3["expansions"] as List<object>))
		{
			list6.Add(item2);
		}
		List<object> list7 = new List<object>();
		foreach (object item3 in (dictionary3["recipes"] as List<object>))
		{
			list7.Add(item3);
		}
		List<object> list8 = new List<object>();
		foreach (object item4 in (dictionary3["movies"] as List<object>))
		{
			list8.Add(item4);
		}
		List<object> value4 = new List<object>();
		List<object> value5 = new List<object>();
		TFUtils.Assert(!dictionary3.ContainsKey("tasks"), "Should not try to define task instances in the initial state");
		List<object> value6 = new List<object>();
		List<object> list9 = new List<object>();
		if (dictionary3.ContainsKey("quests"))
		{
			foreach (object value7 in (dictionary3["quests"] as List<object>))
			{
				Dictionary<string, object> dictionary10 = new Dictionary<string, object>();
				dictionary10["did"] = value7;
				dictionary10["start_time"] = num;
				dictionary10["completion_time"] = null;
				dictionary10["reminded"] = false;
				Dictionary<string, object> dictionary11 = new Dictionary<string, object>();
				dictionary11["met_start_condition_ids"] = new List<object>();
				dictionary11["met_end_condition_ids"] = new List<object>();
				dictionary10["conditions"] = dictionary11;
				list9.Add(dictionary10);
			}
		}
		List<object> value8 = new List<object>();
		dictionary2.Add("buildings", list);
		dictionary2.Add("debris", list2);
		dictionary2.Add("units", list3);
		dictionary2.Add("resources", list4);
		dictionary2.Add("tasks", value6);
		dictionary2.Add("quests", list9);
		dictionary2.Add("generated_quest_definition", value8);
		dictionary2.Add("pavement", list5);
		dictionary2.Add("expansions", list6);
		dictionary2.Add("recipes", list7);
		dictionary2.Add("crafts", value5);
		dictionary2.Add("movies", list8);
		dictionary2.Add("drop_pickups", value4);
		TFUtils.DebugLog("Completed Init Data Load");
		dictionary2.Add("last_action", null);
		dictionary.Add("farm", dictionary2);
		List<object> value9 = new List<object>();
		dictionary.Add("dialogs", value9);
		dictionary["protocol_version"] = GamestateMigrator.CURRENT_VERSION;
		return dictionary;
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x0004EB3C File Offset: 0x0004CD3C
	private static void WriteInitFile(Session session, string outPath)
	{
		Dictionary<string, object> dictionary = InitialGame.LoadInitialGameFromSpreads();
		Simulation simulation = session.TheGame.simulation;
		List<object> list = new List<object>();
		foreach (Simulated simulated in simulation.GetSimulateds())
		{
			if (simulated.entity is DebrisEntity)
			{
				DebrisEntity debrisEntity = (DebrisEntity)simulated.entity;
				if (debrisEntity.ExpansionId == null)
				{
					list.Add(new Dictionary<string, object>
					{
						{
							"did",
							debrisEntity.DefinitionId
						},
						{
							"x",
							simulated.Position.x
						},
						{
							"y",
							simulated.Position.y
						},
						{
							"label",
							debrisEntity.Id.Describe()
						}
					});
				}
			}
		}
		dictionary["debris"] = list;
		List<object> list2 = new List<object>();
		foreach (Simulated simulated2 in simulation.GetSimulateds())
		{
			if ((simulated2.entity.AllTypes & EntityType.BUILDING) != EntityType.INVALID)
			{
				list2.Add(new Dictionary<string, object>
				{
					{
						"did",
						simulated2.entity.DefinitionId
					},
					{
						"x",
						simulated2.Position.x
					},
					{
						"y",
						simulated2.Position.y
					},
					{
						"flip",
						simulated2.Flip
					},
					{
						"label",
						simulated2.entity.Id.Describe()
					},
					{
						"extensions",
						(uint)simulated2.entity.AllTypes
					}
				});
			}
		}
		dictionary["buildings"] = list2;
		List<object> list3 = new List<object>();
		for (int i = 0; i < session.TheGame.terrain.GridWidth; i++)
		{
			for (int j = 0; j < session.TheGame.terrain.GridWidth; j++)
			{
				GridPosition gridPosition = new GridPosition(j, i);
				TerrainType terrainType = session.TheGame.terrain.GetTerrainType(gridPosition);
				if (terrainType != null && terrainType.IsPath())
				{
					list3.Add(new Dictionary<string, object>
					{
						{
							"col",
							i
						},
						{
							"row",
							j
						}
					});
				}
			}
		}
		dictionary["paths"] = list3;
		string path = outPath + Path.DirectorySeparatorChar + "starting_bikini_bottom.json";
		string contents = Json.Serialize(dictionary);
		File.WriteAllText(path, contents);
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x0004EEAC File Offset: 0x0004D0AC
	private static void WriteSlotFiles(Session session, string terrainPath)
	{
		Simulation simulation = session.TheGame.simulation;
		Dictionary<int, TerrainSlot> expansionSlots = session.TheGame.terrain.ExpansionSlots;
		foreach (Simulated simulated in simulation.GetSimulateds())
		{
			if (simulated.entity is DebrisEntity)
			{
				DebrisEntity debris = (DebrisEntity)simulated.entity;
				if (debris.ExpansionId != null)
				{
					TerrainSlot terrainSlot = expansionSlots[debris.ExpansionId.Value];
					int num = terrainSlot.debris.FindIndex((TerrainSlotObject obj) => obj.id == debris.Id);
					if (num >= 0)
					{
						TerrainSlotObject value = terrainSlot.debris[num];
						value.position.col = (int)simulated.Position.x;
						value.position.row = (int)simulated.Position.y;
						terrainSlot.debris[num] = value;
					}
				}
			}
		}
		foreach (TerrainSlot terrainSlot2 in expansionSlots.Values)
		{
			string text = "slot" + terrainSlot2.Id + ".json";
			string streamingAssetsFileInDirectory = TFUtils.GetStreamingAssetsFileInDirectory("Terrain", text);
			string json = TFUtils.ReadAllText(streamingAssetsFileInDirectory);
			Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
			dictionary["debris"] = TerrainSlot.SerializeExpansionObjectData(terrainSlot2.debris);
			string path = terrainPath + Path.DirectorySeparatorChar + text;
			string contents = Json.Serialize(dictionary);
			File.WriteAllText(path, contents);
		}
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0004F0E0 File Offset: 0x0004D2E0
	public static void WriteUpdatedFile(Session session)
	{
		string text = string.Concat(new object[]
		{
			TFUtils.ApplicationPersistentDataPath,
			Path.DirectorySeparatorChar,
			"freeEdit_",
			DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss")
		});
		string text2 = text + Path.DirectorySeparatorChar + "Init";
		Directory.CreateDirectory(text2);
		string text3 = text + Path.DirectorySeparatorChar + "Terrain";
		Directory.CreateDirectory(text3);
		InitialGame.WriteInitFile(session, text2);
		InitialGame.WriteSlotFiles(session, text3);
		Debug.Log("wrote file to " + text);
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x0004F184 File Offset: 0x0004D384
	private static Dictionary<string, object> MakeResource(int did, string name, int amountEarned, int amountSpent, int amountPurchased)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["did"] = did;
		dictionary["name"] = name;
		dictionary["amount_earned"] = amountEarned;
		dictionary["amount_spent"] = amountSpent;
		dictionary["amount_purchased"] = amountPurchased;
		return dictionary;
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0004F1EC File Offset: 0x0004D3EC
	private static Dictionary<string, object> LoadInitialGameFromSpreads()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "init");
		InitialGame.LoadInitialBlueprintsFromSpread(dictionary);
		InitialGame.LoadInitialResourcesFromSpread(dictionary);
		InitialGame.LoadInitialPathsFromSpread(dictionary);
		InitialGame.LoadInitialUnlocksFromSpread(dictionary);
		return dictionary;
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0004F228 File Offset: 0x0004D428
	private static void LoadInitialBlueprintsFromSpread(Dictionary<string, object> pData)
	{
		string text = "InitialBlueprints";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
				string stringCell = instance.GetStringCell(text, rowName, "type");
				if (!string.IsNullOrEmpty(stringCell) && !(stringCell == b))
				{
					if (!pData.ContainsKey(stringCell))
					{
						pData.Add(stringCell, new List<object>());
					}
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
					dictionary.Add("label", instance.GetStringCell(text, rowName, "label"));
					dictionary.Add("flip", instance.GetIntCell(sheetIndex, rowIndex, "flip") == 1);
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, "position x");
					if (intCell >= 0)
					{
						dictionary.Add("x", intCell);
					}
					intCell = instance.GetIntCell(sheetIndex, rowIndex, "position y");
					if (intCell >= 0)
					{
						dictionary.Add("y", intCell);
					}
					intCell = instance.GetIntCell(sheetIndex, rowIndex, "extensions");
					if (intCell >= 0)
					{
						dictionary.Add("extensions", intCell);
					}
					string stringCell2 = instance.GetStringCell(text, rowName, "residence");
					if (!string.IsNullOrEmpty(stringCell2) && stringCell2 != b)
					{
						dictionary.Add("residence", stringCell2);
					}
					((List<object>)pData[stringCell]).Add(dictionary);
				}
			}
		}
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x0004F468 File Offset: 0x0004D668
	private static void LoadInitialResourcesFromSpread(Dictionary<string, object> pData)
	{
		string text = "InitialResources";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		if (!pData.ContainsKey("resources"))
		{
			pData.Add("resources", new List<object>());
		}
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
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("id", instance.GetIntCell(sheetIndex, rowIndex, "did"));
				dictionary.Add("amount_earned", instance.GetIntCell(sheetIndex, rowIndex, "amount earned"));
				dictionary.Add("amount_purchased", 0);
				dictionary.Add("amount_spent", 0);
				((List<object>)pData["resources"]).Add(dictionary);
			}
		}
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0004F5CC File Offset: 0x0004D7CC
	private static void LoadInitialPathsFromSpread(Dictionary<string, object> pData)
	{
		string text = "InitialPaths";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
		if (!pData.ContainsKey("paths"))
		{
			pData.Add("paths", new List<object>());
		}
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
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("col", instance.GetIntCell(sheetIndex, rowIndex, "column"));
				dictionary.Add("row", instance.GetIntCell(sheetIndex, rowIndex, "row"));
				((List<object>)pData["paths"]).Add(dictionary);
			}
		}
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x0004F70C File Offset: 0x0004D90C
	private static void LoadInitialUnlocksFromSpread(Dictionary<string, object> pData)
	{
		string text = "InitialUnlocks";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
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
				string stringCell = instance.GetStringCell(text, rowName, "type");
				if (!string.IsNullOrEmpty(stringCell) && !(stringCell == b))
				{
					if (!pData.ContainsKey(stringCell))
					{
						pData.Add(stringCell, new List<object>());
					}
					((List<object>)pData[stringCell]).Add(instance.GetIntCell(sheetIndex, rowIndex, "did"));
				}
			}
		}
	}

	// Token: 0x040008A1 RID: 2209
	private const string INIT_FILE = "starting_bikini_bottom.json";
}
