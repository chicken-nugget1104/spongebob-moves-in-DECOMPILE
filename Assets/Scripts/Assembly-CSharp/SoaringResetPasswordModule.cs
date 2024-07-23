using System;

// Token: 0x0200036A RID: 874
public class SoaringResetPasswordModule : SoaringModule
{
	// Token: 0x060018F1 RID: 6385 RVA: 0x000A4B7C File Offset: 0x000A2D7C
	public override string ModuleName()
	{
		return "resetUserPassword";
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x000A4B84 File Offset: 0x000A2D84
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, null);
		text = text + ",\n\"data\" : " + callData.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, false);
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x000A4BE8 File Offset: 0x000A2DE8
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringInternal.Delegate.OnPasswordReset(moduleData.state, moduleData.error);
	}
}
