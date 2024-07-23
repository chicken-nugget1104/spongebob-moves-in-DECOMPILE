using System;
using UnityEngine;

// Token: 0x02000357 RID: 855
public class SoaringApplyInviteCodeModule : SoaringModule
{
	// Token: 0x06001889 RID: 6281 RVA: 0x000A2588 File Offset: 0x000A0788
	public override string ModuleName()
	{
		return "applyInvitationCode";
	}

	// Token: 0x0600188A RID: 6282 RVA: 0x000A2590 File Offset: 0x000A0790
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
		base.PushCorePostDataToQueue(soaringDictionary, 2, context, true);
	}

	// Token: 0x0600188B RID: 6283 RVA: 0x000A2634 File Offset: 0x000A0834
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
			}
			SoaringDebug.Log((string)data);
			if (state == SCWebQueue.SCWebQueueState.Finished)
			{
				SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("data");
				SoaringDictionary soaringDictionary3 = (SoaringDictionary)soaringDictionary2.objectWithKey("profile");
				string userId = soaringDictionary3.soaringValue("userId");
				this.HandleDelegateCallback(soaringModuleData.Set(true, null, soaringDictionary2, (SoaringContext)userData));
				SoaringInternal.instance.RequestFriendship(null, null, userId, (SoaringContext)userData);
			}
			else if (state == SCWebQueue.SCWebQueueState.Failed)
			{
				this.HandleDelegateCallback(soaringModuleData.Set(false, null, null, (SoaringContext)userData));
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + this.ModuleName() + ": Error: " + ex.Message, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			this.HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		base.ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	// Token: 0x0600188C RID: 6284 RVA: 0x000A2780 File Offset: 0x000A0980
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringInternal.Delegate.OnApplyInviteCode(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
	}
}
