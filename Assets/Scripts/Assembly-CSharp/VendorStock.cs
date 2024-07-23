using System;
using System.Collections.Generic;

// Token: 0x02000275 RID: 629
public class VendorStock
{
	// Token: 0x0600143B RID: 5179 RVA: 0x0008B578 File Offset: 0x00089778
	public VendorStock(int did, string name, string description, string icon, int minLevel, RewardDefinition rewardDefinition, CdfDictionary<Cost> costs, ResultGenerator instances)
	{
		this.icon = icon;
		this.did = did;
		this.name = name;
		this.description = description;
		this.minLevel = minLevel;
		this.rewardDefinition = rewardDefinition;
		this.costs = costs;
		this.instances = instances;
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x0008B5C8 File Offset: 0x000897C8
	public static VendorStock FromDict(Dictionary<string, object> data)
	{
		if (data == null)
		{
			return null;
		}
		int num = TFUtils.LoadInt(data, "did");
		string arg = TFUtils.LoadString(data, "name");
		string text = TFUtils.LoadString(data, "description");
		int num2 = TFUtils.LoadInt(data, "minimum_level");
		RewardDefinition rewardDefinition = RewardDefinition.FromObject(data["reward"]);
		CdfDictionary<Cost>.ParseT parser = (object val) => Cost.FromObject(val);
		List<object> data2 = TFUtils.LoadList<object>(data, "costs");
		CdfDictionary<Cost> cdfDictionary = CdfDictionary<Cost>.FromList(data2, parser);
		cdfDictionary.Validate(true, string.Format("VendorStock id {0} name {1}", num, arg));
		ResultGenerator resultGenerator;
		if (data["instances"] is Dictionary<string, object>)
		{
			resultGenerator = new ProbabilityTable((Dictionary<string, object>)data["instances"]);
		}
		else if (data["instances"] is List<object>)
		{
			resultGenerator = new UniformGenerator((List<object>)data["instances"]);
		}
		else
		{
			resultGenerator = new ConstantGenerator(data["instances"].ToString());
		}
		string text2 = (string)data["icon"];
		return new VendorStock(num, arg, text, text2, num2, rewardDefinition, cdfDictionary, resultGenerator);
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x0008B710 File Offset: 0x00089910
	public VendingInstance GenerateVendingInstance(int slotId, bool special)
	{
		string result = this.instances.GetResult();
		int remaining = (result != null) ? int.Parse(result) : 1;
		return new VendingInstance(slotId, this.did, remaining, this.costs.Spin(), special);
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x0600143E RID: 5182 RVA: 0x0008B758 File Offset: 0x00089958
	public int Did
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x0600143F RID: 5183 RVA: 0x0008B760 File Offset: 0x00089960
	public string Name
	{
		get
		{
			return this.name;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06001440 RID: 5184 RVA: 0x0008B768 File Offset: 0x00089968
	public string Description
	{
		get
		{
			return this.description;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06001441 RID: 5185 RVA: 0x0008B770 File Offset: 0x00089970
	public int MinimumLevel
	{
		get
		{
			return this.minLevel;
		}
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x0008B778 File Offset: 0x00089978
	public Reward GenerateReward(Simulation simulation)
	{
		return this.rewardDefinition.GenerateReward(simulation, true);
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06001443 RID: 5187 RVA: 0x0008B788 File Offset: 0x00089988
	public string Icon
	{
		get
		{
			return this.icon;
		}
	}

	// Token: 0x04000E38 RID: 3640
	public const string TYPE = "vendor_stock";

	// Token: 0x04000E39 RID: 3641
	private int did;

	// Token: 0x04000E3A RID: 3642
	private string name;

	// Token: 0x04000E3B RID: 3643
	private string description;

	// Token: 0x04000E3C RID: 3644
	private int minLevel;

	// Token: 0x04000E3D RID: 3645
	private string icon;

	// Token: 0x04000E3E RID: 3646
	private RewardDefinition rewardDefinition;

	// Token: 0x04000E3F RID: 3647
	private CdfDictionary<Cost> costs;

	// Token: 0x04000E40 RID: 3648
	private ResultGenerator instances;
}
