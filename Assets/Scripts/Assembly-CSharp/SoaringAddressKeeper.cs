using System;
using System.Text;
using MTools;

// Token: 0x02000383 RID: 899
public class SoaringAddressKeeper
{
	// Token: 0x060019AC RID: 6572 RVA: 0x000A8CD4 File Offset: 0x000A6ED4
	public SoaringAddressKeeper()
	{
		SoaringInternal.instance.RegisterModule(new SoaringAddressKeeperModule());
		this.Load();
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x000A8D38 File Offset: 0x000A6F38
	private void Load()
	{
		try
		{
			MBinaryReader fileStream = ResourceUtils.GetFileStream("SoaringAK", "Soaring", "addr", 3);
			if (fileStream != null && fileStream.IsOpen())
			{
				byte[] array = fileStream.ReadAllBytes();
				if (array != null)
				{
					string @string = Encoding.UTF8.GetString(array);
					if (@string != null)
					{
						this.mSoaringKeys = new SoaringDictionary(@string);
					}
				}
				fileStream.Close();
			}
		}
		catch
		{
			if (this.mSoaringKeys != null)
			{
				this.mSoaringKeys.clear();
			}
		}
		if (this.mSoaringKeys == null)
		{
			this.mSoaringKeys = new SoaringDictionary();
		}
		this.QuickCopySoaring();
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x000A8DF8 File Offset: 0x000A6FF8
	private void QuickCopySoaring()
	{
		this.mSoaringKeys = this.mSoaringKeys.makeCopy();
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x000A8E0C File Offset: 0x000A700C
	private void Save()
	{
		try
		{
			string s = this.mSoaringKeys.ToJsonString();
			string empty = string.Empty;
			string writePath = ResourceUtils.GetWritePath("SoaringAK.addr", empty + "Soaring", 1);
			MBinaryWriter mbinaryWriter = new MBinaryWriter();
			if (!mbinaryWriter.Open(writePath, true, true))
			{
				throw new Exception();
			}
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			int num = bytes.Length;
			for (int i = 0; i < num; i++)
			{
				mbinaryWriter.Write(bytes[i]);
			}
			mbinaryWriter.Close();
		}
		catch
		{
		}
		this.QuickCopySoaring();
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x000A8EC8 File Offset: 0x000A70C8
	public void SetAddressData(SoaringDictionary data)
	{
		if (data == null)
		{
			return;
		}
		int num = data.count();
		string[] array = data.allKeys();
		SoaringObjectBase[] array2 = data.allValues();
		for (int i = 0; i < num; i++)
		{
			SoaringValue val = new SoaringValue(array2[i].ToString());
			this.mSoaringKeys.setValue(val, array[i]);
		}
		this.Save();
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x000A8F28 File Offset: 0x000A7128
	public void SetSoaringAddressData(SoaringDictionary data)
	{
		this.SetAddressData(data);
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x000A8F34 File Offset: 0x000A7134
	public string Address(SoaringAddressKeeper.AddressKeys name)
	{
		SoaringAddressKeeper.AddressKeys addressKeys = name;
		if (addressKeys == SoaringAddressKeeper.AddressKeys.Review)
		{
			name = SoaringAddressKeeper.AddressKeys.GoogleStoreReview;
		}
		int num = (int)name;
		if (num >= 6)
		{
			return string.Empty;
		}
		return this.mKeyNames[num];
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x000A8F70 File Offset: 0x000A7170
	public string Address(string name)
	{
		if (name == null)
		{
			return string.Empty;
		}
		string text = this.mSoaringKeys.soaringValue(name);
		if (text == null)
		{
			text = string.Empty;
		}
		return text;
	}

	// Token: 0x0400109C RID: 4252
	private string[] mKeyNames = new string[]
	{
		"AK_AppleStoreReview",
		"AK_GoogleStoreReview",
		"AK_AmazonStoreReview",
		"AK_Facebook",
		"AK_Twitter",
		"AK_Homepage"
	};

	// Token: 0x0400109D RID: 4253
	private SoaringDictionary mSoaringKeys;

	// Token: 0x02000384 RID: 900
	public enum AddressKeys
	{
		// Token: 0x0400109F RID: 4255
		AppleStoreReview,
		// Token: 0x040010A0 RID: 4256
		GoogleStoreReview,
		// Token: 0x040010A1 RID: 4257
		AmazonStoreReview,
		// Token: 0x040010A2 RID: 4258
		Facebook,
		// Token: 0x040010A3 RID: 4259
		Twitter,
		// Token: 0x040010A4 RID: 4260
		Homepage,
		// Token: 0x040010A5 RID: 4261
		Review
	}
}
