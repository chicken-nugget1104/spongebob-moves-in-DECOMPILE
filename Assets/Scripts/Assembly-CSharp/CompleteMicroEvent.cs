using System;
using System.Collections.Generic;

// Token: 0x02000223 RID: 547
public class CompleteMicroEvent : SessionActionDefinition
{
	// Token: 0x060011F7 RID: 4599 RVA: 0x0007D9F8 File Offset: 0x0007BBF8
	public static CompleteMicroEvent Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		CompleteMicroEvent completeMicroEvent = new CompleteMicroEvent();
		completeMicroEvent.Parse(data, id, startConditions, originatedFromQuest);
		return completeMicroEvent;
	}

	// Token: 0x060011F8 RID: 4600 RVA: 0x0007DA18 File Offset: 0x0007BC18
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.microEventDID = TFUtils.TryLoadInt(data, "id");
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x0007DA48 File Offset: 0x0007BC48
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		Dictionary<string, object> dictionary2 = dictionary;
		string key = "id";
		int? num = this.microEventDID;
		dictionary2[key] = ((num != null) ? this.microEventDID : new int?(-1));
		return dictionary;
	}

	// Token: 0x060011FA RID: 4602 RVA: 0x0007DA94 File Offset: 0x0007BC94
	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		int? num = this.microEventDID;
		if (num == null || this.microEventDID.Value < 0)
		{
			action.MarkFailed();
			return;
		}
		MicroEvent microEvent = session.TheGame.microEventManager.GetMicroEvent(this.microEventDID.Value);
		if (microEvent == null || microEvent.IsCompleted())
		{
			action.MarkFailed();
			return;
		}
		microEvent.m_ulCompleteTime = new ulong?(TFUtils.EpochTime());
		session.TheGame.simulation.ModifyGameState(new MicroEventCompleteAction(microEvent));
		action.MarkSucceeded();
	}

	// Token: 0x04000C46 RID: 3142
	public const string TYPE = "complete_micro_event";

	// Token: 0x04000C47 RID: 3143
	public const string MICRO_EVENT_DID = "id";

	// Token: 0x04000C48 RID: 3144
	private int? microEventDID;
}
