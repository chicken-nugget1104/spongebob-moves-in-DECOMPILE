using System;

// Token: 0x02000437 RID: 1079
public abstract class PeriodicPattern
{
	// Token: 0x0600214D RID: 8525 RVA: 0x000CE388 File Offset: 0x000CC588
	public PeriodicPattern()
	{
	}

	// Token: 0x0600214E RID: 8526 RVA: 0x000CE390 File Offset: 0x000CC590
	protected void Initialize(float minimum, float maximum, float period, float timeOffset)
	{
		this.minimum = minimum;
		this.maximum = maximum;
		this.period = period;
		this.timeOffset = timeOffset;
	}

	// Token: 0x0600214F RID: 8527
	public abstract float ValueAtTime(float atTime);

	// Token: 0x040014A3 RID: 5283
	protected float period;

	// Token: 0x040014A4 RID: 5284
	protected float minimum;

	// Token: 0x040014A5 RID: 5285
	protected float maximum;

	// Token: 0x040014A6 RID: 5286
	protected float timeOffset;
}
