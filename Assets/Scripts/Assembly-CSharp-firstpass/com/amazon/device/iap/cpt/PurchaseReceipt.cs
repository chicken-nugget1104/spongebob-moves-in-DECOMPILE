using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x0200001E RID: 30
	public sealed class PurchaseReceipt : Jsonable
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000052E8 File Offset: 0x000034E8
		// (set) Token: 0x06000104 RID: 260 RVA: 0x000052F0 File Offset: 0x000034F0
		public string ReceiptId { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000052FC File Offset: 0x000034FC
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00005304 File Offset: 0x00003504
		public long CancelDate { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005310 File Offset: 0x00003510
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00005318 File Offset: 0x00003518
		public long PurchaseDate { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005324 File Offset: 0x00003524
		// (set) Token: 0x0600010A RID: 266 RVA: 0x0000532C File Offset: 0x0000352C
		public string Sku { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005338 File Offset: 0x00003538
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00005340 File Offset: 0x00003540
		public string ProductType { get; set; }

		// Token: 0x0600010D RID: 269 RVA: 0x0000534C File Offset: 0x0000354C
		public string ToJson()
		{
			string result;
			try
			{
				Dictionary<string, object> objectDictionary = this.GetObjectDictionary();
				result = Json.Serialize(objectDictionary);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while Jsoning", inner);
			}
			return result;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000053A0 File Offset: 0x000035A0
		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> result;
			try
			{
				result = new Dictionary<string, object>
				{
					{
						"receiptId",
						this.ReceiptId
					},
					{
						"cancelDate",
						this.CancelDate
					},
					{
						"purchaseDate",
						this.PurchaseDate
					},
					{
						"sku",
						this.Sku
					},
					{
						"productType",
						this.ProductType
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005450 File Offset: 0x00003650
		public static PurchaseReceipt CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			PurchaseReceipt result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					PurchaseReceipt purchaseReceipt = new PurchaseReceipt();
					if (jsonMap.ContainsKey("receiptId"))
					{
						purchaseReceipt.ReceiptId = (string)jsonMap["receiptId"];
					}
					if (jsonMap.ContainsKey("cancelDate"))
					{
						purchaseReceipt.CancelDate = (long)jsonMap["cancelDate"];
					}
					if (jsonMap.ContainsKey("purchaseDate"))
					{
						purchaseReceipt.PurchaseDate = (long)jsonMap["purchaseDate"];
					}
					if (jsonMap.ContainsKey("sku"))
					{
						purchaseReceipt.Sku = (string)jsonMap["sku"];
					}
					if (jsonMap.ContainsKey("productType"))
					{
						purchaseReceipt.ProductType = (string)jsonMap["productType"];
					}
					result = purchaseReceipt;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000556C File Offset: 0x0000376C
		public static PurchaseReceipt CreateFromJson(string jsonMessage)
		{
			PurchaseReceipt result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = PurchaseReceipt.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000055CC File Offset: 0x000037CC
		public static Dictionary<string, PurchaseReceipt> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, PurchaseReceipt> dictionary = new Dictionary<string, PurchaseReceipt>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				PurchaseReceipt value = PurchaseReceipt.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005650 File Offset: 0x00003850
		public static List<PurchaseReceipt> ListFromJson(List<object> array)
		{
			List<PurchaseReceipt> list = new List<PurchaseReceipt>();
			foreach (object obj in array)
			{
				list.Add(PurchaseReceipt.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
