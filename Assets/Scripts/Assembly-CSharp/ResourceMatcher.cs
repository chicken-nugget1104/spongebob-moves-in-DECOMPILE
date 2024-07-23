using System;
using System.Collections.Generic;

// Token: 0x02000192 RID: 402
public class ResourceMatcher : Matcher
{
	// Token: 0x06000D82 RID: 3458 RVA: 0x000526AC File Offset: 0x000508AC
	public static ResourceMatcher FromDict(Dictionary<string, object> dict)
	{
		ResourceMatcher resourceMatcher = new ResourceMatcher();
		resourceMatcher.RegisterProperty("resource_id", dict, new MatchableProperty.MatchFn(resourceMatcher.ResourceIdMatchFn));
		resourceMatcher.RegisterProperty("balance", dict, new MatchableProperty.MatchFn(resourceMatcher.BalanceMatchFn));
		resourceMatcher.RegisterProperty("delta", dict, new MatchableProperty.MatchFn(resourceMatcher.DeltaMatchFn));
		return resourceMatcher;
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x0005270C File Offset: 0x0005090C
	public override string DescribeSubject(Game game)
	{
		if (game == null || game.resourceManager == null)
		{
			return string.Empty;
		}
		Resource resource = game.resourceManager.Resources[int.Parse(this.GetTarget("resource_id"))];
		if (resource.Amount > 1)
		{
			return resource.Name_Plural;
		}
		return resource.Name;
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x0005276C File Offset: 0x0005096C
	private uint ResourceIdMatchFn(MatchableProperty idProperty, Dictionary<string, object> triggerData, Game game)
	{
		int key = int.Parse(idProperty.Target.ToString());
		if (triggerData.ContainsKey("resource_amounts"))
		{
			Dictionary<int, int> dictionary = AmountDictionary.FromJSONDict(TFUtils.LoadDict(triggerData, "resource_amounts"));
			if (dictionary.ContainsKey(key))
			{
				return (uint)Math.Abs(dictionary[key]);
			}
		}
		return 0U;
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x000527C8 File Offset: 0x000509C8
	private uint BalanceMatchFn(MatchableProperty balanceProperty, Dictionary<string, object> triggerData, Game game)
	{
		int resourceId = int.Parse(base.GetProperty("resource_id").Target.ToString());
		int amount = game.resourceManager.Query(resourceId);
		return base.CompareOperandRangesToAmount(balanceProperty.Target, amount);
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0005280C File Offset: 0x00050A0C
	private uint DeltaMatchFn(MatchableProperty deltaProperty, Dictionary<string, object> triggerData, Game game)
	{
		int num = int.Parse(base.GetProperty("resource_id").Target.ToString());
		if (triggerData.ContainsKey("resource_amounts"))
		{
			Dictionary<string, object> dictionary = triggerData["resource_amounts"] as Dictionary<string, object>;
			if (dictionary != null)
			{
				if (dictionary.ContainsKey(num.ToString()))
				{
					return base.CompareOperandRangesToAmount(deltaProperty.Target, TFUtils.LoadInt(dictionary, num.ToString()));
				}
				return 0U;
			}
		}
		return 0U;
	}

	// Token: 0x040008F5 RID: 2293
	public const string RESOURCE_ID = "resource_id";

	// Token: 0x040008F6 RID: 2294
	public const string BALANCE = "balance";

	// Token: 0x040008F7 RID: 2295
	public const string DELTA = "delta";
}
