using System;
using UnityEngine;

// Token: 0x020003A9 RID: 937
public class SoaringPlatform
{
	// Token: 0x06001AF0 RID: 6896 RVA: 0x000AFBC8 File Offset: 0x000ADDC8
	private SoaringPlatform(SoaringPlatformType platform)
	{
		if (platform == SoaringPlatformType.System)
		{
			RuntimePlatform platform2 = Application.platform;
			switch (platform2)
			{
			case RuntimePlatform.IPhonePlayer:
				platform = SoaringPlatformType.iPhone;
				break;
			default:
				if (platform2 != RuntimePlatform.OSXPlayer)
				{
					if (platform2 != RuntimePlatform.WindowsPlayer)
					{
						if (platform2 == RuntimePlatform.BB10Player)
						{
							platform = SoaringPlatformType.Blackberry;
						}
					}
					else
					{
						platform = SoaringPlatformType.Windows;
					}
				}
				else
				{
					platform = SoaringPlatformType.OSX;
				}
				break;
			case RuntimePlatform.Android:
				platform = SoaringPlatformType.Android;
				break;
			}
		}
		switch (platform)
		{
		case SoaringPlatformType.iPhone:
			this.platformDelegate = new SBMIIOSPlatformModule();
			goto IL_F9;
		case SoaringPlatformType.Android:
			this.platformDelegate = new SoaringPlatformAndroid();
			goto IL_F9;
		case SoaringPlatformType.Amazon:
			this.platformDelegate = new SoaringPlatformAmazon();
			goto IL_F9;
		case SoaringPlatformType.Windows:
			this.platformDelegate = new SoaringPlatformWindows();
			goto IL_F9;
		case SoaringPlatformType.OSX:
			this.platformDelegate = new SoaringPlatformOSX();
			goto IL_F9;
		}
		SoaringDebug.Log("Unknown Platform", LogType.Error);
		IL_F9:
		this.platformType = platform;
		if (this.platformDelegate != null)
		{
			this.platformDelegate.Init();
		}
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x000AFCF0 File Offset: 0x000ADEF0
	internal static void Init(SoaringPlatformType platform)
	{
		if (SoaringPlatform.sInstance == null)
		{
			SoaringPlatform.sInstance = new SoaringPlatform(platform);
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x000AFD08 File Offset: 0x000ADF08
	public static SoaringLoginType PreferedLoginType
	{
		get
		{
			return SoaringPlatform.sInstance.platformDelegate.PreferedLoginType();
		}
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x000AFD1C File Offset: 0x000ADF1C
	public static void SetPlatformUserData(string userID, string userAlias)
	{
		SoaringPlatform.sInstance.platformDelegate.SetPlatformUserData(userID, userAlias);
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x000AFD30 File Offset: 0x000ADF30
	public static SoaringPlatformType Platform
	{
		get
		{
			return SoaringPlatform.sInstance.platformType;
		}
	}

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000AFD3C File Offset: 0x000ADF3C
	public static SoaringDictionary GenerateDeviceDictionary()
	{
		return SoaringPlatform.sInstance.platformDelegate.GenerateDeviceDictionary();
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x000AFD50 File Offset: 0x000ADF50
	public static bool PlatformLoginAvailable
	{
		get
		{
			return SoaringPlatform.sInstance.platformDelegate.PlatformLoginAvailable();
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x000AFD64 File Offset: 0x000ADF64
	public static bool PlatformLoginAuthenticated
	{
		get
		{
			return SoaringPlatform.sInstance.platformDelegate.PlatformAuthenticated();
		}
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x000AFD78 File Offset: 0x000ADF78
	public static bool AuthenticatedPlatformUser(SoaringContext context)
	{
		return SoaringPlatform.sInstance.platformDelegate.PlatformAuthenticate(context);
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06001AFA RID: 6906 RVA: 0x000AFD8C File Offset: 0x000ADF8C
	public static string DeviceID
	{
		get
		{
			return SoaringPlatform.sInstance.platformDelegate.DeviceID();
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06001AFB RID: 6907 RVA: 0x000AFDA0 File Offset: 0x000ADFA0
	public static string PlatformUserID
	{
		get
		{
			return SoaringPlatform.sInstance.platformDelegate.PlatformID();
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06001AFC RID: 6908 RVA: 0x000AFDB4 File Offset: 0x000ADFB4
	public static string PlatformUserAlias
	{
		get
		{
			return SoaringPlatform.sInstance.platformDelegate.PlatformAlias();
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06001AFD RID: 6909 RVA: 0x000AFDC8 File Offset: 0x000ADFC8
	public static string PushNotificationsProtocol
	{
		get
		{
			return SoaringPlatform.sInstance.platformDelegate.PushNotificationsProtocol();
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x06001AFE RID: 6910 RVA: 0x000AFDDC File Offset: 0x000ADFDC
	public static string PrimaryPlatformName
	{
		get
		{
			return SoaringPlatform.sInstance.platformDelegate.PlatformName();
		}
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x000AFDF0 File Offset: 0x000ADFF0
	public static SoaringPlatform.SoaringPlatformDelegate GetDelegate()
	{
		return SoaringPlatform.sInstance.platformDelegate;
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x000AFDFC File Offset: 0x000ADFFC
	public static bool OpenURL(string url)
	{
		return SoaringPlatform.sInstance.platformDelegate.OpenURL(url);
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x000AFE10 File Offset: 0x000AE010
	public static bool SendEmail(string subject, string body, string email)
	{
		return SoaringPlatform.sInstance.platformDelegate.SendEmail(subject, body, email);
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x000AFE24 File Offset: 0x000AE024
	public static long SystemBootTime()
	{
		return SoaringPlatform.sInstance.platformDelegate.SystemBootTime();
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x000AFE38 File Offset: 0x000AE038
	public static long SystemTimeSinceBootTime()
	{
		return SoaringPlatform.sInstance.platformDelegate.SystemTimeSinceBootTime();
	}

	// Token: 0x040011E1 RID: 4577
	private static SoaringPlatform sInstance;

	// Token: 0x040011E2 RID: 4578
	private SoaringPlatform.SoaringPlatformDelegate platformDelegate;

	// Token: 0x040011E3 RID: 4579
	private SoaringPlatformType platformType;

	// Token: 0x020003AA RID: 938
	public class SoaringPlatformDelegate
	{
		// Token: 0x06001B05 RID: 6917 RVA: 0x000AFE54 File Offset: 0x000AE054
		public virtual void Init()
		{
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x000AFE58 File Offset: 0x000AE058
		public virtual SoaringLoginType PreferedLoginType()
		{
			return SoaringLoginType.Soaring;
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x000AFE5C File Offset: 0x000AE05C
		public virtual string PlatformName()
		{
			return null;
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x000AFE60 File Offset: 0x000AE060
		public virtual bool PlatformLoginAvailable()
		{
			return false;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x000AFE64 File Offset: 0x000AE064
		public virtual bool PlatformAuthenticated()
		{
			return false;
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x000AFE68 File Offset: 0x000AE068
		public virtual bool PlatformAuthenticate(SoaringContext context)
		{
			return false;
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x000AFE6C File Offset: 0x000AE06C
		public virtual string PlatformID()
		{
			return null;
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x000AFE70 File Offset: 0x000AE070
		public virtual string PlatformAlias()
		{
			return null;
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x000AFE74 File Offset: 0x000AE074
		public virtual string DeviceID()
		{
			return null;
		}

		// Token: 0x06001B0E RID: 6926 RVA: 0x000AFE78 File Offset: 0x000AE078
		public virtual SoaringDictionary GenerateDeviceDictionary()
		{
			return null;
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x000AFE7C File Offset: 0x000AE07C
		public virtual string PushNotificationsProtocol()
		{
			return null;
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x000AFE80 File Offset: 0x000AE080
		public virtual void SetPlatformUserData(string userID, string userAlias)
		{
		}

		// Token: 0x06001B11 RID: 6929 RVA: 0x000AFE84 File Offset: 0x000AE084
		public virtual bool OpenURL(string url)
		{
			return false;
		}

		// Token: 0x06001B12 RID: 6930 RVA: 0x000AFE88 File Offset: 0x000AE088
		public virtual bool SendEmail(string subject, string body, string email)
		{
			return false;
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000AFE8C File Offset: 0x000AE08C
		public virtual bool OpenPath(string path)
		{
			return false;
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x000AFE90 File Offset: 0x000AE090
		public virtual long SystemBootTime()
		{
			return (long)(DateTime.UtcNow - SoaringTime.Epoch).TotalSeconds - this.SystemTimeSinceBootTime();
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000AFEBC File Offset: 0x000AE0BC
		public virtual long SystemTimeSinceBootTime()
		{
			return (long)Time.realtimeSinceStartup;
		}
	}
}
