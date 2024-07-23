using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000016 RID: 22
	public sealed class GetPurchaseUpdatesResponse : Jsonable
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004410 File Offset: 0x00002610
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00004418 File Offset: 0x00002618
		public string RequestId { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004424 File Offset: 0x00002624
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x0000442C File Offset: 0x0000262C
		public AmazonUserData AmazonUserData { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004438 File Offset: 0x00002638
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x00004440 File Offset: 0x00002640
		public List<PurchaseReceipt> Receipts { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000444C File Offset: 0x0000264C
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00004454 File Offset: 0x00002654
		public string Status { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004460 File Offset: 0x00002660
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00004468 File Offset: 0x00002668
		public bool HasMore { get; set; }

		// Token: 0x060000B8 RID: 184 RVA: 0x00004474 File Offset: 0x00002674
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

		// Token: 0x060000B9 RID: 185 RVA: 0x000044C8 File Offset: 0x000026C8
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
						"receipts",
						(this.Receipts == null) ? null : Jsonable.unrollObjectIntoList<PurchaseReceipt>(this.Receipts)
					},
					{
						"status",
						this.Status
					},
					{
						"hasMore",
						this.HasMore
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000459C File Offset: 0x0000279C
		public static GetPurchaseUpdatesResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			GetPurchaseUpdatesResponse result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					GetPurchaseUpdatesResponse getPurchaseUpdatesResponse = new GetPurchaseUpdatesResponse();
					if (jsonMap.ContainsKey("requestId"))
					{
						getPurchaseUpdatesResponse.RequestId = (string)jsonMap["requestId"];
					}
					if (jsonMap.ContainsKey("amazonUserData"))
					{
						getPurchaseUpdatesResponse.AmazonUserData = AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("receipts"))
					{
						getPurchaseUpdatesResponse.Receipts = PurchaseReceipt.ListFromJson(jsonMap["receipts"] as List<object>);
					}
					if (jsonMap.ContainsKey("status"))
					{
						getPurchaseUpdatesResponse.Status = (string)jsonMap["status"];
					}
					if (jsonMap.ContainsKey("hasMore"))
					{
						getPurchaseUpdatesResponse.HasMore = (bool)jsonMap["hasMore"];
					}
					result = getPurchaseUpdatesResponse;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000046C0 File Offset: 0x000028C0
		public static GetPurchaseUpdatesResponse CreateFromJson(string jsonMessage)
		{
			GetPurchaseUpdatesResponse result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = GetPurchaseUpdatesResponse.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004720 File Offset: 0x00002920
		public static Dictionary<string, GetPurchaseUpdatesResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, GetPurchaseUpdatesResponse> dictionary = new Dictionary<string, GetPurchaseUpdatesResponse>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				GetPurchaseUpdatesResponse value = GetPurchaseUpdatesResponse.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000047A4 File Offset: 0x000029A4
		public static List<GetPurchaseUpdatesResponse> ListFromJson(List<object> array)
		{
			List<GetPurchaseUpdatesResponse> list = new List<GetPurchaseUpdatesResponse>();
			foreach (object obj in array)
			{
				list.Add(GetPurchaseUpdatesResponse.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
