using System;
using System.Collections.Generic;

// Token: 0x02000159 RID: 345
public class CraftingRecipe
{
	// Token: 0x06000BE5 RID: 3045 RVA: 0x00047E88 File Offset: 0x00046088
	public CraftingRecipe(Dictionary<string, object> data)
	{
		this.recipeName = TFUtils.LoadString(data, "name");
		this.craftDescription = TFUtils.LoadString(data, "craft_description");
		this.identity = TFUtils.LoadInt(data, "did");
		this.productId = TFUtils.LoadInt(data, "product_id");
		this.buildingId = TFUtils.LoadInt(data, "building_id");
		this.recipeTag = TFUtils.TryLoadString(data, "tag");
		if (this.recipeTag == null)
		{
			TFUtils.WarningLog("No recipe tag! Using default.");
			this.recipeTag = string.Format("recipe_{0}", this.identity);
		}
		Dictionary<string, object> dict = (Dictionary<string, object>)data["reward"];
		this.rewardDefinition = RewardDefinition.FromDict(dict);
		Dictionary<string, object> dict2 = (Dictionary<string, object>)data["ingredients"];
		this.cost = Cost.FromDict(dict2);
		this.minimumLevel = TFUtils.LoadInt(data, "minimum_level");
		this.craftTime = TFUtils.LoadUlong(data, "craft.time", 0UL);
		this.displayedCraftTime = TFUtils.DurationToString(this.craftTime);
		this.productGroup = TFUtils.LoadString(data, "product_group");
		this.groupOrder = TFUtils.LoadInt(data, "group_order");
		this.rushCost = ResourceManager.CalculateCraftRushCost(this.craftTime);
		this.startSoundImmediate = TFUtils.LoadString(data, "start_sound");
		this.startSoundBeat = TFUtils.TryLoadString(data, "start_sound_beat");
		this.readySoundImmediate = TFUtils.LoadString(data, "ready_sound");
		this.readySoundBeat = TFUtils.TryLoadString(data, "ready_sound_beat");
		this.recipeSubType = "!!RECIPE";
		if (data.ContainsKey("subtype"))
		{
			this.recipeSubType = TFUtils.LoadString(data, "subtype");
		}
		if (data.ContainsKey("display"))
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)data["display"];
			this.width = Convert.ToInt32(dictionary["width"]);
			this.height = Convert.ToInt32(dictionary["height"]);
		}
		this.developmentDisplayStates = new Dictionary<string, string>();
		if (data.ContainsKey("development_displaystates"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)data["development_displaystates"];
			foreach (string key in dictionary2.Keys)
			{
				this.developmentDisplayStates.Add(key, (string)dictionary2[key]);
			}
		}
		bool? flag = TFUtils.TryLoadBool(data, "ignore_recipe_cap");
		if (flag != null)
		{
			this.ignoreRecipeCap = flag.Value;
		}
		else
		{
			this.ignoreRecipeCap = false;
		}
		flag = TFUtils.TryLoadBool(data, "ignore_random_recipe_quest");
		if (flag != null)
		{
			this.ignoreRandomRecipeQuest = flag.Value;
		}
		else
		{
			this.ignoreRandomRecipeQuest = false;
		}
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x0004819C File Offset: 0x0004639C
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"[CraftingRecipe (productId=",
			this.productId,
			", receipName=",
			this.recipeName,
			")]"
		});
	}

	// Token: 0x040007E3 RID: 2019
	public const string TYPE = "recipe";

	// Token: 0x040007E4 RID: 2020
	public int identity;

	// Token: 0x040007E5 RID: 2021
	public int productId;

	// Token: 0x040007E6 RID: 2022
	public string craftDescription;

	// Token: 0x040007E7 RID: 2023
	public string recipeName;

	// Token: 0x040007E8 RID: 2024
	public string recipeTag;

	// Token: 0x040007E9 RID: 2025
	public string recipeSubType;

	// Token: 0x040007EA RID: 2026
	public RewardDefinition rewardDefinition;

	// Token: 0x040007EB RID: 2027
	public Cost cost;

	// Token: 0x040007EC RID: 2028
	public Cost rushCost;

	// Token: 0x040007ED RID: 2029
	public ulong craftTime;

	// Token: 0x040007EE RID: 2030
	public int minimumLevel;

	// Token: 0x040007EF RID: 2031
	public string displayedCraftTime;

	// Token: 0x040007F0 RID: 2032
	public string startSoundImmediate;

	// Token: 0x040007F1 RID: 2033
	public string startSoundBeat;

	// Token: 0x040007F2 RID: 2034
	public string readySoundImmediate;

	// Token: 0x040007F3 RID: 2035
	public string readySoundBeat;

	// Token: 0x040007F4 RID: 2036
	public float beatLength = 1f;

	// Token: 0x040007F5 RID: 2037
	public int buildingId;

	// Token: 0x040007F6 RID: 2038
	public int height;

	// Token: 0x040007F7 RID: 2039
	public int width;

	// Token: 0x040007F8 RID: 2040
	public string productGroup;

	// Token: 0x040007F9 RID: 2041
	public int groupOrder;

	// Token: 0x040007FA RID: 2042
	public bool ignoreRandomRecipeQuest;

	// Token: 0x040007FB RID: 2043
	public bool ignoreRecipeCap;

	// Token: 0x040007FC RID: 2044
	public Dictionary<string, string> developmentDisplayStates;
}
