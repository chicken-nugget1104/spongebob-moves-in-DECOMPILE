using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000025 RID: 37
	public sealed class SubscriptionExpiredEvent : Jsonable
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000659C File Offset: 0x0000479C
		// (set) Token: 0x0600014D RID: 333 RVA: 0x000065A4 File Offset: 0x000047A4
		public string Sku { get; set; }

		// Token: 0x0600014E RID: 334 RVA: 0x000065B0 File Offset: 0x000047B0
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

		// Token: 0x0600014F RID: 335 RVA: 0x00006604 File Offset: 0x00004804
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

		// Token: 0x06000150 RID: 336 RVA: 0x00006664 File Offset: 0x00004864
		public static SubscriptionExpiredEvent CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			SubscriptionExpiredEvent result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					SubscriptionExpiredEvent subscriptionExpiredEvent = new SubscriptionExpiredEvent();
					if (jsonMap.ContainsKey("sku"))
					{
						subscriptionExpiredEvent.Sku = (string)jsonMap["sku"];
					}
					result = subscriptionExpiredEvent;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000066E8 File Offset: 0x000048E8
		public static SubscriptionExpiredEvent CreateFromJson(string jsonMessage)
		{
			SubscriptionExpiredEvent result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = SubscriptionExpiredEvent.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006748 File Offset: 0x00004948
		public static Dictionary<string, SubscriptionExpiredEvent> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SubscriptionExpiredEvent> dictionary = new Dictionary<string, SubscriptionExpiredEvent>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				SubscriptionExpiredEvent value = SubscriptionExpiredEvent.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000067CC File Offset: 0x000049CC
		public static List<SubscriptionExpiredEvent> ListFromJson(List<object> array)
		{
			List<SubscriptionExpiredEvent> list = new List<SubscriptionExpiredEvent>();
			foreach (object obj in array)
			{
				list.Add(SubscriptionExpiredEvent.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
