using System;

// Token: 0x0200032D RID: 813
public class SBMIAddCredentialsToUserModule : SoaringModule
{
	// Token: 0x060017B7 RID: 6071 RVA: 0x0009D238 File Offset: 0x0009B438
	public override string ModuleName()
	{
		return "addCredentialsToUser";
	}

	// Token: 0x060017B8 RID: 6072 RVA: 0x0009D240 File Offset: 0x0009B440
	public override void CallModule(SoaringDictionary data, SoaringDictionary callData, SoaringContext context)
	{
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(data.objectWithKey("authToken"), "authToken");
		string text = "{\n" + SCQueueTools.CreateJsonMessage("action", this.ModuleName(), null, soaringDictionary);
		text = text + ",\n\"data\" : " + callData.ToJsonString() + "\n}";
		SoaringDictionary soaringDictionary2 = new SoaringDictionary(1);
		soaringDictionary2.addValue(text, "data");
		base.PushCorePostDataToQueue(soaringDictionary2, 1, context, false);
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x0009D2C0 File Offset: 0x0009B4C0
	public override bool ShouldEncryptCall()
	{
		return false;
	}

	// Token: 0x04000FB5 RID: 4021
	public const string NAME = "addCredentialsToUser";
}
