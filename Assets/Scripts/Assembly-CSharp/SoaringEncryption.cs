using System;
using UnityEngine;

// Token: 0x02000394 RID: 916
public class SoaringEncryption : SoaringObjectBase
{
	// Token: 0x06001A21 RID: 6689 RVA: 0x000AAF68 File Offset: 0x000A9168
	public SoaringEncryption(string cipher, string digest) : base(SoaringObjectBase.IsType.Object)
	{
		try
		{
			string text = cipher.ToLower();
			if (text.Contains("rc4"))
			{
				this.mEncryptionBits = -1;
				text = text.Replace("rc4", string.Empty).Replace("-", string.Empty);
				if (!int.TryParse(text, out this.mEncryptionBits))
				{
					this.mEncryptionBits = -1;
				}
				if (this.mEncryptionBits != 0)
				{
					this.mEncryptionBits /= 8;
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x000AB04C File Offset: 0x000A924C
	public bool HasExpired()
	{
		return (DateTime.UtcNow - this.mKeyDateStamp).TotalSeconds > (double)this.mMaxTimeForKeys;
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x06001A24 RID: 6692 RVA: 0x000AB07C File Offset: 0x000A927C
	public static string SID
	{
		get
		{
			return SoaringEncryption.EncryptionSID;
		}
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x000AB084 File Offset: 0x000A9284
	public void SetEncryptionKey(byte[] key)
	{
		SoaringEncryption.EncryptionKey = key;
		if (SoaringEncryption.EncryptionKey != null)
		{
			this.mEncrytionKeyTime = DateTime.UtcNow;
		}
		else
		{
			this.mEncrytionKeyTime = new DateTime(1970, 1, 1, 0, 0, 0);
		}
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x000AB0C8 File Offset: 0x000A92C8
	public void SetSID(string sid)
	{
		SoaringEncryption.EncryptionSID = sid;
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x000AB0D0 File Offset: 0x000A92D0
	public static bool IsEncryptionAvailable()
	{
		return SoaringInternalProperties.SecureCommunication && SoaringEncryption.EncryptionKey != null && SoaringEncryption.EncryptionSID != null;
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x000AB104 File Offset: 0x000A9304
	public byte[] Encrypt(byte[] data)
	{
		return SoaringEncryption.RC4.Encrypt(SoaringEncryption.EncryptionKey, data, this.mEncryptionBits, 256);
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x000AB11C File Offset: 0x000A931C
	public byte[] Encrypt(string data)
	{
		return SoaringEncryption.RC4.EncryptString(SoaringEncryption.EncryptionKey, data, this.mEncryptionBits, 256);
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x000AB134 File Offset: 0x000A9334
	public byte[] Decrypt(byte[] data)
	{
		return SoaringEncryption.RC4.Decrypt(SoaringEncryption.EncryptionKey, data, this.mEncryptionBits);
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x000AB148 File Offset: 0x000A9348
	public void StartUsingEncryption()
	{
		this.mKeyDateStamp = this.mEncrytionKeyTime;
	}

	// Token: 0x040010EA RID: 4330
	private static byte[] EncryptionKey;

	// Token: 0x040010EB RID: 4331
	private static string EncryptionSID;

	// Token: 0x040010EC RID: 4332
	private int mEncryptionBits;

	// Token: 0x040010ED RID: 4333
	private DateTime mKeyDateStamp = new DateTime(1970, 1, 1, 0, 0, 0);

	// Token: 0x040010EE RID: 4334
	private DateTime mEncrytionKeyTime = new DateTime(1970, 1, 1, 0, 0, 0);

	// Token: 0x040010EF RID: 4335
	private int mMaxTimeForKeys = 280;

	// Token: 0x02000395 RID: 917
	public class RC4
	{
		// Token: 0x06001A2E RID: 6702 RVA: 0x000AB164 File Offset: 0x000A9364
		private static void AllocateBuffers(int itteration_length)
		{
			if (SoaringEncryption.RC4.mKey == null)
			{
				SoaringEncryption.RC4.mKey = new int[itteration_length];
			}
			else if (SoaringEncryption.RC4.mKey.Length != itteration_length)
			{
				SoaringEncryption.RC4.mKey = new int[itteration_length];
			}
			if (SoaringEncryption.RC4.mBox == null)
			{
				SoaringEncryption.RC4.mBox = new int[itteration_length];
			}
			else if (SoaringEncryption.RC4.mBox.Length != itteration_length)
			{
				SoaringEncryption.RC4.mBox = new int[itteration_length];
			}
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x000AB1D8 File Offset: 0x000A93D8
		public static byte[] Encrypt(byte[] pwd, byte[] data, int key_length = -1, int itterations = 256)
		{
			if (pwd == null || data == null)
			{
				SoaringDebug.Log("Invalid Encrypt Password Or Key", LogType.Error);
				return null;
			}
			int num = itterations - 1;
			SoaringEncryption.RC4.AllocateBuffers(itterations);
			int num2 = pwd.Length;
			if (num2 != -1)
			{
				if (num2 < key_length)
				{
					SoaringDebug.Log("Invalid Key Length: " + key_length.ToString() + " : " + num2.ToString(), LogType.Error);
					return null;
				}
				num2 = key_length;
			}
			byte[] array = new byte[data.Length];
			int i;
			for (i = 0; i < itterations; i++)
			{
				SoaringEncryption.RC4.mKey[i] = (int)pwd[i % num2];
				SoaringEncryption.RC4.mBox[i] = i;
			}
			int num3;
			for (i = (num3 = 0); i < itterations; i++)
			{
				num3 = (num3 + SoaringEncryption.RC4.mBox[i] + SoaringEncryption.RC4.mKey[i] & num);
				int num4 = SoaringEncryption.RC4.mBox[i];
				SoaringEncryption.RC4.mBox[i] = SoaringEncryption.RC4.mBox[num3];
				SoaringEncryption.RC4.mBox[num3] = num4;
			}
			int num5;
			num3 = (num5 = (i = 0));
			while (i < data.Length)
			{
				num5++;
				num5 %= itterations;
				num3 += SoaringEncryption.RC4.mBox[num5];
				num3 %= itterations;
				int num4 = SoaringEncryption.RC4.mBox[num5];
				SoaringEncryption.RC4.mBox[num5] = SoaringEncryption.RC4.mBox[num3];
				SoaringEncryption.RC4.mBox[num3] = num4;
				int num6 = SoaringEncryption.RC4.mBox[SoaringEncryption.RC4.mBox[num5] + SoaringEncryption.RC4.mBox[num3] & num];
				array[i] = (byte)((int)data[i] ^ num6);
				i++;
			}
			return array;
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x000AB33C File Offset: 0x000A953C
		public static byte[] EncryptString(byte[] pwd, string data, int key_length = -1, int itterations = 256)
		{
			if (pwd == null || data == null)
			{
				SoaringDebug.Log("Invalid Encrypt Password Or Key", LogType.Error);
				return null;
			}
			int num = itterations - 1;
			SoaringEncryption.RC4.AllocateBuffers(itterations);
			int num2 = pwd.Length;
			if (num2 != -1)
			{
				if (num2 < key_length)
				{
					SoaringDebug.Log("Invalid Key Length: " + key_length.ToString() + " : " + num2.ToString(), LogType.Error);
					return null;
				}
				num2 = key_length;
			}
			byte[] array = new byte[data.Length];
			int i;
			for (i = 0; i < itterations; i++)
			{
				SoaringEncryption.RC4.mKey[i] = (int)pwd[i % num2];
				SoaringEncryption.RC4.mBox[i] = i;
			}
			int num3;
			for (i = (num3 = 0); i < itterations; i++)
			{
				num3 = (num3 + SoaringEncryption.RC4.mBox[i] + SoaringEncryption.RC4.mKey[i] & num);
				int num4 = SoaringEncryption.RC4.mBox[i];
				SoaringEncryption.RC4.mBox[i] = SoaringEncryption.RC4.mBox[num3];
				SoaringEncryption.RC4.mBox[num3] = num4;
			}
			int num5;
			num3 = (num5 = (i = 0));
			while (i < data.Length)
			{
				num5++;
				num5 %= itterations;
				num3 += SoaringEncryption.RC4.mBox[num5];
				num3 %= itterations;
				int num4 = SoaringEncryption.RC4.mBox[num5];
				SoaringEncryption.RC4.mBox[num5] = SoaringEncryption.RC4.mBox[num3];
				SoaringEncryption.RC4.mBox[num3] = num4;
				int num6 = SoaringEncryption.RC4.mBox[SoaringEncryption.RC4.mBox[num5] + SoaringEncryption.RC4.mBox[num3] & num];
				array[i] = (byte)((int)data[i] ^ num6);
				i++;
			}
			return array;
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x000AB4AC File Offset: 0x000A96AC
		public static byte[] Decrypt(byte[] pwd, byte[] data, int bit_length = 32)
		{
			return SoaringEncryption.RC4.Encrypt(pwd, data, bit_length, 256);
		}

		// Token: 0x040010F0 RID: 4336
		public const int RC4_Variable = -1;

		// Token: 0x040010F1 RID: 4337
		public const int RC4_40bit = 5;

		// Token: 0x040010F2 RID: 4338
		public const int RC4_128bit = 16;

		// Token: 0x040010F3 RID: 4339
		public const int RC4_256bit = 32;

		// Token: 0x040010F4 RID: 4340
		public const int RC4_512bit = 64;

		// Token: 0x040010F5 RID: 4341
		public static int[] mKey;

		// Token: 0x040010F6 RID: 4342
		public static int[] mBox;
	}
}
