using System;

namespace MTools
{
	// Token: 0x020003B5 RID: 949
	public class MBufferWriter : MWriter
	{
		// Token: 0x06001BC4 RID: 7108 RVA: 0x000B2FD8 File Offset: 0x000B11D8
		public MBufferWriter(int bufferSize)
		{
			this.mDefaultBufferSize = bufferSize;
			this.mBuffersList = new MArray(4);
			this.mBuffersList.addObject(new MBuffer(this.mDefaultBufferSize));
			this.mCurrentBuffer = (MBuffer)this.mBuffersList.objectAtIndex(0);
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x000B302C File Offset: 0x000B122C
		public int BuffersCount
		{
			get
			{
				return this.mBuffersList.count();
			}
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x000B303C File Offset: 0x000B123C
		public int GetBuffer(ref byte[] buffer, int idx)
		{
			buffer = null;
			if (idx < 0 || idx > this.BuffersCount)
			{
				return 0;
			}
			MBuffer mbuffer = (MBuffer)this.mBuffersList.objectAtIndex(idx);
			buffer = mbuffer.buffer;
			return mbuffer.useLength;
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x000B3084 File Offset: 0x000B1284
		private MBuffer AllocateSpace(int space)
		{
			if (this.mCurrentBuffer.writePos + space >= this.mCurrentBuffer.bufferSize)
			{
				this.mCurrentBuffer = new MBuffer(this.mDefaultBufferSize);
				this.mBuffersList.addObject(this.mCurrentBuffer);
			}
			return this.mCurrentBuffer;
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x000B30D8 File Offset: 0x000B12D8
		public override bool Open(string filename, bool deleteExisting, bool createDirectory)
		{
			return true;
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x000B30DC File Offset: 0x000B12DC
		public override bool Open(string filename, bool deleteExisting, bool createDirectory, string backupEXT)
		{
			return true;
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x000B30E0 File Offset: 0x000B12E0
		public override bool IsOpen()
		{
			return true;
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x000B30E4 File Offset: 0x000B12E4
		public override void Write(byte val)
		{
			MBuffer mbuffer = this.AllocateSpace(1);
			mbuffer.buffer[mbuffer.writePos] = val;
			mbuffer.IncrimentWriteBuffer(1);
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x000B3110 File Offset: 0x000B1310
		public override void Write(char val)
		{
			MBuffer mbuffer = this.AllocateSpace(2);
			mbuffer.buffer[mbuffer.writePos] = (byte)val;
			mbuffer.IncrimentWriteBuffer(1);
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x000B313C File Offset: 0x000B133C
		public override void Write(ushort val)
		{
			MBuffer mbuffer = this.AllocateSpace(2);
			mbuffer.buffer[mbuffer.writePos] = (byte)(255 & val);
			mbuffer.buffer[mbuffer.writePos + 1] = (byte)(255 & val >> 8);
			mbuffer.IncrimentWriteBuffer(2);
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x000B3188 File Offset: 0x000B1388
		public override void Write(short val)
		{
			MBuffer mbuffer = this.AllocateSpace(2);
			mbuffer.buffer[mbuffer.writePos] = (byte)(255 & val);
			mbuffer.buffer[mbuffer.writePos + 1] = (byte)(255 & val >> 8);
			mbuffer.IncrimentWriteBuffer(2);
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x000B31D4 File Offset: 0x000B13D4
		public override void Write(uint val)
		{
			MBuffer mbuffer = this.AllocateSpace(4);
			mbuffer.buffer[mbuffer.writePos] = (byte)(255U & val);
			mbuffer.buffer[mbuffer.writePos + 1] = (byte)(255U & val >> 8);
			mbuffer.buffer[mbuffer.writePos + 2] = (byte)(255U & val >> 16);
			mbuffer.buffer[mbuffer.writePos + 3] = (byte)(255U & val >> 24);
			mbuffer.IncrimentWriteBuffer(4);
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x000B3254 File Offset: 0x000B1454
		public override void Write(int val)
		{
			MBuffer mbuffer = this.AllocateSpace(4);
			mbuffer.buffer[mbuffer.writePos] = (byte)(255 & val);
			mbuffer.buffer[mbuffer.writePos + 1] = (byte)(255 & val >> 8);
			mbuffer.buffer[mbuffer.writePos + 2] = (byte)(255 & val >> 16);
			mbuffer.buffer[mbuffer.writePos + 3] = (byte)(255 & val >> 24);
			mbuffer.IncrimentWriteBuffer(4);
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x000B32D4 File Offset: 0x000B14D4
		public override void Write(ulong val)
		{
			MBuffer mbuffer = this.AllocateSpace(8);
			mbuffer.buffer[mbuffer.writePos] = (byte)(255UL & val);
			mbuffer.buffer[mbuffer.writePos + 1] = (byte)(255UL & val >> 8);
			mbuffer.buffer[mbuffer.writePos + 2] = (byte)(255UL & val >> 16);
			mbuffer.buffer[mbuffer.writePos + 3] = (byte)(255UL & val >> 24);
			mbuffer.buffer[mbuffer.writePos + 4] = (byte)(255UL & val >> 32);
			mbuffer.buffer[mbuffer.writePos + 5] = (byte)(255UL & val >> 40);
			mbuffer.buffer[mbuffer.writePos + 6] = (byte)(255UL & val >> 48);
			mbuffer.buffer[mbuffer.writePos + 7] = (byte)(255UL & val >> 56);
			mbuffer.IncrimentWriteBuffer(8);
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x000B33C4 File Offset: 0x000B15C4
		public override void Write(long val)
		{
			MBuffer mbuffer = this.AllocateSpace(8);
			mbuffer.buffer[mbuffer.writePos] = (byte)(255L & val);
			mbuffer.buffer[mbuffer.writePos + 1] = (byte)(255L & val >> 8);
			mbuffer.buffer[mbuffer.writePos + 2] = (byte)(255L & val >> 16);
			mbuffer.buffer[mbuffer.writePos + 3] = (byte)(255L & val >> 24);
			mbuffer.buffer[mbuffer.writePos + 4] = (byte)(255L & val >> 32);
			mbuffer.buffer[mbuffer.writePos + 5] = (byte)(255L & val >> 40);
			mbuffer.buffer[mbuffer.writePos + 6] = (byte)(255L & val >> 48);
			mbuffer.buffer[mbuffer.writePos + 7] = (byte)(255L & val >> 56);
			mbuffer.IncrimentWriteBuffer(8);
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x000B34B4 File Offset: 0x000B16B4
		public override void Write(sbyte val)
		{
			MBuffer mbuffer = this.AllocateSpace(1);
			mbuffer.buffer[mbuffer.writePos] = (byte)val;
			mbuffer.IncrimentWriteBuffer(1);
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x000B34E0 File Offset: 0x000B16E0
		public override void Write(float val)
		{
			MBuffer mbuffer = this.AllocateSpace(4);
			byte[] bytes = BitConverter.GetBytes(val);
			mbuffer.buffer[mbuffer.writePos] = bytes[0];
			mbuffer.buffer[mbuffer.writePos + 1] = bytes[1];
			mbuffer.buffer[mbuffer.writePos + 2] = bytes[2];
			mbuffer.buffer[mbuffer.writePos + 3] = bytes[3];
			mbuffer.IncrimentWriteBuffer(4);
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x000B354C File Offset: 0x000B174C
		public override void Write(double val)
		{
			MBuffer mbuffer = this.AllocateSpace(8);
			byte[] bytes = BitConverter.GetBytes(val);
			mbuffer.buffer[mbuffer.writePos] = bytes[0];
			mbuffer.buffer[mbuffer.writePos + 1] = bytes[1];
			mbuffer.buffer[mbuffer.writePos + 2] = bytes[2];
			mbuffer.buffer[mbuffer.writePos + 3] = bytes[3];
			mbuffer.buffer[mbuffer.writePos + 4] = bytes[4];
			mbuffer.buffer[mbuffer.writePos + 5] = bytes[5];
			mbuffer.buffer[mbuffer.writePos + 6] = bytes[6];
			mbuffer.buffer[mbuffer.writePos + 7] = bytes[7];
			mbuffer.IncrimentWriteBuffer(8);
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x000B3600 File Offset: 0x000B1800
		public override void Write(string val)
		{
			this.WriteCharArrayAsString(val);
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x000B360C File Offset: 0x000B180C
		public override void Write(char[] arry)
		{
			int length = 0;
			if (arry != null)
			{
				length = arry.Length;
			}
			this.Write(arry, length);
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x000B3630 File Offset: 0x000B1830
		public override void Write(char[] arry, int length)
		{
			MBuffer mbuffer = this.AllocateSpace(length + 4);
			mbuffer.buffer[mbuffer.writePos] = (byte)(255 & length);
			mbuffer.buffer[mbuffer.writePos + 1] = (byte)(255 & length >> 8);
			mbuffer.buffer[mbuffer.writePos + 2] = (byte)(255 & length >> 16);
			mbuffer.buffer[mbuffer.writePos + 3] = (byte)(255 & length >> 24);
			mbuffer.IncrimentWriteBuffer(4);
			for (int i = 0; i < length; i++)
			{
				mbuffer.buffer[mbuffer.writePos + i] = (byte)arry[i];
			}
			mbuffer.IncrimentWriteBuffer(length);
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x000B36DC File Offset: 0x000B18DC
		public override void WriteCharArrayAsString(string str)
		{
			int num = 0;
			if (str != null)
			{
				num = str.Length;
			}
			MBuffer mbuffer = this.AllocateSpace(num + 4);
			mbuffer.buffer[mbuffer.writePos] = (byte)(255 & num);
			mbuffer.buffer[mbuffer.writePos + 1] = (byte)(255 & num >> 8);
			mbuffer.buffer[mbuffer.writePos + 2] = (byte)(255 & num >> 16);
			mbuffer.buffer[mbuffer.writePos + 3] = (byte)(255 & num >> 24);
			mbuffer.IncrimentWriteBuffer(4);
			for (int i = 0; i < num; i++)
			{
				mbuffer.buffer[mbuffer.writePos + i] = (byte)str[i];
			}
			mbuffer.IncrimentWriteBuffer(num);
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x000B379C File Offset: 0x000B199C
		public override void WriteRawString(string str)
		{
			int num = 0;
			if (str != null)
			{
				num = str.Length;
			}
			MBuffer mbuffer = this.AllocateSpace(num);
			for (int i = 0; i < num; i++)
			{
				mbuffer.buffer[mbuffer.writePos + i] = (byte)str[i];
			}
			mbuffer.IncrimentWriteBuffer(num);
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x000B37F0 File Offset: 0x000B19F0
		public override void Flush()
		{
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x000B37F4 File Offset: 0x000B19F4
		public override void Seek(int offset)
		{
			int num = 0;
			for (int i = 0; i < this.mBuffersList.count(); i++)
			{
				MBuffer mbuffer = (MBuffer)this.mBuffersList.objectAtIndex(i);
				if (offset < mbuffer.useLength + num && offset > num)
				{
					this.mCurrentBuffer = mbuffer;
					int writePos = mbuffer.useLength + num - offset;
					mbuffer.writePos = writePos;
				}
				else if (offset < mbuffer.useLength)
				{
					mbuffer.writePos = mbuffer.useLength;
				}
				else
				{
					mbuffer.writePos = 0;
				}
			}
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x000B388C File Offset: 0x000B1A8C
		public override int Pos()
		{
			int num = 0;
			for (int i = 0; i < this.mBuffersList.count(); i++)
			{
				MBuffer mbuffer = (MBuffer)this.mBuffersList.objectAtIndex(i);
				num += mbuffer.writePos;
			}
			return num;
		}

		// Token: 0x04001222 RID: 4642
		private MBuffer mCurrentBuffer;

		// Token: 0x04001223 RID: 4643
		private MArray mBuffersList;

		// Token: 0x04001224 RID: 4644
		private int mDefaultBufferSize;
	}
}
