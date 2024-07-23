using System;
using System.Collections.Generic;

// Token: 0x0200005E RID: 94
public class Config
{
	// Token: 0x040001CD RID: 461
	public const string CRAFTING_PATH_SA = "Crafting";

	// Token: 0x040001CE RID: 462
	public const string DIALOG_PACKAGES_PATH_SA = "Dialogs";

	// Token: 0x040001CF RID: 463
	public const string FEATURE_DATA_PATH_SA = "Features";

	// Token: 0x040001D0 RID: 464
	public const string MOVIE_PATH_SA = "Video";

	// Token: 0x040001D1 RID: 465
	public const string NOTIFICATIONS_PATH_SA = "Notifications";

	// Token: 0x040001D2 RID: 466
	public const string QUESTS_PATH_SA = "Quests";

	// Token: 0x040001D3 RID: 467
	public const string BONUS_PAYTABLES_SA = "BonusPaytables";

	// Token: 0x040001D4 RID: 468
	public const string TASKS_PATH_SA = "Tasks";

	// Token: 0x040001D5 RID: 469
	public const string TREASURE_PATH_SA = "Treasure";

	// Token: 0x040001D6 RID: 470
	public const string VENDING_PATH_SA = "Vending";

	// Token: 0x040001D7 RID: 471
	public const string BLUEPRINT_DIRECTORY_PATH_SA = "Blueprints";

	// Token: 0x040001D8 RID: 472
	public const string TERRAIN_PATH_SA = "Terrain";

	// Token: 0x040001D9 RID: 473
	public static string[] SA_FILES = new string[]
	{
		"Features",
		"Video",
		"Quests",
		"Tasks",
		"Blueprints",
		"Terrain"
	};

	// Token: 0x040001DA RID: 474
	public static string[] CRAFTING_PATH;

	// Token: 0x040001DB RID: 475
	public static string[] DIALOG_PACKAGES_PATH;

	// Token: 0x040001DC RID: 476
	public static string[] FEATURE_DATA_PATH;

	// Token: 0x040001DD RID: 477
	public static string[] MOVIE_PATH;

	// Token: 0x040001DE RID: 478
	public static string[] NOTIFICATIONS_PATH;

	// Token: 0x040001DF RID: 479
	public static string[] QUESTS_PATH;

	// Token: 0x040001E0 RID: 480
	public static string[] BONUS_PAYTABLES;

	// Token: 0x040001E1 RID: 481
	public static string[] TASKS_PATH;

	// Token: 0x040001E2 RID: 482
	public static string[] BLUEPRINT_DIRECTORY_PATH;

	// Token: 0x040001E3 RID: 483
	public static string[] TERRAIN_PATH;

	// Token: 0x040001E4 RID: 484
	public static string[] TREASURE_PATH;

	// Token: 0x040001E5 RID: 485
	public static string[] VENDING_PATH;

	// Token: 0x040001E6 RID: 486
	public static Dictionary<string, string> ACHIEVEMENT_ID_DIC_AMAZON = new Dictionary<string, string>
	{
		{
			"SB_Ach_Squid",
			"SBAM_Ach_Squid"
		},
		{
			"SB_Ach_Gene",
			"SBAM_Ach_Gene"
		},
		{
			"SB_Ach_Pearl",
			"SBAM_Ach_Pearl"
		},
		{
			"SB_Ach_Puff",
			"SBAM_Ach_Puff"
		},
		{
			"SB_Ach_Sandy",
			"SBAM_Ach_Sandy"
		},
		{
			"SB_Ach_Barn",
			"SBAM_Ach_Barn"
		},
		{
			"SB_Ach_Larry",
			"SBAM_Ach_Larry"
		},
		{
			"SB_Ach_ManR",
			"SBAM_Ach_ManR"
		},
		{
			"SB_Ach_Squill",
			"SBAM_Ach_Squill"
		},
		{
			"SB_Ach_Reg",
			"SBAM_Ach_Reg"
		},
		{
			"SB_Ach_Pat",
			"SBAM_Ach_Pat"
		},
		{
			"SB_Ach_Mer",
			"SBAM_Ach_Mer"
		},
		{
			"SB_Ach_Kev",
			"SBAM_Ach_Kev"
		},
		{
			"SB_Ach_King",
			"SBAM_Ach_King"
		}
	};

