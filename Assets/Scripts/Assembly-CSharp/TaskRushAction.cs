using System;
using System.Collections.Generic;

// Token: 0x02000102 RID: 258
public class TaskRushAction : PersistedTriggerableAction
{
	// Token: 0x06000936 RID: 2358 RVA: 0x0003A290 File Offset: 0x00038490
	public TaskRushAction(Task pTask, Cost pCost) : base("tra", Identity.Null())
	{
		this.m_pTask = pTask;
		this.m_pCost = pCost;
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000937 RID: 2359 RVA: 0x0003A2B0 File Offset: 0x000384B0
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0003A2B4 File Offset: 0x000384B4
	public new static TaskRushAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "task_invariable");
		Task pTask = new Task(null, pInvariableData, true);
		Cost pCost = Cost.FromDict((Dictionary<string, object>)pData["cost"]);
		return new TaskRushAction(pTask, pCost);
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0003A2F8 File Offset: 0x000384F8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("task_invariable", this.m_pTask.m_pTaskData.GetInvariableData());
		dictionary.Add("cost", this.m_pCost.ToDict());
		return dictionary;
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0003A340 File Offset: 0x00038540
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		Task activeTask = pGame.taskManager.GetActiveTask(this.m_pTask.m_pTaskData.m_nDID);
		if (activeTask != null)
		{
			activeTask.m_ulCompleteTime = ulUtcNow;
		}
		pGame.resourceManager.Apply(this.m_pCost, pGame);
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0003A390 File Offset: 0x00038590
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["tasks_v2"];
		int nDID = this.m_pTask.m_pTaskData.m_nDID;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)list[i];
			if (TFUtils.LoadInt(dictionary, TaskData._sDID) == nDID && dictionary.ContainsKey(Task._sCOMPLETE_TIME))
			{
				dictionary[Task._sCOMPLETE_TIME] = TFUtils.EpochTime();
			}
		}
		ResourceManager.ApplyCostToGameState(this.m_pCost, pGameState);
		base.Confirm(pGameState);
	}

	// Token: 0x04000672 RID: 1650
	public const string TASK_RUSH = "tra";

	// Token: 0x04000673 RID: 1651
	private Task m_pTask;

	// Token: 0x04000674 RID: 1652
	private Cost m_pCost;
}
