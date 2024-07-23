using System;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class SBScaleForLanguage : MonoBehaviour
{
	// Token: 0x060006F7 RID: 1783 RVA: 0x0002C7EC File Offset: 0x0002A9EC
	public void Scale()
	{
		this.Scale(this.scaledObject);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0002C7FC File Offset: 0x0002A9FC
	public void Scale(GameObject ob)
	{
		if (ob == null || this.scales == null)
		{
			return;
		}
		LanguageCode languageCode = Language.CurrentLanguage();
		if (languageCode == LanguageCode.N)
		{
			return;
		}
		SBScaleForLanguage.DeviceType deviceType = this.FindDeviceType();
		SBScaleForLanguage.SBScaler sbscaler = null;
		int num = this.scales.Length;
		for (int i = 0; i < num; i++)
		{
			if (this.scales[i].language == languageCode && this.scales[i].type == deviceType)
			{
				sbscaler = this.scales[i];
				break;
			}
		}
		if (sbscaler == null)
		{
			return;
		}
		ob.transform.localPosition = sbscaler.position;
		ob.transform.localScale = sbscaler.scale;
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0002C8B8 File Offset: 0x0002AAB8
	private SBScaleForLanguage.DeviceType FindDeviceType()
	{
		return SBScaleForLanguage.DeviceType.Free;
	}

	// Token: 0x0400055A RID: 1370
	public SBScaleForLanguage.SBScaler[] scales = new SBScaleForLanguage.SBScaler[0];

	// Token: 0x0400055B RID: 1371
	public GameObject scaledObject;

	// Token: 0x020000B9 RID: 185
	public enum DeviceType
	{
		// Token: 0x0400055D RID: 1373
		Free,
		// Token: 0x0400055E RID: 1374
		iPhone,
		// Token: 0x0400055F RID: 1375
		iPhoneWide,
		// Token: 0x04000560 RID: 1376
		iPad
	}

	// Token: 0x020000BA RID: 186
	[Serializable]
	public class SBScaler
	{
		// Token: 0x04000561 RID: 1377
		public LanguageCode language;

		// Token: 0x04000562 RID: 1378
		public Vector3 position = new Vector3(0f, 0f, 0f);

		// Token: 0x04000563 RID: 1379
		public Vector3 scale = new Vector3(1f, 1f, 1f);

		// Token: 0x04000564 RID: 1380
		public SBScaleForLanguage.DeviceType type;
	}
}
