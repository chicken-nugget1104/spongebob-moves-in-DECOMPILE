using System;

// Token: 0x02000350 RID: 848
internal class SoaringHandshakeTestKeyModule : SoaringModule
{
	// Token: 0x06001865 RID: 6245 RVA: 0x000A1914 File Offset: 0x0009FB14
	public override string ModuleName()
	{
		return "handshake_pt2";
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x000A191C File Offset: 0x0009FB1C
	public override int ModuleChannel()
	{
		return 0;
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x000A1920 File Offset: 0x0009FB20
	public override bool ShouldEncryptCall()
	{
		return false;
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x000A1924 File Offset: 0x0009FB24
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", "handshake", data.soaringValue("gameId"), null) + ",\n";
		text = text + "\"data\" : " + callData.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary();
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, 0, context, false);
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x000A199C File Offset: 0x0009FB9C
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		if (moduleData.data == null)
		{
			moduleData.state = false;
		}
		bool flag = false;
		if (moduleData.state)
		{
			string text = moduleData.data.soaringValue("finish");
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Replace("\n", "\\n");
				SoaringDictionary soaringDictionary = new SoaringDictionary(2);
				soaringDictionary.addValue(text, "finish");
				soaringDictionary.addValue(SoaringEncryption.SID, "sid");
				flag = SoaringInternal.instance.CallModule("handshake_pt3", soaringDictionary, moduleData.context);
			}
		}
		if (!flag)
		{
			SoaringInternal.instance.TriggerOfflineMode(true);
			if (!SoaringInternal.instance.IsInitialized())
			{
				SoaringInternal.instance.HandleFinalGameInitialization(false);
			}
			else
			{
				SoaringInternal.instance.HandleStashedCalls();
			}
		}
	}
}
