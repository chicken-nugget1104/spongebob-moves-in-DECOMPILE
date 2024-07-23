using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class TreasureSpawner
{
	// Token: 0x060013F4 RID: 5108 RVA: 0x000897F8 File Offset: 0x000879F8
	public TreasureSpawner(List<int> didsToSpawn, string persistName, string featureLockName, int spawnLimit, int minTime, int maxTime, int patchySpawner, Session session)
	{
		this.didsToSpawn = didsToSpawn;
		this.spawnMessage = persistName + "_spawn";
		this.persistName = persistName;
		this.featureLockName = featureLockName;
		this.featureUnlocked = true;
		this.minTime = minTime;
		this.maxTime = maxTime;
		this.spawnLimit = spawnLimit;
		this.isPatchySpawner = (patchySpawner == 1);
		this.tickSpawnCount = 1;
		if (this.isPatchySpawner)
		{
			this.tickSpawnCount = this.spawnLimit;
		}
		this.session = session;
		this.timer = new Timer();
		this.timer.AutoReset = false;
		if (TreasureSpawner.rand == null)
		{
			TreasureSpawner.rand = new System.Random(UnityEngine.Random.Range(-100000, 100000));
		}
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x000898C4 File Offset: 0x00087AC4
	public void UpdateFeatureLock()
	{
		if (!this.featureUnlocked)
		{
			this.featureUnlocked = this.session.TheGame.featureManager.CheckFeature(this.featureLockName);
			if (this.featureUnlocked)
			{
				this.Start();
			}
		}
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x00089904 File Offset: 0x00087B04
	private void Stop()
	{
		this.nextTreasureTime = null;
		this.timer.Enabled = false;
		if (TreasureSpawner.logDebugging)
		{
			TFUtils.DebugLog("Treasure: " + this.persistName + " said stop");
		}
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x00089950 File Offset: 0x00087B50
	public void Start()
	{
		if (TreasureSpawner.logDebugging)
		{
			TFUtils.DebugLog("Treasure: " + this.persistName + " said start");
		}
		this.featureUnlocked = this.session.TheGame.featureManager.CheckFeature(this.featureLockName);
		this.RecalculateCount();
		if (TreasureSpawner.logDebugging)
		{
			TFUtils.DebugLog(string.Concat(new object[]
			{
				"Number of ",
				this.didsToSpawn[0],
				" treasures ",
				this.count
			}));
		}
		ulong num = TFUtils.EpochTime();
		if (this.count < this.spawnLimit && this.featureUnlocked)
		{
			if (this.nextTreasureTime == null)
			{
				if (TreasureSpawner.logDebugging)
				{
					TFUtils.DebugLog("Create new time");
				}
				this.Stop();
				this.nextTreasureTime = new ulong?(num + (ulong)((long)TreasureSpawner.rand.Next(this.minTime, this.maxTime)));
				if (!this.session.InFriendsGame)
				{
					this.session.TheGame.simulation.ModifyGameState(new TreasureCooldownAction(this.nextTreasureTime.Value, this.persistName));
				}
			}
			else
			{
				if (this.nextTreasureTime.Value <= num)
				{
					int num2 = this.spawnLimit - this.count;
					ulong num3 = num - this.nextTreasureTime.Value;
					int num4 = num2;
					if (!this.session.InFriendsGame)
					{
						int val = 1 + (int)(num3 / ((double)(this.maxTime + this.minTime) / 2.0));
						num4 = Math.Min(val, num2);
					}
					else
					{
						this.tickSpawnCount = this.spawnLimit;
					}
					this.session.AddAsyncResponse(this.spawnMessage, num4);
					if (TreasureSpawner.logDebugging)
					{
						TFUtils.DebugLog(string.Concat(new object[]
						{
							"Just spawn ",
							num4,
							" items because time was: ",
							this.nextTreasureTime.Value - num
						}));
					}
					return;
				}
				if (TreasureSpawner.logDebugging)
				{
					TFUtils.DebugLog(string.Concat(new object[]
					{
						"Current Time: ",
						num,
						" Value exists: ",
						this.nextTreasureTime.Value
					}));
				}
			}
			this.timer.Enabled = false;
			double num5 = 0.0;
			if (this.session.InFriendsGame)
			{
				if (SBMISoaring.PatchTownTreasureSpawnTimestamp > 0L)
				{
					num5 = (double)SBMISoaring.PatchTownTreasureSpawnTimestamp - (double)SoaringTime.AdjustedServerTime;
				}
				if (num5 <= 0.0)
				{
					SBMISoaring.PatchTownTreasureSpawnTimestamp = SoaringTime.AdjustedServerTime + (long)this.maxTime;
					SBMISoaring.PatchTownTreasureCollected = 0;
					Soaring.UpdateUserProfile(Soaring.Player.CustomData, null);
					num5 = 1.0;
				}
			}
			else
			{
				num5 = (this.nextTreasureTime.Value - num) * 1000.0;
			}
			if (num5 > 2147483647.0)
			{
				num5 = 2147483647.0;
			}
			this.timer.Interval = num5;
			this.timer.Enabled = true;
			this.timer.Elapsed += delegate(object A_1, ElapsedEventArgs A_2)
			{
				TreasureSpawner.TimerTick(this);
			};
			if (TreasureSpawner.logDebugging)
			{
				TFUtils.DebugLog(string.Concat(new object[]
				{
					"Treasure: ",
					this.persistName,
					" will start in: ",
					num5
				}));
			}
		}
		else if (TreasureSpawner.logDebugging)
		{
			TFUtils.DebugLog(string.Concat(new object[]
			{
				"We are not spawning treasure because: count( ",
				this.count,
				" ) of ( ",
				this.spawnLimit,
				" ) and featureLock is ( ",
				this.featureUnlocked,
				" )"
			}));
		}
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x00089D64 File Offset: 0x00087F64
	public void Reset(ulong? time)
	{
		if (TreasureSpawner.logDebugging)
		{
			TFUtils.DebugLog("RESET TIME");
		}
		this.Stop();
		this.nextTreasureTime = time;
		this.Start();
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x00089D90 File Offset: 0x00087F90
	public static void TimerTick(TreasureSpawner timing)
	{
		if (TreasureSpawner.logDebugging)
		{
			TFUtils.DebugLog("Timer fired, spawning with: " + timing.spawnMessage);
		}
		timing.session.AddAsyncResponse(timing.spawnMessage, timing.tickSpawnCount);
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x00089DD0 File Offset: 0x00087FD0
	public void MarkComplete()
	{
		this.Stop();
		this.RecalculateCount();
		if (this.count < this.spawnLimit)
		{
			this.Start();
		}
		if (TreasureSpawner.logDebugging)
		{
			TFUtils.DebugLog("Treasure spawned is done, " + this.count + " treasures left");
		}
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x00089E2C File Offset: 0x0008802C
	public void MarkCollected()
	{
		this.RecalculateCount();
		this.count--;
		this.Start();
		if (TreasureSpawner.logDebugging)
		{
			TFUtils.DebugLog("Treasure was collected, " + this.count + " treasures left");
		}
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x00089E7C File Offset: 0x0008807C
	private void RecalculateCount()
	{
		this.count = 0;
		foreach (int did in this.didsToSpawn)
		{
			List<Simulated> list = this.session.TheGame.simulation.FindAllSimulateds(did, new EntityType?(EntityType.TREASURE));
			this.count += list.Count;
		}
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x00089F18 File Offset: 0x00088118
	public bool PlaceTreasure()
	{
		if (TreasureSpawner.logDebugging)
		{
			TFUtils.DebugLog("Place treasure");
		}
		bool result = false;
		long num = 0L;
		if (this.session.InFriendsGame)
		{
			num = (long)SBMISoaring.PatchTownTreasureCollected;
		}
		if ((long)this.count + num < (long)this.spawnLimit)
		{
			int index = TreasureSpawner.rand.Next(this.didsToSpawn.Count);
			Entity entity = this.session.TheGame.entities.Create(EntityType.TREASURE, this.didsToSpawn[index], true);
			Vector2 position = this.GenerateLocation();
			Simulated simulated = this.session.TheGame.simulation.CreateSimulated(entity, EntityManager.TreasureActions["buried"], position);
			simulated.GetEntity<TreasureEntity>().TreasureTiming = this;
			simulated.Warp(simulated.Position, this.session.TheGame.simulation);
			simulated.Visible = true;
			result = true;
			simulated.SetDisplayOffsetWorld(this.session.TheGame.simulation);
			if (this.session.InFriendsGame)
			{
				this.RecalculateCount();
			}
			else
			{
				this.MarkComplete();
			}
			Debug.Log(string.Concat(new object[]
			{
				"Placing Treasure:",
				this.didsToSpawn[index],
				"  how many spawned:",
				this.count,
				" limit is",
				this.spawnLimit
			}));
			if (!this.session.InFriendsGame)
			{
				this.session.TheGame.simulation.ModifyGameStateSimulated(simulated, new TreasureSpawnAction(simulated, this));
			}
		}
		return result;
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x0008A0D4 File Offset: 0x000882D4
	public Vector2 GenerateLocation()
	{
		int num = 10;
		bool flag = false;
		AlignedBox purchasedExtent = this.session.TheGame.terrain.PurchasedExtent;
		Vector2 zero = Vector2.zero;
		int num2 = Mathf.RoundToInt(purchasedExtent.xmin);
		int maxValue = Mathf.RoundToInt(purchasedExtent.xmax);
		int num3 = Mathf.RoundToInt(purchasedExtent.ymin);
		int maxValue2 = Mathf.RoundToInt(purchasedExtent.ymax);
		while (num > 0 && !flag)
		{
			zero.x = (float)TreasureSpawner.rand.Next(num2, maxValue);
			zero.y = (float)TreasureSpawner.rand.Next(num3, maxValue2);
			Vector2 point = zero;
			int num4 = TreasureSpawner.rand.Next(2);
			if (num4 == 1)
			{
				zero.x = (float)num2;
				point.x = zero.x + 1f;
			}
			else
			{
				zero.y = (float)num3;
				point.y = zero.y + 1f;
			}
			flag = this.session.TheGame.simulation.Terrain.CheckIsPurchasedArea(point);
			num--;
		}
		return zero;
	}

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06001400 RID: 5120 RVA: 0x0008A1F0 File Offset: 0x000883F0
	public string SpawnMessage
	{
		get
		{
			return this.spawnMessage;
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06001401 RID: 5121 RVA: 0x0008A1F8 File Offset: 0x000883F8
	public string PersistName
	{
		get
		{
			return this.persistName;
		}
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06001402 RID: 5122 RVA: 0x0008A200 File Offset: 0x00088400
	public ulong? TimeToTreasure
	{
		get
		{
			return this.nextTreasureTime;
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06001403 RID: 5123 RVA: 0x0008A208 File Offset: 0x00088408
	public bool IsPatchySpawner
	{
		get
		{
			return this.isPatchySpawner;
		}
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06001404 RID: 5124 RVA: 0x0008A210 File Offset: 0x00088410
	public int SpawnLimit
	{
		get
		{
			return this.spawnLimit;
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06001405 RID: 5125 RVA: 0x0008A218 File Offset: 0x00088418
	public int MaxTime
	{
		get
		{
			return this.maxTime;
		}
	}

	// Token: 0x04000DFA RID: 3578
	private static bool logDebugging = true;

	// Token: 0x04000DFB RID: 3579
	private int spawnLimit;

	// Token: 0x04000DFC RID: 3580
	private int count;

	// Token: 0x04000DFD RID: 3581
	private List<int> didsToSpawn;

	// Token: 0x04000DFE RID: 3582
	private string spawnMessage;

	// Token: 0x04000DFF RID: 3583
	private string featureLockName;

	// Token: 0x04000E00 RID: 3584
	private string persistName;

	// Token: 0x04000E01 RID: 3585
	private int minTime;

	// Token: 0x04000E02 RID: 3586
	private int maxTime;

	// Token: 0x04000E03 RID: 3587
	private Session session;

	// Token: 0x04000E04 RID: 3588
	private bool isPatchySpawner;

	// Token: 0x04000E05 RID: 3589
	private int tickSpawnCount;

	// Token: 0x04000E06 RID: 3590
	private Timer timer;

	// Token: 0x04000E07 RID: 3591
	private static System.Random rand;

	// Token: 0x04000E08 RID: 3592
	private ulong? nextTreasureTime;

	// Token: 0x04000E09 RID: 3593
	private bool featureUnlocked;
}
