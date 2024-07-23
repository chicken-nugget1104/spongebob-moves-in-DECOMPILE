using System;

// Token: 0x0200034A RID: 842
public class SoaringCommunityEventDelegate : SoaringDelegate
{
	// Token: 0x0600183A RID: 6202 RVA: 0x000A02C4 File Offset: 0x0009E4C4
	public override void OnComponentFinished(bool bSuccess, string sModule, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		switch (sModule)
		{
		case "setEventValue":
			Soaring.CommunityEventManager._HandleSetValueFinished(bSuccess, pError, pData, pContext);
			break;
		case "getEventValue":
			Soaring.CommunityEventManager._HandleGetValueFinished(bSuccess, pError, pData, pContext);
			break;
		case "acquireEventGift":
			Soaring.CommunityEventManager._HandleAquireGiftFinished(bSuccess, pError, pData, pContext);
			break;
		}
	}
}
