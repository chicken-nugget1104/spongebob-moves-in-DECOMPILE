using System;
using System.Collections.Generic;

// Token: 0x02000104 RID: 260
public class TaskUpdateAction : PersistedTriggerableAction
{
	// Token: 0x06000944 RID: 2372 RVA: 0x0003A5A8 File Offset: 0x000387A8
	public TaskUpdateAction(Task pTask) : base("tua", Identity.Null())
	{
		this.m_pTask = pTask;
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06000945 RID: 2373 RVA: 0x0003A5C4 File Offset: 0x000387C4
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0003A5C8 File Offset: 0x000387C8
	public new static TaskUpdateAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "task_invariable");
		Task pTask = new Task(null, pInvariableData, true);
		return new TaskUpdateAction(pTask);
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0003A5F4 File Offset: 0x000387F4
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("task_invariable", this.m_pTask.m_pTaskData.GetInvariableData());
		return dictionary;
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0003A624 File Offset: 0x00038824
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		Task activeTask = pGame.taskManager.GetActiveTask(this.m_pTask.m_pTaskData.m_nDID);
		if (activeTask != null)
		{
			activeTask.UpdateModifiableData(this.m_pTask.m_ulStartTime, this.m_pTask.m_ulCompleteTime);
		}
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0003A678 File Offset: 0x00038878
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["tasks_v2"];
		int nDID = this.m_pTask.m_pTaskData.m_nDID;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)list[i];
			if (TFUtils.LoadInt(dictionary, TaskData._sDID) == nDID)
			{
				Task.UpdateModifiableDataForDict(dictionary, this.m_pTask);
				break;
			}
		}
		base.Confirm(pGameState);
	}

	// Token: 0x04000677 RID: 1655
	public const string TASK_UPDATE = "tua";

	// Token: 0x04000678 RID: 1656
	private Task m_pTask;
}
