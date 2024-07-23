using System;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class SoaringVersionSoaringModule : SoaringModule
{
	// Token: 0x06001940 RID: 6464 RVA: 0x000A6838 File Offset: 0x000A4A38
	public override string ModuleName()
	{
		return "retrieveVersions";
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x000A6840 File Offset: 0x000A4A40
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = callData.soaringValue("turl");
		SoaringDictionary soaringDictionary = new SoaringDictionary(4);
		if (string.IsNullOrEmpty(text))
		{
			SoaringDictionary soaringDictionary2 = SCQueueTools.CreateMessage(this.ModuleName(), data.soaringValue("gameId"), callData);
			if (soaringDictionary2 != null)
			{
				soaringDictionary.addValue(soaringDictionary2, "tposts");
			}
		}
		else
		{
			soaringDictionary.addValue(text, "turl");
			SoaringValue val = new SoaringValue(text);
			context.addValue(val, "turl");
		}
		soaringDictionary.addValue(0, "tchannel");
		soaringDictionary.addValue(new SCWebQueue.SCWebCallbackObject(new SCWebQueue.SCWebQueueCallback(this.Web_Callback)), "tcallback");
		soaringDictionary.addValue(context, "tobject");
		base.PushCallData(soaringDictionary, context);
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x000A690C File Offset: 0x000A4B0C
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
				this.HandleDelegateCallback(soaringModuleData.Set(true, null, soaringDictionary, (SoaringContext)userData));
			}
			else if (state == SCWebQueue.SCWebQueueState.Failed)
			{
				this.HandleDelegateCallback(soaringModuleData.Set(false, error, null, (SoaringContext)userData));
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(string.Concat(new string[]
			{
				"SoaringModule:",
				this.ModuleName(),
				": Error: ",
				ex.Message,
				"\n",
				ex.StackTrace
			}), LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			this.HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		base.ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x000A6A30 File Offset: 0x000A4C30
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		string source = null;
		string commit = null;
		long newVersion = -1L;
		SoaringArray soaringArray = null;
		SoaringArray diffs = null;
		if (moduleData.state && moduleData.error == null && moduleData.data != null)
		{
			SoaringDictionary data = moduleData.data;
			newVersion = data.soaringValue("version");
			commit = data.soaringValue("commit");
			source = data.soaringValue("source");
			soaringArray = (SoaringArray)data.objectWithKey("contents");
			if (SoaringInternal.Campaign != null && soaringArray != null)
			{
				diffs = (SoaringArray)data.objectWithKey(SoaringInternal.Campaign.Group, true);
			}
			SoaringDictionary soaringDictionary = (SoaringDictionary)data.objectWithKey(SoaringPlatform.PrimaryPlatformName, true);
			SoaringArray subContentCategories = SoaringInternal.instance.Versions.SubContentCategories;
			if (subContentCategories != null)
			{
				for (int i = 0; i < subContentCategories.count(); i++)
				{
					string key = subContentCategories.soaringValue(i);
					SoaringArray soaringArray2 = (SoaringArray)data.objectWithKey(key);
					if (soaringArray2 != null)
					{
						int num = soaringArray2.count();
						for (int j = 0; j < num; j++)
						{
							soaringArray.addObject(soaringArray2.objectAtIndex(i));
						}
					}
				}
				if (soaringDictionary != null)
				{
					for (int k = 0; k < subContentCategories.count(); k++)
					{
						string key2 = subContentCategories.soaringValue(k);
						SoaringArray soaringArray3 = (SoaringArray)soaringDictionary.objectWithKey(key2);
						if (soaringArray3 != null)
						{
							int num2 = soaringArray3.count();
							for (int l = 0; l < num2; l++)
							{
								soaringArray.addObject(soaringArray3.objectAtIndex(k));
							}
						}
					}
				}
			}
		}
		SoaringInternal.instance.Versions.AddFileVersions(soaringArray, diffs, newVersion, source, commit);
	}
}
