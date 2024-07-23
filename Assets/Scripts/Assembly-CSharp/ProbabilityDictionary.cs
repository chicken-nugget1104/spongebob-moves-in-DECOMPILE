using System;
using System.Collections.Generic;

// Token: 0x020001A1 RID: 417
public static class ProbabilityDictionary
{
	// Token: 0x06000DE3 RID: 3555 RVA: 0x00054818 File Offset: 0x00052A18
	public static Dictionary<int, ResultGenerator> FromJSONDict(Dictionary<string, object> srcDict)
	{
		Dictionary<int, ResultGenerator> dictionary = new Dictionary<int, ResultGenerator>();
		foreach (string text in srcDict.Keys)
		{
			int key = int.Parse(text);
			ResultGenerator value;
			if (srcDict[text] is Dictionary<string, object>)
			{
				value = new ProbabilityTable((Dictionary<string, object>)srcDict[text]);
			}
			else if (srcDict[text] is List<object>)
			{
				value = new UniformGenerator((List<object>)srcDict[text]);
			}
			else
			{
				value = new ConstantGenerator(srcDict[text].ToString());
			}
			dictionary[key] = value;
		}
		return dictionary;
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x000548F4 File Offset: 0x00052AF4
	public static Dictionary<int, int> GenerateAmounts(Dictionary<int, ResultGenerator> srcDict)
	{
		if (srcDict == null)
		{
			return null;
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (int key in srcDict.Keys)
		{
			string result = srcDict[key].GetResult();
			if (result != null)
			{
				int value = int.Parse(result);
				dictionary[key] = value;
			}
		}
		return dictionary;
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00054988 File Offset: 0x00052B88
	public static Dictionary<int, float> CalculateExpectedValues(Dictionary<int, ResultGenerator> srcDict)
	{
		if (srcDict == null)
		{
			return null;
		}
		Dictionary<int, float> dictionary = new Dictionary<int, float>();
		foreach (int key in srcDict.Keys)
		{
			string expectedValue = srcDict[key].GetExpectedValue();
			if (expectedValue != null)
			{
				float value = float.Parse(expectedValue);
				dictionary[key] = value;
			}
		}
		return dictionary;
	}
}
