using System;
using System.Collections.Generic;

// Token: 0x020000A0 RID: 160
public class GameCenterScore
{
	// Token: 0x06000653 RID: 1619 RVA: 0x00016ED8 File Offset: 0x000150D8
	public GameCenterScore()
	{
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x00016EE0 File Offset: 0x000150E0
	public GameCenterScore(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("category"))
		{
			this.category = (dict["category"] as string);
		}
		if (dict.ContainsKey("formattedValue"))
		{
			this.formattedValue = (dict["formattedValue"] as string);
		}
		if (dict.ContainsKey("value"))
		{
			this.value = long.Parse(dict["value"].ToString());
		}
		if (dict.ContainsKey("context"))
		{
			this.context = ulong.Parse(dict["context"].ToString());
		}
		if (dict.ContainsKey("playerId"))
		{
			this.playerId = (dict["playerId"] as string);
		}
		if (dict.ContainsKey("rank"))
		{
			this.rank = int.Parse(dict["rank"].ToString());
		}
		if (dict.ContainsKey("isFriend"))
		{
			this.isFriend = (bool)dict["isFriend"];
		}
		if (dict.ContainsKey("alias"))
		{
			this.alias = (dict["alias"] as string);
		}
		else
		{
			this.alias = "Anonymous";
		}
		if (dict.ContainsKey("maxRange"))
		{
			this.maxRange = int.Parse(dict["maxRange"].ToString());
		}
		if (dict.ContainsKey("date"))
		{
			this.rawDate = long.Parse(dict["date"].ToString());
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000655 RID: 1621 RVA: 0x00017098 File Offset: 0x00015298
	public DateTime date
	{
		get
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return dateTime.AddSeconds((double)this.rawDate);
		}
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x000170C8 File Offset: 0x000152C8
	public override string ToString()
	{
		return string.Format("<Score> category: {0}, formattedValue: {1}, date: {2}, rank: {3}, alias: {4}, maxRange: {5}, value: {6}, context: {7}", new object[]
		{
			this.category,
			this.formattedValue,
			this.date,
			this.rank,
			this.alias,
			this.maxRange,
			this.value,
			this.context
		});
	}

	// Token: 0x0400037A RID: 890
	public string category;

	// Token: 0x0400037B RID: 891
	public string formattedValue;

	// Token: 0x0400037C RID: 892
	public long value;

	// Token: 0x0400037D RID: 893
	public ulong context;

	// Token: 0x0400037E RID: 894
	public long rawDate;

	// Token: 0x0400037F RID: 895
	public string playerId;

	// Token: 0x04000380 RID: 896
	public int rank;

	// Token: 0x04000381 RID: 897
	public bool isFriend;

	// Token: 0x04000382 RID: 898
	public string alias;

	// Token: 0x04000383 RID: 899
	public int maxRange;
}
