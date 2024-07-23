using System;

// Token: 0x02000373 RID: 883
public class SoaringSaveSessionModule : SoaringModule
{
	// Token: 0x0600191E RID: 6430 RVA: 0x000A5DBC File Offset: 0x000A3FBC
	public override string ModuleName()
	{
		return "saveGameSession";
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x000A5DC4 File Offset: 0x000A3FC4
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		text += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		soaringDictionary.clear();
		text += "\"data\":";
		SoaringDictionary soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("custom");
		if (soaringDictionary2 != null)
		{
			soaringDictionary.addValue(soaringDictionary2, "custom");
			string text2 = callData.soaringValue("sessionType");
			if (text2 == SoaringSession.GetSoaringSessionTypeString(SoaringSession.SessionType.PersistantOneWay))
			{
				this.mIsPersistantSession = true;
			}
			soaringDictionary.addValue(text2, "sessionType");
		}
		SoaringObjectBase soaringObjectBase = callData.objectWithKey("gameSessionId");
		if (soaringObjectBase != null)
		{
			soaringDictionary.addValue(soaringObjectBase, "gameSessionId");
		}
		else
		{
			soaringObjectBase = callData.objectWithKey("label");
			if (soaringObjectBase != null)
			{
				soaringDictionary.addValue(soaringObjectBase, "label");
			}
		}
		text += soaringDictionary.ToJsonString();
		text += "}\n";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		context.addValue(Soaring.Player.AuthToken, "authToken");
		this.PushDataToQueue(soaringDictionary, this.ModuleChannel(), context);
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x000A5F3C File Offset: 0x000A413C
	protected void PushDataToQueue(SoaringDictionary data, int channel, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(5);
		if (data != null)
		{
			soaringDictionary.addValue(data, "tposts");
		}
		soaringDictionary.addValue(channel, "tchannel");
		SCWebQueue.SCWebCallbackObject val = new SCWebQueue.SCWebCallbackObject(new SCWebQueue.SCWebQueueCallback(this.Web_Callback));
		soaringDictionary.addValue(val, "tcallback");
		if (context.containsKey("trIOff"))
		{
			soaringDictionary.addValue(true, "trIOff");
		}
		val = new SCWebQueue.SCWebCallbackObject(new SCWebQueue.SCWebQueueCallback(this.VerifyCallStillValid));
		soaringDictionary.addValue(val, "tvcallback");
		if (context != null)
		{
			soaringDictionary.addValue(context, "tobject");
		}
		base.PushCallData(soaringDictionary, context);
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x000A5FEC File Offset: 0x000A41EC
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

	// Token: 0x06001922 RID: 6434 RVA: 0x000A6038 File Offset: 0x000A4238
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		bool flag = this.VerifyCallStillValid(SCWebQueue.SCWebQueueState.Finished, moduleData.error, moduleData.context, moduleData.data);
		if (moduleData.state && !flag)
		{
			moduleData.state = flag;
		}
		SoaringDictionary soaringDictionary = null;
		if (this.mIsPersistantSession && moduleData.state)
		{
			bool flag2 = false;
			soaringDictionary = new SoaringDictionary();
			SoaringDictionary soaringDictionary2 = Soaring.Player.CustomData;
			if (soaringDictionary2 == null)
			{
				soaringDictionary2 = new SoaringDictionary();
				flag2 = true;
			}
			SoaringDictionary soaringDictionary3 = (SoaringDictionary)soaringDictionary2.objectWithKey("public");
			if (soaringDictionary3 == null)
			{
				soaringDictionary3 = new SoaringDictionary();
				soaringDictionary2.addValue(soaringDictionary3, "public");
				flag2 = true;
			}
			string text = moduleData.data.soaringValue("gameSessionId");
			if (text != null)
			{
				soaringDictionary3.setValue(text, "gameSessionId");
			}
			if (flag2)
			{
				soaringDictionary = new SoaringDictionary();
				soaringDictionary.addValue(soaringDictionary2, "custom");
				Soaring.Player.SetUserData(soaringDictionary);
			}
		}
		SoaringInternal.Delegate.OnSavingSessionData(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
		if (this.mIsPersistantSession && soaringDictionary != null && moduleData.state)
		{
			Soaring.UpdateUserProfile(null, Soaring.Player.CustomData, moduleData.context);
		}
		this.mIsPersistantSession = false;
	}

	// Token: 0x04001074 RID: 4212
	private bool mIsPersistantSession;
}
