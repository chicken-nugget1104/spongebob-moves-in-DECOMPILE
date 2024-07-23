using System;
using System.Collections.Generic;

// Token: 0x02000267 RID: 615
public class TreasureManager
{
	// Token: 0x060013EC RID: 5100 RVA: 0x00089498 File Offset: 0x00087698
	public TreasureManager(Session session)
	{
		this.treasureSpawners = new List<TreasureSpawner>();
		this.LoadTreasureSpawnersFromSpread(session);
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x000894B4 File Offset: 0x000876B4
	private void LoadTreasureSpawnersFromSpread(Session pSession)
	{
		string text = "TreasureSpawn";
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "is patchy town");
				bool flag = intCell == 1;
				if (flag == pSession.InFriendsGame)
				{
					this.treasureSpawners.Add(new TreasureSpawner(new List<int>
					{
						instance.GetIntCell(sheetIndex, rowIndex, "items to spawn")
					}, instance.GetStringCell(sheetIndex, rowIndex, "persist name"), instance.GetStringCell(sheetIndex, rowIndex, "feature lock"), instance.GetIntCell(sheetIndex, rowIndex, "spawn limit"), instance.GetIntCell(sheetIndex, rowIndex, "min time"), instance.GetIntCell(sheetIndex, rowIndex, "max time"), intCell, pSession));
				}
			}
		}
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x00089610 File Offset: 0x00087810
	private string[] GetFilesToLoad()
	{
		return Config.TREASURE_PATH;
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x00089618 File Offset: 0x00087818
	private string GetFilePathFromString(string filePath)
	{
		return filePath;
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x0008961C File Offset: 0x0008781C
	public void InitializeTreasureTimers(Dictionary<string, object> dict)
	{
		TFUtils.Assert(this.treasureSpawners.Count > 0, "We need to init TreasureManager before getting data from it");
		foreach (TreasureSpawner treasureSpawner in this.treasureSpawners)
		{
			ulong? time = TFUtils.TryLoadNullableUlong(dict, treasureSpawner.PersistName);
			treasureSpawner.Reset(time);
		}
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x000896A8 File Offset: 0x000878A8
	public void OnUpdate(Session session)
	{
		foreach (TreasureSpawner treasureSpawner in this.treasureSpawners)
		{
			treasureSpawner.UpdateFeatureLock();
			int? num = (int?)session.CheckAsyncRequest(treasureSpawner.SpawnMessage);
			if (num != null)
			{
				int num2 = 0;
				while (num != null && num2 < num.Value)
				{
					treasureSpawner.PlaceTreasure();
					num2++;
				}
			}
		}
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x0008975C File Offset: 0x0008795C
	public void StartTreasureTimers()
	{
		foreach (TreasureSpawner treasureSpawner in this.treasureSpawners)
		{
			treasureSpawner.Start();
		}
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x000897C4 File Offset: 0x000879C4
	public TreasureSpawner FindTreasureSpawner(string persistName)
	{
		return this.treasureSpawners.Find((TreasureSpawner t) => t.PersistName == persistName);
	}

	// Token: 0x04000DF8 RID: 3576
	private const string TREASURE_PATH = "Treasure";

	// Token: 0x04000DF9 RID: 3577
	private List<TreasureSpawner> treasureSpawners;
}
