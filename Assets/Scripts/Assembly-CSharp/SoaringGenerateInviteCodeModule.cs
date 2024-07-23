using System;

// Token: 0x0200035E RID: 862
public class SoaringGenerateInviteCodeModule : SoaringModule
{
	// Token: 0x060018AF RID: 6319 RVA: 0x000A3590 File Offset: 0x000A1790
	public override string ModuleName()
	{
		return "retrieveInvitationCode";
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x000A3598 File Offset: 0x000A1798
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		string invite_code = null;
		if (moduleData.data != null)
		{
			invite_code = moduleData.data.soaringValue("invitationCode");
		}
		SoaringInternal.Delegate.OnRetrieveInvitationCode(moduleData.state, moduleData.error, invite_code);
	}
}
