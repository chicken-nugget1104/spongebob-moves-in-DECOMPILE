using System;
using System.Collections.Generic;

// Token: 0x020001AB RID: 427
public class AutoQuest
{
	// Token: 0x06000E45 RID: 3653 RVA: 0x00056F78 File Offset: 0x00055178
	public AutoQuest(int nDID, int nCharacterDID, Dictionary<int, int> pRecipeDIDs, int nGoldReward, int nXPReward, string sName, string sDescription)
	{
		this.m_nDID = nDID;
		this.m_nCharacterDID = nCharacterDID;
		this.m_pRecipes = pRecipeDIDs;
		this.m_nGoldReward = nGoldReward;
		this.m_nXPReward = nXPReward;
		this.m_sName = sName;
		this.m_sDescription = sDescription;
	}

	// Token: 0x06000E46 RID: 3654 RVA: 0x00056FB8 File Offset: 0x000551B8
	public override string ToString()
	{
		string text = string.Concat(new string[]
		{
			"AutoQuest | did: ",
			this.m_nDID.ToString(),
			" characterDID: ",
			this.m_nCharacterDID.ToString(),
			" m_nRecipes: "
		});
		string text2;
		foreach (KeyValuePair<int, int> keyValuePair in this.m_pRecipes)
		{
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"{ did: ",
				keyValuePair.Key,
				", count: ",
				keyValuePair.Value,
				" },"
			});
		}
		text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			" goldReward: ",
			this.m_nGoldReward.ToString(),
			" xpReward: ",
			this.m_nXPReward.ToString(),
			" name: ",
			this.m_sName,
			" description: ",
			this.m_sDescription
		});
		return text;
	}

	// Token: 0x04000965 RID: 2405
	public int m_nDID;

	// Token: 0x04000966 RID: 2406
	public int m_nCharacterDID;

	// Token: 0x04000967 RID: 2407
	public int m_nGoldReward;

	// Token: 0x04000968 RID: 2408
	public int m_nXPReward;

	// Token: 0x04000969 RID: 2409
	public string m_sName;

	// Token: 0x0400096A RID: 2410
	public string m_sDescription;

	// Token: 0x0400096B RID: 2411
	public Dictionary<int, int> m_pRecipes;
}
