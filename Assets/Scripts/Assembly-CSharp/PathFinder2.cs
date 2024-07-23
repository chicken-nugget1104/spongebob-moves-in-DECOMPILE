using System;

// Token: 0x0200042E RID: 1070
public class PathFinder2
{
	// Token: 0x06002139 RID: 8505 RVA: 0x000CD234 File Offset: 0x000CB434
	public PathFinder2(Terrain terrain)
	{
		if (!PathFinder2.initialized)
		{
			PathFinder2.maxRow = terrain.GridHeight + 2;
			PathFinder2.maxColumn = terrain.GridWidth + 2;
			PathFinder2.gridSize = PathFinder2.maxRow * PathFinder2.maxColumn;
			PathFinder2.costGrid = new byte[PathFinder2.gridSize];
			for (int i = 0; i < PathFinder2.maxRow - 1; i++)
			{
				for (int j = 0; j < PathFinder2.maxColumn - 1; j++)
				{
					byte terrainCost = terrain.GetTerrainCost(i, j);
					int num = PathFinder2.RowColToIndex(i, j);
					PathFinder2.costGrid[num] = terrainCost;
				}
			}
			int num2 = (PathFinder2.maxRow - 1) * PathFinder2.maxColumn;
			for (int k = 0; k < PathFinder2.maxColumn; k++)
			{
				PathFinder2.costGrid[k] = byte.MaxValue;
				PathFinder2.costGrid[k + num2] = byte.MaxValue;
			}
			int num3 = PathFinder2.maxColumn;
			for (int l = 1; l < PathFinder2.maxRow - 1; l++)
			{
				PathFinder2.costGrid[num3] = byte.MaxValue;
				PathFinder2.costGrid[num3 + PathFinder2.maxColumn - 1] = byte.MaxValue;
				num3 += PathFinder2.maxColumn;
			}
			PathFinder2.neightbors[0] = -PathFinder2.maxRow;
			PathFinder2.neightbors[1] = 1;
			PathFinder2.neightbors[2] = PathFinder2.maxRow;
			PathFinder2.neightbors[3] = -1;
			PathFinder2.initialized = true;
		}
		this.CreateSearchGrid();
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000CD3B0 File Offset: 0x000CB5B0
	private static int RowColToIndex(int row, int col)
	{
		return (row + 1) * PathFinder2.maxColumn + (col + 1);
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000CD3C0 File Offset: 0x000CB5C0
	private void CreateSearchGrid()
	{
		this.searchGrid = new PathFinder2.SearchGridElem[PathFinder2.gridSize];
		int num = (PathFinder2.maxRow - 1) * PathFinder2.maxColumn;
		for (int i = 0; i < PathFinder2.maxColumn; i++)
		{
			this.searchGrid[i].flags = 2;
			this.searchGrid[i + num].flags = 2;
		}
		int num2 = PathFinder2.maxColumn;
		for (int j = 1; j < PathFinder2.maxRow - 1; j++)
		{
			this.searchGrid[num2].flags = 2;
			this.searchGrid[num2 + PathFinder2.maxColumn - 1].flags = 2;
			num2 += PathFinder2.maxColumn;
		}
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x000CD47C File Offset: 0x000CB67C
	public void Start(GridPosition start, GridPosition goal)
	{
		this.goal = goal;
		this.goalRow = goal.row + 1;
		this.goalColumn = goal.col + 1;
		int num = start.row + 1;
		int num2 = start.col + 1;
		this.goalGridIdx = this.goalRow * PathFinder2.maxRow + this.goalColumn;
		int num3 = num * PathFinder2.maxRow + num2;
		PathFinder2.SearchGridElem[] array = this.searchGrid;
		int num4 = num3;
		array[num4].flags = (array[num4].flags | 1);
		this.searchGrid[num3].gscore = 0;
		int num5 = this.goalRow - num;
		int num6 = this.goalColumn - num2;
		if (num5 < 0)
		{
			num5 = -num5;
		}
		if (num6 < 0)
		{
			num6 = -num6;
		}
		this.searchGrid[num3].hscore = (short)(num5 + num6);
		this.searchGrid[num3].parent = -1;
		int num7 = PathFinder2.maxRow + PathFinder2.maxColumn;
		int num8 = num7 * 100;
		int num9 = 20;
		this.queue = new PathFinder2.PriorityQueue(num8 / num9, num9, num7 * 8);
		this.openCount = 1;
		if (this.queue.Push((ushort)num3, this.searchGrid[num3].gscore + this.searchGrid[num3].hscore))
		{
			this.progress = PathFinder2.PROGRESS.SEEKING;
		}
		else
		{
			this.progress = PathFinder2.PROGRESS.FAILED;
		}
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000CD5E0 File Offset: 0x000CB7E0
	public PathFinder2.PROGRESS Seek(int budget = 2147483647)
	{
		if (this.progress != PathFinder2.PROGRESS.SEEKING)
		{
			return this.progress;
		}
		int num = 1;
		while (this.openCount > 0)
		{
			if (num > budget)
			{
				return this.progress;
			}
			num++;
			int num2 = (int)this.queue.Pop();
			if (this.goalGridIdx == num2)
			{
				this.progress = PathFinder2.PROGRESS.DONE;
				return this.progress;
			}
			PathFinder2.SearchGridElem[] array = this.searchGrid;
			int num3 = num2;
			array[num3].flags = (array[num3].flags & -2);
			PathFinder2.SearchGridElem[] array2 = this.searchGrid;
			int num4 = num2;
			array2[num4].flags = (array2[num4].flags | 2);
			this.openCount--;
			for (int i = 0; i < 4; i++)
			{
				int num5 = num2 + PathFinder2.neightbors[i];
				short flags = this.searchGrid[num5].flags;
				if ((flags & 2) == 0)
				{
					int num6 = (int)(this.searchGrid[num2].gscore + (short)PathFinder2.costGrid[num5]);
					if ((flags & 1) != 0)
					{
						if (num6 < (int)this.searchGrid[num5].gscore)
						{
							if (!this.queue.Reinsert((ushort)num5, this.searchGrid[num5].gscore + this.searchGrid[num5].hscore, (short)(num6 + (int)this.searchGrid[num5].hscore)))
							{
								this.progress = PathFinder2.PROGRESS.FAILED;
								return this.progress;
							}
							this.searchGrid[num5].gscore = (short)num6;
							this.searchGrid[num5].parent = (short)num2;
						}
					}
					else
					{
						this.searchGrid[num5].gscore = (short)num6;
						int num7 = num5 / PathFinder2.maxRow;
						int num8 = num5 - num7 * PathFinder2.maxRow;
						int num9 = this.goalRow - num7;
						int num10 = this.goalColumn - num8;
						int num11 = num9 >> 31;
						num9 ^= num11;
						num9 += (num11 & 1);
						int num12 = num10 >> 31;
						num10 ^= num12;
						num10 += (num12 & 1);
						this.searchGrid[num5].hscore = (short)(num9 + num10);
						PathFinder2.SearchGridElem[] array3 = this.searchGrid;
						int num13 = num5;
						array3[num13].flags = (array3[num13].flags | 1);
						this.searchGrid[num5].parent = (short)num2;
						this.openCount++;
						if (!this.queue.Push((ushort)num5, this.searchGrid[num5].gscore + this.searchGrid[num5].hscore))
						{
							this.progress = PathFinder2.PROGRESS.FAILED;
							return this.progress;
						}
					}
				}
			}
		}
		this.progress = PathFinder2.PROGRESS.FAILED;
		return this.progress;
	}

	// Token: 0x0600213F RID: 8511 RVA: 0x000CD89C File Offset: 0x000CBA9C
	public void BuildPath(out Path<GridPosition> path)
	{
		path = new Path<GridPosition>();
		GridPosition position = this.goal;
		path.Add(position);
		int i = (int)this.searchGrid[this.goalGridIdx].parent;
		int num = 1;
		int num2 = 2 * (PathFinder2.maxRow + PathFinder2.maxColumn);
		while (i > 0)
		{
			int num3 = i / PathFinder2.maxRow;
			int num4 = i - num3 * PathFinder2.maxRow;
			GridPosition position2 = new GridPosition(num3 - 1, num4 - 1);
			path.Add(position2);
			TFUtils.Assert(num++ < num2, "path too long");
			i = (int)this.searchGrid[i].parent;
		}
	}

	// Token: 0x06002140 RID: 8512 RVA: 0x000CD944 File Offset: 0x000CBB44
	public static bool IsInitialized()
	{
		return PathFinder2.initialized;
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x000CD94C File Offset: 0x000CBB4C
	public static void UpdateCost(int row, int column, byte newCost)
	{
		int num = PathFinder2.RowColToIndex(row, column);
		PathFinder2.costGrid[num] = newCost;
	}

	// Token: 0x0400146B RID: 5227
	public const int NOBUDGET = 2147483647;

	// Token: 0x0400146C RID: 5228
	private const short GRID_OPEN = 1;

	// Token: 0x0400146D RID: 5229
	private const short GRID_CLOSED = 2;

	// Token: 0x0400146E RID: 5230
	private static int maxRow;

	// Token: 0x0400146F RID: 5231
	private static int maxColumn;

	// Token: 0x04001470 RID: 5232
	private static int gridSize;

	// Token: 0x04001471 RID: 5233
	private static byte[] costGrid;

	// Token: 0x04001472 RID: 5234
	private PathFinder2.SearchGridElem[] searchGrid;

	// Token: 0x04001473 RID: 5235
	private PathFinder2.PriorityQueue queue;

	// Token: 0x04001474 RID: 5236
	private static int[] neightbors = new int[4];

	// Token: 0x04001475 RID: 5237
	private int goalGridIdx;

	// Token: 0x04001476 RID: 5238
	private PathFinder2.PROGRESS progress;

	// Token: 0x04001477 RID: 5239
	private int goalRow;

	// Token: 0x04001478 RID: 5240
	private int goalColumn;

	// Token: 0x04001479 RID: 5241
	private int openCount;

	// Token: 0x0400147A RID: 5242
	private GridPosition start;

	// Token: 0x0400147B RID: 5243
	private GridPosition goal;

	// Token: 0x0400147C RID: 5244
	private static bool initialized;

	// Token: 0x0200042F RID: 1071
	public enum PROGRESS
	{
		// Token: 0x0400147E RID: 5246
		INACTIVE,
		// Token: 0x0400147F RID: 5247
		SEEKING,
		// Token: 0x04001480 RID: 5248
		FAILED,
		// Token: 0x04001481 RID: 5249
		DONE
	}

	// Token: 0x02000430 RID: 1072
	private struct SearchGridElem
	{
		// Token: 0x04001482 RID: 5250
		public short flags;

		// Token: 0x04001483 RID: 5251
		public short gscore;

		// Token: 0x04001484 RID: 5252
		public short hscore;

		// Token: 0x04001485 RID: 5253
		public short parent;
	}

	// Token: 0x02000431 RID: 1073
	private class PriorityQueue
	{
		// Token: 0x06002142 RID: 8514 RVA: 0x000CD96C File Offset: 0x000CBB6C
		public PriorityQueue(int maxBins, int binStep, int maxValues)
		{
			this.binStep = binStep;
			this.entries = new PathFinder2.PriorityQueue.QueueEntry[maxValues];
			this.binStarts = new PathFinder2.PriorityQueue.BinEntry[maxBins];
			this.maxBins = maxBins;
			this.binsPerPage = 10;
			this.maxPageCount = (byte)(maxBins / (int)this.binsPerPage);
			this.pageCount = 0;
			this.pages = new PathFinder2.PriorityQueue.Page[(int)this.maxPageCount];
			this.freePage = byte.MaxValue;
			for (int i = 0; i < maxBins; i++)
			{
				this.binStarts[i].page = byte.MaxValue;
			}
			for (int j = 0; j < maxValues; j++)
			{
				this.entries[j].gridIdx = ushort.MaxValue;
				this.entries[j].next = (ushort)(j + 1);
			}
			this.entries[maxValues - 1].next = ushort.MaxValue;
			this.minOccupiedBin = int.MaxValue;
			this.minOccupiedEntryInBin = 0;
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x000CDA70 File Offset: 0x000CBC70
		private bool AddPage()
		{
			if (this.pageCount >= this.maxPageCount)
			{
				return false;
			}
			int num = this.binStep * (int)this.binsPerPage;
			this.pages[(int)this.pageCount].entryStarts = new ushort[num];
			for (int i = 0; i < num; i++)
			{
				this.pages[(int)this.pageCount].entryStarts[i] = ushort.MaxValue;
			}
			int num2 = 0;
			for (int j = 0; j < (int)(this.binsPerPage - 1); j++)
			{
				this.pages[(int)this.pageCount].entryStarts[num2] = (ushort)((int)this.pageCount << 8 | j + 1);
				num2 += this.binStep;
			}
			this.pages[(int)this.pageCount].entryStarts[num2] = (ushort)((int)this.freePage << 8 | (int)this.freeBinInPage);
			this.freePage = this.pageCount;
			this.freeBinInPage = 0;
			this.pageCount += 1;
			return true;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x000CDB80 File Offset: 0x000CBD80
		public bool Push(ushort gridIndex, short val)
		{
			int num = (int)val / this.binStep;
			if (num >= this.maxBins)
			{
				return false;
			}
			ushort num2 = this.firstFreeEntry;
			this.firstFreeEntry = this.entries[(int)this.firstFreeEntry].next;
			TFUtils.Assert(this.firstFreeEntry != ushort.MaxValue, "no free list entries in priorityQueue");
			if (this.binStarts[num].entriesCount == 0)
			{
				if (this.freePage == 255 && !this.AddPage())
				{
					return false;
				}
				this.binStarts[num].page = this.freePage;
				this.binStarts[num].binInPage = this.freeBinInPage;
				ushort num3 = this.pages[(int)this.freePage].entryStarts[(int)this.freeBinInPage * this.binStep];
				this.pages[(int)this.freePage].entryStarts[(int)this.freeBinInPage * this.binStep] = ushort.MaxValue;
				this.freePage = (byte)(num3 >> 8);
				this.freeBinInPage = (byte)(num3 & 255);
			}
			PathFinder2.PriorityQueue.BinEntry[] array = this.binStarts;
			int num4 = num;
			array[num4].entriesCount = array[num4].entriesCount + 1;
			int num5 = (int)val - num * this.binStep;
			TFUtils.Assert(num5 >= 0 && num5 < this.binStep, "wrong indexInbin in priorityQueue");
			int page = (int)this.binStarts[num].page;
			int num6 = this.binStep * (int)this.binStarts[num].binInPage + num5;
			this.entries[(int)num2].entryValue = val;
			this.entries[(int)num2].gridIdx = gridIndex;
			ushort next = this.pages[page].entryStarts[num6];
			this.entries[(int)num2].next = next;
			this.pages[page].entryStarts[num6] = num2;
			if (num < this.minOccupiedBin)
			{
				this.minOccupiedBin = num;
				this.minOccupiedEntryInBin = num5;
			}
			else if (num == this.minOccupiedBin && num5 < this.minOccupiedEntryInBin)
			{
				this.minOccupiedEntryInBin = num5;
			}
			return true;
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000CDDBC File Offset: 0x000CBFBC
		public ushort Pop()
		{
			if (this.binStarts[this.minOccupiedBin].page == 255)
			{
				int num = this.minOccupiedBin + 1;
				while (this.binStarts[num].page == 255)
				{
					num++;
					TFUtils.Assert(num < this.maxBins, "bad binIndex in priorityQueue while searching for min bin");
				}
				this.minOccupiedBin = num;
				this.minOccupiedEntryInBin = 0;
			}
			TFUtils.Assert(this.binStarts[this.minOccupiedBin].entriesCount > 0, "no entries in priorityQueue for Pop");
			byte page = this.binStarts[this.minOccupiedBin].page;
			byte binInPage = this.binStarts[this.minOccupiedBin].binInPage;
			int num2 = (int)binInPage * this.binStep;
			while (this.pages[(int)page].entryStarts[num2 + this.minOccupiedEntryInBin] == 65535)
			{
				this.minOccupiedEntryInBin++;
				TFUtils.Assert(num2 + this.minOccupiedEntryInBin < this.binStep * (int)this.binsPerPage, "invalid loop while searching for minOccupiedEntryBin in priorityQueue");
			}
			TFUtils.Assert(this.minOccupiedEntryInBin < this.binStep, "bad minOccupiedEntryInBin in priorityQueue while searching for min index in bin");
			ushort num3 = this.pages[(int)page].entryStarts[num2 + this.minOccupiedEntryInBin];
			ushort next = this.entries[(int)num3].next;
			ushort gridIdx = this.entries[(int)num3].gridIdx;
			this.pages[(int)page].entryStarts[num2 + this.minOccupiedEntryInBin] = next;
			this.entries[(int)num3].next = this.firstFreeEntry;
			this.firstFreeEntry = num3;
			PathFinder2.PriorityQueue.BinEntry[] array = this.binStarts;
			int num4 = this.minOccupiedBin;
			array[num4].entriesCount = array[num4].entriesCount - 1;
			if (this.binStarts[this.minOccupiedBin].entriesCount == 0)
			{
				this.pages[(int)page].entryStarts[num2] = (ushort)((int)this.freePage << 8 | (int)this.freeBinInPage);
				this.freePage = page;
				this.freeBinInPage = binInPage;
				this.binStarts[this.minOccupiedBin].page = byte.MaxValue;
			}
			return gridIdx;
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x000CE00C File Offset: 0x000CC20C
		public bool Reinsert(ushort gridIndex, short oldVal, short newVal)
		{
			int num = (int)oldVal / this.binStep;
			if (num >= this.maxBins)
			{
				return false;
			}
			byte page = this.binStarts[num].page;
			byte binInPage = this.binStarts[num].binInPage;
			int num2 = (int)binInPage * this.binStep;
			int num3 = (int)oldVal - num * this.binStep;
			ushort num4 = ushort.MaxValue;
			ushort num5 = this.pages[(int)page].entryStarts[num2 + num3];
			while (this.entries[(int)num5].gridIdx != gridIndex)
			{
				num4 = num5;
				num5 = this.entries[(int)num5].next;
			}
			if (num4 == 65535)
			{
				this.pages[(int)page].entryStarts[num2 + num3] = this.entries[(int)num5].next;
			}
			else
			{
				this.entries[(int)num4].next = this.entries[(int)num5].next;
			}
			this.entries[(int)num5].next = this.firstFreeEntry;
			this.firstFreeEntry = num5;
			PathFinder2.PriorityQueue.BinEntry[] array = this.binStarts;
			int num6 = num;
			array[num6].entriesCount = array[num6].entriesCount - 1;
			if (this.binStarts[num].entriesCount == 0)
			{
				this.pages[(int)page].entryStarts[num2] = (ushort)((int)this.freePage << 8 | (int)this.freeBinInPage);
				this.freePage = page;
				this.freeBinInPage = binInPage;
				this.binStarts[num].page = byte.MaxValue;
			}
			return this.Push(gridIndex, newVal);
		}

		// Token: 0x04001486 RID: 5254
		private const ushort INVALID_ENTRY = 65535;

		// Token: 0x04001487 RID: 5255
		private const byte INVALID_PAGE = 255;

		// Token: 0x04001488 RID: 5256
		private int binStep;

		// Token: 0x04001489 RID: 5257
		private int maxBins;

		// Token: 0x0400148A RID: 5258
		private int minOccupiedBin;

		// Token: 0x0400148B RID: 5259
		private int minOccupiedEntryInBin;

		// Token: 0x0400148C RID: 5260
		private ushort firstFreeEntry;

		// Token: 0x0400148D RID: 5261
		private short binsPerPage;

		// Token: 0x0400148E RID: 5262
		private byte maxPageCount;

		// Token: 0x0400148F RID: 5263
		private byte pageCount;

		// Token: 0x04001490 RID: 5264
		private PathFinder2.PriorityQueue.QueueEntry[] entries;

		// Token: 0x04001491 RID: 5265
		private PathFinder2.PriorityQueue.BinEntry[] binStarts;

		// Token: 0x04001492 RID: 5266
		private PathFinder2.PriorityQueue.Page[] pages;

		// Token: 0x04001493 RID: 5267
		public byte freePage;

		// Token: 0x04001494 RID: 5268
		public byte freeBinInPage;

		// Token: 0x02000432 RID: 1074
		private struct QueueEntry
		{
			// Token: 0x04001495 RID: 5269
			public short entryValue;

			// Token: 0x04001496 RID: 5270
			public ushort gridIdx;

			// Token: 0x04001497 RID: 5271
			public ushort next;
		}

		// Token: 0x02000433 RID: 1075
		private struct BinEntry
		{
			// Token: 0x04001498 RID: 5272
			public byte page;

			// Token: 0x04001499 RID: 5273
			public byte binInPage;

			// Token: 0x0400149A RID: 5274
			public short entriesCount;
		}

		// Token: 0x02000434 RID: 1076
		private struct Page
		{
			// Token: 0x0400149B RID: 5275
			public ushort[] entryStarts;
		}
	}
}
