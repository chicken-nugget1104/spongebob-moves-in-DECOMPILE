using System;

namespace MTools
{
	// Token: 0x020003B6 RID: 950
	public class MBuffer
	{
		// Token: 0x06001BDE RID: 7134 RVA: 0x000B38D8 File Offset: 0x000B1AD8
		public MBuffer(int size)
		{
			this.bufferSize = size;
			if (this.bufferSize > 32768 || this.bufferSize < 4)
			{
				this.bufferSize = 32768;
			}
			while (this.buffer == null && this.bufferSize > 4)
			{
				try
				{
					byte[] array = new byte[this.bufferSize];
					this.buffer = array;
				}
				catch (Exception)
				{
					this.bufferSize <<= 1;
				}
			}
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x000B3980 File Offset: 0x000B1B80
		public MBuffer(byte[] readbuffer)
		{
			this.buffer = readbuffer;
			this.bufferSize = this.buffer.Length;
			this.useLength = 0;
			this.writePos = 0;
			this.readPos = 0;
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000B39C0 File Offset: 0x000B1BC0
		public void IncrimentWriteBuffer(int size)
		{
			this.writePos += size;
			this.useLength += size;
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x000B39E0 File Offset: 0x000B1BE0
		public void IncrimentReadBuffer(int size)
		{
			this.readPos += size;
		}

		// Token: 0x04001225 RID: 4645
		public byte[] buffer;

		// Token: 0x04001226 RID: 4646
		public int writePos;

		// Token: 0x04001227 RID: 4647
		public int readPos;

		// Token: 0x04001228 RID: 4648
		public int useLength;

		// Token: 0x04001229 RID: 4649
		public int bufferSize;
	}
}
