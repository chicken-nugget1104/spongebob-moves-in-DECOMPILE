using System;

// Token: 0x02000369 RID: 873
public class SoaringRenewPasswordModule : SoaringModule
{
	// Token: 0x060018ED RID: 6381 RVA: 0x000A4AF0 File Offset: 0x000A2CF0
	public override string ModuleName()
	{
		return "renewUserPassword";
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x000A4AF8 File Offset: 0x000A2CF8
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, null);
		text = text + ",\n\"data\" : " + callData.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, false);
	}

	// Token: 0x060018EF RID: 6383 RVA: 0x000A4B5C File Offset: 0x000A2D5C
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringInternal.Delegate.OnPasswordResetConfirmed(moduleData.state, moduleData.error);
	}
}
