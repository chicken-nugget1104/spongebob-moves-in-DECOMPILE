using System;
using System.Collections.Generic;

// Token: 0x02000244 RID: 580
public class SessionActionArray : SessionActionCollection
{
	// Token: 0x060012B2 RID: 4786 RVA: 0x0008153C File Offset: 0x0007F73C
	public static SessionActionArray Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		SessionActionArray sessionActionArray = new SessionActionArray();
		sessionActionArray.Parse(data, id, startConditions, originatedFromQuest);
		return sessionActionArray;
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x0008155C File Offset: 0x0007F75C
	public override void PreActivate(Game game, SessionActionTracker action)
	{
		base.PreActivate(game, action);
		List<SessionActionTracker> dynamic = action.GetDynamic<List<SessionActionTracker>>("collection");
		foreach (SessionActionTracker sessionAction in dynamic)
		{
			game.sessionActionManager.Request(sessionAction, game);
		}
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x000815D8 File Offset: 0x0007F7D8
	public override bool ActiveProcess(Game game, SessionActionTracker action)
	{
		bool flag = false;
		bool flag2 = false;
		List<SessionActionTracker> dynamic = action.GetDynamic<List<SessionActionTracker>>("collection");
		foreach (SessionActionTracker sessionActionTracker in dynamic)
		{
			if (sessionActionTracker.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				action.MarkObliterated(game);
				flag = true;
			}
			else if (sessionActionTracker.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE)
			{
				action.MarkFailed();
				flag = true;
			}
			else if (sessionActionTracker.Status != SessionActionTracker.StatusCode.FINISHED_SUCCESS)
			{
				flag2 = true;
			}
		}
		if (!flag2 && action.Status == SessionActionTracker.StatusCode.STARTED)
		{
			action.MarkSucceeded();
			flag = true;
		}
		return base.ActiveProcess(game, action) || flag;
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x000816B8 File Offset: 0x0007F8B8
	public override void PostComplete(Game game, SessionActionTracker action)
	{
		List<SessionActionTracker> dynamic = action.GetDynamic<List<SessionActionTracker>>("collection");
		if (action.Status == SessionActionTracker.StatusCode.OBLITERATED)
		{
			dynamic.ForEach(delegate(SessionActionTracker t)
			{
				t.MarkObliterated(game);
			});
		}
		else if (action.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE)
		{
			dynamic.ForEach(delegate(SessionActionTracker t)
			{
				if (t.Status != SessionActionTracker.StatusCode.FINISHED_SUCCESS)
				{
					t.MarkFailed();
				}
			});
		}
		else if (action.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS)
		{
			dynamic.ForEach(delegate(SessionActionTracker t)
			{
				if (t.Status != SessionActionTracker.StatusCode.FINISHED_FAILURE)
				{
					t.MarkSucceeded();
				}
			});
		}
		base.PostComplete(game, action);
	}

	// Token: 0x04000CF6 RID: 3318
	public const string TYPE = "array";
}
