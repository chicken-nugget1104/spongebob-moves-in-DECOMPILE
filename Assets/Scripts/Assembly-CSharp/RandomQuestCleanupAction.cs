using System;
using System.Collections.Generic;

// Token: 0x020000F2 RID: 242
public class RandomQuestCleanupAction : PersistedTriggerableAction
{
	// Token: 0x060008BE RID: 2238 RVA: 0x00037FF8 File Offset: 0x000361F8
	private RandomQuestCleanupAction(uint questId) : base("ru", Identity.Null())
	{
		this.questId = questId;
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x00038028 File Offset: 0x00036228
	public RandomQuestCleanupAction(Quest quest) : this(quest.Did)
	{
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00038038 File Offset: 0x00036238
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x0003803C File Offset: 0x0003623C
	public new static RandomQuestCleanupAction FromDict(Dictionary<string, object> data)
	{
		uint num = TFUtils.LoadUint(data, "did");
		return new RandomQuestCleanupAction(num);
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00038060 File Offset: 0x00036260
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = this.questId;
		return dictionary;
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0003808C File Offset: 0x0003628C
	public override void Process(Game game)
	{
		game.questManager.DeactivateQuest(game, QuestDefinition.LastRandomQuestId);
		base.Process(game);
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x000380A8 File Offset: 0x000362A8
	public override void Confirm(Dictionary<string, object> gameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)gameState["farm"])["quests"];
		Predicate<object> match = delegate(object b)
		{
			uint num3 = uint.Parse(((Dictionary<string, object>)b)["did"].ToString());
			return num3 == this.questId;
		};
		list.Remove(list.Find(match));
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		List<object> list2 = (List<object>)dictionary["generated_quest_definition"];
		int num = -1;
		for (int i = 0; i < list2.Count; i++)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)list2[i];
			uint num2 = TFUtils.LoadUint(data, "did");
			if (this.questId == num2)
			{
				num = i;
			}
		}
		if (num != -1)
		{
			list2.RemoveAt(num);
		}
		base.Confirm(gameState);
	}

	// Token: 0x04000638 RID: 1592
	public const string QUEST_CLEANUP = "ru";

	// Token: 0x04000639 RID: 1593
	private uint questId = 400000U;
}
