using System;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class TerrainPathing
{
	// Token: 0x06002372 RID: 9074 RVA: 0x000DA4FC File Offset: 0x000D86FC
	public TerrainPathing(Terrain terrain, Vector2 start, Vector2 goal)
	{
		this.terrain = terrain;
		this.goal = this.terrain.ComputeGridPosition(goal);
		this.pathFinder2 = new PathFinder2(this.terrain);
		this.pathFinder2.Start(this.terrain.ComputeGridPosition(start), this.goal);
	}

	// Token: 0x06002373 RID: 9075 RVA: 0x000DA558 File Offset: 0x000D8758
	public PathFinder2.PROGRESS Seek(int budget)
	{
		return this.pathFinder2.Seek(budget);
	}

	// Token: 0x06002374 RID: 9076 RVA: 0x000DA568 File Offset: 0x000D8768
	public void BuildPath(out Path<GridPosition> path)
	{
		this.pathFinder2.BuildPath(out path);
	}

	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x06002375 RID: 9077 RVA: 0x000DA578 File Offset: 0x000D8778
	public GridPosition Goal
	{
		get
		{
			return this.goal;
		}
	}

	// Token: 0x040015AF RID: 5551
	private GridPosition goal;

	// Token: 0x040015B0 RID: 5552
	private Terrain terrain;

	// Token: 0x040015B1 RID: 5553
	private PathFinder2 pathFinder2;
}
