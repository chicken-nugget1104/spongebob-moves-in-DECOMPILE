using System;
using System.Collections.Generic;

// Token: 0x020000FF RID: 255
public class SpawnWandererAction : PersistedTriggerableAction
{
	// Token: 0x06000921 RID: 2337 RVA: 0x00039CE4 File Offset: 0x00037EE4
	public SpawnWandererAction(int did, string id) : base("sw", Identity.Null())
	{
		this.did = did;
		this.id = id;
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000922 RID: 2338 RVA: 0x00039D04 File Offset: 0x00037F04
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00039D08 File Offset: 0x00037F08
	public new static SpawnWandererAction FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "did");
		string text = TFUtils.LoadString(data, "id");
		return new SpawnWandererAction(num, text);
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00039D38 File Offset: 0x00037F38
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = this.did;
		dictionary["id"] = this.id;
		return dictionary;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00039D74 File Offset: 0x00037F74
	public override void Apply(Game game, ulong utcNow)
	{
		Simulated simulated = game.simulation.FindSimulated(new int?(this.did));
		if (simulated == null)
		{
			ResidentEntity decorator = game.simulation.EntityManager.Create(EntityType.WANDERER, this.did, null, true).GetDecorator<ResidentEntity>();
			simulated = Simulated.Wanderer.Load(decorator, new ulong?(utcNow), decorator.DisableFlee, game.simulation, utcNow);
		}
		else
		{
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			entity.HideExpiresAt = new ulong?(TFUtils.EpochTime());
			simulated.EnterInitialState(EntityManager.WandererActions["spawn"], game.simulation);
		}
		this.id = simulated.Id.Describe();
		base.Apply(game, utcNow);
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x00039E30 File Offset: 0x00038030
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> wandererGameState = ResidentEntity.GetWandererGameState(gameState, this.did);
		if (wandererGameState == null)
		{
			Simulated.Wanderer.AddWandererToGameState(gameState, this.id, this.did);
		}
		else
		{
			wandererGameState["hide_expires_at"] = TFUtils.EpochTime();
		}
		base.Confirm(gameState);
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00039E84 File Offset: 0x00038084
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00039E88 File Offset: 0x00038088
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x04000668 RID: 1640
	public const string SPAWN_WANDERER = "sw";

	// Token: 0x04000669 RID: 1641
	public int did;

	// Token: 0x0400066A RID: 1642
	public string id;
}
