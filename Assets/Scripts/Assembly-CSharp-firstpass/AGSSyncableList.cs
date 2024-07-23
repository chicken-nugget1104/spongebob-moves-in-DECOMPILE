using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000052 RID: 82
public class AGSSyncableList : AGSSyncable
{
	// Token: 0x060002AA RID: 682 RVA: 0x0000C974 File Offset: 0x0000AB74
	public AGSSyncableList(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002AB RID: 683 RVA: 0x0000C980 File Offset: 0x0000AB80
	public AGSSyncableList(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002AC RID: 684 RVA: 0x0000C98C File Offset: 0x0000AB8C
	public void SetMaxSize(int size)
	{
		this.javaObject.Call("setMaxSize", new object[]
		{
			size
		});
	}

	// Token: 0x060002AD RID: 685 RVA: 0x0000C9B0 File Offset: 0x0000ABB0
	public int GetMaxSize()
	{
		return this.javaObject.Call<int>("getMaxSize", new object[0]);
	}

	// Token: 0x060002AE RID: 686 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
	public bool IsSet()
	{
		return this.javaObject.Call<bool>("isSet", new object[0]);
	}

	// Token: 0x060002AF RID: 687 RVA: 0x0000C9E0 File Offset: 0x0000ABE0
	public void Add(string val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("add", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x0000CA14 File Offset: 0x0000AC14
	public void Add(string val)
	{
		this.javaObject.Call("add", new object[]
		{
			val
		});
	}
}
