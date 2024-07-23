using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000021 RID: 33
	public sealed class RequestOutput : Jsonable
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00005ABC File Offset: 0x00003CBC
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00005AC4 File Offset: 0x00003CC4
		public string RequestId { get; set; }

		// Token: 0x06000129 RID: 297 RVA: 0x00005AD0 File Offset: 0x00003CD0
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

		// Token: 0x0600012A RID: 298 RVA: 0x00005B24 File Offset: 0x00003D24
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
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005B84 File Offset: 0x00003D84
		public static RequestOutput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			RequestOutput result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					RequestOutput requestOutput = new RequestOutput();
					if (jsonMap.ContainsKey("requestId"))
					{
						requestOutput.RequestId = (string)jsonMap["requestId"];
					}
					result = requestOutput;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005C08 File Offset: 0x00003E08
		public static RequestOutput CreateFromJson(string jsonMessage)
		{
			RequestOutput result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = RequestOutput.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005C68 File Offset: 0x00003E68
		public static Dictionary<string, RequestOutput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, RequestOutput> dictionary = new Dictionary<string, RequestOutput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				RequestOutput value = RequestOutput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005CEC File Offset: 0x00003EEC
		public static List<RequestOutput> ListFromJson(List<object> array)
		{
			List<RequestOutput> list = new List<RequestOutput>();
			foreach (object obj in array)
			{
				list.Add(RequestOutput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
