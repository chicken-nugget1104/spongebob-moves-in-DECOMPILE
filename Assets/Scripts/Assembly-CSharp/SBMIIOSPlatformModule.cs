using System;
using UnityEngine;

// Token: 0x02000344 RID: 836
public class SBMIIOSPlatformModule : SoaringPlatformIOS
{
	// Token: 0x060017F4 RID: 6132 RVA: 0x0009EAE0 File Offset: 0x0009CCE0
	public override void Init()
	{
		base.Init();
		this.UseResetDeviceID = (PlayerPrefs.GetInt("UseResetDeviceID", 0) != 0);
		this.sResetDeviceID = PlayerPrefs.GetString("ResetDeviceID", string.Empty);
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x0009EB20 File Offset: 0x0009CD20
	public void ResetDeviceID()
	{
		this.UseResetDeviceID = true;
		PlayerPrefs.SetInt("UseResetDeviceID", 1);
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x0009EB34 File Offset: 0x0009CD34
	public override string DeviceID()
	{
		if (!this.UseResetDeviceID)
		{
			return base.DeviceID();
		}
		if (string.IsNullOrEmpty(this.sResetDeviceID))
		{
			this.sResetDeviceID = "SBMIRGK_" + Environment.MachineName + "_" + UnityEngine.Random.Range(10000, 999999).ToString();
			PlayerPrefs.SetString("ResetDeviceID", this.sResetDeviceID);
		}
		return this.sResetDeviceID;
	}

	// Token: 0x04000FFB RID: 4091
	private string sResetDeviceID = string.Empty;

	// Token: 0x04000FFC RID: 4092
	private bool UseResetDeviceID;
}
