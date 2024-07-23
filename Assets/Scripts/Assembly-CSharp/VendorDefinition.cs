using System;
using System.Collections.Generic;

// Token: 0x02000274 RID: 628
public class VendorDefinition
{
	// Token: 0x06001435 RID: 5173 RVA: 0x0008B378 File Offset: 0x00089578
	public VendorDefinition(Dictionary<string, object> data)
	{
		this.did = TFUtils.LoadInt(data, "did");
		this.sessionActionId = TFUtils.LoadString(data, "session_action_id");
		this.cancelButtonTexture = TFUtils.LoadString(data, "texture.cancelbutton");
		this.titleTexture = TFUtils.LoadString(data, "texture.title");
		this.titleIconTexture = TFUtils.LoadString(data, "texture.titleicon");
		this.backgroundColor = ((List<object>)data["background.color"]).ConvertAll<int>((object x) => Convert.ToInt32(x));
		this.buttonLabel = Language.Get(TFUtils.LoadString(data, "button.label"));
		this.openSound = TFUtils.LoadString(data, "open_sound");
		this.closeSound = TFUtils.LoadString(data, "close_sound");
		this.music = TFUtils.LoadNullableString(data, "music");
		if (data.ContainsKey("general"))
		{
			this.generalStock = ((List<object>)data["general"]).ConvertAll<int>((object x) => Convert.ToInt32(x));
		}
		else
		{
			this.generalStock = new List<int>();
		}
		if (data.ContainsKey("specials"))
		{
			this.specialStock = ((List<object>)data["specials"]).ConvertAll<int>((object x) => Convert.ToInt32(x));
		}
		else
		{
			this.specialStock = new List<int>();
		}
		this.rushCost = Cost.FromObject(data["restock_cost"]);
		if (data.ContainsKey("stock_count"))
		{
			this.count = TFUtils.LoadInt(data, "stock_count");
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06001436 RID: 5174 RVA: 0x0008B550 File Offset: 0x00089750
	public Cost RushCost
	{
		get
		{
			return this.rushCost;
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06001437 RID: 5175 RVA: 0x0008B558 File Offset: 0x00089758
	public int InstanceCount
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x04000E25 RID: 3621
	public const string TYPE = "vendor";

	// Token: 0x04000E26 RID: 3622
	public const int COUNT = 12;

	// Token: 0x04000E27 RID: 3623
	public List<int> generalStock;

	// Token: 0x04000E28 RID: 3624
	public List<int> specialStock;

	// Token: 0x04000E29 RID: 3625
	public int did;

	// Token: 0x04000E2A RID: 3626
	public string sessionActionId;

	// Token: 0x04000E2B RID: 3627
	public string cancelButtonTexture;

	// Token: 0x04000E2C RID: 3628
	public string titleTexture;

	// Token: 0x04000E2D RID: 3629
	public string titleIconTexture;

	// Token: 0x04000E2E RID: 3630
	public List<int> backgroundColor;

	// Token: 0x04000E2F RID: 3631
	public string buttonLabel;

	// Token: 0x04000E30 RID: 3632
	public string openSound;

	// Token: 0x04000E31 RID: 3633
	public string closeSound;

	// Token: 0x04000E32 RID: 3634
	public string music;

	// Token: 0x04000E33 RID: 3635
	private Cost rushCost;

	// Token: 0x04000E34 RID: 3636
	private int count = 12;
}
