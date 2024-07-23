using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class GooglePlayDownload : MonoBehaviour
{
	// Token: 0x0600026C RID: 620 RVA: 0x0000C1E4 File Offset: 0x0000A3E4
	private void Start()
	{
		this.expPath = GooglePlayDownloader.GetExpansionFilePath();
		if (this.expPath == null)
		{
			Debug.Log("External storage is not available!");
		}
		else
		{
			this.mainPath = GooglePlayDownloader.GetMainOBBPath(this.expPath);
			if (this.mainPath == null)
			{
				GooglePlayDownloader.FetchOBB();
			}
			base.StartCoroutine(this.CoroutineLoadLevel());
		}
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000C244 File Offset: 0x0000A444
	protected IEnumerator CoroutineLoadLevel()
	{
		bool testResourceLoaded = false;
		while (!testResourceLoaded)
		{
			yield return new WaitForSeconds(0.5f);
			TFUtils.DebugLog("1");
			this.mainPath = GooglePlayDownloader.GetMainOBBPath(this.expPath);
			if (this.mainPath != null)
			{
				TFUtils.DebugLog("2");
				testResourceLoaded = true;
			}
		}
		Application.LoadLevel(1);
		yield break;
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000C260 File Offset: 0x0000A460
	private void Update()
	{
	}

	// Token: 0x0400013D RID: 317
	private string mainPath;

	// Token: 0x0400013E RID: 318
	private string expPath;
}