	// Token: 0x040001E7 RID: 487
	public static Dictionary<string, string> ACHIEVEMENT_ID_DIC_GOOGLE = new Dictionary<string, string>
	{
		{
			"SB_Ach_Squid",
			"CgkI4ujSoJMCEAIQAQ"
		},
		{
			"SB_Ach_Gene",
			"CgkI4ujSoJMCEAIQAg"
		},
		{
			"SB_Ach_Pearl",
			"CgkI4ujSoJMCEAIQAw"
		},
		{
			"SB_Ach_Puff",
			"CgkI4ujSoJMCEAIQBA"
		},
		{
			"SB_Ach_Sandy",
			"CgkI4ujSoJMCEAIQBQ"
		},
		{
			"SB_Ach_Barn",
			"CgkI4ujSoJMCEAIQBg"
		},
		{
			"SB_Ach_Larry",
			"CgkI4ujSoJMCEAIQBw"
		},
		{
			"SB_Ach_ManR",
			"CgkI4ujSoJMCEAIQCA"
		},
		{
			"SB_Ach_Squill",
			"CgkI4ujSoJMCEAIQCQ"
		},
		{
			"SB_Ach_Reg",
			"CgkI4ujSoJMCEAIQCg"
		},
		{
			"SB_Ach_Pat",
			"CgkI4ujSoJMCEAIQCw"
		},
		{
			"SB_Ach_Mer",
			"CgkI4ujSoJMCEAIQDA"
		},
		{
			"SB_Ach_Kev",
			"CgkI4ujSoJMCEAIQDQ"
		},
		{
			"SB_Ach_King",
			"CgkI4ujSoJMCEAIQDg"
		}
	};

	// Token: 0x040001E8 RID: 488
	public static Dictionary<string, string> IAP_ID_DIC_AMAZON = new Dictionary<string, string>
	{
		{
			"com.mtvn.SBMI.jellybundle1",
			"com.mtvn.sbmi.amazonjellybundle1"
		},
		{
			"com.mtvn.SBMI.jellybundle2",
			"com.mtvn.sbmi.amazonjellybundle2"
		},
		{
			"com.mtvn.SBMI.jellybundle3",
			"com.mtvn.sbmi.amazonjellybundle3"
		},
		{
			"com.mtvn.SBMI.jellybundle4",
			"com.mtvn.sbmi.amazonjellybundle4"
		},
		{
			"com.mtvn.SBMI.jellybundle5",
			"com.mtvn.sbmi.amazonjellybundle5"
		},
		{
			"com.mtvn.SBMI.jellybundle6",
			"com.mtvn.sbmi.amazonjellybundle6"
		},
		{
			"com.mtvn.SBMI.bag1",
			"com.mtvn.sbmi.amazonbag1"
		},
		{
			"com.mtvn.SBMI.bag2",
			"com.mtvn.sbmi.amazonbag2"
		},
		{
			"com.mtvn.SBMI.bag3",
			"com.mtvn.sbmi.amazonbag3"
		},
		{
			"com.mtvn.SBMI.bag4",
			"com.mtvn.sbmi.amazonbag4"
		}
	};

	// Token: 0x040001E9 RID: 489
	public static Dictionary<string, string> IAP_ID_DIC_GOOGLE = new Dictionary<string, string>
	{
		{
			"com.mtvn.SBMI.jellybundle1",
			"com.mtvn.sbmi.gplayjellybundle1"
		},
		{
			"com.mtvn.SBMI.jellybundle2",
			"com.mtvn.sbmi.gplayjellybundle2"
		},
		{
			"com.mtvn.SBMI.jellybundle3",
			"com.mtvn.sbmi.gplayjellybundle3"
		},
		{
			"com.mtvn.SBMI.jellybundle4",
			"com.mtvn.sbmi.gplayjellybundle4"
		},
		{
			"com.mtvn.SBMI.jellybundle5",
			"com.mtvn.sbmi.gplayjellybundle5"
		},
		{
			"com.mtvn.SBMI.jellybundle6",
			"com.mtvn.sbmi.gplayjellybundle6"
		},
		{
			"com.mtvn.SBMI.bag1",
			"com.mtvn.sbmi.gplaybag1"
		},
		{
			"com.mtvn.SBMI.bag2",
			"com.mtvn.sbmi.gplaybag2"
		},
		{
			"com.mtvn.SBMI.bag3",
			"com.mtvn.sbmi.gplaybag3"
		},
		{
			"com.mtvn.SBMI.bag4",
			"com.mtvn.sbmi.gplaybag4"
		}
	};
}
