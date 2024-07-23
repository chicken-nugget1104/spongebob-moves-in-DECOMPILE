using System;
using System.Collections.Generic;

// Token: 0x020000ED RID: 237
public abstract class QuestAction : PersistedTriggerableAction
{
	// Token: 0x060008A0 RID: 2208 RVA: 0x00037680 File Offset: 0x00035880
	public QuestAction(string type, uint questId, ulong? startTime, ulong? completionTime) : base(type, Identity.Null())
	{
		this.questId = questId;
		this.startTime = startTime;
		this.completionTime = completionTime;
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x000376B0 File Offset: 0x000358B0
	public QuestAction(string type, Quest quest) : base(type, Identity.Null())
	{
		this.questId = quest.Did;
		this.startTime = quest.StartTime;
		this.completionTime = quest.CompletionTime;
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060008A2 RID: 2210 RVA: 0x000376F0 File Offset: 0x000358F0
	public TriggerableMixin Triggerable
	{
		get
		{
			return this.triggerable;
		}
	}

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060008A3 RID: 2211 RVA: 0x000376F8 File Offset: 0x000358F8
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x000376FC File Offset: 0x000358FC
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dest = base.ToDict();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["did"] = this.questId;
		dictionary["start_time"] = TFUtils.NullableToObject(this.startTime);
		dictionary["completion_time"] = TFUtils.NullableToObject(this.completionTime);
		return TFUtils.ConcatenateDictionaryInPlace<string, object>(dest, dictionary);
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00037760 File Offset: 0x00035960
	public override void Process(Game game)
	{
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00037764 File Offset: 0x00035964
	public override void Apply(Game game, ulong utcNow)
	{
		Quest quest = game.questManager.GetQuest(this.questId);
		if (quest != null)
		{
			if (this.startTime != null)
			{
				quest.Start(this.startTime.Value);
			}
			if (this.completionTime != null)
			{
				quest.Complete(this.completionTime.Value);
			}
			game.questManager.RegisterQuest(game, quest);
		}
		else
		{
			TFUtils.WarningLog("QuestAction.Apply - Quest " + this.questId + " does not exist in the questList.");
		}
		base.Apply(game, utcNow);
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00037804 File Offset: 0x00035A04
	public override void Confirm(Dictionary<string, object> gameState)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)gameState["farm"];
		List<object> list = (List<object>)dictionary["quests"];
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> data = (Dictionary<string, object>)list[i];
			uint num2 = TFUtils.LoadUint(data, "did");
			if (num2 == this.questId)
			{
				num++;
				if (num == 1)
				{
					Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list[i];
					dictionary2["start_time"] = this.startTime;
					dictionary2["completion_time"] = this.completionTime;
					dictionary2["reminded"] = true;
				}
			}
		}
		if (num == 0)
		{
			TFUtils.Assert(false, "Missing a quest " + this.questId + " - should be generated in QuestProgressAction");
		}
		else if (num > 1)
		{
			TFUtils.Assert(false, "Too many quests match did " + this.questId);
		}
		base.Confirm(gameState);
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00037924 File Offset: 0x00035B24
	protected virtual void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data["quest_id"] = (int)this.questId;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00037940 File Offset: 0x00035B40
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x0400062A RID: 1578
	protected uint questId;

	// Token: 0x0400062B RID: 1579
	protected ulong? startTime;

	// Token: 0x0400062C RID: 1580
	protected ulong? completionTime;
}
