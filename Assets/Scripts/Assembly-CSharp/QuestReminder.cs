using System;
using System.Collections.Generic;

// Token: 0x0200023D RID: 573
public class QuestReminder : UiTargetingSessionActionDefinition
{
	// Token: 0x06001280 RID: 4736 RVA: 0x0007FDE0 File Offset: 0x0007DFE0
	public static QuestReminder Create(Dictionary<string, object> data, uint id, ICondition startingConditions, uint originatedFromQuest)
	{
		TFUtils.Assert(!data.ContainsKey("target"), "QuestReminders cannot specify a target!");
		TFUtils.Assert(originatedFromQuest != 0U, "QuestReminders can only be originated from quests! This one wasn't given a quest Did.");
		QuestReminder questReminder = new QuestReminder();
		questReminder.Parse(data, id, startingConditions, originatedFromQuest, QuestDefinition.GenerateSessionActionId(originatedFromQuest));
		questReminder.questID = originatedFromQuest;
		if (data.ContainsKey("bar_texture"))
		{
			questReminder.barTexture = TFUtils.LoadString(data, "bar_texture");
		}
		if (data.ContainsKey("circle_texture"))
		{
			questReminder.circleTexture = TFUtils.LoadString(data, "circle_texture");
		}
		return questReminder;
	}

	// Token: 0x06001281 RID: 4737 RVA: 0x0007FE78 File Offset: 0x0007E078
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> result = base.ToDict();
		this.banner.AddToDict(ref result);
		return result;
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x0007FE9C File Offset: 0x0007E09C
	public override void Handle(Session session, SessionActionTracker action, SBGUIElement target, SBGUIScreen containingScreen)
	{
		if (action.Status != SessionActionTracker.StatusCode.REQUESTED)
		{
			return;
		}
		Quest quest = session.TheGame.questManager.GetQuest(this.questID);
		if (quest == null)
		{
			return;
		}
		SBGUIStandardScreen screen = (SBGUIStandardScreen)session.CheckAsyncRequest("standard_screen");
		session.AddAsyncResponse("standard_screen", screen);
		Action clickHandler = delegate()
		{
			screen.TryFireQuestStatusEvent(session, (int)this.questID);
		};
		if (quest.TriggeredReminder)
		{
			return;
		}
		quest.TriggeredReminder = true;
		this.banner.Spawn(session.TheGame, action, target, containingScreen, clickHandler, this.barTexture, this.circleTexture);
	}

	// Token: 0x04000CB3 RID: 3251
	public const string TYPE = "quest_reminder";

	// Token: 0x04000CB4 RID: 3252
	private QuestReminderBanner banner = new QuestReminderBanner();

	// Token: 0x04000CB5 RID: 3253
	private uint questID;

	// Token: 0x04000CB6 RID: 3254
	private string barTexture;

	// Token: 0x04000CB7 RID: 3255
	private string circleTexture;
}
