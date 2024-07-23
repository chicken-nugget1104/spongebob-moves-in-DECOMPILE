using System;
using System.Collections.Generic;

// Token: 0x02000175 RID: 373
public interface IResourceProgressCalculator
{
	// Token: 0x06000CD6 RID: 3286
	void GetRewardsForIncreasingResource(Simulation simulation, Dictionary<int, Resource> currentResources, int amountToIncrease, out List<Reward> rewards);

	// Token: 0x06000CD7 RID: 3287
	float ComputeProgressPercentage(Dictionary<int, Resource> currentResources);

	// Token: 0x06000CD8 RID: 3288
	string ComputeProgressFraction(Dictionary<int, Resource> currentResources);

	// Token: 0x06000CD9 RID: 3289
	int GetResourceType();
}
