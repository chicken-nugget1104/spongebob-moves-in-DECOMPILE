using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class AmazonGameCircleExampleWSNumberListElementCache
{
	// Token: 0x060001EB RID: 491 RVA: 0x00009DD8 File Offset: 0x00007FD8
	public AmazonGameCircleExampleWSNumberListElementCache(int intVal, long longVal, double doubleVal, string stringVal, Dictionary<string, string> elementMetadata)
	{
		this.valueAsInt = intVal;
		this.valueAsLong = longVal;
		this.valueAsDouble = doubleVal;
		this.valueAsString = stringVal;
		this.metadata = elementMetadata;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00009E20 File Offset: 0x00008020
	public void DrawElement()
	{
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		string text = string.Format("Int {0} : Long {1} : Double {2,5:N1} : String {3}", new object[]
		{
			this.valueAsInt,
			this.valueAsLong,
			this.valueAsDouble,
			this.valueAsString
		});
		AmazonGameCircleExampleGUIHelpers.CenteredLabel(text, new GUILayoutOption[0]);
		if (this.metadata != null && this.metadata.Count > 0)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("Metadata", new GUILayoutOption[0]);
			foreach (KeyValuePair<string, string> keyValuePair in this.metadata)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel(keyValuePair.ToString(), new GUILayoutOption[0]);
			}
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("No metadata", new GUILayoutOption[0]);
		}
		GUILayout.EndVertical();
	}

	// Token: 0x0400012D RID: 301
	private const string listElementLabel = "Int {0} : Long {1} : Double {2,5:N1} : String {3}";

	// Token: 0x0400012E RID: 302
	private const string metadataLabel = "Metadata";

	// Token: 0x0400012F RID: 303
	private const string noMetadataAvailableLabel = "No metadata";

	// Token: 0x04000130 RID: 304
	private int valueAsInt;

	// Token: 0x04000131 RID: 305
	private long valueAsLong;

	// Token: 0x04000132 RID: 306
	private double valueAsDouble;

	// Token: 0x04000133 RID: 307
	private string valueAsString = 0.ToString();

	// Token: 0x04000134 RID: 308
	private Dictionary<string, string> metadata;
}
