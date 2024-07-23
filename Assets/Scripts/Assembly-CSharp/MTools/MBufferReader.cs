using System;

namespace MTools
{
	// Token: 0x020003B4 RID: 948
	public class MBufferReader : MReader
	{
		// Token: 0x06001BA9 RID: 7081 RVA: 0x000B2894 File Offset: 0x000B0A94
		public MBufferReader()
		{
			this.mBufferList = new MArray();
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x000B28A8 File Offset: 0x000B0AA8
		public override bool Open(string path)
		{
			this.mCurrentBuffer = 0;
			return this.IsOpen();
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x000B28B8 File Offset: 0x000B0AB8
		public override bool Open(byte[] byteArray)
		{
			this.mCurrentBuffer = 0;
			return this.IsOpen();
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x000B28C8 File Offset: 0x000B0AC8
		public void SetBuffers(MBuffer[] buffersArray)
		{
			this.mBufferList.clear();
			if (buffersArray == null)
			{
				return;
			}
			for (int i = 0; i < buffersArray.Length; i++)
			{
				buffersArray[i].useLength = buffersArray[i].bufferSize;
				this.mBufferList.addObject(buffersArray[i]);
			}
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x000B291C File Offset: 0x000B0B1C
		public MBuffer CheckActiveBuffer(int writeSpace)
		{
			if (writeSpace < 0 || this.mCurrentBuffer >= this.mBufferList.count())
			{
				return null;
			}
			MBuffer mbuffer = (MBuffer)this.mBufferList.objectAtIndex(this.mCurrentBuffer);
			if (mbuffer.readPos + writeSpace > mbuffer.useLength)
			{
				this.mCurrentBuffer++;
				mbuffer = (MBuffer)this.mBufferList.objectAtIndex(this.mCurrentBuffer);
			}
			return mbuffer;
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x000B2998 File Offset: 0x000B0B98
		public override void Close()
		{
			this.mBufferList.clear();
			this.mCurrentBuffer = 0;
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x000B29AC File Offset: 0x000B0BAC
		public override bool IsOpen()
		{
			return this.mBufferList.count() != 0;
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x000B29C0 File Offset: 0x000B0BC0
		public override byte ReadByte()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(1);
			byte result = mbuffer.buffer[mbuffer.readPos];
			mbuffer.IncrimentReadBuffer(1);
			return result;
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x000B29EC File Offset: 0x000B0BEC
		public override sbyte ReadSByte()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(1);
			sbyte result = (sbyte)mbuffer.buffer[mbuffer.readPos];
			mbuffer.IncrimentReadBuffer(1);
			return result;
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x000B2A18 File Offset: 0x000B0C18
		public override ushort ReadUShort()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(2);
			ushort result = (ushort)((int)mbuffer.buffer[mbuffer.readPos] | (int)mbuffer.buffer[mbuffer.readPos + 1] << 8);
			mbuffer.IncrimentReadBuffer(2);
			return result;
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x000B2A58 File Offset: 0x000B0C58
		public override short ReadShort()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(2);
			short result = (short)((int)mbuffer.buffer[mbuffer.readPos] | (int)mbuffer.buffer[mbuffer.readPos + 1] << 8);
			mbuffer.IncrimentReadBuffer(2);
			return result;
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x000B2A98 File Offset: 0x000B0C98
		public override uint ReadUInt()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(4);
			uint result = (uint)((int)mbuffer.buffer[mbuffer.readPos] | (int)mbuffer.buffer[mbuffer.readPos + 1] << 8 | (int)mbuffer.buffer[mbuffer.readPos + 2] << 16 | (int)mbuffer.buffer[mbuffer.readPos + 3] << 24);
			mbuffer.IncrimentReadBuffer(4);
			return result;
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x000B2AFC File Offset: 0x000B0CFC
		public override int ReadInt()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(4);
			int result = (int)mbuffer.buffer[mbuffer.readPos] | (int)mbuffer.buffer[mbuffer.readPos + 1] << 8 | (int)mbuffer.buffer[mbuffer.readPos + 2] << 16 | (int)mbuffer.buffer[mbuffer.readPos + 3] << 24;
			mbuffer.IncrimentReadBuffer(4);
			return result;
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x000B2B60 File Offset: 0x000B0D60
		public override ulong ReadULong()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(8);
			ulong result = (ulong)((long)((int)mbuffer.buffer[mbuffer.readPos] | (int)mbuffer.buffer[mbuffer.readPos + 1] << 8 | (int)mbuffer.buffer[mbuffer.readPos + 2] << 16 | (int)mbuffer.buffer[mbuffer.readPos + 3] << 24 | (int)mbuffer.buffer[mbuffer.readPos + 4] | (int)mbuffer.buffer[mbuffer.readPos + 5] << 8 | (int)mbuffer.buffer[mbuffer.readPos + 6] << 16 | (int)mbuffer.buffer[mbuffer.readPos + 7] << 24));
			mbuffer.IncrimentReadBuffer(8);
			return result;
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x000B2C0C File Offset: 0x000B0E0C
		public override long ReadLong()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(8);
			long result = (long)((int)mbuffer.buffer[mbuffer.readPos] | (int)mbuffer.buffer[mbuffer.readPos + 1] << 8 | (int)mbuffer.buffer[mbuffer.readPos + 2] << 16 | (int)mbuffer.buffer[mbuffer.readPos + 3] << 24 | (int)mbuffer.buffer[mbuffer.readPos + 4] | (int)mbuffer.buffer[mbuffer.readPos + 5] << 8 | (int)mbuffer.buffer[mbuffer.readPos + 6] << 16 | (int)mbuffer.buffer[mbuffer.readPos + 7] << 24);
			mbuffer.IncrimentReadBuffer(8);
			return result;
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x000B2CB8 File Offset: 0x000B0EB8
		public override float ReadFloat()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(4);
			float result = BitConverter.ToSingle(mbuffer.buffer, mbuffer.readPos);
			mbuffer.IncrimentReadBuffer(4);
			return result;
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x000B2CE8 File Offset: 0x000B0EE8
		public override float ReadSingle()
		{
			return this.ReadFloat();
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x000B2CF0 File Offset: 0x000B0EF0
		public override double ReadDouble()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(8);
			double result = BitConverter.ToDouble(mbuffer.buffer, mbuffer.readPos);
			mbuffer.IncrimentReadBuffer(8);
			return result;
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x000B2D20 File Offset: 0x000B0F20
		public override string ReadString()
		{
			return this.ReadCharArrayAsString();
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x000B2D28 File Offset: 0x000B0F28
		public override char[] ReadCharArray(int count)
		{
			return null;
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000B2D2C File Offset: 0x000B0F2C
		public override int ReadBytes(int length, ref byte[] bytes)
		{
			if (length <= 0)
			{
				return 0;
			}
			MBuffer mbuffer = this.CheckActiveBuffer(length);
			bytes = new byte[length];
			for (int i = 0; i < length; i++)
			{
				bytes[i] = mbuffer.buffer[mbuffer.readPos + i];
			}
			mbuffer.IncrimentReadBuffer(length);
			return length;
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000B2D80 File Offset: 0x000B0F80
		public override byte[] ReadAllBytes()
		{
			return null;
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000B2D84 File Offset: 0x000B0F84
		public override string ReadCharArrayAsString()
		{
			MBuffer mbuffer = this.CheckActiveBuffer(4);
			int num = (int)mbuffer.buffer[mbuffer.readPos] | (int)mbuffer.buffer[mbuffer.readPos + 1] << 8 | (int)mbuffer.buffer[mbuffer.readPos + 2] << 16 | (int)mbuffer.buffer[mbuffer.readPos + 3] << 24;
			mbuffer.IncrimentReadBuffer(4);
			char[] array = new char[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (char)mbuffer.buffer[mbuffer.readPos + i];
			}
			mbuffer.IncrimentReadBuffer(num);
			return new string(array);
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x000B2E20 File Offset: 0x000B1020
		public override int FileLength()
		{
			int num = 0;
			for (int i = 0; i < this.mBufferList.count(); i++)
			{
				MBuffer mbuffer = (MBuffer)this.mBufferList.objectAtIndex(i);
				num += mbuffer.useLength;
			}
			return num;
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x000B2E68 File Offset: 0x000B1068
		public override long FileLengthLong()
		{
			long num = 0L;
			for (int i = 0; i < this.mBufferList.count(); i++)
			{
				MBuffer mbuffer = (MBuffer)this.mBufferList.objectAtIndex(i);
				num += (long)mbuffer.useLength;
			}
			return num;
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000B2EB4 File Offset: 0x000B10B4
		public override void Seek(int offset)
		{
			int num = 0;
			for (int i = 0; i < this.mBufferList.count(); i++)
			{
				MBuffer mbuffer = (MBuffer)this.mBufferList.objectAtIndex(i);
				if (offset < mbuffer.useLength + num && offset > num)
				{
					this.mCurrentBuffer = i;
					int readPos = mbuffer.useLength + num - offset;
					mbuffer.readPos = readPos;
				}
				else
				{
					mbuffer.readPos = mbuffer.useLength;
				}
			}
			for (int j = this.mCurrentBuffer + 1; j < this.mBufferList.count(); j++)
			{
				MBuffer mbuffer = (MBuffer)this.mBufferList.objectAtIndex(j);
				mbuffer.readPos = 0;
			}
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000B2F74 File Offset: 0x000B1174
		public override int Pos()
		{
			int num = 0;
			MBuffer mbuffer;
			for (int i = 0; i < this.mCurrentBuffer; i++)
			{
				mbuffer = (MBuffer)this.mBufferList.objectAtIndex(i);
				num += mbuffer.useLength;
			}
			mbuffer = (MBuffer)this.mBufferList.objectAtIndex(this.mCurrentBuffer);
			return num + mbuffer.readPos;
		}

		// Token: 0x04001220 RID: 4640
		private MArray mBufferList;

		// Token: 0x04001221 RID: 4641
		private int mCurrentBuffer;
	}
}
