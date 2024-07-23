using System;

// Token: 0x02000374 RID: 884
public class SoaringSaveStatModule : SoaringModule
{
	// Token: 0x06001924 RID: 6436 RVA: 0x000A61A8 File Offset: 0x000A43A8
	public override string ModuleName()
	{
		return "saveStat";
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x000A61B0 File Offset: 0x000A43B0
	public override int ModuleChannel()
	{
		return 3;
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x000A61B4 File Offset: 0x000A43B4
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		string text = "{\n\"action\" : {\n\"name\":\"" + this.ModuleName() + "\",\n";
		text = text + "\"authToken\":\"" + data.soaringValue("authToken") + "\"\n},\n\"data\" : ";
		callData.removeObjectWithKey("authToken");
		callData.removeObjectWithKey("gameId");
		text += callData.ToJsonString();
		text += "\n}";
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary, this.ModuleChannel(), context, false);
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x000A6250 File Offset: 0x000A4450
	public override void HandleDelegateCallback(SoaringModule.SoaringModuleData moduleData)
	{
		Soaring.Delegate.OnSaveStat(moduleData.state, false, moduleData.error, moduleData.context);
	}
}
