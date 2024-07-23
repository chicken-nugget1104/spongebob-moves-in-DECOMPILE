using System;

// Token: 0x02000361 RID: 865
public class SoaringLookupUserModule : SoaringModule
{
	// Token: 0x060018BC RID: 6332 RVA: 0x000A3B7C File Offset: 0x000A1D7C
	public override bool ShouldEncryptCall()
	{
		return false;
	}

	// Token: 0x060018BD RID: 6333 RVA: 0x000A3B80 File Offset: 0x000A1D80
	public override string ModuleName()
	{
		return "lookupUser";
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x000A3B88 File Offset: 0x000A1D88
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = (SoaringDictionary)callData.objectWithKey("custom");
		if (soaringDictionary != null)
		{
			callData.removeObjectWithKey("custom");
		}
		callData.addValue(data.objectWithKey("gameId"), "gameId");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, callData);
		if (soaringDictionary != null)
		{
			text = text + ",\n\"data\":" + soaringDictionary.ToJsonString();
		}
		text += "\n}";
		SoaringDictionary soaringDictionary2 = new SoaringDictionary(2);
		soaringDictionary2.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary2, 1, context, true);
	}

	// Token: 0x060018BF RID: 6335 RVA: 0x000A3C30 File Offset: 0x000A1E30
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		bool success = moduleData.error == null;
		Soaring.Delegate.OnLookupUser(success, moduleData.error, moduleData.context);
	}
}
