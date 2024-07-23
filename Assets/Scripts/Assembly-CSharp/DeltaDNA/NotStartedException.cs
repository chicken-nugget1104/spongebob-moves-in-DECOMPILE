using System;

namespace DeltaDNA
{
	// Token: 0x0200000A RID: 10
	public class NotStartedException : Exception
	{
		// Token: 0x06000058 RID: 88 RVA: 0x000042F8 File Offset: 0x000024F8
		public NotStartedException()
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004300 File Offset: 0x00002500
		public NotStartedException(string message) : base(message)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000430C File Offset: 0x0000250C
		public NotStartedException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}
