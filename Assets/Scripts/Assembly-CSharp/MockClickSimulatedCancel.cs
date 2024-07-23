using System;
using System.Collections.Generic;

// Token: 0x02000234 RID: 564
public class MockClickSimulatedCancel : SessionActionDefinition
{
	// Token: 0x0600124F RID: 4687 RVA: 0x0007F198 File Offset: 0x0007D398
	public static MockClickSimulatedCancel Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		MockClickSimulatedCancel mockClickSimulatedCancel = new MockClickSimulatedCancel();
		mockClickSimulatedCancel.Parse(data, id, startConditions, originatedFromQuest);
		return mockClickSimulatedCancel;
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x0007F1B8 File Offset: 0x0007D3B8
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x0007F1CC File Offset: 0x0007D3CC
	public override Dictionary<string, object> ToDict()
	{
		return base.ToDict();
	}

	// Token: 0x06001252 RID: 4690 RVA: 0x0007F1E4 File Offset: 0x0007D3E4
	public void HandleCancel(Session session, SessionActionTracker action)
	{
		session.CheckAsyncRequest("mock_click_sessionaction");
		action.MarkStarted();
		action.MarkSucceeded();
	}

	// Token: 0x04000C91 RID: 3217
	public const string TYPE = "mock_click_simulated_cancel";
}
