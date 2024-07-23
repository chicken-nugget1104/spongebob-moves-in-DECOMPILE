using System;
using System.IO;
using MTools;
using UnityEngine;

// Token: 0x02000385 RID: 901
public class SoaringAnalytics
{
	// Token: 0x17000350 RID: 848
	// (get) Token: 0x060019B6 RID: 6582 RVA: 0x000A8FD8 File Offset: 0x000A71D8
	public static ulong DeviceSequenceID
	{
		get
		{
			ulong result = SoaringAnalytics.mGUIDSequenceID;
			SoaringAnalytics.mGUIDSequenceID += 1UL;
			return result;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x060019B7 RID: 6583 RVA: 0x000A8FFC File Offset: 0x000A71FC
	public static string DeviceGUID
	{
		get
		{
			if (string.IsNullOrEmpty(SoaringAnalytics.mDeviceGUID))
			{
				SoaringAnalytics.LoadSoaringAnalytics();
				SoaringAnalytics.mDeviceGUID = SoaringAnalytics.GenerateGUID();
			}
			return SoaringAnalytics.mDeviceGUID;
		}
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x000A9024 File Offset: 0x000A7224
	public static SoaringDictionary StampDeviceMetadata()
	{
		if (SoaringAnalytics.sMetaData == null)
		{
			SoaringAnalytics.sMetaData = new SoaringDictionary();
			SoaringAnalytics.sMetaData.addValue_unsafe(new SoaringValue(SoaringPlatform.PrimaryPlatformName), "platform");
			SoaringAnalytics.sMetaData.addValue_unsafe(new SoaringValue(SystemInfo.operatingSystem), "os_version");
			SoaringAnalytics.sMetaData.addValue_unsafe(new SoaringValue(SystemInfo.deviceModel), "device_name");
		}
		SoaringDictionary soaringDictionary = SoaringAnalytics.sMetaData.makeCopy();
		soaringDictionary.addValue_unsafe(new SoaringValue(SoaringAnalytics.GenerateGUID()), "guid");
		soaringDictionary.addValue_unsafe(new SoaringValue(SoaringAnalytics.DeviceSequenceID), "device_seq_num");
		soaringDictionary.addValue_unsafe(new SoaringValue(SoaringAnalytics.DeviceGUID), "device_seq_id");
		soaringDictionary.addValue_unsafe(new SoaringValue(SoaringAnalytics.AnalyticTime()), "event_time");
		return soaringDictionary;
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x000A90F4 File Offset: 0x000A72F4
	private static void LoadSoaringAnalytics()
	{
		if (!string.IsNullOrEmpty(SoaringAnalytics.mDeviceGUID))
		{
			return;
		}
		MBinaryReader mbinaryReader = null;
		try
		{
			mbinaryReader = ResourceUtils.GetFileStream("SoaringAnalytic", "Soaring", "dat", 1);
			if (mbinaryReader != null && mbinaryReader.IsOpen())
			{
				if (mbinaryReader.ReadInt() == 1)
				{
					SoaringAnalytics.mDeviceGUID = mbinaryReader.ReadString();
					SoaringAnalytics.mGUIDSequenceID = mbinaryReader.ReadULong();
				}
				mbinaryReader.Close();
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Failed to Load SoaringAnalytics.dat: " + ex.Message + "\n" + ex.StackTrace, LogType.Error);
			try
			{
				if (mbinaryReader != null)
				{
					mbinaryReader.Close();
				}
			}
			catch
			{
			}
		}
		mbinaryReader = null;
		try
		{
			if (string.IsNullOrEmpty(SoaringAnalytics.mDeviceGUID))
			{
				SoaringAnalytics.mDeviceGUID = SoaringAnalytics.GenerateGUID();
				SoaringAnalytics.mGUIDSequenceID = 0UL;
				SoaringAnalytics.SaveSoaringAnalyticFile();
			}
		}
		catch (Exception ex2)
		{
			SoaringDebug.Log("Failed to Verify SoaringAnalytics.dat: " + ex2.Message + "\n" + ex2.StackTrace, LogType.Error);
		}
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x000A9244 File Offset: 0x000A7444
	public void Initialize()
	{
		if (this.mInitialized)
		{
			return;
		}
		if (this.mBuffersData == null)
		{
			return;
		}
		string[] array = new string[]
		{
			"Analytics",
			"Anonymous"
		};
		Debug.Log("Soaring Initialize Analytics");
		SoaringAnalytics.mRandSeed = (uint)SoaringAnalytics.AnalyticTime();
		SoaringAnalytics.LoadSoaringAnalytics();
		for (int i = 0; i < this.mBuffersData.Length; i++)
		{
			this.mBuffersData[i] = new SoaringAnalytics.BufferContainer();
			this.mBuffersData[i].mBuffer = new SoaringAnalytics.Buffer();
			this.mBuffersData[i].mBufferTemp = new SoaringAnalytics.Buffer();
			try
			{
				this.mBuffersData[i].mBuffer.Open(ResourceUtils.GetWritePath("Soaring" + array[i] + "Buffer.json", "Soaring", 1));
				this.mBuffersData[i].mBufferTemp.Open(ResourceUtils.GetWritePath("Soaring" + array[i] + "BufferTemp.json", "Soaring", 1));
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Initialization Failed: " + text + "\n" + ex.StackTrace);
			}
		}
		this.mInitialized = true;
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x000A93A0 File Offset: 0x000A75A0
	private static void SaveSoaringAnalyticFile()
	{
		if (string.IsNullOrEmpty(SoaringAnalytics.mDeviceGUID))
		{
			return;
		}
		MBinaryWriter mbinaryWriter = null;
		try
		{
			string writePath = ResourceUtils.GetWritePath("SoaringAnalytic.dat", "Soaring", 1);
			mbinaryWriter = new MBinaryWriter();
			if (mbinaryWriter.Open(writePath, true, true, "bak"))
			{
				mbinaryWriter.Write(1);
				mbinaryWriter.Write(SoaringAnalytics.mDeviceGUID);
				mbinaryWriter.Write(SoaringAnalytics.mGUIDSequenceID);
				mbinaryWriter.Close();
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("Failed to Save SoaringAnalytics.dat: " + ex.Message + "\n" + ex.StackTrace);
			try
			{
				if (mbinaryWriter != null)
				{
					mbinaryWriter.Close();
				}
			}
			catch
			{
			}
		}
		mbinaryWriter = null;
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x000A9484 File Offset: 0x000A7684
	public void Shutdown()
	{
		if (this.mBuffersData != null)
		{
			for (int i = 0; i < this.mBuffersData.Length; i++)
			{
				if (this.mBuffersData[i] != null)
				{
					this.mBuffersData[i].mBuffer.Close();
					this.mBuffersData[i].mBufferTemp.Close();
				}
			}
		}
		SoaringAnalytics.SaveSoaringAnalyticFile();
		this.mInitialized = false;
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x000A94F8 File Offset: 0x000A76F8
	public void LogAnonymousEvent(string key, SoaringObjectBase value)
	{
		if (!this.mInitialized)
		{
			return;
		}
		this.LogEvent(key, value, 1);
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x000A9510 File Offset: 0x000A7710
	public void LogAnonymousEvents(SoaringArray entries)
	{
		if (!this.mInitialized)
		{
			return;
		}
		this.LogEvents(entries, 1);
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x000A9528 File Offset: 0x000A7728
	public void LogEvent(string key, SoaringObjectBase value, int logIndex = 0)
	{
		if (!this.mInitialized)
		{
			return;
		}
		SoaringArray soaringArray = new SoaringArray(1);
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringArray.addObject(soaringDictionary);
		soaringDictionary.addValue(key, "key");
		soaringDictionary.addValue(value, "value");
		this.LogEvents(soaringArray, logIndex);
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000A957C File Offset: 0x000A777C
	public void LogEvents(SoaringArray entries, int logIndex = 0)
	{
		if (!this.mInitialized)
		{
			return;
		}
		if (entries == null)
		{
			return;
		}
		int num = entries.count();
		switch (this.mEmbededGUIDType)
		{
		case SoaringAnalytics.EmbededGUIDType.AllEntries:
			for (int i = 0; i < num; i++)
			{
				SoaringDictionary soaringDictionary = (SoaringDictionary)entries.objectAtIndex(i);
				if (soaringDictionary != null)
				{
					soaringDictionary.addValue(SoaringAnalytics.GenerateGUID(), "guid");
				}
			}
			break;
		case SoaringAnalytics.EmbededGUIDType.AllValues:
			for (int j = 0; j < num; j++)
			{
				SoaringDictionary soaringDictionary2 = (SoaringDictionary)entries.objectAtIndex(j);
				if (soaringDictionary2 != null)
				{
					SoaringDictionary soaringDictionary3 = (SoaringDictionary)soaringDictionary2.objectWithKey("value");
					if (soaringDictionary3 != null)
					{
						soaringDictionary3.addValue(SoaringAnalytics.GenerateGUID(), "guid");
					}
				}
			}
			break;
		}
		if (!SoaringInternal.IsProductionMode && SoaringAnalytics._bERROR_LOG)
		{
			SoaringDebug.Log("SoaringAnalytics.cs | Logging events: " + entries.ToJsonString());
		}
		if (this.mBuffersData[logIndex].mWaitingForResponse)
		{
			this.mBuffersData[logIndex].mBufferTemp.Append(entries);
		}
		else
		{
			this.mBuffersData[logIndex].mBuffer.Append(entries);
		}
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x000A96D8 File Offset: 0x000A78D8
	public void Update(float deltaTime)
	{
		if (!this.mInitialized || !Soaring.IsInitialized)
		{
			return;
		}
		for (int i = 0; i < this.mBuffersData.Length; i++)
		{
			if (!this.mBuffersData[i].mWaitingForResponse)
			{
				this.mBuffersData[i].mUpdateTime += deltaTime;
				if (this.mBuffersData[i].mUpdateTime >= this.mBuffersData[i].mUpdateInterval)
				{
					this.mBuffersData[i].mUpdateTime = 0f;
					this.mBuffersData[i].mBuffer.Append(this.mBuffersData[i].mBufferTemp.GetData());
					this.mBuffersData[i].mBufferTemp.Clear();
					SoaringArray data = this.mBuffersData[i].mBuffer.GetData();
					if (data.count() > 0)
					{
						this.mBuffersData[i].mWaitingForResponse = true;
						SoaringContext soaringContext = "SoaringAnalyticsNew";
						soaringContext.Responder = new SoaringAnalytics.SoaringAnalyticsDelegate(this);
						if (i == 0)
						{
							SoaringInternal.instance.internal_SaveStat(data, soaringContext);
						}
						else
						{
							SoaringInternal.instance.internal_SaveAnonymousStat(data, soaringContext);
						}
						SoaringAnalytics.SaveSoaringAnalyticFile();
					}
				}
			}
		}
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x000A9820 File Offset: 0x000A7A20
	public void _OnSaveStat(bool success, int nLogIndex, SoaringError error, SoaringContext context)
	{
		if (this.mBuffersData[nLogIndex] == null)
		{
			return;
		}
		if (!this.mBuffersData[nLogIndex].mWaitingForResponse)
		{
			return;
		}
		if (success)
		{
			this.mBuffersData[nLogIndex].mBuffer.Clear();
		}
		else if (Soaring.IsOnline)
		{
			this.mBuffersData[nLogIndex].mUpdateTime = this.mBuffersData[nLogIndex].mUpdateInterval;
		}
		else
		{
			this.mBuffersData[nLogIndex].mUpdateTime = 0f;
		}
		this.mBuffersData[nLogIndex].mWaitingForResponse = false;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x000A98B4 File Offset: 0x000A7AB4
	public static ulong AnalyticTime()
	{
		return (ulong)SoaringTime.AdjustedServerTime;
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x000A98BC File Offset: 0x000A7ABC
	public static string GenerateGUID()
	{
		return SoaringAnalytics.AnalyticTime().ToString() + ((ushort)SoaringAnalytics.Fast_Rand()).ToString() + ((ushort)(Environment.TickCount & int.MaxValue)).ToString();
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x000A9900 File Offset: 0x000A7B00
	public static uint Fast_Rand()
	{
		uint num = (1U & SoaringAnalytics.mRandSeed) + 1U;
		for (uint num2 = 0U; num2 < num; num2 += 1U)
		{
			SoaringAnalytics.mRandVal += SoaringAnalytics.mRandSeed;
		}
		return SoaringAnalytics.mRandVal;
	}

	// Token: 0x040010A6 RID: 4262
	private const int kStandardLog = 0;

	// Token: 0x040010A7 RID: 4263
	private const int kAnonymousLog = 1;

	// Token: 0x040010A8 RID: 4264
	private static bool _bERROR_LOG = true;

	// Token: 0x040010A9 RID: 4265
	private static string mDeviceGUID;

	// Token: 0x040010AA RID: 4266
	private static ulong mGUIDSequenceID;

	// Token: 0x040010AB RID: 4267
	private static SoaringDictionary sMetaData;

	// Token: 0x040010AC RID: 4268
	public bool mInitialized;

	// Token: 0x040010AD RID: 4269
	private SoaringAnalytics.BufferContainer[] mBuffersData = new SoaringAnalytics.BufferContainer[2];

	// Token: 0x040010AE RID: 4270
	private static uint mRandVal = 2147483647U;

	// Token: 0x040010AF RID: 4271
	private static uint mRandSeed = 2147483647U;

	// Token: 0x040010B0 RID: 4272
	private SoaringAnalytics.EmbededGUIDType mEmbededGUIDType;

	// Token: 0x02000386 RID: 902
	private class Buffer
	{
		// Token: 0x060019C6 RID: 6598 RVA: 0x000A9940 File Offset: 0x000A7B40
		public Buffer()
		{
			this.mData = new SoaringArray();
			this.mFilestream = null;
			this.mStreamWriter = null;
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x000A9964 File Offset: 0x000A7B64
		public SoaringArray GetData()
		{
			return this.mData;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x000A996C File Offset: 0x000A7B6C
		public void Open(string filepath)
		{
			this.mData = new SoaringArray();
			try
			{
				string directoryName = Path.GetDirectoryName(filepath);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				if (!File.Exists(filepath))
				{
					this.mFilestream = File.Create(filepath);
				}
				else
				{
					this.mFilestream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite);
				}
			}
			catch (Exception ex)
			{
				this.mFilestream = null;
				this.mStreamWriter = null;
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Buffer Open Failed: " + text + "\n" + ex.StackTrace);
			}
			if (this.mFilestream != null)
			{
				try
				{
					if (this.mFilestream.Length > 0L)
					{
						this.mFilestream.Seek(0L, SeekOrigin.Begin);
						StreamReader streamReader = new StreamReader(this.mFilestream);
						string text2 = streamReader.ReadToEnd();
						string[] array = text2.Split(new char[]
						{
							'|'
						});
						int num = array.Length;
						for (int i = 0; i < num; i++)
						{
							SoaringDictionary soaringDictionary = new SoaringDictionary(array[i]);
							SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectWithKey("entries");
							int num2 = soaringArray.count();
							for (int j = 0; j < num2; j++)
							{
								this.mData.addObject(soaringArray.objectAtIndex(j));
							}
						}
						this.mFilestream.Seek(this.mFilestream.Length, SeekOrigin.Begin);
					}
					this.mStreamWriter = new StreamWriter(this.mFilestream);
				}
				catch (Exception ex2)
				{
					this.mFilestream = null;
					this.mStreamWriter = null;
					string text3 = ex2.Message;
					if (text3 == null)
					{
						text3 = string.Empty;
					}
					SoaringDebug.Log("Soaring Analytics Buffer Read Failed: " + text3 + "\n" + ex2.StackTrace);
				}
			}
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x000A9B80 File Offset: 0x000A7D80
		public void Append(SoaringArray mAppendData)
		{
			if (this.mFilestream == null || this.mStreamWriter == null)
			{
				return;
			}
			if (this.mFilestream.Length >= (long)SoaringInternalProperties.AnalyticsBufferSize)
			{
				return;
			}
			int num = mAppendData.count();
			if (num <= 0)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				this.mData.addObject(mAppendData.objectAtIndex(i));
			}
			try
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(mAppendData, "entries");
				string value;
				if (this.mFilestream.Length <= 0L)
				{
					value = soaringDictionary.ToJsonString();
				}
				else
				{
					value = '|' + soaringDictionary.ToJsonString();
				}
				this.mStreamWriter.Write(value);
				this.mStreamWriter.Flush();
				this.mFilestream.Flush();
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Buffer Append Failed: " + text + "\n" + ex.StackTrace);
			}
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x000A9CB0 File Offset: 0x000A7EB0
		public void Clear()
		{
			if (this.mFilestream == null || this.mStreamWriter == null)
			{
				return;
			}
			this.mData.clear();
			try
			{
				this.mStreamWriter = null;
				this.mFilestream.SetLength(0L);
				this.mStreamWriter = new StreamWriter(this.mFilestream);
			}
			catch (Exception ex)
			{
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Buffer Clear Failed: " + text + "\n" + ex.StackTrace);
			}
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x000A9D5C File Offset: 0x000A7F5C
		public void Close()
		{
			if (this.mFilestream == null || this.mStreamWriter == null)
			{
				return;
			}
			try
			{
				this.mFilestream.Close();
				this.mFilestream = null;
				this.mStreamWriter = null;
			}
			catch (Exception ex)
			{
				this.mFilestream = null;
				this.mStreamWriter = null;
				string text = ex.Message;
				if (text == null)
				{
					text = string.Empty;
				}
				SoaringDebug.Log("Soaring Analytics Buffer Close Failed: " + text + "\n" + ex.StackTrace);
			}
		}

		// Token: 0x040010B1 RID: 4273
		private SoaringArray mData;

		// Token: 0x040010B2 RID: 4274
		private FileStream mFilestream;

		// Token: 0x040010B3 RID: 4275
		private StreamWriter mStreamWriter;
	}

	// Token: 0x02000387 RID: 903
	private class SoaringAnalyticsDelegate : SoaringDelegate
	{
		// Token: 0x060019CC RID: 6604 RVA: 0x000A9DFC File Offset: 0x000A7FFC
		public SoaringAnalyticsDelegate(SoaringAnalytics analytics)
		{
			this.mAnalytics = analytics;
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x000A9E0C File Offset: 0x000A800C
		public override void OnSaveStat(bool success, bool anonymous, SoaringError error, SoaringContext context)
		{
			if (this.mAnalytics != null)
			{
				if (anonymous)
				{
					this.mAnalytics._OnSaveStat(success, 1, error, context);
				}
				else
				{
					this.mAnalytics._OnSaveStat(success, 0, error, context);
				}
			}
		}

		// Token: 0x040010B4 RID: 4276
		private SoaringAnalytics mAnalytics;
	}

	// Token: 0x02000388 RID: 904
	private class BufferContainer
	{
		// Token: 0x040010B5 RID: 4277
		public SoaringAnalytics.Buffer mBuffer;

		// Token: 0x040010B6 RID: 4278
		public SoaringAnalytics.Buffer mBufferTemp;

		// Token: 0x040010B7 RID: 4279
		public bool mWaitingForResponse;

		// Token: 0x040010B8 RID: 4280
		public float mUpdateTime;

		// Token: 0x040010B9 RID: 4281
		public float mUpdateInterval = 10f;
	}

	// Token: 0x02000389 RID: 905
	public enum EmbededGUIDType
	{
		// Token: 0x040010BB RID: 4283
		None,
		// Token: 0x040010BC RID: 4284
		AllEntries,
		// Token: 0x040010BD RID: 4285
		AllValues
	}
}
