using System;

// Token: 0x02000353 RID: 851
public class SoaringAddFriendModule : SoaringModule
{
	// Token: 0x06001876 RID: 6262 RVA: 0x000A1E3C File Offset: 0x000A003C
	public override string ModuleName()
	{
		return "requestFriendship";
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x000A1E44 File Offset: 0x000A0044
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		text += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		SoaringObjectBase soaringObjectBase = callData.objectWithKey("userId");
		if (soaringObjectBase != null && soaringObjectBase.Type == SoaringObjectBase.IsType.Array)
		{
			SoaringArray soaringArray = (SoaringArray)soaringObjectBase;
			callData.setValue(soaringArray.objectAtIndex(0), "userId");
			soaringArray.removeObjectAtIndex(0);
			if (soaringArray.count() > 0)
			{
				if (context == null)
				{
					context = new SoaringContext();
				}
				context.setValue(soaringArray, "userIds");
			}
		}
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 2, context, true);
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x000A1F4C File Offset: 0x000A014C
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.state && moduleData.context != null)
		{
			SoaringArray soaringArray = (SoaringArray)moduleData.context.objectWithKey("userIds");
			if (soaringArray != null && soaringArray.count() > 0)
			{
				SoaringInternal.instance.RequestFriendships(soaringArray, moduleData.context);
				return;
			}
		}
		SoaringInternal.Delegate.OnRequestFriend(moduleData.state, moduleData.error, moduleData.data, moduleData.context);
		Soaring.UpdateFriendsListWithLastSettings(null);
	}
}
