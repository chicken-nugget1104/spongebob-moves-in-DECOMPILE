using System;
using System.Collections.Generic;

// Token: 0x0200010A RID: 266
public class UpdateVariableAction<T> : PersistedTriggerableAction
{
	// Token: 0x0600096A RID: 2410 RVA: 0x0003B110 File Offset: 0x00039310
	public UpdateVariableAction(string sVarName, T pVariable) : base("uv", Identity.Null())
	{
		this.m_sVarName = sVarName;
		this.m_pVariable = pVariable;
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x0600096B RID: 2411 RVA: 0x0003B130 File Offset: 0x00039330
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0003B134 File Offset: 0x00039334
	public new static UpdateVariableAction<T> FromDict(Dictionary<string, object> pData)
	{
		string sVarName = TFUtils.LoadString(pData, "var_name");
		Type typeFromHandle = typeof(T);
		T pVariable;
		if (typeFromHandle == typeof(int))
		{
			pVariable = (T)((object)TFUtils.LoadInt(pData, "variable"));
		}
		else if (typeFromHandle == typeof(uint))
		{
			pVariable = (T)((object)TFUtils.LoadUint(pData, "variable"));
		}
		else if (typeFromHandle == typeof(long))
		{
			pVariable = (T)((object)TFUtils.LoadLong(pData, "variable"));
		}
		else if (typeFromHandle == typeof(ulong))
		{
			pVariable = (T)((object)TFUtils.LoadUlong(pData, "variable", 0UL));
		}
		else if (typeFromHandle == typeof(float))
		{
			pVariable = (T)((object)TFUtils.LoadFloat(pData, "variable"));
		}
		else if (typeFromHandle == typeof(double))
		{
			pVariable = (T)((object)TFUtils.LoadDouble(pData, "variable"));
		}
		else if (typeFromHandle == typeof(bool))
		{
			pVariable = (T)((object)TFUtils.LoadBool(pData, "variable"));
		}
		else if (typeFromHandle == typeof(string))
		{
			pVariable = (T)((object)TFUtils.LoadString(pData, "variable"));
		}
		else
		{
			TFUtils.Assert(false, "UpdateVariableAction.cs unsupported variable type.");
			pVariable = default(T);
		}
		return new UpdateVariableAction<T>(sVarName, pVariable);
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0003B2CC File Offset: 0x000394CC
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["var_name"] = this.m_sVarName;
		dictionary["variable"] = this.m_pVariable;
		return dictionary;
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0003B308 File Offset: 0x00039508
	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0003B314 File Offset: 0x00039514
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary;
		if (gameState.ContainsKey("variables"))
		{
			dictionary = (Dictionary<string, object>)gameState["variables"];
		}
		else
		{
			dictionary = new Dictionary<string, object>();
			gameState.Add("variables", dictionary);
		}
		if (dictionary.ContainsKey(this.m_sVarName))
		{
			dictionary[this.m_sVarName] = this.m_pVariable;
		}
		else
		{
			dictionary.Add(this.m_sVarName, this.m_pVariable);
		}
		base.Confirm(gameState);
	}

	// Token: 0x0400068A RID: 1674
	public const string UPDATE_VARIABLE = "uv";

	// Token: 0x0400068B RID: 1675
	private string m_sVarName;

	// Token: 0x0400068C RID: 1676
	private T m_pVariable;
}
