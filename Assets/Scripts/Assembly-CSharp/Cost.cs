using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class Cost
{
	// Token: 0x06000B71 RID: 2929 RVA: 0x000454B4 File Offset: 0x000436B4
	public Cost()
	{
		this.resourceAmounts = new Dictionary<int, int>();
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x000454C8 File Offset: 0x000436C8
	public Cost(Dictionary<int, int> resourceAmounts)
	{
		this.resourceAmounts = resourceAmounts;
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x000454D8 File Offset: 0x000436D8
	public Cost(Cost other)
	{
		Dictionary<int, int> dictionary = TFUtils.CloneDictionary<int, int>(other.ResourceAmounts);
		this.resourceAmounts = dictionary;
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000B74 RID: 2932 RVA: 0x00045500 File Offset: 0x00043700
	public Dictionary<int, int> ResourceAmounts
	{
		get
		{
			return this.resourceAmounts;
		}
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x00045508 File Offset: 0x00043708
	public static Cost FromDict(Dictionary<string, object> dict)
	{
		if (dict == null)
		{
			return null;
		}
		Dictionary<int, int> dictionary = AmountDictionary.FromJSONDict(dict);
		return new Cost(dictionary);
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x0004552C File Offset: 0x0004372C
	public static Cost FromObject(object o)
	{
		return (o != null) ? Cost.FromDict((Dictionary<string, object>)o) : null;
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x00045548 File Offset: 0x00043748
	public int GetOnlyCostKey()
	{
		TFUtils.Assert(this.ResourceAmounts.Count == 1, "Cost expect to have only one entry, it has " + this.ResourceAmounts.Count);
		int result = -1;
		using (Dictionary<int, int>.KeyCollection.Enumerator enumerator = this.ResourceAmounts.Keys.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				int num = enumerator.Current;
				result = num;
			}
		}
		return result;
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x000455E4 File Offset: 0x000437E4
	public Dictionary<string, object> ToDict()
	{
		return AmountDictionary.ToJSONDict(this.resourceAmounts);
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x000455F4 File Offset: 0x000437F4
	public static Dictionary<string, int> DisplayDictionary(Dictionary<int, int> costDict, ResourceManager resMgr)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (KeyValuePair<int, int> keyValuePair in costDict)
		{
			string resourceTexture = resMgr.Resources[keyValuePair.Key].GetResourceTexture();
			dictionary[resourceTexture] = keyValuePair.Value;
		}
		return dictionary;
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0004567C File Offset: 0x0004387C
	public static Dictionary<string, int> GetResourcesStillRequired(Dictionary<int, int> costDict, ResourceManager resourceManager)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (KeyValuePair<int, int> keyValuePair in costDict)
		{
			int num = resourceManager.Query(keyValuePair.Key);
			int num2 = keyValuePair.Value - num;
			if (num2 > 0)
			{
				string resourceTexture = resourceManager.Resources[keyValuePair.Key].GetResourceTexture();
				dictionary[resourceTexture] = num2;
			}
		}
		return dictionary;
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x00045724 File Offset: 0x00043924
	public static Cost GetResourcesToPurchase(Dictionary<int, int> costDict, ResourceManager resourceManager)
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (KeyValuePair<int, int> keyValuePair in costDict)
		{
			int num = resourceManager.Query(keyValuePair.Key);
			int num2 = keyValuePair.Value - num;
			if (num2 > 0)
			{
				dictionary[keyValuePair.Key] = num2;
			}
		}
		return new Cost(dictionary);
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x000457BC File Offset: 0x000439BC
	public void Prorate(float percentLeft)
	{
		foreach (int key in this.resourceAmounts.Keys.ToArray<int>())
		{
			this.resourceAmounts[key] = Resource.Prorate(this.resourceAmounts[key], percentLeft);
		}
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x00045810 File Offset: 0x00043A10
	public void Prorate(ulong endTime, ulong totalTime)
	{
		this.Prorate(Mathf.Max(0f, endTime - TFUtils.EpochTime()) / totalTime);
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x00045830 File Offset: 0x00043A30
	public static Cost Prorate(Cost full, float percentLeft)
	{
		Cost cost = new Cost(full);
		cost.Prorate(percentLeft);
		return cost;
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x0004584C File Offset: 0x00043A4C
	public static Cost Prorate(Cost full, ulong endTime, ulong totalTime)
	{
		Cost cost = new Cost(full);
		cost.Prorate(endTime, totalTime);
		return cost;
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x0004586C File Offset: 0x00043A6C
	public static Cost Prorate(Cost full, ulong startTime, ulong endTime, ulong currentTime)
	{
		Cost cost = new Cost(full);
		float num = endTime - startTime;
		float num2 = endTime - currentTime;
		cost.Prorate(Mathf.Max(0f, num2 / num));
		return cost;
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x000458A0 File Offset: 0x00043AA0
	public static Cost operator +(Cost c1, Cost c2)
	{
		Cost cost = new Cost(c1);
		foreach (int key in c2.resourceAmounts.Keys)
		{
			int num;
			if (cost.resourceAmounts.TryGetValue(key, out num))
			{
				cost.resourceAmounts[key] = num + c2.resourceAmounts[key];
			}
			else
			{
				cost.resourceAmounts[key] = c2.resourceAmounts[key];
			}
		}
		return cost;
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x00045958 File Offset: 0x00043B58
	public static Cost operator -(Cost c1, Cost c2)
	{
		Cost cost = new Cost(c1);
		foreach (int key in c2.resourceAmounts.Keys)
		{
			int num;
			if (cost.resourceAmounts.TryGetValue(key, out num))
			{
				cost.resourceAmounts[key] = num - c2.resourceAmounts[key];
			}
			else
			{
				cost.resourceAmounts[key] = -c2.resourceAmounts[key];
			}
		}
		return cost;
	}

	// Token: 0x040007A4 RID: 1956
	private Dictionary<int, int> resourceAmounts;

	// Token: 0x020004A1 RID: 1185
	// (Invoke) Token: 0x060024E7 RID: 9447
	public delegate Cost CostAtTime(ulong time);
}
