using System;
using System.IO;
using System.Text;

namespace DeltaDNA
{
	// Token: 0x02000009 RID: 9
	public class MD5 : IDisposable
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003620 File Offset: 0x00001820
		internal MD5()
		{
			this.Initialize();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000366C File Offset: 0x0000186C
		// Note: this type is marked as 'beforefieldinit'.
		static MD5()
		{
			byte[] array = new byte[64];
			array[0] = 128;
			MD5.PADDING = array;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003684 File Offset: 0x00001884
		public static MD5 Create(string hashName)
		{
			if (hashName == "MD5")
			{
				return new MD5();
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000036A4 File Offset: 0x000018A4
		public static string GetMd5String(string source)
		{
			MD5 md = MD5.Create();
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			byte[] bytes = utf8Encoding.GetBytes(source);
			byte[] array = md.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in array)
			{
				stringBuilder.Append(b.ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003714 File Offset: 0x00001914
		public static MD5 Create()
		{
			return new MD5();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000371C File Offset: 0x0000191C
		private static uint F(uint x, uint y, uint z)
		{
			return (x & y) | (~x & z);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003728 File Offset: 0x00001928
		private static uint G(uint x, uint y, uint z)
		{
			return (x & z) | (y & ~z);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003734 File Offset: 0x00001934
		private static uint H(uint x, uint y, uint z)
		{
			return x ^ y ^ z;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000373C File Offset: 0x0000193C
		private static uint I(uint x, uint y, uint z)
		{
			return y ^ (x | ~z);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003744 File Offset: 0x00001944
		private static uint ROTATE_LEFT(uint x, byte n)
		{
			return x << (int)n | x >> (int)(32 - n);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003758 File Offset: 0x00001958
		private static void FF(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			a += MD5.F(b, c, d) + x + ac;
			a = MD5.ROTATE_LEFT(a, s);
			a += b;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000378C File Offset: 0x0000198C
		private static void GG(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			a += MD5.G(b, c, d) + x + ac;
			a = MD5.ROTATE_LEFT(a, s);
			a += b;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000037C0 File Offset: 0x000019C0
		private static void HH(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			a += MD5.H(b, c, d) + x + ac;
			a = MD5.ROTATE_LEFT(a, s);
			a += b;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000037F4 File Offset: 0x000019F4
		private static void II(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
		{
			a += MD5.I(b, c, d) + x + ac;
			a = MD5.ROTATE_LEFT(a, s);
			a += b;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003828 File Offset: 0x00001A28
		public virtual void Initialize()
		{
			this.count[0] = (this.count[1] = 0U);
			this.state[0] = 1732584193U;
			this.state[1] = 4023233417U;
			this.state[2] = 2562383102U;
			this.state[3] = 271733878U;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003880 File Offset: 0x00001A80
		protected virtual void HashCore(byte[] input, int offset, int count)
		{
			int num = (int)(this.count[0] >> 3 & 63U);
			if ((this.count[0] += (uint)((uint)count << 3)) < (uint)((uint)count << 3))
			{
				this.count[1] += 1U;
			}
			this.count[1] += (uint)count >> 29;
			int num2 = 64 - num;
			int num3;
			if (count >= num2)
			{
				Buffer.BlockCopy(input, offset, this.buffer, num, num2);
				this.Transform(this.buffer, 0);
				num3 = num2;
				while (num3 + 63 < count)
				{
					this.Transform(input, offset + num3);
					num3 += 64;
				}
				num = 0;
			}
			else
			{
				num3 = 0;
			}
			Buffer.BlockCopy(input, offset + num3, this.buffer, num, count - num3);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003944 File Offset: 0x00001B44
		protected virtual byte[] HashFinal()
		{
			byte[] array = new byte[16];
			byte[] array2 = new byte[8];
			MD5.Encode(array2, 0, this.count, 0, 8);
			int num = (int)(this.count[0] >> 3 & 63U);
			int num2 = (num >= 56) ? (120 - num) : (56 - num);
			this.HashCore(MD5.PADDING, 0, num2);
			this.HashCore(array2, 0, 8);
			MD5.Encode(array, 0, this.state, 0, 16);
			this.count[0] = (this.count[1] = 0U);
			this.state[0] = 0U;
			this.state[1] = 0U;
			this.state[2] = 0U;
			this.state[3] = 0U;
			this.Initialize();
			return array;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000039FC File Offset: 0x00001BFC
		private void Transform(byte[] block, int offset)
		{
			uint num = this.state[0];
			uint num2 = this.state[1];
			uint num3 = this.state[2];
			uint num4 = this.state[3];
			uint[] array = new uint[16];
			MD5.Decode(array, 0, block, offset, 64);
			MD5.FF(ref num, num2, num3, num4, array[0], 7, 3614090360U);
			MD5.FF(ref num4, num, num2, num3, array[1], 12, 3905402710U);
			MD5.FF(ref num3, num4, num, num2, array[2], 17, 606105819U);
			MD5.FF(ref num2, num3, num4, num, array[3], 22, 3250441966U);
			MD5.FF(ref num, num2, num3, num4, array[4], 7, 4118548399U);
			MD5.FF(ref num4, num, num2, num3, array[5], 12, 1200080426U);
			MD5.FF(ref num3, num4, num, num2, array[6], 17, 2821735955U);
			MD5.FF(ref num2, num3, num4, num, array[7], 22, 4249261313U);
			MD5.FF(ref num, num2, num3, num4, array[8], 7, 1770035416U);
			MD5.FF(ref num4, num, num2, num3, array[9], 12, 2336552879U);
			MD5.FF(ref num3, num4, num, num2, array[10], 17, 4294925233U);
			MD5.FF(ref num2, num3, num4, num, array[11], 22, 2304563134U);
			MD5.FF(ref num, num2, num3, num4, array[12], 7, 1804603682U);
			MD5.FF(ref num4, num, num2, num3, array[13], 12, 4254626195U);
			MD5.FF(ref num3, num4, num, num2, array[14], 17, 2792965006U);
			MD5.FF(ref num2, num3, num4, num, array[15], 22, 1236535329U);
			MD5.GG(ref num, num2, num3, num4, array[1], 5, 4129170786U);
			MD5.GG(ref num4, num, num2, num3, array[6], 9, 3225465664U);
			MD5.GG(ref num3, num4, num, num2, array[11], 14, 643717713U);
			MD5.GG(ref num2, num3, num4, num, array[0], 20, 3921069994U);
			MD5.GG(ref num, num2, num3, num4, array[5], 5, 3593408605U);
			MD5.GG(ref num4, num, num2, num3, array[10], 9, 38016083U);
			MD5.GG(ref num3, num4, num, num2, array[15], 14, 3634488961U);
			MD5.GG(ref num2, num3, num4, num, array[4], 20, 3889429448U);
			MD5.GG(ref num, num2, num3, num4, array[9], 5, 568446438U);
			MD5.GG(ref num4, num, num2, num3, array[14], 9, 3275163606U);
			MD5.GG(ref num3, num4, num, num2, array[3], 14, 4107603335U);
			MD5.GG(ref num2, num3, num4, num, array[8], 20, 1163531501U);
			MD5.GG(ref num, num2, num3, num4, array[13], 5, 2850285829U);
			MD5.GG(ref num4, num, num2, num3, array[2], 9, 4243563512U);
			MD5.GG(ref num3, num4, num, num2, array[7], 14, 1735328473U);
			MD5.GG(ref num2, num3, num4, num, array[12], 20, 2368359562U);
			MD5.HH(ref num, num2, num3, num4, array[5], 4, 4294588738U);
			MD5.HH(ref num4, num, num2, num3, array[8], 11, 2272392833U);
			MD5.HH(ref num3, num4, num, num2, array[11], 16, 1839030562U);
			MD5.HH(ref num2, num3, num4, num, array[14], 23, 4259657740U);
			MD5.HH(ref num, num2, num3, num4, array[1], 4, 2763975236U);
			MD5.HH(ref num4, num, num2, num3, array[4], 11, 1272893353U);
			MD5.HH(ref num3, num4, num, num2, array[7], 16, 4139469664U);
			MD5.HH(ref num2, num3, num4, num, array[10], 23, 3200236656U);
			MD5.HH(ref num, num2, num3, num4, array[13], 4, 681279174U);
			MD5.HH(ref num4, num, num2, num3, array[0], 11, 3936430074U);
			MD5.HH(ref num3, num4, num, num2, array[3], 16, 3572445317U);
			MD5.HH(ref num2, num3, num4, num, array[6], 23, 76029189U);
			MD5.HH(ref num, num2, num3, num4, array[9], 4, 3654602809U);
			MD5.HH(ref num4, num, num2, num3, array[12], 11, 3873151461U);
			MD5.HH(ref num3, num4, num, num2, array[15], 16, 530742520U);
			MD5.HH(ref num2, num3, num4, num, array[2], 23, 3299628645U);
			MD5.II(ref num, num2, num3, num4, array[0], 6, 4096336452U);
			MD5.II(ref num4, num, num2, num3, array[7], 10, 1126891415U);
			MD5.II(ref num3, num4, num, num2, array[14], 15, 2878612391U);
			MD5.II(ref num2, num3, num4, num, array[5], 21, 4237533241U);
			MD5.II(ref num, num2, num3, num4, array[12], 6, 1700485571U);
			MD5.II(ref num4, num, num2, num3, array[3], 10, 2399980690U);
			MD5.II(ref num3, num4, num, num2, array[10], 15, 4293915773U);
			MD5.II(ref num2, num3, num4, num, array[1], 21, 2240044497U);
			MD5.II(ref num, num2, num3, num4, array[8], 6, 1873313359U);
			MD5.II(ref num4, num, num2, num3, array[15], 10, 4264355552U);
			MD5.II(ref num3, num4, num, num2, array[6], 15, 2734768916U);
			MD5.II(ref num2, num3, num4, num, array[13], 21, 1309151649U);
			MD5.II(ref num, num2, num3, num4, array[4], 6, 4149444226U);
			MD5.II(ref num4, num, num2, num3, array[11], 10, 3174756917U);
			MD5.II(ref num3, num4, num, num2, array[2], 15, 718787259U);
			MD5.II(ref num2, num3, num4, num, array[9], 21, 3951481745U);
			this.state[0] += num;
			this.state[1] += num2;
			this.state[2] += num3;
			this.state[3] += num4;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0U;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003FF4 File Offset: 0x000021F4
		private static void Encode(byte[] output, int outputOffset, uint[] input, int inputOffset, int count)
		{
			int num = outputOffset + count;
			int num2 = inputOffset;
			for (int i = outputOffset; i < num; i += 4)
			{
				output[i] = (byte)(input[num2] & 255U);
				output[i + 1] = (byte)(input[num2] >> 8 & 255U);
				output[i + 2] = (byte)(input[num2] >> 16 & 255U);
				output[i + 3] = (byte)(input[num2] >> 24 & 255U);
				num2++;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004060 File Offset: 0x00002260
		private static void Decode(uint[] output, int outputOffset, byte[] input, int inputOffset, int count)
		{
			int num = inputOffset + count;
			int num2 = outputOffset;
			for (int i = inputOffset; i < num; i += 4)
			{
				output[num2] = (uint)((int)input[i] | (int)input[i + 1] << 8 | (int)input[i + 2] << 16 | (int)input[i + 3] << 24);
				num2++;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000040AC File Offset: 0x000022AC
		public virtual bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000040B0 File Offset: 0x000022B0
		public virtual bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004C RID: 76 RVA: 0x000040B4 File Offset: 0x000022B4
		public virtual byte[] Hash
		{
			get
			{
				if (this.State != 0)
				{
					throw new InvalidOperationException();
				}
				return (byte[])this.HashValue.Clone();
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004D RID: 77 RVA: 0x000040D8 File Offset: 0x000022D8
		public virtual int HashSize
		{
			get
			{
				return this.HashSizeValue;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004E RID: 78 RVA: 0x000040E0 File Offset: 0x000022E0
		public virtual int InputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000040E4 File Offset: 0x000022E4
		public virtual int OutputBlockSize
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000040E8 File Offset: 0x000022E8
		public void Clear()
		{
			this.Dispose(true);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000040F4 File Offset: 0x000022F4
		public byte[] ComputeHash(byte[] buffer)
		{
			return this.ComputeHash(buffer, 0, buffer.Length);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004104 File Offset: 0x00002304
		public byte[] ComputeHash(byte[] buffer, int offset, int count)
		{
			this.Initialize();
			this.HashCore(buffer, offset, count);
			this.HashValue = this.HashFinal();
			return (byte[])this.HashValue.Clone();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000413C File Offset: 0x0000233C
		public byte[] ComputeHash(Stream inputStream)
		{
			this.Initialize();
			byte[] input = new byte[4096];
			int num;
			while (0 < (num = inputStream.Read(input, 0, 4096)))
			{
				this.HashCore(input, 0, num);
			}
			this.HashValue = this.HashFinal();
			return (byte[])this.HashValue.Clone();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000419C File Offset: 0x0000239C
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (inputCount < 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException("inputCount");
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (this.State == 0)
			{
				this.Initialize();
				this.State = 1;
			}
			this.HashCore(inputBuffer, inputOffset, inputCount);
			if (inputBuffer != outputBuffer || inputOffset != outputOffset)
			{
				Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
			}
			return inputCount;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000423C File Offset: 0x0000243C
		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (inputOffset < 0)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (inputCount < 0 || inputCount > inputBuffer.Length)
			{
				throw new ArgumentException("inputCount");
			}
			if (inputBuffer.Length - inputCount < inputOffset)
			{
				throw new ArgumentOutOfRangeException("inputOffset");
			}
			if (this.State == 0)
			{
				this.Initialize();
			}
			this.HashCore(inputBuffer, inputOffset, inputCount);
			this.HashValue = this.HashFinal();
			byte[] array = new byte[inputCount];
			Buffer.BlockCopy(inputBuffer, inputOffset, array, 0, inputCount);
			this.State = 0;
			return array;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000042DC File Offset: 0x000024DC
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				this.Initialize();
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000042EC File Offset: 0x000024EC
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0400001D RID: 29
		private const byte S11 = 7;

		// Token: 0x0400001E RID: 30
		private const byte S12 = 12;

		// Token: 0x0400001F RID: 31
		private const byte S13 = 17;

		// Token: 0x04000020 RID: 32
		private const byte S14 = 22;

		// Token: 0x04000021 RID: 33
		private const byte S21 = 5;

		// Token: 0x04000022 RID: 34
		private const byte S22 = 9;

		// Token: 0x04000023 RID: 35
		private const byte S23 = 14;

		// Token: 0x04000024 RID: 36
		private const byte S24 = 20;

		// Token: 0x04000025 RID: 37
		private const byte S31 = 4;

		// Token: 0x04000026 RID: 38
		private const byte S32 = 11;

		// Token: 0x04000027 RID: 39
		private const byte S33 = 16;

		// Token: 0x04000028 RID: 40
		private const byte S34 = 23;

		// Token: 0x04000029 RID: 41
		private const byte S41 = 6;

		// Token: 0x0400002A RID: 42
		private const byte S42 = 10;

		// Token: 0x0400002B RID: 43
		private const byte S43 = 15;

		// Token: 0x0400002C RID: 44
		private const byte S44 = 21;

		// Token: 0x0400002D RID: 45
		private static byte[] PADDING;

		// Token: 0x0400002E RID: 46
		private uint[] state = new uint[4];

		// Token: 0x0400002F RID: 47
		private uint[] count = new uint[2];

		// Token: 0x04000030 RID: 48
		private byte[] buffer = new byte[64];

		// Token: 0x04000031 RID: 49
		protected byte[] HashValue;

		// Token: 0x04000032 RID: 50
		protected int State;

		// Token: 0x04000033 RID: 51
		protected int HashSizeValue = 128;
	}
}
