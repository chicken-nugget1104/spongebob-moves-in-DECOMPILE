using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x0200001F RID: 31
	public sealed class PurchaseResponse : Jsonable
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000056CC File Offset: 0x000038CC
		// (set) Token: 0x06000115 RID: 277 RVA: 0x000056D4 File Offset: 0x000038D4
		public string RequestId { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000056E0 File Offset: 0x000038E0
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000056E8 File Offset: 0x000038E8
		public AmazonUserData AmazonUserData { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000056F4 File Offset: 0x000038F4
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000056FC File Offset: 0x000038FC
		public PurchaseReceipt PurchaseReceipt { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00005708 File Offset: 0x00003908
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00005710 File Offset: 0x00003910
		public string Status { get; set; }

		// Token: 0x0600011C RID: 284 RVA: 0x0000571C File Offset: 0x0000391C
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

		// Token: 0x0600011D RID: 285 RVA: 0x00005770 File Offset: 0x00003970
		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> result;
			try
			{
				result = new Dictionary<string, object>
				{
					{
						"requestId",
						this.RequestId
					},
					{
						"amazonUserData",
						(this.AmazonUserData == null) ? null : this.AmazonUserData.GetObjectDictionary()
					},
					{
						"purchaseReceipt",
						(this.PurchaseReceipt == null) ? null : this.PurchaseReceipt.GetObjectDictionary()
					},
					{
						"status",
						this.Status
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005830 File Offset: 0x00003A30
		public static PurchaseResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			PurchaseResponse result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					PurchaseResponse purchaseResponse = new PurchaseResponse();
					if (jsonMap.ContainsKey("requestId"))
					{
						purchaseResponse.RequestId = (string)jsonMap["requestId"];
					}
					if (jsonMap.ContainsKey("amazonUserData"))
					{
						purchaseResponse.AmazonUserData = AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("purchaseReceipt"))
					{
						purchaseResponse.PurchaseReceipt = PurchaseReceipt.CreateFromDictionary(jsonMap["purchaseReceipt"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("status"))
					{
						purchaseResponse.Status = (string)jsonMap["status"];
					}
					result = purchaseResponse;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005930 File Offset: 0x00003B30
		public static PurchaseResponse CreateFromJson(string jsonMessage)
		{
			PurchaseResponse result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = PurchaseResponse.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005990 File Offset: 0x00003B90
		public static Dictionary<string, PurchaseResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, PurchaseResponse> dictionary = new Dictionary<string, PurchaseResponse>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				PurchaseResponse value = PurchaseResponse.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005A14 File Offset: 0x00003C14
		public static List<PurchaseResponse> ListFromJson(List<object> array)
		{
			List<PurchaseResponse> list = new List<PurchaseResponse>();
			foreach (object obj in array)
			{
				list.Add(PurchaseResponse.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
