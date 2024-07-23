using System;
using System.Collections.Generic;

// Token: 0x02000229 RID: 553
public class ForceCraftingInstanceSlot : SessionActionDefinition
{
	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001219 RID: 4633 RVA: 0x0007E344 File Offset: 0x0007C544
	public int SlotID
	{
		get
		{
			return this.slotId;
		}
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x0007E34C File Offset: 0x0007C54C
	public static ForceCraftingInstanceSlot Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		ForceCraftingInstanceSlot forceCraftingInstanceSlot = new ForceCraftingInstanceSlot();
		forceCraftingInstanceSlot.Parse(data, id, startConditions, originatedFromQuest);
		return forceCraftingInstanceSlot;
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x0007E36C File Offset: 0x0007C56C
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.slotId = TFUtils.LoadInt(data, "slot_id");
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x0007E39C File Offset: 0x0007C59C
	public override Dictionary<string, object> ToDict()
	{
		return base.ToDict();
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x0007E3B4 File Offset: 0x0007C5B4
	public void Handle(Session session, SessionActionTracker action)
	{
		session.AddAsyncResponse("force_crafting_instance_slot_sessionaction", action);
		action.MarkStarted();
		action.MarkSucceeded();
	}

	// Token: 0x04000C63 RID: 3171
	public const string TYPE = "force_crafting_instance_slot";

	// Token: 0x04000C64 RID: 3172
	public const string ACTION = "force_crafting_instance_slot_sessionaction";

	// Token: 0x04000C65 RID: 3173
	private const string SLOT_ID = "slot_id";

	// Token: 0x04000C66 RID: 3174
	private int slotId;
}
