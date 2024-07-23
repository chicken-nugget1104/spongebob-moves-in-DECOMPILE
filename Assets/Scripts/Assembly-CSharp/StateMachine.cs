using System;
using System.Collections.Generic;

// Token: 0x02000455 RID: 1109
public class StateMachine<State, Command>
{
	// Token: 0x06002233 RID: 8755 RVA: 0x000D3078 File Offset: 0x000D1278
	public void AddState(State state)
	{
		if (!this.states.ContainsKey(state))
		{
			this.states.Add(state, new StateMachine<State, Command>.Entry(state));
		}
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000D30A0 File Offset: 0x000D12A0
	public void AddState_Unsafe(State state)
	{
		this.states.Add(state, new StateMachine<State, Command>.Entry(state));
	}

	// Token: 0x1700050C RID: 1292
	// (get) Token: 0x06002235 RID: 8757 RVA: 0x000D30B4 File Offset: 0x000D12B4
	public ICollection<State> States
	{
		get
		{
			return this.states.Keys;
		}
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x000D30C4 File Offset: 0x000D12C4
	public void AddTransition(State current, Command command, State result)
	{
		this.states[current].transitions.Add(command, this.states[result]);
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x000D30EC File Offset: 0x000D12EC
	public void AddDelegate(State deferer, Command command, State handler)
	{
		this.states[deferer].delegates.Add(command, this.states[handler]);
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x000D3114 File Offset: 0x000D1314
	public bool Transition(State current, Command command, out State result)
	{
		StateMachine<State, Command>.Entry entry;
		if (this.states[current].Transition(command, out entry))
		{
			result = entry.state;
			return true;
		}
		result = default(State);
		return false;
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000D3158 File Offset: 0x000D1358
	public bool Delegate(State current, Command command, out State result)
	{
		StateMachine<State, Command>.Entry entry;
		if (this.states[current].Delegate(command, out entry))
		{
			result = entry.state;
			return true;
		}
		result = default(State);
		return false;
	}

	// Token: 0x04001525 RID: 5413
	private Dictionary<State, StateMachine<State, Command>.Entry> states = new Dictionary<State, StateMachine<State, Command>.Entry>();

	// Token: 0x02000456 RID: 1110
	private class Entry
	{
		// Token: 0x0600223A RID: 8762 RVA: 0x000D319C File Offset: 0x000D139C
		public Entry(State state)
		{
			this.state = state;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x000D31C4 File Offset: 0x000D13C4
		public bool Transition(Command command, out StateMachine<State, Command>.Entry result)
		{
			return this.transitions.TryGetValue(command, out result);
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x000D31D4 File Offset: 0x000D13D4
		public bool Delegate(Command command, out StateMachine<State, Command>.Entry result)
		{
			return this.delegates.TryGetValue(command, out result);
		}

		// Token: 0x04001526 RID: 5414
		public State state;

		// Token: 0x04001527 RID: 5415
		public Dictionary<Command, StateMachine<State, Command>.Entry> transitions = new Dictionary<Command, StateMachine<State, Command>.Entry>();

		// Token: 0x04001528 RID: 5416
		public Dictionary<Command, StateMachine<State, Command>.Entry> delegates = new Dictionary<Command, StateMachine<State, Command>.Entry>();
	}
}
