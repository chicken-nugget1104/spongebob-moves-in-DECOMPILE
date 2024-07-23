using System;
using System.Collections.Generic;

// Token: 0x020000BF RID: 191
public class GamestateMigrator
{
	// Token: 0x06000768 RID: 1896 RVA: 0x0003112C File Offset: 0x0002F32C
	static GamestateMigrator()
	{
		GamestateMigrator.RegisterMigration(GamestateMigrator.MigrationTypes.RESOURCE_AMOUNTS_MIGRATION, new ResourceAmountsMigration());
		GamestateMigrator.RegisterMigration(GamestateMigrator.MigrationTypes.SOARING_SAVE_MIGRATION, new SoaringSaveMigration());
		GamestateMigrator.MigrationTypes[] array = (GamestateMigrator.MigrationTypes[])Enum.GetValues(typeof(GamestateMigrator.MigrationTypes));
		GamestateMigrator.CURRENT_VERSION = (int)array[array.Length - 1];
		TFUtils.DebugLog(string.Format("Current protocol version is {0} ({1})", GamestateMigrator.CURRENT_VERSION, Enum.GetName(typeof(GamestateMigrator.MigrationTypes), array[array.Length - 1])));
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x000311B4 File Offset: 0x0002F3B4
	public int GetProtocolVersion(Dictionary<string, object> gamestate)
	{
		int result = 0;
		if (gamestate.ContainsKey("protocol_version"))
		{
			result = TFUtils.LoadInt(gamestate, "protocol_version");
		}
		return result;
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x000311E0 File Offset: 0x0002F3E0
	public void Migrate(Dictionary<string, object> gamestate, List<Dictionary<string, object>> actionList, StaticContentLoader contentLoader, Player p, out int performedMigration)
	{
		int protocolVersion = this.GetProtocolVersion(gamestate);
		performedMigration = 1;
		TFUtils.DebugLog("Gamestate is currently running protocol version " + Enum.GetName(typeof(GamestateMigrator.MigrationTypes), protocolVersion));
		if (protocolVersion > GamestateMigrator.CURRENT_VERSION)
		{
			performedMigration = 3;
		}
		else if (protocolVersion < GamestateMigrator.CURRENT_VERSION)
		{
			GamestateMigrator.MigrationTypes[] array = (GamestateMigrator.MigrationTypes[])Enum.GetValues(typeof(GamestateMigrator.MigrationTypes));
			for (int i = protocolVersion + 1; i < array.Length; i++)
			{
				TFUtils.DebugLog("Migrating gamestate to protocol version " + Enum.GetName(typeof(GamestateMigrator.MigrationTypes), array[i]));
				Migration migration = GamestateMigrator.migrationTypeToMigration[array[i]];
				migration.MigrateGamestate(gamestate, contentLoader);
				migration.MigrateActions(actionList, contentLoader);
				gamestate["protocol_version"] = (int)array[i];
			}
			int protocolVersion2 = this.GetProtocolVersion(gamestate);
			if (protocolVersion2 != protocolVersion)
			{
				performedMigration = 2;
			}
		}
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x000312D4 File Offset: 0x0002F4D4
	public static void RegisterMigration(GamestateMigrator.MigrationTypes migrationType, Migration migration)
	{
		GamestateMigrator.migrationTypeToMigration.Add(migrationType, migration);
	}

	// Token: 0x04000593 RID: 1427
	public const int STATUS_NO_MIGRATION_PERFORMED = 1;

	// Token: 0x04000594 RID: 1428
	public const int STATUS_MIGRATION_PERFORMED = 2;

	// Token: 0x04000595 RID: 1429
	public const int STATUS_CANNOT_MIGRATE_NEWER_PROTOCOL = 3;

	// Token: 0x04000596 RID: 1430
	public static int CURRENT_VERSION = -1;

	// Token: 0x04000597 RID: 1431
	private static Dictionary<GamestateMigrator.MigrationTypes, Migration> migrationTypeToMigration = new Dictionary<GamestateMigrator.MigrationTypes, Migration>();

	// Token: 0x020000C0 RID: 192
	public enum MigrationTypes
	{
		// Token: 0x04000599 RID: 1433
		INITIAL,
		// Token: 0x0400059A RID: 1434
		RESOURCE_AMOUNTS_MIGRATION,
		// Token: 0x0400059B RID: 1435
		SOARING_SAVE_MIGRATION
	}
}
