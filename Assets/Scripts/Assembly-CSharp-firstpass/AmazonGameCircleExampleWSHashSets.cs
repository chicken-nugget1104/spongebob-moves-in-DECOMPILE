using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000037 RID: 55
public class AmazonGameCircleExampleWSHashSets
{
	// Token: 0x060001E0 RID: 480 RVA: 0x000096B4 File Offset: 0x000078B4
	public void DrawGUI(AGSGameDataMap dataMap)
	{
		if (dataMap == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("Missing Whispersync DataMap", new GUILayoutOption[0]);
			return;
		}
		if (this.hashSets == null)
		{
			this.InitializeHashSets(dataMap);
		}
		if (this.hashSets == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("HashSets not yet initialized", new GUILayoutOption[0]);
			return;
		}
		if (GUILayout.Button("Refresh All", new GUILayoutOption[0]))
		{
			this.RefreshAllHashSets();
		}
		foreach (AmazonGameCircleExampleWSHashSet amazonGameCircleExampleWSHashSet in this.hashSets)
		{
			amazonGameCircleExampleWSHashSet.DrawGUI();
		}
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000977C File Offset: 0x0000797C
	private void InitializeHashSets(AGSGameDataMap dataMap)
	{
		if (dataMap == null)
		{
			return;
		}
		this.hashSets = new List<AmazonGameCircleExampleWSHashSet>();
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetHighestNumberKeys", new Func<HashSet<string>>(dataMap.GetHighestNumberKeys)));
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLowestNumberKeys", new Func<HashSet<string>>(dataMap.GetLowestNumberKeys)));
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLatestNumberKeys", new Func<HashSet<string>>(dataMap.GetLatestNumberKeys)));
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetHighNumberListKeys", new Func<HashSet<string>>(dataMap.GetHighNumberListKeys)));
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLowNumberListKeys", new Func<HashSet<string>>(dataMap.GetLowNumberListKeys)));
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLatestNumberListKeys", new Func<HashSet<string>>(dataMap.GetLatestNumberListKeys)));
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLatestStringKeys", new Func<HashSet<string>>(dataMap.GetLatestStringKeys)));
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetLatestStringListKeys", new Func<HashSet<string>>(dataMap.GetLatestStringListKeys)));
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetStringSetKeys", new Func<HashSet<string>>(dataMap.GetStringSetKeys)));
		this.hashSets.Add(new AmazonGameCircleExampleWSHashSet("GetMapKeys", new Func<HashSet<string>>(dataMap.GetMapKeys)));
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x000098E8 File Offset: 0x00007AE8
	private void RefreshAllHashSets()
	{
		if (this.hashSets == null)
		{
			return;
		}
		foreach (AmazonGameCircleExampleWSHashSet amazonGameCircleExampleWSHashSet in this.hashSets)
		{
			amazonGameCircleExampleWSHashSet.Refresh();
		}
	}

	// Token: 0x0400010B RID: 267
	private const string nullDataMapLabel = "Missing Whispersync DataMap";

	// Token: 0x0400010C RID: 268
	private const string nullHashSets = "HashSets not yet initialized";

	// Token: 0x0400010D RID: 269
	private const string refreshAllHashSetsButtonLabel = "Refresh All";

	// Token: 0x0400010E RID: 270
	private List<AmazonGameCircleExampleWSHashSet> hashSets;
}
