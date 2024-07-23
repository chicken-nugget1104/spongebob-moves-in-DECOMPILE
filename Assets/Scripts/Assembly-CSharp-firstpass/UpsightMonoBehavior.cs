using System;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class UpsightMonoBehavior : MonoBehaviour
{
	// Token: 0x0600083A RID: 2106 RVA: 0x0001EFA0 File Offset: 0x0001D1A0
	private void Start()
	{
		Upsight.init(this.androidAppToken, this.androidAppSecret, this.gcmProjectNumber);
		Upsight.requestAppOpen();
		if (this.registerForPushNotifications)
		{
			Upsight.registerForPushNotifications();
		}
	}

	// Token: 0x04000504 RID: 1284
	public string androidAppToken;

	// Token: 0x04000505 RID: 1285
	public string androidAppSecret;

	// Token: 0x04000506 RID: 1286
	public string gcmProjectNumber;

	// Token: 0x04000507 RID: 1287
	public string iosAppToken;

	// Token: 0x04000508 RID: 1288
	public string iosAppSecret;

	// Token: 0x04000509 RID: 1289
	public bool registerForPushNotifications;
}
