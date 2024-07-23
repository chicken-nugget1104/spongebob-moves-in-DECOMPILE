using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class AGSSyncableStringSet : AGSSyncable
{
	// Token: 0x060002D6 RID: 726 RVA: 0x0000CEC8 File Offset: 0x0000B0C8
	public AGSSyncableStringSet(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0000CED4 File Offset: 0x0000B0D4
	public AGSSyncableStringSet(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0000CEE0 File Offset: 0x0000B0E0
	public void Add(string val)
	{
		this.javaObject.Call("add", new object[]
		{
			val
		});
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0000CEFC File Offset: 0x0000B0FC
	public void Add(string val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002DA RID: 730 RVA: 0x0000CF30 File Offset: 0x0000B130
	public AGSSyncableStringElement Get(string val)
	{
		return base.GetAGSSyncable<AGSSyncableStringElement>(AGSSyncable.SyncableMethod.getStringSet, val);
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0000CF3C File Offset: 0x0000B13C
	public bool Contains(string val)
	{
		return this.javaObject.Call<bool>("contains", new object[]
		{
			val
		});
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0000CF58 File Offset: 0x0000B158
	public bool IsSet()
	{
		return this.javaObject.Call<bool>("isSet", new object[0]);
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0000CF70 File Offset: 0x0000B170
	public HashSet<AGSSyncableStringElement> GetValues()
	{
		AndroidJNI.PushLocalFrame(10);
		HashSet<AGSSyncableStringElement> hashSet = new HashSet<AGSSyncableStringElement>();
		AndroidJavaObject androidJavaObject = this.javaObject.Call<AndroidJavaObject>("getValues", new object[0]);
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
			AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("next", new object[0]);
			if (androidJavaObject3 != null)
			{
				hashSet.Add(new AGSSyncableStringElement(androidJavaObject3));
			}
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return hashSet;
	}
}
