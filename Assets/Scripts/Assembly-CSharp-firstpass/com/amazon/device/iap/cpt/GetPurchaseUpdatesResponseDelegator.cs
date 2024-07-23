using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000017 RID: 23
	public sealed class GetPurchaseUpdatesResponseDelegator : IDelegator
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00004818 File Offset: 0x00002A18
		public GetPurchaseUpdatesResponseDelegator(GetPurchaseUpdatesResponseDelegate responseDelegate)
		{
			this.responseDelegate = responseDelegate;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004828 File Offset: 0x00002A28
		public void ExecuteSuccess()
		{
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000482C File Offset: 0x00002A2C
		public void ExecuteSuccess(Dictionary<string, object> objectDictionary)
		{
			this.responseDelegate(GetPurchaseUpdatesResponse.CreateFromDictionary(objectDictionary));
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004840 File Offset: 0x00002A40
		public void ExecuteError(AmazonException e)
		{
		}

		// Token: 0x0400003F RID: 63
		public readonly GetPurchaseUpdatesResponseDelegate responseDelegate;
	}
}
