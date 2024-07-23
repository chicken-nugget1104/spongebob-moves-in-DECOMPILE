using System;
using System.Collections.Generic;

// Token: 0x02000228 RID: 552
public class ForceCraftingInstanceReady : SessionActionDefinition
{
	// Token: 0x06001214 RID: 4628 RVA: 0x0007E14C File Offset: 0x0007C34C
	public static ForceCraftingInstanceReady Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceCraftingInstanceReady forceCraftingInstanceReady = new ForceCraftingInstanceReady();
		forceCraftingInstanceReady.Parse(data, id, startConditions, originatedFromQuest);
		return forceCraftingInstanceReady;
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x0007E16C File Offset: 0x0007C36C
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.slotId = TFUtils.LoadInt(data, "slot_id");
		this.targetDid = TFUtils.TryLoadNullableInt(data, "building_did");
		string text = TFUtils.TryLoadString(data, "building_identity");
		if (text != null)
		{
			this.targetIdentity = new Identity(text);
		}
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x0007E1CC File Offset: 0x0007C3CC
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

	// Token: 0x06001217 RID: 4631 RVA: 0x0007E20C File Offset: 0x0007C40C
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
			CraftingInstance craftingInstance = session.TheGame.craftManager.GetCraftingInstance(simulated.Id, this.slotId);
			if (craftingInstance != null)
			{
				craftingInstance.ReadyTimeFromNow = 0UL;
				session.TheGame.simulation.Router.CancelMatching(Command.TYPE.CRAFTED, simulated.Id, simulated.Id, new Dictionary<string, object>
				{
					{
						"slot_id",
						this.slotId
					}
				});
				session.TheGame.simulation.Router.Send(CraftedCommand.Create(simulated.Id, simulated.Id, this.slotId), 0UL);
			}
		}
		action.MarkSucceeded();
	}

	// Token: 0x04000C5C RID: 3164
	public const string TYPE = "force_crafting_instance_ready";

	// Token: 0x04000C5D RID: 3165
	private const string SLOT_ID = "slot_id";

	// Token: 0x04000C5E RID: 3166
	private const string BUILDING_DID = "building_did";

	// Token: 0x04000C5F RID: 3167
	private const string BUILDING_IDENTITY = "building_identity";

	// Token: 0x04000C60 RID: 3168
	private int slotId;

	// Token: 0x04000C61 RID: 3169
	private int? targetDid;

	// Token: 0x04000C62 RID: 3170
	private Identity targetIdentity;
}
