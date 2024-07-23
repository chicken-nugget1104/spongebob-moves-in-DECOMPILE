using System;
using System.Collections.Generic;

// Token: 0x0200024A RID: 586
public class SessionActionSequence : SessionActionCollection
{
	// Token: 0x060012EA RID: 4842 RVA: 0x00082A74 File Offset: 0x00080C74
	public static SessionActionSequence Create(Dictionary<string, object> data, uint id, ICondition startConditions, uint originatedFromQuest)
	{
		SessionActionSequence sessionActionSequence = new SessionActionSequence();
		sessionActionSequence.Parse(data, id, startConditions, originatedFromQuest);
		return sessionActionSequence;
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x00082A94 File Offset: 0x00080C94
	public override void SetDynamicProperties(ref Dictionary<string, object> propertiesDict)
	{
		propertiesDict["step"] = 0;
		base.SetDynamicProperties(ref propertiesDict);
	}

	// Token: 0x060012EC RID: 4844 RVA: 0x00082AB0 File Offset: 0x00080CB0
	public override bool ActiveProcess(Game game, SessionActionTracker action)
	{
		bool result = false;
		if (action.Definition.Type == "sequence")
		{
			List<SessionActionTracker> dynamic = action.GetDynamic<List<SessionActionTracker>>("collection");
			if (action.Status == SessionActionTracker.StatusCode.OBLITERATED)
			{
				this.ObliterateAllSteps(ref dynamic, game);
			}
			if (action.Status == SessionActionTracker.StatusCode.STARTED)
			{
				int num = action.GetDynamic<int>("step");
				bool flag = true;
				while (flag)
				{
					flag = false;
					if (num >= dynamic.Count)
					{
						action.MarkSucceeded();
						result = true;
					}
					else
					{
						SessionActionTracker sessionActionTracker = dynamic[num];
						switch (sessionActionTracker.Status)
						{
						case SessionActionTracker.StatusCode.INITIAL:
							game.sessionActionManager.Request(sessionActionTracker, game);
							result = true;
							break;
						case SessionActionTracker.StatusCode.FINISHED_SUCCESS:
							num++;
							flag = true;
							result = true;
							break;
						case SessionActionTracker.StatusCode.FINISHED_FAILURE:
							this.ObliterateAllSteps(ref dynamic, game);
							action.MarkFailed();
							result = true;
							break;
						}
					}
				}
				action.SetDynamic("step", num);
			}
		}
		return result;
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x00082BBC File Offset: 0x00080DBC
	public override void OnObliterate(Game game, SessionActionTracker tracker)
	{
		this.ActiveProcess(game, tracker);
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x00082BC8 File Offset: 0x00080DC8
	private void ObliterateAllSteps(ref List<SessionActionTracker> steps, Game game)
	{
		foreach (SessionActionTracker sessionActionTracker in steps)
		{
			game.sessionActionManager.Obliterate(sessionActionTracker.Definition, game);
		}
	}

	// Token: 0x04000D18 RID: 3352
	public const string TYPE = "sequence";

	// Token: 0x04000D19 RID: 3353
	public const string STEP = "step";
}
