using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class AGSSyncableElement : AGSSyncable
{
	// Token: 0x060002A6 RID: 678 RVA: 0x0000C84C File Offset: 0x0000AA4C
	public AGSSyncableElement(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x0000C858 File Offset: 0x0000AA58
	public AGSSyncableElement(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0000C864 File Offset: 0x0000AA64
	public long GetTimestamp()
	{
		return this.javaObject.Call<long>("getTimestamp", new object[0]);
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x0000C87C File Offset: 0x0000AA7C
	public Dictionary<string, string> GetMetadata()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject androidJavaObject = this.javaObject.Call<AndroidJavaObject>("getMetadata", new object[0]);
		if (androidJavaObject == null)
		{
			AGSClient.LogGameCircleError("Whispersync element was unable to retrieve metadata java map");
			return dictionary;
		}
		AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("keySet", new object[0]);
		if (androidJavaObject2 == null)
		{
			AGSClient.LogGameCircleError("Whispersync element was unable to retrieve java keyset");
			return dictionary;
		}
		AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("iterator", new object[0]);
		if (androidJavaObject3 == null)
		{
			AGSClient.LogGameCircleError("Whispersync element was unable to retrieve java iterator");
			return dictionary;
		}
		while (androidJavaObject3.Call<bool>("hasNext", new object[0]))
		{
			string text = androidJavaObject3.Call<string>("next", new object[0]);
			if (text != null)
			{
				string text2 = androidJavaObject.Call<string>("get", new object[]
				{
					text
				});
				if (text2 != null)
				{
					dictionary.Add(text, text2);
				}
			}
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return dictionary;
	}
}
