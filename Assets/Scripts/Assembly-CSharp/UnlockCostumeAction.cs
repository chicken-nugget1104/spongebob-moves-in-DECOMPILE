using System;
using System.Collections.Generic;

// Token: 0x02000109 RID: 265
public class UnlockCostumeAction : PersistedTriggerableAction
{
	// Token: 0x06000964 RID: 2404 RVA: 0x0003B004 File Offset: 0x00039204
	public UnlockCostumeAction(int nCostumeDID) : base("uc", Identity.Null())
	{
		this.m_nCostumeDID = nCostumeDID;
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000965 RID: 2405 RVA: 0x0003B020 File Offset: 0x00039220
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0003B024 File Offset: 0x00039224
	public new static UnlockCostumeAction FromDict(Dictionary<string, object> pData)
	{
		int nCostumeDID = TFUtils.LoadInt(pData, "costume_did");
		return new UnlockCostumeAction(nCostumeDID);
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0003B048 File Offset: 0x00039248
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["costume_did"] = this.m_nCostumeDID;
		return dictionary;
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0003B074 File Offset: 0x00039274
	public override void Apply(Game pGame, ulong ulUtcNow)
	{
		base.Apply(pGame, ulUtcNow);
		pGame.costumeManager.UnlockCostume(this.m_nCostumeDID);
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0003B090 File Offset: 0x00039290
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		if (!dictionary.ContainsKey("costumes"))
		{
			dictionary["costumes"] = new List<object>();
		}
		List<object> list = (List<object>)dictionary["costumes"];
		if (!list.Contains(this.m_nCostumeDID))
		{
			list.Add(this.m_nCostumeDID);
		}
		base.Confirm(pGameState);
	}

	// Token: 0x04000688 RID: 1672
	public const string UNLOCK_COSTUME = "uc";

	// Token: 0x04000689 RID: 1673
	private int m_nCostumeDID;
}
