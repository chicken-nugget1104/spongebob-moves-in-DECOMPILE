using System;

// Token: 0x02000375 RID: 885
public class SoaringSearchUsersModule : SoaringModule
{
	// Token: 0x06001929 RID: 6441 RVA: 0x000A6278 File Offset: 0x000A4478
	public override string ModuleName()
	{
		return "searchUsers";
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x000A6280 File Offset: 0x000A4480
	public override int ModuleChannel()
	{
		return 2;
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x000A6284 File Offset: 0x000A4484
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
		base.PushCorePostDataToQueue(soaringDictionary, this.ModuleChannel(), context, false);
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x000A632C File Offset: 0x000A452C
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringArray<SoaringUser> soaringArray = null;
		SoaringUser[] users = null;
		if (moduleData.state)
		{
			if (moduleData.data != null)
			{
				SoaringArray data = (SoaringArray)moduleData.data.objectWithKey("users");
				soaringArray = SCQueueTools.ParseUsers(data, true);
			}
			if (soaringArray == null)
			{
				moduleData.state = false;
			}
			else if (soaringArray.count() == 0)
			{
				moduleData.state = false;
			}
			else
			{
				users = soaringArray.array();
			}
		}
		SoaringInternal.Delegate.OnFindUser(moduleData.state, moduleData.error, users, moduleData.context);
	}
}
