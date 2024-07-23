using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class AmazonGameCircleExampleWSNumberList
{
	// Token: 0x060001E3 RID: 483 RVA: 0x0000995C File Offset: 0x00007B5C
	public AmazonGameCircleExampleWSNumberList(AmazonGameCircleExampleWSNumberList.AvailableListType availableListType)
	{
		this.listType = availableListType;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00009994 File Offset: 0x00007B94
	public void DrawGUI(AGSGameDataMap dataMap)
	{
		if (dataMap == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("Syncable number list not yet initialized", new GUILayoutOption[0]);
			return;
		}
		if (this.syncableNumberList == null)
		{
			this.InitSyncableNumberList(dataMap);
		}
		if (this.syncableNumberList == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("Syncable number list not yet initialized", new GUILayoutOption[0]);
			return;
		}
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		this.foldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.foldout, this.ListName());
		if (this.foldout)
		{
			if (GUILayout.Button("Refresh List", new GUILayoutOption[0]))
			{
				this.RefreshList();
			}
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("Has list been set yet? {0}", this.isSet), new GUILayoutOption[0]);
			if (this.maxSize != null)
			{
				this.maxSize = new int?((int)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider((float)this.maxSize.Value, 3f, 8f, "Max Size {0}"));
				if (GUILayout.Button("Update Max Size", new GUILayoutOption[0]))
				{
					this.syncableNumberList.SetMaxSize(this.maxSize.Value);
				}
			}
			if (this.syncableNumberElementsCache == null || this.syncableNumberElementsCache.Length == 0)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel("List is empty", new GUILayoutOption[0]);
			}
			else
			{
				foreach (AmazonGameCircleExampleWSNumberListElementCache amazonGameCircleExampleWSNumberListElementCache in this.syncableNumberElementsCache)
				{
					amazonGameCircleExampleWSNumberListElementCache.DrawElement();
				}
			}
			if (GUILayout.Button("Add values", new GUILayoutOption[0]))
			{
				this.AddValuesToList();
				this.IncrementValues();
				this.AddValuesToListWithMetadata();
				this.IncrementValues();
				this.RefreshList();
			}
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x00009B4C File Offset: 0x00007D4C
	private void InitSyncableNumberList(AGSGameDataMap dataMap)
	{
		switch (this.listType)
		{
		case AmazonGameCircleExampleWSNumberList.AvailableListType.HighNumber:
			this.syncableNumberList = dataMap.GetHighNumberList(this.ListName());
			break;
		case AmazonGameCircleExampleWSNumberList.AvailableListType.LowNumber:
			this.syncableNumberList = dataMap.GetLowNumberList(this.ListName());
			break;
		case AmazonGameCircleExampleWSNumberList.AvailableListType.LatestNumber:
			this.syncableNumberList = dataMap.GetLatestNumberList(this.ListName());
			break;
		}
		this.maxSize = new int?(this.syncableNumberList.GetMaxSize());
		this.isSet = this.syncableNumberList.IsSet();
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00009BE4 File Offset: 0x00007DE4
	private void RefreshList()
	{
		if (this.syncableNumberList == null)
		{
			return;
		}
		this.maxSize = new int?(this.syncableNumberList.GetMaxSize());
		this.isSet = this.syncableNumberList.IsSet();
		this.syncableNumberElements = this.syncableNumberList.GetValues();
		this.syncableNumberElementsCache = new AmazonGameCircleExampleWSNumberListElementCache[this.syncableNumberElements.Length];
		for (int i = 0; i < this.syncableNumberElements.Length; i++)
		{
			this.syncableNumberElementsCache[i] = new AmazonGameCircleExampleWSNumberListElementCache(this.syncableNumberElements[i].AsInt(), this.syncableNumberElements[i].AsLong(), this.syncableNumberElements[i].AsDouble(), this.syncableNumberElements[i].AsString(), this.syncableNumberElements[i].GetMetadata());
		}
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x00009CB0 File Offset: 0x00007EB0
	private void AddValuesToList()
	{
		this.syncableNumberList.Add(this.intVal);
		this.syncableNumberList.Add(this.longVal);
		this.syncableNumberList.Add(this.doubleVal);
		this.syncableNumberList.Add((this.intVal * 2).ToString());
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x00009D0C File Offset: 0x00007F0C
	private void AddValuesToListWithMetadata()
	{
		this.syncableNumberList.Add(this.intVal, this.defaultMetadataDictionary);
		this.syncableNumberList.Add(this.longVal, this.defaultMetadataDictionary);
		this.syncableNumberList.Add(this.doubleVal, this.defaultMetadataDictionary);
		this.syncableNumberList.Add((this.intVal * 2).ToString(), this.defaultMetadataDictionary);
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00009D80 File Offset: 0x00007F80
	private void IncrementValues()
	{
		this.intVal++;
		this.longVal += -5L;
		this.doubleVal += 0.1;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00009DC4 File Offset: 0x00007FC4
	private string ListName()
	{
		return this.listType.ToString();
	}

	// Token: 0x0400010F RID: 271
	private const string notInitializedLabel = "Syncable number list not yet initialized";

	// Token: 0x04000110 RID: 272
	private const string refreshSyncableNumberElementsButtonLabel = "Refresh List";

	// Token: 0x04000111 RID: 273
	private const string emptyListLabel = "List is empty";

	// Token: 0x04000112 RID: 274
	private const string addValuesButtonLabel = "Add values";

	// Token: 0x04000113 RID: 275
	private const string metadataKey = "key";

	// Token: 0x04000114 RID: 276
	private const string metadataValue = "value";

	// Token: 0x04000115 RID: 277
	private const string maxSizeLabel = "Max Size {0}";

	// Token: 0x04000116 RID: 278
	private const string updateMaxSizeButtonLabel = "Update Max Size";

	// Token: 0x04000117 RID: 279
	private const string isListSetLabel = "Has list been set yet? {0}";

	// Token: 0x04000118 RID: 280
	private const int intIncrement = 1;

	// Token: 0x04000119 RID: 281
	private const long longIncrement = -5L;

	// Token: 0x0400011A RID: 282
	private const double doubleIncrement = 0.1;

	// Token: 0x0400011B RID: 283
	private const int stringMultiplier = 2;

	// Token: 0x0400011C RID: 284
	private const int minMaxSize = 3;

	// Token: 0x0400011D RID: 285
	private const int maxMaxSize = 8;

	// Token: 0x0400011E RID: 286
	private readonly AmazonGameCircleExampleWSNumberList.AvailableListType listType;

	// Token: 0x0400011F RID: 287
	private AGSSyncableNumberList syncableNumberList;

	// Token: 0x04000120 RID: 288
	private AGSSyncableNumberElement[] syncableNumberElements;

	// Token: 0x04000121 RID: 289
	private AmazonGameCircleExampleWSNumberListElementCache[] syncableNumberElementsCache;

	// Token: 0x04000122 RID: 290
	private bool isSet;

	// Token: 0x04000123 RID: 291
	private int intVal;

	// Token: 0x04000124 RID: 292
	private long longVal;

	// Token: 0x04000125 RID: 293
	private double doubleVal;

	// Token: 0x04000126 RID: 294
	private int? maxSize;

	// Token: 0x04000127 RID: 295
	private bool foldout;

	// Token: 0x04000128 RID: 296
	private readonly Dictionary<string, string> defaultMetadataDictionary = new Dictionary<string, string>
	{
		{
			"key",
			"value"
		}
	};

	// Token: 0x02000039 RID: 57
	public enum AvailableListType
	{
		// Token: 0x0400012A RID: 298
		HighNumber,
		// Token: 0x0400012B RID: 299
		LowNumber,
		// Token: 0x0400012C RID: 300
		LatestNumber
	}
}
