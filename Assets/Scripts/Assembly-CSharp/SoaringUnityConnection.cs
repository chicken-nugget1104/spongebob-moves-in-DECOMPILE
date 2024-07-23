using System;
using MTools;
using UnityEngine;

// Token: 0x02000393 RID: 915
public class SoaringUnityConnection : SoaringConnection
{
	// Token: 0x06001A19 RID: 6681 RVA: 0x000AAC1C File Offset: 0x000A8E1C
	public override bool Create(SCWebQueue.SCData properties)
	{
		if (properties == null)
		{
			return false;
		}
		base.Create(properties);
		string text = this.mProperties.URL;
		if (this.CacheVersion != -1)
		{
			this.mConnection = WWW.LoadFromCacheOrDownload(text, this.CacheVersion);
			return true;
		}
		if (properties.GetParams != null)
		{
			SoaringDictionary getParams = properties.GetParams;
			if (getParams.count() != 0)
			{
				text += "?";
				string[] array = getParams.allKeys();
				SoaringObjectBase[] array2 = getParams.allValues();
				int num = getParams.count();
				for (int i = 0; i < num; i++)
				{
					text += array[i];
					text += "=";
					text += array2[i].ToString();
					if (i + 1 < num)
					{
						text += "@";
					}
				}
			}
		}
		WWWForm wwwform = null;
		if (properties.PostParams != null)
		{
			SoaringDictionary postParams = properties.PostParams;
			if (postParams.count() != 0)
			{
				wwwform = new WWWForm();
				wwwform.headers.Add("content-type", "application/x-www-form-urlencoded");
				wwwform.headers.Add("soaring-sdk", SCWebQueue.ReportedSDK);
				string[] array3 = postParams.allKeys();
				SoaringObjectBase[] array4 = postParams.allValues();
				int num2 = postParams.count();
				bool flag = SoaringDebug.IsLoggingToConsole & SoaringDebug.IsLoggingToFile;
				for (int j = 0; j < num2; j++)
				{
					string text2 = array4[j].ToString();
					wwwform.AddField(array3[j], text2);
					if (flag)
					{
						SoaringDebug.Log(array3[j] + ":" + text2);
					}
				}
			}
		}
		if (wwwform != null)
		{
			this.mConnection = new WWW(text, wwwform);
		}
		else
		{
			this.mConnection = new WWW(text);
		}
		return this.IsValid;
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x000AADF0 File Offset: 0x000A8FF0
	public override bool SaveData()
	{
		bool result = false;
		if (this.mProperties == null)
		{
			return result;
		}
		string saveLocation = this.mProperties.SaveLocation;
		if (string.IsNullOrEmpty(saveLocation))
		{
			return result;
		}
		if (this.CacheVersion != -1)
		{
			return result;
		}
		try
		{
			MBinaryWriter mbinaryWriter = new MBinaryWriter();
			if (mbinaryWriter.Open(this.mProperties.SaveLocation, true, true))
			{
				if (mbinaryWriter.IsOpen())
				{
					mbinaryWriter.Write(this.mConnection.bytes);
					mbinaryWriter.Flush();
					mbinaryWriter.Close();
				}
				result = true;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message, LogType.Error);
		}
		return result;
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x000AAEB4 File Offset: 0x000A90B4
	public override bool IsDone()
	{
		return this.mConnection.isDone;
	}

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x06001A1C RID: 6684 RVA: 0x000AAEC4 File Offset: 0x000A90C4
	public override float Progress
	{
		get
		{
			return this.mConnection.progress;
		}
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06001A1D RID: 6685 RVA: 0x000AAED4 File Offset: 0x000A90D4
	public override string ContentAsText
	{
		get
		{
			return this.mConnection.text;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06001A1E RID: 6686 RVA: 0x000AAEE4 File Offset: 0x000A90E4
	public override byte[] Content
	{
		get
		{
			return this.mConnection.bytes;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06001A1F RID: 6687 RVA: 0x000AAEF4 File Offset: 0x000A90F4
	public override bool HasError
	{
		get
		{
			if (this.mConnection.error != null)
			{
				this.mError = this.mConnection.error;
				this.mErrorCode = -7;
			}
			return this.Error != null;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06001A20 RID: 6688 RVA: 0x000AAF38 File Offset: 0x000A9138
	public override bool IsValid
	{
		get
		{
			return this.mProperties != null && !string.IsNullOrEmpty(this.mProperties.URL);
		}
	}

	// Token: 0x040010E9 RID: 4329
	private WWW mConnection;
}
