using System;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class SoaringLoginUserModule : SoaringModule
{
	// Token: 0x060018B7 RID: 6327 RVA: 0x000A3890 File Offset: 0x000A1A90
	public override string ModuleName()
	{
		return "loginUser";
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x000A3898 File Offset: 0x000A1A98
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		this.userPassword = callData.soaringValue("password");
		this.userTag = callData.soaringValue("tag");
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

	// Token: 0x060018B9 RID: 6329 RVA: 0x000A396C File Offset: 0x000A1B6C
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
				soaringDictionary.setValue(this.userTag, "tag");
				soaringDictionary.setValue(this.userPassword, "password");
				SoaringInternal.instance.UpdatePlayerData(soaringDictionary, true);
				SoaringInternal.instance.SetSoaringInternalData(soaringDictionary);
				this.HandleDelegateCallback(soaringModuleData.Set(true, null, soaringDictionary, (SoaringContext)userData));
				Soaring.UpdateFriendsListWithLastSettings(null);
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
				"\nStack: ",
				ex.StackTrace
			}), LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			this.HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		base.ReturnModuledata(soaringModuleData);
		this.userPassword = null;
		this.userTag = null;
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x000A3B08 File Offset: 0x000A1D08
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.state)
		{
			SoaringPlayer.ValidCredentials = true;
			SoaringInternal.instance.Player.Save();
		}
		SoaringInternal.Delegate.OnAuthorize(moduleData.state, moduleData.error, Soaring.Player, moduleData.context);
		if (moduleData.state)
		{
			SoaringInternal.instance.RetrieveUserProfile(null, moduleData.context);
		}
	}

	// Token: 0x04001066 RID: 4198
	public string userPassword;

	// Token: 0x04001067 RID: 4199
	public string userTag;
}
