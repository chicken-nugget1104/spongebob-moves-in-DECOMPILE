using System;
using System.Collections.Generic;

// Token: 0x0200011B RID: 283
public class Catalog
{
	// Token: 0x06000A42 RID: 2626 RVA: 0x0003FB94 File Offset: 0x0003DD94
	public Catalog()
	{
		this.costs = new Dictionary<int, Cost>();
		this.sellCosts = new Dictionary<int, Cost>();
		this.descriptions = new Dictionary<int, string>();
		this.offersByCode = new Dictionary<string, Dictionary<string, object>>();
		this.premiumCodes = new List<string>();
		this.canSell = new Dictionary<int, bool>();
		this.sellErrors = new Dictionary<int, string>();
		this.LoadCatalog();
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0003FBFC File Offset: 0x0003DDFC
	public Dictionary<string, object> CatalogDict
	{
		get
		{
			return this.catalogDict;
		}
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0003FC04 File Offset: 0x0003DE04
	private void LoadCatalog()
	{
		this.catalogDict = this.LoadCatalogFromSpread();
		TFUtils.Assert(this.catalogDict != null, "Catalog Spread failed to read in.");
		List<object> list = TFUtils.LoadList<object>(this.catalogDict, "offers");
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			this.LoadCostsHelper(this.costs, dictionary, "cost");
			this.LoadCostsHelper(this.sellCosts, dictionary, "sell_cost");
			if (dictionary.ContainsKey("can_sell"))
			{
				int key = TFUtils.LoadInt(dictionary, "identity");
				this.canSell.Add(key, (bool)dictionary["can_sell"]);
				if (dictionary.ContainsKey("sell_error"))
				{
					this.sellErrors.Add(key, (string)dictionary["sell_error"]);
				}
			}
			if (dictionary.ContainsKey("description"))
			{
				int key2 = TFUtils.LoadInt(dictionary, "identity");
				this.descriptions.Add(key2, (string)dictionary["description"]);
			}
			if (dictionary.ContainsKey("code"))
			{
				string text = TFUtils.LoadString(dictionary, "code");
				this.offersByCode.Add(text, dictionary);
				this.premiumCodes.Add(text);
			}
		}
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x0003FD90 File Offset: 0x0003DF90
	private Dictionary<string, object> LoadCatalogFromSpread()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add("type", "catalog");
		if (!this.LoadItemCategoriesFromSpread(dictionary))
		{
			return null;
		}
		if (!this.LoadItemIdentitiesFromSpread(dictionary))
		{
			return null;
		}
		this.LoadIAPIdentitiesFromSpread(dictionary);
		return dictionary;
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x0003FDD8 File Offset: 0x0003DFD8
	private bool LoadItemCategoriesFromSpread(Dictionary<string, object> pData)
	{
		string text = "ItemCategories";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return false;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return false;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return false;
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
				string stringCell = instance.GetStringCell(text, rowName, "category type");
				List<object> list;
				if (!pData.ContainsKey(stringCell))
				{
					list = new List<object>();
					pData.Add(stringCell, list);
				}
				else
				{
					list = (List<object>)pData[stringCell];
				}
				list.Add(new Dictionary<string, object>
				{
					{
						"micro_event_did",
						instance.GetIntCell(sheetIndex, rowIndex, "micro event did")
					},
					{
						"event_only",
						instance.GetIntCell(sheetIndex, rowIndex, "event only") == 1
					},
					{
						"name",
						instance.GetStringCell(text, rowName, "name")
					},
					{
						"type",
						instance.GetStringCell(text, rowName, "item type")
					},
					{
						"label",
						instance.GetStringCell(text, rowName, "label")
					},
					{
						"display.material",
						instance.GetStringCell(text, rowName, "display material")
					}
				});
			}
		}
		return true;
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x0003FF98 File Offset: 0x0003E198
	private bool LoadItemIdentitiesFromSpread(Dictionary<string, object> pData)
	{
		string text = "ItemIdentities";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return false;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return false;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return false;
		}
		List<object> list;
		if (pData.ContainsKey("offers"))
		{
			list = (List<object>)pData["offers"];
		}
		else
		{
			list = new List<object>();
			pData.Add("offers", list);
		}
		if (!pData.ContainsKey("market"))
		{
			TFUtils.Assert(false, "Could not find category group market ");
			return false;
		}
		List<object> list2 = (List<object>)pData["market"];
		int count = list2.Count;
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
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "item did");
				dictionary.Add("identity", intCell);
				dictionary.Add("micro_event_did", instance.GetIntCell(sheetIndex, rowIndex, "micro event did"));
				bool flag = instance.GetIntCell(sheetIndex, rowIndex, "show in store") == 1;
				dictionary.Add("show_in_store", flag);
				dictionary.Add("event_only", instance.GetIntCell(sheetIndex, rowIndex, "event only") == 1);
				dictionary.Add("sale_banner", instance.GetIntCell(sheetIndex, rowIndex, "sale banner") == 1);
				dictionary.Add("new_banner", instance.GetIntCell(sheetIndex, rowIndex, "new banner") == 1);
				dictionary.Add("sale_percent", instance.GetFloatCell(sheetIndex, rowIndex, "sale percent"));
				dictionary.Add("limited_banner", instance.GetIntCell(sheetIndex, rowIndex, "limitedbanner") == 1);
				string stringCell;
				if (flag)
				{
					stringCell = instance.GetStringCell(text, rowName, "category name");
					for (int j = 0; j < count; j++)
					{
						Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list2[j];
						if ((string)dictionary2["name"] == stringCell)
						{
							if (!dictionary2.ContainsKey("dids"))
							{
								dictionary2.Add("dids", new List<object>());
							}
							((List<object>)dictionary2["dids"]).Add(intCell);
							break;
						}
						if (j == count - 1)
						{
							TFUtils.Assert(false, "Could not find category " + stringCell);
							return false;
						}
					}
				}
				dictionary.Add("cost", new Dictionary<string, object>());
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "cost gold");
				if (intCell >= 0)
				{
					((Dictionary<string, object>)dictionary["cost"]).Add("3", intCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "cost jelly");
				if (intCell >= 0)
				{
					((Dictionary<string, object>)dictionary["cost"]).Add("2", intCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "cost special did");
				int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "cost special amount");
				if (intCell >= 2 && intCell2 > 0)
				{
					((Dictionary<string, object>)dictionary["cost"]).Add(intCell.ToString(), intCell2);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "sell cost gold");
				if (intCell >= 0)
				{
					dictionary.Add("sell_cost", new Dictionary<string, object>
					{
						{
							"3",
							intCell
						}
					});
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "sell cost jelly");
				if (intCell >= 0)
				{
					if (!dictionary.ContainsKey("sell_cost"))
					{
						dictionary.Add("sell_cost", new Dictionary<string, object>());
					}
					((Dictionary<string, object>)dictionary["sell_cost"]).Add("2", intCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "can sell");
				if (intCell > 0)
				{
					dictionary.Add("can_sell", true);
				}
				else
				{
					dictionary.Add("can_sell", false);
				}
				stringCell = instance.GetStringCell(text, rowName, "can't sell text");
				if (stringCell != b)
				{
					dictionary.Add("sell_error", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "description");
				if (stringCell != b)
				{
					dictionary.Add("description", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "type");
				if (stringCell != b)
				{
					dictionary.Add("type", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "name");
				if (stringCell != b)
				{
					dictionary.Add("name", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "display model type");
				if (stringCell != b)
				{
					dictionary.Add("display", new Dictionary<string, object>
					{
						{
							"model_type",
							stringCell
						},
						{
							"width",
							instance.GetIntCell(sheetIndex, rowIndex, "display width")
						},
						{
							"height",
							instance.GetIntCell(sheetIndex, rowIndex, "display height")
						}
					});
				}
				stringCell = instance.GetStringCell(text, rowName, "display default texture");
				if (stringCell != b)
				{
					dictionary.Add("display.default", new Dictionary<string, object>
					{
						{
							"texture",
							stringCell
						},
						{
							"name",
							"default"
						}
					});
				}
				list.Add(dictionary);
			}
		}
		return true;
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x000405F0 File Offset: 0x0003E7F0
	private bool LoadIAPIdentitiesFromSpread(Dictionary<string, object> pData)
	{
		string text = "IAPIdentities";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return false;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return false;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return false;
		}
		List<object> list;
		if (pData.ContainsKey("offers"))
		{
			list = (List<object>)pData["offers"];
		}
		else
		{
			list = new List<object>();
			pData.Add("offers", list);
		}
		if (!pData.ContainsKey("market"))
		{
			TFUtils.Assert(false, "Could not find category group market ");
			return false;
		}
		List<object> list2 = (List<object>)pData["market"];
		int count = list2.Count;
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
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "iap did");
				dictionary.Add("identity", intCell);
				string stringCell = instance.GetStringCell(text, rowName, "category name");
				for (int j = 0; j < count; j++)
				{
					Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list2[j];
					if ((string)dictionary2["name"] == stringCell)
					{
						if (!dictionary2.ContainsKey("dids"))
						{
							dictionary2.Add("dids", new List<object>());
						}
						((List<object>)dictionary2["dids"]).Add(intCell);
						break;
					}
					if (j == count - 1)
					{
						TFUtils.Assert(false, "Could not find category " + stringCell);
						return false;
					}
				}
				dictionary.Add("cost", new Dictionary<string, object>());
				dictionary.Add("data", new Dictionary<string, object>());
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "gold amount");
				if (intCell > 0)
				{
					((Dictionary<string, object>)dictionary["data"]).Add("3", intCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, "jelly amount");
				if (intCell > 0)
				{
					((Dictionary<string, object>)dictionary["data"]).Add("2", intCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "description");
				if (stringCell != b)
				{
					dictionary.Add("description", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "type");
				if (stringCell != b)
				{
					dictionary.Add("type", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "name");
				if (stringCell != b)
				{
					dictionary.Add("name", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "result type");
				if (stringCell != b)
				{
					dictionary.Add("result_type", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "iap code");
				if (stringCell != b)
				{
					dictionary.Add("code", stringCell);
				}
				stringCell = instance.GetStringCell(text, rowName, "display model type");
				if (stringCell != b)
				{
					dictionary.Add("display", new Dictionary<string, object>
					{
						{
							"model_type",
							stringCell
						},
						{
							"width",
							instance.GetIntCell(sheetIndex, rowIndex, "display width")
						},
						{
							"height",
							instance.GetIntCell(sheetIndex, rowIndex, "display height")
						}
					});
				}
				stringCell = instance.GetStringCell(text, rowName, "display default texture");
				if (stringCell != b)
				{
					dictionary.Add("display.default", new Dictionary<string, object>
					{
						{
							"texture",
							stringCell
						},
						{
							"name",
							"default"
						}
					});
				}
				list.Add(dictionary);
			}
		}
		return true;
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00040A5C File Offset: 0x0003EC5C
	public Dictionary<string, object> GetOfferByCode(string code)
	{
		return (!this.offersByCode.ContainsKey(code)) ? null : this.offersByCode[code];
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00040A84 File Offset: 0x0003EC84
	private void LoadCostsHelper(Dictionary<int, Cost> costsDict, Dictionary<string, object> offerDict, string key)
	{
		if (offerDict.ContainsKey(key))
		{
			int key2 = TFUtils.LoadInt(offerDict, "identity");
			costsDict[key2] = Cost.FromDict((Dictionary<string, object>)offerDict[key]);
		}
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00040AC4 File Offset: 0x0003ECC4
	private Cost GetCostHelper(Dictionary<int, Cost> dict, int did)
	{
		return (!dict.ContainsKey(did)) ? null : dict[did];
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00040AE0 File Offset: 0x0003ECE0
	public Cost GetCost(int did)
	{
		return this.GetCostHelper(this.costs, did);
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x00040AF0 File Offset: 0x0003ECF0
	public Cost GetSellCost(int did)
	{
		return this.GetCostHelper(this.sellCosts, did);
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x00040B00 File Offset: 0x0003ED00
	public string GetDescription(int did)
	{
		if (this.descriptions.ContainsKey(did))
		{
			return this.descriptions[did];
		}
		return null;
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x00040B24 File Offset: 0x0003ED24
	public bool CanSell(int did)
	{
		return this.canSell.ContainsKey(did) && this.canSell[did];
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x00040B48 File Offset: 0x0003ED48
	public string SellError(int did)
	{
		if (this.sellErrors.ContainsKey(did))
		{
			return this.sellErrors[did];
		}
		return null;
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000A51 RID: 2641 RVA: 0x00040B6C File Offset: 0x0003ED6C
	public List<string> PremiumCodes
	{
		get
		{
			return this.premiumCodes;
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00040B74 File Offset: 0x0003ED74
	public void GetNameAndTypeForDID(int nDID, out string sName, out string sType)
	{
		string empty;
		sType = (empty = string.Empty);
		sName = empty;
		string primaryType = string.Empty;
		foreach (object obj in ((List<object>)this.catalogDict["offers"]))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			if (dictionary.ContainsKey("identity"))
			{
				int num = TFUtils.LoadInt(dictionary, "identity");
				if (num == nDID)
				{
					sType = null;
					if (dictionary.ContainsKey("type"))
					{
						sType = TFUtils.LoadString(dictionary, "type");
					}
					foreach (object obj2 in ((List<object>)this.catalogDict["market"]))
					{
						Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj2;
						if (dictionary2.ContainsKey("dids") && TFUtils.LoadList<int>(dictionary2, "dids").Contains(nDID))
						{
							if (string.IsNullOrEmpty(sType) && dictionary2.ContainsKey("name"))
							{
								sType = TFUtils.LoadString(dictionary2, "name");
							}
							if (dictionary2.ContainsKey("type"))
							{
								primaryType = TFUtils.LoadString(dictionary2, "type");
							}
							break;
						}
					}
					Blueprint blueprint = EntityManager.GetBlueprint(primaryType, nDID, true);
					if (blueprint != null)
					{
						sName = (string)blueprint.Invariable["name"];
						break;
					}
				}
			}
		}
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00040D54 File Offset: 0x0003EF54
	public static string ConvertTypeToDeltaDNAType(string sType)
	{
		if (string.IsNullOrEmpty(sType))
		{
			return string.Empty;
		}
		string text = sType;
		switch (text)
		{
		case "buildings":
			sType = "Characters";
			break;
		case "buildings_no_resident":
			sType = "Buildings";
			break;
		case "production_buildings":
			sType = "Shops";
			break;
		case "trees":
			sType = "Trees";
			break;
		case "decorations":
			sType = "Decorations";
			break;
		case "rmt":
			sType = "Jelly_And_Coins";
			break;
		}
		return sType;
	}

	// Token: 0x04000705 RID: 1797
	private Dictionary<string, object> catalogDict;

	// Token: 0x04000706 RID: 1798
	private Dictionary<int, Cost> costs;

	// Token: 0x04000707 RID: 1799
	private Dictionary<int, Cost> sellCosts;

	// Token: 0x04000708 RID: 1800
	private Dictionary<int, string> descriptions;

	// Token: 0x04000709 RID: 1801
	private Dictionary<string, Dictionary<string, object>> offersByCode;

	// Token: 0x0400070A RID: 1802
	private List<string> premiumCodes;

	// Token: 0x0400070B RID: 1803
	private Dictionary<int, bool> canSell;

	// Token: 0x0400070C RID: 1804
	private Dictionary<int, string> sellErrors;
}
