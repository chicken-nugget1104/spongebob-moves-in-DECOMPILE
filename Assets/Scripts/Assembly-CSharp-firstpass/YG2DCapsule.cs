using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

// Token: 0x020000F6 RID: 246
public class YG2DCapsule : YG2DBody
{
	// Token: 0x06000949 RID: 2377 RVA: 0x000241CC File Offset: 0x000223CC
	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateCapsule(world, this.height, this.radius, this.density);
	}

	// Token: 0x040005E6 RID: 1510
	public float height = 2f;

	// Token: 0x040005E7 RID: 1511
	public float radius = 0.5f;
}
