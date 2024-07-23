using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002D RID: 45
[Serializable]
public class HelpshiftConfig : ScriptableObject
{
	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060001C7 RID: 455 RVA: 0x000093BC File Offset: 0x000075BC
	public static HelpshiftConfig Instance
	{
		get
		{
			HelpshiftConfig.instance = (Resources.Load("HelpshiftConfig") as HelpshiftConfig);
			if (HelpshiftConfig.instance == null)
			{
				HelpshiftConfig.instance = ScriptableObject.CreateInstance<HelpshiftConfig>();
			}
			return HelpshiftConfig.instance;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060001C8 RID: 456 RVA: 0x000093F4 File Offset: 0x000075F4
	// (set) Token: 0x060001C9 RID: 457 RVA: 0x000093FC File Offset: 0x000075FC
	public bool GotoConversation
	{
		get
		{
			return this.gotoConversation;
		}
		set
		{
			if (this.gotoConversation != value)
			{
				this.gotoConversation = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060001CA RID: 458 RVA: 0x00009418 File Offset: 0x00007618
	// (set) Token: 0x060001CB RID: 459 RVA: 0x00009420 File Offset: 0x00007620
	public int ContactUs
	{
		get
		{
			return this.contactUsOption;
		}
		set
		{
			if (this.contactUsOption != value)
			{
				this.contactUsOption = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060001CC RID: 460 RVA: 0x0000943C File Offset: 0x0000763C
	// (set) Token: 0x060001CD RID: 461 RVA: 0x00009444 File Offset: 0x00007644
	public bool PresentFullScreenOniPad
	{
		get
		{
			return this.presentFullScreen;
		}
		set
		{
			if (this.presentFullScreen != value)
			{
				this.presentFullScreen = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x060001CE RID: 462 RVA: 0x00009460 File Offset: 0x00007660
	// (set) Token: 0x060001CF RID: 463 RVA: 0x00009468 File Offset: 0x00007668
	public bool EnableInAppNotification
	{
		get
		{
			return this.enableInApp;
		}
		set
		{
			if (this.enableInApp != value)
			{
				this.enableInApp = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060001D0 RID: 464 RVA: 0x00009484 File Offset: 0x00007684
	// (set) Token: 0x060001D1 RID: 465 RVA: 0x0000948C File Offset: 0x0000768C
	public bool EnableDialogUIForTablets
	{
		get
		{
			return this.enableDialogUIForTablets;
		}
		set
		{
			if (this.enableDialogUIForTablets != value)
			{
				this.enableDialogUIForTablets = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060001D2 RID: 466 RVA: 0x000094A8 File Offset: 0x000076A8
	// (set) Token: 0x060001D3 RID: 467 RVA: 0x000094B0 File Offset: 0x000076B0
	public bool RequireEmail
	{
		get
		{
			return this.requireEmail;
		}
		set
		{
			if (this.requireEmail != value)
			{
				this.requireEmail = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060001D4 RID: 468 RVA: 0x000094CC File Offset: 0x000076CC
	// (set) Token: 0x060001D5 RID: 469 RVA: 0x000094D4 File Offset: 0x000076D4
	public bool HideNameAndEmail
	{
		get
		{
			return this.hideNameAndEmail;
		}
		set
		{
			if (this.hideNameAndEmail != value)
			{
				this.hideNameAndEmail = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060001D6 RID: 470 RVA: 0x000094F0 File Offset: 0x000076F0
	// (set) Token: 0x060001D7 RID: 471 RVA: 0x000094F8 File Offset: 0x000076F8
	public bool EnablePrivacy
	{
		get
		{
			return this.enablePrivacy;
		}
		set
		{
			if (this.enablePrivacy != value)
			{
				this.enablePrivacy = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060001D8 RID: 472 RVA: 0x00009514 File Offset: 0x00007714
	// (set) Token: 0x060001D9 RID: 473 RVA: 0x0000951C File Offset: 0x0000771C
	public bool ShowSearchOnNewConversation
	{
		get
		{
			return this.showSearchOnNewConversation;
		}
		set
		{
			if (this.showSearchOnNewConversation != value)
			{
				this.showSearchOnNewConversation = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060001DA RID: 474 RVA: 0x00009538 File Offset: 0x00007738
	// (set) Token: 0x060001DB RID: 475 RVA: 0x00009540 File Offset: 0x00007740
	public bool ShowConversationResolutionQuestion
	{
		get
		{
			return this.showConversationResolutionQuestion;
		}
		set
		{
			if (this.showConversationResolutionQuestion != value)
			{
				this.showConversationResolutionQuestion = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060001DC RID: 476 RVA: 0x0000955C File Offset: 0x0000775C
	// (set) Token: 0x060001DD RID: 477 RVA: 0x00009564 File Offset: 0x00007764
	public bool EnableDefaultFallbackLanguage
	{
		get
		{
			return this.enableDefaultFallbackLanguage;
		}
		set
		{
			if (this.enableDefaultFallbackLanguage != value)
			{
				this.enableDefaultFallbackLanguage = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060001DE RID: 478 RVA: 0x00009580 File Offset: 0x00007780
	// (set) Token: 0x060001DF RID: 479 RVA: 0x00009588 File Offset: 0x00007788
	public string ConversationPrefillText
	{
		get
		{
			return this.conversationPrefillText;
		}
		set
		{
			if (this.conversationPrefillText != value)
			{
				this.conversationPrefillText = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060001E0 RID: 480 RVA: 0x000095A8 File Offset: 0x000077A8
	// (set) Token: 0x060001E1 RID: 481 RVA: 0x000095B0 File Offset: 0x000077B0
	public string ApiKey
	{
		get
		{
			return this.apiKey;
		}
		set
		{
			if (this.apiKey != value)
			{
				this.apiKey = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060001E2 RID: 482 RVA: 0x000095D0 File Offset: 0x000077D0
	// (set) Token: 0x060001E3 RID: 483 RVA: 0x000095D8 File Offset: 0x000077D8
	public string DomainName
	{
		get
		{
			return this.domainName;
		}
		set
		{
			if (this.domainName != value)
			{
				this.domainName = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060001E4 RID: 484 RVA: 0x000095F8 File Offset: 0x000077F8
	// (set) Token: 0x060001E5 RID: 485 RVA: 0x00009600 File Offset: 0x00007800
	public string AndroidAppId
	{
		get
		{
			return this.androidAppId;
		}
		set
		{
			if (this.androidAppId != value)
			{
				this.androidAppId = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060001E6 RID: 486 RVA: 0x00009620 File Offset: 0x00007820
	// (set) Token: 0x060001E7 RID: 487 RVA: 0x00009628 File Offset: 0x00007828
	public string iOSAppId
	{
		get
		{
			return this.iosAppId;
		}
		set
		{
			if (this.iosAppId != value)
			{
				this.iosAppId = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060001E8 RID: 488 RVA: 0x00009648 File Offset: 0x00007848
	// (set) Token: 0x060001E9 RID: 489 RVA: 0x00009650 File Offset: 0x00007850
	public string UnityGameObject
	{
		get
		{
			return this.unityGameObject;
		}
		set
		{
			if (this.unityGameObject != value)
			{
				this.unityGameObject = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060001EA RID: 490 RVA: 0x00009670 File Offset: 0x00007870
	// (set) Token: 0x060001EB RID: 491 RVA: 0x00009678 File Offset: 0x00007878
	public string NotificationIcon
	{
		get
		{
			return this.notificationIcon;
		}
		set
		{
			if (this.notificationIcon != value)
			{
				this.notificationIcon = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060001EC RID: 492 RVA: 0x00009698 File Offset: 0x00007898
	// (set) Token: 0x060001ED RID: 493 RVA: 0x000096A0 File Offset: 0x000078A0
	public string NotificationSound
	{
		get
		{
			return this.notificationSound;
		}
		set
		{
			if (this.notificationSound != value)
			{
				this.notificationSound = value;
				HelpshiftConfig.DirtyEditor();
			}
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060001EE RID: 494 RVA: 0x000096C0 File Offset: 0x000078C0
	public Dictionary<string, string> InstallConfig
	{
		get
		{
			return HelpshiftConfig.instance.getInstallConfig();
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060001EF RID: 495 RVA: 0x000096CC File Offset: 0x000078CC
	public Dictionary<string, object> ApiConfig
	{
		get
		{
			return HelpshiftConfig.instance.getApiConfig();
		}
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x000096D8 File Offset: 0x000078D8
	private static void DirtyEditor()
	{
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x000096DC File Offset: 0x000078DC
	public void SaveConfig()
	{
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x000096E0 File Offset: 0x000078E0
	public Dictionary<string, object> getApiConfig()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string value = HelpshiftConfig.instance.contactUsOptions[HelpshiftConfig.instance.contactUsOption];
		dictionary.Add("enableContactUs", value);
		dictionary.Add("gotoConversationAfterContactUs", (!HelpshiftConfig.instance.gotoConversation) ? "no" : "yes");
		dictionary.Add("presentFullScreenOniPad", (!HelpshiftConfig.instance.presentFullScreen) ? "no" : "yes");
		dictionary.Add("requireEmail", (!HelpshiftConfig.instance.requireEmail) ? "no" : "yes");
		dictionary.Add("hideNameAndEmail", (!HelpshiftConfig.instance.hideNameAndEmail) ? "no" : "yes");
		dictionary.Add("enableFullPrivacy", (!HelpshiftConfig.instance.enablePrivacy) ? "no" : "yes");
		dictionary.Add("showSearchOnNewConversation", (!HelpshiftConfig.instance.showSearchOnNewConversation) ? "no" : "yes");
		dictionary.Add("showConversationResolutionQuestion", (!HelpshiftConfig.instance.showConversationResolutionQuestion) ? "no" : "yes");
		dictionary.Add("conversationPrefillText", HelpshiftConfig.instance.conversationPrefillText);
		return dictionary;
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000984C File Offset: 0x00007A4C
	public Dictionary<string, string> getInstallConfig()
	{
		return new Dictionary<string, string>
		{
			{
				"unityGameObject",
				HelpshiftConfig.instance.unityGameObject
			},
			{
				"notificationIcon",
				HelpshiftConfig.instance.notificationIcon
			},
			{
				"notificationSound",
				HelpshiftConfig.instance.notificationSound
			},
			{
				"enableDialogUIForTablets",
				(!HelpshiftConfig.instance.enableDialogUIForTablets) ? "no" : "yes"
			},
			{
				"enableInAppNotification",
				(!HelpshiftConfig.instance.enableInApp) ? "no" : "yes"
			},
			{
				"enableDefaultFallbackLanguage",
				(!HelpshiftConfig.instance.enableDefaultFallbackLanguage) ? "no" : "yes"
			},
			{
				"__hs__apiKey",
				HelpshiftConfig.instance.ApiKey
			},
			{
				"__hs__domainName",
				HelpshiftConfig.instance.DomainName
			},
			{
				"__hs__appId",
				HelpshiftConfig.instance.AndroidAppId
			}
		};
	}

	// Token: 0x040000F9 RID: 249
	private const string helpshiftConfigAssetName = "HelpshiftConfig";

	// Token: 0x040000FA RID: 250
	private const string helpshiftConfigPath = "Helpshift/Resources";

	// Token: 0x040000FB RID: 251
	private static HelpshiftConfig instance;

	// Token: 0x040000FC RID: 252
	[SerializeField]
	private string apiKey;

	// Token: 0x040000FD RID: 253
	[SerializeField]
	private string domainName;

	// Token: 0x040000FE RID: 254
	[SerializeField]
	private string iosAppId;

	// Token: 0x040000FF RID: 255
	[SerializeField]
	private string androidAppId;

	// Token: 0x04000100 RID: 256
	[SerializeField]
	private int contactUsOption;

	// Token: 0x04000101 RID: 257
	[SerializeField]
	private bool gotoConversation;

	// Token: 0x04000102 RID: 258
	[SerializeField]
	private bool presentFullScreen;

	// Token: 0x04000103 RID: 259
	[SerializeField]
	private bool enableDialogUIForTablets;

	// Token: 0x04000104 RID: 260
	[SerializeField]
	private bool enableInApp;

	// Token: 0x04000105 RID: 261
	[SerializeField]
	private bool requireEmail;

	// Token: 0x04000106 RID: 262
	[SerializeField]
	private bool hideNameAndEmail;

	// Token: 0x04000107 RID: 263
	[SerializeField]
	private bool enablePrivacy;

	// Token: 0x04000108 RID: 264
	[SerializeField]
	private bool showSearchOnNewConversation;

	// Token: 0x04000109 RID: 265
	[SerializeField]
	private bool showConversationResolutionQuestion;

	// Token: 0x0400010A RID: 266
	[SerializeField]
	private bool enableDefaultFallbackLanguage;

	// Token: 0x0400010B RID: 267
	[SerializeField]
	private string conversationPrefillText;

	// Token: 0x0400010C RID: 268
	[SerializeField]
	private string[] contactUsOptions = new string[]
	{
		"always",
		"never",
		"after_viewing_faqs"
	};

	// Token: 0x0400010D RID: 269
	[SerializeField]
	private string unityGameObject;

	// Token: 0x0400010E RID: 270
	[SerializeField]
	private string notificationIcon;

	// Token: 0x0400010F RID: 271
	[SerializeField]
	private string notificationSound;
}
