using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class AGSSyncable : IDisposable
{
	// Token: 0x06000291 RID: 657 RVA: 0x0000C408 File Offset: 0x0000A608
	public AGSSyncable(AmazonJavaWrapper jo)
	{
		this.javaObject = jo;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0000C418 File Offset: 0x0000A618
	public AGSSyncable(AndroidJavaObject jo)
	{
		this.javaObject = new AmazonJavaWrapper(jo);
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0000C42C File Offset: 0x0000A62C
	public void Dispose()
	{
		if (this.javaObject != null)
		{
			this.javaObject.Dispose();
		}
	}

	// Token: 0x06000294 RID: 660 RVA: 0x0000C444 File Offset: 0x0000A644
	protected AmazonJavaWrapper DictionaryToAndroidHashMap(Dictionary<string, string> dictionary)
	{
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap", new object[0]);
		IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
		object[] array = new object[2];
		foreach (KeyValuePair<string, string> keyValuePair in dictionary)
		{
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", new object[]
			{
				keyValuePair.Key
			}))
			{
				using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", new object[]
				{
					keyValuePair.Value
				}))
				{
					array[0] = androidJavaObject2;
					array[1] = androidJavaObject3;
					jvalue[] args = AndroidJNIHelper.CreateJNIArgArray(array);
					AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, args);
				}
			}
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return new AmazonJavaWrapper(androidJavaObject);
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0000C598 File Offset: 0x0000A798
	protected T GetAGSSyncable<T>(AGSSyncable.SyncableMethod method)
	{
		return this.GetAGSSyncable<T>(method, null);
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0000C5A4 File Offset: 0x0000A7A4
	protected T GetAGSSyncable<T>(AGSSyncable.SyncableMethod method, string key)
	{
		AndroidJavaObject androidJavaObject;
		if (key != null)
		{
			androidJavaObject = this.javaObject.Call<AndroidJavaObject>(method.ToString(), new object[]
			{
				key
			});
		}
		else
		{
			androidJavaObject = this.javaObject.Call<AndroidJavaObject>(method.ToString(), new object[0]);
		}
		if (androidJavaObject != null)
		{
			return (T)((object)Activator.CreateInstance(typeof(T), new object[]
			{
				androidJavaObject
			}));
		}
		return default(T);
	}

	// Token: 0x06000297 RID: 663 RVA: 0x0000C628 File Offset: 0x0000A828
	protected HashSet<string> GetHashSet(AGSSyncable.HashSetMethod method)
	{
		AndroidJNI.PushLocalFrame(10);
		HashSet<string> hashSet = new HashSet<string>();
		AndroidJavaObject androidJavaObject = this.javaObject.Call<AndroidJavaObject>(method.ToString(), new object[0]);
		if (androidJavaObject == null)
		{
			return hashSet;
		}
		AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("iterator", new object[0]);
		if (androidJavaObject2 == null)
		{
			return hashSet;
		}
		while (androidJavaObject2.Call<bool>("hasNext", new object[0]))
		{
			string item = androidJavaObject2.Call<string>("next", new object[0]);
			hashSet.Add(item);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return hashSet;
	}

	// Token: 0x040001A4 RID: 420
	protected AmazonJavaWrapper javaObject;

	// Token: 0x0200004E RID: 78
	public enum SyncableMethod
	{
		// Token: 0x040001A6 RID: 422
		getHighestNumber,
		// Token: 0x040001A7 RID: 423
		getLowestNumber,
		// Token: 0x040001A8 RID: 424
		getLatestNumber,
		// Token: 0x040001A9 RID: 425
		getHighNumberList,
		// Token: 0x040001AA RID: 426
		getLowNumberList,
		// Token: 0x040001AB RID: 427
		getLatestNumberList,
		// Token: 0x040001AC RID: 428
		getAccumulatingNumber,
		// Token: 0x040001AD RID: 429
		getLatestString,
		// Token: 0x040001AE RID: 430
		getLatestStringList,
		// Token: 0x040001AF RID: 431
		getStringSet,
		// Token: 0x040001B0 RID: 432
		getMap
	}

	// Token: 0x0200004F RID: 79
	public enum HashSetMethod
	{
		// Token: 0x040001B2 RID: 434
		getHighestNumberKeys,
		// Token: 0x040001B3 RID: 435
		getLowestNumberKeys,
		// Token: 0x040001B4 RID: 436
		getLatestNumberKeys,
		// Token: 0x040001B5 RID: 437
		getHighNumberListKeys,
		// Token: 0x040001B6 RID: 438
		getLowNumberListKeys,
		// Token: 0x040001B7 RID: 439
		getLatestNumberListKeys,
		// Token: 0x040001B8 RID: 440
		getAccumulatingNumberKeys,
		// Token: 0x040001B9 RID: 441
		getLatestStringKeys,
		// Token: 0x040001BA RID: 442
		getLatestStringListKeys,
		// Token: 0x040001BB RID: 443
		getStringSetKeys,
		// Token: 0x040001BC RID: 444
		getMapKeys
	}
}
