using System;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class AGSSyncableStringList : AGSSyncableList
{
	// Token: 0x060002D3 RID: 723 RVA: 0x0000CE40 File Offset: 0x0000B040
	public AGSSyncableStringList(AmazonJavaWrapper javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x0000CE4C File Offset: 0x0000B04C
	public AGSSyncableStringList(AndroidJavaObject javaObject) : base(javaObject)
	{
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x0000CE58 File Offset: 0x0000B058
	public AGSSyncableString[] GetValues()
	{
		AndroidJNI.PushLocalFrame(10);
		AndroidJavaObject[] array = this.javaObject.Call<AndroidJavaObject[]>("getValues", new object[0]);
		if (array == null || array.Length == 0)
		{
			return null;
		}
		AGSSyncableString[] array2 = new AGSSyncableString[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = new AGSSyncableString(array[i]);
		}
		AndroidJNI.PopLocalFrame(IntPtr.Zero);
		return array2;
	}
}
