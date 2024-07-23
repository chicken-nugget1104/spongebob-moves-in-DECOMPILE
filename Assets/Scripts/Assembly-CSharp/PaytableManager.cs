using System;
using System.Collections.Generic;

// Token: 0x020001BD RID: 445
public class PaytableManager
{
	// Token: 0x06000F35 RID: 3893 RVA: 0x00060ED4 File Offset: 0x0005F0D4
	public PaytableManager()
	{
		this.LoadBonusPaytables();
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x00060EF0 File Offset: 0x0005F0F0
	private string[] GetFilesToLoad()
	{
		return Config.BONUS_PAYTABLES;
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x00060EF8 File Offset: 0x0005F0F8
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x00060EFC File Offset: 0x0005F0FC
	public void LoadBonusPaytables()
	{
		TFUtils.Assert(this.paytableDefinitions == null, "Bonus Paytable Definitions have already been loaded!");
		this.paytableDefinitions = new Dictionary<uint, Paytable>();
		this.LoadFromSpreadsheet("BonusPaytables");
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x00060F34 File Offset: 0x0005F134
	private void LoadFromSpreadsheet(string pSheetName)
	{
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(pSheetName))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(pSheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		int num = instance.GetNumRows(pSheetName);
		if (num < 0)
		{
			return;
		}
		int intCell = instance.GetIntCell(sheetIndex, 0, "number of reward sets");
		string text = instance.GetStringCell(sheetIndex, 0, "number of rewards per set");
		string[] array = text.Split(new char[]
		{
			'|'
		});
		int num2 = array.Length;
		int[] array2 = new int[num2];
		int num3 = -1;
		for (int i = 0; i < num2; i++)
		{
			if (int.TryParse(array[i], out num3))
			{
				array2[i] = num3;
			}
			else
			{
				array2[i] = 0;
			}
		}
		int num4 = -1;
		Dictionary<string, object> dictionary = null;
		Dictionary<string, object> dictionary2 = null;
		for (int j = 0; j < num; j++)
		{
			text = j.ToString();
			if (!instance.HasRow(sheetIndex, text))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(pSheetName, text, "id").ToString());
				int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "paytable did");
				int intCell3 = instance.GetIntCell(pSheetName, text, "paytable trigger did");
				string key = intCell3.ToString();
				string stringCell = instance.GetStringCell(pSheetName, text, "paytable trigger type");
				if (stringCell == "task")
				{
					this.paytableTaskCheck.Add(intCell3);
				}
				int intCell4 = instance.GetIntCell(pSheetName, text, "one time reward");
				if (intCell4 == 1)
				{
				}
				if (intCell2 != num4)
				{
					num4 = intCell2;
					if (dictionary != null)
					{
						dictionary.Add("wagers", dictionary2);
						Paytable paytable = Paytable.FromDict(dictionary);
						TFUtils.Assert(!this.paytableDefinitions.ContainsKey(paytable.Did), "Loading duplicate paytable definition! Did=" + paytable.Did);
						this.paytableDefinitions[paytable.Did] = paytable;
					}
					dictionary = new Dictionary<string, object>();
					dictionary.Add("type", "bonus_paytable");
					dictionary.Add("did", num4);
					dictionary2 = new Dictionary<string, object>();
				}
				bool flag = dictionary2.ContainsKey(key);
				Dictionary<string, object> dictionary3;
				List<object> list;
				if (flag)
				{
					dictionary3 = (Dictionary<string, object>)dictionary2[key];
					list = (List<object>)dictionary3["cdf"];
				}
				else
				{
					dictionary3 = new Dictionary<string, object>();
					list = new List<object>();
				}
				for (int k = 0; k < intCell; k++)
				{
					float floatCell = instance.GetFloatCell(pSheetName, text, "set odds " + (k + 1).ToString());
					string stringCell2 = instance.GetStringCell(pSheetName, text, "set type " + (k + 1).ToString());
					if (!(stringCell2 == "none"))
					{
						Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
						Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
						Dictionary<string, object> dictionary6 = new Dictionary<string, object>();
						dictionary4.Add("p", floatCell);
						int num5 = array2[k];
						for (int l = 0; l < num5; l++)
						{
							int intCell5 = instance.GetIntCell(pSheetName, text, "set " + (k + 1).ToString() + " reward did " + (l + 1).ToString());
							if (intCell5 != -1)
							{
								int intCell6 = instance.GetIntCell(pSheetName, text, "set " + (k + 1).ToString() + " reward amount " + (l + 1).ToString());
								float floatCell2 = instance.GetFloatCell(pSheetName, text, "set " + (k + 1).ToString() + " reward odds " + (l + 1).ToString());
								string key2 = intCell5.ToString();
								flag = dictionary6.ContainsKey(key2);
								Dictionary<string, object> dictionary7;
								if (flag)
								{
									dictionary7 = (Dictionary<string, object>)dictionary6[key2];
								}
								else
								{
									dictionary7 = new Dictionary<string, object>();
								}
								dictionary7.Add(intCell6.ToString(), floatCell2);
								if (flag)
								{
									dictionary6[key2] = dictionary7;
								}
								else
								{
									dictionary6.Add(key2, dictionary7);
								}
							}
						}
						dictionary5.Add(stringCell2, dictionary6);
						dictionary4.Add("value", dictionary5);
						list.Add(dictionary4);
					}
				}
				flag = dictionary2.ContainsKey(key);
				if (flag)
				{
					dictionary3["cdf"] = list;
					dictionary2[key] = dictionary3;
				}
				else
				{
					dictionary3.Add("cdf", list);
					dictionary2.Add(key, dictionary3);
				}
			}
		}
		if (dictionary != null && num4 >= 0 && !this.paytableDefinitions.ContainsKey((uint)num4))
		{
			dictionary.Add("wagers", dictionary2);
			Paytable paytable = Paytable.FromDict(dictionary);
			TFUtils.Assert(!this.paytableDefinitions.ContainsKey(paytable.Did), "Loading duplicate paytable definition! Did=" + paytable.Did);
			this.paytableDefinitions[paytable.Did] = paytable;
		}
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x0006149C File Offset: 0x0005F69C
	public Paytable Get(uint did)
	{
		if (!this.paytableDefinitions.ContainsKey(did))
		{
			TFUtils.ErrorLog(string.Concat(new object[]
			{
				"Did not find a paytable with the definition ID=",
				did,
				". Returning default(",
				1U,
				") instead."
			}));
			return this.paytableDefinitions[1U];
		}
		return this.paytableDefinitions[did];
	}

	// Token: 0x04000A3C RID: 2620
	private const string BONUS_PAYTABLES = "BonusPaytables";

	// Token: 0x04000A3D RID: 2621
	private const uint DEFAULT_PAYTABLE = 1U;

	// Token: 0x04000A3E RID: 2622
	private Dictionary<uint, Paytable> paytableDefinitions;

	// Token: 0x04000A3F RID: 2623
	public List<int> paytableTaskCheck = new List<int>();
}
