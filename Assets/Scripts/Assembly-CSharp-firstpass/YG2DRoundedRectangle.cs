using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class YG2DRoundedRectangle : YG2DBody
{
	// Token: 0x06000955 RID: 2389 RVA: 0x000243B4 File Offset: 0x000225B4
	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateRoundedRectangle(world, this.size.x, this.size.y, this.xRadius, this.yRadius, this.segments, this.density);
	}

	// Token: 0x040005F2 RID: 1522
	public Vector2 size = new Vector2(2f, 0.75f);

	// Token: 0x040005F3 RID: 1523
	public float xRadius = 0.2f;

	// Token: 0x040005F4 RID: 1524
	public float yRadius = 0.2f;

	// Token: 0x040005F5 RID: 1525
	public int segments = 2;
}
