using System;
using System.Collections.Generic;

// Token: 0x02000232 RID: 562
public class LockRecipe : SessionActionDefinition
{
	// Token: 0x06001243 RID: 4675 RVA: 0x0007EF2C File Offset: 0x0007D12C
	public static LockRecipe Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		LockRecipe lockRecipe = new LockRecipe();
		lockRecipe.Parse(data, id, startConditions, originatedFromQuest);
		return lockRecipe;
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x0007EF4C File Offset: 0x0007D14C
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.recipeID = TFUtils.TryLoadInt(data, "id");
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x0007EF7C File Offset: 0x0007D17C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		Dictionary<string, object> dictionary2 = dictionary;
		string key = "id";
		int? num = this.recipeID;
		dictionary2[key] = ((num != null) ? this.recipeID : new int?(-1));
		return dictionary;
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x0007EFC8 File Offset: 0x0007D1C8
	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		int? num = this.recipeID;
		if (num != null)
		{
			int? num2 = this.recipeID;
			if ((num2 == null || num2.Value >= 0) && session.TheGame.craftManager.IsRecipeUnlocked(this.recipeID.Value))
			{
				if (!session.TheGame.craftManager.LockRecipe(this.recipeID.Value))
				{
					action.MarkFailed();
					return;
				}
				session.TheGame.simulation.ModifyGameState(new LockRecipeAction(this.recipeID.Value));
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}

	// Token: 0x04000C88 RID: 3208
	public const string TYPE = "lock_recipe";

	// Token: 0x04000C89 RID: 3209
	public const string RECIPE_ID = "id";

	// Token: 0x04000C8A RID: 3210
	private int? recipeID;
}
