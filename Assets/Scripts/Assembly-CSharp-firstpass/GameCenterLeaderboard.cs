using System;
using System.Collections.Generic;
using Prime31;

// Token: 0x0200009C RID: 156
public class GameCenterLeaderboard
{
	// Token: 0x060005B3 RID: 1459 RVA: 0x00015D7C File Offset: 0x00013F7C
	public GameCenterLeaderboard(string leaderboardId, string title)
	{
		this.leaderboardId = leaderboardId;
		this.title = title;
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x00015D94 File Offset: 0x00013F94
	public static List<GameCenterLeaderboard> fromJSON(string json)
	{
		List<GameCenterLeaderboard> list = new List<GameCenterLeaderboard>();
		Dictionary<string, object> dictionary = json.dictionaryFromJson();
		foreach (KeyValuePair<string, object> keyValuePair in dictionary)
		{
			list.Add(new GameCenterLeaderboard(keyValuePair.Value as string, keyValuePair.Key));
		}
		return list;
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x00015E1C File Offset: 0x0001401C
	public override string ToString()
	{
		return string.Format("<Leaderboard> leaderboardId: {0}, title: {1}", this.leaderboardId, this.title);
	}

	// Token: 0x04000338 RID: 824
	public string leaderboardId;

	// Token: 0x04000339 RID: 825
	public string title;
}
