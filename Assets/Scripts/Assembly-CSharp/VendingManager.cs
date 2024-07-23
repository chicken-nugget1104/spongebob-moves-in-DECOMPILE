using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000273 RID: 627
public class VendingManager
{
	// Token: 0x06001425 RID: 5157 RVA: 0x0008A778 File Offset: 0x00088978
	public VendingManager()
	{
		this.rand = new System.Random(UnityEngine.Random.Range(-100000, 100000));
		this.vendorDefinitions = new Dictionary<int, VendorDefinition>();
		this.stocks = new Dictionary<int, VendorStock>();
		this.instances = new Dictionary<Identity, Dictionary<int, VendingInstance>>();
		this.specialOffers = new Dictionary<Identity, Dictionary<int, VendingInstance>>();
		this.LoadVending();
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x0008A7E4 File Offset: 0x000889E4
	public VendingInstance GetVendingInstance(Identity target, int slotId)
	{
		Dictionary<int, VendingInstance> dictionary = null;
		if (this.instances.TryGetValue(target, out dictionary))
		{
			VendingInstance result = null;
			dictionary.TryGetValue(slotId, out result);
			return result;
		}
		return null;
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x0008A818 File Offset: 0x00088A18
	public VendingInstance GetSpecialInstance(Identity target)
	{
		Dictionary<int, VendingInstance> dictionary = null;
		if (this.specialOffers.TryGetValue(target, out dictionary))
		{
			VendingInstance result = null;
			dictionary.TryGetValue(0, out result);
			return result;
		}
		return null;
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x0008A84C File Offset: 0x00088A4C
	public Dictionary<int, VendingInstance> GetVendingInstances(Identity target)
	{
		Dictionary<int, VendingInstance> result = null;
		this.instances.TryGetValue(target, out result);
		return result;
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x0008A86C File Offset: 0x00088A6C
	public Dictionary<int, VendingInstance> GetSpecialInstances(Identity target)
	{
		Dictionary<int, VendingInstance> result = null;
		this.specialOffers.TryGetValue(target, out result);
		return result;
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x0008A88C File Offset: 0x00088A8C
	public VendorDefinition GetVendorDefinition(int did)
	{
		VendorDefinition result;
		this.vendorDefinitions.TryGetValue(did, out result);
		return result;
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x0008A8AC File Offset: 0x00088AAC
	public VendorStock GetStock(int stockId)
	{
		VendorStock result;
		this.stocks.TryGetValue(stockId, out result);
		return result;
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x0008A8CC File Offset: 0x00088ACC
	public void GenerateNewGeneralInstances(VendingDecorator vendor)
	{
		VendorDefinition vendorDefinition = this.GetVendorDefinition(vendor.VendorId);
		Dictionary<int, VendingInstance> dictionary = new Dictionary<int, VendingInstance>(vendorDefinition.InstanceCount);
		List<int> list = new List<int>(vendorDefinition.generalStock.Count);
		for (int i = 0; i < vendorDefinition.generalStock.Count; i++)
		{
			list.Insert(this.rand.Next(0, list.Count), i);
		}
		for (int j = 0; j < vendorDefinition.InstanceCount; j++)
		{
			if (j < list.Count)
			{
				dictionary[j] = this.GetStock(vendorDefinition.generalStock[list[j]]).GenerateVendingInstance(j, false);
			}
			else
			{
				dictionary[j] = this.GetStock(vendorDefinition.generalStock[this.rand.Next(vendorDefinition.generalStock.Count)]).GenerateVendingInstance(j, false);
			}
		}
		this.instances[vendor.Id] = dictionary;
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x0008A9D8 File Offset: 0x00088BD8
	public void GenerateNewSpecialInstances(VendingDecorator vendor)
	{
		VendorDefinition vendorDefinition = this.GetVendorDefinition(vendor.VendorId);
		if (vendorDefinition.specialStock.Count > 0)
		{
			this.specialOffers[vendor.Id] = new Dictionary<int, VendingInstance>
			{
				{
					0,
					this.GetStock(vendorDefinition.specialStock[this.rand.Next(vendorDefinition.specialStock.Count)]).GenerateVendingInstance(0, true)
				}
			};
		}
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x0008AA50 File Offset: 0x00088C50
	private string[] GetFilesToLoad()
	{
		return Config.VENDING_PATH;
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x0008AA58 File Offset: 0x00088C58
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x0008AA5C File Offset: 0x00088C5C
	private void LoadVending()
	{
		this.LoadVendorsFromSpreadseet("Vendors");
		this.LoadVendingStocksFromSpreadseet("VendingStock");
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x0008AA74 File Offset: 0x00088C74
	private void LoadVendorsFromSpreadseet(string sSheetName)
	{
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(sSheetName))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(sSheetName);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + sSheetName);
			return;
		}
		int num = instance.GetNumRows(sSheetName);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + sSheetName);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<object> list = new List<object>();
		List<object> list2 = new List<object>();
		List<object> list3 = new List<object>();
		string b = "null";
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				dictionary.Clear();
				list.Clear();
				list2.Clear();
				list3.Clear();
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sSheetName, rowName, "id").ToString());
				dictionary.Add("type", "vendor");
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "general items");
					num3 = instance.GetIntCell(sheetIndex, rowIndex, "special items");
				}
				dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
				list.Add(instance.GetIntCell(sheetIndex, rowIndex, "background color r"));
				list.Add(instance.GetIntCell(sheetIndex, rowIndex, "background color g"));
				list.Add(instance.GetIntCell(sheetIndex, rowIndex, "background color b"));
				dictionary.Add("background.color", list);
				dictionary.Add("session_action_id", instance.GetStringCell(sSheetName, rowName, "session action id"));
				dictionary.Add("texture.cancelbutton", instance.GetStringCell(sSheetName, rowName, "cancel button texture"));
				dictionary.Add("texture.title", instance.GetStringCell(sSheetName, rowName, "title texture"));
				dictionary.Add("texture.titleicon", instance.GetStringCell(sSheetName, rowName, "title icon texture"));
				dictionary.Add("button.label", instance.GetStringCell(sSheetName, rowName, "button label"));
				dictionary.Add("open_sound", instance.GetStringCell(sSheetName, rowName, "open sound"));
				dictionary.Add("close_sound", instance.GetStringCell(sSheetName, rowName, "close sound"));
				dictionary.Add("restock_cost", new Dictionary<string, object>
				{
					{
						"2",
						instance.GetIntCell(sheetIndex, rowIndex, "jelly restock cost")
					}
				});
				string text = instance.GetStringCell(sSheetName, rowName, "music");
				if (string.IsNullOrEmpty(text) || text == b)
				{
					text = null;
				}
				dictionary.Add("music", text);
				for (int j = 0; j < num2; j++)
				{
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, "general item did " + (j + 1).ToString());
					if (intCell >= 0)
					{
						list2.Add(intCell);
					}
				}
				dictionary.Add("general", list2);
				for (int k = 0; k < num3; k++)
				{
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, "special item did " + (k + 1).ToString());
					if (intCell >= 0)
					{
						list3.Add(intCell);
					}
				}
				dictionary.Add("specials", list3);
				VendorDefinition vendorDefinition = new VendorDefinition(dictionary);
				if (this.vendorDefinitions.ContainsKey(vendorDefinition.did))
				{
					TFUtils.ErrorLog(string.Concat(new object[]
					{
						"Vendor Definition Collision!\nOld=",
						this.vendorDefinitions[vendorDefinition.did],
						"\nNew=",
						vendorDefinition
					}));
				}
				else
				{
					this.vendorDefinitions.Add(vendorDefinition.did, vendorDefinition);
				}
			}
		}
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x0008AE6C File Offset: 0x0008906C
	private void LoadVendingStocksFromSpreadseet(string sSheetName)
	{
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(sSheetName))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(sSheetName);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + sSheetName);
			return;
		}
		int num = instance.GetNumRows(sSheetName);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + sSheetName);
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<object> list = new List<object>();
		List<object> list2 = new List<object>();
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				dictionary.Clear();
				list.Clear();
				list2.Clear();
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sSheetName, rowName, "id").ToString());
				dictionary.Add("type", "vendor_stock");
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "max instances");
					num3 = instance.GetIntCell(sheetIndex, rowIndex, "max costs");
				}
				dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
				dictionary.Add("minimum_level", instance.GetIntCell(sheetIndex, rowIndex, "min level"));
				dictionary.Add("required_recipe", instance.GetIntCell(sheetIndex, rowIndex, "required recipe"));
				dictionary.Add("name", instance.GetStringCell(sSheetName, rowName, "name"));
				dictionary.Add("tag", instance.GetStringCell(sSheetName, rowName, "tag"));
				dictionary.Add("description", instance.GetStringCell(sSheetName, rowName, "description"));
				dictionary.Add("icon", instance.GetStringCell(sSheetName, rowName, "icon"));
				dictionary.Add("reward", new Dictionary<string, object>
				{
					{
						"resources",
						new Dictionary<string, object>
						{
							{
								instance.GetIntCell(sheetIndex, rowIndex, "recipe did").ToString(),
								1
							}
						}
					}
				});
				for (int j = 0; j < num2; j++)
				{
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, "instance " + (j + 1).ToString());
					if (intCell >= 0)
					{
						list.Add(intCell);
					}
				}
				dictionary.Add("instances", list);
				for (int k = 0; k < num3; k++)
				{
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, "cost type " + (k + 1).ToString());
					if (intCell >= 0)
					{
						float floatCell = instance.GetFloatCell(sheetIndex, rowIndex, "cost odds " + (k + 1).ToString());
						int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "cost " + (k + 1).ToString());
						list2.Add(new Dictionary<string, object>
						{
							{
								"p",
								floatCell
							},
							{
								"value",
								new Dictionary<string, object>
								{
									{
										intCell.ToString(),
										intCell2
									}
								}
							}
						});
					}
				}
				dictionary.Add("costs", list2);
				VendorStock vendorStock = VendorStock.FromDict(dictionary);
				if (this.stocks.ContainsKey(vendorStock.Did))
				{
					TFUtils.ErrorLog(string.Concat(new object[]
					{
						"Vending Stock Collision!\nOld=",
						this.stocks[vendorStock.Did],
						"\nNew=",
						vendorStock
					}));
				}
				else
				{
					this.stocks.Add(vendorStock.Did, vendorStock);
				}
			}
		}
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x0008B24C File Offset: 0x0008944C
	public void LoadVendorInstances(Identity target, Dictionary<string, object> generalInstances, Dictionary<string, object> specialInstances)
	{
		if (generalInstances != null)
		{
			Dictionary<int, VendingInstance> dictionary = new Dictionary<int, VendingInstance>(generalInstances.Count);
			foreach (KeyValuePair<string, object> keyValuePair in generalInstances)
			{
				dictionary[int.Parse(keyValuePair.Key)] = VendingInstance.FromDict((Dictionary<string, object>)keyValuePair.Value);
			}
			this.instances[target] = dictionary;
		}
		if (specialInstances != null)
		{
			Dictionary<int, VendingInstance> dictionary2 = new Dictionary<int, VendingInstance>(specialInstances.Count);
			foreach (KeyValuePair<string, object> keyValuePair2 in specialInstances)
			{
				dictionary2[int.Parse(keyValuePair2.Key)] = VendingInstance.FromDict((Dictionary<string, object>)keyValuePair2.Value);
			}
			this.specialOffers[target] = dictionary2;
		}
	}

	// Token: 0x04000E1B RID: 3611
	public const ulong DEFAULT_RESTOCK_PERIOD = 3600UL;

	// Token: 0x04000E1C RID: 3612
	public const ulong DEFAULT_SPECIAL_PERIOD = 86400UL;

	// Token: 0x04000E1D RID: 3613
	private const string _sVENDORS = "Vendors";

	// Token: 0x04000E1E RID: 3614
	private const string _sVENDING_STOCKS = "VendingStock";

	// Token: 0x04000E1F RID: 3615
	private System.Random rand;

	// Token: 0x04000E20 RID: 3616
	private static readonly string VENDING_PATH = "Vending";

	// Token: 0x04000E21 RID: 3617
	private Dictionary<int, VendorDefinition> vendorDefinitions;

	// Token: 0x04000E22 RID: 3618
	private Dictionary<int, VendorStock> stocks;

	// Token: 0x04000E23 RID: 3619
	private Dictionary<Identity, Dictionary<int, VendingInstance>> instances;

	// Token: 0x04000E24 RID: 3620
	private Dictionary<Identity, Dictionary<int, VendingInstance>> specialOffers;
}
