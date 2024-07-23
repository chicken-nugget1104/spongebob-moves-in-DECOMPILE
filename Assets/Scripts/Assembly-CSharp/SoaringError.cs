using System;

// Token: 0x020003D1 RID: 977
public class SoaringError : SoaringObjectBase
{
	// Token: 0x06001D5C RID: 7516 RVA: 0x000B8C5C File Offset: 0x000B6E5C
	public SoaringError() : base(SoaringObjectBase.IsType.Object)
	{
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x000B8C6C File Offset: 0x000B6E6C
	public SoaringError(string error, int code) : base(SoaringObjectBase.IsType.Object)
	{
		this.mError = error;
		this.mErrorCode = code;
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x06001D5E RID: 7518 RVA: 0x000B8C8C File Offset: 0x000B6E8C
	public string Error
	{
		get
		{
			return this.mError;
		}
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06001D5F RID: 7519 RVA: 0x000B8C94 File Offset: 0x000B6E94
	public int ErrorCode
	{
		get
		{
			return this.mErrorCode;
		}
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x000B8C9C File Offset: 0x000B6E9C
	public bool InvalidErrorCode()
	{
		return this.ErrorCode == -1;
	}

	// Token: 0x06001D61 RID: 7521 RVA: 0x000B8CA8 File Offset: 0x000B6EA8
	public override string ToJsonString()
	{
		return string.Concat(new string[]
		{
			"{\n\"code\":",
			this.mErrorCode.ToString(),
			",\n\"message\":\"",
			this.mError,
			"\"\n}"
		});
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x000B8CF0 File Offset: 0x000B6EF0
	public static implicit operator SoaringError(int b)
	{
		return new SoaringError(null, b);
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x000B8CFC File Offset: 0x000B6EFC
	public static implicit operator SoaringError(string b)
	{
		return new SoaringError(b, -1);
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x000B8D08 File Offset: 0x000B6F08
	public static implicit operator string(SoaringError b)
	{
		if (b == null)
		{
			return null;
		}
		return b.Error;
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x000B8D18 File Offset: 0x000B6F18
	public static implicit operator int(SoaringError b)
	{
		if (b == null)
		{
			return -1;
		}
		return b.ErrorCode;
	}

	// Token: 0x04001295 RID: 4757
	private string mError;

	// Token: 0x04001296 RID: 4758
	private int mErrorCode = -1;
}
