using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class CallbackQueue
{
	// Token: 0x06000A3B RID: 2619 RVA: 0x0003F890 File Offset: 0x0003DA90
	public TFServer.JsonResponseHandler AsyncCallback(TFServer.JsonResponseHandler handler)
	{
		return delegate(Dictionary<string, object> response, object userData)
		{
			this.QueueCallback(handler, response, userData);
		};
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x0003F8C0 File Offset: 0x0003DAC0
	public void QueueCallback(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, object userData)
	{
		CallbackQueue.CallbackEntry item = new CallbackQueue.CallbackEntry(handler, data, userData);
		List<CallbackQueue.CallbackEntry> obj = this.callbackEntries;
		lock (obj)
		{
			this.callbackEntries.Add(item);
		}
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x0003F918 File Offset: 0x0003DB18
	private bool CallbackReady(CallbackQueue.DelayedCallbackEntry entry)
	{
		return entry.scheduledTime <= Time.time;
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x0003F92C File Offset: 0x0003DB2C
	public void ProcessQueue()
	{
		List<CallbackQueue.CallbackEntry> obj = this.callbackEntries;
		List<CallbackQueue.CallbackEntry> list;
		lock (obj)
		{
			list = new List<CallbackQueue.CallbackEntry>(this.callbackEntries);
			this.callbackEntries.Clear();
		}
		List<CallbackQueue.DelayedCallbackEntry> obj2 = this.delayedCallbackEntries;
		lock (obj2)
		{
			List<CallbackQueue.DelayedCallbackEntry> list2 = this.delayedCallbackEntries.FindAll(new Predicate<CallbackQueue.DelayedCallbackEntry>(this.CallbackReady));
			foreach (CallbackQueue.DelayedCallbackEntry item in list2)
			{
				list.Add(item);
				this.delayedCallbackEntries.Remove(item);
			}
		}
		foreach (CallbackQueue.CallbackEntry callbackEntry in list)
		{
			try
			{
				callbackEntry.handler(callbackEntry.data, callbackEntry.customData);
			}
			catch (Exception arg)
			{
				TFUtils.ErrorLog("Callback processing failure: " + arg);
				TFUtils.ErrorLog(string.Concat(new object[]
				{
					"Failed to process callback for ",
					callbackEntry.handler,
					" with data ",
					callbackEntry.data
				}));
			}
		}
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0003FB00 File Offset: 0x0003DD00
	public void QueueCallback(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, float delay, object userData)
	{
		CallbackQueue.DelayedCallbackEntry item = new CallbackQueue.DelayedCallbackEntry(handler, data, Time.time + delay, userData);
		List<CallbackQueue.DelayedCallbackEntry> obj = this.delayedCallbackEntries;
		lock (obj)
		{
			this.delayedCallbackEntries.Add(item);
		}
	}

	// Token: 0x040006FF RID: 1791
	protected List<CallbackQueue.CallbackEntry> callbackEntries = new List<CallbackQueue.CallbackEntry>();

	// Token: 0x04000700 RID: 1792
	protected List<CallbackQueue.DelayedCallbackEntry> delayedCallbackEntries = new List<CallbackQueue.DelayedCallbackEntry>();

	// Token: 0x02000119 RID: 281
	protected class CallbackEntry
	{
		// Token: 0x06000A40 RID: 2624 RVA: 0x0003FB60 File Offset: 0x0003DD60
		public CallbackEntry(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, object userData)
		{
			this.handler = handler;
			this.data = data;
			this.customData = userData;
		}

		// Token: 0x04000701 RID: 1793
		public TFServer.JsonResponseHandler handler;

		// Token: 0x04000702 RID: 1794
		public Dictionary<string, object> data;

		// Token: 0x04000703 RID: 1795
		public object customData;
	}

	// Token: 0x0200011A RID: 282
	protected class DelayedCallbackEntry : CallbackQueue.CallbackEntry
	{
		// Token: 0x06000A41 RID: 2625 RVA: 0x0003FB80 File Offset: 0x0003DD80
		public DelayedCallbackEntry(TFServer.JsonResponseHandler handler, Dictionary<string, object> data, float scheduledTime, object userData) : base(handler, data, userData)
		{
			this.scheduledTime = scheduledTime;
		}

		// Token: 0x04000704 RID: 1796
		public float scheduledTime;
	}
}
