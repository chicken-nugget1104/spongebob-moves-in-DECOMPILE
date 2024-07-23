using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class SimulatedMatcher : Matcher
{
	// Token: 0x06000D88 RID: 3464 RVA: 0x00052894 File Offset: 0x00050A94
	public static SimulatedMatcher FromDict(Dictionary<string, object> dict)
	{
		SimulatedMatcher simulatedMatcher = new SimulatedMatcher();
		simulatedMatcher.RegisterProperty("simulated_guid", dict);
		simulatedMatcher.RegisterProperty("simulated_id", dict);
		simulatedMatcher.RegisterProperty("simulated_type", dict);
		if (simulatedMatcher.IsRequired("simulated_id"))
		{
			TFUtils.Assert(simulatedMatcher.IsRequired("simulated_type"), "You need to include a simulated_type in the condition of the json to go with the specified ID");
		}
		return simulatedMatcher;
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x000528F4 File Offset: 0x00050AF4
	public override string DescribeSubject(Game game)
	{
		if (this.IsRequired("simulated_id"))
		{
			Blueprint blueprint = EntityManager.GetBlueprint(this.GetTarget("simulated_type"), int.Parse(this.GetTarget("simulated_id")), false);
			if (blueprint == null)
			{
				TFUtils.Assert(this.GetTarget("simulated_type") != null && this.GetTarget("simulated_type") != string.Empty, "You need to include a \"simulated_type\" for simulated matching Quest: " + this.ToString());
				blueprint = EntityManager.GetBlueprint(this.GetTarget("simulated_type"), int.Parse(this.GetTarget("simulated_id")), false);
				return "simulated with did " + this.GetTarget("simulated_id");
			}
			return (string)blueprint.Invariable["name"];
		}
		else
		{
			if (!this.IsRequired("simulated_guid"))
			{
				return string.Empty;
			}
			if (game == null)
			{
				return "simulated with id " + this.GetTarget("simulated_guid");
			}
			Identity id = new Identity(this.GetTarget("simulated_guid"));
			Debug.LogError((string)game.simulation.FindSimulated(id).entity.Invariable["name"]);
			return (string)game.simulation.FindSimulated(id).entity.Invariable["name"];
		}
	}

	// Token: 0x040008F8 RID: 2296
	public const string INSTANCE_ID = "simulated_guid";

	// Token: 0x040008F9 RID: 2297
	public const string DEFINITION_ID = "simulated_id";

	// Token: 0x040008FA RID: 2298
	public const string TYPE = "simulated_type";
}
