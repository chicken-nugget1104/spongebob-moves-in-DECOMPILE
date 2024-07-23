using System;
using System.Collections.Generic;

// Token: 0x020000EA RID: 234
public class PickupDropAction : PersistedSimulatedAction
{
	// Token: 0x0600088A RID: 2186 RVA: 0x00037228 File Offset: 0x00035428
	public PickupDropAction(Identity id, Identity dropID) : base("pd", id, typeof(PickupDropAction).ToString())
	{
		this.dropID = dropID;
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x0600088B RID: 2187 RVA: 0x00037258 File Offset: 0x00035458
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0003725C File Offset: 0x0003545C
	public new static PickupDropAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		Identity dropID = new Identity((string)data["dropID"]);
		return new PickupDropAction(id, dropID);
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x000372A0 File Offset: 0x000354A0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["dropID"] = this.dropID.Describe();
		return dictionary;
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x000372CC File Offset: 0x000354CC
	public override void Apply(Game game, ulong utcNow)
	{
		game.dropManager.RemovePickupTrigger(this.dropID);
		base.Apply(game, utcNow);
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x000372E8 File Offset: 0x000354E8
	public override void Confirm(Dictionary<string, object> gameState)
	{
		ItemDropManager.RemovePickupTriggerFromGameState(gameState, this.dropID.Describe());
		base.Confirm(gameState);
	}

	// Token: 0x04000622 RID: 1570
	public const string PICKUP_DROP = "pd";

	// Token: 0x04000623 RID: 1571
	public const int INVALID_INT = -1;
}
