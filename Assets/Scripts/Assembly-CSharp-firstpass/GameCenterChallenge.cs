using System;
using System.Collections.Generic;
using Prime31;

// Token: 0x0200009A RID: 154
public class GameCenterChallenge
{
	// Token: 0x060005AF RID: 1455 RVA: 0x00015AD0 File Offset: 0x00013CD0
	public GameCenterChallenge(Dictionary<string, object> dict)
	{
		if (dict.ContainsKey("issuingPlayerID"))
		{
			this.issuingPlayerID = (dict["issuingPlayerID"] as string);
		}
		if (dict.ContainsKey("receivingPlayerID"))
		{
			this.receivingPlayerID = (dict["receivingPlayerID"] as string);
		}
		if (dict.ContainsKey("state"))
		{
			int num = int.Parse(dict["state"].ToString());
			this.state = (GameCenterChallengeState)num;
		}
		if (dict.ContainsKey("issueDate"))
		{
			double value = double.Parse(dict["issueDate"].ToString());
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			this.issueDate = dateTime.AddSeconds(value);
		}
		if (dict.ContainsKey("completionDate"))
		{
			double value2 = double.Parse(dict["completionDate"].ToString());
			DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			this.completionDate = dateTime2.AddSeconds(value2);
		}
		if (dict.ContainsKey("message"))
		{
			this.message = (dict["message"] as string);
		}
		if (dict.ContainsKey("score"))
		{
			this.score = new GameCenterScore(dict["score"] as Dictionary<string, object>);
		}
		if (dict.ContainsKey("achievement"))
		{
			this.achievement = new GameCenterAchievement(dict["achievement"] as Dictionary<string, object>);
		}
		if (dict.ContainsKey("hash"))
		{
			this.hash = uint.Parse(dict["hash"].ToString());
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00015C90 File Offset: 0x00013E90
	public static List<GameCenterChallenge> fromJson(string json)
	{
		List<object> list = json.listFromJson();
		List<GameCenterChallenge> list2 = new List<GameCenterChallenge>();
		foreach (object obj in list)
		{
			Dictionary<string, object> dict = (Dictionary<string, object>)obj;
			list2.Add(new GameCenterChallenge(dict));
		}
		return list2;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00015D0C File Offset: 0x00013F0C
	public override string ToString()
	{
		return string.Format("<Challenge> issuingPlayerID: {0}, receivingPlayerID: {1}, message: {2}, state: {3}, score: {4}, achievement: {5}, hash: {6}", new object[]
		{
			this.issuingPlayerID,
			this.receivingPlayerID,
			this.message,
			this.state,
			this.score,
			this.achievement,
			this.hash
		});
	}

	// Token: 0x0400032F RID: 815
	public string issuingPlayerID;

	// Token: 0x04000330 RID: 816
	public string receivingPlayerID;

	// Token: 0x04000331 RID: 817
	public GameCenterChallengeState state;

	// Token: 0x04000332 RID: 818
	public DateTime issueDate;

	// Token: 0x04000333 RID: 819
	public DateTime completionDate;

	// Token: 0x04000334 RID: 820
	public string message;

	// Token: 0x04000335 RID: 821
	public uint hash;

	// Token: 0x04000336 RID: 822
	public GameCenterScore score;

	// Token: 0x04000337 RID: 823
	public GameCenterAchievement achievement;
}
