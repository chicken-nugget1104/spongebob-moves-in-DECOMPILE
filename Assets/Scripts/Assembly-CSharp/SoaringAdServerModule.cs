using System;
using UnityEngine;

// Token: 0x02000352 RID: 850
public class SoaringAdServerModule : SoaringModule
{
	// Token: 0x06001871 RID: 6257 RVA: 0x000A1BD8 File Offset: 0x0009FDD8
	public override string ModuleName()
	{
		return "adServer";
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x000A1BE0 File Offset: 0x0009FDE0
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

	// Token: 0x06001873 RID: 6259 RVA: 0x000A1CAC File Offset: 0x0009FEAC
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
				SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("data");
				if (soaringDictionary2 != null)
				{
					soaringDictionary = soaringDictionary2;
				}
				this.HandleDelegateCallback(soaringModuleData.Set(true, null, soaringDictionary, (SoaringContext)userData));
			}
			else if (state == SCWebQueue.SCWebQueueState.Failed)
			{
				SoaringDebug.Log("SoaringModule:" + this.ModuleName() + ": Error: Download Failed", LogType.Error);
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

	// Token: 0x06001874 RID: 6260 RVA: 0x000A1E04 File Offset: 0x000A0004
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		SoaringInternal.instance.AdServer.HandleAdRequestReturn(moduleData.data, moduleData.context);
	}
}
