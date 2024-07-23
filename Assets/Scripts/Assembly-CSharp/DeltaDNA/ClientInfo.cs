using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DeltaDNA
{
	// Token: 0x02000003 RID: 3
	internal static class ClientInfo
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000217C File Offset: 0x0000037C
		public static string Platform
		{
			get
			{
				string result;
				if ((result = ClientInfo.platform) == null)
				{
					result = (ClientInfo.platform = ClientInfo.GetPlatform());
				}
				return result;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002198 File Offset: 0x00000398
		public static string DeviceName
		{
			get
			{
				string result;
				if ((result = ClientInfo.deviceName) == null)
				{
					result = (ClientInfo.deviceName = ClientInfo.GetDeviceName());
				}
				return result;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000021B4 File Offset: 0x000003B4
		public static string DeviceModel
		{
			get
			{
				string result;
				if ((result = ClientInfo.deviceModel) == null)
				{
					result = (ClientInfo.deviceModel = ClientInfo.GetDeviceModel());
				}
				return result;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021D0 File Offset: 0x000003D0
		public static string DeviceType
		{
			get
			{
				string result;
				if ((result = ClientInfo.deviceType) == null)
				{
					result = (ClientInfo.deviceType = ClientInfo.GetDeviceType());
				}
				return result;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021EC File Offset: 0x000003EC
		public static string OperatingSystem
		{
			get
			{
				string result;
				if ((result = ClientInfo.operatingSystem) == null)
				{
					result = (ClientInfo.operatingSystem = ClientInfo.GetOperatingSystem());
				}
				return result;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002208 File Offset: 0x00000408
		public static string OperatingSystemVersion
		{
			get
			{
				string result;
				if ((result = ClientInfo.operatingSystemVersion) == null)
				{
					result = (ClientInfo.operatingSystemVersion = ClientInfo.GetOperatingSystemVersion());
				}
				return result;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002224 File Offset: 0x00000424
		public static string Manufacturer
		{
			get
			{
				string result;
				if ((result = ClientInfo.manufacturer) == null)
				{
					result = (ClientInfo.manufacturer = ClientInfo.GetManufacturer());
				}
				return result;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002240 File Offset: 0x00000440
		public static string TimezoneOffset
		{
			get
			{
				string result;
				if ((result = ClientInfo.timezoneOffset) == null)
				{
					result = (ClientInfo.timezoneOffset = ClientInfo.GetCurrentTimezoneOffset());
				}
				return result;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000225C File Offset: 0x0000045C
		public static string CountryCode
		{
			get
			{
				string result;
				if ((result = ClientInfo.countryCode) == null)
				{
					result = (ClientInfo.countryCode = ClientInfo.GetCountryCode());
				}
				return result;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002278 File Offset: 0x00000478
		public static string LanguageCode
		{
			get
			{
				string result;
				if ((result = ClientInfo.languageCode) == null)
				{
					result = (ClientInfo.languageCode = ClientInfo.GetLanguageCode());
				}
				return result;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002294 File Offset: 0x00000494
		public static string Locale
		{
			get
			{
				string result;
				if ((result = ClientInfo.locale) == null)
				{
					result = (ClientInfo.locale = ClientInfo.GetLocale());
				}
				return result;
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022B0 File Offset: 0x000004B0
		private static string GetPlatform()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.OSXEditor:
				return "MAC_CLIENT";
			case RuntimePlatform.OSXPlayer:
				return "MAC_CLIENT";
			case RuntimePlatform.WindowsPlayer:
				return "PC_CLIENT";
			case RuntimePlatform.OSXWebPlayer:
				return "WEB";
			case RuntimePlatform.OSXDashboardPlayer:
				return "MAC_CLIENT";
			case RuntimePlatform.WindowsWebPlayer:
				return "WEB";
			case RuntimePlatform.WindowsEditor:
				return "PC_CLIENT";
			case RuntimePlatform.PS3:
				return "PS3";
			case RuntimePlatform.XBOX360:
				return "XBOX360";
			case RuntimePlatform.Android:
				return "ANDROID";
			case RuntimePlatform.NaCl:
				return "WEB";
			case RuntimePlatform.LinuxPlayer:
				return "PC_CLIENT";
			case RuntimePlatform.FlashPlayer:
				return "WEB";
			case RuntimePlatform.MetroPlayerX86:
				return "WINDOWS_TABLET";
			case RuntimePlatform.MetroPlayerX64:
				return "WINDOWS_TABLET";
			case RuntimePlatform.MetroPlayerARM:
				return "WINDOWS_TABLET";
			case RuntimePlatform.WP8Player:
				return "WINDOWS_MOBILE";
			case RuntimePlatform.TizenPlayer:
				return "ANDROID";
			}
			Debug.LogWarning("Unsupported platform '" + Application.platform + "' returning UNKNOWN");
			return "UNKNOWN";
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023C0 File Offset: 0x000005C0
		private static string GetDeviceName()
		{
			string text = SystemInfo.deviceModel;
			string text2 = text;
			switch (text2)
			{
			case "iPhone1,1":
				return "iPhone 1G";
			case "iPhone1,2":
				return "iPhone 3G";
			case "iPhone2,1":
				return "iPhone 3GS";
			case "iPhone3,1":
				return "iPhone 4";
			case "iPhone3,2":
				return "iPhone 4";
			case "iPhone3,3":
				return "iPhone 4";
			case "iPhone4,1":
				return "iPhone 4S";
			case "iPhone5,1":
				return "iPhone 5";
			case "iPhone5,2":
				return "iPhone 5";
			case "iPhone5,3":
				return "iPhone 5C";
			case "iPhone5,4":
				return "iPhone 5C";
			case "iPhone6,1":
				return "iPhone 5S";
			case "iPhone6,2":
				return "iPhone 5S";
			case "iPhone7,2":
				return "iPhone 6";
			case "iPhone7,1":
				return "iPhone 6 Plus";
			case "iPod1,1":
				return "iPod Touch 1G";
			case "iPod2,1":
				return "iPod Touch 2G";
			case "iPod3,1":
				return "iPod Touch 3G";
			case "iPod4,1":
				return "iPod Touch 4G";
			case "iPod5,1":
				return "iPod Touch 5G";
			case "iPad1,1":
				return "iPad 1G";
			case "iPad2,1":
				return "iPad 2";
			case "iPad2,2":
				return "iPad 2";
			case "iPad2,3":
				return "iPad 2";
			case "iPad2,4":
				return "iPad 2";
			case "iPad3,1":
				return "iPad 3G";
			case "iPad3,2":
				return "iPad 3G";
			case "iPad3,3":
				return "iPad 3G";
			case "iPad3,4":
				return "iPad 4G";
			case "iPad3,5":
				return "iPad 4G";
			case "iPad3,6":
				return "iPad 4G";
			case "iPad4,1":
				return "iPad Air";
			case "iPad4,2":
				return "iPad Air";
			case "iPad4,3":
				return "iPad Air";
			case "iPad5,3":
				return "iPad Air 2";
			case "iPad5,4":
				return "iPad Air 2";
			case "iPad2,5":
				return "iPad Mini 1G";
			case "iPad2,6":
				return "iPad Mini 1G";
			case "iPad2,7":
				return "iPad Mini 1G";
			case "iPad4,4":
				return "iPad Mini 2G";
			case "iPad4,5":
				return "iPad Mini 2G";
			case "iPad4,6":
				return "iPad Mini 2G";
			case "iPad4,7":
				return "iPad Mini 3";
			case "iPad4,8":
				return "iPad Mini 3";
			case "iPad4,9":
				return "iPad Mini 3";
			case "Amazon KFSAWA":
				return "Fire HDX 8.9 (4th Gen)";
			case "Amazon KFASWI":
				return "Fire HD 7 (4th Gen)";
			case "Amazon KFARWI":
				return "Fire HD 6 (4th Gen)";
			case "Amazon KFAPWA":
			case "Amazon KFAPWI":
				return "Kindle Fire HDX 8.9 (3rd Gen)";
			case "Amazon KFTHWA":
			case "Amazon KFTHWI":
				return "Kindle Fire HDX 7 (3rd Gen)";
			case "Amazon KFSOWI":
				return "Kindle Fire HD 7 (3rd Gen)";
			case "Amazon KFJWA":
			case "Amazon KFJWI":
				return "Kindle Fire HD 8.9 (2nd Gen)";
			case "Amazon KFTT":
				return "Kindle Fire HD 7 (2nd Gen)";
			case "Amazon KFOT":
				return "Kindle Fire (2nd Gen)";
			case "Amazon Kindle Fire":
				return "Kindle Fire (1st Gen)";
			}
			return text;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002920 File Offset: 0x00000B20
		private static string GetDeviceModel()
		{
			return ClientInfo.deviceModel;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002928 File Offset: 0x00000B28
		private static string GetDeviceType()
		{
			switch (SystemInfo.deviceType)
			{
			case UnityEngine.DeviceType.Unknown:
				return "UNKOWN";
			case UnityEngine.DeviceType.Handheld:
				return "HANDHELD";
			case UnityEngine.DeviceType.Console:
				return "CONSOLE";
			case UnityEngine.DeviceType.Desktop:
				return "PC";
			default:
				return "UNKNOWN";
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002974 File Offset: 0x00000B74
		private static string GetOperatingSystem()
		{
			string text = SystemInfo.operatingSystem.ToUpper();
			if (text.Contains("WINDOWS"))
			{
				return "WINDOWS";
			}
			if (text.Contains("OSX"))
			{
				return "OSX";
			}
			if (text.Contains("MAC"))
			{
				return "OSX";
			}
			if (text.Contains("IOS"))
			{
				return "IOS";
			}
			if (text.Contains("LINUX"))
			{
				return "LINUX";
			}
			if (text.Contains("ANDROID"))
			{
				return "ANDROID";
			}
			if (text.Contains("BLACKBERRY"))
			{
				return "BLACKBERRY";
			}
			return "UNKNOWN";
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002A2C File Offset: 0x00000C2C
		private static string GetOperatingSystemVersion()
		{
			string pattern = "^\\w+";
			Regex regex = new Regex(pattern);
			string input = SystemInfo.operatingSystem;
			return regex.Replace(input, string.Empty).Trim();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002A60 File Offset: 0x00000C60
		private static string GetManufacturer()
		{
			return null;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A64 File Offset: 0x00000C64
		private static string GetCurrentTimezoneOffset()
		{
			string result;
			try
			{
				TimeZone currentTimeZone = TimeZone.CurrentTimeZone;
				DateTime now = DateTime.Now;
				TimeSpan utcOffset = currentTimeZone.GetUtcOffset(now);
				result = string.Format("{0}{1:D2}", (utcOffset.Hours < 0) ? string.Empty : "+", utcOffset.Hours);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002AEC File Offset: 0x00000CEC
		private static string GetCountryCode()
		{
			return null;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002AF0 File Offset: 0x00000CF0
		private static string GetLanguageCode()
		{
			switch (Application.systemLanguage)
			{
			case SystemLanguage.Afrikaans:
				return "af";
			case SystemLanguage.Arabic:
				return "ar";
			case SystemLanguage.Basque:
				return "eu";
			case SystemLanguage.Belarusian:
				return "be";
			case SystemLanguage.Bulgarian:
				return "bg";
			case SystemLanguage.Catalan:
				return "ca";
			case SystemLanguage.Chinese:
				return "zh";
			case SystemLanguage.Czech:
				return "cs";
			case SystemLanguage.Danish:
				return "da";
			case SystemLanguage.Dutch:
				return "nl";
			case SystemLanguage.English:
				return "en";
			case SystemLanguage.Estonian:
				return "et";
			case SystemLanguage.Faroese:
				return "fo";
			case SystemLanguage.Finnish:
				return "fi";
			case SystemLanguage.French:
				return "fr";
			case SystemLanguage.German:
				return "de";
			case SystemLanguage.Greek:
				return "el";
			case SystemLanguage.Hebrew:
				return "he";
			case SystemLanguage.Hugarian:
				return "hu";
			case SystemLanguage.Icelandic:
				return "is";
			case SystemLanguage.Indonesian:
				return "id";
			case SystemLanguage.Italian:
				return "it";
			case SystemLanguage.Japanese:
				return "ja";
			case SystemLanguage.Korean:
				return "ko";
			case SystemLanguage.Latvian:
				return "lv";
			case SystemLanguage.Lithuanian:
				return "lt";
			case SystemLanguage.Norwegian:
				return "nn";
			case SystemLanguage.Polish:
				return "pl";
			case SystemLanguage.Portuguese:
				return "pt";
			case SystemLanguage.Romanian:
				return "ro";
			case SystemLanguage.Russian:
				return "ru";
			case SystemLanguage.SerboCroatian:
				return "sr";
			case SystemLanguage.Slovak:
				return "sk";
			case SystemLanguage.Slovenian:
				return "sl";
			case SystemLanguage.Spanish:
				return "es";
			case SystemLanguage.Swedish:
				return "sv";
			case SystemLanguage.Thai:
				return "th";
			case SystemLanguage.Turkish:
				return "tr";
			case SystemLanguage.Ukrainian:
				return "uk";
			case SystemLanguage.Vietnamese:
				return "vi";
			default:
				return "en";
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002CA4 File Offset: 0x00000EA4
		private static string GetLocale()
		{
			if (ClientInfo.CountryCode != null)
			{
				return string.Format("{0}_{1}", ClientInfo.LanguageCode, ClientInfo.CountryCode);
			}
			return string.Format("{0}_ZZ", ClientInfo.LanguageCode);
		}

		// Token: 0x04000002 RID: 2
		private static string platform;

		// Token: 0x04000003 RID: 3
		private static string deviceName;

		// Token: 0x04000004 RID: 4
		private static string deviceModel;

		// Token: 0x04000005 RID: 5
		private static string deviceType;

		// Token: 0x04000006 RID: 6
		private static string operatingSystem;

		// Token: 0x04000007 RID: 7
		private static string operatingSystemVersion;

		// Token: 0x04000008 RID: 8
		private static string manufacturer;

		// Token: 0x04000009 RID: 9
		private static string timezoneOffset;

		// Token: 0x0400000A RID: 10
		private static string countryCode;

		// Token: 0x0400000B RID: 11
		private static string languageCode;

		// Token: 0x0400000C RID: 12
		private static string locale;
	}
}
