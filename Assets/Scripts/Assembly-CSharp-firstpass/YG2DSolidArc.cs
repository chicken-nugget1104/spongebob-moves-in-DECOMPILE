using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class YG2DSolidArc : YG2DBody
{
	// Token: 0x06000957 RID: 2391 RVA: 0x00024434 File Offset: 0x00022634
	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateSolidArc(world, this.density, this.degrees * 0.017453292f, this.sides, this.radius, Vector2.zero, this.angle * 0.017453292f);
	}

	// Token: 0x040005F6 RID: 1526
	public float degrees = 15f;

	// Token: 0x040005F7 RID: 1527
	public float angle = 45f;

	// Token: 0x040005F8 RID: 1528
	public float radius = 0.5f;

	// Token: 0x040005F9 RID: 1529
	public int sides = 8;
}
