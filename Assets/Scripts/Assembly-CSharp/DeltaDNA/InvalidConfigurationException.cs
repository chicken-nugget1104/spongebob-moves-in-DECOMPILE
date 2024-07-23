using System;

namespace DeltaDNA
{
	// Token: 0x02000007 RID: 7
	public class InvalidConfigurationException : Exception
	{
		// Token: 0x06000032 RID: 50 RVA: 0x000035F8 File Offset: 0x000017F8
		public InvalidConfigurationException()
		{
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003600 File Offset: 0x00001800
		public InvalidConfigurationException(string message) : base(message)
		{
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000360C File Offset: 0x0000180C
		public InvalidConfigurationException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
