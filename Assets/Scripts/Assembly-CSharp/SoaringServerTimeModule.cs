using System;

// Token: 0x02000377 RID: 887
public class SoaringServerTimeModule : SoaringModule
{
	// Token: 0x06001932 RID: 6450 RVA: 0x000A64BC File Offset: 0x000A46BC
	public override string ModuleName()
	{
		return "retrieveServerTime";
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x000A64C4 File Offset: 0x000A46C4
	public override int ModuleChannel()
	{
		return 4;
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x000A64C8 File Offset: 0x000A46C8
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		long num = 0L;
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		if (moduleData.state)
		{
			SoaringValue soaringValue = moduleData.data.soaringValue("timestamp");
			if (soaringValue != null)
			{
				long num2 = soaringValue;
				num = num2;
				SoaringTime.UpdateServerTime(num);
			}
		}
		SoaringInternal.Delegate.OnServerTimeUpdated(moduleData.state, moduleData.error, num, moduleData.context);
	}
}
