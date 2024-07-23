using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class LevelingManager : IResourceProgressCalculator
{
	// Token: 0x06000D35 RID: 3381 RVA: 0x000514E0 File Offset: 0x0004F6E0
	public LevelingManager()
	{
		this.LoadLevelingMilestones();
		this.LoadLevelingHeadlines();
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x000514F4 File Offset: 0x0004F6F4
	public int GetResourceType()
	{
		return ResourceManager.XP;
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000D37 RID: 3383 RVA: 0x000514FC File Offset: 0x0004F6FC
	public int MaxLevel
	{
		get
		{
			return this.maxLevel;
		}
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x00051504 File Offset: 0x0004F704
	public string Headline(int level)
	{
		return this.headlines[level - 2];
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x00051514 File Offset: 0x0004F714
	public string HeadlineImage(int level)
	{
		return this.headlineImages[level - 2];
	}

	// Token: 0x06000D3A RID: 3386 RVA: 0x00051524 File Offset: 0x0004F724
	public int AutoQuestLength(int level)
	{
		return this.autoQuestLengths[Mathf.Max(0, level - 2)];
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x0005153C File Offset: 0x0004F73C
	public string VoiceOver(int level)
	{
		return this.voiceOvers[level - 2];
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x0005154C File Offset: 0x0004F74C
	public List<Reward> GetLevelUpRewards(Simulation simulation, int oldLevel, int newXp)
	{
		List<Reward> list = new List<Reward>();
		for (int i = 0; i < this.milestones.Count; i++)
		{
			int num = i + 2;
			if (num > oldLevel && newXp >= this.milestones[i].xp)
			{
				list.Add(this.milestones[i].rewardDef.GenerateReward(simulation, false));
			}
		}
		return list;
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x000515BC File Offset: 0x0004F7BC
	public int GetXpRequiredForLevel(int level)
	{
		int num = level - 2;
		if (level == 1 || num >= this.milestones.Count)
		{
			return 0;
		}
		return this.milestones[num].xp;
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x000515F8 File Offset: 0x0004F7F8
	public void GetRewardsForIncreasingResource(Simulation simulation, Dictionary<int, Resource> currentResources, int amountToIncrease, out List<Reward> rewards)
	{
		int amount = currentResources[ResourceManager.LEVEL].Amount;
		int amount2 = currentResources[ResourceManager.XP].Amount;
		int newXp = amount2 + amountToIncrease;
		rewards = this.GetLevelUpRewards(simulation, amount, newXp);
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x00051638 File Offset: 0x0004F838
	public float ComputeProgressPercentage(Dictionary<int, Resource> currentResources)
	{
		int amount = currentResources[ResourceManager.XP].Amount;
		int amount2 = currentResources[ResourceManager.LEVEL].Amount;
		int xpRequiredForLevel = this.GetXpRequiredForLevel(amount2);
		int xpRequiredForLevel2 = this.GetXpRequiredForLevel(amount2 + 1);
		if (amount2 >= this.maxLevel)
		{
			return 100f;
		}
		return (float)(amount - xpRequiredForLevel) / (float)(xpRequiredForLevel2 - xpRequiredForLevel) * 100f;
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x0005169C File Offset: 0x0004F89C
	public string ComputeProgressFraction(Dictionary<int, Resource> currentResources)
	{
		int amount = currentResources[ResourceManager.XP].Amount;
		int amount2 = currentResources[ResourceManager.LEVEL].Amount;
		int xpRequiredForLevel = this.GetXpRequiredForLevel(amount2);
		int xpRequiredForLevel2 = this.GetXpRequiredForLevel(amount2 + 1);
		if (amount2 >= this.maxLevel)
		{
			return amount.ToString() + " / " + amount.ToString();
		}
		int num = amount - xpRequiredForLevel;
		int num2 = xpRequiredForLevel2 - xpRequiredForLevel;
		return num.ToString() + " / " + num2.ToString();
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x00051724 File Offset: 0x0004F924
	private void LoadLevelingMilestones()
	{
		this.milestones = new List<MilestoneMarker>();
		Dictionary<string, object> dictionary = this.LoadLevelingMilestonesFromSpread();
		List<object> list = (List<object>)dictionary["milestones"];
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
			int xp = Convert.ToInt32(dictionary2["xp"]);
			object o = dictionary2["reward"];
			RewardDefinition rewardDef = RewardDefinition.FromObject(o);
			this.milestones.Add(new MilestoneMarker(xp, rewardDef));
		}
		this.maxLevel = this.milestones.Count + 1;
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x000517FC File Offset: 0x0004F9FC
	private void LoadLevelingHeadlines()
	{
		this.headlines = new List<string>();
		this.headlineImages = new List<string>();
		this.voiceOvers = new List<string>();
		this.autoQuestLengths = new List<int>();
		Dictionary<string, object> dictionary = this.LoadLevelingHeadlinesFromSpread();
		List<object> list = (List<object>)dictionary["levels"];
		foreach (object obj in list)
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)obj;
			string item = Convert.ToString(dictionary2["headline"]);
			this.headlines.Add(item);
			string item2 = Convert.ToString(dictionary2["texture"]);
			this.headlineImages.Add(item2);
			string item3 = Convert.ToString(dictionary2["voice_over"]);
			this.voiceOvers.Add(item3);
			int item4 = Convert.ToInt32(dictionary2["auto_quest_length"]);
			this.autoQuestLengths.Add(item4);
		}
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x00051938 File Offset: 0x0004FB38
	private Dictionary<string, object> LoadLevelingMilestonesFromSpread()
	{
		string text = "Leveling";
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return dictionary;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return dictionary;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return dictionary;
		}
		dictionary.Add("type", "leveling");
		dictionary.Add("milestones", new List<object>());
		string b = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "level");
				if (intCell > 1)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2.Add("_level", intCell);
					dictionary2.Add("xp", instance.GetIntCell(sheetIndex, rowIndex, "xp"));
					dictionary2.Add("reward", new Dictionary<string, object>
					{
						{
							"thought_icon",
							null
						}
					});
					intCell = instance.GetIntCell(sheetIndex, rowIndex, "reward jelly");
					if (intCell > 0)
					{
						((Dictionary<string, object>)dictionary2["reward"]).Add("resources", new Dictionary<string, object>
						{
							{
								"2",
								intCell
							}
						});
					}
					intCell = instance.GetIntCell(sheetIndex, rowIndex, "reward gold");
					if (intCell > 0)
					{
						if (!((Dictionary<string, object>)dictionary2["reward"]).ContainsKey("resources"))
						{
							((Dictionary<string, object>)dictionary2["reward"]).Add("resources", new Dictionary<string, object>());
						}
						((Dictionary<string, object>)((Dictionary<string, object>)dictionary2["reward"])["resources"]).Add("3", intCell);
					}
					intCell = instance.GetIntCell(sheetIndex, rowIndex, "reward special amount");
					if (intCell > 0)
					{
						string stringCell = instance.GetStringCell(text, rowName, "reward special type");
						if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
						{
							if (!((Dictionary<string, object>)dictionary2["reward"]).ContainsKey(stringCell))
							{
								((Dictionary<string, object>)dictionary2["reward"]).Add(stringCell, new Dictionary<string, object>());
							}
							((Dictionary<string, object>)((Dictionary<string, object>)dictionary2["reward"])[stringCell]).Add(instance.GetIntCell(sheetIndex, rowIndex, "reward special did").ToString(), intCell);
						}
					}
					((List<object>)dictionary["milestones"]).Add(dictionary2);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x00051C54 File Offset: 0x0004FE54
	private Dictionary<string, object> LoadLevelingHeadlinesFromSpread()
	{
		string text = "Leveling";
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		DatabaseManager instance = DatabaseManager.Instance;
		if (instance == null || string.IsNullOrEmpty(text))
		{
			return dictionary;
		}
		int sheetIndex = instance.GetSheetIndex(text);
		if (sheetIndex < 0)
		{
			TFUtils.ErrorLog("Cannot find database with sheet name: " + text);
			return dictionary;
		}
		int num = instance.GetNumRows(text);
		if (num <= 0)
		{
			TFUtils.ErrorLog("No rows in sheet name: " + text);
			return dictionary;
		}
		dictionary.Add("type", "leveling");
		dictionary.Add("levels", new List<object>());
		string b = "n/a";
		for (int i = 0; i < num; i++)
		{
			string rowName = i.ToString();
			if (!instance.HasRow(sheetIndex, rowName))
			{
				num++;
			}
			else
			{
				int rowIndex = instance.GetRowIndex(sheetIndex, instance.GetIntCell(text, rowName, "id").ToString());
				int intCell = instance.GetIntCell(sheetIndex, rowIndex, "level");
				if (intCell > 1)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2.Add("_level", intCell);
					dictionary2.Add("auto_quest_length", instance.GetIntCell(sheetIndex, rowIndex, "max autoquest length"));
					string stringCell = instance.GetStringCell(text, rowName, "headline");
					if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
					{
						dictionary2.Add("headline", stringCell);
					}
					stringCell = instance.GetStringCell(text, rowName, "texture");
					if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
					{
						dictionary2.Add("texture", stringCell);
					}
					stringCell = instance.GetStringCell(text, rowName, "voice over");
					if (!string.IsNullOrEmpty(stringCell) && stringCell != b)
					{
						dictionary2.Add("voice_over", stringCell);
					}
					((List<object>)dictionary["levels"]).Add(dictionary2);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x040008DD RID: 2269
	private List<MilestoneMarker> milestones;

	// Token: 0x040008DE RID: 2270
	private List<string> headlines;

	// Token: 0x040008DF RID: 2271
	private List<string> headlineImages;

	// Token: 0x040008E0 RID: 2272
	private List<string> voiceOvers;

	// Token: 0x040008E1 RID: 2273
	private List<int> autoQuestLengths;

	// Token: 0x040008E2 RID: 2274
	private int maxLevel;
}
