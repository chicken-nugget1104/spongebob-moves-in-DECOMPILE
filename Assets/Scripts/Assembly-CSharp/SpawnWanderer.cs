using System;
using System.Collections.Generic;

// Token: 0x02000254 RID: 596
public class SpawnWanderer : SessionActionDefinition
{
	// Token: 0x06001332 RID: 4914 RVA: 0x00084838 File Offset: 0x00082A38
	public static SpawnWanderer Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		SpawnWanderer spawnWanderer = new SpawnWanderer();
		spawnWanderer.Parse(data, id, startConditions, originatedFromQuest);
		return spawnWanderer;
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x00084858 File Offset: 0x00082A58
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.wandererID = TFUtils.TryLoadInt(data, "id");
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x00084888 File Offset: 0x00082A88
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		Dictionary<string, object> dictionary2 = dictionary;
		string key = "id";
		int? num = this.wandererID;
		dictionary2[key] = ((num != null) ? this.wandererID : new int?(-1));
		return dictionary;
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x000848D4 File Offset: 0x00082AD4
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
				ResidentEntity residentEntity = null;
				if (simulated == null)
				{
					residentEntity = session.TheGame.simulation.EntityManager.Create(EntityType.WANDERER, this.wandererID.Value, null, true).GetDecorator<ResidentEntity>();
					simulated = Simulated.Wanderer.Load(residentEntity, new ulong?(TFUtils.EpochTime()), residentEntity.DisableFlee, session.TheGame.simulation, TFUtils.EpochTime());
				}
				if (residentEntity == null)
				{
					residentEntity = simulated.GetEntity<ResidentEntity>();
					residentEntity.HideExpiresAt = new ulong?(TFUtils.EpochTime());
					simulated.EnterInitialState(EntityManager.WandererActions["spawn"], session.TheGame.simulation);
				}
				session.TheGame.simulation.ModifyGameState(new SpawnWandererAction(this.wandererID.Value, simulated.Id.Describe()));
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}

	// Token: 0x04000D46 RID: 3398
	public const string TYPE = "spawn_wanderer";

	// Token: 0x04000D47 RID: 3399
	public const string WANDERER_ID = "id";

	// Token: 0x04000D48 RID: 3400
	private int? wandererID;
}
