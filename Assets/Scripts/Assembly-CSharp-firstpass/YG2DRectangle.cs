using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class YG2DRectangle : YG2DBody
{
	// Token: 0x06000953 RID: 2387 RVA: 0x00024334 File Offset: 0x00022534
	protected override Body GetBody(World world)
	{
		Vector2 vector = this.size * 0.01f;
		return BodyFactory.CreateRectangle(world, vector.x, vector.y, this.density);
	}

	// Token: 0x040005F1 RID: 1521
	public Vector2 size = new Vector2(64f, 64f);
}
