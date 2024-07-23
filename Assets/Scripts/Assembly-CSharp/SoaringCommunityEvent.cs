using System;

// Token: 0x02000348 RID: 840
public class SoaringCommunityEvent
{
	// Token: 0x06001821 RID: 6177 RVA: 0x0009FFE0 File Offset: 0x0009E1E0
	public SoaringCommunityEvent(string sEventID, SoaringDictionary pData)
	{
		this.m_pCommunityRewards = new SoaringCommunityEvent.Reward[0];
		this.m_pIndividualRewards = new SoaringCommunityEvent.Reward[0];
		this.SetData(sEventID, pData);
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001822 RID: 6178 RVA: 0x000A0014 File Offset: 0x0009E214
	// (set) Token: 0x06001823 RID: 6179 RVA: 0x000A001C File Offset: 0x0009E21C
	public string m_sID { get; private set; }

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001824 RID: 6180 RVA: 0x000A0028 File Offset: 0x0009E228
	// (set) Token: 0x06001825 RID: 6181 RVA: 0x000A0030 File Offset: 0x0009E230
	public int m_nValue { get; private set; }

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06001826 RID: 6182 RVA: 0x000A003C File Offset: 0x0009E23C
	// (set) Token: 0x06001827 RID: 6183 RVA: 0x000A0044 File Offset: 0x0009E244
	public int m_nCommunityValue { get; private set; }

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06001828 RID: 6184 RVA: 0x000A0050 File Offset: 0x0009E250
	// (set) Token: 0x06001829 RID: 6185 RVA: 0x000A0064 File Offset: 0x0009E264
	public SoaringCommunityEvent.Reward[] CommunityRewards
	{
		get
		{
			return (SoaringCommunityEvent.Reward[])this.m_pCommunityRewards.Clone();
		}
		private set
		{
			this.m_pCommunityRewards = value;
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x0600182A RID: 6186 RVA: 0x000A0070 File Offset: 0x0009E270
	// (set) Token: 0x0600182B RID: 6187 RVA: 0x000A0084 File Offset: 0x0009E284
	public SoaringCommunityEvent.Reward[] IndividualRewards
	{
		get
		{
			return (SoaringCommunityEvent.Reward[])this.m_pIndividualRewards.Clone();
		}
		private set
		{
			this.m_pIndividualRewards = value;
		}
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x000A0090 File Offset: 0x0009E290
	public SoaringCommunityEvent.Reward GetReward(int nID)
	{
		SoaringCommunityEvent.Reward[] array = this.m_pCommunityRewards;
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			if (array[i].m_nID == nID)
			{
				return array[i];
			}
		}
		array = this.m_pIndividualRewards;
		num = array.Length;
		for (int j = 0; j < num; j++)
		{
			if (array[j].m_nID == nID)
			{
				return array[j];
			}
		}
		return null;
	}

	// Token: 0x0600182D RID: 6189 RVA: 0x000A00FC File Offset: 0x0009E2FC
	public void SetData(string sEventID, SoaringDictionary pData)
	{
		if (pData == null)
		{
			return;
		}
		this.m_sID = sEventID;
		this.m_nValue = pData.soaringValue("value");
		this.m_nCommunityValue = pData.soaringValue("communityValue");
		SoaringArray soaringArray = (SoaringArray)pData.objectWithKey("communityGifts");
		int num = soaringArray.count();
		SoaringCommunityEvent.Reward[] array = new SoaringCommunityEvent.Reward[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = new SoaringCommunityEvent.Reward((SoaringDictionary)soaringArray.objectAtIndex(i));
		}
		this.m_pCommunityRewards = array;
		soaringArray = (SoaringArray)pData.objectWithKey("individualGifts");
		num = soaringArray.count();
		array = new SoaringCommunityEvent.Reward[num];
		for (int j = 0; j < num; j++)
		{
			array[j] = new SoaringCommunityEvent.Reward((SoaringDictionary)soaringArray.objectAtIndex(j));
		}
		this.m_pIndividualRewards = array;
	}

	// Token: 0x0400100B RID: 4107
	private SoaringCommunityEvent.Reward[] m_pCommunityRewards;

	// Token: 0x0400100C RID: 4108
	private SoaringCommunityEvent.Reward[] m_pIndividualRewards;

	// Token: 0x02000349 RID: 841
	public class Reward
	{
		// Token: 0x0600182E RID: 6190 RVA: 0x000A01E4 File Offset: 0x0009E3E4
		public Reward(SoaringDictionary pData)
		{
			this.SetData(pData);
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x0600182F RID: 6191 RVA: 0x000A01F4 File Offset: 0x0009E3F4
		// (set) Token: 0x06001830 RID: 6192 RVA: 0x000A01FC File Offset: 0x0009E3FC
		public int m_nID { get; private set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001831 RID: 6193 RVA: 0x000A0208 File Offset: 0x0009E408
		// (set) Token: 0x06001832 RID: 6194 RVA: 0x000A0210 File Offset: 0x0009E410
		public int m_nValue { get; private set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001833 RID: 6195 RVA: 0x000A021C File Offset: 0x0009E41C
		// (set) Token: 0x06001834 RID: 6196 RVA: 0x000A0224 File Offset: 0x0009E424
		public bool m_bUnlocked { get; private set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001835 RID: 6197 RVA: 0x000A0230 File Offset: 0x0009E430
		// (set) Token: 0x06001836 RID: 6198 RVA: 0x000A0238 File Offset: 0x0009E438
		public bool m_bAcquired { get; private set; }

		// Token: 0x06001837 RID: 6199 RVA: 0x000A0244 File Offset: 0x0009E444
		public void SetData(SoaringDictionary pData)
		{
			this.m_nID = int.Parse(pData.soaringValue("giftDid"));
			this.m_nValue = pData.soaringValue("valueNeeded");
			this.m_bUnlocked = pData.soaringValue("unlocked");
			this.m_bAcquired = pData.soaringValue("acquired");
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x000A02B0 File Offset: 0x0009E4B0
		public void _SetAquired(bool bAquired)
		{
			this.m_bAcquired = bAquired;
		}
	}
}
