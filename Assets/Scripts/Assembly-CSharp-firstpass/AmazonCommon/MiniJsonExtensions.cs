using System;
using System.Collections;
using System.Collections.Generic;

namespace AmazonCommon
{
	// Token: 0x0200002B RID: 43
	public static class MiniJsonExtensions
	{
		// Token: 0x06000180 RID: 384 RVA: 0x00007844 File Offset: 0x00005A44
		public static string toJson(this Hashtable obj)
		{
			return MiniJSON.jsonEncode(obj);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000784C File Offset: 0x00005A4C
		public static string toJson(this Dictionary<string, string> obj)
		{
			return MiniJSON.jsonEncode(obj);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007854 File Offset: 0x00005A54
		public static string toJson(this Dictionary<string, double> obj)
		{
			return MiniJSON.jsonEncode(obj);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000785C File Offset: 0x00005A5C
		public static ArrayList arrayListFromJson(this string json)
		{
			return MiniJSON.jsonDecode(json) as ArrayList;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000786C File Offset: 0x00005A6C
		public static Hashtable hashtableFromJson(this string json)
		{
			return MiniJSON.jsonDecode(json) as Hashtable;
		}
	}
}
