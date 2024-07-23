using System;
using System.Runtime.Serialization;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x0200000A RID: 10
	public class AmazonException : ApplicationException
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002E50 File Offset: 0x00001050
		public AmazonException()
		{
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002E58 File Offset: 0x00001058
		public AmazonException(string message) : base(message)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002E64 File Offset: 0x00001064
		public AmazonException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002E70 File Offset: 0x00001070
		protected AmazonException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
