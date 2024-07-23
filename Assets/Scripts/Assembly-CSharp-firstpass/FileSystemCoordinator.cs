using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020000DF RID: 223
public class FileSystemCoordinator
{
	// Token: 0x0600087D RID: 2173 RVA: 0x0001FB10 File Offset: 0x0001DD10
	public static void Clear()
	{
		FileSystemCoordinator.sFilePaths = new Dictionary<string, UnityEngine.Object>();
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0001FB1C File Offset: 0x0001DD1C
	public static UnityEngine.Object LoadAsset(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
		{
			return null;
		}
		UnityEngine.Object result = null;
		try
		{
			if (!FileSystemCoordinator.sFilePaths.TryGetValue(fileName, out result))
			{
				string path = Application.persistentDataPath + "/Contents/Bundles/" + fileName + "_Android.bundle";
				if (File.Exists(path))
				{
					BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
					AssetBundle assetBundle = AssetBundle.CreateFromMemoryImmediate(binaryReader.ReadBytes((int)binaryReader.BaseStream.Length));
					binaryReader.Close();
					if (assetBundle == null)
					{
						Debug.Log("FileSystemCoordinator: No Bundle Loaded: " + fileName);
					}
					FileSystemBundle component = ((GameObject)assetBundle.mainAsset).GetComponent<FileSystemBundle>();
					if (component == null)
					{
						Debug.Log("FileSystemCoordinator: Invalid Main Game Object: " + fileName);
					}
					int num = component.fileObjects.Length;
					for (int i = 0; i < num; i++)
					{
						FileSystemCoordinator.sFilePaths.Add(component.filePaths[i], component.fileObjects[i]);
						if (component.filePaths[i] == fileName)
						{
							result = component.fileObjects[i];
						}
					}
					assetBundle.Unload(false);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);
		}
		return result;
	}

	// Token: 0x04000535 RID: 1333
	private const string BundlePaths = "/Contents/Bundles/";

	// Token: 0x04000536 RID: 1334
	private static Dictionary<string, UnityEngine.Object> sFilePaths = new Dictionary<string, UnityEngine.Object>();
}
