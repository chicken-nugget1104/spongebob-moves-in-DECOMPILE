using System;
using System.Collections.Generic;

namespace com.amazon.device.iap.cpt
{
	// Token: 0x02000009 RID: 9
	public abstract class Jsonable
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002D30 File Offset: 0x00000F30
		public static Dictionary<string, object> unrollObjectIntoMap<T>(Dictionary<string, T> obj) where T : Jsonable
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, T> keyValuePair in obj)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value.GetObjectDictionary());
			}
			return dictionary;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002DB0 File Offset: 0x00000FB0
		public static List<object> unrollObjectIntoList<T>(List<T> obj) where T : Jsonable
		{
			List<object> list = new List<object>();
			foreach (T t in obj)
			{
				Jsonable jsonable = t;
				list.Add(jsonable.GetObjectDictionary());
			}
			return list;
		}

		// Token: 0x06000021 RID: 33
		public abstract Dictionary<string, object> GetObjectDictionary();

		// Token: 0x06000022 RID: 34 RVA: 0x00002E24 File Offset: 0x00001024
		public static void CheckForErrors(Dictionary<string, object> jsonMap)
		{
			object obj;
			if (jsonMap.TryGetValue("error", out obj))
			{
				throw new AmazonException(obj as string);
			}
		}
	}
}
