using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class AmazonJavaWrapper : IDisposable
{
	// Token: 0x06000154 RID: 340 RVA: 0x00006840 File Offset: 0x00004A40
	public AmazonJavaWrapper()
	{
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00006848 File Offset: 0x00004A48
	public AmazonJavaWrapper(AndroidJavaObject o)
	{
		this.setAndroidJavaObject(o);
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00006858 File Offset: 0x00004A58
	public AndroidJavaObject getJavaObject()
	{
		return this.jo;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00006860 File Offset: 0x00004A60
	public void setAndroidJavaObject(AndroidJavaObject o)
	{
		this.jo = o;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000686C File Offset: 0x00004A6C
	public IntPtr GetRawObject()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return this.jo.GetRawObject();
		}
		return 0;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000689C File Offset: 0x00004A9C
	public IntPtr GetRawClass()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return this.jo.GetRawClass();
		}
		return 0;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x000068CC File Offset: 0x00004ACC
	public void Set<FieldType>(string fieldName, FieldType type)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			this.jo.Set<FieldType>(fieldName, type);
		}
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000068E8 File Offset: 0x00004AE8
	public FieldType Get<FieldType>(string fieldName)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return this.jo.Get<FieldType>(fieldName);
		}
		return default(FieldType);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00006918 File Offset: 0x00004B18
	public void SetStatic<FieldType>(string fieldName, FieldType type)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			this.jo.SetStatic<FieldType>(fieldName, type);
		}
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00006934 File Offset: 0x00004B34
	public FieldType GetStatic<FieldType>(string fieldName)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			return this.jo.GetStatic<FieldType>(fieldName);
		}
		return default(FieldType);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00006964 File Offset: 0x00004B64
	public void CallStatic(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame(args.Length + 1);
			this.jo.CallStatic(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00006998 File Offset: 0x00004B98
	public void Call(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame(args.Length + 1);
			this.jo.Call(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x000069CC File Offset: 0x00004BCC
	public ReturnType CallStatic<ReturnType>(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame(args.Length + 1);
			ReturnType result = this.jo.CallStatic<ReturnType>(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return result;
		}
		return default(ReturnType);
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00006A14 File Offset: 0x00004C14
	public ReturnType Call<ReturnType>(string method, params object[] args)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJNI.PushLocalFrame(args.Length + 1);
			ReturnType result = this.jo.Call<ReturnType>(method, args);
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			return result;
		}
		return default(ReturnType);
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00006A5C File Offset: 0x00004C5C
	public void Dispose()
	{
		if (this.jo != null)
		{
			this.jo.Dispose();
		}
	}

	// Token: 0x0400005C RID: 92
	private AndroidJavaObject jo;
}
