using System;
using System.Collections.Generic;

// Token: 0x020000C2 RID: 194
public class ResourceAmountsMigration : Migration
{
	// Token: 0x06000771 RID: 1905 RVA: 0x000313BC File Offset: 0x0002F5BC
	public override bool MigrateGamestate(Dictionary<string, object> gamestate, StaticContentLoader contentLoader)
	{
		object obj;
		if (!gamestate.TryGetValue("farm", out obj))
		{
			TFUtils.ErrorLog("Unable to access farm");
			return false;
		}
		Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
		if (dictionary == null)
		{
			TFUtils.ErrorLog("Farm is not a dictionary");
			return false;
		}
		if (!dictionary.TryGetValue("resources", out obj))
		{
			TFUtils.ErrorLog("Farm is missing resources");
			return false;
		}
		List<object> list = obj as List<object>;
		if (list == null)
		{
			TFUtils.ErrorLog("Resources is not a list");
			return false;
		}
		foreach (object obj2 in list)
		{
			Dictionary<string, object> dictionary2 = obj2 as Dictionary<string, object>;
			if (dictionary2 == null)
			{
				TFUtils.ErrorLog("Resource is not a dictionary");
				return false;
			}
			if (dictionary2.ContainsKey("amount") || !dictionary2.ContainsKey("amount_earned") || !dictionary2.ContainsKey("amount_spent") || !dictionary2.ContainsKey("amount_purchased"))
			{
				if (!dictionary2.TryGetValue("amount", out obj))
				{
					TFUtils.ErrorLog("Resource is missing amount");
					return false;
				}
				if (!(obj is long))
				{
					TFUtils.ErrorLog("Resource amount is not int");
					return false;
				}
				long num = (long)obj;
				dictionary2.Remove("amount");
				dictionary2.Add("amount_earned", num);
				dictionary2.Add("amount_spent", 0);
				dictionary2.Add("amount_purchased", 0);
			}
		}
		return true;
	}
}
