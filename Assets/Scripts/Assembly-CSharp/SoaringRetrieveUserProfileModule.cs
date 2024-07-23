using System;

// Token: 0x02000372 RID: 882
public class SoaringRetrieveUserProfileModule : SoaringModule
{
	// Token: 0x06001919 RID: 6425 RVA: 0x000A5BEC File Offset: 0x000A3DEC
	public override string ModuleName()
	{
		return "retrieveUserProfile";
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x000A5BF4 File Offset: 0x000A3DF4
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = callData.soaringValue("userId");
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text2 = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		text2 += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text2 = text2 + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text2, "data");
		if (!string.IsNullOrEmpty(text))
		{
			context.addValue(text, "userId");
		}
		context.addValue(Soaring.Player.AuthToken, "authToken");
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, false);
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x000A5CE0 File Offset: 0x000A3EE0
	public override bool VerifyCallStillValid(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		if (userData == null)
		{
			return true;
		}
		SoaringContext soaringContext = (SoaringContext)userData;
		string text = soaringContext.soaringValue("authToken");
		return string.IsNullOrEmpty(text) || Soaring.Player.AuthToken == text;
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x000A5D2C File Offset: 0x000A3F2C
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringUser soaringUser = null;
		if (moduleData.data != null)
		{
			soaringUser = new SoaringUser();
			soaringUser.SetUserData(moduleData.data);
		}
		if (this.VerifyCallStillValid(SCWebQueue.SCWebQueueState.Finished, moduleData.error, moduleData.context, null) && moduleData.context.objectWithKey("userId") == null)
		{
			SoaringInternal.instance.UpdatePlayerData(moduleData.data);
		}
		SoaringInternal.Delegate.OnRetrieveUserProfile(moduleData.state, moduleData.error, soaringUser, moduleData.context);
	}
}
