using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000013 RID: 19
	public sealed class AmazonUserData : Jsonable
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003D1C File Offset: 0x00001F1C
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003D24 File Offset: 0x00001F24
		public string UserId { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003D30 File Offset: 0x00001F30
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00003D38 File Offset: 0x00001F38
		public string Marketplace { get; set; }

		// Token: 0x06000093 RID: 147 RVA: 0x00003D44 File Offset: 0x00001F44
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

		// Token: 0x06000094 RID: 148 RVA: 0x00003D98 File Offset: 0x00001F98
		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> result;
			try
			{
				result = new Dictionary<string, object>
				{
					{
						"userId",
						this.UserId
					},
					{
						"marketplace",
						this.Marketplace
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003E08 File Offset: 0x00002008
		public static AmazonUserData CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			AmazonUserData result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					AmazonUserData amazonUserData = new AmazonUserData();
					if (jsonMap.ContainsKey("userId"))
					{
						amazonUserData.UserId = (string)jsonMap["userId"];
					}
					if (jsonMap.ContainsKey("marketplace"))
					{
						amazonUserData.Marketplace = (string)jsonMap["marketplace"];
					}
					result = amazonUserData;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003EB0 File Offset: 0x000020B0
		public static AmazonUserData CreateFromJson(string jsonMessage)
		{
			AmazonUserData result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = AmazonUserData.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003F10 File Offset: 0x00002110
		public static Dictionary<string, AmazonUserData> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, AmazonUserData> dictionary = new Dictionary<string, AmazonUserData>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				AmazonUserData value = AmazonUserData.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003F94 File Offset: 0x00002194
		public static List<AmazonUserData> ListFromJson(List<object> array)
		{
			List<AmazonUserData> list = new List<AmazonUserData>();
			foreach (object obj in array)
			{
				list.Add(AmazonUserData.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
