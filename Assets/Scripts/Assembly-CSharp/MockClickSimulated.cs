using System;
using System.Collections.Generic;

// Token: 0x02000233 RID: 563
public class MockClickSimulated : SessionActionDefinition
{
	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06001248 RID: 4680 RVA: 0x0007F098 File Offset: 0x0007D298
	public Identity TargetId
	{
		get
		{
			return this.targetId;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06001249 RID: 4681 RVA: 0x0007F0A0 File Offset: 0x0007D2A0
	public int? TargetDid
	{
		get
		{
			return this.targetDid;
		}
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x0007F0A8 File Offset: 0x0007D2A8
	public static MockClickSimulated Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		MockClickSimulated mockClickSimulated = new MockClickSimulated();
		mockClickSimulated.Parse(data, id, startConditions, originatedFromQuest);
		return mockClickSimulated;
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x0007F0C8 File Offset: 0x0007D2C8
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		string text = TFUtils.TryLoadString(data, "instance_id");
		if (text != null)
		{
			this.targetId = new Identity(text);
		}
		this.targetDid = TFUtils.TryLoadNullableInt(data, "definition_id");
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x0007F118 File Offset: 0x0007D318
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		if (this.targetId != null)
		{
			dictionary["instance_id"] = this.targetId;
		}
		int? num = this.targetDid;
		if (num != null)
		{
			dictionary["definition_id"] = this.targetDid;
		}
		return dictionary;
	}

	// Token: 0x0600124D RID: 4685 RVA: 0x0007F174 File Offset: 0x0007D374
	public void HandleClick(Session session, SessionActionTracker action, Simulated simulated)
	{
		session.AddAsyncResponse("mock_click_sessionaction", simulated);
		action.MarkStarted();
		action.MarkSucceeded();
	}

	// Token: 0x04000C8B RID: 3211
	public const string TYPE = "mock_click_simulated";

	// Token: 0x04000C8C RID: 3212
	public const string ACTION = "mock_click_sessionaction";

	// Token: 0x04000C8D RID: 3213
	private const string INSTANCE_ID = "instance_id";

	// Token: 0x04000C8E RID: 3214
	private const string DEFINITION_ID = "definition_id";

	// Token: 0x04000C8F RID: 3215
	private Identity targetId;

	// Token: 0x04000C90 RID: 3216
	private int? targetDid;
}
