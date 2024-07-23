using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class FingerGesturesInitializer : MonoBehaviour
{
	// Token: 0x06000580 RID: 1408 RVA: 0x00015374 File Offset: 0x00013574
	private void Awake()
	{
		if (!FingerGestures.Instance)
		{
			FingerGestures fingerGestures;
			if (Application.isEditor)
			{
				fingerGestures = this.editorGestures;
			}
			else
			{
				fingerGestures = this.androidGestures;
			}
			Debug.Log("Creating FingerGestures using " + fingerGestures.name);
			FingerGestures fingerGestures2 = UnityEngine.Object.Instantiate(fingerGestures) as FingerGestures;
			fingerGestures2.name = fingerGestures.name;
			if (this.makePersistent)
			{
				UnityEngine.Object.DontDestroyOnLoad(fingerGestures2.gameObject);
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040002FC RID: 764
	public FingerGestures editorGestures;

	// Token: 0x040002FD RID: 765
	public FingerGestures desktopGestures;

	// Token: 0x040002FE RID: 766
	public FingerGestures iosGestures;

	// Token: 0x040002FF RID: 767
	public FingerGestures androidGestures;

	// Token: 0x04000300 RID: 768
	public bool makePersistent = true;
}
