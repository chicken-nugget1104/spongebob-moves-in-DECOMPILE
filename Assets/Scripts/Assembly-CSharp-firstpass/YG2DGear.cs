using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

// Token: 0x020000F9 RID: 249
public class YG2DGear : YG2DBody
{
	// Token: 0x0600094F RID: 2383 RVA: 0x000242A4 File Offset: 0x000224A4
	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateGear(world, this.radius, this.teeth, this.tipPercent, this.toothHeight, this.density);
	}

	// Token: 0x040005EB RID: 1515
	public float radius = 0.5f;

	// Token: 0x040005EC RID: 1516
	public int teeth = 9;

	// Token: 0x040005ED RID: 1517
	public float tipPercent = 0.1f;

	// Token: 0x040005EE RID: 1518
	public float toothHeight = 0.25f;
}
