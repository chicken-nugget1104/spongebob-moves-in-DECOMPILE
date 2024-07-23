using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x0200001B RID: 27
	public interface IDelegator
	{
		// Token: 0x060000E1 RID: 225
		void ExecuteSuccess();

		// Token: 0x060000E2 RID: 226
		void ExecuteSuccess(Dictionary<string, object> objDict);

		// Token: 0x060000E3 RID: 227
		void ExecuteError(AmazonException e);
	}
}
