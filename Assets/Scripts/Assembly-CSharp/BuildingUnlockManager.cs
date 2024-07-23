using System;
using System.Collections.Generic;

// Token: 0x02000117 RID: 279
public class BuildingUnlockManager
{
	// Token: 0x06000A37 RID: 2615 RVA: 0x0003F824 File Offset: 0x0003DA24
	public BuildingUnlockManager()
	{
		this.unlockedBuildings = new List<int>();
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0003F838 File Offset: 0x0003DA38
	public void UnlockBuilding(int buildingDid)
	{
		if (!this.unlockedBuildings.Contains(buildingDid))
		{
			this.unlockedBuildings.Add(buildingDid);
		}
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x0003F858 File Offset: 0x0003DA58
	public bool CheckBuildingUnlock(int buildingdid)
	{
		return this.unlockedBuildings.Contains(buildingdid);
	}

	// Token: 0x040006FE RID: 1790
	private List<int> unlockedBuildings;
}
