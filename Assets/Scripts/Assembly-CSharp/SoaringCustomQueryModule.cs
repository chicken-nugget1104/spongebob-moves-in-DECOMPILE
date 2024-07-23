using System;
using System.Text;
using UnityEngine;

// Token: 0x0200035B RID: 859
public class SoaringCustomQueryModule : SoaringModule
{
	// Token: 0x0600189B RID: 6299 RVA: 0x000A2D9C File Offset: 0x000A0F9C
	public virtual string CustomSoaringModuleName()
	{
		return null;
	}

	// Token: 0x0600189C RID: 6300 RVA: 0x000A2DA0 File Offset: 0x000A0FA0
	public override string ModuleName()
	{
		return this.CustomSoaringModuleName();
	}

	// Token: 0x0600189D RID: 6301 RVA: 0x000A2DA8 File Offset: 0x000A0FA8
	public virtual string QueryActionName()
	{
		return "customQuery";
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x000A2DB0 File Offset: 0x000A0FB0
	public override int ModuleChannel()
	{
		return 2;
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x000A2DB4 File Offset: 0x000A0FB4
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(2);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.QueryActionName(), null, soaringDictionary) + ",\n";
		string text2 = text;
		text = string.Concat(new string[]
		{
			text2,
			"\"data\" : {\n\"queryService\" : \"",
			this.CustomSoaringModuleName(),
			"\",\n\"queryParameters\" : ",
			callData.ToJsonString(),
			"\n}\n}"
		});
		soaringDictionary.clear();
		soaringDictionary.addValue(text, "data");
		this.PostCallData(soaringDictionary, context);
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x000A2E74 File Offset: 0x000A1074
	protected void PostCallData(SoaringDictionary parameters, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(6);
		soaringDictionary.addValue(parameters, "tposts");
		soaringDictionary.addValue(this.ModuleChannel(), "tchannel");
		soaringDictionary.addValue(new SCWebQueue.SCWebCallbackObject(new SCWebQueue.SCWebQueueCallback(this.Web_Callback)), "tcallback");
		soaringDictionary.addValue(context, "tobject");
		soaringDictionary.addValue(new SCWebQueue.SCWebCallbackObject(new SCWebQueue.SCWebQueueCallback(this.VerifyCallStillValid)), "tvcallback");
		base.PushCallData(soaringDictionary, context);
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x000A2EF8 File Offset: 0x000A10F8
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
				else
				{
					soaringDictionary = this.DecryptCall(soaringDictionary);
				}
			}
			if (!SoaringInternal.IsProductionMode || SoaringDebug.IsLoggingToConsole)
			{
				SoaringDebug.Log((string)data);
			}
			if (state == SCWebQueue.SCWebQueueState.Finished)
			{
				soaringDictionary = (SoaringDictionary)soaringDictionary.objectWithKey("data");
				base.ExtractTimestamp(soaringDictionary);
				SoaringInternal.instance.SetSoaringInternalData(soaringDictionary);
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
				" Stack:\n",
				ex.StackTrace
			}), LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			this.HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		base.ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	// Token: 0x060018A2 RID: 6306 RVA: 0x000A3060 File Offset: 0x000A1260
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData data)
	{
		if (data.data == null || data.error != null)
		{
			data.state = false;
			data.data = new SoaringDictionary();
		}
		Soaring.Delegate.OnComponentFinished(data.state, this.CustomSoaringModuleName(), data.error, data.data, data.context);
	}

	// Token: 0x060018A3 RID: 6307 RVA: 0x000A30C0 File Offset: 0x000A12C0
	protected override void BuildEncryptedCall(SoaringDictionary call_data)
	{
		try
		{
			if (call_data != null)
			{
				SoaringDictionary soaringDictionary = (SoaringDictionary)call_data.objectWithKey("tposts");
				if (soaringDictionary != null)
				{
					string text = soaringDictionary.soaringValue("data");
					if (text != null)
					{
						byte[] array = SoaringInternal.Encryption.Encrypt(Encoding.ASCII.GetBytes(text));
						if (array != null)
						{
							text = Convert.ToBase64String(array);
							string b = string.Concat(new string[]
							{
								"{\"action\":{\"name\":\"",
								this.QueryActionName(),
								"UsingEncryption\",\"sid\":\"",
								SoaringEncryption.SID,
								"\",\"value\":\"",
								text,
								"\"}}"
							});
							soaringDictionary.setValue(b, "data");
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message + "\n" + ex.StackTrace, LogType.Error);
		}
		catch
		{
			SoaringDebug.Log("BuildEncryptedCall: Unknown Exception Thrown", LogType.Error);
		}
	}
}
