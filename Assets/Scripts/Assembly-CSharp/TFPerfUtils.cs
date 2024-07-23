using System;

// Token: 0x0200045B RID: 1115
public class TFPerfUtils
{
	// Token: 0x06002274 RID: 8820 RVA: 0x000D3A6C File Offset: 0x000D1C6C
	static TFPerfUtils()
	{
		TFPerfUtils.SetNonParticleDevice();
		TFPerfUtils.SetNonScalingDevice();
	}

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x06002275 RID: 8821 RVA: 0x000D3A78 File Offset: 0x000D1C78
	public static CommonUtils.LevelOfDetail MemoryLod
	{
		get
		{
			return CommonUtils.TextureLod();
		}
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x000D3A80 File Offset: 0x000D1C80
	public static bool IsNonParticleDevice()
	{
		return TFPerfUtils.isNonParticleDevice;
	}

	// Token: 0x06002277 RID: 8823 RVA: 0x000D3A88 File Offset: 0x000D1C88
	public static bool IsNonScalingDevice()
	{
		return TFPerfUtils.isNonScalingDevice;
	}

	// Token: 0x06002278 RID: 8824 RVA: 0x000D3A90 File Offset: 0x000D1C90
	private static void SetNonParticleDevice()
	{
		TFPerfUtils.isNonParticleDevice = false;
	}

	// Token: 0x06002279 RID: 8825 RVA: 0x000D3A98 File Offset: 0x000D1C98
	private static void SetNonScalingDevice()
	{
		TFPerfUtils.isNonScalingDevice = false;
	}

	// Token: 0x04001545 RID: 5445
	public const string LOW_RESOURCE_ID = "_lr";

	// Token: 0x04001546 RID: 5446
	private static bool isNonParticleDevice;

	// Token: 0x04001547 RID: 5447
	private static bool isNonScalingDevice;
}
