using System;

// Token: 0x0200036C RID: 876
public class SoaringRetrieveABCampaigndModule : SoaringModule
{
	// Token: 0x060018FB RID: 6395 RVA: 0x000A4C7C File Offset: 0x000A2E7C
	public override string ModuleName()
	{
		return "retrieveABCampaignData";
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x000A4C84 File Offset: 0x000A2E84
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string b = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(b, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 1, context, false);
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x000A4CF4 File Offset: 0x000A2EF4
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringArray soaringArray = null;
		if (moduleData.state && moduleData.data != null)
		{
			soaringArray = (SoaringArray)moduleData.data.objectWithKey("campaigns");
			if (soaringArray != null)
			{
				bool flag = false;
				int num = soaringArray.count();
				SoaringArray soaringArray2 = new SoaringArray(num);
				for (int i = 0; i < num; i++)
				{
					SoaringCampaign soaringCampaign = new SoaringCampaign((SoaringDictionary)soaringArray.objectAtIndex(i));
					if (soaringCampaign.CampaignType == "content")
					{
						SoaringInternal.Campaign = soaringCampaign;
						flag = true;
					}
					soaringArray2.addObject(soaringCampaign);
				}
				soaringArray = soaringArray2;
				if (!flag && num != 0)
				{
					SoaringInternal.Campaign = (SoaringCampaign)soaringArray.objectAtIndex(0);
				}
			}
		}
		SoaringInternal.Delegate.OnRetrieveCampaign(moduleData.state, moduleData.error, soaringArray, moduleData.context);
	}
}
