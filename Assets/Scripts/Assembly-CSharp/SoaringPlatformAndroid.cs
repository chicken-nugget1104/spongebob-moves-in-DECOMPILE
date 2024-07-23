using System;
using System.Text;
using UnityEngine;

// Token: 0x0200037D RID: 893
public class SoaringPlatformAndroid : SoaringPlatform.SoaringPlatformDelegate
{
	// Token: 0x0600195F RID: 6495 RVA: 0x000A7230 File Offset: 0x000A5430
	public override void Init()
	{
		this.mAndroidID = string.Empty;
		this.mIMEI = string.Empty;
		try
		{
			this.cls_Soaring = new AndroidJavaClass("com.fws.soaring.Soaring");
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					this.cls_Soaring.CallStatic<bool>("Init", new object[]
					{
						@static,
						string.Empty
					});
				}
				this.mIMEI = this.IMEI;
				this.mAndroidID = this.AndroidID;
				this.mMacAddresses = this.MacAddresses;
				this.mTotalMemory = this.TotalMemory;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Soaring: " + ex.Message, LogType.Error);
		}
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x000A7360 File Offset: 0x000A5560
	public override string PlatformName()
	{
		return "android";
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x000A7368 File Offset: 0x000A5568
	public override bool PlatformLoginAvailable()
	{
		return false;
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x000A736C File Offset: 0x000A556C
	public override bool PlatformAuthenticated()
	{
		return false;
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x000A7370 File Offset: 0x000A5570
	public override bool PlatformAuthenticate(SoaringContext context)
	{
		return false;
	}

	// Token: 0x06001964 RID: 6500 RVA: 0x000A7374 File Offset: 0x000A5574
	public override string PlatformID()
	{
		return null;
	}

	// Token: 0x06001965 RID: 6501 RVA: 0x000A7378 File Offset: 0x000A5578
	public override string PlatformAlias()
	{
		return null;
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x000A737C File Offset: 0x000A557C
	public override string DeviceID()
	{
		string text = this.AndroidID;
		if (string.IsNullOrEmpty(text))
		{
			text = SystemInfo.deviceUniqueIdentifier;
		}
		return text;
	}

	// Token: 0x06001967 RID: 6503 RVA: 0x000A73A4 File Offset: 0x000A55A4
	public override SoaringDictionary GenerateDeviceDictionary()
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(this.PlatformName(), "platform");
		string text = this.IMEI;
		if (Application.isEditor)
		{
			text = "00000000000000";
		}
		if (!string.IsNullOrEmpty(text))
		{
			soaringDictionary.addValue(text, "imei");
		}
		string androidID = this.AndroidID;
		if (!string.IsNullOrEmpty(androidID))
		{
			soaringDictionary.addValue(androidID, "android_id");
		}
		soaringDictionary.addValue(SystemInfo.deviceUniqueIdentifier, "unique");
		soaringDictionary.addValue(this.DeviceID(), "deviceId");
		return soaringDictionary;
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x000A7450 File Offset: 0x000A5650
	public override string PushNotificationsProtocol()
	{
		return "gcm";
	}

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06001969 RID: 6505 RVA: 0x000A7458 File Offset: 0x000A5658
	public string IMEI
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mIMEI))
			{
				return this.mIMEI;
			}
			try
			{
				this.mIMEI = this.cls_Soaring.CallStatic<string>("GetIMEI", new object[0]);
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("Soaring: " + ex.Message, LogType.Warning);
				this.mIMEI = string.Empty;
			}
			if (this.mIMEI == null)
			{
				this.mIMEI = string.Empty;
			}
			return this.mIMEI;
		}
	}

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x0600196A RID: 6506 RVA: 0x000A74FC File Offset: 0x000A56FC
	public string AndroidID
	{
		get
		{
			if (!string.IsNullOrEmpty(this.mAndroidID))
			{
				return this.mAndroidID;
			}
			try
			{
				this.mAndroidID = this.cls_Soaring.CallStatic<string>("GetAndroidID", new object[0]);
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("Error: " + ex.Message, LogType.Warning);
				this.mAndroidID = string.Empty;
			}
			if (this.mAndroidID == null)
			{
				this.mAndroidID = string.Empty;
			}
			return this.mAndroidID;
		}
	}

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x0600196B RID: 6507 RVA: 0x000A75A0 File Offset: 0x000A57A0
	public string[] MacAddresses
	{
		get
		{
			if (this.mMacAddresses != null)
			{
				return this.mMacAddresses;
			}
			try
			{
				string text = this.cls_Soaring.CallStatic<string>("GetAllMACAddress", new object[0]);
				if (!string.IsNullOrEmpty(text))
				{
					char[] separator = new char[]
					{
						','
					};
					this.mMacAddresses = text.Split(separator);
					for (int i = 0; i < this.mMacAddresses.Length; i++)
					{
						string text2 = SoaringVersions.CalculateMD5Hash(Encoding.UTF8.GetBytes(this.mMacAddresses[i]));
						if (text2.Length > 2)
						{
							text2 = text2.Substring(0, text2.Length - 2);
						}
						this.mMacAddresses[i] = text2;
					}
				}
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("Error: " + ex.Message, LogType.Warning);
				this.mMacAddresses = null;
			}
			if (this.mMacAddresses == null)
			{
				this.mMacAddresses = null;
			}
			return this.mMacAddresses;
		}
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x0600196C RID: 6508 RVA: 0x000A76B0 File Offset: 0x000A58B0
	public long TotalMemory
	{
		get
		{
			if (this.mTotalMemory != 0L)
			{
				return this.mTotalMemory;
			}
			try
			{
				this.mTotalMemory = this.cls_Soaring.CallStatic<long>("GetTotalMemory", new object[0]);
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("TotalMemory: " + ex.Message, LogType.Warning);
				this.mTotalMemory = -1L;
			}
			return this.mTotalMemory;
		}
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x000A7738 File Offset: 0x000A5938
	public override bool OpenURL(string url)
	{
		bool result = false;
		if (url == null)
		{
			return result;
		}
		try
		{
			int num = this.cls_Soaring.CallStatic<int>("OpenURL", new object[]
			{
				url
			});
			if (num == 1)
			{
				result = true;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("OpenURL: " + ex.Message, LogType.Warning);
		}
		return result;
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x000A77B4 File Offset: 0x000A59B4
	public override bool SendEmail(string subject, string body, string email)
	{
		bool result = false;
		if (subject == null || email == null || body == null)
		{
			return result;
		}
		try
		{
			result = this.cls_Soaring.CallStatic<bool>("SendEmail", new object[]
			{
				subject,
				body,
				email
			});
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SendEmail: " + ex.Message, LogType.Warning);
		}
		return result;
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x000A7838 File Offset: 0x000A5A38
	public void OpenDialog(string title, string body, string button)
	{
		try
		{
			this.cls_Soaring.CallStatic("OpenDialog", new object[]
			{
				title,
				body,
				button
			});
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Soaring: " + ex.Message, LogType.Warning);
		}
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x000A78A4 File Offset: 0x000A5AA4
	public override long SystemBootTime()
	{
		long result = 0L;
		try
		{
			result = this.cls_Soaring.CallStatic<long>("DeviceBootTime", new object[0]);
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Time: " + ex.Message, LogType.Warning);
		}
		return result;
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x000A790C File Offset: 0x000A5B0C
	public override long SystemTimeSinceBootTime()
	{
		long result = 0L;
		try
		{
			result = this.cls_Soaring.CallStatic<long>("DeviceTimeSinceBoot", new object[0]);
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Time: " + ex.Message, LogType.Warning);
		}
		return result;
	}

	// Token: 0x04001081 RID: 4225
	private AndroidJavaClass cls_Soaring;

	// Token: 0x04001082 RID: 4226
	private string mAndroidID;

	// Token: 0x04001083 RID: 4227
	private string mIMEI;

	// Token: 0x04001084 RID: 4228
	private string[] mMacAddresses;

	// Token: 0x04001085 RID: 4229
	private long mTotalMemory;
}
