using System;
using System.Collections;
using System.Collections.Generic;

namespace DeltaDNA
{
	// Token: 0x0200000D RID: 13
	internal static class Utils
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00004664 File Offset: 0x00002864
		public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			foreach (object obj in table)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				dictionary.Add((K)((object)dictionaryEntry.Key), (V)((object)dictionaryEntry.Value));
			}
			return dictionary;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000046EC File Offset: 0x000028EC
		public static Dictionary<K, V> HashtableToDictionary<K, V>(Dictionary<K, V> dictionary)
		{
			return dictionary;
		}
	}
}
