using System;
using System.Collections.Generic;

// Token: 0x020000CA RID: 202
public class ChangeCostumeAction : PersistedSimulatedAction
{
	// Token: 0x060007A5 RID: 1957 RVA: 0x00032120 File Offset: 0x00030320
	public ChangeCostumeAction(Identity ID, int nCostumeDID) : base("cca", ID, "ChangeCostume")
	{
		this.m_nCostumeDID = nCostumeDID;
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0003213C File Offset: 0x0003033C
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x00032140 File Offset: 0x00030340
	public new static ChangeCostumeAction FromDict(Dictionary<string, object> pData)
	{
		Identity id = new Identity((string)pData["target"]);
		int nCostumeDID = TFUtils.LoadInt(pData, "costume_did");
		ChangeCostumeAction changeCostumeAction = new ChangeCostumeAction(id, nCostumeDID);
		changeCostumeAction.DropTargetDataFromDict(pData);
		return changeCostumeAction;
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x00032180 File Offset: 0x00030380
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		base.DropTargetDataToDict(dictionary);
		dictionary.Add("costume_did", this.m_nCostumeDID);
		return dictionary;
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x000321B4 File Offset: 0x000303B4
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		Simulated simulated = pGame.simulation.FindSimulated(this.target);
		if (simulated == null)
		{
			return;
		}
		CostumeManager.Costume costume = pGame.costumeManager.GetCostume(this.m_nCostumeDID);
		if (costume != null)
		{
			ResidentEntity entity = simulated.GetEntity<ResidentEntity>();
			entity.CostumeDID = new int?(this.m_nCostumeDID);
			simulated.SetCostume(costume);
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0003221C File Offset: 0x0003041C
	protected override void AddMoreDataToTrigger(ref Dictionary<string, object> pData)
	{
		base.AddMoreDataToTrigger(ref pData);
		pData.Add("costume_id", this.m_nCostumeDID);
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x0003223C File Offset: 0x0003043C
	public override ITrigger CreateTrigger(Dictionary<string, object> pData)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00032270 File Offset: 0x00030470
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> unitGameState = ResidentEntity.GetUnitGameState(pGameState, this.target);
		unitGameState["costume_did"] = this.m_nCostumeDID;
		base.Confirm(pGameState);
	}

	// Token: 0x040005AC RID: 1452
	public const string CHANGE_COSTUME = "cca";

	// Token: 0x040005AD RID: 1453
	public const string TRIGGERTYPE = "ChangeCostume";

	// Token: 0x040005AE RID: 1454
	private int m_nCostumeDID;
}
