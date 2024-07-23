using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000019 RID: 25
	public sealed class GetUserDataResponseDelegator : IDelegator
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00004BA0 File Offset: 0x00002DA0
		public GetUserDataResponseDelegator(GetUserDataResponseDelegate responseDelegate)
		{
			this.responseDelegate = responseDelegate;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004BB0 File Offset: 0x00002DB0
		public void ExecuteSuccess()
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004BB4 File Offset: 0x00002DB4
		public void ExecuteSuccess(Dictionary<string, object> objectDictionary)
		{
			this.responseDelegate(GetUserDataResponse.CreateFromDictionary(objectDictionary));
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004BC8 File Offset: 0x00002DC8
		public void ExecuteError(AmazonException e)
		{
		}

		// Token: 0x04000043 RID: 67
		public readonly GetUserDataResponseDelegate responseDelegate;
	}
}
