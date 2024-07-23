using System;
using UnityEngine;

// Token: 0x0200037E RID: 894
public class SoaringPlatformIOS : SoaringPlatform.SoaringPlatformDelegate
{
	// Token: 0x06001973 RID: 6515 RVA: 0x000A797C File Offset: 0x000A5B7C
	public override void Init()
	{
		this.mADID = string.Empty;
		this.mUDID = string.Empty;
		this.mIDFV = string.Empty;
		this.mOdin1MD5 = string.Empty;
		this.mOdin1SH1 = string.Empty;
		this.mMacAddress = string.Empty;
		this.mPlatformUserID = string.Empty;
		this.mPlatformUserAlias = string.Empty;
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000A79E4 File Offset: 0x000A5BE4
	public override SoaringLoginType PreferedLoginType()
	{
		return SoaringLoginType.GameCenter;
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000A79E8 File Offset: 0x000A5BE8
	public override string PlatformName()
	{
		return "ios";
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x000A79F0 File Offset: 0x000A5BF0
	public override bool PlatformLoginAvailable()
	{
		return false;
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x000A7A00 File Offset: 0x000A5C00
	public override bool PlatformAuthenticated()
	{
		return false;
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x000A7A10 File Offset: 0x000A5C10
	public override bool PlatformAuthenticate(SoaringContext context)
	{
		if (context == null)
		{
			context = new SoaringContext();
		}
		context.Name = "login";
		this.mPlatformUserID = string.Empty;
		this.mPlatformUserAlias = string.Empty;
		return this.PlatformLoginAvailable() && !this.PlatformAuthenticated();
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x000A7A64 File Offset: 0x000A5C64
	public override void SetPlatformUserData(string userID, string userAlias)
	{
		this.mPlatformUserID = userID;
		this.mPlatformUserAlias = userAlias;
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x000A7A74 File Offset: 0x000A5C74
	public override string PlatformID()
	{
		if (!string.IsNullOrEmpty(this.mPlatformUserID))
		{
			return this.mPlatformUserID;
		}
		return this.mPlatformUserID;
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x000A7A94 File Offset: 0x000A5C94
	public override string PlatformAlias()
	{
		if (!string.IsNullOrEmpty(this.mPlatformUserAlias))
		{
			return this.mPlatformUserAlias;
		}
		return this.mPlatformUserAlias;
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x000A7AB4 File Offset: 0x000A5CB4
	public override string DeviceID()
	{
		string text = this.VendorIdentifier;
		if (string.IsNullOrEmpty(text))
		{
			text = SystemInfo.deviceUniqueIdentifier;
		}
		return text;
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x000A7ADC File Offset: 0x000A5CDC
	public override SoaringDictionary GenerateDeviceDictionary()
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		try
		{
			string text = null;
			int num = (int)this.iOSVersion();
			if (num <= 6)
			{
				string odin1Md = this.Odin1Md5;
				if (!string.IsNullOrEmpty(odin1Md))
				{
					soaringDictionary.addValue(odin1Md, "odin1");
					text = odin1Md;
				}
				string udid = this.UDID;
				if (!string.IsNullOrEmpty(udid))
				{
					soaringDictionary.addValue(udid, "udid");
					text = udid;
				}
			}
			if (num >= 6)
			{
				string vendorIdentifier = this.VendorIdentifier;
				if (!string.IsNullOrEmpty(vendorIdentifier))
				{
					soaringDictionary.addValue(vendorIdentifier, "idfv");
					text = vendorIdentifier;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = this.DeviceID();
			}
			soaringDictionary.addValue(text, "deviceId");
			soaringDictionary.addValue(this.PlatformName(), "platform");
		}
		catch
		{
		}
		return soaringDictionary;
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x000A7BE0 File Offset: 0x000A5DE0
	public override string PushNotificationsProtocol()
	{
		return "apns";
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x0600197F RID: 6527 RVA: 0x000A7BE8 File Offset: 0x000A5DE8
	public string UDID
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mUDID))
			{
				return this.mUDID;
			}
			return this.mUDID;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06001980 RID: 6528 RVA: 0x000A7C08 File Offset: 0x000A5E08
	public string MacAddress
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mMacAddress))
			{
				return this.mMacAddress;
			}
			return this.mMacAddress;
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06001981 RID: 6529 RVA: 0x000A7C28 File Offset: 0x000A5E28
	public string Odin1Sha1
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mOdin1SH1))
			{
				return this.mOdin1SH1;
			}
			return this.mOdin1SH1;
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06001982 RID: 6530 RVA: 0x000A7C48 File Offset: 0x000A5E48
	public string Odin1Md5
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mOdin1MD5))
			{
				return this.mOdin1MD5;
			}
			return this.mOdin1MD5;
		}
	}

	// Token: 0x1700034D RID: 845
	// (get) Token: 0x06001983 RID: 6531 RVA: 0x000A7C68 File Offset: 0x000A5E68
	public string AdvertisingIdentifier
	{
		get
		{
			return string.Empty;
		}
	}

	// Token: 0x1700034E RID: 846
	// (get) Token: 0x06001984 RID: 6532 RVA: 0x000A7C70 File Offset: 0x000A5E70
	public string VendorIdentifier
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mIDFV))
			{
				return this.mIDFV;
			}
			return this.mIDFV;
		}
	}

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x06001985 RID: 6533 RVA: 0x000A7C90 File Offset: 0x000A5E90
	public bool AdvertisingIdentifierEnabled
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x000A7C94 File Offset: 0x000A5E94
	private float iOSVersion()
	{
		return -1f;
	}

	// Token: 0x06001987 RID: 6535 RVA: 0x000A7CA8 File Offset: 0x000A5EA8
	public override bool OpenURL(string url)
	{
		Application.OpenURL(url);
		return true;
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x000A7CB4 File Offset: 0x000A5EB4
	public override bool SendEmail(string subject, string body, string email)
	{
		return base.SendEmail(subject, body, email);
	}

	// Token: 0x06001989 RID: 6537 RVA: 0x000A7CC0 File Offset: 0x000A5EC0
	public override bool OpenPath(string path)
	{
		return base.OpenPath(path);
	}

	// Token: 0x0600198A RID: 6538 RVA: 0x000A7CCC File Offset: 0x000A5ECC
	public override long SystemBootTime()
	{
		return base.SystemBootTime();
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x000A7CD4 File Offset: 0x000A5ED4
	public override long SystemTimeSinceBootTime()
	{
		return base.SystemTimeSinceBootTime();
	}

	// Token: 0x04001086 RID: 4230
	private string mADID;

	// Token: 0x04001087 RID: 4231
	private string mUDID;

	// Token: 0x04001088 RID: 4232
	private string mIDFV;

	// Token: 0x04001089 RID: 4233
	private string mOdin1MD5;

	// Token: 0x0400108A RID: 4234
	private string mOdin1SH1;

	// Token: 0x0400108B RID: 4235
	private string mMacAddress;

	// Token: 0x0400108C RID: 4236
	private string mPlatformUserID;

	// Token: 0x0400108D RID: 4237
	private string mPlatformUserAlias;
}
