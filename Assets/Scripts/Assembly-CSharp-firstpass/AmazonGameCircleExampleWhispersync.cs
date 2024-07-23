using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class AmazonGameCircleExampleWhispersync : AmazonGameCircleExampleBase
{
	// Token: 0x060001F7 RID: 503 RVA: 0x0000A5B8 File Offset: 0x000087B8
	public AmazonGameCircleExampleWhispersync()
	{
		this.InitSyncableNumbers();
		this.InitSyncableNumberLists();
		this.InitAccumulatingNumbers();
		this.InitHashSets();
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000A5E4 File Offset: 0x000087E4
	public override string MenuTitle()
	{
		return "Whispersync";
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000A5EC File Offset: 0x000087EC
	public override void DrawMenu()
	{
		if (this.lastCloudDataAvailable != null)
		{
			double totalSeconds = (DateTime.Now - this.lastCloudDataAvailable.Value).TotalSeconds;
			AmazonGameCircleExampleGUIHelpers.CenteredLabel(string.Format("Received cloud data {0,5:N1} second ago.", totalSeconds), new GUILayoutOption[0]);
		}
		else
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("No cloud data received.", new GUILayoutOption[0]);
		}
		if (GUILayout.Button("Synchronize", new GUILayoutOption[0]))
		{
			AGSWhispersyncClient.Synchronize();
		}
		GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
		if (GUILayout.Button("Flush", new GUILayoutOption[0]))
		{
			AGSWhispersyncClient.Flush();
		}
		GUILayout.Label(GUIContent.none, new GUILayoutOption[0]);
		this.InitializeDataMapIfAvailable();
		if (this.dataMap == null)
		{
			AmazonGameCircleExampleGUIHelpers.CenteredLabel("No Whispersync data available.", new GUILayoutOption[0]);
			return;
		}
		this.DrawSyncableNumbers();
		this.DrawAccumulatingNumbers();
		this.DrawSyncableNumberLists();
		this.DrawHashSets();
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000A6E8 File Offset: 0x000088E8
	private void DrawSyncableNumbers()
	{
		if (this.syncableNumbers == null)
		{
			return;
		}
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		this.syncableNumbersFoldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.syncableNumbersFoldout, "Syncable Numbers");
		if (this.syncableNumbersFoldout)
		{
			foreach (AmazonGameCircleExampleWSSyncableNumber amazonGameCircleExampleWSSyncableNumber in this.syncableNumbers)
			{
				amazonGameCircleExampleWSSyncableNumber.DrawGUI(this.dataMap);
			}
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000A79C File Offset: 0x0000899C
	private void DrawAccumulatingNumbers()
	{
		if (this.accumulatingNumbers == null)
		{
			return;
		}
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		this.accumulatingNumbersFoldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.accumulatingNumbersFoldout, "Accumulating Numbers");
		if (this.accumulatingNumbersFoldout)
		{
			foreach (AmazonGameCircleExampleWSAccumulatingNumber amazonGameCircleExampleWSAccumulatingNumber in this.accumulatingNumbers)
			{
				amazonGameCircleExampleWSAccumulatingNumber.DrawGUI(this.dataMap);
			}
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000A850 File Offset: 0x00008A50
	private void DrawSyncableNumberLists()
	{
		if (this.syncableNumberLists == null)
		{
			return;
		}
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		this.syncableNumberListsFoldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.syncableNumberListsFoldout, "Syncable Number Lists");
		if (this.syncableNumberListsFoldout)
		{
			foreach (AmazonGameCircleExampleWSNumberList amazonGameCircleExampleWSNumberList in this.syncableNumberLists)
			{
				amazonGameCircleExampleWSNumberList.DrawGUI(this.dataMap);
			}
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000A904 File Offset: 0x00008B04
	private void DrawHashSets()
	{
		if (this.hashSets == null)
		{
			return;
		}
		GUILayout.BeginVertical(GUI.skin.box, new GUILayoutOption[0]);
		this.hashSetsFoldout = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(this.hashSetsFoldout, "Hash Sets");
		if (this.hashSetsFoldout)
		{
			this.hashSets.DrawGUI(this.dataMap);
		}
		GUILayout.EndVertical();
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000A96C File Offset: 0x00008B6C
	private void InitializeDataMapIfAvailable()
	{
		if (this.dataMap != null)
		{
			return;
		}
		this.dataMap = AGSWhispersyncClient.GetGameData();
		if (this.dataMap != null)
		{
			AGSWhispersyncClient.OnNewCloudDataEvent += this.OnNewCloudData;
		}
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000A9A4 File Offset: 0x00008BA4
	private void InitSyncableNumbers()
	{
		if (this.syncableNumbers != null)
		{
			return;
		}
		this.syncableNumbers = new List<AmazonGameCircleExampleWSSyncableNumber>();
		Array values = Enum.GetValues(typeof(AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior));
		Array values2 = Enum.GetValues(typeof(AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType));
		foreach (object obj in values)
		{
			AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior newBehavior = (AmazonGameCircleExampleWSSyncableNumber.SyncableNumberBehavior)((int)obj);
			foreach (object obj2 in values2)
			{
				AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType newType = (AmazonGameCircleExampleWSSyncableNumber.AvailableSyncableNumberType)((int)obj2);
				this.syncableNumbers.Add(new AmazonGameCircleExampleWSSyncableNumber(newBehavior, newType));
			}
		}
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000AAB0 File Offset: 0x00008CB0
	private void InitSyncableNumberLists()
	{
		if (this.syncableNumberLists != null)
		{
			return;
		}
		this.syncableNumberLists = new List<AmazonGameCircleExampleWSNumberList>();
		Array values = Enum.GetValues(typeof(AmazonGameCircleExampleWSNumberList.AvailableListType));
		foreach (object obj in values)
		{
			AmazonGameCircleExampleWSNumberList.AvailableListType availableListType = (AmazonGameCircleExampleWSNumberList.AvailableListType)((int)obj);
			this.syncableNumberLists.Add(new AmazonGameCircleExampleWSNumberList(availableListType));
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000AB4C File Offset: 0x00008D4C
	private void InitAccumulatingNumbers()
	{
		if (this.accumulatingNumbers != null)
		{
			return;
		}
		this.accumulatingNumbers = new List<AmazonGameCircleExampleWSAccumulatingNumber>();
		Array values = Enum.GetValues(typeof(AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType));
		foreach (object obj in values)
		{
			AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType newType = (AmazonGameCircleExampleWSAccumulatingNumber.AvailableAccumulatingNumberType)((int)obj);
			this.accumulatingNumbers.Add(new AmazonGameCircleExampleWSAccumulatingNumber(newType));
		}
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000ABE8 File Offset: 0x00008DE8
	private void InitHashSets()
	{
		if (this.hashSets != null)
		{
			return;
		}
		this.hashSets = new AmazonGameCircleExampleWSHashSets();
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000AC04 File Offset: 0x00008E04
	private void OnNewCloudData()
	{
		this.lastCloudDataAvailable = new DateTime?(DateTime.Now);
	}

	// Token: 0x04000153 RID: 339
	private const string whispersyncMenuTitle = "Whispersync";

	// Token: 0x04000154 RID: 340
	private const string syncableNumbersLabel = "Syncable Numbers";

	// Token: 0x04000155 RID: 341
	private const string accumulatingNumbersLabel = "Accumulating Numbers";

	// Token: 0x04000156 RID: 342
	private const string syncDataButtonLabel = "Synchronize";

	// Token: 0x04000157 RID: 343
	private const string flushButtonLabel = "Flush";

	// Token: 0x04000158 RID: 344
	private const string noCloudDataReceivedLabel = "No cloud data received.";

	// Token: 0x04000159 RID: 345
	private const string cloudDataLastReceivedLabel = "Received cloud data {0,5:N1} second ago.";

	// Token: 0x0400015A RID: 346
	private const string hashSetsLabel = "Hash Sets";

	// Token: 0x0400015B RID: 347
	private const string numberListsLabel = "Syncable Number Lists";

	// Token: 0x0400015C RID: 348
	private const string whispersyncUnavailableLabel = "No Whispersync data available.";

	// Token: 0x0400015D RID: 349
	private DateTime? lastCloudDataAvailable;

	// Token: 0x0400015E RID: 350
	private bool syncableNumbersFoldout;

	// Token: 0x0400015F RID: 351
	private bool accumulatingNumbersFoldout;

	// Token: 0x04000160 RID: 352
	private bool syncableNumberListsFoldout;

	// Token: 0x04000161 RID: 353
	private bool hashSetsFoldout;

	// Token: 0x04000162 RID: 354
	private List<AmazonGameCircleExampleWSSyncableNumber> syncableNumbers;

	// Token: 0x04000163 RID: 355
	private List<AmazonGameCircleExampleWSAccumulatingNumber> accumulatingNumbers;

	// Token: 0x04000164 RID: 356
	private List<AmazonGameCircleExampleWSNumberList> syncableNumberLists;

	// Token: 0x04000165 RID: 357
	private AmazonGameCircleExampleWSHashSets hashSets;

	// Token: 0x04000166 RID: 358
	private AGSGameDataMap dataMap;
}
