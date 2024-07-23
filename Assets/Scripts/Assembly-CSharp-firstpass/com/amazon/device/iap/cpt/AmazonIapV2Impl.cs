using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using com.amazon.device.iap.cpt.json;
using UnityEngine;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x0200000B RID: 11
	public abstract class AmazonIapV2Impl : MonoBehaviour, IAmazonIapV2
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002E7C File Offset: 0x0000107C
		private AmazonIapV2Impl()
		{
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002EBC File Offset: 0x000010BC
		public static IAmazonIapV2 Instance
		{
			get
			{
				return AmazonIapV2Impl.Builder.instance;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002EC4 File Offset: 0x000010C4
		public static void callback(string jsonMessage)
		{
			try
			{
				AmazonIapV2Impl.logger.Debug("Executing callback");
				Dictionary<string, object> dictionary = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				string callerId = dictionary["callerId"] as string;
				Dictionary<string, object> response = dictionary["response"] as Dictionary<string, object>;
				AmazonIapV2Impl.callbackCaller(response, callerId);
			}
			catch (KeyNotFoundException inner)
			{
				AmazonIapV2Impl.logger.Debug("callerId not found in callback");
				throw new AmazonException("Internal Error: Unknown callback id", inner);
			}
			catch (AmazonException ex)
			{
				AmazonIapV2Impl.logger.Debug("Async call threw exception: " + ex.ToString());
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002F94 File Offset: 0x00001194
		private static void callbackCaller(Dictionary<string, object> response, string callerId)
		{
			IDelegator delegator = null;
			try
			{
				Jsonable.CheckForErrors(response);
				object obj = AmazonIapV2Impl.callbackLock;
				lock (obj)
				{
					delegator = AmazonIapV2Impl.callbackDictionary[callerId];
					AmazonIapV2Impl.callbackDictionary.Remove(callerId);
					delegator.ExecuteSuccess(response);
				}
			}
			catch (AmazonException e)
			{
				object obj2 = AmazonIapV2Impl.callbackLock;
				lock (obj2)
				{
					if (delegator == null)
					{
						delegator = AmazonIapV2Impl.callbackDictionary[callerId];
					}
					AmazonIapV2Impl.callbackDictionary.Remove(callerId);
					delegator.ExecuteError(e);
				}
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003074 File Offset: 0x00001274
		public static void FireEvent(string jsonMessage)
		{
			try
			{
				AmazonIapV2Impl.logger.Debug("eventReceived");
				Dictionary<string, object> dictionary = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				string key = dictionary["eventId"] as string;
				Dictionary<string, object> dictionary2 = null;
				if (dictionary.ContainsKey("response"))
				{
					dictionary2 = (dictionary["response"] as Dictionary<string, object>);
					Jsonable.CheckForErrors(dictionary2);
				}
				object obj = AmazonIapV2Impl.eventLock;
				lock (obj)
				{
					foreach (IDelegator delegator in AmazonIapV2Impl.eventListeners[key])
					{
						if (dictionary2 != null)
						{
							delegator.ExecuteSuccess(dictionary2);
						}
						else
						{
							delegator.ExecuteSuccess();
						}
					}
				}
			}
			catch (AmazonException ex)
			{
				AmazonIapV2Impl.logger.Debug("Event call threw exception: " + ex.ToString());
			}
		}

		// Token: 0x0600002D RID: 45
		public abstract RequestOutput GetUserData();

		// Token: 0x0600002E RID: 46
		public abstract RequestOutput Purchase(SkuInput skuInput);

		// Token: 0x0600002F RID: 47
		public abstract RequestOutput GetProductData(SkusInput skusInput);

		// Token: 0x06000030 RID: 48
		public abstract RequestOutput GetPurchaseUpdates(ResetInput resetInput);

		// Token: 0x06000031 RID: 49
		public abstract void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput);

		// Token: 0x06000032 RID: 50
		public abstract void UnityFireEvent(string jsonMessage);

		// Token: 0x06000033 RID: 51
		public abstract void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		// Token: 0x06000034 RID: 52
		public abstract void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		// Token: 0x06000035 RID: 53
		public abstract void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		// Token: 0x06000036 RID: 54
		public abstract void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		// Token: 0x06000037 RID: 55
		public abstract void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		// Token: 0x06000038 RID: 56
		public abstract void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		// Token: 0x06000039 RID: 57
		public abstract void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);

		// Token: 0x0600003A RID: 58
		public abstract void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);

		// Token: 0x04000021 RID: 33
		private static AmazonLogger logger;

		// Token: 0x04000022 RID: 34
		private static readonly Dictionary<string, IDelegator> callbackDictionary = new Dictionary<string, IDelegator>();

		// Token: 0x04000023 RID: 35
		private static readonly object callbackLock = new object();

		// Token: 0x04000024 RID: 36
		private static readonly Dictionary<string, List<IDelegator>> eventListeners = new Dictionary<string, List<IDelegator>>();

		// Token: 0x04000025 RID: 37
		private static readonly object eventLock = new object();

		// Token: 0x0200000C RID: 12
		private abstract class AmazonIapV2Base : AmazonIapV2Impl
		{
			// Token: 0x0600003B RID: 59 RVA: 0x000031BC File Offset: 0x000013BC
			public AmazonIapV2Base()
			{
				AmazonIapV2Impl.logger = new AmazonLogger(base.GetType().Name);
			}

			// Token: 0x0600003D RID: 61 RVA: 0x000031F0 File Offset: 0x000013F0
			protected void Start()
			{
				if (AmazonIapV2Impl.AmazonIapV2Base.startCalled)
				{
					return;
				}
				object obj = AmazonIapV2Impl.AmazonIapV2Base.startLock;
				lock (obj)
				{
					if (!AmazonIapV2Impl.AmazonIapV2Base.startCalled)
					{
						this.Init();
						this.RegisterCallback();
						this.RegisterEventListener();
						this.RegisterCrossPlatformTool();
						AmazonIapV2Impl.AmazonIapV2Base.startCalled = true;
					}
				}
			}

			// Token: 0x0600003E RID: 62
			protected abstract void Init();

			// Token: 0x0600003F RID: 63
			protected abstract void RegisterCallback();

			// Token: 0x06000040 RID: 64
			protected abstract void RegisterEventListener();

			// Token: 0x06000041 RID: 65
			protected abstract void RegisterCrossPlatformTool();

			// Token: 0x06000042 RID: 66 RVA: 0x0000326C File Offset: 0x0000146C
			public override void UnityFireEvent(string jsonMessage)
			{
				AmazonIapV2Impl.FireEvent(jsonMessage);
			}

			// Token: 0x06000043 RID: 67 RVA: 0x00003274 File Offset: 0x00001474
			public override RequestOutput GetUserData()
			{
				this.Start();
				return RequestOutput.CreateFromJson(this.GetUserDataJson("{}"));
			}

			// Token: 0x06000044 RID: 68 RVA: 0x0000328C File Offset: 0x0000148C
			private string GetUserDataJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = this.NativeGetUserDataJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return result;
			}

			// Token: 0x06000045 RID: 69
			protected abstract string NativeGetUserDataJson(string jsonMessage);

			// Token: 0x06000046 RID: 70 RVA: 0x000032D4 File Offset: 0x000014D4
			public override RequestOutput Purchase(SkuInput skuInput)
			{
				this.Start();
				return RequestOutput.CreateFromJson(this.PurchaseJson(skuInput.ToJson()));
			}

			// Token: 0x06000047 RID: 71 RVA: 0x000032F0 File Offset: 0x000014F0
			private string PurchaseJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = this.NativePurchaseJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return result;
			}

			// Token: 0x06000048 RID: 72
			protected abstract string NativePurchaseJson(string jsonMessage);

			// Token: 0x06000049 RID: 73 RVA: 0x00003338 File Offset: 0x00001538
			public override RequestOutput GetProductData(SkusInput skusInput)
			{
				this.Start();
				return RequestOutput.CreateFromJson(this.GetProductDataJson(skusInput.ToJson()));
			}

			// Token: 0x0600004A RID: 74 RVA: 0x00003354 File Offset: 0x00001554
			private string GetProductDataJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = this.NativeGetProductDataJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return result;
			}

			// Token: 0x0600004B RID: 75
			protected abstract string NativeGetProductDataJson(string jsonMessage);

			// Token: 0x0600004C RID: 76 RVA: 0x0000339C File Offset: 0x0000159C
			public override RequestOutput GetPurchaseUpdates(ResetInput resetInput)
			{
				this.Start();
				return RequestOutput.CreateFromJson(this.GetPurchaseUpdatesJson(resetInput.ToJson()));
			}

			// Token: 0x0600004D RID: 77 RVA: 0x000033B8 File Offset: 0x000015B8
			private string GetPurchaseUpdatesJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = this.NativeGetPurchaseUpdatesJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return result;
			}

			// Token: 0x0600004E RID: 78
			protected abstract string NativeGetPurchaseUpdatesJson(string jsonMessage);

			// Token: 0x0600004F RID: 79 RVA: 0x00003400 File Offset: 0x00001600
			public override void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput)
			{
				this.Start();
				Jsonable.CheckForErrors(Json.Deserialize(this.NotifyFulfillmentJson(notifyFulfillmentInput.ToJson())) as Dictionary<string, object>);
			}

			// Token: 0x06000050 RID: 80 RVA: 0x00003430 File Offset: 0x00001630
			private string NotifyFulfillmentJson(string jsonMessage)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				string result = this.NativeNotifyFulfillmentJson(jsonMessage);
				stopwatch.Stop();
				AmazonIapV2Impl.logger.Debug(string.Format("Successfully called native code in {0} ms", stopwatch.ElapsedMilliseconds));
				return result;
			}

			// Token: 0x06000051 RID: 81
			protected abstract string NativeNotifyFulfillmentJson(string jsonMessage);

			// Token: 0x06000052 RID: 82 RVA: 0x00003478 File Offset: 0x00001678
			public override void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate)
			{
				this.Start();
				string key = "getUserDataResponse";
				object eventLock = AmazonIapV2Impl.eventLock;
				lock (eventLock)
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(key))
					{
						AmazonIapV2Impl.eventListeners[key].Add(new GetUserDataResponseDelegator(responseDelegate));
					}
					else
					{
						List<IDelegator> list = new List<IDelegator>();
						list.Add(new GetUserDataResponseDelegator(responseDelegate));
						AmazonIapV2Impl.eventListeners.Add(key, list);
					}
				}
			}

			// Token: 0x06000053 RID: 83 RVA: 0x00003510 File Offset: 0x00001710
			public override void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate)
			{
				this.Start();
				string key = "getUserDataResponse";
				object eventLock = AmazonIapV2Impl.eventLock;
				lock (eventLock)
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(key))
					{
						foreach (IDelegator delegator in AmazonIapV2Impl.eventListeners[key])
						{
							GetUserDataResponseDelegator getUserDataResponseDelegator = (GetUserDataResponseDelegator)delegator;
							if (getUserDataResponseDelegator.responseDelegate == responseDelegate)
							{
								AmazonIapV2Impl.eventListeners[key].Remove(getUserDataResponseDelegator);
								break;
							}
						}
					}
				}
			}

			// Token: 0x06000054 RID: 84 RVA: 0x000035F0 File Offset: 0x000017F0
			public override void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate)
			{
				this.Start();
				string key = "purchaseResponse";
				object eventLock = AmazonIapV2Impl.eventLock;
				lock (eventLock)
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(key))
					{
						AmazonIapV2Impl.eventListeners[key].Add(new PurchaseResponseDelegator(responseDelegate));
					}
					else
					{
						List<IDelegator> list = new List<IDelegator>();
						list.Add(new PurchaseResponseDelegator(responseDelegate));
						AmazonIapV2Impl.eventListeners.Add(key, list);
					}
				}
			}

			// Token: 0x06000055 RID: 85 RVA: 0x00003688 File Offset: 0x00001888
			public override void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate)
			{
				this.Start();
				string key = "purchaseResponse";
				object eventLock = AmazonIapV2Impl.eventLock;
				lock (eventLock)
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(key))
					{
						foreach (IDelegator delegator in AmazonIapV2Impl.eventListeners[key])
						{
							PurchaseResponseDelegator purchaseResponseDelegator = (PurchaseResponseDelegator)delegator;
							if (purchaseResponseDelegator.responseDelegate == responseDelegate)
							{
								AmazonIapV2Impl.eventListeners[key].Remove(purchaseResponseDelegator);
								break;
							}
						}
					}
				}
			}

			// Token: 0x06000056 RID: 86 RVA: 0x00003768 File Offset: 0x00001968
			public override void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate)
			{
				this.Start();
				string key = "getProductDataResponse";
				object eventLock = AmazonIapV2Impl.eventLock;
				lock (eventLock)
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(key))
					{
						AmazonIapV2Impl.eventListeners[key].Add(new GetProductDataResponseDelegator(responseDelegate));
					}
					else
					{
						List<IDelegator> list = new List<IDelegator>();
						list.Add(new GetProductDataResponseDelegator(responseDelegate));
						AmazonIapV2Impl.eventListeners.Add(key, list);
					}
				}
			}

			// Token: 0x06000057 RID: 87 RVA: 0x00003800 File Offset: 0x00001A00
			public override void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate)
			{
				this.Start();
				string key = "getProductDataResponse";
				object eventLock = AmazonIapV2Impl.eventLock;
				lock (eventLock)
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(key))
					{
						foreach (IDelegator delegator in AmazonIapV2Impl.eventListeners[key])
						{
							GetProductDataResponseDelegator getProductDataResponseDelegator = (GetProductDataResponseDelegator)delegator;
							if (getProductDataResponseDelegator.responseDelegate == responseDelegate)
							{
								AmazonIapV2Impl.eventListeners[key].Remove(getProductDataResponseDelegator);
								break;
							}
						}
					}
				}
			}

			// Token: 0x06000058 RID: 88 RVA: 0x000038E0 File Offset: 0x00001AE0
			public override void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate)
			{
				this.Start();
				string key = "getPurchaseUpdatesResponse";
				object eventLock = AmazonIapV2Impl.eventLock;
				lock (eventLock)
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(key))
					{
						AmazonIapV2Impl.eventListeners[key].Add(new GetPurchaseUpdatesResponseDelegator(responseDelegate));
					}
					else
					{
						List<IDelegator> list = new List<IDelegator>();
						list.Add(new GetPurchaseUpdatesResponseDelegator(responseDelegate));
						AmazonIapV2Impl.eventListeners.Add(key, list);
					}
				}
			}

			// Token: 0x06000059 RID: 89 RVA: 0x00003978 File Offset: 0x00001B78
			public override void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate)
			{
				this.Start();
				string key = "getPurchaseUpdatesResponse";
				object eventLock = AmazonIapV2Impl.eventLock;
				lock (eventLock)
				{
					if (AmazonIapV2Impl.eventListeners.ContainsKey(key))
					{
						foreach (IDelegator delegator in AmazonIapV2Impl.eventListeners[key])
						{
							GetPurchaseUpdatesResponseDelegator getPurchaseUpdatesResponseDelegator = (GetPurchaseUpdatesResponseDelegator)delegator;
							if (getPurchaseUpdatesResponseDelegator.responseDelegate == responseDelegate)
							{
								AmazonIapV2Impl.eventListeners[key].Remove(getPurchaseUpdatesResponseDelegator);
								break;
							}
						}
					}
				}
			}

			// Token: 0x04000026 RID: 38
			private static readonly object startLock = new object();

			// Token: 0x04000027 RID: 39
			private static volatile bool startCalled = false;
		}

		// Token: 0x0200000D RID: 13
		private class AmazonIapV2Default : AmazonIapV2Impl.AmazonIapV2Base
		{
			// Token: 0x0600005B RID: 91 RVA: 0x00003A60 File Offset: 0x00001C60
			protected override void Init()
			{
			}

			// Token: 0x0600005C RID: 92 RVA: 0x00003A64 File Offset: 0x00001C64
			protected override void RegisterCallback()
			{
			}

			// Token: 0x0600005D RID: 93 RVA: 0x00003A68 File Offset: 0x00001C68
			protected override void RegisterEventListener()
			{
			}

			// Token: 0x0600005E RID: 94 RVA: 0x00003A6C File Offset: 0x00001C6C
			protected override void RegisterCrossPlatformTool()
			{
			}

			// Token: 0x0600005F RID: 95 RVA: 0x00003A70 File Offset: 0x00001C70
			protected override string NativeGetUserDataJson(string jsonMessage)
			{
				return "{}";
			}

			// Token: 0x06000060 RID: 96 RVA: 0x00003A78 File Offset: 0x00001C78
			protected override string NativePurchaseJson(string jsonMessage)
			{
				return "{}";
			}

			// Token: 0x06000061 RID: 97 RVA: 0x00003A80 File Offset: 0x00001C80
			protected override string NativeGetProductDataJson(string jsonMessage)
			{
				return "{}";
			}

			// Token: 0x06000062 RID: 98 RVA: 0x00003A88 File Offset: 0x00001C88
			protected override string NativeGetPurchaseUpdatesJson(string jsonMessage)
			{
				return "{}";
			}

			// Token: 0x06000063 RID: 99 RVA: 0x00003A90 File Offset: 0x00001C90
			protected override string NativeNotifyFulfillmentJson(string jsonMessage)
			{
				return "{}";
			}
		}

		// Token: 0x0200000E RID: 14
		private abstract class AmazonIapV2DelegatesBase : AmazonIapV2Impl.AmazonIapV2Base
		{
			// Token: 0x06000065 RID: 101 RVA: 0x00003AA0 File Offset: 0x00001CA0
			protected override void Init()
			{
				this.NativeInit();
			}

			// Token: 0x06000066 RID: 102 RVA: 0x00003AA8 File Offset: 0x00001CA8
			protected override void RegisterCallback()
			{
				this.callbackDelegate = new AmazonIapV2Impl.CallbackDelegate(AmazonIapV2Impl.callback);
				this.NativeRegisterCallback(this.callbackDelegate);
			}

			// Token: 0x06000067 RID: 103 RVA: 0x00003AC8 File Offset: 0x00001CC8
			protected override void RegisterEventListener()
			{
				this.eventDelegate = new AmazonIapV2Impl.CallbackDelegate(AmazonIapV2Impl.FireEvent);
				this.NativeRegisterEventListener(this.eventDelegate);
			}

			// Token: 0x06000068 RID: 104 RVA: 0x00003AE8 File Offset: 0x00001CE8
			protected override void RegisterCrossPlatformTool()
			{
				this.NativeRegisterCrossPlatformTool("XAMARIN");
			}

			// Token: 0x06000069 RID: 105 RVA: 0x00003AF8 File Offset: 0x00001CF8
			public override void UnityFireEvent(string jsonMessage)
			{
				throw new NotSupportedException("UnityFireEvent is not supported");
			}

			// Token: 0x0600006A RID: 106
			protected abstract void NativeInit();

			// Token: 0x0600006B RID: 107
			protected abstract void NativeRegisterCallback(AmazonIapV2Impl.CallbackDelegate callback);

			// Token: 0x0600006C RID: 108
			protected abstract void NativeRegisterEventListener(AmazonIapV2Impl.CallbackDelegate callback);

			// Token: 0x0600006D RID: 109
			protected abstract void NativeRegisterCrossPlatformTool(string crossPlatformTool);

			// Token: 0x04000028 RID: 40
			private const string CrossPlatformTool = "XAMARIN";

			// Token: 0x04000029 RID: 41
			protected AmazonIapV2Impl.CallbackDelegate callbackDelegate;

			// Token: 0x0400002A RID: 42
			protected AmazonIapV2Impl.CallbackDelegate eventDelegate;
		}

		// Token: 0x0200000F RID: 15
		private class Builder
		{
			// Token: 0x0400002B RID: 43
			internal static readonly IAmazonIapV2 instance = AmazonIapV2Impl.AmazonIapV2UnityAndroid.Instance;
		}

		// Token: 0x02000010 RID: 16
		private class AmazonIapV2UnityAndroid : AmazonIapV2Impl.AmazonIapV2UnityBase
		{
			// Token: 0x06000071 RID: 113
			[DllImport("AmazonIapV2Bridge")]
			private static extern string nativeRegisterCallbackGameObject(string name);

			// Token: 0x06000072 RID: 114
			[DllImport("AmazonIapV2Bridge")]
			private static extern string nativeInit();

			// Token: 0x06000073 RID: 115
			[DllImport("AmazonIapV2Bridge")]
			private static extern string nativeGetUserDataJson(string jsonMessage);

			// Token: 0x06000074 RID: 116
			[DllImport("AmazonIapV2Bridge")]
			private static extern string nativePurchaseJson(string jsonMessage);

			// Token: 0x06000075 RID: 117
			[DllImport("AmazonIapV2Bridge")]
			private static extern string nativeGetProductDataJson(string jsonMessage);

			// Token: 0x06000076 RID: 118
			[DllImport("AmazonIapV2Bridge")]
			private static extern string nativeGetPurchaseUpdatesJson(string jsonMessage);

			// Token: 0x06000077 RID: 119
			[DllImport("AmazonIapV2Bridge")]
			private static extern string nativeNotifyFulfillmentJson(string jsonMessage);

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000078 RID: 120 RVA: 0x00003B20 File Offset: 0x00001D20
			public new static AmazonIapV2Impl.AmazonIapV2UnityAndroid Instance
			{
				get
				{
					return AmazonIapV2Impl.AmazonIapV2UnityBase.getInstance<AmazonIapV2Impl.AmazonIapV2UnityAndroid>();
				}
			}

			// Token: 0x06000079 RID: 121 RVA: 0x00003B28 File Offset: 0x00001D28
			protected override void NativeInit()
			{
				AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeInit();
			}

			// Token: 0x0600007A RID: 122 RVA: 0x00003B30 File Offset: 0x00001D30
			protected override void RegisterCallback()
			{
				AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeRegisterCallbackGameObject(base.gameObject.name);
			}

			// Token: 0x0600007B RID: 123 RVA: 0x00003B44 File Offset: 0x00001D44
			protected override void RegisterEventListener()
			{
				AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeRegisterCallbackGameObject(base.gameObject.name);
			}

			// Token: 0x0600007C RID: 124 RVA: 0x00003B58 File Offset: 0x00001D58
			protected override void NativeRegisterCrossPlatformTool(string crossPlatformTool)
			{
			}

			// Token: 0x0600007D RID: 125 RVA: 0x00003B5C File Offset: 0x00001D5C
			protected override string NativeGetUserDataJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeGetUserDataJson(jsonMessage);
			}

			// Token: 0x0600007E RID: 126 RVA: 0x00003B64 File Offset: 0x00001D64
			protected override string NativePurchaseJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativePurchaseJson(jsonMessage);
			}

			// Token: 0x0600007F RID: 127 RVA: 0x00003B6C File Offset: 0x00001D6C
			protected override string NativeGetProductDataJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeGetProductDataJson(jsonMessage);
			}

			// Token: 0x06000080 RID: 128 RVA: 0x00003B74 File Offset: 0x00001D74
			protected override string NativeGetPurchaseUpdatesJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeGetPurchaseUpdatesJson(jsonMessage);
			}

			// Token: 0x06000081 RID: 129 RVA: 0x00003B7C File Offset: 0x00001D7C
			protected override string NativeNotifyFulfillmentJson(string jsonMessage)
			{
				return AmazonIapV2Impl.AmazonIapV2UnityAndroid.nativeNotifyFulfillmentJson(jsonMessage);
			}
		}

		// Token: 0x02000011 RID: 17
		private abstract class AmazonIapV2UnityBase : AmazonIapV2Impl.AmazonIapV2Base
		{
			// Token: 0x06000084 RID: 132 RVA: 0x00003BA0 File Offset: 0x00001DA0
			public static T getInstance<T>() where T : AmazonIapV2Impl.AmazonIapV2UnityBase
			{
				if (AmazonIapV2Impl.AmazonIapV2UnityBase.quit)
				{
					return (T)((object)null);
				}
				if (AmazonIapV2Impl.AmazonIapV2UnityBase.instance != null)
				{
					return (T)((object)AmazonIapV2Impl.AmazonIapV2UnityBase.instance);
				}
				object obj = AmazonIapV2Impl.AmazonIapV2UnityBase.initLock;
				T result;
				lock (obj)
				{
					Type typeFromHandle = typeof(T);
					AmazonIapV2Impl.AmazonIapV2UnityBase.assertTrue(AmazonIapV2Impl.AmazonIapV2UnityBase.instance == null || (AmazonIapV2Impl.AmazonIapV2UnityBase.instance != null && AmazonIapV2Impl.AmazonIapV2UnityBase.instanceType == typeFromHandle), "Only 1 instance of 1 subtype of AmazonIapV2UnityBase can exist.");
					if (AmazonIapV2Impl.AmazonIapV2UnityBase.instance == null)
					{
						AmazonIapV2Impl.AmazonIapV2UnityBase.instanceType = typeFromHandle;
						GameObject gameObject = new GameObject();
						AmazonIapV2Impl.AmazonIapV2UnityBase.instance = gameObject.AddComponent<T>();
						gameObject.name = typeFromHandle.ToString() + "_Singleton";
						UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
					result = (T)((object)AmazonIapV2Impl.AmazonIapV2UnityBase.instance);
				}
				return result;
			}

			// Token: 0x06000085 RID: 133 RVA: 0x00003CAC File Offset: 0x00001EAC
			public void OnDestroy()
			{
				AmazonIapV2Impl.AmazonIapV2UnityBase.quit = true;
			}

			// Token: 0x06000086 RID: 134 RVA: 0x00003CB8 File Offset: 0x00001EB8
			private static void assertTrue(bool statement, string errorMessage)
			{
				if (!statement)
				{
					throw new AmazonException("FATAL: An internal error occurred", new InvalidOperationException(errorMessage));
				}
			}

			// Token: 0x06000087 RID: 135 RVA: 0x00003CD4 File Offset: 0x00001ED4
			protected override void Init()
			{
				this.NativeInit();
			}

			// Token: 0x06000088 RID: 136 RVA: 0x00003CDC File Offset: 0x00001EDC
			protected override void RegisterCrossPlatformTool()
			{
				this.NativeRegisterCrossPlatformTool("UNITY");
			}

			// Token: 0x06000089 RID: 137
			protected abstract void NativeInit();

			// Token: 0x0600008A RID: 138
			protected abstract void NativeRegisterCrossPlatformTool(string crossPlatformTool);

			// Token: 0x0400002C RID: 44
			private const string CrossPlatformTool = "UNITY";

			// Token: 0x0400002D RID: 45
			private static AmazonIapV2Impl.AmazonIapV2UnityBase instance;

			// Token: 0x0400002E RID: 46
			private static Type instanceType;

			// Token: 0x0400002F RID: 47
			private static volatile bool quit = false;

			// Token: 0x04000030 RID: 48
			private static object initLock = new object();
		}

		// Token: 0x02000113 RID: 275
		// (Invoke) Token: 0x06000A12 RID: 2578
		protected delegate void CallbackDelegate(string jsonMessage);
	}
}
