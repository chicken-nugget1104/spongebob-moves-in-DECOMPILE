using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000046 RID: 70
public class AGSLeaderboard
{
	// Token: 0x0600025A RID: 602 RVA: 0x0000BCFC File Offset: 0x00009EFC
	public static AGSLeaderboard fromHashtable(Hashtable ht)
	{
		AGSLeaderboard agsleaderboard = new AGSLeaderboard();
		agsleaderboard.name = ht["name"].ToString();
		agsleaderboard.id = ht["id"].ToString();
		agsleaderboard.displayText = ht["displayText"].ToString();
		agsleaderboard.scoreFormat = ht["scoreFormat"].ToString();
		if (ht.ContainsKey("scores") && ht["scores"] is ArrayList)
		{
			ArrayList list = ht["scores"] as ArrayList;
			agsleaderboard.scores = AGSScore.fromArrayList(list);
		}
		return agsleaderboard;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x0000BDAC File Offset: 0x00009FAC
	public override string ToString()
	{
		return string.Format("name: {0}, id: {1}, displayText: {2}, scoreFormat: {3}", new object[]
		{
			this.name,
			this.id,
			this.displayText,
			this.scoreFormat
		});
	}

	// Token: 0x04000192 RID: 402
	public string name;

	// Token: 0x04000193 RID: 403
	public string id;

	// Token: 0x04000194 RID: 404
	public string displayText;

	// Token: 0x04000195 RID: 405
	public string scoreFormat;

	// Token: 0x04000196 RID: 406
	public List<AGSScore> scores = new List<AGSScore>();
}
