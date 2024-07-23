using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class AmazonGameCircleExampleWSAccumulatingNumber
{
	// Token: 0x060001D2 RID: 466 RVA: 0x000091D4 File Offset: 0x000073D4
	public AmazonGameCircleExampleWSAccumulatingNumber(AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType newType)
	{
		this.type = newType;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x000091E4 File Offset: 0x000073E4
	public void DrawGUI(AGSGameDataMap dataMap)
	{
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		this.foldoutOpen = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.foldoutOpen, this.SyncableVariableName());
		if (this.foldoutOpen)
		{
			if (!this.ValueAvailable())
			{
				this.RetrieveAccumulatingNumberValue(dataMap);
			}
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(this.ValueLabel(), new GUILayoutOption[0]);
			if (GUILayout.Button("Increment", new GUILayoutOption[0]))
			{
				this.IncrementValue(dataMap);
			}
			if (GUILayout.Button("Decrement", new GUILayoutOption[0]))
			{
				this.DecrementValue(dataMap);
			}
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00009288 File Offset: 0x00007488
	private void RetrieveAccumulatingNumberValue(AGSGameDataMap dataMap)
	{
		AGSSyncableAccumulatingNumber accumulatingNumber = dataMap.GetAccumulatingNumber(this.SyncableVariableName());
		switch (this.type)
		{
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Int:
			this.valueAsInt = new int?(accumulatingNumber.AsInt());
			break;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Long:
			this.valueAsLong = new long?(accumulatingNumber.AsLong());
			break;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Double:
			this.valueAsDouble = new double?(accumulatingNumber.AsDouble());
			break;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.String:
			this.valueAsString = accumulatingNumber.AsString();
			break;
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x00009318 File Offset: 0x00007518
	private void IncrementValue(AGSGameDataMap dataMap)
	{
		AGSSyncableAccumulatingNumber accumulatingNumber = dataMap.GetAccumulatingNumber(this.SyncableVariableName());
		switch (this.type)
		{
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Int:
			accumulatingNumber.Increment(1);
			break;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Long:
			accumulatingNumber.Increment(1L);
			break;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Double:
			accumulatingNumber.Increment(0.10000000149011612);
			break;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.String:
			accumulatingNumber.Increment("1");
			break;
		}
		this.RetrieveAccumulatingNumberValue(dataMap);
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00009398 File Offset: 0x00007598
	private void DecrementValue(AGSGameDataMap dataMap)
	{
		AGSSyncableAccumulatingNumber accumulatingNumber = dataMap.GetAccumulatingNumber(this.SyncableVariableName());
		switch (this.type)
		{
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Int:
			accumulatingNumber.Decrement(1);
			break;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Long:
			accumulatingNumber.Decrement(1L);
			break;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Double:
			accumulatingNumber.Decrement(0.10000000149011612);
			break;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.String:
			accumulatingNumber.Decrement("1");
			break;
		}
		this.RetrieveAccumulatingNumberValue(dataMap);
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00009418 File Offset: 0x00007618
	private string ValueLabel()
	{
		if (!this.ValueAvailable())
		{
			return "No value available.";
		}
		switch (this.type)
		{
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Int:
			return string.Format("Accumulating number value: {0}", this.valueAsInt.Value);
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Long:
			return string.Format("Accumulating number value: {0}", this.valueAsLong.Value);
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Double:
			return string.Format("Accumulating number value: {0,5:N1}", this.valueAsDouble.Value);
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.String:
			return string.Format("Accumulating number value: {0}", this.valueAsString);
		default:
			return "No value available.";
		}
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x000094C0 File Offset: 0x000076C0
	private bool ValueAvailable()
	{
		switch (this.type)
		{
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Int:
			return this.valueAsInt != null;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Long:
			return this.valueAsLong != null;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.Double:
			return this.valueAsDouble != null;
		case AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType.String:
			return !string.IsNullOrEmpty(this.valueAsString);
		default:
			return false;
		}
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00009524 File Offset: 0x00007724
	private string SyncableVariableName()
	{
		return this.type.ToString();
	}

	// Token: 0x040000F0 RID: 240
	private const string incrementButtonLabel = "Increment";

	// Token: 0x040000F1 RID: 241
	private const string decrementButtonLabel = "Decrement";

	// Token: 0x040000F2 RID: 242
	private const string accumulatingNumberValueLabel = "Accumulating number value: {0}";

	// Token: 0x040000F3 RID: 243
	private const string accumulatingNumberDoubleValueLabel = "Accumulating number value: {0,5:N1}";

	// Token: 0x040000F4 RID: 244
	private const string noAccumulatingNumberLabel = "No value available.";

	// Token: 0x040000F5 RID: 245
	private const string unableToParseValueAsStringError = "Unable to parse accumulating number.";

	// Token: 0x040000F6 RID: 246
	private const double doubleIncrementValue = 0.10000000149011612;

	// Token: 0x040000F7 RID: 247
	private const int intIncrementValue = 1;

	// Token: 0x040000F8 RID: 248
	private const long longIncrementValue = 1L;

	// Token: 0x040000F9 RID: 249
	private const string stringIncrementValue = "1";

	// Token: 0x040000FA RID: 250
	private readonly AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType type;

	// Token: 0x040000FB RID: 251
	private bool foldoutOpen;

	// Token: 0x040000FC RID: 252
	private double? valueAsDouble;

	// Token: 0x040000FD RID: 253
	private long? valueAsLong;

	// Token: 0x040000FE RID: 254
	private int? valueAsInt;

	// Token: 0x040000FF RID: 255
	private string valueAsString;

	// Token: 0x02000035 RID: 53
	public enum AvailableAccumulatingNumberType
	{
		// Token: 0x04000101 RID: 257
		Int,
		// Token: 0x04000102 RID: 258
		Long,
		// Token: 0x04000103 RID: 259
		Double,
		// Token: 0x04000104 RID: 260
		String
	}
}
