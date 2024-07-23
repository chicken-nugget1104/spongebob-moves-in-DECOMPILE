using System;
using System.Collections.Generic;

// Token: 0x0200026D RID: 621
public static class SimulatedTrigger
{
	// Token: 0x0600140D RID: 5133 RVA: 0x0008A248 File Offset: 0x00088448
	public static ITrigger CreateTrigger(Simulated simulated, string type)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["simulated_guid"] = simulated.entity.Id;
		dictionary["simulated_id"] = simulated.entity.DefinitionId;
		dictionary["simulated_type"] = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		dictionary["simulated"] = simulated;
		return new Trigger(type, dictionary);
	}

	// Token: 0x04000E0B RID: 3595
	public const string SIMULATED = "simulated";

	// Token: 0x04000E0C RID: 3596
	public const string CONSTRUCTION_COMPLETE = "contruction_complete";
}
