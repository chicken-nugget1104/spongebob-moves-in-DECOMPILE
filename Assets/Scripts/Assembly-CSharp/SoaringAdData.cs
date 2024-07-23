using System;
using UnityEngine;

// Token: 0x020003C1 RID: 961
public class SoaringAdData : SoaringObjectBase
{
	// Token: 0x06001C88 RID: 7304 RVA: 0x000B586C File Offset: 0x000B3A6C
	public SoaringAdData() : base(SoaringObjectBase.IsType.Object)
	{
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x000B5884 File Offset: 0x000B3A84
	internal void SetData(Texture2D texture, string addID, string path, long starts, long expires, int mAdDisplays, SoaringAdData.SoaringAdType adType, SoaringDictionary localizations)
	{
		this.mAdType = adType;
		this.mTexture = texture;
		this.mAdID = addID;
		this.mAdPath = path;
		this.mAdExpires = expires;
		this.mAdStarts = starts;
		this.mLocalizations = localizations;
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x000B58BC File Offset: 0x000B3ABC
	internal void SetUserData(SoaringDictionary userData)
	{
		this.mUserData = userData;
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x000B58C8 File Offset: 0x000B3AC8
	internal void SetCachedData(short shown, short clicks)
	{
		this.mTimesClicked = clicks;
		this.mTimesShown = shown;
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x000B58D8 File Offset: 0x000B3AD8
	internal void SetAdShown()
	{
		this.mTimesShown += 1;
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x000B58EC File Offset: 0x000B3AEC
	public bool OpenAdPage()
	{
		bool result = false;
		if ((this.mAdType == SoaringAdData.SoaringAdType.Market || this.mAdType == SoaringAdData.SoaringAdType.Web) && !string.IsNullOrEmpty(this.mAdPath))
		{
			Application.OpenURL(this.mAdPath);
			result = true;
		}
		this.mTimesClicked += 1;
		return result;
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x000B5940 File Offset: 0x000B3B40
	public void Invalidate()
	{
		if (this.mTexture != null)
		{
			UnityEngine.Object.Destroy(this.mTexture);
		}
		this.mTexture = null;
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06001C8F RID: 7311 RVA: 0x000B5968 File Offset: 0x000B3B68
	public Texture2D AdTexture
	{
		get
		{
			return this.mTexture;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06001C90 RID: 7312 RVA: 0x000B5970 File Offset: 0x000B3B70
	public string AdID
	{
		get
		{
			return this.mAdID;
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06001C91 RID: 7313 RVA: 0x000B5978 File Offset: 0x000B3B78
	public string Path
	{
		get
		{
			return this.mAdPath;
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x06001C92 RID: 7314 RVA: 0x000B5980 File Offset: 0x000B3B80
	public long AdExpires
	{
		get
		{
			return this.mAdExpires;
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06001C93 RID: 7315 RVA: 0x000B5988 File Offset: 0x000B3B88
	public long AdStarts
	{
		get
		{
			return this.mAdStarts;
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x06001C94 RID: 7316 RVA: 0x000B5990 File Offset: 0x000B3B90
	public SoaringAdData.SoaringAdType AdType
	{
		get
		{
			return this.mAdType;
		}
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x06001C95 RID: 7317 RVA: 0x000B5998 File Offset: 0x000B3B98
	public short TimesWillBeDisplayed
	{
		get
		{
			return this.mAdDisplays;
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06001C96 RID: 7318 RVA: 0x000B59A0 File Offset: 0x000B3BA0
	public short TimesDisplayed
	{
		get
		{
			return this.mTimesShown;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x06001C97 RID: 7319 RVA: 0x000B59A8 File Offset: 0x000B3BA8
	public short TimesClicked
	{
		get
		{
			return this.mTimesClicked;
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x06001C98 RID: 7320 RVA: 0x000B59B0 File Offset: 0x000B3BB0
	public SoaringDictionary UserData
	{
		get
		{
			return this.mUserData;
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06001C99 RID: 7321 RVA: 0x000B59B8 File Offset: 0x000B3BB8
	public SoaringDictionary AdLocalizations
	{
		get
		{
			return this.mLocalizations;
		}
	}

	// Token: 0x0400124C RID: 4684
	private Texture2D mTexture;

	// Token: 0x0400124D RID: 4685
	private string mAdID;

	// Token: 0x0400124E RID: 4686
	private string mAdPath;

	// Token: 0x0400124F RID: 4687
	private long mAdExpires;

	// Token: 0x04001250 RID: 4688
	private long mAdStarts;

	// Token: 0x04001251 RID: 4689
	private short mAdDisplays = -1;

	// Token: 0x04001252 RID: 4690
	private short mTimesShown;

	// Token: 0x04001253 RID: 4691
	private short mTimesClicked;

	// Token: 0x04001254 RID: 4692
	private SoaringDictionary mUserData;

	// Token: 0x04001255 RID: 4693
	private SoaringDictionary mLocalizations;

	// Token: 0x04001256 RID: 4694
	private SoaringAdData.SoaringAdType mAdType = SoaringAdData.SoaringAdType.Local;

	// Token: 0x020003C2 RID: 962
	public enum SoaringAdType
	{
		// Token: 0x04001258 RID: 4696
		Web,
		// Token: 0x04001259 RID: 4697
		Market,
		// Token: 0x0400125A RID: 4698
		Local,
		// Token: 0x0400125B RID: 4699
		Other
	}
}
