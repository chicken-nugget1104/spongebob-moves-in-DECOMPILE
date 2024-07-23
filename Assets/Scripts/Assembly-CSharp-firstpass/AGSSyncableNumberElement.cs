using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class AGSSyncableNumberElement : AGSSyncableElement
{
	// Token: 0x060002BC RID: 700 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
	public AGSSyncableNumberElement(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002BD RID: 701 RVA: 0x0000CBAC File Offset: 0x0000ADAC
	public AGSSyncableNumberElement(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0000CBB8 File Offset: 0x0000ADB8
	public long AsLong()
	{
		return this.javaObject.Call<long>("asLong", new object[0]);
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0000CBD0 File Offset: 0x0000ADD0
	public double AsDouble()
	{
		return this.javaObject.Call<double>("asDouble", new object[0]);
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x0000CBE8 File Offset: 0x0000ADE8
	public int AsInt()
	{
		return this.javaObject.Call<int>("asInt", new object[0]);
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0000CC00 File Offset: 0x0000AE00
	public string AsString()
	{
		return this.javaObject.Call<string>("asString", new object[0]);
	}
}
