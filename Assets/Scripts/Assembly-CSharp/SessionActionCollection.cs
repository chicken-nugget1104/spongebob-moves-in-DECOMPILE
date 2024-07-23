using System;
using System.Collections.Generic;

// Token: 0x02000245 RID: 581
public abstract class SessionActionCollection : SessionActionDefinition
{
	// Token: 0x17000262 RID: 610
	// (get) Token: 0x060012B9 RID: 4793 RVA: 0x000817A4 File Offset: 0x0007F9A4
	public ICollection<SessionActionDefinition> Collection
	{
		get
		{
			return this.collection;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x060012BA RID: 4794 RVA: 0x000817AC File Offset: 0x0007F9AC
	public override bool ClearOnSessionChange
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x000817B0 File Offset: 0x0007F9B0
	protected virtual void Parse(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		base.Parse(data, id, startConditions, new DumbCondition(0U), originatedFromQuest);
		List<object> list = TFUtils.LoadList<object>(data, "actions");
		this.collection = new List<SessionActionDefinition>();
		foreach (object obj in list)
		{
			Dictionary<string, object> data2 = (Dictionary<string, object>)obj;
			this.collection.Add(SessionActionFactory.Create(data2, new ConstantCondition(0U, true), originatedFromQuest, id++));
		}
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x0008185C File Offset: 0x0007FA5C
	public override void SetDynamicProperties(ref Dictionary<string, object> propertiesDict)
	{
		propertiesDict["collection"] = new List<SessionActionTracker>();
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x00081870 File Offset: 0x0007FA70
	public override void PreActivate(Game game, SessionActionTracker action)
	{
		List<SessionActionTracker> list = new List<SessionActionTracker>();
		foreach (SessionActionDefinition definition in this.Collection)
		{
			list.Add(new SessionActionTracker(definition, new ConstantCondition(0U, true), action.Tag, false));
		}
		action.SetDynamic("collection", list);
		action.MarkStarted();
		base.PreActivate(game, action);
	}

	// Token: 0x04000CF9 RID: 3321
	public const string COLLECTION = "collection";

	// Token: 0x04000CFA RID: 3322
	private const string ACTIONS = "actions";

	// Token: 0x04000CFB RID: 3323
	private ICollection<SessionActionDefinition> collection;
}
