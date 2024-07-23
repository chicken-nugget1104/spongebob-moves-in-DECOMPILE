using System;
using System.Collections.Generic;

// Token: 0x020000C3 RID: 195
public class SoaringSaveMigration : Migration
{
	// Token: 0x06000773 RID: 1907 RVA: 0x0003158C File Offset: 0x0002F78C
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
		if (!dictionary.TryGetValue("buildings", out obj))
		{
			TFUtils.ErrorLog("Farm is missing buildings");
			return false;
		}
		List<object> list = obj as List<object>;
		if (list == null)
		{
			TFUtils.ErrorLog("Buildings is not a list");
			return false;
		}
		foreach (object obj2 in list)
		{
			Dictionary<string, object> dictionary2 = obj2 as Dictionary<string, object>;
			if (dictionary2 != null)
			{
				if (dictionary2.ContainsKey("craft.rewards"))
				{
					if (dictionary2.TryGetValue("craft.rewards", out obj))
					{
						if (obj != null)
						{
							dictionary2.Remove("craft.rewards");
							dictionary2.Add("craft_rewards", obj);
						}
					}
				}
			}
		}
		return true;
	}
}
