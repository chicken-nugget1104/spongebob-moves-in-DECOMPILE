using System;
using System.Collections.Generic;

// Token: 0x0200018B RID: 395
public abstract class Matcher : IMatcher
{
	// Token: 0x06000D57 RID: 3415 RVA: 0x00051F74 File Offset: 0x00050174
	protected Matcher() : this(new Dictionary<string, MatchableProperty>())
	{
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x00051F84 File Offset: 0x00050184
	protected Matcher(Dictionary<string, MatchableProperty> matchableProperties)
	{
		if (matchableProperties.Count > 0)
		{
			this.hasRequirements = true;
		}
		this.matchableProperties = matchableProperties;
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00051FB4 File Offset: 0x000501B4
	public ICollection<string> Keys
	{
		get
		{
			return this.matchableProperties.Keys;
		}
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x00051FC4 File Offset: 0x000501C4
	public virtual uint MatchAmount(Game game, Dictionary<string, object> data)
	{
		uint num = 1U;
		foreach (string key in this.matchableProperties.Keys)
		{
			MatchableProperty matchableProperty = this.matchableProperties[key];
			if (matchableProperty.IsRequired)
			{
				num *= matchableProperty.Evaluate(data, game);
			}
		}
		return num;
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x00052050 File Offset: 0x00050250
	public bool IsRequired(string property)
	{
		TFUtils.Assert(this.matchableProperties.ContainsKey(property), string.Format("Can't query a property({0}) that has not been added!", property));
		return this.matchableProperties[property].IsRequired;
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x0005208C File Offset: 0x0005028C
	public bool HasRequirements()
	{
		return this.hasRequirements;
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x00052094 File Offset: 0x00050294
	protected MatchableProperty GetProperty(string key)
	{
		TFUtils.Assert(this.matchableProperties.ContainsKey(key), "Cannot get property for key=" + key + " since it does not have a registered matchable property.");
		return this.matchableProperties[key];
	}

	// Token: 0x06000D5E RID: 3422 RVA: 0x000520C4 File Offset: 0x000502C4
	public object GetTargetObject(string propertyKey)
	{
		return this.matchableProperties[propertyKey].Target;
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x000520D8 File Offset: 0x000502D8
	public string GetTarget(string propertyKey)
	{
		return this.matchableProperties[propertyKey].Target.ToString();
	}

	// Token: 0x06000D60 RID: 3424
	public abstract string DescribeSubject(Game game);

	// Token: 0x06000D61 RID: 3425 RVA: 0x000520F0 File Offset: 0x000502F0
	protected bool RegisterProperty(string key, Dictionary<string, object> data)
	{
		return this.RegisterProperty(key, data, new MatchableProperty.MatchFn(Matcher.DefaultMatchFn));
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x00052108 File Offset: 0x00050308
	protected bool RegisterProperty(string key, Dictionary<string, object> data, MatchableProperty.MatchFn matchDelegate)
	{
		if (!data.ContainsKey(key))
		{
			this.matchableProperties[key] = new MatchableProperty(false, key, null, matchDelegate);
			return false;
		}
		this.hasRequirements = true;
		Dictionary<string, object> dictionary = data[key] as Dictionary<string, object>;
		if (dictionary != null)
		{
			return this.AddRequiredProperty(key, dictionary, matchDelegate);
		}
		return this.AddRequiredProperty(key, data[key].ToString(), matchDelegate);
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x00052170 File Offset: 0x00050370
	protected bool AddRequiredProperty(string key, object val)
	{
		return this.AddRequiredProperty(key, val, new MatchableProperty.MatchFn(Matcher.DefaultMatchFn));
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x00052188 File Offset: 0x00050388
	protected bool AddRequiredProperty(string key, object val, MatchableProperty.MatchFn matchDelegate)
	{
		this.AssertNotDuplicate(key);
		this.matchableProperties[key] = new MatchableProperty(true, key, val, matchDelegate);
		return true;
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x000521B4 File Offset: 0x000503B4
	private void AssertNotDuplicate(string key)
	{
		TFUtils.Assert(!this.matchableProperties.ContainsKey(key), "Already have value for this key!");
	}

	// Token: 0x06000D66 RID: 3430 RVA: 0x000521D0 File Offset: 0x000503D0
	private static uint DefaultMatchFn(MatchableProperty property, Dictionary<string, object> triggerData, Game game)
	{
		TFUtils.Assert(property.IsRequired, "Should not be trying to match against an optional parameter!");
		if (!triggerData.ContainsKey(property.Key))
		{
			return 0U;
		}
		string text = triggerData[property.Key].ToString();
		if (text.Equals(property.Target))
		{
			return 1U;
		}
		return 0U;
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x00052228 File Offset: 0x00050428
	public override string ToString()
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, MatchableProperty> keyValuePair in this.matchableProperties)
		{
			text += keyValuePair.Value.ToString();
			text += ", ";
		}
		return "Matcher:(properties=[" + text + "])";
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x000522BC File Offset: 0x000504BC
	protected uint CompareOperandRangesToAmount(object target, int amount)
	{
		int num;
		if (!int.TryParse(target.ToString(), out num))
		{
			Dictionary<string, object> dict = target as Dictionary<string, object>;
			return this.CompareOperatorAndROperand(dict, amount);
		}
		if (amount == num)
		{
			return 1U;
		}
		return 0U;
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x000522F8 File Offset: 0x000504F8
	protected uint CompareOperatorAndROperand(Dictionary<string, object> dict, int loperand)
	{
		if (dict != null)
		{
			TFUtils.Assert(!dict.ContainsKey("loperand"), "Do not specify a loperand for a range that compares against a derived amount!");
			int roperand = int.Parse(dict["roperand"].ToString());
			string operatorString = dict["operator"].ToString();
			return this.Compare(operatorString, loperand, roperand);
		}
		return 0U;
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x00052358 File Offset: 0x00050558
	protected uint Compare(string operatorString, int loperand, int roperand)
	{
		if (operatorString == ">" && loperand > roperand)
		{
			return 1U;
		}
		if (operatorString == ">=" && loperand >= roperand)
		{
			return 1U;
		}
		if (operatorString == "<" && loperand < roperand)
		{
			return 1U;
		}
		if (operatorString == "<=" && loperand <= roperand)
		{
			return 1U;
		}
		return 0U;
	}

	// Token: 0x040008E7 RID: 2279
	public const string OPERATOR = "operator";

	// Token: 0x040008E8 RID: 2280
	public const string LOPERAND = "loperand";

	// Token: 0x040008E9 RID: 2281
	public const string ROPERAND = "roperand";

	// Token: 0x040008EA RID: 2282
	private Dictionary<string, MatchableProperty> matchableProperties;

	// Token: 0x040008EB RID: 2283
	private bool hasRequirements;
}
