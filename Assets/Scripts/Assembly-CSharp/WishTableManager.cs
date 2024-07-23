using System;
using System.Collections.Generic;

// Token: 0x02000276 RID: 630
public class WishTableManager
{
	// Token: 0x06001445 RID: 5189 RVA: 0x0008B798 File Offset: 0x00089998
	public WishTableManager()
	{
		this.LoadFromSpreadsheet();
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x0008B7A8 File Offset: 0x000899A8
	public CdfDictionary<int> GetWishTable(int nDID)
	{
		if (this.m_pWishTables.ContainsKey(nDID))
		{
			return this.m_pWishTables[nDID];
		}
		return null;
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x0008B7CC File Offset: 0x000899CC
	private void LoadFromSpreadsheet()
	{
		this.m_pWishTables = new Dictionary<int, CdfDictionary<int>>();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "WishTables";
		int sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		int num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			return;
		}
		CdfDictionary<int>.ParseT parser = (object val) => Convert.ToInt32(val);
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
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "max wishes");
				}
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
				List<object> list = new List<object>();
				for (int j = 0; j < num2; j++)
				{
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "wish did " + (j + 1).ToString());
					if (intCell2 >= 0)
					{
						list.Add(new Dictionary<string, object>
						{
							{
								"p",
								instance.GetFloatCell(sheetIndex, rowIndex, "wish odds " + (j + 1).ToString())
							},
							{
								"value",
								intCell2
							}
						});
					}
				}
				this.m_pWishTables.Add(intCell, CdfDictionary<int>.FromList(list, parser));
			}
		}
	}

	// Token: 0x04000E42 RID: 3650
	private Dictionary<int, CdfDictionary<int>> m_pWishTables;
}
