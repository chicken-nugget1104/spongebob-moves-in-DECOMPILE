using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000020 RID: 32
	public sealed class PurchaseResponseDelegator : IDelegator
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00005A88 File Offset: 0x00003C88
		public PurchaseResponseDelegator(PurchaseResponseDelegate responseDelegate)
		{
			this.responseDelegate = responseDelegate;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005A98 File Offset: 0x00003C98
		public void ExecuteSuccess()
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005A9C File Offset: 0x00003C9C
		public void ExecuteSuccess(Dictionary<string, object> objectDictionary)
		{
			this.responseDelegate(PurchaseResponse.CreateFromDictionary(objectDictionary));
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005AB0 File Offset: 0x00003CB0
		public void ExecuteError(AmazonException e)
		{
		}

		// Token: 0x04000055 RID: 85
		public readonly PurchaseResponseDelegate responseDelegate;
	}
}
