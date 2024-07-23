﻿using System;
using System.Collections.Generic;

// Token: 0x02000476 RID: 1142
public static class WandererStateSetup
{
	// Token: 0x060023D0 RID: 9168 RVA: 0x000DCE38 File Offset: 0x000DB038
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("spawn", Simulated.Wanderer.Spawn);
		actions.Add("idle", Simulated.Wanderer.Idle);
		actions.Add("wandering", Simulated.Wanderer.Wandering);
		actions.Add("clicked", Simulated.Wanderer.Clicked);
		actions.Add("fleeing", Simulated.Wanderer.Fleeing);
		actions.Add("hidden", Simulated.Wanderer.Hidden);
		actions.Add("cheering", Simulated.Wanderer.Cheering);
		foreach (Simulated.StateAction state in actions.Values)
		{
			machine.AddState(state);
		}
		machine.AddTransition(actions["spawn"], Command.TYPE.WANDER, actions["wandering"]);
		machine.AddTransition(actions["spawn"], Command.TYPE.ABORT, actions["hidden"]);
		machine.AddTransition(actions["idle"], Command.TYPE.WANDER, actions["wandering"]);
		machine.AddTransition(actions["idle"], Command.TYPE.CLICKED, actions["clicked"]);
		machine.AddTransition(actions["wandering"], Command.TYPE.IDLE_PAUSE, actions["idle"]);
		machine.AddTransition(actions["wandering"], Command.TYPE.CLICKED, actions["clicked"]);
		machine.AddTransition(actions["clicked"], Command.TYPE.CHEER, actions["cheering"]);
		machine.AddTransition(actions["clicked"], Command.TYPE.FLEE, actions["fleeing"]);
		machine.AddTransition(actions["fleeing"], Command.TYPE.COMPLETE, actions["hidden"]);
		machine.AddTransition(actions["cheering"], Command.TYPE.COMPLETE, actions["wandering"]);
		machine.AddTransition(actions["hidden"], Command.TYPE.SPAWN, actions["spawn"]);
	}
}
