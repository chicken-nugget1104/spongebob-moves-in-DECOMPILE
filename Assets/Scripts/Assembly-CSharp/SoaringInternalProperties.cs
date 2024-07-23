using System;
using MTools;

// Token: 0x020003A6 RID: 934
internal static class SoaringInternalProperties
{
	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x000AF5D4 File Offset: 0x000AD7D4
	// (set) Token: 0x06001AD4 RID: 6868 RVA: 0x000AF5DC File Offset: 0x000AD7DC
	public static bool EnableAddressKeeper
	{
		get
		{
			return SoaringInternalProperties.Get(0);
		}
		set
		{
			SoaringInternalProperties.Set(value, 0);
		}
	}

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x000AF5E8 File Offset: 0x000AD7E8
	// (set) Token: 0x06001AD6 RID: 6870 RVA: 0x000AF5F0 File Offset: 0x000AD7F0
	public static bool EnableVersions
	{
		get
		{
			return SoaringInternalProperties.Get(1);
		}
		set
		{
			SoaringInternalProperties.Set(value, 1);
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x000AF5FC File Offset: 0x000AD7FC
	// (set) Token: 0x06001AD8 RID: 6872 RVA: 0x000AF604 File Offset: 0x000AD804
	public static bool EnableServerTimeVersions
	{
		get
		{
			return SoaringInternalProperties.Get(2);
		}
		set
		{
			SoaringInternalProperties.Set(value, 2);
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x000AF610 File Offset: 0x000AD810
	// (set) Token: 0x06001ADA RID: 6874 RVA: 0x000AF618 File Offset: 0x000AD818
	public static bool EnableDeveloperLogin
	{
		get
		{
			return SoaringInternalProperties.Get(3);
		}
		set
		{
			SoaringInternalProperties.Set(value, 3);
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06001ADB RID: 6875 RVA: 0x000AF624 File Offset: 0x000AD824
	// (set) Token: 0x06001ADC RID: 6876 RVA: 0x000AF62C File Offset: 0x000AD82C
	public static bool EnableLocalMode
	{
		get
		{
			return SoaringInternalProperties.Get(4);
		}
		set
		{
			SoaringInternalProperties.Set(value, 4);
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06001ADD RID: 6877 RVA: 0x000AF638 File Offset: 0x000AD838
	// (set) Token: 0x06001ADE RID: 6878 RVA: 0x000AF640 File Offset: 0x000AD840
	public static bool EnableAdServer
	{
		get
		{
			return SoaringInternalProperties.Get(5);
		}
		set
		{
			SoaringInternalProperties.Set(value, 5);
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06001ADF RID: 6879 RVA: 0x000AF64C File Offset: 0x000AD84C
	// (set) Token: 0x06001AE0 RID: 6880 RVA: 0x000AF654 File Offset: 0x000AD854
	public static bool EnableDeviceData
	{
		get
		{
			return SoaringInternalProperties.Get(6);
		}
		set
		{
			SoaringInternalProperties.Set(value, 6);
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x000AF660 File Offset: 0x000AD860
	// (set) Token: 0x06001AE2 RID: 6882 RVA: 0x000AF668 File Offset: 0x000AD868
	public static bool EnableAnalytics
	{
		get
		{
			return SoaringInternalProperties.Get(7);
		}
		set
		{
			SoaringInternalProperties.Set(value, 7);
		}
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x000AF674 File Offset: 0x000AD874
	// (set) Token: 0x06001AE4 RID: 6884 RVA: 0x000AF67C File Offset: 0x000AD87C
	public static bool LoginOnInitialize
	{
		get
		{
			return SoaringInternalProperties.Get(8);
		}
		set
		{
			SoaringInternalProperties.Set(value, 8);
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x000AF688 File Offset: 0x000AD888
	// (set) Token: 0x06001AE6 RID: 6886 RVA: 0x000AF694 File Offset: 0x000AD894
	public static bool SaveUserCredentials
	{
		get
		{
			return SoaringInternalProperties.Get(9);
		}
		set
		{
			SoaringInternalProperties.Set(value, 9);
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x000AF6A0 File Offset: 0x000AD8A0
	// (set) Token: 0x06001AE8 RID: 6888 RVA: 0x000AF6AC File Offset: 0x000AD8AC
	public static bool SecureCommunication
	{
		get
		{
			return SoaringInternalProperties.Get(10);
		}
		set
		{
			SoaringInternalProperties.Set(value, 10);
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x000AF6B8 File Offset: 0x000AD8B8
	// (set) Token: 0x06001AEA RID: 6890 RVA: 0x000AF6C4 File Offset: 0x000AD8C4
	public static bool AutoChooseUserPlayer
	{
		get
		{
			return SoaringInternalProperties.Get(11);
		}
		set
		{
			SoaringInternalProperties.Set(value, 11);
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06001AEB RID: 6891 RVA: 0x000AF6D0 File Offset: 0x000AD8D0
	// (set) Token: 0x06001AEC RID: 6892 RVA: 0x000AF6DC File Offset: 0x000AD8DC
	public static bool ForceOfflineModeUser
	{
		get
		{
			return SoaringInternalProperties.Get(12);
		}
		set
		{
			SoaringInternalProperties.Set(value, 12);
		}
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x000AF6E8 File Offset: 0x000AD8E8
	private static bool Get(int x)
	{
		return (SoaringInternalProperties.mSettings & 1 << x) != 0;
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x000AF6FC File Offset: 0x000AD8FC
	private static void Set(bool v, int x)
	{
		SoaringInternalProperties.mSettings = ((!v) ? (SoaringInternalProperties.mSettings & ~(1 << x)) : (SoaringInternalProperties.mSettings | 1 << x));
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x000AF728 File Offset: 0x000AD928
	internal static void Load()
	{
		SoaringInternalProperties.EnableAddressKeeper = true;
		SoaringInternalProperties.EnableVersions = true;
		SoaringInternalProperties.EnableServerTimeVersions = true;
		SoaringInternalProperties.EnableDeveloperLogin = false;
		SoaringInternalProperties.EnableLocalMode = false;
		SoaringInternalProperties.EnableAdServer = true;
		SoaringInternalProperties.EnableDeviceData = true;
		SoaringInternalProperties.EnableAnalytics = true;
		SoaringInternalProperties.LoginOnInitialize = true;
		SoaringInternalProperties.SaveUserCredentials = true;
		SoaringInternalProperties.SecureCommunication = false;
		SoaringInternalProperties.AutoChooseUserPlayer = false;
		SoaringInternalProperties.ForceOfflineModeUser = false;
		MBinaryReader fileStream = ResourceUtils.GetFileStream("SoaringPR", "Soaring", "bytes", 3);
		SoaringDictionary soaringDictionary = null;
		if (fileStream != null)
		{
			string text = string.Empty;
			byte[] array = fileStream.ReadAllBytes();
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					text += (char)array[i];
				}
			}
			soaringDictionary = new SoaringDictionary(text);
			fileStream.Close();
		}
		if (soaringDictionary == null)
		{
			soaringDictionary = new SoaringDictionary();
		}
		if (soaringDictionary != null)
		{
			SoaringValue soaringValue = soaringDictionary.soaringValue("AddressKeeper");
			if (soaringValue != null)
			{
				SoaringInternalProperties.EnableAddressKeeper = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("Versions");
			if (soaringValue != null)
			{
				SoaringInternalProperties.EnableAddressKeeper = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("AdServer");
			if (soaringValue != null)
			{
				SoaringInternalProperties.EnableAdServer = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("DeviceData");
			if (soaringValue != null)
			{
				SoaringInternalProperties.EnableDeviceData = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("Analytics");
			if (soaringValue != null)
			{
				SoaringInternalProperties.EnableAnalytics = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("ServerTime");
			if (soaringValue != null)
			{
				SoaringInternalProperties.EnableServerTimeVersions = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("LocalMode");
			if (soaringValue != null)
			{
				SoaringInternalProperties.EnableLocalMode = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("LoginOnInitialize");
			if (soaringValue != null)
			{
				SoaringInternalProperties.LoginOnInitialize = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("DevLogin");
			if (soaringValue != null)
			{
				SoaringInternalProperties.EnableDeveloperLogin = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("SaveUserCredentials");
			if (soaringValue != null)
			{
				SoaringInternalProperties.SaveUserCredentials = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("SecureCommunication");
			if (soaringValue != null)
			{
				SoaringInternalProperties.SecureCommunication = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("AutoChooseUserPlayer");
			if (soaringValue != null)
			{
				SoaringInternalProperties.AutoChooseUserPlayer = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("ForceOfflineModeUser");
			if (soaringValue != null)
			{
				SoaringInternalProperties.ForceOfflineModeUser = (soaringValue != 0);
			}
			soaringValue = soaringDictionary.soaringValue("AnalyticsBufferSize");
			if (soaringValue != null)
			{
				SoaringInternalProperties.AnalyticsBufferSize = soaringValue;
			}
			soaringValue = soaringDictionary.soaringValue("DevName");
			if (soaringValue != null)
			{
				SoaringInternalProperties.DeveloperLoginTag = soaringValue;
				if (string.IsNullOrEmpty(SoaringInternalProperties.DeveloperLoginTag))
				{
					SoaringInternalProperties.DeveloperLoginTag = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("DevPassword");
			if (soaringValue != null)
			{
				SoaringInternalProperties.DeveloperLoginPassword = soaringValue;
				if (string.IsNullOrEmpty(SoaringInternalProperties.DeveloperLoginPassword))
				{
					SoaringInternalProperties.DeveloperLoginPassword = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringQAURL");
			if (soaringValue != null)
			{
				SoaringInternalProperties.SoaringTestingURL = soaringValue;
				if (string.IsNullOrEmpty(SoaringInternalProperties.SoaringTestingURL))
				{
					SoaringInternalProperties.SoaringTestingURL = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringDevURL");
			if (soaringValue != null)
			{
				SoaringInternalProperties.SoaringDevelopmentURL = soaringValue;
				if (string.IsNullOrEmpty(SoaringInternalProperties.SoaringDevelopmentURL))
				{
					SoaringInternalProperties.SoaringDevelopmentURL = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringProductionURL");
			if (soaringValue != null)
			{
				SoaringInternalProperties.SoaringProductionURL = soaringValue;
				if (string.IsNullOrEmpty(SoaringInternalProperties.SoaringProductionURL))
				{
					SoaringInternalProperties.SoaringProductionURL = null;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringQACDN");
			if (soaringValue != null)
			{
				SoaringInternalProperties.SoaringTestingCDN = soaringValue;
				if (string.IsNullOrEmpty(SoaringInternalProperties.SoaringTestingCDN))
				{
					SoaringInternalProperties.SoaringTestingCDN = SoaringInternalProperties.SoaringTestingURL;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringDevCDN");
			if (soaringValue != null)
			{
				SoaringInternalProperties.SoaringDevelopmentCDN = soaringValue;
				if (string.IsNullOrEmpty(SoaringInternalProperties.SoaringDevelopmentCDN))
				{
					SoaringInternalProperties.SoaringDevelopmentCDN = SoaringInternalProperties.SoaringDevelopmentURL;
				}
			}
			soaringValue = soaringDictionary.soaringValue("SoaringProductionCDN");
			if (soaringValue != null)
			{
				SoaringInternalProperties.SoaringProductionCDN = soaringValue;
				if (string.IsNullOrEmpty(SoaringInternalProperties.SoaringProductionCDN))
				{
					SoaringInternalProperties.SoaringProductionCDN = SoaringInternalProperties.SoaringProductionURL;
				}
			}
		}
		SoaringInternalProperties.IsLoaded = true;
	}

	// Token: 0x04001160 RID: 4448
	private static int mSettings;

	// Token: 0x04001161 RID: 4449
	public static int AnalyticsBufferSize;

	// Token: 0x04001162 RID: 4450
	public static string DeveloperLoginTag;

	// Token: 0x04001163 RID: 4451
	public static string DeveloperLoginPassword;

	// Token: 0x04001164 RID: 4452
	public static string SoaringTestingURL = string.Empty;

	// Token: 0x04001165 RID: 4453
	public static string SoaringDevelopmentURL = string.Empty;

	// Token: 0x04001166 RID: 4454
	public static string SoaringProductionURL = string.Empty;

	// Token: 0x04001167 RID: 4455
	public static string SoaringTestingCDN = string.Empty;

	// Token: 0x04001168 RID: 4456
	public static string SoaringDevelopmentCDN = string.Empty;

	// Token: 0x04001169 RID: 4457
	public static string SoaringProductionCDN = string.Empty;

	// Token: 0x0400116A RID: 4458
	private static string DevAuthLoginToken;

	// Token: 0x0400116B RID: 4459
	public static bool IsLoaded;
}
