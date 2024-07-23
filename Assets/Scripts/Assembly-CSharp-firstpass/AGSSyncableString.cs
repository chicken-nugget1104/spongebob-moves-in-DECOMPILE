using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class AGSSyncableString : AGSSyncableStringElement
{
	// Token: 0x060002CB RID: 715 RVA: 0x0000CD90 File Offset: 0x0000AF90
	public AGSSyncableString(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0000CD9C File Offset: 0x0000AF9C
	public AGSSyncableString(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002CD RID: 717 RVA: 0x0000CDA8 File Offset: 0x0000AFA8
	public void Set(string val)
	{
		this.javaObject.Call("set", new object[]
		{
			val
		});
	}

	// Token: 0x060002CE RID: 718 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
	public void Set(string val, Dictionary<string, string> metadata)
	{
		this.javaObject.Call("set", new object[]
		{
			val,
			base.DictionaryToAndroidHashMap(metadata)
		});
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0000CDF8 File Offset: 0x0000AFF8
	public bool IsSet()
	{
		return this.javaObject.Call<bool>("isSet", new object[0]);
	}
}
