using System;
using System.Collections.Generic;
using System.Linq;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000024 RID: 36
	public sealed class SkusInput : Jsonable
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000062C4 File Offset: 0x000044C4
		// (set) Token: 0x06000143 RID: 323 RVA: 0x000062CC File Offset: 0x000044CC
		public List<string> Skus { get; set; }

		// Token: 0x06000144 RID: 324 RVA: 0x000062D8 File Offset: 0x000044D8
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

		// Token: 0x06000145 RID: 325 RVA: 0x0000632C File Offset: 0x0000452C
		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> result;
			try
			{
				result = new Dictionary<string, object>
				{
					{
						"skus",
						this.Skus
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000638C File Offset: 0x0000458C
		public static SkusInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			SkusInput result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					SkusInput skusInput = new SkusInput();
					if (jsonMap.ContainsKey("skus"))
					{
						skusInput.Skus = (from element in (List<object>)jsonMap["skus"]
						select (string)element).ToList<string>();
					}
					result = skusInput;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006434 File Offset: 0x00004634
		public static SkusInput CreateFromJson(string jsonMessage)
		{
			SkusInput result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = SkusInput.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006494 File Offset: 0x00004694
		public static Dictionary<string, SkusInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, SkusInput> dictionary = new Dictionary<string, SkusInput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				SkusInput value = SkusInput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00006518 File Offset: 0x00004718
		public static List<SkusInput> ListFromJson(List<object> array)
		{
			List<SkusInput> list = new List<SkusInput>();
			foreach (object obj in array)
			{
				list.Add(SkusInput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
