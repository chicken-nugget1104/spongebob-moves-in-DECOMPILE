using System;

// Token: 0x0200035D RID: 861
public class SoaringFireEventModule : SoaringModule
{
	// Token: 0x060018AA RID: 6314 RVA: 0x000A3448 File Offset: 0x000A1648
	public override string ModuleName()
	{
		return "fireEvent";
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x000A3450 File Offset: 0x000A1650
	public override int ModuleChannel()
	{
		return 3;
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x000A3454 File Offset: 0x000A1654
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		callData.removeObjectWithKey("gameId");
		callData.removeObjectWithKey("authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		text += ",\n";
		text = text + SCQueueTools.CreateJsonMessage("data", null, null, callData) + "\n}";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, this.ModuleChannel(), context, false);
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x000A34FC File Offset: 0x000A16FC
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.state && moduleData.data != null)
		{
			SoaringArray soaringArray = (SoaringArray)moduleData.data.objectWithKey("events");
			if (soaringArray != null)
			{
				SoaringInternal.instance.Events.LoadEvents(soaringArray);
			}
			SoaringDictionary soaringDictionary = (SoaringDictionary)moduleData.data.objectWithKey("ev");
			if (soaringDictionary != null)
			{
				SoaringArray soaringArray2 = new SoaringArray(1);
				soaringArray2.addObject(soaringDictionary);
				SoaringInternal.instance.Events.LoadEvents(soaringArray2);
			}
		}
	}
}
