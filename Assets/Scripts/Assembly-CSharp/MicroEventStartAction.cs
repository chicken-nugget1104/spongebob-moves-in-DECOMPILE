using System;
using System.Collections.Generic;

// Token: 0x020000E0 RID: 224
public class MicroEventStartAction : PersistedTriggerableAction
{
	// Token: 0x06000846 RID: 2118 RVA: 0x00035130 File Offset: 0x00033330
	public MicroEventStartAction(MicroEvent pMicroEvent) : base("msa", Identity.Null())
	{
		this.m_pMicroEvent = pMicroEvent;
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000847 RID: 2119 RVA: 0x0003514C File Offset: 0x0003334C
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00035150 File Offset: 0x00033350
	public new static MicroEventStartAction FromDict(Dictionary<string, object> pData)
	{
		Dictionary<string, object> pInvariableData = TFUtils.LoadDict(pData, "micro_event_invariable");
		MicroEvent pMicroEvent = new MicroEvent(null, pInvariableData, true);
		return new MicroEventStartAction(pMicroEvent);
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x0003517C File Offset: 0x0003337C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary.Add("micro_event_invariable", this.m_pMicroEvent.m_pMicroEventData.GetInvariableData());
		return dictionary;
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x000351AC File Offset: 0x000333AC
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		pGame.microEventManager.AddMicroEvent(pGame, new MicroEvent(pGame, this.m_pMicroEvent.GetInvariableData(), false), false);
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x000351E0 File Offset: 0x000333E0
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["micro_events"];
		list.Add(this.m_pMicroEvent.GetInvariableData());
		base.Confirm(pGameState);
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00035228 File Offset: 0x00033428
	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
		pData.Add("micro_event_id", this.m_pMicroEvent.m_pMicroEventData.m_nDID);
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x0003524C File Offset: 0x0003344C
	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005F6 RID: 1526
	public const string MICRO_EVENT_START = "msa";

	// Token: 0x040005F7 RID: 1527
	private MicroEvent m_pMicroEvent;
}
