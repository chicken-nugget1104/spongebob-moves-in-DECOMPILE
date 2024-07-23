using System;
using System.Collections.Generic;

// Token: 0x0200015A RID: 346
public class ProductionSlotTable
{
	// Token: 0x06000BE7 RID: 3047 RVA: 0x000481E4 File Offset: 0x000463E4
	public ProductionSlotTable(Dictionary<string, object> data)
	{
		this.did = TFUtils.LoadInt(data, "did");
		this.initSlots = TFUtils.LoadInt(data, "init_slots");
		this.costs = new List<Cost>();
		List<object> list = TFUtils.LoadList<object>(data, "costs");
		foreach (object o in list)
		{
			this.costs.Add(Cost.FromObject(o));
		}
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x00048290 File Offset: 0x00046490
	public ProductionSlotTable(int did, int initSlots, List<Cost> costs)
	{
		this.did = did;
		this.initSlots = initSlots;
		this.costs = costs;
		TFUtils.Assert(costs.Count > 0, "Should not be defining a Production Slot Table with no costs: did " + did);
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x000482CC File Offset: 0x000464CC
	public int MinSlots
	{
		get
		{
			return this.initSlots;
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06000BEA RID: 3050 RVA: 0x000482D4 File Offset: 0x000464D4
	public int MaxSlots
	{
		get
		{
			return this.initSlots + this.costs.Count;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06000BEB RID: 3051 RVA: 0x000482E8 File Offset: 0x000464E8
	public int Did
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x000482F0 File Offset: 0x000464F0
	public Cost GetCostForSlot(int slotId)
	{
		if (slotId >= this.MaxSlots)
		{
			return null;
		}
		if (slotId < this.initSlots)
		{
			return this.costs[0];
		}
		return this.costs[slotId - this.initSlots];
	}

	// Token: 0x040007FD RID: 2045
	public const string TYPE = "slot_costs";

	// Token: 0x040007FE RID: 2046
	private int did;

	// Token: 0x040007FF RID: 2047
	private int initSlots;

	// Token: 0x04000800 RID: 2048
	private List<Cost> costs;
}
