using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class YG2DEllipse : YG2DBody
{
	// Token: 0x0600094D RID: 2381 RVA: 0x00024238 File Offset: 0x00022438
	protected override Body GetBody(World world)
	{
		return BodyFactory.CreateEllipse(world, this.size.x, this.size.y, this.edges, this.density);
	}

	// Token: 0x040005E9 RID: 1513
	public Vector2 size = new Vector2(2f, 1f);

	// Token: 0x040005EA RID: 1514
	public int edges = 16;
}
