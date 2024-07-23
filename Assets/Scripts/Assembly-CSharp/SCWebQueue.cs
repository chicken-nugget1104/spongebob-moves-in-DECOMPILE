using System;
using System.Collections;
using MTools;
using UnityEngine;

// Token: 0x0200038B RID: 907
public class SCWebQueue : MonoBehaviour
{
	// Token: 0x060019D8 RID: 6616 RVA: 0x000AA1D8 File Offset: 0x000A83D8
	private int GetTransportChannel()
	{
		int result = this.mNextTransportChannel;
		this.mNextTransportChannel++;
		if (this.mNextTransportChannel >= 4 + SCWebQueue.Transport_Channels)
		{
			this.mNextTransportChannel = 4;
		}
		return result;
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x000AA214 File Offset: 0x000A8414
	public void Initialize(string sdk)
	{
		this.mRealTimeSinceStartup = Time.realtimeSinceStartup;
		if (this.mChannelList != null)
		{
			return;
		}
		SCWebQueue.ReportedSDK = sdk;
		this.mChannelList = new MArray<SCWebQueue.SCWebChannel>(SCWebQueue.Channel_Total);
		this.mPendingNewConnections = new MArray<SCWebQueue.SCPending>(SCWebQueue.Channel_Total);
		for (int i = 0; i < SCWebQueue.Channel_Total; i++)
		{
			this.mChannelList.addObject(new SCWebQueue.SCWebChannel());
		}
	}

	// Token: 0x060019DA RID: 6618 RVA: 0x000AA284 File Offset: 0x000A8484
	public void ClearConnections()
	{
		if (this.mPendingNewConnections != null)
		{
			this.mPendingNewConnections.clear();
		}
		int num = this.mChannelList.count();
		for (int i = 0; i < num; i++)
		{
			SCWebQueue.SCWebChannel scwebChannel = this.mChannelList[i];
			if (scwebChannel != null)
			{
				scwebChannel.Reset();
			}
		}
	}

	// Token: 0x060019DB RID: 6619 RVA: 0x000AA2E4 File Offset: 0x000A84E4
	private void AddConnection(SCWebQueue.SCData data, int channel)
	{
		this.mPendingNewConnections.addObject(new SCWebQueue.SCPending(data, channel));
	}

	// Token: 0x060019DC RID: 6620 RVA: 0x000AA2F8 File Offset: 0x000A84F8
	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		SoaringInternal.instance.Update(realtimeSinceStartup - this.mRealTimeSinceStartup);
		this.mRealTimeSinceStartup = realtimeSinceStartup;
		if (this.IsActive)
		{
			return;
		}
		if (this.mChannelList == null || this.mPendingNewConnections == null)
		{
			return;
		}
		if (this.mChannelList.count() == 0 || this.mPendingNewConnections.count() == 0)
		{
			return;
		}
		base.StartCoroutine(this.Handle_Connections());
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x000AA378 File Offset: 0x000A8578
	private IEnumerator Handle_Connections()
	{
		if (this.IsActive)
		{
			yield break;
		}
		this.IsActive = true;
		for (;;)
		{
			int nCount = this.mPendingNewConnections.count();
			if (nCount != 0)
			{
				for (int nItter = 0; nItter < nCount; nItter++)
				{
					SCWebQueue.SCPending pending = this.mPendingNewConnections.objectAtIndex(nItter);
					int nChannel = pending.Channel;
					if (nChannel == 4)
					{
						nChannel = this.GetTransportChannel();
					}
					this.mChannelList[nChannel].AddConnection(pending.Data);
				}
				this.mPendingNewConnections.clear();
			}
			if (!this.HasActiveConnections())
			{
				break;
			}
			nCount = this.mChannelList.count();
			int nItter2 = 0;
			while (nItter2 < nCount)
			{
				SCWebQueue.SCWebChannel channel = this.mChannelList[nItter2];
				if (channel.HasActiveConnection() && !channel.TestShouldRetry())
				{
					goto IL_251;
				}
				if (channel.HasConnectionsPending() || channel.TestShouldRetry())
				{
					bool didBuildConnection = false;
					try
					{
						didBuildConnection = channel.BuildConnection(channel.Retries < 3);
					}
					catch (Exception ex)
					{
						Exception e = ex;
						channel.FinalizeConnection(false);
						SoaringDebug.Log("SCWebQueue: " + e.Message);
						didBuildConnection = false;
					}
					if (didBuildConnection)
					{
						goto IL_251;
					}
					SoaringDebug.Log("SCWebQueue: Failed to build web connection: Retries: " + channel.Retries.ToString() + " of " + 3.ToString());
				}
				IL_48D:
				nItter2++;
				continue;
				IL_251:
				if (channel.Connection == null)
				{
					channel.PreformCallback(SCWebQueue.SCWebQueueState.Updated, null, 1f, false);
					SoaringError error = new SoaringError("Connection Is Invalid or Null", -8);
					channel.PreformCallback(SCWebQueue.SCWebQueueState.Failed, error, error.Error, true);
					channel.FinalizeConnection(true);
					goto IL_48D;
				}
				if (channel.Connection.HasError)
				{
					SoaringError error2 = new SoaringError("Could not connect to the server.", -7);
					SoaringDebug.Log("SCWebQueue: " + channel.Connection.Error, LogType.Warning);
					channel.PreformCallback(SCWebQueue.SCWebQueueState.Failed, error2, error2.Error, true);
					channel.FinalizeConnection(true);
					goto IL_48D;
				}
				if (channel.Connection.IsDone())
				{
					channel.PreformCallback(SCWebQueue.SCWebQueueState.Updated, null, 1f, false);
					string connectionText = null;
					if (!channel.SaveData())
					{
						connectionText = channel.Connection.ContentAsText;
					}
					channel.PreformCallback(SCWebQueue.SCWebQueueState.Finished, null, connectionText, false);
					channel.FinalizeConnection(false);
					goto IL_48D;
				}
				if (channel.LastProgress == channel.Connection.Progress)
				{
					float testValue = Time.realtimeSinceStartup + 0.01f - channel.LastProgressTimestamp;
					if (testValue > 30f)
					{
						SoaringError error3 = new SoaringError("Connection Timed Out", -6);
						channel.PreformCallback(SCWebQueue.SCWebQueueState.Failed, error3, error3.Error, true);
						channel.FinalizeConnection(true);
					}
					goto IL_48D;
				}
				channel.LastProgress = channel.Connection.Progress;
				channel.LastProgressTimestamp = Time.realtimeSinceStartup;
				channel.PreformCallback(SCWebQueue.SCWebQueueState.Updated, null, channel.LastProgress, false);
				goto IL_48D;
			}
			yield return new WaitForSeconds(this.QueueUpdateTime);
		}
		this.IsActive = false;
		yield return null;
		yield break;
	}

	// Token: 0x060019DE RID: 6622 RVA: 0x000AA394 File Offset: 0x000A8594
	private bool HasActiveConnections()
	{
		bool result = false;
		for (int i = 0; i < this.mChannelList.count(); i++)
		{
			if (this.mChannelList[i].HasConnectionsPending() || this.mChannelList[i].HasActiveConnection())
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x060019DF RID: 6623 RVA: 0x000AA3F4 File Offset: 0x000A85F4
	public void OnApplicationPause(bool paused)
	{
		SoaringInternal.instance.HandleOnApplicationPaused(paused);
	}

	// Token: 0x060019E0 RID: 6624 RVA: 0x000AA404 File Offset: 0x000A8604
	public void OnApplicationQuit()
	{
		SoaringInternal.instance.HandleOnApplicationQuit();
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x000AA410 File Offset: 0x000A8610
	public bool StartConnection(object userData, string url, SCWebQueue.SCWebQueueCallback callback, SCWebQueue.SCWebQueueCallback verifyCallback)
	{
		return this.StartConnection(0, userData, url, null, null, null, callback, verifyCallback);
	}

	// Token: 0x060019E2 RID: 6626 RVA: 0x000AA42C File Offset: 0x000A862C
	public bool StartConnection(int channel, object userData, string url, SCWebQueue.SCWebQueueCallback callback, SCWebQueue.SCWebQueueCallback verifyCallback)
	{
		return this.StartConnection(channel, userData, url, null, null, null, callback, verifyCallback);
	}

	// Token: 0x060019E3 RID: 6627 RVA: 0x000AA44C File Offset: 0x000A864C
	public bool StartConnection(object userData, string url, string saveData, SoaringDictionary postData, SoaringDictionary urlData, SCWebQueue.SCWebQueueCallback callback, SCWebQueue.SCWebQueueCallback verifyCallback)
	{
		return this.StartConnection(0, userData, url, saveData, postData, urlData, callback, verifyCallback);
	}

	// Token: 0x060019E4 RID: 6628 RVA: 0x000AA46C File Offset: 0x000A866C
	public bool StartConnection(int channel, object userData, string url, string saveData, SoaringDictionary postData, SoaringDictionary urlData, SCWebQueue.SCWebQueueCallback callback, SCWebQueue.SCWebQueueCallback verifyCallback)
	{
		if (string.IsNullOrEmpty(url) || callback == null)
		{
			return false;
		}
		SCWebQueue.SCData data = new SCWebQueue.SCData(url, postData, urlData, saveData, callback, userData, verifyCallback);
		this.AddConnection(data, channel);
		return true;
	}

	// Token: 0x060019E5 RID: 6629 RVA: 0x000AA4A8 File Offset: 0x000A86A8
	public void RegisterEventMessage(SoaringContext context)
	{
		if (context == null)
		{
			SoaringDebug.Log("RegisterEventMessage: No Context", LogType.Warning);
			return;
		}
		if (string.IsNullOrEmpty(context.Name))
		{
			SoaringDebug.Log("RegisterEventMessage: No User Name", LogType.Warning);
			return;
		}
		SoaringDebug.Log("RegisterEventMessage: " + context.Name);
		this.mEventQueue.addValue(context, context.Name);
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x000AA50C File Offset: 0x000A870C
	public void HandleEventMessage(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			SoaringDebug.Log("SCWebQueue: No Event Name", LogType.Warning);
			return;
		}
		this.HandleEventMessage((SoaringContext)this.mEventQueue.objectWithKey(name));
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x000AA548 File Offset: 0x000A8748
	public void HandleEventMessage(SoaringContext context)
	{
		if (context == null)
		{
			SoaringDebug.Log("SCWebQueue: Invalid Message Name: " + base.name, LogType.Error);
			return;
		}
		if (context.ContextResponder != null)
		{
			context.ContextResponder(context);
		}
		else if (context.Responder != null)
		{
			context.Responder.OnComponentFinished(true, context.Name, null, null, context);
		}
		this.mEventQueue.removeObjectWithKey(context.Name);
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x000AA5C0 File Offset: 0x000A87C0
	public void ClearEventMessage(SoaringContext context)
	{
		if (context == null)
		{
			return;
		}
		this.mEventQueue.removeObjectWithKey(context.Name);
	}

	// Token: 0x060019E9 RID: 6633 RVA: 0x000AA5DC File Offset: 0x000A87DC
	public void onExternalMessage(string message)
	{
		if (string.IsNullOrEmpty(message))
		{
			SoaringDebug.Log("onExternalMessage: Null Message", LogType.Warning);
			return;
		}
		SoaringDebug.Log("onExternalMessage: " + message);
		SoaringDictionary soaringDictionary = new SoaringDictionary(message);
		SoaringDebug.Log("onExternalMessage: Json: " + soaringDictionary.ToJsonString());
		string text = soaringDictionary.soaringValue("call");
		if (string.IsNullOrEmpty(text))
		{
			SoaringDebug.Log("SCWebQueue: No Call Name:\n" + soaringDictionary, LogType.Error);
			return;
		}
		SoaringContext soaringContext = (SoaringContext)this.mEventQueue.objectWithKey(text);
		if (soaringContext == null)
		{
			SoaringDebug.Log("SCWebQueue: Invalid Message Name: " + text, LogType.Error);
			return;
		}
		this.mEventQueue.removeObjectWithKey(soaringContext.Name);
		if (soaringContext.Responder != null)
		{
			soaringContext.Responder.OnComponentFinished(true, soaringContext.Name, null, soaringDictionary, soaringContext);
		}
		else if (soaringContext.ContextResponder != null)
		{
			soaringContext.addValue(soaringDictionary, "message");
			soaringContext.ContextResponder(soaringContext);
		}
		SoaringDebug.Log("onExternalMessage: " + message + " done");
	}

	// Token: 0x060019EA RID: 6634 RVA: 0x000AA6F4 File Offset: 0x000A88F4
	public void onMemoryWarningMessage(string message)
	{
		SoaringDebug.Log("onMemoryWarningMessage: " + message, LogType.Warning);
	}

	// Token: 0x040010BE RID: 4286
	public const int Channel_Core = 0;

	// Token: 0x040010BF RID: 4287
	public const int Channel_User = 1;

	// Token: 0x040010C0 RID: 4288
	public const int Channel_Components = 2;

	// Token: 0x040010C1 RID: 4289
	public const int Channel_Analytics = 3;

	// Token: 0x040010C2 RID: 4290
	public const int Channel_Transport = 4;

	// Token: 0x040010C3 RID: 4291
	private static int Transport_Channels = 10;

	// Token: 0x040010C4 RID: 4292
	private static int Channel_Total = SCWebQueue.Transport_Channels + 4;

	// Token: 0x040010C5 RID: 4293
	private int mNextTransportChannel = 4;

	// Token: 0x040010C6 RID: 4294
	public static string ReportedSDK = "0";

	// Token: 0x040010C7 RID: 4295
	private bool IsActive;

	// Token: 0x040010C8 RID: 4296
	private float QueueUpdateTime = 0.1f;

	// Token: 0x040010C9 RID: 4297
	private SoaringDictionary mEventQueue = new SoaringDictionary();

	// Token: 0x040010CA RID: 4298
	private MArray<SCWebQueue.SCWebChannel> mChannelList;

	// Token: 0x040010CB RID: 4299
	private MArray<SCWebQueue.SCPending> mPendingNewConnections;

	// Token: 0x040010CC RID: 4300
	private float mRealTimeSinceStartup;

	// Token: 0x0200038C RID: 908
	public enum SCWebQueueState
	{
		// Token: 0x040010CE RID: 4302
		Failed,
		// Token: 0x040010CF RID: 4303
		Finished,
		// Token: 0x040010D0 RID: 4304
		Updated
	}

	// Token: 0x0200038D RID: 909
	public class SCWebCallbackObject : SoaringObjectBase
	{
		// Token: 0x060019EB RID: 6635 RVA: 0x000AA708 File Offset: 0x000A8908
		public SCWebCallbackObject(SCWebQueue.SCWebQueueCallback cbk) : base(SoaringObjectBase.IsType.Object)
		{
			this.callback = cbk;
		}

		// Token: 0x040010D1 RID: 4305
		public SCWebQueue.SCWebQueueCallback callback;
	}

	// Token: 0x0200038E RID: 910
	public class SCDownloadCallbackObject : SoaringObjectBase
	{
		// Token: 0x060019EC RID: 6636 RVA: 0x000AA718 File Offset: 0x000A8918
		public SCDownloadCallbackObject(SCWebQueue.SCDownloadCallback cbk) : base(SoaringObjectBase.IsType.Object)
		{
			this.callback = cbk;
		}

		// Token: 0x040010D2 RID: 4306
		public SCWebQueue.SCDownloadCallback callback;
	}

	// Token: 0x0200038F RID: 911
	internal class SCWebChannel
	{
		// Token: 0x060019ED RID: 6637 RVA: 0x000AA728 File Offset: 0x000A8928
		public SCWebChannel()
		{
			this.mConnectionsPending = new MList();
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060019EE RID: 6638 RVA: 0x000AA73C File Offset: 0x000A893C
		public SoaringConnection Connection
		{
			get
			{
				return this.mConnection;
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x000AA744 File Offset: 0x000A8944
		public bool TestShouldRetry()
		{
			return this.mShouldRetry;
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x000AA74C File Offset: 0x000A894C
		public void AddConnection(SCWebQueue.SCData data)
		{
			if (data == null)
			{
				return;
			}
			this.mConnectionsPending.PushBack(data);
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x000AA764 File Offset: 0x000A8964
		public bool HasConnectionsPending()
		{
			return this.mConnectionsPending.count() != 0;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x000AA778 File Offset: 0x000A8978
		public bool HasActiveConnection()
		{
			return this.mCurrentData != null;
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x000AA788 File Offset: 0x000A8988
		public bool PreformCallback(SCWebQueue.SCWebQueueState state, SoaringError error, object data, bool canRetry)
		{
			bool result = false;
			if (state == SCWebQueue.SCWebQueueState.Failed)
			{
				if (data != null)
				{
					SoaringDebug.Log("SCWebQueue: Failed: " + data.ToString());
				}
				if (canRetry && this.Retries < 3)
				{
					return true;
				}
			}
			if (this.mCurrentData != null)
			{
				result = this.mCurrentData.PreformCallback(state, error, data);
			}
			return result;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x000AA7E8 File Offset: 0x000A89E8
		public bool SaveData()
		{
			return this.mCurrentData != null && this.mConnection != null && this.mConnection.SaveData();
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000AA810 File Offset: 0x000A8A10
		public bool BuildConnection(bool canRetry)
		{
			if (!this.mShouldRetry && !this.HasConnectionsPending())
			{
				SoaringDebug.Log("SCWebQueue: Failed to build: No Pending Connections");
				return false;
			}
			this.mShouldRetry = false;
			if (!canRetry || this.mCurrentData == null)
			{
				this.mCurrentData = (SCWebQueue.SCData)this.mConnectionsPending.PopFront();
				this.Retries = 0;
			}
			else
			{
				this.Retries++;
			}
			if (this.mCurrentData == null)
			{
				return false;
			}
			if (!this.mCurrentData.PreformVerifyCallback(SCWebQueue.SCWebQueueState.Updated, null))
			{
				SoaringError soaringError = new SoaringError("Failed Call Verification.", -10);
				this.PreformCallback(SCWebQueue.SCWebQueueState.Failed, soaringError, soaringError.Error, false);
				this.FinalizeConnection(false);
				return false;
			}
			this.mConnection = new SoaringUnityConnection();
			if (!this.mConnection.Create(this.mCurrentData))
			{
				SoaringError soaringError2 = new SoaringError("Invalid Connection Data.", -9);
				this.PreformCallback(SCWebQueue.SCWebQueueState.Failed, soaringError2, soaringError2.Error, false);
				this.FinalizeConnection(false);
				return false;
			}
			this.LastProgress = 0f;
			this.LastProgressTimestamp = Time.realtimeSinceStartup;
			this.ConnectionStartTime = this.LastProgressTimestamp;
			return true;
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x000AA938 File Offset: 0x000A8B38
		public void FinalizeConnection(bool canRetry)
		{
			if (!canRetry)
			{
				if (this.mConnection != null && this.mCurrentData != null)
				{
					SoaringDebug.Log(string.Concat(new string[]
					{
						"Finalize: Retries: ",
						this.Retries.ToString(),
						" Time: ",
						(Time.realtimeSinceStartup - this.ConnectionStartTime).ToString(),
						"\nUrl: ",
						this.mCurrentData.URL
					}));
				}
				else
				{
					SoaringDebug.Log("Finalize: Retries: " + this.Retries.ToString() + " Time: " + (Time.realtimeSinceStartup - this.ConnectionStartTime).ToString());
				}
				this.mCurrentData = null;
				this.Retries = 0;
			}
			this.LastProgress = -1f;
			this.mConnection = null;
			this.mConnection = null;
			this.mShouldRetry = canRetry;
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x000AAA24 File Offset: 0x000A8C24
		public void Reset()
		{
			this.FinalizeConnection(false);
			if (this.mConnectionsPending != null)
			{
				this.mConnectionsPending = new MList();
			}
		}

		// Token: 0x040010D3 RID: 4307
		public const float Timeout = 30f;

		// Token: 0x040010D4 RID: 4308
		public const int MaxRetries = 3;

		// Token: 0x040010D5 RID: 4309
		public float LastProgress;

		// Token: 0x040010D6 RID: 4310
		public float LastProgressTimestamp;

		// Token: 0x040010D7 RID: 4311
		public float ConnectionStartTime;

		// Token: 0x040010D8 RID: 4312
		public int Retries;

		// Token: 0x040010D9 RID: 4313
		private MList mConnectionsPending;

		// Token: 0x040010DA RID: 4314
		private SCWebQueue.SCData mCurrentData;

		// Token: 0x040010DB RID: 4315
		private SoaringConnection mConnection;

		// Token: 0x040010DC RID: 4316
		private bool mShouldRetry;
	}

	// Token: 0x02000390 RID: 912
	public class SCData
	{
		// Token: 0x060019F8 RID: 6648 RVA: 0x000AAA44 File Offset: 0x000A8C44
		public SCData()
		{
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x000AAA4C File Offset: 0x000A8C4C
		public SCData(string url, SoaringDictionary post, SoaringDictionary gt, SCWebQueue.SCWebQueueCallback cbk, object userdata, SCWebQueue.SCWebQueueCallback v_cbk)
		{
			this.mUserData = userdata;
			this.mCallback = cbk;
			this.mVerifyCallback = v_cbk;
			this.SetGetParams(gt);
			this.SetPostParams(post);
			this.SetURL(url);
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x000AAA8C File Offset: 0x000A8C8C
		public SCData(string url, SoaringDictionary post, SoaringDictionary gt, string save, SCWebQueue.SCWebQueueCallback cbk, object userdata, SCWebQueue.SCWebQueueCallback v_cbk)
		{
			this.mUserData = userdata;
			this.mCallback = cbk;
			this.mVerifyCallback = v_cbk;
			this.SetGetParams(gt);
			this.SetPostParams(post);
			this.SetSaveLocation(save);
			this.SetURL(url);
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x000AAAD4 File Offset: 0x000A8CD4
		public SoaringDictionary GetParams
		{
			get
			{
				return this.mGetParams;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x000AAADC File Offset: 0x000A8CDC
		public SoaringDictionary PostParams
		{
			get
			{
				return this.mPostParams;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x000AAAE4 File Offset: 0x000A8CE4
		public string URL
		{
			get
			{
				return this.mURL;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x000AAAEC File Offset: 0x000A8CEC
		public string SaveLocation
		{
			get
			{
				return this.mSaveLocation;
			}
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x000AAAF4 File Offset: 0x000A8CF4
		public bool PreformCallback(SCWebQueue.SCWebQueueState state, SoaringError error, object obj)
		{
			bool result = false;
			if (this.mCallback != null)
			{
				result = this.mCallback(state, error, this.mUserData, obj);
			}
			return result;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x000AAB24 File Offset: 0x000A8D24
		public bool PreformVerifyCallback(SCWebQueue.SCWebQueueState state, SoaringError error)
		{
			bool result = true;
			if (this.mVerifyCallback != null)
			{
				result = this.mVerifyCallback(state, error, this.mUserData, null);
			}
			return result;
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x000AAB54 File Offset: 0x000A8D54
		public void SetGetParams(SoaringDictionary p)
		{
			this.mGetParams = p;
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x000AAB60 File Offset: 0x000A8D60
		public void SetPostParams(SoaringDictionary p)
		{
			this.mPostParams = p;
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x000AAB6C File Offset: 0x000A8D6C
		public void SetURL(string url)
		{
			this.mURL = url;
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000AAB78 File Offset: 0x000A8D78
		public void SetSaveLocation(string p)
		{
			this.mSaveLocation = p;
		}

		// Token: 0x040010DD RID: 4317
		private object mUserData;

		// Token: 0x040010DE RID: 4318
		private SoaringDictionary mGetParams;

		// Token: 0x040010DF RID: 4319
		private SoaringDictionary mPostParams;

		// Token: 0x040010E0 RID: 4320
		private string mURL;

		// Token: 0x040010E1 RID: 4321
		private string mSaveLocation;

		// Token: 0x040010E2 RID: 4322
		private SCWebQueue.SCWebQueueCallback mCallback;

		// Token: 0x040010E3 RID: 4323
		private SCWebQueue.SCWebQueueCallback mVerifyCallback;
	}

	// Token: 0x02000391 RID: 913
	internal class SCPending
	{
		// Token: 0x06001A05 RID: 6661 RVA: 0x000AAB84 File Offset: 0x000A8D84
		public SCPending()
		{
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x000AAB8C File Offset: 0x000A8D8C
		public SCPending(SCWebQueue.SCData connectionData, int channel)
		{
			this.mConnectionData = connectionData;
			this.mChannel = channel;
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x000AABA4 File Offset: 0x000A8DA4
		public SCWebQueue.SCData Data
		{
			get
			{
				return this.mConnectionData;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x000AABAC File Offset: 0x000A8DAC
		public int Channel
		{
			get
			{
				return this.mChannel;
			}
		}

		// Token: 0x040010E4 RID: 4324
		private SCWebQueue.SCData mConnectionData;

		// Token: 0x040010E5 RID: 4325
		private int mChannel;
	}

	// Token: 0x020004B3 RID: 1203
	// (Invoke) Token: 0x0600252F RID: 9519
	public delegate bool SCWebQueueCallback(SCWebQueue.SCWebQueueState state, SoaringError error, object userData, object call_data);

	// Token: 0x020004B4 RID: 1204
	// (Invoke) Token: 0x06002533 RID: 9523
	public delegate void SCDownloadCallback(string id, bool success, string path);
}
