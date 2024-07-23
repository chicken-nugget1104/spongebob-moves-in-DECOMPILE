using System;
using System.IO;

namespace MTools
{
	// Token: 0x020003B7 RID: 951
	public class MCommon
	{
		// Token: 0x06001BE2 RID: 7138 RVA: 0x000B39F0 File Offset: 0x000B1BF0
		private MCommon()
		{
		}

		// Token: 0x06001BE3 RID: 7139 RVA: 0x000B39F8 File Offset: 0x000B1BF8
		public static string CreateFileHash(string filePath)
		{
			string text = string.Empty;
			if (!File.Exists(filePath))
			{
				return text;
			}
			MBinaryReader mbinaryReader = new MBinaryReader(filePath);
			if (mbinaryReader == null)
			{
				return text;
			}
			if (!mbinaryReader.IsOpen())
			{
				return text;
			}
			long num = mbinaryReader.FileLengthLong();
			long num2 = 0L;
			while (num > 0L)
			{
				ulong num3 = 0UL;
				long num4 = 16777210L;
				if (num4 > num)
				{
					num4 = num;
				}
				int num5 = 0;
				while ((long)num5 < num4)
				{
					num3 += (ulong)mbinaryReader.ReadByte();
					num2 += 1L;
					num5++;
				}
				num -= num4;
				byte[] bytes = BitConverter.GetBytes(num3);
				text += Convert.ToBase64String(bytes);
			}
			text = mbinaryReader.FileLengthLong().ToString() + ":" + text;
			mbinaryReader.Close();
			return text;
		}

		// Token: 0x06001BE4 RID: 7140 RVA: 0x000B3AD0 File Offset: 0x000B1CD0
		public static string CreateStringHash(string message)
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(message))
			{
				return text;
			}
			int length = message.Length;
			uint num = 0U;
			for (int i = 0; i < length; i++)
			{
				num += (uint)message[i];
			}
			text = length + ":";
			byte[] bytes = BitConverter.GetBytes(num);
			return text + Convert.ToBase64String(bytes);
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x000B3B40 File Offset: 0x000B1D40
		public static bool ValidateFileHash(string filePath, string hash)
		{
			if (string.IsNullOrEmpty(hash))
			{
				return false;
			}
			string text = MCommon.CreateFileHash(filePath);
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			int num = text.LastIndexOf(':');
			if (num >= 0)
			{
				text = text.Substring(num, text.Length - num);
			}
			num = hash.LastIndexOf(':');
			if (num >= 0)
			{
				hash = hash.Substring(num, hash.Length - num);
			}
			return text == hash;
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x000B3BB8 File Offset: 0x000B1DB8
		public static bool ValidateStringHash(string message, string existing_hash)
		{
			if (string.IsNullOrEmpty(existing_hash))
			{
				return false;
			}
			string a = MCommon.CreateStringHash(message);
			return !string.IsNullOrEmpty(message) && a == existing_hash;
		}

		// Token: 0x020004B5 RID: 1205
		// (Invoke) Token: 0x06002537 RID: 9527
		public delegate void StandardDelegate();

		// Token: 0x020004B6 RID: 1206
		// (Invoke) Token: 0x0600253B RID: 9531
		public delegate void StandardDelegate_Object(object o);

		// Token: 0x020004B7 RID: 1207
		// (Invoke) Token: 0x0600253F RID: 9535
		public delegate void StandardDelegate_String(string s);

		// Token: 0x020004B8 RID: 1208
		// (Invoke) Token: 0x06002543 RID: 9539
		public delegate void StandardDelegate_Key(string s, object o);

		// Token: 0x020004B9 RID: 1209
		// (Invoke) Token: 0x06002547 RID: 9543
		public delegate bool StandardDelegate_Check(object o);
	}
}
