using System;

// Token: 0x02000355 RID: 853
public class SoaringAddressKeeperModule : SoaringModule
{
	// Token: 0x0600187F RID: 6271 RVA: 0x000A2394 File Offset: 0x000A0594
	public override string ModuleName()
	{
		return "retrieveGameLinks";
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x000A239C File Offset: 0x000A059C
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary data2 = SCQueueTools.CreateMessage(this.ModuleName(), data.soaringValue("gameId"), callData);
		base.PushCorePostDataToQueue(data2, 0, context, false);
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x000A23D0 File Offset: 0x000A05D0
	public override bool ShouldEncryptCall()
	{
		return false;
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x000A23D4 File Offset: 0x000A05D4
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.data != null)
		{
			SoaringObjectBase soaringObjectBase = moduleData.data.objectWithKey("links");
			if (soaringObjectBase.Type == SoaringObjectBase.IsType.Array)
			{
				SoaringArray soaringArray = (SoaringArray)soaringObjectBase;
				int num = soaringArray.count();
				moduleData.data = new SoaringDictionary(num);
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray.objectAtIndex(i);
					string text = soaringDictionary.soaringValue("key");
					string text2 = soaringDictionary.soaringValue("url");
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
					{
						moduleData.data.addValue(text2, text);
					}
				}
			}
		}
		SoaringInternal.instance.AddressesKeeper.SetSoaringAddressData(moduleData.data);
	}
}
