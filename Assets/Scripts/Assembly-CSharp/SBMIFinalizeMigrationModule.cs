using System;

// Token: 0x0200032F RID: 815
public class SBMIFinalizeMigrationModule : SoaringCustomQueryModule
{
	// Token: 0x060017BD RID: 6077 RVA: 0x0009D2DC File Offset: 0x0009B4DC
	public override string CustomSoaringModuleName()
	{
		return "finalizeMigration";
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x0009D2E4 File Offset: 0x0009B4E4
	public override bool ShouldEncryptCall()
	{
		return SoaringInternalProperties.SecureCommunication;
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x0009D2EC File Offset: 0x0009B4EC
	public override string QueryActionName()
	{
		return "customQuery2";
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x0009D2F4 File Offset: 0x0009B4F4
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(data.soaringValue("gameId"), "gameId");
		callData.removeObjectWithKey("authToken");
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
		base.PostCallData(soaringDictionary, context);
	}

	// Token: 0x04000FB7 RID: 4023
	public const string NAME = "finalizeMigration";
}
