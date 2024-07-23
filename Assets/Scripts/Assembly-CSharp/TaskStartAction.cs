using System;
using System.Collections.Generic;

// Token: 0x02000103 RID: 259
public class TaskStartAction : PersistedSimulatedAction
{
	// Token: 0x0600093C RID: 2364 RVA: 0x0003A444 File Offset: 0x00038644
	public TaskStartAction(Task pTask) : base("tsa", Identity.Null(), null)
	{
		this.m_pTask = pTask;
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x0600093D RID: 2365 RVA: 0x0003A460 File Offset: 0x00038660
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0003A464 File Offset: 0x00038664
	public new static TaskStartAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "task_invariable");
		Task pTask = new Task(null, pInvariableData, true);
		return new TaskStartAction(pTask);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0003A490 File Offset: 0x00038690
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("task_invariable", this.m_pTask.m_pTaskData.GetInvariableData());
		return dictionary;
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0003A4C0 File Offset: 0x000386C0
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		pGame.taskManager.AddActiveTask(pGame, new Task(pGame, this.m_pTask.GetInvariableData(), false), false);
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0003A4F4 File Offset: 0x000386F4
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["tasks_v2"];
		list.Add(this.m_pTask.GetInvariableData());
		base.Confirm(pGameState);
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0003A53C File Offset: 0x0003873C
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
		base.AddMoreDataToTrigger(ref pData);
		pData.Add("task_id", this.m_pTask.m_pTaskData.m_nDID);
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0003A574 File Offset: 0x00038774
	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x04000675 RID: 1653
	public const string TASK_START = "tsa";

	// Token: 0x04000676 RID: 1654
	private Task m_pTask;
}
