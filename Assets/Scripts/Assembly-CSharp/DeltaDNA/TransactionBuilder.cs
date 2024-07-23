using System;

namespace DeltaDNA
{
	// Token: 0x02000011 RID: 17
	public class TransactionBuilder
	{
		// Token: 0x060000BE RID: 190 RVA: 0x000057FC File Offset: 0x000039FC
		internal TransactionBuilder(SDK sdk)
		{
			this.sdk = sdk;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000580C File Offset: 0x00003A0C
		public void BuyVirtualCurrency(string transactionName, string realCurrencyType, int realCurrencyAmount, string virtualCurrencyName, string virtualCurrencyType, int virtualCurrencyAmount, string transactionReceipt = null)
		{
			EventBuilder eventParams = new EventBuilder().AddParam("transactionType", "PURCHASE").AddParam("transactionName", transactionName).AddParam("productsSpent", new ProductBuilder().AddRealCurrency(realCurrencyType, realCurrencyAmount)).AddParam("productsReceived", new ProductBuilder().AddVirtualCurrency(virtualCurrencyName, virtualCurrencyType, virtualCurrencyAmount)).AddParam("transactionReceipt", transactionReceipt);
			this.sdk.RecordEvent("transaction", eventParams);
		}

		// Token: 0x0400006A RID: 106
		private SDK sdk;
	}
}
