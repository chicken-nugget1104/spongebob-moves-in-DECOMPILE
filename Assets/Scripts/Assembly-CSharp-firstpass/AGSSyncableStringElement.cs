using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public class AGSSyncableStringElement : AGSSyncableElement
{
	// Token: 0x060002D0 RID: 720 RVA: 0x0000CE10 File Offset: 0x0000B010
	public AGSSyncableStringElement(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0000CE1C File Offset: 0x0000B01C
	public AGSSyncableStringElement(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x0000CE28 File Offset: 0x0000B028
	public string GetValue()
	{
		return this.javaObject.Call<string>("getValue", new object[0]);
	}
}
