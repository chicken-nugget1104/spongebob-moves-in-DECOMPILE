using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class AGSSyncableNumber : AGSSyncableNumberElement
{
	// Token: 0x060002B1 RID: 689 RVA: 0x0000CA30 File Offset: 0x0000AC30
	public AGSSyncableNumber(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x0000CA3C File Offset: 0x0000AC3C
	public AGSSyncableNumber(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0000CA48 File Offset: 0x0000AC48
	public void Set(long val)
	{
		this.javaObject.Call("set", new object[]
		{
			val
		});
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x0000CA6C File Offset: 0x0000AC6C
	public void Set(double val)
	{
		this.javaObject.Call("set", new object[]
		{
			val
		});
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0000CA90 File Offset: 0x0000AC90
	public void Set(int val)
	{
		this.javaObject.Call("set", new object[]
		{
			val
		});
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0000CAB4 File Offset: 0x0000ACB4
	public void Set(string val)
	{
		this.javaObject.Call("set", new object[]
		{
			val
		});
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0000CAD0 File Offset: 0x0000ACD0
	public void Set(long val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0000CAFC File Offset: 0x0000ACFC
	public void Set(double val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x0000CB28 File Offset: 0x0000AD28
	public void Set(int val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0000CB54 File Offset: 0x0000AD54
	public void Set(string val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0000CB88 File Offset: 0x0000AD88
	public bool IsSet()
	{
		return this.javaObject.Call<bool>("isSet", new object[0]);
	}
}
