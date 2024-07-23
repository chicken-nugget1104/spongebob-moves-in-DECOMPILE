using System;

// Token: 0x02000378 RID: 888
public class SoaringUpdateUserModule : SoaringModule
{
	// Token: 0x06001936 RID: 6454 RVA: 0x000A6540 File Offset: 0x000A4740
	public override string ModuleName()
	{
		return "updateUserProfile";
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x000A6548 File Offset: 0x000A4748
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n\"action\" : {\n\"name\":\"" + this.ModuleName() + "\",\n";
		text = text + "\"authToken\":\"" + callData.soaringValue("authToken") + "\"\n},";
		text += "\"data\" : ";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		SoaringArray soaringArray = (SoaringArray)callData.objectWithKey("emails");
		if (soaringArray != null)
		{
			context.addValue(soaringArray, "emails");
		}
		text += callData.ToJsonString();
		text += "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		context.addValue(Soaring.Player.AuthToken, "authToken");
		base.PushCorePostDataToQueue(soaringDictionary, 1, context, false);
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x000A6628 File Offset: 0x000A4828
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

	// Token: 0x06001939 RID: 6457 RVA: 0x000A6674 File Offset: 0x000A4874
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.state && moduleData.data != null)
		{
			if (moduleData.context != null)
			{
				SoaringObjectBase soaringObjectBase = moduleData.context.objectWithKey("emails");
				if (soaringObjectBase != null)
				{
					moduleData.data.addValue(soaringObjectBase, "emails");
				}
			}
			if (this.VerifyCallStillValid(SCWebQueue.SCWebQueueState.Finished, moduleData.error, moduleData.context, null))
			{
				SoaringInternal.instance.UpdatePlayerData(moduleData.data, false);
			}
		}
		SoaringInternal.Delegate.OnUpdatingUserProfile(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
	}
}
