using System;
using System.Collections.Generic;

// Token: 0x0200022E RID: 558
public class ForceTreasureSpawn : SessionActionDefinition
{
	// Token: 0x06001233 RID: 4659 RVA: 0x0007EACC File Offset: 0x0007CCCC
	public static ForceTreasureSpawn Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceTreasureSpawn forceTreasureSpawn = new ForceTreasureSpawn();
		forceTreasureSpawn.Parse(data, id, startConditions, originatedFromQuest);
		return forceTreasureSpawn;
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x0007EAEC File Offset: 0x0007CCEC
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.targetSpawner = TFUtils.LoadString(data, "persist_name");
		this.succeedOnFailure = TFUtils.TryLoadBool(data, "succeed_on_failure");
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x0007EB24 File Offset: 0x0007CD24
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["persist_name"] = this.targetSpawner;
		dictionary["succeed_on_failure"] = this.succeedOnFailure;
		return dictionary;
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x0007EB60 File Offset: 0x0007CD60
	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		TreasureSpawner treasureSpawner = session.TheGame.treasureManager.FindTreasureSpawner(this.targetSpawner);
		TFUtils.Assert(treasureSpawner != null, "Failed to find the treasure spawner: " + this.targetSpawner);
		if ((treasureSpawner != null && treasureSpawner.PlaceTreasure()) || (this.succeedOnFailure != null && this.succeedOnFailure.Value))
		{
			action.MarkSucceeded();
			return;
		}
		action.MarkFailed();
	}

	// Token: 0x04000C7F RID: 3199
	public const string TYPE = "force_treasure_spawn";

	// Token: 0x04000C80 RID: 3200
	private const string TARGET_SPAWNER = "persist_name";

	// Token: 0x04000C81 RID: 3201
	private const string SUCCEED_ON_FAILURE = "succeed_on_failure";

	// Token: 0x04000C82 RID: 3202
	private string targetSpawner;

	// Token: 0x04000C83 RID: 3203
	private bool? succeedOnFailure;
}
