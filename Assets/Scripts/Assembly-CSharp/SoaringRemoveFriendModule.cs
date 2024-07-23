using System;

// Token: 0x02000367 RID: 871
public class SoaringRemoveFriendModule : SoaringModule
{
	// Token: 0x060018E4 RID: 6372 RVA: 0x000A4650 File Offset: 0x000A2850
	public override string ModuleName()
	{
		return "removeFriendship";
	}

	// Token: 0x060018E5 RID: 6373 RVA: 0x000A4658 File Offset: 0x000A2858
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(callData.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", "removeFriendship", null, soaringDictionary);
		text += ",\n";
		soaringDictionary.clear();
		soaringDictionary.addValue(callData.objectWithKey("userId"), "userId");
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, soaringDictionary) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, true);
	}

	// Token: 0x060018E6 RID: 6374 RVA: 0x000A4700 File Offset: 0x000A2900
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnRemoveFriend(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
	}
}
