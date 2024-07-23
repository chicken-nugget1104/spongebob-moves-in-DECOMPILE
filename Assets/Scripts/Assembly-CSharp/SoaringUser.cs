using System;

// Token: 0x020003CC RID: 972
public class SoaringUser : SoaringObjectBase
{
	// Token: 0x06001CED RID: 7405 RVA: 0x000B7610 File Offset: 0x000B5810
	public SoaringUser() : base(SoaringObjectBase.IsType.Object)
	{
	}

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06001CEE RID: 7406 RVA: 0x000B761C File Offset: 0x000B581C
	public virtual bool IsFriend
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06001CEF RID: 7407 RVA: 0x000B7620 File Offset: 0x000B5820
	public void SetUserData(SoaringDictionary userData)
	{
		this.SetUserData(userData, false);
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x000B762C File Offset: 0x000B582C
	public void SetUserData(SoaringDictionary userData, bool clearExisting)
	{
		if (userData == null)
		{
			return;
		}
		if (this.mUserData == null || clearExisting)
		{
			this.mUserData = userData;
		}
		else
		{
			int num = userData.count();
			string[] array = userData.allKeys();
			SoaringObjectBase[] array2 = userData.allValues();
			for (int i = 0; i < num; i++)
			{
				this.mUserData.setValue(array2[i], array[i]);
			}
		}
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x000B7698 File Offset: 0x000B5898
	public void SetUserInfo(SoaringValue val, string key)
	{
		if (string.IsNullOrEmpty(key) || val == null)
		{
			return;
		}
		if (this.mUserData == null)
		{
			this.mUserData = new SoaringDictionary();
		}
		this.mUserData.addValue(val, key);
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x000B76D0 File Offset: 0x000B58D0
	public SoaringValue GetUserInfo(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return null;
		}
		if (this.mUserData == null)
		{
			return null;
		}
		return this.mUserData.soaringValue(key);
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x000B7704 File Offset: 0x000B5904
	// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x000B7748 File Offset: 0x000B5948
	public string UserID
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("userId");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
		set
		{
			if (this.mUserData == null)
			{
				return;
			}
			this.mUserData.setValue(value, "userId");
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x000B7778 File Offset: 0x000B5978
	// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x000B77BC File Offset: 0x000B59BC
	public string UserTag
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("tag");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
		set
		{
			if (this.mUserData == null)
			{
				return;
			}
			this.mUserData.setValue(value, "tag");
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x000B77EC File Offset: 0x000B59EC
	// (set) Token: 0x06001CF8 RID: 7416 RVA: 0x000B7830 File Offset: 0x000B5A30
	public string PictureUrl
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("pictureUrl");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
		set
		{
			if (this.mUserData == null)
			{
				return;
			}
			this.mUserData.setValue(value, "pictureUrl");
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x000B7860 File Offset: 0x000B5A60
	// (set) Token: 0x06001CFA RID: 7418 RVA: 0x000B78A0 File Offset: 0x000B5AA0
	public int Score
	{
		get
		{
			if (this.mUserData == null)
			{
				return 0;
			}
			SoaringValue soaringValue = this.mUserData.soaringValue("score");
			if (soaringValue == null)
			{
				soaringValue = 0;
			}
			return soaringValue;
		}
		set
		{
			if (this.mUserData == null)
			{
				return;
			}
			this.mUserData.setValue(value, "score");
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x06001CFB RID: 7419 RVA: 0x000B78D0 File Offset: 0x000B5AD0
	public string UserStatus
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("status");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06001CFC RID: 7420 RVA: 0x000B7914 File Offset: 0x000B5B14
	public string UserEmail
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("email");
			if (string.IsNullOrEmpty(text))
			{
				SoaringArray soaringArray = (SoaringArray)this.mUserData.objectWithKey("emails");
				if (soaringArray != null && soaringArray.count() != 0)
				{
					SoaringValue soaringValue = (SoaringValue)soaringArray.objectAtIndex(0);
					if (soaringValue != null)
					{
						text = soaringValue;
					}
				}
				if (text == null)
				{
					text = string.Empty;
				}
			}
			return text;
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06001CFD RID: 7421 RVA: 0x000B79A4 File Offset: 0x000B5BA4
	// (set) Token: 0x06001CFE RID: 7422 RVA: 0x000B79EC File Offset: 0x000B5BEC
	public string FacebookID
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			SoaringValue soaringValue = this.mUserData.soaringValue("facebookId");
			if (soaringValue == null)
			{
				soaringValue = string.Empty;
			}
			return soaringValue;
		}
		set
		{
			if (this.mUserData == null)
			{
				return;
			}
			this.mUserData.setValue(value, "facebookId");
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06001CFF RID: 7423 RVA: 0x000B7A1C File Offset: 0x000B5C1C
	// (set) Token: 0x06001D00 RID: 7424 RVA: 0x000B7A64 File Offset: 0x000B5C64
	public string Name
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			SoaringValue soaringValue = this.mUserData.soaringValue("name");
			if (soaringValue == null)
			{
				soaringValue = string.Empty;
			}
			return soaringValue;
		}
		set
		{
			if (this.mUserData == null)
			{
				return;
			}
			this.mUserData.setValue(value, "name");
		}
	}

	// Token: 0x170003CA RID: 970
	// (get) Token: 0x06001D01 RID: 7425 RVA: 0x000B7A94 File Offset: 0x000B5C94
	public string UserGameSesssionID
	{
		get
		{
			string text = null;
			SoaringDictionary soaringDictionary = this.PublicData;
			if (soaringDictionary == null)
			{
				soaringDictionary = this.CustomData;
				if (this.CustomData != null)
				{
					text = soaringDictionary.soaringValue("gameSessionId");
				}
				if (text == null)
				{
					text = string.Empty;
				}
			}
			else
			{
				text = soaringDictionary.soaringValue("gameSessionId");
				if (text == null)
				{
					text = string.Empty;
				}
			}
			return text;
		}
	}

	// Token: 0x170003CB RID: 971
	// (get) Token: 0x06001D02 RID: 7426 RVA: 0x000B7B04 File Offset: 0x000B5D04
	public SoaringDictionary CustomData
	{
		get
		{
			if (this.mUserData == null)
			{
				return null;
			}
			return (SoaringDictionary)this.mUserData.objectWithKey("custom");
		}
	}

	// Token: 0x170003CC RID: 972
	// (get) Token: 0x06001D03 RID: 7427 RVA: 0x000B7B38 File Offset: 0x000B5D38
	public SoaringDictionary PublicData
	{
		get
		{
			if (this.mUserData == null)
			{
				return null;
			}
			SoaringDictionary customData = this.CustomData;
			if (customData == null)
			{
				return null;
			}
			return (SoaringDictionary)customData.objectWithKey("public");
		}
	}

	// Token: 0x170003CD RID: 973
	// (get) Token: 0x06001D04 RID: 7428 RVA: 0x000B7B74 File Offset: 0x000B5D74
	public SoaringDictionary PublicData_Safe
	{
		get
		{
			if (this.mUserData == null)
			{
				return null;
			}
			SoaringDictionary soaringDictionary = this.CustomData;
			if (soaringDictionary == null)
			{
				soaringDictionary = new SoaringDictionary();
				this.mUserData.addValue(new SoaringDictionary(), "custom");
			}
			SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("public");
			if (soaringDictionary2 == null)
			{
				soaringDictionary2 = new SoaringDictionary();
				soaringDictionary.addValue(soaringDictionary2, "public");
			}
			return soaringDictionary2;
		}
	}

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x06001D05 RID: 7429 RVA: 0x000B7BE0 File Offset: 0x000B5DE0
	public SoaringDictionary PrivateData
	{
		get
		{
			if (this.mUserData == null)
			{
				return null;
			}
			SoaringDictionary customData = this.CustomData;
			if (customData == null)
			{
				return null;
			}
			return (SoaringDictionary)customData.objectWithKey("private");
		}
	}

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x06001D06 RID: 7430 RVA: 0x000B7C1C File Offset: 0x000B5E1C
	public SoaringDictionary PrivateData_Safe
	{
		get
		{
			if (this.mUserData == null)
			{
				return null;
			}
			SoaringDictionary soaringDictionary = this.CustomData;
			if (soaringDictionary == null)
			{
				soaringDictionary = new SoaringDictionary();
				this.mUserData.addValue(new SoaringDictionary(), "custom");
			}
			SoaringDictionary soaringDictionary2 = (SoaringDictionary)soaringDictionary.objectWithKey("private");
			if (soaringDictionary2 == null)
			{
				soaringDictionary2 = new SoaringDictionary();
				soaringDictionary.addValue(soaringDictionary2, "private");
			}
			return soaringDictionary2;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06001D07 RID: 7431 RVA: 0x000B7C88 File Offset: 0x000B5E88
	public SoaringDictionary UserData
	{
		get
		{
			return this.mUserData;
		}
	}

	// Token: 0x04001286 RID: 4742
	protected SoaringDictionary mUserData;
}
