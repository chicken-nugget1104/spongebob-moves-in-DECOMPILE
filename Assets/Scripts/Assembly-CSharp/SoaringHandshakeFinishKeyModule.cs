using System;

// Token: 0x02000351 RID: 849
internal class SoaringHandshakeFinishKeyModule : SoaringModule
{
	// Token: 0x0600186B RID: 6251 RVA: 0x000A1A80 File Offset: 0x0009FC80
	public override string ModuleName()
	{
		return "handshake_pt3";
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x000A1A88 File Offset: 0x0009FC88
	public override int ModuleChannel()
	{
		return 0;
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x000A1A8C File Offset: 0x0009FC8C
	public override bool ShouldEncryptCall()
	{
		return false;
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x000A1A90 File Offset: 0x0009FC90
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", "handshake", data.soaringValue("gameId"), null) + ",\n";
		text = text + "\"data\" : " + callData.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, false);
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x000A1B08 File Offset: 0x0009FD08
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData data)
	{
		if (data.state)
		{
			SoaringInternal.Encryption = (SoaringEncryption)data.context.objectWithKey("encryption");
			if (SoaringInternal.Encryption != null)
			{
				SoaringInternal.Encryption.StartUsingEncryption();
			}
			else
			{
				SoaringInternal.instance.TriggerOfflineMode(true);
			}
		}
		else
		{
			SoaringInternal.instance.TriggerOfflineMode(true);
		}
		if (!SoaringInternal.instance.IsInitialized())
		{
			SoaringInternal.instance.HandleFinalGameInitialization(data.state);
		}
		else
		{
			SoaringInternal.instance.HandleStashedCalls();
		}
		if (data.context != null && data.context.ContextResponder != null)
		{
			data.context.ContextResponder(data.context);
		}
	}
}
