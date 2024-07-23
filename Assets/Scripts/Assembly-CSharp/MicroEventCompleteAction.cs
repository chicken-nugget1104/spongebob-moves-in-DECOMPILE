using System;
using System.Collections.Generic;

// Token: 0x020000DE RID: 222
public class MicroEventCompleteAction : PersistedTriggerableAction
{
	// Token: 0x06000836 RID: 2102 RVA: 0x00034D94 File Offset: 0x00032F94
	public MicroEventCompleteAction(MicroEvent pMicroEvent) : base("mca", Identity.Null())
	{
		this.m_pMicroEvent = pMicroEvent;
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000837 RID: 2103 RVA: 0x00034DB0 File Offset: 0x00032FB0
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00034DB4 File Offset: 0x00032FB4
	public new static MicroEventCompleteAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "micro_event_invariable");
		MicroEvent pMicroEvent = new MicroEvent(null, pInvariableData, true);
		return new MicroEventCompleteAction(pMicroEvent);
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00034DDC File Offset: 0x00032FDC
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("micro_event_invariable", this.m_pMicroEvent.m_pMicroEventData.GetInvariableData());
		return dictionary;
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00034E0C File Offset: 0x0003300C
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		MicroEvent microEvent = pGame.microEventManager.GetMicroEvent(this.m_pMicroEvent.m_pMicroEventData.m_nDID);
		if (microEvent != null)
		{
			microEvent.m_ulCompleteTime = this.m_pMicroEvent.m_ulCompleteTime;
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00034E54 File Offset: 0x00033054
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["micro_events"];
		int nDID = this.m_pMicroEvent.m_pMicroEventData.m_nDID;
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)list[i];
			if (TFUtils.LoadInt(dictionary, MicroEventData._sDID) == nDID)
			{
				if (dictionary.ContainsKey(MicroEvent._sCOMPLETE_TIME))
				{
					dictionary[MicroEvent._sCOMPLETE_TIME] = this.m_pMicroEvent.m_ulCompleteTime;
				}
				else
				{
					dictionary.Add(MicroEvent._sCOMPLETE_TIME, this.m_pMicroEvent.m_ulCompleteTime);
				}
				break;
			}
		}
		base.Confirm(pGameState);
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00034F28 File Offset: 0x00033128
	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
		pData.Add("micro_event_id", this.m_pMicroEvent.m_pMicroEventData.m_nDID);
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00034F4C File Offset: 0x0003314C
	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005F2 RID: 1522
	public const string MICRO_EVENT_COMPLETE = "mca";

	// Token: 0x040005F3 RID: 1523
	private MicroEvent m_pMicroEvent;
}
