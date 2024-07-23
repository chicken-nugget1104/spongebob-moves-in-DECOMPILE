using System;

namespace MTools
{
	// Token: 0x020003BF RID: 959
	public class MWriter
	{
		// Token: 0x06001C57 RID: 7255 RVA: 0x000B4EEC File Offset: 0x000B30EC
		public virtual bool Open(string filename)
		{
			return false;
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x000B4EF0 File Offset: 0x000B30F0
		public virtual bool Open(string filename, bool deleteExisting)
		{
			return false;
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x000B4EF4 File Offset: 0x000B30F4
		public virtual bool Open(string filename, bool deleteExisting, bool createDirectory)
		{
			return false;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000B4EF8 File Offset: 0x000B30F8
		public virtual bool Open(string filename, bool deleteExisting, bool createDirectory, string backupExt)
		{
			return false;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000B4EFC File Offset: 0x000B30FC
		public virtual bool IsOpen()
		{
			return false;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000B4F00 File Offset: 0x000B3100
		public virtual void Close()
		{
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000B4F04 File Offset: 0x000B3104
		public virtual void Write(byte val)
		{
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000B4F08 File Offset: 0x000B3108
		public virtual void Write(char val)
		{
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000B4F0C File Offset: 0x000B310C
		public virtual void Write(ushort val)
		{
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000B4F10 File Offset: 0x000B3110
		public virtual void Write(short val)
		{
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000B4F14 File Offset: 0x000B3114
		public virtual void Write(uint val)
		{
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x000B4F18 File Offset: 0x000B3118
		public virtual void Write(int val)
		{
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000B4F1C File Offset: 0x000B311C
		public virtual void Write(ulong val)
		{
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000B4F20 File Offset: 0x000B3120
		public virtual void Write(long val)
		{
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000B4F24 File Offset: 0x000B3124
		public virtual void Write(sbyte val)
		{
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000B4F28 File Offset: 0x000B3128
		public virtual void Write(float val)
		{
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000B4F2C File Offset: 0x000B312C
		public virtual void Write(double val)
		{
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000B4F30 File Offset: 0x000B3130
		public virtual void Write(string val)
		{
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000B4F34 File Offset: 0x000B3134
		public virtual void Write(char[] arry)
		{
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000B4F38 File Offset: 0x000B3138
		public virtual void Write(char[] arry, int length)
		{
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000B4F3C File Offset: 0x000B313C
		public virtual void Flush()
		{
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000B4F40 File Offset: 0x000B3140
		public virtual void WriteCharArrayAsString(string str)
		{
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000B4F44 File Offset: 0x000B3144
		public virtual void WriteRawString(string str)
		{
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000B4F48 File Offset: 0x000B3148
		public virtual void Seek(int offset)
		{
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000B4F4C File Offset: 0x000B314C
		public virtual int Pos()
		{
			return 0;
		}
	}
}
