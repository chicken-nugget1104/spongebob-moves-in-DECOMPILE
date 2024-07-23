using System;
using System.Collections.Generic;

// Token: 0x0200046D RID: 1133
public static class TreasureStateSetup
{
	// Token: 0x060023A9 RID: 9129 RVA: 0x000DC24C File Offset: 0x000DA44C
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine, bool friendMode)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		if (friendMode)
		{
			actions.Add("buried", Simulated.Treasure.Buried_Friend);
			actions.Add("claiming", Simulated.Treasure.Claiming_Friend);
		}
		else
		{
			actions.Add("buried", Simulated.Treasure.Buried);
			actions.Add("claiming", Simulated.Treasure.Claiming);
		}
		actions.Add("uncovering", Simulated.Treasure.Uncovering);
		actions.Add("deleting", Simulated.Treasure.Deleting);
		foreach (Simulated.StateAction state in actions.Values)
		{
			machine.AddState(state);
		}
		machine.AddTransition(actions["buried"], Command.TYPE.CLICKED, actions["uncovering"]);
		machine.AddTransition(actions["uncovering"], Command.TYPE.COMPLETE, actions["claiming"]);
		machine.AddTransition(actions["claiming"], Command.TYPE.CLICKED, actions["deleting"]);
	}
}
