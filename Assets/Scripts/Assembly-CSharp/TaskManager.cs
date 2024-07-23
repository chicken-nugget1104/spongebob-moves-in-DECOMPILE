using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000263 RID: 611
public class TaskManager
{
	// Token: 0x060013D2 RID: 5074 RVA: 0x00087A50 File Offset: 0x00085C50
	public TaskManager()
	{
		this.LoadFromSpreadsheet();
		this.m_pActiveTasks = new Dictionary<int, Task>();
		this.m_pBlueprintTaskMap = new Dictionary<int, int>();
		this.m_pSimulatedTaskMap = new Dictionary<string, List<int>>();
		this.m_pTaskCompletionCounts = new Dictionary<int, int>();
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x00087A98 File Offset: 0x00085C98
	public TaskData GetTaskData(int nDID, bool bDefaultActiveTaskData = false)
	{
		if (bDefaultActiveTaskData && this.m_pActiveTasks.ContainsKey(nDID))
		{
			return this.m_pActiveTasks[nDID].m_pTaskData;
		}
		if (this.m_pTaskDatas.ContainsKey(nDID))
		{
			return this.m_pTaskDatas[nDID];
		}
		return null;
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x00087AF0 File Offset: 0x00085CF0
	public List<TaskData> GetTaskDatasForSource(int nSourceDID, bool bDefaultActiveTaskData = false)
	{
		List<TaskData> list = new List<TaskData>();
		foreach (KeyValuePair<int, TaskData> keyValuePair in this.m_pTaskDatas)
		{
			if (bDefaultActiveTaskData && this.m_pActiveTasks.ContainsKey(keyValuePair.Key))
			{
				TaskData taskData = this.m_pActiveTasks[keyValuePair.Key].m_pTaskData;
				if (taskData.m_nSourceDID == nSourceDID)
				{
					list.Add(taskData);
				}
			}
			else
			{
				TaskData taskData = keyValuePair.Value;
				if (taskData.m_nSourceDID == nSourceDID)
				{
					list.Add(taskData);
				}
			}
		}
		return list;
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x00087BC0 File Offset: 0x00085DC0
	public List<int> GetActiveSourcesForTarget(Identity sIdentity)
	{
		List<int> list = new List<int>();
		if (sIdentity == null)
		{
			return list;
		}
		foreach (KeyValuePair<int, Task> keyValuePair in this.m_pActiveTasks)
		{
			if (keyValuePair.Value.m_pTargetIdentity != null && keyValuePair.Value.m_pTargetIdentity.Equals(sIdentity))
			{
				list.Add(keyValuePair.Value.m_pTaskData.m_nSourceDID);
			}
		}
		return list;
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x00087C70 File Offset: 0x00085E70
	public List<int> GetActiveSourcesWithMatchBonusForTarget(Simulation pSimulation, Identity sIdentity)
	{
		List<int> list = new List<int>();
		if (sIdentity == null)
		{
			return list;
		}
		foreach (KeyValuePair<int, Task> keyValuePair in this.m_pActiveTasks)
		{
			if (keyValuePair.Value.m_pTargetIdentity != null && keyValuePair.Value.m_pTargetIdentity.Equals(sIdentity))
			{
				int nSourceDID = keyValuePair.Value.m_pTaskData.m_nSourceDID;
				Simulated simulated = pSimulation.FindSimulated(new int?(nSourceDID));
				if (simulated.HasEntity<ResidentEntity>())
				{
					ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
					if (entity.MatchBonus != null)
					{
						list.Add(nSourceDID);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x00087D4C File Offset: 0x00085F4C
	public bool IsTaskAvailable(Game pGame, int nDID, bool bDefaultActiveTaskData = false)
	{
		TaskData taskData = this.GetTaskData(nDID, bDefaultActiveTaskData);
		return this.GetTaskBlockedStatus(pGame, taskData, -1).m_eTaskBlockedType == TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone;
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x00087D74 File Offset: 0x00085F74
	public bool IsTaskActive(int nDID)
	{
		return this.m_pActiveTasks.ContainsKey(nDID);
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x00087D84 File Offset: 0x00085F84
	public TaskManager.TaskBlockedStatus GetTaskBlockedStatus(Game pGame, TaskData pTaskData, int nOverwriteSourceCostumeDID = -1)
	{
		TaskManager.TaskBlockedStatus taskBlockedStatus = new TaskManager.TaskBlockedStatus();
		if (pTaskData == null)
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eNoTask, 0);
			return taskBlockedStatus;
		}
		int num = pTaskData.m_nDID;
		if (this.IsTaskActive(num))
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eActive, num);
		}
		num = this.GetTaskCompletionCount(num);
		if (!pTaskData.m_bRepeatable && num > 0)
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eRepeatable, num);
		}
		Simulation simulation = pGame.simulation;
		num = pTaskData.m_nSourceDID;
		if (this.GetTaskingStateForSimulated(pGame.simulation, num, null) != TaskManager._eBlueprintTaskingState.eNone)
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eSource, num);
		}
		Simulated simulated = simulation.FindSimulated(new int?(num));
		num = pTaskData.m_nSourceCostumeDID;
		bool flag = false;
		if (num >= 0)
		{
			if (nOverwriteSourceCostumeDID < 0)
			{
				if (simulated != null && simulated.HasEntity<ResidentEntity>())
				{
					ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
					if (entity != null && entity.CostumeDID != null && entity.CostumeDID.Value == num)
					{
						flag = true;
					}
				}
			}
			else if (nOverwriteSourceCostumeDID == num)
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (!flag)
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eSourceCostume, num);
		}
		num = pTaskData.m_nTargetDID;
		if (num >= 0 && this.GetAvailableSimulatedIdentity(pGame, pTaskData, num, true, true) == null)
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eTarget, num);
		}
		num = pTaskData.m_nPartnerDID;
		if (num >= 0 && this.GetTaskingStateForSimulated(pGame.simulation, num, null) != TaskManager._eBlueprintTaskingState.eNone)
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.ePartner, num);
		}
		flag = false;
		simulated = simulation.FindSimulated(new int?(num));
		num = pTaskData.m_nPartnerCostumeDID;
		if (num >= 0 && simulated != null && simulated.HasEntity<ResidentEntity>())
		{
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			if (entity != null && entity.CostumeDID != null && entity.CostumeDID.Value == num)
			{
				flag = true;
			}
		}
		else
		{
			flag = true;
		}
		if (!flag)
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.ePartnerCostume, num);
		}
		num = pTaskData.m_nMinLevel;
		if (num > 0 && pGame.resourceManager.PlayerLevelAmount < num)
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eLevel, pGame.resourceManager.PlayerLevelAmount);
		}
		num = pTaskData.m_nMicroEventDID;
		if (num >= 0 && (pGame.microEventManager.GetMicroEvent(num) == null || !pGame.microEventManager.IsMicroEventActive(num)))
		{
			MicroEvent microEvent = pGame.microEventManager.GetMicroEvent(num);
			if (microEvent == null || !microEvent.IsActive() || (pTaskData.m_bEventOnly && microEvent.IsCompleted()))
			{
				taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eMicroEvent, num);
			}
		}
		num = pTaskData.m_nActiveQuestDID;
		if (num >= 0 && !pGame.questManager.IsQuestActive((uint)num))
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eActiveQuest, num);
		}
		num = pTaskData.m_nQuestUnlockDID;
		if (num >= 0 && !pGame.questManager.IsQuestCompleted((uint)num))
		{
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eQuestUnlock, num);
		}
		num = pTaskData.m_nQuestRelockDid;
		if (num >= 0 && pGame.questManager.IsQuestCompleted((uint)num))
		{
			num = pTaskData.m_nQuestReunlockDid;
			if (num >= 0 && !pGame.questManager.IsQuestCompleted((uint)num))
			{
				taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eQuestRelock, num);
			}
			taskBlockedStatus.AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType.eQuestUnlock, num);
		}
		return taskBlockedStatus;
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x000880F0 File Offset: 0x000862F0
	public string GetTaskBlockedStatusString(Game pGame, TaskData pTaskData, int nOverwriteSourceCostumeDID = -1)
	{
		TaskManager.TaskBlockedStatus taskBlockedStatus = this.GetTaskBlockedStatus(pGame, pTaskData, nOverwriteSourceCostumeDID);
		string text = null;
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eLevel) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				Language.Get("!!TASK_LEVEL"),
				" ",
				pTaskData.m_nMinLevel.ToString(),
				", "
			});
		}
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.ePartner) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			Blueprint blueprint = EntityManager.GetBlueprint(EntityType.RESIDENT, pTaskData.m_nPartnerDID, false);
			text = text + Language.Get((string)blueprint.Invariable["name"]) + ", ";
		}
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eSourceCostume) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			CostumeManager.Costume costume = pGame.costumeManager.GetCostume(pTaskData.m_nSourceCostumeDID);
			text = text + Language.Get(costume.m_sName) + ", ";
		}
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.ePartnerCostume) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			CostumeManager.Costume costume2 = pGame.costumeManager.GetCostume(pTaskData.m_nPartnerCostumeDID);
			text = text + Language.Get(costume2.m_sName) + ", ";
		}
		if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eTarget) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
		{
			Blueprint blueprint2 = EntityManager.GetBlueprint(pTaskData.m_sTargetType, pTaskData.m_nTargetDID, false);
			text = text + Language.Get((string)blueprint2.Invariable["name"]) + ", ";
		}
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Remove(text.Length - 2);
		}
		return text;
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x00088278 File Offset: 0x00086478
	public Task CreateActiveTask(Game pGame, int nTaskDID)
	{
		if (!this.IsTaskAvailable(pGame, nTaskDID, false))
		{
			return null;
		}
		TaskData taskData = this.GetTaskData(nTaskDID, false);
		Task task = null;
		if (taskData != null)
		{
			task = new Task(pGame, nTaskDID, TFUtils.EpochTime(), this.GetAvailableSimulatedIdentity(pGame, taskData, taskData.m_nTargetDID, true, true));
			this.AddActiveTask(pGame, task, false);
		}
		return task;
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x000882D0 File Offset: 0x000864D0
	public void AddActiveTask(Game pGame, Task pTask, bool bLoading = false)
	{
		TaskData pTaskData = pTask.m_pTaskData;
		int nDID = pTaskData.m_nDID;
		if (!bLoading)
		{
			TaskManager.TaskBlockedStatus taskBlockedStatus = this.GetTaskBlockedStatus(pGame, pTaskData, -1);
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eNoTask) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that doesn't exist");
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eActive) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that is already activated: " + nDID.ToString());
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eLevel) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that does not meet the level requirement for task: " + nDID.ToString() + " level: " + taskBlockedStatus.m_pBlockVars[TaskManager.TaskBlockedStatus._eTaskBlockedType.eLevel].ToString());
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eSource) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has a source in another task: " + nDID.ToString() + " source: " + taskBlockedStatus.m_pBlockVars[TaskManager.TaskBlockedStatus._eTaskBlockedType.eSource].ToString());
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eTarget) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has an unavailable target for task: " + nDID.ToString() + " target: " + taskBlockedStatus.m_pBlockVars[TaskManager.TaskBlockedStatus._eTaskBlockedType.eTarget].ToString());
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.ePartner) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has an unavailable partner for task: " + nDID.ToString() + " partner: " + taskBlockedStatus.m_pBlockVars[TaskManager.TaskBlockedStatus._eTaskBlockedType.ePartner].ToString());
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eMicroEvent) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has a unopen micro event for task: " + nDID.ToString() + " micro event: " + taskBlockedStatus.m_pBlockVars[TaskManager.TaskBlockedStatus._eTaskBlockedType.eMicroEvent].ToString());
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eActiveQuest) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has a inactive quest for task: " + nDID.ToString() + " quest: " + taskBlockedStatus.m_pBlockVars[TaskManager.TaskBlockedStatus._eTaskBlockedType.eActiveQuest].ToString());
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eQuestUnlock) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that has an incomplete quest for task: " + nDID.ToString() + " quest: " + taskBlockedStatus.m_pBlockVars[TaskManager.TaskBlockedStatus._eTaskBlockedType.eQuestUnlock].ToString());
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eRepeatable) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that cannot be repeated again: " + nDID.ToString());
			}
			if ((taskBlockedStatus.m_eTaskBlockedType & TaskManager.TaskBlockedStatus._eTaskBlockedType.eQuestRelock) != TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone)
			{
				TFUtils.Assert(false, "TaskManager | trying to activate a task that is blocked by questrelock: " + nDID.ToString() + " quest: " + taskBlockedStatus.m_pBlockVars[TaskManager.TaskBlockedStatus._eTaskBlockedType.eQuestRelock].ToString());
			}
		}
		if (this.m_pBlueprintTaskMap.ContainsKey(pTaskData.m_nSourceDID))
		{
			return;
		}
		this.m_pActiveTasks.Add(nDID, pTask);
		this.m_pBlueprintTaskMap.Add(pTaskData.m_nSourceDID, nDID);
		if (pTask.m_pTargetIdentity != null)
		{
			string key = pTask.m_pTargetIdentity.Describe();
			if (this.m_pSimulatedTaskMap.ContainsKey(key))
			{
				this.m_pSimulatedTaskMap[key].Add(nDID);
			}
			else
			{
				this.m_pSimulatedTaskMap.Add(pTask.m_pTargetIdentity.Describe(), new List<int>
				{
					nDID
				});
			}
		}
		if (pTaskData.m_nPartnerDID >= 0)
		{
			this.m_pBlueprintTaskMap.Add(pTaskData.m_nPartnerDID, nDID);
		}
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x00088640 File Offset: 0x00086840
	public void RemoveActiveTask(int nDID)
	{
		if (!this.m_pActiveTasks.ContainsKey(nDID))
		{
			return;
		}
		Task task = this.m_pActiveTasks[nDID];
		TaskData pTaskData = task.m_pTaskData;
		this.m_pActiveTasks.Remove(nDID);
		this.m_pBlueprintTaskMap.Remove(pTaskData.m_nSourceDID);
		if (task.m_pTargetIdentity != null)
		{
			this.m_pSimulatedTaskMap[task.m_pTargetIdentity.Describe()].Remove(nDID);
		}
		int nPartnerDID = pTaskData.m_nPartnerDID;
		if (nPartnerDID >= 0)
		{
			this.m_pBlueprintTaskMap.Remove(nPartnerDID);
		}
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x000886D8 File Offset: 0x000868D8
	public Task GetActiveTask(int nTaskDID)
	{
		if (this.IsTaskActive(nTaskDID))
		{
			return this.m_pActiveTasks[nTaskDID];
		}
		return null;
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x000886F4 File Offset: 0x000868F4
	public List<Task> GetActiveTasksForSimulated(int nSimulatedDID, Identity pIdentity, bool bIncludeReadyToCollect = true)
	{
		List<Task> activeTasksForIdentity = this.GetActiveTasksForIdentity(pIdentity, bIncludeReadyToCollect);
		Task activeTaskForDID = this.GetActiveTaskForDID(nSimulatedDID, bIncludeReadyToCollect);
		if (activeTaskForDID != null && !activeTasksForIdentity.Contains(activeTaskForDID))
		{
			activeTasksForIdentity.Add(activeTaskForDID);
		}
		return activeTasksForIdentity;
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x00088730 File Offset: 0x00086930
	public TaskManager._eBlueprintTaskingState GetTaskingStateForSimulated(Simulation pSimulation, int nDID, Identity pIdentity)
	{
		if (pSimulation.FindSimulated(new int?(nDID)) == null)
		{
			return TaskManager._eBlueprintTaskingState.eNotInSim;
		}
		if (!this.m_pBlueprintTaskMap.ContainsKey(nDID))
		{
			return TaskManager._eBlueprintTaskingState.eNone;
		}
		Task task = this.m_pActiveTasks[this.m_pBlueprintTaskMap[nDID]];
		TaskData pTaskData = task.m_pTaskData;
		if (pTaskData.m_nSourceDID == nDID)
		{
			return TaskManager._eBlueprintTaskingState.eSource;
		}
		if (task.m_pTargetIdentity != null && task.m_pTargetIdentity.Equals(pIdentity))
		{
			return TaskManager._eBlueprintTaskingState.eTarget;
		}
		return TaskManager._eBlueprintTaskingState.ePartner;
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x000887B0 File Offset: 0x000869B0
	public string GetActiveDisplayStateForTarget(Identity pIdentity, out Task pTask)
	{
		List<Task> activeTasksForIdentity = this.GetActiveTasksForIdentity(pIdentity, false);
		pTask = null;
		int count = activeTasksForIdentity.Count;
		for (int i = 0; i < count; i++)
		{
			pTask = activeTasksForIdentity[i];
			if (pTask.m_pTaskData.m_eTaskType == TaskData._eTaskType.eActivate)
			{
				return pTask.m_pTaskData.m_sTargetDisplayState;
			}
		}
		pTask = null;
		return null;
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x00088810 File Offset: 0x00086A10
	public int GetTaskCompletionCount(int nDID)
	{
		if (this.m_pTaskCompletionCounts.ContainsKey(nDID))
		{
			return this.m_pTaskCompletionCounts[nDID];
		}
		return 0;
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x00088834 File Offset: 0x00086A34
	public void SetTaskCompletionCount(int nDID, int nCount)
	{
		if (this.m_pTaskCompletionCounts.ContainsKey(nDID))
		{
			this.m_pTaskCompletionCounts[nDID] = nCount;
		}
		else
		{
			this.m_pTaskCompletionCounts.Add(nDID, nCount);
		}
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x00088874 File Offset: 0x00086A74
	public void IncrementTaskCompletionCount(int nDID)
	{
		if (this.m_pTaskCompletionCounts.ContainsKey(nDID))
		{
			this.m_pTaskCompletionCounts[nDID] = this.m_pTaskCompletionCounts[nDID] + 1;
		}
		else
		{
			this.m_pTaskCompletionCounts.Add(nDID, 1);
		}
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x000888C0 File Offset: 0x00086AC0
	private Task GetActiveTaskForDID(int nDID, bool bIncludeReadyToCollect)
	{
		if (this.m_pBlueprintTaskMap.ContainsKey(nDID) && this.m_pActiveTasks.ContainsKey(this.m_pBlueprintTaskMap[nDID]) && (bIncludeReadyToCollect || this.m_pActiveTasks[nDID].GetTimeLeft() > 0UL))
		{
			return this.m_pActiveTasks[this.m_pBlueprintTaskMap[nDID]];
		}
		return null;
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x00088934 File Offset: 0x00086B34
	public void RemoveUnsafeActiveTasks(Game pGame)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, Task> keyValuePair in this.m_pActiveTasks)
		{
			Task value = keyValuePair.Value;
			if (pGame.simulation.FindSimulated(new int?(value.m_pTaskData.m_nSourceDID)) == null)
			{
				list.Add(value.m_pTaskData.m_nDID);
			}
			else if (value.m_pTargetIdentity != null && pGame.simulation.FindSimulated(value.m_pTargetIdentity) == null)
			{
				list.Add(value.m_pTaskData.m_nDID);
			}
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			this.RemoveActiveTask(list[i]);
		}
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x00088A3C File Offset: 0x00086C3C
	private List<Task> GetActiveTasksForIdentity(Identity pIdentity, bool bIncludeReadyToCollect)
	{
		List<Task> list = new List<Task>();
		if (pIdentity != null)
		{
			string key = pIdentity.Describe();
			if (this.m_pSimulatedTaskMap.ContainsKey(key))
			{
				List<int> list2 = this.m_pSimulatedTaskMap[key];
				int count = list2.Count;
				for (int i = 0; i < count; i++)
				{
					if (this.m_pActiveTasks.ContainsKey(list2[i]) && (bIncludeReadyToCollect || this.m_pActiveTasks[list2[i]].GetTimeLeft() > 0UL))
					{
						list.Add(this.m_pActiveTasks[list2[i]]);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x00088AF0 File Offset: 0x00086CF0
	private Identity GetAvailableSimulatedIdentity(Game pGame, TaskData pTaskData, int nSimulatedDID, bool bShuffle = false, bool bRecalculate = true)
	{
		if (!bRecalculate)
		{
			return this.m_pAvailableSimulatedIdentity;
		}
		if (nSimulatedDID < 0)
		{
			this.m_pAvailableSimulatedIdentity = null;
			return this.m_pAvailableSimulatedIdentity;
		}
		List<Simulated> list = pGame.simulation.FindAllSimulateds(nSimulatedDID, null);
		int i = list.Count;
		for (int j = 0; j < i; j++)
		{
			Simulated simulated = list[j];
			Task task;
			if (this.GetTaskingStateForSimulated(pGame.simulation, simulated.entity.DefinitionId, simulated.Id) != TaskManager._eBlueprintTaskingState.eNone)
			{
				list.RemoveAt(j);
				j--;
				i--;
			}
			else if (pGame.inventory.HasItem(simulated.Id))
			{
				list.RemoveAt(j);
				j--;
				i--;
			}
			else if (pTaskData.m_eTaskType == TaskData._eTaskType.eActivate && this.GetActiveDisplayStateForTarget(simulated.Id, out task) != null)
			{
				list.RemoveAt(j);
				j--;
				i--;
			}
			else if (simulated.HasEntity<BuildingEntity>())
			{
				BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
				if (entity.HasDecorator<ActivatableDecorator>())
				{
					ActivatableDecorator decorator = entity.GetDecorator<ActivatableDecorator>();
					if (decorator.Activated == 0UL)
					{
						list.RemoveAt(j);
						j--;
						i--;
					}
				}
			}
		}
		if (i <= 0)
		{
			this.m_pAvailableSimulatedIdentity = null;
			return this.m_pAvailableSimulatedIdentity;
		}
		if (bShuffle)
		{
			while (i > 1)
			{
				i--;
				int index = UnityEngine.Random.Range(0, i + 1);
				Simulated simulated = list[index];
				list[index] = list[i];
				list[i] = simulated;
			}
		}
		this.m_pAvailableSimulatedIdentity = list[0].Id;
		return this.m_pAvailableSimulatedIdentity;
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x00088CBC File Offset: 0x00086EBC
	private void LoadFromSpreadsheet()
	{
		this.m_pTaskDatas = new Dictionary<int, TaskData>();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "Tasks";
		int sheetIndex = instance.GetSheetIndex(sheetName);
		if (sheetIndex < 0)
		{
			return;
		}
		int num = instance.GetNumRows(sheetName);
		if (num <= 0)
		{
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(sheetName, rowName, "id").ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "did");
				if (this.m_pTaskDatas.ContainsKey(intCell))
				{
					TFUtils.ErrorLog("Task Collision! DID: " + intCell.ToString());
				}
				else
				{
					dictionary.Add(TaskData._sDID, intCell);
					dictionary.Add(TaskData._sSOURCE_DID, instance.GetIntCell(sheetIndex, rowIndex, "source unit did"));
					dictionary.Add(TaskData._sPARTNER_DID, -1);
					dictionary.Add(TaskData._sTARGET_DID, instance.GetIntCell(sheetIndex, rowIndex, "target did"));
					dictionary.Add(TaskData._sSOURCE_COSTUME_DID, instance.GetIntCell(sheetIndex, rowIndex, "source unit required costume"));
					dictionary.Add(TaskData._sPARTNER_COSTUME_DID, instance.GetIntCell(sheetIndex, rowIndex, "partner required costume"));
					dictionary.Add(TaskData._sMICRO_EVENT_DID, instance.GetIntCell(sheetIndex, rowIndex, "micro event did"));
					dictionary.Add(TaskData._sACTIVE_QUEST_DID, instance.GetIntCell(sheetIndex, rowIndex, "active during quest did"));
					dictionary.Add(TaskData._sQUEST_UNLOCK_DID, instance.GetIntCell(sheetIndex, rowIndex, "quest unlock did"));
					dictionary.Add(TaskData._sDURATION, instance.GetIntCell(sheetIndex, rowIndex, "duration"));
					dictionary.Add(TaskData._sMIN_LEVEL, instance.GetIntCell(sheetIndex, rowIndex, "min level"));
					dictionary.Add(TaskData._sPOS_OFFSET_TARG_X, instance.GetIntCell(sheetIndex, rowIndex, "position offset from target x"));
					dictionary.Add(TaskData._sPOS_OFFSET_TARG_Y, instance.GetIntCell(sheetIndex, rowIndex, "position offset from target y"));
					dictionary.Add(TaskData._sPARTNER_POS_OFFSET_TARG_X, instance.GetIntCell(sheetIndex, rowIndex, "partner position offset from target x"));
					dictionary.Add(TaskData._sPARTNER_POS_OFFSET_TARG_Y, instance.GetIntCell(sheetIndex, rowIndex, "partner position offset from target y"));
					dictionary.Add(TaskData._sSORT_ORDER, instance.GetIntCell(sheetIndex, rowIndex, "sort order"));
					dictionary.Add(TaskData._sHIDDEN_UNTIL_UNLOCKED, instance.GetIntCell(sheetIndex, rowIndex, "hidden until unlocked") == 1);
					dictionary.Add(TaskData._sSOURCE_FLIPPED, instance.GetIntCell(sheetIndex, rowIndex, "source facing") == 1);
					dictionary.Add(TaskData._sPARTNER_FLIPPED, instance.GetIntCell(sheetIndex, rowIndex, "partner facing") == 1);
					dictionary.Add(TaskData._sEVENT_ONLY, instance.GetIntCell(sheetIndex, rowIndex, "event only") == 1);
					dictionary.Add(TaskData._sREPEATABLE, instance.GetIntCell(sheetIndex, rowIndex, "repeatable") == 1);
					dictionary.Add(TaskData._sQUEST_RELOCK_DID, instance.GetIntCell(sheetIndex, rowIndex, "quest relock did"));
					dictionary.Add(TaskData._sQUEST_REUNLOCK_DID, instance.GetIntCell(sheetIndex, rowIndex, "quest re-unlock did"));
					dictionary.Add(TaskData._sWANDER_TIME, instance.GetFloatCell(sheetIndex, rowIndex, "wander time"));
					dictionary.Add(TaskData._sIDLE_TIME, instance.GetFloatCell(sheetIndex, rowIndex, "idle time"));
					dictionary.Add(TaskData._sMOVEMENT_SPEED, instance.GetFloatCell(sheetIndex, rowIndex, "movement speed"));
					dictionary.Add(TaskData._sNAME, instance.GetStringCell(sheetName, rowName, "name"));
					dictionary.Add(TaskData._sTARGET_TYPE, instance.GetStringCell(sheetName, rowName, "target type"));
					dictionary.Add(TaskData._sSOURCE_DISPLAY_STATE_WALK, instance.GetStringCell(sheetName, rowName, "source unit display state walk"));
					dictionary.Add(TaskData._sPARTNER_DISPLAY_STATE_WALK, instance.GetStringCell(sheetName, rowName, "partner display state walk"));
					dictionary.Add(TaskData._sSOURCE_DISPLAY_STATE_IDLE, instance.GetStringCell(sheetName, rowName, "source unit display state idle"));
					dictionary.Add(TaskData._sPARTNER_DISPLAY_STATE_IDLE, instance.GetStringCell(sheetName, rowName, "partner display state idle"));
					dictionary.Add(TaskData._sTARGET_DISPLAY_STATE, instance.GetStringCell(sheetName, rowName, "target display state"));
					dictionary.Add(TaskData._sSTART_VO, instance.GetStringCell(sheetName, rowName, "start vo"));
					dictionary.Add(TaskData._sFINISH_VO, instance.GetStringCell(sheetName, rowName, "finish vo"));
					dictionary.Add(TaskData._sSTART_SOUND, instance.GetStringCell(sheetName, rowName, "start sound"));
					dictionary.Add(TaskData._sFINISH_SOUND, instance.GetStringCell(sheetName, rowName, "finish sound"));
					dictionary.Add(TaskData._sPAYTABLE_REWARD_ICON, instance.GetStringCell(sheetName, rowName, "paytable reward icon"));
					dictionary.Add(TaskData._sREWARD, new Dictionary<string, object>
					{
						{
							"resources",
							new Dictionary<string, object>()
						},
						{
							"thought_icon",
							instance.GetStringCell(sheetIndex, rowIndex, "reward thought icon")
						}
					});
					int intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "reward gold");
					if (intCell2 > 0)
					{
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary[TaskData._sREWARD])["resources"]).Add("3", intCell2);
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "reward special did");
					if (intCell2 >= 0)
					{
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary[TaskData._sREWARD])["resources"]).Add(intCell2.ToString(), instance.GetIntCell(sheetIndex, rowIndex, "reward special amount"));
					}
					intCell2 = instance.GetIntCell(sheetIndex, rowIndex, "reward xp");
					if (intCell2 > 0)
					{
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary[TaskData._sREWARD])["resources"]).Add("5", intCell2);
					}
					TaskData._eTaskType eTaskType = TaskData._eTaskType.eNumTypes;
					string stringCell = instance.GetStringCell(sheetIndex, rowIndex, "movement type");
					string text = stringCell;
					switch (text)
					{
					case "enter":
						eTaskType = TaskData._eTaskType.eEnter;
						break;
					case "wander":
						eTaskType = TaskData._eTaskType.eWander;
						break;
					case "stand":
						eTaskType = TaskData._eTaskType.eStand;
						break;
					case "activate":
						eTaskType = TaskData._eTaskType.eActivate;
						break;
					}
					if (eTaskType == TaskData._eTaskType.eNumTypes)
					{
						TFUtils.Assert(false, string.Concat(new string[]
						{
							"Unsupported movement type (",
							stringCell,
							") for task did (",
							intCell.ToString(),
							")."
						}));
					}
					else
					{
						dictionary.Add(TaskData._sTASK_TYPE, (int)eTaskType);
						this.m_pTaskDatas.Add(intCell, new TaskData(dictionary, null));
					}
				}
			}
		}
	}

	// Token: 0x04000DD9 RID: 3545
	private Identity m_pAvailableSimulatedIdentity;

	// Token: 0x04000DDA RID: 3546
	private Dictionary<int, TaskData> m_pTaskDatas;

	// Token: 0x04000DDB RID: 3547
	private Dictionary<int, Task> m_pActiveTasks;

	// Token: 0x04000DDC RID: 3548
	private Dictionary<int, int> m_pBlueprintTaskMap;

	// Token: 0x04000DDD RID: 3549
	private Dictionary<string, List<int>> m_pSimulatedTaskMap;

	// Token: 0x04000DDE RID: 3550
	private Dictionary<int, int> m_pTaskCompletionCounts;

	// Token: 0x02000264 RID: 612
	public class TaskBlockedStatus
	{
		// Token: 0x060013EA RID: 5098 RVA: 0x00089440 File Offset: 0x00087640
		public TaskBlockedStatus()
		{
			this.m_eTaskBlockedType = TaskManager.TaskBlockedStatus._eTaskBlockedType.eNone;
			this.m_pBlockVars = new Dictionary<TaskManager.TaskBlockedStatus._eTaskBlockedType, int>();
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0008945C File Offset: 0x0008765C
		public void AddBlock(TaskManager.TaskBlockedStatus._eTaskBlockedType eTaskBlockedType, int nVar)
		{
			this.m_eTaskBlockedType |= eTaskBlockedType;
			if (!this.m_pBlockVars.ContainsKey(eTaskBlockedType))
			{
				this.m_pBlockVars.Add(eTaskBlockedType, nVar);
			}
		}

		// Token: 0x04000DE0 RID: 3552
		public TaskManager.TaskBlockedStatus._eTaskBlockedType m_eTaskBlockedType;

		// Token: 0x04000DE1 RID: 3553
		public Dictionary<TaskManager.TaskBlockedStatus._eTaskBlockedType, int> m_pBlockVars;

		// Token: 0x02000265 RID: 613
		[Flags]
		public enum _eTaskBlockedType : ulong
		{
			// Token: 0x04000DE3 RID: 3555
			eNone = 0UL,
			// Token: 0x04000DE4 RID: 3556
			eNoTask = 1UL,
			// Token: 0x04000DE5 RID: 3557
			eActive = 2UL,
			// Token: 0x04000DE6 RID: 3558
			eSource = 4UL,
			// Token: 0x04000DE7 RID: 3559
			eTarget = 8UL,
			// Token: 0x04000DE8 RID: 3560
			ePartner = 16UL,
			// Token: 0x04000DE9 RID: 3561
			eLevel = 32UL,
			// Token: 0x04000DEA RID: 3562
			eSourceCostume = 64UL,
			// Token: 0x04000DEB RID: 3563
			ePartnerCostume = 128UL,
			// Token: 0x04000DEC RID: 3564
			eMicroEvent = 256UL,
			// Token: 0x04000DED RID: 3565
			eActiveQuest = 512UL,
			// Token: 0x04000DEE RID: 3566
			eRepeatable = 1024UL,
			// Token: 0x04000DEF RID: 3567
			eQuestUnlock = 2048UL,
			// Token: 0x04000DF0 RID: 3568
			eQuestRelock = 5096UL
		}
	}

	// Token: 0x02000266 RID: 614
	public enum _eBlueprintTaskingState
	{
		// Token: 0x04000DF2 RID: 3570
		eNone,
		// Token: 0x04000DF3 RID: 3571
		eSource,
		// Token: 0x04000DF4 RID: 3572
		ePartner,
		// Token: 0x04000DF5 RID: 3573
		eTarget,
		// Token: 0x04000DF6 RID: 3574
		eNotInSim,
		// Token: 0x04000DF7 RID: 3575
		eNumTypes
	}
}
