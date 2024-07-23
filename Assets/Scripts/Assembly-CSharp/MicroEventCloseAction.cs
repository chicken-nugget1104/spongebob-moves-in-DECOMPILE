using System;
using System.Collections.Generic;

// Token: 0x020000DD RID: 221
public class MicroEventCloseAction : PersistedTriggerableAction
{
	// Token: 0x0600082E RID: 2094 RVA: 0x00034BE4 File Offset: 0x00032DE4
	public MicroEventCloseAction(MicroEvent pMicroEvent) : base("mcla", Identity.Null())
	{
		this.m_pMicroEvent = pMicroEvent;
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x0600082F RID: 2095 RVA: 0x00034C00 File Offset: 0x00032E00
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00034C04 File Offset: 0x00032E04
	public new static MicroEventCloseAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "micro_event_invariable");
		MicroEvent pMicroEvent = new MicroEvent(null, pInvariableData, true);
		return new MicroEventCloseAction(pMicroEvent);
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00034C2C File Offset: 0x00032E2C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("micro_event_invariable", this.m_pMicroEvent.m_pMicroEventData.GetInvariableData());
		return dictionary;
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00034C5C File Offset: 0x00032E5C
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		MicroEvent microEvent = pGame.microEventManager.GetMicroEvent(this.m_pMicroEvent.m_pMicroEventData.m_nDID);
		if (microEvent != null)
		{
			microEvent.m_bIsClosed = true;
		}
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00034C9C File Offset: 0x00032E9C
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
				if (dictionary.ContainsKey(MicroEvent._sCLOSED))
				{
					dictionary[MicroEvent._sCLOSED] = true;
				}
				else
				{
					dictionary.Add(MicroEvent._sCLOSED, true);
				}
				break;
			}
		}
		base.Confirm(pGameState);
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00034D5C File Offset: 0x00032F5C
	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00034D60 File Offset: 0x00032F60
	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005F0 RID: 1520
	public const string MICRO_EVENT_CLOSE = "mcla";

	// Token: 0x040005F1 RID: 1521
	private MicroEvent m_pMicroEvent;
}
