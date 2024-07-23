using System;
using System.Collections.Generic;

// Token: 0x0200022B RID: 555
public class ForceRentReady : SessionActionDefinition
{
	// Token: 0x06001224 RID: 4644 RVA: 0x0007E560 File Offset: 0x0007C760
	public static ForceRentReady Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceRentReady forceRentReady = new ForceRentReady();
		forceRentReady.Parse(data, id, startConditions, originatedFromQuest);
		return forceRentReady;
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x0007E580 File Offset: 0x0007C780
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.targetDid = TFUtils.TryLoadNullableInt(data, "building_did");
		string text = TFUtils.TryLoadString(data, "building_identity");
		if (text != null)
		{
			this.targetIdentity = new Identity(text);
		}
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x0007E5D0 File Offset: 0x0007C7D0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		int? num = this.targetDid;
		if (num != null)
		{
			dictionary["building_did"] = this.targetDid;
		}
		return dictionary;
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x0007E610 File Offset: 0x0007C810
	public void Handle(Session session, SessionActionTracker action)
	{
		action.MarkStarted();
		Simulated simulated = null;
		if (this.targetIdentity != null)
		{
			simulated = session.TheGame.simulation.FindSimulated(this.targetIdentity);
		}
		else if (this.targetDid != null)
		{
			simulated = session.TheGame.simulation.FindSimulated(new int?(this.targetDid.Value));
		}
		TFUtils.Assert(simulated != null, "Failed to find a simulated for Force Rent Ready Session Action: " + this.ToString());
		if (simulated != null)
		{
			simulated.ClearPendingCommands();
			session.TheGame.simulation.Router.CancelMatching(Command.TYPE.COMPLETE, simulated.Id, simulated.Id, null);
			BuildingEntity entity = simulated.GetEntity<BuildingEntity>();
			if (entity.HasDecorator<PeriodicProductionDecorator>())
			{
				entity.GetDecorator<PeriodicProductionDecorator>().ProductReadyTime = TFUtils.EpochTime();
				simulated.EnterInitialState(EntityManager.BuildingActions["produced"], session.TheGame.simulation);
			}
			else
			{
				simulated.EnterInitialState(EntityManager.BuildingActions["active"], session.TheGame.simulation);
			}
		}
		action.MarkSucceeded();
	}

	// Token: 0x04000C6C RID: 3180
	public const string TYPE = "force_rent_ready";

	// Token: 0x04000C6D RID: 3181
	private const string BUILDING_DID = "building_did";

	// Token: 0x04000C6E RID: 3182
	private const string BUILDING_IDENTITY = "building_identity";

	// Token: 0x04000C6F RID: 3183
	private int? targetDid;

	// Token: 0x04000C70 RID: 3184
	private Identity targetIdentity;
}
