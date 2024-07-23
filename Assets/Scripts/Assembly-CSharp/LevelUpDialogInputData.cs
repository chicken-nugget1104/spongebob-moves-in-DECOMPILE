using System;
using System.Collections.Generic;

// Token: 0x02000165 RID: 357
public class LevelUpDialogInputData : PersistedDialogInputData
{
	// Token: 0x06000C33 RID: 3123 RVA: 0x00049B58 File Offset: 0x00047D58
	public LevelUpDialogInputData(int newLevel, List<Reward> rewards) : base(uint.MaxValue, "level_up", "Dialog_LevelUp", null)
	{
		this.newLevel = newLevel;
		this.rewards = rewards;
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00049B88 File Offset: 0x00047D88
	public int NewLevel
	{
		get
		{
			return this.newLevel;
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00049B90 File Offset: 0x00047D90
	public List<Reward> Rewards
	{
		get
		{
			return this.rewards;
		}
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x00049B98 File Offset: 0x00047D98
	public override Dictionary<string, object> ToPersistenceDict()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["type"] = "level_up";
		dictionary["level"] = this.newLevel;
		List<object> list = new List<object>();
		foreach (Reward reward in this.rewards)
		{
			list.Add(Reward.RewardToDict(reward));
		}
		dictionary["rewards"] = list;
		return dictionary;
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x00049C44 File Offset: 0x00047E44
	public new static LevelUpDialogInputData FromPersistenceDict(Dictionary<string, object> dict)
	{
		int num = TFUtils.LoadInt(dict, "level");
		List<object> list = (List<object>)dict["rewards"];
		List<Reward> list2 = new List<Reward>();
		foreach (object o in list)
		{
			list2.Add(Reward.FromObject(o));
		}
		return new LevelUpDialogInputData(num, list2);
	}

	// Token: 0x0400082E RID: 2094
	public const string DIALOG_TYPE = "level_up";

	// Token: 0x0400082F RID: 2095
	private int newLevel;

	// Token: 0x04000830 RID: 2096
	private List<Reward> rewards;
}
