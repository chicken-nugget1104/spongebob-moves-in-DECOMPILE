using System;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class SoaringCreateUserModule : SoaringModule
{
	// Token: 0x06001896 RID: 6294 RVA: 0x000A2A0C File Offset: 0x000A0C0C
	public override string ModuleName()
	{
		return "registerUser";
	}

	// Token: 0x06001897 RID: 6295 RVA: 0x000A2A14 File Offset: 0x000A0C14
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		context.addValue(callData.soaringValue("password"), "password");
		context.addValue(callData.soaringValue("tag"), "tag");
		SoaringObjectBase soaringObjectBase = callData.objectWithKey("tregister");
		if (soaringObjectBase != null)
		{
			context.addValue(soaringObjectBase, "tregister");
			callData.removeObjectWithKey("tregister");
		}
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
		base.PushCorePostDataToQueue(soaringDictionary2, 0, context, true);
	}

	// Token: 0x06001898 RID: 6296 RVA: 0x000A2B14 File Offset: 0x000A0D14
	protected override bool Web_Callback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		if (state == SCWebQueue.SCWebQueueState.Updated)
		{
			return true;
		}
		SoaringModule.SoaringModuleData soaringModuleData = base.CreateModuleData();
		try
		{
			SoaringDictionary soaringDictionary = null;
			if (error == null)
			{
				error = SCQueueTools.CheckAndHandleError((string)data, ref soaringDictionary);
				if (error != null || soaringDictionary == null)
				{
					state = SCWebQueue.SCWebQueueState.Failed;
				}
				else
				{
					soaringDictionary = this.DecryptCall(soaringDictionary);
				}
			}
			SoaringDebug.Log((string)data);
			if (state == SCWebQueue.SCWebQueueState.Finished)
			{
				soaringDictionary = (SoaringDictionary)soaringDictionary.objectWithKey("data");
				SoaringContext soaringContext = (SoaringContext)userData;
				string text = soaringContext.soaringValue("tag");
				string text2 = soaringContext.soaringValue("password");
				string text3 = soaringDictionary.soaringValue("newtag");
				if (!string.IsNullOrEmpty(text3))
				{
					text = text3;
				}
				if (!string.IsNullOrEmpty(text))
				{
					soaringDictionary.setValue(text, "tag");
				}
				if (!string.IsNullOrEmpty(text2))
				{
					soaringDictionary.setValue(text2, "password");
				}
				SoaringInternal.instance.UpdatePlayerData(soaringDictionary, true);
				this.HandleDelegateCallback(soaringModuleData.Set(true, null, soaringDictionary, soaringContext));
			}
			else if (state == SCWebQueue.SCWebQueueState.Failed)
			{
				this.HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(string.Concat(new string[]
			{
				"SoaringModule:",
				this.ModuleName(),
				": Error: ",
				ex.Message,
				" Stack: ",
				ex.StackTrace
			}), LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			this.HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		base.ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	// Token: 0x06001899 RID: 6297 RVA: 0x000A2CE8 File Offset: 0x000A0EE8
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringObjectBase soaringObjectBase = null;
		if (moduleData.data != null)
		{
			SoaringContext context = moduleData.context;
			if (context != null)
			{
				soaringObjectBase = context.objectWithKey("tregister");
			}
		}
		else
		{
			moduleData.state = false;
		}
		if (soaringObjectBase == null)
		{
			SoaringPlayer.ValidCredentials = moduleData.state;
			SoaringInternal.Delegate.OnRegisterUser(moduleData.state, moduleData.error, Soaring.Player, moduleData.context);
		}
		else
		{
			SoaringPlayer.ValidCredentials = true;
			SoaringInternal.instance.GenerateInviteCode();
			SoaringInternal.instance.HandleLogin(SoaringLoginType.Soaring, moduleData.state, moduleData.error, moduleData.data, moduleData.context);
		}
	}
}
