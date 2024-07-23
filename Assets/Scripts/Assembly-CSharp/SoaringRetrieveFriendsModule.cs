using System;
using UnityEngine;

// Token: 0x0200036D RID: 877
public class SoaringRetrieveFriendsModule : SoaringModule
{
	// Token: 0x060018FF RID: 6399 RVA: 0x000A4DDC File Offset: 0x000A2FDC
	public override string ModuleName()
	{
		return "retrieveFriendsList";
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x000A4DE4 File Offset: 0x000A2FE4
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
		if ((SoaringArray)context.objectWithKey("_FriendList") == null)
		{
			context.addValue(new SoaringArray(), "_FriendList");
		}
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, true);
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x000A4EB0 File Offset: 0x000A30B0
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
				soaringDictionary = (SoaringDictionary)soaringDictionary.objectWithKey("data");
				this.HandleDelegateCallback(soaringModuleData.Set(true, null, soaringDictionary, (SoaringContext)userData));
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
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x000A4FE8 File Offset: 0x000A31E8
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringArray<SoaringUser> soaringArray = null;
		int num = 0;
		int num2 = 0;
		SoaringArray soaringArray2 = null;
		if (moduleData.data != null)
		{
			SoaringArray soaringArray3 = (SoaringArray)moduleData.data.objectWithKey("friends");
			num = moduleData.data.soaringValue("friendsCount");
			if (soaringArray3 != null)
			{
				soaringArray = SCQueueTools.ParseUsers(soaringArray3, false);
			}
		}
		if (soaringArray != null)
		{
			num2 = soaringArray.count();
		}
		else
		{
			soaringArray = new SoaringArray<SoaringUser>();
		}
		if (moduleData.context != null)
		{
			soaringArray2 = (SoaringArray)moduleData.context.objectWithKey("_FriendList");
		}
		if (soaringArray2 == null)
		{
			soaringArray2 = new SoaringArray();
		}
		for (int i = 0; i < num2; i++)
		{
			SoaringUser obj = soaringArray[i];
			soaringArray2.addObject(obj);
		}
		if (soaringArray2.count() < num && num != 0)
		{
			Soaring.UpdateFriendsListWithLastSettings(soaringArray2.count(), num, moduleData.context);
		}
		else
		{
			int num3 = 0;
			int num4 = soaringArray2.count();
			for (int j = 0; j < num4; j++)
			{
				SoaringUser soaringUser = (SoaringUser)soaringArray2.objectAtIndex(j);
				if (soaringUser != null)
				{
					num3++;
				}
			}
			SoaringArray<SoaringUser> soaringArray4 = new SoaringArray<SoaringUser>(num3);
			for (int k = 0; k < num4; k++)
			{
				SoaringUser soaringUser2 = (SoaringUser)soaringArray2.objectAtIndex(k);
				if (soaringUser2 != null)
				{
					soaringArray4.addObject(soaringUser2);
				}
			}
			SoaringUser[] array = soaringArray4.array();
			if (array == null)
			{
				array = new SoaringUser[0];
			}
			SoaringInternal.instance.Player.SetFriendsData(soaringArray4);
			SoaringInternal.Delegate.OnUpdateFriendList(moduleData.state, moduleData.error, array, moduleData.context);
		}
	}

	// Token: 0x04001072 RID: 4210
	private const string kFriendList = "_FriendList";
}
