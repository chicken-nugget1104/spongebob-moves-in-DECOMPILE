using System;
using System.Collections.Generic;

// Token: 0x020001C0 RID: 448
public class RewardDefinition
{
	// Token: 0x06000F62 RID: 3938 RVA: 0x00062550 File Offset: 0x00060750
	private RewardDefinition(CdfDictionary<RewardDefinition.GeneratorBucket> buckets, Reward summary)
	{
		this.generatorBuckets = buckets;
		this.summary = summary;
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00062568 File Offset: 0x00060768
	public static RewardDefinition FromDict(Dictionary<string, object> dict)
	{
		if (dict == null)
		{
			return null;
		}
		Reward reward = null;
		if (dict.ContainsKey("summary"))
		{
			reward = Reward.FromObject(TFUtils.LoadDict(dict, "summary"));
		}
		CdfDictionary<RewardDefinition.GeneratorBucket> cdfDictionary;
		if (!dict.ContainsKey("cdf"))
		{
			cdfDictionary = new CdfDictionary<RewardDefinition.GeneratorBucket>();
			RewardDefinition.GeneratorBucket generatorBucket = RewardDefinition.FromDictInnerHelper(dict);
			cdfDictionary.Add(generatorBucket, 1.0);
			if (reward == null)
			{
				reward = generatorBucket.summary;
			}
		}
		else
		{
			cdfDictionary = CdfDictionary<RewardDefinition.GeneratorBucket>.FromList(TFUtils.LoadList<object>(dict, "cdf"), new CdfDictionary<RewardDefinition.GeneratorBucket>.ParseT(RewardDefinition.FromDictInnerHelper));
		}
		return new RewardDefinition(cdfDictionary, reward);
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00062604 File Offset: 0x00060804
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
		foreach (KeyValuePair<int, int> keyValuePair in this.summary.ResourceAmounts)
		{
			dictionary2[keyValuePair.Key.ToString()] = keyValuePair.Value;
		}
		dictionary["resources"] = dictionary2;
		return dictionary;
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x000626A4 File Offset: 0x000608A4
	private static RewardDefinition.GeneratorBucket FromDictInnerHelper(object obj)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
		Dictionary<int, ResultGenerator> dictionary2 = RewardDefinition.ParseOrNull(dictionary, "resources");
		Dictionary<int, ResultGenerator> buildingsGenerator = RewardDefinition.ParseOrNull(dictionary, "buildings");
		Dictionary<int, ResultGenerator> recipesGenerator = RewardDefinition.ParseOrNull(dictionary, "recipes");
		Dictionary<int, ResultGenerator> moviesGenerator = RewardDefinition.ParseOrNull(dictionary, "movies");
		Dictionary<int, ResultGenerator> costumesGenerator = RewardDefinition.ParseOrNull(dictionary, "costumes");
		Reward reward = null;
		if (dictionary.ContainsKey("summary"))
		{
			reward = Reward.FromDict((Dictionary<string, object>)dictionary["summary"]);
		}
		else if (dictionary2 != null)
		{
			Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
			foreach (int key in dictionary2.Keys)
			{
				dictionary3[key] = (int)float.Parse(dictionary2[key].GetExpectedValue());
			}
			string rewardThoughtIcon = TFUtils.TryLoadNullableString(dictionary, "thought_icon");
			bool? flag = TFUtils.TryLoadBool(dictionary, "random_land");
			reward = new Reward(dictionary3, null, null, null, null, null, null, null, flag != null && flag.Value, rewardThoughtIcon);
		}
		return new RewardDefinition.GeneratorBucket(dictionary2, buildingsGenerator, recipesGenerator, moviesGenerator, costumesGenerator, reward);
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x000627FC File Offset: 0x000609FC
	public static RewardDefinition FromObject(object o)
	{
		return (o != null) ? RewardDefinition.FromDict((Dictionary<string, object>)o) : null;
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000F67 RID: 3943 RVA: 0x00062818 File Offset: 0x00060A18
	public Reward Summary
	{
		get
		{
			return this.summary;
		}
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x00062820 File Offset: 0x00060A20
	public int LowestResourceValue(int nKey)
	{
		List<RewardDefinition.GeneratorBucket> valuesClone = this.generatorBuckets.ValuesClone;
		int count = valuesClone.Count;
		int num = -1;
		for (int i = 0; i < count; i++)
		{
			RewardDefinition.GeneratorBucket generatorBucket = valuesClone[i];
			if (generatorBucket != null && generatorBucket.resourcesGenerator != null && generatorBucket.resourcesGenerator.ContainsKey(nKey))
			{
				int num2 = (int)float.Parse(generatorBucket.resourcesGenerator[nKey].GetExpectedValue());
				if (num == -1 || num2 < num)
				{
					num = num2;
				}
			}
		}
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x000628C0 File Offset: 0x00060AC0
	public Reward GenerateReward(Simulation simulation, bool forceReward)
	{
		return this.GenerateReward(simulation, false, forceReward);
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x000628CC File Offset: 0x00060ACC
	public Reward GenerateReward(Simulation simulation, bool inferThoughtIconIfNull, bool forceReward)
	{
		return this.GenerateReward(simulation, new Reward(new Dictionary<int, int>(), null, null, null, null, null, null, null, this.summary != null && this.summary.RandomLand, null), inferThoughtIconIfNull, forceReward);
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x00062910 File Offset: 0x00060B10
	public Reward GenerateReward(Simulation simulation, Reward consolationReward, bool inferThoughtIconIfNull, bool forceReward)
	{
		RewardDefinition.GeneratorBucket generatorBucket = this.generatorBuckets.Spin();
		Dictionary<int, int> dictionary = null;
		Dictionary<int, int> dictionary2 = null;
		Dictionary<int, int> dictionary3 = null;
		Dictionary<int, int> dictionary4 = null;
		if (generatorBucket == null)
		{
			return null;
		}
		dictionary = ProbabilityDictionary.GenerateAmounts(generatorBucket.resourcesGenerator);
		dictionary2 = ProbabilityDictionary.GenerateAmounts(generatorBucket.buildingsGenerator);
		Dictionary<int, int> dictionary5;
		if (generatorBucket.recipesGenerator != null)
		{
			dictionary5 = ProbabilityDictionary.GenerateAmounts(generatorBucket.recipesGenerator);
		}
		else
		{
			dictionary5 = null;
		}
		dictionary3 = ProbabilityDictionary.GenerateAmounts(generatorBucket.moviesGenerator);
		dictionary4 = ProbabilityDictionary.GenerateAmounts(generatorBucket.costumesGenerator);
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		if (dictionary5 != null)
		{
			foreach (int num in dictionary5.Keys)
			{
				if (!simulation.craftManager.Recipes.ContainsKey(num))
				{
					TFUtils.WarningLog("missing recipe: " + num + "! make sure this is a recipe and NOT a resource id");
				}
				else if (dictionary5[num] > 0 && !simulation.craftManager.UnlockedRecipesCopy.Contains(num) && !simulation.craftManager.ReservedRecipesCopy.Contains(num) && simulation.craftManager.GetRecipeById(num).minimumLevel <= simulation.resourceManager.Query(ResourceManager.LEVEL))
				{
					if (forceReward)
					{
						list.Add(num);
					}
					else if (simulation.craftManager.CanMakeRecipe(num))
					{
						list.Add(num);
					}
				}
			}
		}
		if (dictionary3 != null)
		{
			foreach (int num2 in dictionary3.Keys)
			{
				if (dictionary3[num2] > 0 && !simulation.movieManager.UnlockedMovies.Contains(num2))
				{
					list2.Add(num2);
				}
			}
		}
		if (dictionary4 != null)
		{
			foreach (int num3 in dictionary4.Keys)
			{
				if (dictionary4[num3] > 0 && !simulation.game.costumeManager.IsCostumeUnlocked(num3))
				{
					list3.Add(num3);
				}
			}
		}
		string text = (this.summary != null) ? this.summary.ThoughtIcon : null;
		if (inferThoughtIconIfNull && text == null)
		{
			text = this.InferThoughtIcon(dictionary, simulation.resourceManager);
		}
		Reward reward;
		if ((dictionary == null || dictionary.Count == 0) && (dictionary2 == null || dictionary2.Count == 0) && list.Count == 0 && list2.Count == 0 && list3.Count == 0)
		{
			reward = consolationReward;
		}
		else
		{
			reward = new Reward(dictionary, dictionary2, null, list, list2, list3, null, null, this.summary != null && this.summary.RandomLand, text);
		}
		if (!forceReward && simulation.rewardCap.Filter(simulation, ref reward))
		{
			if (reward.ResourceAmounts.ContainsKey(ResourceManager.SOFT_CURRENCY))
			{
				Dictionary<int, int> resourceAmounts;
				Dictionary<int, int> dictionary6 = resourceAmounts = reward.ResourceAmounts;
				int num4;
				int key = num4 = ResourceManager.SOFT_CURRENCY;
				num4 = resourceAmounts[num4];
				dictionary6[key] = num4 + 25;
			}
			else
			{
				reward.ResourceAmounts[ResourceManager.SOFT_CURRENCY] = 25;
			}
		}
		return reward;
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x00062D00 File Offset: 0x00060F00
	public RewardDefinition Join(RewardDefinition that)
	{
		return new RewardDefinition(this.generatorBuckets.Join(that.generatorBuckets), null);
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x00062D1C File Offset: 0x00060F1C
	public void Normalize()
	{
		this.generatorBuckets.Normalize();
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x00062D2C File Offset: 0x00060F2C
	public void Validate(bool ensureFullRange)
	{
		this.generatorBuckets.Validate(ensureFullRange, string.Empty);
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x00062D40 File Offset: 0x00060F40
	private static Dictionary<int, ResultGenerator> ParseOrNull(Dictionary<string, object> dict, string key)
	{
		if (!dict.ContainsKey(key))
		{
			return null;
		}
		return ProbabilityDictionary.FromJSONDict((Dictionary<string, object>)dict[key]);
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x00062D64 File Offset: 0x00060F64
	private string InferThoughtIcon(Dictionary<int, int> resourceAmounts, ResourceManager resourceMgr)
	{
		foreach (int num in resourceAmounts.Keys)
		{
			if (num != ResourceManager.SOFT_CURRENCY && num != ResourceManager.HARD_CURRENCY && num != ResourceManager.XP && num != ResourceManager.LEVEL)
			{
				return resourceMgr.Resources[num].GetResourceTexture();
			}
		}
		string result = null;
		if (this.IdToStringHelper(ResourceManager.HARD_CURRENCY, ref result, resourceAmounts, resourceMgr) || this.IdToStringHelper(ResourceManager.SOFT_CURRENCY, ref result, resourceAmounts, resourceMgr) || !this.IdToStringHelper(ResourceManager.XP, ref result, resourceAmounts, resourceMgr))
		{
		}
		return result;
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x00062E48 File Offset: 0x00061048
	private bool IdToStringHelper(int productId, ref string rv, Dictionary<int, int> resourceAmounts, ResourceManager resourceMgr)
	{
		if (resourceAmounts.ContainsKey(productId))
		{
			rv = resourceMgr.Resources[productId].GetResourceTexture();
			return true;
		}
		return false;
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x00062E78 File Offset: 0x00061078
	public override string ToString()
	{
		string text = "[RewardDefinition (";
		text = text + "summary=" + this.summary;
		text = text + "\nbuckets=" + this.generatorBuckets;
		return text + ")]";
	}

	// Token: 0x04000A67 RID: 2663
	private const string SUMMARY = "summary";

	// Token: 0x04000A68 RID: 2664
	private CdfDictionary<RewardDefinition.GeneratorBucket> generatorBuckets;

	// Token: 0x04000A69 RID: 2665
	private Reward summary;

	// Token: 0x020001C1 RID: 449
	private class GeneratorBucket
	{
		// Token: 0x06000F73 RID: 3955 RVA: 0x00062EBC File Offset: 0x000610BC
		public GeneratorBucket(Dictionary<int, ResultGenerator> resourcesGenerator, Dictionary<int, ResultGenerator> buildingsGenerator, Dictionary<int, ResultGenerator> recipesGenerator, Dictionary<int, ResultGenerator> moviesGenerator, Dictionary<int, ResultGenerator> costumesGenerator, Reward summary)
		{
			this.resourcesGenerator = resourcesGenerator;
			this.buildingsGenerator = buildingsGenerator;
			this.recipesGenerator = recipesGenerator;
			this.moviesGenerator = moviesGenerator;
			this.costumesGenerator = costumesGenerator;
			this.summary = summary;
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00062EF4 File Offset: 0x000610F4
		public override string ToString()
		{
			string rv = "[GeneratorBucket (";
			Action<Dictionary<int, ResultGenerator>> action = delegate(Dictionary<int, ResultGenerator> generators)
			{
				if (generators != null)
				{
					foreach (KeyValuePair<int, ResultGenerator> keyValuePair in generators)
					{
						string rv = rv;
						rv = string.Concat(new object[]
						{
							rv,
							keyValuePair.Key,
							": ",
							keyValuePair.Value
						});
					}
				}
			};
			action(this.resourcesGenerator);
			action(this.buildingsGenerator);
			action(this.recipesGenerator);
			action(this.moviesGenerator);
			action(this.costumesGenerator);
			rv += ")]";
			return rv;
		}

		// Token: 0x04000A6A RID: 2666
		public Dictionary<int, ResultGenerator> resourcesGenerator;

		// Token: 0x04000A6B RID: 2667
		public Dictionary<int, ResultGenerator> buildingsGenerator;

		// Token: 0x04000A6C RID: 2668
		public Dictionary<int, ResultGenerator> recipesGenerator;

		// Token: 0x04000A6D RID: 2669
		public Dictionary<int, ResultGenerator> moviesGenerator;

		// Token: 0x04000A6E RID: 2670
		public Dictionary<int, ResultGenerator> costumesGenerator;

		// Token: 0x04000A6F RID: 2671
		public Reward summary;
	}
}
