using System;
using System.Collections.Generic;
using Prime31;

// Token: 0x02000094 RID: 148
public class GameCenterAchievement
{
	// Token: 0x06000590 RID: 1424 RVA: 0x00015624 File Offset: 0x00013824
	public GameCenterAchievement(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("identifier"))
		{
			this.identifier = (dict["identifier"] as string);
		}
		if (dict.ContainsKey("hidden"))
		{
			this.isHidden = (bool)dict["hidden"];
		}
		if (dict.ContainsKey("completed"))
		{
			this.completed = (bool)dict["completed"];
		}
		if (dict.ContainsKey("percentComplete"))
		{
			this.percentComplete = float.Parse(dict["percentComplete"].ToString());
		}
		if (dict.ContainsKey("lastReportedDate"))
		{
			double value = double.Parse(dict["lastReportedDate"].ToString());
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			this.lastReportedDate = dateTime.AddSeconds(value);
		}
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0001571C File Offset: 0x0001391C
	public static List<GameCenterAchievement> fromJSON(string json)
	{
		List<GameCenterAchievement> list = new List<GameCenterAchievement>();
		List<object> list2 = json.listFromJson();
		foreach (object obj in list2)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)obj;
			list.Add(new GameCenterAchievement(dict));
		}
		return list;
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00015798 File Offset: 0x00013998
	public override string ToString()
	{
		return string.Format("<Achievement> identifier: {0}, hidden: {1}, completed: {2}, percentComplete: {3}, lastReported: {4}", new object[]
		{
			this.identifier,
			this.isHidden,
			this.completed,
			this.percentComplete,
			this.lastReportedDate
		});
	}

	// Token: 0x04000316 RID: 790
	public string identifier;

	// Token: 0x04000317 RID: 791
	public bool isHidden;

	// Token: 0x04000318 RID: 792
	public bool completed;

	// Token: 0x04000319 RID: 793
	public DateTime lastReportedDate;

	// Token: 0x0400031A RID: 794
	public float percentComplete;
}
