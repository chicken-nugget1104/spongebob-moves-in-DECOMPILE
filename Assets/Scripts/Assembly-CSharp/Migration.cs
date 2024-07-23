using System;
using System.Collections.Generic;

// Token: 0x020000C1 RID: 193
public abstract class Migration
{
	// Token: 0x0600076C RID: 1900 RVA: 0x000312E4 File Offset: 0x0002F4E4
	public Migration()
	{
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x000312F8 File Offset: 0x0002F4F8
	protected void RegisterActionMigrationDelegate(string actionType, Migration.ActionMigrationDelegate migrationDelegate)
	{
		this.actionToMigrationDelegate.Add(actionType, migrationDelegate);
	}

	// Token: 0x0600076E RID: 1902
	public abstract bool MigrateGamestate(Dictionary<string, object> gamestate, StaticContentLoader contentLoader);

	// Token: 0x0600076F RID: 1903 RVA: 0x00031308 File Offset: 0x0002F508
	public void MigrateActions(List<Dictionary<string, object>> actionList, StaticContentLoader contentLoader)
	{
		foreach (Dictionary<string, object> dictionary in actionList)
		{
			if (!dictionary.ContainsKey("type"))
			{
				TFUtils.DebugLog("Attempting to migration an action from malformed data! This should not have occurred, locate the source and fix it.");
			}
			string key = (string)dictionary["type"];
			if (this.actionToMigrationDelegate.ContainsKey(key))
			{
				Migration.ActionMigrationDelegate actionMigrationDelegate = this.actionToMigrationDelegate[key];
				actionMigrationDelegate(dictionary, contentLoader);
			}
		}
	}

	// Token: 0x0400059C RID: 1436
	private Dictionary<string, Migration.ActionMigrationDelegate> actionToMigrationDelegate = new Dictionary<string, Migration.ActionMigrationDelegate>();

	// Token: 0x0200049E RID: 1182
	// (Invoke) Token: 0x060024DB RID: 9435
	public delegate bool ActionMigrationDelegate(Dictionary<string, object> actionDict, StaticContentLoader contentLoader);
}
