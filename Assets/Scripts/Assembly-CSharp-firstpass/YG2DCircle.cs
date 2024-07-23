using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

// Token: 0x020000F7 RID: 247
public class YG2DCircle : YG2DBody
{
	// Token: 0x0600094B RID: 2379 RVA: 0x000241FC File Offset: 0x000223FC
	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateCircle(world, this.radius, this.density);
	}

	// Token: 0x040005E8 RID: 1512
	public float radius = 0.5f;
}
