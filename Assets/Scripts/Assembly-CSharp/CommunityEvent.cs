using System;
using System.Collections.Generic;
using MTools;

// Token: 0x0200011C RID: 284
public class CommunityEvent
{
	// Token: 0x06000A54 RID: 2644 RVA: 0x00040E58 File Offset: 0x0003F058
	public CommunityEvent(Dictionary<string, object> pData)
	{
		this.m_sID = TFUtils.LoadUint(pData, "did").ToString();
		this.m_sName = TFUtils.LoadString(pData, "name");
		this.m_nValueID = TFUtils.LoadInt(pData, "value_did");
		this.m_nQuestPrereqID = TFUtils.LoadInt(pData, "prerequisite_quest");
		this.m_pStartDate = DateTime.Parse(TFUtils.LoadString(pData, "start_date"));
		this.m_pEndDate = DateTime.Parse(TFUtils.LoadString(pData, "end_date"));
		this.m_sEventButtonTexture = TFUtils.LoadString(pData, "event_button_texture");
		this.m_sTabOneTexture = TFUtils.LoadString(pData, "tab_one_texture");
		this.m_sTabTwoTexture = TFUtils.LoadString(pData, "tab_two_texture");
		this.m_sLeftBannerTexture = TFUtils.LoadString(pData, "left_banner_texture");
		this.m_sRightBannerTexture = TFUtils.LoadString(pData, "right_banner_texture");
		this.m_sRightBannerTitle = TFUtils.LoadString(pData, "right_banner_title");
		this.m_sRightBannerDescription = TFUtils.LoadString(pData, "right_banner_description");
		this.m_sIndividualFooterText = TFUtils.LoadString(pData, "individual_footer_text");
		this.m_sCommunityHeaderText = TFUtils.LoadString(pData, "community_header_text");
		this.m_sCommunityFooterText = TFUtils.LoadString(pData, "community_footer_text");
		this.m_sCommunityFooterAllUnlocksText = TFUtils.LoadString(pData, "community_footer_all_unlocks_text");
		this.m_sCommunityFooterTexture = TFUtils.LoadString(pData, "community_footer_texture");
		this.m_sQuestIcon = TFUtils.LoadString(pData, "quest_icon");
		this.m_bHideUI = TFUtils.LoadBool(pData, "hide_ui");
		this.m_bActive = false;
		this.m_pRewards = new MDictionary();
		if (pData.ContainsKey("rewards"))
		{
			List<object> list = TFUtils.LoadList<object>(pData, "rewards");
			foreach (object obj in list)
			{
				Dictionary<string, object> pData2 = (Dictionary<string, object>)obj;
				CommunityEvent.Reward reward = new CommunityEvent.Reward(pData2);
				this.m_pRewards.addValue(reward, reward.m_nID.ToString());
			}
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000A55 RID: 2645 RVA: 0x00041074 File Offset: 0x0003F274
	// (set) Token: 0x06000A56 RID: 2646 RVA: 0x0004107C File Offset: 0x0003F27C
	public string m_sID { get; private set; }

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000A57 RID: 2647 RVA: 0x00041088 File Offset: 0x0003F288
	// (set) Token: 0x06000A58 RID: 2648 RVA: 0x00041090 File Offset: 0x0003F290
	public string m_sName { get; private set; }

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0004109C File Offset: 0x0003F29C
	// (set) Token: 0x06000A5A RID: 2650 RVA: 0x000410A4 File Offset: 0x0003F2A4
	public bool m_bActive { get; private set; }

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000A5B RID: 2651 RVA: 0x000410B0 File Offset: 0x0003F2B0
	// (set) Token: 0x06000A5C RID: 2652 RVA: 0x000410B8 File Offset: 0x0003F2B8
	public bool m_bHideUI { get; private set; }

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000A5D RID: 2653 RVA: 0x000410C4 File Offset: 0x0003F2C4
	// (set) Token: 0x06000A5E RID: 2654 RVA: 0x000410CC File Offset: 0x0003F2CC
	public int m_nValueID { get; private set; }

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000A5F RID: 2655 RVA: 0x000410D8 File Offset: 0x0003F2D8
	// (set) Token: 0x06000A60 RID: 2656 RVA: 0x000410E0 File Offset: 0x0003F2E0
	public int m_nQuestPrereqID { get; private set; }

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x000410EC File Offset: 0x0003F2EC
	// (set) Token: 0x06000A62 RID: 2658 RVA: 0x000410F4 File Offset: 0x0003F2F4
	public DateTime m_pStartDate { get; private set; }

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000A63 RID: 2659 RVA: 0x00041100 File Offset: 0x0003F300
	// (set) Token: 0x06000A64 RID: 2660 RVA: 0x00041108 File Offset: 0x0003F308
	public DateTime m_pEndDate { get; private set; }

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000A65 RID: 2661 RVA: 0x00041114 File Offset: 0x0003F314
	// (set) Token: 0x06000A66 RID: 2662 RVA: 0x0004111C File Offset: 0x0003F31C
	public string m_sEventButtonTexture { get; private set; }

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000A67 RID: 2663 RVA: 0x00041128 File Offset: 0x0003F328
	// (set) Token: 0x06000A68 RID: 2664 RVA: 0x00041130 File Offset: 0x0003F330
	public string m_sTabOneTexture { get; private set; }

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0004113C File Offset: 0x0003F33C
	// (set) Token: 0x06000A6A RID: 2666 RVA: 0x00041144 File Offset: 0x0003F344
	public string m_sTabTwoTexture { get; private set; }

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000A6B RID: 2667 RVA: 0x00041150 File Offset: 0x0003F350
	// (set) Token: 0x06000A6C RID: 2668 RVA: 0x00041158 File Offset: 0x0003F358
	public string m_sLeftBannerTexture { get; private set; }

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00041164 File Offset: 0x0003F364
	// (set) Token: 0x06000A6E RID: 2670 RVA: 0x0004116C File Offset: 0x0003F36C
	public string m_sRightBannerTexture { get; private set; }

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000A6F RID: 2671 RVA: 0x00041178 File Offset: 0x0003F378
	// (set) Token: 0x06000A70 RID: 2672 RVA: 0x00041180 File Offset: 0x0003F380
	public string m_sRightBannerTitle { get; private set; }

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0004118C File Offset: 0x0003F38C
	// (set) Token: 0x06000A72 RID: 2674 RVA: 0x00041194 File Offset: 0x0003F394
	public string m_sRightBannerDescription { get; private set; }

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000A73 RID: 2675 RVA: 0x000411A0 File Offset: 0x0003F3A0
	// (set) Token: 0x06000A74 RID: 2676 RVA: 0x000411A8 File Offset: 0x0003F3A8
	public string m_sIndividualFooterText { get; private set; }

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000A75 RID: 2677 RVA: 0x000411B4 File Offset: 0x0003F3B4
	// (set) Token: 0x06000A76 RID: 2678 RVA: 0x000411BC File Offset: 0x0003F3BC
	public string m_sCommunityHeaderText { get; private set; }

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000A77 RID: 2679 RVA: 0x000411C8 File Offset: 0x0003F3C8
	// (set) Token: 0x06000A78 RID: 2680 RVA: 0x000411D0 File Offset: 0x0003F3D0
	public string m_sCommunityFooterText { get; private set; }

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000A79 RID: 2681 RVA: 0x000411DC File Offset: 0x0003F3DC
	// (set) Token: 0x06000A7A RID: 2682 RVA: 0x000411E4 File Offset: 0x0003F3E4
	public string m_sCommunityFooterAllUnlocksText { get; private set; }

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000A7B RID: 2683 RVA: 0x000411F0 File Offset: 0x0003F3F0
	// (set) Token: 0x06000A7C RID: 2684 RVA: 0x000411F8 File Offset: 0x0003F3F8
	public string m_sCommunityFooterTexture { get; private set; }

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000A7D RID: 2685 RVA: 0x00041204 File Offset: 0x0003F404
	// (set) Token: 0x06000A7E RID: 2686 RVA: 0x0004120C File Offset: 0x0003F40C
	public string m_sQuestIcon { get; private set; }

	// Token: 0x06000A7F RID: 2687 RVA: 0x00041218 File Offset: 0x0003F418
	public void SetActive(bool bActive)
	{
		this.m_bActive = bActive;
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x00041224 File Offset: 0x0003F424
	public CommunityEvent.Reward GetReward(int nID)
	{
		return this.GetReward(nID.ToString());
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00041234 File Offset: 0x0003F434
	public CommunityEvent.Reward GetReward(string sID)
	{
		if (this.m_pRewards.containsKey(sID))
		{
			return (CommunityEvent.Reward)this.m_pRewards.objectWithKey(sID);
		}
		return null;
	}

	// Token: 0x0400070E RID: 1806
	private MDictionary m_pRewards;

	// Token: 0x0200011D RID: 285
	public class Reward
	{
		// Token: 0x06000A82 RID: 2690 RVA: 0x00041268 File Offset: 0x0003F468
		public Reward(Dictionary<string, object> pData)
		{
			this.m_nID = TFUtils.LoadInt(pData, "did");
			this.m_nWidth = TFUtils.LoadInt(pData, "width");
			this.m_nHeight = TFUtils.LoadInt(pData, "height");
			this.m_nDialogSequenceID = TFUtils.LoadInt(pData, "dialog_sequence");
			this.m_nAutoPlaceX = TFUtils.LoadInt(pData, "auto_place_x");
			this.m_nAutoPlaceY = TFUtils.LoadInt(pData, "auto_place_y");
			this.m_sTexture = TFUtils.LoadString(pData, "texture");
			this.m_sType = TFUtils.LoadString(pData, "entity_type");
			this.m_sLockedTexture = TFUtils.LoadString(pData, "locked_texture");
			this.m_bHideNameWhenLocked = TFUtils.LoadBool(pData, "hide_name_when_locked");
			List<int> list = TFUtils.LoadList<int>(pData, "land_dids");
			int count = list.Count;
			this.m_pLandIDs = new int[count];
			for (int i = 0; i < count; i++)
			{
				this.m_pLandIDs[i] = list[i];
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x00041368 File Offset: 0x0003F568
		// (set) Token: 0x06000A84 RID: 2692 RVA: 0x00041370 File Offset: 0x0003F570
		public int m_nID { get; private set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0004137C File Offset: 0x0003F57C
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x00041384 File Offset: 0x0003F584
		public string m_sTexture { get; private set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x00041390 File Offset: 0x0003F590
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x00041398 File Offset: 0x0003F598
		public int m_nWidth { get; private set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x000413A4 File Offset: 0x0003F5A4
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x000413AC File Offset: 0x0003F5AC
		public int m_nHeight { get; private set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x000413B8 File Offset: 0x0003F5B8
		// (set) Token: 0x06000A8C RID: 2700 RVA: 0x000413C0 File Offset: 0x0003F5C0
		public int m_nDialogSequenceID { get; private set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x000413CC File Offset: 0x0003F5CC
		// (set) Token: 0x06000A8E RID: 2702 RVA: 0x000413D4 File Offset: 0x0003F5D4
		public int m_nAutoPlaceX { get; private set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x000413E0 File Offset: 0x0003F5E0
		// (set) Token: 0x06000A90 RID: 2704 RVA: 0x000413E8 File Offset: 0x0003F5E8
		public int m_nAutoPlaceY { get; private set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x000413F4 File Offset: 0x0003F5F4
		// (set) Token: 0x06000A92 RID: 2706 RVA: 0x000413FC File Offset: 0x0003F5FC
		public string m_sType { get; private set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x00041408 File Offset: 0x0003F608
		// (set) Token: 0x06000A94 RID: 2708 RVA: 0x00041410 File Offset: 0x0003F610
		public string m_sLockedTexture { get; private set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x0004141C File Offset: 0x0003F61C
		// (set) Token: 0x06000A96 RID: 2710 RVA: 0x00041424 File Offset: 0x0003F624
		public bool m_bHideNameWhenLocked { get; private set; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x00041430 File Offset: 0x0003F630
		// (set) Token: 0x06000A98 RID: 2712 RVA: 0x00041444 File Offset: 0x0003F644
		public int[] LandIDs
		{
			get
			{
				return (int[])this.m_pLandIDs.Clone();
			}
			private set
			{
				this.m_pLandIDs = value;
			}
		}

		// Token: 0x04000724 RID: 1828
		private int[] m_pLandIDs;
	}
}
