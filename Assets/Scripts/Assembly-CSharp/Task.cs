using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000260 RID: 608
public class Task
{
	// Token: 0x06001392 RID: 5010 RVA: 0x00086800 File Offset: 0x00084A00
	public Task(Game pGame, Dictionary<string, object> pInvariableData, bool bIgnoreNullTaskData = false)
	{
		if (!pInvariableData.ContainsKey(TaskData._sDID))
		{
			TFUtils.Assert(false, "Task | Invariable Data does not contain key: " + TaskData._sDID);
		}
		if (!pInvariableData.ContainsKey(Task._sSTART_TIME))
		{
			TFUtils.Assert(false, "Task | Invariable Data does not contain key: " + Task._sSTART_TIME);
		}
		int num = TFUtils.LoadInt(pInvariableData, TaskData._sDID);
		this.m_ulStartTime = TFUtils.LoadUlong(pInvariableData, Task._sSTART_TIME, 0UL);
		this.m_ulCompleteTime = TFUtils.LoadUlong(pInvariableData, Task._sCOMPLETE_TIME, 0UL);
		this.m_bMovingToTarget = false;
		this.m_bAtTarget = false;
		this.m_sTargetPrevDisplayState = null;
		this.m_ulMovingTimeStart = 0UL;
		if (pInvariableData.ContainsKey(Task._sTARGET_ID))
		{
			this.m_pTargetIdentity = new Identity(TFUtils.LoadString(pInvariableData, Task._sTARGET_ID));
		}
		this.m_pTaskData = null;
		if (pGame != null)
		{
			this.m_pTaskData = pGame.taskManager.GetTaskData(num, false);
		}
		if (this.m_pTaskData == null)
		{
			if (!bIgnoreNullTaskData)
			{
				TFUtils.Assert(false, "Task | Cannot find task data for did(invariable): " + num);
			}
			else
			{
				this.m_pTaskData = new TaskData(null, pInvariableData);
			}
		}
		else
		{
			this.m_pTaskData = new TaskData(this.m_pTaskData.ToDict(), pInvariableData);
		}
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x00086948 File Offset: 0x00084B48
	public Task(Game pGame, int nDID, ulong ulStartTime, Identity pTargetIdentity)
	{
		this.m_pTaskData = pGame.taskManager.GetTaskData(nDID, false);
		if (this.m_pTaskData == null)
		{
			TFUtils.Assert(false, "Task | Cannot find task data for did: " + nDID);
		}
		this.m_ulStartTime = ulStartTime;
		this.m_ulCompleteTime = ulStartTime + (ulong)((long)this.m_pTaskData.m_nDuration);
		this.m_pTargetIdentity = pTargetIdentity;
		this.m_bMovingToTarget = false;
		this.m_bAtTarget = false;
		this.m_sTargetPrevDisplayState = null;
		this.m_ulMovingTimeStart = 0UL;
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06001395 RID: 5013 RVA: 0x000869F0 File Offset: 0x00084BF0
	// (set) Token: 0x06001396 RID: 5014 RVA: 0x000869F8 File Offset: 0x00084BF8
	public TaskData m_pTaskData { get; private set; }

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06001397 RID: 5015 RVA: 0x00086A04 File Offset: 0x00084C04
	// (set) Token: 0x06001398 RID: 5016 RVA: 0x00086A0C File Offset: 0x00084C0C
	public Identity m_pTargetIdentity { get; private set; }

	// Token: 0x06001399 RID: 5017 RVA: 0x00086A18 File Offset: 0x00084C18
	public void UpdateModifiableData(ulong ulStartTime, ulong ulCompleteTime)
	{
		this.m_ulStartTime = ulStartTime;
		this.m_ulCompleteTime = ulCompleteTime;
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x00086A28 File Offset: 0x00084C28
	public static void UpdateModifiableDataForDict(Dictionary<string, object> pData, Task pTask)
	{
		if (pData.ContainsKey(Task._sSTART_TIME))
		{
			pData[Task._sSTART_TIME] = pTask.m_ulStartTime;
		}
		else
		{
			pData.Add(Task._sSTART_TIME, pTask.m_ulStartTime);
		}
		if (pData.ContainsKey(Task._sCOMPLETE_TIME))
		{
			pData[Task._sCOMPLETE_TIME] = pTask.m_ulCompleteTime;
		}
		else
		{
			pData.Add(Task._sCOMPLETE_TIME, pTask.m_ulCompleteTime);
		}
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x00086AB8 File Offset: 0x00084CB8
	public Dictionary<string, object> GetInvariableData()
	{
		Dictionary<string, object> invariableData = this.m_pTaskData.GetInvariableData();
		invariableData.Add(Task._sSTART_TIME, this.m_ulStartTime);
		invariableData.Add(Task._sCOMPLETE_TIME, this.m_ulCompleteTime);
		if (this.m_pTargetIdentity != null)
		{
			invariableData.Add(Task._sTARGET_ID, this.m_pTargetIdentity.Describe());
		}
		return invariableData;
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x00086B20 File Offset: 0x00084D20
	public ulong GetTimeLeft()
	{
		ulong result = 0UL;
		ulong num = TFUtils.EpochTime();
		ulong num2 = 0UL;
		if (this.m_bMovingToTarget && this.m_ulMovingTimeStart > this.m_ulStartTime)
		{
			num2 = num - this.m_ulMovingTimeStart;
		}
		if (this.m_ulCompleteTime + num2 > num)
		{
			result = this.m_ulCompleteTime + num2 - num;
		}
		return result;
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x00086B78 File Offset: 0x00084D78
	public float GetTimeLeftPercentage()
	{
		ulong timeLeft = this.GetTimeLeft();
		ulong num = 0UL;
		if (this.m_bMovingToTarget && this.m_ulMovingTimeStart > this.m_ulStartTime)
		{
			num = TFUtils.EpochTime() - this.m_ulMovingTimeStart;
		}
		ulong num2 = this.m_ulCompleteTime + num - this.m_ulStartTime;
		return Mathf.Clamp01(1f - (float)(timeLeft / num2));
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x00086BDC File Offset: 0x00084DDC
	public Cost RushCostNow()
	{
		return ResourceManager.CalculateTaskRushCost(this.GetTimeLeft());
	}

	// Token: 0x04000D9B RID: 3483
	public static string _sSTART_TIME = "start_time";

	// Token: 0x04000D9C RID: 3484
	public static string _sCOMPLETE_TIME = "complete_time";

	// Token: 0x04000D9D RID: 3485
	public static string _sTARGET_ID = "target_id";

	// Token: 0x04000D9E RID: 3486
	public ulong m_ulStartTime;

	// Token: 0x04000D9F RID: 3487
	public ulong m_ulCompleteTime;

	// Token: 0x04000DA0 RID: 3488
	public ulong m_ulMovingTimeStart;

	// Token: 0x04000DA1 RID: 3489
	public bool m_bMovingToTarget;

	// Token: 0x04000DA2 RID: 3490
	public bool m_bAtTarget;

	// Token: 0x04000DA3 RID: 3491
	public string m_sTargetPrevDisplayState;
}
