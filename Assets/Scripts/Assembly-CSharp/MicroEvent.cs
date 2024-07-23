using System;
using System.Collections.Generic;

// Token: 0x0200019C RID: 412
public class MicroEvent
{
	// Token: 0x06000DAA RID: 3498 RVA: 0x000535A0 File Offset: 0x000517A0
	public MicroEvent(Game pGame, Dictionary<string, object> pInvariableData, bool bIgnoreNullMicroEventData = false)
	{
		if (!pInvariableData.ContainsKey(MicroEventData._sDID))
		{
			TFUtils.Assert(false, "MicroEvent | Invariable Data does not contain key: " + MicroEventData._sDID);
		}
		int num = TFUtils.LoadInt(pInvariableData, MicroEventData._sDID);
		this.m_ulStartTime = TFUtils.LoadUlong(pInvariableData, MicroEvent._sSTART_TIME, 0UL);
		this.m_ulCompleteTime = TFUtils.TryLoadNullableUlong(pInvariableData, MicroEvent._sCOMPLETE_TIME);
		this.m_bIsClosed = TFUtils.LoadBool(pInvariableData, MicroEvent._sCLOSED);
		this.m_pMicroEventData = null;
		if (pGame != null)
		{
			this.m_pMicroEventData = pGame.microEventManager.GetMicroEventData(num, false);
		}
		if (this.m_pMicroEventData == null)
		{
			if (!bIgnoreNullMicroEventData)
			{
				TFUtils.Assert(false, "MicroEvent | Cannot find micro event data for did(invariable): " + num);
			}
			else
			{
				this.m_pMicroEventData = new MicroEventData(null, pInvariableData);
			}
		}
		else
		{
			this.m_pMicroEventData = new MicroEventData(this.m_pMicroEventData.ToDict(), pInvariableData);
		}
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x00053690 File Offset: 0x00051890
	public MicroEvent(Game pGame, int nDID, ulong ulStartTime)
	{
		this.m_pMicroEventData = pGame.microEventManager.GetMicroEventData(nDID, false);
		if (this.m_pMicroEventData == null)
		{
			TFUtils.Assert(false, "MicroEvent | Cannot find micro event data for did: " + nDID);
		}
		this.m_ulStartTime = ulStartTime;
		this.m_ulCompleteTime = null;
		this.m_bIsClosed = false;
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000DAD RID: 3501 RVA: 0x00053714 File Offset: 0x00051914
	// (set) Token: 0x06000DAE RID: 3502 RVA: 0x0005371C File Offset: 0x0005191C
	public MicroEventData m_pMicroEventData { get; private set; }

	// Token: 0x06000DAF RID: 3503 RVA: 0x00053728 File Offset: 0x00051928
	public bool IsCompleted()
	{
		return this.m_ulCompleteTime != null;
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x00053738 File Offset: 0x00051938
	public bool IsActive()
	{
		return !this.m_pMicroEventData.m_bClosedEvent || !this.m_bIsClosed;
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x00053758 File Offset: 0x00051958
	public Dictionary<string, object> GetInvariableData()
	{
		Dictionary<string, object> invariableData = this.m_pMicroEventData.GetInvariableData();
		invariableData.Add(MicroEvent._sSTART_TIME, this.m_ulStartTime);
		invariableData.Add(MicroEvent._sCLOSED, this.m_bIsClosed);
		if (this.m_ulCompleteTime != null)
		{
			invariableData.Add(MicroEvent._sCOMPLETE_TIME, this.m_ulCompleteTime.Value);
		}
		return invariableData;
	}

	// Token: 0x04000919 RID: 2329
	public static string _sSTART_TIME = "start_time";

	// Token: 0x0400091A RID: 2330
	public static string _sCOMPLETE_TIME = "complete_time";

	// Token: 0x0400091B RID: 2331
	public static string _sCLOSED = "closed";

	// Token: 0x0400091C RID: 2332
	public ulong m_ulStartTime;

	// Token: 0x0400091D RID: 2333
	public ulong? m_ulCompleteTime;

	// Token: 0x0400091E RID: 2334
	public bool m_bIsClosed;
}
