using System;
using System.Collections.Generic;

// Token: 0x02000272 RID: 626
public class VendingInstance
{
	// Token: 0x0600141D RID: 5149 RVA: 0x0008A5BC File Offset: 0x000887BC
	public VendingInstance(int slotId, int stockId, int remaining, Cost cost, bool special)
	{
		this.slotId = slotId;
		this.stockId = stockId;
		this.remaining = remaining;
		this.cost = cost;
		this.special = special;
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x0008A5EC File Offset: 0x000887EC
	public static VendingInstance FromDict(Dictionary<string, object> data)
	{
		int num = TFUtils.LoadInt(data, "slot_id");
		int num2 = TFUtils.LoadInt(data, "stock_id");
		int num3 = TFUtils.LoadInt(data, "remaining");
		Cost cost = Cost.FromObject(data["cost"]);
		bool flag = TFUtils.LoadBool(data, "special");
		return new VendingInstance(num, num2, num3, cost, flag);
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x0008A648 File Offset: 0x00088848
	public Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["slot_id"] = this.slotId;
		dictionary["stock_id"] = this.stockId;
		dictionary["remaining"] = this.remaining;
		dictionary["cost"] = this.cost.ToDict();
		dictionary["special"] = this.special;
		return dictionary;
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x0008A6CC File Offset: 0x000888CC
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"[VendingInstance (vendorStockId= ",
			this.stockId,
			", cost= ",
			this.cost,
			", slotId= ",
			this.slotId,
			", remaining= ",
			this.remaining,
			", special= ",
			this.special,
			")]"
		});
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06001421 RID: 5153 RVA: 0x0008A758 File Offset: 0x00088958
	public int StockId
	{
		get
		{
			return this.stockId;
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06001422 RID: 5154 RVA: 0x0008A760 File Offset: 0x00088960
	public int SlotId
	{
		get
		{
			return this.slotId;
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06001423 RID: 5155 RVA: 0x0008A768 File Offset: 0x00088968
	public Cost Cost
	{
		get
		{
			return this.cost;
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001424 RID: 5156 RVA: 0x0008A770 File Offset: 0x00088970
	public bool Special
	{
		get
		{
			return this.special;
		}
	}

	// Token: 0x04000E16 RID: 3606
	public int remaining;

	// Token: 0x04000E17 RID: 3607
	private int stockId;

	// Token: 0x04000E18 RID: 3608
	private int slotId;

	// Token: 0x04000E19 RID: 3609
	private Cost cost;

	// Token: 0x04000E1A RID: 3610
	private bool special;
}
