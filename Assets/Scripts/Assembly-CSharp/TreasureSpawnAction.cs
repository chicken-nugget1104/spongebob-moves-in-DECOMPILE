using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class TreasureSpawnAction : PersistedSimulatedAction
{
	// Token: 0x06000957 RID: 2391 RVA: 0x0003AB04 File Offset: 0x00038D04
	public TreasureSpawnAction(Identity id, int did, EntityType extensions, Vector2 position, string persistName, ulong? timeToTreasure) : base("ts", id, typeof(TreasureSpawnAction).ToString())
	{
		this.extensions = extensions;
		this.did = did;
		this.position = position;
		this.persistName = persistName;
		this.nextTreasureTime = timeToTreasure;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0003AB54 File Offset: 0x00038D54
	public TreasureSpawnAction(Simulated simulated, TreasureSpawner treasureTiming) : this(simulated.Id, simulated.entity.DefinitionId, simulated.entity.AllTypes, simulated.Position, treasureTiming.PersistName, treasureTiming.TimeToTreasure)
	{
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000959 RID: 2393 RVA: 0x0003AB98 File Offset: 0x00038D98
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0003AB9C File Offset: 0x00038D9C
	public new static TreasureSpawnAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		EntityType entityType = (EntityType)TFUtils.LoadUint(data, "extensions");
		int num = TFUtils.LoadInt(data, "did");
		Vector2 vector = new Vector2(TFUtils.LoadFloat(data, "x"), TFUtils.LoadFloat(data, "y"));
		string text = "time_to_spawn";
		ulong? timeToTreasure = null;
		if (data.ContainsKey("persist_name"))
		{
			text = TFUtils.LoadString(data, "persist_name");
			timeToTreasure = TFUtils.TryLoadUlong(data, "next_treasure_time", 0UL);
		}
		return new TreasureSpawnAction(id, num, entityType, vector, text, timeToTreasure);
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0003AC44 File Offset: 0x00038E44
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["extensions"] = (uint)this.extensions;
		dictionary["did"] = this.did;
		dictionary["x"] = this.position.x;
		dictionary["y"] = this.position.y;
		dictionary["persist_name"] = this.persistName;
		dictionary["next_treasure_time"] = this.nextTreasureTime;
		return dictionary;
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0003ACE4 File Offset: 0x00038EE4
	public override void Apply(Game game, ulong utcNow)
	{
		Entity entity = game.entities.Create(this.extensions, this.did, this.target, true);
		TreasureEntity decorator = entity.GetDecorator<TreasureEntity>();
		if (decorator.TreasureTiming == null)
		{
			decorator.TreasureTiming = game.treasureManager.FindTreasureSpawner(this.persistName);
		}
		TreasureSpawner treasureTiming = decorator.TreasureTiming;
		treasureTiming.MarkComplete();
		Simulated simulated = Simulated.Treasure.Load(decorator, game.simulation, this.position, utcNow);
		simulated.EnterInitialState(EntityManager.TreasureActions["buried"], game.simulation);
		base.Apply(game, utcNow);
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0003AD80 File Offset: 0x00038F80
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		if (dictionary.ContainsKey("treasure_state"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["treasure_state"];
			dictionary2[this.persistName] = this.nextTreasureTime;
		}
		List<object> orCreateList = TFUtils.GetOrCreateList<object>(dictionary, "treasure");
		Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
		dictionary3["did"] = this.did;
		dictionary3["extensions"] = (uint)this.extensions;
		dictionary3["label"] = this.target.Describe();
		dictionary3["x"] = this.position.x;
		dictionary3["y"] = this.position.y;
		dictionary3["treasure_spawner_name"] = this.persistName;
		orCreateList.Add(dictionary3);
		base.Confirm(gameState);
	}

	// Token: 0x04000680 RID: 1664
	public const string TREASURE_SPAWN = "ts";

	// Token: 0x04000681 RID: 1665
	private Vector2 position;

	// Token: 0x04000682 RID: 1666
	private EntityType extensions;

	// Token: 0x04000683 RID: 1667
	private int did;

	// Token: 0x04000684 RID: 1668
	private string persistName;

	// Token: 0x04000685 RID: 1669
	private ulong? nextTreasureTime;
}
