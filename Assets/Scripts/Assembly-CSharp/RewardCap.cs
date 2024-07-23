using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class RewardCap
{
	// Token: 0x06000F5D RID: 3933 RVA: 0x0006230C File Offset: 0x0006050C
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["recipe_count"] = this.recipes;
		dictionary["jelly_count"] = this.jelly;
		dictionary["expiration"] = this.expiration;
		return dictionary;
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x00062364 File Offset: 0x00060564
	public bool Filter(Simulation simulation, ref Reward reward)
	{
		bool result = false;
		bool flag = false;
		if (TFUtils.EpochTime() >= this.expiration)
		{
			this.Clear();
		}
		if (reward.ResourceAmounts.ContainsKey(ResourceManager.HARD_CURRENCY))
		{
			int num = reward.ResourceAmounts[ResourceManager.HARD_CURRENCY];
			if (this.jelly + num > 1000)
			{
				num = 1000 - this.jelly;
				if (num > 0)
				{
					reward.ResourceAmounts[ResourceManager.HARD_CURRENCY] = 1000 - num;
				}
				else
				{
					reward.ResourceAmounts.Remove(ResourceManager.HARD_CURRENCY);
				}
				Debug.LogWarning("Hit the JJ Cap - think about increasing the cap!");
				result = true;
			}
			flag = true;
			this.jelly += num;
		}
		if (reward.RecipeUnlocks != null)
		{
			int num2 = 0;
			if (this.FilterRecipes(simulation, reward, out num2))
			{
				Debug.LogWarning("Hit the Recipe Cap - think about increasing the cap!");
				this.recipes = 5;
				result = true;
			}
			else
			{
				this.recipes += num2;
			}
			if (num2 > 0)
			{
				flag = true;
			}
		}
		if (flag)
		{
			simulation.game.Record(new RewardCapAction(this.jelly, this.recipes, this.expiration));
		}
		return result;
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x0006249C File Offset: 0x0006069C
	public void Reset(int jelly, int recipes, ulong expiration)
	{
		this.jelly = jelly;
		this.recipes = recipes;
		this.expiration = expiration;
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x000624B4 File Offset: 0x000606B4
	private void Clear()
	{
		this.jelly = 0;
		this.recipes = 0;
		this.expiration = TFUtils.EpochTime() + 86400UL;
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x000624E4 File Offset: 0x000606E4
	private bool FilterRecipes(Simulation simulation, Reward reward, out int complexRecipes)
	{
		int count = reward.RecipeUnlocks.Count;
		int i = 0;
		Predicate<int> match = (int recipeId) => !simulation.craftManager.IsComplexRecipe(simulation.craftManager.GetRecipeById(recipeId)) || i++ < 5 - this.recipes;
		reward.RecipeUnlocks = reward.RecipeUnlocks.FindAll(match);
		complexRecipes = i;
		return count != reward.RecipeUnlocks.Count;
	}

	// Token: 0x04000A5C RID: 2652
	public const string REWARD_CAP_FIELD = "caps";

	// Token: 0x04000A5D RID: 2653
	public const string RECIPE_COUNT = "recipe_count";

	// Token: 0x04000A5E RID: 2654
	public const string JELLY_COUNT = "jelly_count";

	// Token: 0x04000A5F RID: 2655
	public const string EXPIRATION = "expiration";

	// Token: 0x04000A60 RID: 2656
	public const int CONSOLATION_SOFT_CURRENCY_AMOUNT = 25;

	// Token: 0x04000A61 RID: 2657
	private const int JELLY_CAP = 1000;

	// Token: 0x04000A62 RID: 2658
	private const int RECIPE_CAP = 5;

	// Token: 0x04000A63 RID: 2659
	private const int PERIOD = 86400;

	// Token: 0x04000A64 RID: 2660
	private ulong expiration;

	// Token: 0x04000A65 RID: 2661
	private int recipes;

	// Token: 0x04000A66 RID: 2662
	private int jelly;
}
