using System;

// Token: 0x0200039E RID: 926
public class SoaringEvents : SoaringDelegate
{
	// Token: 0x06001A44 RID: 6724 RVA: 0x000ABD74 File Offset: 0x000A9F74
	public SoaringEvents()
	{
		this.mBannerAdEvents = new SoaringArray();
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x000ABD88 File Offset: 0x000A9F88
	public void LoadEvents(SoaringArray events)
	{
		if (events == null)
		{
			return;
		}
		int num = events.count();
		for (int i = 0; i < num; i++)
		{
			SoaringDictionary soaringDictionary = (SoaringDictionary)events.objectAtIndex(i);
			if (soaringDictionary != null)
			{
				SoaringEvent soaringEvent = new SoaringEvent(soaringDictionary);
				if (soaringEvent.HasDisplayBannerEvent() && soaringEvent.Requires == null)
				{
					this.mBannerAdEvents.addObject(soaringEvent);
				}
				else
				{
					Soaring.Delegate.OnRecievedEvent(this, soaringEvent);
				}
			}
		}
		this.HandleBannerAdEvent();
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x000ABE10 File Offset: 0x000AA010
	public bool AddBannerEvent(SoaringEvent ev)
	{
		if (ev == null)
		{
			return false;
		}
		if (!ev.HasDisplayBannerEvent())
		{
			return false;
		}
		this.mBannerAdEvents.addObject(ev);
		this.HandleBannerAdEvent();
		return true;
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x000ABE48 File Offset: 0x000AA048
	public bool HandleEventsHandled(SoaringEvent ev, bool handleActions = true)
	{
		return false;
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x000ABE4C File Offset: 0x000AA04C
	public bool HandleEventsActionsHandled(SoaringEvent.SoaringEventAction ac)
	{
		return false;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x000ABE50 File Offset: 0x000AA050
	protected void HandleBannerAdEvent()
	{
		if (this.mBannerAdEventActive)
		{
			return;
		}
		if (this.mBannerAdEvents == null)
		{
			return;
		}
		if (this.mBannerAdEvents.count() == 0)
		{
			return;
		}
		SoaringEvent soaringEvent = (SoaringEvent)this.mBannerAdEvents.objectAtIndex(0);
		this.mBannerAdEvents.removeObjectAtIndex(0);
		SoaringEvent.SoaringEventAction soaringEventAction = null;
		soaringEvent.HasDisplayBannerEvent(ref soaringEventAction);
		if (soaringEventAction == null)
		{
			SoaringDebug.Log(soaringEvent.Name + " : No Banner Action Returned");
			Soaring.Delegate.OnRecievedEvent(this, soaringEvent);
			this.HandleBannerAdEvent();
			return;
		}
		if (string.IsNullOrEmpty(soaringEventAction.Value))
		{
			SoaringDebug.Log(soaringEvent.Name + " : No Banner Name");
			Soaring.Delegate.OnRecievedEvent(this, soaringEvent);
			this.HandleBannerAdEvent();
			return;
		}
		SoaringContext soaringContext = new SoaringContext();
		soaringContext.Responder = this;
		soaringContext.addValue(soaringEvent, "event");
		soaringContext.addValue(soaringEventAction.Value, "event_banner");
		this.mBannerAdEventActive = true;
		Soaring.RequestSoaringAdvert(soaringEventAction.Value, false, soaringContext);
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x000ABF58 File Offset: 0x000AA158
	public override void OnAdServed(bool success, SoaringAdData adData, SoaringAdServerState state, SoaringContext context)
	{
		if (context == null)
		{
			if (this.mBannerAdEventActive)
			{
				SoaringDebug.Log("No Banner Context");
				Soaring.Delegate.OnRecievedEvent(this, (SoaringEvent)context.objectWithKey("event"));
				this.mBannerAdEventActive = false;
				this.HandleBannerAdEvent();
			}
			return;
		}
		if (state == SoaringAdServerState.Failed)
		{
			if (this.mBannerAdEventActive)
			{
				SoaringDebug.Log("Banner Failed To Be Retrieved");
				this.mBannerAdEventActive = false;
				this.HandleBannerAdEvent();
			}
			return;
		}
		if (state == SoaringAdServerState.Retrieved)
		{
			string text = context.soaringValue("event_banner");
			if (string.IsNullOrEmpty(text))
			{
				Soaring.Delegate.OnRecievedEvent(this, (SoaringEvent)context.objectWithKey("event"));
				this.mBannerAdEventActive = false;
				this.HandleBannerAdEvent();
				return;
			}
			if (!Soaring.SoaringDisplayAdvert(text))
			{
				SoaringDebug.Log("No Banner To Display");
				Soaring.Delegate.OnRecievedEvent(this, (SoaringEvent)context.objectWithKey("event"));
				this.mBannerAdEventActive = false;
				this.HandleBannerAdEvent();
			}
		}
		else if (state == SoaringAdServerState.Clicked || state == SoaringAdServerState.Closed)
		{
			Soaring.Delegate.OnRecievedEvent(this, (SoaringEvent)context.objectWithKey("event"));
			this.mBannerAdEventActive = false;
			this.HandleBannerAdEvent();
		}
	}

	// Token: 0x04001126 RID: 4390
	public SoaringArray mBannerAdEvents;

	// Token: 0x04001127 RID: 4391
	public bool mBannerAdEventActive;
}
