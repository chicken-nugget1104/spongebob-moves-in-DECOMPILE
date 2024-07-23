using System;
using System.Collections.Generic;

// Token: 0x020000E8 RID: 232
public abstract class PersistedSimulatedAction : PersistedTriggerableAction
{
	// Token: 0x0600087A RID: 2170 RVA: 0x00036E90 File Offset: 0x00035090
	protected PersistedSimulatedAction(string type, Identity target, string triggerType) : base(type, target)
	{
		this.triggerType = triggerType;
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00036EAC File Offset: 0x000350AC
	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data["simulated_guid"] = this.entityId;
		data["simulated_id"] = this.definitionId;
		data["simulated_type"] = this.simType;
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00036EEC File Offset: 0x000350EC
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		if (!data.ContainsKey("simulated"))
		{
			TFUtils.Assert(data.ContainsKey("simulated"), string.Format("Did not find key({0}) in dictionary:{1}", "simulated", TFUtils.DebugDictToString(data)));
		}
		Simulated simulated = (Simulated)data["simulated"];
		this.entityId = simulated.entity.Id;
		this.definitionId = simulated.entity.DefinitionId;
		this.simType = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00036F9C File Offset: 0x0003519C
	public override ITrigger CreateTrigger(string type)
	{
		return this.triggerable.BuildTrigger(type, new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), this.target, this.dropID);
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00036FC4 File Offset: 0x000351C4
	protected void DropTargetDataFromDict(Dictionary<string, object> data)
	{
		this.entityId = new Identity((string)data["entityId"]);
		this.definitionId = TFUtils.LoadInt(data, "definitionId");
		this.simType = (string)data["simType"];
		if (data.ContainsKey("dropID"))
		{
			this.dropID = new Identity((string)data["dropID"]);
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00037040 File Offset: 0x00035240
	protected void DropTargetDataToDict(Dictionary<string, object> data)
	{
		data["entityId"] = this.entityId.Describe();
		data["definitionId"] = this.definitionId;
		data["simType"] = this.simType;
		if (this.dropID != null)
		{
			data["dropID"] = this.dropID.Describe();
		}
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x000370AC File Offset: 0x000352AC
	public void AddDropData(Simulated simulated, Identity dropID)
	{
		this.entityId = simulated.entity.Id;
		this.definitionId = simulated.entity.DefinitionId;
		this.simType = EntityTypeNamingHelper.TypeToString(simulated.entity.AllTypes);
		this.dropID = dropID;
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x000370F8 File Offset: 0x000352F8
	public void AddPickup(Simulation simulation)
	{
		Trigger trigger = (Trigger)this.CreateTrigger(this.triggerType);
		simulation.DropManager.AddPickupTrigger(trigger.ToDict());
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00037128 File Offset: 0x00035328
	public void AddPickupToGameState(Dictionary<string, object> gameState)
	{
		Trigger trigger = (Trigger)this.CreateTrigger(this.triggerType);
		ItemDropManager.AddPickupTriggerToGameState(gameState, trigger.ToDict());
	}

	// Token: 0x0400061B RID: 1563
	public const string SIMULATED = "simulated";

	// Token: 0x0400061C RID: 1564
	protected Identity entityId;

	// Token: 0x0400061D RID: 1565
	protected int definitionId;

	// Token: 0x0400061E RID: 1566
	protected string simType;

	// Token: 0x0400061F RID: 1567
	private string triggerType = "undefined";

	// Token: 0x04000620 RID: 1568
	public Identity dropID;
}
