using System;

// Token: 0x020003C5 RID: 965
public class SoaringPlayer : SoaringUser
{
	// Token: 0x06001CB1 RID: 7345 RVA: 0x000B5CC4 File Offset: 0x000B3EC4
	public void SetFriendsData(SoaringArray<SoaringUser> users)
	{
		if (users == null)
		{
			return;
		}
		this.mFriends = users;
	}

	// Token: 0x170003B3 RID: 947
	// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x000B5CD4 File Offset: 0x000B3ED4
	public SoaringUser[] Friends
	{
		get
		{
			return this.mFriends.array();
		}
	}

	// Token: 0x170003B4 RID: 948
	// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x000B5CE4 File Offset: 0x000B3EE4
	// (set) Token: 0x06001CB4 RID: 7348 RVA: 0x000B5CEC File Offset: 0x000B3EEC
	public SoaringLoginType LoginType { get; set; }

	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x000B5CF8 File Offset: 0x000B3EF8
	// (set) Token: 0x06001CB6 RID: 7350 RVA: 0x000B5D00 File Offset: 0x000B3F00
	public bool IsLocalAuthorized { get; set; }

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x000B5D0C File Offset: 0x000B3F0C
	public bool HasFriend
	{
		get
		{
			if (this.mFriends == null)
			{
				return false;
			}
			int num = this.mFriends.count();
			for (int i = 0; i <= num; i++)
			{
				if (this.mFriends[i] != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x000B5D58 File Offset: 0x000B3F58
	public string AuthToken
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("authToken");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06001CB9 RID: 7353 RVA: 0x000B5D9C File Offset: 0x000B3F9C
	public string GameCenterID
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("gamecenterId");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06001CBA RID: 7354 RVA: 0x000B5DE0 File Offset: 0x000B3FE0
	public string GoogleID
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("googleId");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06001CBB RID: 7355 RVA: 0x000B5E24 File Offset: 0x000B4024
	public string AmazonID
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("amazonId");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06001CBC RID: 7356 RVA: 0x000B5E68 File Offset: 0x000B4068
	public string Password
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("password");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	// Token: 0x170003BC RID: 956
	// (get) Token: 0x06001CBD RID: 7357 RVA: 0x000B5EAC File Offset: 0x000B40AC
	public string InviteCode
	{
		get
		{
			if (this.mUserData == null)
			{
				return string.Empty;
			}
			string text = this.mUserData.soaringValue("invitationCode");
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06001CBE RID: 7358 RVA: 0x000B5EF0 File Offset: 0x000B40F0
	public bool LightUser
	{
		get
		{
			return this.mUserData == null || this.mUserData.soaringValue("autoCreated");
		}
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x000B5F20 File Offset: 0x000B4120
	public bool Load(string userID = null)
	{
		return SoaringPlayerResolver.Load(this, userID);
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x000B5F2C File Offset: 0x000B412C
	public void Save()
	{
		SoaringPlayerResolver.Save(null);
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x000B5F34 File Offset: 0x000B4134
	public void ClearSavedCredentials()
	{
		bool flag = this.mCanSaveUserCredentials;
		this.mCanSaveUserCredentials = false;
		this.Save();
		this.mCanSaveUserCredentials = flag;
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x000B5F5C File Offset: 0x000B415C
	// (set) Token: 0x06001CC3 RID: 7363 RVA: 0x000B5F64 File Offset: 0x000B4164
	public bool CanSaveUserCredentials
	{
		get
		{
			return this.mCanSaveUserCredentials;
		}
		set
		{
			this.mCanSaveUserCredentials = value;
		}
	}

	// Token: 0x04001264 RID: 4708
	private SoaringArray<SoaringUser> mFriends = new SoaringArray<SoaringUser>();

	// Token: 0x04001265 RID: 4709
	private bool mCanSaveUserCredentials;

	// Token: 0x04001266 RID: 4710
	public static bool ValidCredentials;
}
