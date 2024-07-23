using System;

// Token: 0x02000365 RID: 869
public class SoaringRedeemRewardModule : SoaringModule
{
	// Token: 0x060018DC RID: 6364 RVA: 0x000A4420 File Offset: 0x000A2620
	public override string ModuleName()
	{
		return "tearVirtualGoodCoupons";
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x000A4428 File Offset: 0x000A2628
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		text += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 1, context, true);
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x000A44CC File Offset: 0x000A26CC
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringInternal.Delegate.OnRedeemUserReward(moduleData.state, moduleData.error, moduleData.data);
	}
}
