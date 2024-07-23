using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000023 RID: 35
	public sealed class SkuInput : Jsonable
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00006018 File Offset: 0x00004218
		// (set) Token: 0x0600013A RID: 314 RVA: 0x00006020 File Offset: 0x00004220
		public string Sku { get; set; }

		// Token: 0x0600013B RID: 315 RVA: 0x0000602C File Offset: 0x0000422C
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

		// Token: 0x0600013C RID: 316 RVA: 0x00006080 File Offset: 0x00004280
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
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000060E0 File Offset: 0x000042E0
		public static SkuInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			SkuInput result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					SkuInput skuInput = new SkuInput();
					if (jsonMap.ContainsKey("sku"))
					{
						skuInput.Sku = (string)jsonMap["sku"];
					}
					result = skuInput;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006164 File Offset: 0x00004364
		public static SkuInput CreateFromJson(string jsonMessage)
		{
			SkuInput result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = SkuInput.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000061C4 File Offset: 0x000043C4
		public static Dictionary<string, SkuInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SkuInput> dictionary = new Dictionary<string, SkuInput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				SkuInput value = SkuInput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006248 File Offset: 0x00004448
		public static List<SkuInput> ListFromJson(List<object> array)
		{
			List<SkuInput> list = new List<SkuInput>();
			foreach (object obj in array)
			{
				list.Add(SkuInput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
