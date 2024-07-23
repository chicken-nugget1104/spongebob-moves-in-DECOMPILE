using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000220 RID: 544
public class AchievementUnlock : SessionActionDefinition
{
	// Token: 0x060011E8 RID: 4584 RVA: 0x0007D61C File Offset: 0x0007B81C
	public static AchievementUnlock Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		AchievementUnlock achievementUnlock = new AchievementUnlock();
		achievementUnlock.Parse(data, id, startConditions, originatedFromQuest);
		return achievementUnlock;
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x0007D63C File Offset: 0x0007B83C
	protected void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		this.achievementId = TFUtils.LoadString(data, "achievement_id");
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x0007D66C File Offset: 0x0007B86C
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dictionary = base.ToDict();
		dictionary["achievement_id"] = this.achievementId;
		return dictionary;
	}

	// Token: 0x060011EB RID: 4587 RVA: 0x0007D694 File Offset: 0x0007B894
	public override void PreActivate(Game game, SessionActionTracker action)
	{
		GameObject gameObject = GameObject.Find("SBGameCenterManager");
		SBGameCenterManager component = gameObject.GetComponent<SBGameCenterManager>();
		component.ReportAchievement(this.achievementId, 100f);
	}

	// Token: 0x04000C3F RID: 3135
	public const string TYPE = "achievement_unlock";

	// Token: 0x04000C40 RID: 3136
	private const string ACHIEVEMENT_ID = "achievement_id";

	// Token: 0x04000C41 RID: 3137
	private string achievementId;
}
