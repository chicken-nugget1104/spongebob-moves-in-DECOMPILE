using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class MicroEventManager
{
	// Token: 0x06000DC6 RID: 3526 RVA: 0x00053CD4 File Offset: 0x00051ED4
	public MicroEventManager()
	{
		this.m_pMicroEvents = new Dictionary<int, MicroEvent>();
		this.LoadFromSpreadsheet();
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00053D04 File Offset: 0x00051F04
	public void AddMicroEvent(Game pGame, MicroEvent pMicroEvent, bool bLoading = false)
	{
		if (pMicroEvent != null)
		{
			this.m_pMicroEvents.Add(pMicroEvent.m_pMicroEventData.m_nDID, pMicroEvent);
		}
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00053D24 File Offset: 0x00051F24
	public MicroEventData GetMicroEventData(int nDID, bool bDefaultActiveMicroEventData = false)
	{
		if (bDefaultActiveMicroEventData && this.m_pMicroEvents.ContainsKey(nDID))
		{
			return this.m_pMicroEvents[nDID].m_pMicroEventData;
		}
		if (this.m_pMicroEventDatas.ContainsKey(nDID))
		{
			return this.m_pMicroEventDatas[nDID];
		}
		return null;
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x00053D7C File Offset: 0x00051F7C
	public MicroEvent GetMicroEvent(int nDID)
	{
		if (this.m_pMicroEvents.ContainsKey(nDID))
		{
			return this.m_pMicroEvents[nDID];
		}
		return null;
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00053DA0 File Offset: 0x00051FA0
	public bool IsMicroEventActive(int nDID)
	{
		if (this.m_pMicroEvents.ContainsKey(nDID))
		{
			return this.IsMicroEventActive(this.m_pMicroEvents[nDID]);
		}
		return this.m_pMicroEventDatas.ContainsKey(nDID) && this.IsMicroEventActive(this.m_pMicroEventDatas[nDID]);
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x00053DF8 File Offset: 0x00051FF8
	public bool IsMicroEventActive(MicroEvent pMicroEvent)
	{
		return pMicroEvent != null && pMicroEvent.IsActive();
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x00053E08 File Offset: 0x00052008
	public bool IsMicroEventActive(MicroEventData pMicroEventData)
	{
		return pMicroEventData != null && pMicroEventData.IsActive();
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x00053E18 File Offset: 0x00052018
	public void OnUpdate(Session pSession)
	{
		this.m_fUpdateTimer += Time.deltaTime;
		if (this.m_fPreviousClosedEventUpdateTime != 0f && Time.time - this.m_fPreviousClosedEventUpdateTime <= this.m_nClosedEventMinWaitTime)
		{
			return;
		}
		this.UpdateClosedTypeEvents(pSession.TheGame);
		this.m_fPreviousClosedEventUpdateTime = Time.time;
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x00053E78 File Offset: 0x00052078
	private void UpdateClosedTypeEvents(Game pGame)
	{
		foreach (KeyValuePair<int, MicroEvent> keyValuePair in this.m_pMicroEvents)
		{
			MicroEvent value = keyValuePair.Value;
			if (value.m_pMicroEventData.m_bClosedEvent)
			{
				if (value.m_bIsClosed && value.m_pMicroEventData.IsActive())
				{
					value.m_bIsClosed = false;
					pGame.simulation.ModifyGameState(new MicroEventOpenAction(value));
					pGame.questManager.HandleMicroEventClosedStatusChange(pGame, value);
				}
				else if (!value.m_bIsClosed && !value.m_pMicroEventData.IsActive())
				{
					value.m_bIsClosed = true;
					pGame.simulation.ModifyGameState(new MicroEventCloseAction(value));
					pGame.questManager.HandleMicroEventClosedStatusChange(pGame, value);
				}
			}
		}
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x00053F80 File Offset: 0x00052180
	private void LoadFromSpreadsheet()
	{
		this.m_pMicroEventDatas = new Dictionary<int, MicroEventData>();
		DatabaseManager instance = DatabaseManager.Instance;
		string sheetName = "MicroEvents";
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
				if (this.m_pMicroEventDatas.ContainsKey(intCell))
				{
					TFUtils.ErrorLog("Micro Event Collision! DID: " + intCell.ToString());
				}
				else
				{
					dictionary.Add(MicroEventData._sDID, intCell);
					dictionary.Add(MicroEventData._sCLOSE_DIALOG_SEQUENCE_DID, instance.GetIntCell(sheetIndex, rowIndex, "closing dialog sequence"));
					dictionary.Add(MicroEventData._sCLOSED_EVENT, instance.GetIntCell(sheetIndex, rowIndex, "closed event") == 1);
					dictionary.Add(MicroEventData._sNAME, instance.GetStringCell(sheetIndex, rowIndex, "name"));
					DateTime value;
					if (DateTime.TryParse(instance.GetStringCell(sheetIndex, rowIndex, "start date"), out value))
					{
						dictionary.Add(MicroEventData._sSTART_DATE, (DateTime.SpecifyKind(value, DateTimeKind.Utc) - SoaringTime.Epoch).TotalSeconds);
					}
					else
					{
						dictionary.Add(MicroEventData._sSTART_DATE, 0);
						TFUtils.ErrorLog("MicroEventManager | cannot parse start date for micro event DID: " + intCell.ToString());
					}
					if (DateTime.TryParse(instance.GetStringCell(sheetIndex, rowIndex, "end date"), out value))
					{
						dictionary.Add(MicroEventData._sEND_DATE, (DateTime.SpecifyKind(value, DateTimeKind.Utc) - SoaringTime.Epoch).TotalSeconds);
					}
					else
					{
						dictionary.Add(MicroEventData._sEND_DATE, 0);
						TFUtils.ErrorLog("MicroEventManager | cannot parse end date for micro event DID: " + intCell.ToString());
					}
					this.m_pMicroEventDatas.Add(intCell, new MicroEventData(dictionary, null));
				}
			}
		}
	}

	// Token: 0x04000929 RID: 2345
	private float m_fUpdateTimer;

	// Token: 0x0400092A RID: 2346
	private float m_fPreviousClosedEventUpdateTime;

	// Token: 0x0400092B RID: 2347
	private float m_nClosedEventMinWaitTime = 300f;

	// Token: 0x0400092C RID: 2348
	private Dictionary<int, MicroEventData> m_pMicroEventDatas;

	// Token: 0x0400092D RID: 2349
	private Dictionary<int, MicroEvent> m_pMicroEvents;
}
