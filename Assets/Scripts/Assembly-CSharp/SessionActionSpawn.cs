using System;

// Token: 0x0200024C RID: 588
public abstract class SessionActionSpawn : ISessionActionSpawn
{
	// Token: 0x1700026F RID: 623
	// (get) Token: 0x060012F5 RID: 4853 RVA: 0x0008349C File Offset: 0x0008169C
	public SessionActionTracker ParentAction
	{
		get
		{
			return this.parentAction;
		}
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x000834A4 File Offset: 0x000816A4
	protected virtual void RegisterNewInstance(Game game, SessionActionTracker parentAction)
	{
		this.parentAction = parentAction;
		if (parentAction.Status == SessionActionTracker.StatusCode.REQUESTED)
		{
			this.parentAction.MarkStarted();
			game.sessionActionManager.RequestProcess(game);
		}
		game.sessionActionManager.RegisterSpawn(this);
		if (this.ToString() == "GuideArrow")
		{
			game.simulation.soundEffectManager.PlaySound("tutorial_arrow");
		}
		if (this.parentAction.Definition.Sound != null)
		{
			game.simulation.soundEffectManager.PlaySound(this.parentAction.Definition.Sound);
		}
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x00083548 File Offset: 0x00081748
	public virtual SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		if (this.parentAction.Status == SessionActionTracker.StatusCode.FINISHED_SUCCESS || this.parentAction.Status == SessionActionTracker.StatusCode.FINISHED_FAILURE || this.parentAction.Status == SessionActionTracker.StatusCode.OBLITERATED)
		{
			this.Destroy();
			return SessionActionManager.SpawnReturnCode.KILL;
		}
		return SessionActionManager.SpawnReturnCode.KEEP_ALIVE;
	}

	// Token: 0x060012F8 RID: 4856
	public abstract void Destroy();

	// Token: 0x04000D1B RID: 3355
	protected SessionActionTracker parentAction;
}
