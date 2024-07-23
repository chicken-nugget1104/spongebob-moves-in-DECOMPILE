using System;

// Token: 0x02000371 RID: 881
public class SoaringRetrieveSessionModule : SoaringModule
{
	// Token: 0x06001915 RID: 6421 RVA: 0x000A58E0 File Offset: 0x000A3AE0
	public override string ModuleName()
	{
		return "retrieveGameSession";
	}

	// Token: 0x06001916 RID: 6422 RVA: 0x000A58E8 File Offset: 0x000A3AE8
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		text += ",\n";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text += "\"data\":{";
		bool flag = false;
		SoaringDictionary soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("query");
		if (soaringDictionary2 != null)
		{
			text = text + "\n\"query\":" + soaringDictionary2.ToJsonString();
			flag = true;
		}
		soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("sort");
		if (soaringDictionary2 != null)
		{
			text = text + ((!flag) ? "\n" : ",\n") + "\"sort\":" + soaringDictionary2.ToJsonString();
			flag = true;
		}
		SoaringArray soaringArray = (SoaringArray)callData.objectWithKey("identifiers");
		if (soaringArray != null)
		{
			text = text + ((!flag) ? "\n" : ",\n") + "\"identifiers\":" + soaringArray.ToJsonString();
			flag = true;
		}
		string text2 = callData.soaringValue("queryType");
		if (text2 != null)
		{
			string text3 = text;
			text = string.Concat(new string[]
			{
				text3,
				(!flag) ? "\n" : ",\n",
				"\"queryType\":\"",
				text2,
				"\""
			});
			flag = true;
		}
		text2 = callData.soaringValue("sessionType");
		if (!string.IsNullOrEmpty(text2))
		{
			string text3 = text;
			text = string.Concat(new string[]
			{
				text3,
				(!flag) ? "\n" : ",\n",
				"\"sessionType\":\"",
				text2,
				"\""
			});
		}
		text += "\n}\n}\n";
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 1, context, false);
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x000A5AF8 File Offset: 0x000A3CF8
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		SoaringArray soaringArray = null;
		if (moduleData.data != null)
		{
			SoaringArray soaringArray2 = (SoaringArray)moduleData.data.objectWithKey("sessions");
			if (soaringArray2 != null)
			{
				int num = soaringArray2.count();
				soaringArray = new SoaringArray(num);
				for (int i = 0; i < num; i++)
				{
					SoaringDictionary soaringDictionary = (SoaringDictionary)soaringArray2.objectAtIndex(i);
					SoaringObjectBase soaringObjectBase = soaringDictionary.objectWithKey("custom");
					if (soaringObjectBase != null)
					{
						soaringArray.addObject(soaringObjectBase);
					}
				}
			}
			SoaringObjectBase soaringObjectBase2 = moduleData.data.objectWithKey("custom");
			if (soaringObjectBase2 != null)
			{
				if (soaringObjectBase2.Type == SoaringObjectBase.IsType.Array)
				{
					soaringArray = (SoaringArray)soaringObjectBase2;
				}
				else
				{
					if (soaringArray != null)
					{
						soaringArray = new SoaringArray(1);
					}
					soaringArray.addObject(soaringObjectBase2);
				}
			}
		}
		SoaringInternal.Delegate.OnRequestingSessionData(moduleData.state, moduleData.error, soaringArray, moduleData.data, moduleData.context);
	}
}
