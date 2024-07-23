using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000018 RID: 24
	public sealed class GetUserDataResponse : Jsonable
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000484C File Offset: 0x00002A4C
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00004854 File Offset: 0x00002A54
		public string RequestId { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00004860 File Offset: 0x00002A60
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00004868 File Offset: 0x00002A68
		public AmazonUserData AmazonUserData { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004874 File Offset: 0x00002A74
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x0000487C File Offset: 0x00002A7C
		public string Status { get; set; }

		// Token: 0x060000C9 RID: 201 RVA: 0x00004888 File Offset: 0x00002A88
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

		// Token: 0x060000CA RID: 202 RVA: 0x000048DC File Offset: 0x00002ADC
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

		// Token: 0x060000CB RID: 203 RVA: 0x00004974 File Offset: 0x00002B74
		public static GetUserDataResponse CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			GetUserDataResponse result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					GetUserDataResponse getUserDataResponse = new GetUserDataResponse();
					if (jsonMap.ContainsKey("requestId"))
					{
						getUserDataResponse.RequestId = (string)jsonMap["requestId"];
					}
					if (jsonMap.ContainsKey("amazonUserData"))
					{
						getUserDataResponse.AmazonUserData = AmazonUserData.CreateFromDictionary(jsonMap["amazonUserData"] as Dictionary<string, object>);
					}
					if (jsonMap.ContainsKey("status"))
					{
						getUserDataResponse.Status = (string)jsonMap["status"];
					}
					result = getUserDataResponse;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004A48 File Offset: 0x00002C48
		public static GetUserDataResponse CreateFromJson(string jsonMessage)
		{
			GetUserDataResponse result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = GetUserDataResponse.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004AA8 File Offset: 0x00002CA8
		public static Dictionary<string, GetUserDataResponse> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, GetUserDataResponse> dictionary = new Dictionary<string, GetUserDataResponse>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				GetUserDataResponse value = GetUserDataResponse.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004B2C File Offset: 0x00002D2C
		public static List<GetUserDataResponse> ListFromJson(List<object> array)
		{
			List<GetUserDataResponse> list = new List<GetUserDataResponse>();
			foreach (object obj in array)
			{
				list.Add(GetUserDataResponse.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
