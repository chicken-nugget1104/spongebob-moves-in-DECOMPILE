using System;
using System.Text;
using UnityEngine;

// Token: 0x02000363 RID: 867
public class SoaringModule : SoaringObjectBase
{
	// Token: 0x060018C4 RID: 6340 RVA: 0x000A3D64 File Offset: 0x000A1F64
	public SoaringModule() : base(SoaringObjectBase.IsType.Module)
	{
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x000A3D7C File Offset: 0x000A1F7C
	protected SoaringModule.SoaringModuleData CreateModuleData()
	{
		if (SoaringModule.sModuleDataArray == null)
		{
			SoaringModule.sModuleDataArray = new SoaringArray();
		}
		SoaringModule.SoaringModuleData soaringModuleData;
		if (SoaringModule.sModuleDataArray.count() == 0)
		{
			soaringModuleData = new SoaringModule.SoaringModuleData();
		}
		else
		{
			soaringModuleData = (SoaringModule.SoaringModuleData)SoaringModule.sModuleDataArray.objectAtIndex(0);
			SoaringModule.sModuleDataArray.removeObjectAtIndex(0);
			soaringModuleData.Reset();
		}
		return soaringModuleData;
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x000A3DDC File Offset: 0x000A1FDC
	public virtual bool ShouldEncryptCall()
	{
		return this.encryptedCall;
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x000A3DE4 File Offset: 0x000A1FE4
	protected void ReturnModuledata(SoaringModule.SoaringModuleData data)
	{
		if (data == null || SoaringModule.sModuleDataArray == null)
		{
			return;
		}
		SoaringModule.sModuleDataArray.addObject(data);
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x000A3E04 File Offset: 0x000A2004
	public virtual int ModuleChannel()
	{
		return 0;
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x000A3E08 File Offset: 0x000A2008
	public virtual string ModuleName()
	{
		return null;
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x000A3E0C File Offset: 0x000A200C
	public virtual void InitializeModule(SoaringDictionary data)
	{
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x000A3E10 File Offset: 0x000A2010
	public virtual void FinalizeModule(SoaringDictionary data)
	{
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x000A3E14 File Offset: 0x000A2014
	public virtual bool VerifyCallStillValid(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return true;
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x000A3E18 File Offset: 0x000A2018
	public virtual void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary data2 = SCQueueTools.CreateMessage(this.ModuleName(), data.soaringValue("gameId"), callData);
		this.PushCorePostDataToQueue(data2, this.ModuleChannel(), context, true);
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x000A3E54 File Offset: 0x000A2054
	protected void PushCorePostDataToQueue(SoaringDictionary data, int channel, SoaringContext context, bool updatePlayer)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(4);
		if (data != null)
		{
			soaringDictionary.addValue(data, "tposts");
		}
		soaringDictionary.addValue(channel, "tchannel");
		SCWebQueue.SCWebCallbackObject val;
		if (updatePlayer)
		{
			val = new SCWebQueue.SCWebCallbackObject(new SCWebQueue.SCWebQueueCallback(this.Web_Callback));
		}
		else
		{
			val = new SCWebQueue.SCWebCallbackObject(new SCWebQueue.SCWebQueueCallback(this.Web_Callback_NoPlayerUpdate));
		}
		soaringDictionary.addValue(val, "tcallback");
		val = new SCWebQueue.SCWebCallbackObject(new SCWebQueue.SCWebQueueCallback(this.VerifyCallStillValid));
		soaringDictionary.addValue(val, "tvcallback");
		if (context != null)
		{
			soaringDictionary.addValue(context, "tobject");
		}
		this.PushCallData(soaringDictionary, context);
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x000A3F04 File Offset: 0x000A2104
	protected virtual void BuildEncryptedCall(SoaringDictionary call_data)
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
						byte[] array = SoaringInternal.Encryption.Encrypt(text);
						if (array != null)
						{
							text = Convert.ToBase64String(array);
							string b = string.Concat(new string[]
							{
								"{\"action\":{\"name\":\"",
								this.ModuleName(),
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

	// Token: 0x060018D1 RID: 6353 RVA: 0x000A4034 File Offset: 0x000A2234
	protected virtual SoaringDictionary DecryptCall(SoaringDictionary encodedData)
	{
		if (!this.ShouldEncryptCall() || !SoaringInternalProperties.SecureCommunication || encodedData == null)
		{
			return encodedData;
		}
		try
		{
			SoaringDictionary soaringDictionary = (SoaringDictionary)encodedData.objectWithKey("data");
			if (soaringDictionary != null)
			{
				string text = soaringDictionary.soaringValue("value");
				if (text != null)
				{
					byte[] array = SoaringInternal.Encryption.Decrypt(Convert.FromBase64String(text));
					if (array != null)
					{
						string @string = Encoding.ASCII.GetString(array);
						encodedData = new SoaringDictionary(@string);
						if ((!SoaringInternal.IsProductionMode || SoaringDebug.IsLoggingToConsole) && encodedData != null)
						{
							SoaringDebug.Log("Decrypted: " + @string);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			SoaringDebug.Log(ex.Message + "\n" + ex.StackTrace, LogType.Error);
			encodedData = null;
		}
		return encodedData;
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x000A412C File Offset: 0x000A232C
	protected void PushCallData(SoaringDictionary call_data, SoaringContext context)
	{
		if (this.ShouldEncryptCall())
		{
			if (!SoaringEncryption.IsEncryptionAvailable())
			{
				this.Web_Callback(SCWebQueue.SCWebQueueState.Failed, "{ \"error_message\" : \"Failed to generate connection request\"}", context, "{ \"error_message\" : \"Failed to generate connection request\"}");
				return;
			}
			this.BuildEncryptedCall(call_data);
		}
		SoaringInternal instance = SoaringInternal.instance;
		if (!instance.PushCall(call_data))
		{
			this.Web_Callback(SCWebQueue.SCWebQueueState.Failed, "{ \"error_message\" : \"Failed to generate connection request\"}", context, "{ \"error_message\" : \"Failed to generate connection request\"}");
		}
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x000A41A0 File Offset: 0x000A23A0
	protected virtual bool Web_Callback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return this.Web_Callback_Handler(state, error, userData, data, true);
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x000A41B0 File Offset: 0x000A23B0
	protected virtual bool Web_Callback_NoPlayerUpdate(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data)
	{
		return this.Web_Callback_Handler(state, error, userData, data, false);
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x000A41C0 File Offset: 0x000A23C0
	protected virtual bool Web_Callback_Handler(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object data, bool updatePlayer)
	{
		if (state == SCWebQueue.SCWebQueueState.Updated)
		{
			return true;
		}
		SoaringModule.SoaringModuleData soaringModuleData = this.CreateModuleData();
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
				if (updatePlayer)
				{
					SoaringInternal.instance.UpdatePlayerData(soaringDictionary);
				}
				SoaringInternal.instance.SetSoaringInternalData(soaringDictionary);
				this.ExtractTimestamp(soaringDictionary);
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
				": Stack: ",
				ex.StackTrace
			}), LogType.Warning);
			state = SCWebQueue.SCWebQueueState.Failed;
			this.HandleDelegateCallback(soaringModuleData.Set(false, ex.Message, null, (SoaringContext)userData));
		}
		this.ReturnModuledata(soaringModuleData);
		return state == SCWebQueue.SCWebQueueState.Finished;
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x000A433C File Offset: 0x000A253C
	protected void ExtractTimestamp(SoaringDictionary data)
	{
		if (data == null)
		{
			return;
		}
		SoaringDictionary soaringDictionary = (SoaringDictionary)data.objectWithKey("serverTime");
		if (soaringDictionary == null)
		{
			return;
		}
		SoaringValue soaringValue = soaringDictionary.soaringValue("timestamp");
		if (soaringValue == null)
		{
			return;
		}
		long num = soaringValue;
		if (num <= 0L)
		{
			return;
		}
		SoaringTime.UpdateServerTime(num);
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x000A4394 File Offset: 0x000A2594
	public virtual void HandleDelegateCallback(SoaringModule.SoaringModuleData data)
	{
		SoaringInternal.Delegate.OnComponentFinished(data.state, this.ModuleName(), data.error, data.data, data.context);
	}

	// Token: 0x04001068 RID: 4200
	private static SoaringArray sModuleDataArray;

	// Token: 0x04001069 RID: 4201
	public bool encryptedCall = SoaringInternalProperties.SecureCommunication;

	// Token: 0x02000364 RID: 868
	public class SoaringModuleData : SoaringObjectBase
	{
		// Token: 0x060018D8 RID: 6360 RVA: 0x000A43CC File Offset: 0x000A25CC
		public SoaringModuleData() : base(SoaringObjectBase.IsType.Object)
		{
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x000A43D8 File Offset: 0x000A25D8
		public void Reset()
		{
			this.state = false;
			this.data = null;
			this.context = null;
			this.error = null;
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x000A43F8 File Offset: 0x000A25F8
		public SoaringModule.SoaringModuleData Set(bool state, SoaringError error, SoaringDictionary data, SoaringContext context_data)
		{
			this.state = state;
			this.error = error;
			this.data = data;
			this.context = context_data;
			return this;
		}

		// Token: 0x0400106A RID: 4202
		public bool state;

		// Token: 0x0400106B RID: 4203
		public SoaringDictionary data;

		// Token: 0x0400106C RID: 4204
		public SoaringContext context;

		// Token: 0x0400106D RID: 4205
		public SoaringError error;
	}
}
