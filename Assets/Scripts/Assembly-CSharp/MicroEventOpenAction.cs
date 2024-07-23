using System;
using System.Collections.Generic;

// Token: 0x020000DF RID: 223
public class MicroEventOpenAction : PersistedTriggerableAction
{
	// Token: 0x0600083E RID: 2110 RVA: 0x00034F80 File Offset: 0x00033180
	public MicroEventOpenAction(MicroEvent pMicroEvent) : base("moa", Identity.Null())
	{
		this.m_pMicroEvent = pMicroEvent;
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x0600083F RID: 2111 RVA: 0x00034F9C File Offset: 0x0003319C
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x00034FA0 File Offset: 0x000331A0
	public new static MicroEventOpenAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "micro_event_invariable");
		MicroEvent pMicroEvent = new MicroEvent(null, pInvariableData, true);
		return new MicroEventOpenAction(pMicroEvent);
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00034FC8 File Offset: 0x000331C8
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("micro_event_invariable", this.m_pMicroEvent.m_pMicroEventData.GetInvariableData());
		return dictionary;
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00034FF8 File Offset: 0x000331F8
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		MicroEvent microEvent = pGame.microEventManager.GetMicroEvent(this.m_pMicroEvent.m_pMicroEventData.m_nDID);
		if (microEvent != null)
		{
			microEvent.m_bIsClosed = false;
		}
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00035038 File Offset: 0x00033238
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
					dictionary[MicroEvent._sCLOSED] = false;
				}
				else
				{
					dictionary.Add(MicroEvent._sCLOSED, false);
				}
				break;
			}
		}
		base.Confirm(pGameState);
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x000350F8 File Offset: 0x000332F8
	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x000350FC File Offset: 0x000332FC
	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005F4 RID: 1524
	public const string MICRO_EVENT_OPEN = "moa";

	// Token: 0x040005F5 RID: 1525
	private MicroEvent m_pMicroEvent;
}
