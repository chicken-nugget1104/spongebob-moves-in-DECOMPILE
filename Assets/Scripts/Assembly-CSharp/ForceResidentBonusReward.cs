using System;
using System.Collections.Generic;

// Token: 0x0200022C RID: 556
public class ForceResidentBonusReward : SessionActionDefinition
{
	// Token: 0x06001229 RID: 4649 RVA: 0x0007E740 File Offset: 0x0007C940
	public static ForceResidentBonusReward Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceResidentBonusReward forceResidentBonusReward = new ForceResidentBonusReward();
		forceResidentBonusReward.Parse(data, id, startConditions, originatedFromQuest);
		return forceResidentBonusReward;
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x0007E760 File Offset: 0x0007C960
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
		string text = TFUtils.TryLoadString(data, "identity");
		if (text != null)
		{
			this.targetIdentity = new Identity(text);
		}
		if (data.ContainsKey("reward"))
		{
			this.reward = RewardDefinition.FromObject(data["reward"]);
		}
		TFUtils.Assert(this.reward != null, "Failed to define a RewardDefinition for ForceResidentBonusReward Session Action");
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x0007E7EC File Offset: 0x0007C9EC
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

	// Token: 0x0600122C RID: 4652 RVA: 0x0007E82C File Offset: 0x0007CA2C
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
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			if (entity != null)
			{
				entity.ForcedBonusReward = this.reward;
				action.MarkSucceeded();
				return;
			}
		}
		action.MarkFailed();
	}

	// Token: 0x04000C71 RID: 3185
	public const string TYPE = "force_bonus_reward";

	// Token: 0x04000C72 RID: 3186
	private const string DEFINITION_ID = "definition_id";

	// Token: 0x04000C73 RID: 3187
	private const string IDENTITY = "identity";

	// Token: 0x04000C74 RID: 3188
	private const string REWARD = "reward";

	// Token: 0x04000C75 RID: 3189
	private int? targetDid;

	// Token: 0x04000C76 RID: 3190
	private Identity targetIdentity;

	// Token: 0x04000C77 RID: 3191
	private RewardDefinition reward;
}
