using System;
using System.Collections.Generic;

// Token: 0x02000101 RID: 257
public class TaskCompleteAction : PersistedSimulatedAction
{
	// Token: 0x0600092E RID: 2350 RVA: 0x00039F94 File Offset: 0x00038194
	public TaskCompleteAction(Identity ID, Task pTask, Reward pReward, int nTaskCompletionCount) : base("tca", ID, "TaskPickup")
	{
		this.m_pTask = pTask;
		this.m_pReward = pReward;
		this.m_nTaskCompletionCount = nTaskCompletionCount;
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x0600092F RID: 2351 RVA: 0x00039FC0 File Offset: 0x000381C0
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00039FC4 File Offset: 0x000381C4
	public new static TaskCompleteAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "task_invariable");
		Task pTask = new Task(null, pInvariableData, true);
		Reward pReward = Reward.FromObject(pData["reward"]);
		Identity id = new Identity((string)pData["target"]);
		int? num = TFUtils.TryLoadInt(pData, "task_completion_count");
		TaskCompleteAction taskCompleteAction = new TaskCompleteAction(id, pTask, pReward, (num == null) ? 0 : num.Value);
		taskCompleteAction.DropTargetDataFromDict(pData);
		return taskCompleteAction;
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0003A048 File Offset: 0x00038248
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("task_invariable", this.m_pTask.m_pTaskData.GetInvariableData());
		dictionary.Add("reward", this.m_pReward.ToDict());
		dictionary.Add("task_completion_count", this.m_nTaskCompletionCount);
		base.DropTargetDataToDict(dictionary);
		return dictionary;
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0003A0AC File Offset: 0x000382AC
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		int nDID = this.m_pTask.m_pTaskData.m_nDID;
		pGame.taskManager.RemoveActiveTask(nDID);
		pGame.ApplyReward(this.m_pReward, ulUtcNow, true);
		base.AddPickup(pGame.simulation);
		pGame.taskManager.SetTaskCompletionCount(nDID, this.m_nTaskCompletionCount);
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0003A10C File Offset: 0x0003830C
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["tasks_v2"];
		int nDID = this.m_pTask.m_pTaskData.m_nDID;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			Dictionary<string, object> d = (Dictionary<string, object>)list[i];
			if (TFUtils.LoadInt(d, TaskData._sDID) == nDID)
			{
				list.RemoveAt(i);
				break;
			}
		}
		Dictionary<string, object> dictionary = (Dictionary<string, object>)((Dictionary<string, object>)pGameState["farm"])["task_completion_counts"];
		string key = nDID.ToString();
		if (dictionary.ContainsKey(key))
		{
			dictionary[key] = this.m_nTaskCompletionCount;
		}
		else
		{
			dictionary.Add(key, this.m_nTaskCompletionCount);
		}
		RewardManager.ApplyToGameState(this.m_pReward, base.GetTime(), pGameState);
		base.AddPickupToGameState(pGameState);
		base.Confirm(pGameState);
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0003A218 File Offset: 0x00038418
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
		base.AddMoreDataToTrigger(ref pData);
		this.m_pReward.AddDataToTrigger(ref pData);
		pData.Add("task_id", this.m_pTask.m_pTaskData.m_nDID);
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0003A25C File Offset: 0x0003845C
	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x0400066D RID: 1645
	public const string TASK_COMPLETE = "tca";

	// Token: 0x0400066E RID: 1646
	public const string PICKUP_TRIGGERTYPE = "TaskPickup";

	// Token: 0x0400066F RID: 1647
	private Task m_pTask;

	// Token: 0x04000670 RID: 1648
	private Reward m_pReward;

	// Token: 0x04000671 RID: 1649
	private int m_nTaskCompletionCount;
}
