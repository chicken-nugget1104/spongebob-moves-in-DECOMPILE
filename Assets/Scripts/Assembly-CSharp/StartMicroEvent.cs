using System;
using System.Collections.Generic;

// Token: 0x02000255 RID: 597
public class StartMicroEvent : SessionActionDefinition
{
	// Token: 0x06001337 RID: 4919 RVA: 0x00084A24 File Offset: 0x00082C24
	public static StartMicroEvent Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		StartMicroEvent startMicroEvent = new StartMicroEvent();
		startMicroEvent.Parse(data, id, startConditions, originatedFromQuest);
		return startMicroEvent;
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x00084A44 File Offset: 0x00082C44
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.microEventDID = TFUtils.TryLoadInt(data, "id");
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x00084A74 File Offset: 0x00082C74
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		Dictionary<string, object> dictionary2 = dictionary;
		string key = "id";
		int? num = this.microEventDID;
		dictionary2[key] = ((num != null) ? this.microEventDID : new int?(-1));
		return dictionary;
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x00084AC0 File Offset: 0x00082CC0
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
		if (microEvent != null)
		{
			action.MarkFailed();
			return;
		}
		if (session.TheGame.microEventManager.GetMicroEventData(this.microEventDID.Value, false) == null)
		{
			action.MarkFailed();
			return;
		}
		session.TheGame.microEventManager.AddMicroEvent(session.TheGame, new MicroEvent(session.TheGame, this.microEventDID.Value, TFUtils.EpochTime()), false);
		microEvent = session.TheGame.microEventManager.GetMicroEvent(this.microEventDID.Value);
		if (microEvent == null)
		{
			action.MarkFailed();
			return;
		}
		session.TheGame.simulation.ModifyGameState(new MicroEventStartAction(microEvent));
		action.MarkSucceeded();
	}

	// Token: 0x04000D49 RID: 3401
	public const string TYPE = "start_micro_event";

	// Token: 0x04000D4A RID: 3402
	public const string MICRO_EVENT_DID = "id";

	// Token: 0x04000D4B RID: 3403
	private int? microEventDID;
}
