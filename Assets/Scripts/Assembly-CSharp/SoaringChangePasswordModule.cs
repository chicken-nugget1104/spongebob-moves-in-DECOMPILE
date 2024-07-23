using System;

// Token: 0x02000358 RID: 856
public class SoaringChangePasswordModule : SoaringModule
{
	// Token: 0x0600188E RID: 6286 RVA: 0x000A27B8 File Offset: 0x000A09B8
	public override string ModuleName()
	{
		return "changeUserPassword";
	}

	// Token: 0x0600188F RID: 6287 RVA: 0x000A27C0 File Offset: 0x000A09C0
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringValue val = callData.soaringValue("newPassword");
		SoaringValue val2 = callData.soaringValue("oldPassword");
		callData.removeObjectWithKey("newPassword");
		callData.removeObjectWithKey("oldPassword");
		callData.removeObjectWithKey("gameId");
		context.addValue(val, "password");
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(val, "newPassword");
		soaringDictionary.addValue(val2, "oldPassword");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, callData);
		text = text + ",\n\"data\" : " + soaringDictionary.ToJsonString() + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 1, context, true);
	}

	// Token: 0x06001890 RID: 6288 RVA: 0x000A2888 File Offset: 0x000A0A88
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData data)
	{
		if (data.context != null && data.state)
		{
			SoaringDictionary soaringDictionary = new SoaringDictionary();
			soaringDictionary.addValue(data.context.objectWithKey("password"), "password");
			SoaringInternal.instance.UpdatePlayerData(soaringDictionary, false);
		}
		SoaringInternal.Delegate.OnPasswordChanged(data.state, data.error, data.context);
	}
}
