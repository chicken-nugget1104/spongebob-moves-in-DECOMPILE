using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000015 RID: 21
	public sealed class GetProductDataResponseDelegator : IDelegator
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x000043DC File Offset: 0x000025DC
		public GetProductDataResponseDelegator(GetProductDataResponseDelegate responseDelegate)
		{
			this.responseDelegate = responseDelegate;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000043EC File Offset: 0x000025EC
		public void ExecuteSuccess()
		{
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000043F0 File Offset: 0x000025F0
		public void ExecuteSuccess(Dictionary<string, object> objectDictionary)
		{
			this.responseDelegate(GetProductDataResponse.CreateFromDictionary(objectDictionary));
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004404 File Offset: 0x00002604
		public void ExecuteError(AmazonException e)
		{
		}

		// Token: 0x04000039 RID: 57
		public readonly GetProductDataResponseDelegate responseDelegate;
	}
}
