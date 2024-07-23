using System;
using System.Collections.Generic;

// Token: 0x020000D4 RID: 212
public class DisableFleeAction : PersistedTriggerableAction
{
	// Token: 0x060007F0 RID: 2032 RVA: 0x00033DB0 File Offset: 0x00031FB0
	public DisableFleeAction(int did, string id) : base("df", Identity.Null())
	{
		this.did = did;
		this.id = id;
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00033DD0 File Offset: 0x00031FD0
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x00033DD4 File Offset: 0x00031FD4
	public new static DisableFleeAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "did");
		string text = TFUtils.LoadString(data, "id");
		return new DisableFleeAction(num, text);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x00033E04 File Offset: 0x00032004
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = this.did;
		dictionary["id"] = this.id;
		return dictionary;
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00033E40 File Offset: 0x00032040
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = null;
		if (!string.IsNullOrEmpty(this.id))
		{
			simulated = game.simulation.FindSimulated(new Identity(this.id));
		}
		if (simulated != null)
		{
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			entity.DisableFlee = new bool?(true);
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00033E98 File Offset: 0x00032098
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> wandererGameState = ResidentEntity.GetWandererGameState(gameState, new Identity(this.id));
		if (wandererGameState != null)
		{
			wandererGameState["disable_flee"] = true;
		}
		base.Confirm(gameState);
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x00033ED8 File Offset: 0x000320D8
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x00033EDC File Offset: 0x000320DC
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005D5 RID: 1493
	public const string DISABLE_FLEE = "df";

	// Token: 0x040005D6 RID: 1494
	public int did;

	// Token: 0x040005D7 RID: 1495
	public string id;
}
