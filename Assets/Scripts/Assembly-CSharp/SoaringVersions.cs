using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MTools;
using UnityEngine;

// Token: 0x020003AC RID: 940
public class SoaringVersions : SoaringDelegate
{
	// Token: 0x06001B23 RID: 6947 RVA: 0x000B035C File Offset: 0x000AE55C
	public SoaringVersions(string serverAddress)
	{
		this.mFileDictionary = new SoaringDictionary();
		this.mServerURL = serverAddress;
		this.mServerRepoURL = serverAddress;
		this.mVersionFileName = "Soaring/SoaringVR.ver";
		this.mLocalFileRepo = ResourceUtils.GetFilePath(string.Empty, "Soaring/Content", false);
		SoaringInternal.instance.RegisterModule(new SoaringVersionSoaringModule());
		this.LoadVersionData();
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06001B25 RID: 6949 RVA: 0x000B03F0 File Offset: 0x000AE5F0
	public bool VersionsFileExists
	{
		get
		{
			return this.mVersionsFileExists;
		}
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x000B03F8 File Offset: 0x000AE5F8
	public float CurrentUpdateProgress()
	{
		return this.mCurrentProgress;
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06001B27 RID: 6951 RVA: 0x000B0400 File Offset: 0x000AE600
	public SoaringArray SubContentCategories
	{
		get
		{
			if (this.mSubContentCategories == null)
			{
				this.mSubContentCategories = new SoaringArray();
			}
			return this.mSubContentCategories;
		}
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x000B0420 File Offset: 0x000AE620
	public void SetVersionServer(string versioning, string webrepo, string filerepo)
	{
		this.SetVersionServer(versioning, webrepo, filerepo, null);
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x000B042C File Offset: 0x000AE62C
	public void SetVersionServer(string versioning, string webrepo, string filerepo, string versionFileName)
	{
		if (!string.IsNullOrEmpty(versioning))
		{
			this.mServerURL = versioning;
			this.mServerRandomAppend = true;
		}
		if (!string.IsNullOrEmpty(webrepo))
		{
			this.mServerRepoURL = webrepo;
		}
		if (!string.IsNullOrEmpty(filerepo))
		{
			this.mLocalFileRepo = filerepo;
			char c = this.mLocalFileRepo[this.mLocalFileRepo.Length - 1];
			if (c != '/' && c != '\\')
			{
				this.mLocalFileRepo += "/";
			}
		}
		if (!string.IsNullOrEmpty(versionFileName) && this.mVersionFileName != versionFileName)
		{
			this.mVersionFileName = versionFileName;
			this.LoadVersionData();
		}
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x000B04E4 File Offset: 0x000AE6E4
	public string GetFilePath(string fileID)
	{
		string result = null;
		if (string.IsNullOrEmpty(fileID))
		{
			return result;
		}
		SoaringVersions.SoaringFileVersion soaringFileVersion = (SoaringVersions.SoaringFileVersion)this.mFileDictionary.objectWithKey(fileID);
		if (soaringFileVersion != null)
		{
			result = soaringFileVersion.filePath;
		}
		return result;
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x000B0520 File Offset: 0x000AE720
	public string GetFileHash(string name)
	{
		if (string.IsNullOrEmpty(name) || this.mFileDictionary == null)
		{
			return null;
		}
		SoaringVersions.SoaringFileVersion soaringFileVersion = (SoaringVersions.SoaringFileVersion)this.mFileDictionary.objectWithKey(name);
		if (soaringFileVersion == null)
		{
			return null;
		}
		return soaringFileVersion.hash;
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x000B0568 File Offset: 0x000AE768
	public SoaringVersions.SoaringFileVersion GetVersionInfo(string name)
	{
		if (string.IsNullOrEmpty(name) || this.mFileDictionary == null)
		{
			return null;
		}
		return (SoaringVersions.SoaringFileVersion)this.mFileDictionary.objectWithKey(name);
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000B0594 File Offset: 0x000AE794
	private bool LoadVersionData()
	{
		bool result = false;
		MBinaryReader mbinaryReader = ResourceUtils.GetFileStream(this.mVersionFileName, null, null, 5);
		if (mbinaryReader == null)
		{
			this.mVersionsFileExists = false;
			return result;
		}
		if (!mbinaryReader.IsOpen())
		{
			this.mVersionsFileExists = false;
			return result;
		}
		try
		{
			if (this.mIsRawSave)
			{
				string @string = Encoding.UTF8.GetString(mbinaryReader.ReadAllBytes());
				SoaringDictionary soaringDictionary = new SoaringDictionary(@string);
				this.mVersioningVersion = soaringDictionary.soaringValue("version");
				this.mVersioningCommit = soaringDictionary.soaringValue("commit");
				this.mVersioningSource = soaringDictionary.soaringValue("source");
				SoaringArray soaringArray = (SoaringArray)soaringDictionary.objectWithKey("contents");
				int num = soaringArray.count();
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringArray.objectAtIndex(i);
					SoaringVersions.SoaringFileVersion soaringFileVersion = new SoaringVersions.SoaringFileVersion();
					soaringFileVersion.filePath = soaringDictionary2.soaringValue("n");
					soaringFileVersion.fileID = soaringFileVersion.filePath;
					soaringFileVersion.hash = soaringDictionary2.soaringValue("d");
					soaringFileVersion.localVersion = soaringDictionary2.soaringValue("l");
					this.mFileDictionary.setValue(soaringFileVersion, soaringFileVersion.fileID);
				}
			}
			else
			{
				short num2 = mbinaryReader.ReadShort();
				if (num2 != 2)
				{
					SoaringDebug.Log("SoaringVersions: Invalid Version ID: " + num2, LogType.Warning);
				}
				else
				{
					this.mVersioningSource = mbinaryReader.ReadCharArrayAsString();
					this.mVersioningCommit = mbinaryReader.ReadCharArrayAsString();
					this.mVersioningVersion = mbinaryReader.ReadLong();
					int num3 = mbinaryReader.ReadInt();
					for (int j = 0; j < num3; j++)
					{
						SoaringVersions.SoaringFileVersion soaringFileVersion2 = new SoaringVersions.SoaringFileVersion();
						soaringFileVersion2.filePath = mbinaryReader.ReadCharArrayAsString();
						soaringFileVersion2.fileID = mbinaryReader.ReadCharArrayAsString();
						soaringFileVersion2.hash = mbinaryReader.ReadCharArrayAsString();
						this.mFileDictionary.setValue(soaringFileVersion2, soaringFileVersion2.fileID);
					}
				}
			}
		}
		catch (Exception ex)
		{
			this.mVersionsFileExists = false;
			SoaringDebug.Log(ex.Message, LogType.Error);
			this.ResetVersionDownloads();
		}
		mbinaryReader.Close();
		mbinaryReader = null;
		return result;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000B07F0 File Offset: 0x000AE9F0
	private bool SaveSessionData()
	{
		bool result = false;
		string writePath = ResourceUtils.GetWritePath(this.mVersionFileName, string.Empty, 1);
		if (string.IsNullOrEmpty(writePath))
		{
			return result;
		}
		MBinaryWriter mbinaryWriter = new MBinaryWriter();
		if (!mbinaryWriter.Open(writePath, true, true))
		{
			return result;
		}
		if (!mbinaryWriter.IsOpen())
		{
			return result;
		}
		try
		{
			if (this.mIsRawSave)
			{
				SoaringDictionary soaringDictionary = new SoaringDictionary(4);
				soaringDictionary.addValue(this.mVersioningVersion, "version");
				soaringDictionary.addValue(this.mVersioningCommit, "commit");
				soaringDictionary.addValue(this.mVersioningSource, "source");
				SoaringObjectBase[] array = this.mFileDictionary.allValues();
				SoaringArray soaringArray = new SoaringArray(array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						soaringArray.addObject(array[i]);
					}
				}
				soaringDictionary.addValue(soaringArray, "contents");
				mbinaryWriter.WriteRawString(soaringDictionary.ToJsonString());
				mbinaryWriter.Flush();
				result = true;
			}
			else
			{
				mbinaryWriter.Write(2);
				mbinaryWriter.WriteCharArrayAsString(this.mVersioningSource);
				mbinaryWriter.WriteCharArrayAsString(this.mVersioningCommit);
				mbinaryWriter.Write(this.mVersioningVersion);
				SoaringObjectBase[] array2 = this.mFileDictionary.allValues();
				int num = this.mFileDictionary.count();
				mbinaryWriter.Write(num);
				for (int j = 0; j < num; j++)
				{
					SoaringVersions.SoaringFileVersion soaringFileVersion = (SoaringVersions.SoaringFileVersion)array2[j];
					mbinaryWriter.WriteCharArrayAsString(soaringFileVersion.filePath);
					mbinaryWriter.WriteCharArrayAsString(soaringFileVersion.fileID);
					mbinaryWriter.WriteCharArrayAsString(soaringFileVersion.hash);
				}
				mbinaryWriter.Flush();
				result = true;
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message, LogType.Error);
		}
		mbinaryWriter.Close();
		mbinaryWriter = null;
		return result;
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x000B09E4 File Offset: 0x000AEBE4
	private string PostAppendUrlString()
	{
		if (string.IsNullOrEmpty(this.platformInitial))
		{
			try
			{
				this.platformInitial = Application.platform.ToString();
				this.platformInitial = "?" + char.ToLower(this.platformInitial[0]).ToString();
			}
			catch (Exception ex)
			{
				SoaringDebug.Log(ex.Message, LogType.Error);
				this.platformInitial = "?";
			}
		}
		return this.platformInitial + UnityEngine.Random.Range(1000, int.MaxValue);
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x000B0A9C File Offset: 0x000AEC9C
	internal void CheckFilesForUpdates(bool updateFiles)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		this.mShouldUpdateFiles = updateFiles;
		string text = this.mServerURL;
		if (this.mServerRandomAppend)
		{
			text += this.PostAppendUrlString();
		}
		soaringDictionary.addValue(text, "turl");
		soaringDictionary.addValue(SoaringInternal.GameID, "gameId");
		if (!SoaringInternal.instance.CallModule("retrieveVersions", soaringDictionary, null))
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, "Invalid Call", null);
		}
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x000B0B28 File Offset: 0x000AED28
	public bool CheckValidFileData(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return false;
		}
		bool result = false;
		try
		{
			SoaringVersions.SoaringFileVersion soaringFileVersion = (SoaringVersions.SoaringFileVersion)this.mFileDictionary.objectWithKey(id);
			result = this.ValidateHash(soaringFileVersion.filePath, soaringFileVersion.hash);
		}
		catch
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x000B0B94 File Offset: 0x000AED94
	internal void AddFileVersions(SoaringArray versions, SoaringArray diffs, long newVersion, string source, string commit)
	{
		if (versions == null)
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, new SoaringError("No Version Data Found", -1), null);
			return;
		}
		if (newVersion == this.mVersioningVersion && this.mVersioningCommit == commit)
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, new SoaringError("Version Up To Date", 33), null);
			return;
		}
		if (!this.mShouldUpdateFiles)
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Update, null, null);
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Success, null, null);
			return;
		}
		if (source != this.mVersioningSource)
		{
			this.ClearAllContent();
		}
		this.mPendingUpdate = new SoaringVersions.SoaringPendingUpdates();
		this.mPendingUpdate.Commit = commit;
		this.mPendingUpdate.Version = newVersion;
		this.mPendingUpdate.Source = source;
		SoaringArray soaringArray = null;
		int num = versions.count();
		if (diffs != null)
		{
			soaringArray = new SoaringArray(num);
			for (int i = 0; i < num; i++)
			{
				SoaringVersions.SoaringFileVersion soaringFileVersion = new SoaringVersions.SoaringFileVersion();
				SoaringDictionary soaringDictionary = (SoaringDictionary)versions.objectAtIndex(i);
				soaringFileVersion.hash = soaringDictionary.soaringValue("d");
				soaringFileVersion.filePath = soaringDictionary.soaringValue("n");
				soaringFileVersion.fileID = soaringFileVersion.filePath;
				soaringArray.addObject(soaringFileVersion);
			}
			int num2 = diffs.count();
			for (int j = 0; j < num2; j++)
			{
				SoaringDictionary soaringDictionary2 = (SoaringDictionary)diffs.objectAtIndex(j);
				string text = soaringDictionary2.soaringValue("d");
				string text2 = soaringDictionary2.soaringValue("n");
				bool flag = false;
				for (int k = 0; k < num; k++)
				{
					SoaringVersions.SoaringFileVersion soaringFileVersion2 = (SoaringVersions.SoaringFileVersion)soaringArray.objectAtIndex(k);
					if (soaringFileVersion2.fileID == text2)
					{
						if (soaringFileVersion2.hash != text)
						{
							soaringFileVersion2.hash = text;
							SoaringDebug.Log("Diff File: " + soaringFileVersion2.fileID);
						}
						flag = true;
					}
				}
				if (!flag)
				{
					SoaringVersions.SoaringFileVersion soaringFileVersion3 = new SoaringVersions.SoaringFileVersion();
					soaringFileVersion3.hash = text;
					soaringFileVersion3.filePath = text2;
					soaringFileVersion3.fileID = soaringFileVersion3.filePath;
					soaringArray.addObject(soaringFileVersion3);
					SoaringDebug.Log("Diff File Added: " + soaringFileVersion3.fileID);
				}
			}
			num = soaringArray.count();
		}
		for (int l = 0; l < num; l++)
		{
			SoaringVersions.SoaringFileVersion soaringFileVersion4;
			if (soaringArray == null)
			{
				soaringFileVersion4 = new SoaringVersions.SoaringFileVersion();
				SoaringDictionary soaringDictionary3 = (SoaringDictionary)versions.objectAtIndex(l);
				soaringFileVersion4.hash = soaringDictionary3.soaringValue("d");
				soaringFileVersion4.filePath = soaringDictionary3.soaringValue("n");
				soaringFileVersion4.fileID = soaringFileVersion4.filePath;
			}
			else
			{
				soaringFileVersion4 = (SoaringVersions.SoaringFileVersion)soaringArray.objectAtIndex(l);
			}
			SoaringVersions.SoaringFileVersion soaringFileVersion5 = (SoaringVersions.SoaringFileVersion)this.mFileDictionary.objectWithKey(soaringFileVersion4.fileID);
			if (soaringFileVersion5 == null)
			{
				this.mPendingUpdate.PendingFiles.addValue(soaringFileVersion4, soaringFileVersion4.fileID);
			}
			else if (soaringFileVersion4.hash != soaringFileVersion5.hash)
			{
				this.mPendingUpdate.PendingFiles.addValue(soaringFileVersion4, soaringFileVersion4.fileID);
			}
		}
		this.mInitialFileCount = this.mPendingUpdate.PendingFiles.count();
		SoaringDebug.Log("Soaring Updating: " + this.mInitialFileCount + " files", LogType.Log);
		if (!this.NextDownload())
		{
			this.HandleSuccess();
		}
		else
		{
			SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Update, null, null);
		}
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x000B0F58 File Offset: 0x000AF158
	private bool NextDownload()
	{
		if (this.mPendingUpdate.PendingFiles.count() == 0 && this.mPendingUpdate.DownloadingFiles.count() == 0)
		{
			return false;
		}
		while (this.mPendingUpdate.PendingFiles.count() != 0)
		{
			if (this.mPendingUpdate.DownloadingFiles.count() >= this.MaxActiveConnections)
			{
				return true;
			}
			SoaringVersions.SoaringFileVersion soaringFileVersion = (SoaringVersions.SoaringFileVersion)this.mPendingUpdate.PendingFiles.objectAtIndex(0);
			this.mPendingUpdate.PendingFiles.removeObjectAtIndex(0);
			SoaringDebug.Log(this.mLocalFileRepo + soaringFileVersion.filePath, LogType.Log);
			string text = this.mServerRepoURL + soaringFileVersion.hash;
			if (this.mServerRandomAppend)
			{
				text += this.PostAppendUrlString();
			}
			this.mPendingUpdate.DownloadingFiles.addValue(soaringFileVersion, soaringFileVersion.fileID);
			SoaringInternal.instance.DownloadFileWithSoaring(soaringFileVersion.fileID, text, this.mLocalFileRepo + soaringFileVersion.filePath, new SCWebQueue.SCDownloadCallback(this.SCDownloadCallback), this);
		}
		return true;
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x000B1088 File Offset: 0x000AF288
	private void SCDownloadCallback(string id, bool success, string path)
	{
		SoaringVersions.SoaringFileVersion soaringFileVersion = null;
		if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(path))
		{
			success = false;
		}
		else
		{
			if (this.mPendingUpdate == null)
			{
				SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, new SoaringError("Failed to download file " + id, 34), null);
				return;
			}
			if (this.mPendingUpdate.PendingFiles == null || this.mPendingUpdate.DownloadingFiles == null)
			{
				SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, "Failed to download file " + id, null);
				return;
			}
			soaringFileVersion = (SoaringVersions.SoaringFileVersion)this.mPendingUpdate.DownloadingFiles.objectWithKey(id);
			this.mPendingUpdate.DownloadingFiles.removeObjectWithKey(id);
			soaringFileVersion.localVersion++;
			if (success && soaringFileVersion != null)
			{
				if (!string.IsNullOrEmpty(soaringFileVersion.hash))
				{
					success = this.ValidateHash(path, soaringFileVersion.hash);
				}
			}
			else
			{
				success = false;
			}
		}
		if (success)
		{
			SoaringObjectBase soaringObjectBase = this.mPendingUpdate.PendingFiles.objectWithKey(id);
			this.mPendingUpdate.PendingFiles.removeObjectWithKey(id);
			if (soaringObjectBase != null)
			{
				this.mFileDictionary.setValue(soaringObjectBase, id);
			}
		}
		else
		{
			if (this.mRetries >= 3)
			{
				this.mRetries = 0;
				this.mPendingUpdate.PendingFiles.clear();
				this.mPendingUpdate.DownloadingFiles.clear();
				SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Fail, "Failed to download file " + id, null);
				return;
			}
			this.mRetries++;
			if (soaringFileVersion != null)
			{
				this.mPendingUpdate.PendingFiles.addValue(soaringFileVersion, soaringFileVersion.fileID);
			}
		}
		if (this.mPendingUpdate.PendingFiles.count() == 0 && this.mPendingUpdate.DownloadingFiles.count() == 0)
		{
			this.HandleSuccess();
		}
		else
		{
			this.SaveSessionData();
			this.NextDownload();
		}
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x000B128C File Offset: 0x000AF48C
	public void HandleSuccess()
	{
		if (this.mPendingUpdate != null)
		{
			this.mPendingUpdate.PendingFiles.clear();
			this.mPendingUpdate.DownloadingFiles.clear();
			this.mVersioningVersion = this.mPendingUpdate.Version;
			this.mVersioningCommit = this.mPendingUpdate.Commit;
			this.mVersioningSource = this.mPendingUpdate.Source;
		}
		this.SaveSessionData();
		SoaringInternal.Delegate.OnFileVersionsUpdated(SoaringState.Success, null, null);
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x000B130C File Offset: 0x000AF50C
	public void ResetVersionDownloads()
	{
		if (this.mFileDictionary != null)
		{
			this.mFileDictionary.clear();
		}
		if (this.mPendingUpdate != null)
		{
			this.mPendingUpdate.PendingFiles.clear();
			this.mPendingUpdate.DownloadingFiles.clear();
		}
		this.mPendingUpdate = null;
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x000B1364 File Offset: 0x000AF564
	public void ClearAllContent()
	{
		try
		{
			this.ResetVersionDownloads();
			if (Directory.Exists(this.mLocalFileRepo))
			{
				Directory.Delete(this.mLocalFileRepo, true);
			}
			string filePath = ResourceUtils.GetFilePath(this.mVersionFileName, null, false);
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			else
			{
				SoaringDebug.Log("Invalid Manifiest: " + filePath, LogType.Error);
			}
			this.LoadVersionData();
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message, LogType.Error);
		}
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x000B1404 File Offset: 0x000AF604
	public void RemoveVersionFile(string fileID)
	{
		if (string.IsNullOrEmpty(fileID))
		{
			return;
		}
		SoaringVersions.SoaringFileVersion soaringFileVersion = (SoaringVersions.SoaringFileVersion)this.mFileDictionary.objectWithKey(fileID);
		if (soaringFileVersion == null)
		{
			string extension = Path.GetExtension(fileID);
			if (!string.IsNullOrEmpty(extension))
			{
				int num = this.mFileDictionary.count();
				for (int i = 0; i < num; i++)
				{
					soaringFileVersion = (SoaringVersions.SoaringFileVersion)this.mFileDictionary.objectAtIndex(i);
					if (soaringFileVersion.filePath.Contains(fileID))
					{
						this.mFileDictionary.removeObjectWithKey(this.mFileDictionary.allKeys()[i]);
						break;
					}
					soaringFileVersion = null;
				}
			}
			if (soaringFileVersion == null)
			{
				return;
			}
		}
		else
		{
			this.mFileDictionary.removeObjectWithKey(fileID);
		}
		this.SaveSessionData();
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x000B14C8 File Offset: 0x000AF6C8
	public override void OnFileDownloadUpdate(SoaringState state, SoaringError error, object data, SoaringContext context)
	{
		if (state != SoaringState.Update || error != null || this.mPendingUpdate == null)
		{
			return;
		}
		if (this.mPendingUpdate.PendingFiles == null)
		{
			return;
		}
		if (this.mInitialFileCount == 0)
		{
		}
		float num = (float)(this.mInitialFileCount - (this.mPendingUpdate.PendingFiles.count() + this.mPendingUpdate.DownloadingFiles.count())) / (float)this.mInitialFileCount;
		this.mCurrentProgress = num + (float)data / 1f / (float)this.mInitialFileCount;
		SoaringInternal.Delegate.OnFileVersionsUpdated(state, error, this.mCurrentProgress);
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x000B1580 File Offset: 0x000AF780
	public bool ValidateHash(string filePath, string hash)
	{
		bool result = false;
		if (!File.Exists(filePath))
		{
			return result;
		}
		try
		{
			MBinaryReader mbinaryReader = new MBinaryReader(filePath);
			if (mbinaryReader == null)
			{
				return result;
			}
			if (!mbinaryReader.IsOpen())
			{
				return result;
			}
			byte[] allBytes = mbinaryReader.ReadAllBytes();
			if (SoaringVersions.CheckAndCalculateMD5Hash(allBytes, hash))
			{
				result = true;
			}
			mbinaryReader.Close();
		}
		catch
		{
			SoaringDebug.Log("ValidateHash: Failed", LogType.Error);
		}
		return result;
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x000B1614 File Offset: 0x000AF814
	public static string CalculateMD5Hash(byte[] allBytes)
	{
		if (allBytes == null)
		{
			return null;
		}
		MD5 md = MD5.Create();
		byte[] array = md.ComputeHash(allBytes);
		StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
		foreach (int num in array)
		{
			if (SoaringVersions.IntToHexArr[num] == null)
			{
				string text = num.ToString("x2");
				SoaringVersions.IntToHexArr[num] = text;
			}
			stringBuilder.Append(SoaringVersions.IntToHexArr[num]);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x000B16A0 File Offset: 0x000AF8A0
	public static bool CheckAndCalculateMD5Hash(byte[] allBytes, string file_hash)
	{
		if (allBytes == null)
		{
			return false;
		}
		MD5 md = MD5.Create();
		byte[] array = md.ComputeHash(allBytes);
		int num = array.Length << 1;
		if (file_hash.Length != num)
		{
			SoaringDebug.Log(string.Concat(new object[]
			{
				"CheckAndCalculateMD5Hash: FAILED Size: ",
				file_hash.Length,
				" : ",
				num
			}), LogType.Error);
			return false;
		}
		int num2 = 0;
		foreach (int num3 in array)
		{
			if (SoaringVersions.IntToHexArr[num3] == null)
			{
				string text = num3.ToString("x2");
				SoaringVersions.IntToHexArr[num3] = text;
			}
			if (SoaringVersions.IntToHexArr[num3][0] != file_hash[num2] || SoaringVersions.IntToHexArr[num3][1] != file_hash[num2 + 1])
			{
				SoaringDebug.Log(string.Concat(new string[]
				{
					"CheckAndCalculateMD5Hash: FAILED Data: ",
					file_hash,
					" : ",
					file_hash[num2].ToString(),
					file_hash[num2 + 1].ToString(),
					" : ",
					SoaringVersions.IntToHexArr[num3].ToString()
				}), LogType.Error);
				return false;
			}
			num2 += 2;
		}
		return true;
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x000B1804 File Offset: 0x000AFA04
	private void CreateHexTable()
	{
		SoaringVersions.IntToHexArr = new string[256];
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x000B1818 File Offset: 0x000AFA18
	private void DestroyHexTable()
	{
		SoaringVersions.IntToHexArr = null;
	}

	// Token: 0x040011EF RID: 4591
	public const int Error_UpToData = 33;

	// Token: 0x040011F0 RID: 4592
	public const int Error_VersionReset = 34;

	// Token: 0x040011F1 RID: 4593
	private const int kVersion = 2;

	// Token: 0x040011F2 RID: 4594
	private SoaringDictionary mFileDictionary;

	// Token: 0x040011F3 RID: 4595
	private SoaringVersions.SoaringPendingUpdates mPendingUpdate;

	// Token: 0x040011F4 RID: 4596
	private bool mIsRawSave = true;

	// Token: 0x040011F5 RID: 4597
	private string mServerURL;

	// Token: 0x040011F6 RID: 4598
	private string mServerRepoURL;

	// Token: 0x040011F7 RID: 4599
	private string mLocalFileRepo;

	// Token: 0x040011F8 RID: 4600
	private int mRetries;

	// Token: 0x040011F9 RID: 4601
	private bool mServerRandomAppend;

	// Token: 0x040011FA RID: 4602
	public bool mVersionsFileExists = true;

	// Token: 0x040011FB RID: 4603
	private string mVersionFileName;

	// Token: 0x040011FC RID: 4604
	private string mVersioningSource;

	// Token: 0x040011FD RID: 4605
	private string mVersioningCommit;

	// Token: 0x040011FE RID: 4606
	private long mVersioningVersion;

	// Token: 0x040011FF RID: 4607
	private bool mShouldUpdateFiles = true;

	// Token: 0x04001200 RID: 4608
	private float mCurrentProgress;

	// Token: 0x04001201 RID: 4609
	private int mInitialFileCount;

	// Token: 0x04001202 RID: 4610
	public int MaxActiveConnections = 6;

	// Token: 0x04001203 RID: 4611
	private SoaringArray mSubContentCategories;

	// Token: 0x04001204 RID: 4612
	private string platformInitial;

	// Token: 0x04001205 RID: 4613
	private static string[] IntToHexArr = new string[256];

	// Token: 0x020003AD RID: 941
	public class SoaringFileVersion : SoaringObjectBase
	{
		// Token: 0x06001B3F RID: 6975 RVA: 0x000B1820 File Offset: 0x000AFA20
		public SoaringFileVersion() : base(SoaringObjectBase.IsType.Object)
		{
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x000B182C File Offset: 0x000AFA2C
		public override string ToJsonString()
		{
			return string.Concat(new object[]
			{
				"{\n\"n\":\"",
				this.fileID,
				"\",\n\"d\":\"",
				this.hash,
				"\",\n\"l\":",
				this.localVersion,
				"\n}"
			});
		}

		// Token: 0x04001206 RID: 4614
		public string fileID;

		// Token: 0x04001207 RID: 4615
		public string filePath;

		// Token: 0x04001208 RID: 4616
		public string hash;

		// Token: 0x04001209 RID: 4617
		public int localVersion;
	}

	// Token: 0x020003AE RID: 942
	private class SoaringPendingUpdates
	{
		// Token: 0x06001B41 RID: 6977 RVA: 0x000B1884 File Offset: 0x000AFA84
		public SoaringPendingUpdates()
		{
			this.PendingFiles = new SoaringDictionary();
			this.DownloadingFiles = new SoaringDictionary();
		}

		// Token: 0x0400120A RID: 4618
		public SoaringDictionary PendingFiles;

		// Token: 0x0400120B RID: 4619
		public SoaringDictionary DownloadingFiles;

		// Token: 0x0400120C RID: 4620
		public string Source;

		// Token: 0x0400120D RID: 4621
		public string Commit;

		// Token: 0x0400120E RID: 4622
		public long Version;
	}
}
