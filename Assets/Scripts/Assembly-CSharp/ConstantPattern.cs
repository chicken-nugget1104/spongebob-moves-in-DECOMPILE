using System;

// Token: 0x02000435 RID: 1077
public class ConstantPattern : PeriodicPattern
{
	// Token: 0x06002147 RID: 8519 RVA: 0x000CE1BC File Offset: 0x000CC3BC
	public ConstantPattern(float constant)
	{
		this.constant = constant;
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x000CE1CC File Offset: 0x000CC3CC
	public override float ValueAtTime(float atTime)
	{
		return this.constant;
	}

	// Token: 0x0400149C RID: 5276
	protected float constant;
}
