using System;
using System.Collections.Generic;

// Token: 0x02000248 RID: 584
public class SessionActionManager : ITriggerObserver
{
	// Token: 0x060012D6 RID: 4822 RVA: 0x0008203C File Offset: 0x0008023C
	public SessionActionManager()
	{
		this.listeners = new Dictionary<string, Action<SessionActionTracker>>();
		this.readiedActions = new List<SessionActionTracker>();
		this.postponedActions = new List<SessionActionTracker>();
		this.runningActions = new List<SessionActionTracker>();
		this.spawns = new List<ISessionActionSpawn>();
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x00082094 File Offset: 0x00080294
	public void SetActionHandler(string id, Session session, List<SBGUIScreen> searchableScreens, SessionActionManager.Handler handler)
	{
		TFUtils.Assert(!this.listeners.ContainsKey(id), "Clobbering existing action handler with the given id(" + id + ")");
		this.listeners[id] = delegate(SessionActionTracker a)
		{
			handler(session, searchableScreens, a);
		};
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x000820FC File Offset: 0x000802FC
	public void ClearActionHandler(string id, Session session)
	{
		this.listeners.Remove(id);
		this.ClearStaleTrackers(id, session);
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x00082114 File Offset: 0x00080314
	public void ClearActions()
	{
		foreach (ISessionActionSpawn sessionActionSpawn in this.spawns)
		{
			sessionActionSpawn.Destroy();
		}
		this.spawns.Clear();
		this.readiedActions.Clear();
		this.postponedActions.Clear();
		this.runningActions.Clear();
		this.listeners.Clear();
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x000821B0 File Offset: 0x000803B0
	public void ClearStaleTrackers(string id, Session session)
	{
		foreach (ISessionActionSpawn sessionActionSpawn in this.spawns)
		{
			sessionActionSpawn.Destroy();
		}
		this.spawns.Clear();
		foreach (SessionActionTracker sessionActionTracker in this.runningActions.ToArray())
		{
			if (sessionActionTracker.Definition.ClearOnSessionChange && sessionActionTracker.Status != SessionActionTracker.StatusCode.OBLITERATED && sessionActionTracker.Status != SessionActionTracker.StatusCode.FINISHED_SUCCESS)
			{
				this.runningActions.Remove(sessionActionTracker);
				sessionActionTracker.ReActivate(session.TheGame);
				this.Request(sessionActionTracker, session.TheGame);
			}
		}
		this.MakeDirty();
	}

	// Token: 0x060012DB RID: 4827 RVA: 0x000822A0 File Offset: 0x000804A0
	public bool ExistsActionHandler(string id)
	{
		return this.listeners.ContainsKey(id);
	}

	// Token: 0x060012DC RID: 4828 RVA: 0x000822B0 File Offset: 0x000804B0
	public void RequestProcess(Game game)
	{
		this.MakeDirty();
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x000822B8 File Offset: 0x000804B8
	public void ProcessTrigger(ITrigger trigger, Game game)
	{
		this.triggersToProcess[trigger] = true;
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x000822C8 File Offset: 0x000804C8
	public void Request(SessionActionTracker sessionAction, Game game)
	{
		this.Request(sessionAction, game, sessionAction.Tag);
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x000822D8 File Offset: 0x000804D8
	public void Request(SessionActionTracker sessionAction, Game game, string tagOverride)
	{
		sessionAction.Tag = tagOverride;
		TFUtils.Assert(sessionAction != null, "Don't request to track a null session action.");
		TFUtils.Assert(!this.readiedActions.Contains(sessionAction), "Re-requesting a session action. You should not do that.");
		TFUtils.Assert(!this.postponedActions.Contains(sessionAction), "Re-requesting a session action. You should not do that.");
		TFUtils.Assert(!this.runningActions.Contains(sessionAction), "Re-requesting a session action. You should not do that.");
		if (sessionAction.Status != SessionActionTracker.StatusCode.INITIAL && sessionAction.Status != SessionActionTracker.StatusCode.POSTPONED)
		{
			TFUtils.Assert(false, "Requested session action should be in the initial state. Encountered:" + sessionAction.Status);
		}
		if (sessionAction.ShouldSetPostponeTimer())
		{
			sessionAction.StartPostponeTimer();
			sessionAction.MarkPostponed();
			this.postponedActions.Add(sessionAction);
		}
		else
		{
			sessionAction.MarkRequested();
			this.readiedActions.Add(sessionAction);
			this.MakeDirty();
		}
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x000823BC File Offset: 0x000805BC
	public void Obliterate(SessionActionDefinition actionDefinition, Game game)
	{
		List<SessionActionTracker> list = this.AllActions();
		foreach (SessionActionTracker sessionActionTracker in list)
		{
			if (actionDefinition == sessionActionTracker.Definition)
			{
				this.Obliterate(sessionActionTracker, game);
				break;
			}
		}
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x00082438 File Offset: 0x00080638
	public void Obliterate(SessionActionTracker actionTracker, Game game)
	{
		if (actionTracker.Status != SessionActionTracker.StatusCode.OBLITERATED)
		{
			actionTracker.MarkObliterated(game);
			this.MakeDirty();
		}
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x00082454 File Offset: 0x00080654
	public void ObliterateAnyTagged(string tag, Game game)
	{
		Predicate<SessionActionTracker> match = (SessionActionTracker action) => action.Tag == tag;
		List<SessionActionTracker> list = this.AllActions().FindAll(match);
		foreach (SessionActionTracker actionTracker in list)
		{
			this.Obliterate(actionTracker, game);
		}
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x000824E0 File Offset: 0x000806E0
	public void OnUpdate(Game game)
	{
		List<SessionActionTracker> list = new List<SessionActionTracker>(this.postponedActions);
		foreach (SessionActionTracker sessionActionTracker in list)
		{
			if (sessionActionTracker.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				this.postponedActions.Remove(sessionActionTracker);
			}
			if (sessionActionTracker.IsPostponeComplete())
			{
				this.postponedActions.Remove(sessionActionTracker);
				this.Request(sessionActionTracker, game);
			}
		}
		List<ITrigger> list2 = new List<ITrigger>();
		foreach (KeyValuePair<ITrigger, bool> keyValuePair in this.triggersToProcess)
		{
			list2.Add(keyValuePair.Key);
		}
		this.triggersToProcess.Clear();
		foreach (ITrigger trigger in list2)
		{
			this.ProcessActions(trigger, game);
		}
		List<ISessionActionSpawn> list3 = new List<ISessionActionSpawn>();
		foreach (ISessionActionSpawn sessionActionSpawn in this.spawns)
		{
			if (sessionActionSpawn.OnUpdate(game) == SessionActionManager.SpawnReturnCode.KILL)
			{
				list3.Add(sessionActionSpawn);
			}
		}
		foreach (ISessionActionSpawn item in list3)
		{
			this.spawns.Remove(item);
		}
		if (list3.Count > 0)
		{
			this.MakeDirty();
		}
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x00082728 File Offset: 0x00080928
	public void RegisterSpawn(ISessionActionSpawn spawn)
	{
		this.spawns.Add(spawn);
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x00082738 File Offset: 0x00080938
	private void ProcessActions(ITrigger trigger, Game game)
	{
		bool flag = false;
		List<SessionActionTracker> list = new List<SessionActionTracker>();
		List<SessionActionTracker> list2 = new List<SessionActionTracker>(this.readiedActions);
		foreach (SessionActionTracker sessionActionTracker in list2)
		{
			if (sessionActionTracker.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				list.Add(sessionActionTracker);
			}
			else
			{
				sessionActionTracker.ActivationProgress.Recalculate(game, trigger, null);
				if (sessionActionTracker.ActivationProgress.Examine() == ConditionResult.PASS)
				{
					sessionActionTracker.PreActivate(game);
					ICollection<Action<SessionActionTracker>> collection = new List<Action<SessionActionTracker>>(this.listeners.Values);
					foreach (Action<SessionActionTracker> action in collection)
					{
						action(sessionActionTracker);
					}
					if (sessionActionTracker.Status == SessionActionTracker.StatusCode.STARTED)
					{
						this.runningActions.Add(sessionActionTracker);
						flag = true;
					}
					else if (sessionActionTracker.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS || sessionActionTracker.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE)
					{
						list.Add(sessionActionTracker);
						flag = true;
					}
				}
			}
		}
		foreach (SessionActionTracker sessionActionTracker2 in this.runningActions)
		{
			this.readiedActions.Remove(sessionActionTracker2);
			flag |= sessionActionTracker2.ActiveProcess(game);
			sessionActionTracker2.SuccessProgress.Recalculate(game, trigger, null);
			if (sessionActionTracker2.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS || sessionActionTracker2.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE || sessionActionTracker2.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				sessionActionTracker2.PostComplete(game);
				list.Add(sessionActionTracker2);
			}
		}
		foreach (SessionActionTracker sessionActionTracker3 in list)
		{
			this.readiedActions.Remove(sessionActionTracker3);
			this.postponedActions.Remove(sessionActionTracker3);
			this.runningActions.Remove(sessionActionTracker3);
			sessionActionTracker3.Destroy(game);
			if (sessionActionTracker3.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE && sessionActionTracker3.RepeatOnFail)
			{
				this.Request(new SessionActionTracker(sessionActionTracker3.Definition, new ConstantCondition(sessionActionTracker3.Definition.StartConditions.FindNextId(), true), sessionActionTracker3.Tag, false), game);
			}
		}
		if (flag)
		{
			this.MakeDirty();
		}
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x00082A14 File Offset: 0x00080C14
	private void MakeDirty()
	{
		this.triggersToProcess[Trigger.Null] = true;
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x00082A28 File Offset: 0x00080C28
	private bool IsDirty()
	{
		return this.triggersToProcess.Count > 0;
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x00082A38 File Offset: 0x00080C38
	private List<SessionActionTracker> AllActions()
	{
		List<SessionActionTracker> list = new List<SessionActionTracker>(this.readiedActions);
		list.AddRange(this.runningActions);
		list.AddRange(this.postponedActions);
		return list;
	}

	// Token: 0x04000D0E RID: 3342
	public const bool CONDENSED_LOGGING = true;

	// Token: 0x04000D0F RID: 3343
	private Dictionary<string, Action<SessionActionTracker>> listeners;

	// Token: 0x04000D10 RID: 3344
	private List<SessionActionTracker> readiedActions;

	// Token: 0x04000D11 RID: 3345
	private List<SessionActionTracker> postponedActions;

	// Token: 0x04000D12 RID: 3346
	private List<SessionActionTracker> runningActions;

	// Token: 0x04000D13 RID: 3347
	private List<ISessionActionSpawn> spawns;

	// Token: 0x04000D14 RID: 3348
	private Dictionary<ITrigger, bool> triggersToProcess = new Dictionary<ITrigger, bool>();

	// Token: 0x02000249 RID: 585
	public enum SpawnReturnCode
	{
		// Token: 0x04000D16 RID: 3350
		KEEP_ALIVE,
		// Token: 0x04000D17 RID: 3351
		KILL
	}

	// Token: 0x020004AB RID: 1195
	// (Invoke) Token: 0x0600250F RID: 9487
	public delegate void Handler(Session session, List<SBGUIScreen> hud, SessionActionTracker action);
}
