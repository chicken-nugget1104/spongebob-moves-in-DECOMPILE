using System;

// Token: 0x02000392 RID: 914
public class SoaringConnection
{
	// Token: 0x06001A0A RID: 6666 RVA: 0x000AABBC File Offset: 0x000A8DBC
	public static void BeginUpdates()
	{
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x000AABC0 File Offset: 0x000A8DC0
	public static void EndUpdates()
	{
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x000AABC4 File Offset: 0x000A8DC4
	public virtual bool Create(SCWebQueue.SCData properties)
	{
		this.mProperties = properties;
		return false;
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x000AABD0 File Offset: 0x000A8DD0
	public virtual bool IsDone()
	{
		return false;
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x000AABD4 File Offset: 0x000A8DD4
	public virtual bool SaveData()
	{
		return false;
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06001A0F RID: 6671 RVA: 0x000AABD8 File Offset: 0x000A8DD8
	public virtual bool HasError
	{
		get
		{
			return this.mError != null;
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x06001A10 RID: 6672 RVA: 0x000AABE8 File Offset: 0x000A8DE8
	public virtual string Error
	{
		get
		{
			return this.mError;
		}
	}

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x06001A11 RID: 6673 RVA: 0x000AABF0 File Offset: 0x000A8DF0
	public virtual int ErrorCode
	{
		get
		{
			return this.mErrorCode;
		}
	}

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x06001A12 RID: 6674 RVA: 0x000AABF8 File Offset: 0x000A8DF8
	public virtual float Progress
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x06001A13 RID: 6675 RVA: 0x000AAC00 File Offset: 0x000A8E00
	public virtual long Length
	{
		get
		{
			return 0L;
		}
	}

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x06001A14 RID: 6676 RVA: 0x000AAC04 File Offset: 0x000A8E04
	public virtual bool IsValid
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x06001A15 RID: 6677 RVA: 0x000AAC08 File Offset: 0x000A8E08
	public virtual string ContentAsText
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06001A16 RID: 6678 RVA: 0x000AAC0C File Offset: 0x000A8E0C
	public virtual byte[] Content
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06001A17 RID: 6679 RVA: 0x000AAC10 File Offset: 0x000A8E10
	public virtual int CacheVersion
	{
		get
		{
			return -1;
		}
	}

	// Token: 0x040010E6 RID: 4326
	protected string mError;

	// Token: 0x040010E7 RID: 4327
	protected int mErrorCode;

	// Token: 0x040010E8 RID: 4328
	protected SCWebQueue.SCData mProperties;
}
