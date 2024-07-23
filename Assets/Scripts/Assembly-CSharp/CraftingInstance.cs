using System;
using System.Collections.Generic;

// Token: 0x02000157 RID: 343
public class CraftingInstance
{
	// Token: 0x06000BB8 RID: 3000 RVA: 0x000464D4 File Offset: 0x000446D4
	public CraftingInstance(Dictionary<string, object> data)
	{
		this.buildingLabel = new Identity((string)data["building_label"]);
		this.readyTimeUtc = TFUtils.LoadUlong(data, "ready_time", 0UL);
		this.recipeId = TFUtils.LoadInt(data, "recipe_id");
		this.reward = Reward.FromObject(data["reward"]);
		this.slotId = TFUtils.LoadInt(data, "slot_id");
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00046550 File Offset: 0x00044750
	public CraftingInstance(Identity label, int recipeId, ulong readyTimeUtc, Reward reward, int slotId)
	{
		this.buildingLabel = label;
		this.readyTimeUtc = readyTimeUtc;
		this.recipeId = recipeId;
		this.reward = reward;
		this.slotId = slotId;
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x00046580 File Offset: 0x00044780
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"[CraftingInstance (label=",
			this.buildingLabel,
			", readyTimeUtc= ",
			this.readyTimeUtc,
			", recipeId= ",
			this.recipeId,
			", reward= ",
			this.reward,
			", slotId= ",
			this.slotId,
			")]"
		});
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000BBC RID: 3004 RVA: 0x00046614 File Offset: 0x00044814
	// (set) Token: 0x06000BBB RID: 3003 RVA: 0x00046608 File Offset: 0x00044808
	public ulong ReadyTimeUtc
	{
		get
		{
			return this.readyTimeUtc;
		}
		set
		{
			this.readyTimeUtc = value;
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0004662C File Offset: 0x0004482C
	// (set) Token: 0x06000BBD RID: 3005 RVA: 0x0004661C File Offset: 0x0004481C
	public ulong ReadyTimeFromNow
	{
		get
		{
			return this.readyTimeUtc - TFUtils.EpochTime();
		}
		set
		{
			this.readyTimeUtc = value + TFUtils.EpochTime();
		}
	}

	// Token: 0x040007CB RID: 1995
	public Identity buildingLabel;

	// Token: 0x040007CC RID: 1996
	public int slotId;

	// Token: 0x040007CD RID: 1997
	private ulong readyTimeUtc;

	// Token: 0x040007CE RID: 1998
	public int recipeId;

	// Token: 0x040007CF RID: 1999
	public Reward reward;

	// Token: 0x040007D0 RID: 2000
	public bool rushed;
}
