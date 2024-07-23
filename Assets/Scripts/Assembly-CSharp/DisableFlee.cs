using System;
using System.Collections.Generic;

// Token: 0x02000224 RID: 548
public class DisableFlee : SessionActionDefinition
{
	// Token: 0x060011FC RID: 4604 RVA: 0x0007DB44 File Offset: 0x0007BD44
	public static DisableFlee Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		DisableFlee disableFlee = new DisableFlee();
		disableFlee.Parse(data, id, startConditions, originatedFromQuest);
		return disableFlee;
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x0007DB64 File Offset: 0x0007BD64
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.wandererID = TFUtils.TryLoadInt(data, "id");
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x0007DB94 File Offset: 0x0007BD94
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		Dictionary<string, object> dictionary2 = dictionary;
		string key = "id";
		int? num = this.wandererID;
		dictionary2[key] = ((num != null) ? this.wandererID : new int?(-1));
		return dictionary;
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x0007DBE0 File Offset: 0x0007BDE0
	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		int? num = this.wandererID;
		if (num != null)
		{
			int? num2 = this.wandererID;
			if (num2 == null || num2.Value >= 0)
			{
				Simulated simulated = session.TheGame.simulation.FindSimulated(new int?(this.wandererID.Value));
				if (simulated == null)
				{
					action.MarkFailed();
					return;
				}
				ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
				entity.DisableFlee = new bool?(true);
				session.TheGame.simulation.ModifyGameState(new DisableFleeAction(this.wandererID.Value, simulated.Id.Describe()));
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}

	// Token: 0x04000C49 RID: 3145
	public const string TYPE = "disable_flee";

	// Token: 0x04000C4A RID: 3146
	public const string WANDERER_ID = "id";

	// Token: 0x04000C4B RID: 3147
	private int? wandererID;
}
