using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class AutoQuestDatabase
{
	// Token: 0x06000E61 RID: 3681 RVA: 0x00057404 File Offset: 0x00055604
	public AutoQuestDatabase()
	{
		this.LoadDatabase();
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x00057414 File Offset: 0x00055614
	public void AddPreviousAutoQuests(int nAutoQuestDID, int nCharacterDID)
	{
		AutoQuestDatabase.m_pPreviousAutoQuests.Add(nAutoQuestDID);
		while (AutoQuestDatabase.m_pPreviousAutoQuests.Count > 3 || AutoQuestDatabase.m_pPreviousAutoQuests.Count >= this.m_pAutoQuests.Count)
		{
			AutoQuestDatabase.m_pPreviousAutoQuests.RemoveAt(0);
		}
		if (AutoQuestDatabase.m_pPreviousAutoQuestCharacters.ContainsKey(nAutoQuestDID))
		{
			if (AutoQuestDatabase.m_pPreviousAutoQuestCharacters[nAutoQuestDID].Contains(nCharacterDID))
			{
				AutoQuestDatabase.m_pPreviousAutoQuestCharacters[nAutoQuestDID] = new List<int>
				{
					nCharacterDID
				};
			}
			else
			{
				AutoQuestDatabase.m_pPreviousAutoQuestCharacters[nAutoQuestDID].Add(nCharacterDID);
			}
		}
		else
		{
			AutoQuestDatabase.m_pPreviousAutoQuestCharacters.Add(nAutoQuestDID, new List<int>
			{
				nCharacterDID
			});
		}
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x000574DC File Offset: 0x000556DC
	public static void SetPreviousAutoQuestDataFramGameState(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		if (!dictionary.ContainsKey("previous_auto_quests"))
		{
			AutoQuestDatabase.m_pPreviousAutoQuests = new List<int>();
			AutoQuestDatabase.m_pPreviousAutoQuestCharacters = new Dictionary<int, List<int>>();
			return;
		}
		Dictionary<string, object> dictionary2 = TFUtils.TryLoadDict(dictionary, "previous_auto_quests");
		if (dictionary2.ContainsKey("auto_quest_dids"))
		{
			AutoQuestDatabase.m_pPreviousAutoQuests = TFUtils.TryLoadList<int>(dictionary2, "auto_quest_dids");
		}
		else
		{
			AutoQuestDatabase.m_pPreviousAutoQuests = new List<int>();
		}
		AutoQuestDatabase.m_pPreviousAutoQuestCharacters = new Dictionary<int, List<int>>();
		if (dictionary2.ContainsKey("characters_per_quest"))
		{
			List<Dictionary<string, object>> list = TFUtils.TryLoadList<Dictionary<string, object>>(dictionary2, "characters_per_quest");
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				Dictionary<string, object> dictionary3 = list[i];
				AutoQuestDatabase.m_pPreviousAutoQuestCharacters.Add(TFUtils.LoadInt(dictionary3, "auto_quest_did"), TFUtils.TryLoadList<int>(dictionary3, "character_dids"));
			}
		}
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x000575C8 File Offset: 0x000557C8
	public static void WritePreviousAutoQuestDataToGameState(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		List<object> list = new List<object>();
		if (AutoQuestDatabase.m_pPreviousAutoQuests == null)
		{
			AutoQuestDatabase.m_pPreviousAutoQuests = new List<int>();
		}
		if (AutoQuestDatabase.m_pPreviousAutoQuestCharacters == null)
		{
			AutoQuestDatabase.m_pPreviousAutoQuestCharacters = new Dictionary<int, List<int>>();
		}
		int count = AutoQuestDatabase.m_pPreviousAutoQuests.Count;
		for (int i = 0; i < count; i++)
		{
			list.Add(AutoQuestDatabase.m_pPreviousAutoQuests[i]);
		}
		dictionary2.Add("auto_quest_dids", list);
		List<object> list2 = new List<object>();
		foreach (KeyValuePair<int, List<int>> keyValuePair in AutoQuestDatabase.m_pPreviousAutoQuestCharacters)
		{
			Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
			dictionary3.Add("auto_quest_did", keyValuePair.Key);
			int count2 = keyValuePair.Value.Count;
			List<object> list3 = new List<object>();
			for (int j = 0; j < count2; j++)
			{
				list3.Add(keyValuePair.Value[j]);
			}
			dictionary3.Add("character_dids", list3);
			list2.Add(dictionary3);
		}
		dictionary2.Add("characters_per_quest", list2);
		if (dictionary.ContainsKey("previous_auto_quests"))
		{
			dictionary["previous_auto_quests"] = dictionary2;
		}
		else
		{
			dictionary.Add("previous_auto_quests", dictionary2);
		}
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x00057770 File Offset: 0x00055970
	public AutoQuestData.DialogData GetDialogData(int nAutoQuestDID, int nCharacterDID)
	{
		if (!this.m_pAutoQuests.ContainsKey(nAutoQuestDID))
		{
			return null;
		}
		return this.m_pAutoQuests[nAutoQuestDID].GetDialogData(nCharacterDID);
	}

	// Token: 0x06000E66 RID: 3686 RVA: 0x00057798 File Offset: 0x00055998
	public AutoQuest GenerateNextAutoQuest(Game pGame)
	{
		if (this.m_pAutoQuests == null)
		{
			return null;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, AutoQuestData> keyValuePair in this.m_pAutoQuests)
		{
			if (this.IsAutoQuestAvailable(pGame, keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		int count = list.Count;
		if (count <= 0)
		{
			return null;
		}
		return this.GenerateAutoQuest(pGame, list[UnityEngine.Random.Range(0, count)]);
	}

	// Token: 0x06000E67 RID: 3687 RVA: 0x00057850 File Offset: 0x00055A50
	public bool IsQuestValid(Game pGame, QuestDefinition pQuestDef)
	{
		CraftingManager craftManager = pGame.craftManager;
		List<int> list = new List<int>();
		List<QuestBookendInfo.ChunkConditions> chunks = pQuestDef.End.Chunks;
		int num = chunks.Count - 1;
		for (int i = 0; i < num; i++)
		{
			QuestBookendInfo.ChunkConditions chunkConditions = chunks[i];
			Dictionary<string, object> dictionary = chunkConditions.Condition.ToDict();
			if (dictionary.ContainsKey("resource_id"))
			{
				list.Add(TFUtils.LoadInt(dictionary, "resource_id"));
			}
		}
		int count = list.Count;
		for (int j = 0; j < count; j++)
		{
			int productId = list[j];
			CraftingRecipe recipeByProductId = craftManager.GetRecipeByProductId(productId);
			if (recipeByProductId == null)
			{
				return false;
			}
			if (!craftManager.IsRecipeUnlocked(recipeByProductId.identity))
			{
				return false;
			}
			if (recipeByProductId.buildingId >= 0 && pGame.simulation.FindSimulated(new int?(recipeByProductId.buildingId)) == null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000E68 RID: 3688 RVA: 0x00057954 File Offset: 0x00055B54
	private void LoadDatabase()
	{
		this.m_pCategoryItems = new Dictionary<string, List<int>>();
		this.m_pAutoQuests = new Dictionary<int, AutoQuestData>();
		AutoQuestDatabase.m_pPreviousAutoQuests = new List<int>();
		AutoQuestDatabase.m_pPreviousAutoQuestCharacters = new Dictionary<int, List<int>>();
		this.LoadCategories();
		this.LoadAutoQuests();
	}

	// Token: 0x06000E69 RID: 3689 RVA: 0x00057998 File Offset: 0x00055B98
	private AutoQuest GenerateAutoQuest(Game pGame, int nDID)
	{
		if (nDID == 2 || nDID == 4)
		{
		}
		AutoQuestData autoQuestData = this.m_pAutoQuests[nDID];
		CraftingManager craftManager = pGame.craftManager;
		ResourceManager resourceManager = pGame.resourceManager;
		int num = UnityEngine.Random.Range(autoQuestData.m_nMinItems, autoQuestData.m_nMaxItems + 1);
		ulong num2 = 0UL;
		ulong num3 = (ulong)((long)pGame.levelingManager.AutoQuestLength(pGame.resourceManager.Query(ResourceManager.LEVEL)));
		float num4 = 0f;
		int num5 = 0;
		int num6 = 0;
		string[] itemCategories = autoQuestData.GetItemCategories();
		bool[] pickOneCategories = autoQuestData.GetPickOneCategories();
		int num7 = itemCategories.Length;
		int[] characters = autoQuestData.GetCharacters();
		int num8 = characters.Length;
		List<int> list = null;
		List<int> list2 = new List<int>();
		int nCharacterDID = 0;
		AutoQuestDatabase.m_pPreviousAutoQuestCharacters.TryGetValue(nDID, out list);
		for (int i = 0; i < num8; i++)
		{
			if ((list == null || !list.Contains(characters[i])) && pGame.simulation.FindSimulated(new int?(characters[i])) != null)
			{
				list2.Add(characters[i]);
			}
		}
		if (list2.Count <= 0)
		{
			for (int j = 0; j < num8; j++)
			{
				if (pGame.simulation.FindSimulated(new int?(characters[j])) != null)
				{
					list2.Add(characters[j]);
				}
			}
			if (list2.Count < 0)
			{
				TFUtils.ErrorLog("AutoQuestDatabase | GenerateAutoQuest: No available character for quest id " + nDID.ToString() + ".");
				return null;
			}
			nCharacterDID = list2[UnityEngine.Random.Range(0, list2.Count)];
		}
		else
		{
			nCharacterDID = list2[UnityEngine.Random.Range(0, list2.Count)];
		}
		Dictionary<int, int>[] array = new Dictionary<int, int>[num7];
		List<int>[] array2 = new List<int>[num7];
		List<int>[] array3 = new List<int>[num7];
		int l;
		for (int k = 0; k < num7; k++)
		{
			array[k] = new Dictionary<int, int>();
			array2[k] = new List<int>();
			array3[k] = new List<int>();
			string key = itemCategories[k];
			if (this.m_pCategoryItems.ContainsKey(key))
			{
				List<int> list3 = this.m_pCategoryItems[key];
				l = list3.Count;
				for (int m = 0; m < l; m++)
				{
					if (craftManager.IsRecipeUnlocked(list3[m]))
					{
						CraftingRecipe craftingRecipe = craftManager.GetRecipeById(list3[m]);
						if (craftingRecipe != null)
						{
							if (craftingRecipe.buildingId < 0 || pGame.simulation.FindSimulated(new int?(craftingRecipe.buildingId)) != null)
							{
								if (!resourceManager.Resources.ContainsKey(craftingRecipe.productId))
								{
									TFUtils.ErrorLog(string.Concat(new object[]
									{
										"Could not find product did: ",
										craftingRecipe.productId,
										" for recipe did: ",
										craftingRecipe.identity,
										" in resources. "
									}));
								}
								else if (resourceManager.Resources[craftingRecipe.productId].Amount > 0)
								{
									array[k].Add(craftingRecipe.productId, resourceManager.Resources[craftingRecipe.productId].Amount);
								}
								else
								{
									array2[k].Add(craftingRecipe.productId);
								}
							}
						}
						else
						{
							TFUtils.ErrorLog("AutoQuestDatabase | failed to find crafting recipe with did: " + list3[m].ToString());
						}
					}
				}
			}
		}
		AutoQuestData.eDistributionType eDistribution = autoQuestData.m_eDistribution;
		if (eDistribution != AutoQuestData.eDistributionType.eEqual)
		{
			if (eDistribution == AutoQuestData.eDistributionType.eRandom)
			{
				if (num7 > 0)
				{
					l = 0;
					while (num > 0 && num2 < num3)
					{
						if (l >= num7)
						{
							l = 0;
						}
						if (pickOneCategories[l] && array3[l].Count > 0)
						{
							array3[l].Add(array3[l][0]);
							CraftingRecipe craftingRecipe = craftManager.GetRecipeByProductId(array3[l][0]);
							Reward reward = craftingRecipe.rewardDefinition.GenerateReward(pGame.simulation, true, false);
							num2 += craftingRecipe.craftTime;
							num5 += this.GetXpForCraftingRecipe(pGame, craftingRecipe);
							num4 += this.GetGoldForCraftingRecipe(pGame, craftingRecipe);
							num--;
							l++;
						}
						else
						{
							int num9 = -1;
							if ((num6 > 0 && array2[l].Count > 0) || array[l].Count <= 0)
							{
								int num10 = (!pickOneCategories[l]) ? (array2[l].Count + 1) : array2[l].Count;
								num10 = UnityEngine.Random.Range(0, num10);
								if (num10 != array2[l].Count)
								{
									num9 = array2[l][num10];
									num6--;
								}
							}
							else
							{
								int num10 = (!pickOneCategories[l]) ? (array[l].Count + 1) : array[l].Count;
								num10 = UnityEngine.Random.Range(0, num10);
								if (num10 != array[l].Count)
								{
									int num11 = 0;
									foreach (KeyValuePair<int, int> keyValuePair in array[l])
									{
										if (num11 == num10)
										{
											num9 = keyValuePair.Key;
											array[l][keyValuePair.Key] = array[l][keyValuePair.Key] - 1;
											if (array[l][keyValuePair.Key] <= 0)
											{
												array[l].Remove(keyValuePair.Key);
												array2[l].Add(keyValuePair.Key);
											}
											num6++;
											break;
										}
										num11++;
									}
								}
							}
							if (num9 >= 0)
							{
								array3[l].Add(num9);
								CraftingRecipe craftingRecipe = craftManager.GetRecipeByProductId(num9);
								Reward reward = craftingRecipe.rewardDefinition.GenerateReward(pGame.simulation, true, false);
								num2 += craftingRecipe.craftTime;
								num5 += this.GetXpForCraftingRecipe(pGame, craftingRecipe);
								num4 += this.GetGoldForCraftingRecipe(pGame, craftingRecipe);
								num--;
							}
							l++;
						}
					}
				}
			}
		}
		else if (num7 > 0)
		{
			num = Mathf.RoundToInt((float)num / (float)num7);
			for (int n = 0; n < num; n++)
			{
				if (num2 >= num3)
				{
					break;
				}
				for (l = 0; l < num7; l++)
				{
					if (num2 >= num3)
					{
						break;
					}
					if (pickOneCategories[l] && array3[l].Count > 0)
					{
						array3[l].Add(array3[l][0]);
						CraftingRecipe craftingRecipe = craftManager.GetRecipeByProductId(array3[l][0]);
						Reward reward = craftingRecipe.rewardDefinition.GenerateReward(pGame.simulation, true, false);
						num2 += craftingRecipe.craftTime;
						num5 += this.GetXpForCraftingRecipe(pGame, craftingRecipe);
						num4 += this.GetGoldForCraftingRecipe(pGame, craftingRecipe);
					}
					else
					{
						int num9 = -1;
						if ((num6 > 0 && array2[l].Count > 0) || array[l].Count <= 0)
						{
							int num10 = UnityEngine.Random.Range(0, array2[l].Count);
							if (num10 != array2[l].Count)
							{
								num9 = array2[l][num10];
								num6--;
							}
						}
						else
						{
							int num10 = UnityEngine.Random.Range(0, array[l].Count);
							if (num10 != array[l].Count)
							{
								int num11 = 0;
								foreach (KeyValuePair<int, int> keyValuePair2 in array[l])
								{
									if (num11 == num10)
									{
										num9 = keyValuePair2.Key;
										array[l][keyValuePair2.Key] = array[l][keyValuePair2.Key] - 1;
										if (array[l][keyValuePair2.Key] <= 0)
										{
											array[l].Remove(keyValuePair2.Key);
											array2[l].Add(keyValuePair2.Key);
										}
										num6++;
										break;
									}
									num11++;
								}
							}
						}
						if (num9 >= 0)
						{
							array3[l].Add(num9);
							CraftingRecipe craftingRecipe = craftManager.GetRecipeByProductId(num9);
							Reward reward = craftingRecipe.rewardDefinition.GenerateReward(pGame.simulation, true, false);
							num2 += craftingRecipe.craftTime;
							num5 += this.GetXpForCraftingRecipe(pGame, craftingRecipe);
							num4 += this.GetGoldForCraftingRecipe(pGame, craftingRecipe);
						}
					}
				}
			}
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		l = array3.Length;
		for (int num12 = 0; num12 < l; num12++)
		{
			int num11 = array3[num12].Count;
			for (int num13 = 0; num13 < num11; num13++)
			{
				if (dictionary.ContainsKey(array3[num12][num13]))
				{
					Dictionary<int, int> dictionary3;
					Dictionary<int, int> dictionary2 = dictionary3 = dictionary;
					int num14;
					int key2 = num14 = array3[num12][num13];
					num14 = dictionary3[num14];
					dictionary2[key2] = num14 + 1;
				}
				else
				{
					dictionary.Add(array3[num12][num13], 1);
				}
			}
		}
		return new AutoQuest(nDID, nCharacterDID, dictionary, Mathf.RoundToInt(num4 * autoQuestData.m_fGoldMultiplier), Mathf.RoundToInt((float)num5 * autoQuestData.m_fExpMultiplier), autoQuestData.m_sName, autoQuestData.m_sDescription);
	}

	// Token: 0x06000E6A RID: 3690 RVA: 0x00058394 File Offset: 0x00056594
	private int GetXpForCraftingRecipe(Game pGame, CraftingRecipe pCraftingRecipe)
	{
		int num = 0;
		Resource resource = pGame.resourceManager.Resources[pCraftingRecipe.productId];
		if (resource.Reward != null)
		{
			num = resource.Reward.LowestResourceValue(ResourceManager.XP);
		}
		Cost cost = pCraftingRecipe.cost;
		if (cost != null)
		{
			foreach (KeyValuePair<int, int> keyValuePair in cost.ResourceAmounts)
			{
				if (keyValuePair.Key != ResourceManager.SOFT_CURRENCY && keyValuePair.Key != ResourceManager.HARD_CURRENCY)
				{
					CraftingRecipe recipeByProductId = pGame.craftManager.GetRecipeByProductId(keyValuePair.Key);
					if (recipeByProductId != null && recipeByProductId.cost != null)
					{
						num += this.GetXpForCraftingRecipe(pGame, recipeByProductId) * keyValuePair.Value;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06000E6B RID: 3691 RVA: 0x00058494 File Offset: 0x00056694
	private float GetGoldForCraftingRecipe(Game pGame, CraftingRecipe pCraftingRecipe)
	{
		int num = pCraftingRecipe.rewardDefinition.LowestResourceValue(pCraftingRecipe.productId);
		float num2 = this.GetGoldForCost(pGame, pGame.craftManager, pCraftingRecipe.cost) / (float)num;
		if (pCraftingRecipe.cost.ResourceAmounts.Count <= 0 && pCraftingRecipe.buildingId >= 0)
		{
			int num3 = Mathf.RoundToInt(1f / pGame.resourceManager.Resources[pCraftingRecipe.productId].HardCurrencyConversion);
			num2 += (float)num3 * pGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].HardCurrencyConversion;
		}
		Resource resource = pGame.resourceManager.Resources[pCraftingRecipe.productId];
		if (resource.Reward != null)
		{
			int num4 = resource.Reward.LowestResourceValue(ResourceManager.HARD_CURRENCY);
			if (num4 > 0)
			{
				num2 += (float)num4 * pGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].HardCurrencyConversion;
			}
			else
			{
				num4 = resource.Reward.LowestResourceValue(ResourceManager.SOFT_CURRENCY);
				num2 += (float)num4;
			}
		}
		return num2;
	}

	// Token: 0x06000E6C RID: 3692 RVA: 0x000585B0 File Offset: 0x000567B0
	private float GetGoldForCost(Game pGame, CraftingManager pCraftingManager, Cost pCost)
	{
		float num = 0f;
		foreach (KeyValuePair<int, int> keyValuePair in pCost.ResourceAmounts)
		{
			if (keyValuePair.Key == ResourceManager.SOFT_CURRENCY)
			{
				num += (float)keyValuePair.Value;
			}
			else if (keyValuePair.Key == ResourceManager.HARD_CURRENCY)
			{
				num += (float)keyValuePair.Value * pGame.resourceManager.Resources[ResourceManager.SOFT_CURRENCY].HardCurrencyConversion;
			}
			else
			{
				CraftingRecipe recipeByProductId = pCraftingManager.GetRecipeByProductId(keyValuePair.Key);
				if (recipeByProductId != null && recipeByProductId.cost != null)
				{
					num += this.GetGoldForCraftingRecipe(pGame, recipeByProductId) * (float)keyValuePair.Value;
				}
			}
		}
		return num;
	}

	// Token: 0x06000E6D RID: 3693 RVA: 0x000586A8 File Offset: 0x000568A8
	private bool IsAutoQuestAvailable(Game pGame, int nDID)
	{
		if (pGame == null || this.m_pAutoQuests == null || !this.m_pAutoQuests.ContainsKey(nDID))
		{
			return false;
		}
		CraftingManager craftManager = pGame.craftManager;
		Simulation simulation = pGame.simulation;
		AutoQuestData autoQuestData = this.m_pAutoQuests[nDID];
		string[] itemCategories = autoQuestData.GetItemCategories();
		int num = AutoQuestDatabase.m_pPreviousAutoQuests.Count;
		for (int i = 0; i < num; i++)
		{
			if (AutoQuestDatabase.m_pPreviousAutoQuests[i] == nDID)
			{
				return false;
			}
		}
		int[] characters = autoQuestData.GetCharacters();
		num = characters.Length;
		bool flag = false;
		for (int j = 0; j < num; j++)
		{
			if (simulation.FindSimulated(new int?(characters[j])) != null)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		num = itemCategories.Length;
		for (int k = 0; k < num; k++)
		{
			string key = itemCategories[k];
			if (!this.m_pCategoryItems.ContainsKey(key))
			{
				return false;
			}
			List<int> list = this.m_pCategoryItems[key];
			int count = list.Count;
			flag = false;
			for (int l = 0; l < count; l++)
			{
				if (craftManager.IsRecipeUnlocked(list[l]))
				{
					CraftingRecipe recipeById = craftManager.GetRecipeById(list[l]);
					if (recipeById.buildingId < 0 || simulation.FindSimulated(new int?(recipeById.buildingId)) != null)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000E6E RID: 3694 RVA: 0x0005884C File Offset: 0x00056A4C
	private void LoadCategories()
	{
		string text = "AutoQuestCategories";
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
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "items");
				}
				List<int> list = new List<int>();
				for (int j = 1; j <= num2; j++)
				{
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, "item did " + j.ToString());
					if (intCell >= 0)
					{
						list.Add(intCell);
					}
				}
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "category name");
				if (this.m_pCategoryItems.ContainsKey(stringCell))
				{
					TFUtils.Assert(false, "Trying to add duplicate Auto Quest Category: " + stringCell + ".");
				}
				else
				{
					this.m_pCategoryItems.Add(stringCell, list);
				}
			}
		}
	}

	// Token: 0x06000E6F RID: 3695 RVA: 0x000589B8 File Offset: 0x00056BB8
	private void LoadAutoQuests()
	{
		string text = "AutoQuest";
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
		string b = "n/a";
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
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "item categories");
					num3 = instance.GetIntCell(sheetIndex, rowIndex, "characters");
				}
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
				if (this.m_pAutoQuests.ContainsKey(intCell))
				{
					TFUtils.Assert(false, "Trying to add duplicate Auto Quest: " + intCell + ".");
				}
				else
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2.Add("did", intCell);
					dictionary2.Add("min_items", instance.GetIntCell(sheetIndex, rowIndex, "min total items"));
					dictionary2.Add("max_items", instance.GetIntCell(sheetIndex, rowIndex, "max total items"));
					dictionary2.Add("exp_multiplier", instance.GetFloatCell(sheetIndex, rowIndex, "xp multiplier"));
					dictionary2.Add("gold_multiplier", instance.GetFloatCell(sheetIndex, rowIndex, "gold multiplier"));
					dictionary2.Add("distribution", instance.GetStringCell(sheetIndex, rowIndex, "distribution"));
					dictionary2.Add("item_categories", new List<string>());
					dictionary2.Add("pick_one_categories", new List<bool>());
					for (int j = 1; j <= num2; j++)
					{
						string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "item category " + j.ToString());
						if (string.IsNullOrEmpty(stringCell) || stringCell == b)
						{
							break;
						}
						((List<string>)dictionary2["item_categories"]).Add(stringCell);
						((List<bool>)dictionary2["pick_one_categories"]).Add(instance.GetIntCell(sheetIndex, rowIndex, "pick one category " + j.ToString()) == 1);
					}
					dictionary2.Add("characters", new List<int>());
					for (int k = 1; k <= num3; k++)
					{
						intCell = instance.GetIntCell(sheetIndex, rowIndex, "character " + k.ToString());
						if (intCell < 0)
						{
							break;
						}
						((List<int>)dictionary2["characters"]).Add(intCell);
					}
					dictionary.Add((int)dictionary2["did"], dictionary2);
				}
			}
		}
		text = "AutoQuestDialog";
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
		int num4 = -1;
		for (int l = 0; l < num; l++)
		{
			string rowName = l.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				if (num4 < 0)
				{
					num4 = instance.GetIntCell(sheetIndex, rowIndex, "num dialogs");
				}
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
				if (!dictionary.ContainsKey(intCell))
				{
					TFUtils.ErrorLog("AutoQuestDialog: No auto quest found for did: " + intCell.ToString());
				}
				else
				{
					Dictionary<string, object> dictionary2 = dictionary[intCell];
					dictionary2.Add("name", instance.GetStringCell(sheetIndex, rowIndex, "autoQuest title"));
					dictionary2.Add("description", instance.GetStringCell(sheetIndex, rowIndex, "autoQuest description"));
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					for (int m = 1; m <= num4; m++)
					{
						string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "intro dialog " + m.ToString());
						if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
						{
							dictionary3.Add(m.ToString(), stringCell);
						}
					}
					dictionary2.Add("intro_dialog_data", dictionary3);
					dictionary3 = new Dictionary<string, object>();
					for (int n = 1; n <= num4; n++)
					{
						string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "outro dialog " + n.ToString());
						if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
						{
							dictionary3.Add(n.ToString(), stringCell);
						}
					}
					dictionary2.Add("outro_dialog_data", dictionary3);
				}
			}
		}
		foreach (KeyValuePair<int, Dictionary<string, object>> keyValuePair in dictionary)
		{
			AutoQuestData autoQuestData = new AutoQuestData(keyValuePair.Value);
			this.m_pAutoQuests.Add(autoQuestData.m_nDID, autoQuestData);
		}
	}

	// Token: 0x0400097E RID: 2430
	private const int _nNumSavedAutoQuests = 3;

	// Token: 0x0400097F RID: 2431
	private Dictionary<string, List<int>> m_pCategoryItems;

	// Token: 0x04000980 RID: 2432
	private Dictionary<int, AutoQuestData> m_pAutoQuests;

	// Token: 0x04000981 RID: 2433
	private static List<int> m_pPreviousAutoQuests;

	// Token: 0x04000982 RID: 2434
	private static Dictionary<int, List<int>> m_pPreviousAutoQuestCharacters;
}
