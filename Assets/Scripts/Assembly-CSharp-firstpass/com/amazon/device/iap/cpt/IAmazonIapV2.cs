using System;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x0200001A RID: 26
	public interface IAmazonIapV2
	{
		// Token: 0x060000D3 RID: 211
		RequestOutput GetUserData();

		// Token: 0x060000D4 RID: 212
		RequestOutput Purchase(SkuInput skuInput);

		// Token: 0x060000D5 RID: 213
		RequestOutput GetProductData(SkusInput skusInput);

		// Token: 0x060000D6 RID: 214
		RequestOutput GetPurchaseUpdates(ResetInput resetInput);

		// Token: 0x060000D7 RID: 215
		void NotifyFulfillment(NotifyFulfillmentInput notifyFulfillmentInput);

		// Token: 0x060000D8 RID: 216
		void UnityFireEvent(string jsonMessage);

		// Token: 0x060000D9 RID: 217
		void AddGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		// Token: 0x060000DA RID: 218
		void RemoveGetUserDataResponseListener(GetUserDataResponseDelegate responseDelegate);

		// Token: 0x060000DB RID: 219
		void AddPurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		// Token: 0x060000DC RID: 220
		void RemovePurchaseResponseListener(PurchaseResponseDelegate responseDelegate);

		// Token: 0x060000DD RID: 221
		void AddGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		// Token: 0x060000DE RID: 222
		void RemoveGetProductDataResponseListener(GetProductDataResponseDelegate responseDelegate);

		// Token: 0x060000DF RID: 223
		void AddGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);

		// Token: 0x060000E0 RID: 224
		void RemoveGetPurchaseUpdatesResponseListener(GetPurchaseUpdatesResponseDelegate responseDelegate);
	}
}
