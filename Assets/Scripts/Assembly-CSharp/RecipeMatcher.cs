using System;
using System.Collections.Generic;

// Token: 0x02000190 RID: 400
public class RecipeMatcher : Matcher
{
	// Token: 0x06000D7B RID: 3451 RVA: 0x000525A0 File Offset: 0x000507A0
	public static RecipeMatcher FromDict(Dictionary<string, object> dict)
	{
		RecipeMatcher recipeMatcher = new RecipeMatcher();
		recipeMatcher.RegisterProperty("recipe_id", dict, new MatchableProperty.MatchFn(recipeMatcher.RecipeIdMatchFn));
		return recipeMatcher;
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x000525D0 File Offset: 0x000507D0
	public override string DescribeSubject(Game game)
	{
		if (game != null && game.craftManager != null)
		{
			return game.craftManager.Recipes[int.Parse(this.GetTarget("recipe_id"))].recipeName;
		}
		return string.Empty;
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x0005261C File Offset: 0x0005081C
	public uint RecipeIdMatchFn(MatchableProperty idProperty, Dictionary<string, object> triggerData, Game game)
	{
		int num = int.Parse(idProperty.Target.ToString());
		if (triggerData.ContainsKey("recipes"))
		{
			List<object> list = (List<object>)triggerData["recipes"];
			if (list.Contains(num))
			{
				return 1U;
			}
		}
		return 0U;
	}

	// Token: 0x040008F3 RID: 2291
	public const string RECIPE_ID = "recipe_id";
}
