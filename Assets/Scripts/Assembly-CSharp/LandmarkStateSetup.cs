using System;
using System.Collections.Generic;

// Token: 0x02000422 RID: 1058
public static class LandmarkStateSetup
{
	// Token: 0x060020B5 RID: 8373 RVA: 0x000CA38C File Offset: 0x000C858C
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine, bool friendMode)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("unpurchased", Simulated.Landmark.Unpurchased);
		actions.Add("inactive", Simulated.Landmark.Inactive);
		actions.Add("active", Simulated.Landmark.Active);
		foreach (Simulated.StateAction state in actions.Values)
		{
			machine.AddState(state);
		}
		if (friendMode)
		{
			return;
		}
		machine.AddTransition(actions["unpurchased"], Command.TYPE.PURCHASE, actions["inactive"]);
		machine.AddTransition(actions["inactive"], Command.TYPE.ACTIVATE, actions["active"]);
		machine.AddTransition(actions["active"], Command.TYPE.COMPLETE, actions["inactive"]);
		machine.AddTransition(actions["active"], Command.TYPE.ABORT, actions["inactive"]);
	}
}
