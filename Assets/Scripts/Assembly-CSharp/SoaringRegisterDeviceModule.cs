using System;
using UnityEngine;

// Token: 0x02000366 RID: 870
public class SoaringRegisterDeviceModule : SoaringModule
{
	// Token: 0x060018E0 RID: 6368 RVA: 0x000A44F4 File Offset: 0x000A26F4
	public override string ModuleName()
	{
		return "registerDevice";
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x000A44FC File Offset: 0x000A26FC
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
		base.PushCorePostDataToQueue(soaringDictionary, 2, context, false);
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x000A45A0 File Offset: 0x000A27A0
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.context != null && moduleData.state)
		{
			try
			{
				string value = moduleData.context.soaringValue("trToken");
				if (!string.IsNullOrEmpty(value))
				{
					PlayerPrefs.SetString("trToken", value);
				}
			}
			catch (Exception ex)
			{
				SoaringDebug.Log("RegisterDevice" + ex.Message);
			}
		}
		SoaringInternal.Delegate.OnDeviceRegistered(moduleData.state, moduleData.error, moduleData.context);
	}
}
