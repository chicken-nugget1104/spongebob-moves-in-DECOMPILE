using System;

// Token: 0x02000230 RID: 560
public interface ISessionActionSpawn
{
	// Token: 0x0600123B RID: 4667
	SessionActionManager.SpawnReturnCode OnUpdate(Game game);

	// Token: 0x0600123C RID: 4668
	void Destroy();
}
