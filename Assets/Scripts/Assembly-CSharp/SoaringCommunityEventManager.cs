using System;
using System.Collections.Generic;
using MTools;

// Token: 0x0200034B RID: 843
public class SoaringCommunityEventManager
{
	// Token: 0x0600183B RID: 6203 RVA: 0x000A037C File Offset: 0x0009E57C
	public SoaringCommunityEventManager()
	{
		this.m_pEvents = new MDictionary();
	}

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x0600183C RID: 6204 RVA: 0x000A0390 File Offset: 0x0009E590
	// (remove) Token: 0x0600183D RID: 6205 RVA: 0x000A03A8 File Offset: 0x0009E5A8
	public static event Action<bool, SoaringError, SoaringDictionary, SoaringContext> SetValueFinished;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x0600183E RID: 6206 RVA: 0x000A03C0 File Offset: 0x0009E5C0
	// (remove) Token: 0x0600183F RID: 6207 RVA: 0x000A03D8 File Offset: 0x0009E5D8
	public static event Action<bool, SoaringError, SoaringDictionary, SoaringContext> GetValueFinished;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06001840 RID: 6208 RVA: 0x000A03F0 File Offset: 0x0009E5F0
	// (remove) Token: 0x06001841 RID: 6209 RVA: 0x000A0408 File Offset: 0x0009E608
	public static event Action<bool, SoaringError, SoaringDictionary, SoaringContext> AquireGiftFinished;

	// Token: 0x06001842 RID: 6210 RVA: 0x000A0420 File Offset: 0x0009E620
	public SoaringCommunityEvent GetEvent(string sEventID)
	{
		if (this.m_pEvents == null || string.IsNullOrEmpty(sEventID))
		{
			return null;
		}
		return (SoaringCommunityEvent)this.m_pEvents.objectWithKey(sEventID);
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x000A044C File Offset: 0x0009E64C
	public void _HandleSetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		string sEventID = pContext.soaringValue("eventDid").ToString();
		if (bSuccess)
		{
			SoaringCommunityEvent @event = this.GetEvent(sEventID);
			if (@event == null)
			{
				this.AddEvent(sEventID, pData);
			}
			else
			{
				@event.SetData(sEventID, pData);
			}
		}
		else if (pError.ErrorCode == 404)
		{
			this.RemoveEvent(sEventID);
		}
		if (SoaringCommunityEventManager.SetValueFinished != null)
		{
			SoaringCommunityEventManager.SetValueFinished(bSuccess, pError, pData, pContext);
		}
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x000A04CC File Offset: 0x0009E6CC
	public void _HandleGetValueFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		string sEventID = pContext.soaringValue("eventDid").ToString();
		if (bSuccess)
		{
			SoaringCommunityEvent @event = this.GetEvent(sEventID);
			if (@event == null)
			{
				this.AddEvent(sEventID, pData);
			}
			else
			{
				@event.SetData(sEventID, pData);
			}
		}
		else if (pError.ErrorCode == 404)
		{
			this.RemoveEvent(sEventID);
		}
		if (SoaringCommunityEventManager.GetValueFinished != null)
		{
			SoaringCommunityEventManager.GetValueFinished(bSuccess, pError, pData, pContext);
		}
	}

	// Token: 0x06001845 RID: 6213 RVA: 0x000A054C File Offset: 0x0009E74C
	public void _HandleAquireGiftFinished(bool bSuccess, SoaringError pError, SoaringDictionary pData, SoaringContext pContext)
	{
		string sEventID = pContext.soaringValue("eventDid").ToString();
		int nID = pContext.soaringValue("giftDid");
		if (bSuccess)
		{
			SoaringCommunityEvent @event = this.GetEvent(sEventID);
			if (@event != null)
			{
				SoaringCommunityEvent.Reward reward = @event.GetReward(nID);
				if (reward != null)
				{
					reward._SetAquired(true);
				}
			}
		}
		else if (pError.ErrorCode == 404)
		{
			this.RemoveEvent(sEventID);
		}
		if (SoaringCommunityEventManager.AquireGiftFinished != null)
		{
			SoaringCommunityEventManager.AquireGiftFinished(bSuccess, pError, pData, pContext);
		}
	}

	// Token: 0x06001846 RID: 6214 RVA: 0x000A05DC File Offset: 0x0009E7DC
	private void AddEvent(string sEventID, SoaringDictionary pData)
	{
		string text = "Default";
		SoaringCommunityEvent soaringCommunityEvent = null;
		string text2 = text;
		if (text2 != null)
		{
			if (SoaringCommunityEventManager.<>f__switch$map19 == null)
			{
				SoaringCommunityEventManager.<>f__switch$map19 = new Dictionary<string, int>(1)
				{
					{
						"Default",
						0
					}
				};
			}
			int num;
			if (SoaringCommunityEventManager.<>f__switch$map19.TryGetValue(text2, out num))
			{
				if (num == 0)
				{
					soaringCommunityEvent = new SoaringCommunityEvent(sEventID, pData);
				}
			}
		}
		if (this.m_pEvents.containsKey(soaringCommunityEvent.m_sID))
		{
			this.RemoveEvent(soaringCommunityEvent.m_sID);
		}
		this.m_pEvents.addValue(soaringCommunityEvent, soaringCommunityEvent.m_sID);
	}

	// Token: 0x06001847 RID: 6215 RVA: 0x000A067C File Offset: 0x0009E87C
	private void RemoveEvent(string sEventID)
	{
		if (this.m_pEvents == null || string.IsNullOrEmpty(sEventID))
		{
			return;
		}
		this.m_pEvents.removeObjectWithKey(sEventID);
	}

	// Token: 0x04001015 RID: 4117
	private MDictionary m_pEvents;
}
