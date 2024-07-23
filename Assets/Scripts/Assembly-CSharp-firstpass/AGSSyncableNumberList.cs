using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class AGSSyncableNumberList : AGSSyncableList
{
	// Token: 0x060002C2 RID: 706 RVA: 0x0000CC18 File Offset: 0x0000AE18
	public AGSSyncableNumberList(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0000CC24 File Offset: 0x0000AE24
	public AGSSyncableNumberList(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x0000CC30 File Offset: 0x0000AE30
	public void Add(long val)
	{
		this.javaObject.Call("add", new object[]
		{
			val
		});
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x0000CC54 File Offset: 0x0000AE54
	public void Add(double val)
	{
		this.javaObject.Call("add", new object[]
		{
			val
		});
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x0000CC78 File Offset: 0x0000AE78
	public void Add(int val)
	{
		this.javaObject.Call("add", new object[]
		{
			val
		});
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x0000CC9C File Offset: 0x0000AE9C
	public void Add(long val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x0000CCC8 File Offset: 0x0000AEC8
	public void Add(double val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x0000CCF4 File Offset: 0x0000AEF4
	public void Add(int val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0000CD20 File Offset: 0x0000AF20
	public AGSSyncableNumberElement[] GetValues()
	{
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject[] array = this.javaObject.Call<AndroidJavaObject[]>("getValues", new object[0]);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		AGSSyncableNumberElement[] array2 = new AGSSyncableNumberElement[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = new AGSSyncableNumber(array[i]);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return array2;
	}
}
