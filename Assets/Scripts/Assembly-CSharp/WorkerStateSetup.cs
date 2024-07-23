using System;
using System.Collections.Generic;

// Token: 0x02000479 RID: 1145
public static class WorkerStateSetup
{
	// Token: 0x060023D3 RID: 9171 RVA: 0x000DD0B0 File Offset: 0x000DB2B0
	public static void Generate(out Dictionary<string, Simulated.StateAction> actions, out StateMachine<Simulated.StateAction, Command.TYPE> machine)
	{
		actions = new Dictionary<string, Simulated.StateAction>();
		machine = new StateMachine<Simulated.StateAction, Command.TYPE>();
		actions.Add("idle", Simulated.Worker.Idle);
		actions.Add("moving", Simulated.Worker.Moving);
		actions.Add("erecting", Simulated.Worker.Erecting);
		actions.Add("returning", Simulated.Worker.Returning);
		foreach (Simulated.StateAction state in actions.Values)
		{
			machine.AddState(state);
		}
		machine.AddTransition(actions["idle"], Command.TYPE.MOVE, actions["moving"]);
		machine.AddTransition(actions["idle"], Command.TYPE.RETURN, actions["returning"]);
		machine.AddTransition(actions["idle"], Command.TYPE.ERECT, actions["erecting"]);
		machine.AddTransition(actions["moving"], Command.TYPE.ERECT, actions["erecting"]);
		machine.AddTransition(actions["moving"], Command.TYPE.COMPLETE, actions["idle"]);
		machine.AddTransition(actions["moving"], Command.TYPE.RETURN, actions["returning"]);
		machine.AddTransition(actions["returning"], Command.TYPE.COMPLETE, actions["idle"]);
		machine.AddTransition(actions["erecting"], Command.TYPE.ERECT, actions["erecting"]);
		machine.AddTransition(actions["erecting"], Command.TYPE.COMPLETE, actions["idle"]);
		machine.AddTransition(actions["erecting"], Command.TYPE.RETURN, actions["returning"]);
	}
}
