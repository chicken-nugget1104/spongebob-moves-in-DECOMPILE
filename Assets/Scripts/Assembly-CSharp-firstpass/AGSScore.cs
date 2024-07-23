using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000047 RID: 71
public class AGSScore
{
	// Token: 0x0600025D RID: 605 RVA: 0x0000BDF8 File Offset: 0x00009FF8
	public static AGSScore fromHashtable(Hashtable ht)
	{
		return new AGSScore
		{
			playerAlias = ht["playerAlias"].ToString(),
			rank = int.Parse(ht["rank"].ToString()),
			scoreString = ht["scoreString"].ToString(),
			scoreValue = long.Parse(ht["scoreValue"].ToString())
		};
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0000BE70 File Offset: 0x0000A070
	public static List<AGSScore> fromArrayList(ArrayList list)
	{
		List<AGSScore> list2 = new List<AGSScore>();
		foreach (object obj in list)
		{
			Hashtable ht = (Hashtable)obj;
			list2.Add(AGSScore.fromHashtable(ht));
		}
		return list2;
	}

	// Token: 0x0600025F RID: 607 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
	public override string ToString()
	{
		return string.Format("playerAlias: {0}, rank: {1}, scoreString: {2}", this.playerAlias, this.rank, this.scoreString);
	}

	// Token: 0x04000197 RID: 407
	public string playerAlias;

	// Token: 0x04000198 RID: 408
	public int rank;

	// Token: 0x04000199 RID: 409
	public string scoreString;

	// Token: 0x0400019A RID: 410
	public long scoreValue;
}
