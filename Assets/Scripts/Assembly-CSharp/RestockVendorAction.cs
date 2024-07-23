using System;
using System.Collections.Generic;

// Token: 0x020000F5 RID: 245
public class RestockVendorAction : PersistedSimulatedAction
{
	// Token: 0x060008D5 RID: 2261 RVA: 0x000385CC File Offset: 0x000367CC
	public RestockVendorAction(Identity id, ulong restockTime, ulong specialRestockTime, Dictionary<string, object> generalInstances, Dictionary<string, object> specialInstances) : base("vr", id, typeof(RestockVendorAction).ToString())
	{
		this.nextRestock = restockTime;
		this.nextSpecialRestock = specialRestockTime;
		this.generalInstances = generalInstances;
		this.specialInstances = specialInstances;
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00038608 File Offset: 0x00036808
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0003860C File Offset: 0x0003680C
	public static RestockVendorAction Create(Identity id, ulong restockTime, ulong specialRestockTime, Dictionary<int, VendingInstance> generalInstances, Dictionary<int, VendingInstance> specialInstances)
	{
		Dictionary<string, object> dictionary;
		if (generalInstances != null)
		{
			dictionary = new Dictionary<string, object>(generalInstances.Count);
			foreach (KeyValuePair<int, VendingInstance> keyValuePair in generalInstances)
			{
				dictionary.Add(keyValuePair.Key.ToString(), keyValuePair.Value.ToDict());
			}
		}
		else
		{
			dictionary = new Dictionary<string, object>();
		}
		Dictionary<string, object> dictionary2;
		if (specialInstances != null)
		{
			dictionary2 = new Dictionary<string, object>(specialInstances.Count);
			foreach (KeyValuePair<int, VendingInstance> keyValuePair2 in specialInstances)
			{
				dictionary2.Add(keyValuePair2.Key.ToString(), keyValuePair2.Value.ToDict());
			}
		}
		else
		{
			dictionary2 = new Dictionary<string, object>();
		}
		return new RestockVendorAction(id, restockTime, specialRestockTime, dictionary, dictionary2);
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x00038740 File Offset: 0x00036940
	public new static RestockVendorAction FromDict(Dictionary<string, object> data)
	{
		Identity id = new Identity((string)data["target"]);
		ulong restockTime = TFUtils.LoadUlong(data, "general_restock", 0UL);
		ulong specialRestockTime = TFUtils.LoadUlong(data, "special_restock", 0UL);
		Dictionary<string, object> dictionary;
		if (data.ContainsKey("general_instances"))
		{
			dictionary = TFUtils.LoadDict(data, "general_instances");
		}
		else
		{
			dictionary = new Dictionary<string, object>();
		}
		Dictionary<string, object> dictionary2;
		if (data.ContainsKey("special_instances"))
		{
			dictionary2 = TFUtils.LoadDict(data, "special_instances");
		}
		else
		{
			dictionary2 = new Dictionary<string, object>();
		}
		return new RestockVendorAction(id, restockTime, specialRestockTime, dictionary, dictionary2);
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x000387E0 File Offset: 0x000369E0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["general_restock"] = this.nextRestock;
		if (this.generalInstances != null)
		{
			dictionary["general_instances"] = this.generalInstances;
		}
		dictionary["special_restock"] = this.nextSpecialRestock;
		if (this.specialInstances != null)
		{
			dictionary["special_instances"] = this.specialInstances;
		}
		return dictionary;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0003885C File Offset: 0x00036A5C
	public override void Apply(Game game, ulong utcNow)
	{
		VendingDecorator vendingDecorator = null;
		Simulated simulated = game.simulation.FindSimulated(this.target);
		if (simulated != null)
		{
			vendingDecorator = simulated.GetEntity<VendingDecorator>();
		}
		if (vendingDecorator != null)
		{
			vendingDecorator.RestockTime = this.nextRestock;
			vendingDecorator.SpecialRestockTime = this.nextSpecialRestock;
		}
		game.vendingManager.LoadVendorInstances(this.target, this.generalInstances, this.specialInstances);
		base.Apply(game, utcNow);
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x000388D0 File Offset: 0x00036AD0
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["vending"];
		string targetString = this.target.Describe();
		Predicate<object> match = (object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString);
		Dictionary<string, object> dictionary = (Dictionary<string, object>)list.Find(match);
		if (dictionary == null)
		{
			dictionary = new Dictionary<string, object>
			{
				{
					"label",
					targetString
				}
			};
			list.Add(dictionary);
		}
		if (this.generalInstances != null)
		{
			dictionary["general_instances"] = this.generalInstances;
		}
		if (this.specialInstances != null)
		{
			dictionary["special_instances"] = this.specialInstances;
		}
		List<object> list2 = (List<object>)((Dictionary<string, object>)gameState["farm"])["buildings"];
		match = ((object b) => ((string)((Dictionary<string, object>)b)["label"]).Equals(targetString));
		Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list2.Find(match);
		if (dictionary2 != null)
		{
			dictionary2["general_restock"] = this.nextRestock;
			dictionary2["special_restock"] = this.nextSpecialRestock;
		}
		base.Confirm(gameState);
	}

	// Token: 0x04000640 RID: 1600
	public const string VENDOR_RESTOCK = "vr";

	// Token: 0x04000641 RID: 1601
	private Dictionary<string, object> generalInstances;

	// Token: 0x04000642 RID: 1602
	private Dictionary<string, object> specialInstances;

	// Token: 0x04000643 RID: 1603
	private ulong nextRestock;

	// Token: 0x04000644 RID: 1604
	private ulong nextSpecialRestock;
}
