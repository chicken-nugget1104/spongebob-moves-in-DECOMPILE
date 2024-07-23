using System;

// Token: 0x02000356 RID: 854
public class SoaringAnonymousSaveStatModule : SoaringModule
{
	// Token: 0x06001884 RID: 6276 RVA: 0x000A24B8 File Offset: 0x000A06B8
	public override string ModuleName()
	{
		return "saveAnonymousStat";
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x000A24C0 File Offset: 0x000A06C0
	public override int ModuleChannel()
	{
		return 3;
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x000A24C4 File Offset: 0x000A06C4
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n\"action\" : {\n\"name\":\"" + this.ModuleName() + "\",\n";
		text = text + "\"gameId\":\"" + data.soaringValue("gameId") + "\"\n},\n\"data\" : ";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text += callData.ToJsonString();
		text += "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, this.ModuleChannel(), context, false);
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x000A2560 File Offset: 0x000A0760
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnSaveStat(moduleData.state, true, moduleData.error, moduleData.context);
	}
}
