using System;
using System.Collections.Generic;

// Token: 0x0200019D RID: 413
public class MicroEventData
{
	// Token: 0x06000DB2 RID: 3506 RVA: 0x000537CC File Offset: 0x000519CC
	public MicroEventData(Dictionary<string, object> pDatabaseData, Dictionary<string, object> pInvariableData)
	{
		this.m_pData = new ReadOnlyIndexer(new Dictionary<string, object>(MicroEventData._sInvariableKeys.Length + MicroEventData._sVariableKeys.Length)
		{
			{
				MicroEventData._sDID,
				this.GetDictPriorityInt(MicroEventData._sDID, pInvariableData, pDatabaseData)
			},
			{
				MicroEventData._sCLOSE_DIALOG_SEQUENCE_DID,
				this.GetDictPriorityInt(MicroEventData._sCLOSE_DIALOG_SEQUENCE_DID, pInvariableData, pDatabaseData)
			},
			{
				MicroEventData._sSTART_DATE,
				this.GetDictPriorityLong(MicroEventData._sSTART_DATE, pInvariableData, pDatabaseData)
			},
			{
				MicroEventData._sEND_DATE,
				this.GetDictPriorityLong(MicroEventData._sEND_DATE, pInvariableData, pDatabaseData)
			},
			{
				MicroEventData._sCLOSED_EVENT,
				this.GetDictPriorityBool(MicroEventData._sCLOSED_EVENT, pInvariableData, pDatabaseData)
			},
			{
				MicroEventData._sNAME,
				this.GetDictPriorityString(MicroEventData._sNAME, pInvariableData, pDatabaseData)
			}
		});
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0005393C File Offset: 0x00051B3C
	public int m_nDID
	{
		get
		{
			return (int)this.m_pData[MicroEventData._sDID];
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00053954 File Offset: 0x00051B54
	public int m_nCloseDialogSequenceDID
	{
		get
		{
			return (int)this.m_pData[MicroEventData._sCLOSE_DIALOG_SEQUENCE_DID];
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x0005396C File Offset: 0x00051B6C
	public long m_lStartDate
	{
		get
		{
			return (long)this.m_pData[MicroEventData._sSTART_DATE];
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00053984 File Offset: 0x00051B84
	public long m_lEndDate
	{
		get
		{
			return (long)this.m_pData[MicroEventData._sEND_DATE];
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x0005399C File Offset: 0x00051B9C
	public bool m_bClosedEvent
	{
		get
		{
			return (bool)this.m_pData[MicroEventData._sCLOSED_EVENT];
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x000539B4 File Offset: 0x00051BB4
	public string m_sName
	{
		get
		{
			return (string)this.m_pData[MicroEventData._sNAME];
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06000DBA RID: 3514 RVA: 0x000539CC File Offset: 0x00051BCC
	// (set) Token: 0x06000DBB RID: 3515 RVA: 0x000539D4 File Offset: 0x00051BD4
	public ReadOnlyIndexer m_pData { get; private set; }

	// Token: 0x06000DBC RID: 3516 RVA: 0x000539E0 File Offset: 0x00051BE0
	public bool IsActive()
	{
		long adjustedServerTime = SoaringTime.AdjustedServerTime;
		return adjustedServerTime >= this.m_lStartDate && adjustedServerTime <= this.m_lEndDate;
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x00053A10 File Offset: 0x00051C10
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		int num = MicroEventData._sInvariableKeys.Length;
		for (int i = 0; i < num; i++)
		{
			dictionary.Add(MicroEventData._sInvariableKeys[i], this.m_pData[MicroEventData._sInvariableKeys[i]]);
		}
		num = MicroEventData._sVariableKeys.Length;
		for (int j = 0; j < num; j++)
		{
			dictionary.Add(MicroEventData._sVariableKeys[j], this.m_pData[MicroEventData._sVariableKeys[j]]);
		}
		return dictionary;
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x00053A98 File Offset: 0x00051C98
	public Dictionary<string, object> GetInvariableData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		int num = MicroEventData._sInvariableKeys.Length;
		for (int i = 0; i < num; i++)
		{
			dictionary.Add(MicroEventData._sInvariableKeys[i], this.m_pData[MicroEventData._sInvariableKeys[i]]);
		}
		return dictionary;
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x00053AE8 File Offset: 0x00051CE8
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

	// Token: 0x06000DC0 RID: 3520 RVA: 0x00053B2C File Offset: 0x00051D2C
	private long GetDictPriorityLong(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadLong(pDictOne, sKey);
		}
		if (pDictTwo != null && pDictTwo.ContainsKey(sKey))
		{
			return TFUtils.LoadLong(pDictTwo, sKey);
		}
		return 0L;
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x00053B70 File Offset: 0x00051D70
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

	// Token: 0x06000DC2 RID: 3522 RVA: 0x00053BB8 File Offset: 0x00051DB8
	private bool GetDictPriorityBool(string sKey, Dictionary<string, object> pDictOne, Dictionary<string, object> pDictTwo)
	{
		if (pDictOne != null && pDictOne.ContainsKey(sKey))
		{
			return TFUtils.LoadBool(pDictOne, sKey);
		}
		return pDictTwo != null && pDictTwo.ContainsKey(sKey) && TFUtils.LoadBool(pDictTwo, sKey);
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x00053BFC File Offset: 0x00051DFC
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

	// Token: 0x06000DC4 RID: 3524 RVA: 0x00053C44 File Offset: 0x00051E44
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

	// Token: 0x06000DC5 RID: 3525 RVA: 0x00053C8C File Offset: 0x00051E8C
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

	// Token: 0x04000920 RID: 2336
	public static string _sDID = "did";

	// Token: 0x04000921 RID: 2337
	public static string _sNAME = "name";

	// Token: 0x04000922 RID: 2338
	public static string _sCLOSED_EVENT = "closed_event";

	// Token: 0x04000923 RID: 2339
	public static string _sSTART_DATE = "start_date";

	// Token: 0x04000924 RID: 2340
	public static string _sEND_DATE = "end_date";

	// Token: 0x04000925 RID: 2341
	public static string _sCLOSE_DIALOG_SEQUENCE_DID = "close_dialog_sequence_did";

	// Token: 0x04000926 RID: 2342
	private static string[] _sInvariableKeys = new string[]
	{
		MicroEventData._sDID
	};

	// Token: 0x04000927 RID: 2343
	private static string[] _sVariableKeys = new string[]
	{
		MicroEventData._sNAME,
		MicroEventData._sCLOSED_EVENT,
		MicroEventData._sSTART_DATE,
		MicroEventData._sEND_DATE,
		MicroEventData._sCLOSE_DIALOG_SEQUENCE_DID
	};
}
