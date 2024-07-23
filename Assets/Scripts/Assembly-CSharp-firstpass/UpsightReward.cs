using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x020000D3 RID: 211
public class UpsightReward
{
	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600084D RID: 2125 RVA: 0x0001F28C File Offset: 0x0001D48C
	// (set) Token: 0x0600084E RID: 2126 RVA: 0x0001F294 File Offset: 0x0001D494
	public string name { get; private set; }

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600084F RID: 2127 RVA: 0x0001F2A0 File Offset: 0x0001D4A0
	// (set) Token: 0x06000850 RID: 2128 RVA: 0x0001F2A8 File Offset: 0x0001D4A8
	public int quantity { get; private set; }

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000851 RID: 2129 RVA: 0x0001F2B4 File Offset: 0x0001D4B4
	// (set) Token: 0x06000852 RID: 2130 RVA: 0x0001F2BC File Offset: 0x0001D4BC
	public string receipt { get; private set; }

	// Token: 0x06000853 RID: 2131 RVA: 0x0001F2C8 File Offset: 0x0001D4C8
	public static UpsightReward rewardFromJson(string json)
	{
		UpsightReward upsightReward = new UpsightReward();
		Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
		if (dictionary != null)
		{
			if (dictionary.ContainsKey("name"))
			{
				upsightReward.name = dictionary["name"].ToString();
			}
			if (dictionary.ContainsKey("quantity"))
			{
				upsightReward.quantity = int.Parse(dictionary["quantity"].ToString());
			}
			if (dictionary.ContainsKey("receipt"))
			{
				upsightReward.receipt = dictionary["receipt"].ToString();
			}
		}
		return upsightReward;
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0001F368 File Offset: 0x0001D568
	public override string ToString()
	{
		return string.Format("[UpsightReward: name={0}, quantity={1}, receipt={2}]", this.name, this.quantity, this.receipt);
	}
}
