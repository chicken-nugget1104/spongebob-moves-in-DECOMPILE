using System;
using System.Collections.Generic;

// Token: 0x020000C5 RID: 197
public class AutoQuestCleanupAction : PersistedTriggerableAction
{
	// Token: 0x0600077C RID: 1916 RVA: 0x0003178C File Offset: 0x0002F98C
	private AutoQuestCleanupAction(uint uQuestId) : base("au", Identity.Null())
	{
		this.m_uQuestId = uQuestId;
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x000317BC File Offset: 0x0002F9BC
	public AutoQuestCleanupAction(Quest pQuest) : this(pQuest.Did)
	{
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x0600077E RID: 1918 RVA: 0x000317CC File Offset: 0x0002F9CC
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x000317D0 File Offset: 0x0002F9D0
	public new static AutoQuestCleanupAction FromDict(Dictionary<string, object> pData)
	{
		uint uQuestId = TFUtils.LoadUint(pData, "did");
		return new AutoQuestCleanupAction(uQuestId);
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x000317F4 File Offset: 0x0002F9F4
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["did"] = this.m_uQuestId;
		return dictionary;
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00031820 File Offset: 0x0002FA20
	public override void Process(Game pGame)
	{
		pGame.questManager.DeactivateQuest(pGame, QuestDefinition.LastAutoQuestId);
		base.Process(pGame);
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0003183C File Offset: 0x0002FA3C
	public override void Confirm(Dictionary<string, object> pGameState)
	{
		List<object> list = (List<object>)((Dictionary<string, object>)pGameState["farm"])["quests"];
		Predicate<object> match = delegate(object pObj)
		{
			uint num3 = uint.Parse(((Dictionary<string, object>)pObj)["did"].ToString());
			return num3 == this.m_uQuestId;
		};
		list.Remove(list.Find(match));
		Dictionary<string, object> dictionary = (Dictionary<string, object>)pGameState["farm"];
		List<object> list2 = (List<object>)dictionary["generated_quest_definition"];
		int num = -1;
		for (int i = 0; i < list2.Count; i++)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)list2[i];
			uint num2 = TFUtils.LoadUint(data, "did");
			if (this.m_uQuestId == num2)
			{
				num = i;
			}
		}
		if (num != -1)
		{
			list2.RemoveAt(num);
		}
		base.Confirm(pGameState);
	}

	// Token: 0x0400059F RID: 1439
	public const string QUEST_CLEANUP = "au";

	// Token: 0x040005A0 RID: 1440
	private uint m_uQuestId = 500001U;
}
