using System;
using System.IO;
using MTools;
using UnityEngine;

// Token: 0x02000382 RID: 898
public class SoaringAdServer : SoaringObjectBase
{
	// Token: 0x0600199E RID: 6558 RVA: 0x000A7F44 File Offset: 0x000A6144
	public SoaringAdServer() : base(SoaringObjectBase.IsType.Object)
	{
		this.mSoaringAdDataReferences = new SoaringArray();
		this.mActiveAdRequests = new SoaringDictionary();
		string empty = string.Empty;
		this.mAdFilePath = ResourceUtils.GetFilePath(empty + "Soaring/AdServer", string.Empty, 9, false);
		SoaringInternal.instance.RegisterModule(new SoaringAdServerModule());
		this.LoadAdReferences();
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x000A7FA8 File Offset: 0x000A61A8
	public void RequestAd(string adName, bool displayAdOnComplete, SoaringContext context)
	{
		if (string.IsNullOrEmpty(adName))
		{
			adName = "kS_ANY";
		}
		this.mActiveAdRequests.removeObjectWithKey(adName);
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(Application.platform.ToString(), "platform");
		soaringDictionary.addValue(this.mAdServer + "advert.json?" + UnityEngine.Random.Range(0, int.MaxValue).ToString(), "turl");
		SoaringContext soaringContext = context;
		if (soaringContext == null)
		{
			soaringContext = new SoaringContext();
		}
		soaringContext.addValue(displayAdOnComplete, "display_advert");
		soaringContext.addValue(adName, "advert_name");
		if (!SoaringInternal.instance.CallModule("adServer", soaringDictionary, soaringContext))
		{
			this.HandleAdDownload(null, soaringContext);
		}
	}

	// Token: 0x060019A0 RID: 6560 RVA: 0x000A807C File Offset: 0x000A627C
	public bool AdAvailable(string adName)
	{
		if (string.IsNullOrEmpty(adName))
		{
			adName = "kS_ANY";
		}
		SoaringContext soaringContext = (SoaringContext)this.mActiveAdRequests.objectWithKey(adName);
		if (soaringContext == null)
		{
			return false;
		}
		SoaringAdData soaringAdData = (SoaringAdData)soaringContext.objectWithKey("advert_data");
		return soaringAdData != null && soaringAdData.AdTexture != null;
	}

	// Token: 0x060019A1 RID: 6561 RVA: 0x000A80DC File Offset: 0x000A62DC
	public bool DisplayAd(string adName)
	{
		if (string.IsNullOrEmpty(adName))
		{
			adName = "kS_ANY";
		}
		if (!this.AdAvailable(adName))
		{
			return false;
		}
		SoaringContext soaringContext = (SoaringContext)this.mActiveAdRequests.objectWithKey(adName);
		if (soaringContext == null)
		{
			return false;
		}
		this.mActiveAdRequests.removeObjectWithKey(adName);
		SoaringAdData soaringAdData = (SoaringAdData)soaringContext.objectWithKey("advert_data");
		return soaringAdData != null && SoaringAdView.CreateAdView(soaringAdData, this, soaringContext);
	}

	// Token: 0x060019A2 RID: 6562 RVA: 0x000A8158 File Offset: 0x000A6358
	public void SetAdServerURL(string url)
	{
		this.mAdServer = url;
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x000A8164 File Offset: 0x000A6364
	private void CleanupAds()
	{
		if (this.mSoaringAdDataReferences == null)
		{
			return;
		}
		bool flag = false;
		long adjustedServerTime = SoaringTime.AdjustedServerTime;
		for (int i = 0; i < this.mSoaringAdDataReferences.count(); i++)
		{
			SoaringAdData soaringAdData = (SoaringAdData)this.mSoaringAdDataReferences.objectAtIndex(i);
			if (adjustedServerTime > soaringAdData.AdExpires && soaringAdData.AdExpires > 0L)
			{
				this.mSoaringAdDataReferences.removeObjectAtIndex(i);
				if (!string.IsNullOrEmpty(soaringAdData.AdID))
				{
					try
					{
						if (File.Exists(this.mAdFilePath + "/" + soaringAdData.AdID))
						{
							File.Delete(this.mAdFilePath + "/" + soaringAdData.AdID);
						}
					}
					catch
					{
					}
				}
				flag = true;
				i--;
			}
		}
		if (flag)
		{
			this.SaveAdReferences();
		}
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x000A825C File Offset: 0x000A645C
	private SoaringAdData CheckAdExists(string adID)
	{
		SoaringAdData soaringAdData = null;
		if (this.mSoaringAdDataReferences == null)
		{
			return null;
		}
		for (int i = 0; i < this.mSoaringAdDataReferences.count(); i++)
		{
			soaringAdData = (SoaringAdData)this.mSoaringAdDataReferences.objectAtIndex(i);
			if (soaringAdData.AdID == adID)
			{
				break;
			}
			soaringAdData = null;
		}
		return soaringAdData;
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000A82C0 File Offset: 0x000A64C0
	private void SetAdReference(SoaringAdData data)
	{
		if (data == null)
		{
			return;
		}
		if (this.mSoaringAdDataReferences == null)
		{
			return;
		}
		for (int i = 0; i < this.mSoaringAdDataReferences.count(); i++)
		{
			SoaringAdData soaringAdData = (SoaringAdData)this.mSoaringAdDataReferences.objectAtIndex(i);
			if (data.AdID == soaringAdData.AdID)
			{
				this.mSoaringAdDataReferences.setObjectAtIndex(data, i);
				return;
			}
		}
		this.mSoaringAdDataReferences.addObject(data);
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x000A8340 File Offset: 0x000A6540
	public void MarkAdAsShown(SoaringAdData data)
	{
		if (data == null)
		{
			return;
		}
		data.SetAdShown();
		this.SaveAdReferences();
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x000A8358 File Offset: 0x000A6558
	internal void HandleAdRequestReturn(SoaringDictionary returnData, SoaringContext context)
	{
		this.CleanupAds();
		if (returnData == null)
		{
			this.HandleAdDownload(null, context);
			return;
		}
		if (this.mSoaringAdDataReferences == null)
		{
			this.LoadAdReferences();
			if (this.mSoaringAdDataReferences == null)
			{
				this.mSoaringAdDataReferences = new SoaringArray();
			}
		}
		SoaringArray soaringArray = null;
		string primaryPlatformName = SoaringPlatform.PrimaryPlatformName;
		if (!string.IsNullOrEmpty(primaryPlatformName) && returnData.containsKey(primaryPlatformName))
		{
			soaringArray = (SoaringArray)returnData.objectWithKey(primaryPlatformName);
		}
		if (soaringArray == null)
		{
			soaringArray = (SoaringArray)returnData.objectWithKey("Adverts");
		}
		if (soaringArray != null)
		{
			int num = soaringArray.count();
			if (num == 0)
			{
				this.HandleAdDownload(null, context);
				return;
			}
			SoaringArray soaringArray2 = new SoaringArray();
			int i = 0;
			while (i < num)
			{
				SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(i);
				string text = soaringDictionary.soaringValue("AdID");
				int num2 = soaringDictionary.soaringValue("AdNumShows");
				int num3 = 0;
				int num4 = 0;
				SoaringAdData soaringAdData = this.CheckAdExists(text);
				if (soaringAdData == null)
				{
					goto IL_122;
				}
				if ((int)soaringAdData.TimesDisplayed < num2 || num2 == -1)
				{
					num3 = (int)soaringAdData.TimesDisplayed;
					num4 = (int)soaringAdData.TimesClicked;
					goto IL_122;
				}
				IL_2E6:
				i++;
				continue;
				IL_122:
				long num5 = soaringDictionary.soaringValue("AdStart");
				long num6 = soaringDictionary.soaringValue("AdExpires");
				string text2 = soaringDictionary.soaringValue("AdStartTime");
				string text3 = soaringDictionary.soaringValue("AdEndTime");
				SoaringDictionary localizations = (SoaringDictionary)soaringDictionary.objectWithKey("AdLocalization");
				try
				{
					if (!string.IsNullOrEmpty(text2))
					{
						DateTime time = DateTime.Parse(text2).ToUniversalTime();
						num5 = MTime.TimeSinceEpoch(time);
					}
					if (!string.IsNullOrEmpty(text3))
					{
						DateTime time2 = DateTime.Parse(text3).ToUniversalTime();
						num6 = MTime.TimeSinceEpoch(time2);
					}
				}
				catch (Exception ex)
				{
					SoaringDebug.Log(ex.Message, LogType.Error);
				}
				if (num5 > 0L && num6 > 0L)
				{
					long adjustedServerTime = SoaringTime.AdjustedServerTime;
					if (num5 > adjustedServerTime || num6 < adjustedServerTime)
					{
						goto IL_2E6;
					}
				}
				string text4 = soaringDictionary.soaringValue("AdType");
				SoaringAdData.SoaringAdType adType = SoaringAdData.SoaringAdType.Other;
				text4 = text4.ToLower();
				if (text4 == "local")
				{
					adType = SoaringAdData.SoaringAdType.Local;
				}
				else if (text4 == "market")
				{
					adType = SoaringAdData.SoaringAdType.Market;
				}
				else if (text4 == "web")
				{
					adType = SoaringAdData.SoaringAdType.Web;
				}
				string path = soaringDictionary.soaringValue("Path");
				SoaringDictionary userData = (SoaringDictionary)soaringDictionary.objectWithKey("UserData");
				SoaringAdData soaringAdData2 = new SoaringAdData();
				soaringAdData2.SetData(null, text, path, num5, num6, num2, adType, localizations);
				soaringAdData2.SetUserData(userData);
				soaringAdData2.SetCachedData((short)num3, (short)num4);
				soaringArray2.addObject(soaringAdData2);
				this.SetAdReference(soaringAdData2);
				goto IL_2E6;
			}
			string text5 = context.soaringValue("advert_name");
			int num7 = 0;
			num = soaringArray2.count();
			if (num == 0)
			{
				this.HandleAdDownload(null, context);
				return;
			}
			if (num > 1)
			{
				num7 = (int)MTime.ConstantTimeStamp() % num;
			}
			if (num7 >= num)
			{
				this.HandleAdDownload(null, context);
				return;
			}
			bool flag = false;
			SoaringAdData soaringAdData3 = null;
			if (text5 != null && text5 != "kS_ANY")
			{
				flag = true;
				if (text5.Contains("%"))
				{
					flag = false;
				}
				else
				{
					for (int j = 0; j < soaringArray2.count(); j++)
					{
						SoaringAdData soaringAdData4 = (SoaringAdData)soaringArray2.objectAtIndex(j);
						if (soaringAdData4.AdID == text5)
						{
							soaringAdData3 = soaringAdData4;
							break;
						}
					}
				}
			}
			if (soaringAdData3 == null && !flag)
			{
				soaringAdData3 = (SoaringAdData)soaringArray2.objectAtIndex(num7);
			}
			if (soaringAdData3 == null || string.IsNullOrEmpty(soaringAdData3.AdID))
			{
				this.HandleAdDownload(null, context);
				return;
			}
			this.SaveAdReferences();
			if (string.IsNullOrEmpty(text5))
			{
				text5 = "kS_ANY";
			}
			context.addValue(soaringAdData3, "advert_data");
			this.mActiveAdRequests.addValue(context, text5);
			LanguageCode mSoaringLanguage = SoaringInternal.instance.mSoaringLanguage;
			string text6 = null;
			if (soaringAdData3.AdLocalizations != null)
			{
				SoaringValue soaringValue = (SoaringValue)soaringAdData3.AdLocalizations.objectWithKey(mSoaringLanguage.ToString().ToLower());
				if (soaringValue != null)
				{
					text6 = soaringValue;
				}
			}
			if (string.IsNullOrEmpty(text6))
			{
				text6 = soaringAdData3.AdID;
			}
			if (File.Exists(this.mAdFilePath + "/" + soaringAdData3.AdID))
			{
				this.AdCallback(text5, true, this.mAdFilePath + "/" + soaringAdData3.AdID);
			}
			else
			{
				SoaringInternal.instance.DownloadFileWithSoaring(text5, this.mAdServer + text6, this.mAdFilePath + "/" + soaringAdData3.AdID, new SCWebQueue.SCDownloadCallback(this.AdCallback));
			}
		}
		else
		{
			this.HandleAdDownload(null, context);
		}
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x000A88A8 File Offset: 0x000A6AA8
	private void AdCallback(string id, bool success, string path)
	{
		SoaringContext soaringContext = (SoaringContext)this.mActiveAdRequests.objectWithKey(id);
		if (soaringContext == null)
		{
			SoaringDebug.Log("SoaringAdModule: Error: No Context", LogType.Error);
			this.HandleAdDownload(null, soaringContext);
		}
		SoaringAdData soaringAdData = (SoaringAdData)soaringContext.objectWithKey("advert_data");
		if (success && soaringAdData != null)
		{
			if (path != null)
			{
				if (!File.Exists(path))
				{
					this.HandleAdDownload(null, soaringContext);
					return;
				}
				byte[] array = null;
				try
				{
					MBinaryReader mbinaryReader = new MBinaryReader(path);
					if (!mbinaryReader.IsOpen())
					{
						this.HandleAdDownload(null, soaringContext);
						return;
					}
					array = mbinaryReader.ReadAllBytes();
					mbinaryReader.Close();
				}
				catch (Exception ex)
				{
					SoaringDebug.Log(ex.Message, LogType.Error);
					soaringAdData = null;
				}
				try
				{
					if (array != null && soaringAdData != null)
					{
						Texture2D texture2D = new Texture2D(0, 0, TextureFormat.RGB24, false);
						if (texture2D.LoadImage(array))
						{
							soaringAdData.SetData(texture2D, soaringAdData.AdID, soaringAdData.Path, soaringAdData.AdStarts, soaringAdData.AdExpires, (int)soaringAdData.TimesWillBeDisplayed, soaringAdData.AdType, soaringAdData.AdLocalizations);
						}
						else
						{
							soaringAdData = null;
						}
					}
					else
					{
						soaringAdData = null;
					}
				}
				catch (Exception ex2)
				{
					SoaringDebug.Log(ex2.Message, LogType.Error);
					soaringAdData = null;
				}
			}
		}
		else
		{
			soaringAdData = null;
		}
		this.HandleAdDownload(soaringAdData, soaringContext);
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x000A8A28 File Offset: 0x000A6C28
	private void HandleAdDownload(SoaringAdData adData, SoaringContext context)
	{
		if (adData != null)
		{
			bool flag = context.soaringValue("display_advert");
			if (flag)
			{
				this.DisplayAd(context.soaringValue("advert_name"));
			}
		}
		bool flag2 = adData != null;
		Soaring.Delegate.OnAdServed(flag2, adData, (!flag2) ? SoaringAdServerState.Failed : SoaringAdServerState.Retrieved, context);
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x000A8A8C File Offset: 0x000A6C8C
	private void LoadAdReferences()
	{
		if (this.mSoaringAdDataReferences == null)
		{
			return;
		}
		this.mSoaringAdDataReferences.clear();
		MBinaryReader mbinaryReader = new MBinaryReader(this.mAdFilePath + "/SoaringAD.ad");
		if (mbinaryReader == null)
		{
			return;
		}
		if (!mbinaryReader.IsOpen())
		{
			return;
		}
		try
		{
			if (mbinaryReader.ReadInt() == 0)
			{
				int num = mbinaryReader.ReadInt();
				for (int i = 0; i < num; i++)
				{
					string addID = mbinaryReader.ReadString();
					long expires = mbinaryReader.ReadLong();
					long starts = mbinaryReader.ReadLong();
					SoaringAdData.SoaringAdType adType = (SoaringAdData.SoaringAdType)mbinaryReader.ReadInt();
					short mAdDisplays = mbinaryReader.ReadShort();
					short shown = mbinaryReader.ReadShort();
					short clicks = mbinaryReader.ReadShort();
					string path = mbinaryReader.ReadString();
					SoaringAdData soaringAdData = new SoaringAdData();
					soaringAdData.SetData(null, addID, path, starts, expires, (int)mAdDisplays, adType, null);
					soaringAdData.SetCachedData(shown, clicks);
					this.mSoaringAdDataReferences.addObject(soaringAdData);
				}
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringAdServer: " + ex.Message);
		}
		mbinaryReader.Close();
		mbinaryReader = null;
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x000A8BBC File Offset: 0x000A6DBC
	private void SaveAdReferences()
	{
		if (this.mSoaringAdDataReferences == null)
		{
			return;
		}
		MBinaryWriter mbinaryWriter = new MBinaryWriter(this.mAdFilePath + "/SoaringAD.ad");
		if (mbinaryWriter == null)
		{
			return;
		}
		if (!mbinaryWriter.IsOpen())
		{
			return;
		}
		try
		{
			mbinaryWriter.Write(0);
			int num = this.mSoaringAdDataReferences.count();
			mbinaryWriter.Write(num);
			for (int i = 0; i < num; i++)
			{
				SoaringAdData soaringAdData = (SoaringAdData)this.mSoaringAdDataReferences.objectAtIndex(i);
				mbinaryWriter.Write(soaringAdData.AdID);
				mbinaryWriter.Write(soaringAdData.AdExpires);
				mbinaryWriter.Write(soaringAdData.AdStarts);
				mbinaryWriter.Write((int)soaringAdData.AdType);
				mbinaryWriter.Write(soaringAdData.TimesWillBeDisplayed);
				mbinaryWriter.Write(soaringAdData.TimesDisplayed);
				mbinaryWriter.Write(soaringAdData.TimesClicked);
				mbinaryWriter.Write(soaringAdData.Path);
			}
			mbinaryWriter.Flush();
		}
		catch
		{
		}
		mbinaryWriter.Close();
		mbinaryWriter = null;
	}

	// Token: 0x04001093 RID: 4243
	private const int kFormatVersionNumber = 0;

	// Token: 0x04001094 RID: 4244
	private const string kAnyAdvert = "kS_ANY";

	// Token: 0x04001095 RID: 4245
	private const string kAdvertDisplay = "display_advert";

	// Token: 0x04001096 RID: 4246
	private const string kAdvertName = "advert_name";

	// Token: 0x04001097 RID: 4247
	private const string kAdvertData = "advert_data";

	// Token: 0x04001098 RID: 4248
	private string mAdServer;

	// Token: 0x04001099 RID: 4249
	private string mAdFilePath;

	// Token: 0x0400109A RID: 4250
	private SoaringArray mSoaringAdDataReferences;

	// Token: 0x0400109B RID: 4251
	private SoaringDictionary mActiveAdRequests;
}
