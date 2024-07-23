using System;

namespace MTools
{
	// Token: 0x020003BC RID: 956
	public class MReader
	{
		// Token: 0x06001C1C RID: 7196 RVA: 0x000B4774 File Offset: 0x000B2974
		public virtual bool Open(string path)
		{
			return false;
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000B4778 File Offset: 0x000B2978
		public virtual bool Open(byte[] byteArray)
		{
			return false;
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x000B477C File Offset: 0x000B297C
		public virtual void Close()
		{
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000B4780 File Offset: 0x000B2980
		public virtual bool IsOpen()
		{
			return false;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000B4784 File Offset: 0x000B2984
		public virtual byte ReadByte()
		{
			return 0;
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000B4788 File Offset: 0x000B2988
		public virtual sbyte ReadSByte()
		{
			return 0;
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000B478C File Offset: 0x000B298C
		public virtual ushort ReadUShort()
		{
			return 0;
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x000B4790 File Offset: 0x000B2990
		public virtual short ReadShort()
		{
			return 0;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x000B4794 File Offset: 0x000B2994
		public virtual uint ReadUInt()
		{
			return 0U;
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x000B4798 File Offset: 0x000B2998
		public virtual int ReadInt()
		{
			return 0;
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x000B479C File Offset: 0x000B299C
		public virtual ulong ReadULong()
		{
			return 0UL;
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x000B47A0 File Offset: 0x000B29A0
		public virtual long ReadLong()
		{
			return 0L;
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x000B47A4 File Offset: 0x000B29A4
		public virtual float ReadFloat()
		{
			return 0f;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x000B47AC File Offset: 0x000B29AC
		public virtual float ReadSingle()
		{
			return 0f;
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x000B47B4 File Offset: 0x000B29B4
		public virtual double ReadDouble()
		{
			return 0.0;
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x000B47C0 File Offset: 0x000B29C0
		public virtual string ReadString()
		{
			return null;
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x000B47C4 File Offset: 0x000B29C4
		public virtual char[] ReadCharArray(int count)
		{
			return null;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000B47C8 File Offset: 0x000B29C8
		public virtual int ReadBytes(int length, ref byte[] buffer)
		{
			return 0;
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x000B47CC File Offset: 0x000B29CC
		public virtual byte[] ReadAllBytes()
		{
			return null;
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x000B47D0 File Offset: 0x000B29D0
		public virtual string ReadCharArrayAsString()
		{
			return null;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x000B47D4 File Offset: 0x000B29D4
		public virtual int FileLength()
		{
			return 0;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x000B47D8 File Offset: 0x000B29D8
		public virtual long FileLengthLong()
		{
			return 0L;
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x000B47DC File Offset: 0x000B29DC
		public virtual void Seek(int offset)
		{
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x000B47E0 File Offset: 0x000B29E0
		public virtual int Pos()
		{
			return 0;
		}
	}
}
