using System;
using System.IO;
using UnityEngine;

namespace MTools
{
	// Token: 0x020003B2 RID: 946
	public class MBinaryReader : MReader
	{
		// Token: 0x06001B6E RID: 7022 RVA: 0x000B203C File Offset: 0x000B023C
		public MBinaryReader(string filePath)
		{
			this.Open(filePath);
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x000B204C File Offset: 0x000B024C
		public MBinaryReader(byte[] data)
		{
			this.Open(data);
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x000B205C File Offset: 0x000B025C
		public MBinaryReader()
		{
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001B71 RID: 7025 RVA: 0x000B2064 File Offset: 0x000B0264
		public Stream Stream
		{
			get
			{
				return this.stream;
			}
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x000B206C File Offset: 0x000B026C
		~MBinaryReader()
		{
			this.Close();
			this.stream = null;
			this.reader = null;
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x000B20B8 File Offset: 0x000B02B8
		public override bool Open(string path)
		{
			if (this.isFileOpen)
			{
				return false;
			}
			if (!File.Exists(path))
			{
				return false;
			}
			try
			{
				this.FilePath = path;
				this.stream = File.OpenRead(this.FilePath);
				this.reader = new System.IO.BinaryReader(this.stream);
				this.isFileOpen = true;
			}
			catch
			{
				this.isFileOpen = false;
			}
			return this.isFileOpen;
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x000B2144 File Offset: 0x000B0344
		public override bool Open(byte[] byteArray)
		{
			if (this.isFileOpen)
			{
				return false;
			}
			if (byteArray == null)
			{
				return false;
			}
			if (byteArray.Length == 0)
			{
				return false;
			}
			try
			{
				this.FilePath = null;
				this.stream = new MemoryStream(byteArray);
				this.reader = new System.IO.BinaryReader(this.stream);
				this.isFileOpen = true;
			}
			catch
			{
				this.stream = null;
				this.reader = null;
				this.isFileOpen = false;
			}
			return this.isFileOpen;
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x000B21E0 File Offset: 0x000B03E0
		public override void Close()
		{
			if (this.isFileOpen)
			{
				try
				{
					if (this.reader != null)
					{
						this.reader.Close();
					}
					if (this.stream != null)
					{
						this.stream.Close();
					}
					this.stream = null;
					this.reader = null;
					this.isFileOpen = false;
				}
				catch (Exception ex)
				{
					Debug.Log(ex.Message);
				}
			}
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x000B226C File Offset: 0x000B046C
		public override bool IsOpen()
		{
			return this.isFileOpen;
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x000B2274 File Offset: 0x000B0474
		public override byte ReadByte()
		{
			return this.reader.ReadByte();
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x000B2284 File Offset: 0x000B0484
		public override sbyte ReadSByte()
		{
			return this.reader.ReadSByte();
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x000B2294 File Offset: 0x000B0494
		public override ushort ReadUShort()
		{
			return this.reader.ReadUInt16();
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x000B22A4 File Offset: 0x000B04A4
		public override short ReadShort()
		{
			return this.reader.ReadInt16();
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x000B22B4 File Offset: 0x000B04B4
		public override uint ReadUInt()
		{
			return this.reader.ReadUInt32();
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x000B22C4 File Offset: 0x000B04C4
		public override int ReadInt()
		{
			return this.reader.ReadInt32();
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x000B22D4 File Offset: 0x000B04D4
		public override ulong ReadULong()
		{
			return this.reader.ReadUInt64();
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x000B22E4 File Offset: 0x000B04E4
		public override long ReadLong()
		{
			return this.reader.ReadInt64();
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x000B22F4 File Offset: 0x000B04F4
		public override float ReadFloat()
		{
			return this.reader.ReadSingle();
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x000B2304 File Offset: 0x000B0504
		public override float ReadSingle()
		{
			return this.reader.ReadSingle();
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x000B2314 File Offset: 0x000B0514
		public override double ReadDouble()
		{
			return this.reader.ReadDouble();
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x000B2324 File Offset: 0x000B0524
		public override string ReadString()
		{
			return this.ReadCharArrayAsString();
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x000B232C File Offset: 0x000B052C
		public override char[] ReadCharArray(int count)
		{
			return this.reader.ReadChars(count);
		}

		// Token: 0x06001B84 RID: 7044 RVA: 0x000B233C File Offset: 0x000B053C
		public override int ReadBytes(int length, ref byte[] buffer)
		{
			if (buffer == null)
			{
				buffer = new byte[length];
			}
			long num = this.reader.BaseStream.Length - this.reader.BaseStream.Position;
			if (num - (long)length > 0L)
			{
				num = (long)length;
			}
			int num2 = 0;
			while ((long)num2 < num)
			{
				buffer[num2] = this.reader.ReadByte();
				num2++;
			}
			return (int)num;
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x000B23B0 File Offset: 0x000B05B0
		public override byte[] ReadAllBytes()
		{
			if (!this.isFileOpen)
			{
				return null;
			}
			this.reader.BaseStream.Position = 0L;
			return this.reader.ReadBytes((int)this.reader.BaseStream.Length);
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x000B23F8 File Offset: 0x000B05F8
		public override string ReadCharArrayAsString()
		{
			string text = string.Empty;
			ushort num = this.reader.ReadUInt16();
			if (num == 0)
			{
				return text;
			}
			for (int i = 0; i < (int)num; i++)
			{
				text += (char)this.reader.ReadByte();
			}
			return text;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000B244C File Offset: 0x000B064C
		public override int FileLength()
		{
			return (int)this.reader.BaseStream.Length;
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x000B2460 File Offset: 0x000B0660
		public override long FileLengthLong()
		{
			return this.reader.BaseStream.Length;
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000B2474 File Offset: 0x000B0674
		public override void Seek(int offset)
		{
			this.reader.BaseStream.Seek((long)offset, SeekOrigin.Begin);
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x000B248C File Offset: 0x000B068C
		public override int Pos()
		{
			return (int)this.reader.BaseStream.Position;
		}

		// Token: 0x04001218 RID: 4632
		private string FilePath;

		// Token: 0x04001219 RID: 4633
		private bool isFileOpen;

		// Token: 0x0400121A RID: 4634
		private Stream stream;

		// Token: 0x0400121B RID: 4635
		private System.IO.BinaryReader reader;
	}
}
