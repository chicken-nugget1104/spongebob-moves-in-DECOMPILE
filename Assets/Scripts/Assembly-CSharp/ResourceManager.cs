using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarg;

// Token: 0x020001BA RID: 442
public class ResourceManager
{
	// Token: 0x06000EFC RID: 3836 RVA: 0x0005E4F4 File Offset: 0x0005C6F4
	public ResourceManager(Session session)
	{
		this.consumableResources = new HashSet<int>();
		this.resources = this.LoadResourceDefinitions();
		ResourceManager.internal_resources = this.resources;
		this.session = session;
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0005E5A8 File Offset: 0x0005C7A8
	public static string TypeDescription(int typeID)
	{
		if (typeID == ResourceManager.DEFAULT_WISH)
		{
			return "default_wish";
		}
		if (typeID == ResourceManager.HARD_CURRENCY)
		{
			return "hard_currency";
		}
		if (typeID == ResourceManager.SOFT_CURRENCY)
		{
			return "soft_currency";
		}
		if (typeID == ResourceManager.LEVEL)
		{
			return "level";
		}
		if (typeID == ResourceManager.XP)
		{
			return "xp";
		}
		if (typeID == ResourceManager.HALLOWEEN_CURRENCY)
		{
			return "halloween_currency";
		}
		if (typeID == ResourceManager.CHRISTMAS_CURRENCY)
		{
			return "christmas_currency";
		}
		if (typeID == ResourceManager.CHRISTMAS_CURRENCY_V2)
		{
			return "christmas_bottles_currency";
		}
		if (typeID == ResourceManager.VALENTINES_CURRENCY)
		{
			return "valentines_currency";
		}
		if (typeID == ResourceManager.SPONGY_GAMES_CURRENCY)
		{
			return "squilliams_currency";
		}
		if (typeID == ResourceManager.BONES_CURRENCY)
		{
			return "halloween_bones_currency";
		}
		if (typeID == ResourceManager.VALENTINES_2015_CURRENCY)
		{
			return "valentines_2015_currency";
		}
		TFUtils.Assert(false, "Invalid Resource for Type Description, please add to ResourceManager if needed");
		return "Invalid";
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0005E694 File Offset: 0x0005C894
	public Dictionary<int, Resource> Resources
	{
		get
		{
			return this.resources;
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000F00 RID: 3840 RVA: 0x0005E69C File Offset: 0x0005C89C
	public int PlayerLevelAmount
	{
		get
		{
			return this.resources[ResourceManager.LEVEL].Amount;
		}
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x0005E6B4 File Offset: 0x0005C8B4
	public static void ApplyCostToGameState(Cost cost, Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["resources"];
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			int key = TFUtils.LoadInt(dictionary, "did");
			int num = 0;
			if (cost.ResourceAmounts.TryGetValue(key, out num))
			{
				dictionary["amount_spent"] = TFUtils.LoadInt(dictionary, "amount_spent") + num;
			}
		}
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x0005E774 File Offset: 0x0005C974
	public static void ApplyCostToGameState(int resourceId, int amount, Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["resources"];
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			int num = TFUtils.LoadInt(dictionary, "did");
			if (num == resourceId)
			{
				dictionary["amount_spent"] = TFUtils.LoadInt(dictionary, "amount_spent") + amount;
				return;
			}
		}
		TFUtils.Assert(false, string.Format("Just tried to apply a spend on Resource Did {0} and do not have that defined in the GameState!", resourceId));
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x0005E840 File Offset: 0x0005CA40
	public static void AddAmountToGameState(int resourceId, int amount, Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["resources"];
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			int num = TFUtils.LoadInt(dictionary, "did");
			if (num == resourceId)
			{
				dictionary["amount_earned"] = TFUtils.LoadInt(dictionary, "amount_earned") + amount;
				return;
			}
		}
		if (ResourceManager.internal_resources != null && !ResourceManager.internal_resources.ContainsKey(resourceId))
		{
			string message = "Invalid Resource ID Added: " + resourceId;
			TFUtils.ErrorLog(message);
			TFUtils.LogDump(null, "invalid_resource", new Exception(message), null);
			return;
		}
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		dictionary2["did"] = resourceId;
		dictionary2["amount_earned"] = amount;
		dictionary2["amount_spent"] = 0;
		dictionary2["amount_purchased"] = 0;
		list.Add(dictionary2);
	}

	// Token: 0x06000F04 RID: 3844 RVA: 0x0005E994 File Offset: 0x0005CB94
	public static void ApplyPurchasesToGameState(Cost cost, Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["resources"];
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
			int key = TFUtils.LoadInt(dictionary, "did");
			if (cost.ResourceAmounts.ContainsKey(key))
			{
				dictionary["amount_purchased"] = cost.ResourceAmounts[key];
			}
		}
	}

	// Token: 0x06000F05 RID: 3845 RVA: 0x0005EA4C File Offset: 0x0005CC4C
	private string[] GetFilesToLoad()
	{
		return TFUtils.GetFilesInPath(ResourceManager.QUESTS_PATH, "resources.json");
	}

	// Token: 0x06000F06 RID: 3846 RVA: 0x0005EA60 File Offset: 0x0005CC60
	private Dictionary<int, Resource> LoadResourceDefinitions()
	{
		Dictionary<string, object> conversionDataFromSpread = this.GetConversionDataFromSpread();
		Dictionary<string, object> categoryOrderDataFromSpread = this.GetCategoryOrderDataFromSpread();
		Dictionary<string, object> d = TFUtils.LoadDict(conversionDataFromSpread, "time_factor");
		Dictionary<string, object> d2 = TFUtils.LoadDict(conversionDataFromSpread, "compression_base");
		ResourceManager.RESOURCE_TIME_FACTOR = TFUtils.LoadDouble(d, "resource");
		ResourceManager.RESOURCE_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "resource");
		ResourceManager.RENT_TIME_FACTOR = TFUtils.LoadDouble(d, "rent");
		ResourceManager.RENT_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "rent");
		ResourceManager.FULLNESS_TIME_FACTOR = TFUtils.LoadDouble(d, "fullness");
		ResourceManager.FULLNESS_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "fullness");
		ResourceManager.DEBRIS_TIME_FACTOR = TFUtils.LoadDouble(d, "debris");
		ResourceManager.DEBRIS_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "debris");
		ResourceManager.CONSTRUCTION_TIME_FACTOR = TFUtils.LoadDouble(d, "construction");
		ResourceManager.CONSTRUCTION_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "construction");
		ResourceManager.TASK_TIME_FACTOR = TFUtils.LoadDouble(d, "task");
		ResourceManager.TASK_COMPRESSION_BASE = TFUtils.LoadDouble(d2, "task");
		this.resourceCategoryOrder = TFUtils.LoadList<string>(categoryOrderDataFromSpread, "category_order");
		List<Dictionary<string, object>> list = TFUtils.LoadList<Dictionary<string, object>>(categoryOrderDataFromSpread, "category_to_productgroups");
		foreach (Dictionary<string, object> data in list)
		{
			ResourceCategory resourceCategory = ResourceCategory.FromDict(data);
			this.resourceCategories.Add(resourceCategory.name, resourceCategory);
		}
		List<object> resourceDictsFromSpread = this.GetResourceDictsFromSpread();
		Dictionary<int, Resource> dictionary = new Dictionary<int, Resource>();
		foreach (object obj in resourceDictsFromSpread)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
			int num = TFUtils.LoadInt(dictionary2, "did");
			string text = Language.Get(TFUtils.LoadString(dictionary2, "name"));
			string text2 = TFUtils.TryLoadString(dictionary2, "name_plural");
			if (text2 == null)
			{
				text2 = text;
			}
			else
			{
				text2 = Language.Get(text2);
			}
			string tag = TFUtils.TryLoadString(dictionary2, "tag");
			int maxAmount = 32767;
			if (dictionary2.ContainsKey("max_amount"))
			{
				maxAmount = TFUtils.LoadInt(dictionary2, "max_amount");
			}
			float width = -1f;
			float height = -1f;
			string text3 = null;
			if (dictionary2.ContainsKey("texture"))
			{
				text3 = TFUtils.LoadString(dictionary2, "texture");
				TFUtils.Assert(YGTextureLibrary.HasAtlasCoords(text3), "The texture atlas does not have an entry for " + text3);
				AtlasCoords atlasCoords = YGTextureLibrary.GetAtlasCoords(text3).atlasCoords;
				width = (float)TFAnimatedSprite.CalcWorldSize((double)atlasCoords.frame.width, 0.8);
				height = (float)TFAnimatedSprite.CalcWorldSize((double)atlasCoords.frame.height, 0.8);
			}
			string collectedSound = null;
			if (dictionary2.ContainsKey("collected_sound"))
			{
				collectedSound = TFUtils.LoadString(dictionary2, "collected_sound");
			}
			string tapSound = null;
			if (dictionary2.ContainsKey("tap_sound"))
			{
				tapSound = TFUtils.LoadString(dictionary2, "tap_sound");
			}
			string eatenSound = null;
			if (dictionary2.ContainsKey("eaten_sound"))
			{
				eatenSound = TFUtils.LoadString(dictionary2, "eaten_sound");
			}
			RewardDefinition reward = null;
			if (dictionary2.ContainsKey("reward"))
			{
				reward = RewardDefinition.FromObject(dictionary2["reward"]);
			}
			float jellyConversion = (!dictionary2.ContainsKey("jelly_conversion")) ? 1f : TFUtils.LoadFloat(dictionary2, "jelly_conversion");
			int fullnessTime = (!dictionary2.ContainsKey("fullness_time")) ? 0 : TFUtils.LoadInt(dictionary2, "fullness_time");
			bool forceTapToCollect = false;
			object obj2;
			if (dictionary2.TryGetValue("force_tap_to_collect", out obj2))
			{
				forceTapToCollect = (bool)obj2;
			}
			bool forceWishMatch = false;
			object obj3;
			if (dictionary2.TryGetValue("force_wish_match", out obj3))
			{
				forceWishMatch = (bool)obj3;
			}
			bool forceNoWishPayout = false;
			object obj4;
			if (dictionary2.TryGetValue("force_no_wish_payout", out obj4))
			{
				forceNoWishPayout = (bool)obj4;
			}
			bool ignoreWishDurationTimer = false;
			object obj5;
			if (dictionary2.TryGetValue("ignore_wish_duration_timer", out obj5))
			{
				ignoreWishDurationTimer = (bool)obj5;
			}
			bool flag = false;
			object obj6;
			if (dictionary2.TryGetValue("consumable", out obj6))
			{
				flag = (bool)obj6;
			}
			if (flag)
			{
				this.consumableResources.Add(num);
			}
			int currencyDisplayQuestTrigger = TFUtils.LoadInt(dictionary2, "currency_display_quest_trigger");
			if (dictionary2.ContainsKey("type"))
			{
				string text4 = TFUtils.LoadString(dictionary2, "type");
				string text5 = text4;
				switch (text5)
				{
				case "default_wish":
					ResourceManager.DEFAULT_WISH = num;
					goto IL_61F;
				case "soft_currency":
					ResourceManager.SOFT_CURRENCY = num;
					goto IL_61F;
				case "hard_currency":
					ResourceManager.HARD_CURRENCY = num;
					goto IL_61F;
				case "halloween_currency":
					ResourceManager.HALLOWEEN_CURRENCY = num;
					goto IL_61F;
				case "christmas_currency":
					ResourceManager.CHRISTMAS_CURRENCY = num;
					goto IL_61F;
				case "christmas_bottles_currency":
					ResourceManager.CHRISTMAS_CURRENCY_V2 = num;
					goto IL_61F;
				case "valentines_currency":
					ResourceManager.VALENTINES_CURRENCY = num;
					goto IL_61F;
				case "squilliams_currency":
					ResourceManager.SPONGY_GAMES_CURRENCY = num;
					goto IL_61F;
				case "halloween_bones_currency":
					ResourceManager.BONES_CURRENCY = num;
					goto IL_61F;
				case "level":
					ResourceManager.LEVEL = num;
					goto IL_61F;
				case "xp":
					ResourceManager.XP = num;
					goto IL_61F;
				case "default_jj":
					ResourceManager.DEFAULT_JJ = num;
					goto IL_61F;
				case "valentines_2015_currency":
					ResourceManager.VALENTINES_2015_CURRENCY = num;
					goto IL_61F;
				}
				TFUtils.Assert(false, "Unknown Resource 'type' found in Resource Definition JSON: " + text4);
			}
			IL_61F:
			dictionary[num] = new Resource(text, text2, tag, width, height, maxAmount, text3, collectedSound, tapSound, eatenSound, reward, jellyConversion, fullnessTime, forceTapToCollect, forceWishMatch, ignoreWishDurationTimer, forceNoWishPayout, num, currencyDisplayQuestTrigger, flag);
		}
		return dictionary;
	}

	// Token: 0x06000F07 RID: 3847 RVA: 0x0005F118 File Offset: 0x0005D318
	public void LoadResources(List<object> resources)
	{
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		try
		{
			foreach (object obj in resources)
			{
				Dictionary<string, object> d = (Dictionary<string, object>)obj;
				num = TFUtils.LoadInt(d, "did");
				num2 = TFUtils.LoadInt(d, "amount_earned");
				num3 = TFUtils.LoadInt(d, "amount_spent");
				this.resources[num].SetAmounts(num2, num3);
				int amountPurchased = TFUtils.LoadInt(d, "amount_purchased");
				this.resources[num].SetAmountPurchased(amountPurchased);
			}
		}
		catch (Exception ex)
		{
			ex.Data.Add("did", num);
			ex.Data.Add("amount_earned", num2);
			ex.Data.Add("amount_spent", num3);
			ex.Data.Add("error_code", 303);
			throw ex;
		}
	}

	// Token: 0x06000F08 RID: 3848 RVA: 0x0005F260 File Offset: 0x0005D460
	public void UpdateLevelExpToMilestone(LevelingManager manager)
	{
		int amount = this.resources[ResourceManager.XP].Amount;
		int amount2 = this.resources[ResourceManager.LEVEL].Amount;
		if (amount2 >= manager.MaxLevel)
		{
			return;
		}
		int xpRequiredForLevel = manager.GetXpRequiredForLevel(amount2);
		int xpRequiredForLevel2 = manager.GetXpRequiredForLevel(amount2 + 1);
		if (amount < xpRequiredForLevel)
		{
			this.resources[ResourceManager.XP].SetAmountEarned(xpRequiredForLevel);
		}
		else if (amount >= xpRequiredForLevel2)
		{
			this.resources[ResourceManager.XP].SetAmountEarned(xpRequiredForLevel2 - 1);
		}
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x0005F2FC File Offset: 0x0005D4FC
	public bool CanPay(Cost cost)
	{
		foreach (int num in cost.ResourceAmounts.Keys)
		{
			if (!this.HasEnough(num, cost.ResourceAmounts[num]))
			{
				TFUtils.DebugLog("Not enough " + this.resources[num].Name);
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x0005F3A4 File Offset: 0x0005D5A4
	public void Apply(Cost cost, Game game)
	{
		foreach (int num in cost.ResourceAmounts.Keys)
		{
			this.Spend(num, cost.ResourceAmounts[num], game);
		}
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x0005F41C File Offset: 0x0005D61C
	public void SellFor(Cost cost, Game game)
	{
		foreach (int num in cost.ResourceAmounts.Keys)
		{
			this.Add(num, cost.ResourceAmounts[num], game);
		}
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x0005F494 File Offset: 0x0005D694
	public bool HasEnough(int resourceId, int minimumAmount)
	{
		return this.resources.ContainsKey(resourceId) && this.resources[resourceId].Amount >= minimumAmount;
	}

	// Token: 0x06000F0D RID: 3853 RVA: 0x0005F4CC File Offset: 0x0005D6CC
	public int Query(int resourceId)
	{
		return this.resources[resourceId].Amount;
	}

	// Token: 0x06000F0E RID: 3854 RVA: 0x0005F4E0 File Offset: 0x0005D6E0
	public float QueryProgressPercentage(IResourceProgressCalculator calculator)
	{
		return calculator.ComputeProgressPercentage(this.resources);
	}

	// Token: 0x06000F0F RID: 3855 RVA: 0x0005F4F0 File Offset: 0x0005D6F0
	public string QueryProgressFraction(IResourceProgressCalculator calculator)
	{
		return calculator.ComputeProgressFraction(this.resources);
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x0005F500 File Offset: 0x0005D700
	public void Spend(Cost cost, Game game)
	{
		foreach (int num in cost.ResourceAmounts.Keys)
		{
			this.Spend(num, cost.ResourceAmounts[num], game);
		}
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x0005F578 File Offset: 0x0005D778
	public void Spend(int resourceId, int amount, Game game)
	{
		if (!this.HasEnough(resourceId, amount))
		{
			TFUtils.ErrorLog("Not enough of this resource(" + resourceId + ")");
			return;
		}
		bool flag = false;
		if (this.session.gameInitialized)
		{
			if (resourceId == ResourceManager.HARD_CURRENCY && this.resources[resourceId].Amount >= 20)
			{
				flag = true;
			}
			if (resourceId == ResourceManager.SOFT_CURRENCY && this.resources[resourceId].Amount >= 100)
			{
				flag = true;
			}
		}
		this.resources[resourceId].SubtractAmount(amount);
		if (flag)
		{
			if (resourceId == ResourceManager.HARD_CURRENCY && this.resources[resourceId].Amount < 20)
			{
				this.session.PlayHavenController.RequestContent("low_balance_jellyfish_jelly");
			}
			if (resourceId == ResourceManager.SOFT_CURRENCY && this.resources[resourceId].Amount < 100)
			{
				this.session.PlayHavenController.RequestContent("low_balance_coins");
			}
		}
		if (this.resources[resourceId].Tag != null)
		{
			game.analytics.LogResourceEconomySink(this.resources[resourceId].Tag, amount, this.resources[ResourceManager.LEVEL].Amount);
		}
		game.triggerRouter.RouteTrigger(this.CreateModifyResourceTrigger(resourceId, -amount), game);
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x0005F6F0 File Offset: 0x0005D8F0
	public void Add(int resourceId, int amount, Game game)
	{
		int amount2 = this.resources[ResourceManager.LEVEL].Amount;
		this.resources[resourceId].AddAmount(amount);
		if (this.resources[resourceId].Tag != null)
		{
			game.analytics.LogResourceEconomySource(this.resources[resourceId].Tag, amount, amount2, this.resources[ResourceManager.LEVEL].Amount, this);
		}
		if (amount2 != this.resources[ResourceManager.LEVEL].Amount)
		{
			List<Reward> levelUpRewards = game.levelingManager.GetLevelUpRewards(game.simulation, amount2, game.levelingManager.GetXpRequiredForLevel(this.resources[ResourceManager.LEVEL].Amount));
			AnalyticsWrapper.LogLevelUp(game, levelUpRewards, this.resources[ResourceManager.LEVEL].Amount);
			Catalog catalog = this.session.TheGame.catalog;
			Dictionary<int, SBMarketOffer> dictionary = new Dictionary<int, SBMarketOffer>();
			foreach (object obj in ((List<object>)catalog.CatalogDict["offers"]))
			{
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
				if (!dictionary2.ContainsKey("show_in_store") || (bool)dictionary2["show_in_store"])
				{
					SBMarketOffer sbmarketOffer = new SBMarketOffer((Dictionary<string, object>)obj);
					dictionary[sbmarketOffer.identity] = sbmarketOffer;
				}
			}
			foreach (object obj2 in ((List<object>)catalog.CatalogDict["market"]))
			{
				SBMarketCategory sbmarketCategory = new SBMarketCategory((Dictionary<string, object>)obj2);
				foreach (int key in sbmarketCategory.Dids)
				{
					SBMarketOffer sbmarketOffer;
					if (dictionary.TryGetValue(key, out sbmarketOffer))
					{
						if (sbmarketOffer.type == null)
						{
							sbmarketOffer.type = sbmarketCategory.Type;
						}
						Blueprint blueprint = EntityManager.GetBlueprint(sbmarketOffer.type, sbmarketOffer.identity, true);
						if (blueprint != null)
						{
							int num = (int)blueprint.Invariable["level.minimum"];
							if (num > amount2 && num <= this.resources[ResourceManager.LEVEL].Amount)
							{
								AnalyticsWrapper.LogFeatureUnlocked(this.session.TheGame, (string)blueprint.Invariable["name"], sbmarketCategory.DeltaDNAName);
							}
						}
					}
				}
			}
		}
		game.triggerRouter.RouteTrigger(this.CreateModifyResourceTrigger(resourceId, amount), game);
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x0005FA0C File Offset: 0x0005DC0C
	public Dictionary<string, object>[] ToDict()
	{
		Dictionary<string, object>[] array = new Dictionary<string, object>[this.resources.Count];
		int num = 0;
		foreach (int num2 in this.resources.Keys)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did"] = num2;
			dictionary["amount"] = this.resources[num2];
			array[num++] = dictionary;
		}
		return array;
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x0005FAC0 File Offset: 0x0005DCC0
	public override string ToString()
	{
		string str = "Resources:\n";
		string[] array = new string[this.resources.Count];
		int num = 0;
		foreach (int key in this.resources.Keys)
		{
			array[num++] = string.Concat(new object[]
			{
				"\t",
				this.resources[key].Name,
				": ",
				this.resources[key]
			});
		}
		return str + string.Join("\n", array);
	}

	// Token: 0x06000F15 RID: 3861 RVA: 0x0005FB98 File Offset: 0x0005DD98
	public List<int> ConsumableProducts(CraftingManager craftManager)
	{
		return craftManager.UnlockedProductsShallowCopy.Intersect(this.consumableResources).ToList<int>();
	}

	// Token: 0x06000F16 RID: 3862 RVA: 0x0005FBB0 File Offset: 0x0005DDB0
	public void PurchaseResourcesWithHardCurrency(int hcCost, Cost resources, Game game)
	{
		this.Spend(ResourceManager.HARD_CURRENCY, hcCost, game);
		foreach (KeyValuePair<int, int> keyValuePair in resources.ResourceAmounts)
		{
			this.Add(keyValuePair.Key, keyValuePair.Value, game);
		}
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x0005FC34 File Offset: 0x0005DE34
	public void SetPurchasedResources(Cost resources)
	{
		foreach (KeyValuePair<int, int> keyValuePair in resources.ResourceAmounts)
		{
			this.resources[keyValuePair.Key].SetAmountPurchased(keyValuePair.Value);
			TFUtils.DebugLog(string.Concat(new object[]
			{
				"SetPurchasedResources: ",
				keyValuePair.Key,
				" : ",
				keyValuePair.Value
			}));
		}
	}

	// Token: 0x06000F18 RID: 3864 RVA: 0x0005FCF0 File Offset: 0x0005DEF0
	public int GetResourcesPackageCostInHardCurrencyValue(Cost resourcesNeeded)
	{
		float num = 0f;
		foreach (KeyValuePair<int, int> keyValuePair in resourcesNeeded.ResourceAmounts)
		{
			TFUtils.Assert(this.resources[keyValuePair.Key].HardCurrencyConversion != 0f, "Should not have a zero value conversion!");
			TFUtils.Assert((float)keyValuePair.Value / this.resources[keyValuePair.Key].HardCurrencyConversion > 0f, "Hard Currency Conversion should NEVER result in giving the User HardCurrency!");
			num += (float)keyValuePair.Value / this.resources[keyValuePair.Key].HardCurrencyConversion;
		}
		return Mathf.CeilToInt(num);
	}

	// Token: 0x06000F19 RID: 3865 RVA: 0x0005FDDC File Offset: 0x0005DFDC
	public static Cost CalculateCraftRushCost(ulong recipeTime)
	{
		return ResourceManager.CalculateTimeToJjCost(recipeTime, ResourceManager.RESOURCE_TIME_FACTOR, ResourceManager.RESOURCE_COMPRESSION_BASE);
	}

	// Token: 0x06000F1A RID: 3866 RVA: 0x0005FDF0 File Offset: 0x0005DFF0
	public static Cost CalculateRentRushCost(ulong rentTime)
	{
		return ResourceManager.CalculateTimeToJjCost(rentTime, ResourceManager.RENT_TIME_FACTOR, ResourceManager.RENT_COMPRESSION_BASE);
	}

	// Token: 0x06000F1B RID: 3867 RVA: 0x0005FE04 File Offset: 0x0005E004
	public static Cost CalculateFullnessRushCost(ulong fullnessTime)
	{
		return ResourceManager.CalculateTimeToJjCost(fullnessTime, ResourceManager.FULLNESS_TIME_FACTOR, ResourceManager.FULLNESS_COMPRESSION_BASE);
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x0005FE18 File Offset: 0x0005E018
	public static Cost CalculateDebrisRushCost(ulong timeLeft)
	{
		return ResourceManager.CalculateTimeToJjCost(timeLeft, ResourceManager.DEBRIS_TIME_FACTOR, ResourceManager.DEBRIS_COMPRESSION_BASE);
	}

	// Token: 0x06000F1D RID: 3869 RVA: 0x0005FE2C File Offset: 0x0005E02C
	public static Cost CalculateConstructionRushCost(ulong timeLeft)
	{
		return ResourceManager.CalculateTimeToJjCost(timeLeft, ResourceManager.CONSTRUCTION_TIME_FACTOR, ResourceManager.CONSTRUCTION_COMPRESSION_BASE);
	}

	// Token: 0x06000F1E RID: 3870 RVA: 0x0005FE40 File Offset: 0x0005E040
	public static Cost CalculateTaskRushCost(ulong timeLeft)
	{
		return ResourceManager.CalculateTimeToJjCost(timeLeft, ResourceManager.TASK_TIME_FACTOR, ResourceManager.TASK_COMPRESSION_BASE);
	}

	// Token: 0x06000F1F RID: 3871 RVA: 0x0005FE54 File Offset: 0x0005E054
	private static Cost CalculateTimeToJjCost(ulong time, double timeToJjFactor, double timeCompressionBase)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		double originalCost = time * timeToJjFactor;
		int value = ResourceManager.CompressTimeCost(originalCost, timeCompressionBase);
		dictionary.Add(ResourceManager.HARD_CURRENCY, value);
		return new Cost(dictionary);
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x0005FE88 File Offset: 0x0005E088
	private static int CompressTimeCost(double originalCost, double compressionBase)
	{
		return (int)Math.Ceiling(Math.Log(originalCost + 1.0, compressionBase));
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x0005FEA4 File Offset: 0x0005E0A4
	public int GetNumDisplayableResources()
	{
		int num = 0;
		foreach (Resource resource in this.resources.Values)
		{
			if (resource.GetResourceTexture() != null)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0005FF1C File Offset: 0x0005E11C
	private ITrigger CreateModifyResourceTrigger(int resourceId, int amount)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary[resourceId.ToString()] = amount;
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		dictionary2["resource_amounts"] = dictionary;
		return new Trigger("UpdateResource", dictionary2);
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x0005FF60 File Offset: 0x0005E160
	public List<int> SortRecipesByProductGroup(CraftingManager craftManager, List<int> unsortedList)
	{
		List<int> list = new List<int>();
		foreach (string key in this.resourceCategoryOrder)
		{
			ResourceCategory resourceCategory = this.resourceCategories[key];
			foreach (ResourceProductGroup resourceProductGroup in resourceCategory.productGroups)
			{
				foreach (int id in resourceProductGroup.recipeDids)
				{
					CraftingRecipe recipeById = craftManager.GetRecipeById(id);
					if (unsortedList.Contains(recipeById.identity))
					{
						list.Add(recipeById.identity);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x000600A0 File Offset: 0x0005E2A0
	public void UpdateProductGroups(CraftingManager craftManager)
	{
		foreach (CraftingRecipe craftingRecipe in craftManager.Recipes.Values)
		{
			foreach (ResourceCategory resourceCategory in this.resourceCategories.Values)
			{
				ResourceProductGroup productGroupByName = resourceCategory.GetProductGroupByName(craftingRecipe.productGroup);
				if (productGroupByName != null)
				{
					productGroupByName.AddRecipe(craftManager, craftingRecipe);
					break;
				}
			}
		}
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0006017C File Offset: 0x0005E37C
	private Dictionary<string, object> GetConversionDataFromSpread()
	{
		string text = "Conversions";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return null;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return null;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return null;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>
		{
			{
				"time_factor",
				new Dictionary<string, object>()
			},
			{
				"compression_base",
				new Dictionary<string, object>()
			}
		};
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(text, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				string stringCell = instance.GetStringCell(text, rowName, "conversion type");
				((Dictionary<string, object>)dictionary["time_factor"]).Add(stringCell, instance.GetFloatCell(sheetIndex, rowIndex, "time factor"));
				((Dictionary<string, object>)dictionary["compression_base"]).Add(stringCell, instance.GetFloatCell(sheetIndex, rowIndex, "compression base"));
			}
		}
		return dictionary;
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x000602D8 File Offset: 0x0005E4D8
	private Dictionary<string, object> GetCategoryOrderDataFromSpread()
	{
		string text = "StoreOrder";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return null;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return null;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return null;
		}
		List<string> list = new List<string>();
		List<Dictionary<string, object>> list2 = new List<Dictionary<string, object>>();
		string b = "n/a";
		int num2 = -1;
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(text, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				if (num2 < 0)
				{
					num2 = instance.GetIntCell(sheetIndex, rowIndex, "num items");
				}
				string stringCell = instance.GetStringCell(text, rowName, "category");
				if (!list.Contains(stringCell))
				{
					list.Add(stringCell);
					List<string> list3 = new List<string>();
					for (int j = 1; j <= num2; j++)
					{
						string stringCell2 = instance.GetStringCell(text, rowName, "item " + j.ToString());
						if (stringCell2 == b)
						{
							break;
						}
						if (!list3.Contains(stringCell2))
						{
							list3.Add(stringCell2);
						}
					}
					list2.Add(new Dictionary<string, object>
					{
						{
							stringCell,
							list3
						}
					});
				}
			}
		}
		return new Dictionary<string, object>
		{
			{
				"category_order",
				list
			},
			{
				"category_to_productgroups",
				list2
			}
		};
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x00060498 File Offset: 0x0005E698
	private List<object> GetResourceDictsFromSpread()
	{
		string text = "Resources";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return null;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return null;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return null;
		}
		List<object> list = new List<object>();
		int columnIndexInSheet = instance.GetColumnIndexInSheet(sheetIndex, "id");
		int columnIndexInSheet2 = instance.GetColumnIndexInSheet(sheetIndex, "did");
		int columnIndexInSheet3 = instance.GetColumnIndexInSheet(sheetIndex, "name");
		int columnIndexInSheet4 = instance.GetColumnIndexInSheet(sheetIndex, "tag");
		int columnIndexInSheet5 = instance.GetColumnIndexInSheet(sheetIndex, "ignore wish duration");
		int columnIndexInSheet6 = instance.GetColumnIndexInSheet(sheetIndex, "force wish match");
		int columnIndexInSheet7 = instance.GetColumnIndexInSheet(sheetIndex, "force no wish payout");
		int columnIndexInSheet8 = instance.GetColumnIndexInSheet(sheetIndex, "consumable");
		int columnIndexInSheet9 = instance.GetColumnIndexInSheet(sheetIndex, "expected order");
		int columnIndexInSheet10 = instance.GetColumnIndexInSheet(sheetIndex, "fullness time");
		int columnIndexInSheet11 = instance.GetColumnIndexInSheet(sheetIndex, "max amount");
		int columnIndexInSheet12 = instance.GetColumnIndexInSheet(sheetIndex, "jelly conversion");
		int columnIndexInSheet13 = instance.GetColumnIndexInSheet(sheetIndex, "texture");
		int columnIndexInSheet14 = instance.GetColumnIndexInSheet(sheetIndex, "collected sound");
		int columnIndexInSheet15 = instance.GetColumnIndexInSheet(sheetIndex, "eaten sound");
		int columnIndexInSheet16 = instance.GetColumnIndexInSheet(sheetIndex, "type");
		int columnIndexInSheet17 = instance.GetColumnIndexInSheet(sheetIndex, "tap sound");
		int columnIndexInSheet18 = instance.GetColumnIndexInSheet(sheetIndex, "reward gold");
		int columnIndexInSheet19 = instance.GetColumnIndexInSheet(sheetIndex, "reward xp");
		int columnIndexInSheet20 = instance.GetColumnIndexInSheet(sheetIndex, "special currency hud display");
		int columnIndexInSheet21 = instance.GetColumnIndexInSheet(sheetIndex, "currency display quest trigger");
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
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("did", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet2));
				dictionary.Add("currency_display_quest_trigger", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet21));
				dictionary.Add("name", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet3));
				dictionary.Add("tag", instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet4));
				dictionary.Add("ignore_wish_duration_timer", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet5) == 1);
				dictionary.Add("force_wish_match", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet6) == 1);
				dictionary.Add("force_no_wish_payout", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet7) == 1);
				dictionary.Add("consumable", instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet8) == 1);
				if (ResourceManager.SPECIAL_CURRENCY < 0 && instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet20) == 1)
				{
					ResourceManager.SPECIAL_CURRENCY = (int)dictionary["did"];
				}
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet9);
				if (intCell >= 0)
				{
					dictionary.Add("_expected_order", intCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet10);
				if (intCell >= 0)
				{
					dictionary.Add("fullness_time", intCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet11);
				if (intCell >= 0)
				{
					dictionary.Add("max_amount", intCell);
				}
				float floatCell = instance.GetFloatCell(sheetIndex, rowIndex, columnIndexInSheet12);
				if (floatCell >= 0f)
				{
					dictionary.Add("jelly_conversion", floatCell);
				}
				string stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet13);
				if (stringCell != b)
				{
					dictionary.Add("texture", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet14);
				if (stringCell != b)
				{
					dictionary.Add("collected_sound", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet15);
				if (stringCell != b)
				{
					dictionary.Add("eaten_sound", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet16);
				if (stringCell != b)
				{
					dictionary.Add("type", stringCell);
				}
				stringCell = instance.GetStringCell(sheetIndex, rowIndex, columnIndexInSheet17);
				if (stringCell != b)
				{
					dictionary.Add("tap_sound", stringCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet18);
				if (intCell > 0)
				{
					if (!dictionary.ContainsKey("reward"))
					{
						dictionary.Add("reward", new Dictionary<string, object>
						{
							{
								"resources",
								new Dictionary<string, object>()
							}
						});
					}
					((Dictionary<string, object>)((Dictionary<string, object>)dictionary["reward"])["resources"]).Add("3", intCell);
				}
				intCell = instance.GetIntCell(sheetIndex, rowIndex, columnIndexInSheet19);
				if (intCell > 0)
				{
					if (!dictionary.ContainsKey("reward"))
					{
						dictionary.Add("reward", new Dictionary<string, object>
						{
							{
								"resources",
								new Dictionary<string, object>()
							}
						});
					}
					((Dictionary<string, object>)((Dictionary<string, object>)dictionary["reward"])["resources"]).Add("5", intCell);
				}
				list.Add(dictionary);
			}
		}
		return list;
	}

	// Token: 0x04000A00 RID: 2560
	private const int MAX_AMOUNT = 32767;

	// Token: 0x04000A01 RID: 2561
	public const string TYPE_HARD_CURRENCY = "hard_currency";

	// Token: 0x04000A02 RID: 2562
	public const string TYPE_SOFT_CURRENCY = "soft_currency";

	// Token: 0x04000A03 RID: 2563
	public const string TYPE_HALLOWEEN_CURRENCY = "halloween_currency";

	// Token: 0x04000A04 RID: 2564
	public const string TYPE_CHRISTMAS_CURRENCY = "christmas_currency";

	// Token: 0x04000A05 RID: 2565
	public const string TYPE_VALENTINES_CURRENCY = "valentines_currency";

	// Token: 0x04000A06 RID: 2566
	public const string TYPE_SPONGY_GAMES_CURRENCY = "squilliams_currency";

	// Token: 0x04000A07 RID: 2567
	public const string TYPE_BONES_CURRENCY = "halloween_bones_currency";

	// Token: 0x04000A08 RID: 2568
	public const string TYPE_CHRISTMAS_CURRENCY_V2 = "christmas_bottles_currency";

	// Token: 0x04000A09 RID: 2569
	public const string TYPE_LEVEL = "level";

	// Token: 0x04000A0A RID: 2570
	public const string TYPE_XP = "xp";

	// Token: 0x04000A0B RID: 2571
	public const string TYPE_DEFAULT_WISH = "default_wish";

	// Token: 0x04000A0C RID: 2572
	public const string TYPE_DEFAULT_JJ = "default_jj";

	// Token: 0x04000A0D RID: 2573
	public const string TYPE_VALENTINES_2015_CURRENCY = "valentines_2015_currency";

	// Token: 0x04000A0E RID: 2574
	public const string EMPTY_WISH_TEXTURE = "empty.png";

	// Token: 0x04000A0F RID: 2575
	public const string UPDATE_RESOURCE = "UpdateResource";

	// Token: 0x04000A10 RID: 2576
	public const int RESOURCE_TYPE_HOLIDAY_CHEER = 9100;

	// Token: 0x04000A11 RID: 2577
	private static readonly string QUESTS_PATH = "Resources";

	// Token: 0x04000A12 RID: 2578
	public static int DEFAULT_WISH = -1;

	// Token: 0x04000A13 RID: 2579
	public static int HARD_CURRENCY = 2;

	// Token: 0x04000A14 RID: 2580
	public static int SOFT_CURRENCY = 3;

	// Token: 0x04000A15 RID: 2581
	public static int HALLOWEEN_CURRENCY = -1;

	// Token: 0x04000A16 RID: 2582
	public static int CHRISTMAS_CURRENCY = -1;

	// Token: 0x04000A17 RID: 2583
	public static int CHRISTMAS_CURRENCY_V2 = -1;

	// Token: 0x04000A18 RID: 2584
	public static int VALENTINES_CURRENCY = -1;

	// Token: 0x04000A19 RID: 2585
	public static int SPONGY_GAMES_CURRENCY = -1;

	// Token: 0x04000A1A RID: 2586
	public static int BONES_CURRENCY = -1;

	// Token: 0x04000A1B RID: 2587
	public static int SPECIAL_CURRENCY = -1;

	// Token: 0x04000A1C RID: 2588
	public static int LEVEL = 4;

	// Token: 0x04000A1D RID: 2589
	public static int XP = 5;

	// Token: 0x04000A1E RID: 2590
	public static int DEFAULT_JJ = 33;

	// Token: 0x04000A1F RID: 2591
	public static int VALENTINES_2015_CURRENCY = -1;

	// Token: 0x04000A20 RID: 2592
	private Dictionary<int, Resource> resources;

	// Token: 0x04000A21 RID: 2593
	private static Dictionary<int, Resource> internal_resources;

	// Token: 0x04000A22 RID: 2594
	private HashSet<int> consumableResources;

	// Token: 0x04000A23 RID: 2595
	public List<string> resourceCategoryOrder;

	// Token: 0x04000A24 RID: 2596
	private Session session;

	// Token: 0x04000A25 RID: 2597
	public Dictionary<string, ResourceCategory> resourceCategories = new Dictionary<string, ResourceCategory>();

	// Token: 0x04000A26 RID: 2598
	private static double RESOURCE_TIME_FACTOR;

	// Token: 0x04000A27 RID: 2599
	private static double RESOURCE_COMPRESSION_BASE;

	// Token: 0x04000A28 RID: 2600
	private static double RENT_TIME_FACTOR;

	// Token: 0x04000A29 RID: 2601
	private static double RENT_COMPRESSION_BASE;

	// Token: 0x04000A2A RID: 2602
	private static double FULLNESS_TIME_FACTOR;

	// Token: 0x04000A2B RID: 2603
	private static double FULLNESS_COMPRESSION_BASE;

	// Token: 0x04000A2C RID: 2604
	private static double DEBRIS_TIME_FACTOR;

	// Token: 0x04000A2D RID: 2605
	private static double DEBRIS_COMPRESSION_BASE;

	// Token: 0x04000A2E RID: 2606
	private static double CONSTRUCTION_TIME_FACTOR;

	// Token: 0x04000A2F RID: 2607
	private static double CONSTRUCTION_COMPRESSION_BASE;

	// Token: 0x04000A30 RID: 2608
	private static double TASK_TIME_FACTOR;

	// Token: 0x04000A31 RID: 2609
	private static double TASK_COMPRESSION_BASE;
}
