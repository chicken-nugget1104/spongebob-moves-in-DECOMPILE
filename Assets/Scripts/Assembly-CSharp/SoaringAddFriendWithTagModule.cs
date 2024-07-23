using System;
using UnityEngine;

// Token: 0x02000354 RID: 852
public class SoaringAddFriendWithTagModule : SoaringModule
{
	// Token: 0x0600187A RID: 6266 RVA: 0x000A1FDC File Offset: 0x000A01DC
	public override string ModuleName()
	{
		return "requestFriendshipWithTag";
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x000A1FE4 File Offset: 0x000A01E4
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		if (data == null && callData == null)
		{
			this.AddingFriend = true;
		}
		else
		{
			this.AddingFriend = false;
		}
		if (!this.AddingFriend)
		{
			this.AuthToken = data.soaringValue("authToken");
			this.UserTag = callData.soaringValue("tag");
			soaringDictionary.addValue(this.AuthToken, "authToken");
			string text = "{\n" + SCQueueTools.CreateJsonMessage("action", "searchUsers", null, soaringDictionary);
			text += ",\n";
			soaringDictionary.clear();
			soaringDictionary.addValue("tag", "type");
			soaringDictionary.addValue(this.UserTag, "value");
			text = text + SCQueueTools.CreateJsonMessage("data", null, null, soaringDictionary) + "\n}";
			soaringDictionary.clear();
			soaringDictionary.addValue(text, "data");
		}
		else
		{
			soaringDictionary.addValue(this.AuthToken, "authToken");
			string text2 = "{\n" + SCQueueTools.CreateJsonMessage("action", "requestFriendship", null, soaringDictionary);
			text2 += ",\n";
			soaringDictionary.clear();
			soaringDictionary.addValue(this.UserID, "userId");
			text2 = text2 + SCQueueTools.CreateJsonMessage("data", null, null, soaringDictionary) + "\n}";
			soaringDictionary.clear();
			soaringDictionary.addValue(text2, "data");
		}
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, true);
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x000A2188 File Offset: 0x000A0388
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
				if (!this.AddingFriend)
				{
					this.AddingFriend = true;
					soaringDictionary = (SoaringDictionary)soaringDictionary.objectWithKey("data");
					SoaringArray data2 = (SoaringArray)soaringDictionary.objectWithKey("users");
					SoaringArray<SoaringUser> soaringArray = SCQueueTools.ParseUsers(data2, true);
					if (soaringArray == null)
					{
						this.HandleDelegateCallback(soaringModuleData.Set(false, "No Users Found", null, (SoaringContext)userData));
					}
					else if (soaringArray.count() == 0)
					{
						this.HandleDelegateCallback(soaringModuleData.Set(false, "No Users Found", null, (SoaringContext)userData));
					}
					else
					{
						SoaringUser soaringUser = soaringArray.objectAtIndex(0);
						this.UserID = soaringUser.UserID;
						this.CallModule(null, null, (SoaringContext)userData);
					}
				}
				else
				{
					this.HandleDelegateCallback(soaringModuleData.Set(true, null, soaringDictionary, (SoaringContext)userData));
					this.AddingFriend = false;
					Soaring.UpdateFriendsListWithLastSettings(null);
				}
			}
			else if (state == SCWebQueue.SCWebQueueState.Failed)
			{
				this.HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log("SoaringModule:" + this.ModuleName() + ": Error: " + ex.Message, LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			this.HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
			this.AddingFriend = false;
		}
		base.ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x000A235C File Offset: 0x000A055C
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnRequestFriend(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
	}

	// Token: 0x04001062 RID: 4194
	private bool AddingFriend;

	// Token: 0x04001063 RID: 4195
	private string AuthToken;

	// Token: 0x04001064 RID: 4196
	private string UserID;

	// Token: 0x04001065 RID: 4197
	private string UserTag;
}
