using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020000B6 RID: 182
public static class MiniJsonExtensions
{
	// Token: 0x06000708 RID: 1800 RVA: 0x0001B208 File Offset: 0x00019408
	public static string toJson(this Hashtable obj)
	{
		return MiniJSON_Prime31.jsonEncode(obj);
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0001B210 File Offset: 0x00019410
	public static string toJson(this Dictionary<string, string> obj)
	{
		return MiniJSON_Prime31.jsonEncode(obj);
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0001B218 File Offset: 0x00019418
	public static ArrayList arrayListFromJson(this string json)
	{
		return MiniJSON_Prime31.jsonDecode(json) as ArrayList;
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0001B228 File Offset: 0x00019428
	public static Hashtable hashtableFromJson(this string json)
	{
		return MiniJSON_Prime31.jsonDecode(json) as Hashtable;
	}
}
