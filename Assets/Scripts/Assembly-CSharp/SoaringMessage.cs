using System;

// Token: 0x020003C4 RID: 964
public class SoaringMessage : SoaringObjectBase
{
	// Token: 0x06001C9D RID: 7325 RVA: 0x000B5A18 File Offset: 0x000B3C18
	public SoaringMessage() : base(SoaringObjectBase.IsType.Object)
	{
		this.mUsers = new SoaringArray();
		this.mCategory = string.Empty;
		this.mBody = string.Empty;
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x000B5A50 File Offset: 0x000B3C50
	public SoaringMessage(string to, string body, string category) : base(SoaringObjectBase.IsType.Object)
	{
		this.mUsers = new SoaringArray();
		this.AddRecipientTag(to);
		this.SetCategory(category);
		this.mBody = body;
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06001C9F RID: 7327 RVA: 0x000B5A7C File Offset: 0x000B3C7C
	public string Category
	{
		get
		{
			return this.mCategory;
		}
	}

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x000B5A84 File Offset: 0x000B3C84
	public string MessageID
	{
		get
		{
			return this.mMessageID;
		}
	}

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x000B5A8C File Offset: 0x000B3C8C
	public string SenderID
	{
		get
		{
			return this.mSenderID;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x000B5A94 File Offset: 0x000B3C94
	public int SenderDate
	{
		get
		{
			return this.mSendDate;
		}
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x000B5A9C File Offset: 0x000B3C9C
	public string MessageBody
	{
		get
		{
			return this.mBody;
		}
	}

	// Token: 0x06001CA4 RID: 7332 RVA: 0x000B5AA4 File Offset: 0x000B3CA4
	public void SetMessageSendData(int date)
	{
		this.mSendDate = date;
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x000B5AB0 File Offset: 0x000B3CB0
	public void SetMessageID(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return;
		}
		this.mMessageID = id;
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x000B5AC8 File Offset: 0x000B3CC8
	public void SetSenderID(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return;
		}
		this.mSenderID = id;
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x000B5AE0 File Offset: 0x000B3CE0
	public string RecipientUserID
	{
		get
		{
			string result = string.Empty;
			int num = this.mUsers.count();
			for (int i = 0; i < num; i++)
			{
				SoaringDictionary soaringDictionary = (SoaringDictionary)this.mUsers.objectAtIndex(i);
				string text = soaringDictionary.soaringValue("userId");
				if (!string.IsNullOrEmpty(text))
				{
					result = text;
					break;
				}
			}
			return result;
		}
	}

	// Token: 0x06001CA8 RID: 7336 RVA: 0x000B5B4C File Offset: 0x000B3D4C
	public void AddRecipientUserID(string userID)
	{
		if (string.IsNullOrEmpty(userID))
		{
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(userID, "userId");
		this.mUsers.addObject(soaringDictionary);
	}

	// Token: 0x06001CA9 RID: 7337 RVA: 0x000B5B8C File Offset: 0x000B3D8C
	public void AddRecipientInviteCode(string ic)
	{
		if (string.IsNullOrEmpty(ic))
		{
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(ic, "ic");
		this.mUsers.addObject(soaringDictionary);
	}

	// Token: 0x06001CAA RID: 7338 RVA: 0x000B5BCC File Offset: 0x000B3DCC
	public void AddRecipientTag(string tag)
	{
		if (string.IsNullOrEmpty(tag))
		{
			return;
		}
		SoaringDictionary soaringDictionary = new SoaringDictionary(1);
		soaringDictionary.addValue(tag, "tag");
		this.mUsers.addObject(soaringDictionary);
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x000B5C0C File Offset: 0x000B3E0C
	public void SetCategory(string cat)
	{
		if (string.IsNullOrEmpty(cat))
		{
			return;
		}
		this.mCategory = cat;
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x000B5C24 File Offset: 0x000B3E24
	public void SetTextBody(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		this.mBody = text;
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x000B5C3C File Offset: 0x000B3E3C
	public override string ToJsonString()
	{
		return string.Concat(new string[]
		{
			"{\n\"to\" : ",
			this.mUsers.ToJsonString(),
			",\n\"category\" : \"",
			this.mCategory,
			"\",\n\"body\" : \"",
			this.mBody,
			"\"\n}"
		});
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x000B5C98 File Offset: 0x000B3E98
	public override string ToString()
	{
		return string.Format("[SoaringMessage]", new object[0]);
	}

	// Token: 0x0400125E RID: 4702
	private string mSenderID;

	// Token: 0x0400125F RID: 4703
	private string mMessageID;

	// Token: 0x04001260 RID: 4704
	private SoaringArray mUsers;

	// Token: 0x04001261 RID: 4705
	private string mCategory;

	// Token: 0x04001262 RID: 4706
	private string mBody;

	// Token: 0x04001263 RID: 4707
	private int mSendDate;
}
