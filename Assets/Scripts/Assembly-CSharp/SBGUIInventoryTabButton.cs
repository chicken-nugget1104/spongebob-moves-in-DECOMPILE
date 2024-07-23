using System;
using System.Collections.Generic;

// Token: 0x02000085 RID: 133
public class SBGUIInventoryTabButton : SBGUIButton
{
	// Token: 0x0600050F RID: 1295 RVA: 0x0002028C File Offset: 0x0001E48C
	public void SetSelected(bool selected)
	{
		if (selected)
		{
		}
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x0002029C File Offset: 0x0001E49C
	private static void SetupTabNames()
	{
		SBGUIInventoryTabButton.tabNames = new Dictionary<string, string>();
		SBGUIInventoryTabButton.tabNames["building"] = "Buildings";
		SBGUIInventoryTabButton.tabNames["trees"] = "Trees";
		SBGUIInventoryTabButton.tabNames["unit"] = "Characters";
		SBGUIInventoryTabButton.tabNames["worker"] = "Characters";
		SBGUIInventoryTabButton.tabNames["decoration"] = "Decorations";
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00020318 File Offset: 0x0001E518
	private static string GetTabName(string t)
	{
		if (SBGUIInventoryTabButton.tabNames == null)
		{
			SBGUIInventoryTabButton.SetupTabNames();
		}
		string empty = string.Empty;
		SBGUIInventoryTabButton.tabNames.TryGetValue(t, out empty);
		return empty;
	}

	// Token: 0x040003DA RID: 986
	private static Dictionary<string, string> tabNames;

	// Token: 0x040003DB RID: 987
	public string tabName;
}
