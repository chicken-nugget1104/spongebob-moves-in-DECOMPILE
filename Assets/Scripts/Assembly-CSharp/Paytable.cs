using System;
using System.Collections.Generic;

// Token: 0x020001BC RID: 444
public class Paytable
{
	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000F2C RID: 3884 RVA: 0x00060B34 File Offset: 0x0005ED34
	public uint Did
	{
		get
		{
			return this.did;
		}
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x00060B3C File Offset: 0x0005ED3C
	public static Paytable FromDict(Dictionary<string, object> data)
	{
		Paytable paytable = new Paytable();
		paytable.did = TFUtils.LoadUint(data, "did");
		paytable.wagers = new Dictionary<uint, RewardDefinition>();
		Dictionary<string, object> dictionary = TFUtils.LoadDict(data, "wagers");
		foreach (string text in dictionary.Keys)
		{
			paytable.wagers[uint.Parse(text)] = RewardDefinition.FromObject(dictionary[text]);
		}
		return paytable;
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x00060BE8 File Offset: 0x0005EDE8
	public Paytable Join(Paytable that)
	{
		Paytable paytable = new Paytable();
		paytable.wagers = new Dictionary<uint, RewardDefinition>(this.wagers);
		if (that != null)
		{
			foreach (uint key in that.wagers.Keys)
			{
				if (paytable.wagers.ContainsKey(key))
				{
					paytable.wagers[key] = paytable.wagers[key].Join(that.wagers[key]);
				}
				else
				{
					paytable.wagers.Add(key, that.wagers[key]);
				}
			}
		}
		return paytable;
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x00060CC4 File Offset: 0x0005EEC4
	public void Normalize()
	{
		foreach (RewardDefinition rewardDefinition in this.wagers.Values)
		{
			rewardDefinition.Normalize();
		}
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00060D30 File Offset: 0x0005EF30
	public bool CanWager(uint wager)
	{
		return this.wagers.ContainsKey(wager);
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x00060D40 File Offset: 0x0005EF40
	public void ValidateProbabilities()
	{
		foreach (RewardDefinition rewardDefinition in this.wagers.Values)
		{
			rewardDefinition.Validate(true);
		}
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x00060DAC File Offset: 0x0005EFAC
	public Reward Spin(uint wager, Simulation simulation, Reward consolationReward)
	{
		if (this.wagers.ContainsKey(wager))
		{
			return this.wagers[wager].GenerateReward(simulation, consolationReward, false, false);
		}
		return null;
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x00060DE4 File Offset: 0x0005EFE4
	public Reward Spin(int wager, Simulation simulation, Reward consolationReward)
	{
		if (this.wagers.ContainsKey((uint)wager))
		{
			return this.wagers[(uint)wager].GenerateReward(simulation, consolationReward, false, false);
		}
		return null;
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x00060E1C File Offset: 0x0005F01C
	public override string ToString()
	{
		string text = "[Paytable (wagers=[\n";
		foreach (KeyValuePair<uint, RewardDefinition> keyValuePair in this.wagers)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"  wager(",
				keyValuePair.Key,
				"): "
			});
			text = text + keyValuePair.Value + "\n";
		}
		text += "] )]";
		return text;
	}

	// Token: 0x04000A35 RID: 2613
	private const string DID = "did";

	// Token: 0x04000A36 RID: 2614
	private const string WAGERS = "wagers";

	// Token: 0x04000A37 RID: 2615
	private const uint DYNAMIC_ID = 0U;

	// Token: 0x04000A38 RID: 2616
	public static Reward CONSOLATION_REWARD = new Reward(new Dictionary<int, int>
	{
		{
			ResourceManager.SOFT_CURRENCY,
			5
		}
	}, null, null, null, null, null, null, null, false, null);

	// Token: 0x04000A39 RID: 2617
	public static Reward CONSOLATION_REWARD_HALLOWEEN = new Reward(new Dictionary<int, int>
	{
		{
			ResourceManager.HALLOWEEN_CURRENCY,
			1
		}
	}, null, null, null, null, null, null, null, false, null);

	// Token: 0x04000A3A RID: 2618
	private uint did;

	// Token: 0x04000A3B RID: 2619
	private Dictionary<uint, RewardDefinition> wagers;
}
