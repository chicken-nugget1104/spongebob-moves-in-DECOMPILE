using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class DownloadObbExample : MonoBehaviour
{
	// Token: 0x060000C1 RID: 193 RVA: 0x00005890 File Offset: 0x00003A90
	private void OnGUI()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "Use GooglePlayDownloader only on Android device!");
			return;
		}
		string expansionFilePath = GooglePlayDownloader.GetExpansionFilePath();
		if (expansionFilePath == null)
		{
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "External storage is not available!");
		}
		else
		{
			string mainOBBPath = GooglePlayDownloader.GetMainOBBPath(expansionFilePath);
			string patchOBBPath = GooglePlayDownloader.GetPatchOBBPath(expansionFilePath);
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "Main = ..." + ((mainOBBPath != null) ? mainOBBPath.Substring(expansionFilePath.Length) : " NOT AVAILABLE"));
			GUI.Label(new Rect(10f, 25f, (float)(Screen.width - 10), 20f), "Patch = ..." + ((patchOBBPath != null) ? patchOBBPath.Substring(expansionFilePath.Length) : " NOT AVAILABLE"));
			if ((mainOBBPath == null || patchOBBPath == null) && GUI.Button(new Rect(10f, 100f, 100f, 100f), "Fetch OBBs"))
			{
				GooglePlayDownloader.FetchOBB();
			}
		}
	}
}
