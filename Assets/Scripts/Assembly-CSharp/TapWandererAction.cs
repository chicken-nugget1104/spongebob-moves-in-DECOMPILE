using System;
using System.Collections.Generic;

// Token: 0x02000100 RID: 256
public class TapWandererAction : PersistedSimulatedAction
{
	// Token: 0x06000929 RID: 2345 RVA: 0x00039EBC File Offset: 0x000380BC
	public TapWandererAction(Identity id, int did) : base("tw", id, typeof(TapWandererAction).ToString())
	{
		this.dId = did;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00039EEC File Offset: 0x000380EC
	public TapWandererAction(Simulated simulated) : base("tw", simulated.Id, typeof(TapWandererAction).ToString())
	{
		Entity entity = simulated.entity;
		this.dId = entity.DefinitionId;
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x0600092B RID: 2347 RVA: 0x00039F2C File Offset: 0x0003812C
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00039F30 File Offset: 0x00038130
	public new static TapWandererAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		int did = TFUtils.LoadInt(data, "did");
		return new TapWandererAction(id, did);
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00039F68 File Offset: 0x00038168
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = this.dId;
		return dictionary;
	}

	// Token: 0x0400066B RID: 1643
	public const string TAP_WANDERER = "tw";

	// Token: 0x0400066C RID: 1644
	public int dId;
}
