using System;

// Token: 0x02000463 RID: 1123
public class TFError
{
	// Token: 0x0600231E RID: 8990 RVA: 0x000D6F24 File Offset: 0x000D5124
	public static void DM_LOG_ERROR_INVALID_SHEET(string sheetName)
	{
		TFUtils.ErrorLog("Cannot find database with sheet name: " + sheetName);
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x000D6F38 File Offset: 0x000D5138
	public static void DM_LOG_ERROR_NO_ROWS(string sheetName)
	{
		TFUtils.ErrorLog("No rows in sheet name: " + sheetName);
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000D6F4C File Offset: 0x000D514C
	public static void DM_LOG_ERROR_INVALID_COLUMN(string col)
	{
		TFUtils.ErrorLog("Invalid Columns Name: " + col);
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x000D6F60 File Offset: 0x000D5160
	public static int GetErrorCode(Exception e, int default_code)
	{
		if (e == null)
		{
			return default_code;
		}
		if (e.Data == null)
		{
			return default_code;
		}
		if (!e.Data.Contains("error_code"))
		{
			return default_code;
		}
		return (int)e.Data["error_code"];
	}

	// Token: 0x04001582 RID: 5506
	public const string ERROR_CODE_KEY = "error_code";

	// Token: 0x04001583 RID: 5507
	public const int CONNECTION_NO_CONNECTION_AVAILABLE = 101;

	// Token: 0x04001584 RID: 5508
	public const int CONNECTION_NO_SOARING_USER = 102;

	// Token: 0x04001585 RID: 5509
	public const int SOARING_INTERNAL_ERROR = 103;

	// Token: 0x04001586 RID: 5510
	public const int SOARING_AUTH_FAILED = 104;

	// Token: 0x04001587 RID: 5511
	public const int INVALID_GAME_STATE = 200;

	// Token: 0x04001588 RID: 5512
	public const int SAVE_GAMES_ALL_INVALID = 250;

	// Token: 0x04001589 RID: 5513
	public const int SAVE_SERVER_GAME_INVALID = 301;

	// Token: 0x0400158A RID: 5514
	public const int SAVE_CLIENT_GAME_INVALID = 302;

	// Token: 0x0400158B RID: 5515
	public const int INVALID_RESOURCE = 303;
}
