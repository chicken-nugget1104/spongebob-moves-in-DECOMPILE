using System;
using System.Collections.Generic;

// Token: 0x020001B8 RID: 440
public class ResourceCalculatorManager
{
	// Token: 0x06000EF7 RID: 3831 RVA: 0x0005E31C File Offset: 0x0005C51C
	public ResourceCalculatorManager(LevelingManager levelingManager)
	{
		this.calculators = new Dictionary<int, IResourceProgressCalculator>();
		this.calculators[ResourceManager.XP] = levelingManager;
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0005E34C File Offset: 0x0005C54C
	public IResourceProgressCalculator GetResourceCalculator(int resourceId)
	{
		IResourceProgressCalculator result = null;
		this.calculators.TryGetValue(resourceId, out result);
		return result;
	}

	// Token: 0x040009FD RID: 2557
	private Dictionary<int, IResourceProgressCalculator> calculators;
}
