using System;

// Token: 0x0200036E RID: 878
public class SoaringRetrieveMessagesModule : SoaringModule
{
	// Token: 0x06001904 RID: 6404 RVA: 0x000A51B0 File Offset: 0x000A33B0
	public override string ModuleName()
	{
		return "retrieveUnreadMessages";
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x000A51B8 File Offset: 0x000A33B8
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary data2 = SCQueueTools.CreateMessage(this.ModuleName(), null, callData);
		base.PushCorePostDataToQueue(data2, 0, context, true);
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x000A51E0 File Offset: 0x000A33E0
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringArray soaringArray = null;
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		if (moduleData.state)
		{
			SoaringArray soaringArray2 = (SoaringArray)moduleData.data.objectWithKey("messages");
			if (soaringArray2 != null)
			{
				soaringArray = new SoaringArray();
				int num = soaringArray2.count();
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray2.objectAtIndex(i);
					SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("header");
					if (soaringDictionary2 != null)
					{
						SoaringMessage soaringMessage = new SoaringMessage();
						soaringMessage.SetCategory(soaringDictionary2.soaringValue("category"));
						soaringMessage.SetMessageID(soaringDictionary2.soaringValue("messageId"));
						soaringMessage.SetSenderID(soaringDictionary2.soaringValue("fromUserId"));
						soaringMessage.SetMessageSendData(soaringDictionary2.soaringValue("sentDate"));
						soaringMessage.SetTextBody(soaringDictionary.soaringValue("body"));
						soaringArray.addObject(soaringMessage);
					}
				}
			}
		}
		SoaringInternal.Delegate.OnCheckMessages(moduleData.state, moduleData.error, soaringArray);
	}
}
