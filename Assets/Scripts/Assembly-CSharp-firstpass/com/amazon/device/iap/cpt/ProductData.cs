using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x0200001D RID: 29
	public sealed class ProductData : Jsonable
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004EC8 File Offset: 0x000030C8
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004ED0 File Offset: 0x000030D0
		public string Sku { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004EDC File Offset: 0x000030DC
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004EE4 File Offset: 0x000030E4
		public string ProductType { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004EF0 File Offset: 0x000030F0
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004EF8 File Offset: 0x000030F8
		public string Price { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004F04 File Offset: 0x00003104
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00004F0C File Offset: 0x0000310C
		public string Title { get; set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004F18 File Offset: 0x00003118
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00004F20 File Offset: 0x00003120
		public string Description { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00004F2C File Offset: 0x0000312C
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00004F34 File Offset: 0x00003134
		public string SmallIconUrl { get; set; }

		// Token: 0x060000FC RID: 252 RVA: 0x00004F40 File Offset: 0x00003140
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

		// Token: 0x060000FD RID: 253 RVA: 0x00004F94 File Offset: 0x00003194
		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> result;
			try
			{
				result = new Dictionary<string, object>
				{
					{
						"sku",
						this.Sku
					},
					{
						"productType",
						this.ProductType
					},
					{
						"price",
						this.Price
					},
					{
						"title",
						this.Title
					},
					{
						"description",
						this.Description
					},
					{
						"smallIconUrl",
						this.SmallIconUrl
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005048 File Offset: 0x00003248
		public static ProductData CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			ProductData result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					ProductData productData = new ProductData();
					if (jsonMap.ContainsKey("sku"))
					{
						productData.Sku = (string)jsonMap["sku"];
					}
					if (jsonMap.ContainsKey("productType"))
					{
						productData.ProductType = (string)jsonMap["productType"];
					}
					if (jsonMap.ContainsKey("price"))
					{
						productData.Price = (string)jsonMap["price"];
					}
					if (jsonMap.ContainsKey("title"))
					{
						productData.Title = (string)jsonMap["title"];
					}
					if (jsonMap.ContainsKey("description"))
					{
						productData.Description = (string)jsonMap["description"];
					}
					if (jsonMap.ContainsKey("smallIconUrl"))
					{
						productData.SmallIconUrl = (string)jsonMap["smallIconUrl"];
					}
					result = productData;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005188 File Offset: 0x00003388
		public static ProductData CreateFromJson(string jsonMessage)
		{
			ProductData result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = ProductData.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000051E8 File Offset: 0x000033E8
		public static Dictionary<string, ProductData> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, ProductData> dictionary = new Dictionary<string, ProductData>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				ProductData value = ProductData.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000526C File Offset: 0x0000346C
		public static List<ProductData> ListFromJson(List<object> array)
		{
			List<ProductData> list = new List<ProductData>();
			foreach (object obj in array)
			{
				list.Add(ProductData.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
