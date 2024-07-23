using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000261 RID: 609
public class TaskData : IComparable<TaskData>
{
	// Token: 0x0600139F RID: 5023 RVA: 0x00086BEC File Offset: 0x00084DEC
	public TaskData(Dictionary<string, object> pDatabaseData, Dictionary<string, object> pInvariableData)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>(TaskData._sInvariableKeys.Length + TaskData._sVariableKeys.Length);
		dictionary.Add(TaskData._sDID, this.GetDictPriorityInt(TaskData._sDID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sSOURCE_DID, this.GetDictPriorityInt(TaskData._sSOURCE_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPARTNER_DID, this.GetDictPriorityInt(TaskData._sPARTNER_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sMICRO_EVENT_DID, this.GetDictPriorityInt(TaskData._sMICRO_EVENT_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sACTIVE_QUEST_DID, this.GetDictPriorityInt(TaskData._sACTIVE_QUEST_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sQUEST_UNLOCK_DID, this.GetDictPriorityInt(TaskData._sQUEST_UNLOCK_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sTARGET_DID, this.GetDictPriorityInt(TaskData._sTARGET_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sSOURCE_COSTUME_DID, this.GetDictPriorityInt(TaskData._sSOURCE_COSTUME_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPARTNER_COSTUME_DID, this.GetDictPriorityInt(TaskData._sPARTNER_COSTUME_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sDURATION, this.GetDictPriorityInt(TaskData._sDURATION, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sMIN_LEVEL, this.GetDictPriorityInt(TaskData._sMIN_LEVEL, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPOS_OFFSET_TARG_X, this.GetDictPriorityInt(TaskData._sPOS_OFFSET_TARG_X, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPOS_OFFSET_TARG_Y, this.GetDictPriorityInt(TaskData._sPOS_OFFSET_TARG_Y, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPARTNER_POS_OFFSET_TARG_X, this.GetDictPriorityInt(TaskData._sPARTNER_POS_OFFSET_TARG_X, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPARTNER_POS_OFFSET_TARG_Y, this.GetDictPriorityInt(TaskData._sPARTNER_POS_OFFSET_TARG_Y, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sTASK_TYPE, this.GetDictPriorityInt(TaskData._sTASK_TYPE, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sSORT_ORDER, this.GetDictPriorityInt(TaskData._sSORT_ORDER, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sQUEST_RELOCK_DID, this.GetDictPriorityInt(TaskData._sQUEST_RELOCK_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sQUEST_REUNLOCK_DID, this.GetDictPriorityInt(TaskData._sQUEST_REUNLOCK_DID, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sMOVEMENT_SPEED, this.GetDictPriorityFloat(TaskData._sMOVEMENT_SPEED, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sWANDER_TIME, this.GetDictPriorityFloat(TaskData._sWANDER_TIME, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sIDLE_TIME, this.GetDictPriorityFloat(TaskData._sIDLE_TIME, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sHIDDEN_UNTIL_UNLOCKED, this.GetDictPriorityBool(TaskData._sHIDDEN_UNTIL_UNLOCKED, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sSOURCE_FLIPPED, this.GetDictPriorityBool(TaskData._sSOURCE_FLIPPED, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPARTNER_FLIPPED, this.GetDictPriorityBool(TaskData._sPARTNER_FLIPPED, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sEVENT_ONLY, this.GetDictPriorityBool(TaskData._sEVENT_ONLY, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sREPEATABLE, this.GetDictPriorityBool(TaskData._sREPEATABLE, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sNAME, this.GetDictPriorityString(TaskData._sNAME, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sTARGET_TYPE, this.GetDictPriorityString(TaskData._sTARGET_TYPE, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sSOURCE_DISPLAY_STATE_WALK, this.GetDictPriorityString(TaskData._sSOURCE_DISPLAY_STATE_WALK, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPARTNER_DISPLAY_STATE_WALK, this.GetDictPriorityString(TaskData._sPARTNER_DISPLAY_STATE_WALK, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sSOURCE_DISPLAY_STATE_IDLE, this.GetDictPriorityString(TaskData._sSOURCE_DISPLAY_STATE_IDLE, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPARTNER_DISPLAY_STATE_IDLE, this.GetDictPriorityString(TaskData._sPARTNER_DISPLAY_STATE_IDLE, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sTARGET_DISPLAY_STATE, this.GetDictPriorityString(TaskData._sTARGET_DISPLAY_STATE, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sSTART_VO, this.GetDictPriorityString(TaskData._sSTART_VO, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sFINISH_VO, this.GetDictPriorityString(TaskData._sFINISH_VO, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sSTART_SOUND, this.GetDictPriorityString(TaskData._sSTART_SOUND, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sFINISH_SOUND, this.GetDictPriorityString(TaskData._sFINISH_SOUND, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sREWARD, this.GetDictPriorityDict(TaskData._sREWARD, pInvariableData, pDatabaseData));
		dictionary.Add(TaskData._sPAYTABLE_REWARD_ICON, this.GetDictPriorityString(TaskData._sPAYTABLE_REWARD_ICON, pInvariableData, pDatabaseData));
		this.m_pRewardData = Reward.FromDict((Dictionary<string, object>)dictionary[TaskData._sREWARD]);
		this.m_pData = new ReadOnlyIndexer(dictionary);
		if (this.m_sPaytableRewardIcon != "n/a")
		{
			this.tasksHasBonus.Add(this.m_nDID);
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x060013A1 RID: 5025 RVA: 0x000873C0 File Offset: 0x000855C0
	public int m_nDID
	{
		get
		{
			return (int)this.m_pData[TaskData._sDID];
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x060013A2 RID: 5026 RVA: 0x000873D8 File Offset: 0x000855D8
	public int m_nSourceDID
	{
		get
		{
			return (int)this.m_pData[TaskData._sSOURCE_DID];
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x060013A3 RID: 5027 RVA: 0x000873F0 File Offset: 0x000855F0
	public int m_nPartnerDID
	{
		get
		{
			return (int)this.m_pData[TaskData._sPARTNER_DID];
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x060013A4 RID: 5028 RVA: 0x00087408 File Offset: 0x00085608
	public int m_nTargetDID
	{
		get
		{
			return (int)this.m_pData[TaskData._sTARGET_DID];
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x060013A5 RID: 5029 RVA: 0x00087420 File Offset: 0x00085620
	public int m_nSourceCostumeDID
	{
		get
		{
			return (int)this.m_pData[TaskData._sSOURCE_COSTUME_DID];
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x060013A6 RID: 5030 RVA: 0x00087438 File Offset: 0x00085638
	public int m_nPartnerCostumeDID
	{
		get
		{
			return (int)this.m_pData[TaskData._sPARTNER_COSTUME_DID];
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x060013A7 RID: 5031 RVA: 0x00087450 File Offset: 0x00085650
	public int m_nMicroEventDID
	{
		get
		{
			return (int)this.m_pData[TaskData._sMICRO_EVENT_DID];
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x060013A8 RID: 5032 RVA: 0x00087468 File Offset: 0x00085668
	public int m_nActiveQuestDID
	{
		get
		{
			return (int)this.m_pData[TaskData._sACTIVE_QUEST_DID];
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x060013A9 RID: 5033 RVA: 0x00087480 File Offset: 0x00085680
	public int m_nQuestUnlockDID
	{
		get
		{
			return (int)this.m_pData[TaskData._sQUEST_UNLOCK_DID];
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x060013AA RID: 5034 RVA: 0x00087498 File Offset: 0x00085698
	public int m_nDuration
	{
		get
		{
			return (int)this.m_pData[TaskData._sDURATION];
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x060013AB RID: 5035 RVA: 0x000874B0 File Offset: 0x000856B0
	public int m_nMinLevel
	{
		get
		{
			return (int)this.m_pData[TaskData._sMIN_LEVEL];
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x060013AC RID: 5036 RVA: 0x000874C8 File Offset: 0x000856C8
	public int m_nSortOrder
	{
		get
		{
			return (int)this.m_pData[TaskData._sSORT_ORDER];
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x060013AD RID: 5037 RVA: 0x000874E0 File Offset: 0x000856E0
	public int m_nQuestRelockDid
	{
		get
		{
			return (int)this.m_pData[TaskData._sQUEST_RELOCK_DID];
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x060013AE RID: 5038 RVA: 0x000874F8 File Offset: 0x000856F8
	public int m_nQuestReunlockDid
	{
		get
		{
			return (int)this.m_pData[TaskData._sQUEST_REUNLOCK_DID];
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x060013AF RID: 5039 RVA: 0x00087510 File Offset: 0x00085710
	public float m_fMovementSpeed
	{
		get
		{
			return (float)this.m_pData[TaskData._sMOVEMENT_SPEED];
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00087528 File Offset: 0x00085728
	public float m_fWanderTime
	{
		get
		{
			return (float)this.m_pData[TaskData._sWANDER_TIME];
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00087540 File Offset: 0x00085740
	public float m_fIdleTime
	{
		get
		{
			return (float)this.m_pData[TaskData._sIDLE_TIME];
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x060013B2 RID: 5042 RVA: 0x00087558 File Offset: 0x00085758
	public bool m_bHiddenUntilUnlocked
	{
		get
		{
			return (bool)this.m_pData[TaskData._sHIDDEN_UNTIL_UNLOCKED];
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x060013B3 RID: 5043 RVA: 0x00087570 File Offset: 0x00085770
	public bool m_bSourceFlipped
	{
		get
		{
			return (bool)this.m_pData[TaskData._sSOURCE_FLIPPED];
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x060013B4 RID: 5044 RVA: 0x00087588 File Offset: 0x00085788
	public bool m_bPartnerFlipped
	{
		get
		{
			return (bool)this.m_pData[TaskData._sPARTNER_FLIPPED];
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x060013B5 RID: 5045 RVA: 0x000875A0 File Offset: 0x000857A0
	public bool m_bEventOnly
	{
		get
		{
			return (bool)this.m_pData[TaskData._sEVENT_ONLY];
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x060013B6 RID: 5046 RVA: 0x000875B8 File Offset: 0x000857B8
	public bool m_bRepeatable
	{
		get
		{
			return (bool)this.m_pData[TaskData._sREPEATABLE];
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x060013B7 RID: 5047 RVA: 0x000875D0 File Offset: 0x000857D0
	public string m_sName
	{
		get
		{
			return (string)this.m_pData[TaskData._sNAME];
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x060013B8 RID: 5048 RVA: 0x000875E8 File Offset: 0x000857E8
	public string m_sTargetType
	{
		get
		{
			return (string)this.m_pData[TaskData._sTARGET_TYPE];
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x060013B9 RID: 5049 RVA: 0x00087600 File Offset: 0x00085800
	public string m_sSourceDisplayStateWalk
	{
		get
		{
			return (string)this.m_pData[TaskData._sSOURCE_DISPLAY_STATE_WALK];
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x060013BA RID: 5050 RVA: 0x00087618 File Offset: 0x00085818
	public string m_sPartnerDisplayStateWalk
	{
		get
		{
			return (string)this.m_pData[TaskData._sPARTNER_DISPLAY_STATE_WALK];
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x060013BB RID: 5051 RVA: 0x00087630 File Offset: 0x00085830
	public string m_sSourceDisplayStateIdle
	{
		get
		{
			return (string)this.m_pData[TaskData._sSOURCE_DISPLAY_STATE_IDLE];
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x060013BC RID: 5052 RVA: 0x00087648 File Offset: 0x00085848
	public string m_sPartnerDisplayStateIdle
	{
		get
		{
			return (string)this.m_pData[TaskData._sPARTNER_DISPLAY_STATE_IDLE];
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x060013BD RID: 5053 RVA: 0x00087660 File Offset: 0x00085860
	public string m_sTargetDisplayState
	{
		get
		{
			return (string)this.m_pData[TaskData._sTARGET_DISPLAY_STATE];
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x060013BE RID: 5054 RVA: 0x00087678 File Offset: 0x00085878
	public string m_sStartVO
	{
		get
		{
			return (string)this.m_pData[TaskData._sSTART_VO];
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x060013BF RID: 5055 RVA: 0x00087690 File Offset: 0x00085890
	public string m_sFinishVO
	{
		get
		{
			return (string)this.m_pData[TaskData._sFINISH_VO];
		}
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x060013C0 RID: 5056 RVA: 0x000876A8 File Offset: 0x000858A8
	public string m_sStartSound
	{
		get
		{
			return (string)this.m_pData[TaskData._sSTART_SOUND];
		}
	}

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x060013C1 RID: 5057 RVA: 0x000876C0 File Offset: 0x000858C0
	public string m_sFinishSound
	{
		get
		{
			return (string)this.m_pData[TaskData._sFINISH_SOUND];
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x060013C2 RID: 5058 RVA: 0x000876D8 File Offset: 0x000858D8
	public string m_sPaytableRewardIcon
	{
		get
		{
			return (string)this.m_pData[TaskData._sPAYTABLE_REWARD_ICON];
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x060013C3 RID: 5059 RVA: 0x000876F0 File Offset: 0x000858F0
	public TaskData._eTaskType m_eTaskType
	{
		get
		{
			return (TaskData._eTaskType)((int)this.m_pData[TaskData._sTASK_TYPE]);
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00087708 File Offset: 0x00085908
	public Vector2 m_pPosOffsetFromTarget
	{
		get
		{
			return new Vector2((float)((int)this.m_pData[TaskData._sPOS_OFFSET_TARG_X]), (float)((int)this.m_pData[TaskData._sPOS_OFFSET_TARG_Y]));
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x060013C5 RID: 5061 RVA: 0x00087748 File Offset: 0x00085948
	public Vector2 m_pPartnerPosOffsetFromTarget
	{
		get
		{
			return new Vector2((float)((int)this.m_pData[TaskData._sPARTNER_POS_OFFSET_TARG_X]), (float)((int)this.m_pData[TaskData._sPARTNER_POS_OFFSET_TARG_Y]));
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x060013C6 RID: 5062 RVA: 0x00087788 File Offset: 0x00085988
	public Reward m_pReward
	{
		get
		{
			return this.m_pRewardData;
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x060013C7 RID: 5063 RVA: 0x00087790 File Offset: 0x00085990
	// (set) Token: 0x060013C8 RID: 5064 RVA: 0x00087798 File Offset: 0x00085998
	public ReadOnlyIndexer m_pData { get; private set; }

	// Token: 0x060013C9 RID: 5065 RVA: 0x000877A4 File Offset: 0x000859A4
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		int num = TaskData._sInvariableKeys.Length;
		for (int i = 0; i < num; i++)
		{
			dictionary.Add(TaskData._sInvariableKeys[i], this.m_pData[TaskData._sInvariableKeys[i]]);
		}
		num = TaskData._sVariableKeys.Length;
		for (int j = 0; j < num; j++)
		{
			dictionary.Add(TaskData._sVariableKeys[j], this.m_pData[TaskData._sVariableKeys[j]]);
		}
		return dictionary;
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x0008782C File Offset: 0x00085A2C
	public Dictionary<string, object> GetInvariableData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		int num = TaskData._sInvariableKeys.Length;
		for (int i = 0; i < num; i++)
		{
			dictionary.Add(TaskData._sInvariableKeys[i], this.m_pData[TaskData._sInvariableKeys[i]]);
		}
		return dictionary;
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x0008787C File Offset: 0x00085A7C
	public int CompareTo(TaskData pTaskData)
	{
		if (pTaskData == null)
		{
			return 1;
		}
		return this.m_nSortOrder.CompareTo(pTaskData.m_nSortOrder);
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x000878A8 File Offset: 0x00085AA8
	private int GetDictPriorityInt(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadInt(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadInt(pDictTwo, sKey);
		}
		return 0;
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x000878EC File Offset: 0x00085AEC
	private float GetDictPriorityFloat(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadFloat(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadFloat(pDictTwo, sKey);
		}
		return 0f;
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x00087934 File Offset: 0x00085B34
	private bool GetDictPriorityBool(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadBool(pDictOne, sKey);
		}
		return pDictTwo != null && pDictTwo.ContainsKey(sKey) && TFUtils.LoadBool(pDictTwo, sKey);
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x00087978 File Offset: 0x00085B78
	private string GetDictPriorityString(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadString(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadString(pDictTwo, sKey);
		}
		return string.Empty;
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x000879C0 File Offset: 0x00085BC0
	private List<T> GetDictPriorityList<T>(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadList<T>(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadList<T>(pDictTwo, sKey);
		}
		return new List<T>();
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00087A08 File Offset: 0x00085C08
	private Dictionary<string, object> GetDictPriorityDict(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadDict(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadDict(pDictTwo, sKey);
		}
		return new Dictionary<string, object>();
	}

	// Token: 0x04000DA6 RID: 3494
	public static string _sDID = "did";

	// Token: 0x04000DA7 RID: 3495
	public static string _sSOURCE_DID = "source_did";

	// Token: 0x04000DA8 RID: 3496
	public static string _sTARGET_DID = "target_did";

	// Token: 0x04000DA9 RID: 3497
	public static string _sPARTNER_DID = "partner_did";

	// Token: 0x04000DAA RID: 3498
	public static string _sMICRO_EVENT_DID = "micro_event_did";

	// Token: 0x04000DAB RID: 3499
	public static string _sACTIVE_QUEST_DID = "active_quest_did";

	// Token: 0x04000DAC RID: 3500
	public static string _sQUEST_UNLOCK_DID = "quest_unlock_did";

	// Token: 0x04000DAD RID: 3501
	public static string _sMIN_LEVEL = "min_level";

	// Token: 0x04000DAE RID: 3502
	public static string _sTARGET_TYPE = "target_type";

	// Token: 0x04000DAF RID: 3503
	public static string _sSOURCE_COSTUME_DID = "source_unit_required_costume";

	// Token: 0x04000DB0 RID: 3504
	public static string _sPARTNER_COSTUME_DID = "partner_unit_required_costume";

	// Token: 0x04000DB1 RID: 3505
	public static string _sTASK_TYPE = "task_type";

	// Token: 0x04000DB2 RID: 3506
	public static string _sQUEST_RELOCK_DID = "quest_relock_did";

	// Token: 0x04000DB3 RID: 3507
	public static string _sQUEST_REUNLOCK_DID = "quest_re_unlock_did";

	// Token: 0x04000DB4 RID: 3508
	public static string _sDURATION = "duration";

	// Token: 0x04000DB5 RID: 3509
	public static string _sNAME = "name";

	// Token: 0x04000DB6 RID: 3510
	public static string _sREWARD = "reward";

	// Token: 0x04000DB7 RID: 3511
	public static string _sPOS_OFFSET_TARG_X = "position_offset_from_target_x";

	// Token: 0x04000DB8 RID: 3512
	public static string _sPOS_OFFSET_TARG_Y = "position_offset_from_target_y";

	// Token: 0x04000DB9 RID: 3513
	public static string _sPARTNER_POS_OFFSET_TARG_X = "partner_position_offset_from_target_x";

	// Token: 0x04000DBA RID: 3514
	public static string _sPARTNER_POS_OFFSET_TARG_Y = "partner_position_offset_from_target_y";

	// Token: 0x04000DBB RID: 3515
	public static string _sMOVEMENT_SPEED = "movement_speed";

	// Token: 0x04000DBC RID: 3516
	public static string _sHIDDEN_UNTIL_UNLOCKED = "hidden_until_unlocked";

	// Token: 0x04000DBD RID: 3517
	public static string _sWANDER_TIME = "wander_time";

	// Token: 0x04000DBE RID: 3518
	public static string _sIDLE_TIME = "idle_time";

	// Token: 0x04000DBF RID: 3519
	public static string _sSOURCE_DISPLAY_STATE_WALK = "source_display_state_walk";

	// Token: 0x04000DC0 RID: 3520
	public static string _sPARTNER_DISPLAY_STATE_WALK = "partner_display_state_walk";

	// Token: 0x04000DC1 RID: 3521
	public static string _sSOURCE_DISPLAY_STATE_IDLE = "source_display_state_idle";

	// Token: 0x04000DC2 RID: 3522
	public static string _sPARTNER_DISPLAY_STATE_IDLE = "partner_display_state_idle";

	// Token: 0x04000DC3 RID: 3523
	public static string _sTARGET_DISPLAY_STATE = "target_display_state";

	// Token: 0x04000DC4 RID: 3524
	public static string _sSTART_VO = "start_vo";

	// Token: 0x04000DC5 RID: 3525
	public static string _sFINISH_VO = "finish_vo";

	// Token: 0x04000DC6 RID: 3526
	public static string _sSTART_SOUND = "start_sound";

	// Token: 0x04000DC7 RID: 3527
	public static string _sFINISH_SOUND = "finish_sound";

	// Token: 0x04000DC8 RID: 3528
	public static string _sSOURCE_FLIPPED = "source_flipped";

	// Token: 0x04000DC9 RID: 3529
	public static string _sPARTNER_FLIPPED = "partner_flipped";

	// Token: 0x04000DCA RID: 3530
	public static string _sEVENT_ONLY = "event_only";

	// Token: 0x04000DCB RID: 3531
	public static string _sSORT_ORDER = "sort_order";

	// Token: 0x04000DCC RID: 3532
	public static string _sREPEATABLE = "repeatable";

	// Token: 0x04000DCD RID: 3533
	public static string _sPAYTABLE_REWARD_ICON = "paytable_reward_icon";

	// Token: 0x04000DCE RID: 3534
	private static string[] _sInvariableKeys = new string[]
	{
		TaskData._sDID,
		TaskData._sSOURCE_DID,
		TaskData._sTARGET_DID,
		TaskData._sPARTNER_DID,
		TaskData._sMICRO_EVENT_DID,
		TaskData._sACTIVE_QUEST_DID,
		TaskData._sQUEST_UNLOCK_DID,
		TaskData._sMIN_LEVEL,
		TaskData._sTARGET_TYPE,
		TaskData._sSOURCE_COSTUME_DID,
		TaskData._sPARTNER_COSTUME_DID,
		TaskData._sTASK_TYPE,
		TaskData._sQUEST_RELOCK_DID,
		TaskData._sQUEST_REUNLOCK_DID
	};

	// Token: 0x04000DCF RID: 3535
	private static string[] _sVariableKeys = new string[]
	{
		TaskData._sDURATION,
		TaskData._sNAME,
		TaskData._sREWARD,
		TaskData._sPOS_OFFSET_TARG_X,
		TaskData._sPOS_OFFSET_TARG_Y,
		TaskData._sPARTNER_POS_OFFSET_TARG_X,
		TaskData._sPARTNER_POS_OFFSET_TARG_Y,
		TaskData._sMOVEMENT_SPEED,
		TaskData._sHIDDEN_UNTIL_UNLOCKED,
		TaskData._sWANDER_TIME,
		TaskData._sIDLE_TIME,
		TaskData._sSOURCE_DISPLAY_STATE_WALK,
		TaskData._sPARTNER_DISPLAY_STATE_WALK,
		TaskData._sSOURCE_DISPLAY_STATE_IDLE,
		TaskData._sPARTNER_DISPLAY_STATE_IDLE,
		TaskData._sTARGET_DISPLAY_STATE,
		TaskData._sSTART_VO,
		TaskData._sFINISH_VO,
		TaskData._sSTART_SOUND,
		TaskData._sFINISH_SOUND,
		TaskData._sSOURCE_FLIPPED,
		TaskData._sPARTNER_FLIPPED,
		TaskData._sEVENT_ONLY,
		TaskData._sSORT_ORDER,
		TaskData._sREPEATABLE,
		TaskData._sPAYTABLE_REWARD_ICON
	};

	// Token: 0x04000DD0 RID: 3536
	private Reward m_pRewardData;

	// Token: 0x04000DD1 RID: 3537
	public List<int> tasksHasBonus = new List<int>();

	// Token: 0x02000262 RID: 610
	public enum _eTaskType
	{
		// Token: 0x04000DD4 RID: 3540
		eWander,
		// Token: 0x04000DD5 RID: 3541
		eEnter,
		// Token: 0x04000DD6 RID: 3542
		eStand,
		// Token: 0x04000DD7 RID: 3543
		eActivate,
		// Token: 0x04000DD8 RID: 3544
		eNumTypes
	}
}
