using System;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class DeviceSpec
{
	// Token: 0x0600035F RID: 863 RVA: 0x000110F8 File Offset: 0x0000F2F8
	public DeviceSpec()
	{
		this.width = (float)Screen.width;
		this.height = (float)Screen.height;
		if (Screen.dpi >= 250f)
		{
			this.density = DeviceSpec.ResolutionDensity.Dense;
		}
		else
		{
			this.density = DeviceSpec.ResolutionDensity.Standard;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000360 RID: 864 RVA: 0x00011148 File Offset: 0x0000F348
	public float Width
	{
		get
		{
			return this.width;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000361 RID: 865 RVA: 0x00011150 File Offset: 0x0000F350
	public float Height
	{
		get
		{
			return this.height;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000362 RID: 866 RVA: 0x00011158 File Offset: 0x0000F358
	public Vector3 Center3
	{
		get
		{
			return new Vector3(this.width / 2f, this.height / 2f, 0f);
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000363 RID: 867 RVA: 0x00011188 File Offset: 0x0000F388
	public DeviceSpec.ResolutionDensity Density
	{
		get
		{
			return this.density;
		}
	}

	// Token: 0x0400023D RID: 573
	private const float DPI_DENSE = 250f;

	// Token: 0x0400023E RID: 574
	private DeviceSpec.ResolutionDensity density;

	// Token: 0x0400023F RID: 575
	private float width;

	// Token: 0x04000240 RID: 576
	private float height;

	// Token: 0x02000056 RID: 86
	public enum ResolutionDensity
	{
		// Token: 0x04000242 RID: 578
		Standard,
		// Token: 0x04000243 RID: 579
		Dense
	}
}
