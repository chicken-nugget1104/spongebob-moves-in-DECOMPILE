using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019F RID: 415
public static class AmountDictionary
{
	// Token: 0x06000DD0 RID: 3536 RVA: 0x000541C8 File Offset: 0x000523C8
	public static Dictionary<string, object> ToJSONDict(Dictionary<int, int> srcDict)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (KeyValuePair<int, int> keyValuePair in srcDict)
		{
			dictionary[keyValuePair.Key.ToString()] = keyValuePair.Value;
		}
		return dictionary;
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x0005424C File Offset: 0x0005244C
	public static Dictionary<int, int> FromJSONDict(Dictionary<string, object> srcDict)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (string text in srcDict.Keys)
		{
			int key = int.Parse(text);
			int value = TFUtils.LoadInt(srcDict, text);
			dictionary[key] = value;
		}
		return dictionary;
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x000542CC File Offset: 0x000524CC
	public static Dictionary<string, object> ToJSONDict(Dictionary<int, Vector2> srcDict)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (KeyValuePair<int, Vector2> keyValuePair in srcDict)
		{
			dictionary[keyValuePair.Key.ToString()] = keyValuePair.Value;
		}
		return dictionary;
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x00054350 File Offset: 0x00052550
	public static Dictionary<int, Vector2> FromJSONDictVector2(Dictionary<string, object> srcDict)
	{
		Dictionary<int, Vector2> dictionary = new Dictionary<int, Vector2>();
		foreach (string text in srcDict.Keys)
		{
			int key = int.Parse(text);
			Vector2 value;
			TFUtils.LoadVector2(out value, (Dictionary<string, object>)srcDict[text]);
			dictionary[key] = value;
		}
		return dictionary;
	}
}
