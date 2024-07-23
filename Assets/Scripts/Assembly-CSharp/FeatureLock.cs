using System;
using System.Collections.Generic;

// Token: 0x0200016F RID: 367
public class FeatureLock
{
	// Token: 0x06000C5E RID: 3166 RVA: 0x0004A6B8 File Offset: 0x000488B8
	public FeatureLock(Dictionary<string, object> data)
	{
		this.feature = TFUtils.LoadString(data, "feature");
		TFUtils.Assert(Features.FeatureSet.Contains(this.feature), "Unknown Feature found " + this.feature + " in FeatureLocks.");
		if (data.ContainsKey("unlock_action"))
		{
			this.unlockActionData = TFUtils.LoadDict(data, "unlock_action");
		}
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000C5F RID: 3167 RVA: 0x0004A728 File Offset: 0x00048928
	public string Feature
	{
		get
		{
			return this.feature;
		}
	}

	// Token: 0x06000C60 RID: 3168 RVA: 0x0004A730 File Offset: 0x00048930
	public void Activate(Game game)
	{
		SessionActionDefinition definition = SessionActionFactory.Create(this.unlockActionData, new ConstantCondition(0U, true));
		SessionActionTracker sessionAction = new SessionActionTracker(definition);
		game.sessionActionManager.Request(sessionAction, game);
	}

	// Token: 0x0400084B RID: 2123
	private string feature;

	// Token: 0x0400084C RID: 2124
	private Dictionary<string, object> unlockActionData;
}
