using System;
using System.Collections.Generic;

// Token: 0x0200022D RID: 557
public class ForceResidentHunger : SessionActionDefinition
{
	// Token: 0x0600122E RID: 4654 RVA: 0x0007E8EC File Offset: 0x0007CAEC
	public static ForceResidentHunger Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceResidentHunger forceResidentHunger = new ForceResidentHunger();
		forceResidentHunger.Parse(data, id, startConditions, originatedFromQuest);
		return forceResidentHunger;
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x0007E90C File Offset: 0x0007CB0C
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
		string text = TFUtils.TryLoadString(data, "identity");
		if (text != null)
		{
			this.targetIdentity = new Identity(text);
		}
		this.resourceId = TFUtils.LoadInt(data, "resource_id");
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x0007E96C File Offset: 0x0007CB6C
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

	// Token: 0x06001231 RID: 4657 RVA: 0x0007E9AC File Offset: 0x0007CBAC
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
		TFUtils.Assert(simulated != null, "Failed to find a simulated for Force Hunger Session Action: " + this.ToString());
		if (simulated != null)
		{
			simulated.forcedWish = new int?(this.resourceId);
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			if (entity != null)
			{
				entity.PreviousResourceId = entity.HungerResourceId;
				entity.HungerResourceId = new int?(this.resourceId);
				entity.WishExpiresAt = new ulong?(18446744073709551614UL);
				simulated.ClearPendingCommands();
				if (entity.m_pTask == null)
				{
					simulated.EnterInitialState(EntityManager.ResidentActions["wishing"], session.TheGame.simulation);
				}
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}

	// Token: 0x04000C78 RID: 3192
	public const string TYPE = "force_wish";

	// Token: 0x04000C79 RID: 3193
	private const string DEFINITION_ID = "definition_id";

	// Token: 0x04000C7A RID: 3194
	private const string IDENTITY = "identity";

	// Token: 0x04000C7B RID: 3195
	private const string RESOURCE_ID = "resource_id";

	// Token: 0x04000C7C RID: 3196
	private int? targetDid;

	// Token: 0x04000C7D RID: 3197
	private Identity targetIdentity;

	// Token: 0x04000C7E RID: 3198
	private int resourceId;
}
