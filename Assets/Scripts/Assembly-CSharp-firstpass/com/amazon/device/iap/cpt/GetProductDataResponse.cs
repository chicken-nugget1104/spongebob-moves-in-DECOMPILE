using System;
using System.Collections.Generic;
using System.Linq;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000014 RID: 20
	public sealed class GetProductDataResponse : Jsonable
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004010 File Offset: 0x00002210
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00004018 File Offset: 0x00002218
		public string RequestId { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00004024 File Offset: 0x00002224
		// (set) Token: 0x0600009D RID: 157 RVA: 0x0000402C File Offset: 0x0000222C
		public Dictionary<string, ProductData> ProductDataMap { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004038 File Offset: 0x00002238
		// (set) Token: 0x0600009F RID: 159 RVA: 0x00004040 File Offset: 0x00002240
		public List<string> UnavailableSkus { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000404C File Offset: 0x0000224C
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00004054 File Offset: 0x00002254
		public string Status { get; set; }

		// Token: 0x060000A2 RID: 162 RVA: 0x00004060 File Offset: 0x00002260
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

		// Token: 0x060000A3 RID: 163 RVA: 0x000040B4 File Offset: 0x000022B4
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
						"productDataMap",
						(this.ProductDataMap == null) ? null : Jsonable.unrollObjectIntoMap<ProductData>(this.ProductDataMap)
					},
					{
						"unavailableSkus",
						this.UnavailableSkus
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

		// Token: 0x060000A4 RID: 164 RVA: 0x0000415C File Offset: 0x0000235C
		public static GetProductDataResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			GetProductDataResponse result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					GetProductDataResponse getProductDataResponse = new GetProductDataResponse();
					if (jsonMap.ContainsKey("requestId"))
					{
						getProductDataResponse.RequestId = (string)jsonMap["requestId"];
					}
					if (jsonMap.ContainsKey("productDataMap"))
					{
						getProductDataResponse.ProductDataMap = ProductData.MapFromJson(jsonMap["productDataMap"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("unavailableSkus"))
					{
						getProductDataResponse.UnavailableSkus = (from element in (List<object>)jsonMap["unavailableSkus"]
						select (string)element).ToList<string>();
					}
					if (jsonMap.ContainsKey("status"))
					{
						getProductDataResponse.Status = (string)jsonMap["status"];
					}
					result = getProductDataResponse;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000427C File Offset: 0x0000247C
		public static GetProductDataResponse CreateFromJson(string jsonMessage)
		{
			GetProductDataResponse result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = GetProductDataResponse.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000042DC File Offset: 0x000024DC
		public static Dictionary<string, GetProductDataResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, GetProductDataResponse> dictionary = new Dictionary<string, GetProductDataResponse>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				GetProductDataResponse value = GetProductDataResponse.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004360 File Offset: 0x00002560
		public static List<GetProductDataResponse> ListFromJson(List<object> array)
		{
			List<GetProductDataResponse> list = new List<GetProductDataResponse>();
			foreach (object obj in array)
			{
				list.Add(GetProductDataResponse.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
