using System;
using System.Collections.Generic;

// Token: 0x02000158 RID: 344
public class CraftingManager
{
	// Token: 0x06000BBF RID: 3007 RVA: 0x0004663C File Offset: 0x0004483C
	public CraftingManager()
	{
		this.cookbooks = new Dictionary<int, CraftingCookbook>();
		this.recipes = new Dictionary<int, CraftingRecipe>();
		this.activeCrafts = new Dictionary<Identity, Dictionary<int, CraftingInstance>>();
		this.prodSlotTables = new Dictionary<int, ProductionSlotTable>();
		this.unlockedRecipes = new HashSet<int>();
		this.unlockedProductsShallow = new HashSet<int>();
		this.unlockedProductsDeep = new HashSet<int>();
		this.reservedRecipes = new HashSet<int>();
		this.jellyBasedRecipes = new HashSet<int>();
		this.ignoreRandomQuestRecipes = new HashSet<int>();
		this.LoadCrafting();
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x000466DC File Offset: 0x000448DC
	public CraftingCookbook GetCookbookById(int id)
	{
		TFUtils.Assert(this.cookbooks.ContainsKey(id), "Crafting Manager does not have a cookbook with id = " + id);
		return this.cookbooks[id];
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x0004670C File Offset: 0x0004490C
	public bool ContainsRecipe(int id)
	{
		return this.recipes.ContainsKey(id);
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x0004671C File Offset: 0x0004491C
	public CraftingRecipe GetRecipeById(int id)
	{
		TFUtils.Assert(this.recipes.ContainsKey(id), "Crafting Manager does not have a recipe with id = " + id);
		return this.recipes[id];
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x0004674C File Offset: 0x0004494C
	public CraftingRecipe GetRecipeByProductId(int productId)
	{
		foreach (KeyValuePair<int, CraftingRecipe> keyValuePair in this.recipes)
		{
			if (keyValuePair.Value.productId == productId)
			{
				return keyValuePair.Value;
			}
		}
		TFUtils.ErrorLog("Crafting Manager does not have a recipe with product_id = " + productId);
		return null;
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x000467E4 File Offset: 0x000449E4
	public void UnlockRecipe(int id, Game game)
	{
		if (!this.recipes.ContainsKey(id))
		{
			TFUtils.ErrorLog("Cannot unlock recipe with id=" + id + " since it was not loaded [properly]");
			return;
		}
		int productId = this.recipes[id].productId;
		this.unlockedRecipes.Add(id);
		foreach (KeyValuePair<int, int> keyValuePair in this.recipes[id].cost.ResourceAmounts)
		{
			if (keyValuePair.Key == ResourceManager.HARD_CURRENCY)
			{
				this.jellyBasedRecipes.Add(productId);
			}
		}
		if (this.recipes[id].ignoreRandomRecipeQuest)
		{
			this.ignoreRandomQuestRecipes.Add(productId);
		}
		if (!this.unlockedProductsShallow.Contains(productId))
		{
			this.unlockedProductsShallow.Add(productId);
		}
		foreach (KeyValuePair<int, CraftingRecipe> keyValuePair2 in this.recipes)
		{
			if (this.unlockedProductsShallow.Contains(keyValuePair2.Key) && !this.unlockedProductsDeep.Contains(keyValuePair2.Key))
			{
				bool flag = true;
				foreach (KeyValuePair<int, int> keyValuePair3 in this.recipes[keyValuePair2.Key].cost.ResourceAmounts)
				{
					if (!this.unlockedProductsShallow.Contains(keyValuePair3.Key) && keyValuePair3.Key != ResourceManager.HARD_CURRENCY && keyValuePair3.Key != ResourceManager.SOFT_CURRENCY)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.unlockedProductsDeep.Add(keyValuePair2.Key);
				}
			}
		}
		game.triggerRouter.RouteTrigger(UnlockableTrigger.CreateTrigger("recipe", id), game);
		this.UnlockedEvent.FireEvent();
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00046A64 File Offset: 0x00044C64
	public bool LockRecipe(int id)
	{
		if (!this.recipes.ContainsKey(id))
		{
			TFUtils.ErrorLog("Cannot unlock recipe with id=" + id + " since it was not loaded [properly]");
			return false;
		}
		int productId = this.recipes[id].productId;
		this.unlockedRecipes.Remove(id);
		this.jellyBasedRecipes.Remove(productId);
		this.ignoreRandomQuestRecipes.Remove(productId);
		this.unlockedProductsShallow.Remove(productId);
		this.unlockedProductsDeep.Remove(id);
		this.unlockedProductsDeep.Remove(productId);
		return true;
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00046B00 File Offset: 0x00044D00
	public void UnlockAllRecipes(Game game)
	{
		foreach (KeyValuePair<int, CraftingRecipe> keyValuePair in this.recipes)
		{
			int key = keyValuePair.Key;
			if (!this.unlockedRecipes.Contains(key))
			{
				this.UnlockRecipe(key, game);
			}
		}
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00046B80 File Offset: 0x00044D80
	public void UnlockAllRecipesToGamestate(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (!dictionary.ContainsKey("recipes"))
		{
			dictionary["recipes"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["recipes"];
		foreach (KeyValuePair<int, CraftingRecipe> keyValuePair in this.recipes)
		{
			int key = keyValuePair.Key;
			if (!list.Contains(key))
			{
				list.Add(key);
			}
		}
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00046C4C File Offset: 0x00044E4C
	public bool CanMakeRecipe(int id)
	{
		foreach (KeyValuePair<int, int> keyValuePair in this.recipes[id].cost.ResourceAmounts)
		{
			if (!this.unlockedProductsShallow.Contains(keyValuePair.Key) && keyValuePair.Key != ResourceManager.HARD_CURRENCY && keyValuePair.Key != ResourceManager.SOFT_CURRENCY)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000BCA RID: 3018 RVA: 0x00046D00 File Offset: 0x00044F00
	public Dictionary<int, CraftingRecipe> Recipes
	{
		get
		{
			return this.recipes;
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x06000BCB RID: 3019 RVA: 0x00046D08 File Offset: 0x00044F08
	public HashSet<int> UnlockedRecipesCopy
	{
		get
		{
			return new HashSet<int>(this.unlockedRecipes);
		}
	}

	// Token: 0x17000196 RID: 406
	// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00046D18 File Offset: 0x00044F18
	public HashSet<int> UnlockedProductsShallowCopy
	{
		get
		{
			return new HashSet<int>(this.unlockedProductsShallow);
		}
	}

	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06000BCD RID: 3021 RVA: 0x00046D28 File Offset: 0x00044F28
	public HashSet<int> UnlockedProductsDeepCopy
	{
		get
		{
			return new HashSet<int>(this.unlockedProductsDeep);
		}
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06000BCE RID: 3022 RVA: 0x00046D38 File Offset: 0x00044F38
	public HashSet<int> ReservedRecipesCopy
	{
		get
		{
			return new HashSet<int>(this.reservedRecipes);
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06000BCF RID: 3023 RVA: 0x00046D48 File Offset: 0x00044F48
	public HashSet<int> JellyBasedRecipesCopy
	{
		get
		{
			return new HashSet<int>(this.jellyBasedRecipes);
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00046D58 File Offset: 0x00044F58
	public HashSet<int> IgnoreRandomQuestRecipesCopy
	{
		get
		{
			return new HashSet<int>(this.ignoreRandomQuestRecipes);
		}
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x00046D68 File Offset: 0x00044F68
	public void ReserveRecipe(int recipeId)
	{
		this.reservedRecipes.Add(recipeId);
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x00046D78 File Offset: 0x00044F78
	public int GetNumUnlockedComplexRecipes()
	{
		int num = 0;
		foreach (int id in this.unlockedRecipes)
		{
			CraftingRecipe recipeById = this.GetRecipeById(id);
			if (this.IsComplexRecipe(recipeById))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x00046DF4 File Offset: 0x00044FF4
	public bool IsComplexRecipe(CraftingRecipe recipe)
	{
		return recipe.cost.ResourceAmounts.Count != 0 && !recipe.ignoreRecipeCap && (recipe.cost.ResourceAmounts.Count != 1 || !recipe.cost.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY));
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x00046E54 File Offset: 0x00045054
	public bool IsRecipeUnlocked(int id)
	{
		return this.unlockedRecipes.Contains(id) || this.reservedRecipes.Contains(id);
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x00046E84 File Offset: 0x00045084
	public int GetNextSlot(Identity id, int maxSlot)
	{
		if (maxSlot == -1)
		{
			return -1;
		}
		if (this.activeCrafts.ContainsKey(id))
		{
			for (int i = 0; i < maxSlot; i++)
			{
				if (!this.activeCrafts[id].ContainsKey(i))
				{
					return i;
				}
			}
			return -1;
		}
		return 0;
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x00046EDC File Offset: 0x000450DC
	public bool AddCraftingInstance(CraftingInstance instance)
	{
		if (!this.activeCrafts.ContainsKey(instance.buildingLabel))
		{
			this.activeCrafts[instance.buildingLabel] = new Dictionary<int, CraftingInstance>();
		}
		if (this.activeCrafts[instance.buildingLabel].ContainsKey(instance.slotId))
		{
			TFUtils.ErrorLog(string.Format("already have a crafting instance {0} for building {1}", this.activeCrafts[instance.buildingLabel][instance.slotId], instance.buildingLabel));
			return false;
		}
		this.activeCrafts[instance.buildingLabel].Add(instance.slotId, instance);
		return true;
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x00046F88 File Offset: 0x00045188
	public CraftingInstance GetCraftingInstance(Identity id, int slot)
	{
		if (this.activeCrafts.ContainsKey(id) && this.activeCrafts[id].ContainsKey(slot))
		{
			return this.activeCrafts[id][slot];
		}
		return null;
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x00046FD4 File Offset: 0x000451D4
	public void RemoveCraftingInstance(Identity id, int slot)
	{
		TFUtils.Assert(this.activeCrafts.ContainsKey(id) && this.activeCrafts[id][slot] != null, "Trying to remove an instance which does not exist for id: " + id.Describe());
		this.activeCrafts[id].Remove(slot);
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x00047038 File Offset: 0x00045238
	public bool Crafting(Identity id)
	{
		return this.activeCrafts.ContainsKey(id) && this.activeCrafts[id].Count > 0;
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x0004706C File Offset: 0x0004526C
	public bool HasCapacity(Identity id, int maxSlots)
	{
		return !this.activeCrafts.ContainsKey(id) || this.activeCrafts[id].Count < maxSlots;
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x000470A0 File Offset: 0x000452A0
	public bool HasInitialSlots(int did)
	{
		return this.prodSlotTables.ContainsKey(did);
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x000470B0 File Offset: 0x000452B0
	public int GetInitialSlots(int did)
	{
		if (this.prodSlotTables.ContainsKey(did))
		{
			return this.prodSlotTables[did].MinSlots;
		}
		return -1;
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x000470E4 File Offset: 0x000452E4
	public int GetMaxSlots(int did)
	{
		if (this.prodSlotTables.ContainsKey(did))
		{
			return this.prodSlotTables[did].MaxSlots;
		}
		return -1;
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x00047118 File Offset: 0x00045318
	public Cost GetSlotExpandCost(int did, int slotId)
	{
		if (this.prodSlotTables.ContainsKey(did))
		{
			return this.prodSlotTables[did].GetCostForSlot(slotId);
		}
		return null;
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x00047140 File Offset: 0x00045340
	private string[] GetFilesToLoad()
	{
		return Config.CRAFTING_PATH;
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x00047148 File Offset: 0x00045348
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x0004714C File Offset: 0x0004534C
	private void LoadCrafting()
	{
		this.LoadRecipesFromSpreadseet("Recipes");
		this.LoadCookbooksFromSpreadseet("CraftBuildings");
		this.LoadProductionSlotsFromSpreadseet("ProductionSlots");
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x00047170 File Offset: 0x00045370
	private void LoadRecipesFromSpreadseet(string sSheetName)
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
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary6 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary7 = new Dictionary<string, object>();
		Dictionary<string, object> dictionary8 = new Dictionary<string, object>();
		List<object> list = new List<object>();
		string b = "n/a";
		int num2 = 6;
		int num3 = 3;
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
				dictionary2.Clear();
				dictionary3.Clear();
				dictionary4.Clear();
				dictionary5.Clear();
				dictionary6.Clear();
				list.Clear();
				dictionary8.Clear();
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sSheetName, rowName, "id").ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "product id");
				dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "did"));
				dictionary.Add("product_id", intCell);
				dictionary.Add("building_id", instance.GetIntCell(sheetIndex, rowIndex, "building id"));
				dictionary.Add("craft.time", instance.GetIntCell(sheetIndex, rowIndex, "craft time"));
				dictionary.Add("minimum_level", instance.GetIntCell(sheetIndex, rowIndex, "minimum level"));
				dictionary.Add("group_order", instance.GetIntCell(sheetIndex, rowIndex, "group order"));
				dictionary.Add("ignore_recipe_cap", instance.GetIntCell(sheetIndex, rowIndex, "ignore recipe cap") == 1);
				dictionary.Add("ignore_random_recipe_quest", instance.GetIntCell(sheetIndex, rowIndex, "ignore random recipe quest") == 1);
				dictionary.Add("type", "recipe");
				string stringCell = instance.GetStringCell(sSheetName, rowName, "name");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("name", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "tag");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("tag", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "craft  description");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("craft_description", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "start sound");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("start_sound", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "ready sound");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("ready_sound", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "product group");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("product_group", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "garden display started");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary2.Add("started", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "garden display half");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary2.Add("halfdone", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "garden display done");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary2.Add("done", stringCell);
				}
				if (dictionary2.Count > 0)
				{
					dictionary.Add("development_displaystates", dictionary2);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "touch sound");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("sound_on_touch", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "start sound beat");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("start_sound_beat", stringCell);
				}
				stringCell = instance.GetStringCell(sSheetName, rowName, "ready sound beat");
				if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
				{
					dictionary.Add("ready_sound_beat", stringCell);
				}
				bool flag = false;
				for (int j = 0; j < num3; j++)
				{
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "coral bits amount " + (j + 1).ToString());
					if (intCell2 != -1)
					{
						dictionary4.Add(intCell2.ToString(), instance.GetFloatCell(sheetIndex, rowIndex, "coral bits odds " + (j + 1).ToString()));
						flag = true;
					}
				}
				if (dictionary4.Count > 0)
				{
					dictionary6.Add("2001", dictionary4);
				}
				dictionary6.Add("5", instance.GetIntCell(sheetIndex, rowIndex, "xp reward"));
				dictionary6.Add(intCell.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "amount crafted"));
				if (!flag)
				{
					dictionary5.Add("resources", dictionary6);
				}
				else
				{
					list.Clear();
					dictionary8.Clear();
					dictionary7.Clear();
					dictionary7.Add("resources", dictionary6);
					dictionary8.Add("p", 1f);
					dictionary8.Add("value", dictionary7);
					list.Add(dictionary8);
					dictionary5.Add("cdf", list);
				}
				dictionary.Add("reward", dictionary5);
				for (int k = 0; k < num2; k++)
				{
					int intCell3 = instance.GetIntCell(sheetIndex, rowIndex, "ingredient id " + (k + 1).ToString());
					if (intCell3 != -1)
					{
						dictionary3.Add(intCell3.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "ingredient amount " + (k + 1).ToString()));
					}
				}
				dictionary.Add("ingredients", dictionary3);
				CraftingRecipe craftingRecipe = new CraftingRecipe(dictionary);
				int identity = craftingRecipe.identity;
				if (this.recipes.ContainsKey(identity))
				{
					TFUtils.ErrorLog(string.Concat(new object[]
					{
						"Recipe Collision!\nOld=",
						this.recipes[identity],
						"\nNew=",
						craftingRecipe
					}));
				}
				else
				{
					this.recipes.Add(identity, craftingRecipe);
				}
			}
		}
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x000478BC File Offset: 0x00045ABC
	private void LoadCookbooksFromSpreadseet(string sSheetName)
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
				dictionary.Clear();
				list.Clear();
				list2.Clear();
				int num3 = instance.GetIntCell(sSheetName, rowName, "id");
				dictionary.Add("id", num3);
				num3 = instance.GetRowIndex(sheetIndex, num3.ToString());
				dictionary.Add("type", "cookbook");
				list.Add(instance.GetIntCell(sheetIndex, num3, "background color r"));
				list.Add(instance.GetIntCell(sheetIndex, num3, "background color g"));
				list.Add(instance.GetIntCell(sheetIndex, num3, "background color b"));
				dictionary.Add("background.color", list);
				dictionary.Add("session_action_id", instance.GetStringCell(sSheetName, rowName, "session action id"));
				dictionary.Add("texture.cancelbutton", instance.GetStringCell(sSheetName, rowName, "cancel button texture"));
				dictionary.Add("texture.title", instance.GetStringCell(sSheetName, rowName, "title texture"));
				dictionary.Add("texture.slot", instance.GetStringCell(sSheetName, rowName, "slot texture"));
				dictionary.Add("texture.titleicon", instance.GetStringCell(sSheetName, rowName, "title icon texture"));
				dictionary.Add("button.label", instance.GetStringCell(sSheetName, rowName, "button label"));
				dictionary.Add("open_sound", instance.GetStringCell(sSheetName, rowName, "open sound"));
				dictionary.Add("close_sound", instance.GetStringCell(sSheetName, rowName, "close sound"));
				string text = instance.GetStringCell(sSheetName, rowName, "button icon");
				if (string.IsNullOrEmpty(text) || text == b)
				{
					text = null;
				}
				dictionary.Add("button.icon", text);
				text = instance.GetStringCell(sSheetName, rowName, "music");
				if (string.IsNullOrEmpty(text) || text == b)
				{
					text = null;
				}
				dictionary.Add("music", text);
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, num3, "recipe columns");
				}
				for (int j = 0; j < num2; j++)
				{
					int intCell = instance.GetIntCell(sheetIndex, num3, "Recipe " + (j + 1).ToString());
					if (intCell >= 0)
					{
						list2.Add(intCell);
					}
				}
				dictionary.Add("recipes", list2);
				CraftingCookbook craftingCookbook = new CraftingCookbook(dictionary);
				int identity = craftingCookbook.identity;
				if (this.cookbooks.ContainsKey(identity))
				{
					TFUtils.ErrorLog(string.Concat(new object[]
					{
						"Cookbook Collision!\nOld=",
						this.cookbooks[identity],
						"\nNew=",
						craftingCookbook
					}));
				}
				else
				{
					this.cookbooks.Add(craftingCookbook.identity, craftingCookbook);
				}
			}
		}
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x00047C48 File Offset: 0x00045E48
	private void LoadProductionSlotsFromSpreadseet(string sSheetName)
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
				dictionary.Clear();
				list.Clear();
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sSheetName, rowName, "id").ToString());
				dictionary.Add("type", "slot_costs");
				dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, "building did"));
				dictionary.Add("init_slots", instance.GetIntCell(sheetIndex, rowIndex, "starting slots"));
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "total slots");
				}
				for (int j = 0; j < num2; j++)
				{
					int intCell = instance.GetIntCell(sheetIndex, rowIndex, "currency type " + (j + 1).ToString());
					if (intCell >= 0)
					{
						list.Add(new Dictionary<string, object>(1)
						{
							{
								intCell.ToString(),
								instance.GetIntCell(sheetIndex, rowIndex, "slot cost " + (j + 1).ToString())
							}
						});
					}
				}
				dictionary.Add("costs", list);
				ProductionSlotTable productionSlotTable = new ProductionSlotTable(dictionary);
				if (this.prodSlotTables.ContainsKey(productionSlotTable.Did))
				{
					TFUtils.ErrorLog(string.Concat(new object[]
					{
						"Crafting Production Slot Table Collision!\nOld=",
						this.prodSlotTables[productionSlotTable.Did],
						"\nNew=",
						productionSlotTable
					}));
				}
				else
				{
					this.prodSlotTables.Add(productionSlotTable.Did, productionSlotTable);
				}
			}
		}
	}

	// Token: 0x040007D1 RID: 2001
	public const bool DEBUG_LOG_CRAFTING = false;

	// Token: 0x040007D2 RID: 2002
	private const string _sRECIPES = "Recipes";

	// Token: 0x040007D3 RID: 2003
	private const string _sCOOKBOOKS = "CraftBuildings";

	// Token: 0x040007D4 RID: 2004
	private const string _sPRODUCTION_SLOTS = "ProductionSlots";

	// Token: 0x040007D5 RID: 2005
	public const string CRAFTING_SLOT = "slot_id";

	// Token: 0x040007D6 RID: 2006
	public const int INVALID_SLOT = -1;

	// Token: 0x040007D7 RID: 2007
	public EventDispatcher UnlockedEvent = new EventDispatcher();

	// Token: 0x040007D8 RID: 2008
	private static readonly string CRAFTING_PATH = "Crafting";

	// Token: 0x040007D9 RID: 2009
	private Dictionary<int, CraftingCookbook> cookbooks;

	// Token: 0x040007DA RID: 2010
	private Dictionary<int, CraftingRecipe> recipes;

	// Token: 0x040007DB RID: 2011
	private Dictionary<int, ProductionSlotTable> prodSlotTables;

	// Token: 0x040007DC RID: 2012
	private HashSet<int> unlockedRecipes;

	// Token: 0x040007DD RID: 2013
	private HashSet<int> unlockedProductsShallow;

	// Token: 0x040007DE RID: 2014
	private HashSet<int> unlockedProductsDeep;

	// Token: 0x040007DF RID: 2015
	private HashSet<int> reservedRecipes;

	// Token: 0x040007E0 RID: 2016
	private HashSet<int> jellyBasedRecipes;

	// Token: 0x040007E1 RID: 2017
	private HashSet<int> ignoreRandomQuestRecipes;

	// Token: 0x040007E2 RID: 2018
	private Dictionary<Identity, Dictionary<int, CraftingInstance>> activeCrafts;
}
