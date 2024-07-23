using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class AmazonGameCircleExampleWSSyncableNumber
{
	// Token: 0x060001ED RID: 493 RVA: 0x00009F3C File Offset: 0x0000813C
	public AmazonGameCircleExampleWSSyncableNumber(AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior newBehavior, AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType newType)
	{
		this.behavior = newBehavior;
		this.type = newType;
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00009F8C File Offset: 0x0000818C
	public void DrawGUI(AGSGameDataMap dataMap)
	{
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		this.foldoutOpen = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.foldoutOpen, this.BehaviorAndTypeAsString());
		if (this.foldoutOpen)
		{
			this.DrawSlider();
			if (GUILayout.Button(string.Format("Get {0}", this.BehaviorAndTypeAsString()), new GUILayoutOption[0]))
			{
				using (AGSSyncableNumber syncableNumber = this.GetSyncableNumber(dataMap))
				{
					if (syncableNumber != null)
					{
						this.GetSyncableValue(syncableNumber);
					}
				}
			}
			GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
			if (GUILayout.Button(string.Format("Set {0}", this.BehaviorAndTypeAsString()), new GUILayoutOption[0]))
			{
				using (AGSSyncableNumber syncableNumber2 = this.GetSyncableNumber(dataMap))
				{
					if (syncableNumber2 != null)
					{
						this.SetSyncableValue(syncableNumber2);
					}
				}
			}
			GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
			if (GUILayout.Button(string.Format("Set {0} with metadata", this.BehaviorAndTypeAsString()), new GUILayoutOption[0]))
			{
				using (AGSSyncableNumber syncableNumber3 = this.GetSyncableNumber(dataMap))
				{
					if (syncableNumber3 != null)
					{
						this.SetSyncableValueWithMetadata(syncableNumber3);
					}
				}
			}
			GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
			if (GUILayout.Button(string.Format("Get metadata", this.BehaviorAndTypeAsString()), new GUILayoutOption[0]))
			{
				using (AGSSyncableNumber syncableNumber4 = this.GetSyncableNumber(dataMap))
				{
					if (syncableNumber4 != null)
					{
						this.metadataDictionary = this.GetMetadata(syncableNumber4);
					}
				}
			}
			this.DisplayMetadata();
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0000A198 File Offset: 0x00008398
	private void DisplayMetadata()
	{
		if (this.metadataDictionary != null && this.metadataDictionary.Count > 0)
		{
			foreach (KeyValuePair<string, string> keyValuePair in this.metadataDictionary)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(keyValuePair.ToString(), new GUILayoutOption[0]);
			}
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("No metadata set.", new GUILayoutOption[0]);
		}
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x0000A23C File Offset: 0x0000843C
	private string BehaviorAndTypeAsString()
	{
		return string.Format("{0}:{1}", this.behavior.ToString(), this.type.ToString());
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000A274 File Offset: 0x00008474
	private void DrawSlider()
	{
		switch (this.type)
		{
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Int:
			this.intNumber = (int)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider((float)this.intNumber, -10000f, 10000f, "{0}");
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Long:
			this.longNumber = (long)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider((float)this.longNumber, -10000f, 10000f, "{0}");
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Double:
			this.doubleNumber = (double)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider((float)this.doubleNumber, -10000f, 10000f, "{0}");
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.String:
			if (int.TryParse(this.stringNumber, out this.intNumber))
			{
				this.intNumber = (int)AmazonGameCircleExampleGUIHelpers.DisplayCenteredSlider((float)this.intNumber, -10000f, 10000f, "{0}");
				this.stringNumber = this.intNumber.ToString();
			}
			else
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.stringNumber, new GUILayoutOption[0]);
			}
			break;
		default:
			AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
			break;
		}
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000A38C File Offset: 0x0000858C
	private AGSSyncableNumber GetSyncableNumber(AGSGameDataMap dataMap)
	{
		if (dataMap == null)
		{
			return null;
		}
		string name = this.BehaviorAndTypeAsString();
		switch (this.behavior)
		{
		case AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior.Highest:
			return dataMap.GetHighestNumber(name);
		case AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior.Lowest:
			return dataMap.GetLowestNumber(name);
		case AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior.Latest:
			return dataMap.GetLatestNumber(name);
		default:
			AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
			return null;
		}
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000A3EC File Offset: 0x000085EC
	private void GetSyncableValue(AGSSyncableNumber syncableNumber)
	{
		if (syncableNumber == null)
		{
			return;
		}
		switch (this.type)
		{
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Int:
			this.intNumber = syncableNumber.AsInt();
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Long:
			this.longNumber = syncableNumber.AsLong();
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Double:
			this.doubleNumber = syncableNumber.AsDouble();
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.String:
			this.stringNumber = syncableNumber.AsString();
			break;
		default:
			AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
			break;
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000A478 File Offset: 0x00008678
	private void SetSyncableValue(AGSSyncableNumber syncableNumber)
	{
		if (syncableNumber == null)
		{
			return;
		}
		switch (this.type)
		{
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Int:
			syncableNumber.Set(this.intNumber);
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Long:
			syncableNumber.Set(this.longNumber);
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Double:
			syncableNumber.Set(this.doubleNumber);
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.String:
			syncableNumber.Set(this.stringNumber);
			break;
		default:
			AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
			break;
		}
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000A504 File Offset: 0x00008704
	private void SetSyncableValueWithMetadata(AGSSyncableNumber syncableNumber)
	{
		if (syncableNumber == null)
		{
			return;
		}
		switch (this.type)
		{
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Int:
			syncableNumber.Set(this.intNumber, this.defaultMetadataDictionary);
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Long:
			syncableNumber.Set(this.longNumber, this.defaultMetadataDictionary);
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.Double:
			syncableNumber.Set(this.doubleNumber, this.defaultMetadataDictionary);
			break;
		case AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType.String:
			syncableNumber.Set(this.stringNumber, this.defaultMetadataDictionary);
			break;
		default:
			AGSClient.LogGameCircleWarning("Whispersync unhandled syncable number type");
			break;
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000A5A8 File Offset: 0x000087A8
	private Dictionary<string, string> GetMetadata(AGSSyncableNumber syncableNumber)
	{
		if (syncableNumber == null)
		{
			return null;
		}
		return syncableNumber.GetMetadata();
	}

	// Token: 0x04000135 RID: 309
	private const string behaviorAndTypeLabel = "{0}:{1}";

	// Token: 0x04000136 RID: 310
	private const string getValueLabel = "Get {0}";

	// Token: 0x04000137 RID: 311
	private const string setValueLabel = "Set {0}";

	// Token: 0x04000138 RID: 312
	private const string setWithMetadataValueLabel = "Set {0} with metadata";

	// Token: 0x04000139 RID: 313
	private const string numberSliderLabel = "{0}";

	// Token: 0x0400013A RID: 314
	private const string unhandledSyncableNumberTypeError = "Whispersync unhandled syncable number type";

	// Token: 0x0400013B RID: 315
	private const string metadataKey = "key";

	// Token: 0x0400013C RID: 316
	private const string metadataValue = "value";

	// Token: 0x0400013D RID: 317
	private const string getMetadataButtonLabel = "Get metadata";

	// Token: 0x0400013E RID: 318
	private const string noMetaDataAvailableLabel = "No metadata set.";

	// Token: 0x0400013F RID: 319
	private const float lowestSliderValue = -10000f;

	// Token: 0x04000140 RID: 320
	private const float highestSlidervalue = 10000f;

	// Token: 0x04000141 RID: 321
	private readonly AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior behavior;

	// Token: 0x04000142 RID: 322
	private readonly AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType type;

	// Token: 0x04000143 RID: 323
	private bool foldoutOpen;

	// Token: 0x04000144 RID: 324
	private int intNumber;

	// Token: 0x04000145 RID: 325
	private long longNumber;

	// Token: 0x04000146 RID: 326
	private double doubleNumber;

	// Token: 0x04000147 RID: 327
	private string stringNumber = 0.ToString();

	// Token: 0x04000148 RID: 328
	private Dictionary<string, string> metadataDictionary;

	// Token: 0x04000149 RID: 329
	private readonly Dictionary<string, string> defaultMetadataDictionary = new Dictionary<string, string>
	{
		{
			"key",
			"value"
		}
	};

	// Token: 0x0200003C RID: 60
	public enum SyncableNumberBehavior
	{
		// Token: 0x0400014B RID: 331
		Highest,
		// Token: 0x0400014C RID: 332
		Lowest,
		// Token: 0x0400014D RID: 333
		Latest
	}

	// Token: 0x0200003D RID: 61
	public enum AvailableSyncableNumberType
	{
		// Token: 0x0400014F RID: 335
		Int,
		// Token: 0x04000150 RID: 336
		Long,
		// Token: 0x04000151 RID: 337
		Double,
		// Token: 0x04000152 RID: 338
		String
	}
}
