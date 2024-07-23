using System;

// Token: 0x02000376 RID: 886
public class SoaringSendMessageModule : SoaringModule
{
	// Token: 0x0600192E RID: 6446 RVA: 0x000A63C8 File Offset: 0x000A45C8
	public override string ModuleName()
	{
		return "sendMessage";
	}

	// Token: 0x0600192F RID: 6447 RVA: 0x000A63D0 File Offset: 0x000A45D0
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(callData.soaringValue("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		soaringDictionary.clear();
		SoaringMessage soaringMessage = (SoaringMessage)callData.objectWithKey("messages");
		text = text + ",\n\"data\" : " + soaringMessage.ToJsonString() + "\n}";
		soaringDictionary.addValue(text, "data");
		context.addValue(soaringMessage, "message");
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, true);
	}

	// Token: 0x06001930 RID: 6448 RVA: 0x000A646C File Offset: 0x000A466C
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduledata)
	{
		SoaringMessage message = null;
		if (moduledata.context != null)
		{
			message = (SoaringMessage)moduledata.context.objectWithKey("message");
		}
		Soaring.Delegate.OnSendMessage(moduledata.state, moduledata.error, message);
	}
}
