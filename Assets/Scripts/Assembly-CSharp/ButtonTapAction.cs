using System;
using System.Collections.Generic;

// Token: 0x020000C9 RID: 201
public class ButtonTapAction : PersistedTriggerableAction
{
	// Token: 0x0600079C RID: 1948 RVA: 0x0003204C File Offset: 0x0003024C
	public ButtonTapAction(string sID) : base("bt", Identity.Null())
	{
		this.m_sID = sID;
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x0600079D RID: 1949 RVA: 0x00032068 File Offset: 0x00030268
	public TriggerableMixin Triggerable
	{
		get
		{
			return this.triggerable;
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x0600079E RID: 1950 RVA: 0x00032070 File Offset: 0x00030270
	public override bool IsUserInitiated
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x00032074 File Offset: 0x00030274
	public new static ButtonTapAction FromDict(Dictionary<string, object> data)
	{
		string sID = TFUtils.LoadString(data, "button_id");
		return new ButtonTapAction(sID);
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00032098 File Offset: 0x00030298
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["button_id"] = this.m_sID;
		return dictionary;
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x000320C0 File Offset: 0x000302C0
	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x000320CC File Offset: 0x000302CC
	public override void Confirm(Dictionary<string, object> gameState)
	{
		base.Confirm(gameState);
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x000320D8 File Offset: 0x000302D8
	public virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data.Add("button_id", this.m_sID);
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x000320EC File Offset: 0x000302EC
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x040005AA RID: 1450
	public const string BUTTON_TAP = "bt";

	// Token: 0x040005AB RID: 1451
	public string m_sID;
}
