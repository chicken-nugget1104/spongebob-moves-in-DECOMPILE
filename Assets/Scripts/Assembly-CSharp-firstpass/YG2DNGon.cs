using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

// Token: 0x020000FA RID: 250
public class YG2DNGon : YG2DBody
{
	// Token: 0x06000951 RID: 2385 RVA: 0x000242F4 File Offset: 0x000224F4
	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateEllipse(world, this.size, this.size, this.sides, this.density);
	}

	// Token: 0x040005EF RID: 1519
	public float size = 1f;

	// Token: 0x040005F0 RID: 1520
	public int sides = 5;
}
