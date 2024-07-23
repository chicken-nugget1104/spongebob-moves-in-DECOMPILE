using System;

// Token: 0x0200035C RID: 860
public class SoaringDownloadModule : SoaringModule
{
	// Token: 0x060018A5 RID: 6309 RVA: 0x000A3204 File Offset: 0x000A1404
	public override string ModuleName()
	{
		return "downloadFiles";
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x000A320C File Offset: 0x000A140C
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(4);
		string text = callData.soaringValue("turl");
		if (!string.IsNullOrEmpty(text))
		{
			SoaringDebug.Log("SoaringDownloadModule: url: " + text);
			soaringDictionary.addValue(text, "turl");
		}
		SoaringDictionary soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("tgets");
		if (soaringDictionary2 != null)
		{
			soaringDictionary.addValue(soaringDictionary2, "tgets");
		}
		soaringDictionary2 = (SoaringDictionary)callData.objectWithKey("tposts");
		if (soaringDictionary2 != null)
		{
			soaringDictionary.addValue(soaringDictionary2, "tposts");
		}
		object obj = callData.objectWithKey("tcallback");
		SoaringObjectBase val = callData.objectWithKey("tsave");
		context.addValue((SCWebQueue.SCDownloadCallbackObject)obj, "tcallback");
		context.addValue(callData.objectWithKey("tobject"), "tobject");
		context.addValue(val, "tsave");
		soaringDictionary.addValue(4, "tchannel");
		soaringDictionary.addValue(new SCWebQueue.SCWebCallbackObject(new SCWebQueue.SCWebQueueCallback(this.Web_Callback)), "tcallback");
		soaringDictionary.addValue(val, "tsave");
		soaringDictionary.addValue(context, "tobject");
		base.PushCallData(soaringDictionary, context);
	}

	// Token: 0x060018A7 RID: 6311 RVA: 0x000A3340 File Offset: 0x000A1540
	protected override bool Web_Callback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		if (state == SCWebQueue.SCWebQueueState.Updated)
		{
			SoaringInternal.Delegate.OnFileDownloadUpdate(SoaringState.Update, null, data, (SoaringContext)userData);
			return true;
		}
		if (error != null)
		{
			state = SCWebQueue.SCWebQueueState.Failed;
		}
		SoaringModule.SoaringModuleData soaringModuleData = base.CreateModuleData();
		this.HandleDelegateCallback(soaringModuleData.Set(state == SCWebQueue.SCWebQueueState.Finished, error, null, (SoaringContext)userData));
		base.ReturnModuledata(soaringModuleData);
		return state != SCWebQueue.SCWebQueueState.Finished;
	}

	// Token: 0x060018A8 RID: 6312 RVA: 0x000A33A0 File Offset: 0x000A15A0
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.context != null)
		{
			SoaringContext context = moduleData.context;
			SCWebQueue.SCDownloadCallbackObject scdownloadCallbackObject = (SCWebQueue.SCDownloadCallbackObject)context.objectWithKey("tcallback");
			string id = context.soaringValue("tobject");
			string path = context.soaringValue("tsave");
			if (scdownloadCallbackObject.callback != null)
			{
				scdownloadCallbackObject.callback(id, moduleData.state, path);
			}
		}
		else
		{
			SoaringInternal.Delegate.OnFileDownloadUpdate((!moduleData.state) ? SoaringState.Fail : SoaringState.Success, moduleData.error, null, moduleData.context);
		}
	}
}
