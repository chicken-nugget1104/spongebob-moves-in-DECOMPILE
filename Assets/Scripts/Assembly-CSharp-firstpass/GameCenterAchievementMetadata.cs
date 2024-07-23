using System;
using System.Collections.Generic;
using Prime31;

// Token: 0x02000095 RID: 149
public class GameCenterAchievementMetadata
{
	// Token: 0x06000593 RID: 1427 RVA: 0x000157F8 File Offset: 0x000139F8
	public GameCenterAchievementMetadata(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("identifier"))
		{
			this.identifier = (dict["identifier"] as string);
		}
		if (dict.ContainsKey("achievedDescription"))
		{
			this.description = (dict["achievedDescription"] as string);
		}
		if (dict.ContainsKey("unachievedDescription"))
		{
			this.unachievedDescription = (dict["unachievedDescription"] as string);
		}
		if (dict.ContainsKey("hidden"))
		{
			this.isHidden = (bool)dict["hidden"];
		}
		if (dict.ContainsKey("maximumPoints"))
		{
			this.maximumPoints = int.Parse(dict["maximumPoints"].ToString());
		}
		if (dict.ContainsKey("title"))
		{
			this.title = (dict["title"] as string);
		}
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x000158F4 File Offset: 0x00013AF4
	public static List<GameCenterAchievementMetadata> fromJSON(string json)
	{
		List<GameCenterAchievementMetadata> list = new List<GameCenterAchievementMetadata>();
		List<object> list2 = json.listFromJson();
		foreach (object obj in list2)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)obj;
			list.Add(new GameCenterAchievementMetadata(dict));
		}
		return list;
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x00015970 File Offset: 0x00013B70
	public override string ToString()
	{
		return string.Format("<AchievementMetaData> identifier: {0}, hidden: {1}, maxPoints: {2}, title: {3} desc: {4}, unachDesc: {5}", new object[]
		{
			this.identifier,
			this.isHidden,
			this.maximumPoints,
			this.title,
			this.description,
			this.unachievedDescription
		});
	}

	// Token: 0x0400031B RID: 795
	public string identifier;

	// Token: 0x0400031C RID: 796
	public string description;

	// Token: 0x0400031D RID: 797
	public string unachievedDescription;

	// Token: 0x0400031E RID: 798
	public bool isHidden;

	// Token: 0x0400031F RID: 799
	public int maximumPoints;

	// Token: 0x04000320 RID: 800
	public string title;
}
