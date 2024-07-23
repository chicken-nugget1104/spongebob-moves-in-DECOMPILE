using System;
using System.Collections.Generic;

// Token: 0x020001BB RID: 443
public class ResourceProductGroup
{
	// Token: 0x06000F29 RID: 3881 RVA: 0x00060A34 File Offset: 0x0005EC34
	public void AddRecipe(CraftingManager craftingManager, CraftingRecipe recipe)
	{
		int num = this.recipeDids.Count;
		for (int i = 0; i < this.recipeDids.Count; i++)
		{
			CraftingRecipe recipeById = craftingManager.GetRecipeById(this.recipeDids[i]);
			if (recipe.groupOrder > recipeById.groupOrder)
			{
				num = i;
				break;
			}
		}
		if (num == this.recipeDids.Count)
		{
			this.recipeDids.Add(recipe.identity);
		}
		else
		{
			this.recipeDids.Insert(num, recipe.identity);
		}
	}

	// Token: 0x04000A33 RID: 2611
	public string name;

	// Token: 0x04000A34 RID: 2612
	public List<int> recipeDids = new List<int>();
}
