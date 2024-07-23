using System;
using System.IO;
using MTools;
using UnityEngine;

// Token: 0x020003C0 RID: 960
public class ResourceUtils
{
	// Token: 0x06001C72 RID: 7282 RVA: 0x000B4F68 File Offset: 0x000B3168
	public static void CopyMeta(string filePathNew, string filePathOld, bool deleteOldMeta)
	{
		string text = filePathNew + ".meta";
		string text2 = filePathOld + ".meta";
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		if (File.Exists(text2))
		{
			File.Copy(text2, text);
			if (File.Exists(text2) && deleteOldMeta)
			{
				File.Delete(text2);
			}
		}
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x000B4FC8 File Offset: 0x000B31C8
	public static string CropOffsetPath(string offset)
	{
		if (string.IsNullOrEmpty(offset))
		{
			return offset;
		}
		int length = offset.Length;
		int num = 0;
		int num2 = length;
		if (offset[0] == '/')
		{
			num = 1;
		}
		if (offset[length - 1] == '/')
		{
			num2 = length - 1;
		}
		if (num >= num2)
		{
			return string.Empty;
		}
		if (num == 0 && num2 == length - 1)
		{
			return offset;
		}
		return offset.Substring(num, num2 - num);
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x000B503C File Offset: 0x000B323C
	private static bool _CheckKey(byte key, byte check_option)
	{
		return (key & check_option) == check_option;
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x000B5044 File Offset: 0x000B3244
	private static string _PersistantPath()
	{
		return Application.persistentDataPath + "/";
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x000B5058 File Offset: 0x000B3258
	private static string _PersistantPathEditor()
	{
		return Application.dataPath + "/";
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x000B506C File Offset: 0x000B326C
	public static string GetFilePath(string fileName, string offsetPath = null, bool checkValidPath = false)
	{
		byte b = 128;
		return ResourceUtils.GetFilePath(fileName, offsetPath, ResourceUtils.DefaultReadFileOptions, checkValidPath, ref b);
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x000B5090 File Offset: 0x000B3290
	public static string GetFilePath(string fileName, string offsetPath, byte fileOptions, bool checkValidPath)
	{
		byte b = 128;
		return ResourceUtils.GetFilePath(fileName, offsetPath, fileOptions, checkValidPath, ref b);
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x000B50B0 File Offset: 0x000B32B0
	public static string GetFilePath(string fileName, string offsetPath, byte fileOptions, bool checkValidPath, ref byte return_file_type)
	{
		string text = fileName;
		if (!string.IsNullOrEmpty(offsetPath))
		{
			offsetPath = ResourceUtils.CropOffsetPath(offsetPath);
			if (!string.IsNullOrEmpty(offsetPath))
			{
				text = offsetPath + "/" + fileName;
			}
		}
		if (ResourceUtils._CheckKey(fileOptions, 1))
		{
			string text2 = ResourceUtils._PersistantPath() + text;
			if (!checkValidPath)
			{
				TFUtils.DebugLog("ResourceUtils.Persistant: " + text2 + " Name: " + fileName);
				return_file_type = 1;
				return text2;
			}
			if (ResourceUtils.FileExists(text2))
			{
				TFUtils.DebugLog("ResourceUtils.Persistant: " + text2 + " Name: " + fileName);
				return_file_type = 1;
				return text2;
			}
		}
		if (ResourceUtils._CheckKey(fileOptions, 4))
		{
			string str = string.Empty;
			string text2;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				text2 = Application.dataPath + "/Raw/" + text;
			}
			else if (Application.platform == RuntimePlatform.Android)
			{
				str = "jar:file://";
				checkValidPath = false;
				text2 = Application.dataPath + "!/assets/" + text;
			}
			else
			{
				text2 = Application.dataPath + "/StreamingAssets/" + text;
			}
			if (!checkValidPath)
			{
				TFUtils.DebugLog("ResourceUtils.Streamed: " + text2 + " Name: " + fileName);
				return_file_type = 4;
				return str + text2;
			}
			if (ResourceUtils.FileExists(text2))
			{
				TFUtils.DebugLog("ResourceUtils.Streamed: " + text2 + " Name: " + fileName);
				return_file_type = 4;
				return str + text2;
			}
		}
		if (ResourceUtils._CheckKey(fileOptions, 2))
		{
			string text2 = ResourceUtils.FileNameWithoutExtension(text);
			TFUtils.DebugLog("ResourceUtils.Resources: " + text2 + " Name: " + fileName);
			return_file_type = 2;
			return text2;
		}
		return null;
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x000B524C File Offset: 0x000B344C
	public static string GetWritePath(string fileName, string offsetPath, byte option = 1)
	{
		offsetPath = ResourceUtils.CropOffsetPath(offsetPath);
		if (offsetPath != null)
		{
			fileName = offsetPath + "/" + fileName;
		}
		return ResourceUtils._PersistantPath() + fileName;
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x000B5278 File Offset: 0x000B3478
	public static MBinaryReader GetFileStream(string filename)
	{
		return ResourceUtils.GetFileStream(filename, null, null, ResourceUtils.DefaultReadFileOptions);
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x000B5288 File Offset: 0x000B3488
	public static MBinaryReader GetFileStream(string filename, string directory, string ext)
	{
		return ResourceUtils.GetFileStream(filename, directory, ext, ResourceUtils.DefaultReadFileOptions);
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x000B5298 File Offset: 0x000B3498
	public static MBinaryReader GetFileStream(string filename, string directory, string ext, byte options)
	{
		MBinaryReader mbinaryReader = null;
		if (filename == null)
		{
			return mbinaryReader;
		}
		string text = filename;
		if (ext != null)
		{
			text = filename + "." + ext;
		}
		byte key = 128;
		string filePath = ResourceUtils.GetFilePath(text, directory, options, true, ref key);
		TFUtils.DebugLog("ResourceUtils.Path: " + filePath + " Name: " + text);
		if (filePath != null)
		{
			if (ResourceUtils._CheckKey(key, 4))
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					try
					{
						WWW www = new WWW(filePath);
						while (!www.isDone)
						{
							if (www.error != null)
							{
								TFUtils.DebugLog("ResourceUtils.Error: " + www.error);
								return null;
							}
						}
						mbinaryReader = new MBinaryReader(www.bytes);
					}
					catch (Exception ex)
					{
						TFUtils.DebugLog("ResourceUtils.Exception: " + ex.Message);
						return null;
					}
				}
				else
				{
					mbinaryReader = new MBinaryReader(filePath);
				}
			}
			else if (ResourceUtils._CheckKey(key, 2))
			{
				TextAsset textAsset = (TextAsset)Resources.Load(filePath);
				TFUtils.DebugLog("ResourceUtils.Loading");
				if (textAsset != null)
				{
					mbinaryReader = new MBinaryReader(textAsset.bytes);
					Resources.UnloadAsset(textAsset);
				}
				else
				{
					TFUtils.DebugLog("ResourceUtils.Failed");
				}
			}
			else
			{
				mbinaryReader = new MBinaryReader(filePath);
			}
		}
		if (mbinaryReader == null || !mbinaryReader.IsOpen())
		{
			return null;
		}
		return mbinaryReader;
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x000B5430 File Offset: 0x000B3630
	public static Stream GetRawFileStream(string filename)
	{
		return ResourceUtils.GetRawFileStream(filename, null, null, ResourceUtils.DefaultReadFileOptions);
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x000B5440 File Offset: 0x000B3640
	public static Stream GetRawFileStream(string filename, string directory, string ext)
	{
		return ResourceUtils.GetRawFileStream(filename, directory, ext, ResourceUtils.DefaultReadFileOptions);
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x000B5450 File Offset: 0x000B3650
	public static Stream GetRawFileStream(string filename, string directory, string ext, byte options)
	{
		Stream stream = null;
		if (filename == null)
		{
			return stream;
		}
		string text = filename;
		if (ext != null)
		{
			text = filename + "." + ext;
		}
		byte key = 128;
		string filePath = ResourceUtils.GetFilePath(text, directory, options, true, ref key);
		TFUtils.DebugLog("ResourceUtils.Path: " + filePath + " Name: " + text);
		if (filePath != null)
		{
			if (ResourceUtils._CheckKey(key, 4))
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					try
					{
						WWW www = new WWW(filePath);
						while (!www.isDone)
						{
							if (www.error != null)
							{
								TFUtils.DebugLog("ResourceUtils.Error: " + www.error);
								return null;
							}
						}
						stream = new MemoryStream(www.bytes);
					}
					catch (Exception ex)
					{
						TFUtils.DebugLog("ResourceUtils.Exception: " + ex.Message);
						return null;
					}
				}
				else if (File.Exists(filePath))
				{
					stream = File.OpenRead(filePath);
				}
			}
			else if (ResourceUtils._CheckKey(key, 2))
			{
				TextAsset textAsset = (TextAsset)Resources.Load(filePath);
				TFUtils.DebugLog("ResourceUtils.Loading");
				if (textAsset != null)
				{
					stream = new MemoryStream(textAsset.bytes);
					Resources.UnloadAsset(textAsset);
				}
				else
				{
					TFUtils.DebugLog("ResourceUtils.Failed");
				}
			}
			else if (File.Exists(filePath))
			{
				stream = File.OpenRead(filePath);
			}
		}
		if (stream == null || stream.Length == 0L)
		{
			return null;
		}
		return stream;
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x000B55FC File Offset: 0x000B37FC
	public static byte[] GetVersionedFileBytes(string filename)
	{
		return ResourceUtils.GetFileBytes(filename, null, null, ResourceUtils.DefaultReadFileOptions);
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x000B560C File Offset: 0x000B380C
	public static byte[] GetVersionedFileBytes(string filename, string ext)
	{
		return ResourceUtils.GetFileBytes(filename, null, ext, ResourceUtils.DefaultReadFileOptions);
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x000B561C File Offset: 0x000B381C
	public static byte[] GetVersionedFileBytes(string filename, string directory, string ext)
	{
		return ResourceUtils.GetFileBytes(filename, directory, ext, ResourceUtils.DefaultReadFileOptions);
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x000B562C File Offset: 0x000B382C
	public static byte[] GetFileBytes(string filename, string directory, string ext, byte file_type)
	{
		byte[] result = null;
		if (filename == null)
		{
			return result;
		}
		string text = filename;
		if (ext != null)
		{
			text = filename + "." + ext;
		}
		byte key = 128;
		string filePath = ResourceUtils.GetFilePath(text, directory, file_type, true, ref key);
		Debug.Log("ResourceUtils.Path: " + filePath + " Name: " + text);
		if (filePath != null)
		{
			if (ResourceUtils._CheckKey(key, 4))
			{
				if (Application.platform == RuntimePlatform.Android)
				{
					try
					{
						WWW www = new WWW(filePath);
						while (!www.isDone)
						{
							if (www.error != null)
							{
								Debug.Log("ResourceUtils.Error: " + www.error);
								return null;
							}
						}
						Debug.Log(www.bytes.Length);
						MBinaryReader mbinaryReader = new MBinaryReader(www.bytes);
						result = mbinaryReader.ReadAllBytes();
						mbinaryReader.Close();
					}
					catch (Exception ex)
					{
						Debug.Log("ResourceUtils.Exception: " + ex.Message);
						return null;
					}
				}
				else
				{
					MBinaryReader mbinaryReader2 = new MBinaryReader(filePath);
					result = mbinaryReader2.ReadAllBytes();
					mbinaryReader2.Close();
				}
			}
			else if (ResourceUtils._CheckKey(key, 2))
			{
				TextAsset textAsset = (TextAsset)Resources.Load(filePath);
				if (textAsset != null)
				{
					result = textAsset.bytes;
					Resources.UnloadAsset(textAsset);
				}
			}
			else
			{
				MBinaryReader mbinaryReader3 = new MBinaryReader(filePath);
				result = mbinaryReader3.ReadAllBytes();
				mbinaryReader3.Close();
			}
		}
		return result;
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x000B57E0 File Offset: 0x000B39E0
	public static string FileNameWithoutExtension(string fileExt)
	{
		int num = fileExt.LastIndexOf('.');
		if (num == -1)
		{
			return fileExt;
		}
		return fileExt.Substring(0, num);
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x000B5808 File Offset: 0x000B3A08
	public static string FileNameWithoutPath(string fileExt)
	{
		int num = fileExt.LastIndexOf('/');
		if (num == -1)
		{
			return fileExt;
		}
		return fileExt.Substring(num + 1, fileExt.Length - (num + 1));
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x000B583C File Offset: 0x000B3A3C
	public static bool FileExists(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return false;
		}
		FileInfo fileInfo = new FileInfo(path);
		return fileInfo != null && fileInfo.Exists;
	}

	// Token: 0x04001244 RID: 4676
	public const byte FileSys_Persistant = 1;

	// Token: 0x04001245 RID: 4677
	public const byte FileSys_Resources = 2;

	// Token: 0x04001246 RID: 4678
	public const byte FileSys_Streamed = 4;

	// Token: 0x04001247 RID: 4679
	public const byte FileSys_Editor = 8;

	// Token: 0x04001248 RID: 4680
	public const byte FileSys_Invalid = 128;

	// Token: 0x04001249 RID: 4681
	public const byte FileSys_All = 255;

	// Token: 0x0400124A RID: 4682
	public static byte DefaultReadFileOptions = 7;

	// Token: 0x0400124B RID: 4683
	public static byte DefaultWriteFileOptions = 1;
}
