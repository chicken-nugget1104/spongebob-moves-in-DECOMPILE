using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public class Waypoint
{
	// Token: 0x060023D1 RID: 9169 RVA: 0x000DD090 File Offset: 0x000DB290
	public Waypoint(Simulated sim)
	{
		this.sim = sim;
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x060023D2 RID: 9170 RVA: 0x000DD0A0 File Offset: 0x000DB2A0
	public Vector2 Position
	{
		get
		{
			return this.sim.PointOfInterest;
		}
	}

	// Token: 0x04001621 RID: 5665
	private Simulated sim;
}
