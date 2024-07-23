using System;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class Sinusoid : PeriodicPattern
{
	// Token: 0x06002150 RID: 8528 RVA: 0x000CE3B0 File Offset: 0x000CC5B0
	public Sinusoid(float minimum, float maximum, float period, float timeOffset)
	{
		timeOffset += period / 2f;
		base.Initialize(minimum, maximum, period, timeOffset);
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x000CE3D0 File Offset: 0x000CC5D0
	public override float ValueAtTime(float atTime)
	{
		float num = this.maximum - this.minimum;
		float num2 = num / 2f;
		float num3 = this.minimum + num2;
		float num4 = 6.2831855f / this.period;
		float num5 = Mathf.Cos((atTime + this.timeOffset) * num4);
		return num3 + num5 * num2;
	}
}
