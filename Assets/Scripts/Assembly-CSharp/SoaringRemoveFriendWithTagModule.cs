using System;
using UnityEngine;

// Token: 0x02000368 RID: 872
public class SoaringRemoveFriendWithTagModule : SoaringModule
{
	// Token: 0x060018E8 RID: 6376 RVA: 0x000A4738 File Offset: 0x000A2938
	public override string ModuleName()
	{
		return "removeFriendshipWithTag";
	}

	// Token: 0x060018E9 RID: 6377 RVA: 0x000A4740 File Offset: 0x000A2940
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
			string text2 = "{\n" + SCQueueTools.CreateJsonMessage("action", "removeFriendship", null, soaringDictionary);
			text2 += ",\n";
			soaringDictionary.clear();
			soaringDictionary.addValue(this.UserID, "userId");
			text2 = text2 + SCQueueTools.CreateJsonMessage("data", null, null, soaringDictionary) + "\n}";
			soaringDictionary.clear();
			soaringDictionary.addValue(text2, "data");
		}
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, true);
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x000A48E4 File Offset: 0x000A2AE4
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

	// Token: 0x060018EB RID: 6379 RVA: 0x000A4AB8 File Offset: 0x000A2CB8
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnRemoveFriend(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
	}

	// Token: 0x0400106E RID: 4206
	private bool AddingFriend;

	// Token: 0x0400106F RID: 4207
	private string AuthToken;

	// Token: 0x04001070 RID: 4208
	private string UserID;

	// Token: 0x04001071 RID: 4209
	private string UserTag;
}
