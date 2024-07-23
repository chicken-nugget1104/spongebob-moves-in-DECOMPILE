using System;

// Token: 0x02000379 RID: 889
public class SoaringVerifyServerRecieptModule : SoaringModule
{
	// Token: 0x0600193B RID: 6459 RVA: 0x000A6720 File Offset: 0x000A4920
	public override string ModuleName()
	{
		return "validateIapReceipt";
	}

	// Token: 0x0600193C RID: 6460 RVA: 0x000A6728 File Offset: 0x000A4928
	public override int ModuleChannel()
	{
		return 0;
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x000A672C File Offset: 0x000A492C
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		callData.removeObjectWithKey("gameId");
		callData.removeObjectWithKey("authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		text += ",\n";
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, this.ModuleChannel(), context, false);
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x000A67D4 File Offset: 0x000A49D4
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		bool success = false;
		if (moduleData.error == null && moduleData.data != null)
		{
			SoaringValue b = moduleData.data.soaringValue("valid");
			if (b)
			{
				success = true;
			}
		}
		Soaring.Delegate.OnRecieptValidated(success, moduleData.error, moduleData.context);
	}
}
