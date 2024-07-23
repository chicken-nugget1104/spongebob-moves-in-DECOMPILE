using System;

// Token: 0x02000362 RID: 866
public class SoaringMarkMessageReadModule : SoaringModule
{
	// Token: 0x060018C1 RID: 6337 RVA: 0x000A3C68 File Offset: 0x000A1E68
	public override string ModuleName()
	{
		return "markMessagesAsRead";
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x000A3C70 File Offset: 0x000A1E70
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		callData.removeObjectWithKey("gameId");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		text += ",\n";
		SoaringArray soaringArray = (SoaringArray)callData.objectWithKey("messages");
		if (soaringArray != null)
		{
			callData.removeObjectWithKey("messages");
			callData.addValue(soaringArray, "messageIds");
		}
		callData.removeObjectWithKey("authToken");
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, true);
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x000A3D44 File Offset: 0x000A1F44
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnMessageStateChanged(moduleData.state, moduleData.error, moduleData.data);
	}
}
