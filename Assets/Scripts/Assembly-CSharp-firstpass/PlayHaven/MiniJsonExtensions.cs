using System;
using System.Collections;
using System.Collections.Generic;

namespace PlayHaven
{
	// Token: 0x020000C2 RID: 194
	public static class MiniJsonExtensions
	{
		// Token: 0x060007B4 RID: 1972 RVA: 0x0001D4F8 File Offset: 0x0001B6F8
		public static string toJson(this Hashtable obj)
		{
			return MiniJSON.jsonEncode(obj);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001D500 File Offset: 0x0001B700
		public static string toJson(this Dictionary<string, string> obj)
		{
			return MiniJSON.jsonEncode(obj);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001D508 File Offset: 0x0001B708
		public static ArrayList arrayListFromJson(this string json)
		{
			return MiniJSON.jsonDecode(json) as ArrayList;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001D518 File Offset: 0x0001B718
		public static Hashtable hashtableFromJson(this string json)
		{
			return MiniJSON.jsonDecode(json) as Hashtable;
		}
	}
}
