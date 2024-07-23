using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x0200001C RID: 28
	public sealed class NotifyFulfillmentInput : Jsonable
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00004BD4 File Offset: 0x00002DD4
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00004BDC File Offset: 0x00002DDC
		public string ReceiptId { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004BE8 File Offset: 0x00002DE8
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00004BF0 File Offset: 0x00002DF0
		public string FulfillmentResult { get; set; }

		// Token: 0x060000E9 RID: 233 RVA: 0x00004BFC File Offset: 0x00002DFC
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

		// Token: 0x060000EA RID: 234 RVA: 0x00004C50 File Offset: 0x00002E50
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
						"fulfillmentResult",
						this.FulfillmentResult
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004CC0 File Offset: 0x00002EC0
		public static NotifyFulfillmentInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			NotifyFulfillmentInput result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					NotifyFulfillmentInput notifyFulfillmentInput = new NotifyFulfillmentInput();
					if (jsonMap.ContainsKey("receiptId"))
					{
						notifyFulfillmentInput.ReceiptId = (string)jsonMap["receiptId"];
					}
					if (jsonMap.ContainsKey("fulfillmentResult"))
					{
						notifyFulfillmentInput.FulfillmentResult = (string)jsonMap["fulfillmentResult"];
					}
					result = notifyFulfillmentInput;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004D68 File Offset: 0x00002F68
		public static NotifyFulfillmentInput CreateFromJson(string jsonMessage)
		{
			NotifyFulfillmentInput result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = NotifyFulfillmentInput.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004DC8 File Offset: 0x00002FC8
		public static Dictionary<string, NotifyFulfillmentInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, NotifyFulfillmentInput> dictionary = new Dictionary<string, NotifyFulfillmentInput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				NotifyFulfillmentInput value = NotifyFulfillmentInput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00004E4C File Offset: 0x0000304C
		public static List<NotifyFulfillmentInput> ListFromJson(List<object> array)
		{
			List<NotifyFulfillmentInput> list = new List<NotifyFulfillmentInput>();
			foreach (object obj in array)
			{
				list.Add(NotifyFulfillmentInput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
