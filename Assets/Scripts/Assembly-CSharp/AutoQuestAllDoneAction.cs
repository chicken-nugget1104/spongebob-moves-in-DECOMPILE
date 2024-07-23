using System;
using System.Collections.Generic;

// Token: 0x020000C4 RID: 196
public class AutoQuestAllDoneAction : PersistedTriggerableAction
{
	// Token: 0x06000774 RID: 1908 RVA: 0x000316C0 File Offset: 0x0002F8C0
	public AutoQuestAllDoneAction(uint questId) : base("aqad", Identity.Null())
	{
		this.questId = questId;
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000775 RID: 1909 RVA: 0x000316DC File Offset: 0x0002F8DC
	public override bool IsUserInitiated
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x000316E0 File Offset: 0x0002F8E0
	public override Dictionary<string, object> ToDict()
	{
		Dictionary<string, object> dest = base.ToDict();
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["did"] = this.questId;
		return TFUtils.ConcatenateDictionaryInPlace<string, object>(dest, dictionary);
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00031718 File Offset: 0x0002F918
	public override void Process(Game game)
	{
		base.Process(game);
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00031724 File Offset: 0x0002F924
	public override void Apply(Game game, ulong utcNow)
	{
		base.Apply(game, utcNow);
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00031730 File Offset: 0x0002F930
	public override void Confirm(Dictionary<string, object> gameState)
	{
		base.Confirm(gameState);
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0003173C File Offset: 0x0002F93C
	protected void AddMoreDataToTrigger(ref Dictionary<string, object> data)
	{
		data["quest_id"] = (int)this.questId;
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00031758 File Offset: 0x0002F958
	public override ITrigger CreateTrigger(Dictionary<string, object> data)
	{
		return this.triggerable.BuildTrigger(base.GetType().ToString(), new TriggerableMixin.AddDataCallback(this.AddMoreDataToTrigger), null, null);
	}

	// Token: 0x0400059D RID: 1437
	public const string AUTO_QUEST_ALL_DONE = "aqad";

	// Token: 0x0400059E RID: 1438
	protected uint questId;
}
