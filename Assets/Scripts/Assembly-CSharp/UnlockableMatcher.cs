using System;
using System.Collections.Generic;

// Token: 0x0200019B RID: 411
public class UnlockableMatcher : Matcher
{
	// Token: 0x06000DA7 RID: 3495 RVA: 0x00053470 File Offset: 0x00051670
	public static UnlockableMatcher FromDict(Dictionary<string, object> dict)
	{
		UnlockableMatcher unlockableMatcher = new UnlockableMatcher();
		unlockableMatcher.RegisterProperty("unlockable_type", dict);
		unlockableMatcher.RegisterProperty("unlockable_id", dict);
		TFUtils.Assert(unlockableMatcher.IsRequired("unlockable_type"), "You must specify an unlockable type");
		return unlockableMatcher;
	}

	// Token: 0x06000DA8 RID: 3496 RVA: 0x000534B4 File Offset: 0x000516B4
	public override string DescribeSubject(Game game)
	{
		string target = this.GetTarget("unlockable_type");
		if (target != null)
		{
			if (UnlockableMatcher.<>f__switch$mapD == null)
			{
				UnlockableMatcher.<>f__switch$mapD = new Dictionary<string, int>(1)
				{
					{
						"recipe",
						0
					}
				};
			}
			int num;
			if (UnlockableMatcher.<>f__switch$mapD.TryGetValue(target, out num))
			{
				if (num == 0)
				{
					return "!!COND_SECRET_RECIPE";
				}
			}
		}
		TFUtils.ErrorLog("Trying to describe an unkown unlockable type! type=" + this.GetTarget("unlockable_type"));
		return string.Empty;
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x00053538 File Offset: 0x00051738
	public override uint MatchAmount(Game game, Dictionary<string, object> data)
	{
		if (this.GetTarget("unlockable_type") == "recipe")
		{
			int id = -1;
			bool flag = int.TryParse(base.GetProperty("unlockable_id").Target.ToString(), out id);
			if (flag && game.craftManager.IsRecipeUnlocked(id))
			{
				return 1U;
			}
		}
		return base.MatchAmount(game, data);
	}

	// Token: 0x04000916 RID: 2326
	public const string UNLOCKABLE_TYPE = "unlockable_type";

	// Token: 0x04000917 RID: 2327
	public const string UNLOCKABLE_ID = "unlockable_id";
}
