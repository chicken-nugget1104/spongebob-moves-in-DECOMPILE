using System;
using System.Collections.Generic;

// Token: 0x0200022A RID: 554
public class ForceProduce : SessionActionDefinition
{
	// Token: 0x0600121F RID: 4639 RVA: 0x0007E3D8 File Offset: 0x0007C5D8
	public static ForceProduce Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceProduce forceProduce = new ForceProduce();
		forceProduce.Parse(data, id, startConditions, originatedFromQuest);
		return forceProduce;
	}

	// Token: 0x06001220 RID: 4640 RVA: 0x0007E3F8 File Offset: 0x0007C5F8
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
		string text = TFUtils.TryLoadString(data, "identity");
		if (text != null)
		{
			this.targetIdentity = new Identity(text);
		}
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x0007E448 File Offset: 0x0007C648
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		int? num = this.targetDid;
		if (num != null)
		{
			dictionary["definition_id"] = this.targetDid;
		}
		return dictionary;
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x0007E488 File Offset: 0x0007C688
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
		TFUtils.Assert(simulated != null, "Failed to find a simulated for Force Produce Session Action: " + this.ToString());
		if (simulated != null && simulated.GetEntity<BuildingEntity>() != null)
		{
			simulated.ClearPendingCommands();
			simulated.EnterInitialState(EntityManager.BuildingActions["produced"], session.TheGame.simulation);
			action.MarkSucceeded();
			return;
		}
		action.MarkFailed();
	}

	// Token: 0x04000C67 RID: 3175
	public const string TYPE = "force_produce";

	// Token: 0x04000C68 RID: 3176
	private const string DEFINITION_ID = "definition_id";

	// Token: 0x04000C69 RID: 3177
	private const string IDENTITY = "identity";

	// Token: 0x04000C6A RID: 3178
	private int? targetDid;

	// Token: 0x04000C6B RID: 3179
	private Identity targetIdentity;
}
