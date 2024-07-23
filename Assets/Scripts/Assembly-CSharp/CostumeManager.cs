using System;
using System.Collections.Generic;

// Token: 0x02000154 RID: 340
public class CostumeManager
{
	// Token: 0x06000B83 RID: 2947 RVA: 0x00045A10 File Offset: 0x00043C10
	public CostumeManager()
	{
		this.LoadFromSpreadsheet();
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x00045A20 File Offset: 0x00043C20
	public CostumeManager.Costume GetCostume(int nCostumeDID)
	{
		if (this.m_pCostumes.ContainsKey(nCostumeDID))
		{
			return this.m_pCostumes[nCostumeDID];
		}
		return null;
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x00045A44 File Offset: 0x00043C44
	public List<CostumeManager.Costume> GetCostumesForUnit(int nUnitDID, bool bIncludeLocked = true, bool bIncludeHiddenIfLocked = true)
	{
		List<CostumeManager.Costume> list = new List<CostumeManager.Costume>();
		if (!this.m_pUnitCostumeMap.ContainsKey(nUnitDID))
		{
			return list;
		}
		List<int> list2 = this.m_pUnitCostumeMap[nUnitDID];
		int count = list2.Count;
		int i = 0;
		while (i < count)
		{
			CostumeManager.Costume costume = this.GetCostume(list2[i]);
			if (bIncludeLocked && bIncludeHiddenIfLocked)
			{
				goto IL_92;
			}
			bool flag = this.IsCostumeUnlocked(costume.m_nDID);
			if (bIncludeLocked || flag)
			{
				if (bIncludeHiddenIfLocked || flag || !costume.m_bHiddenUntilUnlocked)
				{
					goto IL_92;
				}
			}
			IL_BD:
			i++;
			continue;
			IL_92:
			if (!bIncludeLocked && !this.IsCostumeUnlocked(list2[i]))
			{
				goto IL_BD;
			}
			if (costume != null)
			{
				list.Add(costume);
				goto IL_BD;
			}
			goto IL_BD;
		}
		return list;
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00045B20 File Offset: 0x00043D20
	public bool IsCostumeUnlocked(int nCostumeDID)
	{
		return this.m_pUnlockedCostumes.Contains(nCostumeDID);
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00045B30 File Offset: 0x00043D30
	public void UnlockCostume(int nCostumeDID)
	{
		if (!this.m_pUnlockedCostumes.Contains(nCostumeDID))
		{
			this.m_pUnlockedCostumes.Add(nCostumeDID);
		}
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x00045B50 File Offset: 0x00043D50
	public void RemoveCostume(int nCostumeDID)
	{
		if (this.m_pUnlockedCostumes.Contains(nCostumeDID))
		{
			this.m_pUnlockedCostumes.Remove(nCostumeDID);
		}
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x00045B70 File Offset: 0x00043D70
	public void LockCostumeInStore(int nCostumeDID)
	{
		if (this.m_pCostumes.ContainsKey(nCostumeDID))
		{
			this.m_pCostumes[nCostumeDID].m_bLockedViaCSpanel = true;
		}
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x00045B98 File Offset: 0x00043D98
	public void UnLockCostumeInStore(int nCostumeDID)
	{
		if (this.m_pCostumes.ContainsKey(nCostumeDID))
		{
			this.m_pCostumes[nCostumeDID].m_bLockedViaCSpanel = false;
		}
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x00045BC0 File Offset: 0x00043DC0
	public bool IsCostumeValidForUnit(int nUnitDID, int nCostumeDID)
	{
		if (this.m_pCostumes.ContainsKey(nCostumeDID))
		{
			CostumeManager.Costume costume = this.m_pCostumes[nCostumeDID];
			if (costume.m_nUnitDID == nUnitDID)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x00045BFC File Offset: 0x00043DFC
	public void UnlockAllCostumes()
	{
		foreach (KeyValuePair<int, CostumeManager.Costume> keyValuePair in this.m_pCostumes)
		{
			this.UnlockCostume(keyValuePair.Value.m_nDID);
		}
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x00045C70 File Offset: 0x00043E70
	public void UnlockAllCostumesToGamestate(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		if (!dictionary.ContainsKey("costumes"))
		{
			dictionary["costumes"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["costumes"];
		foreach (KeyValuePair<int, CostumeManager.Costume> keyValuePair in this.m_pCostumes)
		{
			if (!list.Contains(keyValuePair.Value.m_nDID))
			{
				list.Add(keyValuePair.Value.m_nDID);
			}
		}
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00045D44 File Offset: 0x00043F44
	private void LoadFromSpreadsheet()
	{
		this.m_pCostumes = new Dictionary<int, CostumeManager.Costume>();
		this.m_pUnitCostumeMap = new Dictionary<int, List<int>>();
		this.m_pUnlockedCostumes = new List<int>();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "Costumes";
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
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
				dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
				dictionary.Add("unit_did", instance.GetIntCell(sheetIndex, rowIndex, "unit did"));
				dictionary.Add("wish_table_did", instance.GetIntCell(sheetIndex, rowIndex, "wishtable did"));
				dictionary.Add("unlock_level", instance.GetIntCell(sheetName, rowName, "unlock criteria level"));
				dictionary.Add("unlock_asset_did", instance.GetIntCell(sheetName, rowName, "unlock criteria asset did"));
				dictionary.Add("unlock_quest1", instance.GetIntCell(sheetName, rowName, "unlock criteria quest 1"));
				dictionary.Add("unlock_quest2", instance.GetIntCell(sheetName, rowName, "unlock criteria quest 2"));
				dictionary.Add("hidden_until_unlocked", instance.GetIntCell(sheetIndex, rowIndex, "hidden until unlocked") == 1);
				dictionary.Add("name", instance.GetStringCell(sheetName, rowName, "costume name"));
				dictionary.Add("skeleton", instance.GetStringCell(sheetName, rowName, "costume prefab"));
				dictionary.Add("texture", instance.GetStringCell(sheetName, rowName, "texture"));
				dictionary.Add("portrait", instance.GetStringCell(sheetName, rowName, "portait"));
				dictionary.Add("material", instance.GetStringCell(sheetName, rowName, "material"));
				dictionary.Add("unlock_text", instance.GetStringCell(sheetName, rowName, "unlock criteria text"));
				dictionary.Add("unlock_quest1_description", instance.GetStringCell(sheetName, rowName, "unlock criteria quest description 1"));
				dictionary.Add("unlock_quest2_description", instance.GetStringCell(sheetName, rowName, "unlock criteria quest description 2"));
				CostumeManager.Costume costume = new CostumeManager.Costume(dictionary);
				this.m_pCostumes.Add(costume.m_nDID, costume);
				if (this.m_pUnitCostumeMap.ContainsKey(costume.m_nUnitDID))
				{
					this.m_pUnitCostumeMap[costume.m_nUnitDID].Add(costume.m_nDID);
				}
				else
				{
					this.m_pUnitCostumeMap.Add(costume.m_nUnitDID, new List<int>
					{
						costume.m_nDID
					});
				}
				if (instance.GetIntCell(sheetIndex, rowIndex, "default costume") == 1)
				{
					this.m_pUnlockedCostumes.Add(costume.m_nDID);
				}
			}
		}
	}

	// Token: 0x040007A5 RID: 1957
	private Dictionary<int, CostumeManager.Costume> m_pCostumes;

	// Token: 0x040007A6 RID: 1958
	private Dictionary<int, List<int>> m_pUnitCostumeMap;

	// Token: 0x040007A7 RID: 1959
	private List<int> m_pUnlockedCostumes;

	// Token: 0x02000155 RID: 341
	public class Costume
	{
		// Token: 0x06000B8F RID: 2959 RVA: 0x0004606C File Offset: 0x0004426C
		public Costume(Dictionary<string, object> pData)
		{
			this.m_nDID = TFUtils.LoadInt(pData, "did");
			this.m_nUnitDID = TFUtils.LoadInt(pData, "unit_did");
			this.m_nWishTableDID = TFUtils.LoadInt(pData, "wish_table_did");
			this.m_nUnlockLevel = TFUtils.LoadInt(pData, "unlock_level");
			this.m_nUnlockAssetDid = TFUtils.LoadInt(pData, "unlock_asset_did");
			this.m_nUnlockQuest1 = TFUtils.LoadInt(pData, "unlock_quest1");
			this.m_nUnlockQuest2 = TFUtils.LoadInt(pData, "unlock_quest2");
			this.m_sName = TFUtils.LoadString(pData, "name");
			this.m_sTexture = TFUtils.LoadString(pData, "texture");
			this.m_sMaterial = TFUtils.LoadString(pData, "material");
			this.m_sPortrait = TFUtils.LoadString(pData, "portrait");
			this.m_sSkeleton = TFUtils.LoadString(pData, "skeleton");
			this.m_sUnlockText = TFUtils.LoadString(pData, "unlock_text");
			this.m_sUnlockQuest1Descript = TFUtils.LoadString(pData, "unlock_quest1_description");
			this.m_sUnlockQuest2Descript = TFUtils.LoadString(pData, "unlock_quest2_description");
			this.m_bHiddenUntilUnlocked = TFUtils.LoadBool(pData, "hidden_until_unlocked");
			this.m_nCriteriaCount = 0;
			if (this.m_nUnlockLevel > 0)
			{
				this.m_nCriteriaCount++;
			}
			if (this.m_nUnlockAssetDid > 0)
			{
				this.m_nCriteriaCount++;
			}
			if (this.m_nUnlockQuest1 > 0)
			{
				this.m_nCriteriaCount++;
			}
			if (this.m_nUnlockQuest2 > 0)
			{
				this.m_nCriteriaCount++;
			}
			this.m_bLockedViaCSpanel = false;
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00046208 File Offset: 0x00044408
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00046210 File Offset: 0x00044410
		public int m_nDID { get; private set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0004621C File Offset: 0x0004441C
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00046224 File Offset: 0x00044424
		public int m_nUnitDID { get; private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00046230 File Offset: 0x00044430
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x00046238 File Offset: 0x00044438
		public int m_nWishTableDID { get; private set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00046244 File Offset: 0x00044444
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x0004624C File Offset: 0x0004444C
		public int m_nUnlockLevel { get; private set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00046258 File Offset: 0x00044458
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x00046260 File Offset: 0x00044460
		public int m_nUnlockAssetDid { get; private set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0004626C File Offset: 0x0004446C
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x00046274 File Offset: 0x00044474
		public int m_nUnlockQuest1 { get; private set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x00046280 File Offset: 0x00044480
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x00046288 File Offset: 0x00044488
		public int m_nUnlockQuest2 { get; private set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00046294 File Offset: 0x00044494
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x0004629C File Offset: 0x0004449C
		public int m_nCriteriaCount { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x000462A8 File Offset: 0x000444A8
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x000462B0 File Offset: 0x000444B0
		public string m_sName { get; private set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x000462BC File Offset: 0x000444BC
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x000462C4 File Offset: 0x000444C4
		public string m_sTexture { get; private set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x000462D0 File Offset: 0x000444D0
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x000462D8 File Offset: 0x000444D8
		public string m_sMaterial { get; private set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x000462E4 File Offset: 0x000444E4
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x000462EC File Offset: 0x000444EC
		public string m_sPortrait { get; private set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x000462F8 File Offset: 0x000444F8
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x00046300 File Offset: 0x00044500
		public string m_sSkeleton { get; private set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0004630C File Offset: 0x0004450C
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x00046314 File Offset: 0x00044514
		public string m_sUnlockText { get; private set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00046320 File Offset: 0x00044520
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x00046328 File Offset: 0x00044528
		public string m_sUnlockQuest1Descript { get; private set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x00046334 File Offset: 0x00044534
		// (set) Token: 0x06000BAF RID: 2991 RVA: 0x0004633C File Offset: 0x0004453C
		public string m_sUnlockQuest2Descript { get; private set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00046348 File Offset: 0x00044548
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x00046350 File Offset: 0x00044550
		public bool m_bHiddenUntilUnlocked { get; private set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x0004635C File Offset: 0x0004455C
		// (set) Token: 0x06000BB3 RID: 2995 RVA: 0x00046364 File Offset: 0x00044564
		public bool m_bLockedViaCSpanel { get; set; }
	}
}
