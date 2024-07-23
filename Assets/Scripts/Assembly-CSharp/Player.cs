using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class Player
{
	// Token: 0x06000E1D RID: 3613 RVA: 0x0005620C File Offset: 0x0005440C
	public Player(string playerId)
	{
		TFUtils.DebugLog(string.Format("player created with id: {0} ", playerId));
		this.playerId = playerId;
		this.cacheDir = Player.CACHE_ROOT + this.PlayerFolder();
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x000562A4 File Offset: 0x000544A4
	public static Player LoadFromSoaringID(string userID)
	{
		return new Player(userID);
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x000562AC File Offset: 0x000544AC
	public static bool CheckSoaringPathExists(string userID)
	{
		string path = Player.CACHE_ROOT + Path.DirectorySeparatorChar + userID;
		return Directory.Exists(path);
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x000562D8 File Offset: 0x000544D8
	public static void MigratePlayerData(string soaringUserID, string playerId)
	{
		string text = Player.CACHE_ROOT + Path.DirectorySeparatorChar + soaringUserID;
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string text2 = null;
		if (TFUtils.FileIsExists(Player.PLAYER_ID_MAP_FILE))
		{
			if (playerId != null)
			{
				string text3 = playerId;
				if (text3.Length > 16)
				{
					text3 = playerId.Substring(0, 16);
				}
				string json = TFUtils.ReadAllText(Player.PLAYER_ID_MAP_FILE).Trim();
				Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(json);
				if (dictionary.ContainsKey(playerId))
				{
					text2 = Player._CheckMigrateDirectory((string)dictionary[playerId]);
				}
				else if (dictionary.ContainsKey(text3))
				{
					text2 = Player._CheckMigrateDirectory((string)dictionary[text3]);
				}
				if (text2 == null)
				{
					string previousDeviceIdFromPlayerMap = Player.GetPreviousDeviceIdFromPlayerMap(playerId);
					if (string.IsNullOrEmpty(playerId))
					{
						previousDeviceIdFromPlayerMap = Player.GetPreviousDeviceIdFromPlayerMap(text3);
					}
					if (!string.IsNullOrEmpty(previousDeviceIdFromPlayerMap) && (text2 = Player._CheckMigrateDirectory(previousDeviceIdFromPlayerMap)) == null && dictionary.ContainsKey(previousDeviceIdFromPlayerMap))
					{
						text2 = Player._CheckMigrateDirectory((string)dictionary[previousDeviceIdFromPlayerMap]);
					}
				}
				if (text2 == null)
				{
					text2 = Player._CheckMigrateDirectory("p_" + playerId);
				}
				if (text2 == null && dictionary.ContainsKey("p_" + playerId))
				{
					text2 = Player._CheckMigrateDirectory("p_" + playerId);
				}
			}
			else
			{
				text2 = Player.CACHE_ROOT + TFUtils.ReadAllText(Player.LAST_PLAYED_FILE).Trim() + Path.DirectorySeparatorChar;
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				"SoaringID: ",
				soaringUserID,
				" : ",
				playerId,
				" : ",
				text2
			}));
			if (TFUtils.FileIsExists(text2 + "actions.json"))
			{
				File.Copy(text2 + "actions.json", text + Path.DirectorySeparatorChar + "actions.json", true);
			}
			if (TFUtils.FileIsExists(text2 + "game.json"))
			{
				File.Copy(text2 + "game.json", text + Path.DirectorySeparatorChar + "game.json", true);
			}
			if (TFUtils.FileIsExists(text2 + "lastETag"))
			{
				File.Copy(text2 + "lastETag", text + Path.DirectorySeparatorChar + "lastETag", true);
			}
		}
		else
		{
			SoaringDebug.Log("Invalid Player ID, No Directory Found", LogType.Error);
		}
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x0005656C File Offset: 0x0005476C
	public static bool ValidTimeStamp(long timestamp)
	{
		return timestamp > 0L;
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x00056574 File Offset: 0x00054774
	public void SetStagedTimestamp(long ts)
	{
		this.mStagedTimestamp = ts;
		Debug.LogWarning("SetStagedTimestamp: " + this.mStagedTimestamp);
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00056598 File Offset: 0x00054798
	public long ReadTimestamp()
	{
		long num = -1L;
		string text = this.CacheFile("timestamp");
		if (TFUtils.FileIsExists(text))
		{
			try
			{
				string s = TFUtils.ReadAllText(text);
				if (!long.TryParse(s, out num))
				{
					num = -1L;
				}
			}
			catch
			{
				num = -1L;
			}
		}
		Debug.LogWarning(string.Concat(new object[]
		{
			"ReadTimestamp: ",
			num,
			" : ",
			text
		}));
		return num;
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x00056630 File Offset: 0x00054830
	public void SaveStagedTimestamp()
	{
		if (Player.ValidTimeStamp(this.mStagedTimestamp))
		{
			Debug.LogWarning("SaveStagedTimestamp: " + this.mStagedTimestamp);
			this.SaveTimestamp(this.mStagedTimestamp);
		}
		this.mStagedTimestamp = -1L;
	}

	// Token: 0x06000E26 RID: 3622 RVA: 0x0005667C File Offset: 0x0005487C
	public void SaveTimestamp(long timestamp)
	{
		File.WriteAllText(this.CacheFile("timestamp"), timestamp.ToString());
		Debug.LogWarning("SaveTimestamp: " + timestamp);
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x000566B8 File Offset: 0x000548B8
	public void DeleteTimestamp()
	{
		Debug.LogWarning("DeleteTimestamp");
		string text = this.CacheFile("timestamp");
		if (TFUtils.FileIsExists(text))
		{
			File.Delete(text);
		}
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x000566EC File Offset: 0x000548EC
	public static string RemovePrefix(string playerId)
	{
		if (playerId == null)
		{
			return null;
		}
		if (playerId.StartsWith("p_"))
		{
			return playerId.Substring(2);
		}
		return playerId;
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x00056710 File Offset: 0x00054910
	public static string LastPlayerId()
	{
		string text = TFUtils.ReadAllText(Player.LAST_PLAYED_FILE);
		if (text == null)
		{
			return null;
		}
		return Player.RemovePrefix(text.Trim());
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x0005673C File Offset: 0x0005493C
	public string CacheFile(string fileName)
	{
		return this.cacheDir + Path.DirectorySeparatorChar + fileName;
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x00056754 File Offset: 0x00054954
	public static string PlayerCacheFile(string player, string fileName)
	{
		return string.Concat(new object[]
		{
			Player.CACHE_ROOT,
			player,
			Path.DirectorySeparatorChar,
			fileName
		});
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x0005678C File Offset: 0x0005498C
	public string CacheDir()
	{
		return this.cacheDir;
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x00056794 File Offset: 0x00054994
	private string PlayerFolder()
	{
		return this.playerId;
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x0005679C File Offset: 0x0005499C
	public static Dictionary<string, object> GetPlayerMap()
	{
		if (File.Exists(Player.PLAYER_ID_MAP_FILE))
		{
			string json = TFUtils.ReadAllText(Player.PLAYER_ID_MAP_FILE);
			return (Dictionary<string, object>)Json.Deserialize(json);
		}
		return new Dictionary<string, object>();
	}

	// Token: 0x06000E2F RID: 3631 RVA: 0x000567D4 File Offset: 0x000549D4
	private static string GetPreviousDeviceIdFromPlayerMap(string currentDeviceId)
	{
		Dictionary<string, object> playerMap = Player.GetPlayerMap();
		foreach (KeyValuePair<string, object> keyValuePair in playerMap)
		{
			if (!keyValuePair.Key.Contains("G:") && keyValuePair.Key != currentDeviceId && keyValuePair.Key != "0f607264fc6318a9")
			{
				return keyValuePair.Key;
			}
		}
		return null;
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x00056884 File Offset: 0x00054A84
	private static string _CheckMigrateDirectory(string playerID)
	{
		if (string.IsNullOrEmpty(playerID))
		{
			return null;
		}
		string text = Player.CACHE_ROOT + playerID + Path.DirectorySeparatorChar;
		if (Directory.Exists(text))
		{
			return text;
		}
		return null;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x000568C4 File Offset: 0x00054AC4
	public static void Init()
	{
		Player.CACHE_ROOT = Application.persistentDataPath + Path.DirectorySeparatorChar;
	}

	// Token: 0x04000950 RID: 2384
	private const string LAST_PLAYED = "lastplayer";

	// Token: 0x04000951 RID: 2385
	private const string PLAYER_ID_MAP = "player_map";

	// Token: 0x04000952 RID: 2386
	private const string USER_FILE = "user.json";

	// Token: 0x04000953 RID: 2387
	private const string PLAYER_TIMESTAMP = "timestamp";

	// Token: 0x04000954 RID: 2388
	private const string CORRUPT_IOS7_DEVICE_ID = "0f607264fc6318a9";

	// Token: 0x04000955 RID: 2389
	private static string CACHE_ROOT = Application.persistentDataPath + Path.DirectorySeparatorChar;

	// Token: 0x04000956 RID: 2390
	private static string LAST_PLAYED_FILE = Player.CACHE_ROOT + "lastplayer";

	// Token: 0x04000957 RID: 2391
	private static string PLAYER_ID_MAP_FILE = Player.CACHE_ROOT + "player_map";

	// Token: 0x04000958 RID: 2392
	private string cacheDir;

	// Token: 0x04000959 RID: 2393
	private long mStagedTimestamp = -1L;

	// Token: 0x0400095A RID: 2394
	public string playerId;
}
