using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class Reward
{
	// Token: 0x06000F3B RID: 3899 RVA: 0x00061510 File Offset: 0x0005F710
	public Reward(Dictionary<int, int> resourceAmounts, Dictionary<int, int> buildingAmounts, Dictionary<int, Vector2> buildingPositions, List<int> recipesAwarded, List<int> moviesAwarded, List<int> costumesAwarded, List<int> clearedLandsAwarded, List<int> buildingUnlocksAwarded, bool randomLand, string rewardThoughtIcon)
	{
		this.resourceAmounts = ((resourceAmounts != null) ? resourceAmounts : new Dictionary<int, int>());
		this.buildingAmounts = ((buildingAmounts != null) ? buildingAmounts : new Dictionary<int, int>());
		this.buildingPositions = ((buildingPositions != null) ? buildingPositions : new Dictionary<int, Vector2>());
		this.recipesAwarded = ((recipesAwarded != null) ? recipesAwarded : new List<int>());
		this.moviesAwarded = ((moviesAwarded != null) ? moviesAwarded : new List<int>());
		this.costumesAwarded = ((costumesAwarded != null) ? costumesAwarded : new List<int>());
		this.clearedLandsAwarded = ((clearedLandsAwarded != null) ? clearedLandsAwarded : new List<int>());
		this.buildingUnlocksAwarded = ((buildingUnlocksAwarded != null) ? buildingUnlocksAwarded : new List<int>());
		this.rewardThoughtIcon = rewardThoughtIcon;
		this.rewardRandomLand = randomLand;
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x000615F8 File Offset: 0x0005F7F8
	public Reward(Dictionary<int, int> resourceAmounts, Dictionary<int, int> buildingAmounts, Dictionary<int, Vector2> buildingPositions, List<int> recipesAwarded, List<int> moviesAwarded, List<int> costumesAwarded, List<int> clearedLandsAwarded, List<int> buildingUnlocksAwarded, bool randomLand, string rewardThoughtIcon, Dictionary<string, object> buildingLabels)
	{
		this.resourceAmounts = ((resourceAmounts != null) ? resourceAmounts : new Dictionary<int, int>());
		this.buildingAmounts = ((buildingAmounts != null) ? buildingAmounts : new Dictionary<int, int>());
		this.buildingPositions = ((buildingPositions != null) ? buildingPositions : new Dictionary<int, Vector2>());
		this.recipesAwarded = ((recipesAwarded != null) ? recipesAwarded : new List<int>());
		this.moviesAwarded = ((moviesAwarded != null) ? moviesAwarded : new List<int>());
		this.costumesAwarded = ((costumesAwarded != null) ? costumesAwarded : new List<int>());
		this.clearedLandsAwarded = ((clearedLandsAwarded != null) ? clearedLandsAwarded : new List<int>());
		this.buildingUnlocksAwarded = ((buildingUnlocksAwarded != null) ? buildingUnlocksAwarded : new List<int>());
		this.rewardThoughtIcon = rewardThoughtIcon;
		this.buildingLabels = buildingLabels;
		this.rewardRandomLand = randomLand;
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000F3D RID: 3901 RVA: 0x000616E8 File Offset: 0x0005F8E8
	public Dictionary<int, int> ResourceAmounts
	{
		get
		{
			return this.resourceAmounts;
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000F3E RID: 3902 RVA: 0x000616F0 File Offset: 0x0005F8F0
	public Dictionary<int, int> BuildingAmounts
	{
		get
		{
			return this.buildingAmounts;
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06000F3F RID: 3903 RVA: 0x000616F8 File Offset: 0x0005F8F8
	public Dictionary<int, Vector2> BuildingPositions
	{
		get
		{
			return this.buildingPositions;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06000F40 RID: 3904 RVA: 0x00061700 File Offset: 0x0005F900
	// (set) Token: 0x06000F41 RID: 3905 RVA: 0x00061708 File Offset: 0x0005F908
	public List<int> RecipeUnlocks
	{
		get
		{
			return this.recipesAwarded;
		}
		set
		{
			this.recipesAwarded = value;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06000F42 RID: 3906 RVA: 0x00061714 File Offset: 0x0005F914
	public List<int> MovieUnlocks
	{
		get
		{
			return this.moviesAwarded;
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0006171C File Offset: 0x0005F91C
	public List<int> CostumeUnlocks
	{
		get
		{
			return this.costumesAwarded;
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000F44 RID: 3908 RVA: 0x00061724 File Offset: 0x0005F924
	public List<int> ClearedLands
	{
		get
		{
			return this.clearedLandsAwarded;
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0006172C File Offset: 0x0005F92C
	public List<int> BuildingUnlocks
	{
		get
		{
			return this.buildingUnlocksAwarded;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000F46 RID: 3910 RVA: 0x00061734 File Offset: 0x0005F934
	public string ThoughtIcon
	{
		get
		{
			return this.rewardThoughtIcon;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0006173C File Offset: 0x0005F93C
	public bool RandomLand
	{
		get
		{
			return this.rewardRandomLand;
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000F48 RID: 3912 RVA: 0x00061744 File Offset: 0x0005F944
	public Dictionary<string, object> BuildingLabels
	{
		get
		{
			if (this.buildingLabels == null && this.buildingAmounts != null)
			{
				this.buildingLabels = new Dictionary<string, object>();
				foreach (KeyValuePair<int, int> keyValuePair in this.BuildingAmounts)
				{
					int key = keyValuePair.Key;
					List<object> list = new List<object>();
					for (int i = 0; i < keyValuePair.Value; i++)
					{
						Identity identity = new Identity();
						list.Add(identity.Describe());
					}
					this.buildingLabels[key.ToString()] = list;
				}
			}
			return this.buildingLabels;
		}
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0006181C File Offset: 0x0005FA1C
	public static Reward FromDict(Dictionary<string, object> dict)
	{
		if (dict == null)
		{
			return null;
		}
		Dictionary<int, int> dictionary = Reward.ParseAmountDictOrEmpty(dict, "resources");
		Dictionary<int, int> dictionary2 = Reward.ParseAmountDictOrEmpty(dict, "buildings");
		Dictionary<int, Vector2> dictionary3 = Reward.ParseAmountDictOrEmptyVector2(dict, "building_positions");
		List<int> list = Reward.ParseIntListOrEmpty(dict, "recipes");
		List<int> list2 = Reward.ParseIntListOrEmpty(dict, "movies");
		List<int> list3 = Reward.ParseIntListOrEmpty(dict, "costumes");
		List<int> list4 = Reward.ParseIntListOrEmpty(dict, "cleared_land");
		List<int> list5 = Reward.ParseIntListOrEmpty(dict, "building_unlocks");
		Dictionary<string, object> dictionary4 = dict.ContainsKey("building_labels") ? ((Dictionary<string, object>)dict["building_labels"]) : null;
		string text = TFUtils.TryLoadNullableString(dict, "thought_icon");
		bool? flag = TFUtils.TryLoadBool(dict, "random_land");
		return new Reward(dictionary, dictionary2, dictionary3, list, list2, list3, list4, list5, flag != null && flag.Value, text, dictionary4);
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x00061908 File Offset: 0x0005FB08
	public static Reward FromObject(object o)
	{
		return (o != null) ? Reward.FromDict((Dictionary<string, object>)o) : null;
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x00061924 File Offset: 0x0005FB24
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (this.resourceAmounts != null)
		{
			dictionary["resources"] = AmountDictionary.ToJSONDict(this.resourceAmounts);
		}
		if (this.buildingAmounts != null)
		{
			dictionary["buildings"] = AmountDictionary.ToJSONDict(this.buildingAmounts);
		}
		if (this.buildingPositions != null)
		{
			dictionary["building_positions"] = AmountDictionary.ToJSONDict(this.buildingPositions);
		}
		if (this.recipesAwarded != null)
		{
			dictionary["recipes"] = this.recipesAwarded.ConvertAll<object>((int x) => x.ToString());
		}
		if (this.moviesAwarded != null)
		{
			dictionary["movies"] = this.moviesAwarded.ConvertAll<object>((int x) => x.ToString());
		}
		if (this.costumesAwarded != null)
		{
			dictionary["costumes"] = this.costumesAwarded.ConvertAll<object>((int x) => x.ToString());
		}
		if (this.clearedLandsAwarded != null)
		{
			dictionary["cleared_land"] = this.clearedLandsAwarded.ConvertAll<object>((int x) => x.ToString());
		}
		if (this.buildingUnlocksAwarded != null)
		{
			dictionary["building_unlocks"] = this.buildingUnlocksAwarded.ConvertAll<object>((int x) => x.ToString());
		}
		if (this.BuildingLabels != null)
		{
			dictionary["building_labels"] = this.buildingLabels;
		}
		dictionary["thought_icon"] = this.rewardThoughtIcon;
		dictionary["random_land"] = this.rewardRandomLand;
		return dictionary;
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x00061B14 File Offset: 0x0005FD14
	public void AddDataToTrigger(ref Dictionary<string, object> data)
	{
		data["resource_amounts"] = AmountDictionary.ToJSONDict(this.resourceAmounts);
		data["recipes"] = this.recipesAwarded.ConvertAll<object>((int x) => x.ToString());
		data["buildings"] = this.moviesAwarded.ConvertAll<object>((int x) => x.ToString());
		data["costumes"] = this.costumesAwarded.ConvertAll<object>((int x) => x.ToString());
		data["building_unlocks"] = this.buildingUnlocksAwarded.ConvertAll<object>((int x) => x.ToString());
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x00061C08 File Offset: 0x0005FE08
	public static Dictionary<string, object> RewardToDict(Reward reward)
	{
		return (reward != null) ? reward.ToDict() : null;
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x00061C1C File Offset: 0x0005FE1C
	private static List<int> ParseIntListOrEmpty(Dictionary<string, object> dict, string key)
	{
		if (!dict.ContainsKey(key))
		{
			return new List<int>();
		}
		return ((List<object>)dict[key]).ConvertAll<int>((object x) => Convert.ToInt32(x));
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x00061C6C File Offset: 0x0005FE6C
	private static Dictionary<int, int> ParseAmountDictOrEmpty(Dictionary<string, object> dict, string key)
	{
		if (!dict.ContainsKey(key))
		{
			return new Dictionary<int, int>();
		}
		return AmountDictionary.FromJSONDict((Dictionary<string, object>)dict[key]);
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x00061C94 File Offset: 0x0005FE94
	private static Dictionary<int, Vector2> ParseAmountDictOrEmptyVector2(Dictionary<string, object> dict, string key)
	{
		if (!dict.ContainsKey(key))
		{
			return new Dictionary<int, Vector2>();
		}
		return AmountDictionary.FromJSONDictVector2((Dictionary<string, object>)dict[key]);
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00061CBC File Offset: 0x0005FEBC
	public static Reward operator +(Reward r1, Reward r2)
	{
		if (r1 == null)
		{
			return r2;
		}
		if (r2 == null)
		{
			return r1;
		}
		Reward reward = new Reward(null, null, null, null, null, null, null, null, false, null);
		reward.resourceAmounts = ((r1.resourceAmounts != null) ? TFUtils.CloneDictionary<int, int>(r1.resourceAmounts) : new Dictionary<int, int>());
		reward.buildingAmounts = ((r1.buildingAmounts != null) ? TFUtils.CloneDictionary<int, int>(r1.buildingAmounts) : new Dictionary<int, int>());
		reward.buildingPositions = ((r1.buildingPositions != null) ? TFUtils.CloneDictionary<int, Vector2>(r1.buildingPositions) : new Dictionary<int, Vector2>());
		reward.recipesAwarded = ((r1.recipesAwarded != null) ? r1.recipesAwarded : new List<int>());
		reward.moviesAwarded = ((r1.moviesAwarded != null) ? r1.moviesAwarded : new List<int>());
		reward.costumesAwarded = ((r1.costumesAwarded != null) ? r1.costumesAwarded : new List<int>());
		reward.clearedLandsAwarded = ((r1.clearedLandsAwarded != null) ? r1.clearedLandsAwarded : new List<int>());
		reward.buildingUnlocksAwarded = ((r1.buildingUnlocksAwarded != null) ? r1.buildingUnlocksAwarded : new List<int>());
		if (r2.resourceAmounts != null)
		{
			foreach (int num in r2.resourceAmounts.Keys)
			{
				if (reward.resourceAmounts.ContainsKey(num))
				{
					Dictionary<int, int> dictionary2;
					Dictionary<int, int> dictionary = dictionary2 = reward.resourceAmounts;
					int num2;
					int key = num2 = num;
					num2 = dictionary2[num2];
					dictionary[key] = num2 + r2.resourceAmounts[num];
				}
				else
				{
					reward.resourceAmounts[num] = r2.resourceAmounts[num];
				}
			}
		}
		if (r2.buildingAmounts != null)
		{
			foreach (int num3 in r2.buildingAmounts.Keys)
			{
				if (reward.buildingAmounts.ContainsKey(num3))
				{
					Dictionary<int, int> dictionary4;
					Dictionary<int, int> dictionary3 = dictionary4 = reward.buildingAmounts;
					int num2;
					int key2 = num2 = num3;
					num2 = dictionary4[num2];
					dictionary3[key2] = num2 + r2.buildingAmounts[num3];
				}
				else
				{
					reward.buildingAmounts[num3] = r2.buildingAmounts[num3];
				}
			}
		}
		if (r2.buildingPositions != null)
		{
			foreach (int key3 in r2.buildingPositions.Keys)
			{
				if (!reward.buildingPositions.ContainsKey(key3))
				{
					reward.buildingPositions[key3] = r2.buildingPositions[key3];
				}
			}
		}
		if (r2.recipesAwarded != null)
		{
			foreach (int item in r2.recipesAwarded)
			{
				if (!r1.recipesAwarded.Contains(item))
				{
					reward.recipesAwarded.Add(item);
				}
			}
		}
		if (r2.moviesAwarded != null)
		{
			foreach (int item2 in r2.moviesAwarded)
			{
				if (!r1.moviesAwarded.Contains(item2))
				{
					reward.moviesAwarded.Add(item2);
				}
			}
		}
		if (r2.costumesAwarded != null)
		{
			foreach (int item3 in r2.costumesAwarded)
			{
				if (!r1.costumesAwarded.Contains(item3))
				{
					reward.costumesAwarded.Add(item3);
				}
			}
		}
		if (r2.clearedLandsAwarded != null)
		{
			foreach (int item4 in r2.clearedLandsAwarded)
			{
				if (!r1.clearedLandsAwarded.Contains(item4))
				{
					reward.clearedLandsAwarded.Add(item4);
				}
			}
		}
		if (r2.buildingUnlocksAwarded != null)
		{
			foreach (int item5 in r2.buildingUnlocksAwarded)
			{
				if (!r1.buildingUnlocksAwarded.Contains(item5))
				{
					reward.buildingUnlocksAwarded.Add(item5);
				}
			}
		}
		if (r1.rewardThoughtIcon == r2.rewardThoughtIcon)
		{
			reward.rewardThoughtIcon = r1.rewardThoughtIcon;
		}
		return reward;
	}

	// Token: 0x04000A40 RID: 2624
	public const string THOUGHT_ICON = "thought_icon";

	// Token: 0x04000A41 RID: 2625
	public const string RECIPES = "recipes";

	// Token: 0x04000A42 RID: 2626
	public const string BUILDINGS = "buildings";

	// Token: 0x04000A43 RID: 2627
	public const string COSTUMES = "costumes";

	// Token: 0x04000A44 RID: 2628
	public const string RANDOM_LAND = "random_land";

	// Token: 0x04000A45 RID: 2629
	public const string BUILDING_UNLOCKS = "building_unlocks";

	// Token: 0x04000A46 RID: 2630
	public const string MOVIES = "movies";

	// Token: 0x04000A47 RID: 2631
	private Dictionary<int, int> resourceAmounts;

	// Token: 0x04000A48 RID: 2632
	private Dictionary<int, int> buildingAmounts;

	// Token: 0x04000A49 RID: 2633
	private Dictionary<int, Vector2> buildingPositions;

	// Token: 0x04000A4A RID: 2634
	private List<int> recipesAwarded;

	// Token: 0x04000A4B RID: 2635
	private List<int> moviesAwarded;

	// Token: 0x04000A4C RID: 2636
	private List<int> costumesAwarded;

	// Token: 0x04000A4D RID: 2637
	private List<int> clearedLandsAwarded;

	// Token: 0x04000A4E RID: 2638
	private List<int> buildingUnlocksAwarded;

	// Token: 0x04000A4F RID: 2639
	private Dictionary<string, object> buildingLabels;

	// Token: 0x04000A50 RID: 2640
	private string rewardThoughtIcon;

	// Token: 0x04000A51 RID: 2641
	private bool rewardRandomLand;
}
