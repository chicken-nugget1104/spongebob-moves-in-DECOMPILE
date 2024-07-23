using System;
using System.Collections.Generic;
using com.amazon.device.iap.cpt.json;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000022 RID: 34
	public sealed class ResetInput : Jsonable
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00005D68 File Offset: 0x00003F68
		// (set) Token: 0x06000131 RID: 305 RVA: 0x00005D70 File Offset: 0x00003F70
		public bool Reset { get; set; }

		// Token: 0x06000132 RID: 306 RVA: 0x00005D7C File Offset: 0x00003F7C
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

		// Token: 0x06000133 RID: 307 RVA: 0x00005DD0 File Offset: 0x00003FD0
		public override Dictionary<string, object> GetObjectDictionary()
		{
			Dictionary<string, object> result;
			try
			{
				result = new Dictionary<string, object>
				{
					{
						"reset",
						this.Reset
					}
				};
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while getting object dictionary", inner);
			}
			return result;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005E34 File Offset: 0x00004034
		public static ResetInput CreateFromDictionary(Dictionary<string, object> jsonMap)
		{
			ResetInput result;
			try
			{
				if (jsonMap == null)
				{
					result = null;
				}
				else
				{
					ResetInput resetInput = new ResetInput();
					if (jsonMap.ContainsKey("reset"))
					{
						resetInput.Reset = (bool)jsonMap["reset"];
					}
					result = resetInput;
				}
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while creating Object from dicionary", inner);
			}
			return result;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005EB8 File Offset: 0x000040B8
		public static ResetInput CreateFromJson(string jsonMessage)
		{
			ResetInput result;
			try
			{
				Dictionary<string, object> jsonMap = Json.Deserialize(jsonMessage) as Dictionary<string, object>;
				Jsonable.CheckForErrors(jsonMap);
				result = ResetInput.CreateFromDictionary(jsonMap);
			}
			catch (ApplicationException inner)
			{
				throw new AmazonException("Error encountered while UnJsoning", inner);
			}
			return result;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005F18 File Offset: 0x00004118
		public static Dictionary<string, ResetInput> MapFromJson(Dictionary<string, object> jsonMap)
		{
			Dictionary<string, ResetInput> dictionary = new Dictionary<string, ResetInput>();
			foreach (KeyValuePair<string, object> keyValuePair in jsonMap)
			{
				ResetInput value = ResetInput.CreateFromDictionary(keyValuePair.Value as Dictionary<string, object>);
				dictionary.Add(keyValuePair.Key, value);
			}
			return dictionary;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005F9C File Offset: 0x0000419C
		public static List<ResetInput> ListFromJson(List<object> array)
		{
			List<ResetInput> list = new List<ResetInput>();
			foreach (object obj in array)
			{
				list.Add(ResetInput.CreateFromDictionary(obj as Dictionary<string, object>));
			}
			return list;
		}
	}
}
